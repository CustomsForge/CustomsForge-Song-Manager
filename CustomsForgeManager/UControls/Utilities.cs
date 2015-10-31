using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using Microsoft.Win32;

namespace CustomsForgeManager.UControls
{
    public partial class Utilities : UserControl
    {
        public Utilities()
        {
            InitializeComponent();
            PopulateUtilities();
        }

        public void PopulateUtilities()
        {
            Globals.Log("Populating Utilities GUI ...");
        }

        private void btnLaunchSteam_Click(object sender, System.EventArgs e)
        {
            Extensions.LaunchRocksmith2014();
        }

        private void btnBackupRSProfile_Click(object sender, System.EventArgs e)
        {
            Extensions.BackupRocksmithProfile();
        }

        private void btnUploadSong_Click(object sender, System.EventArgs e)
        {
            Extensions.UploadToCustomsForge();
        }

        private void btnRequestSong_Click(object sender, System.EventArgs e)
        {
            Extensions.RequestSongOnCustomsForge();
        }


    }
}
