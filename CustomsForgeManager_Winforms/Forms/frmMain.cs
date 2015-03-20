using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Controls;
using CustomsForgeManager_Winforms.lib;
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
using System.Reflection;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {
        const string rscompatibility = "rs1compatibility";

        private bool allSelected = false;
        private bool sortDescending = true;
        private readonly Log myLog;
        private Settings mySettings;
        private Stopwatch counterStopwatch = new Stopwatch();
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private Version appVersion;
        private int songCounter = 1;

        private BindingList<SongData> SongCollection = new BindingList<SongData>();
        private List<SongData> DupeCollection = new List<SongData>();
        private List<SongData> SortedSongCollection = new List<SongData>();
        private Dictionary<string, SongData> OutdatedSongList = new Dictionary<string, SongData>();
        private List<string> CurrentFileList = new List<string>();

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
            myLog.AddTargetTextBox(tbLog);
            myLog.AddTargetNotifyIcon(notifyIcon_Main);

            #endregion

            #region Get version information

            string cfm_version = String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
            if (ApplicationDeployment.IsNetworkDeployed)
                Log(string.Format("Application loaded, using version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion), 100);

            if (appVersion != null)
            {
                lbl_AppVersion.Text = "Version: " + appVersion.ToString();
            }

            Log(GetRSTKLibVersion());

            #endregion

            #region Load Settings file and deserialize to Settings class

            LoadSettingsFromFile();

            #endregion

            if (mySettings.RescanOnStartup)
                BackgroundScan();
            else
                LoadSongCollectionFromFile();
        }

        private string GetRSTKLibVersion()
        {
            Assembly assembly = Assembly.LoadFrom("RocksmithToolkitLib.dll");
            Version ver = assembly.GetName().Version;
            return String.Format("RocksmithToolkitLib version: {0}", ver);
        }

        private void BackgroundScan()
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();

            toolStripStatusLabel_MainCancel.Visible = true;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;
            bWorker.RunWorkerAsync();
        }
        void PopulateCompletedHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            PopulateDataGridView();
            Log("Finished scanning songs...", 100);
            SaveSongCollectionToFile();
            ToggleUIControls();
            PopulateColumnList();
            toolStripStatusLabel_MainCancel.Visible = false;
        }
        void PopulateListHandler(object sender, DoWorkEventArgs e)
        {
            ToggleUIControls();
            PopulateSongList();
            PopulateDupeList();
        }

        private void PopulateColumnList()
        {
            listDisabledColumns.Items.Clear();
            foreach (DataGridViewColumn col in dgvSongs.Columns)
            {
                listDisabledColumns.Items.Add(new ListViewItem(new[] { "", col.HeaderText, col.Visible ? "Yes" : "No" }));
            }
        }

        private void parsePSARC(int counter, string enabled, string file)
        {
            try
            {
                var browser = new PsarcBrowser(file);
                var songInfo = browser.GetSongs();
                foreach (var songData in songInfo.Distinct())
                {
                    songData.Enabled = enabled;
                    if (songData.Version == "N/A")
                    {
                        string fileNameVersion = songData.GetVersionFromFileName();
                        if (fileNameVersion != "")
                            songData.Version = fileNameVersion;
                    }
                    if (songData.ToolkitVer == "")
                    {//ODLC
                        if (!songData.FileName.Contains(rscompatibility))
                            SongCollection.Add(songData);
                        else if (songData.FileName.Contains(rscompatibility) && mySettings.IncludeRS1DLCs)
                            SongCollection.Add(songData);
                        //else skip
                    }
                    else
                        SongCollection.Add(songData);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Error reading JObject"))
                {
                    Log("<ERROR>: " + file + ":" + "DLC is corrupt!");
                }
                else
                {
                    Log("<ERROR>: " + file + ":" + ex.Message);
                }
            }
            finally
            {
                toolStripStatusLabel_Main.Text = string.Format(" Songs found: {0}", counter);
                //populate dgv here
            }
        }

        private void PopulateSongList()
        {
            Log("Scanning for songs...");
            dgvSongs.InvokeIfRequired(delegate
            {
                var dataGridViewColumn = dgvSongs.Columns["colSelect"];
                if (dataGridViewColumn != null)
                    dataGridViewColumn.Visible = false;
                dgvSongs.DataSource = null;
                SongCollection.Clear();
            });
            string enabled = "";
            CurrentFileList = FilesList(mySettings.RSInstalledDir + "\\dlc", mySettings.IncludeRS1DLCs);
            //List<string> disabledFilesList = new List<string>(FilesList(mySettings.RSInstalledDir + "\\" + Constants.DefaultDisabledSubDirectory,mySettings.IncludeRS1DLCs));
            //filesList.AddRange(disabledFilesList);
            Log(String.Format("Raw songs count: {0}", CurrentFileList.Count));
            counterStopwatch.Start();
            foreach (string file in CurrentFileList)
            {
                if (!bWorker.CancellationPending)
                {
                    Progress(songCounter++ * 100 / CurrentFileList.Count);

                    enabled = file.Contains(".disabled.") ? "No" : "Yes";
                    parsePSARC(songCounter, enabled, file);
                }
            }
            dgvSongs.InvokeIfRequired(delegate
            {
                var dataGridViewColumn = dgvSongs.Columns["colSelect"];
                if (dataGridViewColumn != null) dataGridViewColumn.Visible = true;
                dgvSongs.DataSource = SongCollection;
            });
        }
        private void PopulateDupeList()
        {
            var dups = SongCollection.GroupBy(x => new { x.Song, x.Album, x.Artist })
                        .Where(group => group.Count() > 1)
                        .SelectMany(group => group).ToList();
            dups.RemoveAll(x => x.FileName.Contains(rscompatibility));

            if (dups.Count > 0)
            {
                foreach (var song in dups)
                {
                    listDupeSongs.InvokeIfRequired(delegate
                    {
                        listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Artist, song.Song, song.Album, song.Updated, song.Path }));
                    });
                }
            }

            DupeCollection.AddRange(dups);

            tpDuplicates.InvokeIfRequired(delegate
            {
                tpDuplicates.Text = "Duplicates(" + dups.Count + ")";
            });
        }
        private void PopulateDataGridView()
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);


            dgvSongs.InvokeIfRequired(delegate
            {
                var bs = new BindingSource();
                bs.DataSource = SongCollection;
                dgvSongs.DataSource = bs;
                dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                SortedSongCollection = SongCollection.ToList();
                if (mySettings.ManagerGridSettings != null)
                    dgvSongs.ReLoadColumnOrder(mySettings.ManagerGridSettings.ColumnOrder);
                dgvSongs.Columns["colSelect"].Visible = true;
                dgvSongs.Columns["colSelect"].DisplayIndex = 0;
            });

            numberOfDisabledDLC = SongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;

            toolStripStatusLabel_DisabledCounter.Visible = true;
            toolStripStatusLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate + " | Disabled DLC: " + numberOfDisabledDLC.ToString();

            counterStopwatch.Stop();
            Log(string.Format("Finished. Task took {0}", counterStopwatch.Elapsed));
        }

        #region Settings
        private void ResetSettings()
        {
            mySettings = new Settings();
            mySettings.LogFilePath = Constants.DefaultWorkingDirectory + Constants.DefaultLogName;
            mySettings.RSInstalledDir = GetInstallDirFromRegistry();
            mySettings.RescanOnStartup = true;
            mySettings.IncludeRS1DLCs = false;
            mySettings.EnabledLogBaloon = true;
            tbSettingsRSDir.Text = mySettings.RSInstalledDir;
            Log("Settings reset to defaults...");
        }
        private void SaveSettingsToFile(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = Constants.DefaultWorkingDirectory + "\\settings.bin";

            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                if (mySettings == null)
                {//Odd cases!
                    ResetSettings();
                }
                else
                {//getMySettings();
                    mySettings.RSInstalledDir = tbSettingsRSDir.Text ?? mySettings.RSInstalledDir;
                    mySettings.RescanOnStartup = checkRescanOnStartup.Checked;
                    mySettings.IncludeRS1DLCs = checkIncludeRS1DLC.Checked;
                    if (dgvSongs != null)
                    {
                        var settings = new RADataGridViewSettings();
                        var columns = dgvSongs.Columns;
                        if (columns.Count > 1)//HACK:dirt
                        {
                            for (int i = 0; i < columns.Count; i++)
                            {
                                settings.ColumnOrder.Add(new ColumnOrderItem
                                {
                                    ColumnIndex = i,
                                    DisplayIndex = columns[i].DisplayIndex,
                                    Visible = columns[i].Visible,
                                    Width = columns[i].Width
                                });
                            }
                            mySettings.ManagerGridSettings = settings;
                        }
                    }
                }
                mySettings.Serialze(fs);
                Log("Saved settings...");
                fs.Flush();
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
                        fs.Flush();
                    }
                    SaveSettingsToFile(path);
                }
                else
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var deserialized = fs.DeSerialize() as Settings;

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
                            checkIncludeRS1DLC.InvokeIfRequired(delegate
                            {
                                checkIncludeRS1DLC.Checked = mySettings.IncludeRS1DLCs;
                            });
                            checkEnableLogBaloon.InvokeIfRequired(delegate
                            {
                                checkEnableLogBaloon.Checked = mySettings.EnabledLogBaloon;
                            });
                            Log("Loaded settings...");
                        }
                        fs.Flush();
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
            string path = Constants.DefaultWorkingDirectory + @"\songs.bin";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                SongCollection.Serialze(fs);
                Log("Song collection saved...");
                fs.Flush();
            }
        }
        private void LoadSongCollectionFromFile()
        {
            string path = Constants.DefaultWorkingDirectory + @"\songs.bin";
            if (!File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
                {
                    BackgroundScan();
                    Log("Song collection file created...");
                    fs.Flush();
                }
                SaveSettingsToFile(path);
            }
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var songs = fs.DeSerialize() as BindingList<SongData>;
                if (songs != null)
                {
                    SongCollection = songs;
                    Log("Song collection loaded...");
                    fs.Flush();
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

            btnBatchRenamer.InvokeIfRequired(delegate
            {
                btnBatchRenamer.Enabled = !btnBatchRenamer.Enabled;
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

            btnDeleteDupeSong.InvokeIfRequired(delegate
            {
                btnDeleteDupeSong.Enabled = !btnDeleteDupeSong.Enabled;
            });

            btnDisableEnableSongs.InvokeIfRequired(delegate
            {
                btnDisableEnableSongs.Enabled = !btnDisableEnableSongs.Enabled;
            });

            btnExportSongList.InvokeIfRequired(delegate
            {
                btnExportSongList.Enabled = !btnExportSongList.Enabled;
            });

            btnBackupSelectedDLCs.InvokeIfRequired(delegate
            {
                btnBackupSelectedDLCs.Enabled = !btnBackupSelectedDLCs.Enabled;
            });

            radioBtn_ExportToBBCode.InvokeIfRequired(delegate
            {
                radioBtn_ExportToBBCode.Enabled = !radioBtn_ExportToBBCode.Enabled;
            });

            radioBtn_ExportToCSV.InvokeIfRequired(delegate
            {
                radioBtn_ExportToCSV.Enabled = !radioBtn_ExportToCSV.Enabled;
            });

            radioBtn_ExportToHTML.InvokeIfRequired(delegate
            {
                radioBtn_ExportToHTML.Enabled = !radioBtn_ExportToHTML.Enabled;
            });

            checkIncludeRS1DLC.InvokeIfRequired(delegate
            {
                checkIncludeRS1DLC.Enabled = !checkIncludeRS1DLC.Enabled;
            });

            linkLblSelectAll.InvokeIfRequired(delegate
            {
                linkLblSelectAll.Enabled = !linkLblSelectAll.Enabled;
            });

            link_MainClearResults.InvokeIfRequired(delegate
            {
                link_MainClearResults.Enabled = !link_MainClearResults.Enabled;
            });

            lbl_ExportTo.InvokeIfRequired(delegate
            {
                lbl_ExportTo.Enabled = !lbl_ExportTo.Enabled;
            });
        }

        private static List<string> FilesList(string path, bool includeRS1Pack = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("<Error>: No path provided for file scanning");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var files = Directory.EnumerateFiles(path, "*_p.psarc", SearchOption.AllDirectories).ToList();
            files.AddRange(Directory.EnumerateFiles(path, "*_p.disabled.psarc", SearchOption.AllDirectories).ToList());
            if (!includeRS1Pack)
            {
                files = files.Where(file => !file.Contains(rscompatibility)).ToList();
            }
            return files;
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
                    appVersion = info.MinimumRequiredVersion;

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
            string test = String.Empty;
            const string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
            const string rsX64Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            test = Registry.GetValue(rsX64Path, "installdir", null).ToString();
            if (!String.IsNullOrEmpty(test))
                return test;
            test = Registry.GetValue(rsX64Steam, "InstallLocation", null).ToString();
            return test;
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
            var results = SongCollection.Where(x =>
                x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) ||
                x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null &&
                x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) || (x.IgnitionID != null
                && x.IgnitionID.ToLower().Contains(criteria.ToLower())))
            ).ToList();

            SortedSongCollection = SongCollection.Where(x =>
                x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) ||
                x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null &&
                x.IgnitionAuthor.ToLower().Contains(criteria.ToLower())) || (x.IgnitionID != null &&
                x.IgnitionID.ToLower().Contains(criteria.ToLower()))
            ).ToList();

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
                var path = selectedRow.Cells["Path"].Value.ToString();

                var song =
                    SongCollection.FirstOrDefault(x => x.Song == title && x.Album == album && x.Artist == artist && x.Path == path);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
        }

        #region GUIEventHandlers
        #region DataGridView events
        private void dgvSongs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (row.Selected)
                    {
                        if (row.Cells["colSelect"].Value != null && (bool)row.Cells["colSelect"].Value)
                        {
                            row.Cells["colSelect"].Value = false;
                        }
                        else
                        {
                            row.Cells["colSelect"].Value = true;
                        }
                    }
                }
            }
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
        private void dgvSongs_DataSourceChanged(object sender, EventArgs e)
        {
            var dataGridViewColumn = ((DataGridView)sender).Columns["Preview"];
            if (dataGridViewColumn != null && dataGridViewColumn.Visible)
                dataGridViewColumn.Visible = false;

            PopulateColumnList();
        }
        private void dgvSongs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) //if it's not header
                ShowSongInfo();
        }
        private void dgvSongs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvSongs.DataSource != null)
            {
                int scrollHorizontalOffset = 0;
                int scrollVerticalOffset = 0;
                BindingSource bs = new BindingSource { DataSource = SongCollection };
                var songsToShow = SortedSongCollection;

                Dictionary<SongData, Color> currentRows = new Dictionary<SongData, Color>();

                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (row.DefaultCellStyle.BackColor != Color.White || row.DefaultCellStyle.BackColor != Color.Gray)
                        currentRows.Add((SongData)row.DataBoundItem, row.DefaultCellStyle.BackColor);
                }

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
                            bs.DataSource =
                                songsToShow.OrderByDescending(
                                    song =>
                                        DateTime.ParseExact(song.Updated, "M-d-y H:m",
                                            System.Globalization.CultureInfo.InvariantCulture));
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource =
                                songsToShow.OrderBy(
                                    song =>
                                        DateTime.ParseExact(song.Updated, "M-d-y H:m",
                                            System.Globalization.CultureInfo.InvariantCulture));
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
                    case "Version":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.Version);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.Version);
                            sortDescending = true;
                        }
                        break;
                    case "ToolkitVer":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.ToolkitVer);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.ToolkitVer);
                            sortDescending = true;
                        }
                        break;
                    case "Path":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.Path);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.Path);
                            sortDescending = true;
                        }
                        break;
                    case "FileName":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.FileName);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.FileName);
                            sortDescending = true;
                        }
                        break;
                    case "SongYear":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.SongYear);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.SongYear);
                            sortDescending = true;
                        }
                        break;
                    case "IgntionVersion":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionVersion);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.IgnitionVersion);
                            sortDescending = true;
                        }
                        break;
                    case "IgnitionID":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionID);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.IgnitionID);
                            sortDescending = true;
                        }
                        break;
                    case "IgnitionUpdated":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.Updated);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.Updated);
                            sortDescending = true;
                        }
                        break;
                    case "IgnitionAuthor":
                        if (sortDescending)
                        {
                            bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionAuthor);
                            sortDescending = false;
                        }
                        else
                        {
                            bs.DataSource = songsToShow.OrderBy(song => song.IgnitionAuthor);
                            sortDescending = true;
                        }
                        break;
                }
                scrollHorizontalOffset = dgvSongs.HorizontalScrollingOffset;
                scrollVerticalOffset = dgvSongs.VerticalScrollingOffset;
                dgvSongs.DataSource = bs;
                dgvSongs.HorizontalScrollingOffset = scrollHorizontalOffset;

                if (scrollVerticalOffset != 0)
                {
                    PropertyInfo verticalOffset = dgvSongs.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    verticalOffset.SetValue(this.dgvSongs, scrollVerticalOffset, null);
                }


                foreach (KeyValuePair<SongData, Color> row in currentRows)
                {
                    dgvSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == row.Key).DefaultCellStyle.BackColor = row.Value;
                }
            }
        }
        #endregion
        #region Link events
        private void lnkAboutCF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/");
        }

        private void link_CFManager_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/forum/81-customsforge-song-manager/");
        }
        private void linkOpenCFHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/");
        }

        private void linkOpenIgnition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/");
        }

        private void linkOpenOldSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://search.customsforge.com/");
        }

        private void linkOpenRequests_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://requests.customsforge.com/?b");
        }

        private void linkDonationsPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/donate/");
        }
        private void linkOpenCFVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/videos/");
        }

        private void linkCFFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/faq/");
        }
        private void linkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                row.Cells["colSelect"].Value = !allSelected;
            }
            allSelected = !allSelected;
        }
        private void link_MainClearResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgvSongs.InvokeIfRequired(delegate
            {
                Dictionary<SongData, Color> currentRows = new Dictionary<SongData, Color>();
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (row.DefaultCellStyle.BackColor != Color.White || row.DefaultCellStyle.BackColor != Color.Gray)
                        currentRows.Add((SongData)row.DataBoundItem, row.DefaultCellStyle.BackColor);
                }

                SortedSongCollection = SongCollection.ToList();
                dgvSongs.DataSource = new BindingSource().DataSource = SongCollection;

                foreach (KeyValuePair<SongData, Color> row in currentRows)
                {
                    dgvSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == row.Key).DefaultCellStyle.BackColor = row.Value;
                }
            });
            tbSearch.InvokeIfRequired(delegate { tbSearch.Text = ""; });
        }
        private void link_LovromanProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/43194-lovroman/");
        }
        private void link_DarjuszProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/5299-darjusz/");
        }
        private void link_Alex360Profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/20759-zerkz/");
        }
        private void link_UnleashedProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/1-unleashed2k/");
        }
        private void link_ForgeOnProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/345-forgeon/");
        }
        #endregion
        #region Button events
        private void btnEnableColumns_Click(object sender, EventArgs e)
        {
            dgvSongs.InvokeIfRequired(delegate
            {
                foreach (ListViewItem column in listDisabledColumns.CheckedItems)
                {
                    bool visible = false;
                    if (column.SubItems[1].Text == "Select")
                    {
                        visible = dgvSongs.Columns["colSelect"].Visible;
                        dgvSongs.Columns["colSelect"].Visible = !visible;
                        column.SubItems[2].Text = !visible ? "Yes" : "No";
                    }
                    else
                    {
                        visible = dgvSongs.Columns[column.SubItems[1].Text].Visible;
                        dgvSongs.Columns[column.SubItems[1].Text].Visible = !visible;
                        column.SubItems[2].Text = !visible ? "Yes" : "No";
                    }
                    column.Checked = false;
                }
            });
        }
        private void btnExportSongList_Click(object sender, EventArgs e)
        {
            if (radioBtn_ExportToBBCode.Checked)
                SongListToBBCode();
            else if (radioBtn_ExportToHTML.Checked)
                SongListToHTML();
            else if (radioBtn_ExportToCSV.Checked)
                SongListToCSV();
            else
                MessageBox.Show("Export format not selected", "Please select export format!");
        }
        private void btnDisableEnableSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                var cell = (DataGridViewCheckBoxCell)row.Cells["colSelect"];

                if (cell != null && cell.Value != null && cell.Value.ToString().ToLower() == "true")
                {
                    var originalPath = row.Cells["Path"].Value.ToString();
                    if (!originalPath.Contains(rscompatibility + "disc"))
                    {
                        if (row.Cells["Enabled"].Value.ToString() == "Yes")
                        {
                            var disabledDLCPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledDLCPath);
                            row.Cells["Path"].Value = disabledDLCPath;
                            row.Cells["Enabled"].Value = "No";
                        }
                        else if (row.Cells["Enabled"].Value.ToString() == "No")
                        {
                            var enabledDLCPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledDLCPath);
                            row.Cells["Path"].Value = enabledDLCPath;
                            row.Cells["Enabled"].Value = "Yes";
                        }
                        cell.Value = "false";

                        numberOfDisabledDLC = SongCollection.Where(song => song.Enabled == "No").ToList().Count();
                        toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate.ToString() + " | Disabled DLC:" + numberOfDisabledDLC.ToString();
                    }
                    else
                    {
                        Log("This is Rocksmith 1 song. It can't be disabled at this moment. (You just can disable all of them!)");
                    }
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

        private void btnLaunchSteam_Click(object sender, EventArgs e)
        {
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                Process.Start("steam://rungameid/221680");
        }
        private void btnBackupRSProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.DefaultWorkingDirectory, timestamp);
                string profilePath = "";
                string steamUserdataPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) + @"\userdata";

                var subdirs = new DirectoryInfo(steamUserdataPath).GetDirectories("*", SearchOption.AllDirectories).ToArray();
                foreach (DirectoryInfo info in subdirs)
                {
                    if (info.FullName.Contains(@"221680\remote"))
                    {
                        profilePath = info.FullName;
                    }
                }
                if (File.Exists(profilePath))
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
        {//Same issue as with regular rescan..
            listDupeSongs.Items.Clear();
            DupeCollection.Clear();
            tpDuplicates.InvokeIfRequired(delegate
            {
                tpDuplicates.Text = "Duplicates(0)";
            });
            // btnRescan_Click(null, null);
            Rescan();
        }
        private void btnDeleteSongOne_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
            {
                for (int i = 0; i < listDupeSongs.Items.Count; i++)
                {
                    if (listDupeSongs.Items[i].Checked || listDupeSongs.Items[i].Selected)
                    {
                        try
                        {
                            File.Delete(DupeCollection[i].Path);
                            if (DupeCollection.Where(song => song.Song == DupeCollection[i].Song && song.Album == DupeCollection[i].Album).ToList().Count == 2)
                            {
                                DupeCollection.RemoveAll(song => song.Song == DupeCollection[i].Song && song.Album == DupeCollection[i].Album);
                                listDupeSongs.Items.Clear();
                                foreach (var song in DupeCollection)
                                {
                                    listDupeSongs.InvokeIfRequired(delegate
                                    {
                                        listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Artist, song.Song, song.Album, song.Path }));
                                    });
                                }
                            }
                            else
                            {
                                DupeCollection.RemoveAt(i);
                                listDupeSongs.Items.RemoveAt(i);
                            }

                            tpDuplicates.Text = "Duplicates(" + DupeCollection.Count.ToString() + ")";
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });
        }
        private void btnRescan_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(mySettings.RSInstalledDir))
            {
                MessageBox.Show("Please, make sure that you've got Rocksmith 2014 installed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //BackgroundScan();
                listDupeSongs.Items.Clear();
                DupeCollection.Clear();
                tpDuplicates.InvokeIfRequired(delegate
                {
                    tpDuplicates.Text = "Duplicates(0)";
                });
                Rescan();
            }
        }

        private void btnCheckAllForUpdates_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += checkAllForUpdates;
            toolStripStatusLabel_MainCancel.Visible = true;
            //toolStripStatusLabel_MainCancel.Click += delegate
            //{
            //    bWorker.CancelAsync();
            //    bWorker.Abort();
            //};
            bWorker.RunWorkerAsync();
        }
        private void btnBatchRenamer_Click(object sender, EventArgs e)
        {
            //frmRenamer renamer = new frmRenamer(myLog);
            //renamer.ShowDialog();
        }
        #endregion
        #region ToolStripMenuItem events
        private void deleteSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(SongCollection[dgvSongs.SelectedRows[0].Index].Path);
                SongCollection.RemoveAt(dgvSongs.SelectedRows[0].Index);
            }
            catch (IOException ex)
            {
                myLog.Write("<ERROR>:" + ex.Message, false);
            }
        }
        private void toolStripStatusLabel_ClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private void showDLCInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }
        private void openDLCPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvSongs.SelectedRows.Count == 1)
            {
                var song = GetSongByRow(dgvSongs.SelectedRows[0]);
                if (song != null)
                {
                    if (song.IgnitionID == null || song.IgnitionID == "No Results")
                        song.IgnitionID = Ignition.GetDLCInfoFromURL(song.GetInfoURL(), "id");
                    if (song.IgnitionID == null || song.IgnitionID == "No Results")
                        myLog.Write("<ERROR>: Song doesn't exist in Ignition anymore.");
                    else
                        Process.Start(Constants.DefaultDetailsURL + "/" + song.IgnitionID);
                }
            }
        }
        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += CheckForUpdatesEvent;
            bWorker.RunWorkerAsync();
        }
        private void toolStripStatusLabel_MainCancel_Click(object sender, EventArgs e)
        {
            bWorker.CancelAsync();
            bWorker.Abort();
            //bWorker.Dispose();
            //bWorker = null;
            toolStripStatusLabel_MainCancel.Visible = false;
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bWorker.IsBusy)
                bWorker.CancelAsync();
            Application.Exit();
        }
        private void getAuthorNameStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvSongs.InvokeIfRequired(delegate
            {
                if (dgvSongs.SelectedRows.Count > 0)
                {
                    UpdateAuthor(dgvSongs.SelectedRows[0]);
                }
            });
        }
        private void editDLCToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void openDLCLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = dgvSongs.SelectedRows[0].Cells["Path"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
        }
        #endregion
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
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbSearch.Text.Length > 0)// && e.KeyCode == Keys.Enter)
            {
                SearchDLC(tbSearch.Text);
            }
            else
            {
                dgvSongs.DataSource = new BindingSource().DataSource = SongCollection;
            }
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: if nothing changed, just close or serialize default settings.
            if (dgvSongs != null)
            {
                SaveSettingsToFile();
                SaveSongCollectionToFile();
            }
        }
        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
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
        }

        private void CheckForUpdatesEvent(object o, DoWorkEventArgs args)
        {
            dgvSongs.InvokeIfRequired(delegate
            {
                if (dgvSongs.SelectedRows.Count > 0)
                {
                    CheckRowForUpdate(dgvSongs.SelectedRows[0]);
                    SaveSongCollectionToFile();
                }
            });
        }
        #endregion
        void checkAllForUpdates(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(3000);
            counterStopwatch.Start();
            btnCheckAllForUpdates.InvokeIfRequired(delegate
            {
                btnCheckAllForUpdates.Enabled = false;
            });

            dgvSongs.InvokeIfRequired(delegate
            {
                if (!bWorker.CancellationPending)
                {
                    foreach (DataGridViewRow row in dgvSongs.Rows)
                    {
                        //string songname = row.Cells[3].Value.ToString();
                        if (!bWorker.CancellationPending)
                        {
                            DataGridViewRow currentRow = (DataGridViewRow)row;
                            if (!currentRow.Cells["FileName"].Value.ToString().Contains("rs1comp"))
                            {
                                CheckRowForUpdate(currentRow);
                            }
                        }
                        else
                        {
                            bWorker.Abort();
                        }
                    }
                }
                SaveSongCollectionToFile();
            });
            counterStopwatch.Stop();
        }

        private void UpdateAuthor(DataGridViewRow selectedRow)
        {
            var currentSong = GetSongByRow(selectedRow);
            if (currentSong != null)
            {
                currentSong.IgnitionAuthor = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "name");
                selectedRow.Cells["IgnitionAuthor"].Value = currentSong.IgnitionAuthor;
            }
        }
        #region Export SongList
        private void SongListToHTML()
        {
            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("<table>");
            sbTXT.AppendLine("<tr>");
            sbTXT.AppendLine("<th>Song</th><th>Artist</th><th>Album</th><th>Updated</th><th>Tuning</th><th>DD</th><th>Arangements</th><th>Author</th>");
            sbTXT.AppendLine("</tr>");

            if (checkedRows.Count == 0)
            {
                foreach (var song in SongCollection)
                {
                    sbTXT.AppendLine("<tr>");
                    if (song.Author == null)
                    {
                        sbTXT.AppendLine("<td>" + song.Song + "</td><td>" + song.Artist + "</td><td>" + song.Album + "</td><td>" + song.Updated + "</td><td>" + song.Tuning + "</td><td>" + song.DD.DifficultyToDD() + "</td><td>" + song.Arrangements + "</td>");
                    }
                    else
                    {
                        sbTXT.AppendLine("<td>" + song.Song + "</td><td>" + song.Artist + "</td><td>" + song.Album + "</td><td>" + song.Updated + "</td><td>" + song.Tuning + "</td><td>" + song.DD.DifficultyToDD() + "</td><td>" + song.Arrangements + "</td><td>" + song.Author + "</td>");
                    }
                    sbTXT.AppendLine("</tr>");
                }
            }
            else
            {
                foreach (var row in checkedRows)
                {
                    sbTXT.AppendLine("<tr>");
                    if (row.Cells["Author"].Value == null)
                    {
                        sbTXT.AppendLine("<td>" + row.Cells["Song"].Value + "</td><td>" + row.Cells["Artist"].Value + "</td><td>" + row.Cells["Album"].Value + "</td><td>" + row.Cells["Updated"].Value + "</td><td>" + row.Cells["Tuning"].Value + "</td><td>" + (row.Cells["DD"].Value == "0" ? "No" : "Yes") + "</td><td>" + row.Cells["Arrangements"].Value + "</td>");
                    }
                    else
                    {
                        sbTXT.AppendLine("<td>" + row.Cells["Song"].Value + "</td><td>" + row.Cells["Artist"].Value + "</td><td>" + row.Cells["Album"].Value + "</td><td>" + row.Cells["Updated"].Value + "</td><td>" + row.Cells["Tuning"].Value + "</td><td>" + (row.Cells["DD"].Value == "0" ? "No" : "Yes") + "</td><td>" + row.Cells["Arrangements"].Value + "</td><td>" + row.Cells["Author"].Value + "</td>");
                    }
                    sbTXT.AppendLine("</tr>");
                }
            }

            sbTXT.AppendLine("</table>");

            frmSongListExport FormSongListExport = new frmSongListExport();
            FormSongListExport.SongList = sbTXT.ToString();
            FormSongListExport.Text = "Song list to HTML";
            FormSongListExport.Show();
        }

        private void SongListToBBCode()
        {
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("Song - Artist, Album, Updated, Tuning, DD, Arangements, Author");
            sbTXT.AppendLine("[LIST=1]");

            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            if (checkedRows.Count == 0)
            {
                foreach (var song in SongCollection)
                {
                    if (song.Author == null)
                    {
                        sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + song.DD.DifficultyToDD() + ", " + song.Arrangements + "[/*]");
                    }
                    else
                    {
                        sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + song.DD.DifficultyToDD() + ", " + song.Arrangements + ", " + song.Author + "[/*]");
                    }
                }
            }
            else
            {
                foreach (var row in checkedRows)
                {
                    if (row.Cells["Author"].Value == null)
                    {
                        sbTXT.AppendLine("[*]" + row.Cells["Song"].Value + " - " + row.Cells["Artist"].Value + ", " + row.Cells["Album"].Value + ", " + row.Cells["Updated"].Value + ", " + row.Cells["Tuning"].Value + ", " + row.Cells["DD"].Value == "0" ? "No" : "Yes" + ", " + row.Cells["Arrangements"].Value + "[/*]");
                    }
                    else
                    {
                        sbTXT.AppendLine("[*]" + row.Cells["Song"].Value + " - " + row.Cells["Artist"].Value + ", " + row.Cells["Album"].Value + ", " + row.Cells["Updated"].Value + ", " + row.Cells["Tuning"].Value + ", " + row.Cells["DD"].Value + ", " + row.Cells["Arrangements"].Value + ", " + row.Cells["Author"].Value);
                    }
                }
            }

            sbTXT.AppendLine("[/LIST]");

            frmSongListExport FormSongListExport = new frmSongListExport();
            FormSongListExport.SongList = sbTXT.ToString();
            FormSongListExport.Text = "Song list to BBCode";
            FormSongListExport.Show();
        }

        private void SongListToCSV()
        {
            var sbCSV = new StringBuilder();
            string path = Constants.DefaultWorkingDirectory + @"\SongListCSV.csv";
            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
            sfdSongListToCSV.FileName = "SongListCSV";

            if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
            {
                path = sfdSongListToCSV.FileName;
            }

            sbCSV.AppendLine(@"sep=;");
            sbCSV.AppendLine("Artist;Song;Album;Year;Tuning;Arrangements");

            if (checkedRows.Count == 0)
            {
                foreach (var song in SongCollection)
                {
                    if (song.Author == null)
                    {
                        sbCSV.AppendLine(song.Song + ";" + song.Artist + ";" + song.Album + ";" + song.Updated + ";" + song.Tuning + ";" + song.DD.DifficultyToDD() + ";" + song.Arrangements);
                    }
                    else
                    {
                        sbCSV.AppendLine(song.Song + ";" + song.Artist + ";" + song.Album + ";" + song.Updated + ";" + song.Tuning + ";" + song.DD.DifficultyToDD() + ";" + song.Arrangements + ";" + song.Author);
                    }
                }
            }
            else
            {
                foreach (var row in checkedRows)
                {
                    if (row.Cells["Author"].Value == null)
                    {
                        sbCSV.AppendLine(row.Cells["Song"].Value + ";" + row.Cells["Artist"].Value + ";" + row.Cells["Album"].Value + ";" + row.Cells["Updated"].Value + ";" + row.Cells["Tuning"].Value + ";" + row.Cells["DD"].Value == "0" ? "No" : "Yes" + ";" + row.Cells["Arrangements"].Value);
                    }
                    else
                    {
                        sbCSV.AppendLine(row.Cells["Song"].Value + ";" + row.Cells["Artist"].Value + ";" + row.Cells["Album"].Value + ";" + row.Cells["Updated"].Value + ";" + row.Cells["Tuning"].Value + ";" + row.Cells["DD"].Value + ";" + row.Cells["Arrangements"].Value + ";" + row.Cells["Author"].Value);
                    }
                }
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
        #endregion
        private void CheckRowForUpdate(DataGridViewRow dataGridViewRow)
        {
            if (!bWorker.CancellationPending)
            {
                var currentSong = GetSongByRow(dataGridViewRow);
                if (currentSong != null)
                {
                    //currentSong.IgnitionVersion = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "version");
                    string url = currentSong.GetInfoURL();
                    string response = "";
                    string cfUrl = "";
                    var client = new WebClient();
                    int version = 0;

                    client.DownloadStringCompleted += (sender, e) =>
                    {
                        if (!bWorker.CancellationPending)
                        {
                            response = e.Result;

                            currentSong.IgnitionID = Ignition.GetDLCInfoFromResponse(response, "id");
                            currentSong.IgnitionUpdated = Ignition.GetDLCInfoFromResponse(response, "updated");
                            currentSong.IgnitionVersion = Ignition.GetDLCInfoFromResponse(response, "version");
                            currentSong.IgnitionAuthor = Ignition.GetDLCInfoFromResponse(response, "name");

                            if (!bWorker.CancellationPending)
                            {
                                dataGridViewRow.Cells["IgnitionVersion"].Value = currentSong.IgnitionVersion;
                            }

                            if (int.TryParse(currentSong.Version, out version))
                            {
                                currentSong.Version += ".0";
                            }

                            if (int.TryParse(currentSong.IgnitionVersion, out version))
                            {
                                currentSong.IgnitionVersion += ".0";
                            }

                            if (currentSong.IgnitionVersion == "No Results")
                            {
                                dataGridViewRow.DefaultCellStyle.BackColor = Color.OrangeRed;
                                myLog.Write(
                                    string.Format(
                                        "<ERROR>: Song \"{0}\" from \"{1}\" album by {2} not found in ignition.",
                                        currentSong.Song, currentSong.Album, currentSong.Author), false);
                            }
                            else if (currentSong.Version == "N/A")
                            {
                                //TODO: Check for updates by release/update date
                            }
                            else if (currentSong.IgnitionVersion != currentSong.Version)
                            {
                                dataGridViewRow.DefaultCellStyle.BackColor = Color.Gold;
                                myLog.Write(
                                    string.Format(
                                        "Update found for \"{0}\" from \"{1}\" album by {2}. Local version: {3}, Ignition version: {4} ",
                                        currentSong.Song, currentSong.Album, currentSong.Author, currentSong.Version,
                                        currentSong.IgnitionVersion), false);

                                numberOfDLCPendingUpdate++;
                                toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate.ToString() + " | Disabled DLC:" + numberOfDisabledDLC.ToString();

                                cfUrl = Constants.DefaultCFSongUrl + currentSong.Song.Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "-") + "-r" + currentSong.IgnitionID;

                                if (!OutdatedSongList.ContainsKey(cfUrl))
                                {
                                    OutdatedSongList.Add(cfUrl, currentSong);
                                }
                            }
                        }
                        if (dataGridViewRow.Index == dgvSongs.Rows.Count - 1)
                        {
                            btnCheckAllForUpdates.InvokeIfRequired(delegate
                            {
                                btnCheckAllForUpdates.Enabled = true;
                            });
                            Log(string.Format("Finished update check. Task took {0}", counterStopwatch.Elapsed));
                            if (OutdatedSongList.Count != 0)
                            {
                                frmOutdatedSongs frmOutdated = new frmOutdatedSongs();
                                frmOutdated.OutdatedSongList = OutdatedSongList;
                                frmOutdated.Show();
                            }
                        }
                    };
                    if (!bWorker.CancellationPending)
                    {
                        client.DownloadStringAsync(new Uri(url));
                    }
                }
            }
        }

        private SongData GetSongByRow(DataGridViewRow dataGridViewRow)
        {
            return SongCollection.Distinct().FirstOrDefault(x => x.Song == dataGridViewRow.Cells["Song"].Value.ToString() && x.Artist == dataGridViewRow.Cells["Artist"].Value.ToString() && x.Album == dataGridViewRow.Cells["Album"].Value.ToString() && x.Path == dataGridViewRow.Cells["Path"].Value.ToString());
        }

        private void lnk_ReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var releaseNotes = new frmReleaseNotes();
            releaseNotes.ShowDialog();
        }

        private void lbl_UnleashedRole_Click(object sender, EventArgs e)
        {

        }

        private void btnEOFSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/eof");
        }

        private void btnRSTKSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.rscustom.net/");
        }


        private void checkEnableLogBaloon_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEnableLogBaloon.Checked)
                myLog.RemoveTargetNotifyIcon(notifyIcon_Main);
            else
                myLog.AddTargetNotifyIcon(notifyIcon_Main);
        }


        public void Rescan()
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();

            bWorker.DoWork += RescanSongs;
            bWorker.RunWorkerCompleted += RescanCompleted;
            bWorker.RunWorkerAsync();
            counterStopwatch.Start();
        }

        public void RescanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);

            numberOfDisabledDLC = SongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;

            toolStripStatusLabel_DisabledCounter.Visible = true;
            toolStripStatusLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate + " | Disabled DLC: " + numberOfDisabledDLC.ToString();

            counterStopwatch.Stop();
            Log(string.Format("Finished. Task took {0}", counterStopwatch.Elapsed));
        }

        public void RescanSongs(object sender, DoWorkEventArgs e)
        {
            var newFileList = FilesList(mySettings.RSInstalledDir + "\\dlc", mySettings.IncludeRS1DLCs);
            var oldFileList = CurrentFileList;
            string enabled = "";

            CurrentFileList = newFileList; //update the main file list

            foreach (string file in oldFileList) //check if there's a song that was deleted after the first scan
            {
                if (!newFileList.Any(file.Contains))
                {
                    dgvSongs.InvokeIfRequired(delegate
                    {
                        var deletedSong = SongCollection.FirstOrDefault(song => song.Path == file);
                        SongCollection.Remove(deletedSong);
                        songCounter--;
                    });
                }
            }

            foreach (string file in newFileList)//check if there's any new songs, if there is, add them to the GridView/SongCollection
            {
                if (!oldFileList.Any(file.Contains))
                {
                    enabled = file.Contains(".disabled.") ? "No" : "Yes";
                    parsePSARC(songCounter, enabled, file);
                }
            }
            PopulateDupeList();
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void backupDLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string backupPath = mySettings.RSInstalledDir + @"\backup";
            string fileName = "";
            string filePath = "";
            try
            {
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                filePath = SongCollection[dgvSongs.SelectedRows[0].Index].Path;
                fileName = Path.GetFileName(filePath);

                if (File.Exists(Path.Combine(backupPath, fileName)))
                    File.Delete(filePath);
                File.Copy(filePath, Path.Combine(backupPath, fileName));

            }
            catch (IOException ex)
            {
                Log("<ERROR>: " + ex.Message);
            }
        }

        private void btnBackupSelectedDLCs_Click(object sender, EventArgs e)
        {
            string backupPath = mySettings.RSInstalledDir + @"\backup";
            string fileName = "";
            string filePath = "";
            try
            {
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (row.Selected)
                    {
                        filePath = row.Cells["Path"].Value.ToString();
                        fileName = Path.GetFileName(filePath);

                        if (File.Exists(Path.Combine(backupPath, fileName)))
                            File.Delete(filePath);
                        File.Copy(filePath, Path.Combine(backupPath, fileName));
                        continue;
                    }

                    if (row.Cells["colSelect"].Value != null && Convert.ToBoolean(row.Cells["colSelect"].Value))
                    {
                        filePath = row.Cells["Path"].Value.ToString();
                        fileName = Path.GetFileName(filePath);

                        if (File.Exists(Path.Combine(backupPath, fileName)))
                            File.Delete(filePath);
                        File.Copy(filePath, Path.Combine(backupPath, fileName));
                    }
                }
            }
            catch (IOException ex)
            {
                Log("<ERROR>: " + ex.Message);
            }
        }
    }
}
