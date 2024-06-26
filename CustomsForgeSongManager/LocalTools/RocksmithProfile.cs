﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using GenTools;
using Microsoft.Win32;
using System.Globalization;
using System.Management;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using UserProfileLib;
using System.Text;

namespace CustomsForgeSongManager.LocalTools
{
    public static class RocksmithProfile
    {
        // Win XP:         HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\InstallPath
        // Win 7 32-bit:   HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\InstallPath
        // Win 7 64-bit:   HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\InstallPath
        // Win 8 32-bit: 
        // Win 8 64-bit:   HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\InstallPath
        // Win 8.1 32-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam\InstallPath
        // Win 8.1 64-bit: HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\InstallPath
        // Win 10 32-bit:
        // Win 10 64-bit:  HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam\InstallPath

        // TODO: use method to detect Win
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

        public static bool FoundProfileBackups(string profileBackupsDir)
        {
            if (!Directory.Exists(profileBackupsDir))
                return false;

            string[] filePatterns = new string[] { "ProfileBackup_*.zip" };
            foreach (var filePattern in filePatterns)
            {
                if (Directory.EnumerateFiles(profileBackupsDir, filePattern, SearchOption.TopDirectoryOnly).Any())
                    return true;
            }

            return false;
        }

        public static string GetRemoteDir(bool confirmPath = true)
        {
            bool foundSteamDirPath = false;
            var remoteDirPath = String.Empty;
            var steamDirPath = String.Empty;
            string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
            string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";

            try
            {
                if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Path, "InstallPath", null).ToString()))
                {
                    steamDirPath = Registry.GetValue(rsX64Path, "InstallPath", null).ToString();
                    foundSteamDirPath = true;
                }
                // TODO: confirm the following is correct for x86 machines
                else if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "UserData", null).ToString()))
                {
                    steamDirPath = Registry.GetValue(rsX86Path, "UserData", null).ToString();
                    foundSteamDirPath = true;
                }
                else if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "InstallPath", null).ToString()))
                {
                    steamDirPath = Registry.GetValue(rsX86Path, "InstallPath", null).ToString();
                    foundSteamDirPath = true;
                }
            }
            catch (NullReferenceException)
            {
                if (!foundSteamDirPath) // prevents unnecessesary display of the message
                {
                    // needed for WinXP SP3 which throws NullReferenceException when registry not found
                    Globals.Log("RS2014 User Profile Directory not found in Registry");
                    Globals.Log("You will need to manually locate the user profile directory");
                }
            }

            if (foundSteamDirPath && FoundProfileBackups(steamDirPath))
            {
                var subdirs = new DirectoryInfo(steamDirPath).GetDirectories("*", SearchOption.AllDirectories).ToArray();

                foreach (DirectoryInfo info in subdirs)
                {
                    if (info.FullName.Contains(@"221680\remote"))
                    {
                        remoteDirPath = info.FullName;
                        break;
                    }
                }
            }

            // user should still confirm location of remoteDirPath
            if (confirmPath)
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    if (!String.IsNullOrEmpty(AppSettings.Instance.RSProfileDir.Trim()))
                        fbd.SelectedPath = AppSettings.Instance.RSProfileDir;
                    else
                    {
                        if (String.IsNullOrEmpty(remoteDirPath))
                            fbd.SelectedPath = steamDirPath;
                        else
                            fbd.SelectedPath = remoteDirPath;
                    }

                    fbd.Description = "Select the Rocksmith 2014 user profile directory location." + Environment.NewLine + "HINT: Do a Windows Search for '*_prfldb' files to find the path" + Environment.NewLine + "then back out of the subfolder and select the '221680' root folder.";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return null;

                    remoteDirPath = fbd.SelectedPath;
                }
            }

            if (!String.IsNullOrEmpty(remoteDirPath))
            {
                AppSettings.Instance.RSProfileDir = remoteDirPath;
                Globals.Log("User Profile Directory changed to: " + remoteDirPath);
            }

            return remoteDirPath;
        }

        public static void BackupRestore(bool resetProfileDirPath)
        {
            try
            {
                // locate Steam 'remote' folder
                if (resetProfileDirPath || String.IsNullOrEmpty(AppSettings.Instance.RSProfileDir.Trim()))
                {
                    if (String.IsNullOrEmpty(GetRemoteDir()))
                        return;
                }

                if (DialogResult.Yes == BetterDialog2.ShowDialog("Backup or restore a Rocksmith 2014 user profile?", "User Profile Backup/Restore", null, "Backup", "Restore", Bitmap.FromHicon(SystemIcons.Question.Handle), "Pick One", 150, 150))
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMddTHHmmss"); // use ISO8601 format
                    var backupFileName = String.Format("ProfileBackup_{0}.zip", timestamp);
                    var backupPath = Path.Combine(Constants.ProfileBackupsFolder, backupFileName);
                    BackupProfiles(AppSettings.Instance.RSProfileDir, backupPath);
                }
                else
                {
                    if (FoundProfileBackups(Constants.ProfileBackupsFolder))
                    {
                        frmProfileBackups frmBackups = new frmProfileBackups();
                        frmBackups.PopulateBackupList(GetProfileBackupsList());
                        frmBackups.Show();
                    }
                    else
                        Globals.Log("<Error>: No Profile Backups Found ...");
                }
            }
            catch (Exception ex)
            {
                Globals.Log("<Error>: " + ex.Message);
                Globals.Log(" - Right mouse click the 'User Profiles' button to reset the path ...");
            }
        }

        public static void BackupProfiles(string remoteDirPath, string backupPath)
        {
            if (!Directory.Exists(Constants.ProfileBackupsFolder))
                Directory.CreateDirectory(Constants.ProfileBackupsFolder);

            // backup the entire 221680 folder/subfolders which includes remotecache.vdf file
            var ndx221680 = remoteDirPath.IndexOf("221680", StringComparison.OrdinalIgnoreCase);
            if (ndx221680 != -1)
                remoteDirPath = remoteDirPath.Substring(0, ndx221680 + 6);

            if (Directory.Exists(remoteDirPath))
            {
                if (File.Exists(backupPath))
                    Globals.Log(" - Official User Profile Backup already exists ...");
                else
                {
                    var vdfFile = Directory.EnumerateFiles(remoteDirPath, "remotecache.vdf", SearchOption.AllDirectories).ToList();
                    if (!vdfFile.Any())
                        Globals.Log("<WARNING> Did not find remotecache.vdf file ...");

                    if (ZipUtilities.ZipDirectory(remoteDirPath, backupPath, preserveRoot: true))
                    {
                        Globals.Log(" - Successfully Created Official User Profile Backup ...");
                        Globals.Log(" - From: " + remoteDirPath);
                        Globals.Log(" - To: " + backupPath);
                    }
                    else
                        Globals.Log(" - <ERROR> Backup FAILED ...");
                }
            }
            else
            {
                Globals.Log(" - Detected Unofficial User Profile Path ...");
                // simple backup of unofficial profile path (missing 221680 directory) internally checks if backup already exists
                FileTools.CreateBackupOfType(remoteDirPath, Constants.ProfileBackupsFolder, Constants.EXT_BAK);
            }
        }

        public static void RestoreBackup(string backupPath, string steamProfileDir)
        {
            // unzip and restore the files
            if (DialogResult.Cancel == MessageBox.Show("Existing files will be overwritten.  Do you may want" + Environment.NewLine + "to make a backup of the corrupt files before proceeding.  " + Environment.NewLine + "Are you sure you want to restore the profile backup?", Constants.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand))
                return;

            Globals.Log("Restore user profile ...");

            // restore 221680 folder/subfolders which includes important remotecache.vdf
            if (steamProfileDir.Contains("221680\\remote")) // official steam dir
                steamProfileDir = Path.GetDirectoryName(Path.GetDirectoryName(steamProfileDir));

            if (ZipUtilities.UnzipDir(backupPath, steamProfileDir))
            {
                Globals.Log("From: " + backupPath);
                Globals.Log("To: " + steamProfileDir);
                Globals.Log("User profile restore ... SUCCESSFUL");
            }
            else
                Globals.Log("User profile restore ... FAILED");
        }

        public static List<ProfileData> GetProfileBackupsList()
        {
            List<ProfileData> backups = new List<ProfileData>();

            foreach (string backupPath in Directory.EnumerateFiles(Constants.ProfileBackupsFolder, "ProfileBackup_*.zip", SearchOption.TopDirectoryOnly).ToList())
            {
                var dateString = Path.GetFileName(backupPath).Replace("ProfileBackup_", "").Replace(".zip", "");
                var dateTime = DateTime.ParseExact(dateString, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture);

                ProfileData backup = new ProfileData();
                backup.Selected = false;
                backup.ArchivePath = backupPath;
                backup.ArchiveDate = dateTime;

                backups.Add(backup);
            }

            return backups;
        }

        public static string SelectProfile()
        {
            using (var ofd = new OpenFileDialog())
            {
                var srcDir = GetRemoteDir(false);

                if (string.IsNullOrEmpty(srcDir))
                    srcDir = AppSettings.Instance.RSProfileDir;

                if (string.IsNullOrEmpty(srcDir))
                {
                    srcDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    if (Environment.OSVersion.Version.Major >= 6)
                        srcDir = Directory.GetParent(srcDir).ToString();

                    if (Constants.DebugMode)
                        srcDir = "D:\\Temp"; // for dev testing
                }

                ofd.Filter = "All Files (*.*)|*.*|Game Save Profiles (*.json, *_prfldb, crd)|*.json; *_prfldb; crd";
                ofd.Title = "Select the Rocksmith 2014 User Profile file";
                ofd.FilterIndex = 2;
                ofd.InitialDirectory = srcDir;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() != DialogResult.OK)
                    return null;

                var profilePath = ofd.FileName;
                var remoteDirPath = Path.GetDirectoryName(profilePath);
                AppSettings.Instance.RSProfileDir = remoteDirPath;
                Globals.Log("User Profile Directory changed to: " + remoteDirPath);
                AppSettings.Instance.RSProfilePath = profilePath;

                return profilePath;
            }
        }

    }
}