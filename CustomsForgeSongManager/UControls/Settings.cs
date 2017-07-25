using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            Leave += Settings_Leave;
            AppSettings.Instance.PropertyChanged += SettingsPropChanged;
        }

        public void LoadSettingsFromFile(DataGridView dgvCurrent)
        {
            try
            {
                Globals.DgvCurrent = dgvCurrent;
                AppSettings.Instance.LoadFromFile(Constants.AppSettingsPath, dgvCurrent);

                cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                chkEnableAutoUpdate.Checked = AppSettings.Instance.EnableAutoUpdate;
                chkIncludeRS1CompSongs.Checked = AppSettings.Instance.IncludeRS1CompSongs;
                chkIncludeRS2BaseSongs.Checked = AppSettings.Instance.IncludeRS2BaseSongs;
                chkEnableLogBallon.Checked = AppSettings.Instance.EnableLogBaloon;
                rbCleanOnClosing.Checked = AppSettings.Instance.CleanOnClosing;
                txtCharterName.Text = AppSettings.Instance.CharterName;

                ValidateRsDir();
            }
            catch (Exception ex)
            {
                Globals.MyLog.Write(String.Format("<Error>: {0}", ex.Message));
            }
        }

        public void PopulateSettings(DataGridView dgvCurrent)
        {
            // done everytime loaded in case there are any changes to SongManager
            Globals.Log("Populating Settings GUI for " + dgvCurrent.Name + " ...");
            Globals.DgvCurrent = dgvCurrent;

            // each DGV contains a tag which holds a friendly name, aka current TabPage.Text
            var dgvTag = dgvCurrent.Tag.ToString();

            // show which DataGridView is loaded
            lblDgvColumns.Text = String.Format("Settings for {0} from file: {1}", dgvTag, Path.GetFileName(Constants.GridSettingsPath));

            // initialize column list
            lstDgvColumns.Items.Clear();
            foreach (DataGridViewColumn col in Globals.DgvCurrent.Columns)
            {
                ListViewItem newItem = new ListViewItem(new[] { String.Empty, col.Name, col.HeaderText, col.Width.ToString() }) { Checked = col.Visible };
                lstDgvColumns.Items.Add(newItem);
            }
        }

        public void SaveSettingsToFile(DataGridView dgvCurrent)
        {
            Globals.DgvCurrent = dgvCurrent;
            Debug.WriteLine("Save DataGridView Settings: " + dgvCurrent.Name);

            try
            {
                using (var fs = new FileStream(Constants.AppSettingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    AppSettings.Instance.SerializeXml(fs);
                    Globals.Log("Saved File: " + Path.GetFileName(Constants.AppSettingsPath));                }

                if (String.IsNullOrEmpty(dgvCurrent.Name))
                    return;

                if (!Directory.Exists(Constants.GridSettingsFolder))
                    Directory.CreateDirectory(Constants.GridSettingsFolder);

                SerialExtensions.SaveToFile(Constants.GridSettingsPath, RAExtensions.SaveColumnOrder(dgvCurrent));
                Globals.Log("Saved File: " + Path.GetFileName(Constants.GridSettingsPath));
            }
            catch (Exception ex)
            {
                Globals.Log(String.Format("<Error>: {0}", ex.Message));
            }
        }

        private void ToogleRescan(bool rescan)
        {
            // rescan on tabpage change ... safest but may be better way
            // possibly never overwrite masterSongControl with Duplicates
            Globals.RescanSongManager = rescan;
            Globals.RescanDuplicates = rescan;
            Globals.RescanSetlistManager = rescan;
            Globals.RescanRenamer = rescan;
            Globals.ReloadSongPacks = rescan;
        }

        private void ValidateRsDir()
        {
            // validate Rocksmith installation directory
            var rsDir = AppSettings.Instance.RSInstalledDir;
            if (String.IsNullOrEmpty(rsDir) || !Directory.Exists(rsDir)) // || rsDir.Text.Contains("Click here"))
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select Rocksmith 2014 Installation Directory";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return;

                    cueRsDir.Text = fbd.SelectedPath;
                }

                if (!Directory.Exists(Path.Combine(cueRsDir.Text, "dlc")))
                {
                    MessageBox.Show(new Form { TopMost = true }, String.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ValidateRsDir(); // put the user into a loop (force selection)
                }

                AppSettings.Instance.RSInstalledDir = cueRsDir.Text;
                Globals.Log("Rocksmith Installation Directory: " + AppSettings.Instance.RSInstalledDir);
            }
        }

        private void chkEnableLogBaloon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableLogBallon.Checked)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
        }

        private void SettingsPropChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RSInstalledDir":
                    cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                    break;
                case "EnableAutoUpdate":
                    chkEnableAutoUpdate.Checked = AppSettings.Instance.EnableAutoUpdate;
                    break;
                case "IncludeRS2BaseSongs":
                    chkIncludeRS2BaseSongs.Checked = AppSettings.Instance.IncludeRS2BaseSongs;
                    break;
                case "IncludeRS1CompSongs":
                    chkIncludeRS1CompSongs.Checked = AppSettings.Instance.IncludeRS1CompSongs;
                    break;
                case "IncludeCustomPacks":
                    chkIncludeCustomPacks.Checked = AppSettings.Instance.IncludeCustomPacks;
                    break;
                case "IncludeAnalyzerData":
                    chkIncludeAnalyzerData.Checked = AppSettings.Instance.IncludeAnalyzerData;
                    break;
                case "EnableLogBaloon":
                    chkEnableLogBallon.Checked = AppSettings.Instance.EnableLogBaloon;
                    break;
                case "CleanOnClosing":
                    rbCleanOnClosing.Checked = AppSettings.Instance.CleanOnClosing;
                    break;
                case "CreatorName":
                    txtCharterName.Text = AppSettings.Instance.CharterName;
                    break;
            }
        }

        private void Settings_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
                ValidateRsDir();

            SaveSettingsToFile(Globals.DgvCurrent);
        }

        private void btnEmptyLogs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to empty the log files?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            GenExtensions.DeleteFile(AppSettings.Instance.LogFilePath);
            Globals.MyLog.AddTargetFile(AppSettings.Instance.LogFilePath);
            GenExtensions.DeleteFile(Constants.RepairsErrorLogPath);
            Globals.TbLog.Clear();
            Globals.Log("Log files have been emptied ...");
            Globals.Log("Starting new log ...");
            Globals.Log(Constants.AppTitle);
        }

        private void btnResetDownloads_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.DownloadsDir = String.Empty;
            Globals.Log("CDLC 'Downloads' folder path was reset ...");
        }

        private void btnSettingsLoad_Click(object sender, EventArgs e)
        {
            LoadSettingsFromFile(Globals.DgvCurrent);
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            ValidateRsDir();
            SaveSettingsToFile(Globals.DgvCurrent);
        }

        private void chkEnableAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.EnableAutoUpdate = chkEnableAutoUpdate.Checked;
        }

        private void chkIncludeCustomPacks_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeCustomPacks = chkIncludeCustomPacks.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeAnalyzerData_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeAnalyzerData = chkIncludeAnalyzerData.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeRS1CompSongs_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeRS1CompSongs = chkIncludeRS1CompSongs.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeRS2BaseSongs_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeRS2BaseSongs = chkIncludeRS2BaseSongs.Checked;
            ToogleRescan(true);
        }

        private void cueRsDir_MouseClick(object sender, MouseEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Rocksmith 2014 Installation Directory";
                fbd.SelectedPath = GetInstallDirFromRegistry();

                if (fbd.ShowDialog() != DialogResult.OK) return;
                cueRsDir.Text = fbd.SelectedPath;
            }

            if (!Directory.Exists(Path.Combine(cueRsDir.Text, "dlc")))
            {
                MessageBox.Show(string.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToogleRescan(true);

            // update RSInstalledDir after above error check passes
            AppSettings.Instance.RSInstalledDir = cueRsDir.Text;
        }

        private void listDisabledColumns_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            DataGridViewColumn column = Globals.DgvCurrent.Columns[e.Item.SubItems[1].Text];
            if (column != null)
            {
                column.Visible = e.Item.Checked;
                column.Width = Convert.ToInt16(e.Item.SubItems[3].Text);
            }
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool deselect = lstDgvColumns.Items[1].Checked;

            for (int i = 1; i < lstDgvColumns.Items.Count; i++)
            {
                lstDgvColumns.Items[i].Checked = !deselect;
            }
        }

        private void rbCleanOnClosing_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CleanOnClosing = rbCleanOnClosing.Checked;
        }

        private void tbCreator_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CharterName = txtCharterName.Text;
        }


        private static string GetInstallDirFromRegistry()
        {
            return LocalExtensions.GetSteamDirectory();
        }

    }
}