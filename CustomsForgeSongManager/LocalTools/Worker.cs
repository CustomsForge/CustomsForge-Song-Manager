using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using GenTools;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XmlRepository;
using CustomsForgeSongManager.Properties;

//
// Reusable background worker class for parsing songs
//

namespace CustomsForgeSongManager.LocalTools
{
    internal sealed class Worker : IDisposable
    {
        private AbortableBackgroundWorker bWorker;
        private Stopwatch counterStopwatch = new Stopwatch();
        public List<SongData> bwSongCollection = new List<SongData>();
        private Control workOrder;

        public void BackgroundScan(object sender, AbortableBackgroundWorker backgroundWorker = null)
        {
            // keep track of UC that started worker
            workOrder = ((Control)sender);
            Globals.ResetToolStripGlobals();
            Globals.TsLabel_Cancel.Visible = true;
            Globals.TsLabel_Cancel.Enabled = true;
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.WorkerFinished = Globals.Tristate.False;

            if (backgroundWorker == null)
                bWorker = new AbortableBackgroundWorker();
            else
                bWorker = backgroundWorker;

            bWorker.SetDefaults();

            if (workOrder.Name == "Renamer")
                bWorker.DoWork += WorkerRenameSongs;
            else if (workOrder.Name == "Analyzer")
                bWorker.DoWork += WorkerAnalyzerParseSong;
            else
                bWorker.DoWork += WorkerParseSongs;

            bWorker.RunWorkerCompleted += WorkerComplete;
            // don't run bWorker more than once
            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void WorkerProgress(int value)
        {
            if (Globals.TsProgressBar_Main != null && value <= 100)
            {
                // perma fix for periodic cross threading issue with TsProgressBar on initial startups
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate { Globals.TsProgressBar_Main.Value = value; });

                if (workOrder.Name != "Renamer")
                {
                    GenExtensions.InvokeIfRequired(Globals.TsLabel_MainMsg.GetCurrentParent(), delegate { Globals.TsLabel_MainMsg.Text = String.Format("Rocksmith Song Count: {0}", bwSongCollection.Count); });
                }
            }
        }

        private void WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            GenExtensions.InvokeIfRequired(workOrder, delegate { Globals.TsLabel_Cancel.Visible = false; });

            if (e.Cancelled || Globals.TsLabel_Cancel.Text == "Canceling" || Globals.CancelBackgroundScan)
            {
                // bWorker.Abort(); // don't use abort
                Globals.Log(Resources.UserCancelledProcess);
                Globals.TsLabel_MainMsg.Text = Resources.UserCancelled;
                Globals.WorkerFinished = Globals.Tristate.Cancelled;
            }
            else
            {
                WorkerProgress(100);

                if (workOrder.Name == "SongManager" || workOrder.Name == "Duplicates" || workOrder.Name == "SetlistManager")
                    Globals.Log(String.Format("Finished parsing took: {0}", counterStopwatch.Elapsed));
                else if (workOrder.Name == "Renamer")
                    Globals.Log(String.Format("Finished renaming took: {0}", counterStopwatch.Elapsed));

                Globals.WorkerFinished = Globals.Tristate.True;
            }

            bWorker.Dispose();
            bWorker = null;
            Globals.IsScanning = false;
        }

        private void WorkerRenameSongs(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
            {
                Globals.Renamer.RenameSongs();
            }
        }

        private void WorkerParseSongs(object sender, DoWorkEventArgs e)
        {
            ParseSongs(sender, e);
        }

        private void WorkerAnalyzerParseSong(object sender, DoWorkEventArgs e)
        {
            ParseSongs(sender, e, true);
        }

