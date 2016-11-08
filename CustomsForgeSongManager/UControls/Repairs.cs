using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using DataGridViewTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Xml;

namespace CustomsForgeSongManager.UControls
{
    public partial class Repairs : UserControl, INotifyTabChanged
    {
        private const string TKI_ARRID = "(Arrangement ID by CFSM)";
        private const string TKI_REMASTER = "(Remastered by CFSM)";
        private const string corExt = ".cor.psarc";
        private const string orgExt = ".org.psarc";
        private const string oldCorExt = ".cor";
        private const string oldOrgExt = ".org";
        private const string psarcExt = ".psarc";
        private AbortableBackgroundWorker bWorker;
        private StringBuilder sbErrors = new StringBuilder();
        private List<string> dlcFilePaths = new List<string>();
        private List<string> orgFilePaths = new List<string>();

        public Repairs()
        {
            InitializeComponent();
            CreateFolders();
        }

        private bool PreserveStats
        {
            get { return chkPreserve.Checked; }
        }

        private bool RepairOrg
        {
            get { return chkRepairOrg.Checked; }
        }

        private void CreateFolders()
        {
            if (!Directory.Exists(Constants.Remastered_Folder))
                Directory.CreateDirectory(Constants.Remastered_Folder);

            if (!Directory.Exists(Constants.Remastered_CorruptCDLCFolder))
                Directory.CreateDirectory(Constants.Remastered_CorruptCDLCFolder);

            if (!Directory.Exists(Constants.Remastered_OrgCDLCFolder))
                Directory.CreateDirectory(Constants.Remastered_OrgCDLCFolder);
        }

        private string OfficialOrRepaired(string filePath)
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

        private bool RemasterSong(string filePath)
        {
            Globals.Log("Remastering: " + Path.GetFileName(filePath));

            if (!RepairOrg)
            {
                Globals.Log(" - Making a backup copy (" + orgExt + ") ...");
                try
                {
                    var orgFileName = Path.GetFileName(filePath).Replace(psarcExt, orgExt);
                    var orgFilePath = Path.Combine(Constants.Remastered_OrgCDLCFolder, orgFileName);
                    if (!File.Exists(orgFilePath))
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal);
                        File.Copy(filePath, orgFilePath, false);
                        Globals.Log(" - Sucessfully created backup ...");
                    }
                    else
                        Globals.Log(" - Backup already exists ...");
                }
                catch (Exception ex)
                {
                    // it is critical that backup of originals was successful before proceeding
                    Globals.Log(" - Backup failed ...");
                    Globals.Log(ex.Message);
                    sbErrors.AppendLine(String.Format("{0}, Backup Failed", filePath));
                    return false;
                }
            }
            else
            {
                var dlcFileName = Path.GetFileName(filePath).Replace(orgExt, psarcExt);
                var dlcFilePath = Path.Combine(Constants.Rs2DlcDirectory, dlcFileName);
                try
                {
                    // make sure (.org) file gets put back into the correct 'dlc' subfolder
                    // if CDLC is not found then (.org) file is put into default 'dlc' folder
                    var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                    if (remasteredFilePath != null)
                        dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                    // copy but don't delete (.org)
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Copy(filePath, dlcFilePath, true);
                    filePath = dlcFilePath;
                    Globals.Log(" - Sucessfully restored backup ...");
                }
                catch (Exception ex)
                {
                    // this should never happen but just in case
                    Globals.Log(" - Restore (" + orgExt + ") failed ...");
                    Globals.Log(ex.Message);
                    sbErrors.AppendLine(String.Format("{0}, Restore Failed", filePath));
                    return false;
                }
            }

