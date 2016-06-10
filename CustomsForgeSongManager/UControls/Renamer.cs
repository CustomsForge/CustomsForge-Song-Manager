using System.Linq;
using System.Windows.Forms;
using Antlr4.StringTemplate;
using System.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using Newtonsoft.Json;

namespace CustomsForgeSongManager.UControls
{
    public partial class Renamer : UserControl
    {
        private List<SongData> renSongCollection = new List<SongData>();

        public Renamer()
        {
            InitializeComponent();
            PopulateRenamer();
            if (!String.IsNullOrEmpty(AppSettings.Instance.RenameTemplate))
                txtRenameTemplate.Text = AppSettings.Instance.RenameTemplate;
        }

        public void PopulateRenamer()
        {
            Globals.Log("Populating Renamer GUI ...");
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("CustomsForgeSongManager.Resources.renamer_properties.json");
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    renamerPropertyDataSet = new DataSet();
                    renamerPropertyDataSet = (DataSet) JsonConvert.DeserializeObject(json, (typeof (DataSet)));
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

        public void ShowRenamePreview()
        {
            SongData sd = null;
            List<SongData> List = renSongCollection.Count > 0 ? renSongCollection : Globals.SongCollection.ToList();
            if (List.Count == 0)
                Globals.Log(Properties.Resources.NoSongsFoundForRenamePreview);
            int x = Globals.random.Next(List.Count() - 1);
            sd = List[x];
            if (sd != null)
                Globals.Log(string.Format(Properties.Resources.RenamerPreviewX0, GetNewSongName(sd)));
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.ShowLogWindow = true;
            ShowRenamePreview();
        }

        public string GetNewSongName(SongData data)
        {
            if (chkTheMover.Checked)
                if (data.Artist.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                    data.Artist = String.Format("{0}, The", data.Artist.Substring(4, data.Artist.Length - 4)).Trim();

            Template template = new Template(txtRenameTemplate.Text);
            template.Add("artist", data.Artist.Replace('\\', '_'));
            template.Add("title", data.Title.Replace('\\', '_'));
            template.Add("album", data.Album.Replace('\\', '_'));
            template.Add("filename", data.FileName);
            template.Add("tuning", data.Tuning.Split(new[] {", "}, StringSplitOptions.None).FirstOrDefault());
            template.Add("dd", data.DD > 0 ? "_DD" : "");
            template.Add("ddlvl", data.DD);
            template.Add("year", data.SongYear);
            template.Add("version", data.Version == null ? "" : data.Version);
            template.Add("author", String.IsNullOrEmpty(data.CharterName) ? "Unknown" : data.CharterName.Replace('\\', '_'));
            template.Add("arrangements", data.ArrangementInitials);

            // CAREFUL - lots to go wrong in this simple method :(
            // there could be setlist directories or user added directory(s)
            // beethoven_p.psarc is a good song to use for testing
            var oldFilePath = data.FilePath;
            var newFileName = template.Render();

            var newFilePath = Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc");

            // renamed files could be enabled or disabled
            if (Path.GetFileName(oldFilePath).ToLower().Contains("disabled"))
                newFileName = String.Format("{0}_p.disabled.psarc", newFileName);
            else
                newFileName = String.Format("{0}_p.psarc", newFileName);

            // strip any user added a directory(s) from file name and add to file path
            var dirSeperator = new string[] {"\\"};
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

            return Path.Combine(newFilePath, newFileName);
        }

        public void RenameSongs()
        {
            foreach (SongData data in renSongCollection)
            {
                var oldFilePath = data.FilePath;
                var newFilePath = GetNewSongName(data);
                var newFileDirectory = Path.GetDirectoryName(newFilePath);

                if (!Directory.Exists(newFileDirectory))
                    Directory.CreateDirectory(newFileDirectory);
                try
                {
                    File.Move(oldFilePath, newFilePath);
                    Globals.Log(string.Format(Properties.Resources.Renamer_RenamingFrom, oldFilePath));
                    Globals.Log("To: " + newFilePath);
                    data.FilePath = newFilePath;
                    if (chkDeleteEmptyDir.Checked)
                        new DirectoryInfo(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc")).DeleteEmptyDirs();
                }
                catch (Exception e)
                {
                    //todo: Localize this
                    if (e.Message.ToLower().Contains("cannot create a file"))
                        Globals.Log(string.Format(Properties.Resources.Renamer_UseDuplicatesToDeleteMove, Path.GetFileName(newFilePath)));

                    Globals.Log(string.Format("{0}: {1}", Properties.Resources.ERROR, e.Message));
                }
            }
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSongManager || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show(string.Format(Properties.Resources.Renamer_SongsNeedToBeRescanned, Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            var selectedDLC = Globals.SongCollection.Where(song => song.Selected).ToList().Count();
            var tsldcMsg = String.Format(Properties.Resources.SelectedSongsInSongManagerCountX0, selectedDLC);
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
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(Globals.SongManager);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls();
        }

        private void ToggleUIControls()
        {
            GenExtensions.InvokeIfRequired(btnRenameAll, delegate { btnRenameAll.Enabled = !btnRenameAll.Enabled; });
            GenExtensions.InvokeIfRequired(txtRenameTemplate, delegate { txtRenameTemplate.Enabled = !txtRenameTemplate.Enabled; });
            GenExtensions.InvokeIfRequired(chkDeleteEmptyDir, delegate { chkDeleteEmptyDir.Enabled = !chkDeleteEmptyDir.Enabled; });
        }

        private bool ValidateInput()
        {
            if (!txtRenameTemplate.Text.Contains("<title>"))
            {
                MessageBox.Show(Properties.Resources.RenameTemplateRequiresTitleToPrevent);
                return false;
            }

            if (renSongCollection == null || renSongCollection.Count == 0)
            {
                MessageBox.Show(Properties.Resources.PleaseScanInAtLeastOneSong);
                return false;
            }

            if (Globals.RescanSongManager || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show(string.Format(Properties.Resources.SongsNeedToBeRescannedWithX0SongManagerBef, Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // check for duplicates
            var dups = renSongCollection.GroupBy(x => new {Song = x.Title, x.Album, x.Artist}).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dups.Any())
            {
                MessageBox.Show(string.Format(Properties.Resources.UseTheDuplicatesTabmenuToDeleteMoveX0Dupli, Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (MessageBox.Show("Rename all files?", "Comfirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                Globals.RescanSongManager = false;
                Globals.RescanDuplicates = false;
                Globals.RescanSetlistManager = false;
            }
        }

        private void dgvRenamerProperties_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // file name template builder
            var grid = (DataGridView) sender;

            if (e.RowIndex != -1)
            {
                string s = txtRenameTemplate.Text;
                if (txtRenameTemplate.SelectionLength > 0)
                    s = s.Remove(txtRenameTemplate.SelectionStart, txtRenameTemplate.SelectionLength);
                //insert at selection start
                txtRenameTemplate.Text = s.Insert(txtRenameTemplate.SelectionStart, String.Format("<{0}>", grid.Rows[e.RowIndex].Cells["Key"].Value));

                //if (String.IsNullOrEmpty(txtRenameTemplate.Text))
                //    txtRenameTemplate.Text = String.Format("<{0}>", grid.Rows[e.RowIndex].Cells["Key"].Value);
                //else
                //    if (txtRenameTemplate.Text.Substring(txtRenameTemplate.Text.Length - 2) == @"\\")
                //        txtRenameTemplate.Text = String.Format("{0}<{1}>", txtRenameTemplate.Text, grid.Rows[e.RowIndex].Cells["Key"].Value);
                //    else
                //        txtRenameTemplate.Text = String.Format("{0}_<{1}>", txtRenameTemplate.Text, grid.Rows[e.RowIndex].Cells["Key"].Value);
            }
        }

        private void txtRenameTemplate_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.RenameTemplate = txtRenameTemplate.Text;
        }
    }
}