        private void ParseSongs(object sender, DoWorkEventArgs e, bool getAnalyzerData = false)
        {
            Globals.IsScanning = true;
            List<string> filesList;

            // is this a full rescan
            if (Globals.MasterCollection.Count == 0)
                filesList = FilesList(Constants.Rs2DlcFolder, AppSettings.Instance.IncludeRS1CompSongs, AppSettings.Instance.IncludeRS2BaseSongs, AppSettings.Instance.IncludeCustomPacks);
            else
                filesList = FilesList(Constants.Rs2DlcFolder, false, false, false);

            filesList = filesList.Where(fi => !fi.ToLower().Contains("inlay")).ToList();
            
            // initialization
            bwSongCollection = Globals.MasterCollection.ToList();

            //// "Raw" is good descriptor :)
            Globals.Log(String.Format("Raw songs count: {0}", filesList.Count));

            if (filesList.Count == 0)
                return;

            int songCounter = 0;
            int oldCount = bwSongCollection.Count();
            bwSongCollection.RemoveAll(sd => !File.Exists(sd.FilePath));

            int removed = bwSongCollection.Count() - oldCount;
            if (removed > 0)
                Globals.Log(String.Format(Resources.RemovedX0ObsoleteSongs, removed));

            // skip dup check of songs.psarc or compatibility and song packs
            List<SongData> checkThese = bwSongCollection
                .Where(x => !x.FilePath.ToLower().Contains(Constants.RS1COMP) &&
                !x.FilePath.ToLower().Contains(Constants.SONGPACK) &&
                !x.FilePath.ToLower().Contains(Constants.ABVSONGPACK) &&
                !x.FilePath.ToLower().Equals(Constants.SongsPsarcPath.ToLower())) // must have ToLower()
                .ToList() as List<SongData>;

            // this is improbable ... two songs have same FilePath
            var dupPaths = checkThese.GroupBy(x => x.FilePath).Where(group => group.Count() > 1);
            if (dupPaths.Count() > 0)
            {
                foreach (var x in dupPaths)
                {
                    var toDelete = x.Where(z => z != x.First());
                    bwSongCollection.RemoveAll(sd => toDelete.Contains(sd));
                }
            }

            Globals.DebugLog("Parsing files ...");
            foreach (string file in filesList)
            {
                if (bWorker.CancellationPending || Globals.TsLabel_Cancel.Text == "Canceling" || Globals.CancelBackgroundScan)
                {
                    bWorker.CancelAsync();
                    e.Cancel = true;
                    Globals.DebugLog(Resources.UserCancelledProcess);
                    return;
                }

                WorkerProgress(songCounter++ * 100 / filesList.Count);

                // check current collection data against the actual file data and rescan if necessary
                bool canScan = true;
                var sInfo = bwSongCollection.FirstOrDefault(s => s.FilePath.Equals(file, StringComparison.OrdinalIgnoreCase));
                if (sInfo != null)
                {
                    var fInfo = new FileInfo(file);
                    if ((int)fInfo.Length == sInfo.FileSize && fInfo.LastWriteTimeUtc == sInfo.FileDate)
                        canScan = false;
                    else
                        bwSongCollection.Remove(sInfo);
                }

                if (canScan)
                    ParsePSARC(file, getAnalyzerData);
            }

            // cleanup and sort
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var prop = typeof(SongData).GetProperty(AppSettings.Instance.SortColumn);
                if (prop != null)
                {
                    Type interfaceType = prop.PropertyType.GetInterface("IComparable");
                    if (interfaceType != null)
                    {
                        try
                        {
                            bwSongCollection.Sort((x1, x2) =>
                                {
                                    var c1 = (prop.GetValue(x1, new object[] { }) as IComparable);
                                    var c2 = (prop.GetValue(x2, new object[] { }) as IComparable);
                                    if (c1 == null || c2 == null)
                                        return -1;

                                    if (AppSettings.Instance.SortAscending)
                                        return c1.CompareTo(c2);
                                    else
                                        return c2.CompareTo(c1);
                                });
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            Globals.MasterCollection = new BindingList<SongData>(bwSongCollection);
            // -- CRITCAL -- this populates Arrangment DLCKey info in Arrangements2D
            Globals.MasterCollection.ToList().ForEach(a => a.Arrangements2D.ToList().ForEach(arr => arr.Parent = a));
        }

        private void ParsePSARC(string filePath, bool getAnalyzerData = false)
        {
            // 2x speed hack ... preload the TuningDefinition and fix for tuning 'Other' issue           
            if (Globals.TuningXml == null || Globals.TuningXml.Count == 0)
                Globals.TuningXml = TuningDefinitionRepository.Instance.LoadTuningDefinitions(GameVersion.RS2014);

            try
            {
                using (var browser = new PsarcBrowser(filePath))
                {
                    var songInfo = browser.GetSongData(getAnalyzerData);

                    foreach (var songData in songInfo.Distinct())
                    {
                        //foreach (var arr in songData.Arrangements2D)
                        //     arr.Tuning = Extensions.TuningToName(arr.Tuning);

                        if (songData.Version == "N/A")
                        {
                            var fileNameVersion = songData.GetVersionFromFileName();
                            if (fileNameVersion != "")
                                songData.Version = fileNameVersion;
                        }

                        GenExtensions.InvokeIfRequired(workOrder, delegate { bwSongCollection.Add(songData); });

                        //if (songData.FileName.ToLower().Contains(Constants.RS1COMP) && AppSettings.Instance.IncludeRS1DLCs)
                        //    GenExtensions.InvokeIfRequired(workOrder, delegate { bwSongCollection.Add(songData); });

                        //if (songData.FileName.ToLower().Contains(Constants.SongsPsarcPath.ToLower()) && AppSettings.Instance.IncludeRS2014BaseSongs)
                        //    GenExtensions.InvokeIfRequired(workOrder, delegate { bwSongCollection.Add(songData); });

                        //if (songData.FileName.ToLower().Contains(Constants.SONGPACK) || songData.FileName..ToLower().Contains(Constants.ABVSONGPACK) && AppSettings.Instance.IncludeCustomPacks)
                        //    GenExtensions.InvokeIfRequired(workOrder, delegate { bwSongCollection.Add(songData); });
                    }
                }
            }
            catch (Exception ex)
            {
                // move to Quarantine folder
                if (ex.Message.StartsWith("Error reading JObject"))
                    Globals.Log(String.Format("<ERROR>: {0}  -  CDLC is corrupt!", filePath));
                else
                    Globals.Log(String.Format("<ERROR>: {0}  -  {1}", filePath, ex.Message));

                if (AppSettings.Instance.MoveToQuarantine)
                {
                    var corFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".cor");
                    var corFilePath = Path.Combine(Constants.QuarantineFolder, corFileName);
                    Globals.Log("File has been moved to: " + Constants.QuarantineFolder);

                    if (!Directory.Exists(Constants.QuarantineFolder))
                        Directory.CreateDirectory(Constants.QuarantineFolder);

                    //if (File.Exists(corFilePath))
                    //    File.Delete(corFilePath);

                    File.Move(filePath, corFilePath);
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "bWorker")]
        public void Dispose()
        {
        }

        // Invoke Template if needed
        // GenExtensions.InvokeIfRequired(workOrder, delegate
        //    {
        //    });

        public static List<string> FilesList(string filePath, bool includeRS1Pack = false, bool includeRS2014BaseSongs = false, bool includeCustomPacks = false)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new Exception("<ERROR>: No path provided for file scanning");

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var files = Directory.EnumerateFiles(filePath, "*" + Constants.PsarcExtension, SearchOption.AllDirectories).ToList();
            files.AddRange(Directory.EnumerateFiles(filePath, "*" + Constants.DisabledPsarcExtension, SearchOption.AllDirectories).ToList());

            if (!includeRS1Pack)
                files = files.Where(file => !file.ToLower().Contains(Constants.RS1COMP)).ToList();

            if (!includeCustomPacks)
                files = files.Where(file => !file.ToLower().Contains(Constants.SONGPACK) && !file.ToLower().Contains(Constants.ABVSONGPACK)).ToList();

            if (includeRS2014BaseSongs)
                files.Add(Constants.SongsPsarcPath);

            return files;
        }

    }
}