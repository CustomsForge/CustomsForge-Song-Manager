
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Antlr4.StringTemplate;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace CustomsForgeManager.UControls
{
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

        public void RenameSongs()
        {
            foreach (SongData data in renSongCollection)
            {
                // 'The' mover
                if (chkTheMover.Checked)
                    if (data.Artist.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                        data.Artist = String.Format("{0}, The", data.Artist.Substring(4, data.Artist.Length - 4)).Trim();

                Template template = new Template(txtRenameTemplate.Text);
                template.Add("artist", data.Artist);
                template.Add("title", data.Title);
                template.Add("album", data.Album);
                template.Add("filename", data.FileName);
                template.Add("tuning", data.Tuning.Split(new[] { ", " }, StringSplitOptions.None).FirstOrDefault());

                if (Convert.ToInt32(data.DD.Split(new[] { ", " }, StringSplitOptions.None).FirstOrDefault()) > 0)
                    template.Add("dd", "_DD");
                else
                    template.Add("dd", "");

                if (!String.IsNullOrEmpty(data.SongYear))
                    template.Add("year", data.SongYear);

                if (!String.IsNullOrEmpty(data.Version) && !data.Version.ToLower().Contains("n/a"))
                    template.Add("version", data.Version);

                if (!String.IsNullOrEmpty(data.Charter))
                    template.Add("author", data.Charter);

                // CAREFUL - lots to go wrong in this simple method :(
                // there could be setlist directories or user added directory(s)
                // beethoven_p.psarc is a good song to use for testing
                var oldFilePath = data.Path;
                var newFileName = template.Render();
                var newFilePath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");

                // renamed files could be enabled or disabled
                if (Path.GetFileName(oldFilePath).ToLower().Contains("disabled"))
                    newFileName = String.Format("{0}_p.disabled.psarc", newFileName);
                else
                    newFileName = String.Format("{0}_p.psarc", newFileName);

                // strip any user added a directory(s) from file name and add to file path
                var dirSeperator = new string[] { "\\" };
                var parts = newFileName.Split(dirSeperator, StringSplitOptions.None);
                if (parts.Any())
                {
                    for (int i = 0; i < parts.Count() - 1; i++)
                        newFilePath = Path.Combine(newFilePath, parts[i]);

                    newFileName = parts[parts.Count() - 1];
                }

                // generated file name and/or file path could be invalid
                newFilePath = String.Join("", newFilePath.Split(Path.GetInvalidPathChars()));
                newFileName = String.Join("", newFileName.Split(Path.GetInvalidFileNameChars()));
                newFileName = newFileName.Replace("__", "_");

                if (chkRemoveSpaces.Checked)
                    newFileName = newFileName.Replace(" ", "");

                if (!Directory.Exists(newFilePath))
                    Directory.CreateDirectory(newFilePath);

                try
                {
                    newFilePath = Path.Combine(newFilePath, newFileName);
                    File.Move(oldFilePath, newFilePath);
                    Globals.Log("Renaming From: " + oldFilePath);
                    Globals.Log("To: " + newFilePath);

                    if (chkDeleteEmptyDir.Checked)
                        new DirectoryInfo(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc")).DeleteEmptyDirs();
                }
                catch (Exception e)
                {
                    if (e.Message.ToLower().Contains("cannot create a file"))
                        Globals.Log("Use Duplicates to delete/move '" + Path.GetFileName(newFilePath) + "' before using Renamer.");

                    Globals.Log("Error: " + e.Message);
                }
            }
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSongManager || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Songs need to be rescanned with  " + Environment.NewLine + "Song Manager before renaming!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            var selectedDLC = Globals.SongCollection.Where(song => song.Selected).ToList().Count();
            var tsldcMsg = String.Format("Selected Songs in SongManager Count: {0}", selectedDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;
        }

        private void BackgroundRenamer()
        {
            ToggleUIControls();

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls();
        }

        private void ToggleUIControls()
        {
            Extensions.InvokeIfRequired(btnRenameAll, delegate { btnRenameAll.Enabled = !btnRenameAll.Enabled; });
            Extensions.InvokeIfRequired(txtRenameTemplate, delegate { txtRenameTemplate.Enabled = !txtRenameTemplate.Enabled; });
            Extensions.InvokeIfRequired(chkDeleteEmptyDir, delegate { chkDeleteEmptyDir.Enabled = !chkDeleteEmptyDir.Enabled; });
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
            var dups = renSongCollection.GroupBy(x => new { Song = x.Title, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dups.Any())
            {
                MessageBox.Show("Use the Duplicates tabmenu to delete/move" + Environment.NewLine + "duplicates before attempting to use Renamer.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnClearTemplate_Click(object sender, EventArgs e)
        {
            txtRenameTemplate.Text = String.Empty;
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
            Globals.RescanSetlistManager = true;
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
    }
}
