using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using GenTools;
using CustomsForgeSongManager.Forms;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Xml;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib;
using Arrangement = RocksmithToolkitLib.DLCPackage.Arrangement;

namespace CustomsForgeSongManager.LocalTools
{
    public class RepairTools
    {
        private static bool addedDD = false;
        private static bool ddError = false;
        private static RepairOptions options;
        private static ProgressStatus repairStatus;
        private static StringBuilder sbErrors;

        #region Class Methods

        public static void DoRepairs(object sender, List<SongData> songs, RepairOptions repairOptions)
        {
            options = repairOptions;
            // run new generic worker
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = "repairing songs";
                gWorker.BackgroundProcess(sender);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        public static void InitRepairTools()
        {
            GenericWorker.InitReportProgress();
            sbErrors = new StringBuilder();
            options = new RepairOptions();
            repairStatus = new ProgressStatus();
            FileTools.VerifyCfsmFolders();
        }

        public static DLCPackageData MaxFiveArrangements(DLCPackageData packageData)
        {
            const int playableArrLimit = 5; // one based limit
            var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
            if (!options.IgnoreStopLimit && playableArrCount <= playableArrLimit)
                return packageData;

            var removalNdx = playableArrCount - playableArrLimit; // zero based index
            var packageDataKept = new DLCPackageData();
            packageDataKept.Arrangements = new List<Arrangement>();

            foreach (var arr in packageData.Arrangements)
            {
                // skip vocal and showlight arrangements
                if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                    continue;

                var isKept = true;
                var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                var mf = new ManifestFunctions(GameVersion.RS2014);

                if (mf.GetMaxDifficulty(songXml) == 0 && options.RemoveNDD) isKept = false;
                if (arr.ArrangementType == ArrangementType.Bass && options.RemoveBass) isKept = false;
                if (arr.ArrangementType == ArrangementType.Guitar && options.RemoveGuitar) isKept = false;
                if (arr.BonusArr && options.RemoveBonus) isKept = false;
                if (arr.Metronome == Metronome.Generate && options.RemoveMetronome) isKept = false;

                if (isKept || removalNdx == 0)
                {
                    Globals.Log(" - Kept Arrangement: " + arr);
                    packageDataKept.Arrangements.Add(arr);

                    if (packageDataKept.Arrangements.Count == playableArrLimit)
                    {
                        Globals.Log(" - Kept first [" + playableArrLimit + "] arrangements matching the repair criteria");
                        break;
                    }
                }
                else
                {
                    Globals.Log(" - Removed Arrangement: " + arr);
                    if (!options.IgnoreStopLimit)
                        removalNdx--;
                }
            }

            // replace original arrangements with kept arrangements
            packageData.Arrangements = packageDataKept.Arrangements;
            return packageData;
        }

        public static bool RemasterSong(string srcFilePath)
        {
            if (options.UsingOrgFiles)
            {
                srcFilePath = FileTools.GetOriginal(srcFilePath);
                if (String.IsNullOrEmpty(srcFilePath))
                    return false;
            }
            else
            {
                if (!FileTools.CreateBackupOfType(srcFilePath, Constants.RemasteredOrgFolder, Constants.EXT_ORG))
                    return false;
            }

            try
            {
                // SNG's needs to be regenerated
                // ArrangmentIDs are stored in multiple place and all need to be updated
                // therefore we are going to unpack, apply repair, and repack
                Globals.Log(" - Extracting CDLC artifacts");
                // DDC generation variables
                addedDD = false;
                ddError = false;

                DLCPackageData packageData;
                using (var psarcOld = new PsarcPackager())
                    packageData = psarcOld.ReadPackage(srcFilePath, options.IgnoreMultitone);

                // TODO: selectively remove arrangements here before remastering
                if (options.RepairMaxFive)
                    packageData = MaxFiveArrangements(packageData);

                var playableArrCount = packageData.Arrangements.Count(arr => arr.ArrangementType == ArrangementType.Guitar || arr.ArrangementType == ArrangementType.Bass);
                if (playableArrCount > 5)
                    throw new CustomException("Maximum playable arrangement limit exceeded");

                // Update arrangement song info
                foreach (Arrangement arr in packageData.Arrangements)
                {
                    if (!options.PreserveStats)
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

                    // only add DD to NDD arrangements              
                    var mf = new ManifestFunctions(GameVersion.RS2014);
                    var maxDD = mf.GetMaxDifficulty(songXml);

                    if ((options.AddDD && maxDD == 0) || (options.AddDD && options.OverwriteDD))
                    {
                        // phrase length should be at least 8 to fix chord density bug
                        if (options.PhraseLength < 8) throw new Exception("DD Phrase Length less than eight.");

                        var consoleOutput = String.Empty;
                        var result = DynamicDifficulty.ApplyDD(arr.SongXml.File, options.PhraseLength, options.RemoveSustain, options.RampUpPath, options.CfgPath, out consoleOutput, true);
                        if (result == -1)
                            throw new CustomException("ddc.exe is missing");

                        if (String.IsNullOrEmpty(consoleOutput))
                        {
                            Globals.Log(" - Added DD to " + arr);
                            addedDD = true;
                        }
                        else
                        {
                            Globals.Log(" - " + arr + " DDC console output: " + consoleOutput);
                            sbErrors.AppendLine(String.Format("{0}, Could not apply DD to: {1}", srcFilePath, arr));
                            ddError = true;
                        }
                    }

                    // put arrangment comments in correct order
                    Song2014.WriteXmlComments(arr.SongXml.File);
                }

                if (!options.PreserveStats)
                    packageData = packageData.AddPackageComment(Constants.TKI_ARRID);

                if (options.RepairMaxFive)
                    packageData = packageData.AddPackageComment(Constants.TKI_MAX5);

                if (options.AddDD && addedDD)
                    packageData = packageData.AddPackageComment(Constants.TKI_DDC);

                // add comment to ToolkitInfo to identify Remastered CDLC
                packageData = packageData.AddPackageComment(Constants.TKI_REMASTER);

                // add default package version if missing
                if (String.IsNullOrEmpty(packageData.ToolkitInfo.PackageVersion))
                    packageData.ToolkitInfo.PackageVersion = "1";
                else
                    packageData.ToolkitInfo.PackageVersion = packageData.ToolkitInfo.PackageVersion.GetValidVersion();

                // validate packageData (important)
                packageData.Name = packageData.Name.GetValidKey(); // DLC Key                 
                Globals.Log(" - Repackaging Remastered CDLC");

                // regenerates the SNG with the repair and repackages               
                using (var psarcNew = new PsarcPackager(true))
                    psarcNew.WritePackage(srcFilePath, packageData);

                Globals.Log(String.Format(" - {0}", options.PreserveStats ? "Preserved Song Stats" : "Reset Song Stats"));
                if (options.UsingOrgFiles)
                    Globals.Log(" - Used [" + Constants.EXT_ORG + "] File");
                if (addedDD)
                    Globals.Log(" - Added Dynamic Difficulty");
                if (!ddError)
                    Globals.Log(" - Repair was sucessful");
                else
                    Globals.Log(" - Repair was sucessful, but DD could not be applied");
                if (options.ProcessDLFolder)
                {
                    GenExtensions.MoveFile(srcFilePath, Constants.Rs2DlcFolder);
                    Globals.Log(" - Moved new CDLC to 'dlc' folder");
                }

            }
            catch (CustomException ex)
            {
                Globals.Log(" - Repair failed ... " + ex.Message);
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file");

                if (ex.Message.Contains("Maximum"))
                {
                    //  copy (org) to maximum (max), delete backup (org), delete original
                    var properExt = Path.GetExtension(srcFilePath);
                    var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), Constants.EXT_ORG, properExt).Trim();
                    var maxFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredMaxFolder, Path.GetFileNameWithoutExtension(srcFilePath)), Constants.EXT_MAX, properExt).Trim();
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
                Globals.Log(" - See '" + Path.GetFileName(Constants.RemasteredErrorLogPath) + "' file");

