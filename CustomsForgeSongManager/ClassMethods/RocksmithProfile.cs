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

namespace CustomsForgeSongManager.ClassMethods
{
    public static class RocksmithProfile
    {
        public static void BackupRestore()
        {
            // TODO: confirm steamProfileDir is being set properly
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.WorkDirectory, timestamp);
                string userProfilePath = String.Empty;
                string steamProfileDir = AppSettings.Instance.RSProfileDir;

                if (String.IsNullOrEmpty(steamProfileDir))
                {
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
                        Globals.Log("You will need to manually locate the user profile directory");
                    }
                }

                if (String.IsNullOrEmpty(steamProfileDir))
                    steamProfileDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\userdata\YOUR_USER_ID\221680\remote");

                if (DialogResult.Yes == BetterDialog.ShowDialog("Backup or restore a Rocksmith 2014 user profile?", "User Profile Backup/Restore", null, "Backup", "Restore", Bitmap.FromHicon(SystemIcons.Question.Handle), "Pick One", 150, 150))
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        fbd.SelectedPath = steamProfileDir;
                        fbd.Description = "Select Rocksmith 2014 user profile directory location" + Environment.NewLine + "HINT: Do a Windows Search for '*_prfldb' files to find the path.";

                        if (fbd.ShowDialog() != DialogResult.OK) return;
                        steamProfileDir = fbd.SelectedPath;
                        AppSettings.Instance.RSProfileDir = steamProfileDir;
                    }

                    var subdirs = new DirectoryInfo(steamProfileDir).GetDirectories("*", SearchOption.AllDirectories).ToArray();

                    if (!subdirs.Any())
                    {
                        List<string> files = new List<string>();
                        string[] filePatterns = new string[] {"*_prfldb", "localprofiles.json", "crd"};

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
                        ZipUtilities.ZipDirectory(userProfilePath, backupPath);
                        Globals.Log(Properties.Resources.CreatedUserProfileBackup);
                        Globals.Log(backupPath);
                    }
                    else
                        Globals.Log(Properties.Resources.Rocksmith2014UserProfileNotFound);
                }
                else
                {
                    // get and restore a profile backup file
                    var zipFiles = Directory.EnumerateFiles(Constants.WorkDirectory, "profile.backup.*.zip", SearchOption.TopDirectoryOnly).ToArray();
                    if (!zipFiles.Any())
                    {
                        MessageBox.Show("No user profile backups found in:" + Environment.NewLine + Constants.WorkDirectory, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    var srcZipPath = String.Empty;

                    using (var ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "(profile.backup.*.zip)|profile.backup.*.zip";

                        ofd.Title = "Select the Rocksmith 2014 user profile backup to restore";
                        ofd.FilterIndex = 1;
                        ofd.InitialDirectory = Constants.WorkDirectory;
                        ofd.CheckPathExists = true;
                        ofd.Multiselect = false;

                        if (ofd.ShowDialog() != DialogResult.OK) return;
                        srcZipPath = ofd.FileName;
                    }

                    // unzip and restore the files
                    using (var fbd = new FolderBrowserDialog())
                    {
                        fbd.SelectedPath = steamProfileDir;
                        fbd.Description = "Select Rocksmith 2014 user profile directory location" + Environment.NewLine + "HINT: Do a Windows Search for '*_prfldb' files to find the path.";

                        if (fbd.ShowDialog() != DialogResult.OK) return;
                        steamProfileDir = fbd.SelectedPath;
                        AppSettings.Instance.RSProfileDir = steamProfileDir;
                    }

                    if (DialogResult.Cancel == MessageBox.Show("Existing files will be overwritten.  You may want" + Environment.NewLine + "to make a backup of the corrupt files before proceeding.  " + Environment.NewLine + "Are you sure you want to restore the profile backup?", Constants.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand))
                        return;

                    if (ZipUtilities.UnzipDir(srcZipPath, steamProfileDir))
                        Globals.Log("Restored user profile backup files ... SUCESSFUL");
                    else
                        Globals.Log("Restored user profile backup files ... FAILED");
                }
            }
            catch (Exception ex)
            {
                Globals.Log("<Error>:" + ex.Message);
            }
        }
    }
}