            try
            {
                // SNG's needs to be regenerated
                // ArrangmentIDs are stored in multiple place and all need to be updated
                // therefore we are going to unpack, apply repair, and repack
                Globals.Log(" - Extracting CDLC artifacts ...");
                DLCPackageData packageData;

                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(filePath);

                // Update arrangement song info
                foreach (RocksmithToolkitLib.DLCPackage.Arrangement arr in packageData.Arrangements)
                {
                    if (!PreserveStats)
                    {
                        // generate new AggregateGraph
                        arr.SongFile = new RocksmithToolkitLib.DLCPackage.AggregateGraph.SongFile { File = "" };

                        // generate new Arrangement IDs
                        arr.Id = IdGenerator.Guid();
                        arr.MasterId = RandomGenerator.NextInt();
                    }

                    // skip vocal and showlight arrangements
                    if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                        continue;

                    // validate SongInfo
                    var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                    songXml.AlbumYear = packageData.SongInfo.SongYear.ToString().GetValidYear();
                    songXml.ArtistName = packageData.SongInfo.Artist.GetValidAtaSpaceName();
                    songXml.Title = packageData.SongInfo.SongDisplayName.GetValidAtaSpaceName();
                    songXml.AlbumName = packageData.SongInfo.Album.GetValidAtaSpaceName();
                    songXml.ArtistNameSort = packageData.SongInfo.ArtistSort.GetValidSortableName();
                    songXml.SongNameSort = packageData.SongInfo.SongDisplayNameSort.GetValidSortableName();
                    songXml.AlbumNameSort = packageData.SongInfo.AlbumSort.GetValidSortableName();
                    songXml.AverageTempo = Convert.ToSingle(packageData.SongInfo.AverageTempo.ToString().GetValidTempo());

                    // write updated xml arrangement
                    using (var stream = File.Open(arr.SongXml.File, FileMode.Create))
                        songXml.Serialize(stream, true);

                    // add comments back to xml arrangement   
                    Song2014.WriteXmlComments(arr.SongXml.File, arr.XmlComments);
                }

                if (!PreserveStats)
                {
                    // add comment to ToolkitInfo to identify CDLC
                    var arrIdComment = packageData.PackageComment;
                    if (String.IsNullOrEmpty(arrIdComment))
                        arrIdComment = TKI_ARRID;
                    else if (!arrIdComment.Contains(TKI_ARRID))
                        arrIdComment = arrIdComment + " " + TKI_ARRID;

                    packageData.PackageComment = arrIdComment;
                }

                // add comment to ToolkitInfo to identify CDLC
                var remasterComment = packageData.PackageComment;
                if (String.IsNullOrEmpty(remasterComment))
                    remasterComment = TKI_REMASTER;
                else if (!remasterComment.Contains(TKI_REMASTER))
                    remasterComment = remasterComment + " " + TKI_REMASTER;

                packageData.PackageComment = remasterComment;

                // add default package version if missing
                if (String.IsNullOrEmpty(packageData.PackageVersion))
                    packageData.PackageVersion = "1";
                else
                    packageData.PackageVersion = packageData.PackageVersion.GetValidVersion();

                // validate packageData (important)
                packageData.Name = packageData.Name.GetValidKey(); // DLC Key                 
                Globals.Log(" - Repackaging Remastered CDLC ...");

                // regenerates the SNG with the repair and repackages               
                using (var psarcNew = new PsarcPackager(true))
                    psarcNew.WritePackage(filePath, packageData);

                Globals.Log(" - Repair was sucessful ...");
            }
            catch (Exception ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See 'remastered_error.log' file ... ");

                // copy (org) to corrupt (cor), delete backup (org), delete original
                var orgFileName = Path.GetFileName(filePath).Replace(psarcExt, orgExt);
                var orgFilePath = Path.Combine(Constants.Remastered_OrgCDLCFolder, orgFileName);
                var corFileName = Path.GetFileName(filePath).Replace(psarcExt, corExt);
                var corFilePath = Path.Combine(Constants.Remastered_CorruptCDLCFolder, corFileName);

                File.SetAttributes(orgFilePath, FileAttributes.Normal);
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Copy(orgFilePath, corFilePath, true);
                File.Delete(orgFilePath);
                File.Delete(filePath);
                sbErrors.AppendLine(String.Format("{0}, Corrupt CDLC", corFilePath));

                return false;
            }

