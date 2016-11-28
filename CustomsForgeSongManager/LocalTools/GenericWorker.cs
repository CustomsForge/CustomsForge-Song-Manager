using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Properties;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XmlRepository;

//
// Reusable generic background worker class
//

namespace CustomsForgeSongManager.LocalTools
{
    internal sealed class GenericWorker : IDisposable
    {
        private AbortableBackgroundWorker bWorker;
        private Stopwatch counterStopwatch = new Stopwatch();
        private Control workOrder;

        public string WorkDescription = String.Empty;

        public void BackgroundProcess(object sender, AbortableBackgroundWorker backgroundWorker = null)
        {
            // keep track of UC that started worker
            workOrder = ((Control)sender);
            Globals.ResetToolStripGlobals();
            Globals.TsLabel_Cancel.Visible = true;
            Globals.TsLabel_Cancel.Enabled = true;
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = true;
            Globals.WorkerFinished = Globals.Tristate.False;

            if (backgroundWorker == null)
                bWorker = new AbortableBackgroundWorker();
            else
                bWorker = backgroundWorker;

            bWorker.SetDefaults();
            counterStopwatch.Restart();

            if (WorkDescription.ToLower().Contains("repairing a single"))
                bWorker.DoWork += WorkerRepairSong_Single;
            else if (WorkDescription.ToLower().Contains("repairing the selection"))
                bWorker.DoWork += WorkerRepairSong_Selection;
            else if (WorkDescription.ToLower().Contains("repairing"))
                bWorker.DoWork += WorkerRepairSong;
            else if (WorkDescription.ToLower().Contains(".org"))
                bWorker.DoWork += WorkerRestoreOrgBackups;
            else if (WorkDescription.ToLower().Contains(".max"))
                bWorker.DoWork += WorkerRestoreMaxBackups;
            else if (WorkDescription.ToLower().Contains(".cor"))
                bWorker.DoWork += WorkerRestoreCorBackups;
            else if (WorkDescription.ToLower().Contains("archiving"))
                bWorker.DoWork += WorkerArchiveCorruptSongs;
            else if (WorkDescription.ToLower().Contains("dd to a single"))
                bWorker.DoWork += WorkerApplyDD_Single;
            else if (WorkDescription.ToLower().Contains("dd to the selection"))
                bWorker.DoWork += WorkerApplyDD_Selection;
            else if (WorkDescription.ToLower().Contains("pitch shift to a single"))
                bWorker.DoWork += WorkerPitchShift_Single;
            else if (WorkDescription.ToLower().Contains("pitch shift to the selection"))
                bWorker.DoWork += WorkerPitchShift_Selection;
            else if (WorkDescription.ToLower().Contains("moving songs from downloads"))
                bWorker.DoWork += WorkerMoveSongsFromDownloads;
            else
                throw new Exception("I'm not that kind of worker ...");

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
            }
        }

        private void WorkerComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            GenExtensions.InvokeIfRequired(workOrder, delegate { Globals.TsLabel_Cancel.Visible = false; });

            if (e.Cancelled || Globals.TsLabel_Cancel.Text == "Canceling" || Globals.CancelBackgroundScan)
            {
                // bWorker.Abort(); // don't use abort
                bWorker.Dispose();
                bWorker = null;
                Globals.Log(Resources.UserCanceledProcess);
                Globals.TsLabel_MainMsg.Text = Resources.UserCanceled;
                Globals.TsLabel_StatusMsg.Text = "";
                Globals.WorkerFinished = Globals.Tristate.Cancelled;
            }
            else
            {
                WorkerProgress(100);
                counterStopwatch.Stop();
                Globals.Log(String.Format("Finished " + WorkDescription.ToLower() + " ... took {0}", counterStopwatch.Elapsed));
                Globals.WorkerFinished = Globals.Tristate.True;
            }

            Globals.RescanSongManager = true;
        }

        private void WorkerRepairSong(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.BulkRepairs.RepairSongs();
        }

        private void WorkerRestoreOrgBackups(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.BulkRepairs.RestoreBackups(".org", Constants.RemasteredOrgFolder);
        }

        private void WorkerRestoreCorBackups(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.BulkRepairs.RestoreBackups(".cor", Constants.RemasteredCorFolder);
        }
        
        private void WorkerRestoreMaxBackups(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.BulkRepairs.RestoreBackups(".max", Constants.RemasteredMaxFolder);
        }

        private void WorkerArchiveCorruptSongs(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.BulkRepairs.ArchiveCorruptCDLC();
        }

        private void WorkerApplyDD_Single(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.ApplyDD_Single();
        }

        private void WorkerApplyDD_Selection(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.ApplyDD_Selection();
        }

        private void WorkerPitchShift_Single(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.PitchShift_Single();
        }

        private void WorkerPitchShift_Selection(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.PitchShift_Selection();
        }

        private void WorkerRepairSong_Single(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.RepairSong_Single();
        }

        private void WorkerRepairSong_Selection(object sender, DoWorkEventArgs e)
        {
             if (!bWorker.CancellationPending)
                 Globals.SongManager.RepairSong_Selection();
        }

        private void WorkerMoveSongsFromDownloads(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
                Globals.SongManager.MoveSongsFromDownloads();
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "bWorker")]
        public void Dispose()
        {
        }


    }
}