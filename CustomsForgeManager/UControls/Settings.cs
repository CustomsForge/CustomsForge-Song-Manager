using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.ComponentModel;
using CustomsForgeManager.Forms;

namespace CustomsForgeManager.UControls
{
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            Leave += Settings_Leave;
            AppSettings.Instance.PropertyChanged += SettingsPropChanged;

#if !CUSTOMUI
            btnCustomize.Visible = false;
#endif
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

                AppSettings.Instance.LoadFromFile(settingsPath, Constants.GridSettingsPath);

                cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                chkIncludeRS1DLC.Checked = AppSettings.Instance.IncludeRS1DLCs;
                chkEnableLogBallon.Checked = AppSettings.Instance.EnabledLogBaloon;
                tbCreator.Text = AppSettings.Instance.CreatorName;

                ValidateRsDir();
            }
            catch (Exception ex)
            {
                Globals.MyLog.Write(String.Format("<Error>: {0}", ex.Message));
            }
        }

        void SettingsPropChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RSInstalledDir":
                    cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                    break;
                case "IncludeRS1DLCs":
                    chkIncludeRS1DLC.Checked = AppSettings.Instance.IncludeRS1DLCs;
                    break;
                case "EnabledLogBaloon":
                    chkEnableLogBallon.Checked = AppSettings.Instance.EnabledLogBaloon;
                    break;
                case "CreatorName":
                    tbCreator.Text = AppSettings.Instance.CreatorName;
                    break;
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

        }

        public void ResetSettings()
        {
            AppSettings.Instance.Reset();
            Globals.MyLog.Write("Reset settings to defaults ...");
        }

        public void SaveSettingsToFile()
        {
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
                    AppSettings.Instance.ManagerGridSettings = settings;
                }
            }

            try
            {
                using (var fs = new FileStream(Constants.SettingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    AppSettings.Instance.SerializeXml(fs);
                    Globals.Log("Saved settings file ...");
                }
                using (var fs = new FileStream(Constants.GridSettingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    AppSettings.Instance.ManagerGridSettings.SerializeXml(fs);
                    Globals.Log("Saved grid settings file ...");
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
            var rsDir = AppSettings.Instance.RSInstalledDir;
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
                    MessageBox.Show(new Form { TopMost = true }, String.Format("Please select a directory that  {0}contains a 'dlc' subdirectory.",
                        Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ValidateRsDir();
                }

                AppSettings.Instance.RSInstalledDir = cueRsDir.Text;
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
            AppSettings.Instance.RSInstalledDir = cueRsDir.Text;
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

        private void tbCreator_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CreatorName = tbCreator.Text;
        } 


    }
}
