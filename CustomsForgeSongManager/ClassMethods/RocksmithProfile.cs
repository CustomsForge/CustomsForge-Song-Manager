using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFSM.GenTools;
using CustomsForgeSongManager.CustomControls;
using CustomsForgeSongManager.DataObjects;
using Microsoft.Win32;
using System.Globalization;
using CustomsForgeSongManager.Forms;
using System.Management;

namespace CustomsForgeSongManager.ClassMethods
{
    public static class RocksmithProfile
    {
        //Win XP: HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\\UserData (My installed to ..\Valve\Steam\\InstallPath )
        //Win 7 32-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\\InstallPath
        //Win 7 64-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\\InstallPath
        //Win 8 32-bit: 
        //Win 8 64-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\\InstallPath
        //Win 8.1 32-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\\InstallPath
        //Win 8.1 64-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\\InstallPath
        //Win 10 32-bit:
        //Win 10 64-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\\InstallPath

        public static string DetectWin()
        {
            string result = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
            foreach (ManagementObject os in searcher.Get())
            {
                result = os["Caption"].ToString();
                break;
            }

            return result;
        }

        public static int AmountOfProfileFiles(string path)
        {
            var subdirs = new DirectoryInfo(path).GetDirectories("*", SearchOption.AllDirectories).ToArray();

            if (!subdirs.Any())
            {
                List<string> files = new List<string>();
                string[] filePatterns = new string[] { "*_prfldb", "localprofiles.json", "crd" };

                foreach (var pattern in filePatterns)
                {
                    var partial = Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
                    files.AddRange(partial);
                }

                return files.Count();
            }
            return -1;
        }