            return true;
        }

        private void ReportProgress(int processed, int total, int skipped, int failed)
        {
            var progress = processed * 100 / total;
            if (Globals.TsProgressBar_Main != null && progress <= 100)
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = progress; });

            GenExtensions.InvokeIfRequired(Globals.TsLabel_MainMsg.GetCurrentParent(), delegate { Globals.TsLabel_MainMsg.Text = String.Format("Files Processed: {0} of {1}", processed, total); });
            GenExtensions.InvokeIfRequired(Globals.TsLabel_StatusMsg.GetCurrentParent(), delegate { Globals.TsLabel_StatusMsg.Text = String.Format("Skipped: {0}  Failed: {1}", skipped, failed); });
            GenExtensions.InvokeIfRequired(this, delegate { this.Refresh(); });
        }

        private void ToggleToolStrip(bool visible)
        {
            dgvLog.Rows.Clear();
            Globals.TsLabel_MainMsg.Text = "";
            Globals.TsLabel_StatusMsg.Text = "";
            Globals.TsLabel_MainMsg.Visible = visible;
            Globals.TsLabel_StatusMsg.Visible = visible;
            Globals.TsProgressBar_Main.Value = 0;
        }

        private void ArchiveCorruptCDLC(object sender, DoWorkEventArgs e)
        {
            GenExtensions.InvokeIfRequired(this, delegate
                {
                    var corFilePaths = Directory.EnumerateFiles(Constants.Remastered_CorruptCDLCFolder, "*" + corExt).ToList();
                    if (!corFilePaths.Any())
                    {
                        Globals.Log("No corrupt CDLC found in: " + Constants.Remastered_CorruptCDLCFolder);
                        return;
                    }

                    var fileName = String.Format("{0}_{1}", "Corrupt_CDLC", DateTime.Now.ToString("yyyyMMdd_hhmm")).GetValidFileName();
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Filter = ".zip files (*.zip)|*.zip";
                        sfd.FilterIndex = 0;
                        sfd.InitialDirectory = Constants.Remastered_Folder;
                        sfd.FileName = fileName;

                        if (sfd.ShowDialog() != DialogResult.OK)
                            return;

                        fileName = sfd.FileName;
                    }

                    Globals.Log("Archiving corrupt CDLC ...");

                    // save zip file to 'remastered' subfolder so that it is not deleted accidently
                    try
                    {
                        if (ZipUtilities.ZipDirectory(Constants.Remastered_CorruptCDLCFolder, Path.Combine(Constants.Remastered_Folder, fileName)))
                            Globals.Log("Archive saved to: " + Path.Combine(Constants.Remastered_Folder, fileName));
                        else
                            Globals.Log("Archiving failed ...");
                    }
                    catch (IOException ex)
                    {
                        Globals.Log(ex.Message);
                    }
                });
        }

        private void CleanupDlcFolder(object sender, DoWorkEventArgs e)
        {
            // remove (.org) and (.cor) files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { orgExt, corExt, oldOrgExt, oldCorExt };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcDirectory, "*.*", SearchOption.AllDirectories)
                .Where(fi => extensions.Any(fi.ToLower().EndsWith)).ToList();

            foreach (var extFilePath in extFilePaths)
            {
                string destFilePath;
                if (extFilePath.EndsWith(orgExt))
                    destFilePath = Path.Combine(Constants.Remastered_OrgCDLCFolder, Path.GetFileName(extFilePath));
                else if (extFilePath.EndsWith(corExt))
                    destFilePath = Path.Combine(Constants.Remastered_CorruptCDLCFolder, Path.GetFileName(extFilePath));
                else // for future expansion
                    destFilePath = Path.Combine(Constants.Remastered_Folder, Path.GetFileName(extFilePath));

                try
                {
                    File.SetAttributes(extFilePath, FileAttributes.Normal);

                    if (!File.Exists(destFilePath))
                    {
                        File.Copy(extFilePath, destFilePath, true);
                        Globals.Log("Moved file: " + Path.GetFileName(extFilePath));
                    }
                    else
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));

                    // this could throw an error if file is "Read-Only" or does not exist
                    File.Delete(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                }
            }

            // Commented out ... so devs don't hear, "I deleted all my cdlc files" 
            // Remove originals from Remastered_backup/orignals folder
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_OrgCDLCFolder);
            //backupDir.CleanDir();

            if (extFilePaths.Any())
            {
                Globals.RescanSongManager = true;
                Globals.Log("Finished cleaning 'dlc' folder and subfolders ...");
            }
            else
                Globals.Log("The 'dlc' folder and subfolders didn't need cleaning ...");
        }

        private void DeleteCorruptFiles(object sender, DoWorkEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all corrupt CDLC files?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                Globals.Log("Deleting corrupt CDLC files ...");
                DirectoryInfo backupDir = new DirectoryInfo(Constants.Remastered_CorruptCDLCFolder);
                if (backupDir.GetFiles("*").Any())
                {
                    backupDir.CleanDir();
                    Globals.Log("Corrupt CDLC deleted ...");
                }
                else
                    Globals.Log("No corrupt CDLC found in: " + Constants.Remastered_CorruptCDLCFolder);
            }
            catch (IOException ex)
            {
                Globals.Log(ex.Message);
            }
        }

        private void RepairAllCDLC(object sender, DoWorkEventArgs e)
        {
            // make sure 'dlc' folder is clean before continuing
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += CleanupDlcFolder;
            // TODO: WorkerComplete causes hang if used here so commented out
            // bWorker.RunWorkerCompleted += WorkerComplete; 

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();

            // wait for the CleanupDlcFolder worker to finish
            while (bWorker.IsBusy)
                Application.DoEvents();

            Globals.Log("Applying Remastered repair to CDLC ...");
            var srcFilePaths = new List<string>();
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcDirectory, "*" + psarcExt, SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();

            if (RepairOrg)
                srcFilePaths = Directory.EnumerateFiles(Constants.Remastered_OrgCDLCFolder, "*" + orgExt).ToList();
            else
                srcFilePaths = dlcFilePaths;

            int total = srcFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;

            foreach (var srcFilePath in srcFilePaths)
            {
                GenExtensions.InvokeIfRequired(this, delegate { dgvLog.ClearSelection(); });
                processed++;

                var officialOrRepaired = OfficialOrRepaired(srcFilePath);
                if (!String.IsNullOrEmpty(officialOrRepaired))
                {
                    if (officialOrRepaired.Contains(@"Official"))
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath), "Skipped ODLC File"); });
                    else if (officialOrRepaired.Contains(@"Remastered"))
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Remastered File"); });
                    else
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Unknown Status File"); });

                    skipped++;
                }
                else
                {
                    // remaster the CDLC file
                    var remSucess = RemasterSong(srcFilePath);
                    if (remSucess)
                    {
                        var message = String.Format("Repair Sucessful ... {0}", PreserveStats ? "Preserved Song Stats" : "Reset Song Stats");
                        if (RepairOrg)
                            message += " Using (" + orgExt + ") File";

                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath).Replace(orgExt, psarcExt), message); });
                    }
                    else
                    {
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath), "Corrupt CDLC ... Moved File and Added to Error Log"); });
                        failed++;
                    }
                }

                ReportProgress(processed, total, skipped, failed);
                GenExtensions.InvokeIfRequired(this, delegate
                    {
                        dgvLog.Rows[dgvLog.Rows.Count - 1].Selected = true;
                        dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.Rows.Count - 1;
                        dgvLog.Refresh();
                    });
            }

            if (failed > 0)
            {
                // error log can be turned into CSV file
                sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
                sbErrors.Insert(0, DateTime.Now.ToString("MM-dd-yy HH:mm") + Environment.NewLine);
                var errorLogPath = Path.Combine(Constants.Remastered_Folder, "remastered_error.log");
                using (TextWriter tw = new StreamWriter(errorLogPath, true))
                {
                    tw.WriteLine(sbErrors + Environment.NewLine);
                    tw.Close();
                }

                Globals.Log("Saved error log ... ");
                Globals.Log(errorLogPath);
            }

