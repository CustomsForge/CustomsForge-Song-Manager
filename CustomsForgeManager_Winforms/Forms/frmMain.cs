using System;
using System.Deployment.Application;
using System.IO;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Logging;
using CustomsForgeManager_Winforms.Utilities;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {
        private bool sortDescending = true;
        private readonly Log myLog;
        private Settings mySettings;

        private BindingList<SongData> SongCollection = new BindingList<SongData>();
        private List<SongDupeData> DupeCollection = new List<SongDupeData>();

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

            if (mySettings.RescanOnStartup)
                BackgroundScan();
            else
                LoadSongCollectionFromFile();
        }
        private void BackgroundScan()
        {
            bWorker.DoWork -= PopulateListHandler;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted -= PopulateCompletedHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;
            bWorker.RunWorkerAsync();
        }
        void PopulateCompletedHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            PopulateDataGridView();
            SaveSongCollectionToFile();
            ToggleUIControls();
        }
        void PopulateListHandler(object sender, DoWorkEventArgs e)
        {
            ToggleUIControls();
            PopulateSongList();
            PopulateDupeList();
        }
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
                        var newSong = new SongData
                        {
                            Song = song.Title,
                            Artist = song.Artist,
                            Album = song.Album,
                            Updated = song.Updated,
                            SongYear = song.Year,
                            Tuning = TuningToName(song.Tuning),
                            Arrangements = arrangements,
                            Author = song.Author,
                            NewAvailable = "",
                            Path = file,
                            DD = song.DD
                        };
                        SongCollection.Add(newSong);
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
            tpDuplicates.InvokeIfRequired(delegate
            {
                tpDuplicates.Text = "Duplicates(" + DupeCollection.Count + ")";
            });
        }
        private void PopulateDataGridView()
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);
            if (DupeCollection.Count > 0)
            {
                foreach (SongDupeData song in DupeCollection)
                {
                    listDupeSongs.InvokeIfRequired(delegate
                    {
                        listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Artist, song.Song, song.Album, song.SongOnePath, song.SongTwoPath }));
                    });
                }
            }

            dgvSongs.InvokeIfRequired(delegate
            {
                BindingSource bs = new BindingSource();
                var songsToShow = from song in SongCollection
                                  select new
                                  {
                                      song.Song, song.Artist, song.Album, song.Updated, song.Tuning,
                                      DD = DifficultyToDD(song.DD), song.Arrangements, song.Author, song.NewAvailable
                                  };
                bs.DataSource = songsToShow;
                dgvSongs.DataSource = bs;
                dgvSongs.Columns[0].Visible = true;
                dgvSongs.Columns[1].Visible = true;
                dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            });
            Log("Finished scanning songs...", 100);

        }

        #region GUIEventHandlers
        private void btnSongsToCSV_Click(object sender, EventArgs e)
        {
            var sbCSV = new StringBuilder();
            string path = Constants.DefaultWorkingDirectory + @"\SongList.csv";
            if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
            {
                path = sfdSongListToCSV.FileName;
            }
            sbCSV.AppendLine(@"sep=;");
            sbCSV.AppendLine("Artist;Song;Album;Year;Tuning;Arrangements");
            foreach (var song in SongCollection)
            {
                sbCSV.AppendLine(song.Artist + ";" + song.Song + ";" + song.Album + ";" + song.SongYear + ";" + song.Tuning + ";" + song.Arrangements);
            }
            try
            {
                using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
                {
                    file.Write(sbCSV.ToString());
                }
                Log("Song list saved to:" + path);
            }
            catch (IOException ex)
            {
                Log("<Error>:" + ex.Message);
            }
        }
        private void dgvSongs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            BindingSource bs = new BindingSource();
            var songsToShow = from song in SongCollection
                              select new
                              {
                                  song.Song, song.Artist, song.Album, song.Updated, song.Tuning,
                                  DD = DifficultyToDD(song.DD), song.Arrangements, song.Author, song.NewAvailable
                              };
            if (e.ColumnIndex == 2)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.Song);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.Song);
                    sortDescending = true;
                }
            }
            else if (e.ColumnIndex == 3)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.Artist);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.Artist);
                    sortDescending = true;
                }
            }
            else if (e.ColumnIndex == 4)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.Album);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.Album);
                    sortDescending = true;
                }
            }
            else if (e.ColumnIndex == 5)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => DateTime.ParseExact(song.Updated, "M-d-y H:m", System.Globalization.CultureInfo.InvariantCulture));
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => DateTime.ParseExact(song.Updated, "M-d-y H:m", System.Globalization.CultureInfo.InvariantCulture));
                    sortDescending = true;
                }
            }
            else if (e.ColumnIndex == 6)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.Tuning);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.Tuning);
                    sortDescending = true;
                }
            }
            else if (e.ColumnIndex == 7)
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.DD);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.DD);
                    sortDescending = true;
                }
            }
            else
            {
                if (sortDescending)
                {
                    bs.DataSource = songsToShow.OrderByDescending(song => song.Song);
                    sortDescending = false;
                }
                else
                {
                    bs.DataSource = songsToShow.OrderBy(song => song.Song);
                    sortDescending = true;
                }
            }
            dgvSongs.DataSource = bs;
        }
        private void btnLaunchSteam_Click(object sender, EventArgs e)
        {
            Process[] rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                Process.Start("steam://rungameid/221680");
        }

        private void btnBackupRSProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month,
                    DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.DefaultWorkingDirectory, timestamp);
                string profilePath = "";
                string steamUserdataPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null).ToString() + @"\userdata";
                DirectoryInfo dInfo = new DirectoryInfo(steamUserdataPath);
                DirectoryInfo[] subdirs = dInfo.GetDirectories("*", SearchOption.AllDirectories);
                foreach (DirectoryInfo info in subdirs)
                {
                    if (info.FullName.Contains(@"221680\remote"))
                    {
                        profilePath = info.FullName;
                    }
                }
                if (profilePath != "")
                {
                    ZipFile.CreateFromDirectory(profilePath, backupPath);
                    Log("Backup created at " + backupPath, 100);
                }
                else
                {
                    Log("Steam profile not found!");
                }
            }
            catch (Exception ex)
            {
                Log("<Error>:" + ex.Message);
            }
        }
        private void btnDupeRescan_Click(object sender, EventArgs e)
        {
            listDupeSongs.Items.Clear();
            DupeCollection.Clear();
            SongCollection.Clear();
            tpDuplicates.InvokeIfRequired(delegate
            {
                tpDuplicates.Text = "Duplicates(0)";
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
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if ((bool)row.Cells[1].Value)
                    {
                        fileName =  Path.GetFileName(SongCollection[i].Path);
                        File.Copy(SongCollection[i].Path, Path.Combine(backupPath, fileName));
                    }
                    i++;
                }
            }
            catch (IOException)
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
                                try
                                {
                                    File.Delete(DupeCollection[i].SongOnePath);
                                    DupeCollection.RemoveAt(i);
                                    listDupeSongs.Items.RemoveAt(i);
                                    tpDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
                                }
                                catch (IndexOutOfRangeException)
                                {
                                }
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
                        try
                        {
                            File.Delete(DupeCollection[i].SongTwoPath);
                            DupeCollection.RemoveAt(i);
                            listDupeSongs.Items.RemoveAt(i);
                            tpDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
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
            Process.Start("http://customsforge.com");
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
                    mySettings.RSInstalledDir = tbSettingsRSDir.Text;
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

        private void link_MainClearResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvSongs.DataSource = SongCollection;
            tbSearch.InvokeIfRequired(delegate { tbSearch.Text = ""; });
        }

        private void dgvSongs_DataSourceChanged(object sender, EventArgs e)
        {
            var dataGridViewColumn = ((DataGridView)sender).Columns["Preview"];
            if (dataGridViewColumn != null && dataGridViewColumn.Visible)
                dataGridViewColumn.Visible = false;
        }

        private void dgvSongs_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSongs.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSongs.SelectedRows[0];
                var title = selectedRow.Cells["Song"].Value.ToString();
                var artist = selectedRow.Cells["Artist"].Value.ToString();
                var album = selectedRow.Cells["Album"].Value.ToString();

                var song =
                    SongCollection.SingleOrDefault(x => x.Song == title && x.Album == album && x.Artist == artist);
                if (song != null)
                    PopulateSongInfo(song);
            }
        }

        private void link_LovromanProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/43194-lovroman/");
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
            mySettings.RescanOnStartup = true;
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
                    mySettings.RescanOnStartup = true;
                    Log("Default settings created...");
                }
                mySettings.RSInstalledDir = tbSettingsRSDir.Text;
                mySettings.RescanOnStartup = checkRescanOnStartup.Checked;
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
                        mySettings.RescanOnStartup = deserialized.RescanOnStartup;
                        tbSettingsRSDir.InvokeIfRequired(delegate
                        {
                            tbSettingsRSDir.Text = mySettings.RSInstalledDir;
                        });
                        checkRescanOnStartup.InvokeIfRequired(delegate
                        {
                            checkRescanOnStartup.Checked = mySettings.RescanOnStartup;
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

        private void SaveSongCollectionToFile()
        {
            string path = Constants.DefaultWorkingDirectory + "\\songs.bin";
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                SongCollection.Serialze(fs);
                Log("Song collection saved...");
                fs.Close();
            }
        }
        private void LoadSongCollectionFromFile()
        {
            string path = Constants.DefaultWorkingDirectory + "\\songs.bin";
            if (!File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                {
                    BackgroundScan();
                    Log("Song collection file created...");
                    fs.Close();
                }
                SaveSettingsToFile(path);
            }
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BindingList<SongData> songs = fs.DeSerialize() as BindingList<SongData>;
                if (songs != null)
                {
                    SongCollection = songs;
                    Log("Song collection loaded...");
                    fs.Close();
                    PopulateDataGridView();
                }
            }
        }
        private void ToggleUIControls()
        {
            btnRescan.InvokeIfRequired(delegate
            {
                btnRescan.Enabled = !btnRescan.Enabled;
            });

            //Uncomment after implementing:
            //btnEditDLC.Enabled = !btnEditDLC.Enabled;
            //btnBackupDLC.Enabled = !btnBackupDLC.Enabled;
            //btnSaveDLC.Enabled = !btnSaveDLC.Enabled;
            //btnDLCPage.Enabled = !btnDLCPage.Enabled;

            checkRescanOnStartup.InvokeIfRequired(delegate
            {
                checkRescanOnStartup.Enabled = !checkRescanOnStartup.Enabled;
            });

            btnBackupRSProfile.InvokeIfRequired(delegate
            {
                btnBackupRSProfile.Enabled = !btnBackupRSProfile.Enabled;
            });

            btnSearch.InvokeIfRequired(delegate
            {
                btnSearch.Enabled = !btnSearch.Enabled;
            });

            btnSettingsSave.InvokeIfRequired(delegate
            {
                btnSettingsSave.Enabled = !btnSettingsSave.Enabled;
            });

            btnSettingsLoad.InvokeIfRequired(delegate
            {
                btnSettingsLoad.Enabled = !btnSettingsLoad.Enabled;
            });

            tbSearch.InvokeIfRequired(delegate
            {
                tbSearch.Enabled = !tbSearch.Enabled;
            });

            tbSettingsRSDir.InvokeIfRequired(delegate
            {
                tbSettingsRSDir.Enabled = !tbSettingsRSDir.Enabled;
            });

            btnDupeRescan.InvokeIfRequired(delegate
            {
                btnDupeRescan.Enabled = !btnDupeRescan.Enabled;
            });

            btnDeleteSongOne.InvokeIfRequired(delegate
            {
                btnDeleteSongOne.Enabled = !btnDeleteSongOne.Enabled;
            });

            btnDeleteSongTwo.InvokeIfRequired(delegate
            {
                btnDeleteSongTwo.Enabled = !btnDeleteSongTwo.Enabled;
            });
        }
        public static List<string> FilesList(string path)
        {
            List<string> files = new List<string>(Directory.GetFiles(path, "*_p.psarc", SearchOption.AllDirectories));
            return files;
        }
        public string DifficultyToDD(string maxDifficulty)
        {
            if (maxDifficulty == "0")
            {
                return "No";
            }
            return "Yes";
        }

        public static string TuningToName(string tuning)
        {
            switch (tuning)
            {
                case "000000":
                    return "E Standard";
                case "-1-1-1-1-1-1":
                    return "Eb Standard";
                case "-2-2-2-2-2-2":
                    return "D Standard";
                case "-3-3-3-3-3-3":
                    return "C# Standard";
                case "-4-4-4-4-4-4":
                    return "C Standard";
                case "-5-5-5-5-5-5":
                    return "B Standard";
                case "-6-6-6-6-6-6":
                    return "Bb Standard";
                case "-7-7-7-7-7-7":
                    return "A Standard";
                case "-8-8-8-8-8-8":
                    return "Ab Standard";
                case "-100000":
                    return "E Drop D";
                case "-3-1-1-1-1-1":
                    return "Eb Drop Db";
                case "-4-2-2-2-2-2":
                    return "D Drop C";
                case "-5-3-3-3-3-3":
                    return "C# Drop B";
                case "-7-5-5-5-5-5":
                    return "B Drop A";
                case "-8-6-6-6-6-6":
                    return "Bb Drop Ab";
                case "00000-10":
                    return "Open A";
                case "-799900":
                    return "Open B";
                case "-4-2-2010":
                    return "Open C";
                case "-200-1-2-2":
                    return "Open D";
                case "-2-2000-2":
                    return "Open G";
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
            var results = SongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower())).ToList();
            dgvSongs.DataSource = results;
        }

        

        private void PopulateSongInfo(SongData song)
        {
            lbl_PanelSongTitle.Text = song.Song;
            lbl_PanelSongAlbum.Text = song.Album;
            lbl_PanelSongArtist.Text = song.Artist;
            lbl_PanelSongYear.Text = song.SongYear;
            lbl_PanelSongTuning.Text = song.Tuning;
            lbl_PanelSongArrangements.Text = song.Arrangements;
            lbl_PanelSongDD.Text = DifficultyToDD(song.DD);
            lbl_PanelSongAuthor.Text = song.Author;
        }
    }
}
