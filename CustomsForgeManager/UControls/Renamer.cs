﻿
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Antlr4.StringTemplate;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.Data;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CustomsForgeManager.UControls
{
    // TODO: make this more visual and intuitive
    // bring the outer working to the GUI so user sees some action
    // hard code the datagridview so that fields can easily be selected
    public partial class Renamer : UserControl
    {
        private List<SongData> renSongCollection = new List<SongData>();

        public Renamer()
        {
            InitializeComponent();
            PopulateRenamer();
        }

        public void PopulateRenamer()
        {
            Globals.Log("Populating Renamer GUI ...");

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.renamer_properties.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    renamerPropertyDataSet = new DataSet();
                    renamerPropertyDataSet = (DataSet)JsonConvert.DeserializeObject(json, (typeof(DataSet)));
                    dgvRenamer.AutoGenerateColumns = true;
                    dgvRenamer.DataSource = renamerPropertyDataSet.Tables[0];
                }

                Globals.Log("Loaded renamer_properties.json template ...");
            }
            catch (Exception e)
            {
                Globals.Log(e.Message);
            }
        }

        private void btnRenameAll_Click(object sender, System.EventArgs e)
        {

            renSongCollection = new List<SongData>(Globals.SongCollection);
            // do not rename RS1 compatiblity files
            renSongCollection.RemoveAll(x => x.FileName.Contains(Constants.RS1COMP));
            // do not rename any disabled songs
            renSongCollection.RemoveAll(x => x.Enabled.Contains("No"));
            // rename only user selected songs
            if (chkRenameOnlySelected.Checked)
                renSongCollection.RemoveAll(x => x.Selected == false);

            if (!ValidateInput())
                return;

            // TODO: why use a background worker here?
            BackgroundRenamer();
            // could just do this instead
            // RenameSongs();
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
        }

        private bool ValidateInput()
        {

            if (!txtRenameTemplate.Text.Contains("<title>"))
            {
                MessageBox.Show("Rename Template requires <title> to prevent overwriting songs.");
                return false;
            }

            if (renSongCollection == null || renSongCollection.Count == 0)
            {
                MessageBox.Show("Please scan in at least one song.");
                return false;
            }

            if (Globals.RescanSongManager || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Songs need to be rescanned with  " + Environment.NewLine + "Song Manager before renaming!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // check for duplicates
            var dups = renSongCollection.GroupBy(x => new { x.Song, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dups.Any())
            {
                MessageBox.Show("Use the Duplicates tabmenu to delete/move" + Environment.NewLine + "duplicates before attempting to use Renamer.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        public void RenameSongs()
        {
            try
            {
                foreach (SongData data in renSongCollection)
                {
                    // 'The' mover
                    if (chkTheMover.Checked)
                        if (data.Artist.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                            data.Artist = String.Format("{0}, The", data.Artist.Substring(4, data.Artist.Length - 4)).Trim();

                    Template template = new Template(txtRenameTemplate.Text);
                    template.Add("artist", data.Artist);
                    template.Add("title", data.Song);
                    template.Add("album", data.Album);
                    template.Add("version", data.Version);
                    template.Add("author", data.Author);
                    template.Add("filename", data.FileName);
                    template.Add("year", data.SongYear);

                    if ("Yes".Equals(data.DD))
                        template.Add("dd", "_dd");
                    else
                        template.Add("dd", "");

                    string newFilePath = Globals.MySettings.RSInstalledDir + "\\dlc\\" + template.Render() + "_p.psarc";
                    string oldFilePath = data.Path;
                    FileInfo newFileInfo = new FileInfo(newFilePath);
                    Directory.CreateDirectory(newFileInfo.Directory.FullName);
                    Globals.Log("Renaming/Moving:" + oldFilePath);

                    Globals.Log("---> " + newFilePath);
                    File.Move(oldFilePath, newFilePath);
                }

                if (chkDeleteEmptyDir.Checked)
                    new DirectoryInfo(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc")).DeleteEmptyDirs();
            }
            //lazy exception catch for now. Future me will punch me for such astrocities, alas its a desktop app.
            catch (Exception e)
            {
                Globals.Log(e.Message);
                // this should not occure if initial error checking works as expected
                if (e.Message.ToLower().Contains("cannot create a file"))
                    Globals.Log("Use Duplicates to delete/move duplicates before using Renamer.");
            }
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSongManager || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Songs need to be rescanned with  " + Environment.NewLine + "Song Manager before renaming!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Globals.TsLabel_StatusMsg.Text = String.Format("Renamer Song Count: {0}", renSongCollection.Count);
            Globals.TsLabel_StatusMsg.Visible = true;
        }

        private void BackgroundRenamer()
        {
            // run new worker
            using (Worker worker = new Worker())
            {
                ToggleUIControls();
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                {
                    Application.DoEvents();
                }

                ToggleUIControls();
            }
        }

        private void ToggleUIControls()
        {
            Extensions.InvokeIfRequired(btnRenameAll, delegate { btnRenameAll.Enabled = !btnRenameAll.Enabled; });
            Extensions.InvokeIfRequired(txtRenameTemplate, delegate { txtRenameTemplate.Enabled = !txtRenameTemplate.Enabled; });
            Extensions.InvokeIfRequired(chkDeleteEmptyDir, delegate { chkDeleteEmptyDir.Enabled = !chkDeleteEmptyDir.Enabled; });
        }

        private void dgvRenamerProperties_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // file name template builder
            var grid = (DataGridView)sender;

            if (e.RowIndex != -1)
                if (String.IsNullOrEmpty(txtRenameTemplate.Text))
                    txtRenameTemplate.Text = String.Format("<{0}>", grid.Rows[e.RowIndex].Cells["Key"].Value);
                else
                    if (txtRenameTemplate.Text.Substring(txtRenameTemplate.Text.Length - 2) == @"\\")
                        txtRenameTemplate.Text = String.Format("{0}<{1}>", txtRenameTemplate.Text, grid.Rows[e.RowIndex].Cells["Key"].Value);
                    else
                        txtRenameTemplate.Text = String.Format("{0}_<{1}>", txtRenameTemplate.Text, grid.Rows[e.RowIndex].Cells["Key"].Value);
        }

        private void btnClearTemplate_Click(object sender, EventArgs e)
        {
            txtRenameTemplate.Text = String.Empty;
        }

    }
}
