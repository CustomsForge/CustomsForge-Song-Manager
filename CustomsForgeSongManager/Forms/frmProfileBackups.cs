using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using System.IO;
using CustomsForgeSongManager.LocalTools;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmProfileBackups : Form
    {
        public frmProfileBackups()
        {
            InitializeComponent();
        }

        public void PopulateBackupList(List<ProfileData> backups)
        {
            // this method of population allows rows to be deleted when data is unbound
            foreach (ProfileData backup in backups)
                dgvProfileBackups.Rows.Add(backup.Selected, backup.ArchiveDate, backup.ArchiveName, backup.ArchivePath);

            // resize table to fit the actual data
            dgvProfileBackups.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == BetterDialog2.ShowDialog("Delete selected backups?", "Delete Backups", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Question.Handle), "Pick One", 150, 150))
                return;

            try
            {
                // use bottoms up approach to allow for multi row/file deletion
                var rowNdx = dgvProfileBackups.Rows.Count - 1;
                do
                {
                    if (Convert.ToBoolean(dgvProfileBackups.Rows[rowNdx].Cells["colSelect"].Value)) // || dgvProfileBackups.Rows[i].Selected)
                    {
                        var filePath = dgvProfileBackups.Rows[rowNdx].Cells["colPath"].Value.ToString();
                        if (File.Exists(filePath))
                            File.Delete(filePath);

                        dgvProfileBackups.Rows.RemoveAt(rowNdx);
                    }

                    rowNdx--;
                }
                while (rowNdx > -1);
            }
            catch (IOException ex)
            {
                Globals.Log("Unable to delete the profile backup, error: " + ex.Message.ToString());
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            if (dgvProfileBackups.Rows.Count > 0)
            {
                var selectedCount = dgvProfileBackups.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells["colSelect"].Value));
                if (selectedCount == 1)
                {
                    var index = dgvProfileBackups.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToBoolean(r.Cells[0].Value)).Select(r => r.Index).First();
                    RocksmithProfile.RestoreBackup(dgvProfileBackups.Rows[index].Cells["colPath"].Value.ToString(), AppSettings.Instance.RSProfileDir);
                }
                else
                    BetterDialog2.ShowDialog("Select a single profile to restore.", "Restore Backups", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 150, 150);
            }
        }

        private void dgvProfileBackups_SelectionChanged(object sender, EventArgs e)
        {
            dgvProfileBackups.ClearSelection();
        }

    }
}
