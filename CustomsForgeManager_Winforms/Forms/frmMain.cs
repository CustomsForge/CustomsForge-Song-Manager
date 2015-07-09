using System;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Controls;
using CustomsForgeManager_Winforms.Models;
using CustomsForgeManager_Winforms.lib;
using Antlr4.StringTemplate;
using CustomsForgeManager_Winforms.lib.RS;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.VisualBasic;
using Constants = CustomsForgeManager_Winforms.lib.Constants;
using Extensions = CustomsForgeManager_Winforms.lib.Extensions;
using DLogNet;
using zlib;

namespace CustomsForgeManager_Winforms.Forms
{
    //
    // TODO: add a sepereate user controls for each tab control in frmMain
    // TODO: improve code sepeartion, flexiblity and efficiency
    // AVOID USE OF: simple string concatinations "+" as this may cause cross/multi platform issues 
    //
    public partial class frmMain : Form
    {
        private const string rscompatibility = "rs1compatibility";

        private readonly DLogger myLog;
        private List<string> CurrentFileList = new List<string>();
        private List<SongData> DupeCollection = new List<SongData>();
        private Dictionary<string, SongData> OutdatedSongList = new Dictionary<string, SongData>();
        private BindingList<SongData> SongCollection = new BindingList<SongData>();
        private List<SongData> SortedSongCollection = new List<SongData>();
        private bool allSelected = false;
        private Version appVersion;
        private Stopwatch counterStopwatch = new Stopwatch();
        private string customUserdataPath = "";
        private Settings mySettings;
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private int songCounter = 1;
        private bool sortDescending = true;

        private frmMain()
        {
            throw new Exception("Improper constructor used");
        }

        public frmMain(DLogger myLog, Settings mySettings)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            this.myLog = myLog;
            this.mySettings = mySettings;
            InitializeComponent();
            this.Text = String.Format("CustomsForge Song Manager (v{0})", Constants.CSMVersion());
            ApplicationInitialize();
        }

