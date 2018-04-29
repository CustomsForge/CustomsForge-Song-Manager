using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Antlr4.StringTemplate;
using System.Data;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using GenTools;
using Newtonsoft.Json;

// TODO: localize all messages that are single usage
namespace CustomsForgeSongManager.UControls
{
    public partial class Renamer : UserControl
    {
        private List<SongData> renSongList = new List<SongData>();

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

        public void ShowRenamePreview()
        {
            SongData sd = null;
            List<SongData> List = renSongList.Count > 0 ? renSongList : Globals.MasterCollection.ToList();
            if (List.Count == 0)
                Globals.Log("No songs found for rename preview.");
            int x = Globals.random.Next(List.Count() - 1);
            sd = List[x];
            if (sd != null)
                Globals.Log(String.Format("Renamer preview : {0}", GetNewSongName(sd)));
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

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
            template.Add("tuning", data.Tunings1D.Split(new[] { ", " }, StringSplitOptions.None).FirstOrDefault());
            template.Add("dd", data.DD > 0 ? "DD" : "");
            template.Add("ddlvl", data.DD);
            template.Add("year", data.SongYear);
            var pkgVersion = data.PackageVersion;
            if (pkgVersion == "N/A") pkgVersion = "0";
            if (String.IsNullOrEmpty(pkgVersion)) pkgVersion = "1";
            template.Add("version", String.Format("v{0}", pkgVersion));
            template.Add("author", String.IsNullOrEmpty(data.PackageAuthor) ? "Unknown" : data.PackageAuthor.Replace('\\', '_'));
            template.Add("arrangements", data.ArrangementsInitials);
            template.Add("_", "_");

            // CAREFUL - lots to go wrong in this simple method :(
            // there could be setlist directories or user added directory(s)
            // beethoven_p.psarc is a good song to use for testing
            var oldFilePath = data.FilePath;
            var newFileName = template.Render();
            var dlcDir = Constants.Rs2DlcFolder;

            // renamed files could be enabled or disabled
            if (Path.GetFileName(oldFilePath).ToLower().Contains("disabled"))
                newFileName = String.Format("{0}" + Constants.DisabledPsarcExtension, newFileName);
            else
                newFileName = String.Format("{0}" + Constants.PsarcExtension, newFileName);

            // strip any user added a directory(s) from file name and add to file path
            var dirSeperator = new string[] { "\\" };
            var parts = newFileName.Split(dirSeperator, StringSplitOptions.None);
            if (parts.Any())
            {
                for (int i = 0; i < parts.Count() - 1; i++)
                    dlcDir = Path.Combine(dlcDir, parts[i]);

                newFileName = parts[parts.Count() - 1];
            }

            // file name, path and file path length validations
            dlcDir = String.Join("", dlcDir.Split(Path.GetInvalidPathChars()));
            newFileName = String.Join("", newFileName.Split(Path.GetInvalidFileNameChars()));
            newFileName = newFileName.Replace("__", "_");

            if (chkRemoveSpaces.Checked)
                newFileName = newFileName.Replace(" ", "");

            // test file path length is valid
            var newFilePath = Path.Combine(dlcDir, newFileName);

            if (!newFilePath.IsFilePathValid())
            {
                var dialogMsg = "New file path: " + newFilePath + Environment.NewLine + Environment.NewLine +
                    "is not valid.  Check file path for excessive length and/or invalid characters try again.";
                var iconMsg = "Warning: File Path Length May Exceed System Capabilities";
                BetterDialog2.ShowDialog(dialogMsg, "Renamer", "OK", null, null, Bitmap.FromHicon(SystemIcons.Warning.Handle), iconMsg, 150, 150);
            }

            return newFilePath;
        }

