using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Xml;

// NOTE: be carefull with use of GenExtensions.InvokeIfRequired
// only apply as needed and fully test for bugs, i.e. watch for duplicate dgv entries

namespace CustomsForgeSongManager.UControls
{
    public partial class Repairs : UserControl, INotifyTabChanged
    {
        #region Constants

        private const string TKI_ARRID = "(Arrangement ID by CFSM)";
        private const string TKI_REMASTER = "(Remastered by CFSM)";
        private const string corExt = ".cor";
        private const string orgExt = ".org";

        #endregion

        private AbortableBackgroundWorker bWorker;
        private List<string> dlcFilePaths = new List<string>();
        private List<string> orgFilePaths = new List<string>();
        private StringBuilder sbErrors = new StringBuilder();
        private int rProgress, rProcessed, rTotal, rSkipped, rFailed;

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

        private void CleanDlcFolder()
        {
            // remove (.org) and (.cor) files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { orgExt, corExt };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories)
                .Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            var processed = 0; var failed = 0; var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var extFilePath in extFilePaths)
            {
                processed++;
                var destFilePath = extFilePath;
                if (extFilePath.Contains(orgExt))
                    destFilePath = Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(corExt))
                    destFilePath = Path.Combine(Constants.RemasteredCorruptFolder, Path.GetFileName(extFilePath));

