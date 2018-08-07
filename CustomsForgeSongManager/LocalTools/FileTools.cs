using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using GenTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PsarcLoader;

namespace CustomsForgeSongManager.LocalTools
{
    public static class FileTools
    {
        public static void ArchiveFiles(string srcExt, string srcFolder, bool srcDelete = false)
        {
            Globals.Log("Archiving  [" + srcExt + "] files ...");

            var srcFilePaths = Directory.EnumerateFiles(srcFolder, "*" + srcExt + "*").ToList();
            if (!srcFilePaths.Any())
            {
                Globals.Log("No files to archive: " + srcFolder);
                return;
            }

            var fileName = String.Format("{0}{1}.zip", DateTime.Now.ToString("yyyyMMddTHHmmss"), srcExt).GetValidFileName();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = ".zip files (*.zip)|*.zip";
                sfd.FilterIndex = 0;
                sfd.InitialDirectory = Constants.RemasteredArcFolder;
                sfd.FileName = fileName;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                fileName = sfd.FileName;
            }

            // save zip file to 'remastered' folder so that it is not accidently deleted
            try
            {
                if (ZipUtilities.ZipDirectory(srcFolder, Path.Combine(Constants.RemasteredArcFolder, fileName)))
                    Globals.Log("Archive saved to: " + Path.Combine(Constants.RemasteredArcFolder, fileName));
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
                gWorker.WorkDescription = Constants.GWORKER_ACHRIVE;
                gWorker.BackgroundProcess(sender);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        public static void ArtistFolders(string dlcDir, List<SongData> selectedSongs, bool isUndo)
        {

            if (isUndo)
            {
                Globals.Log("Restoring CDLC files to 'dlc/cdlc' folder ...");
                if (!Directory.Exists(Constants.Rs2CdlcFolder))
                    Directory.CreateDirectory(Constants.Rs2CdlcFolder);
            }
            else
                Globals.Log("Organizing CDLC into ArtistName Folders ...");

            var total = selectedSongs.Count();
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            foreach (var songInfo in selectedSongs)
            {
                var srcFilePath = songInfo.FilePath;
                Globals.Log(" - Processing: " + Path.GetFileName(srcFilePath));
                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

                string destFilePath;
                if (isUndo)
                {
                    if (songInfo.PackageAuthor == "Ubisoft")
                        destFilePath = Path.Combine(Constants.Rs2DlcFolder, Path.GetFileName(srcFilePath));
                    else
                        destFilePath = Path.Combine(Constants.Rs2CdlcFolder, Path.GetFileName(srcFilePath));
                }
                else
                {
                    var version = songInfo.PackageVersion;

                    // workaround for old toolkit behavior
                    if (String.IsNullOrEmpty(version) || version == "Null")
                        version = "1";

                    // workaround to identify ODLC
                    if (songInfo.PackageAuthor == "Ubisoft")
                        version = "0";

                    var artistName = songInfo.Artist;
                    var titleName = songInfo.Title;
                    var destFileName = String.Format("{0}_{1}_v{2}{3}", artistName, titleName, version, Constants.PsarcExtension);
                    var destDir = Path.Combine(dlcDir, artistName);
                    destFilePath = Path.Combine(destDir, destFileName);

                    // create new ArtistName folder for song files
                    if (!Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);
                }

                try
                {
                    // no point moving what is already in the correct folder
                    if (srcFilePath != destFilePath)
                    {
                        // update Global SongCollection
                        var song = Globals.MasterCollection.FirstOrDefault(s => s.FilePath == srcFilePath);
                        int index = Globals.MasterCollection.IndexOf(song);
                        Globals.MasterCollection[index].FilePath = destFilePath;
                        GenExtensions.MoveFile(srcFilePath, destFilePath, false);
                    }
                    else
                        skipped++;
                }
                catch
                {
                    if (isUndo)
                        Globals.Log("<ERROR> Failed to restore CDLC: " + srcFilePath);
                    else
                        Globals.Log("<ERROR> Failed to organized CDLC: " + srcFilePath);

                    failed++;
                }
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);

            if (processed > 0)
            {
                // remove empty directories from inside the 'dlc' folder
                new DirectoryInfo(dlcDir).DeleteEmptyDirs();

                if (isUndo)
                    Globals.Log("Sucessully restored CDLC files to 'dlc/cdlc' folder and removed empty ArtistName folders ...");
                else
                    Globals.Log("Sucessully organized and renamed CDLC into ArtistName Folders ...");
            }
        }

        public static void CleanDlcFolder()
        {
            // remove any .bak, .org, .max and .cor files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { Constants.EXT_BAK, Constants.EXT_ORG, Constants.EXT_MAX, Constants.EXT_COR };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories).Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            foreach (var extFilePath in extFilePaths)
            {
                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

                var destFilePath = extFilePath;
                if (extFilePath.Contains(Constants.EXT_ORG))
                    destFilePath = Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(Constants.EXT_MAX))
                    destFilePath = Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(Constants.EXT_COR))
                    destFilePath = Path.Combine(Constants.RemasteredCorFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(Constants.EXT_DUP))
                    destFilePath = Path.Combine(Constants.DuplicatesFolder, Path.GetFileName(extFilePath));


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
            }

