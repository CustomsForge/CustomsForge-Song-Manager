using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using SevenZip;


namespace CustomsForgeSongManager.UControls
{
    public partial class Repairs : UserControl
    {
        List<string> failedRepairsLog;

        public Repairs()
        {
            InitializeComponent();
            lblProgress.Text = "Progress: 0/" + Globals.SongCollection.Count().ToString();
            failedRepairsLog = new List<string>();

            CreateFolders();
        }

        public void CreateFolders()
        {
            if (!Directory.Exists(Constants.RemasteredCLI_Folder))
                Directory.CreateDirectory(Constants.RemasteredCLI_Folder);

            if (!Directory.Exists(Constants.RemasteredCLI_CorruptCDLCFolder))
                Directory.CreateDirectory(Constants.RemasteredCLI_CorruptCDLCFolder);

            if (!Directory.Exists(Constants.RemasteredCLI_OrgCDLCFolder))
                Directory.CreateDirectory(Constants.RemasteredCLI_OrgCDLCFolder);
        }

        public void RepairAllCDLC()
        {
            List<string> log = new List<string>();
            string extraArgs = "", message = "", songPath = "", newPath = "";
            int progress = 0, failed = 0, songCount = Globals.SongCollection.Count();
            string logFilePath = Path.Combine(Constants.RemasteredCLI_CorruptCDLCFolder, "log.txt");

            dgvLog.Rows.Clear();

            if (!File.Exists("remastered.exe"))
            {
                MessageBox.Show("Required assemblies are missing, please reinstall CFSM and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Globals.Log("Started repairing the song collection...");

            extraArgs += @" ""-log """"" + Constants.RemasteredCLI_OrgCDLCFolder + @""""""""; //Gotta love excessive qutation marks...

            if (checkPre.Checked)
                extraArgs += " -pre ";

            if (checkPre.Checked)
                extraArgs += " -org ";

            foreach (SongData song in Globals.SongCollection)
            {
                if (!song.OfficialDLC && File.Exists(song.FilePath))
                {
                    songPath = song.FilePath;

                    if (checkOrg.Checked) //TODO: .org files are not in the dlc folder anymore, so this should be fixed/replaced/removed
                    {
                        //if (File.Exists(songPath + ".org"))
                        //    songPath = songPath + ".org";
                        //else
                        //    continue;
                    }

                    message = GenExtensions.RunExtExe("remastered.exe", true, true, true, @"""" + songPath + @"""" + extraArgs);

                    if (message.ToLower().Contains("repair was sucessful"))
                        dgvLog.Rows.Add(song.Artist + "-" + song.Title, "Repair sucessful!");
                    else if (message.ToLower().Contains("skipped"))
                        dgvLog.Rows.Add(song.Artist + "-" + song.Title, "Skipped! (already repaired)");
                    else
                    {
                        dgvLog.Rows.Add(song.Artist + "-" + song.Title, "Error! (saved to the log)");

                        if (message.Length > 0)
                        {
                            message = message.Replace("Initializing Remastered CLI ...", "").Replace("\r\n\r\n", "").Replace("- Making a backup copy ...\r\n - Sucessfully created backup ...\r\n - Extracting CDLC artifacts ...", "");
                            message = message.Remove(message.IndexOf("- See 'remastered_error.log' file"));
                        }

                        log.Add(message + "\n --------------------------------------------------------------------------------------------------------------");

                        failed++;
                        lblFailedRepairs.Text = failed.ToString();
                    }
                }

                progress++;

                lblProgress.Text = "Progress: " + progress.ToString() + "/" + songCount.ToString();
            }

            try
            {
                foreach (string corSong in Directory.EnumerateFiles(Constants.RemasteredCLI_OrgCDLCFolder).Where(sng => sng.Contains(".cor")).ToList())
                {
                    newPath = Path.Combine(Constants.RemasteredCLI_CorruptCDLCFolder, Path.GetFileName(corSong));

                    if (File.Exists(newPath))
                        File.Delete(newPath);

                    File.Move(corSong, newPath);
                }

                failedRepairsLog = log;

                if (failedRepairsLog.Count() > 0)
                {
                    string stringLog = string.Join("\n", failedRepairsLog);


                    if (File.Exists(logFilePath))
                        File.Delete(logFilePath);

                    File.WriteAllText(logFilePath, stringLog);

                }
            }
            catch (IOException ex)
            {
                Globals.Log(ex.ToString());
            }

            //NOTE: this should probably be enabled/uncommented :)
            //if (progress != failed)
            //    Globals.RescanSongManager = true;

            MessageBox.Show("Song collection repairing done! \n\nDon't forget to thank cozy1 on CustomsForge for making this possible! :))", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Globals.Log("Song collection repairing done...");
        }

        private void btnRepairCDLCs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to repair all your CDLC songs?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RepairAllCDLC();
            }
        }

        private void btnRemoveBackup_Click(object sender, EventArgs e)
        {
            string repairedPath, backupPath;
            try
            {
                if (MessageBox.Show("Are you sure you want to remove your backuped songs?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Globals.Log("Removing backuped songs...");

                    //--> Potentially can be removed (remove backups from dlc folder) <--
                    List<string> backups = Directory.GetFiles(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"), "*_p.psarc.org", SearchOption.AllDirectories).ToList();

                    for (int i = backups.Count - 1; i >= 0; i--)
                    {
                        backupPath = backups[i];
                        repairedPath = backups[i].Replace("org", "");

                        if (File.Exists(repairedPath))
                            File.Delete(backupPath);
                        else
                            File.Move(backupPath, repairedPath);
                    }

                    //--> Remove backups from RemasteredCLI_backup folder <--
                    DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_OrgCDLCFolder);
                    backupDir.CleanDir();

                    Globals.Log("Backuped songs removed...");
                }
            }
            catch (IOException ex)
            {
                Globals.Log(ex.Message);
            }
        }

        private void btnRestoreBackup_Click(object sender, EventArgs e)
        {
            string repairedPath, backupPath;

            try
            {
                if (MessageBox.Show("Are you sure you want to restore your backuped songs?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Globals.Log("Restoring backuped songs...");

                    List<string> backups = Directory.GetFiles(Constants.RemasteredCLI_OrgCDLCFolder, "*_p.psarc.org", SearchOption.AllDirectories).ToList();

                    for (int i = backups.Count - 1; i >= 0; i--)
                    {
                        backupPath = backups[i];
                        repairedPath = backups[i].Replace(".org", "");

                        repairedPath = Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", Path.GetFileName(repairedPath));

                        if (File.Exists(repairedPath))
                            File.Delete(repairedPath);

                        File.Move(backupPath, repairedPath);
                    }

                    Globals.Log("Backuped songs restored...");
                }
            }
            catch (IOException ex)
            {
                Globals.Log(ex.Message);
            }
        }

        private void btnShowLogOfFailedRepairs_Click(object sender, EventArgs e)
        {
            string stringLog = string.Join("\n", failedRepairsLog);

            if (string.IsNullOrEmpty(stringLog))
                stringLog = "No song repairs failed!";

            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.PopulateText(stringLog);
                noteViewer.ShowDialog();
            }
        }

        private void btnArchiveCorruptCDLC_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = ".zip files (*.zip)|*.zip";
                sfd.FilterIndex = 0;
                sfd.RestoreDirectory = true;
                sfd.FileName = "corruptCDLC";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Globals.Log("Archiving corrupt songs ...");

                    if (ZipUtilities.ZipDirectory(Constants.RemasteredCLI_CorruptCDLCFolder, sfd.FileName))
                        Globals.Log("Corrupt songs archived... ");
                    else
                        Globals.Log("Corrupt songs archiving failed...");
                }
            }
            catch (IOException ex)
            {
                Globals.Log(ex.Message);
            }
        }

        private void btnRemoveCorruptFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to remove corrupt songs?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Globals.Log("Removing corrupt songs ...");

                    DirectoryInfo backupDir = new DirectoryInfo(Constants.RemasteredCLI_CorruptCDLCFolder);
                    backupDir.CleanDir();

                    Globals.Log("Corrupt songs removed...");
                }
            }
            catch (IOException ex)
            {
                Globals.Log(ex.Message);
            }
        }
    }
}

