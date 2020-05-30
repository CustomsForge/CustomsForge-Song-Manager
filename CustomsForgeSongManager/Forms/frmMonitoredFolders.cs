using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmMonitoredFolders : Form
    {
        public frmMonitoredFolders()
        {
            InitializeComponent();

            foreach (var dir in AppSettings.Instance.MonitoredFolders)
                lvMonitoredFolders.Items.Add(dir);

            lvMonitoredFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnCloseMonitored_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRemoveMonitoredFolder_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedDir in lvMonitoredFolders.SelectedItems)
            {
                AppSettings.Instance.MonitoredFolders.Remove(selectedDir.Text);
                lvMonitoredFolders.Items.Remove(selectedDir);
            }
        }

        private void btnAddNewMonitoredFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the folder where new CDLC downloads are stored.";

                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                string currentPath = fbd.SelectedPath;
                if (!AppSettings.Instance.MonitoredFolders.Contains(currentPath))
                {
                    AppSettings.Instance.MonitoredFolders.Add(currentPath);
                    lvMonitoredFolders.Items.Add(currentPath);
                    Globals.Settings.SaveSettingsToFile(Globals.DgvCurrent);
                }
            }

            lvMonitoredFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
