using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.SongEditor;
using CustomsForgeSongManager.UITheme;
using GenTools;
using DataGridViewTools;
using Newtonsoft.Json;
using System.Xml;
using CustomsForgeSongManager.Properties;
using System.Net.Cache;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using BetterDialog2 = CustomControls.BetterDialog2;
using System.Threading.Tasks;
using UserProfileLib;
using System.Resources;
using System.Reflection;
using Newtonsoft.Json.Linq;
using RocksmithToolkitLib;
using System.Globalization;

// TODO: convert SongManager, Duplicates, SetlistManager to use a common bound FilterBindingList<SongData>() dataset.
// TODO: use binding source filtering to show/hide data

namespace CustomsForgeSongManager.UControls
{
    public partial class SongManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        public Delegate PlaySongFunction;
        private Bitmap MinusBitmap = new Bitmap(Properties.Resources.minus);
        private Bitmap PlusBitmap = new Bitmap(Properties.Resources.plus);
        private Color _Disabled = Color.Red;
        private Color _Enabled = Color.Lime;
        private bool allSelected = false;
        private AbortableBackgroundWorker bWorker;
        private bool bindingCompleted = false;
        private Stopwatch counterStopwatch;
        private bool dgvPainted = false;
        private int firstIndex = 0;
        private bool ignoreCheckStateChanged = false;
        private bool keepOpen;
        private string lastSelectedSongPath = String.Empty;
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private List<SongData> songList = new List<SongData>(); // non-binding prevents filtering from being inherited
        private DgvStatus statusSongsMaster = new DgvStatus();

        public SongManager()
        {
            InitializeComponent();
            // TODO: fix custom control ToolStripNumUpDown.cs
            // hard coded these because a NumericUpDown Control bug keeps deleting settings in VS IDE
            this.tsmiAddDDNumericUpDown.Text = "Phrase Length";
            this.tsmiAddDDNumericUpDown.TextVisible = true;
            this.tsmiNudScrollSpeed.Text = "(Default 1.3)";
            this.tsmiNudScrollSpeed.TextVisible = true;
            // POC audio volume adjustment variables (workaround prevents IDE changes)
            tsmiCorrectionFactor.DecimalValue = (decimal)1.0;
            tsmiCorrectionFactor.Increment = (decimal)0.1;
            tsmiCorrectionFactor.Maximum = (decimal)1.0;
            tsmiCorrectionFactor.Minimum = (decimal)-1.0;
            //
            tsmiCorrectionMultiplier.DecimalValue = (decimal)-1.0;
            tsmiCorrectionMultiplier.Increment = (decimal)0.1;
            tsmiCorrectionMultiplier.Maximum = (decimal)5.0;
            tsmiCorrectionMultiplier.Minimum = (decimal)-5.0;
            //
            tsmiTargetAudioVolume.DecimalValue = (decimal)-7.0;
            tsmiTargetAudioVolume.Increment = (decimal)0.1;
            tsmiTargetAudioVolume.Maximum = (decimal)30.0;
            tsmiTargetAudioVolume.Minimum = (decimal)-30.0;
            //
            tsmiTargetPreviewVolume.DecimalValue = (decimal)-5.0;
            tsmiTargetPreviewVolume.Increment = (decimal)0.1;
            tsmiTargetPreviewVolume.Maximum = (decimal)30.0;
            tsmiTargetPreviewVolume.Minimum = (decimal)-30.0;
            //
            tsmiTargetToneVolume.DecimalValue = (decimal)-20.0;
            tsmiTargetToneVolume.Increment = (decimal)0.1;
            tsmiTargetToneVolume.Maximum = (decimal)30.0;
            tsmiTargetToneVolume.Minimum = (decimal)-30.0;
            //
            tsmiTargetLUFS.DecimalValue = (decimal)-16.0;
            tsmiTargetLUFS.Increment = (decimal)0.1;
            tsmiTargetLUFS.Maximum = (decimal)30.0;
            tsmiTargetLUFS.Minimum = (decimal)-30.0;
            //
            dgvSongsDetail.Visible = false;
            // TODO: future get Ignition based API data
            cmsCheckForUpdate.Visible = GeneralExtension.IsInDesignMode ? true : false;
            cmsGetCharterName.Visible = GeneralExtension.IsInDesignMode ? true : false;
            cmsOpenSongPage.Visible = GeneralExtension.IsInDesignMode ? true : false;
            toolStripSeparator11.Visible = GeneralExtension.IsInDesignMode ? true : false;

            PopulateSongManager(); // check SongData version first

            PopulateTagger();
            InitializeRepairMenu();
            tsmiRepairs.HideDropDown();
            UserSupport();
            this.TabEnter();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;

            // POC Audio Volume Correction Validation Event Handlers
            //tsmiCorrectionFactor.KeyUp += new KeyEventHandler(tsmiAudio_KeyUp);
            //tsmiCorrectionFactor.MouseUp += new MouseEventHandler(tsmiAudio_MouseUp);
            //tsmiCorrectionMultiplier.KeyUp += new KeyEventHandler(tsmiAudio_KeyUp);
            //tsmiCorrectionMultiplier.MouseUp += new MouseEventHandler(tsmiAudio_MouseUp);

            // developer sandbox
            tsmiDevUseOnly.Visible = GeneralExtension.IsInDesignMode ? true : false;
        }

