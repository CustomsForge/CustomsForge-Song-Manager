using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DataGridViewTools;
using RocksmithToolkitLib;


//
// Reusable background worker class for parsing songs
//
namespace CustomsForgeManager.CustomsForgeManagerLib
{
    sealed class Worker : IDisposable
    {
        private AbortableBackgroundWorker bWorker;
        private Stopwatch counterStopwatch = new Stopwatch();
     //   private List<string> bwFileCollection = new List<string>();
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
                Extensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                    {
                        Globals.TsProgressBar_Main.Value = value;
                    });

                if (workOrder.Name != "Renamer")
                    Globals.TsLabel_MainMsg.Text = String.Format("Rocksmith Songs Count: {0}", bwSongCollection.Count);
            }
        }

        private void WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Extensions.InvokeIfRequired(workOrder, delegate { Globals.TsLabel_Cancel.Visible = false; });

            if (e.Cancelled || Globals.TsLabel_Cancel.Text == "Canceling")
            {
                // bWorker.Abort(); // don't use abort
                bWorker.Dispose();
                bWorker = null;
                Globals.Log("User canceled process ...");
                Globals.TsLabel_MainMsg.Text = "User Canceled";
                Globals.WorkerFinished = Globals.Tristate.Cancelled;
            }
            else
            {
                WorkerProgress(100);

                if (workOrder.Name == "SongManager" || workOrder.Name == "Duplicates" || workOrder.Name == "SetlistManager")
                    Globals.Log(String.Format("Finished parsing ... took {0}", counterStopwatch.Elapsed));

                else if (workOrder.Name == "Renamer")
                    Globals.Log(String.Format("Finished renaming ... took {0}", counterStopwatch.Elapsed));

                Globals.SongCollection = new BindingList<SongData>(bwSongCollection);                 
                Globals.WorkerFinished = Globals.Tristate.True;
            }
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
            Globals.IsScanning = true;
            List<string> fileList = Extensions.FilesList(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"),
                AppSettings.Instance.IncludeRS1DLCs);

            bwSongCollection = Globals.SongCollection.ToList();

            //// "Raw" is good descriptor :)
            Globals.Log(String.Format("Raw songs count: {0}", fileList.Count));

            if (fileList.Count == 0)
                return;

            counterStopwatch.Restart();
            int songCounter = 0;
            int oldCount = bwSongCollection.Count();
            bwSongCollection.RemoveAll(sd => !File.Exists(sd.Path));
            int removed = bwSongCollection.Count() - oldCount;
            if (removed > 0)
                Globals.Log(String.Format("Removed {0} obsolete songs.", removed));

            Globals.DebugLog("Parsing files");
            foreach (string file in fileList)
            {
                if (bWorker.CancellationPending || Globals.TsLabel_Cancel.Text == "Canceling")
                {
                    bWorker.CancelAsync();
                    e.Cancel = true;
                    break;
                }

                WorkerProgress(songCounter++ * 100 / fileList.Count);
                bool canScan = true;
                var sInfo = bwSongCollection.FirstOrDefault(s => s.Path.Equals(file, StringComparison.OrdinalIgnoreCase));
                if (sInfo != null)
                {
                    var fInfo = new FileInfo(file);
                    if ((int)fInfo.Length == sInfo.FileSize && fInfo.LastWriteTimeUtc == sInfo.FileDate)
                        canScan = false;
                }

                if (canScan)
                    ParsePSARC(file);
            }
            Globals.DebugLog("Parsing done.");

            counterStopwatch.Stop();
        }

        private void ParsePSARC(string filePath)
        {
            // 2x speed hack ... preload the TuningDefinition
            Globals.TuningXml = TuningDefinitionRepository.LoadTuningDefinitions(GameVersion.RS2014);
            try
            {
                using (var browser = new PsarcBrowser(filePath))
                {
                    var songInfo = browser.GetSongs();

                    foreach (var songData in songInfo.Distinct())
                    {
                        if (songData.Version == "N/A")
                        {
                            var fileNameVersion = songData.GetVersionFromFileName();
                            if (fileNameVersion != "")
                                songData.Version = fileNameVersion;
                        }

                        if (!songData.FileName.ToLower().Contains(Constants.RS1COMP))
                            Extensions.InvokeIfRequired(workOrder, delegate
                                {
                                    bwSongCollection.Add(songData);
                                });

                        if (songData.FileName.ToLower().Contains(Constants.RS1COMP) && AppSettings.Instance.IncludeRS1DLCs)
                            Extensions.InvokeIfRequired(workOrder, delegate
                               {
                                   bwSongCollection.Add(songData);
                               });
                    }
                }
            }
            catch (Exception ex)
            {
                // move to Quarantine folder
                var corDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "cdlc_quarantined");
                var corFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".corrupt");
                var corFilePath = Path.Combine(corDir, corFileName);

                if (ex.Message.StartsWith("Error reading JObject"))
                    Globals.Log(string.Format("<ERROR>: {0}  -  CDLC is corrupt!", filePath));
                else
                    Globals.Log(string.Format("<ERROR>: {0}  -  {1}", filePath, ex.Message));

                Globals.Log("File has been moved to: " + corDir);

                if (!Directory.Exists(corDir))
                    Directory.CreateDirectory(corDir);

                //if (!File.Exists(corFilePath))
                //    File.Delete(corFilePath);

                File.Move(filePath, corFilePath);
            }

            // free up memory
            Globals.TuningXml.Clear();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "bWorker")]
        public void Dispose() 
        { 
            
        }

        // Invoke Template if needed
        // Extensions.InvokeIfRequired(workOrder, delegate
        //    {
        //    });

    }
}
