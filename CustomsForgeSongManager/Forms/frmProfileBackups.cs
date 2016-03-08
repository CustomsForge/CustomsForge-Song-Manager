using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.CustomControls;
using System.IO;
using CustomsForgeSongManager.ClassMethods;

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
            foreach (ProfileData backup in backups)
                dgvProfileBackups.Rows.Add(false, backup.Date, backup.Path);
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            RocksmithProfile.RestoreBackup(dgvProfileBackups.SelectedRows[0].Cells["colProfilePath"].Value.ToString(), AppSettings.Instance.RSProfileDir);
        }

        private void btnDeleteBackup_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgvProfileBackups.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvProfileBackups.Rows[i].Cells["colProfileSelect"].Value) || dgvProfileBackups.Rows[i].Selected)
                    {
                        if (DialogResult.Yes == BetterDialog.ShowDialog("Remove selected backup?", "Remove backup", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Question.Handle), "Pick One", 150, 150))
                        {
                            File.Delete(dgvProfileBackups.Rows[i].Cells["colProfilePath"].Value.ToString());
                            dgvProfileBackups.Rows.RemoveAt(i);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Globals.Log("Unable to delete the profile backup, error: " + ex.Message.ToString());
            }
        }
    }
}
