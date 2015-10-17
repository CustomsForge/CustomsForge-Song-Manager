using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;

namespace CustomsForgeManager.UControls
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            Leave += Settings_Leave;
        }

        public void LoadSettingsFromFile()
        {
            var settingsPath = Constants.SettingsPath;

            // initial application startup or detect bad settings file
            if (!File.Exists(settingsPath))
            {
                ResetSettings();
                ValidateRsDir();
                SaveSettingsToFile();
            }

            try
            {
                using (FileStream fs = new FileStream(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Globals.MySettings = fs.DeserializeXml<AppSettings>();
                    Globals.Log("Loaded settings file ...");
                    fs.Flush();
                }

                cueRsDir.Text = Globals.MySettings.RSInstalledDir;
                chkIncludeRS1DLC.Checked = Globals.MySettings.IncludeRS1DLCs;
                chkEnableLogBallon.Checked = Globals.MySettings.EnabledLogBaloon;

                ValidateRsDir();
            }
            catch (Exception ex)
            {
                Globals.MyLog.Write(String.Format("<Error>: {0}", ex.Message));
            }
        }

        public void PopulateSettings()
        {
            // done everytime loaded in case there are any changes to SongManager
            Globals.Log("Populating Settings GUI ...");

            // initialize column list
            listDisabledColumns.Items.Clear();
            foreach (DataGridViewColumn col in Globals.DgvSongs.Columns)
            {
                ListViewItem newItem = new ListViewItem(new[] { String.Empty, col.Name, col.HeaderText, col.Width.ToString() }) { Checked = col.Visible };
                listDisabledColumns.Items.Add(newItem);
            }

            // initialize variables
            cueRsDir.Text = Globals.MySettings.RSInstalledDir;
            chkIncludeRS1DLC.Checked = Globals.MySettings.IncludeRS1DLCs;
            chkEnableLogBallon.Checked = Globals.MySettings.EnabledLogBaloon;
        }

        public void ResetSettings()
        {
            // initialize object if null
            if (Globals.MySettings == null)
                Globals.MySettings = new AppSettings();

            Globals.MySettings.LogFilePath = Constants.LogFilePath;
            Globals.MySettings.RSInstalledDir = GetInstallDirFromRegistry();
            Globals.MySettings.IncludeRS1DLCs = false;  // changed to false (fewer issues)

            //Baloons are really annoying on win10
            //if (Constants.DebugMode)
            //    Globals.MySettings.EnabledLogBaloon = true;
            //else
            Globals.MySettings.EnabledLogBaloon = false; // fewer notfication issues

            cueRsDir.Text = Globals.MySettings.RSInstalledDir;
            chkIncludeRS1DLC.Checked = Globals.MySettings.IncludeRS1DLCs;
            chkEnableLogBallon.Checked = Globals.MySettings.EnabledLogBaloon;

            Globals.MyLog.Write("Reset settings to defaults ...");
        }

        public void SaveSettingsToFile(bool includeUcSettings = true)
        {
            if (Globals.MySettings == null)
                ResetSettings();

            if (includeUcSettings)
            {
                Globals.MySettings.RSInstalledDir = cueRsDir.Text;
                Globals.MySettings.IncludeRS1DLCs = chkIncludeRS1DLC.Checked;
                Globals.MySettings.EnabledLogBaloon = chkEnableLogBallon.Checked;
            }

            if (Globals.DgvSongs != null)
            {
                var settings = new RADataGridViewSettings();
                var columns = Globals.DgvSongs.Columns;
                if (columns.Count > 1)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        settings.ColumnOrder.Add(new ColumnOrderItem { ColumnIndex = i, DisplayIndex = columns[i].DisplayIndex, Visible = columns[i].Visible, Width = columns[i].Width, ColumnName = columns[i].Name });
                    }
                    Globals.MySettings.ManagerGridSettings = settings;
                }
            }

            var settingsPath = Constants.SettingsPath;
            try
            {
                using (var fs = new FileStream(settingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    Globals.MySettings.SerializeXml(fs);
                    Globals.Log("Saved settings file ...");
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                Globals.Log(String.Format("<Error>: {0}", ex.Message));
            }
        }

        private void ValidateRsDir()
        {
            // validate Rocksmith installation directory
            var rsDir = Globals.MySettings.RSInstalledDir;
            if (String.IsNullOrEmpty(rsDir) || !Directory.Exists(rsDir)) // || rsDir.Text.Contains("Click here"))
            {
                Globals.Log("Select your Rocksmith installation directory ...");
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.Description = "Select the RS2014 installation directory";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return;

                    cueRsDir.Text = fbd.SelectedPath;
                }

                if (!Directory.Exists(Path.Combine(cueRsDir.Text, "dlc")))
                {
                    MessageBox.Show(string.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.",
                        Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ValidateRsDir();
                }

                // this is done in UC SongManager for better protection
                // the default initial load condition does not include RS1 Compatiblity files
                //var dlcFiles = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*.psarc", SearchOption.AllDirectories)
                //    .Where(fi => !fi.ToLower().Contains(Constants.RS1COMP)).ToArray();

                //if (!dlcFiles.Any())
                //{
                //    var msgText = "Houston ... we have a problem!" + Environment.NewLine +
                //                  "There are no Rocksmith 2014 songs in:" + Environment.NewLine +
                //                  Path.Combine(cueRsDir.Text, "dlc") + Environment.NewLine + Environment.NewLine +
                //                  "Please select a Rocksmith 2014" + Environment.NewLine +
                //                  "installation directory that has some songs.";
                //    MessageBox.Show(msgText, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                //    //// prevents write log attempt
                //    //Environment.Exit(0);

                //    ValidateRsDir();
                //}

                Globals.MySettings.RSInstalledDir = cueRsDir.Text;
            }
        }

        private void Settings_Leave(object sender, EventArgs e)
        {
            ValidateRsDir();
            SaveSettingsToFile();
        }

        private void btnSettingsLoad_Click(object sender, EventArgs e)
        {
            LoadSettingsFromFile();
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            ValidateRsDir();
            SaveSettingsToFile();
        }

        private void chkEnableLogBaloon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableLogBallon.Checked)
                Globals.MyLog.AddTargetNotifyIcon(Globals.Notifier);
            else
                Globals.MyLog.RemoveTargetNotifyIcon(Globals.Notifier);
        }

        private void cueRsDir_MouseClick(object sender, MouseEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the RS2014 installation directory";
                fbd.SelectedPath = GetInstallDirFromRegistry();

                if (fbd.ShowDialog() != DialogResult.OK) return;
                cueRsDir.Text = fbd.SelectedPath;
            }

            if (!Directory.Exists(Path.Combine(cueRsDir.Text, "dlc")))
            {
                MessageBox.Show(string.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.", Environment.NewLine),
                    Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // rescan on tabpage change
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;

            // update RSInstalledDir after above error check passes
            Globals.MySettings.RSInstalledDir = cueRsDir.Text;
        }

        private void listDisabledColumns_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            DataGridViewColumn column = Globals.DgvSongs.Columns[e.Item.SubItems[1].Text];
            if (column != null)
            {
                column.Visible = e.Item.Checked;
                column.Width = Convert.ToInt16(e.Item.SubItems[3].Text);
            }
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool deselect = listDisabledColumns.Items[1].Checked;

            for (int i = 1; i < listDisabledColumns.Items.Count; i++)
            {
                listDisabledColumns.Items[i].Checked = !deselect;
            }
        }

        private static string GetInstallDirFromRegistry()
        {            
            return Extensions.GetSteamDirectory();
        }


    }
}
