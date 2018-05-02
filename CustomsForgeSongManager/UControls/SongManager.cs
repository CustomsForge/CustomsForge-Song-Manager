using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Stopwatch counterStopwatch = new Stopwatch();
        private bool dgvPainted = false;
        private int firstIndex = 0;
        private bool ignoreCheckStateChanged = false;
        private bool keepOpen;
        private string lastSelectedSongPath = String.Empty;
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private List<SongData> songList = new List<SongData>(); // prevents filtering from being inherited

        public SongManager()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            dgvSongsDetail.Visible = false;

            tsmiDevDebugUse.Visible = GenExtensions.IsInDesignMode ? true : false;
            cmsCheckForUpdate.Visible = GenExtensions.IsInDesignMode ? true : false;
            cmsOpenSongLocation.Visible = GenExtensions.IsInDesignMode ? true : false;

            PopulateTagger();
            cmsTaggerPreview.Visible = true; // ???
            PopulateSongManager();
            InitializeRepairMenu();
        }

        public void PlaySelectedSong()
        {
            var song = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
            if (song != null)
            {
                if (String.IsNullOrEmpty(song.AudioCache))
                    song.AudioCache = string.Format("{0}_{1}", Guid.NewGuid().ToString().Replace("-", ""), song.FileSize);

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
                    Globals.Log(String.Format("Playing {0} by {1} ... ({2})", song.Title, song.Artist, Path.GetFileName(song.FilePath)));
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
            //Load Song Collection from file must be called before
            LoadSongCollectionFromFile();

            // Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvSongsMaster.Columns.Cast<DataGridViewColumn>().Where(col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvSongsMaster.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }

            // TODO: maybe reapply previous filtering and search here

            UpdateToolStrip();
        }

        public void SaveSongCollectionToFile()
        {
            var dom = Globals.MasterCollection.XmlSerializeToDom();
            XmlElement versionNode = dom.CreateElement("SongDataList");
            versionNode.SetAttribute("version", SongData.SongDataListCurrentVersion);
            versionNode.SetAttribute("AppVersion", Constants.CustomVersion());
            dom.DocumentElement.AppendChild(versionNode);

            foreach (XmlElement songData in dom.GetElementsByTagName("ArrayOfSongData")[0].ChildNodes)
            {
                var arrangementsNode = songData.GetElementsByTagName("Arrangements")[0];
                if (arrangementsNode != null)
                {
                    var arrNodes = arrangementsNode.ChildNodes.OfType<XmlNode>().ToList();

                    foreach (var arrNode in arrNodes)
                    {
                        var isVocals = arrNode.InnerXml.Contains("<Name>Vocals</Name>");
                        var innerNodes = arrNode.ChildNodes.OfType<XmlNode>().ToList();

                        // reduce songInfo.xml file size
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
            ro.PhraseLength = (int)tsmiAddDDNumericUpDown.Value;
            ro.RemoveSustain = tsmiAddDDRemoveSustain.Checked;
            ro.CfgPath = tsmiAddDDCfgPath.Tag == null ? "" : tsmiAddDDCfgPath.Tag.ToString();
            ro.RampUpPath = tsmiAddDDRampUpPath.Tag == null ? "" : tsmiAddDDRampUpPath.Tag.ToString();
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
            ro.FixLowBass = tsmiFixLowBass.Checked;
            ro.ProcessDLFolder = tsmiProcessDLFolder.Checked;

            AppSettings.Instance.RepairOptions = ro;
            return ro;
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSongManager)
            {
                Globals.RescanSongManager = false;
                Globals.ReloadRenamer = true;
                Globals.ReloadSetlistManager = true;
                Globals.ReloadDuplicates = true;
                // full rescan of song collection
                Rescan(true);
                PopulateDataGridView();
            }

            if (Globals.ReloadSongManager)
            {
                Globals.ReloadSongManager = false;
                // smart quick rescan of song collection
                Rescan(false);
                PopulateDataGridView();
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Song Count: {0}", songList.Count);
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
            if (sd == null || sd.OfficialDLC || sd.IsRsCompPack)
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

        private void DoWork(string workDescription, dynamic workerParm1 = null, dynamic workerParm2 = null, dynamic workerParm3 = null)
        {
            using (var gWorker = new GenericWorker())
            {
                gWorker.WorkDescription = workDescription;
                gWorker.WorkParm1 = workerParm1;
                gWorker.WorkParm2 = workerParm2;
                gWorker.WorkParm3 = workerParm3;
                gWorker.BackgroundProcess(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
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
            tsmiRemoveSections.Checked = AppSettings.Instance.RepairOptions.RemoveSections;
            tsmiFixLowBass.Checked = AppSettings.Instance.RepairOptions.FixLowBass;
            tsmiProcessDLFolder.Checked = AppSettings.Instance.RepairOptions.ProcessDLFolder;
            tsmiRepairsAddDD.Checked = AppSettings.Instance.RepairOptions.AddDD;
            tsmiOverwriteDD.Checked = AppSettings.Instance.RepairOptions.OverwriteDD;
            tsmiAddDDNumericUpDown.Value = AppSettings.Instance.RepairOptions.PhraseLength;
            tsmiAddDDRemoveSustain.Checked = AppSettings.Instance.RepairOptions.RemoveSustain;

            tsmiAddDDCfgPath.Tag = AppSettings.Instance.RepairOptions.CfgPath;
            if (!String.IsNullOrEmpty(tsmiAddDDCfgPath.Tag.ToString()))
                tsmiAddDDCfgPath.Text = Path.GetFileName(tsmiAddDDCfgPath.Tag.ToString());

            tsmiAddDDCfgPath.Tag = AppSettings.Instance.RepairOptions.RampUpPath;
            if (!String.IsNullOrEmpty(tsmiAddDDCfgPath.Tag.ToString()))
                tsmiAddDDRampUpPath.Text = Path.GetFileName(tsmiAddDDRampUpPath.Tag.ToString());

            ignoreCheckStateChanged = false;
        }

        private void InitializeRepairMenu()
        {
            // restore saved repair options
            GetRepairOptions();

            var items = tsmiRepairs.DropDownItems;
            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>()
                .Where(item => item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton))
            {
                ToggleRepairMenu(item);
            }
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSongsMaster.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvSongsMaster.DataSource = bs;
        }

        private void LoadSongCollectionFromFile()
        {
            chkSubFolders.Checked = true;
            bool correctVersion = false;

            try
            {
                // load songsInfo.xml if it exists 
                if (File.Exists(Constants.SongsInfoPath))
                {
                    XmlDocument dom = new XmlDocument();
                    dom.Load(Constants.SongsInfoPath);
                    Globals.Log("Loaded File: " + Path.GetFileName(Constants.SongsInfoPath));

                    // Depricated Code ... load analyzerData.xml
                    //XmlDocument arrDom = new XmlDocument();
                    //if (AppSettings.Instance.IncludeAnalyzerData && File.Exists(Constants.AnalyzerDataPath))
                    //{
                    //    arrDom.Load(Constants.AnalyzerDataPath);
                    //    Globals.Log("Loaded File: " + Path.GetFileName(Constants.AnalyzerDataPath));
                    //}
                    ////else // remove old anlayzer data
                    ////    GenExtensions.DeleteFile(Constants.AnalyzerDataPath);

                    //foreach (XmlElement songData in dom.GetElementsByTagName("ArrayOfSongData")[0].ChildNodes)
                    //{
                    //    if (songData.Name == "SongDataList")
                    //        continue;

                    //    string dlcKey = songData.SelectSingleNode("DLCKey").ChildNodes[0].Value.ToString();
                    //    XmlElement arrangementsNode = null;
                    //    if (arrDom.HasChildNodes)
                    //    {
                    //        arrangementsNode = arrDom.DocumentElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Attributes["DLCKey"].Value == dlcKey);
                    //        var originalArrNode = songData.SelectSingleNode("Arrangements");
                    //        originalArrNode.ParentNode.ReplaceChild(dom.ImportNode(arrangementsNode, true), originalArrNode);
                    //    }
                    //}

                    // remove version info node
                    var listNode = dom["ArrayOfSongData"];
                    if (listNode != null)
                    {
                        var versionNode = listNode["SongDataList"];
                        if (versionNode != null)
                        {
                            if (versionNode.HasAttribute("version"))
                                correctVersion = (versionNode.GetAttribute("version") == SongData.SongDataListCurrentVersion);

                            listNode.RemoveChild(versionNode);
                        }

                        songList = SerialExtensions.XmlDeserialize<List<SongData>>(listNode.OuterXml);

                        if (songList == null || songList.Count == 0)
                            throw new SongCollectionException(songList == null ? "Master Collection = null" : "Master Collection.Count = 0");

                        Globals.MasterCollection = new BindingList<SongData>(songList);
                    }
                }

                // smart scan
                if (correctVersion)
                {
                    Globals.Log("Performing quick rescan of song collection ...");
                    Rescan(false);
                }
                else
                {
                    if (File.Exists(Constants.SongsInfoPath))
                        Globals.Log("<WARNING> Incorrect song collection version found ...");

                    try
                    {
                        // DO NOT use the bulldozer here
                        // 'My Documents/CFSM' may contain some original files
                        ZipUtilities.RemoveReadOnlyAttribute(Constants.WorkFolder);
                        GenExtensions.DeleteFile(Constants.LogFilePath);
                        GenExtensions.DeleteFile(Constants.SongsInfoPath);
                        GenExtensions.DeleteFile(Constants.AppSettingsPath);
                        GenExtensions.DeleteDirectory(Constants.GridSettingsFolder);
                    }
                    catch (Exception ex)
                    {
                        Globals.Log("<ERROR> Cleaning " + Path.GetFileName(Constants.WorkFolder) + " : " + ex.Message);
                    }

                    Globals.Log("Performing full rescan of song collection ...");
                    Rescan(true);
                }

                // Rescan calls BackgroundScan/ParseSongs and loads Globals.MasterCollection
                // and local songCollection is loaded with Globals.MasterCollection
                PopulateDataGridView();
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

                MessageBox.Show(string.Format("{0}{1}{1}CFSM will now shut down.", err, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void PopulateDataGridView()
        {
            // respect processing order
            DgvExtensions.DoubleBuffered(dgvSongsMaster);
            LoadFilteredBindingList(songList);
            CFSMTheme.InitializeDgvAppearance(dgvSongsMaster);
            // reload column order, width, visibility
            Globals.Settings.LoadSettingsFromFile(dgvSongsMaster, true);

            if (RAExtensions.ManagerGridSettings != null)
                dgvSongsMaster.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);
            else
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);

            // lock OfficialDLC from being selected
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(row);
                if (sd.OfficialDLC)
                {
                    row.Cells["colSelect"].Value = false;
                    row.Cells["colSelect"].ReadOnly = sd.OfficialDLC;
                    sd.Selected = false;
                }
            }
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
                // items[i].Size = new Size(173, 22);
                items[i].Click += TagStyle_Click;
            }
            tsmiTagStyle.DropDownItems.AddRange(items);

            cmsTaggerPreview.DropDownItems.Clear();
            foreach (string tagPreview in Globals.Tagger.Themes)
            {
                var tsi = cmsTaggerPreview.DropDownItems.Add(tagPreview);
                tsi.Click += (s, e) =>
                    {
                        var sel = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
                        if (sel != null)
                        {
                            string ATheme = ((ToolStripItem)s).Text;
                            var img = Globals.Tagger.Preview(sel, ATheme);
                            if (img != null)
                            {
                                using (Form f = new Form())
                                {
                                    f.Text = "Tagger Preview of " + ATheme;
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
                                Globals.Log(String.Format("<Error>: Previewing '{0}' ...", sel.Title));
                        }
                    };
            }
        }

        private void RefreshDgv(bool fullRescan)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan(fullRescan);
            PopulateDataGridView();
            UpdateToolStrip();
        }

        private void RemoveFilter()
        {
            // save current sorting before removing filter
            DgvExtensions.SaveSorting(dgvSongsMaster);

            // remove the filter
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongsMaster);
            ResetDetail();
            UpdateToolStrip();

            // reapply sort direction to reselect the filtered song
            DgvExtensions.RestoreSorting(dgvSongsMaster);
            this.Refresh();
        }

        private void Rescan(bool fullRescan)
        {
            dgvSongsMaster.DataSource = null;
            dgvSongsDetail.Visible = false;

            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show("<Error>: Rocksmith 2014 Installation Directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // this is done here in case user decided to manually delete songs
            List<string> filesList = Worker.FilesList(Constants.Rs2DlcFolder, AppSettings.Instance.IncludeRS1CompSongs, AppSettings.Instance.IncludeRS2BaseSongs, AppSettings.Instance.IncludeCustomPacks);
            if (!filesList.Any())
            {
                var msgText = string.Format("Houston ... We have a problem!{0}There are no Rocksmith 2014 songs in:" + "{0}{1}{0}{0}Please select a valid Rocksmith 2014{0}installation directory when you restart CFSM.  ", Environment.NewLine, Constants.Rs2DlcFolder);
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
                Globals.ReloadSetlistManager = true;
                Globals.ReloadDuplicates = true;
                Globals.ReloadRenamer = true;
                Globals.ReloadArrangements = true;
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

            var results = songList
                .Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) ||
                    x.Tunings1D.ToLower().Contains(lowerCriteria) ||
                    x.Arrangements1D.ToLower().Contains(lowerCriteria) ||
                    x.PackageAuthor.ToLower().Contains(lowerCriteria) ||
                    (x.IgnitionAuthor != null &&
                    x.IgnitionAuthor.ToLower().Contains(lowerCriteria) ||
                    (x.IgnitionID != null &&
                    x.IgnitionID.ToLower().Contains(lowerCriteria)) ||
                    x.SongYear.ToString().Contains(criteria) ||
                    x.FilePath.ToLower().Contains(lowerCriteria))).ToList();

            if (!chkSubFolders.Checked)
                results = results.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
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

        private void SelectionDeleteBackup(DataGridView dgvCurrent, bool modeDelete = false)
        {
            // user must check Select to Delete/Move
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any()) return;

            if (modeDelete)
            {
                var diaMsg = "You are about to delete CDLC file(s)." + Environment.NewLine + "Deletion is permenant and can not be undone." + Environment.NewLine + "Do you want to continue?";
                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete CDLC ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
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
            Globals.RescanDuplicates = true;
            Globals.ReloadSongManager = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
            UpdateToolStrip();
        }

        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            // user must check Select to Enable/Disable
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any()) return;

            for (int ndx = dgvCurrent.Rows.Count - 1; ndx >= 0; ndx--)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, ndx);
                var originalPath = sd.FilePath;
                if (!originalPath.ToLower().Contains(Constants.RS1COMP))
                {
                    DataGridViewRow row = dgvCurrent.Rows[ndx];
                    if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    {
                        try
                        {
                            if (sd.Enabled == "Yes")
                            {
                                var disabledPath = originalPath.Replace(Constants.PsarcExtension, Constants.DisabledPsarcExtension);
                                File.Move(originalPath, disabledPath);
                                sd.FilePath = disabledPath;
                                sd.Enabled = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace(Constants.DisabledPsarcExtension, Constants.PsarcExtension);
                                File.Move(originalPath, enabledPath);
                                sd.FilePath = enabledPath;
                                sd.Enabled = "Yes";
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er, Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                        }
                    }
                }
                else
                    Globals.Log(String.Format(Properties.Resources.ThisIsARocksmith1CompatiblitySongX0RS1Comp, Environment.NewLine));
            }

            dgvCurrent.Refresh();
        }

        private void ShowSongInfo()
        {
            if (dgvSongsMaster.SelectedRows.Count > 0)
            {
                var song = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(string.Format("Please select (highlight) the song that  {0}you would like information about.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var items = tsmiRepairs.DropDownItems;

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>()
                .Where(item => item.RadioButtonGroupName == clickedItemGroup))
            {
                if (item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.CheckBox)
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

            tsmiRepairsRun.BackColor = SystemColors.Control;
            tsmiRepairsRun.ToolTipText = "Select repair options first";

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton && item.Checked)
                {
                    tsmiRepairsRun.Enabled = true;
                    tsmiRepairsRun.BackColor = Color.LimeGreen;
                    tsmiRepairsRun.ToolTipText = "Press to start repair";
                    break;
                }
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
                MessageBox.Show(string.Format("Please connect to the internet  {0}to use this feature.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Globals.Log("Need to be connected to the internet to use this feature");
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

        private void tsmiModsMyCDLC_CheckedChanged(object sender, EventArgs e)
        {
            var charterName = AppSettings.Instance.CharterName;

            if (!String.IsNullOrEmpty(charterName))
            {
                if (tsmiModsMyCDLC.Checked)
                    LoadFilteredBindingList(songList.Where(x => x.PackageAuthor == charterName).ToList());
                else
                    PopulateDataGridView();
            }
            else
            {
                Globals.Log("To use this feature, go to 'Settings' menu and enter a charter name ...");
                tsmiModsMyCDLC.Checked = false;
            }
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
            tsmiAddDDSettings.ShowDropDown();
            menuStrip.Focus();
        }

        private void RepairsButton_Click(object sender, EventArgs e)
        {
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
                tsmiProcessDLFolder.Enabled = false;
                tsmiProcessDLFolder.Checked = false;
            }
            else
                tsmiProcessDLFolder.Enabled = true;

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

            firstIndex = dgvSongsMaster.FirstDisplayedCell.RowIndex;
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

        private void TaggerProgress(object sender, TaggerProgress e)
        {
            Globals.TsProgressBar_Main.Value = e.Progress;
            Application.DoEvents();
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

        private void chkSubFolders_MouseUp(object sender, MouseEventArgs e)
        {
            cueSearch.Text = String.Empty;
            songList = Globals.MasterCollection.ToList();

            if (!chkSubFolders.Checked)
            {
                var results = songList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                LoadFilteredBindingList(songList);
        }

        private void cmsBackup_Click(object sender, EventArgs e)
        {
            SelectionDeleteBackup(dgvSongsMaster, false);
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
            SelectionDeleteBackup(dgvSongsMaster, true);
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            DgvExtensions.SaveSorting(dgvSongsMaster);
            var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
            var filePath = sd.FilePath;
            var enabled = sd.Enabled == "Yes";

            // DO NOT edit/modify/repair disabled CDLC
            if (!enabled)
            {
                var diaMsg = Environment.NewLine + "Disabled CDLC may not be edited." + Environment.NewLine + "Please enable the CLDC and then edit it.";
                BetterDialog2.ShowDialog(diaMsg, "Disabled CDLC ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "", 0, 150);
                return;
            }

            using (var songEditor = new frmSongEditor(filePath))
            {
                songEditor.Text = String.Format("{0}{1}", "Song Editor ... Loaded: ", Path.GetFileName(filePath));
                songEditor.ShowDialog();
            }

            if (Globals.ReloadSongManager)
                UpdateToolStrip();

            DgvExtensions.RestoreSorting(dgvSongsMaster);
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

        private void cmsOpenDLCLocation_Click(object sender, EventArgs e)
        {
            var path = dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
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
                        Globals.Log("<ERROR>: Song doesn't exist in Ignition anymore.");
                    else
                        Process.Start(string.Format("{0}/{1}", Constants.DefaultDetailsURL, sd.IgnitionID));
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
            // save current sort
            DgvExtensions.SaveSorting(dgvSongsMaster);
            ResetDetail();

            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(songList);

            // restore current sort
            DgvExtensions.RestoreSorting(dgvSongsMaster);
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
                            Debug.WriteLine("Resize colDetailSections");
                            dgvSongsDetail.Columns["colDetailSections"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        // LOOK HERE ... if any unusual errors show up
        private void dgvSongsMaster_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            // speed hacks ...
            if (dgvSongsMaster.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvSongsMaster.Rows[e.RowIndex].IsNewRow) // || !dgvSongsMaster.IsCurrentRowDirty)
                return;
            if (dgvSongsMaster.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            SongData song = dgvSongsMaster.Rows[e.RowIndex].DataBoundItem as SongData;

            if (song != null)
            {
                if (song.OfficialDLC)
                {
                    e.CellStyle.Font = Constants.OfficialDLCFont;
                    // prevent checking (selecting) ODCL all together ... evil genious code
                    DataGridViewCell cell = dgvSongsMaster.Rows[e.RowIndex].Cells["colSelect"];
                    DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                    chkCell.Value = false;
                    chkCell.FlatStyle = FlatStyle.Flat;
                    chkCell.Style.ForeColor = Color.DarkGray;
                    cell.ReadOnly = true;
                }

                if (e.ColumnIndex == colBass.Index || e.ColumnIndex == colVocals.Index || e.ColumnIndex == colLead.Index || e.ColumnIndex == colRhythm.Index)
                {
                    string arrInit = song.Arrangements1D.ToUpper();

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
            // TODO: make consistent with other grids
            // has precedent over a ColumnHeader_MouseClick
            // MouseUp detection is more reliable than MouseDown
            var grid = (DataGridView)sender;
            var rowIndex = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                if (rowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, e.RowIndex);
                    cmsEdit.Enabled = !sd.OfficialDLC;
                    cmsDelete.Enabled = !sd.OfficialDLC;
                    cmsTaggerPreview.Enabled = !sd.OfficialDLC;
                    cmsSongManager.Show(Cursor.Position);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsSongManagerColumns);
                    cmsSongManagerColumns.Show(Cursor.Position);
                }
            }

            //SongData song = dgvSongsMaster.Rows[e.RowIndex].DataBoundItem as SongData;
            //// prevent selection of ODCL
            //if (song != null)
            //    if (song.OfficialDLC)
            //        (grid.Rows[e.RowIndex].Cells["colSelect"] as DataGridViewCheckBoxCell).Value = false;

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // beyound current scope of CFSM
                if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                else // required to force selected row change
                {
                    TemporaryDisableDatabindEvent(() => { dgvSongsMaster.EndEdit(); });
                }
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvSongsMaster.Refresh();
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
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && this.dgvSongsMaster.CurrentCell != null && String.IsNullOrEmpty(cueSearch.Text))
                RemoveFilter();

            // save filter - future use
            AppSettings.Instance.FilterString = DataGridViewAutoFilterColumnHeaderCell.GetFilterString(dgvSongsMaster);
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
                if (lastSelectedSongPath != string.Empty)
                {
                    int newRowIndex = dgvSongsMaster.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells["colFilePath"].Value.ToString() == lastSelectedSongPath).Index;
                    dgvSongsMaster.Rows[newRowIndex].Selected = true;
                    dgvSongsMaster.FirstDisplayedScrollingRowIndex = newRowIndex;
                }
                else
                    lastSelectedSongPath = String.Empty;
            }

            dgvSongsMaster.Refresh();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // RemoveFilter();

            cueSearch.Text = String.Empty;
            tsmiModsMyCDLC.Checked = false;

            // save current sorting before clearing search
            DgvExtensions.SaveSorting(dgvSongsMaster);

            songList = Globals.MasterCollection.ToList();
            if (!chkSubFolders.Checked)
            {
                var results = songList.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                LoadFilteredBindingList(songList);

            UpdateToolStrip();

            DgvExtensions.RestoreSorting(dgvSongsMaster);

            AppSettings.Instance.FilterString = String.Empty;
            AppSettings.Instance.SearchString = String.Empty;
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectAllNone();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
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
                    return;

                tsmiAddDDCfgPath.Tag = ofd.FileName;
                tsmiAddDDCfgPath.Text = Path.GetFileName(ofd.FileName);
            }

            tsmiRepairs.ShowDropDown();
            tsmiAddDDSettings.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiAddDDRampUpPath_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML Files (*.xml)|*.xml";
                ofd.InitialDirectory = Path.GetDirectoryName(SettingsDDC.Instance.RampPath);
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                tsmiAddDDRampUpPath.Tag = ofd.FileName;
                tsmiAddDDRampUpPath.Text = Path.GetFileName(ofd.FileName);
            }

            tsmiRepairs.ShowDropDown();
            tsmiAddDDSettings.ShowDropDown();
            menuStrip.Focus();
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
            using (var ODlcCheckForm = new frmCODLCDuplicates())
            {
                // declared for debugging
                var conflicted = ODlcCheckForm.PopulateLists();
                if (conflicted)
                    ODlcCheckForm.ShowDialog();
            }
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
                return;

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

            if (!selection.Any()) return;

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
            // TODO: consider using this type of method for other mods and repairs
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            frmModAppId.BatchEdit(selection.ToArray());
        }

        private void tsmiModsMyCDLC_CheckStateChanged(object sender, EventArgs e)
        {
            var charterName = AppSettings.Instance.CharterName;

            if (!String.IsNullOrEmpty(charterName))
            {
                if (tsmiModsMyCDLC.Checked)
                    LoadFilteredBindingList(songList.Where(x => x.PackageAuthor == charterName).ToList());
                else
                    PopulateDataGridView();
            }
            else
            {
                Globals.Log("To use this feature, go to 'Settings' menu and enter a charter name ...");
                tsmiModsMyCDLC.Checked = false;
            }

            tsmiMods.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiModsPitchShifter_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
                return;

            this.Refresh();
            DoWork(Constants.GWORKER_PITCHSHIFT, selection, tsmiModsPitchShiftOverwrite.Checked, tsmiModsPitchShiftStandard.Checked);

            // quickly reload the songCollection to the dgv
            if (Globals.ReloadSongManager)
            {
                LoadFilteredBindingList(songList);
                Globals.ReloadSongManager = false;
            }

            dgvSongsMaster.Refresh();
        }

        private void tsmiModsTagArtwork_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster); //.Where(sd => sd.Tagged == false);
            if (!selection.Any()) return;

            // should not occure because tagger is defaulting to frack theme
            if (String.IsNullOrEmpty(Globals.Tagger.ThemeName))
            {
                MessageBox.Show("Please select a tag style first");
                return;
            }

            Globals.Tagger.OnProgress += TaggerProgress;
            try
            {
                Globals.Tagger.TagSongs(selection.ToArray());
            }
            finally
            {
                Globals.Tagger.OnProgress -= TaggerProgress;
                // force dgvSongsMaster data to refresh after Tagging
                GetGrid().Invalidate();
                GetGrid().Refresh();
            }
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
            if (!selection.Any()) return;

            Globals.Tagger.OnProgress += TaggerProgress;
            try
            {
                Globals.Tagger.UntagSongs(selection.ToArray());
            }
            finally
            {
                Globals.Tagger.OnProgress -= TaggerProgress;
                // force dgvSongsMaster data to refresh after Untagging
                GetGrid().Invalidate();
                GetGrid().Refresh();
            }
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
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            var items = tsmiRepairs.DropDownItems;
            var repairString = String.Empty;

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.Checked)
                    repairString += item.Name.Replace("tsmi", " ");
            }

            repairString = repairString.Trim();

            if (String.IsNullOrEmpty(repairString))
                return;

            // RepairsMaxFive Remove
            if (repairString.Contains("RepairsMaxFive") && !repairString.Contains("Remove"))
            {
                var diaMsg = Environment.NewLine + "Max Five Arrangements was selected" + Environment.NewLine + "but no removal options were checked." + Environment.NewLine + "Please choose removal optons and try again.";

                BetterDialog2.ShowDialog(diaMsg, "Repair Options ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "", 0, 150);
                return;
            }

            this.Refresh();

            if (tsmiProcessDLFolder.Checked)
                FileTools.ValidateDownloadsDir();

            // start new generic worker
            DoWork(Constants.GWORKER_REPAIR, selection, SetRepairOptions());
            UpdateToolStrip();
        }

        private void tsmiRescanFull_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to run a full rescan of all song data." + Environment.NewLine +
                "Do you want to proceed?", "Full Rescan", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            RefreshDgv(true);
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
            Globals.DgvCurrent = dgvSongsMaster;
            Globals.Log("Song Manager GUI Activated ...");
        }

        public void TabLeave()
        {
            SetRepairOptions();
            if (songList.Any())
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);

            Globals.Log("Song Manager GUI Deactivated ...");
        }

        private void tsmiDevsDebugUse_Click(object sender, EventArgs e)
        {
            // temporarily debugging some things here
            var stopHere = songList;
            var stopHere2 = Globals.MasterCollection;
            var stopHere3 = AppSettings.Instance.FilterString;
        }

    }
}

