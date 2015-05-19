using System;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Controls;
using CustomsForgeManager_Winforms.lib;
using CustomsForgeManager_Winforms.Utilities;
using DLog.NET;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Antlr4.StringTemplate;
using Microsoft.VisualBasic;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmMain : Form
    {
        const string rscompatibility = "rs1compatibility";

        private bool allSelected = false;
        private bool sortDescending = true;
        private readonly DLogger myLog;
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
        public frmMain(DLogger myLog, Settings mySettings)
        {
            // TODO: Complete member initialization
            this.SetStyle(
                  ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint |
                  ControlStyles.DoubleBuffer, true);

            this.myLog = myLog;
            this.mySettings = mySettings;

            //this.renamerPropertyDataGridView.AutoGenerateColumns = true;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager_Winforms.Resources.renamer_properties.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    this.renamerPropertyDataSet = (System.Data.DataSet)JsonConvert.DeserializeObject(json, (typeof(System.Data.DataSet)));
                    this.renamerPropertyDataGridView.DataSource = this.renamerPropertyDataSet.Tables[0];
                };
            }
            catch (Exception e)
            {
                this.myLog.Write(e.Message);
            }

            InitializeComponent();
            this.renamerPropertyDataGridView.AutoGenerateColumns = true;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager_Winforms.Resources.renamer_properties.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    this.renamerPropertyDataSet = (System.Data.DataSet)JsonConvert.DeserializeObject(json, (typeof(System.Data.DataSet)));
                    this.renamerPropertyDataGridView.DataSource = this.renamerPropertyDataSet.Tables[0];

                };
            }
            catch (Exception e)
            {
                this.myLog.Write(e.Message);
            }

            Init();
        }
        private void Init()
        {
            #region Create directory structure if not exists

            string configFolderPath = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory;
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
                Log(string.Format("Working directory created at {0}", configFolderPath));
            }

            #endregion

            #region Logging setup

            //myLog.AddTargetFile(mySettings.LogFilePath);
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

            myLog.Write(GetRSTKLibVersion());

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

                    DateTime updateDateTime = new DateTime();
                    if (DateTime.TryParse(songData.Updated, out updateDateTime))
                        songData.Updated = updateDateTime.ToString("g");

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
            List<string> fileList = FilesList(mySettings.RSInstalledDir + "\\dlc", mySettings.IncludeRS1DLCs);
            //List<string> disabledFilesList = new List<string>(FilesList(mySettings.RSInstalledDir + "\\" + Constants.DefaultDisabledSubDirectory,mySettings.IncludeRS1DLCs));
            //filesList.AddRange(disabledFilesList);
            Log(String.Format("Raw songs count: {0}", fileList.Count));
            counterStopwatch.Start();
            foreach (string file in fileList)
            {
                if (!bWorker.CancellationPending)
                {
                    Progress(songCounter++ * 100 / fileList.Count);

                    enabled = file.Contains(".disabled.") ? "No" : "Yes";
                    parsePSARC(songCounter, enabled, file);
                    CurrentFileList.Add(file);
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
                        listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Enabled, song.Artist, song.Song, song.Album, song.Updated, song.Path }));
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
            mySettings.LogFilePath = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + CustomsForgeManager_Winforms.Utilities.Constants.DefaultLogName;
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
                path = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + "\\settings.bin";

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
                    mySettings.EnabledLogBaloon = checkEnableLogBaloon.Checked;
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
                mySettings.Serialize(fs);
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
                    path = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + "\\settings.bin";
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
            catch (Exception ex)
            {
                Log(string.Format("<Error>: {0}", ex.Message));
            }
        }
        #endregion

        private void SaveSongCollectionToFile()
        {
            string path = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + @"\songs.bin";
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            {
                SongCollection.Serialize(fs);
                CurrentFileList.Serialize(fs);
                Log("Song collection saved...");
                fs.Flush();
            }
        }
        private void LoadSongCollectionFromFile()
        {
            string path = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + @"\songs.bin";
            try
            {
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
                    var songs = (BindingList<SongData>)fs.DeSerialize();
                    var fileList = (List<string>)fs.DeSerialize();
                    if (fileList != null)
                    {
                        CurrentFileList = fileList;
                    }
                    if (songs != null)
                    {
                        SongCollection = songs;
                        Log("Song collection loaded...");
                        fs.Flush();
                        PopulateDataGridView();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Application couldn't read song collection file and it has to be deleted. Restart is needed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                File.Delete(path);
            }
        }

        private void RestartApp()
        {
            Process.Start(Application.ExecutablePath);
            Process.GetCurrentProcess().Kill();
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

            checkEnableLogBaloon.InvokeIfRequired(delegate
            {
                checkEnableLogBaloon.Enabled = !checkEnableLogBaloon.Enabled;
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
                if (toolStripProgressBarMain.ProgressBar != null && value <= 100)
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
                x.Artist.ToLower().Contains(criteria.ToLower()) ||
                x.Album.ToLower().Contains(criteria.ToLower()) ||
                x.Song.ToLower().Contains(criteria.ToLower()) ||
                x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Author.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower())) ||
                x.SongYear.Contains(criteria) ||
                x.Path.ToLower().Contains(criteria.ToLower())

                )
            ).ToList();

            SortedSongCollection = SongCollection.Where(x =>
                x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) ||
                x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null &&
                x.IgnitionAuthor.ToLower().Contains(criteria.ToLower())) || (x.IgnitionID != null &&
                x.IgnitionID.ToLower().Contains(criteria.ToLower()) || x.SongYear.Contains(criteria) ||
                x.Path.ToLower().Contains(criteria.ToLower()))
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
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
                {
                    foreach (DataGridViewRow row in dgvSongs.Rows)
                    {
                        row.Cells["colSelect"].Value = !allSelected;
                    }
                };
            bWorker.RunWorkerCompleted += (se, ev) =>
                {
                    allSelected = !allSelected;
                };
            bWorker.RunWorkerAsync();
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
        private void btnDisableEnableSong_Click(object sender, EventArgs e)
        {
            listDupeSongs.InvokeIfRequired(delegate
            {
                try
                {
                    for (int i = 0; i < listDupeSongs.Items.Count; i++)
                    {
                        if (listDupeSongs.Items[i].Checked || listDupeSongs.Items[i].Selected)
                        {
                            string songPath = listDupeSongs.Items[i].SubItems[6].Text;
                            if (songPath.Contains("_p.disabled.psarc"))
                            {
                                string enabledSongPath = songPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(songPath, enabledSongPath);
                                listDupeSongs.Items[i].SubItems[1].Text = "Yes";
                                listDupeSongs.Items[i].SubItems[6].Text = enabledSongPath;
                            }
                            else
                            {
                                string disabledSongPath = songPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(songPath, disabledSongPath);
                                listDupeSongs.Items[i].SubItems[1].Text = "No";
                                listDupeSongs.Items[i].SubItems[6].Text = disabledSongPath;
                            }
                        }
                    }
                }
                catch (IOException)
                {
                    myLog.Write("<ERROR>: Unable to disable the song(s)!");
                }
            });
        }
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
                        toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate.ToString() + " | Disabled DLC: " + numberOfDisabledDLC.ToString();
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
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory, timestamp);
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
            if (CurrentFileList.Count > 0)
            {
                if (String.IsNullOrEmpty(mySettings.RSInstalledDir))
                {
                    MessageBox.Show("Please, make sure that you've got Rocksmith 2014 installed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    toolStripStatusLabel_MainCancel.Visible = true;
                    listDupeSongs.Items.Clear();
                    DupeCollection.Clear();
                    tpDuplicates.InvokeIfRequired(delegate
                    {
                        tpDuplicates.Text = "Duplicates(0)";
                    });
                    BackgroundScan();
                }
            }
            else
            {
                BackgroundScan();
            }
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
            if (CurrentFileList.Count > 0)
            {
                if (String.IsNullOrEmpty(mySettings.RSInstalledDir))
                {
                    MessageBox.Show("Please, make sure that you've got Rocksmith 2014 installed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    CurrentFileList.Clear();
                    toolStripStatusLabel_MainCancel.Visible = true;
                    listDupeSongs.Items.Clear();
                    DupeCollection.Clear();
                    tpDuplicates.InvokeIfRequired(delegate
                    {
                        tpDuplicates.Text = "Duplicates(0)";
                    });
                    BackgroundScan();
                }
            }
            else
            {
                BackgroundScan();
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

        #endregion
        #region ToolStripMenuItem events
        private void deleteSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(SongCollection[dgvSongs.SelectedRows[0].Index].Path);
                SongCollection.RemoveAt(dgvSongs.SelectedRows[0].Index);
                dgvSongs.Rows.RemoveAt(dgvSongs.SelectedRows[0].Index);
            }
            catch (IOException ex)
            {
                myLog.Write("<ERROR>:" + ex.Message);
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
                        Process.Start(CustomsForgeManager_Winforms.Utilities.Constants.DefaultDetailsURL + "/" + song.IgnitionID);
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
            string path = CustomsForgeManager_Winforms.Utilities.Constants.DefaultWorkingDirectory + @"\SongListCSV.csv";
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
                                //myLog.Write(
                                //    string.Format(
                                //        "<ERROR>: Song \"{0}\" from \"{1}\" album by {2} not found in ignition.",
                                //        currentSong.Song, currentSong.Album, currentSong.Author), false);
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
                                        currentSong.IgnitionVersion));

                                numberOfDLCPendingUpdate++;
                                toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate.ToString() + " | Disabled DLC:" + numberOfDisabledDLC.ToString();

                                cfUrl = CustomsForgeManager_Winforms.Utilities.Constants.DefaultCFSongUrl + currentSong.Song.Replace("'", "").Replace("(", "").Replace(")", "").Replace(" ", "-") + "-r" + currentSong.IgnitionID;

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
            counterStopwatch.Reset();
            counterStopwatch.Start();
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();

            bWorker.DoWork += RescanSongs;
            bWorker.RunWorkerCompleted += RescanCompleted;
            bWorker.RunWorkerAsync();
        }

        public void RescanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);

            numberOfDisabledDLC = SongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;

            toolStripStatusLabel_MainCancel.Visible = false;

            toolStripStatusLabel_DisabledCounter.Text = "Outdated: " + numberOfDLCPendingUpdate + " | Disabled DLC: " + numberOfDisabledDLC.ToString();

            counterStopwatch.Stop();
            Log(string.Format("Finished rescanning. Task took {0}", counterStopwatch.Elapsed));
        }

        public void RescanSongs(object sender, DoWorkEventArgs e)
        {
            var newFileList = FilesList(mySettings.RSInstalledDir + "\\dlc", mySettings.IncludeRS1DLCs);
            var oldFileList = CurrentFileList;
            string enabled = "";

            CurrentFileList = newFileList; //update the main file list


            if (checkIncludeRS1DLC.Checked == false && SongCollection.Any(sng => sng.Path.Contains(rscompatibility)))
            {
                var rs1Songs = SongCollection.Where(sng => sng.Path.Contains(rscompatibility)).ToList();
                songCounter -= rs1Songs.Count;
                dgvSongs.InvokeIfRequired(delegate
                {
                    dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    foreach (var rs1Song in rs1Songs)
                    {
                        SongCollection.Remove(rs1Song);
                    }
                    dgvSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                });
            }

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
                if (file != rscompatibility)
                {
                    if (!oldFileList.Any(file.Contains))
                    {
                        dgvSongs.InvokeIfRequired(delegate
                        {
                            enabled = file.Contains(".disabled.") ? "No" : "Yes";
                            if (checkIncludeRS1DLC.Checked == false && !file.Contains(rscompatibility))
                                parsePSARC(songCounter, enabled, file);
                            else
                                parsePSARC(songCounter, enabled, file);
                        });
                    }
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

        private void renameAllButton_Click(object sender, EventArgs e)
        {
            SortedSongCollection = SongCollection.ToList();
            if (!renameTemplateTextBox.Text.Contains("<title>"))
            {
                MessageBox.Show("Rename Template requires <title> atleast once to prevent overwriting songs.");
                return;
            }
            if (SortedSongCollection == null || SortedSongCollection.Count == 0)
            {
                MessageBox.Show("Please scan in atleast one song.");
                return;
            }
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += doRenameSongs;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;

            bWorker.RunWorkerAsync();
        }

        //TODO: Refactor this somewhere else?
        private void renameSongs(List<SongData> songList, String templateString, Boolean deleteEmptyDirectories)
        {
            try
            {
                foreach (SongData data in songList)
                {
                    Template template = new Template(templateString);
                    template.Add("title", data.Song);
                    template.Add("artist", data.Artist);
                    template.Add("version", data.Version);
                    template.Add("author", data.Author);
                    template.Add("album", data.Album);
                    if ("Yes".Equals(data.DD))
                    {
                        template.Add("dd", "_dd");
                    }
                    else
                    {
                        template.Add("dd", "");
                    }

                    template.Add("year", data.SongYear);
                    template.Add("author", data.Updated);

                    String newFilePath = mySettings.RSInstalledDir + "\\dlc\\" + template.Render() + "_p.psarc";

                    string oldFilePath = data.Path;
                    FileInfo newFileInfo = new FileInfo(newFilePath);
                    System.IO.Directory.CreateDirectory(newFileInfo.Directory.FullName);
                    myLog.Write("Renaming/Moving:" + oldFilePath);

                    myLog.Write("---> " + newFilePath);
                    System.IO.File.Move(oldFilePath, newFilePath);
                }
                if (deleteEmptyDirectories)
                {
                    new DirectoryInfo(mySettings.RSInstalledDir + "\\dlc\\").DeleteEmptyDirs();
                }

            }
            //lazy exception catch for now. Future me will punch me for such astrocities, alas its a desktop app.
            catch (Exception e)
            {
                myLog.Write(e.Message);
            }

        }

        private void doRenameSongs(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
            {
                renameSongs(SortedSongCollection, renameTemplateTextBox.Text, deleteEmptyDirCheckBox.Checked);
            }
        }

        private void btnRequestSong_Click(object sender, EventArgs e)
        {
            Process.Start("http://requests.customsforge.com/");
        }

        private void btn_UploadCDLC_Click(object sender, EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/creators/submit");
        }
        #region Setlist Manager
        private bool DirOK()
        {
            if (mySettings.RSInstalledDir == "")
            {
                MessageBox.Show("Please fill RS path textbox!", "RS path empty!");
                return false;
            }
            else
            {
                if (!Directory.Exists(mySettings.RSInstalledDir))
                {
                    MessageBox.Show("Please fix RS path!", "RS Folder doesn't exist!");
                    return false;
                }
            }
            return true;
        }
        public void LoadSetlists()
        {
            string[] dirs = null;
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
            {
                if (DirOK())
                {
                    string dlcFolderPath = Path.Combine(mySettings.RSInstalledDir, "dlc");

                    if (Directory.Exists(dlcFolderPath))
                    {
                        dirs = Directory.GetDirectories(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*", SearchOption.TopDirectoryOnly);
                        foreach (var setlistPath in dirs)
                        {
                            bool setlistEnabled = true;
                            dgvSetlists.InvokeIfRequired(delegate
                            {
                                foreach (string song in Directory.GetFiles(setlistPath, "*_p.psarc*"))
                                {
                                    if (song.Contains(".disabled"))
                                        setlistEnabled = false;
                                }
                                if (setlistEnabled == false)
                                    dgvSetlists.Rows.Add(true, "No", Path.GetFileName(setlistPath.Replace("-disabled", "")));
                                else
                                    dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlistPath));
                            });
                        }

                        string[] filesInSetlist = null;
                        if (dirs.Length > 0 && dirs[0] != null)
                        {
                            filesInSetlist = Directory.GetFiles(dirs[0]);
                            string[] unsortedSongs = Directory.GetFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*_p.psarc*", SearchOption.TopDirectoryOnly);
                            string[] zipFiles = Directory.GetFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*zip", SearchOption.TopDirectoryOnly);

                            listDLCsInSetlist.InvokeIfRequired(delegate
                            {
                                foreach (string song in filesInSetlist)
                                    listDLCsInSetlist.Items.Add(Path.GetFileName(song));
                            });

                            dgvUnsortedDLCs.InvokeIfRequired(delegate
                            {
                                foreach (string song in unsortedSongs)
                                {
                                    if (!song.Contains("rs1"))
                                    {
                                        if (song.Contains(".disabled"))
                                            dgvUnsortedDLCs.Rows.Add(true, "No", Path.GetFileName(song.Replace(".disabled", "")));
                                        else
                                            dgvUnsortedDLCs.Rows.Add(false, "Yes", Path.GetFileName(song));
                                    }
                                }
                            });

                            dgvOfficialSongs.InvokeIfRequired(delegate
                            {
                                string cachePsarcPath = Path.Combine(mySettings.RSInstalledDir, "cache.psarc");
                                string cachePsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "disabled.cache.psarc.disabled");

                                string rs1PsarcPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.psarc");
                                string rs1PsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "disabled.rs1compatibilitydlc_p.psarc.disabled");

                                string rs1DLCPsarcPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.psarc");
                                string rs1DLCPsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "disabled.rs1compatibilitydisc_p.psarc.disabled");

                                if (File.Exists(cachePsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "cache.psarc");
                                else if (File.Exists(cachePsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "cache.psarc");

                                if (File.Exists(rs1PsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydlc_p.psarc");
                                else if (File.Exists(rs1PsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "rs1compatibilitydlc_p.psarc");

                                if (File.Exists(rs1DLCPsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                                else if (File.Exists(rs1DLCPsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "rs1compatibilitydisc_p.psarc");
                            });
                        }
                    }
                }
            };
            bWorker.RunWorkerAsync();
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
            {
                listDLCsInSetlist.InvokeIfRequired(delegate
                {
                    listDLCsInSetlist.Items.Clear();
                });
                string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", dgvSetlists.SelectedRows[0].Cells["colSetlist"].Value.ToString());

                foreach (string song in Directory.GetFiles(setlistPath))
                {
                    listDLCsInSetlist.InvokeIfRequired(delegate
                    {
                        listDLCsInSetlist.Items.Add(Path.GetFileName(song));
                    });
                }
            };

            bWorker.RunWorkerAsync();
        }
        private void btnLoadSetlists_Click(object sender, EventArgs e)
        {
            listDLCsInSetlist.Items.Clear();
            dgvSetlists.Rows.Clear();
            dgvOfficialSongs.Rows.Clear();
            dgvUnsortedDLCs.Rows.Clear();
            LoadSetlists();
        }

        private void btnRunRSWithSetlist_Click(object sender, EventArgs e)
        {
            string rs1MainPack = "";
            string rs1DLCPack = "";
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");

            List<string> rs1DLCFiles = Directory.EnumerateFiles(Path.Combine(tbRSPath.Text, "dlc"), "rs1compatibilitydlc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs1Files = Directory.EnumerateFiles(Path.Combine(tbRSPath.Text, "dlc"), "rs1compatibilitydisc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();

            frmComboBoxPopup comboPopup = new frmComboBoxPopup();

            if (rs1DLCFiles.Count > 0)
            {
                comboPopup.ComboBoxItems.Add("Select a setlist");

                foreach (string rs1DLCFile in rs1DLCFiles)
                    comboPopup.ComboBoxItems.Add(new FileInfo(rs1DLCFile).Directory.Name);

                comboPopup.LblText = "Select a RS1 DLC pack to restore from the selected setlist:";
                comboPopup.FrmText = "Duplicate RS1 DLC pack detected";
                comboPopup.BtnText = "OK";

                comboPopup.ShowDialog();

                rs1DLCPack = comboPopup.Combo.SelectedItem.ToString();

                if (rs1DLCPack != "Select a setlist")
                {
                    foreach (string rs1DLCFile in rs1DLCFiles)
                    {
                        if (Path.GetDirectoryName(rs1DLCFile) != Path.Combine(tbRSPath.Text, "dlc", rs1DLCPack))
                        {
                            File.Move(rs1DLCFile, rs1DLCFile.Replace("_p.psarc", "_p.disabled.psarc"));
                        }
                    }
                }
            }

            if (rs1Files.Count > 0)
            {
                comboPopup.ComboBoxItems.Clear();
                comboPopup.ComboBoxItems.Add("Select a setlist");

                foreach (string rs1File in rs1Files)
                    comboPopup.ComboBoxItems.Add(new FileInfo(rs1File).Directory.Name);

                comboPopup.LblText = "Select a RS1 pack to restore from the selected setlist:";
                comboPopup.FrmText = "Duplicate RS1 pack detected";
                comboPopup.BtnText = "OK";

                comboPopup.ShowDialog();

                rs1MainPack = comboPopup.Combo.SelectedItem.ToString();

                if (rs1MainPack != "Select a setlist")
                {
                    foreach (string rs1File in rs1Files)
                    {
                        if (Path.GetDirectoryName(rs1File) != Path.Combine(tbRSPath.Text, "dlc", rs1MainPack))
                        {
                            File.Move(rs1File, rs1File.Replace("_p.psarc", "_p.disabled.psarc"));
                        }
                    }
                }
            }

            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                Process.Start("steam://rungameid/221680");
        }
        #region Setlists
        private void btnCreateNewSetlist_Click(object sender, EventArgs e)
        {
            string setlistName = Interaction.InputBox("Please enter setlist name", "Setlist name");
            try
            {
                if (DirOK())
                {
                    string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);
                    if (!Directory.Exists(setlistPath))
                    {
                        Directory.CreateDirectory(setlistPath);
                        if (Directory.Exists(setlistPath))
                            dgvSetlists.Rows.Add(false, "Yes", setlistName);
                    }
                    else
                        MessageBox.Show("Setlist with this name already exists!", "Setlist already exists");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to create a new setlist! Error: \n\n" + ex.ToString());
            }
        }

        private void btnRemoveSongsFromSetlist_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listDLCsInSetlist.SelectedItems.Count; i++)
                {
                    if (DirOK() && listDLCsInSetlist.SelectedItems.Count > 0)
                    {
                        string song = listDLCsInSetlist.SelectedItems[i].ToString();

                        string dlcFolderPath = Path.Combine(mySettings.RSInstalledDir, "dlc");
                        string setlistSongPath = Path.Combine(dlcFolderPath, dgvSetlists.SelectedRows[0].Cells["colSetlist"].Value.ToString());
                        string songPath = Path.Combine(setlistSongPath, song);
                        string finalSongPath = Path.Combine(dlcFolderPath, song).Replace("_p.disabled.psarc", "_p.psarc");

                        if (File.Exists(finalSongPath))
                            File.Delete(finalSongPath);
                        File.Move(songPath, finalSongPath);

                        listDLCsInSetlist.Items.Remove(song);
                        dgvUnsortedDLCs.Rows.Add(false, "Yes", Path.GetFileName(finalSongPath));
                    }
                    else
                    {
                        MessageBox.Show("Please fill RS path textbox!s", "RS path empty!");
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n\n" + ex.ToString(), "IO Error");
            }
        }

        private void btnEnblDisbSelectedSetlist_Click(object sender, EventArgs e)
        {
            if (DirOK())
            {
                foreach (DataGridViewRow row in dgvSetlists.Rows)
                {
                    listDLCsInSetlist.Items.Clear();
                    if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                    {
                        string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());

                        foreach (string song in Directory.GetFiles(setlistPath))
                        {
                            string newPath = row.Cells["colSetlistEnabled"].Value == "Yes" ? song.Replace("_p.psarc", "_p.disabled.psarc") : song.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(song, newPath);

                            listDLCsInSetlist.Items.Add(Path.GetFileName(newPath));
                        }

                        if (row.Cells["colSetlistEnabled"].Value == "Yes")
                            row.Cells["colSetlistEnabled"].Value = "No";
                        else
                            row.Cells["colSetlistEnabled"].Value = "Yes";
                    }
                }
            }
        }

        private void btnDeleteSelectedSetlist_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvSetlists.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                    {
                        string songArchivePath = row.Cells["colSetlistSetlist"].Value.ToString();

                        if (mySettings.RSInstalledDir != "")
                            songArchivePath = Path.Combine(mySettings.RSInstalledDir, "dlc", songArchivePath);

                        if (Directory.Exists(songArchivePath))
                        {
                            if (checkDeleteSongsAndSetlists.Checked)
                            {
                                Directory.Delete(songArchivePath, true);
                                dgvSetlists.Rows.Remove(row);
                            }
                            else
                            {
                                string[] songs = Directory.GetFiles(songArchivePath);
                                foreach (string song in songs)
                                {
                                    if (song.Contains("dlc"))
                                        File.Move(Path.Combine(songArchivePath, song), Path.Combine(mySettings.RSInstalledDir, "dlc", song));
                                }
                                Directory.Delete(songArchivePath, true);
                                dgvSetlists.Rows.Remove(row);
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to delete the song archive! \n" + ex.ToString(), "IO Error");
            }
        }
        private void btnEnableAllSetlists_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
            {
                try
                {
                    if (DirOK())
                    {
                        foreach (DataGridViewRow row in dgvSetlists.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                            {
                                string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlistSetlist"].Value.ToString());
                                List<string> setlistDisabledSongs = Directory.GetFiles(setlistPath).Where(sng => sng.Contains(".disabled")).ToList();

                                foreach (string song in setlistDisabledSongs)
                                {
                                    File.Move(song, song.Replace("_p.psarc", "_p.disabled.psarc"));

                                    dgvSetlists.InvokeIfRequired(delegate
                                    {
                                        row.Cells["colSetlistEnabled"].Value = "Yes";
                                    });
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to enable all setlists! Error: \n\n" + ex.ToString());
                }
            };
            bWorker.RunWorkerAsync();
        }
        #endregion

        #region Unsorted songs
        private void btnMoveSongToSetlist_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvSetlists.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                    {
                        foreach (DataGridViewRow unsortedRow in dgvUnsortedDLCs.Rows)
                        {
                            if (DirOK())
                            {
                                if (unsortedRow.Selected || (bool)unsortedRow.Cells["colUnsortedSelect"].Value == true)
                                {
                                    string song = unsortedRow.Cells["colUnsortedSong"].Value.ToString();
                                    string songPath = Path.Combine(mySettings.RSInstalledDir, "dlc", song);
                                    string setlistSongPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());

                                    if (unsortedRow.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                                        song += ".disabled";

                                    if (row.Cells["colSetlistEnabled"].Value == "Yes")
                                    {
                                        if (File.Exists(Path.Combine(setlistSongPath, song.Replace(".disabled", ""))))
                                            File.Delete(Path.Combine(setlistSongPath, song.Replace(".disabled", "")));
                                        File.Move(songPath, Path.Combine(setlistSongPath, song.Replace(".disabled", "")));
                                    }
                                    else
                                    {
                                        if (unsortedRow.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                                        {
                                            if (File.Exists(Path.Combine(setlistSongPath, song)))
                                                File.Delete(Path.Combine(setlistSongPath, song));
                                            File.Move(songPath, Path.Combine(setlistSongPath, song));
                                        }
                                        else
                                        {
                                            if (File.Exists(Path.Combine(setlistSongPath, song + ".disabled")))
                                                File.Delete(Path.Combine(setlistSongPath, song + ".disabled"));
                                            File.Move(songPath, Path.Combine(setlistSongPath, song + ".disabled"));
                                        }
                                    }

                                    dgvUnsortedDLCs.Rows.Remove(unsortedRow);
                                    listDLCsInSetlist.Items.Add(song);
                                }
                            }
                        }
                        //   row.Selected = false;
                        //   row.Cells["colSelect"].Value = false;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n" + ex.ToString(), "IO Error");
            }
        }

        private void btnEnableDisableSelectedSongs_Click(object sender, EventArgs e)
        {
            try
            {
                if (DirOK())
                {
                    foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colUnsortedSelect"].Value) == true || row.Selected)
                        {
                            string songPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colUnsortedSong"].Value.ToString());
                            if (row.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                            {
                                File.Move(songPath.Replace("_p.psarc", "_p.disabled.psarc"), songPath);
                                row.Cells["colUnsortedEnabled"].Value = "Yes";
                            }
                            else
                            {
                                File.Move(songPath, songPath.Replace("_p.psarc", "_p.disabled.psarc"));
                                row.Cells["colUnsortedEnabled"].Value = "No";
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to disalbe/enable selected songs! Error: \n\n" + ex.ToString());
            }
        }

        private void btnDeleteSelectedSongs_Click(object sender, EventArgs e)
        {
            try
            {
                if (DirOK())
                {
                    foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colUnsortedSelect"].Value) == true || row.Selected)
                        {
                            string songPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colUnsortedSong"].Value.ToString());

                            if (row.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                                songPath.Replace("_p.psarc", "_p.disabled.psarc");

                            File.Delete(songPath);

                            if (!File.Exists(songPath))
                                dgvUnsortedDLCs.Rows.Remove(row);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to delete selected song(s)! Error: \n\n" + ex.ToString());
            }
        }
        #endregion

        #region Official song packs

        private void btnEnblDisblOfficialSongPack_Click(object sender, EventArgs e)
        {
            if (DirOK())
            {
                foreach (DataGridViewRow row in dgvOfficialSongs.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["colOfficialSelect"].Value) == true || row.Selected)
                    {
                        if (mySettings.RSInstalledDir != "" && !row.Cells["colOfficialSongPack"].Value.ToString().Contains("cache"))
                        {
                            string currentSongPackPath = "";
                            string finalSongPackPath = "";
                            string currentSPFileName = row.Cells["colOfficialSongPack"].Value.ToString();

                            if (row.Cells["colOfficialEnabled"].Value == "Yes")
                            {
                                if (currentSPFileName.Contains("cache"))
                                    currentSongPackPath = Path.Combine(mySettings.RSInstalledDir, currentSPFileName);
                                else
                                    currentSongPackPath = Path.Combine(mySettings.RSInstalledDir, "dlc", currentSPFileName);

                                finalSongPackPath = Path.Combine(mySettings.RSInstalledDir, "dlc", currentSPFileName.Replace("_p.psarc", "_p.disabled.psarc"));
                            }
                            else
                            {
                                if (currentSPFileName.Contains("cache"))
                                    finalSongPackPath = Path.Combine(mySettings.RSInstalledDir, currentSPFileName);
                                else
                                    finalSongPackPath = Path.Combine(mySettings.RSInstalledDir, "dlc", currentSPFileName);

                                currentSongPackPath = Path.Combine(mySettings.RSInstalledDir, "dlc", currentSPFileName.Replace("_p.psarc", "_p.disabled.psarc"));
                            }

                            try
                            {
                                File.Move(currentSongPackPath, finalSongPackPath);
                            }
                            catch (IOException ex)
                            {
                                MessageBox.Show("Unable to disable the offical song pack(s)! Error: \n\n" + ex.ToString());
                            }

                            row.Cells["colOfficialEnabled"].Value = row.Cells["colOfficialEnabled"].Value.ToString() == "Yes" ? "No" : "Yes";
                        }
                    }
                }
            }
        }

        private void btnSngPackToSetlist_Click(object sender, EventArgs e)
        {
            try
            {
                if (DirOK())
                {
                    foreach (DataGridViewRow row in dgvSetlists.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                        {

                            foreach (DataGridViewRow officialSPRow in dgvOfficialSongs.Rows)
                            {
                                if (officialSPRow.Selected || (bool)officialSPRow.Cells["colOfficialSelect"].Value == true)
                                {
                                    string songPack = officialSPRow.Cells["colOfficialSongPack"].Value.ToString();
                                    string songPackPath = Path.Combine(mySettings.RSInstalledDir, "dlc", songPack);
                                    string setlistSongPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                    string finalSongPackPath = Path.Combine(setlistSongPath, songPack);

                                    if (officialSPRow.Cells["colOfficialEnabled"].Value == "Yes")
                                    {
                                        if (File.Exists(finalSongPackPath))
                                            File.Delete(finalSongPackPath);
                                        File.Move(songPackPath, finalSongPackPath);
                                    }
                                    else
                                    {
                                        if (File.Exists(finalSongPackPath.Replace("_p.psarc", "_p.disabled.psarc")))
                                            File.Delete(finalSongPackPath.Replace("_p.psarc", "_p.disabled.psarc"));
                                        File.Move(songPackPath.Replace("_p.psarc", "_p.disabled.psarc"), finalSongPackPath.Replace("_p.psarc", "_p.disabled.psarc"));
                                    }

                                    dgvOfficialSongs.Rows.Remove(officialSPRow);
                                }
                            }
                        }
                        //   row.Selected = false;
                        //   row.Cells["colSelect"].Value = false;
                    }
                }
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n" + ex.ToString(), "IO Error");
            }
        }

        private void btnRestoreOfficialsBackup_Click(object sender, EventArgs e)
        {
            if (DirOK())
            {
                string backupPath = Path.Combine(mySettings.RSInstalledDir, "backup");
                string dlcPath = Path.Combine(mySettings.RSInstalledDir, "dlc");

                string rs1BackupPath = Path.Combine(backupPath, "rs1compatibilitydisc_p.psarc.backup");
                string rs1DLCBackupPath = Path.Combine(backupPath, "rs1compatibilitydlc_p.psarc.backup");
                string rs2014BackupPath = Path.Combine(backupPath, "cache.psarc.backup");

                string rs1FinalPath = Path.Combine(dlcPath, "rs1compatibilitydisc_p.psarc");
                string rs1DLCFinalPath = Path.Combine(dlcPath, "rs1compatibilitydlc_p.psarc");
                string rs2014FinalPath = Path.Combine(mySettings.RSInstalledDir, "cache.psarc");

                try
                {
                    if (File.Exists(rs1BackupPath))
                    {
                        File.Copy(rs1BackupPath, rs1FinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("disc")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                    }
                    if (File.Exists(rs1DLCBackupPath))
                    {
                        File.Copy(rs1DLCBackupPath, rs1DLCFinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("dlc")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydlc_p.psarc");
                    }
                    if (File.Exists(rs2014BackupPath))
                    {
                        File.Copy(rs2014BackupPath, rs2014FinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("cache")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "cache.psarc");
                    }
                    if (File.Exists(rs1FinalPath) || File.Exists(rs1DLCFinalPath) || File.Exists(rs2014FinalPath))
                        MessageBox.Show("Backup restored!", "Done");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to restore backup! Error:\n\n" + ex.Message.ToString(), "Error");
                }

                foreach (DataGridViewRow row in dgvOfficialSongs.Rows)
                    row.Cells["colOfficialEnabled"].Value = "Yes";

                MessageBox.Show("Official songs zipped!");
            }
        }
        #endregion
        #endregion
    }
}
