using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using GenTools;
using DataGridViewTools;
using CustomControls;
using System.Drawing;

namespace CustomsForgeSongManager.UControls
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            // save AppSettings.Instance
            Leave += Settings_Leave;
        }

        public void LoadSettingsFromFile(DataGridView dgvCurrent = null, bool verbose = false)
        {
            try
            {
                Globals.DgvCurrent = dgvCurrent;
                AppSettings.Instance.LoadFromFile(Constants.AppSettingsPath, verbose);
                var debugMe = AppSettings.Instance.ArrangementAnalyzerFilter;

                cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                chkIncludeRS1CompSongs.Checked = AppSettings.Instance.IncludeRS1CompSongs;
                chkIncludeRS2BaseSongs.Checked = AppSettings.Instance.IncludeRS2BaseSongs;
                chkIncludeCustomPacks.Checked = AppSettings.Instance.IncludeCustomPacks;
                chkIncludeArrangementData.Checked = AppSettings.Instance.IncludeArrangementData;
                chkEnableAutoUpdate.Checked = AppSettings.Instance.EnableAutoUpdate;
                chkEnableNotifications.Checked = AppSettings.Instance.EnableNotifications;
                chkEnableQuarantine.Checked = AppSettings.Instance.EnableQuarantine;
                chkValidateD3D.Checked = AppSettings.Instance.ValidateD3D;
                chkMacMode.Checked = AppSettings.Instance.MacMode;
                rbCleanOnClosing.Checked = AppSettings.Instance.CleanOnClosing;
                txtCharterName.Text = AppSettings.Instance.CharterName;

                // check validation only on startup
                if (dgvCurrent == null)
                {
                    ValidateRsDir();
                    ValidateD3D();

                    if (!AppSettings.Instance.EnableQuarantine)
                        Globals.Log("<WARNING> 'Auto Quarantine' is disabled ...");
                }
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

        public void SaveSettingsToFile(DataGridView dgvCurrent, bool verbose = false)
        {
            Globals.DgvCurrent = dgvCurrent;
            Debug.WriteLine("Save DataGridView Settings: " + dgvCurrent.Name);

            try
            {
                using (var fs = new FileStream(Constants.AppSettingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    AppSettings.Instance.SerializeXml(fs);
                    if (verbose)
                        Globals.Log("Saved File: " + Path.GetFileName(Constants.AppSettingsPath));
                }

                if (String.IsNullOrEmpty(dgvCurrent.Name) || dgvCurrent.RowCount == 0)
                    return;

                if (!Directory.Exists(Constants.GridSettingsFolder))
                    Directory.CreateDirectory(Constants.GridSettingsFolder);

                // TODO: allow customized grid settings to be saved and loaded by name
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
            Globals.RescanArrangements = rescan;
            Globals.RescanDuplicates = rescan;
            Globals.RescanSetlistManager = rescan;
            Globals.RescanRenamer = rescan;
            Globals.ReloadSongPacks = rescan;
        }

        private bool ValidateD3D()
        {
            if (!AppSettings.Instance.ValidateD3D)
            {
                Globals.Log("<WARNING> 'Validate D3DX9_42.dll' is disabled ...");
                return false;
            }

            if (Constants.OnMac)
            {
                Globals.Log("Send the 'debug.log' file to CFSM Developer, Cozy1 for analysis ...");
                Globals.Log("<WARNING> 'D3DX9_42.dll' file validation is disabled while in Mac Mode ...");
                Globals.Log("AppSettings.Instance.RSInstalledDir = " + AppSettings.Instance.RSInstalledDir);
                Globals.Log("Application.ExecutablePath = " + Application.ExecutablePath);
                Globals.Log("Path.GetDirectoryName(Application.ExecutablePath) = " + Path.GetDirectoryName(Application.ExecutablePath));
                Globals.Log("Constants.ApplicationFolder = " + Constants.ApplicationFolder);
                Globals.Log("Found 'Application Support' folder: " + Constants.Rs2DlcFolder.Contains("Application Support"));

                return false;
            }

            // validates either old and new (Remastered) version of Rocksmith 2014 D3DX9_42.dll
            var luaPath = Path.Combine(AppSettings.Instance.RSInstalledDir, "lua5.1.dll");
            var d3dPath = Path.Combine(AppSettings.Instance.RSInstalledDir, "D3DX9_42.dll");

            if (!File.Exists(d3dPath))
            {
                var diaMsg = "The 'D3DX9_42.dll' file could not be found. Would you like CFSM to install the dll file that is required to play CDLC files?";
                if (DialogResult.No == BetterDialog2.ShowDialog(GeneralExtensions.SplitString(diaMsg, 30), "Validating D3DX9_42.dll ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                {
                    Globals.Log("<WARNING> User aborted installing 'D3DX9_42.dll' file ...");
                    return false;
                }

                // working directly with the file rather than an embedded resource 
                if (File.Exists(luaPath))
                    GeneralExtensions.CopyFile(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.old"), Path.Combine(AppSettings.Instance.RSInstalledDir, "D3DX9_42.dll"), true, false);
                else
                    GeneralExtensions.CopyFile(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.new"), Path.Combine(AppSettings.Instance.RSInstalledDir, "D3DX9_42.dll"), true, false);

                Globals.Log("Installed 'D3DX9_42.dll' file ...");
            }
            else
            {
                // verify correct dll is installed using MD5 Hash
                var d3dFileMD5 = GeneralExtensions.GetMD5Hash(d3dPath);
                var d3dNewMD5 = GeneralExtensions.GetMD5Hash(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.new"));
                var d3dOldMD5 = GeneralExtensions.GetMD5Hash(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.old"));

                if ((File.Exists(luaPath) && d3dFileMD5 != d3dOldMD5) || (!File.Exists(luaPath) && d3dFileMD5 != d3dNewMD5))
                {
                    var dlgMsg1 = "The installed 'D3DX9_42.dll' file MD5 hash value is invalid. Would you like CFSM to update the dll file that is required to play CDLC files?";
                    var dlgMsg2 = "Note: If your CDLC are working fine then answer 'No' and then disable future validation checks in the 'Settings' tab menu.";
                    var dlgMsg = GeneralExtensions.SplitString(dlgMsg1, 30) + Environment.NewLine + Environment.NewLine + GeneralExtensions.SplitString(dlgMsg2, 30);

                    if (DialogResult.No == BetterDialog2.ShowDialog(dlgMsg, "Validating D3DX9_42.dll ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                    {
                        Globals.Log("<WARNING> User aborted updating the 'D3DX9_42.dll' file ...");
                        return false;
                    }

                    if (File.Exists(luaPath))
                        GeneralExtensions.CopyFile(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.old"), Path.Combine(AppSettings.Instance.RSInstalledDir, "D3DX9_42.dll"), true, false);
                    else
                        GeneralExtensions.CopyFile(Path.Combine(Constants.ApplicationFolder, "D3DX9_42.dll.new"), Path.Combine(AppSettings.Instance.RSInstalledDir, "D3DX9_42.dll"), true, false);

                    Globals.Log("Updated 'D3DX9_42.dll' file installation ...");
                }
                else
                    Globals.Log("Validated 'D3DX9_42.dll' file installation ...");
            }

            return true;
        }

        private void ValidateRsDir()
        {
            // validate Rocksmith installation directory
            var rsDir = AppSettings.Instance.RSInstalledDir;
            if (String.IsNullOrEmpty(rsDir) || !Directory.Exists(rsDir))
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select Rocksmith 2014 Installation Directory";
                    fbd.SelectedPath = LocalExtensions.GetSteamDirectory();

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
                SaveSettingsToFile(Globals.DgvCurrent);
            }

            Globals.Log("Validated RS2014 Installation Directory: " + AppSettings.Instance.RSInstalledDir);
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

            GeneralExtensions.DeleteFile(AppSettings.Instance.LogFilePath);
            Globals.MyLog.AddTargetFile(AppSettings.Instance.LogFilePath);
            GeneralExtensions.DeleteFile(Constants.RepairsErrorLogPath);
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
            LoadSettingsFromFile(Globals.DgvCurrent, true);
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            ValidateRsDir();
            SaveSettingsToFile(Globals.DgvCurrent);
        }

        private void chkEnableAutoUpdate_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.EnableAutoUpdate = chkEnableAutoUpdate.Checked;
        }

        private void chkEnableNotifications_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.EnableNotifications = chkEnableNotifications.Checked;

            if (chkEnableNotifications.Checked)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
        }

        private void chkEnableQuarantine_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.EnableQuarantine = chkEnableQuarantine.Checked;
        }

        private void chkIncludeArrangementData_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeArrangementData = chkIncludeArrangementData.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeCustomPacks_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeCustomPacks = chkIncludeCustomPacks.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeRS1CompSongs_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeRS1CompSongs = chkIncludeRS1CompSongs.Checked;
            ToogleRescan(true);
        }

        private void chkIncludeRS2BaseSongs_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeRS2BaseSongs = chkIncludeRS2BaseSongs.Checked;
            ToogleRescan(true);
        }

        private void chkMacMode_Click(object sender, EventArgs e)
        {
            if (chkMacMode.Checked)
            {
                MessageBox.Show("Switching to Mac Compatibility Mode ...  " + Environment.NewLine + "CFSM will automatically restart!", "Mac Mode Enabled ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Globals.Log("Switched to Mac Compatibility Mode ...");
            }
            else
            {
                MessageBox.Show("Switching to PC Compatibility Mode ...  " + Environment.NewLine + "CFSM will automatically restart!", "PC Mode Enabled ...", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Globals.Log("Switched to PC Compatibility Mode ...");
            }

            GeneralExtensions.DeleteFile(Constants.SongsInfoPath);
            AppSettings.Instance.MacMode = chkMacMode.Checked;

            // restart new instance of application and shutdown original
            Application.Restart(); // this triggers frmMain_FormClosing method
            Environment.Exit(0);
        }

        private void chkValidateD3D_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.ValidateD3D = chkValidateD3D.Checked;
            ValidateD3D();
        }

        private void cueRsDir_MouseClick(object sender, MouseEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select RS2014 Installation Directory ...";
                fbd.SelectedPath = LocalExtensions.GetSteamDirectory();
             
                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                cueRsDir.Text = fbd.SelectedPath;
            }

            if (!Directory.Exists(Path.Combine(cueRsDir.Text, "dlc")))
            {
                MessageBox.Show(String.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ToogleRescan(true);

            // update RSInstalledDir after above error check passes
            AppSettings.Instance.RSInstalledDir = cueRsDir.Text;
            Globals.Log("Updated RS2014 Installation Directory: " + AppSettings.Instance.RSInstalledDir);
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

        private void rbCleanOnClosing_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.CleanOnClosing = rbCleanOnClosing.Checked;
        }

        private void tbCreator_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CharterName = txtCharterName.Text;
        }

        private void btnResetThreading_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.MultiThread = -1;
            Globals.Log("CFSM multi threading usage was reset ...");

        }



    }
}