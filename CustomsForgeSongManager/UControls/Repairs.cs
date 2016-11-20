using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Xml;
using Arrangement = RocksmithToolkitLib.DLCPackage.Arrangement;

// NOTE: be carefull with use of GenExtensions.InvokeIfRequired
// only apply as needed and fully test for bugs if used,
// HINT: watch for duplicate dgv entries which show up if not used correctly

namespace CustomsForgeSongManager.UControls
{
    public partial class Repairs : UserControl, INotifyTabChanged
    {
        #region Constants

        private const string TKI_ARRID = "(Arrangement ID by CFSM)";
        private const string TKI_MAX5 = "(Max5 by CFSM)";
        private const string TKI_REMASTER = "(Remastered by CFSM)";
        private const string TKI_DDC = "(DDC by CFSM)";
        private const string corExt = ".cor";
        private const string maxExt = ".max"; // backup
        private const string orgExt = ".org"; // backup

        #endregion

        private bool addedDD = false;
        private List<string> bakFilePaths = new List<string>();
        private byte checkByte; // tracks repair checkbox condition
        private List<string> dlcFilePaths = new List<string>();
        private int rFailed;
        private int rProcessed;
        private int rProgress;
        private int rSkipped;
        private int rTotal;
        private StringBuilder sbErrors = new StringBuilder();

        public Repairs()
        {
            InitializeComponent();

            rbRepairMastery.CheckedChanged += RepairOptions_CheckedChanged;
            rbRepairMaxFive.CheckedChanged += RepairOptions_CheckedChanged;
            rbAddDD.CheckedChanged += RepairOptions_CheckedChanged;
            chkPreserve.CheckedChanged += RepairOptions_CheckedChanged;
            chkRepairOrg.CheckedChanged += RepairOptions_CheckedChanged;
            chkRemoveBass.CheckedChanged += RepairOptions_CheckedChanged;
            chkRemoveBonus.CheckedChanged += RepairOptions_CheckedChanged;
            chkRemoveGuitar.CheckedChanged += RepairOptions_CheckedChanged;
            chkRemoveMetronome.CheckedChanged += RepairOptions_CheckedChanged;
            chkRemoveNdd.CheckedChanged += RepairOptions_CheckedChanged;
            chkIgnoreLimit.CheckedChanged += RepairOptions_CheckedChanged;

            CreateFolders();
        }

        private bool IgnoreLimit
        {
            get { return chkIgnoreLimit.Checked; }
        }

        private bool PreserveStats
        {
            get { return chkPreserve.Checked; }
        }

        private bool RepairOrg
        {
            get { return chkRepairOrg.Checked; }
        }

        private bool AddDD
        {
            get { return rbAddDD.Checked; }
        }

        public void ArchiveCorruptCDLC()
        {
            Globals.Log("Archiving corrupt CDLC files ...");

            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorFolder, "*" + corExt + "*").ToList();
            if (!corFilePaths.Any())
            {
                Globals.Log("No corrupt CDLC to archive: " + Constants.RemasteredCorFolder);
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
                if (ZipUtilities.ZipDirectory(Constants.RemasteredCorFolder, Path.Combine(Constants.RemasteredFolder, fileName)))
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

        public void RepairSongs()
        {
            // make sure 'dlc' folder is clean
            CleanDlcFolder();
            Globals.Log("Applying selected repairs to CDLC ...");

            var srcFilePaths = new List<string>();
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*_p.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();

            if (RepairOrg)
                srcFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + orgExt + "*").ToList();
            else
                srcFilePaths = dlcFilePaths;

            var total = srcFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                dgvRepair.ClearSelection();
                processed++;

                var officialOrRepaired = OfficialOrRepaired(srcFilePath);
                if (!String.IsNullOrEmpty(officialOrRepaired))
                {
                    if (officialOrRepaired.Contains("Official"))
                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped ODLC File"); });
                    else if (officialOrRepaired.Contains("Remastered") && rbRepairMastery.Checked)
                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Remastered File"); });
                    else
                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Unknown Status File"); });

                    skipped++;
                }
                else
                {
                    // remaster the CDLC file

                    var rSucess = RemasterSong(srcFilePath);
                    if (rSucess)
                    {
                        var message = String.Format("Repair Sucessful ... {0}", PreserveStats ? "Preserved Song Stats" : "Reset Song Stats");
                        if (RepairOrg)
                            message += " ... Used (" + orgExt + ") File";
                        if (addedDD)
                            message += " ... Added Dynamic Difficulty";


                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath).Replace(orgExt, ""), message); });

