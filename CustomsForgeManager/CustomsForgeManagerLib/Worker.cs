using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using CustomsForgeManager.UControls;

//
// Reusable background worker class for parsing songs
//
namespace CustomsForgeManager.CustomsForgeManagerLib
{
    class Worker : IDisposable
    {
        private AbortableBackgroundWorker bWorker;
        private Stopwatch counterStopwatch = new Stopwatch();
        private List<string> bwFileCollection = new List<string>();
        public BindingList<SongData> bwSongCollection = new BindingList<SongData>();
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

            if (e.Cancelled || Globals.TsLabel_Cancel.Text == "Cancelling")
            {
                // bWorker.Abort(); // don't use abort
                bWorker.Dispose();
                bWorker = null;
                Globals.Log("User cancelled process ...");
                Globals.TsLabel_MainMsg.Text = "User Cancelled";
                Globals.WorkerFinished = Globals.Tristate.Cancelled;
            }
            else
            {
                WorkerProgress(100);

                if (workOrder.Name == "SongManager" || workOrder.Name == "Duplicates" || workOrder.Name == "SetlistManager")
                    Globals.Log(String.Format("Finished parsing ... took {0}", counterStopwatch.Elapsed));

                else if (workOrder.Name == "Renamer")
                    Globals.Log(String.Format("Finished renaming ... took {0}", counterStopwatch.Elapsed));

                Globals.SongCollection = bwSongCollection;
                Globals.FileCollection = bwFileCollection;
                Globals.WorkerFinished = Globals.Tristate.True;
            }

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
            bwFileCollection.Clear();
            Extensions.InvokeIfRequired(workOrder, delegate
              {
                  // bwSongCollection.Clear();
                  bwSongCollection = new BindingList<SongData>();
              });

            List<string> fileList = Extensions.FilesList(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), Globals.MySettings.IncludeRS1DLCs);

            // "Raw" is good descriptor :)
            Globals.Log(String.Format("Raw songs count: {0}", fileList.Count));

            if (fileList.Count == 0)
                return;

            Globals.Log("Parsing songs ...");
            counterStopwatch.Restart();
            int songCounter = 0;

            foreach (string file in fileList)
            {
                if (bWorker.CancellationPending || Globals.TsLabel_Cancel.Text == "Cancelling")
                {
                    bWorker.CancelAsync();
                    e.Cancel = true;
                    break;
                }

                WorkerProgress(songCounter++ * 100 / fileList.Count);

                var enabled = file.Contains(".disabled.") ? "No" : "Yes";
                ParsePSARC(enabled, file);
                bwFileCollection.Add(file);
            }

            counterStopwatch.Stop();
        }

        private void ParsePSARC(string enabled, string file)
        {
            try
            {
                using (var browser = new PsarcBrowser(file))
                {
                    var songInfo = browser.GetSongs();

                    foreach (var songData in songInfo.Distinct())
                    {
                        songData.Enabled = enabled;
                        
                        // **************
                        // CAUTION - ANY DateTime CHANGES MUST BE TESTED WITH MULTIPLE CULTURE VARIANTS
                        // convert date string to usable DateTime format
                        DateTime updateDateTime = new DateTime();
                        if (DateTime.TryParse(songData.Updated, out updateDateTime))
                            songData.Updated = updateDateTime.ToString(CultureInfo.GetCultureInfo("en-US").DateTimeFormat);

                        // prevent mixed culture variants appearing in same table
                        songData.Updated = DateTime.Parse(songData.Updated, CultureInfo.GetCultureInfo("en-US")).ToString();
                        // **************
                       
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

                        if (songData.FileName.ToLower().Contains(Constants.RS1COMP) && Globals.MySettings.IncludeRS1DLCs)
                            Extensions.InvokeIfRequired(workOrder, delegate
                               {
                                   bwSongCollection.Add(songData);
                               });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Error reading JObject"))
                    Globals.Log("<ERROR>: " + file + "  :  " + "CDLC is corrupt!");
                else
                    Globals.Log("<ERROR>: " + file + "  :  " + ex.Message);
            }
        }

        public void Dispose() { }

        // Invoke Template if needed
        // Extensions.InvokeIfRequired(workOrder, delegate
        //    {
        //    });

    }
}
