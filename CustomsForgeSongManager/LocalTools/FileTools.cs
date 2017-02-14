using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using GenTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PsarcLoader;

namespace CustomsForgeSongManager.LocalTools
{
    public static class FileTools
    {
        #region Class Methods

        public static void ArchiveFiles(string srcExt, string srcFolder, bool srcDelete = false)
        {
            Globals.Log("Archiving  (" + srcExt + ") files ...");

            var srcFilePaths = Directory.EnumerateFiles(srcFolder, "*" + srcExt + "*").ToList();
            if (!srcFilePaths.Any())
            {
                Globals.Log("No files to archive: " + srcFolder);
                return;
            }

            var fileName = String.Format("{0}{1}.zip", DateTime.Now.ToString("yyyyMMdd_hhmm"), srcExt).GetValidFileName();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = ".zip files (*.zip)|*.zip";
                sfd.FilterIndex = 0;
                sfd.InitialDirectory = Constants.ArchiveFolder;
                sfd.FileName = fileName;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                fileName = sfd.FileName;
            }

            // save zip file to 'remastered' folder so that it is not accidently deleted
            try
            {
                if (ZipUtilities.ZipDirectory(srcFolder, Path.Combine(Constants.ArchiveFolder, fileName)))
                    Globals.Log("Archive saved to: " + Path.Combine(Constants.ArchiveFolder, fileName));
                else
                    throw new IOException();

                if (srcDelete)
                {
                    GenExtensions.DeleteDirectory(srcFolder);
                    GenExtensions.MakeDir(srcFolder);
                }
            }
            catch (IOException ex)
            {
                Globals.Log("<ERROR> Archiving failed ...");
                Globals.Log(ex.Message);
            }
        }

        public static void ArchiveFilesWorker(object sender)
        {
            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "archiving files";
                gWorker.BackgroundProcess(sender);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        public static void CleanDlcFolder()
        {
            // remove any .bak, .org, .max and .cor files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { Constants.EXT_BAK, Constants.EXT_ORG, Constants.EXT_MAX, Constants.EXT_COR };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories).Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var extFilePath in extFilePaths)
            {
                processed++;
                var destFilePath = extFilePath;
                if (extFilePath.Contains(Constants.EXT_ORG))
                    destFilePath = Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(Constants.EXT_MAX))
                    destFilePath = Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(Constants.EXT_COR))
                    destFilePath = Path.Combine(Constants.RemasteredCorFolder, Path.GetFileName(extFilePath));

                try
                {
                    if (!File.Exists(destFilePath))
                    {
                        GenExtensions.CopyFile(extFilePath, destFilePath, true, false);
                        Globals.Log("Moved file to: " + Path.GetFileName(destFilePath));
                    }
                    else
                    {
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));
                        skipped++;
                    }