                        if (Constants.DebugMode)
                        {
                            // cleanup every nth record
                            if (processed % 50 == 0)
                                GenExtensions.CleanLocalTemp();
                        }
                    }
                    else
                    {
                        var lines = sbErrors.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (lines.Last().ToLower().Contains("maximum"))
                            GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Exceeds Playable Arrangements Limit ... Moved File ... Added To Error Log"); });
                        else
                            GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Corrupt CDLC ... Moved File ... Added To Error Log"); });

                        failed++;
                    }
                }

                GenExtensions.InvokeIfRequired(this, delegate
                    {
                        // dgvRepair.Rows[dgvRepair.Rows.Count - 1].Selected = true;
                        dgvRepair.ClearSelection();
                        dgvRepair.FirstDisplayedScrollingRowIndex = dgvRepair.Rows.Count - 1;
                        dgvRepair.Refresh();
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

                Globals.Log("Saved error log to: " + errorLogPath + " ...");
            }

            if (processed > 0)
            {
                Globals.Log("CDLC repair was sucessful ...");
                Globals.RescanSongManager = true;

                if (Constants.DebugMode)
                    GenExtensions.CleanLocalTemp();
            }
            else
                Globals.Log("No CDLC were repaired ...");
        }

        public void RestoreBackups(string backupExt, string backupFolder)
        {
            Globals.Log("Restoring (" + backupExt + ") CDLC ...");
            dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            bakFilePaths = Directory.EnumerateFiles(backupFolder, "*" + backupExt + "*").ToList();

            var dlcFilePath = String.Empty;
            var total = bakFilePaths.Count;
            int processed = 0, failed = 0, skipped = 0;
            ReportProgress(processed, total, skipped, failed);

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
                    File.SetAttributes(bakFilePath, FileAttributes.Normal);
                    File.Copy(bakFilePath, dlcFilePath, true);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(dlcFilePath), "Sucessfully Restored (" + backupExt + ") Backup"); });
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(dlcFilePath), "Could Not Restore (" + backupExt + ") Backup"); });
                    failed++;
                }

                GenExtensions.InvokeIfRequired(this, delegate
                    {
                        // dgvRepair.Rows[dgvRepair.Rows.Count - 1].Selected = true;
                        dgvRepair.ClearSelection();
                        dgvRepair.FirstDisplayedScrollingRowIndex = dgvRepair.Rows.Count - 1;
                        dgvRepair.Refresh();
                    });

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
            {
                Globals.Log("CDLC (" + backupExt + ") backups restored to original location in 'dlc' folder ...");
                Globals.RescanSongManager = true;
            }
            else
                Globals.Log("No (" + backupExt + ") backup CDLC to restore: " + Constants.RemasteredOrgFolder);
        }

        private void CleanDlcFolder()
        {
            // remove any (.org, (.max) and (.cor) files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { orgExt, maxExt, corExt };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories).Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            ReportProgress(processed, total, skipped, failed);

            foreach (var extFilePath in extFilePaths)
            {
                processed++;
                var destFilePath = extFilePath;
                if (extFilePath.Contains(orgExt))
                    destFilePath = Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(maxExt))
                    destFilePath = Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileName(extFilePath));
                if (extFilePath.Contains(corExt))
                    destFilePath = Path.Combine(Constants.RemasteredCorFolder, Path.GetFileName(extFilePath));

                try
                {
                    File.SetAttributes(extFilePath, FileAttributes.Normal);
                    if (!File.Exists(destFilePath))
                    {
                        File.Copy(extFilePath, destFilePath, true);
                        Globals.Log("Moved file to: " + Path.GetFileName(destFilePath));
                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(destFilePath), "Moved File To: " + Path.GetDirectoryName(destFilePath)); });
                    }
                    else
                    {
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));
                        GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Deleted Duplicate File"); });
                        skipped++;
                    }

                    // this could throw an error if file is "Read-Only" or does not exist
                    File.Delete(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Move File Failed"); });
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

            if (!Directory.Exists(Constants.RemasteredOrgFolder))
                Directory.CreateDirectory(Constants.RemasteredOrgFolder);

            if (!Directory.Exists(Constants.RemasteredMaxFolder))
                Directory.CreateDirectory(Constants.RemasteredMaxFolder);

            if (!Directory.Exists(Constants.RemasteredCorFolder))
                Directory.CreateDirectory(Constants.RemasteredCorFolder);
        }

        private void DeleteCorruptFiles()
        {
            Globals.Log("Deleting corrupt CDLC files ...");
            // very fast but little oppertunity for feedback
            //DirectoryInfo backupDir = new DirectoryInfo(Constants.Remastered_CorruptCDLCFolder);
            //if (backupDir.GetFiles().Any())
            //{
            //    backupDir.CleanDir();                
            var corFilePaths = Directory.EnumerateFiles(Constants.RemasteredCorFolder, "*" + corExt + "*").ToList();
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
                    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(corFilePath), "Deleted Corrupt CDLC"); });
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(corFilePath), "Could Not Delete Corrupt CDLC"); });
                    failed++;
                }

                ReportProgress(processed, total, skipped, failed);
            }

            if (processed > 0)
                Globals.Log("Corrupt CDLC deletion finished ...");
            else
                Globals.Log("No corrupt CDLC to delete: " + Constants.RemasteredCorFolder);
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

        private DLCPackageData MaxFiveArrangements(DLCPackageData packageData)
        {
            if (!IgnoreLimit)
            {
                var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
                if (playableArrCount < 6)
                    return packageData;
            }

            var packageDataKept = new DLCPackageData();
            packageDataKept.Arrangements = new List<Arrangement>();

            foreach (var arr in packageData.Arrangements)
            {
                // skip vocal and showlight arrangements
                if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                    continue;

                var isKept = true;
                var isNDD = false;
                var isBass = false;
                var isGuitar = false;
                var isBonus = false;
                var isMetronome = false;

                var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                var mf = new ManifestFunctions(GameVersion.RS2014);
                if (mf.GetMaxDifficulty(songXml) == 0) isNDD = true;
                if (arr.ArrangementType == ArrangementType.Bass) isBass = true;
                if (arr.ArrangementType == ArrangementType.Guitar) isGuitar = true;
                if (arr.BonusArr) isBonus = true;
                if (arr.Metronome == Metronome.Generate) isMetronome = true;

                switch (checkByte)
                {
                    case 0x00: // repair max arrangement
                        break;
                    case 0x01: // remove - NDD
                        if (isNDD) isKept = false;
                        break;
                    case 0x02: // remove - Bass
                        if (isBass) isKept = false;
                        break;
                    case 0x03: // remove - NDD, Bass
                        if (isNDD || isBass) isKept = false;
                        break;
                    case 0x04: // remove - Guitar
                        if (isGuitar) isKept = false;
                        break;
                    case 0x05: // remove - NDD, Guitar
                        if (isNDD || isGuitar) isKept = false;
                        break;
                    case 0x06: // remove - Bass, Guitar
                        if (isBass || isGuitar) isKept = false;
                        break;
                    case 0x07: // remove - NDD, Bass, Guitar
                        if (isNDD || isBass || isGuitar) isKept = false;
                        break;
                    //
                    case 0x08: // remove - Bonus
                        if (isBonus) isKept = false;
                        break;
                    case 0x09: // remove - Bounus, NDD
                        if (isBonus || isNDD) isKept = false;
                        break;
                    case 0x10: // remove - Bonus, Bass
                        if (isBonus || isBass) isKept = false;
                        break;
                    case 0x11: // remove - Bounus, NDD, Bass
                        if (isBonus || isNDD || isBass) isKept = false;
                        break;
                    case 0x12: // remove - Bonus, Guitar
                        if (isBonus || isGuitar) isKept = false;
                        break;
                    case 0x13: // remove - Bonus, NDD, Guitar
                        if (isBonus || isNDD || isGuitar) isKept = false;
                        break;
                    case 0x14: // remove - Bonus, Bass, Guitar
                        if (isBonus || isBass || isGuitar) isKept = false;
                        break;
                    case 0x15: // remove - Bonus, NDD, Bass, Guitar
                        if (isBonus || isNDD || isBass || isGuitar) isKept = false;
                        break;
                    //
                    case 0x16: // remove - Metronome
                        if (isMetronome) isKept = false;
                        break;
                    case 0x17: // remove - Metronome, NDD
                        if (isMetronome || isNDD) isKept = false;
                        break;
                    case 0x18: // remove - Metronome, Bass
                        if (isMetronome || isBass) isKept = false;
                        break;
                    case 0x19: // remove - Metronome, NDD, Bass
                        if (isMetronome || isNDD || isBass) isKept = false;
                        break;
                    case 0x20: // remove - Metronome, Guitar
                        if (isMetronome || isGuitar) isKept = false;
                        break;
                    case 0x21: // remove - Metronome, NDD, Guitar
                        if (isMetronome || isNDD || isGuitar) isKept = false;
                        break;
                    case 0x22: // remove - Metronome, Bass, Guitar
                        if (isMetronome || isBass || isBass) isKept = false;
                        break;
                    case 0x23: // remove - Metronome, NDD, Bass, Guitar
                        if (isMetronome || isNDD || isBass || isGuitar) isKept = false;
                        break;
                    //
                    case 0x24: // remove - Metronome, Bonus
                        if (isMetronome || isBonus) isKept = false;
                        break;
                    case 0x25: // remove - Metronome, Bounus, NDD
                        if (isMetronome || isBonus || isNDD) isKept = false;
                        break;
                    case 0x26: // remove - Metronome, Bonus, Bass
                        if (isMetronome || isBonus || isBass) isKept = false;
                        break;
                    case 0x27: // remove - Metronome, Bounus, NDD, Bass
                        if (isMetronome || isBonus || isNDD || isBass) isKept = false;
                        break;
                    case 0x28: // remove - Metronome, Bonus, Guitar
                        if (isMetronome || isBonus || isGuitar) isKept = false;
                        break;
                    case 0x29: // remove - Metronome, Bonus, NDD, Guitar
                        if (isMetronome || isBonus || isNDD || isGuitar) isKept = false;
                        break;
                    case 0x30: // remove - Metronome, Bonus, Bass, Guitar
                        if (isMetronome || isBonus || isBass || isGuitar) isKept = false;
                        break;
                    case 0x31: // remove - Metronome, Bonus, NDD, Bass, Guitar
                        if (isMetronome || isBonus || isNDD || isBass || isGuitar) isKept = false;
                        break;
                }

                if (isKept)
                {
                    Globals.Log(" - Kept: " + arr + " ...");
                    packageDataKept.Arrangements.Add(arr);

                    if (!IgnoreLimit && packageDataKept.Arrangements.Count == 5)
                    {
                        Globals.Log(" - Kept first five arrangements matching the repair criteria ...");
                        break;
                    }
                }
                else
                    Globals.Log(" - Removed: " + arr + " ...");

            }

            // replace original arrangements with kept arrangements
            packageData.Arrangements = packageDataKept.Arrangements;
            return packageData;
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
                // DDC generation variables
                addedDD = false;
                SettingsDDC.Instance.LoadConfigXml();
                var phraseLen = SettingsDDC.Instance.PhraseLen;
                // removeSus may be depricated in latest DDC but left here for comptiblity
                var removeSus = SettingsDDC.Instance.RemoveSus;
                var rampPath = SettingsDDC.Instance.RampPath;
                var cfgPath = SettingsDDC.Instance.CfgPath;

                DLCPackageData packageData;
                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath);

                // TODO: selectively remove arrangements here before remastering
                if (rbRepairMaxFive.Checked)
                    packageData = MaxFiveArrangements(packageData);

                var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
                if (playableArrCount > 5)
                    throw new CustomException("Maximum playable arrangement limit exceeded");

                // Update arrangement song info
                foreach (Arrangement arr in packageData.Arrangements)
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

                    // only add DD to NDD arrangements              
                    var mf = new ManifestFunctions(GameVersion.RS2014);
                    var maxDD = mf.GetMaxDifficulty(songXml);

                    if (AddDD && maxDD == 0)
                    {
                        var consoleOutput = String.Empty;
                        var result = DynamicDifficulty.ApplyDD(arr.SongXml.File, phraseLen, removeSus, rampPath, cfgPath, out consoleOutput, true);
                        if (result == -1)
                            throw new CustomException("ddc.exe is missing");

                        if (String.IsNullOrEmpty(consoleOutput))
                            Globals.Log(" - Added DD to " + arr + " ...");
                        else
                            Globals.Log(" - " + arr + " DDC console output: " + consoleOutput + " ...");

                        addedDD = true;
                    }

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

                if (rbRepairMaxFive.Checked)
                {
                    // add comment to ToolkitInfo to identify CDLC
                    var arrIdComment = packageData.PackageComment;
                    if (String.IsNullOrEmpty(arrIdComment))
                        arrIdComment = TKI_MAX5;
                    else if (!arrIdComment.Contains(TKI_MAX5))
                        arrIdComment = arrIdComment + " " + TKI_MAX5;

                    packageData.PackageComment = arrIdComment;
                }

                if (AddDD && addedDD)
                {
                    // add TKI_DDC comment
                    var ddcComment = packageData.PackageComment;
                    if (String.IsNullOrEmpty(ddcComment))
                        ddcComment = TKI_DDC;
                    else if (!ddcComment.Contains(TKI_DDC))
                        ddcComment = ddcComment + " " + TKI_DDC;

                    packageData.PackageComment = ddcComment;
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
            catch (CustomException ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See 'remastered_error.log' file ... ");

                //  copy (org) to maximum (max), delete backup (org), delete original
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();
                var maxFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileNameWithoutExtension(srcFilePath)), maxExt, properExt).Trim();
                File.SetAttributes(orgFilePath, FileAttributes.Normal);
                File.SetAttributes(srcFilePath, FileAttributes.Normal);
                File.Copy(orgFilePath, maxFilePath, true);
                File.Delete(orgFilePath);
                File.Delete(srcFilePath);
                sbErrors.AppendLine(String.Format("{0}, Maximum playable arrangement limit exceeded", maxFilePath));

                return false;
            }
            catch (Exception ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See 'remastered_error.log' file ... ");

                //  copy (org) to corrupt (cor), delete backup (org), delete original
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), orgExt, properExt).Trim();
                var corFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredCorFolder, Path.GetFileNameWithoutExtension(srcFilePath)), corExt, properExt).Trim();
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

        private void ToggleUIControls(bool enable)
        {
            if (!enable)
            {
                //for (int i = dgvRepair.Rows.Count - 1; i >= 0; i--)
                //    dgvRepair.Rows.RemoveAt(i);

                dgvRepair.Rows.Clear();
                dgvRepair.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
                dgvRepair.BackgroundColor = SystemColors.AppWorkspace;
                dgvRepair.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvLook.DefaultCellStyle.BackColor; // or removes selection highlight
                dgvRepair.DefaultCellStyle.SelectionForeColor = dgvRepair.DefaultCellStyle.ForeColor;
                dgvRepair.Refresh();

                Globals.TsLabel_MainMsg.Text = "";
                Globals.TsLabel_StatusMsg.Text = "";
                Globals.TsProgressBar_Main.Value = 0;
            }

            pnlTop.Enabled = enable;
            pnlBottom.Enabled = enable;
        }

        private void RepairOptions_CheckedChanged(object sender, EventArgs e)
        {
            chkPreserve.Enabled = rbRepairMastery.Checked;
            chkRepairOrg.Enabled = rbRepairMastery.Checked;

            if (!rbRepairMastery.Checked)
            {
                chkPreserve.Checked = false;
                chkRepairOrg.Checked = false;
            }

            chkRemoveBass.Enabled = rbRepairMaxFive.Checked;
            chkRemoveBonus.Enabled = rbRepairMaxFive.Checked;
            chkRemoveGuitar.Enabled = rbRepairMaxFive.Checked;
            chkRemoveMetronome.Enabled = rbRepairMaxFive.Checked;
            chkRemoveNdd.Enabled = rbRepairMaxFive.Checked;
            chkIgnoreLimit.Enabled = rbRepairMaxFive.Checked;

            if (!rbRepairMaxFive.Checked)
            {
                chkRemoveBass.Checked = false;
                chkRemoveBonus.Checked = false;
                chkRemoveGuitar.Checked = false;
                chkRemoveMetronome.Checked = false;
                chkRemoveNdd.Checked = false;
                chkIgnoreLimit.Checked = false;
            }

            if (rbAddDD.Checked)
                chkRemoveNdd.Checked = false;

            // possible conditions, 2^2=4
            checkByte = 0x00; // nothing checked
            if (rbRepairMastery.Checked) checkByte += 0x01;
            if (rbRepairMaxFive.Checked) checkByte += 0x02;

            switch (checkByte)
            {
                case 0x00: // no repairs
                    btnRepairSongs.Text = "Make Repair Selection";
                    break;
                case 0x01: // repair mastery 
                    btnRepairSongs.Text = "Repair 100% Mastery Bug";
                    break;
                case 0x02: // repair playable
                    btnRepairSongs.Text = "Repair Maximum Playable Arrangements";

                    break;
                case 0x03: // repair mastery and playable
                    btnRepairSongs.Text = "Repair Mastery and Maximum Playable Arrangements";

                    break;
            }

            // RepairMaxFive possible repair conditions, 2^5=32
            checkByte = 0x00; // nothing checked
            if (chkRemoveNdd.Checked) checkByte += 0x01;
            if (chkRemoveBass.Checked) checkByte += 0x02;
            if (chkRemoveGuitar.Checked) checkByte += 0x04;
            if (chkRemoveBonus.Checked) checkByte += 0x08;
            if (chkRemoveMetronome.Checked) checkByte += 0x16;

            // provide desired button action
            btnRepairSongs.Enabled = rbRepairMastery.Checked || rbRepairMaxFive.Checked && checkByte != 0x00;
            if (rbRepairMaxFive.Checked && checkByte == 0x00)
                btnRepairSongs.Enabled = false;
        }

        private void btnArchiveCorruptSongs_Click(object sender, EventArgs e)
        {
            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "archiving corrupt songs";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        private void btnCleanDlcFolder_Click(object sender, EventArgs e)
        {
            ToggleUIControls(false);
            CleanDlcFolder();
            ToggleUIControls(true);
        }

        private void btnDeleteCorruptSongs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete all corrupt CDLC files?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);
            DeleteCorruptFiles();
            ToggleUIControls(true);
        }

        private void btnRepairSongs_Click(object sender, EventArgs e)
        {
            var curBackColor = BackColor;
            var curForeColor = ForeColor;
            this.BackColor = Color.Black;
            this.ForeColor = Color.Red;
            var diaMsg = "Are you sure you want to repair all CDLC files that are located in" + Environment.NewLine +
                         "the 'dlc' folder and subfolders using the selected repair options?" + Environment.NewLine + Environment.NewLine +
                         "Do you have a complete backup of your CDLC collection?";

            if (DialogResult.Yes != BetterDialog.ShowDialog(diaMsg, "Repair All CDLC ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Question.Handle), "", 0, 150))
            {
                this.BackColor = curBackColor;
                this.ForeColor = curForeColor;
                return;
            }

            this.BackColor = curBackColor;
            this.ForeColor = curForeColor;
            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                if (rbRepairMastery.Checked && !rbRepairMaxFive.Checked)
                    gWorker.WorkDescription = "repairing mastery";
                else if (!rbRepairMastery.Checked && rbRepairMaxFive.Checked)
                    gWorker.WorkDescription = "repairing maximum playable arrangements";
                else if (rbRepairMastery.Checked && rbRepairMaxFive.Checked)
                    gWorker.WorkDescription = "repairing mastery and maximum playable arrangements";
                else
                    gWorker.WorkDescription = "unknown repair";

                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        private void btnRestoreMax_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore (" + maxExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "restoring (.max) backups";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
        }

        private void btnRestoreOrg_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore (" + orgExt + ") CDLC to the 'dlc' folder?", Constants.ApplicationName + " ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            ToggleUIControls(false);

            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "restoring (.org) backups";
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);
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

        private void rbAddDD_Click(object sender, EventArgs e)
        {
            rbAddDD.Checked = !rbAddDD.Checked;
            Thread.Sleep(200); // debounce
        }

        private void rbRepairMastery_Click(object sender, EventArgs e)
        {
            rbRepairMastery.Checked = !rbRepairMastery.Checked;
            Thread.Sleep(200); // debounce
        }

        private void rbRepairMaxFive_Click(object sender, EventArgs e)
        {
            rbRepairMaxFive.Checked = !rbRepairMaxFive.Checked;
            Thread.Sleep(200); // debounce
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

    internal class CustomException : Exception
    {
        public CustomException() : base() { }
        public CustomException(string message) : base(message) { }
    }
}




