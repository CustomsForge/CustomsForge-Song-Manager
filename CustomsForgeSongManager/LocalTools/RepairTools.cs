using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CustomsForgeSongManager.DataObjects;
using CFSM.GenTools;
using CustomsForgeSongManager.UControls;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Xml;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib;

namespace CustomsForgeSongManager.LocalTools
{
    class RepairTools
    {
        #region Constants

        public static string corExt = ".cor";
        public static string maxExt = ".max"; // backup
        public static string orgExt = ".org"; // backup

        #endregion

        private static bool addedDD = false;
        private static bool ddError = false;
        private static List<string> bakFilePaths = new List<string>();
        private static List<string> dlcFilePaths = new List<string>();
        #region Other

        private void ReportProgress(int processed, int total, int skipped, int failed)
        {
            //Should be done inside specific UC-s, to keep current values saved?

            /* int progress;
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
             */
        }
        #endregion

        #region Backups
        private void ValidateBackupFolders()
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

        private static bool CreateBackup(string srcFilePath, ref StringBuilder sbErrors)
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

        public void CleanDlcFolder()
        {
            ValidateBackupFolders();

            // remove any (.org, (.max) and (.cor) files from dlc folder and subfolders
            Globals.Log("Cleaning 'dlc' folder and subfolders ...");
            string[] extensions = { orgExt, maxExt, corExt };
            var extFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.*", SearchOption.AllDirectories).Where(fi => extensions.Any(fi.ToLower().Contains)).ToList();

            var total = extFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            //     ReportProgress(processed, total, skipped, failed);

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
                        //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(destFilePath), "Moved File To: " + Path.GetDirectoryName(destFilePath)); });

                        //TODO: gotta fix this ^, or just don't remove this method from BulkRepairs 
                    }
                    else
                    {
                        Globals.Log("Deleted duplicate file: " + Path.GetFileName(extFilePath));
                        //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Deleted Duplicate File"); });
                        skipped++;
                    }

                    // this could throw an error if file is "Read-Only" or does not exist
                    File.Delete(extFilePath);
                }
                catch (IOException ex)
                {
                    Globals.Log(ex.Message);
                    //GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(extFilePath), "Move File Failed"); });
                    failed++;
                }

                //ReportProgress(processed, total, skipped, failed);
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

        #endregion

        private void DeleteCorruptFiles()
        {
            /*
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
             */
        }

        private static string GetOriginal(string srcFilePath, ref StringBuilder sbErrors)
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

        private static DLCPackageData MaxFiveArrangements(DLCPackageData packageData, byte CheckByte, bool IgnoreLimit)
        {
            const int playableArrLimit = 5; // one based limit
            var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
            if (!IgnoreLimit && playableArrCount <= playableArrLimit)
                return packageData;

            var removalNdx = playableArrCount - playableArrLimit; // zero based index
            var packageDataKept = new DLCPackageData();
            packageDataKept.Arrangements = new List<RocksmithToolkitLib.DLCPackage.Arrangement>();

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

                switch (CheckByte)
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

                if (isKept || removalNdx == 0)
                {
                    Globals.Log(" - Kept: " + arr + " ...");
                    packageDataKept.Arrangements.Add(arr);

                    if (packageDataKept.Arrangements.Count == playableArrLimit)
                    {
                        Globals.Log(" - Kept first [" + playableArrLimit + "] arrangements matching the repair criteria ...");
                        break;
                    }
                }
                else
                {
                    Globals.Log(" - Removed: " + arr + " ...");
                    if (!IgnoreLimit)
                        removalNdx--;
                }
            }

            // replace original arrangements with kept arrangements
            packageData.Arrangements = packageDataKept.Arrangements;
            return packageData;
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