        private void ApplicationInitialize()
        {
            // Hide main dgvSongs until load completes
            dgvSongs.Visible = false;

            // Create directory structure if not exists
            string configFolderPath = Constants.DefaultWorkingDirectory;
            if (!Directory.Exists(configFolderPath))
            {
                Directory.CreateDirectory(configFolderPath);
                Log(string.Format("Working directory created at {0}", configFolderPath));
            }

            // Logging setup ... also see program.cs
            myLog.AddTargetTextBox(tbLog);
            myLog.AddTargetNotifyIcon(notifyIcon_Main);

            // Get version information
            string cfm_version = String.Format("Version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
            if (ApplicationDeployment.IsNetworkDeployed)
                Log(string.Format("Application loaded, using version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion), 100);

            if (appVersion != null)
                lbl_AppVersion.Text = "Version: " + appVersion.ToString();

            myLog.Write(GetRSTKLibVersion());

            // Load Settings file
            LoadSettingsFromFile();

            // Load Song Collection
            if (mySettings.RescanOnStartup)
                BackgroundScan();
            else
                LoadSongCollectionFromFile();
        }

        private void BackgroundScan()
        {
            ToggleUIControls();
            toolStripStatusLabel_MainCancel.Visible = true;
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;
            // don't run bWorker more than once
            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
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
                            MessageBox.Show("This application has detected a mandatory update from your current version to version" + info.MinimumRequiredVersion + "The application will now install the update and restart.", "Update_Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                if (!dde.Message.ToLower().Contains("update not available"))
                                    Log("<Error>: " + dde.Message);
                            }
                        }
                    }
                }
                catch (Exception dde)
                {
                    if (!dde.Message.ToLower().Contains("update not available"))
                        Log("<Error>: " + dde.Message);
                }
            }
        }

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
                                    myLog.Write(string.Format("Update found for \"{0}\" from \"{1}\" album by {2}. Local version: {3}, Ignition version: {4} ", currentSong.Song, currentSong.Album, currentSong.Author, currentSong.Version, currentSong.IgnitionVersion));

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
                                Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = true; });
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

        private void ERR_NI()
        {
            MessageBox.Show("Not implemented yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private string GetInstallDirFromRegistry()
        {
            const string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
            const string rsX64Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            // TODO: confirm the following constants for x86 machines
            const string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Rocksmith2014";
            const string rsX86Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            try
            {
                if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Path, "installdir", null).ToString()))
                    return Registry.GetValue(rsX64Path, "installdir", null).ToString();
                if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Steam, "InstallLocation", null).ToString()))
                    return Registry.GetValue(rsX64Steam, "InstallLocation", null).ToString();

                // TODO: confirm the following is correct for x86 machines
                if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Steam, "InstallLocation", null).ToString()))
                    return Registry.GetValue(rsX86Path, "installdir", null).ToString();
                if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Steam, "InstallLocation", null).ToString()))
                    return Registry.GetValue(rsX86Steam, "InstallLocation", null).ToString();

            }
            catch (NullReferenceException)
            {
                // needed for WinXP SP3 which throws NullReferenceException when registry not found
                myLog.Write("RS2014 Installation Directory not found in Registry");
                myLog.Write("Specify default RS2014 Directory in the Setting tab");
            }

            return String.Empty;
        }

        private string GetRSTKLibVersion()
        {
            Assembly assembly = Assembly.LoadFrom("RocksmithToolkitLib.dll");
            Version ver = assembly.GetName().Version;
            return String.Format("RocksmithToolkitLib version: {0}", ver);
        }

        private SongData GetSongByRow(DataGridViewRow dataGridViewRow)
        {
            return SongCollection.Distinct().FirstOrDefault(x => x.Song == dataGridViewRow.Cells["Song"].Value.ToString() && x.Artist == dataGridViewRow.Cells["Artist"].Value.ToString() && x.Album == dataGridViewRow.Cells["Album"].Value.ToString() && x.Path == dataGridViewRow.Cells["Path"].Value.ToString());
        }

        private void LoadSettingsFromFile()
        {
            var settingsPath = Path.Combine(Constants.DefaultWorkingDirectory, "settings.bin");

#if (DEBUG) // make config files humanly readable
            settingsPath = Path.ChangeExtension(settingsPath, ".xml");
#endif

            // initial application startup or bad settings file
            if (!File.Exists(settingsPath))
                SaveSettingsToFile();

            try
            {
                using (FileStream fs = new FileStream(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    Settings deserialized = null;

#if (DEBUG) // make config files humanly readable
                    deserialized = fs.DeserializeXml(new Settings());
#else
                    deserialized = fs.DeserializeBin() as Settings;
#endif

                    if (deserialized != null)
                    {
                        mySettings = deserialized;
                        Extensions.InvokeIfRequired(tbSettingsRSDir, delegate { tbSettingsRSDir.Text = mySettings.RSInstalledDir; });
                        Extensions.InvokeIfRequired(checkRescanOnStartup, delegate { checkRescanOnStartup.Checked = mySettings.RescanOnStartup; });
                        Extensions.InvokeIfRequired(checkIncludeRS1DLC, delegate { checkIncludeRS1DLC.Checked = mySettings.IncludeRS1DLCs; });
                        Extensions.InvokeIfRequired(checkEnableLogBaloon, delegate { checkEnableLogBaloon.Checked = mySettings.EnabledLogBaloon; });
                        Log("Loaded settings file ...");
                    }
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("<Error>: {0}", ex.Message));
            }
        }


        private void LoadSongCollectionFromFile()
        {
            var songsPath = Path.Combine(Constants.DefaultWorkingDirectory, "songs.bin");

#if (DEBUG) // make config files humanly readable
            songsPath = Path.ChangeExtension(songsPath, ".xml");
#endif

            if (!File.Exists(songsPath))
            {
                // load songs into memory
                BackgroundScan();
                return;
            }

            try
            {
                using (var fs = new FileStream(songsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BindingList<SongData> songs = null;
                    List<string> fileList = null;

#if (DEBUG)
                    // make config files humanly readable
                    songs = fs.DeserializeXml(new BindingList<SongData>());
                    fs.Flush(); // free up memory asap

                    var fileListPath = Path.Combine(Constants.DefaultWorkingDirectory, "filelist.xml");
                    using (var fsfl = new FileStream(fileListPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        fileList = fsfl.DeserializeXml(new List<string>());
#else
                    songs = (BindingList<SongData>)fs.DeserializeBin();
                    fileList = (List<string>)fs.DeserializeBin();
#endif

                    if (fileList != null)
                        CurrentFileList = fileList;

                    if (songs != null)
                    {
                        SongCollection = songs;
                        PopulateDataGridView();
                        Log("Loaded song collection file ...");
                        // update Duplicates(n) tabcontrol text
                        PopulateDupeList();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Application couldn't read song collection file and it has to be deleted. Restart is needed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                File.Delete(songsPath);
            }
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

        private void PopulateDataGridView()
        {
            toolStripStatusLabel_Main.Text = string.Format("{0} total Rocksmith songs found", SongCollection.Count);
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    var bs = new BindingSource();
                    bs.DataSource = SongCollection;
                    dgvSongs.DataSource = bs;
                    SortedSongCollection = SongCollection.ToList();

                    // update datagrid appearance
                    DgvSongsAppearance();

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
            Log(string.Format("Finished ... Task took {0}", counterStopwatch.Elapsed));
        }

        private void PopulateDupeList()
        {
            DupeCollection.Clear();
            listDupeSongs.Items.Clear();

            var dups = SongCollection.GroupBy(x => new { x.Song, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            dups.RemoveAll(x => x.FileName.Contains(rscompatibility));

            if (dups.Count > 0)
            {
                foreach (var song in dups)
                {
                    Extensions.InvokeIfRequired(listDupeSongs, delegate { listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Enabled, song.Artist, song.Song, song.Album, song.Updated, song.Path })); });
                }
            }

            DupeCollection.AddRange(dups);
            // updates tabcontrol Duplicates(n) text
            Extensions.InvokeIfRequired(tpDuplicates, delegate { tpDuplicates.Text = "Duplicates(" + dups.Count + ")"; });

            Log("Scanned memory for duplicate songs ...");
        }

        private void PopulateRenamer()
        {
            // TODO: initialization tabcontrols only as they are used
            Log("Scanned renamer ...");

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager_Winforms.Resources.renamer_properties.json"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    renamerPropertyDataSet = new DataSet();
                    renamerPropertyDataSet = (DataSet)JsonConvert.DeserializeObject(json, (typeof(DataSet)));
                    this.renamerPropertyDataGridView.AutoGenerateColumns = true;
                    renamerPropertyDataGridView.DataSource = renamerPropertyDataSet.Tables[0];
                }
            }
            catch (Exception e)
            {
                this.myLog.Write(e.Message);
            }
        }

        private void PopulateSettings()
        {
            Log("Scanned settings ...");
            listDisabledColumns.Items.Clear();
            foreach (DataGridViewColumn col in dgvSongs.Columns)
            {
                ListViewItem newItem = new ListViewItem(new[] { string.Empty, col.Name, col.HeaderText, col.Width.ToString() });
                newItem.Checked = col.Visible;
                listDisabledColumns.Items.Add(newItem);
            }

            // update from saved settings
            tbSettingsRSDir.Text = mySettings.RSInstalledDir;
            checkRescanOnStartup.Checked = mySettings.RescanOnStartup;
            checkIncludeRS1DLC.Checked = mySettings.IncludeRS1DLCs;
            checkEnableLogBaloon.Checked = mySettings.EnabledLogBaloon;
        }

        private void PopulateSongList()
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    var dataGridViewColumn = dgvSongs.Columns["colSelect"];
                    if (dataGridViewColumn != null)
                        dataGridViewColumn.Visible = false;
                    dgvSongs.DataSource = null;
                    SongCollection.Clear();
                });

            List<string> fileList = FilesList(Path.Combine(mySettings.RSInstalledDir, "dlc"), mySettings.IncludeRS1DLCs);
            //List<string> disabledFilesList = new List<string>(FilesList(Path.Combine(mySettings.RSInstalledDir, Constants.DefaultDisabledSubDirectory),mySettings.IncludeRS1DLCs));
            //filesList.AddRange(disabledFilesList);

            Log(String.Format("Raw songs count: {0}", fileList.Count));
            if (fileList.Count == 0)
                return;

            songCounter = 0;
            counterStopwatch.Start();
            CurrentFileList.Clear();

            foreach (string file in fileList)
            {
                if (bWorker == null || (bWorker != null || !bWorker.CancellationPending))
                {
                    Progress(songCounter++ * 100 / fileList.Count);

                    string enabled = file.Contains(".disabled.") ? "No" : "Yes";
                    parsePSARC(songCounter, enabled, file);
                    CurrentFileList.Add(file);
                }
            }

            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    var dataGridViewColumn = dgvSongs.Columns["colSelect"];
                    if (dataGridViewColumn != null) dataGridViewColumn.Visible = true;
                    dgvSongs.DataSource = SongCollection;
                });

            Log("Populated song list ...");
        }

        private void Progress(int value)
        {
            Extensions.InvokeIfRequired(toolStripProgressBarMain.ProgressBar, delegate
                {
                    if (toolStripProgressBarMain.ProgressBar != null && value <= 100)
                        toolStripProgressBarMain.ProgressBar.Value = value;
                });
        }

        private void ResetSettings()
        {
            // initialize object if null
            if (mySettings == null)
                mySettings = new Settings();

            mySettings.RSInstalledDir = GetInstallDirFromRegistry();
            mySettings.LogFilePath = Constants.DefaultLogName;
            mySettings.RescanOnStartup = true;
            mySettings.IncludeRS1DLCs = true;
            mySettings.EnabledLogBaloon = true;

            Extensions.InvokeIfRequired(tbSettingsRSDir, delegate { tbSettingsRSDir.Text = mySettings.RSInstalledDir; });
            Extensions.InvokeIfRequired(checkIncludeRS1DLC, delegate { checkIncludeRS1DLC.Checked = mySettings.IncludeRS1DLCs; });
            Extensions.InvokeIfRequired(tbSettingsRSDir, delegate { checkRescanOnStartup.Checked = mySettings.RescanOnStartup; });
            Extensions.InvokeIfRequired(tbSettingsRSDir, delegate { checkEnableLogBaloon.Checked = mySettings.EnabledLogBaloon; });

            Log("Reset settings to defaults ...");
        }

        private void RestartApp()
        {
            Process.Start(Application.ExecutablePath);
            Process.GetCurrentProcess().Kill();
        }

        private void SaveSettingsToFile()
        {
            if (mySettings.RSInstalledDir == null)
                ResetSettings();

            // save user options
            if (mySettings.RSInstalledDir != tbSettingsRSDir.Text)
                mySettings.RSInstalledDir = tbSettingsRSDir.Text;
            if (mySettings.RescanOnStartup != checkRescanOnStartup.Checked)
                mySettings.RescanOnStartup = checkRescanOnStartup.Checked;
            if (mySettings.IncludeRS1DLCs != checkIncludeRS1DLC.Checked)
                mySettings.IncludeRS1DLCs = checkIncludeRS1DLC.Checked;
            if (mySettings.EnabledLogBaloon != checkEnableLogBaloon.Checked)
                mySettings.EnabledLogBaloon = checkEnableLogBaloon.Checked;

            // validate Rocksmith installation directory information
            if (tbSettingsRSDir.Text.Contains("Click here") || String.IsNullOrEmpty(tbSettingsRSDir.Text))
            {
                myLog.Write("Select your Rocksmith installation directory ...");
                tbSettingsRSDir_MouseClick(null, null);
            }

            if (dgvSongs != null)
            {
                var settings = new RADataGridViewSettings();
                var columns = dgvSongs.Columns;
                if (columns.Count > 1) //HACK:dirt
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        settings.ColumnOrder.Add(new ColumnOrderItem { ColumnIndex = i, DisplayIndex = columns[i].DisplayIndex, Visible = columns[i].Visible, Width = columns[i].Width });
                    }
                    mySettings.ManagerGridSettings = settings;
                }
            }

            var settingsPath = Path.Combine(Constants.DefaultWorkingDirectory, "settings.bin");

