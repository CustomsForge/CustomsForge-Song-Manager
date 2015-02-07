using System;
using System.Deployment.Application;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Logging;
using CustomsForgeManager_Winforms.Utilities;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {

        private readonly Log myLog;
        private Settings mySettings;

        private BindingList<SongData> SongCollection = new BindingList<SongData>();
        private List<SongDupeData> DupeCollection = new List<SongDupeData>();
        private List<SongData> SongSearchCollection = new List<SongData>();

        private frmMain()
        {
            throw new Exception("Improper constructor used");
        }

        public frmMain(Log myLog, Settings mySettings)
        {
            // TODO: Complete member initialization
            this.myLog = myLog;
            this.mySettings = mySettings;

            InitializeComponent();
            Init();
        }


        private void Init()
        {
            #region Create directory structure if not exists

            string configFolderPath = Constants.DefaultWorkingDirectory;
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
                Log(string.Format("Working directory created at {0}", configFolderPath));
            }

            #endregion

            #region Logging setup

            myLog.AddTargetFile(mySettings.LogFilePath);
            myLog.AddTargetControls(tbLog);

            #endregion

            #region Load Settings file and deserialize to Settings class
            LoadSettingsFromFile();
            #endregion

            if (ApplicationDeployment.IsNetworkDeployed)
                Log(string.Format("Application loaded, using version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion), 100);

            BackgroundScan();

          
        }

        private void BackgroundScan()
        {
            bWorker.DoWork -= PopulateListHandler;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted -= PopulateCompletedHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;
            bWorker.RunWorkerAsync();
        }

        void PopulateCompletedHandler(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);
            if (DupeCollection.Count > 0)
            {
                foreach (SongDupeData song in DupeCollection)
                {
                    listDupeSongs.InvokeIfRequired(delegate
                    {
                        listDupeSongs.Items.Add(new ListViewItem(new string[] { " ", song.Artist, song.Song, song.Album, song.SongOnePath, song.SongTwoPath }));
                    });
                }
            }

            dgvSongs.InvokeIfRequired(delegate
            {
                BindingSource bs = new BindingSource();
                var songsToShow = from song in SongCollection select new
                                  {
                                      Song = song.Song,
                                      Artist = song.Artist,
                                      Album = song.Album,
                                      Updated = song.Updated,
                                      Tuning = song.Tuning,
                                      Arrangements = song.Arrangements,
                                      Author = song.Author,
                                      NewAvailable = song.NewAvailable
                                  };
                bs.DataSource = songsToShow;
                dgvSongs.DataSource = bs;
                dgvSongs.Columns[0].Visible = true;
                dgvSongs.Columns[1].Visible = true;
                dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            });
            Log("Finished scanning songs...", 100);
            ToggleUIControls();
            
        }

        

        void PopulateListHandler(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ToggleUIControls();
            PopulateSongList();
            PopulateDupeList();
        }

        private void ToggleUIControls()
        {
            btnRescan.Enabled = !btnRescan.Enabled;

            //Uncomment after implementing:
            //btnEditDLC.Enabled = !btnEditDLC.Enabled;
            //btnBackupDLC.Enabled = !btnBackupDLC.Enabled;
            //btnSaveDLC.Enabled = !btnSaveDLC.Enabled;
            //btnDLCPage.Enabled = !btnDLCPage.Enabled;

            btnSearch.Enabled = !btnSearch.Enabled;

            btnSettingsSave.Enabled = !btnSettingsSave.Enabled;
            btnSettingsLoad.Enabled = !btnSettingsLoad.Enabled;

            tbSearch.Enabled = !tbSearch.Enabled;
            
            tbSettingsRSDir.Enabled = !tbSettingsRSDir.Enabled;
            
        }

        #region GUIEventHandlers
        private void btnDupeRescan_Click(object sender, EventArgs e)
        {
            listDupeSongs.Items.Clear();
            DupeCollection.Clear();
            SongCollection.Clear();
            tbDuplicates.InvokeIfRequired(delegate
            {
                tbDuplicates.Text = "Duplicates(0)";
            });
            BackgroundScan();
        }
        private void btnBackupDLC_Click(object sender, EventArgs e)
        {
            string backupPath = mySettings.RSInstalledDir + @"\backup";
            string fileName = "";
            try
            {
                 int i = 0;
                 if(!Directory.Exists(backupPath))
                 {
                     Directory.CreateDirectory(backupPath);
                 }
                 //foreach (ListViewItem listItem in dgvSongs.Items)
                 //{
                 //    if (listItem.Checked)
                 //    {
                 //        fileName =  Path.GetFileName(SongCollection[i].Path);
                 //        File.Copy(SongCollection[i].Path, Path.Combine(backupPath, fileName));
                 //    }
                 //    i++;
                 //}
            }
            catch(IOException)
            {
                Log("File" + fileName + "already exists!");
            }
        }
        private void btnDeleteSongOne_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
                    {
                        int i = 0;
                        foreach (ListViewItem listItem in listDupeSongs.Items)
                        {
                            if (listItem.Checked)
                            {
                                //Is a try/catch block required here?
                                File.Delete(DupeCollection[i].SongOnePath);
                                DupeCollection.RemoveAt(i);
                                listDupeSongs.Items.RemoveAt(i);
                                tbDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
                            }
                            i++;
                        }
                    });
        }

        private void btnDeleteSongTwo_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
            {
                int i = 0;
                foreach (ListViewItem listItem in listDupeSongs.Items)
                {
                    if (listItem.Checked)
                    {
                        File.Delete(DupeCollection[i].SongTwoPath);
                        DupeCollection.RemoveAt(i);
                        listDupeSongs.Items.RemoveAt(i);    
                        tbDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
                    }
                    i++;
                }
            });
        }
        private void btnRescan_Click(object sender, EventArgs e)
        {
            SongCollection.Clear();
            BackgroundScan();
        }
        private void lnkAboutCF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com");
        }

        private void timerAutoUpdate_Tick(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void tbSettingsRSDir_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tbSettingsRSDir.Enabled)
            {
                if (folderBrowserDialog_SettingsRSPath.ShowDialog() == DialogResult.OK)
                {
                    tbSettingsRSDir.Text = folderBrowserDialog_SettingsRSPath.SelectedPath;
                }
            }
        }
        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            SaveSettingsToFile();
        }
        private void btnSettingsLoad_Click(object sender, EventArgs e)
        {
            LoadSettingsFromFile();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchDLC(tbSearch.Text);
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbSearch.Text.Length > 0 && e.KeyCode == Keys.Enter)
            {
                SearchDLC(tbSearch.Text);
            }
        }

        private void link_LovromanProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com/user/43194-lovroman/");
        }

        private void link_DarjuszProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com/user/5299-darjusz/");
        }

        private void link_Alex360Profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com/user/236-alex360/");
        }

        private void link_UnleashedProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com/user/1-unleashed2k/");
        }

        private void link_ForgeOnProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://customsforge.com/user/345-forgeon/");
        }

        #endregion

        #region Settings
        private void ResetSettings()
        {
            mySettings = new Settings();
            mySettings.LogFilePath = Constants.DefaultWorkingDirectory + "\\settings.bin";
            mySettings.RSInstalledDir = GetInstallDirFromRegistry();
        }
        private void SaveSettingsToFile(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = Constants.DefaultWorkingDirectory + "\\settings.bin";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                if (mySettings == null)
                {
                    mySettings = new Settings();
                    mySettings.LogFilePath = Constants.DefaultWorkingDirectory + Constants.DefaultLogName;
                    mySettings.RSInstalledDir = tbSettingsRSDir.Text;
                    Log("Default settings created...");
                }
                mySettings.Serialze(fs);
                Log("Saved settings...");
                fs.Close();
            }
        }
        private void LoadSettingsFromFile(string path = "")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    path = Constants.DefaultWorkingDirectory + "\\settings.bin";
                if (!File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                    {
                        ResetSettings();
                        Log("Settings file created...");
                        fs.Close();
                    }
                    SaveSettingsToFile(path);
                }
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    Settings deserialized = fs.DeSerialize() as Settings;
                    if (deserialized != null)
                    {
                        mySettings.RSInstalledDir = deserialized.RSInstalledDir;
                        mySettings.LogFilePath = deserialized.LogFilePath;
                        tbSettingsRSDir.InvokeIfRequired(delegate
                        {
                            tbSettingsRSDir.Text = mySettings.RSInstalledDir;
                        });
                        Log("Loaded settings...");
                        fs.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Log(string.Format("<Error>: {0}", ex.Message));
            }
        }
        #endregion
        private void PopulateSongList()
        {
            Log("Scanning for songs...");

            int counter = 1;
            List<string> filesList = new List<string>(FilesList(mySettings.RSInstalledDir));
            foreach (string file in filesList)
            {
                Progress(counter++ * 100 / filesList.Count);
                try
                {
                    var browser = new PsarcBrowser(file);
                    var songList = browser.GetSongList();
                    foreach (var song in songList)
                    {
                        var arrangements = "";
                        foreach (string arrangement in song.Arrangements)
                        {
                            arrangements += "," + arrangement;
                        }
                        arrangements = arrangements.Remove(0, 1);
                        SongCollection.Add(new SongData
                        {
                            Preview = "",
                            Song = song.Title,
                            Artist = song.Artist,
                            Album = song.Album,
                            Updated = song.Updated,
                            Tuning = TuningToName(song.Tuning),
                            Arrangements = arrangements,
                            Author = song.Author,
                            NewAvailable = "",
                            Path = file
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log(file + ":" + ex.Message);
                }
                finally
                {
                    toolStripStatusLabel_Main.Text = string.Format("{0} songs found...", counter);
                }
            }
        }

        private void PopulateDupeList()
        {
            foreach (SongData song in SongCollection)
            {
                var dupes = SongCollection.Where(x => x.Song.ToLower() == song.Song.ToLower() && x.Album == song.Album).ToList();
                if (dupes.Count > 1)
                {
                    if (DupeCollection.Where(x => x.Song.ToLower() == song.Song.ToLower()).ToList().Count > 0)
                    {
                        continue;
                    }
                    else
                    {
                        DupeCollection.Add(new SongDupeData
                                               {
                                                   Song = dupes[0].Song,
                                                   Artist = dupes[0].Artist,
                                                   Album = dupes[0].Album,
                                                   SongOnePath = dupes[0].Path,
                                                   SongTwoPath = dupes[1].Path
                                               });
                    }
                }
            }
            tbDuplicates.InvokeIfRequired(delegate
            {
                tbDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
            });
        }
        public static List<string> FilesList(string path)
        {
            List<string> files = new List<string>(Directory.GetFiles(path, "*_p.psarc", SearchOption.AllDirectories));
            return files;
        }

        public static string TuningToName(string tuning)
        {
            switch (tuning)
            {
                case "000000":
                    return "E Standard";
                case "-100000":
                    return "E Drop D";
                case "-2-2-2-2-2-2":
                    return "D Standard";
                case "-3-1-1-1-1-1":
                    return "Eb Drop Db";
                case "-3-3-3-3-3-3":
                    return "C# Standard";
                case "-4-4-4-4-4-4":
                    return "C Standard";
                case "-4-2-2-2-2-2":
                    return "D Drop C";
                case "-5-5-5-5-5-5":
                    return "B Standard";
                case "-5-3-3-3-3-3":
                    return "C# Drop B";
                case "-7-7-7-7-7-7":
                    return "A Standard";
                case "-7-5-5-5-5-5":
                    return "B Drop A";
                default:
                    return "Other";
            }
        }

        private void CheckForUpdate()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                UpdateCheckInfo info;
                try
                {
                    info = ad.CheckForDetailedUpdate();
                    if (info.UpdateAvailable)
                    {
                        Boolean doUpdate = true;

                        if (!info.IsUpdateRequired)
                        {
                            DialogResult dr = MessageBox.Show("An update is available! Would you like to update the application now", "Update Available", MessageBoxButtons.OKCancel);
                            if (DialogResult.OK != dr)
                            {
                                doUpdate = false;
                            }
                        }
                        else
                        {
                            // Display a message that the app MUST reboot. Display the minimum required version.
                            MessageBox.Show("This application has detected a mandatory update from your current version to version"
                                + info.MinimumRequiredVersion
                                + "The application will now install the update and restart.",
                                "Update_Available", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }

                        if (doUpdate)
                        {
                            try
                            {
                                ad.Update();
                                MessageBox.Show("The application has been upgraded and will now restart");
                                Application.Restart();
                            }
                            catch (DeploymentDownloadException dde)
                            {
                                Log("<Error>: " + dde.Message);
                            }
                        }
                    }
                }
                catch (Exception dde)
                {
                    Log("<Error>: " + dde.Message);
                }
            }
        }
        private string GetInstallDirFromRegistry()
        {
            string result = "";
            object test = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014", "installdir", null);
            if (test != null)
                result = test.ToString();
            else
            {
                test =
                    Registry.GetValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680",
                        "InstallLocation", null);
                if (test != null)
                    result = test.ToString();
            }
            return result;
        }
        private void Progress(int value)
        {
            toolStripProgressBarMain.ProgressBar.InvokeIfRequired(delegate
            {
                if (toolStripProgressBarMain.ProgressBar != null)
                    toolStripProgressBarMain.ProgressBar
                        .Value = value;
            });
        }
        private void Log(string message)
        {
            myLog.Write(message);
        }
        private void Log(string logMessage, int progress = 1)
        {
            Log(logMessage);
            Progress(progress);
        }
        private void ERR_NI()
        {
            MessageBox.Show("Not implemented yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        

        private void SearchDLC(string criteria)
        {
            MessageBox.Show("Work in progress!","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        
    }
}