        public static void GetProfileDirPath()
        {
            bool found = false;
            string steamDirPath = "", userDirPath = AppSettings.Instance.RSProfileDir;
            if (String.IsNullOrEmpty(userDirPath) || AmountOfProfileFiles(userDirPath) <= 0) //If RS profile dir path is empty or if there's no profile files on the existing path, search for the correct path
            {
                string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
                string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";

                try
                {
                    if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Path, "InstallPath", null).ToString()))
                    {
                        steamDirPath = Registry.GetValue(rsX64Path, "InstallPath", null).ToString();
                        found = true;
                    }
                    else if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "UserData", null).ToString()))
                    {
                        steamDirPath = Registry.GetValue(rsX86Path, "UserData", null).ToString();     // TODO: confirm the following is correct for x86 machines
                        found = true;
                    }
                    else if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "InstallPath", null).ToString()))
                    {
                        steamDirPath = Registry.GetValue(rsX86Path, "InstallPath", null).ToString();
                        found = true;
                    }
                }
                catch (NullReferenceException)
                {
                    if (!found) //To prevent unnecessesary showing of the message
                    {
                        // needed for WinXP SP3 which throws NullReferenceException when registry not found
                        Globals.Log("RS2014 User Profile Directory not found in Registry");
                        Globals.Log("You will need to manually locate the user profile directory");
                    }
                }
            }

            if (String.IsNullOrEmpty(steamDirPath)) //TODO: Any point of actually doing this - maybe point it to Steam\userdata instead of the whole apth?   
                steamDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\userdata\YOUR_USER_ID\221680\remote");

            if (!Directory.Exists(steamDirPath)) //If we have a non existing path, ask the user to manually point the app to the correct location
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = steamDirPath;
                    fbd.Description = "Select Rocksmith 2014 user profile directory location" + Environment.NewLine + "HINT: Do a Windows Search for '*_prfldb' files to find the path.";

                    if (fbd.ShowDialog() != DialogResult.OK) return;
                    userDirPath = fbd.SelectedPath;
                }
            }

            if (AmountOfProfileFiles(steamDirPath) <= 0) //If we have path of the Steam folder, get path of the RS profile subfolder
            {
                var subdirs = new DirectoryInfo(steamDirPath).GetDirectories("*", SearchOption.AllDirectories).ToArray();

                foreach (DirectoryInfo info in subdirs)
                {
                    if (info.FullName.Contains(@"221680\remote"))
                    {
                        userDirPath = info.FullName;
                        break;
                    }
                }
            }

            //If we have the correct path, save it
            if (AmountOfProfileFiles(userDirPath) > 0)
                AppSettings.Instance.RSProfileDir = userDirPath;
        }

        public static void BackupRestore()
        {
            //TODO: confirm steamProfileDir is being set properly
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.WorkDirectory, timestamp);
                string userProfilePath = String.Empty;
                string steamProfileDir = AppSettings.Instance.RSProfileDir;

                if (String.IsNullOrEmpty(AppSettings.Instance.RSProfileDir) || AmountOfProfileFiles(AppSettings.Instance.RSProfileDir) <= 0)
                    GetProfileDirPath();

                if (!String.IsNullOrEmpty(AppSettings.Instance.RSProfileDir) || AmountOfProfileFiles(AppSettings.Instance.RSProfileDir) > 0) //Proceed only if there's a Steam folder has been detected
                {
                    if (DialogResult.Yes == BetterDialog.ShowDialog("Backup or restore a Rocksmith 2014 user profile?", "User Profile Backup/Restore", null, "Backup", "Restore", Bitmap.FromHicon(SystemIcons.Question.Handle), "Pick One", 150, 150))
                    {
                        BackupProfiles(AppSettings.Instance.RSProfileDir, backupPath);
                    }
                    else
                    { //Restore profile backup
                        //check if there's any backups to restore 
                        var zipFiles = Directory.EnumerateFiles(Constants.WorkDirectory, "profile.backup.*.zip", SearchOption.TopDirectoryOnly).ToArray();
                        if (!zipFiles.Any())
                        {
                            MessageBox.Show("No user profile backups found in:" + Environment.NewLine + Constants.WorkDirectory, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                        frmProfileBackups frmBackups = new frmProfileBackups();
                        frmBackups.PopulateBackupList(GetProfileBackupsList());
                        frmBackups.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Log("<Error>:" + ex.Message);
            }
        }

        public static void BackupProfiles(string userProfilePath, string backupPath)
        {
            if (Directory.Exists(userProfilePath))
            {
                ZipUtilities.ZipDirectory(userProfilePath, backupPath);
                Globals.Log(Properties.Resources.CreatedUserProfileBackup);
                Globals.Log(backupPath);
            }
            else
                Globals.Log(Properties.Resources.Rocksmith2014UserProfileNotFound);
        }

        public static void RestoreBackup(string backupPath, string steamProfileDir)
        {
            //using (var ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "(profile.backup.*.zip)|profile.backup.*.zip";

            //    ofd.Title = "Select the Rocksmith 2014 user profile backup to restore";
            //    ofd.FilterIndex = 1;
            //    ofd.InitialDirectory = Constants.WorkDirectory;
            //    ofd.CheckPathExists = true;
            //    ofd.Multiselect = false;

            //    if (ofd.ShowDialog() != DialogResult.OK) return;
            //    srcZipPath = ofd.FileName;
            //}

            // unzip and restore the files
            if (DialogResult.Cancel == MessageBox.Show("Existing files will be overwritten.  You may want" + Environment.NewLine + "to make a backup of the corrupt files before proceeding.  " + Environment.NewLine + "Are you sure you want to restore the profile backup?", Constants.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand))
                return;

            if (ZipUtilities.UnzipDir(backupPath, steamProfileDir))
                Globals.Log("Restored user profile backup files ... SUCESSFUL");
            else
                Globals.Log("Restored user profile backup files ... FAILED");
        }

        public static List<ProfileData> GetProfileBackupsList()
        {
            List<ProfileData> backups = new List<ProfileData>();
            ProfileData backup = new ProfileData();
            DateTime date = new DateTime();
            string dateString = "";

            foreach (string backupPath in Directory.EnumerateFiles(Constants.WorkDirectory, "profile.backup.*.zip", SearchOption.TopDirectoryOnly).ToList())
            {
                dateString = Path.GetFileName(backupPath).Replace("profile.backup.", "").Replace(".zip", "");

                date = DateTime.ParseExact(dateString, "M-d-yyyy.H-m-s", CultureInfo.InvariantCulture);

                backup.Path = backupPath;
                backup.Date = date;

                backups.Add(backup);
            }

            return backups;
        }
    }
}