                try
                {
                    File.SetAttributes(extFilePath, FileAttributes.Normal);
                    if (!File.Exists(destFilePath))
                    {
                        File.Copy(extFilePath, destFilePath, true);
                        Globals.Log("Moved file to: " + Path.GetFileName(destFilePath));
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(destFilePath), "Moved File To: " + Path.GetDirectoryName(destFilePath)); });
                    }
                    else
                    {
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(extFilePath), "Deleted Duplicate File"); });
                        skipped++;
                    }

                    // this could throw an error if file is "Read-Only" or does not exist
                    File.Delete(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(extFilePath), "Move File Failed"); });
                    failed++;
                }

                ReportProgress(processed, total, skipped, failed);
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

        private void CleanLocalTemp()
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

        private bool CreateBackup(string srcFilePath)
        {
            Globals.Log(" - Making a backup copy (" + orgExt + ") ...");
            try
            {
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();

                if (!File.Exists(orgFilePath))
                {
                    File.SetAttributes(srcFilePath, FileAttributes.Normal);
                    File.Copy(srcFilePath, orgFilePath, false);
                    Globals.Log(" - Sucessfully created backup ..."); // a good thing
                }
                else
                    Globals.Log(" - Backup already exists ..."); // also a good thing
            }
            catch (Exception ex)
            {
                // it is critical that backup of originals was successful before proceeding
                Globals.Log(" - Backup failed ..."); // a bad thing
                Globals.Log(ex.Message);
                sbErrors.AppendLine(String.Format("{0}, Backup Failed", srcFilePath));
                return false;
            }

            return true;
        }

        private void CreateFolders()
        {
            if (!Directory.Exists(Constants.RemasteredFolder))
                Directory.CreateDirectory(Constants.RemasteredFolder);

            if (!Directory.Exists(Constants.RemasteredCorruptFolder))
                Directory.CreateDirectory(Constants.RemasteredCorruptFolder);

            if (!Directory.Exists(Constants.RemasteredOrgFolder))
                Directory.CreateDirectory(Constants.RemasteredOrgFolder);
        }

        private void DeleteCorruptFiles()
        {
            Globals.Log("Deleting corrupt CDLC files ...");
            // very fast but little oppertunity for feedback
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.Remastered_CorruptCDLCFolder);
            //if (backupDir.GetFiles().Any())
            //{
            //    backupDir.CleanDir();                
            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorruptFolder, "*" + corExt + "*").ToList();
            var total = corFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var corFilePath in corFilePaths)
            {
                processed++;
                try
                {
                    File.SetAttributes(corFilePath, FileAttributes.Normal);
                    File.Delete(corFilePath);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(corFilePath), "Deleted Corrupt CDLC"); });
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(corFilePath), "Could Not Delete Corrupt CDLC"); });
                    failed++;
                }

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
                Globals.Log("Corrupt CDLC deletion finished ...");
            else
                Globals.Log("No corrupt CDLC to delete: " + Constants.RemasteredCorruptFolder);
        }

        private string GetOriginal(string srcFilePath)
        {
            var dlcFileName = Path.GetFileName(srcFilePath).Replace(orgExt, "");
            var dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);
            try
            {
                // make sure (.org) file gets put back into the correct 'dlc' subfolder
                // if CDLC is not found then (.org) file is put into default 'dlc' folder
                var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                if (remasteredFilePath != null)
                    dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                // copy but don't delete (.org)
                File.SetAttributes(srcFilePath, FileAttributes.Normal);
                File.Copy(srcFilePath, dlcFilePath, true);
                Globals.Log(" - Sucessfully restored backup ...");
                return dlcFilePath;
            }
            catch (Exception ex)
            {
                // this should never happen but just in case
                Globals.Log(" - Restore (" + orgExt + ") failed ...");
                Globals.Log(ex.Message);
                sbErrors.AppendLine(String.Format("{0}, Restore Failed", srcFilePath));
                return String.Empty;
            }
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

        private bool RemasterSong(string srcFilePath)
        {
            if (RepairOrg)
            {
                srcFilePath = GetOriginal(srcFilePath);
                if (String.IsNullOrEmpty(srcFilePath))
                    return false;
            }

            if (!CreateBackup(srcFilePath))
                return false;

            Globals.Log("Remastering: " + Path.GetFileName(srcFilePath));
            try
            {
                // SNG's needs to be regenerated
                // ArrangmentIDs are stored in multiple place and all need to be updated
                // therefore we are going to unpack, apply repair, and repack
                Globals.Log(" - Extracting CDLC artifacts ...");
                DLCPackageData packageData;

                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath);

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
                    psarcNew.WritePackage(srcFilePath, packageData);

                Globals.Log(" - Repair was sucessful ...");
            }
            catch (Exception ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See 'remastered_error.log' file ... ");

                //  copy (org) to corrupt (cor), delete backup (org), delete original
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();
                var corFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredCorruptFolder, Path.GetFileNameWithoutExtension(srcFilePath)), corExt, properExt).Trim();
                File.SetAttributes(orgFilePath, FileAttributes.Normal);
                File.SetAttributes(srcFilePath, FileAttributes.Normal);
                File.Copy(orgFilePath, corFilePath, true);
                File.Delete(orgFilePath);
                File.Delete(srcFilePath);
                sbErrors.AppendLine(String.Format("{0}, Corrupt CDLC", corFilePath));

                return false;
            }

            return true;
        }

        private void ReportProgress(int processed, int total, int skipped, int failed)
        {
            int progress;
            if (total > 0)
                progress = processed * 100 / total;
            else
                progress = 100;

            // load to private memory so that when user leaves tab they have some data to come back to
            rProgress = progress;
            rProcessed = processed;
            rTotal = total;
            rSkipped = skipped;
            rFailed = failed;

            if (Globals.TsProgressBar_Main != null && progress <= 100)
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = progress; });

            GenExtensions.InvokeIfRequired(Globals.TsLabel_MainMsg.GetCurrentParent(), delegate { Globals.TsLabel_MainMsg.Text = String.Format("Files Processed: {0} of {1}", processed, total); });
            GenExtensions.InvokeIfRequired(Globals.TsLabel_StatusMsg.GetCurrentParent(), delegate { Globals.TsLabel_StatusMsg.Text = String.Format("Skipped: {0}  Failed: {1}", skipped, failed); });
            GenExtensions.InvokeIfRequired(this, delegate { this.Refresh(); });
        }

        private void ToggleControls(bool enable)
        {
            if (!enable)
            {
                //for (int i = dgvLog.Rows.Count - 1; i >= 0; i--)
                //    dgvLog.Rows.RemoveAt(i);

                dgvLog.Rows.Clear();
                dgvLog.Refresh();
                Globals.TsLabel_MainMsg.Text = "";
                Globals.TsLabel_StatusMsg.Text = "";
                Globals.TsProgressBar_Main.Value = 0;
            }

            btnArchiveCorruptCDLC.Enabled = enable;
            btnCleanupDlcFolder.Enabled = enable;
            btnDeleteCorruptFiles.Enabled = enable;
            btnRemasterAllCDLC.Enabled = enable;
            btnRestoreBackup.Enabled = enable;
            btnViewErrorLog.Enabled = enable;
            chkPreserve.Enabled = enable;
            chkRepairOrg.Enabled = enable;
        }

        private void ArchiveCorruptCDLC(object sender, DoWorkEventArgs e)
        {
            Globals.Log("Archiving corrupt CDLC files ...");

            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorruptFolder, "*" + corExt).ToList();
            if (!corFilePaths.Any())
            {
                Globals.Log("No corrupt CDLC to archive: " + Constants.RemasteredCorruptFolder);
                return;
            }

            var fileName = String.Format("{0}_{1}", "Corrupt_CDLC", DateTime.Now.ToString("yyyyMMdd_hhmm")).GetValidFileName();
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = ".zip files (*.zip)|*.zip";
                sfd.FilterIndex = 0;
                sfd.InitialDirectory = Constants.RemasteredFolder;
                sfd.FileName = fileName;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                fileName = sfd.FileName;
            }

            // save zip file to 'remastered' folder so that it is not accidently deleted
            try
            {
                if (ZipUtilities.ZipDirectory(Constants.RemasteredCorruptFolder, Path.Combine(Constants.RemasteredFolder, fileName)))
                    Globals.Log("Archive saved to: " + Path.Combine(Constants.RemasteredFolder, fileName));
                else
                    Globals.Log("Archiving failed ...");
            }
            catch (IOException ex)
            {
                Globals.Log("Archiving failed ...");
                Globals.Log(ex.Message);
            }
        }

        private void RepairAllSongs(object sender, DoWorkEventArgs e)
        {
            // make sure 'dlc' folder is clean
            CleanDlcFolder();

            Globals.Log("Applying Remastered repair to CDLC ...");
            var srcFilePaths = new List<string>();
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*_p.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();

            if (RepairOrg)
                srcFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + orgExt).ToList();
            else
                srcFilePaths = dlcFilePaths;

            var total = srcFilePaths.Count;
            var processed = 0; var failed = 0; var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                dgvLog.ClearSelection();
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

                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath).Replace(orgExt, ""), message); });
                    }
                    else
                    {
                        GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(srcFilePath), "Corrupt CDLC ... Moved File and Added To Error Log"); });
                        failed++;
                    }
                }

                GenExtensions.InvokeIfRequired(this, delegate
                {
                    dgvLog.Rows[dgvLog.Rows.Count - 1].Selected = true;
                    dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.Rows.Count - 1;
                    dgvLog.Refresh();
                });

                ReportProgress(processed, total, skipped, failed);
            }

            if (failed > 0)
            {
                // error log can be turned into CSV file
                sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
                sbErrors.Insert(0, DateTime.Now.ToString("MM-dd-yy HH:mm") + Environment.NewLine);
                var errorLogPath = Path.Combine(Constants.RemasteredFolder, "remastered_error.log");
                using (TextWriter tw = new StreamWriter(errorLogPath, true))
                {
                    tw.WriteLine(sbErrors + Environment.NewLine);
                    tw.Close();
                }

                Globals.Log("Saved error log to: " + errorLogPath);
            }

            if (processed > 0)
            {
                Globals.Log("Remastering CDLC Finished ...");
                Globals.RescanSongManager = true;

                if (Constants.DebugMode)
                    CleanLocalTemp();
            }
            else
                Globals.Log("No CDLC were Remastered ...");
        }

        private void RestoreBackups(object sender, DoWorkEventArgs e)
        {
            Globals.Log("Restoring (" + orgExt + ") CDLC ...");
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            orgFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + orgExt + "*").ToList();

            var dlcFilePath = String.Empty;
            var total = orgFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var orgFilePath in orgFilePaths)
            {
                processed++;
                try
                {
                    var dlcFileName = Path.GetFileName(orgFilePath).Replace(orgExt, "");
                    dlcFilePath = Path.Combine(Constants.Rs2DlcFolder, dlcFileName);

                    // make sure (.org) file gets put back into the correct 'dlc' subfolder
                    // if CDLC is not found then (.org) file is put into default 'dlc' folder
                    var remasteredFilePath = dlcFilePaths.FirstOrDefault(x => x.Contains(dlcFileName));
                    if (remasteredFilePath != null)
                        dlcFilePath = Path.Combine(Path.GetDirectoryName(remasteredFilePath), dlcFileName);

                    // copy but don't delete (.org)
                    File.SetAttributes(orgFilePath, FileAttributes.Normal);
                    File.Copy(orgFilePath, dlcFilePath, true);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(dlcFilePath), "Sucessfully restored backup"); });
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvLog.Rows.Add(Path.GetFileName(dlcFilePath), "Could not restore backup"); });
                    failed++;
                }

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
            {
                Globals.Log("CDLC backups restored to original location in 'dlc' folder ...");
                Globals.RescanSongManager = true;
            }
            else
                Globals.Log("No backup CDLC to restore: " + Constants.RemasteredOrgFolder);
        }

        private void WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = 100; });
            ToggleControls(true);
        }

        private void btnArchiveCorruptCDLC_Click(object sender, EventArgs e)
        {
            ToggleControls(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += ArchiveCorruptCDLC;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void btnCleanupDlcFolder_Click(object sender, EventArgs e)
        {
            ToggleControls(false);
            CleanDlcFolder();
            ToggleControls(true);
        }

        private void btnDeleteCorruptFiles_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all corrupt CDLC files?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleControls(false);
            DeleteCorruptFiles();
            ToggleControls(true);
        }

        private void btnRemasterAllCDLC_Click(object sender, EventArgs e)
        {
            var curBackColor = BackColor;
            var curForeColor = ForeColor;
            this.BackColor = Color.Black;
            this.ForeColor = Color.Red;

            if (MessageBox.Show("Are you sure you want to remaster all CDLC files?" + Environment.NewLine +
                                "Do you have a complete backup of your CDLC collection?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                this.BackColor = curBackColor;
                this.ForeColor = curForeColor;
                return;
            }

            this.BackColor = curBackColor;
            this.ForeColor = curForeColor;

            ToggleControls(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += RepairAllSongs;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();

        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore (" + orgExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleControls(false);
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += RestoreBackups;
            bWorker.RunWorkerCompleted += WorkerComplete;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void btnViewErrorLog_Click(object sender, EventArgs e)
        {
            string stringLog;
            var errorLogPath = Path.Combine(Constants.RemasteredFolder, "remastered_error.log");

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

        public void TabEnter()
        {
            Globals.Log("Repairs GUI TabEnter ...");
            // this is required if user goes in and out of Repair tab to refresh Toolstrip
            GenExtensions.InvokeIfRequired(this, delegate
            {
                Globals.TsLabel_MainMsg.Text = String.Format("Files Processed: {0} of {1}", rProcessed, rTotal);
                Globals.TsLabel_StatusMsg.Text = String.Format("Skipped: {0}  Failed: {1}", rSkipped, rFailed);
                Globals.TsProgressBar_Main.Value = rProgress;
                Globals.TsProgressBar_Main.Visible = true;
                Globals.TsLabel_StatusMsg.Visible = true;
                Globals.TsLabel_MainMsg.Visible = true;
            });
        }

        public void TabLeave()
        {
            Globals.Log("Repairs GUI TabLeave ...");
        }

    }
}