        public void RepairSongs()
        {
            //sbErrors = new StringBuilder();

            //// make sure 'dlc' folder is clean
            //CleanDlcFolder();
            //Globals.Log("Applying selected repairs to CDLC ...");
            //var srcFilePaths = new List<string>();

            //dlcFilePaths = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*_p.psarc", SearchOption.AllDirectories).Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            //// ignore the inlay(s) folder
            //dlcFilePaths = dlcFilePaths.Where(x => !x.ToLower().Contains("inlay")).ToList();

            //if (RepairOrg)
            //    srcFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + orgExt + "*").ToList();
            //else
            //    srcFilePaths = dlcFilePaths;

            //var total = srcFilePaths.Count;
            //var processed = 0;
            //var failed = 0;
            //var skipped = 0;
            //ReportProgress(processed, total, skipped, failed);

            //foreach (var srcFilePath in srcFilePaths)
            //{
            //    var isSkipped = false;
            //    dgvRepair.ClearSelection();
            //    processed++;

            //    var officialOrRepaired = OfficialOrRepaired(srcFilePath);
            //    if (!String.IsNullOrEmpty(officialOrRepaired))
            //    {
            //        if (officialOrRepaired.Contains("Official"))
            //        {
            //            GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped ODLC File"); });
            //            skipped++;
            //            isSkipped = true;
            //        }

            //        if (officialOrRepaired.Contains("Remastered") && SkipRepaired)
            //        {
            //            GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Skipped Remastered File"); });
            //            skipped++;
            //            isSkipped = true;
            //        }
            //    }

            //    // remaster the CDLC file
            //    if (!isSkipped)
            //    {
            //        var rSucess = RemasterSong(srcFilePath);
            //        if (rSucess)
            //        {
            //            var message = String.Format("Repair Sucessful ... {0}", PreserveStats ? "Preserved Song Stats" : "Reset Song Stats");
            //            if (RepairOrg)
            //                message += " ... Used (" + orgExt + ") File";
            //            if (addedDD)
            //                message += " ... Added Dynamic Difficulty";
            //            if (ddError)
            //                message += " ... Error Adding Dynamic Difficulty";

            //            GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath).Replace(orgExt, ""), message); });

            //            if (Constants.DebugMode)
            //            {
            //                // cleanup every nth record
            //                if (processed % 50 == 0)
            //                    GenExtensions.CleanLocalTemp();
            //            }
            //        }
            //        else
            //        {
            //            var lines = sbErrors.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //            if (lines.Last().ToLower().Contains("maximum"))
            //                GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Exceeds Playable Arrangements Limit ... Moved File ... Added To Error Log"); });
            //            else
            //                GenExtensions.InvokeIfRequired(this, delegate { dgvRepair.Rows.Add(Path.GetFileName(srcFilePath), "Corrupt CDLC ... Moved File ... Added To Error Log"); });

            //            failed++;
            //        }
            //    }

            //    GenExtensions.InvokeIfRequired(this, delegate
            //    {
            //        // dgvRepair.Rows[dgvRepair.Rows.Count - 1].Selected = true;
            //        dgvRepair.ClearSelection();
            //        dgvRepair.FirstDisplayedScrollingRowIndex = dgvRepair.Rows.Count - 1;
            //        dgvRepair.Refresh();
            //    });

            //    ReportProgress(processed, total, skipped, failed);
            //}

            //if (!String.IsNullOrEmpty(sbErrors.ToString())) //failed > 0)
            //{
            //    // error log can be turned into CSV file
            //    sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
            //    sbErrors.Insert(0, DateTime.Now.ToString("MM-dd-yy HH:mm") + Environment.NewLine);
            //    using (TextWriter tw = new StreamWriter(Constants.RemasteredErrorLogPath, true))
            //    {
            //        tw.WriteLine(sbErrors + Environment.NewLine);
            //        tw.Close();
            //    }

            //    Globals.Log("Saved error log to: " + Constants.RemasteredErrorLogPath + " ...");
            //}

            //if (processed > 0)
            //{
            //    Globals.Log("CDLC repair completed ..."); 
            //    Globals.RescanSongManager = true;

            //    if (Constants.DebugMode)
            //        GenExtensions.CleanLocalTemp();
            //}
            //else
            //    Globals.Log("No CDLC were repaired ...");
        }

        public static bool RepairOnly(string SrcFilePath, ref StringBuilder sbErrors, bool PreserveStats = true, bool IgnoreMultitoneEx = true, bool IgnoreLimit = true, byte CheckByte = 0x0, bool ReapplyDD = false, bool RepairMaxFive = false, bool RepairOrg = false)
        {
            return RemasterSong(SrcFilePath, ref sbErrors, CheckByte, PreserveStats, RepairOrg, IgnoreMultitoneEx, false, RepairMaxFive, IgnoreLimit);
        }

        public static bool RepairWithDD(string SrcFilePath, ref StringBuilder sbErrors, bool PreserveStats = true, bool IgnoreMultitoneEx = true, bool IgnoreLimit = true, SettingsDDC SettingsDD = null, bool ReapplyDD = false, byte CheckByte = 0x0, bool RepairMaxFive = false, bool RepairOrg = false)
        {
            return RemasterSong(SrcFilePath, ref sbErrors, CheckByte, PreserveStats, RepairOrg, IgnoreMultitoneEx, true, RepairMaxFive, IgnoreLimit, SettingsDD, ReapplyDD);
        }