            // Commented out ... so devs don't hear, "I deleted all my cdlc files" 
            // Remove originals from Remastered_backup/orignals folder
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_OrgCDLCFolder);
            //backupDir.CleanDir();

            GenericWorker.ReportProgress(processed, total, skipped, failed);

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
                Globals.Log(" - <ERROR> Backup failed ..."); // a bad thing
                Globals.Log(ex.Message);
                return false;
            }

            return true;
        }

        public static bool CreateBackupOfType(List<SongData> songs, string destFolder, string backupExt, bool isBackup = true)
        {
            if (isBackup)
                Globals.Log("Backing up selected CDLC files ...");
            else
                Globals.Log("Moving selected CDLC files ...");

            VerifyCfsmFolders();
            var srcFilePaths = SongFilePaths(songs);
            var total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            foreach (var srcFilePath in srcFilePaths)
            {
                Globals.Log("Processing File: " + Path.GetFileName(srcFilePath));
                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

                try
                {
                    var properExt = Path.GetExtension(srcFilePath);
                    var destFilePath = String.Format(@"{0}{1}{2}", Path.Combine(destFolder, Path.GetFileNameWithoutExtension(srcFilePath)), backupExt, properExt).Trim();

                    if (srcFilePath.Contains(Constants.RS1COMP))
                    {
                        Globals.Log(" - Can not process individual RS1 Compatiblity DLC");
                        ++skipped;
                    }
                    else if (!File.Exists(destFilePath))
                    {
                        GenExtensions.CopyFile(srcFilePath, destFilePath, false);
                        Globals.Log(" - Successfully processed file"); // a good thing
                    }
                    else
                    {
                        Globals.Log(" - File already exists"); // also a good thing
                        skipped++;
                    }
                }
                catch (Exception ex)
                {
                    // it is critical that backup of originals was successful before proceeding
                    Globals.Log(" - <ERROR> CreateBackupOfType method failed ..."); // a bad thing
                    Globals.Log(ex.Message);
                    failed++;
                }

                if (failed > 0)
                    return false;
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);

            if (processed > 0)
            {
                if (isBackup)
                    Globals.Log("Finished backing up selected files ...");
                else
                    Globals.Log("Finished moving selected files ...");

                Globals.Log("Files saved to: " + destFolder);
                return true;
            }

            Globals.Log("No files processed ...");
            return false;
        }

        public static void DeleteFiles(List<SongData> songs, bool verbose = true)
        {
            if (verbose)
                Globals.Log("Deleting selected CDLC files ...");

            var srcFilePaths = SongFilePaths(songs);
            var total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            GenericWorker.InitReportProgress();

            foreach (var srcFilePath in srcFilePaths)
            {
                if (verbose)
                    Globals.Log("Processing File: " + Path.GetFileName(srcFilePath));

                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

                try
                {
                    GenExtensions.DeleteFile(srcFilePath);

                    if (verbose)
                        Globals.Log(" - Successfully deleted file"); // a good thing
                }
                catch (IOException ex)
                {
                    Globals.Log(" - <ERROR> Deletion failed ..."); // a bad thing
                    Globals.Log(ex.Message);
                    failed++;
                }
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);

            if (verbose)
            {
                if (processed > 0)
                    Globals.Log("Finished deleting files ...");
                else
                    Globals.Log("No files deleted ...");
            }
        }

        public static string RestoreOriginal(string srcFilePath)
        {
            var srcFileName = Path.GetFileName(srcFilePath).Replace(Constants.EXT_ORG, "");
            var destFilePath = Path.Combine(Constants.Rs2DlcFolder, srcFileName);
            try
            {
                // make sure [.org] file gets put back into the correct 'dlc' subfolder
                // if CDLC is not found then [.org] file is renamed and put into default 'dlc' folder
                var remasteredFilePath = Globals.MasterCollection.FirstOrDefault(s => s.FilePath.Contains(srcFileName)).FilePath;
                if (remasteredFilePath.Any())
                    destFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), srcFileName);

                // copy but don't delete [.org]
                if (GenExtensions.CopyFile(srcFilePath, destFilePath, true, false))
                    Globals.Log(" - Successfully restored file: " + Path.GetFileName(srcFilePath));
                else
                {
                    Globals.Log(" - <ERROR> Could not restore file: " + Path.GetFileName(srcFilePath));
                    destFilePath = String.Empty;
                }

                return destFilePath;
            }
            catch (Exception ex)
            {
                // this should never happen but just in case
                Globals.Log(" - <ERROR> Restore [" + Constants.EXT_ORG + "] failed ...");
                Globals.Log(ex.Message);
                return String.Empty;
            }
        }

        public static bool IsDirectory(string path)
        {
            bool isDirectory = false;

            try
            {
                FileAttributes attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    isDirectory = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Invalid directory." + Environment.NewLine + ex.Message);
            }

            return isDirectory;
        }

        public static string IsOfficialRepairedDisabled(string filePath)
        {
            if (filePath.ToLower().Contains("disable"))
                return "Disabled";

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
            GenericWorker.InitReportProgress();

            foreach (var bakFilePath in bakFilePaths)
            {
                processed++;
                GenericWorker.ReportProgress(processed, total, skipped, failed);

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
            }

            GenericWorker.ReportProgress(processed, total, skipped, failed);

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

        public static bool ValidateDownloadsDir()
        {
            var dlDirectory = AppSettings.Instance.DownloadsDir;

            if (String.IsNullOrEmpty(dlDirectory) || !Directory.Exists(dlDirectory))
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    // set valid initial default speical folder path
                    fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    fbd.Description = "Select the folder where new CDLC 'Downloads' are stored.";

                    if (fbd.ShowDialog() != DialogResult.OK)
                        return false;

                    dlDirectory = fbd.SelectedPath;
                    AppSettings.Instance.DownloadsDir = dlDirectory;
                    Globals.Settings.SaveSettingsToFile(Globals.DgvCurrent);
                }
            }

            Globals.Log("Validated Downloads Directory: " + dlDirectory + " ...");
            return true;
        }
        
        public static void VerifyCfsmFolders()
        {
            try
            {
                // use 'My Documents/CFSM' to avoid future OS Permission and AV issues
                // validate/create CFSM subfolders            
                GenExtensions.MakeDir(Constants.TempWorkFolder);
                GenExtensions.MakeDir(Constants.BackupsFolder);
                GenExtensions.MakeDir(Constants.DuplicatesFolder);
                GenExtensions.MakeDir(Constants.RemasteredArcFolder);
                GenExtensions.MakeDir(Constants.RemasteredOrgFolder);
                GenExtensions.MakeDir(Constants.RemasteredMaxFolder);
                GenExtensions.MakeDir(Constants.RemasteredCorFolder);
                GenExtensions.MakeDir(Constants.QuarantineFolder);
                GenExtensions.MakeDir(Constants.SongPacksFolder);

                // make sure we have write access to Rocksmith2014 folders
                var rsDir = AppSettings.Instance.RSInstalledDir;
                if (Directory.Exists(rsDir))
                {
                    // make sure we have write access to the RSInstallDir
                    if (!ZipUtilities.EnsureWritableDirectory(rsDir))
                        ZipUtilities.RemoveReadOnlyAttribute(rsDir);

                    // make sure we have write access to all files in 'dlc' folder
                    ZipUtilities.RemoveReadOnlyAttribute(Constants.Rs2DlcFolder);
                }

                // TODO: eventually this conditional check can be depricated
                // if old CFSM remenants exist then move them to 'My Documents/CFSM' 
                if (Directory.Exists(Constants.Rs2CfsmFolder))
                {
                    // leave these important orginal files in RS root (file attribute flags are unchanged)
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "songpacks", "originals"), Constants.Rs2OriginalsFolder);
                    //             
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "archives"), Constants.RemasteredArcFolder);
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "backups"), Constants.BackupsFolder);
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "duplicates"), Constants.DuplicatesFolder);
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "remastered"), Constants.RemasteredFolder);
                    GenExtensions.CopyDir(Path.Combine(Constants.Rs2CfsmFolder, "songpacks"), Constants.SongPacksFolder, false);

                    // make sure we have write access to all files in 'cfsm' folder
                    ZipUtilities.RemoveReadOnlyAttribute(Constants.Rs2CfsmFolder);
                    GenExtensions.DeleteDirectory(Constants.Rs2CfsmFolder);
                }

                GenExtensions.CopyDir(Path.Combine(AppSettings.Instance.RSInstalledDir, "duplicates"), Constants.DuplicatesFolder);
                GenExtensions.DeleteDirectory(Path.Combine(AppSettings.Instance.RSInstalledDir, "cdlc_quarantined"));
                GenExtensions.DeleteDirectory(Path.Combine(AppSettings.Instance.RSInstalledDir, "cdlc_duplicates"));
                GenExtensions.DeleteDirectory(Path.Combine(AppSettings.Instance.RSInstalledDir, "duplicates"));
            }
            catch (Exception ex)
            {
                Globals.Log("<ERROR> Could not verify CFSM work folders ...");
                Globals.Log(ex.Message);
                throw new Exception(); // force app to stop here
            }
        }
    }
}

