using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.ComponentModel;
using CFSM.Utils;

namespace CustomsForgeManager.UControls
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
            var settingsPath = Constants.SettingsPath;

            // initialize application startup 
            if (!File.Exists(settingsPath))
            {
                ResetSettings();
                ValidateRsDir();
                SaveSettingsToFile(dgvCurrent);
                return;
            }


            if (!String.IsNullOrEmpty(dgvCurrent.Name))
            {
                Debug.WriteLine("Load DataGridView Settings: " + dgvCurrent.Name);
                Globals.DgvCurrent = dgvCurrent;
            }

            try
            {
                AppSettings.Instance.LoadFromFile(settingsPath, Globals.DgvCurrent);

                cueRsDir.Text = AppSettings.Instance.RSInstalledDir;
                chkIncludeRS1DLC.Checked = AppSettings.Instance.IncludeRS1DLCs;
                chkEnableLogBallon.Checked = AppSettings.Instance.EnabledLogBaloon;
                chkCleanOnClosing.Checked = AppSettings.Instance.CleanOnClosing;
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
                case "CleanOnClosing":
                    chkCleanOnClosing.Checked = AppSettings.Instance.CleanOnClosing;
                    break;
                case "CreatorName":
                    tbCreator.Text = AppSettings.Instance.CreatorName;
                    break;
            }

        }

        public void PopulateSettings(DataGridView dgvCurrent)
        {
            // done everytime loaded in case there are any changes to SongManager
            Globals.Log("Populating Settings GUI for " + dgvCurrent.Name + " ...");
            Globals.DgvCurrent = dgvCurrent;

            // this is weak method and could fail at some point but it works for now
            var parentName = dgvCurrent.Name;
            if (!String.IsNullOrEmpty(dgvCurrent.Parent.Parent.Name))
                parentName = dgvCurrent.Parent.Parent.Name;

            // show which DataGridView is loaded
            lblDgvColumns.Text = String.Format("Settings for {0} from file: {1}", parentName, Path.GetFileName(Constants.GridSettingsPath));

            // initialize column list
            lstDgvColumns.Items.Clear();
            foreach (DataGridViewColumn col in Globals.DgvCurrent.Columns)
            {
                ListViewItem newItem = new ListViewItem(new[] { String.Empty, col.Name, col.HeaderText, col.Width.ToString() }) { Checked = col.Visible };
                lstDgvColumns.Items.Add(newItem);
            }
        }

        public void ResetSettings()
        {
            AppSettings.Instance.Reset();
            Globals.MyLog.Write("Reset settings to defaults ...");
        }

        public void SaveSettingsToFile(DataGridView dgvCurrent)
        {
            Globals.DgvCurrent = dgvCurrent;
            Debug.WriteLine("Save DataGridView Settings: " + dgvCurrent.Name);
            var settings = new RADataGridViewSettings();
            var columns = dgvCurrent.Columns;
            if (columns.Count > 1)
            {
                for (int i = 0; i < columns.Count; i++)
                    settings.ColumnOrder.Add(new ColumnOrderItem { ColumnIndex = i, DisplayIndex = columns[i].DisplayIndex, Visible = columns[i].Visible, Width = columns[i].Width, ColumnName = columns[i].Name });

                AppSettings.Instance.ManagerGridSettings = settings;
            }

            try
            {
                using (var fs = new FileStream(Constants.SettingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    AppSettings.Instance.SerializeXml(fs);
                    Globals.Log("Saved settings file ...");
                }

                if (String.IsNullOrEmpty(dgvCurrent.Name))
                    return;

                if (!Directory.Exists(Constants.GridSettingsDirectory))
                    Directory.CreateDirectory(Constants.GridSettingsDirectory);

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
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
                ValidateRsDir();

            SaveSettingsToFile(Globals.DgvCurrent);
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
            Globals.ReloadSongPacks = true;

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

        private static string GetInstallDirFromRegistry()
        {
            return Extensions.GetSteamDirectory();
        }

        private void tbCreator_TextChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CreatorName = tbCreator.Text;
        }

        private void chkIncludeRS1DLC_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeRS1DLCs = chkIncludeRS1DLC.Checked;
        }

        private void chkCleanOnClosing_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.CleanOnClosing = chkCleanOnClosing.Checked;
        }


    }
}
