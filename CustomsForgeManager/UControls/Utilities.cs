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
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                Process.Start("steam://rungameid/221680");
        }

        private void btnBackupRSProfile_Click(object sender, System.EventArgs e)
        {
            // TODO: confirm this works
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.WorkDirectory, timestamp);
                string userProfilePath = String.Empty;
                string steamProfileDir = String.Empty;

                string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
                // TODO: confirm the following constant for x86 machines
                string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";

                // for WinXP SP3 x86 compatiblity
                try
                {
                    if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Path, "userdata", null).ToString()))
                        steamProfileDir = Registry.GetValue(rsX64Path, "userdata", null).ToString();
                    // TODO: confirm the following is correct for x86 machines
                    if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "UserData", null).ToString()))
                        steamProfileDir = Registry.GetValue(rsX86Path, "UserData", null).ToString();
                }
                catch (NullReferenceException)
                {
                    // needed for WinXP SP3 which throws NullReferenceException when registry not found
                    Globals.Log("RS2014 User Profile Directory not found in Registry");
                    Globals.Log("You will need to manually locate the directory");
                }

                if (String.IsNullOrEmpty(steamProfileDir))
                    using (var fbd = new FolderBrowserDialog())
                    {
                        fbd.Description = "Select Rocksmith 2014 user profile directory location";

                        if (fbd.ShowDialog() != DialogResult.OK) return;
                        steamProfileDir = fbd.SelectedPath;
                    }

                var subdirs = new DirectoryInfo(steamProfileDir).GetDirectories("*", SearchOption.AllDirectories).ToArray();

                if (!subdirs.Any())
                {
                    List<string> files = new List<string>();
                    string[] filePatterns = new string[] { "*_prfldb", "localprofiles.json", "crd" };

                    foreach (var pattern in filePatterns)
                    {
                        var partial = Directory.GetFiles(steamProfileDir, pattern, SearchOption.AllDirectories);
                        files.AddRange(partial);
                    }

                    if (files.Count > 1)
                        userProfilePath = steamProfileDir;
                }
                else
                    foreach (DirectoryInfo info in subdirs)
                        if (info.FullName.Contains(@"221680\remote"))
                        {
                            userProfilePath = info.FullName;
                            break;
                        }

                if (Directory.Exists(userProfilePath))
                {
                    string[] filenames = Directory.GetFiles(userProfilePath, "*", SearchOption.AllDirectories);
                    DotNetZip.ZipFiles(filenames, backupPath);

                    Globals.Log("Created user profile backup:");
                    Globals.Log(backupPath);
                }
                else
                    Globals.Log("Rocksmith 2014 user profile not found!");
            }
            catch (Exception ex)
            {
                Globals.Log("<Error>:" + ex.Message);
            }
        }

        private void btnUploadSong_Click(object sender, System.EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/creators/submit");
        }

        private void btnRequestSong_Click(object sender, System.EventArgs e)
        {
            Process.Start("http://requests.customsforge.com/");
        }


    }
}