        public static bool RemasterSong(string srcFilePath, ref StringBuilder sbErrors, byte ArrCheckByte = 0x0, bool PreserveStats = true, bool RepairOrg = false, bool IgnoreMultitoneEx = true, bool AddDD = true, bool RepairMaxFive = true, bool IgnoreLimit = false, SettingsDDC SettingsDD = null, bool ReapplyDD = false)
        {
            if (RepairOrg)
            {
                srcFilePath = GetOriginal(srcFilePath, ref sbErrors);
                if (String.IsNullOrEmpty(srcFilePath))
                    return false;
            }

            if (!CreateBackup(srcFilePath, ref sbErrors))
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
                ddError = false;

                SettingsDDC.Instance.LoadConfigXml();
                // phrase length should be at least 8 to fix chord density bug
                // using 12 bar blues beat for default phrase length
                var phraseLen = 12; // SettingsDDC.Instance.PhraseLen;
                // removeSus may be depricated in latest DDC but left here for comptiblity
                var removeSus = SettingsDDC.Instance.RemoveSus;
                var rampPath = SettingsDDC.Instance.RampPath;
                var cfgPath = SettingsDDC.Instance.CfgPath;

                if (SettingsDD != null)
                {
                    phraseLen = SettingsDD.PhraseLen;
                    removeSus = SettingsDD.RemoveSus;
                    rampPath = SettingsDD.RampPath;
                    cfgPath = SettingsDD.CfgPath;
                }

                DLCPackageData packageData;
                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath, IgnoreMultitoneEx);

                // TODO: selectively remove arrangements here before remastering
                if (RepairMaxFive)
                    packageData = MaxFiveArrangements(packageData, ArrCheckByte, IgnoreLimit);

                var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
                if (playableArrCount > 5)
                    throw new CustomException("Maximum playable arrangement limit exceeded");

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
                    songXml.ValidateData(packageData);

                    // write updated xml arrangement
                    using (var stream = File.Open(arr.SongXml.File, FileMode.Create))
                        songXml.Serialize(stream, true);

                    // add comments back to xml arrangement   
                    Song2014.WriteXmlComments(arr.SongXml.File, arr.XmlComments);

                    // only add DD to NDD arrangements              
                    var mf = new ManifestFunctions(GameVersion.RS2014);
                    var maxDD = mf.GetMaxDifficulty(songXml);

                    if (AddDD && (maxDD == 0 || ReapplyDD))
                    {
                        var consoleOutput = String.Empty;
                        var result = DynamicDifficulty.ApplyDD(arr.SongXml.File, phraseLen, removeSus, rampPath, cfgPath, out consoleOutput, true);
                        if (result == -1)
                            throw new CustomException("ddc.exe is missing");

                        if (String.IsNullOrEmpty(consoleOutput))
                        {
                            Globals.Log(" - Added DD to " + arr + " ...");
                            addedDD = true;
                        }
                        else
                        {
                            Globals.Log(" - " + arr + " DDC console output: " + consoleOutput + " ...");
                            sbErrors.AppendLine(String.Format("{0}, Could not apply DD to: {1}", srcFilePath, arr));
                            ddError = true;
                        }
                    }

                    // put arrangment comments in correct order
                    Song2014.WriteXmlComments(arr.SongXml.File);
                }

                // add comment to ToolkitInfo to identify CDLC
                if (!PreserveStats)
                    packageData.AddMsgToPackageComment(Constants.TKI_ARRID);

                if (RepairMaxFive)
                    packageData.AddMsgToPackageComment(Constants.TKI_MAX5);

                // add TKI_DDC comment
                if (AddDD && addedDD)
                    packageData.AddMsgToPackageComment(Constants.TKI_DDC);

                // add comment to ToolkitInfo to identify CDLC
                packageData.AddMsgToPackageComment(Constants.TKI_REMASTER);

                // add default package version if missing
                packageData.AddDefaultPackageVersion();

                // validate packageData (important)
                packageData.Name = packageData.Name.GetValidKey(); // DLC Key                 
                Globals.Log(" - Repackaging Remastered CDLC ...");

                // regenerates the SNG with the repair and repackages               
                using (var psarcNew = new PsarcPackager(true))
                    psarcNew.WritePackage(srcFilePath, packageData);

                if (!ddError)
                    Globals.Log(" - Repair was sucessful ...");
                else
                    Globals.Log(" - Repair was sucessful, but DD could not be applied ...");
            }
            catch (CustomException ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file ... ");

                if (ex.Message.Contains("Maximum"))
                {
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
                }

                return false;
            }
            catch (Exception ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file ... ");

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

        internal class CustomException : Exception
        {
            public CustomException() : base() { }
            public CustomException(string message) : base(message) { }
        }
    }
}