#if (!DEBUG)
            CleanupLocalTemp();
#endif

            Globals.RescanSongManager = true;
            Globals.Log("Finished Remastering CDLC ...");
        }

        private void RestoreOrgFiles(object sender, DoWorkEventArgs e)
        {
            Globals.Log("Restoring (" + orgExt + ") CDLC ...");
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcDirectory, "*" + psarcExt, SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            orgFilePaths = Directory.EnumerateFiles(Constants.Remastered_OrgCDLCFolder, "*" + orgExt).ToList();
            var total = orgFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;

            foreach (var orgFilePath in orgFilePaths)
            {
                processed++;
                try
                {
                    var dlcFileName = Path.GetFileName(orgFilePath).Replace(orgExt, psarcExt);
                    var dlcFilePath = Path.Combine(Constants.Rs2DlcDirectory, dlcFileName);

                    // make sure (.org) file gets put back into the correct 'dlc' subfolder
                    // if CDLC is not found then (.org) file is put into default 'dlc' folder
                    var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                    if (remasteredFilePath != null)
                        dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                    // copy but don't delete (.org)
                    File.SetAttributes(orgFilePath, FileAttributes.Normal);
                    File.Copy(orgFilePath, dlcFilePath, true);
                }
                catch (IOException ex)
                {
                    failed++;
                    Globals.Log(ex.Message);
                }

                ReportProgress(processed, total, skipped, failed);
            }

            Globals.Log("CDLC backups restored to original location in 'dlc' folder ...");
            Globals.RescanSongManager = true;
        }

        private void btnArchiveCorruptCDLC_Click(object sender, EventArgs e)
        {
            ToggleToolStrip(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += ArchiveCorruptCDLC;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void btnCleanupDlcFolder_Click(object sender, EventArgs e)
        {
            ToggleToolStrip(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += CleanupDlcFolder;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void btnDeleteCorruptFiles_Click(object sender, EventArgs e)
        {
            ToggleToolStrip(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += DeleteCorruptFiles;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void btnRemasterAllCDLC_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remaster all CDLC files?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ToggleToolStrip(true);
                bWorker = new AbortableBackgroundWorker();
                bWorker.SetDefaults();
                bWorker.DoWork += RepairAllCDLC;
                bWorker.RunWorkerCompleted += WorkerComplete;

                if (!bWorker.IsBusy)
                    bWorker.RunWorkerAsync();
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore (" + orgExt + ") CDLC to the 'dlc' folder?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ToggleToolStrip(true);
                bWorker = new AbortableBackgroundWorker();
                bWorker.SetDefaults();
                bWorker.DoWork += RestoreOrgFiles;
                bWorker.RunWorkerCompleted += WorkerComplete;

                if (!bWorker.IsBusy)
                    bWorker.RunWorkerAsync();
            }
        }

        private void btnViewErrorLog_Click(object sender, EventArgs e)
        {
            string stringLog;
            var errorLogPath = Path.Combine(Constants.Remastered_Folder, "remastered_error.log");

            if (!File.Exists(errorLogPath))
                stringLog = "remastered_error.log is empty ...";
            else
                stringLog = File.ReadAllText(errorLogPath);

            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Width = 700;
                noteViewer.PopulateText(stringLog);
                noteViewer.ShowDialog();
            }
        }
        private void WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            bWorker.Dispose();
            bWorker = null;
        }

        public void TabEnter()
        {
            Globals.Log("Repairs GUI TabEnter ...");
            ToggleToolStrip(true);
        }

        public void TabLeave()
        {
            //ToggleToolStrip(false);
            Globals.Log("Repairs GUI TabLeave...");
        }

        private void CleanupLocalTemp()
        {
            var di = new DirectoryInfo(Path.GetTempPath());

            // 'Local Settings\Temp' in WinXp
            // 'AppData\Local\Temp' in Win7
            // confirm this is the correct temp directory before deleting
            if (di.Parent != null)
            {
                if (di.Parent.Name.Contains("Local") && di.Name == "Temp")
                {
                    foreach (FileInfo file in di.GetFiles())
                        try
                        {
                            File.SetAttributes(file.FullName, FileAttributes.Normal);
                            file.Delete();
                        }
                        catch { /*Don't worry just skip locked file*/ }

                    foreach (DirectoryInfo dir in di.GetDirectories())
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { /*Don't worry just skip locked directory*/ }
                }
            }
        }


    }
}