                    // this could throw an error if file is "Read-Only" or does not exist
                    GenExtensions.DeleteFile(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log("<ERROR> Move File Failed: " + Path.GetFileName(extFilePath) + " ...");
                    Globals.Log(ex.Message);
                    failed++;
                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);
            }

            // Commented out ... so devs don't hear, "I deleted all my cdlc files" 
            // Remove originals from Remastered_backup/orignals folder
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_OrgCDLCFolder);
            //backupDir.CleanDir();

            if (processed > 0)
            {
                Globals.RescanSongManager = true;
                Globals.Log("Finished cleaning 'dlc' folder and subfolders ...");
            }
            else
                Globals.Log("The 'dlc' folder and subfolders didn't need cleaning ...");
        }

        public static bool CreateBackupOfType(string srcFilePath, string destFolder, string backupExt)
        {
            try
            {
                var properExt = Path.GetExtension(srcFilePath);
                var destFilePath = String.Format(@"{0}{1}{2}", Path.Combine(destFolder, Path.GetFileNameWithoutExtension(srcFilePath)), backupExt, properExt).Trim();

                if (srcFilePath.Contains(Constants.RS1COMP))
                {
                    Globals.Log(" - Can not backup individual RS1 Compatiblity DLC");
                    return false;
                }

                if (!File.Exists(destFilePath))
                {
                    GenExtensions.CopyFile(srcFilePath, destFilePath, false);
                    Globals.Log(" - Successfully created backup"); // a good thing
                }
                else
                    Globals.Log(" - Backup already exists"); // also a good thing
            }
            catch (Exception ex)
            {
                // it is critical that backup of originals was successful before proceeding
                Globals.Log(" - <ERROR> Backup failed"); // a bad thing
                Globals.Log(ex.Message);
                return false;
            }

            return true;
        }

        public static bool CreateBackupOfType(List<SongData> songs, string destFolder, string backupExt)
        {
            Globals.Log("Backing up selected CDLC files ...");
            VerifyCfsmFolders();
            var srcFilePaths = SongFilePaths(songs);
            var total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                Globals.Log("Processing File: " + Path.GetFileName(srcFilePath));
                processed++;

                try
                {
                    var properExt = Path.GetExtension(srcFilePath);
                    var destFilePath = String.Format(@"{0}{1}{2}", Path.Combine(destFolder, Path.GetFileNameWithoutExtension(srcFilePath)), backupExt, properExt).Trim();

                    if (srcFilePath.Contains(Constants.RS1COMP))
                    {
                        Globals.Log(" - Can not backup individual RS1 Compatiblity DLC");
                        ++skipped;
                    }
                    else if (!File.Exists(destFilePath))
                    {
                        GenExtensions.CopyFile(srcFilePath, destFilePath, false);
                        Globals.Log(" - Successfully created backup"); // a good thing
                    }
                    else
                    {
                        Globals.Log(" - Backup already exists"); // also a good thing
                        skipped++;
                    }
                }
                catch (Exception ex)
                {
                    // it is critical that backup of originals was successful before proceeding
                    Globals.Log(" - <ERROR> Backup failed"); // a bad thing
                    Globals.Log(ex.Message);
                    failed++;
                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);

                if (failed > 0)
                    return false;
            }

            if (processed > 0)
            {
                Globals.Log("Finished backing up files ...");
                return true;
            }

            Globals.Log("No files backed up ...");
            return false;
        }

        public static void DeleteFiles(List<SongData> songs)
        {
            Globals.Log("Deleting selected CDLC files ...");
            var srcFilePaths = SongFilePaths(songs);
            var total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                Globals.Log("Processing File: " + Path.GetFileName(srcFilePath));
                processed++;
                try
                {
                    GenExtensions.DeleteFile(srcFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log("<Error> Could Not Delete File: " + Path.GetFileName(srcFilePath));
                    Globals.Log(ex.Message);
                    failed++;
                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
                Globals.Log("Finished deleting files ...");
            else
                Globals.Log("No files deleted ...");
        }

        public static string GetOriginal(string srcFilePath)
        {
            var dlcFileName = Path.GetFileName(srcFilePath).Replace(Constants.EXT_ORG, "");
            var dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);
            try
            {
                // make sure (.org) file gets put back into the correct 'dlc' subfolder
                // if CDLC is not found then (.org) file is renamed and put into default 'dlc' folder
                var remasteredFilePath = Globals.SongCollection.FirstOrDefault(s => s.FilePath.Contains(dlcFileName)).FilePath;
                if (remasteredFilePath.Any())
                    dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                // copy but don't delete (.org)
                GenExtensions.CopyFile(srcFilePath, dlcFilePath, true, false);
                Globals.Log(" - Successfully restored backup");
                return dlcFilePath;
            }
            catch (Exception ex)
            {
                // this should never happen but just in case
                Globals.Log(" - <ERROR> Restore (" + Constants.EXT_ORG + ") failed");
                Globals.Log(ex.Message);
                return String.Empty;
            }
        }

        public static string OfficialOrRepaired(string filePath)
        {
            ToolkitInfo entryTkInfo;
            using (var browser = new PsarcLoader(filePath, true))
                entryTkInfo = browser.ExtractToolkitInfo();

            if (entryTkInfo == null)
                return "Official";

            if (entryTkInfo != null && entryTkInfo.PackageAuthor != null)
                if (entryTkInfo.PackageAuthor.Equals("Ubisoft"))
                    return "Official";

            if (entryTkInfo != null && entryTkInfo.PackageComment != null)
                if (entryTkInfo.PackageComment.Contains("Remastered"))
                    return "Remastered";

            return null;
        }

        public static void RestoreBackups(string backupExt, string backupFolder)
        {
            Globals.Log("Restoring [" + backupExt + "] CDLC ...");
            // get contents of backup folder
            var bakFilePaths = Directory.EnumerateFiles(backupFolder, "*" + backupExt + "*").ToList();
            // get contents of 'dlc' folder for comparison
            var dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.psarc", SearchOption.AllDirectories)
                .Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && // ignore compatibility packs
                             !fi.ToLower().Contains(Constants.SONGPACK) && // ignore songpacks
                             !fi.ToLower().Contains(Constants.ABVSONGPACK) && // ignore _sp_
                             !fi.ToLower().Contains("inlay")) // ignore inlays
                .ToList();

            var dlcFilePath = String.Empty;
            var total = bakFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var bakFilePath in bakFilePaths)
            {
                processed++;
                try
                {
                    var dlcFileName = Path.GetFileName(bakFilePath).Replace(backupExt, "");
                    dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);

                    // make sure bakExt file gets put back into the correct 'dlc' subfolder
                    // if CDLC is not found then bakExt file is put into default 'dlc' folder
                    var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                    if (remasteredFilePath != null)
                        dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                    // copy but don't delete bakExt
                    GenExtensions.CopyFile(bakFilePath, dlcFilePath, true, false);
                    Globals.Log("Successfully Restored: " + Path.GetFileName(dlcFilePath));
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    Globals.Log("<ERROR> Could Not Restore: " + Path.GetFileName(dlcFilePath));
                    failed++;
                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
            {
                Globals.Log("CDLC backups with extension [" + backupExt + "] were restored to original location in 'dlc' folder ...");
                Globals.RescanSongManager = true;
            }
            else
                Globals.Log("No CDLC were restored from: " + backupFolder);
        }

        public static List<string> SongFilePaths(List<SongData> songs)
        {

            var srcFilePaths = new List<string>();
            songs = songs.Where(s => !s.FilePath.ToLower().Contains(Constants.RS1COMP) &&
                                     !s.FilePath.ToLower().Contains(Constants.SONGPACK) &&
                                     !s.FilePath.ToLower().Contains(Constants.ABVSONGPACK) &&
                                     !s.FilePath.ToLower().Contains("inlay"))
                .ToList();

            songs.ForEach(s => srcFilePaths.Add(s.FilePath));

            return srcFilePaths;
        }

        public static void VerifyCfsmFolders()
        {
            if (!Directory.Exists(Constants.Rs2CfsmFolder))
                Directory.CreateDirectory(Constants.Rs2CfsmFolder);

            if (!Directory.Exists(Constants.BackupFolder))
                Directory.CreateDirectory(Constants.BackupFolder);

            if (!Directory.Exists(Constants.ArchiveFolder))
                Directory.CreateDirectory(Constants.ArchiveFolder);

            if (!Directory.Exists(Constants.RemasteredFolder))
                Directory.CreateDirectory(Constants.RemasteredFolder);

            if (!Directory.Exists(Constants.RemasteredOrgFolder))
                Directory.CreateDirectory(Constants.RemasteredOrgFolder);

            if (!Directory.Exists(Constants.RemasteredMaxFolder))
                Directory.CreateDirectory(Constants.RemasteredMaxFolder);

            if (!Directory.Exists(Constants.RemasteredCorFolder))
                Directory.CreateDirectory(Constants.RemasteredCorFolder);
        }

        public static void ValidateDownloadsDir()
        {
            var downloadsDir = AppSettings.Instance.DownloadsDir;
     
            if (String.IsNullOrEmpty(downloadsDir) || !Directory.Exists(downloadsDir))
            {
                Globals.Log("Select CDLC 'Downloads' directory ...");
                using (var fbd = new FolderBrowserDialog())
                {
                    // set valid initial default speical folder path
                    fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    fbd.Description = "Select the folder where new CDLC 'Downloads' are stored.";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return;

                    downloadsDir = fbd.SelectedPath;
                    AppSettings.Instance.DownloadsDir = downloadsDir;
                }
            }
        }

        #endregion
    }
}
