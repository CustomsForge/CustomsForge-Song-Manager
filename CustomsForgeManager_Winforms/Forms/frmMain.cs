using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {
        private bool allSelected = false;
        private bool sortDescending = true;
        private readonly Log myLog;
        private Settings mySettings;
        private static XElement root;

        private BindingList<SongData> SongCollection = new BindingList<SongData>();
        private List<SongDupeData> DupeCollection = new List<SongDupeData>();
        private List<SongData> SortedSongCollection = new List<SongData>();

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
            root = XElement.Load("tunings.xml");
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

            string enabled = "";
            int counter = 1;
            List<string> filesList = new List<string>(FilesList(mySettings.RSInstalledDir + "\\dlc"));
            filesList.AddRange(FilesList(mySettings.DisabledDLCDir));

            foreach (string file in filesList)
            {
                Progress(counter++ * 100 / filesList.Count);

                if (file.Contains("disabledDLC"))
                {
                    enabled = "No";
                }
                else
                {
                    enabled = "Yes";
                }

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
                            DD = DifficultyToDD(song.DD),
                            Enabled = enabled
                        };
                        dgvSongs.InvokeIfRequired(delegate
                        {
                            SongCollection.Add(newSong);
                        });
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Error reading JObject"))
                    {
                        Log(file + ":" + "DLC is corrupt!");
                    }
                    else
                    {
                        Log(file + ":" + ex.Message);
                    }
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
                    if (DupeCollection.Where(x => x.Song.ToLower() == song.Song.ToLower()).ToList().Count == 0)
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
                bs.DataSource = SongCollection;
                dgvSongs.DataSource = bs;
                dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                SortedSongCollection = SongCollection.ToList();
                if (mySettings.ManagerGridSettings != null)
                    dgvSongs.ReLoadColumnOrder(mySettings.ManagerGridSettings.ColumnOrder);
                dgvSongs.Columns["colSelect"].Visible = true;
                dgvSongs.Columns["colSelect"].DisplayIndex = 0;
            });
            Log("Finished scanning songs...", 100);
        }

        #region Settings
        private void ResetSettings()
        {
            mySettings = new Settings();
            mySettings.LogFilePath = Constants.DefaultWorkingDirectory + "\\debug.log";
            mySettings.RSInstalledDir = GetInstallDirFromRegistry();
            mySettings.DisabledDLCDir = mySettings.RSInstalledDir + @"\disabledDLC";
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
                    mySettings.DisabledDLCDir = mySettings.RSInstalledDir + @"\disabledDLC";
                    mySettings.RescanOnStartup = true;
                    Log("Default settings created...");
                }
                if (!string.IsNullOrEmpty(tbSettingsRSDir.Text))
                    mySettings.RSInstalledDir = tbSettingsRSDir.Text;
                mySettings.RescanOnStartup = checkRescanOnStartup.Checked;
                mySettings.DisabledDLCDir = mySettings.RSInstalledDir + @"\disabledDLC";

                RADataGridViewSettings settings = new RADataGridViewSettings();
                var columns = dgvSongs.Columns;

                if (columns.Count > 1)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        settings.ColumnOrder.Add(new ColumnOrderItem
                        {
                            ColumnIndex = i,
                            DisplayIndex = columns[i].DisplayIndex,
                            //Visible = columns[i].Visible,
                            Width = columns[i].Width
                        });
                    }
                    mySettings.ManagerGridSettings = settings;
                }
                mySettings.Serialze(fs);
                Log("Saved settings...");
                fs.Close();
            }
        }
        private void LoadSettingsFromFile(string path = "")
        {
            //Dictionary<string, List<ColumnOrderItem>> columnOrder = new Dictionary<string, List<ColumnOrderItem>>();
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
                        mySettings = deserialized;

                        tbSettingsRSDir.InvokeIfRequired(delegate
                        {
                            tbSettingsRSDir.Text = mySettings.RSInstalledDir;
                        });
                        checkRescanOnStartup.InvokeIfRequired(delegate
                        {
                            checkRescanOnStartup.Checked = mySettings.RescanOnStartup;
                        });
                        if (mySettings.ManagerGridSettings != null)
                        {
                            dgvSongs.ReLoadColumnOrder(mySettings.ManagerGridSettings.ColumnOrder);
                        }
                        Log("Loaded settings...");
                    }
                    fs.Close();
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

            btnCheckAllForUpdates.InvokeIfRequired(delegate
            {
                btnCheckAllForUpdates.Enabled = !btnCheckAllForUpdates.Enabled;
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
        public static string TuningToName(string tuningStrings)
        {
            var tuningName = root.Elements("Tuning").Where(tuning => tuning.Attribute("Strings").Value == tuningStrings).Select(tuning => tuning.Attribute("Name")).ToList();
            if (tuningName.Count == 0)
            {
                return "Other";
            }
            return tuningName[0].Value;
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
            var results = SongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower())).ToList();
            SortedSongCollection = (List<SongData>)(SongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()))).ToList();
            dgvSongs.InvokeIfRequired(delegate
            {
               dgvSongs.DataSource = results;
            });
        }
        private void ShowSongInfo()
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
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
        }

        #region GUIEventHandlers
        private void btnDisableEnableSongs_Click(object sender, EventArgs e)
        {
            string disabledFolderPath = mySettings.RSInstalledDir + @"\dlcDisabled";
            string dlcPath = "";
            if (!Directory.Exists(disabledFolderPath))
            {
                Directory.CreateDirectory(disabledFolderPath);
            }
            try
            {
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colSelect"].Value) == true)
                    {
                        SongData currentSong = SongCollection.Where(song => song.Song == (string)row.Cells["Song"].Value).ToList()[0];
                        if (currentSong.Enabled == "Yes")
                        {
                            dlcPath = mySettings.DisabledDLCDir + @"\" + Path.GetFileName(currentSong.Path);
                            File.Move(currentSong.Path, dlcPath);
                            currentSong.Path = dlcPath;
                        }
                        else
                        {
                            dlcPath = mySettings.RSInstalledDir + @"\dlc\" + Path.GetFileName(currentSong.Path);
                            File.Move(currentSong.Path, dlcPath);
                            currentSong.Path = dlcPath;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Log(ex.Message);
            }
        }
        private void showDLCInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }
        private void dgvSongs_DataSourceChanged(object sender, EventArgs e)
        {
            var dataGridViewColumn = ((DataGridView)sender).Columns["Preview"];
            if (dataGridViewColumn != null && dataGridViewColumn.Visible)
                dataGridViewColumn.Visible = false;
        }
        private void dgvSongs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) //if it's not header
                ShowSongInfo();
        }
        private void dgvSongs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int scrollOffset = 0;
            BindingSource bs = new BindingSource();
            var songsToShow = SortedSongCollection;
            
            switch (dgvSongs.Columns[e.ColumnIndex].Name)
            {
                case "colSelect":
                    bs.DataSource = songsToShow.ToList();
                    break;
                case "Enabled":
                    if (sortDescending)
                    {
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Enabled);
                        sortDescending = false;
                    }
                    else
                    {
                        bs.DataSource = songsToShow.OrderBy(song => song.Enabled);
                        sortDescending = true;
                    }
                    break;
                case "Song":
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
                    break;
                case "Artist":
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
                    break;
                case "Album":
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
                    break;
                case "Updated":
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
                    break;
                case "Tuning":
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
                    break;
                case "DD":
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
                    break;
                case "Arrangements":
                    if (sortDescending)
                    {
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Arrangements);
                        sortDescending = false;
                    }
                    else
                    {
                        bs.DataSource = songsToShow.OrderBy(song => song.Arrangements);
                        sortDescending = true;
                    }
                    break;
                case "Author":
                    if (sortDescending)
                    {
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Author);
                        sortDescending = false;
                    }
                    else
                    {
                        bs.DataSource = songsToShow.OrderBy(song => song.Author);
                        sortDescending = true;
                    }
                    break;
                case "NewAvailable":
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
                    break;
            }
            scrollOffset = dgvSongs.HorizontalScrollingOffset;
            dgvSongs.DataSource = bs;
            dgvSongs.HorizontalScrollingOffset = scrollOffset;
           
        }
        private void btnSongsToBBCode_Click(object sender, EventArgs e)
        {
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("Song - Artist, Album, Updated, Tuning, DD, Arangements, Author");
            sbTXT.AppendLine("[LIST=1]");
            foreach (var song in SongCollection)
            {
                if (song.Author == null)
                {
                    sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + DifficultyToDD(song.DD) + ", " + song.Arrangements + "[/*]");
                }
                else
                {
                    sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + DifficultyToDD(song.DD) + ", " + song.Arrangements + ", " + song.Author + "[/*]");
                }
            }
            sbTXT.AppendLine("[/LIST]");

            using (frmBBCode FormBBCode = new frmBBCode())
            {
                FormBBCode.BBCode = sbTXT.ToString();
                FormBBCode.ShowDialog();
            }
        }
        private void btnSongsToCSV_Click(object sender, EventArgs e)
        {
            var sbCSV = new StringBuilder();
            string path = Constants.DefaultWorkingDirectory + @"\SongListCSV.csv";

            sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
            sfdSongListToCSV.FileName = "SongListCSV";

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
                string steamUserdataPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) + @"\userdata";
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
        private void btnDeleteSongOne_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
            {
                for (int i = 0; i < listDupeSongs.Items.Count; i++)
                {
                    if (listDupeSongs.Items[i].Checked)
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
                }
            });
        }
        private void btnDeleteSongTwo_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
            {
                for (int i = 0; i < listDupeSongs.Items.Count; i++)
                {
                    if (listDupeSongs.Items[i].Checked)
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
        private void linkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (allSelected)
                    {
                        row.Cells["colSelect"].Value = false;
                    }
                    else
                    {
                        row.Cells["colSelect"].Value = true;
                    }
                }
                allSelected = !allSelected;
        }

        private void link_MainClearResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvSongs.InvokeIfRequired(delegate
            {
                dgvSongs.DataSource = SongCollection;
            });
            tbSearch.InvokeIfRequired(delegate { tbSearch.Text = ""; });
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
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettingsToFile();
        }

        private void dgvSongs_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvSongs.SelectedRows.Count > 0)
            {
                CheckRowForUpdate(dgvSongs.SelectedRows[0]);
            }
        }

        private void btnCheckAllForUpdates_Click(object sender, EventArgs e)
        {
            int count = dgvSongs.RowCount;
            foreach (var row in dgvSongs.Rows)
            {
                CheckRowForUpdate((DataGridViewRow)row);
            }
        }

        private void openDLCPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ERR_NI();
        }
        #endregion

        private string GetDLCInfoFromURL(string apiUrl, string fieldName)
        {
            string result = "";
            WebClient client = new WebClient();
            var response = client.DownloadString(apiUrl);
            JArray jsonJArray = JArray.Parse(response);
            JToken jsonJToken = jsonJArray.First;
            result = jsonJToken.SelectToken(fieldName).ToString();
            return result;
        }
        private void CheckRowForUpdate(DataGridViewRow dataGridViewRow)
        {
            var selectedRow = dataGridViewRow;
            string name = selectedRow.Cells["Song"].Value.ToString();
            string artist = selectedRow.Cells["Artist"].Value.ToString();
            string album = selectedRow.Cells["Album"].Value.ToString();

            string apiUrl = Constants.DefaultServiceURL + "/" + artist + "/" + album + "/" + name;

            try
            {
                string ignition_updated = GetDLCInfoFromURL(apiUrl, "updated");
                string file_updated = selectedRow.Cells["Updated"].Value.ToString();

                DateTime ignitionDate;
                DateTime fileDate;

                if (DateTime.TryParse(ignition_updated, out ignitionDate))
                {
                    if (DateTime.TryParse(file_updated, out fileDate))
                    {
                        DateTime ignitionDays = new DateTime(year: ignitionDate.Year, month: ignitionDate.Month, day: ignitionDate.Day);
                        DateTime fileDays = new DateTime(year: fileDate.Year, month: fileDate.Month, day: fileDate.Day);

                        if (!ignitionDays.Equals(fileDays))
                        {
                            selectedRow.DefaultCellStyle.BackColor = Color.Red;
                            myLog.Write(string.Format("New version available for \"{0}\" from \"{1}\" album by {2}", name, album, artist));
                            selectedRow.Selected = false;
                        }
                        else
                            myLog.Write(string.Format("No new version found for \"{0}\" from \"{1}\" album by {2}", name, album, artist));
                    }
                }
            }
            catch (Exception wex)
            {
                myLog.Write(string.Format("<Error>: {0}", wex.Message));
            }
        }
    }
}