        public void RenameSongs()
        {
            foreach (SongData data in renSongList)
            {
                var oldFilePath = data.FilePath;
                var newFilePath = GetNewSongName(data);
                var newFileDirectory = Path.GetDirectoryName(newFilePath);

                if (!Directory.Exists(newFileDirectory))
                    Directory.CreateDirectory(newFileDirectory);
                try
                {
                    File.Move(oldFilePath, newFilePath);
                    Globals.Log(String.Format("Renaming From: {0}", oldFilePath));
                    Globals.Log("To: " + newFilePath);
                    data.FilePath = newFilePath;

                    if (chkDeleteEmptyDir.Checked)
                        new DirectoryInfo(Constants.Rs2DlcFolder).DeleteEmptyDirs();
                }
                catch (Exception e)
                {
                    if (e.Message.ToLower().Contains("cannot create a file"))
                        Globals.Log("Use the Duplicates tabmenu to delete/move" + Environment.NewLine +
                            Path.GetFileName(newFilePath) + "duplicates before attempting to use Renamer.");

                    Globals.Log(String.Format("<ERROR>: {0}", e.Message));
                }
            }
        }

        public void UpdateToolStrip()
        {
            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Please rescan the song collection" + Environment.NewLine + "using Song Manager tab first.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, Globals.MasterCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            var selectedDLC = Globals.MasterCollection.Where(song => song.Selected).ToList().Count();
            var tsldcMsg = String.Format("Selected Songs in SongManager Count: {0}", selectedDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;

            // move caret to textbox
            txtRenameTemplate.Focus();
            txtRenameTemplate.Select(txtRenameTemplate.Text.Length, 0);
        }

        private void BackgroundRenamer()
        {
            ToggleUiControls(false);

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUiControls(true);

            // force reload
            Globals.ReloadSetlistManager = true;
            Globals.ReloadDuplicates = true;
            //Globals.ReloadRenamer = true;
            Globals.ReloadSongManager = true;
        }

        private void ToggleUiControls(bool enabled)
        {
            btnRenameAll.Enabled = enabled;
            txtRenameTemplate.Enabled = enabled;
            chkDeleteEmptyDir.Enabled = enabled;
        }

        private bool ValidateInput()
        {
            if (!txtRenameTemplate.Text.Contains("<title>"))
            {
                MessageBox.Show("The template must contain at least" + Environment.NewLine + "<title> to prevent renamining errors.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

           // renSongList = new List<SongData>(Globals.MasterCollection);
            renSongList = Globals.MasterCollection.ToList();
            // do not rename RS1 compatiblity files
            renSongList.RemoveAll(x => x.FileName.Contains(Constants.RS1COMP));

            // do not rename any disabled songs
            renSongList.RemoveAll(x => x.Enabled.Contains("No"));

            // rename only user selected songs
            if (chkRenameOnlySelected.Checked)
                renSongList.RemoveAll(x => x.Selected == false);

            if (renSongList == null || renSongList.Count == 0 || Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Please rescan the song collection" + Environment.NewLine + "using Song Manager tab first.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // check for duplicates
            var dups = renSongList.GroupBy(x => new { Song = x.Title, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dups.Any())
            {
                MessageBox.Show("Please remove duplicate songs" + Environment.NewLine + "using the Duplicates tab first.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (MessageBox.Show("Rename all files?", "Comfirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (!ValidateInput())
                return;

            BackgroundRenamer();
        }

        private void dgvRenamerProperties_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // file name template builder
            var grid = (DataGridView)sender;

            if (e.RowIndex != -1)
            {
                var selStart = txtRenameTemplate.SelectionStart;
                var newProp = String.Format("<{0}>", grid.Rows[e.RowIndex].Cells["Key"].Value);
                newProp = newProp.Replace("<_>", "_");
                
                if (selStart != txtRenameTemplate.Text.Length)
                    txtRenameTemplate.Text = txtRenameTemplate.Text.Insert(selStart, newProp);
                else
                    txtRenameTemplate.Text += newProp;

                // move to the end of new property addition
                txtRenameTemplate.Select(selStart + newProp.Length, 0);
                txtRenameTemplate.Focus();
            }
        }

        private void txtRenameTemplate_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.RenameTemplate = txtRenameTemplate.Text;
        }
    }
}