        public void DoWork(string workDescription, dynamic workerParm1 = null, dynamic workerParm2 = null, dynamic workerParm3 = null, dynamic workerParm4 = null)
        {
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = workDescription;
                gWorker.WorkParm1 = workerParm1;
                gWorker.WorkParm2 = workerParm2;
                gWorker.WorkParm3 = workerParm3;
                gWorker.WorkParm4 = workerParm4;
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }
        }

        public void PlaySelectedSong()
        {
            var song = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
            if (song != null)
            {
                if (song.FileName.ToLower().EndsWith(Constants.BASESONGS) || song.FileName.ToLower().EndsWith(Constants.BASESONGSDISABLED) || song.FileName.ToLower().Contains(Constants.RS1COMP) || song.FileName.ToLower().Contains(Constants.SONGPACK) || song.FileName.ToLower().Contains(Constants.ABVSONGPACK))
                {
                    Globals.Log("<WARNING> Audio from SongPacks is not available for playback ...");
                    return;
                }

                if (String.IsNullOrEmpty(song.AudioCache))
                    song.AudioCache = String.Format("{0}_{1}", Guid.NewGuid().ToString().Replace("-", ""), song.FileSize);

                var audioCacheDir = Constants.AudioCacheFolder;
                if (!Directory.Exists(audioCacheDir))
                    Directory.CreateDirectory(audioCacheDir);

                var fullname = Path.Combine(audioCacheDir, song.AudioCache + ".ogg");

                if (File.Exists(fullname))
                {
                    bool canDelete = false;
                    //check if it needs to be updated using song filesize,
                    if (!song.AudioCache.Contains("_"))
                        canDelete = true;
                    else
                    {
                        var ss = song.AudioCache.Split('_');
                        if (ss[1] != song.FileSize.ToString())
                            canDelete = true;
                    }
                    if (canDelete)
                        File.Delete(fullname);
                }

                if (!File.Exists(fullname))
                {
                    //extract the audio...
                    if (!PsarcBrowser.ExtractAudio(song.FilePath, fullname, ""))
                    {
                        Globals.Log(Properties.Resources.CouldNotExtractTheAudio);
                        song.AudioCache = String.Empty;
                        return;
                    }
                }

                if (!File.Exists(fullname))
                {
                    song.AudioCache = String.Empty;
                    return;
                }

                if (Globals.AudioEngine.OpenAudioFile(fullname))
                {
                    //float inGameVolume = (float)Math.Pow(10, sng.SongVolume / 10) - 1; // if in dB
                    //Globals.AudioEngine.SetVolume(1.0f - sng.SongVolume);

                    Globals.AudioEngine.Play();
                    Globals.Log(String.Format("Playing {0} by {1} ... ({2})", song.Title.Trim(), song.Artist.Trim(), Path.GetFileName(song.FilePath)));
                }
                else
                    Globals.Log("Unable to open audio file.");
            }
        }

        public void PopulateSongManager()
        {
            Globals.Log("Populating SongManager GUI ...");
            // Hide main dgvSongsMaster until load completes
            dgvSongsMaster.Visible = false;

            InitializeSongsMaster(); // reloads settings

            //Load Song Collection from file
            if (!LoadSongCollectionFromFile())
                return;

            // Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvSongsMaster.Columns.Cast<DataGridViewColumn>().Where(col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvSongsMaster.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
        }

        public void SaveSongCollectionToFile()
        {
            var dom = Globals.MasterCollection.XmlSerializeToDom();
            XmlElement versionNode = dom.CreateElement("SongDataList");
            versionNode.SetAttribute("version", SongData.SongDataVersion);
            versionNode.SetAttribute("AppVersion", Constants.CustomVersion());
            dom.DocumentElement.AppendChild(versionNode);

            foreach (XmlElement songData in dom.GetElementsByTagName("ArrayOfSongData")[0].ChildNodes)
            {
                // reduce songInfo.xml file size by removing extraneous/null/empty elements
                var arrangementsNode = songData.GetElementsByTagName("Arrangements")[0];
                if (arrangementsNode != null)
                {
                    var arrNodes = arrangementsNode.ChildNodes.OfType<XmlNode>().ToList();

                    foreach (var arrNode in arrNodes)
                    {
                        var isVocals = arrNode.InnerXml.Contains("<Name>Vocals</Name>");
                        var innerNodes = arrNode.ChildNodes.OfType<XmlNode>().ToList();

                        foreach (var n in innerNodes)
                        {
                            // remove analyzer data from vocals
                            if (n.InnerText == "0" && isVocals)
                                arrNode.RemoveChild(n);

                            // remove null/empty data
                            if (String.IsNullOrEmpty(n.InnerText))
                                arrNode.RemoveChild(n);
                        }
                    }
                }
            }

            dom.Save(Constants.SongsInfoPath);
            Globals.Log("Saved File: " + Path.GetFileName(Constants.SongsInfoPath));
        }

        public RepairOptions SetRepairOptions()
        {
            var ro = new RepairOptions();
            ro.SkipRemastered = tsmiSkipRemastered.Checked;
            ro.AddDD = tsmiRepairsAddDD.Checked;
            ro.PhraseLength = (int)tsmiAddDDNumericUpDown.DecimalValue;
            ro.RemoveSustain = tsmiAddDDRemoveSustain.Checked;
            ro.CfgPath = String.IsNullOrEmpty(tsmiAddDDCfgPath.Tag.ToString()) ? "" : tsmiAddDDCfgPath.Tag.ToString();
            ro.RampUpPath = String.IsNullOrEmpty(tsmiAddDDRampUpPath.Tag.ToString()) ? "" : tsmiAddDDRampUpPath.Tag.ToString();
            ro.OverwriteDD = tsmiOverwriteDD.Checked;
            ro.RepairMastery = tsmiRepairsMastery.Checked;
            ro.PreserveStats = tsmiRepairsPreserveStats.Checked;
            ro.UsingOrgFiles = tsmiRepairsUsingOrg.Checked;
            ro.IgnoreMultitone = tsmiRepairsMultitone.Checked;
            ro.RepairMaxFive = tsmiRepairsMaxFive.Checked;
            ro.RemoveNDD = tsmiRemoveNDD.Checked;
            ro.RemoveBass = tsmiRemoveBass.Checked;
            ro.RemoveGuitar = tsmiRemoveGuitar.Checked;
            ro.RemoveBonus = tsmiRemoveBonus.Checked;
            ro.RemoveMetronome = tsmiRemoveMetronome.Checked;
            ro.IgnoreStopLimit = tsmiIgnoreStopLimit.Checked;
            ro.RemoveSections = tsmiRemoveSections.Checked;
            ro.AdjustScrollSpeed = tsmiAdjustScrollSpeed.Checked;
            ro.ScrollSpeed = tsmiNudScrollSpeed.DecimalValue;
            ro.FixLowBass = tsmiFixLowBass.Checked;
            ro.FixAppId = tsmiFixAppId.Checked;
            ro.DLFolderProcess = tsmiDLFolderProcess.Checked;
            ro.DLFolderMonitor = tsmiDLFolderMonitor.Checked;
            ro.SkipDupes = tsmiSkipDupes.Checked;

            AppSettings.Instance.RepairOptions = ro;
            return ro;
        }

        public void UpdateToolStrip()
        {
            chkIncludeSubfolders.Checked = AppSettings.Instance.IncludeSubfolders;
            chkProtectODLC.Checked = AppSettings.Instance.ProtectODLC;
            cueSearch.Text = AppSettings.Instance.SearchString;

            // save settings incase user made any changes to grid
            if (Globals.RescanSongManager || Globals.ReloadSongManager)
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);

            if (Globals.RescanSongManager)
            {
                // full rescan of song collection
                Settings.ToogleRescan(false);
                Rescan(true);
                PopulateSongManager();
            }
            else if (Globals.ReloadSongManager)
            {
                // smart quick rescan of song collection
                Globals.ReloadSongManager = false;
                Rescan(false);
                PopulateSongManager();
            }

            IncludeSubfolders(false);
            ProtectODLC();

            try
            {
                // apply saved search (filters can not be applied the same way)
                if (!String.IsNullOrEmpty(AppSettings.Instance.SearchString))
                {
                    SearchCDLC(AppSettings.Instance.SearchString);
                    Thread.Sleep(200); // debounce search
                    dgvSongsMaster.AllowUserToAddRows = false; // corrects initial Song Count
                }
            }
            catch (Exception ex)
            {
                Globals.Log("<ERROR> Save Search caused exception ...");
                Globals.Log("<ERROR> Saved Search: " + ex.Message);
                ClearSearch();
            }

            if (!AppSettings.Instance.IncludeArrangementData)
                colSongAverageTempo.ToolTipText = "Use Arrangement Analyzer, Rescan\r\nFull to confirm BPM accuracy";
            else
                colSongAverageTempo.ToolTipText = "";

            // there is a periodic issue where the datagrid is empty
            // force rescan in case user deleted songInfo.xml or switch to/from MacMode
            if (dgvSongsMaster.Rows.Count == 0 && String.IsNullOrEmpty(cueSearch.Text) && !AppSettings.Instance.FirstRun)
            {
                Globals.RescanSongManager = true;
                UpdateToolStrip();
            }

            Globals.TsLabel_MainMsg.Text = String.Format("Rocksmith Song Count: {0}", dgvSongsMaster.Rows.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            numberOfDisabledDLC = songList.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;
            var tsldcMsg = String.Format("Outdated: {0} | Disabled CDLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
            // TemporaryDisableDatabindEvent(() => { dgvSongsMaster.Refresh(); });
        }

        private void CheckForUpdatesEvent(object o, DoWorkEventArgs args)
        {
            // part of ContextMenuStrip action
            GenExtensions.InvokeIfRequired(dgvSongsMaster, delegate
            {
                if (dgvSongsMaster.SelectedRows.Count > 0)
                {
                    CheckRowForUpdate(dgvSongsMaster.SelectedRows[0]);
                }
            });
        }

        private void CheckRowForUpdate(DataGridViewRow dataGridViewRow)
        {
            // part of ContextMenuStrip action
            if (bWorker.CancellationPending)
                return;

            var sd = DgvExtensions.GetObjectFromRow<SongData>(dataGridViewRow);
            if (sd == null || sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc)
                return;

            //currentSong.IgnitionVersion = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "version");
            string url = sd.GetInfoURL();
            //string response = String.Empty;
            //string cfUrl = String.Empty;
            //int version = 0;

            string auth_token = String.Empty;

            var body = new { grant_type = "client_credentials", client_id = "CFSM", client_secret = "snIsh4bir9Il4woTh1aG6jiD5Ag8An" };
            using (var client = new WebClient())
            {
                var dataString = JsonConvert.SerializeObject(body);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                string authresponse = client.UploadString(new Uri(Constants.DefaultAuthURL), "POST", dataString);
                dynamic deserialized = JsonConvert.DeserializeObject(authresponse);
                auth_token = deserialized.access_token;
            }

            using (WebClient client = new WebClient())
            {
                const string myParametersTemplate = "filters[artist]={artist}&filters[album]={album}&filters[title]={title}&per_page=1";
                string myParameters = myParametersTemplate.Replace("{artist}", sd.Artist).Replace("{album}", sd.Album).Replace("{title}", sd.Title);

                string clientURL = string.Concat(url, "?", myParameters);

                string authHeader = string.Concat("Bearer ", auth_token);

                client.Headers[HttpRequestHeader.ContentType] = "Content-Type: application/json";
                client.Headers.Add("Authorization", authHeader);

                if (client.Headers["Authorization"] == null)
                    throw new Exception("Cannot add auth header");
                else if (string.IsNullOrEmpty(client.Headers["Authorization"]))
                    throw new Exception("Header auth value is empty");

                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                dynamic HtmlResult = client.UploadString(clientURL, string.Empty);

                //currentSong.IgnitionID = HtmlResult.data.id;
                //currentSong.IgnitionUpdated = HtmlResult.data.updated;
                //currentSong.IgnitionVersion = HtmlResult.data.version;
                //currentSong.IgnitionAuthor = HtmlResult.data.name;                    
            }
        }

        private void ColumnMenuItemClick(object sender, EventArgs eventArgs)
        {
            ToolStripMenuItem currentContextMenuItem = sender as ToolStripMenuItem;
            if (currentContextMenuItem != null)
            {
                if (!string.IsNullOrEmpty(currentContextMenuItem.Tag.ToString()))
                {
                    var dataGridViewColumn = dgvSongsMaster.Columns[currentContextMenuItem.Tag.ToString()];
                    if (dataGridViewColumn != null)
                    {
                        var columnIndex = dataGridViewColumn.Index;
                        var columnSetting = RAExtensions.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvSongsMaster.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                        }
                    }
                }
            }
        }

        private void GetRepairOptions()
        {
            ignoreCheckStateChanged = true;
            tsmiSkipRemastered.Checked = AppSettings.Instance.RepairOptions.SkipRemastered;
            tsmiRepairsMastery.Checked = AppSettings.Instance.RepairOptions.RepairMastery;
            tsmiRepairsPreserveStats.Checked = AppSettings.Instance.RepairOptions.PreserveStats;
            tsmiModsPreserveStats.Checked = AppSettings.Instance.RepairOptions.PreserveStats;
            tsmiRepairsUsingOrg.Checked = AppSettings.Instance.RepairOptions.UsingOrgFiles;
            tsmiRepairsMultitone.Checked = AppSettings.Instance.RepairOptions.IgnoreMultitone;
            tsmiRepairsMaxFive.Checked = AppSettings.Instance.RepairOptions.RepairMaxFive;
            tsmiRemoveNDD.Checked = AppSettings.Instance.RepairOptions.RemoveNDD;
            tsmiRemoveBass.Checked = AppSettings.Instance.RepairOptions.RemoveBass;
            tsmiRemoveGuitar.Checked = AppSettings.Instance.RepairOptions.RemoveGuitar;
            tsmiRemoveBonus.Checked = AppSettings.Instance.RepairOptions.RemoveBonus;
            tsmiRemoveMetronome.Checked = AppSettings.Instance.RepairOptions.RemoveMetronome;
            tsmiIgnoreStopLimit.Checked = AppSettings.Instance.RepairOptions.IgnoreStopLimit;
            tsmiAdjustScrollSpeed.Checked = AppSettings.Instance.RepairOptions.AdjustScrollSpeed;
            tsmiNudScrollSpeed.DecimalValue = AppSettings.Instance.RepairOptions.ScrollSpeed;
            tsmiRemoveSections.Checked = AppSettings.Instance.RepairOptions.RemoveSections;
            tsmiFixLowBass.Checked = AppSettings.Instance.RepairOptions.FixLowBass;
            tsmiFixAppId.Checked = AppSettings.Instance.RepairOptions.FixAppId;
            tsmiDLFolderProcess.Checked = AppSettings.Instance.RepairOptions.DLFolderProcess;
            tsmiRepairsAddDD.Checked = AppSettings.Instance.RepairOptions.AddDD;
            tsmiOverwriteDD.Checked = AppSettings.Instance.RepairOptions.OverwriteDD;
            tsmiAddDDNumericUpDown.DecimalValue = AppSettings.Instance.RepairOptions.PhraseLength;
            tsmiAddDDRemoveSustain.Checked = AppSettings.Instance.RepairOptions.RemoveSustain;

            tsmiAddDDCfgPath.Tag = AppSettings.Instance.RepairOptions.CfgPath;
            if (!String.IsNullOrEmpty(tsmiAddDDCfgPath.Tag.ToString()))
                tsmiAddDDCfgPath.Text = Path.GetFileName(tsmiAddDDCfgPath.Tag.ToString());

            tsmiAddDDRampUpPath.Tag = AppSettings.Instance.RepairOptions.RampUpPath;
            if (!String.IsNullOrEmpty(tsmiAddDDRampUpPath.Tag.ToString()))
                tsmiAddDDRampUpPath.Text = Path.GetFileName(tsmiAddDDRampUpPath.Tag.ToString());

            ignoreCheckStateChanged = false;

            tsmiSkipDupes.Checked = AppSettings.Instance.RepairOptions.SkipDupes; //NOTE: make sure that all other (new) repair options are set before tsmiDLFolderMonitor.Checked is set, because it calls a different OnCheckedChanged Event (which starts the Monitoring Process) and which will mess up saved settings

            // starts/stops DL folder monitoring
            tsmiDLFolderMonitor.Checked = AppSettings.Instance.RepairOptions.DLFolderMonitor;
        }

        private void IncludeSubfolders(bool clearSearchBox = true)
        {
            // search killer
            if (clearSearchBox)
            {
                cueSearch.Text = String.Empty;
                AppSettings.Instance.SearchString = String.Empty;
            }

            songList = Globals.MasterCollection.ToList();

            if (!chkIncludeSubfolders.Checked)
                songList = songList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            if (tsmiModsMyCDLC.Checked)
                songList = (songList.Where(x => x.PackageAuthor == AppSettings.Instance.CharterName).ToList());

            // remove inlays JIC
            songList = songList.Where(fi => !fi.FileName.ToLower().Contains("inlay")).ToList();

            LoadFilteredBindingList(songList);
        }

        private void InitializeRepairMenu()
        {
            // restore saved audio options (POC)
            GetAudioOptions();

            // restore saved repair options
            GetRepairOptions();

            var items = tsmiRepairs.DropDownItems;
            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>().Where(item => item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton))
                ToggleRepairMenu(item);
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with dropdown filtering
            dgvSongsMaster.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvSongsMaster.DataSource = bs;
        }

        private bool LoadSongCollectionFromFile()
        {
            bool correctVersion = false;

            try
            {
                // load songsInfo.xml if it exists 
                if (File.Exists(Constants.SongsInfoPath))
                {
                    XmlDocument dom = new XmlDocument();
                    dom.Load(Constants.SongsInfoPath);
                    Globals.Log("Loaded File: " + Path.GetFileName(Constants.SongsInfoPath));

                    // remove version info node
                    var listNode = dom["ArrayOfSongData"];
                    if (listNode != null)
                    {
                        var versionNode = listNode["SongDataList"];
                        if (versionNode != null)
                        {
                            if (versionNode.HasAttribute("version"))
                                correctVersion = (versionNode.GetAttribute("version") == SongData.SongDataVersion);

                            listNode.RemoveChild(versionNode);
                        }

                        songList = SerialExtensions.XmlDeserialize<List<SongData>>(listNode.OuterXml);

                        if (songList == null || songList.Count == 0)
                            throw new SongCollectionException(songList == null ? "Master Collection = null" : "Master Collection.Count = 0");

                        Globals.MasterCollection = new BindingList<SongData>(songList);
                    }
                }

                // smart scan
                if (correctVersion && !AppSettings.Instance.FirstRun)
                {
                    Globals.Log("Performing quick rescan of song collection ...");
                    Rescan(false);
                }
                else
                {
                    var hdrMsg = "CFSM Initial Run ...";
                    if (File.Exists(Constants.SongsInfoPath))
                    {
                        hdrMsg = "CFSM New SongData Version ...";
                        Globals.Log("<WARNING> Incorrect song collection version found ...");
                    }

                    try
                    {
                        // DO NOT use the bulldozer here
                        // 'My Documents/CFSM' may contain some original files
                        ZipUtilities.RemoveReadOnlyAttribute(Constants.WorkFolder);
                        GenExtensions.DeleteFile(Constants.SongsInfoPath);
                        GenExtensions.DeleteFile(Constants.AppSettingsPath);
                        GenExtensions.DeleteDirectory(Constants.GridSettingsFolder);
                        GenExtensions.DeleteDirectory(Constants.TaggerWorkingFolder);
                    }
                    catch (Exception ex)
                    {
                        Globals.Log("<ERROR> Cleaning " + Path.GetFileName(Constants.WorkFolder) + " : " + ex.Message);
                    }

                    // auto show release notes when there are significant or SongData revisions
                    frmNoteViewer.ViewExternalFile("ReleaseNotes.txt", "Release Notes");

                    // switch to Setting tab so user can customize first run settings
                    var diaMsg = "CFSM will now switch to the Settings tabmenu." + Environment.NewLine +
                                 "Please customize the CFSM Settings options" + Environment.NewLine +
                                 "before returning to the Song Manager tab." + Environment.NewLine;

                    BetterDialog2.ShowDialog(diaMsg, hdrMsg, null, null, "Ok", Bitmap.FromHicon(SystemIcons.Information.Handle), "ReadMe", 0, 150);
                    Globals.DgvCurrent = dgvSongsMaster;

                    // selects Settings tabmenu even if tab order is changed
                    var tabIndex = Globals.MainForm.tcMain.TabPages.IndexOf(Globals.MainForm.tpSettings);
                    Globals.MainForm.tcMain.SelectedIndex = tabIndex;
                    Globals.Log("Customize CFSM Settings options before returning to Song Manager ...");
                }

                // Rescan calls BackgroundScan/ParseSongs and loads Globals.MasterCollection
                // and local songCollection is loaded with Globals.MasterCollection
            }
            catch (Exception ex)
            {
                // failsafe ... delete My Documents/CFSM folder and files with option not to delete
                var diaMsg = "A fatal CFSM application error has occured." + Environment.NewLine +
                    "You are about to delete all work files created" + Environment.NewLine +
                    "by CFSM, including any backups of CDLC files." + Environment.NewLine +
                    "Deletion is permenant and can not be undone." + Environment.NewLine +
                    "Do you want to continue?";

                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete 'My Documents/CFSM' ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                {
                    Globals.Log("User aborted deleting CFSM folder and subfolders from My Documents ...");
                    Environment.Exit(0);
                }

                string err = ex.Message;
                if (ex.InnerException != null)
                    err += ", Inner: " + ex.InnerException.Message;

                // log message needs to written before it is deleted ... Bazinga
                Globals.Log("<Error>: " + ex.Message);
                Globals.Log("Deleted CFSM folder and subfolders from My Documents ...");

                // use the bulldozer
                ZipUtilities.RemoveReadOnlyAttribute(Constants.WorkFolder);
                GenExtensions.DeleteDirectory(Constants.WorkFolder);
                FileTools.VerifyCfsmFolders();

                MessageBox.Show(String.Format("{0}{1}{1}CFSM will now shut down.", err, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
                return false;
            }

            return true;
        }

        private void InitializeSongsMaster()
        {
            // respect processing order
            DgvExtensions.DoubleBuffered(dgvSongsMaster);
            CFSMTheme.InitializeDgvAppearance(dgvSongsMaster);
            // reload column order, width, visibility, and settings
            Globals.Settings.LoadSettingsFromFile(dgvSongsMaster, true);

            if (RAExtensions.ManagerGridSettings != null)
                dgvSongsMaster.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            else
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {
            // fix for contextual menu bug 'Object reference not set to an instance of an object.' 
            // that occur on startup when dgv settings have not yet been saved       
            if (RAExtensions.ManagerGridSettings == null)
            {
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
                Globals.Settings.LoadSettingsFromFile(dgvSongsMaster);
                dgvSongsMaster.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            }

            contextMenuStrip.Items.Clear();
            foreach (ColumnOrderItem columnOrderItem in RAExtensions.ManagerGridSettings.ColumnOrder)
            {
                var cn = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);

                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick) { Checked = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Visible, Tag = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void PopulateTagger()
        {
            tsmiTagStyle.DropDownItems.Clear();
            var tagStyle = Globals.Tagger.Themes;
            var items = new ToolStripEnhancedMenuItem[tagStyle.Count];

            for (int i = 0; i < tagStyle.Count; i++)
            {
                items[i] = new ToolStripEnhancedMenuItem();
                items[i].Name = tagStyle[i];
                items[i].Text = tagStyle[i];
                items[i].AssociatedEnumValue = null;
                items[i].CheckMarkDisplayStyle = CheckMarkDisplayStyle.RadioButton;
                items[i].CheckOnClick = true;
                items[i].RadioButtonGroupName = null;
                items[i].Click += TagStyle_Click;
            }

            tsmiTagStyle.DropDownItems.AddRange(items);
            // select DefaultThemeName
            ((ToolStripMenuItem)tsmiTagStyle.DropDownItems[TaggerTools.DefaultThemeName]).Checked = true;
            cmsTaggerPreview.DropDownItems.Clear();

            foreach (string tagPreview in Globals.Tagger.Themes)
            {
                var tsmi = new ToolStripEnhancedMenuItem();
                tsmi.Name = tagPreview;
                tsmi.Text = tagPreview;
                tsmi.ToolTipText = "Left Mouse Click to preview tagged album arwork." + Environment.NewLine +
                                   "Right Mouse Click to see tag template information." + Environment.NewLine +
                                   "Ctrl + Right Mouse Click to see tag example image.";

                cmsTaggerPreview.DropDownItems.Add(tsmi);

                tsmi.MouseUp += (s, e) =>
                {
                    // get/show tag theme preview or info
                    if (e.Button == MouseButtons.Right)
                    {
                        if (ModifierKeys == Keys.Control)
                            Globals.Tagger.GetThemePreview(tsmi.Text);
                        else
                            Globals.Tagger.GetThemeInfo(tsmi.Text);

                    }
                    else // get/show actual album artwork tag preview
                    {
                        // apply rating star updates before tag preview
                        if (Globals.Tagger.ThemeName.Contains("_stars"))
                            if (Globals.PackageRatingNeedsUpdate && !Globals.UpdateInProgress)
                                PackageDataTools.UpdatePackageRating();

                        var sd = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
                        if (sd != null)
                        {
                            var tagTheme = ((ToolStripItem)s).Text;
                            var img = Globals.Tagger.Preview(sd, tagTheme);

                            if (img != null)
                            {
                                using (Form f = new Form())
                                {
                                    f.Text = "Tag Theme Preview: " + tagTheme;
                                    f.StartPosition = FormStartPosition.CenterParent;
                                    f.ShowIcon = false;
                                    f.MaximizeBox = false;
                                    f.MinimizeBox = false;
                                    f.AutoSize = true;
                                    PictureBox pb = new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage, Dock = DockStyle.Fill, Image = img };
                                    f.Controls.Add(pb);
                                    f.ShowDialog(this.FindForm());
                                }
                            }
                            else
                                Globals.Log(String.Format("<Error>: Previewing '{0}' ...", sd.Title));
                        }
                    }
                };
            }
        }

        private void ProtectODLC()
        {
            // deselect and protect ODLC 
            if (chkProtectODLC.Checked)
            {
                foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                {
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, row.Index);
                    if (sd != null && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                        dgvSongsMaster.Rows[row.Index].Cells["colSelect"].Value = false;
                }

                chkProtectODLC.ForeColor = Color.Green;
                chkProtectODLC.BackColor = Color.LightGray;
            }
            else
            {
                chkProtectODLC.ForeColor = Color.Red;
                chkProtectODLC.BackColor = Color.LightGray;
            }
        }

        private void RefreshDgv(bool fullRescan)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan(fullRescan);
            PopulateSongManager();
            UpdateToolStrip();
        }

        private void RemoveFilter()
        {
            AppSettings.Instance.SongManagerFilter = String.Empty;
            // save current sorting before removing filter
            statusSongsMaster.SaveSorting(dgvSongsMaster);

            // remove the filter
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);
            if (!String.IsNullOrEmpty(filterStatus))
                DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongsMaster);

            ResetDetail();
            UpdateToolStrip();
            // reapply sort direction to reselect the filtered song
            statusSongsMaster.RestoreSorting(dgvSongsMaster);
            Refresh();
        }

        private void Rescan(bool fullRescan)
        {
            Thread.Sleep(200); // debounce
            try
            {
                dgvSongsDetail.Visible = false;
                dgvSongsMaster.DataSource = null;
            }
            catch {/* DO NOTHING */}


            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show("<ERROR> Rocksmith 2014 Installation Directory setting is null or empty ...", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // this is done here in case user decided to manually delete songs
            List<string> filesList = Worker.FilesList(Constants.Rs2DlcFolder, AppSettings.Instance.IncludeRS1CompSongs, AppSettings.Instance.IncludeRS2BaseSongs, AppSettings.Instance.IncludeCustomPacks);

            // remove inlays
            filesList = filesList.Where(fi => !fi.ToLower().Contains("inlay")).ToList();

            if (!filesList.Any())
            {
                var msgText = String.Format("Houston ... We have a problem!{0}There are no Rocksmith 2014 songs in:" + "{0}{1}{0}{0}Please select a valid Rocksmith 2014{0}installation directory when you restart CFSM.  ", Environment.NewLine, Constants.Rs2DlcFolder);
                MessageBox.Show(msgText, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (Directory.Exists(Constants.WorkFolder))
                {
                    File.Delete(Constants.SongsInfoPath);
                    if (Directory.Exists(Constants.AudioCacheFolder))
                        Directory.Delete(Constants.AudioCacheFolder);
                }

                // prevents write log attempt and shuts down the app
                // Environment.Exit(0);

                // some users have highly customized Rocksmith directory paths
                // this provides better user option than just killing the app down
                return;
            }

            ToggleUIControls(false);

            if (fullRescan)
            {
                // force full rescan by clearing MasterCollection before calling BackgroundScan
                Globals.MasterCollection.Clear();
                // force reload
                Globals.ReloadArrangements = true;
                Globals.ReloadDuplicates = true;
                Globals.ReloadSetlistManager = true;
                // do not reload ProfileSongLists
                //Globals.ReloadProfileSongLists = true;
                tsmiModsMyCDLC.Checked = false;
            }

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(this, bWorker);

                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            ToggleUIControls(true);

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                Globals.Log(Resources.UserCancelledProcess);
                return;
            }

            // BackgroundScan populates Globals.MasterCollection
            songList = Globals.MasterCollection.ToList();
            SaveSongCollectionToFile();
        }

        private void ResetDetail()
        {
            // reset plus/minus in case user did not
            for (int ndx = dgvSongsMaster.Rows.Count - 1; ndx >= 0; ndx--)
                if (!String.IsNullOrEmpty(dgvSongsMaster.Rows[ndx].Cells["colShowDetail"].Tag as String))
                {
                    dgvSongsMaster.Rows[ndx].Cells["colShowDetail"].Value = PlusBitmap;
                    dgvSongsMaster.Rows[ndx].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                    break;
                }
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            AppSettings.Instance.SearchString = lowerCriteria;

            if (songList.Count == 0)
                IncludeSubfolders(false);

            List<SongData> results = new List<SongData>();
            if (songList != null && songList.Count > 0)
            {
                if (chkAdvancedSearch.Checked)
                    results = songList.Where(x => x != null && x.ArtistTitleAlbum != null && x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) ||
                        x.Tunings1D.ToLower().Contains(lowerCriteria) ||
                        x.Arrangements1D.ToLower().Contains(lowerCriteria) ||
                        x.PackageAuthor.ToLower().Contains(lowerCriteria) ||
                        (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(lowerCriteria)) ||
                        (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(lowerCriteria)) ||
                        x.SongYear.ToString().Contains(criteria) ||
                        x.PackageComment.ToLower().Contains(criteria) ||
                        x.FilePath.ToLower().Contains(lowerCriteria)).ToList();
                else
                    results = songList.Where(x => x != null && x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria)).ToList();

                if (!chkIncludeSubfolders.Checked)
                    results = results.Where(x => x != null && Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
            }

            if (results.Any())
                LoadFilteredBindingList(results);
            else
                dgvSongsMaster.Rows.Clear();
        }

        private void SelectAllNone()
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                        row.Cells["colSelect"].Value = !allSelected;
                });

            allSelected = !allSelected;
            dgvSongsMaster.Refresh();
        }

        private void SelectionBackupDelete(DataGridView dgvCurrent, bool modeDelete = false)
        {
            // user must check Select to Backup or Delete
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the select checkbox and" + Environment.NewLine + "then right mouse click to quickly delete or make backup.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (modeDelete)
            {
                var diaMsg = "You are about to permenantly delete the selected songs." + Environment.NewLine +
                    "This action can not be undone." + Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to continue?";
                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete Song(s) ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                    return;
            }

            if (!modeDelete)
                FileTools.CreateBackupOfType(selection, Constants.BackupsFolder, "");
            else
            {
                for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
                {
                    DataGridViewRow row = dgvCurrent.Rows[ndx];
                    if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                        dgvCurrent.Rows.Remove(row);
                }

                FileTools.DeleteFiles(selection);
            }

            // force reload/rescan
            Globals.ReloadSongManager = true;
            Globals.ReloadArrangements = true;
            Globals.RescanDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
            UpdateToolStrip();
        }

        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            // user must check Select to Enable/Disable
            var selections = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selections.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine +
                                "First left mouse click the select checkbox and" + Environment.NewLine +
                                "then right mouse click to quickly Enable/Disable.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvCurrent.Rows[ndx];
                if (!Convert.ToBoolean(row.Cells["colSelect"].Value))
                    continue;

                try
                {
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, ndx);
                    var originalPath = sd.FilePath;

                    if (sd.Enabled == "Yes")
                    {
                        var disabledPath = originalPath.Replace(Constants.EnabledExtension, Constants.DisabledExtension);
                        File.Move(originalPath, disabledPath);
                        sd.FilePath = disabledPath;
                        sd.Enabled = "No";
                    }
                    else
                    {
                        var enabledPath = originalPath.Replace(Constants.DisabledExtension, Constants.EnabledExtension);
                        File.Move(originalPath, enabledPath);
                        sd.FilePath = enabledPath;
                        sd.Enabled = "Yes";
                    }
                }
                catch {/* DO NOTHING */}
            }

            dgvCurrent.Refresh();
        }

        private void ShowSongInfo()
        {
            if (dgvSongsMaster.SelectedRows.Count > 0)
            {
                var sd = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
                if (sd != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(sd);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(String.Format("Please select (highlight) the song that  {0}you would like information about.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // use to manipulate data without causing error
        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvSongsMaster.DataBindingComplete -= dgvSongsMaster_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvSongsMaster.DataBindingComplete += dgvSongsMaster_DataBindingComplete;
            }
        }

        private void ToggleRepairMenu(object sender)
        {
            // enables/disables repair options depending on button selection
            var clickedItem = (ToolStripEnhancedMenuItem)sender;
            var clickedItemGroup = clickedItem.RadioButtonGroupName;
            var repairString = String.Empty;
            var items = tsmiRepairs.DropDownItems;
            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.Checked)
                    repairString += item.Name.Replace("tsmi", " ");

                if (item.RadioButtonGroupName == clickedItemGroup &&
                    item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.CheckBox)
                {
                    if (!clickedItem.Checked)
                    {
                        item.Enabled = false;
                        item.Checked = false;
                    }
                    else
                        item.Enabled = true;
                }
            }

            repairString = repairString.Replace("DLFolderProcess", "").Replace("DLFolderMonitor", "").Trim();
            if (String.IsNullOrEmpty(repairString))
            {
                tsmiRepairsRun.BackColor = Color.LightSteelBlue;
                tsmiRepairsRun.ToolTipText = "Select repair options first";
            }
            else
            {
                tsmiRepairsRun.Enabled = true;
                tsmiRepairsRun.BackColor = Color.LimeGreen;
                tsmiRepairsRun.ToolTipText = "Press to start repair";
            }
        }

        private void ToggleUIControls(bool enable)
        {
            GenExtensions.InvokeIfRequired(cueSearch, delegate { cueSearch.Enabled = enable; });
            GenExtensions.InvokeIfRequired(menuStrip, delegate { menuStrip.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkLblSelectAll, delegate { lnkLblSelectAll.Enabled = enable; });
            GenExtensions.InvokeIfRequired(lnkClearSearch, delegate { lnkClearSearch.Enabled = enable; });
        }

        private void UpdateCharter(DataGridViewRow selectedRow)
        {
            try
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(selectedRow);
                if (sd != null)
                {
                    sd.IgnitionAuthor = Ignition.GetSongInfoFromURL(sd.GetInfoURL(), "name");
                    selectedRow.Cells["IgnitionAuthor"].Value = sd.IgnitionAuthor;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(String.Format("Please connect to the internet  {0}to use this feature.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Globals.Log("Need to be connected to the internet to use this feature");
            }
        }

        private void UserSupport()
        {
            // request for support is shown only one time
            if (!AppSettings.Instance.OneTime && AppSettings.Instance.RepairOptions.DLFolderMonitor)
            {
                // message only shown if the users is actively using the feature
                var diaMsg = "You are using the 'Auto Monitor Downloads Folder'" + Environment.NewLine +
                               "feature.  Your donation is needed to support this and" + Environment.NewLine +
                               "other user requested special features and updates.";

                if (DialogResult.Yes == BetterDialog2.ShowDialog(diaMsg, "Donate if you can ...", null, "I'm a donor", "I'll donate", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150))
                    Globals.Log(" - Thank you for your support ...");
                else
                    Process.Start("https://goo.gl/iTPfRU");

                AppSettings.Instance.OneTime = true;
            }
        }

        private void dgvSongsMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // right mouse click anywhere popup Context Control
            // left mouse click on header sort routines with BLRV refresh           

            if (dgvSongsMaster.DataSource == null)
                return;

            ResetDetail();
            // Ctrl Key w/ left mouse click to quickly turn off column visiblity
            if (ModifierKeys == Keys.Control)
            {
                dgvSongsMaster.Columns[e.ColumnIndex].Visible = false;
                return;
            }
        }

        private void tsmiModsMyCDLC_Click(object sender, EventArgs e)
        {
            // using Click instead of CheckedChanged or CheckStateChanged to prevent double calling
            if (tsmiModsMyCDLC.Checked)
            {
                using (var userInput = new FormUserInput(false))
                {
                    userInput.CustomInputLabel = "Enter the CDLC Charter's Name ...";
                    userInput.FrmHeaderText = "Find CDLC by Charter's Name ...";
                    userInput.CustomInputText = AppSettings.Instance.CharterName;
                    userInput.SetHeightWidth();
                    userInput.StartPosition = FormStartPosition.Manual;
                    userInput.Location = new Point(Parent.Bounds.Right - userInput.Width - 20, Parent.Bounds.Bottom + userInput.Height - 40);
                    userInput.TopMost = true;

                    if (DialogResult.OK != userInput.ShowDialog())
                        return;

                    AppSettings.Instance.CharterName = userInput.CustomInputText;
                }
            }

            if (tsmiModsMyCDLC.Checked && !String.IsNullOrEmpty(AppSettings.Instance.CharterName))
                Globals.Log("Now showing CDLC for Charter's Name: " + AppSettings.Instance.CharterName);
            else
            {
                Globals.Log("Showing CDLC for all Charters ...");
                tsmiModsMyCDLC.Checked = false;
            }

            UpdateToolStrip();
        }

        private void CheckAllForUpdates(object sender, DoWorkEventArgs e)
        {
            if (Globals.TsLabel_Cancel.Visible)
            {
                if (bWorker.IsBusy)
                {
                    bWorker.CancelAsync();
                    bWorker.Abort();
                }

                GenExtensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Visible = false; });
            }

            //Thread.Sleep(3000);
            counterStopwatch = new Stopwatch();
            counterStopwatch.Restart();

            GenExtensions.InvokeIfRequired(dgvSongsMaster, delegate
                {
                    foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                    {
                        if (bWorker.CancellationPending)
                        {
                            bWorker.Abort();
                            Globals.Log("<WARNING> User aborted checking for updates on CF ...");
                            break;
                        }

                        DataGridViewRow currentRow = (DataGridViewRow)row;
                        CheckRowForUpdate(currentRow);
                    }
                });

            counterStopwatch.Stop();
        }

        private void ModsPitchShift_CheckStateChanged(object sender, EventArgs e)
        {
            if (ignoreCheckStateChanged)
                return;

            tsmiMods.ShowDropDown();
            menuStrip.Focus();
        }

        private void RepairsAddDDSettings_Click(object sender, EventArgs e)
        {
            tsmiRepairs.ShowDropDown();
            tsmiDDSettings.ShowDropDown();
            menuStrip.Focus();
        }

        private void RepairsButton_Click(object sender, EventArgs e)
        {
            // TODO: fix screen flicker
            ToggleRepairMenu(sender);
            tsmiRepairs.ShowDropDown();
            menuStrip.Focus();
        }

        private void Repairs_CheckStateChanged(object sender, EventArgs e)
        {
            // to make a tsmi enhanced RadioButton work like a CheckBox ... 
            // set the RadioButtonGroupName for each RadioButton to a unique name
            if (tsmiRepairsUsingOrg.Checked)
            {
                tsmiDLFolderProcess.Enabled = false;
                tsmiDLFolderProcess.Checked = false;
            }
            else
                tsmiDLFolderProcess.Enabled = true;

            // hide the Repairs dropdown menu on startup
            if (ignoreCheckStateChanged)
                return;

            tsmiRepairs.ShowDropDown();
            menuStrip.Focus();
        }

        private void SongManager_Resize(object sender, EventArgs e)
        {
            if (dgvSongsMaster.DataSource == null || dgvSongsMaster.RowCount == 0)
                return;

            ResetDetail();

            // alternate method: maintains SongsDetail visiblity while resizing
            //if (dgvSongsDetail.Visible)
            //{
            //    var vertScrollWidth = 0;
            //    if (dgvSongsMaster.Controls.OfType<VScrollBar>().First().Visible)
            //        vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            //    dgvSongsDetail.Width = dgvSongsMaster.Width - vertScrollWidth - dgvSongsDetail.Location.X + 5; // tweak

            //    var indexAfter = dgvSongsMaster.FirstDisplayedCell.RowIndex;
            //    var indexOffset = firstIndex - indexAfter;
            //    var rowHeight = dgvSongsMaster.Rows[dgvSongsMaster.FirstDisplayedCell.RowIndex].Height;
            //    var vertOffset = rowHeight * indexOffset;
            //    var vertLocation = dgvSongsDetail.Location.Y;
            //    var horizLocation = dgvSongsDetail.Location.X;
            //    dgvSongsDetail.Location = new Point(horizLocation, vertLocation + vertOffset);
            //    dgvSongsDetail.Invalidate();
            //}

            try
            {
                firstIndex = dgvSongsMaster.FirstDisplayedCell.RowIndex;
            }
            catch { /* DO NOTHING */}
        }

        private void TagStyle_Click(object sender, EventArgs e)
        {
            var clickedItem = (ToolStripEnhancedMenuItem)sender;
            var items = tsmiTagStyle.DropDownItems;
            foreach (ToolStripEnhancedMenuItem item in items)
            {
                if (item.Text != clickedItem.Text)
                    item.Checked = false;

                if (item.Checked)
                    Globals.Tagger.ThemeName = clickedItem.Text;
            }

            tsmiMods.ShowDropDown();
            tsmiTagStyle.ShowDropDown();
            menuStrip.Focus();
        }

        private void chkIncludeSubfolders_MouseUp(object sender, MouseEventArgs e)
        {
            AppSettings.Instance.IncludeSubfolders = chkIncludeSubfolders.Checked;
            UpdateToolStrip();
        }

        private void chkProtectODLC_MouseUp(object sender, MouseEventArgs e)
        {
            AppSettings.Instance.ProtectODLC = chkProtectODLC.Checked;
            ProtectODLC();
        }

        private void cmsBackup_Click(object sender, EventArgs e)
        {
            SelectionBackupDelete(dgvSongsMaster, false);
        }

        private void cmsCheckForUpdate_Click(object sender, EventArgs e)
        {
            // TODO: need to remove RS1 Compatiblity DLCs from this check
            GenExtensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Enabled = true; });
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += CheckAllForUpdates; // check all rows
            // bWorker.DoWork += CheckForUpdateEvent; // check single row

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();

            GenExtensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Visible = false; });
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            SelectionBackupDelete(dgvSongsMaster, true);
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            // confirmed method now working on VM Mac Mojave
            // with changes to PsarcPackage platform detection
            // if (Constants.OnMac)
            //{
            //    var diaMsg = "This feature is not supported in Mac mode" + Environment.NewLine +
            //                 "Try using the toolkit to make similar changes.";
            //    BetterDialog2.ShowDialog(diaMsg, "Unsupported Feature ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
            //    return;
            //}

            statusSongsMaster.SaveSorting(dgvSongsMaster);
            var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
            if (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc)
                return;

            // DO NOT edit/modify/repair disabled CDLC
            if (sd.Enabled != "Yes")
            {
                var diaMsg = "Disabled CDLC may not be edited ..." + Environment.NewLine +
                             "Please enable the CLDC and then edit it." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Disabled CDLC ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            var filePath = sd.FilePath;
            using (var songEditor = new frmSongEditor(filePath))
            {
                // position songEditor at top/center of mainForm to avoid having to reposition later
                songEditor.Location = new Point((this.Width - songEditor.Width) / 2, this.ParentForm.Location.Y);
                songEditor.Text = String.Format("{0}{1}", "Song Editor ... Loaded: ", Path.GetFileName(filePath));
                songEditor.IsTagged = sd.Tagged == SongTaggerStatus.True ? true : false;

                songEditor.ShowDialog();
            }

            if (Globals.ReloadSongManager)
                UpdateToolStrip();

            statusSongsMaster.RestoreSorting(dgvSongsMaster);
        }

        private void cmsEnableDisable_Click(object sender, EventArgs e)
        {
            SelectionEnableDisable(dgvSongsMaster);
        }

        private void cmsGetCharterName_Click(object sender, EventArgs e)
        {
            // TODO: add image for GetCharterName to Context Menu Strip item
            GenExtensions.InvokeIfRequired(dgvSongsMaster, delegate
                {
                    if (dgvSongsMaster.SelectedRows.Count > 0)
                        UpdateCharter(dgvSongsMaster.SelectedRows[0]);
                });
        }

        private void cmsOpenSongLocation_Click(object sender, EventArgs e)
        {
            var path = dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", String.Format("/select,\"{0}\"", directory.FullName));
        }

        private void cmsOpenSongPage_Click(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SelectedRows.Count == 1)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
                if (sd != null)
                {
                    if (sd.IgnitionID == null || sd.IgnitionID == "No Results")
                        sd.IgnitionID = Ignition.GetSongInfoFromURL(sd.GetInfoURL(), "id");

                    if (sd.IgnitionID == null || sd.IgnitionID == "No Results")
                        Globals.Log("<ERROR> Song doesn't exist in Ignition anymore ...");
                    else
                        Process.Start(String.Format("{0}/{1}", Constants.DefaultDetailsURL, sd.IgnitionID));
                }
            }
        }

        private void cmsPlaySelected_Click(object sender, EventArgs e)
        {
            if (lastSelectedSongPath != string.Empty && lastSelectedSongPath != dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString())
                Globals.AudioEngine.Stop();

            PlaySongFunction.DynamicInvoke();
        }

        private void cmsShowSongInfo_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }

        private void cmsSongManager_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (keepOpen && e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;

        }

        private void cmsSongManager_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // keeps cms open if Actions clicked
            if (e.ClickedItem == cmsSongManager.Items["cmsActions"])
                keepOpen = true;
            else
                keepOpen = false;
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            // wait for return key to run search, and re-run search on backspace/left arrow
            if (e.KeyCode != Keys.Return && e.KeyCode != Keys.Back && e.KeyCode != Keys.Left)
                return;

            // debounce KeyUp to eliminate intermittent NullReferenceException
            Thread.Sleep(50);

            // save current sort
            statusSongsMaster.SaveSorting(dgvSongsMaster);
            ResetDetail();
            SearchCDLC(cueSearch.Text);
            UpdateToolStrip();
            // restore current sort
            statusSongsMaster.RestoreSorting(dgvSongsMaster);
        }

        private void dgvSongsMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // make sure grid has been painted before proceeding
            if (!dgvPainted)
                return;

            if (dgvSongsMaster.SelectedRows.Count > 0)
                lastSelectedSongPath = dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString();

            // get detail from master
            if (e.RowIndex >= 0 && e.ColumnIndex == colShowDetail.Index)
            {
                if (dgvSongsMaster.Rows[e.RowIndex].Cells["colKey"].Value == null)
                    return;

                var songKey = dgvSongsMaster.Rows[e.RowIndex].Cells["colKey"].Value.ToString();

                if (String.IsNullOrEmpty(songKey))
                    return;

                if (dgvSongsDetail.Visible)
                    if (dgvSongsDetail.Rows[0].Cells["colDetailKey"].Value.ToString() != songKey)
                        ResetDetail();

                if (String.IsNullOrEmpty(dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag as String))
                {
                    var songDetails = songList.Where(master => (master.DLCKey == songKey)).ToList();
                    if (!songDetails.Any())
                        MessageBox.Show("No Song Details Found");
                    else
                    {
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = "TRUE";
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = MinusBitmap;

                        // CRITICAL EXECUTION ORDER - Workaround for intermitent bug not displaying horizscrollbar when last row is selected
                        dgvSongsDetail.Visible = true;
                        dgvSongsDetail.ScrollBars = ScrollBars.Horizontal;
                        // keep column headers on single line
                        dgvSongsDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                        // CRITICAL CODE AREA - CAREFUL - No Filtering KISS
                        dgvSongsDetail.AutoGenerateColumns = false;
                        dgvSongsDetail.DataSource = songDetails;
                        dgvSongsDetail.DataMember = "Arrangements2D";

                        // apply some fixed formatting
                        // any column/row sizing triggers SubclassedDataGridView
                        dgvSongsDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                        foreach (DataGridViewColumn col in dgvSongsDetail.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable; // to ensure proper header alignments
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        dgvSongsDetail.Columns["colDetailKey"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgvSongsDetail.Columns["colDetailKey"].Width = dgvSongsMaster.Columns["colKey"].Width - 1;
                        dgvSongsDetail.Columns["colDetailPID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dgvSongsDetail.Columns["colDetailPID"].Width = 220;

                        // allow user to resize the column after it has been autosized
                        var width = dgvSongsDetail.Columns["colDetailArrangement"].Width;
                        dgvSongsDetail.Columns["colDetailArrangement"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // work around forces horiz scroll to display
                        dgvSongsDetail.Columns["colDetailArrangement"].Width = width;
                        dgvSongsDetail.Columns["colDetailArrangement"].Resizable = DataGridViewTriState.True;

                        // calculate the height and width of dgvSongsDetail
                        var colHeaderHeight = dgvSongsMaster.Columns[e.ColumnIndex].HeaderCell.Size.Height;
                        var rowHeight = dgvSongsMaster.Rows[e.RowIndex].Height;
                        var colWidth = dgvSongsMaster.Columns[e.ColumnIndex].Width;
                        var horizScrollHeight = SystemInformation.HorizontalScrollBarHeight;

                        var vertScrollWidth = 0;
                        if (dgvSongsMaster.Controls.OfType<VScrollBar>().First().Visible)
                            vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

                        dgvSongsDetail.Width = dgvSongsMaster.Width - vertScrollWidth - dgvSongsDetail.Location.X + 5; // tweak
                        dgvSongsDetail.Height = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height + 1) + horizScrollHeight + colHeaderHeight + 2;

                        // height tweak
                        if (dgvSongsDetail.RowCount < 3)
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 3;

                        // given current formatting what is the real width
                        //var rowsWidth = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height);
                        var colsWidth = dgvSongsDetail.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width);
                        if (colsWidth < dgvSongsDetail.Width)
                        {
                            Debug.WriteLine("Resize colDetailDDMax");
                            dgvSongsDetail.Columns["colDetailDDMax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }

                        firstIndex = dgvSongsMaster.FirstDisplayedCell.RowIndex;
                        int maxRows = dgvSongsMaster.Height / rowHeight - 1; // tweaked
                        var selectedIndex = e.RowIndex;
                        var indexOffset = selectedIndex - firstIndex;
                        Rectangle cellRectangle = dgvSongsMaster.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                        // display SongsDetail below selected row
                        if (dgvSongsDetail.Height / rowHeight < maxRows - indexOffset - 1)
                            dgvSongsDetail.Location = new Point(colWidth + 7, cellRectangle.Bottom + rowHeight - 4); // tweaked
                        else // display SongsDetail above selected row
                            dgvSongsDetail.Location = new Point(colWidth + 7, cellRectangle.Bottom - dgvSongsDetail.Height - 4); // tweaked

                        // required here to refresh the scrollbar   
                        dgvSongsDetail.Invalidate();
                    }
                }
                else
                {
                    dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = PlusBitmap;
                    dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                }
            }
        }

        private void dgvSongsMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1) //if it's not header
                ShowSongInfo();
        }

        // <CRITICAL> LOOK HERE ... if any unusual errors show up, e.g., Index exceptions
        private void dgvSongsMaster_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // for degugging
            var dgv = sender as DataGridView;
            var name = dgv.Name;

            if (e.RowIndex == -1)
                return;
            // speed hacks ...
            if (dgvSongsMaster.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvSongsMaster.Rows[e.RowIndex].IsNewRow) // || !dgvSongsMaster.IsCurrentRowDirty)
                return;
            if (dgvSongsMaster.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            try // If a filter is reapplied after changing a row (say enabling a song after having 'No' filter for Enabled column), a row without a bound item will remain and cause errors 
            {
                if (dgvSongsMaster.Rows[e.RowIndex].DataBoundItem == null)
                    return;
            }
            catch (IndexOutOfRangeException ex)
            {
                for (int i = 0; i < dgvSongsMaster.Columns.Count; i++)
                {
                    if (dgvSongsMaster.Rows[e.RowIndex].Cells[i].Value == null) //Makeshift solution at best, but if this happens and the column a column doesn't have a value, it usually means it's due to the row not matching the filter any more
                    {
                        CurrencyManager cm = (CurrencyManager)BindingContext[dgvSongsMaster.DataSource];
                        cm.SuspendBinding();
                        dgvSongsMaster.Rows[e.RowIndex].Visible = false;
                        cm.ResumeBinding();
                        return;
                    }
                }

                Globals.Log($"DGV error: {ex.Message}");
                return;
            }


            SongData sd = dgvSongsMaster.Rows[e.RowIndex].DataBoundItem as SongData;
            if (sd != null)
            {
                if (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc)
                {
                    e.CellStyle.Font = Constants.OfficialDLCFont;
                    DataGridViewCell cell = dgvSongsMaster.Rows[e.RowIndex].Cells["colSelect"];
                    DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                    if (chkProtectODLC.Checked)
                    {
                        chkCell.Style.ForeColor = Color.DarkGray;
                        chkCell.FlatStyle = FlatStyle.Flat;
                        chkCell.Value = false;
                        cell.ReadOnly = true;
                    }
                    else
                    {
                        chkCell.Style.ForeColor = Color.Black;
                        chkCell.FlatStyle = FlatStyle.Popup;
                        cell.ReadOnly = false;
                    }
                }

                if (e.ColumnIndex == colBass.Index || e.ColumnIndex == colVocals.Index || e.ColumnIndex == colLead.Index || e.ColumnIndex == colRhythm.Index)
                {
                    string arrInit = sd.Arrangements1D.ToUpper();

                    if (e.ColumnIndex == colBass.Index)
                        e.CellStyle.BackColor = arrInit.Contains("BASS") ? _Enabled : _Disabled;
                    else if (e.ColumnIndex == colVocals.Index)
                        e.CellStyle.BackColor = arrInit.Contains("VOCAL") ? _Enabled : _Disabled;
                    else if (e.ColumnIndex == colLead.Index)
                    {
                        if (arrInit.Contains("COMBO"))
                            e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                        else
                            e.CellStyle.BackColor = arrInit.Contains("LEAD") ? _Enabled : _Disabled;
                    }
                    else if (e.ColumnIndex == colRhythm.Index)
                    {
                        if (arrInit.Contains("COMBO"))
                            e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                        else
                            e.CellStyle.BackColor = arrInit.Contains("RHYTHM") ? _Enabled : _Disabled;
                    }
                }
            }
        }

        private void dgvSongsMaster_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, e.RowIndex);
                    sd.Selected = !sd.Selected;
                }
                // select multiple rows by Shift-Click two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvSongsMaster.SelectedRows.Count > 0)
                    {
                        var first = dgvSongsMaster.SelectedRows[0];
                        var start = first.Index;
                        var end = e.RowIndex + 1;

                        if (start > end)
                        {
                            var tmp = start;
                            start = end;
                            end = tmp;
                        }
                        TemporaryDisableDatabindEvent(() =>
                            {
                                for (int i = start; i < end; i++)
                                {
                                    var s = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, i);
                                    s.Selected = !s.Selected;
                                }
                            });
                    }
                }
            }
        }

        private void dgvSongsMaster_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown
            var grid = (DataGridView)sender;

            var sd = DgvExtensions.GetObjectFromRow<SongData>(grid, e.RowIndex);
            if (sd == null && e.RowIndex != -1)
                return;


            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                    cmsDelete.Enabled = true;
                    cmsBackup.Enabled = true;
                    cmsEnableDisable.Enabled = true;
                    cmsEdit.Enabled = !sd.IsODLC && !sd.IsRsCompPack && !sd.IsSongsPsarc && !sd.IsSongPack;
                    cmsTaggerPreview.Enabled = !sd.IsODLC && !sd.IsRsCompPack && !sd.IsSongsPsarc;

                    if (chkProtectODLC.Checked && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                    {
                        cmsDelete.Enabled = false;
                        cmsBackup.Enabled = false;
                        cmsEnableDisable.Enabled = false;
                    }

                    cmsSongManager.Show(Cursor.Position);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsSongManagerColumns);
                    cmsSongManagerColumns.Show(Cursor.Position);
                }
            }

            // user complained that clicking a row should not autocheck Select
            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // use SongPacks tabmenu
                if (sd.IsRsCompPack || sd.IsSongsPsarc || sd.IsSongPack)
                {
                    var diaMsg = "Please use Song Packs tabmenu to see or change" + Environment.NewLine +
                                 "the enabled/disabled status of this song ...";
                    BetterDialog2.ShowDialog(diaMsg, "Song Packs ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);

                    // programatically uncheck the select checkbox
                    grid.Rows[e.RowIndex].Cells["colSelect"].Value = !(bool)(grid.Rows[e.RowIndex].Cells["colSelect"].Value);
                    grid.Rows[e.RowIndex].Cells["colSelect"].Value = false;
                    grid.Rows[e.RowIndex].Selected = true;
                }
                else
                {
                    try
                    {
                        grid.Rows[e.RowIndex].Cells["colSelect"].Value = !(bool)(grid.Rows[e.RowIndex].Cells["colSelect"].Value);
                        grid.Rows[e.RowIndex].Selected = true;
                    }
                    catch
                    {
                        // debounce excess clicking
                        TemporaryDisableDatabindEvent(() => { grid.EndEdit(); });
                    }
                }
            }

            // makes checkbox mark appear correctly
            TemporaryDisableDatabindEvent(() =>
                {
                    grid.EndEdit();
                    grid.Refresh();
                });
        }

        private void dgvSongsMaster_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (e.RowIndex != -1 && e.ColumnIndex == colPackageRating.Index)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(grid, e.RowIndex);
                if (sd != null)
                {
                    sd.RatingStars = (int)grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    sd.PackageRating = sd.RatingStars.ToString();
                    sd.NeedsUpdate = true;
                    Globals.PackageRatingNeedsUpdate = true;
                }
            }
        }

        private void dgvSongsMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongsMaster")
                return;

            if (e.ListChangedType != ListChangedType.Reset)
                return;

            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (!bindingCompleted)
            {
                // Globals.Log("DataBinding Complete ... ");
                bindingCompleted = true;
            }

            if (!dgvPainted) // speed hack
                return;

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);
            // filter applied
            if (!String.IsNullOrEmpty(filterStatus))
            {
                Globals.TsLabel_StatusMsg.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_StatusMsg.Text = "Show &All";
                Globals.TsLabel_StatusMsg.IsLink = true;
                Globals.TsLabel_StatusMsg.LinkBehavior = LinkBehavior.HoverUnderline;
                Globals.TsLabel_StatusMsg.Visible = true;
                Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
                Globals.TsLabel_DisabledCounter.Text = filterStatus;
                Globals.TsLabel_DisabledCounter.Visible = true;
                tsmiModsMyCDLC.Checked = false;

                // auto save filter - future use
                //AppSettings.Instance.SongManagerFilter = DataGridViewAutoFilterColumnHeaderCell.GetFilterString(dgvSongsMaster);
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && this.dgvSongsMaster.CurrentCell != null && String.IsNullOrEmpty(cueSearch.Text))
                RemoveFilter();
        }

        private void dgvSongsMaster_KeyDown(object sender, KeyEventArgs e)
        {
            // shortcut keys to show column filter dropdown
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell = this.dgvSongsMaster.CurrentCell.OwningColumn.HeaderCell as DataGridViewAutoFilterColumnHeaderCell;

                if (filterCell != null)
                {
                    filterCell.ShowDropDownList();
                    e.Handled = true;
                }
            }

            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
            {
                for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
                {
                    if (dgvSongsMaster.Rows[i].Selected)
                    {
                        var song = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, i);
                        // beyound current scope of CFSM
                        if (song.IsRsCompPack)
                            Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                        else
                            song.Selected = !song.Selected;
                    }
                }
            }
        }

        private void dgvSongsMaster_KeyUp(object sender, KeyEventArgs e)
        {
            // TODO: fix cludgy action of this selection method
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                TemporaryDisableDatabindEvent(() =>
                    {
                        for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
                        {
                            DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, i).Selected = allSelected;
                        }
                    });

                allSelected = !allSelected;
            }
        }

        private void dgvSongsMaster_Paint(object sender, PaintEventArgs e)
        {
            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.Log("dgvSongsMaster Painted ... ");
            }
        }

        private void dgvSongsMaster_Scroll(object sender, ScrollEventArgs e)
        {
            if (dgvSongsMaster.DataSource == null || dgvSongsMaster.RowCount == 0)
                return;

            if (dgvSongsDetail.Visible)
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    var indexAfter = dgvSongsMaster.FirstDisplayedCell.RowIndex;
                    var indexOffset = firstIndex - indexAfter;
                    var rowHeight = dgvSongsMaster.Rows[dgvSongsMaster.FirstDisplayedCell.RowIndex].Height;
                    var vertOffset = rowHeight * indexOffset;
                    var vertLocation = dgvSongsDetail.Location.Y;
                    var horizLocation = dgvSongsDetail.Location.X;
                    dgvSongsDetail.Location = new Point(horizLocation, vertLocation + vertOffset);
                    dgvSongsDetail.Invalidate();
                }
            }

            firstIndex = dgvSongsMaster.FirstDisplayedCell.RowIndex;
        }

        private void dgvSongsMaster_Sorted(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SortedColumn != null)
            {
                AppSettings.Instance.SortColumn = dgvSongsMaster.SortedColumn.DataPropertyName;
                AppSettings.Instance.SortAscending = dgvSongsMaster.SortOrder == SortOrder.Ascending ? true : false;

                // refresh is necessary to avoid exceptions when row has been deleted
                dgvSongsMaster.Refresh();

                // Reselect last selected row after sorting
                if (!String.IsNullOrEmpty(lastSelectedSongPath) && dgvSongsMaster.Rows.Count > 0)
                {
                    try
                    {
                        int newRowIndex = dgvSongsMaster.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells["colFilePath"].Value.ToString() == lastSelectedSongPath).Index;
                        dgvSongsMaster.Rows[newRowIndex].Selected = true;
                        dgvSongsMaster.FirstDisplayedScrollingRowIndex = newRowIndex;
                    }
                    catch
                    {
                        lastSelectedSongPath = String.Empty;
                    }
                }
                else
                    lastSelectedSongPath = String.Empty;
            }

            dgvSongsMaster.Refresh();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ClearSearch();
        }

        private void ClearSearch()
        {
            tsmiModsMyCDLC.Checked = false;
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Type characters to search for then hit return ...";
            AppSettings.Instance.SearchString = String.Empty;
            SearchCDLC(cueSearch.Text);
            RemoveFilter();
            Globals.Log("Cleared Filters and Search ...");
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectAllNone();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
            Globals.Settings.SaveSettingsToFile(Globals.DgvCurrent);
        }

        private void lnklblToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                        row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
                });

            dgvSongsMaster.Refresh();
        }

        private void tsmiAddDDCfgPath_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CFG Files (*.cfg)|*.cfg";
                ofd.InitialDirectory = Path.GetDirectoryName(SettingsDDC.Instance.CfgPath);

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    // user clicked 'x' restore defaults
                    tsmiAddDDCfgPath.Tag = "";
                    tsmiAddDDCfgPath.Text = "Click to set CFG path";
                }
                else
                {
                    tsmiAddDDCfgPath.Tag = ofd.FileName;
                    tsmiAddDDCfgPath.Text = Path.GetFileName(ofd.FileName);
                }
            }

            tsmiRepairs.ShowDropDown();
            tsmiDDSettings.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiAddDDRampUpPath_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML Files (*.xml)|*.xml";
                ofd.InitialDirectory = Path.GetDirectoryName(SettingsDDC.Instance.RampPath);
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    // user clicked 'x' restore defaults
                    tsmiAddDDRampUpPath.Tag = "";
                    tsmiAddDDRampUpPath.Text = "Click to set RampUp path";
                }
                else
                {
                    tsmiAddDDRampUpPath.Tag = ofd.FileName;
                    tsmiAddDDRampUpPath.Text = Path.GetFileName(ofd.FileName);
                }
            }

            tsmiRepairs.ShowDropDown();
            tsmiDDSettings.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiDLFolderMonitor_CheckStateChanged(object sender, EventArgs e)
        {
            var repairString = String.Empty;
            var items = tsmiRepairs.DropDownItems;

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.Checked)
                    repairString += item.Name.Replace("tsmi", " ");
            }

            repairString = repairString.Replace("DLFolderProcess", "").Replace("DLFolderMonitor", "").Trim();

            if (String.IsNullOrEmpty(repairString) && tsmiDLFolderMonitor.Checked)
            {
                var diaMsg = "First select some repair options to be used, and" + Environment.NewLine +
                             "then check 'Auto Monitor Downloads Folder'.";
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                tsmiDLFolderMonitor.Checked = false;
                return;
            }

            tsmiRepairs.HideDropDown();
            AppSettings.Instance.RepairOptions.DLFolderMonitor = tsmiDLFolderMonitor.Checked;

            RepairTools.DLFolderWatcher(SetRepairOptions());
            Globals.Log(" - Please donate at https://goo.gl/iTPfRU to support user requested special features like this ...");
        }

        private void tsmiDLFolderSupport_Click(object sender, EventArgs e)
        {
            Process.Start("https://goo.gl/iTPfRU");
        }

        private void tsmiDevUseOnly_Click(object sender, EventArgs e)
        {
            // developer sandbox area for testing new methods
            UserSupport();

            // For Devoper Use Only
            // this updates the embedded resource OfficialSongs.json
            // from scraped Ignition ODLC data saved as local IgnitionData.json file
            var workingPath = Environment.CurrentDirectory;
            var rootDir = Path.GetPathRoot(workingPath);
            var projectPath = Directory.GetParent(workingPath).Parent.FullName;
            var resourcesPath = Path.Combine(projectPath, "Resources", "OfficialSongs.json");
            var ignitionDataPath = Path.Combine(rootDir, "IgnitionData.json");

            // the embedded resources is editable (can be updated) only while in debug mode
            if (File.Exists(ignitionDataPath) && Constants.DebugMode)
            {
                using (StreamReader fsr = new StreamReader(ignitionDataPath))
                {
                    string json = fsr.ReadToEnd();
                    var ignitionData = JsonConvert.DeserializeObject<List<IgnitionData>>(json);
                    var officialSongs = new List<OfficialSong>();
                    const string DOWNLOAD_BASE = "http://customsforge.com/process.php?id=";

                    foreach (var data in ignitionData)
                    {
                        var officialSong = new OfficialSong();
                        officialSong.Artist = data.Artist;
                        officialSong.Title = data.Title;
                        officialSong.ReleaseDate = data.Created;
                        officialSong.Link = DOWNLOAD_BASE + data.CFID.ToString();
                        officialSong.Pack = "Single";

                        officialSongs.Add(officialSong);
                    }

                    // write the new OfficialSongs.json file for embedded resources
                    using (StreamWriter fsw = new StreamWriter(resourcesPath))
                    {
                        JToken serializedJson = JsonConvert.SerializeObject(officialSongs, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { });
                        fsw.Write(serializedJson.ToString());
                    }

                    Globals.OfficialDLCSongList = officialSongs;
                    Globals.Log("<DEVELOPER> Updated embedded resource and loaded OfficialSongs.json ...");
                    Globals.Log("<DEVELOPER> Answer 'Yes to All' for any VS IDE popup question about reloading a file ...");
                }
            }
            else
                MessageBox.Show("<ERROR> Did not update OfficialSongs.json file ...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return;

            var prfldbPath = RocksmithProfile.SelectProfile();
            if (prfldbPath == null)
                return;

            var localProfilesPath = Path.Combine(Path.GetDirectoryName(prfldbPath), "localprofiles.json");
            var songListsRoot = UserProfiles.ReadSongListsRoot(prfldbPath);
            Globals.Log(" - User Profile SongListsRoot Loaded ...");
            songListsRoot.SongLists[0] = new List<string>() { "Cozy1", "Was", "Here!" };
            UserProfiles.WriteSongListsRoot(songListsRoot, prfldbPath);
            Globals.Log(" - User Profile SongListsRoot Updated ...");
            var result = UserProfiles.SyncronizeFiles(localProfilesPath, prfldbPath);
            Globals.Log(" - User Profile Files Syncronized ...");
            return;

            Process[] processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    var processMsg = String.Format("process.ProcessName: {0} <> process.Responding: {1} <> process.HasExited: {2}", process.ProcessName, process.Responding, process.HasExited);
                    Globals.Log(processMsg);
                }
                catch (Exception ex)
                {
                    Globals.Log("process.ProcessName: " + process.ProcessName + " <> Exception: " + ex.Message);
                }
            }
            return;

            var stopHere = songList;
            var stopHere2 = Globals.MasterCollection;
            var stopHere3 = AppSettings.Instance.ArrangementAnalyzerFilter;

            if (!String.IsNullOrEmpty(AppSettings.Instance.ArrangementAnalyzerFilter))
            {
                DataGridViewAutoFilterColumnHeaderCell.SetFilter(dgvSongsMaster, AppSettings.Instance.ArrangementAnalyzerFilter);
            }
            return;

            PackageDataTools.ShowPackageRatingWarning();
            return;
        }

        private void tsmiFilesArcBak_Click(object sender, EventArgs e)
        {
            this.Refresh();
            DoWork(Constants.GWORKER_ACHRIVE, Constants.EXT_BAK, Constants.BackupsFolder, tsmiFilesArcDeleteAfter.Checked);
        }

        private void tsmiFilesArcCor_Click(object sender, EventArgs e)
        {
            this.Refresh();
            DoWork(Constants.GWORKER_ACHRIVE, Constants.EXT_COR, Constants.RemasteredCorFolder, tsmiFilesArcDeleteAfter.Checked);
        }

        private void tsmiFilesArcDelete_CheckedChanged(object sender, EventArgs e)
        {
            tsmiFiles.ShowDropDown();
            tsmiFilesArchive.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiFilesArcMax_Click(object sender, EventArgs e)
        {
            this.Refresh();
            DoWork(Constants.GWORKER_ACHRIVE, Constants.EXT_MAX, Constants.RemasteredMaxFolder, tsmiFilesArcDeleteAfter.Checked);
        }

        private void tsmiFilesArcOrg_Click(object sender, EventArgs e)
        {
            this.Refresh();
            DoWork(Constants.GWORKER_ACHRIVE, Constants.EXT_ORG, Constants.RemasteredOrgFolder, tsmiFilesArcDeleteAfter.Checked);
        }

        private void tsmiFilesCheckODLC_Click(object sender, EventArgs e)
        {
            using (var ODlcCheckForm = new frmCODLCReplacements())
            {
                // declared for debugging
                var conflicted = ODlcCheckForm.PopulateLists();
                if (conflicted)
                    ODlcCheckForm.ShowDialog();
            }

            if (Globals.RescanSongManager)
                UpdateToolStrip();
        }

        private void tsmiFilesCleanDlc_Click(object sender, EventArgs e)
        {
            FileTools.CleanDlcFolder();
        }

        private void tsmiFilesIncludeODLC_Click(object sender, EventArgs e)
        {
            tsmiFiles.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiFilesOrganize_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);

            if (tsmiFilesIncludeODLC.Checked)
            {
                selection.AddRange(songList.Where(x => x.PackageAuthor == "Ubisoft").ToList());
                // prevent double selecting ODLC files
                selection = selection.GroupBy(x => x.FilePath).Select(s => s.First()).ToList();
            }

            // never try to rename dlc compatibility files ... doh!
            selection = selection.Where(fi => !fi.FileName.ToLower().Contains(Constants.RS1COMP) && !fi.FileName.ToLower().Contains(Constants.SONGPACK) && !fi.FileName.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            if (!selection.Any())
            {
                var diaMsg = "First select some songs using the 'Select' checkbox" + Environment.NewLine +
                             "then try using the organize feature again." + Environment.NewLine;

                BetterDialog2.ShowDialog(diaMsg, "Organize Artist Name Subfolders ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Hand.Handle), "ReadMe", 0, 150);
                return;
            }

            // start new generic worker
            DoWork(Constants.GWORKER_ORGANIZE, Constants.Rs2DlcFolder, selection, false);
            dgvSongsMaster.Refresh();
        }

        private void tsmiFilesRestoreBak_Click(object sender, EventArgs e)
        {
            FileTools.RestoreBackups(Constants.EXT_BAK, Constants.BackupsFolder);
            RefreshDgv(false);
        }

        private void tsmiFilesRestoreCor_Click(object sender, EventArgs e)
        {
            FileTools.RestoreBackups(Constants.EXT_COR, Constants.RemasteredCorFolder);
            RefreshDgv(false);
        }

        private void tsmiFilesRestoreMax_Click(object sender, EventArgs e)
        {
            FileTools.RestoreBackups(Constants.EXT_MAX, Constants.RemasteredMaxFolder);
            RefreshDgv(false);
        }

        private void tsmiFilesRestoreOrg_Click(object sender, EventArgs e)
        {
            FileTools.RestoreBackups(Constants.EXT_ORG, Constants.RemasteredOrgFolder);
            RefreshDgv(false);
        }

        private void tsmiFilesUnorganize_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);

            if (tsmiFilesIncludeODLC.Checked)
            {
                selection.AddRange(songList.Where(x => x.PackageAuthor == "Ubisoft").ToList());
                // prevent double selecting ODLC files
                selection = selection.GroupBy(x => x.FilePath).Select(s => s.First()).ToList();
            }

            if (!selection.Any())
                return;

            // start new generic worker
            DoWork(Constants.GWORKER_ORGANIZE, Constants.Rs2DlcFolder, selection, true);
            dgvSongsMaster.Refresh();
        }

        private void tsmiHelpErrorLog_Click(object sender, EventArgs e)
        {
            RepairTools.ViewErrorLog();
        }

        private void tsmiHelpGeneral_Click(object sender, EventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSongMgr.rtf", "Song Manager Help");
        }

        private void tsmiHelpRepairs_Click(object sender, EventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpRepairs.rtf", "Repairs Help");
        }

        private void tsmiModPreserveStats_Click(object sender, EventArgs e)
        {
            // prevents flicker
            ignoreCheckStateChanged = true;
            // syncronize PreserveStats checkboxes
            tsmiRepairsPreserveStats.Checked = tsmiModsPreserveStats.Checked;
            ignoreCheckStateChanged = false;
            AppSettings.Instance.RepairOptions.PreserveStats = tsmiModsPreserveStats.Checked;
        }

        private void tsmiModsChangeAppId_Click(object sender, EventArgs e)
        {
            // confirmed method working on Mac
            // TODO: consider using this type of method for other mods and repairs
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
                return;

            frmModAppId.BatchEdit(selection.ToArray());
            dgvSongsMaster.Refresh();
        }

        private void tsmiModsPitchShifter_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
                return;

            this.Refresh();
            DoWork(Constants.GWORKER_PITCHSHIFT, selection, tsmiModsPitchShiftOverwrite.Checked, tsmiModsPitchShiftStandard.Checked);
            UpdateToolStrip();
        }

        private void tsmiModsTagArtwork_Click(object sender, EventArgs e)
        {
            // confirmed method working on Mac
            // apply rating star updates before tagging
            if (Globals.Tagger.ThemeName.Contains("_stars"))
                if (Globals.PackageRatingNeedsUpdate && !Globals.UpdateInProgress)
                    PackageDataTools.UpdatePackageRating();

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster); //.Where(sd => sd.Tagged == false);
            if (!selection.Any())
            {
                var diaMsg = "Please select some CDLC to Tag using the 'Select' column." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Tag Artwork ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);

                return;
            }

            // should not occure because tagger has default theme
            if (String.IsNullOrEmpty(Globals.Tagger.ThemeName))
            {
                MessageBox.Show("Please select a tag style first");
                return;
            }

            DoWork(Constants.GWORKER_TAG, selection, false);
            // force dgvSongsMaster data to refresh after Tagging
            GetGrid().Invalidate();
            GetGrid().Refresh();
        }

        private void tsmiModsTheMover_CheckStateChanged(object sender, EventArgs e)
        {
            if (dgvSongsMaster.DataSource == null)
                return;

            for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
            {
                DataGridViewRow row = dgvSongsMaster.Rows[i];
                var artist = (string)row.Cells["colArtist"].Value;

                // 'The' mover
                if (tsmiModsTheMover.Checked)
                {
                    if (artist.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                        row.Cells["colArtist"].Value = String.Format("{0}, The", artist.Substring(4, artist.Length - 4)).Trim();
                }
                else
                {
                    if (artist.EndsWith(", The", StringComparison.CurrentCultureIgnoreCase))
                        row.Cells["colArtist"].Value = String.Format("The {0}", artist.Substring(0, artist.Length - 5)).Trim();
                }
            }
        }

        private void tsmiModsUntagArtwork_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster); //.Where(sd => sd.Tagged);
            if (!selection.Any())
            {
                var diaMsg = "Please select some CDLC to Un-Tag using the 'Select' column." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Un-Tag Artwork...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);

                return;
            }

            DoWork(Constants.GWORKER_UNTAG, selection, true);
            // force dgvSongsMaster data to refresh after Untagging
            GetGrid().Invalidate();
            GetGrid().Refresh();
        }

        private void tsmiRepairsPreserveStats_Click(object sender, EventArgs e)
        {
            // prevents flicker
            ignoreCheckStateChanged = true;
            // syncronize PreserveStats checkboxes
            tsmiModsPreserveStats.Checked = tsmiRepairsPreserveStats.Checked;
            ignoreCheckStateChanged = false;
            AppSettings.Instance.RepairOptions.PreserveStats = tsmiRepairsPreserveStats.Checked;
        }

        private void tsmiRepairsRun_Click(object sender, EventArgs e)
        {
            // confirmed method working on Mac
            if (tsmiDLFolderMonitor.Checked)
            {
                var diaMsg = "Please uncheck 'Auto Monitor Downloads Folder'" + Environment.NewLine +
                             "before using the 'Run Selected Repair Optons'" + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            if (tsmiDLFolderProcess.Checked && !FileTools.ValidateDownloadsDirs())
            {
                var diaMsg = "Please select a valid downloads folder and try again." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            if (tsmiDLFolderMonitor.Checked && Directory.Exists(AppSettings.Instance.DLMonitorDesinationFolder))
                FileTools.SetDLDestinationFolder();

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any() && !tsmiDLFolderProcess.Checked)
            {
                var diaMsg = "Please select some CDLC to repair using the 'Select' column." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            // CRITICAL DO NOT repair tagged CDLC - artifact data will be lost forever
            if (selection.Any(sd => sd.Tagged == SongTaggerStatus.True))
            {
                var diaMsg = "Tagged CDLC may not be repaired ..." + Environment.NewLine +
                             "Please untag the CLDC and then repair it." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Tagged CDLC ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            var items = tsmiRepairs.DropDownItems;
            var repairString = String.Empty;

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.Checked)
                    repairString += item.Name.Replace("tsmi", " ");
            }

            repairString = repairString.Replace("DLFolderProcess", "").Replace("DLFolderMonitor", "").Trim();
            if (String.IsNullOrEmpty(repairString))
            {
                var diaMsg = "Please select some repair options and try again." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            // RepairsMaxFive Remove
            if (repairString.Contains("RepairsMaxFive") && !repairString.Contains("Remove"))
            {
                var diaMsg = "Max Five Arrangements was selected" + Environment.NewLine +
                             "but no removal options were checked." + Environment.NewLine +
                             "Please choose removal optons and try again." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            // set minimum default repair option and preserve stats
            if (!tsmiRepairsMastery.Checked)
            {
                tsmiRepairsMastery.Checked = true;
                tsmiRepairsPreserveStats.Checked = true;
                Globals.Log(" - User did not select 'Repairs' option 'Mastery 100% Bug' ...");
                Globals.Log(" - By default, CFSM will fix the bug and preserve the user stats ...");
            }

            tsmiRepairs.HideDropDown();
            DoWork(Constants.GWORKER_REPAIR, selection, SetRepairOptions());
            Globals.ReloadArrangements = true;
            Globals.ReloadSetlistManager = true;
            UpdateToolStrip();
            this.Refresh();
        }

        private void tsmiRescanFull_Click(object sender, EventArgs e)
        {
            // just for fun ... estimate parsing time
            // based on machine specs (speed, cores and OS) (P4 2500 C1 5) (i7 3500 C4 10)                       
            float psarcFactor = 7000.0f; // adjust as needed (lower factor => less time)
            if (AppSettings.Instance.IncludeArrangementData)
                psarcFactor = 21000.0f;

            var osMajor = Environment.OSVersion.Version.Major;
            var processorSpeed = SysExtensions.GetProcessorSpeed();
            var coreCount = SysExtensions.GetCoreCount();
            var secsPerSong = (float)Math.Round(psarcFactor / (processorSpeed * coreCount * osMajor), 2);
            var songsCount = Globals.MasterCollection.Count;
            var secsEPT = songsCount == 0 ? "???" : (songsCount * secsPerSong).ToString(); // estimated pasing time (secs)
            var songsCountAsString = songsCount == 0 ? "all" : songsCount.ToString();
            var diaMsg = "You are about to run a full rescan of (" + songsCountAsString + ") songs." + Environment.NewLine +
                         "Operation will take approximately (" + secsEPT + ") seconds  " + Environment.NewLine +
                         "to complete." + Environment.NewLine + Environment.NewLine +
                         "Do you want to proceed?";

            if (DialogResult.Yes != BetterDialog2.ShowDialog(diaMsg, "Full Rescan", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Question.Handle), "INFO", 0, 150))
                return;

            Globals.Log("OS Version: " + osMajor);
            Globals.Log("Processor Speed (MHz): " + processorSpeed);
            Globals.Log("Processor Cores: " + coreCount);
            Globals.Log("Songs Count: " + songsCount);
            Globals.Log("Estimate Parsing Time (secs): " + secsEPT);

            Stopwatch sw = new Stopwatch();
            sw.Restart();
            RefreshDgv(true);
            Globals.Log("Rescan Parsing Time (secs): " + sw.ElapsedMilliseconds / 1000f);
            sw.Stop();
        }

        private void tsmiRescanQuick_Click(object sender, EventArgs e)
        {
            RefreshDgv(false);
        }

        public DataGridView GetGrid()
        {
            return dgvSongsMaster;
        }

        public void TabEnter()
        {
            var debugMe = Globals.MasterCollection;
            Globals.DgvCurrent = dgvSongsMaster;
            DataGridViewAutoFilterColumnHeaderCell.SavedColumnFilter = AppSettings.Instance.SongManagerFilter;
            GetGrid().ResetBindings(); // force grid data to rebind/refresh
            statusSongsMaster.RestoreSorting(Globals.DgvCurrent);
            Globals.Log("SongManagerFilter Available: " + (String.IsNullOrEmpty(AppSettings.Instance.SongManagerFilter) ? "None" : AppSettings.Instance.SongManagerFilter));
            Globals.Log("Song Manager GUI Activated ...");
        }

        public void TabLeave()
        {
            statusSongsMaster.SaveSorting(Globals.DgvCurrent);
            GetGrid().ResetBindings(); // force grid data to rebind/refresh
            SetRepairOptions(); // saves current repair options   
            SetAudioOptions(); // saves current audio options

            // save new filter
            if (!String.IsNullOrEmpty(DataGridViewAutoFilterColumnHeaderCell.SavedColumnFilter) && DataGridViewAutoFilterColumnHeaderCell.SavedColumnFilter != AppSettings.Instance.SongManagerFilter)
            {
                AppSettings.Instance.SongManagerFilter = DataGridViewAutoFilterColumnHeaderCell.SavedColumnFilter;
                Globals.Log("Saved SongManagerFilter: " + AppSettings.Instance.SongManagerFilter);
            }

            if (Globals.PackageRatingNeedsUpdate && !Globals.UpdateInProgress)
                PackageDataTools.UpdatePackageRating();

            Globals.Settings.SaveSettingsToFile(Globals.DgvCurrent);
            Globals.Log("Song Manager GUI Deactivated ...");
        }

        private void dgvSongsMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Globals.DebugLog(String.Format("<ERROR> (Row: {0}, Col: {1}), {2} ...", e.RowIndex, e.ColumnIndex, e.Exception.Message));
            e.Cancel = true;
        }

        private void TagUnTagSelection(bool removeTag)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
            {
                Globals.Log("Please select at least one song you would like to tag!");
                return;
            }

            string tag = tsmiCustomTitleTagTextBox.Text;

            DoWork(Constants.GWORKER_TITLETAG, selection, tag, tsmiCustomTitleTagPrefix.Checked, removeTag);
            UpdateToolStrip();
        }

        private void tsmiCustomTitleTagRemove_Click(object sender, EventArgs e)
        {
            tsmiMods.HideDropDown();
            TagUnTagSelection(true);
        }

        private void tsmiCustomTitleTagAdd_Click(object sender, EventArgs e)
        {
            tsmiMods.HideDropDown();
            TagUnTagSelection(false);
        }

        private void CustomTitleTag_Click(object sender, EventArgs e)
        {
            tsmiMods.ShowDropDown();
            tsmiCustomTitleTag.ShowDropDown();
            menuStrip.Focus();
        }

        private void chkAdvancedSearch_CheckedChanged(object sender, EventArgs e)
        {
            SearchCDLC(cueSearch.Text);
        }

        private void tsmiAutoAdjustVolume_MouseUp(object sender, MouseEventArgs e)
        {

            // Proof of Concept (POC) 
            // extract audio
            // convert audio to usable format
            // measure audio LUFS
            // calculate corrected toolkit LF
            // inject corrected toolkit LF

            // note it may be necessary to similary adjust toolkit tone volumes as well

            if (e.Button == MouseButtons.Right)
            {
                frmNoteViewer.ViewExternalFile("ffmpeg/ReadMe.txt", "Auto Adjust CDLC Volume", 17, 0);
                return;
            }

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
            {
                var diaMsg = "Please select some CDLC to Normalize Audio using the 'Select' column." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Custom Mods ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return;
            }

            tsmiMods.HideDropDown();

            if (AudioNormalizer.ValidateFfmpeg())
                DoWork(Constants.GWORKER_NORMALIZE, selection, SetAudioOptions());
        }

        public AudioOptions SetAudioOptions()
        {
            AudioOptions audioOptions = new AudioOptions()
            {
                CorrectionFactor = Convert.ToSingle(tsmiCorrectionFactor.DecimalValue, CultureInfo.InvariantCulture),
                CorrectionMultiplier = Convert.ToSingle(tsmiCorrectionMultiplier.DecimalValue, CultureInfo.InvariantCulture),
                TargetAudioVolume = Convert.ToSingle(tsmiTargetAudioVolume.DecimalValue, CultureInfo.InvariantCulture),
                TargetPreviewVolume = Convert.ToSingle(tsmiTargetPreviewVolume.DecimalValue, CultureInfo.InvariantCulture),
                TargetToneVolume = Convert.ToSingle(tsmiTargetToneVolume.DecimalValue, CultureInfo.InvariantCulture),
                TargetLUFS = Convert.ToSingle(tsmiTargetLUFS.DecimalValue, CultureInfo.InvariantCulture)
            };

            AppSettings.Instance.AudioOptions = audioOptions;
            return audioOptions;
        }

        private void GetAudioOptions()
        {
            tsmiCorrectionFactor.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.CorrectionFactor, CultureInfo.InvariantCulture);
            tsmiCorrectionMultiplier.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.CorrectionMultiplier, CultureInfo.InvariantCulture);
            tsmiTargetAudioVolume.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.TargetAudioVolume, CultureInfo.InvariantCulture);
            tsmiTargetPreviewVolume.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.TargetPreviewVolume, CultureInfo.InvariantCulture);
            tsmiTargetToneVolume.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.TargetToneVolume, CultureInfo.InvariantCulture);
            tsmiTargetLUFS.DecimalValue = Convert.ToDecimal(AppSettings.Instance.AudioOptions.TargetLUFS, CultureInfo.InvariantCulture);
        }

        private void tsmiAudio_KeyUp(object sender, KeyEventArgs e)
        {
            if (tsmiCorrectionFactor.DecimalValue == 0)
            {
                tsmiCorrectionFactor.DecimalValue = (decimal)0.1;
                Globals.Log("<WARNING> Correction Factor must be non-zero ...");
            }

            if (tsmiCorrectionMultiplier.DecimalValue == 0)
            {
                tsmiCorrectionMultiplier.DecimalValue = (decimal)0.1;
                Globals.Log("<WARNING> Correction Multiplier must be non-zero ...");
            }
        }

        private void tsmiAudio_MouseUp(object sender, MouseEventArgs e)
        {
            if (tsmiCorrectionFactor.DecimalValue == 0)
            {
                tsmiCorrectionFactor.DecimalValue = (decimal)0.1;
                Globals.Log("<WARNING> Correction Factor must be non-zero ...");
            }

            if (tsmiCorrectionMultiplier.DecimalValue == 0)
            {
                tsmiCorrectionMultiplier.DecimalValue = (decimal)0.1;
                Globals.Log("<WARNING> Correction Multiplier must be non-zero ...");
            }
        }

        private void tsmiPocText_MouseUp(object sender, MouseEventArgs e)
        {
            tsmiMods.ShowDropDown();
            tsmiAutoAdjustVolume.ShowDropDown();
        }

        private void tsmiChangeMonitoredFolders_Click(object sender, EventArgs e)
        {
            frmMonitoredFolders frmMonitoredFolders = new frmMonitoredFolders();
            frmMonitoredFolders.Show();
        }

        private void tsmiChangeDestinationFolder_Click(object sender, EventArgs e)
        {
            FileTools.SetDLDestinationFolder();
        }

        private void tsmiCheckForUpdates_Click(object sender, EventArgs e)
        {
            frmOutdatedSongs frmOutdatedSongs = new frmOutdatedSongs();
            frmOutdatedSongs.Show();
        }
    }
}

