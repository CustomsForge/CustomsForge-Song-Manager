using System;
using System.Deployment.Application;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Logging;
using CustomsForgeManager_Winforms.Utilities;
using Microsoft.Win32;
using System.Collections.Generic;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {
        
        private readonly Log myLog;
        private readonly Settings mySettings;
        private readonly SettingsData settingsData;
        private List<SongData> SongCollection = new List<SongData>();

        private frmMain()
        {
            InitializeComponent();
            Init();
        }

        public frmMain(Log myLog, Settings mySettings, SettingsData settingsData)
        {
            // TODO: Complete member initialization
            this.myLog = myLog;
            this.mySettings = mySettings;
            this.settingsData = settingsData;
            InitializeComponent();
            Init();
            //PopulateList();
        }

        void LoadSaveDefaultSettingsFile() 
        {
            using (FileStream fs = new FileStream(Constants.DefaultWorkingDirectory + @"\settings.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                if (File.Exists(Constants.DefaultWorkingDirectory + @"\settings.bin"))
                {
                    SettingsData deserialized = fs.DeSerialize() as SettingsData;
                    if (deserialized != null)
                    {
                        mySettings.RSInstalledDir = deserialized.RSInstalledDir;
                        mySettings.LogFilePath = deserialized.LogFilePath;
                    }
                    else
                    {
                        settingsData.RSInstalledDir = Constants.DefaultRocksmithPath;
                        settingsData.LogFilePath = Constants.DefaultLogName;
                        mySettings.RSInstalledDir = Constants.DefaultRocksmithPath;
                        Extensions.Serialze(settingsData, fs);
                    }
                }
            }
        }
        private void Init()
        {
            #region Create directory structure if not exists

            string configFolderPath = Constants.DefaultWorkingDirectory;
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
            }

            #endregion

            #region Load Settings file and deserialize to Settings class
            if (Directory.Exists(Constants.DefaultWorkingDirectory))
            {
                LoadSaveDefaultSettingsFile();
            }
            else
            {
                Directory.CreateDirectory(Constants.DefaultWorkingDirectory);
                LoadSaveDefaultSettingsFile();
            }
            #endregion

            #region Logging setup

            myLog.AddTargetFile(mySettings.LogFilePath);
            myLog.AddTargetControls(tbLog);

            #endregion

            if (ApplicationDeployment.IsNetworkDeployed)
                Log(string.Format("Application loaded, using version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion), 100);

            if (!Directory.Exists(mySettings.RSInstalledDir))
            {
                Log("Getting Rocksmith 2014 install dir...");
                mySettings.RSInstalledDir = GetInstallDirFromRegistry();
                if (Directory.Exists(mySettings.RSInstalledDir))
                {
                    Log(string.Format("Found Rocksmith at: {0}", mySettings.RSInstalledDir), 100);
                    tbSettingsRSDir.Text = mySettings.RSInstalledDir;
                }
                else 
                {
                    tcMain.SelectedTab = tpSettings;
                    MessageBox.Show("Please enter Rocksmith path in the textbox and press Save button!");
                }
            }
            //Execute this if the dir exists or ...? 
            BackgroundScan();
        }

        private void BackgroundScan()
        {
            btnRescan.Enabled = false;
            bWorker.DoWork -= PopulateListHandler;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted -= PopulateCompletedHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;
            bWorker.RunWorkerAsync();
        }

        void PopulateCompletedHandler(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            listSongs.InvokeIfRequired(delegate { listSongs.Items.Clear(); });
            toolStripStatusLabel_Main.Text = string.Format("{0} songs found", SongCollection.Count);
            foreach (SongData song in SongCollection)
            {
                listSongs.InvokeIfRequired(delegate
                {
                    listSongs.Items.Add(new ListViewItem(new string[] { " ", song.Preview, song.Artist, song.Song, song.Album, song.Tuning, song.Arrangements, song.Updated, song.Author, song.NewAvailable }));
                });
            }
            listSongs.InvokeIfRequired(delegate
            {
                listSongs.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);//.ColumnContent);
            });
            Log("Finished scanning songs...", 100);
            btnRescan.Enabled = true;
        }

        void PopulateListHandler(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PopulateList();
        }

        #region GUIEventHandlers

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
            ERR_NI();
        }
        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            mySettings.RSInstalledDir = tbSettingsRSDir.Text;
            settingsData.RSInstalledDir = tbSettingsRSDir.Text;
            using (FileStream fs = new FileStream(Constants.DefaultWorkingDirectory + @"\settings.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Extensions.Serialze(settingsData, fs);
            }
        }
        private void btnSettingsLoad_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(Constants.DefaultWorkingDirectory + @"\settings.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SettingsData deserialized = (SettingsData)Extensions.DeSerialize(fs);
                mySettings.RSInstalledDir = deserialized.RSInstalledDir;
                mySettings.LogFilePath = deserialized.LogFilePath;
            }
        }
        #endregion

        private void PopulateList()
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
                            NewAvailable = ""
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log(file + ":" + ex.Message);
                }
            }
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

        private void btnRescan_Click(object sender, EventArgs e)
        {
            listSongs.Items.Clear();
            SongCollection.Clear();
            BackgroundScan();
        }
    }
}