#if (DEBUG) // make config files humanly readable
            settingsPath = Path.ChangeExtension(settingsPath, ".xml");
#endif

            using (var fs = new FileStream(settingsPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {

#if (DEBUG) // make config files humanly readable
                mySettings.SerializeXml(fs);
#else
                mySettings.SerializeBin(fs);
#endif

                Log("Saved settings file ...");
                fs.Flush();
            }
        }

        private void SaveSongCollectionToFile()
        {
            var songsinPath = Path.Combine(Constants.DefaultWorkingDirectory, "songs.bin");

#if (DEBUG) // make config files humanly readable
            songsinPath = Path.ChangeExtension(songsinPath, ".xml");
#endif

            using (var fs = new FileStream(songsinPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {

#if (DEBUG) // make config files humanly readable
                SongCollection.SerializeXml(fs);
                var fileListPath = Path.Combine(Constants.DefaultWorkingDirectory, "filelist.xml");
                using (var fsfl = new FileStream(fileListPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    CurrentFileList.SerializeXml(fsfl);

                Log("Saved file list file ...");
#else
                SongCollection.SerializeBin(fs);
                CurrentFileList.SerializeBin(fs);
#endif

                fs.Flush();
                Log("Saved song collection file ...");
            }
        }

        private void SearchDLC(string criteria)
        {
            var results = SongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) || x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower())) || x.SongYear.Contains(criteria) || x.Path.ToLower().Contains(criteria.ToLower()))).ToList();

            SortedSongCollection = SongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) || x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower())) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower()) || x.SongYear.Contains(criteria) || x.Path.ToLower().Contains(criteria.ToLower()))).ToList();

            Extensions.InvokeIfRequired(dgvSongs, delegate { dgvSongs.DataSource = results; });
        }

        // Code from CGT used with permission
        private void DgvSongsAppearance()
        {
            // processing order is important to appearance
            dgvSongs.Visible = true;
            dgvSongs.Font = new Font("Arial", 8);
            dgvSongs.ColumnHeadersVisible = true;
            dgvSongs.RowHeadersVisible = false; // remove row arrow
            dgvSongs.AllowUserToAddRows = false; // removes empty row at bottom
            dgvSongs.AllowUserToDeleteRows = false;
            dgvSongs.ReadOnly = true;
            //dgvSongs.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;
            dgvSongs.EnableHeadersVisualStyles = true;
            dgvSongs.AllowUserToResizeColumns = true;
            // set initial column width to column header width
            dgvSongs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            
            dgvSongs.ClearSelection();
        }

        private void ShowHideArrangementColumns()
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                if (row.Cells["cLead"].Visible)
                    (row.Cells["cLead"]).Style.BackColor = Color.Red;
                if (row.Cells["cRhythm"].Visible)
                    (row.Cells["cRhythm"]).Style.BackColor = Color.Red;
                if (row.Cells["cBass"].Visible)
                    (row.Cells["cBass"]).Style.BackColor = Color.Red;
                if (row.Cells["cVocals"].Visible)
                    (row.Cells["cVocals"]).Style.BackColor = Color.Red;

                string arr = row.Cells["Arrangements"].Value.ToString();
                if (!string.IsNullOrEmpty(arr))
                {
                    foreach (string arrangment in arr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (arrangment.ToLower() != "combo")
                        {
                            string columnName = "c" + arrangment;
                            row.Cells[columnName].Style.BackColor = Color.Green;
                        }
                    }
                }
            }
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

                var song = SongCollection.FirstOrDefault(x => x.Song == title && x.Album == album && x.Artist == artist && x.Path == path);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
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

        private void ToggleUIControls()
        {
            Extensions.InvokeIfRequired(btnRescan, delegate { btnRescan.Enabled = !btnRescan.Enabled; });
            Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = !btnCheckAllForUpdates.Enabled; });
            //Uncomment after implementing:
            //btnEditDLC.Enabled = !btnEditDLC.Enabled;
            //btnBackupDLC.Enabled = !btnBackupDLC.Enabled;
            //btnSaveDLC.Enabled = !btnSaveDLC.Enabled;
            //btnDLCPage.Enabled = !btnDLCPage.Enabled;
            Extensions.InvokeIfRequired(checkRescanOnStartup, delegate { checkRescanOnStartup.Enabled = !checkRescanOnStartup.Enabled; });
            Extensions.InvokeIfRequired(checkEnableLogBaloon, delegate { checkEnableLogBaloon.Enabled = !checkEnableLogBaloon.Enabled; });
            Extensions.InvokeIfRequired(btnBackupRSProfile, delegate { btnBackupRSProfile.Enabled = !btnBackupRSProfile.Enabled; });
            //btnSearch.InvokeIfRequired(delegate
            //{
            //    btnSearch.Enabled = !btnSearch.Enabled;
            //});
            Extensions.InvokeIfRequired(btnSettingsSave, delegate { btnSettingsSave.Enabled = !btnSettingsSave.Enabled; });
            Extensions.InvokeIfRequired(btnSettingsLoad, delegate { btnSettingsLoad.Enabled = !btnSettingsLoad.Enabled; });
            Extensions.InvokeIfRequired(tbSearch, delegate { tbSearch.Enabled = !tbSearch.Enabled; });
            Extensions.InvokeIfRequired(tbSettingsRSDir, delegate { tbSettingsRSDir.Enabled = !tbSettingsRSDir.Enabled; });
            Extensions.InvokeIfRequired(btnDupeRescan, delegate { btnDupeRescan.Enabled = !btnDupeRescan.Enabled; });
            Extensions.InvokeIfRequired(btnDeleteDupeSong, delegate { btnDeleteDupeSong.Enabled = !btnDeleteDupeSong.Enabled; });
            Extensions.InvokeIfRequired(btnDisableEnableSongs, delegate { btnDisableEnableSongs.Enabled = !btnDisableEnableSongs.Enabled; });
            Extensions.InvokeIfRequired(btnExportSongList, delegate { btnExportSongList.Enabled = !btnExportSongList.Enabled; });
            Extensions.InvokeIfRequired(btnBackupSelectedDLCs, delegate { btnBackupSelectedDLCs.Enabled = !btnBackupSelectedDLCs.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToBBCode, delegate { radioBtn_ExportToBBCode.Enabled = !radioBtn_ExportToBBCode.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToCSV, delegate { radioBtn_ExportToCSV.Enabled = !radioBtn_ExportToCSV.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToHTML, delegate { radioBtn_ExportToHTML.Enabled = !radioBtn_ExportToHTML.Enabled; });
            Extensions.InvokeIfRequired(checkIncludeRS1DLC, delegate { checkIncludeRS1DLC.Enabled = !checkIncludeRS1DLC.Enabled; });
            Extensions.InvokeIfRequired(linkLblSelectAll, delegate { linkLblSelectAll.Enabled = !linkLblSelectAll.Enabled; });
            Extensions.InvokeIfRequired(link_MainClearResults, delegate { link_MainClearResults.Enabled = !link_MainClearResults.Enabled; });
            Extensions.InvokeIfRequired(lbl_ExportTo, delegate { lbl_ExportTo.Enabled = !lbl_ExportTo.Enabled; });
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

        private void btnEnableColumns_Click(object sender, EventArgs e)
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchDLC(tbSearch.Text);
        }

        private void linkOpenOldSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://search.customsforge.com/");
        }

        private void link_ForgeOnProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/345-forgeon/");
        }

        private void lnkAboutCF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/");
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
                    {
                        //ODLC
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
                toolStripStatusLabel_Main.Text = string.Format(" files found: {0}", counter);
                //populate dgv here
            }
        }

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
                    Directory.CreateDirectory(newFileInfo.Directory.FullName);
                    myLog.Write("Renaming/Moving:" + oldFilePath);

                    myLog.Write("---> " + newFilePath);
                    File.Move(oldFilePath, newFilePath);
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

        public bool SetlistEnabled(string setlistName)
        {
            string setlistPath = "";
            if (setlistName.Contains(mySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);

            var songsInSetlist = Directory.EnumerateFiles(setlistPath, "*_p.*psarc", SearchOption.AllDirectories);

            if (songsInSetlist.Where(sng => sng.Contains(".disabled")).Count() == songsInSetlist.Count())
                return false;
            else
                return true;
        }

        public bool SetlistModified(string setlistName)
        {
            string setlistPath = "";
            if (setlistName.Contains(mySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);

            var disabledCount = Directory.EnumerateFiles(setlistPath, "*_p.*psarc", SearchOption.AllDirectories).Where(sng => sng.Contains(".disabled")).Count();

            if (disabledCount > 0)
                return true;
            else
                return false;
        }

        public bool SetlistContainsSong(string setlistName, string songName)
        {
            string setlistPath = "";
            if (setlistName.Contains(mySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);

            var songsInSetlist = Directory.EnumerateFiles(setlistPath, "*_p.*psarc");

            if (songsInSetlist.Where(sng => sng.ToLower().Contains(songName.ToLower())).Count() > 0 || setlistName.ToLower().Contains(songName.ToLower()))
                return true;
            else
                return false;
        }


        public void RefreshSelectedSongs(string search = "")
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                {
                    dgvDLCsInSetlist.Rows.Clear();

                    if (dgvSetlists.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvSetlists.SelectedRows)
                        {
                            string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());

                            if (Directory.Exists(setlistPath))
                            {
                                var matchingSongs = SongCollection.Where(sng => (sng.Artist.ToLower().Contains(search) || sng.Album.ToLower().Contains(search) || sng.Song.ToLower().Contains(search) || sng.Path.ToLower().Contains(search)) && sng.Path.Contains(setlistPath)).ToList();
                                foreach (SongData song in matchingSongs)
                                {
                                    dgvDLCsInSetlist.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Path);
                                }
                            }
                        }
                    }
                });
            };

            bWorker.RunWorkerAsync();
        }

        private void LoadSetlistsHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadSetlists();
        }

        public void LoadSetlists()
        {
            string[] dirs = null;
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    if (DirOK())
                    {
                        string dlcFolderPath = Path.Combine(mySettings.RSInstalledDir, "dlc");

                        if (Directory.Exists(dlcFolderPath))
                        {
                            dirs = Directory.GetDirectories(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*", SearchOption.AllDirectories);
                            foreach (var setlistPath in dirs)
                            {
                                bool setlistEnabled = true;
                                Extensions.InvokeIfRequired(dgvSetlists, delegate
                                    {
                                        if (!SetlistEnabled(setlistPath))
                                            dgvSetlists.Rows.Add(false, "No", Path.GetFileName(setlistPath.Replace("-disabled", "")));
                                        else if (SetlistModified(setlistPath))
                                            dgvSetlists.Rows.Add(false, "Modded", Path.GetFileName(setlistPath));
                                        else
                                            dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlistPath));
                                    });
                            }

                            string[] filesInSetlist = null;
                            if (dirs.Length > 0 && dirs[0] != null)
                            {
                                filesInSetlist = Directory.GetFiles(dirs[0]);
                                string[] unsortedSongs = Directory.GetFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*_p.*psarc", SearchOption.TopDirectoryOnly);

                                Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                                    {
                                        foreach (string songPath in filesInSetlist)
                                        {
                                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                            if (song != null)
                                                dgvDLCsInSetlist.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Path);
                                        }
                                    });

                                Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate
                                    {
                                        foreach (string songPath in unsortedSongs)
                                        {
                                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                            if (!songPath.Contains("rs1") && song != null)
                                            {
                                                if (songPath.Contains(".disabled"))
                                                    dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Path);
                                                else
                                                    dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Path);
                                            }
                                        }
                                    });

                                Extensions.InvokeIfRequired(dgvOfficialSongs, delegate
                                    {
                                        string cachePsarcPath = Path.Combine(mySettings.RSInstalledDir, "cache.psarc");
                                        string cachePsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "cache.disabled.psarc");

                                        string rs1PsarcPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.psarc");
                                        string rs1PsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.disabled.psarc");

                                        string rs1DLCPsarcPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.psarc");
                                        string rs1DLCPsarcBackupPath = Path.Combine(mySettings.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.disabled.psarc");

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
                Extensions.InvokeIfRequired(dgvSongs, delegate
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
                    Extensions.InvokeIfRequired(dgvSongs, delegate
                        {
                            var deletedSong = SongCollection.FirstOrDefault(song => song.Path == file);
                            SongCollection.Remove(deletedSong);
                            songCounter--;
                        });
                }
            }

            foreach (string file in newFileList) //check if there's any new songs, if there is, add them to the GridView/SongCollection
            {
                if (file != rscompatibility)
                {
                    if (!oldFileList.Any(file.Contains))
                    {
                        Extensions.InvokeIfRequired(dgvSongs, delegate
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
        }

        private void CheckForUpdatesEvent(object o, DoWorkEventArgs args)
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    if (dgvSongs.SelectedRows.Count > 0)
                    {
                        CheckRowForUpdate(dgvSongs.SelectedRows[0]);
                        SaveSongCollectionToFile();
                    }
                });
        }

        private void PopulateCompletedHandler(object sender, RunWorkerCompletedEventArgs e)
        {
            // processing order is important
            Log("Songs loaded into memory ...", 100);
            PopulateDupeList(); // update Duplicates(n) tabcontrol text            
            PopulateDataGridView();
            ToggleUIControls();
            toolStripStatusLabel_MainCancel.Visible = false;
        }

        private void PopulateListHandler(object sender, DoWorkEventArgs e)
        {
            PopulateSongList();
        }

        private void backupDLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string backupPath = Path.Combine(mySettings.RSInstalledDir, "backup");
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

        private void btnBackupRSProfile_Click(object sender, EventArgs e)
        {
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.DefaultWorkingDirectory, timestamp);
                string profilePath = "";
                string steamUserdataPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) + @"\userdata";

                if (String.IsNullOrEmpty(steamUserdataPath))
                    steamUserdataPath = customUserdataPath;

                var subdirs = new DirectoryInfo(steamUserdataPath).GetDirectories("*", SearchOption.AllDirectories).ToArray();
                foreach (DirectoryInfo info in subdirs)
                {
                    if (info.FullName.Contains(@"221680\remote"))
                    {
                        profilePath = info.FullName;
                    }
                }

                if (Directory.Exists(profilePath))
                {
                    string[] filenames = Directory.GetFiles(profilePath, "*", SearchOption.AllDirectories);
                    foreach (var filename in filenames)
                    {
                        ZlibNet.ZipFile(filename, backupPath);
                    }

                    //ZipFile.CreateFromDirectory(profilePath, backupPath);
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

        private void btnDeleteSelectedSetlist_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    try
                    {
                        foreach (DataGridViewRow row in dgvSetlists.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                            {
                                if (DirOK())
                                {
                                    string setlistName = row.Cells["colSetlist"].Value.ToString();
                                    string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);

                                    if (Directory.Exists(setlistPath))
                                    {
                                        string[] songs = Directory.GetFiles(setlistPath);

                                        if (checkDeleteSongsAndSetlists.Checked)
                                        {
                                            var confirmResult = MessageBox.Show("Are you sure you want to permanently delete this setlist (" + setlistName + ") and all songs it contains?", "Permanently delete setlist?", MessageBoxButtons.YesNo);
                                            if (confirmResult == DialogResult.Yes)
                                            {
                                                foreach (string song in songs)
                                                {
                                                    var songToBeDeleted = SongCollection.FirstOrDefault(sng => sng.Path == song);
                                                    SongCollection.Remove(songToBeDeleted);
                                                }
                                                Directory.Delete(setlistPath, true);
                                            }
                                        }
                                        else
                                        {
                                            foreach (string songPath in songs)
                                            {
                                                var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                                bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                                if (songPath.Contains("dlc"))
                                                {
                                                    string finalSongPath = songPath.Replace(row.Cells["colSetlist"].Value.ToString(), "").Replace("_p.disabled.psarc", "_p.psarc");

                                                    Extensions.InvokeIfRequired(dgvSongs, delegate
                                                        {
                                                            if (File.Exists(finalSongPath))
                                                            {
                                                                var songDupe = SongCollection.FirstOrDefault(sng => sng.Path == finalSongPath);

                                                                File.Delete(finalSongPath);

                                                                SongCollection.Remove(songDupe);
                                                            }

                                                            if (File.Exists(finalSongPath + ".disabled"))
                                                            {
                                                                var songDupe = SongCollection.FirstOrDefault(sng => sng.Path == (finalSongPath + ".disabled"));

                                                                File.Delete(finalSongPath + ".disabled");

                                                                SongCollection.Remove(songDupe);
                                                            }

                                                            if (File.Exists(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc")))
                                                            {
                                                                var songDupe = SongCollection.FirstOrDefault(sng => sng.Path == finalSongPath);

                                                                File.Delete(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc"));

                                                                SongCollection.Remove(songDupe);
                                                            }
                                                        });

                                                    File.Move(songPath, finalSongPath);

                                                    if (tagged)
                                                        File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                                    song.Path = finalSongPath;
                                                }

                                                dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Path);
                                            }

                                            Directory.Delete(setlistPath, true);
                                            dgvUnsortedDLCs.Sort(dgvUnsortedDLCs.Columns["colUnsortedSong"], ListSortDirection.Ascending);
                                        }

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
                };
            bWorker.RunWorkerAsync();
        }

        private void btnDeleteSelectedSongs_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    try
                    {
                        if (DirOK())
                        {
                            var confirmResult = MessageBox.Show("Are you sure you want to permanently delete all selected songs?", "Permanently delete songs?", MessageBoxButtons.YesNo);
                            if (confirmResult == DialogResult.Yes)
                            {
                                foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows)
                                {
                                    if (Convert.ToBoolean(row.Cells["colUnsortedSelect"].Value) == true || row.Selected)
                                    {
                                        string songPath = row.Cells["colUnsortedPath"].Value.ToString();
                                        if (!songPath.Contains("rs1comp"))
                                        {
                                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                            File.Delete(songPath);

                                            if (!File.Exists(songPath))
                                                rowsToDelete.Add(row);

                                            Extensions.InvokeIfRequired(dgvSongs, delegate { SongCollection.Remove(song); });
                                        }
                                    }
                                }
                            }
                            Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate
                                {
                                    foreach (DataGridViewRow dataGridViewRow in rowsToDelete)
                                        dgvUnsortedDLCs.Rows.Remove(dataGridViewRow);
                                });
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Unable to delete selected song(s)! Error: \n\n" + ex.ToString());
                    }
                };
            bWorker.RunWorkerAsync();
        }

        private void btnDeleteSongOne_Click(object sender, EventArgs e)
        {
            Extensions.InvokeIfRequired(listDupeSongs, delegate
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
                                        Extensions.InvokeIfRequired(listDupeSongs, delegate { listDupeSongs.Items.Add(new ListViewItem(new[] { " ", song.Enabled, song.Artist, song.Song, song.Album, song.Updated, song.Path })); });
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

        private void btnDisableEnableSong_Click(object sender, EventArgs e)
        {
            Extensions.InvokeIfRequired(listDupeSongs, delegate
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

        private void btnDupeRescan_Click(object sender, EventArgs e)
        {
            //Same issue as with regular rescan..
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
                    Extensions.InvokeIfRequired(tpDuplicates, delegate { tpDuplicates.Text = "Duplicates(0)"; });
                    BackgroundScan();
                }
            }
            else
            {
                BackgroundScan();
            }
        }

        private void btnEOFSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/eof");
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
                                    string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                    string finalSongPath = "";
                                    List<string> setlistDisabledSongs = Directory.GetFiles(setlistPath).Where(sng => sng.Contains(".disabled")).ToList();

                                    foreach (string songPath in setlistDisabledSongs)
                                    {
                                        var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                        bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);
                                        finalSongPath = songPath.Replace("_p.psarc", "_p.disabled.psarc");

                                        File.Move(songPath, finalSongPath);

                                        if (tagged)
                                            File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                        song.Path = finalSongPath;
                                        song.Enabled = "Yes";

                                        Extensions.InvokeIfRequired(dgvSetlists, delegate
                                            {
                                                row.Cells["colSetlistEnabled"].Value = "Yes";
                                                row.DefaultCellStyle.BackColor = Color.White;
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
                            string songPath = row.Cells["colUnsortedPath"].Value.ToString();
                            string finalSongPath = "";
                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                            bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                            if (row.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                            {
                                finalSongPath = songPath.Replace("_p.disabled.psarc", "_p.psarc");

                                File.Move(songPath, finalSongPath);

                                row.Cells["colUnsortedEnabled"].Value = "Yes";
                                row.DefaultCellStyle.BackColor = Color.White;
                            }
                            else
                            {
                                finalSongPath = songPath.Replace("_p.psarc", "_p.disabled.psarc");

                                File.Move(songPath, finalSongPath);

                                row.Cells["colUnsortedEnabled"].Value = "No";
                                row.DefaultCellStyle.BackColor = Color.LightGray;
                            }

                            if (tagged)
                                File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                            song.Path = finalSongPath;
                            song.Enabled = song.Enabled == "Yes" ? "No" : "Yes";
                            row.Cells["colUnsortedPath"].Value = finalSongPath;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to disalbe/enable selected songs! Error: \n\n" + ex.ToString());
            }
        }

        private void btnEnblDisbSelectedSetlist_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                 {
                     if (DirOK())
                     {
                         if (radioEnableDisableSetlists.Checked)
                         {
                             foreach (DataGridViewRow row in dgvSetlists.Rows)
                             {
                                 dgvDLCsInSetlist.Rows.Clear();
                                 if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                                 {
                                     string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                     string setlistEnabledValue = row.Cells["colSetlistEnabled"].Value.ToString();
                                     bool setlistEnabled = setlistEnabledValue == "Yes" || setlistEnabledValue == "Modded" ? true : false;

                                     foreach (string songPath in Directory.GetFiles(setlistPath))
                                     {
                                         var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                         string finalSongPath = setlistEnabled ? song.Path.Replace("_p.psarc", "_p.disabled.psarc") : song.Path.Replace("_p.disabled.psarc", "_p.psarc");
                                         string songEnabled = setlistEnabled ? "No" : "Yes";
                                         bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                         File.Move(song.Path, finalSongPath);

                                         if (tagged)
                                             File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                         song.Path = finalSongPath;
                                         song.Enabled = songEnabled;

                                         dgvDLCsInSetlist.Rows.Add(false, songEnabled, song.Artist, song.Song, song.Album, Path.GetFileName(finalSongPath));
                                     }

                                     if (setlistEnabled)
                                     {
                                         row.Cells["colSetlistEnabled"].Value = "No";
                                         row.DefaultCellStyle.BackColor = Color.LightGray;
                                     }
                                     else
                                     {
                                         row.Cells["colSetlistEnabled"].Value = "Yes";
                                         row.DefaultCellStyle.BackColor = Color.White;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             foreach (DataGridViewRow selectedSong in dgvDLCsInSetlist.SelectedRows)
                             {
                                 foreach (DataGridViewRow row in dgvSetlists.SelectedRows)
                                 {
                                     string setlistName = row.Cells["colSetlist"].Value.ToString();
                                     string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", setlistName);
                                     string songPath = selectedSong.Cells["colDLCPath"].Value.ToString();

                                     if (SetlistContainsSong(setlistName, songPath))
                                     {
                                         var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                         string finalSongPath = songPath;

                                         bool enabled = !songPath.Contains("_p.disabled.psarc") ? true : false;
                                         bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                         if (!enabled)
                                             finalSongPath = finalSongPath.Replace("_p.disabled.psarc", "_p.psarc");
                                         else
                                             finalSongPath = finalSongPath.Replace("_p.psarc", "_p.disabled.psarc");

                                         File.Move(songPath, finalSongPath);

                                         song.Path = finalSongPath;
                                         song.Enabled = !enabled ? "Yes" : "No";

                                         selectedSong.Cells["colDLCPath"].Value = finalSongPath;
                                         selectedSong.Cells["colDLCEnabled"].Value = !enabled ? "Yes" : "No";

                                         if (tagged)
                                             File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                         if (!SetlistEnabled(setlistName))
                                             row.Cells["colSetlistEnabled"].Value = "No";
                                         else if (SetlistModified(setlistName))
                                             row.Cells["colSetlistEnabled"].Value = "Modded";
                                         else
                                             row.Cells["colSetlistEnabled"].Value = "Yes";
                                     }
                                 }
                             }
                         }
                     }
                 });
                };
            bWorker.RunWorkerAsync();
        }

        private void btnEnblDisblOfficialSongPack_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
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

                                    if (row.Cells["colOfficialEnabled"].Value == "Yes")
                                    {
                                        row.Cells["colOfficialEnabled"].Value = "No";
                                        row.DefaultCellStyle.BackColor = Color.LightGray;
                                        row.Selected = false;
                                        row.Cells["colOfficialSelect"].Value = false;
                                    }
                                    else
                                    {
                                        row.Cells["colOfficialEnabled"].Value = "Yes";
                                        row.DefaultCellStyle.BackColor = Color.White;
                                        row.Selected = false;
                                        row.Cells["colOfficialSelect"].Value = false;
                                    }
                                }
                            }
                        }
                    }
                };
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

        private void btnLaunchSteam_Click(object sender, EventArgs e)
        {
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                Process.Start("steam://rungameid/221680");
        }

        private void btnLoadSetlists_Click(object sender, EventArgs e)
        {
            dgvDLCsInSetlist.Rows.Clear();
            dgvSetlists.Rows.Clear();
            dgvOfficialSongs.Rows.Clear();
            dgvUnsortedDLCs.Rows.Clear();

            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();

            bWorker.DoWork += RescanSongs; //to make sure that all songs are shown
            bWorker.RunWorkerCompleted += RescanCompleted;
            bWorker.RunWorkerCompleted += LoadSetlistsHandler;
            bWorker.RunWorkerAsync();
        }

        private void btnMoveSongToSetlist_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
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
                                            string songPath = unsortedRow.Cells["colUnsortedPath"].Value.ToString();
                                            string setlistPath = Path.Combine(mySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                            string finalSongPath = "";

                                            string setlistEnabledValue = row.Cells["colSetlistEnabled"].Value.ToString();
                                            bool setlistEnabled = setlistEnabledValue == "Yes" || setlistEnabledValue == "Modded" ? true : false;

                                            bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);
                                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                            if (setlistEnabled)
                                                finalSongPath = Path.Combine(setlistPath, Path.GetFileName(songPath.Replace(".disabled", "")));
                                            else
                                            {
                                                finalSongPath = Path.Combine(setlistPath, Path.GetFileName(songPath));

                                                if (unsortedRow.Cells["colUnsortedEnabled"].Value.ToString() != "No")
                                                    finalSongPath = finalSongPath.Replace("_p.psarc", "_p.disabled.psarc");
                                            }

                                            if (File.Exists(finalSongPath))
                                                File.Delete(finalSongPath);
                                            File.Move(songPath, finalSongPath);

                                            if (tagged)
                                                File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                            Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                                                {
                                                    dgvDLCsInSetlist.Rows.Add(false, setlistEnabled ? "Yes" : "No", song.Artist, song.Song, song.Album, song.Path);
                                                });

                                            Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate { dgvUnsortedDLCs.Rows.Remove(unsortedRow); });

                                            song.Path = finalSongPath;
                                            song.Enabled = setlistEnabled ? "Yes" : "No";
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
                };
            bWorker.RunWorkerAsync();
        }

        private void btnRSTKSite_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.rscustom.net/");
        }

        private void btnRemoveSongsFromSetlist_Click(object sender, EventArgs e)
        {
            bool permaDelete = false;
            try
            {
                foreach (DataGridViewRow row in dgvDLCsInSetlist.Rows)
                {
                    if (DirOK())
                    {
                        if (row.Selected || Convert.ToBoolean(row.Cells["colDLCSelect"].Value) == true)
                        {
                            if (checkDeleteSongsAndSetlists.Checked)
                            {
                                var confirmResult = MessageBox.Show("Are you sure you want to permanently delete all selected songs?", "Permanently delete songs?", MessageBoxButtons.YesNo);
                                if (confirmResult == DialogResult.Yes)
                                    permaDelete = true;
                            }

                            string dlcFolderPath = Path.Combine(mySettings.RSInstalledDir, "dlc");
                            string setlistSongPath = Path.Combine(dlcFolderPath, dgvSetlists.SelectedRows[0].Cells["colSetlist"].Value.ToString());
                            string songPath = row.Cells["colDLCPath"].Value.ToString();
                            string finalSongPath = Path.Combine(dlcFolderPath, Path.GetFileName(songPath)).Replace("_p.disabled.psarc", "_p.psarc");

                            var song = SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                            bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                            if (permaDelete || songPath.Contains("rs1comp") || songPath.Contains("cache.psarc"))
                            {
                                File.Delete(songPath);
                                SongCollection.Remove(song);
                                dgvDLCsInSetlist.Rows.Remove(row);
                            }
                            else if (!checkDeleteSongsAndSetlists.Checked)
                            {
                                if (File.Exists(finalSongPath))
                                    File.Delete(finalSongPath);

                                if (File.Exists(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc")))
                                    File.Delete(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc"));

                                if (File.Exists(finalSongPath + ".disabled"))
                                    File.Delete(finalSongPath + ".disabled");

                                File.Move(songPath, finalSongPath);

                                if (tagged)
                                    File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Album, finalSongPath);
                                dgvUnsortedDLCs.Sort(dgvUnsortedDLCs.Columns["colUnsortedSong"], ListSortDirection.Ascending);

                                dgvDLCsInSetlist.Rows.Remove(row);

                                song.Path = finalSongPath;
                                song.Enabled = "Yes";
                            }
                            else if (permaDelete == false)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n\n" + ex.ToString(), "IO Error");
            }
        }

        private void btnRequestSong_Click(object sender, EventArgs e)
        {
            Process.Start("http://requests.customsforge.com/");
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            // unlikely condition
            if (String.IsNullOrEmpty(mySettings.RSInstalledDir))
            {
                MessageBox.Show("Please, make sure that you've got Rocksmith 2014 installed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // save settings (column widths) in case user has modified
            SaveSettingsToFile();

            // reset the Song Collection File to force fresh scan
            var songsPath = Path.Combine(Constants.DefaultWorkingDirectory, "songs.bin");
#if (DEBUG)
            songsPath = Path.ChangeExtension(songsPath, ".xml");
#endif
            if (File.Exists(songsPath))
                File.Delete(songsPath);

            // rescan preserves column width, display settings and duplicates
            LoadSongCollectionFromFile();

            // update Duplicates(n) tabcontrol
            //PopulateDupeList();
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
                        if (File.Exists(rs1FinalPath.Replace("_p.psarc", "_p.disabled.psarc")))
                            File.Delete(rs1FinalPath.Replace("_p.psarc", "_p.disabled.psarc"));

                        File.Copy(rs1BackupPath, rs1FinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("disc")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                    }
                    if (File.Exists(rs1DLCBackupPath))
                    {
                        if (File.Exists(rs1DLCFinalPath.Replace("_p.psarc", "_p.disabled.psarc")))
                            File.Delete(rs1DLCFinalPath.Replace("_p.psarc", "_p.disabled.psarc"));

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

        private void btnRunRSWithSetlist_Click(object sender, EventArgs e)
        {
            string rs2014Pack = "";
            string rs1MainPack = "";
            string rs1DLCPack = "";
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");

            List<string> rs1DLCFiles = Directory.EnumerateFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "rs1compatibilitydlc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs1Files = Directory.EnumerateFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "rs1compatibilitydisc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs2014Files = Directory.EnumerateFiles(Path.Combine(mySettings.RSInstalledDir, "dlc"), "cache.psarc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();

            frmComboBoxPopup comboPopup = new frmComboBoxPopup();

            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    if (rs2014Files.Count > 0)
                    {
                        comboPopup.ComboBoxItems.Add("Use actual (rootdir) pack");

                        foreach (string rs2014File in rs2014Files)
                            comboPopup.ComboBoxItems.Add(new FileInfo(rs2014File).Directory.Name);

                        comboPopup.LblText = "Select a RS2014 official song pack to restore from the selected setlist:";
                        comboPopup.FrmText = "Duplicate RS2014 official song pack detected";
                        comboPopup.BtnText = "OK";
                        comboPopup.Combo.SelectedIndex = 0;

                        comboPopup.ShowDialog();

                        rs2014Pack = comboPopup.Combo.SelectedItem.ToString();

                        if (rs2014Pack != "Use actual (rootdir) pack")
                        {
                            foreach (string rs2014File in rs2014Files)
                            {
                                if (Path.GetDirectoryName(rs2014File) != Path.Combine(mySettings.RSInstalledDir, rs2014Pack))
                                {
                                    File.Move(rs2014File, rs2014File + ".disabled");
                                }
                            }

                            if (File.Exists(Path.Combine(mySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc")))
                                File.Copy(Path.Combine(mySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc"), Path.Combine(mySettings.RSInstalledDir, "cache.psarc"), true);
                            else if (File.Exists(Path.Combine(mySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled")))
                                File.Copy(Path.Combine(mySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled"), Path.Combine(mySettings.RSInstalledDir, "cache.psarc"), true);
                        }
                    }

                    if (rs1DLCFiles.Count > 1)
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
                                if (Path.GetDirectoryName(rs1DLCFile) != Path.Combine(mySettings.RSInstalledDir, "dlc", rs1DLCPack))
                                {
                                    File.Move(rs1DLCFile, rs1DLCFile.Replace("_p.psarc", "_p.disabled.psarc"));
                                }
                            }
                        }
                    }

                    if (rs1Files.Count > 1)
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
                                if (Path.GetDirectoryName(rs1File) != Path.Combine(mySettings.RSInstalledDir, "dlc", rs1MainPack))
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
                };
            bWorker.RunWorkerAsync();
        }

        private void btnSettingsLoad_Click(object sender, EventArgs e)
        {
            LoadSettingsFromFile();
        }

        private void btnSettingsSave_Click(object sender, EventArgs e)
        {
            SaveSettingsToFile();
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
                                        File.Copy(songPackPath, finalSongPackPath);
                                    }
                                    else
                                    {
                                        finalSongPackPath = finalSongPackPath.Replace("_p.psarc", "_p.disabled.psarc");
                                        songPackPath = songPackPath.Replace("_p.psarc", "_p.disabled.psarc");

                                        if (File.Exists(finalSongPackPath))
                                            File.Delete(finalSongPackPath);
                                        File.Copy(songPackPath, finalSongPackPath);
                                    }

                                    dgvOfficialSongs.Rows.Remove(officialSPRow);

                                    var song = new SongData { Song = songPack.Replace(".psarc", ""), Artist = "Ubisoft", Author = "Ubisoft", Path = finalSongPackPath };
                                    SongCollection.Add(song);
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

        private void btn_UploadCDLC_Click(object sender, EventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/creators/submit");
        }

        private void checkAllForUpdates(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(3000);
            counterStopwatch.Start();
            Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = false; });

            Extensions.InvokeIfRequired(dgvSongs, delegate
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

        private void checkEnableLogBaloon_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEnableLogBaloon.Checked)
                myLog.RemoveTargetNotifyIcon(notifyIcon_Main);
            else
                myLog.AddTargetNotifyIcon(notifyIcon_Main);
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += CheckForUpdatesEvent;
            bWorker.RunWorkerAsync();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bWorker.IsBusy)
                bWorker.CancelAsync();
            Application.Exit();
        }

        private void deleteSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you really want to remove this CDLC? This cannot be undone?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    File.Delete(SongCollection[dgvSongs.SelectedRows[0].Index].Path);
                    SongCollection.RemoveAt(dgvSongs.SelectedRows[0].Index);
                    dgvSongs.Rows.RemoveAt(dgvSongs.SelectedRows[0].Index);
                }
            }
            catch (IOException ex)
            {
                myLog.Write("<ERROR>:" + ex.Message);
            }
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
                {
                    if (tbUnsortedSearch.Text != "Search")
                        RefreshSelectedSongs(tbUnsortedSearch.Text.ToLower());
                    else
                        RefreshSelectedSongs();
                };

            bWorker.RunWorkerAsync();
        }

        private void dgvSongs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1) //if it's not header
                ShowSongInfo();
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

        private void dgvSongs_DataSourceChanged(object sender, EventArgs e)
        {
            var dataGridViewColumn = ((DataGridView)sender).Columns["Preview"];
            if (dataGridViewColumn != null && dataGridViewColumn.Visible)
                dataGridViewColumn.Visible = false;

            // PopulateColumnList();
            // source of LRBV column headers during rescan
            ShowHideArrangementColumns();
        }

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

        private void doRenameSongs(object sender, DoWorkEventArgs e)
        {
            if (!bWorker.CancellationPending)
            {
                renameSongs(SortedSongCollection, renameTemplateTextBox.Text, deleteEmptyDirCheckBox.Checked);
            }
        }

        private void editDLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: if nothing changed, just close or serialize default settings.
            if (dgvSongs != null)
            {
                Log("Shutting down ...");
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

        private void getAuthorNameStripMenuItem_Click(object sender, EventArgs e)
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    if (dgvSongs.SelectedRows.Count > 0)
                    {
                        UpdateAuthor(dgvSongs.SelectedRows[0]);
                    }
                });
        }

        private void lbl_ShowHideLog_Click(object sender, EventArgs e)
        {
            scMain.Panel2Collapsed = !scMain.Panel2Collapsed;
            lbl_ShowHideLog.Text = scMain.Panel2Collapsed ? "Show Log" : "Hide Log";
        }

        private void lbl_UnleashedRole_Click(object sender, EventArgs e)
        {
        }

        private void linkCFFAQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/faq/");
        }

        private void linkDonationsPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/donate/");
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
            bWorker.RunWorkerCompleted += (se, ev) => { allSelected = !allSelected; };
            bWorker.RunWorkerAsync();
        }

        private void linkOpenCFHomePage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/");
        }

        private void linkOpenCFVideos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/videos/");
        }

        private void linkOpenIgnition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ignition.customsforge.com/");
        }

        private void linkOpenRequests_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://requests.customsforge.com/?b");
        }

        private void linkOpenSngMgrHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager_Winforms.Resources.SngMgrHelp.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                MessageBox.Show(reader.ReadToEnd(), "Help");
            }
        }

        private void link_Alex360Profile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/20759-zerkz/");
        }

        private void link_CFManager_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/forum/81-customsforge-song-manager/");
        }

        private void link_DarjuszProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/5299-darjusz/");
        }

        private void link_LovromanProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/43194-lovroman/");
        }

        private void link_MainClearResults_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
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
            Extensions.InvokeIfRequired(tbSearch, delegate { tbSearch.Text = ""; });
        }

        private void link_UnleashedProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://customsforge.com/user/1-unleashed2k/");
        }

        private void listDisabledColumns_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            DataGridViewColumn column = dgvSongs.Columns[e.Item.SubItems[1].Text];
            if (column != null)
            {
                column.Visible = e.Item.Checked;
                column.Width = Convert.ToInt16(e.Item.SubItems[3].Text);
            }
        }

        private void lnk_ReleaseNotes_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var releaseNotes = new frmReleaseNotes();
            releaseNotes.ShowDialog();
        }

        private void lnk_SettingsSelectAllColumns_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool deselect = listDisabledColumns.Items[1].Checked;

            for (int i = 1; i < listDisabledColumns.Items.Count; i++)
            {
                listDisabledColumns.Items[i].Checked = !deselect;
            }
        }

        private void openDLCLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = dgvSongs.SelectedRows[0].Cells["Path"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
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

        private void renameAllButton_Click(object sender, EventArgs e)
        {
            SortedSongCollection = SongCollection.ToList();
            if (!renameTemplateTextBox.Text.Contains("<title>"))
            {
                MessageBox.Show("Rename Template requires <title> at least one to prevent overwriting songs.");
                return;
            }
            if (SortedSongCollection == null || SortedSongCollection.Count == 0)
            {
                MessageBox.Show("Please scan in at least one song.");
                return;
            }
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += doRenameSongs;
            bWorker.DoWork += PopulateListHandler;
            bWorker.RunWorkerCompleted += PopulateCompletedHandler;

            bWorker.RunWorkerAsync();
        }

        private void showDLCInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
            {
                SearchDLC(tbSearch.Text);
            }
            else
            {
                dgvSongs.DataSource = new BindingSource().DataSource = SongCollection;
            }
        }

        private void tbSettingsRSDir_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbSettingsRSDir.Enabled)
            {
                // TODO: customize
                if (folderBrowserDialog_SettingsRSPath.ShowDialog() == DialogResult.OK)
                {
                    tbSettingsRSDir.Text = folderBrowserDialog_SettingsRSPath.SelectedPath;
                    mySettings.RSInstalledDir = tbSettingsRSDir.Text;
                }
            }
        }

        private void tbUnsortedSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbUnsortedSearch.Text != "Search")
            {
                dgvUnsortedDLCs.Rows.Clear();
                var matchingSongs = SongCollection.Where(sng => sng.Artist.ToLower().Contains(tbUnsortedSearch.Text.ToLower()) || sng.Song.ToLower().Contains(tbUnsortedSearch.Text.ToLower()) && Path.GetFileName(Path.GetDirectoryName(sng.Path)) == "dlc");

                if (matchingSongs.Where(sng => sng.Path.Contains(".disabled")).Count() > matchingSongs.Where(sng => !sng.Path.Contains(".disabled")).Count())
                {
                    foreach (var song in matchingSongs)
                    {
                        if (song.Path.Contains(".disabled"))
                            dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Path);
                        else
                            dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Path);
                    }
                }
                else
                {
                    foreach (var song in matchingSongs)
                    {
                        if (!song.Path.Contains(".disabled"))
                            dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Path);
                        else
                            dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Path);
                    }
                }

                foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colUnsortedEnabled"].Value.ToString() == "No"))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }

                if (checkSearchInAllSetlists.Checked)
                {
                    dgvSetlists.Rows.Clear();
                    string dlcFolderPath = Path.Combine(mySettings.RSInstalledDir, "dlc");

                    var dirs = Directory.EnumerateDirectories(Path.Combine(mySettings.RSInstalledDir, "dlc"), "*", SearchOption.TopDirectoryOnly);
                    foreach (var setlist in dirs)
                    {
                        if (SetlistContainsSong(setlist, tbUnsortedSearch.Text.ToLower()))
                        {
                            if (!SetlistEnabled(setlist))
                                dgvSetlists.Rows.Add(false, "No", Path.GetFileName(setlist.Replace("-disabled", "")));
                            else if (SetlistModified(setlist))
                                dgvSetlists.Rows.Add(false, "Modded", Path.GetFileName(setlist));
                            else
                                dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlist));
                        }
                    }

                    if (dgvSetlists.Rows.Count > 0)
                        dgvSetlists.Rows[0].Selected = true;

                    RefreshSelectedSongs(tbUnsortedSearch.Text.ToLower());
                }
            }
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // populate tab control item only as needed for efficiency
            // get first four charters from tab control text
            switch (tcMain.SelectedTab.Text.Substring(0, 4).ToUpper())
            {
                case "SONG":
                    // do nothing for now
                    break;
                case "DUPL": // works with Duplicates(n)                    
                    PopulateDupeList();
                    break;
                case "BATC":
                    PopulateRenamer();
                    break;
                case "SETL":
                    PopulateSongList();
                    break;
                case "UTIL":
                    // do nothing for now
                    break;
                case "SETT":
                    PopulateSettings();
                    break;
                case "ABOU":
                    // do nothing for now
                    break;
            }

            Refresh();
        }

        private void timerAutoUpdate_Tick(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void toolStripStatusLabel_ClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private void toolStripStatusLabel_MainCancel_Click(object sender, EventArgs e)
        {
            bWorker.CancelAsync();
            bWorker.Abort();
            //bWorker.Dispose();
            //bWorker = null;
            toolStripStatusLabel_MainCancel.Visible = false;
        }

        // taken from CGT used with permission
        // avoid abrupt bwWorker disposal on e.Cancelled
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (toolStripStatusLabel_MainCancel.Visible)
            {
                bWorker.CancelAsync();
                e.Cancel = true;
                return;
            }

            base.OnFormClosing(e);
        }


        #region Class Methods

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

        #endregion

    }
}