                //  copy (org) to corrupt (cor), delete backup (org), delete original
                var properExt = Path.GetExtension(srcFilePath);
                var orgFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredOrgFolder, Path.GetFileNameWithoutExtension(srcFilePath)), Constants.EXT_ORG, properExt).Trim();
                var corFilePath = String.Format(@"{0}{1}{2}", Path.Combine(Constants.RemasteredCorFolder, Path.GetFileNameWithoutExtension(srcFilePath)), Constants.EXT_COR, properExt).Trim();
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

        public static StringBuilder RepairSongs(List<SongData> songs, RepairOptions repairOptions)
        {
            InitRepairTools();
            options = repairOptions;

            // make sure 'dlc' folder is clean
            FileTools.CleanDlcFolder();
            Globals.Log("Applying selected repairs to CDLC ...");
            var srcFilePaths = new List<string>();

            if (options.UsingOrgFiles)
            {
                Globals.Log("Using (.org) files for all repairs ...");
                srcFilePaths = Directory.EnumerateFiles(Constants.RemasteredOrgFolder, "*" + Constants.EXT_ORG + "*").ToList();
            }
            else if (options.ProcessDLFolder)
            {
                // TODO: maybe make sure new CDLC have been unzipped/unrar'd first
                var dlDirPath = FileTools.GetDownloadsPath();
                if (!String.IsNullOrEmpty(dlDirPath))
                {
                    Globals.Log("Repairing CDLC files from: " + dlDirPath + " ...");
                    srcFilePaths = Directory.EnumerateFiles(dlDirPath, "*.psarc").ToList();
                }
            }
            else
                srcFilePaths = FileTools.SongFilePaths(songs);

            var total = srcFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                var isSkipped = false;
                Globals.Log("Processing: " + Path.GetFileName(srcFilePath));
                processed++;

                var officialOrRepaired = FileTools.OfficialOrRepaired(srcFilePath);
                if (!String.IsNullOrEmpty(officialOrRepaired))
                {
                    if (officialOrRepaired.Contains("Official"))
                    {
                        Globals.Log(Path.GetFileName(srcFilePath) + " - Skipped ODLC File");
                        skipped++;
                        isSkipped = true;
                    }

                    if (officialOrRepaired.Contains("Remastered") && options.SkipRemastered)
                    {
                        Globals.Log(Path.GetFileName(srcFilePath) + " - Skipped Remastered File");
                        skipped++;
                        isSkipped = true;
                    }
                }

                // remaster the CDLC file
                if (!isSkipped)
                {
                    var rSucess = RemasterSong(srcFilePath);
                    if (!rSucess)
                    {
                        if (!String.IsNullOrEmpty(sbErrors.ToString()))
                        {
                            var lines = sbErrors.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            if (lines.Last().ToLower().Contains("maximum"))
                                Globals.Log(Path.GetFileName(srcFilePath) + " - Exceeds Playable Arrangements Limit ... Moved file to 'maxfive' subfolder");
                        }
                        else
                            Globals.Log(Path.GetFileName(srcFilePath) + " - Corrupt CDLC ... Moved file to 'corrupt' subfolder");

                        failed++;
                    }
                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);

                if (!Constants.DebugMode)
                {
                    // cleanup after every nth record
                    if (processed % 50 == 0)
                        GenExtensions.CleanLocalTemp();
                }
            }

            if (!String.IsNullOrEmpty(sbErrors.ToString())) //failed > 0)
            {
                // error log can be turned into CSV file
                sbErrors.Insert(0, "File Path, Error Message" + Environment.NewLine);
                sbErrors.Insert(0, DateTime.Now.ToString("MM-dd-yy HH:mm") + Environment.NewLine);
                using (TextWriter tw = new StreamWriter(Constants.RemasteredErrorLogPath, true))
                {
                    tw.WriteLine(sbErrors + Environment.NewLine);
                    tw.Close();
                }

                Globals.Log("Saved error log to: " + Constants.RemasteredErrorLogPath + " ...");
            }

            if (processed > 0)
            {
                Globals.Log("CDLC repair completed ...");
                Globals.RescanSongManager = true;

                if (!Constants.DebugMode)
                    GenExtensions.CleanLocalTemp();
            }
            else
                Globals.Log("No CDLC were repaired ...");

            return sbErrors;
        }

        public static void ShowNoteViewer(string resourceHelpPath = "CustomsForgeSongManager.Resources.HelpGeneral.txt", string windowText = "Default") 
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resourceHelpPath);
            using (StreamReader reader = new StreamReader(stream))
            {
                var helpGeneral = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, windowText);
                    noteViewer.PopulateText(helpGeneral);
                    noteViewer.ShowDialog();
                }
            }
        }

        public static void ViewErrorLog()
        {
            string stringLog;

            if (!File.Exists(Constants.RemasteredErrorLogPath))
                stringLog = Path.GetFileName(Constants.RemasteredErrorLogPath) + " is empty ...";
            else
            {
                stringLog = Constants.RemasteredErrorLogPath + Environment.NewLine;
                stringLog = stringLog + File.ReadAllText(Constants.RemasteredErrorLogPath);
                stringLog = stringLog + Environment.NewLine + AppSettings.Instance.LogFilePath + Environment.NewLine;
                stringLog = stringLog + File.ReadAllText(AppSettings.Instance.LogFilePath);
            }

            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Remastered Error Log");
                noteViewer.Width = 700;
                noteViewer.PopulateText(stringLog);
                noteViewer.ShowDialog();
            }
        }

        #endregion
    }

    internal class CustomException : Exception
    {
        public CustomException() : base() { }
        public CustomException(string message) : base(message) { }
    }

    [Serializable]
    public class RepairOptions
    {
        private string _cfgPath;
        private int _phraseLen;
        private string _rampUpPath;
        public bool AddDD { get; set; }

        public string CfgPath
        {
            get { return _cfgPath ?? (_cfgPath = String.Empty); }
            set { _cfgPath = value; }
        }

        // this object order reflects in the apperance of the settings xml file
        public bool IgnoreMultitone { get; set; }
        public bool IgnoreStopLimit { get; set; }
        public bool OverwriteDD { get; set; }

        public int PhraseLength
        {
            get { return _phraseLen < 8 ? (_phraseLen = 8) : _phraseLen; }
            set { _phraseLen = value; }
        }

        public bool PreserveStats { get; set; }
        public bool ProcessDLFolder { get; set; }

        public string RampUpPath
        {
            get { return _rampUpPath ?? (_rampUpPath = String.Empty); }
            set { _rampUpPath = value; }
        }

        public bool RemoveBass { get; set; }
        public bool RemoveBonus { get; set; }
        public bool RemoveGuitar { get; set; }
        public bool RemoveMetronome { get; set; }
        public bool RemoveNDD { get; set; }
        public bool RemoveSections { get; set; }

        public bool RemoveSustain { get; set; }
        public bool RepairMastery { get; set; }
        public bool RepairMaxFive { get; set; }
        public bool SkipRemastered { get; set; }
        public bool UsingOrgFiles { get; set; }
    }



}
