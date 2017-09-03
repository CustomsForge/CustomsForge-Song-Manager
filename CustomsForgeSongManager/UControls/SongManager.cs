using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CustomControls;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.SongEditor;
using CustomsForgeSongManager.UITheme;
using DataGridViewTools;
using Newtonsoft.Json;
using System.Xml;
using Globals = CustomsForgeSongManager.DataObjects.Globals;

// TODO: apply Lovro's SongCollection rescan/updated as a generic method to
// replace usages of Globals.ReloadSongManager or RescanSongManager = true
// see comment about databinding in DeleteSelection method

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
        private bool ignoreCheckStateChanged = false;
        private string lastSelectedSongPath = string.Empty;
        private BindingList<SongData> masterSongCollection = new BindingList<SongData>();
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private int firstIndex = 0;

        public SongManager()
        {
            InitializeComponent();

            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            dgvSongsDetail.Visible = false;

            PopulateTagger();
            PopulateSongManager();
            cmsTaggerPreview.Visible = true; // ???
            InitializeRepairMenu();
        }

        public void PlaySelectedSong()
        {
            var sng = DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvSongsMaster);
            if (sng != null)
            {
                if (String.IsNullOrEmpty(sng.AudioCache))
                {
                    sng.AudioCache = string.Format("{0}_{1}", Guid.NewGuid().ToString().Replace("-", ""), sng.FileSize);
                }

                var audioCacheDir = Constants.AudioCacheFolder;
                if (!Directory.Exists(audioCacheDir))
                    Directory.CreateDirectory(audioCacheDir);

                var fullname = Path.Combine(audioCacheDir, sng.AudioCache + ".ogg");

                if (File.Exists(fullname))
                {
                    bool canDelete = false;
                    //check if it needs to be updated using song filesize,
                    if (!sng.AudioCache.Contains("_"))
                        canDelete = true;
                    else
                    {
                        var ss = sng.AudioCache.Split('_');
                        if (ss[1] != sng.FileSize.ToString())
                            canDelete = true;
                    }
                    if (canDelete)
                        File.Delete(fullname);
                }

                if (!File.Exists(fullname))
                {
                    //extract the audio...
                    if (!PsarcBrowser.ExtractAudio(sng.FilePath, fullname, ""))
                    {
                        Globals.Log(Properties.Resources.CouldNotExtractTheAudio);
                        sng.AudioCache = String.Empty;
                        return;
                    }
                }

                if (!File.Exists(fullname))
                {
                    sng.AudioCache = String.Empty;
                    return;
                }

                if (Globals.AudioEngine.OpenAudioFile(fullname))
                {
                    //float inGameVolume = (float)Math.Pow(10, sng.SongVolume / 10) - 1; // if in dB
                    //Globals.AudioEngine.SetVolume(1.0f - sng.SongVolume);

                    Globals.AudioEngine.Play();
                    Globals.Log(String.Format("Playing {0} by {1} ... ({2})", sng.Title, sng.Artist, Path.GetFileName(sng.FilePath)));
                }
                else
                    Globals.Log("Unable to open audio file.");
            }
        }

        public void PopulateSongManager()
        {
            // Hide main dgvSongsMaster until load completes
            dgvSongsMaster.Visible = false;
            //Load Song Collection from file must be called before
            LoadSongCollectionFromFile();

            Globals.Log("Populating SongManager GUI ...");

            //Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvSongsMaster.Columns.Cast<DataGridViewColumn>().Where(col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvSongsMaster.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }

            UpdateToolStrip();
        }

        public void SaveSongCollectionToFile()
        {
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);
            //only save when not filtered
            if (String.IsNullOrEmpty(filterStatus))
            {
                //save with some version info
                var dom = masterSongCollection.XmlSerializeToDom();
                XmlElement versionNode = dom.CreateElement("SongDataList");
                versionNode.SetAttribute("version", SongData.SongDataListCurrentVersion);
                versionNode.SetAttribute("AppVersion", Constants.CustomVersion());
                dom.DocumentElement.AppendChild(versionNode);

                var arrDom = new XmlDocument();
                var allArrsNode = arrDom.CreateElement("AnalyzerData");
                string keptInfo = "persistentid_dmax_name_dd_tuning_tonebase_tonebase_sectioncount"; //don't move these to ArrInfo file

                foreach (XmlElement songData in dom.GetElementsByTagName("ArrayOfSongData")[0].ChildNodes)
                {
                    var arrangementsNode = songData.GetElementsByTagName("Arrangements")[0];
                    if (arrangementsNode != null)
                    {
                        string dlcKey = songData.SelectSingleNode("DLCKey").ChildNodes[0].Value.ToString();
                        ((XmlElement)arrangementsNode).SetAttribute("DLCKey", dlcKey);

                        var extraMetaDataScanned = Convert.ToBoolean(songData.SelectSingleNode("ExtraMetaDataScanned").ChildNodes[0].Value);
                        if (extraMetaDataScanned)
                            allArrsNode.AppendChild(arrDom.ImportNode(arrangementsNode, true));

                        var arrNodes = arrangementsNode.ChildNodes.OfType<XmlNode>().ToList();
                        arrNodes.ForEach(arr =>
                        {
                            arr.ChildNodes.OfType<XmlNode>().ToList().ForEach(n =>
                            {
                                if (!keptInfo.Contains(n.Name.ToLower()))
                                    arr.RemoveChild(n);
                            });
                        });
                    }
                }

                // save analyzerData.xml
                if (AppSettings.Instance.IncludeAnalyzerData && !allArrsNode.IsEmpty)
                {
                    arrDom.AppendChild(allArrsNode);
                    arrDom.Save(Constants.AnalyzerDataPath);
                    Globals.Log("Saved File: " + Path.GetFileName(Constants.AnalyzerDataPath));
                }
                //else // remove old anlayzer data
                //{
                //    if (GenExtensions.DeleteFile(Constants.AnalyzerDataPath))
                //    {
                //        Globals.Log("User Disabled Analyzer ...");
                //        Globals.Log("Deleted File: " + Path.GetFileName(Constants.AnalyzerDataPath));
                //    }
                //}

                dom.Save(Constants.SongsInfoPath);
                Globals.Log("Saved File: " + Path.GetFileName(Constants.SongsInfoPath));
            }
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
                // smart scan of song collection
                Rescan(false);
                PopulateDataGridView();
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Song Count: {0}", masterSongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            numberOfDisabledDLC = masterSongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;
            var tsldcMsg = String.Format("Outdated: {0} | Disabled CDLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
            TemporaryDisableDatabindEvent(() => { dgvSongsMaster.Refresh(); });
        }

        private void CheckForUpdatesEvent(object o, DoWorkEventArgs args)
        {
            // part of ContextMenuStrip action
            GenExtensions.InvokeIfRequired(dgvSongsMaster, delegate
                {
                    if (dgvSongsMaster.SelectedRows.Count > 0)
                    {
                        CheckRowForUpdate(dgvSongsMaster.SelectedRows[0]);
                        SaveSongCollectionToFile();
                    }
                });
        }

        private void CheckRowForUpdate(DataGridViewRow dataGridViewRow)
        {
            // part of ContextMenuStrip action
            if (!bWorker.CancellationPending)
            {
                var currentSong = DgvExtensions.GetObjectFromRow<SongData>(dataGridViewRow);
                if (currentSong != null)
                {
                    //currentSong.IgnitionVersion = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "version");
                    string url = currentSong.GetInfoURL();
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
                        string myParameters = myParametersTemplate.Replace("{artist}", currentSong.Artist).Replace("{album}", currentSong.Album).Replace("{title}", currentSong.Title);

                        string clientURL = string.Concat(url, "?", myParameters);

                        string authHeader = string.Concat("Bearer ", auth_token);

                        client.Headers[HttpRequestHeader.ContentType] = "Content-Type: application/json";
                        client.Headers.Add("Authorization", authHeader);

                        if (client.Headers["Authorization"] == null)
                            throw new Exception("Cannot add auth header");
                        else if (string.IsNullOrEmpty(client.Headers["Authorization"]))
                            throw new Exception("Header auth value is empty");

                        client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                        dynamic HtmlResult = client.UploadString(clientURL, string.Empty);

                        //currentSong.IgnitionID = HtmlResult.data.id;
                        //currentSong.IgnitionUpdated = HtmlResult.data.updated;
                        //currentSong.IgnitionVersion = HtmlResult.data.version;
                        //currentSong.IgnitionAuthor = HtmlResult.data.name;
                    }
                }
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

        private void DeleteSelection()
        {
            // INFORMATION - deleting data from the dgv is the same as deleting data
            // from the SongCollection because the dgv and the SongCollection are bound
            // similarly deleting data from the SongCollection is the same as deleting data
            // from the dgv after it is refreshed because the two are bound to each other
            var stopHere = Globals.SongCollection; // for debugging

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            var diaMsg = "You are about to delete CDLC file(s)." + Environment.NewLine +
                         "Deletion is permenant and can not be undone." + Environment.NewLine +
                         "Do you want to continue?";

            if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete CDLC ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                return;

            for (int ndx = dgvSongsMaster.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSongsMaster.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value) || row.Selected)
                    TemporaryDisableDatabindEvent(() => dgvSongsMaster.Rows.Remove(row));
            }

            var stopHere2 = Globals.SongCollection; // for debugging

            FileTools.DeleteFiles(selection);
            Globals.RescanSongManager = false; // stops full rescan set by worker
            UpdateToolStrip();
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
            masterSongCollection.Clear();
            bool correctVersion = false;

            try
            {
                // load songsInfo.xml if it exists 
                if (File.Exists(Constants.SongsInfoPath))
                {
                    XmlDocument dom = new XmlDocument();
                    dom.Load(Constants.SongsInfoPath);
                    Globals.Log("Loaded File: " + Path.GetFileName(Constants.SongsInfoPath));

                    // load analyzerData.xml
                    XmlDocument arrDom = new XmlDocument();
                    if (AppSettings.Instance.IncludeAnalyzerData && File.Exists(Constants.AnalyzerDataPath))
                    {
                        arrDom.Load(Constants.AnalyzerDataPath);
                        Globals.Log("Loaded File: " + Path.GetFileName(Constants.AnalyzerDataPath));
                    }
                    //else // remove old anlayzer data
                    //    GenExtensions.DeleteFile(Constants.AnalyzerDataPath);

                    foreach (XmlElement songData in dom.GetElementsByTagName("ArrayOfSongData")[0].ChildNodes)
                    {
                        if (songData.Name == "SongDataList")
                            continue;

                        string dlcKey = songData.SelectSingleNode("DLCKey").ChildNodes[0].Value.ToString();
                        XmlElement arrangementsNode = null;
                        if (arrDom.HasChildNodes)
                        {
                            arrangementsNode = arrDom.DocumentElement.ChildNodes.OfType<XmlElement>().FirstOrDefault(n => n.Attributes["DLCKey"].Value == dlcKey);
                            var originalArrNode = songData.SelectSingleNode("Arrangements");
                            originalArrNode.ParentNode.ReplaceChild(dom.ImportNode(arrangementsNode, true), originalArrNode);
                        }
                    }

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

                        masterSongCollection = SerialExtensions.XmlDeserialize<BindingList<SongData>>(listNode.OuterXml);

                        if (masterSongCollection == null || masterSongCollection.Count == 0)
                            throw new SongCollectionException(masterSongCollection == null ? "Master Collection = null" : "Master Collection.Count = 0");
                    }
                }

                // load song collection into memory
                if (correctVersion)
                {
                    Globals.SongCollection = masterSongCollection;
                    Rescan(false); // smart scan
                }
                else
                {
                    if (File.Exists(Constants.SongsInfoPath))
                        Globals.Log("<WARNING> Incorrect song collection version found ...");

                    Globals.Log("Performing full scan of song collection ...");

                    try
                    {
                        // DO NOT use the bulldozer here
                        // 'My Documents/CFSM' may contain some original files
                        GenExtensions.DeleteFile(Constants.LogFilePath);
                        GenExtensions.DeleteFile(Constants.SongsInfoPath);
                        GenExtensions.DeleteFile(Constants.AnalyzerDataPath);
                        GenExtensions.DeleteFile(Constants.AppSettingsPath);
                    }
                    catch (Exception ex)
                    {
                        Globals.Log("<ERROR> Cleaning " + Path.GetFileName(Constants.WorkFolder) + " : " + ex.Message);
                    }

                    // full scan
                    Rescan(true);
                }

                PopulateDataGridView();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.InnerException != null)
                    err += Environment.NewLine + "Inner: " + ex.InnerException.Message;


                // failsafe ... delete My Documents/CFSM folder and files with option not to delete
                var diaMsg = "A fatal CFSM application error has occured:" + Environment.NewLine +
                             err + Environment.NewLine +
                             "You are about to delete all work files created" + Environment.NewLine +
                             "by CFSM, including any backups of CDLC files." + Environment.NewLine +
                             "Deletion is permenant and can not be undone." + Environment.NewLine +
                             "Do you want to continue?";

                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete 'My Documents/CFSM' ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                {
                    Globals.Log("<Fatal Error>: " + err);
                    Globals.Log("User aborted deleting CFSM folder and subfolders from My Documents ...");
                    Environment.Exit(0);
                }

                // use the bulldozer
                ZipUtilities.RemoveReadOnlyAttribute(Constants.WorkFolder);
                GenExtensions.DeleteDirectory(Constants.WorkFolder);
                FileTools.VerifyCfsmFolders();

                diaMsg = "Deleted CFSM folder and subfolders from My Documents ..." + Environment.NewLine +
                    err + Environment.NewLine + Environment.NewLine + "CFSM will now shut down.";
                MessageBox.Show(diaMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            // var debugMe = masterSongCollection;
        }

        private void PopulateDataGridView() // binding data to grid
        {
            // respect processing order
            DgvExtensions.DoubleBuffered(dgvSongsMaster);
            LoadFilteredBindingList(masterSongCollection);
            CFSMTheme.InitializeDgvAppearance(dgvSongsMaster);
            // reload column order, width, visibility
            Globals.Settings.LoadSettingsFromFile(dgvSongsMaster);
            if (RAExtensions.ManagerGridSettings != null)
                dgvSongsMaster.ReLoadColumnOrder(RAExtensions.ManagerGridSettings.ColumnOrder);

            // lock OfficialDLC from being selected
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                var sng = DgvExtensions.GetObjectFromRow<SongData>(row);
                if (sng.OfficialDLC)
                {
                    row.Cells["colSelect"].Value = false;
                    row.Cells["colSelect"].ReadOnly = sng.OfficialDLC;
                    sng.Selected = false;
                }
            }
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {

            if (RAExtensions.ManagerGridSettings == null)
            {
                if (Globals.DgvCurrent == null)
                    Globals.DgvCurrent = dgvSongsMaster;

                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
            }

            contextMenuStrip.Items.Clear();
            var gridSettings = RAExtensions.ManagerGridSettings;

            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
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

        private void RefreshDgv(bool fullRescan, bool parseExtraData = false)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan(fullRescan, parseExtraData);
            PopulateDataGridView();
            UpdateToolStrip();
            Globals.ReloadDuplicates = true;
            Globals.ReloadSetlistManager = true;
            Globals.ReloadRenamer = true;
        }

        private void RemoveFilter()
        {
            Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
            tsmiModsMyCDLC.Checked = false;
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongsMaster);
            ResetDetail();

            if (!chkSubFolders.Checked)
            {
                var results = masterSongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                LoadFilteredBindingList(masterSongCollection);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSongsMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void Rescan(bool fullRescan, bool parseExtraData = false)
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
            if (dgvSongsMaster.DataSource != null)
                Globals.Settings.SaveSettingsToFile(dgvSongsMaster);

            if (fullRescan)
                Globals.SongCollection.Clear();

            var sw = new Stopwatch();
            sw.Restart();

            // run new worker
            using (Worker worker = new Worker())
            {
                if (parseExtraData || (AppSettings.Instance.IncludeAnalyzerData))
                {
                    string oldName = dgvSongsMaster.Name; //TODO: replace this with a more suitable/less hacky way to the bigger rescan
                    dgvSongsMaster.Name = "Analyzer";
                    worker.BackgroundScan(dgvSongsMaster, bWorker);
                    dgvSongsMaster.Name = oldName;
                }
                else
                    worker.BackgroundScan(dgvSongsMaster, bWorker);

                while (Globals.WorkerFinished == Globals.Tristate.False)
                {
                    Application.DoEvents();
                }
            }

            ToggleUIControls(true);

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
                return;
            // throw new Exception("User canceled scanning");

            // background worker populated the Globals.SongCollection
            masterSongCollection = Globals.SongCollection;
            // -- CRITCAL -- this populates Arrangment DLCKey info in Arrangements2D
            masterSongCollection.ToList().ForEach(a => a.Arrangements2D.ToList().ForEach(arr => arr.Parent = a));

            sw.Stop();
            Globals.Log(String.Format("Parsing archives from {0} took: {1} (msec)", Constants.Rs2DlcFolder, sw.ElapsedMilliseconds));
            Globals.RescanSetlistManager = false;
            Globals.RescanDuplicates = false;
            Globals.RescanSongManager = false;
            Globals.RescanRenamer = false;
            Globals.ReloadSongManager = false;

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
            var results = masterSongCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tuning.ToLower().Contains(lowerCriteria) || x.Arrangements.ToLower().Contains(lowerCriteria) || x.CharterName.ToLower().Contains(lowerCriteria) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(lowerCriteria) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(lowerCriteria)) || x.SongYear.ToString().Contains(criteria) || x.FilePath.ToLower().Contains(lowerCriteria))).ToList();

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
                var currentSong = DgvExtensions.GetObjectFromRow<SongData>(selectedRow);
                if (currentSong != null)
                {
                    currentSong.IgnitionAuthor = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "name");
                    selectedRow.Cells["IgnitionAuthor"].Value = currentSong.IgnitionAuthor;
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
                    LoadFilteredBindingList(masterSongCollection.Where(x => x.CharterName == charterName).ToList());
                else
                    PopulateDataGridView();
            }
            else
            {
                Globals.Log("To use this feature, go to 'Settings' menu and enter a charter name ...");
                tsmiModsMyCDLC.Checked = false;
            }
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

        private void checkAllForUpdates(object sender, DoWorkEventArgs e)
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
                    if (!bWorker.CancellationPending)
                    {
                        foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                        {
                            //string songname = row.Cells[3].Value.ToString();
                            if (!bWorker.CancellationPending)
                            {
                                DataGridViewRow currentRow = (DataGridViewRow)row;
                                if (!currentRow.Cells["FileName"].Value.ToString().Contains("rs1comp"))
                                    CheckRowForUpdate(currentRow);
                            }
                            else
                                bWorker.Abort();
                        }
                    }
                    SaveSongCollectionToFile();
                });

            counterStopwatch.Stop();
        }

        private void chkSubFolders_MouseUp(object sender, MouseEventArgs e)
        {
            cueSearch.Text = String.Empty;

            if (!chkSubFolders.Checked)
            {
                var results = masterSongCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                RemoveFilter();
        }

        private void cmsBackupSelection_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            FileTools.CreateBackupOfType(selection, Constants.BackupsFolder, "");
        }

        private void cmsCheckForUpdate_Click(object sender, EventArgs e)
        {
            //bWorker = new AbortableBackgroundWorker();
            //bWorker.SetDefaults();
            //bWorker.DoWork += CheckForUpdatesEvent;
            //// don't run bWorker more than once
            //if (!bWorker.IsBusy)
            //    bWorker.RunWorkerAsync();
        }

        private void cmsDeleteSong_Click(object sender, EventArgs e)
        {
            DeleteSelection();
        }

        private void cmsDisableEnableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
            var originalPath = selection.FilePath;
            var originalFile = selection.FileName;

            if (!originalPath.ToLower().Contains(Constants.RS1COMP))
            {
                try
                {
                    if (selection.Enabled == "Yes")
                    {
                        var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                        File.Move(originalPath, disabledPath);
                        dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value = disabledPath;
                        dgvSongsMaster.SelectedRows[0].Cells["colFileName"].Value = originalFile.Replace("_p.psarc", "_p.disabled.psarc");
                        dgvSongsMaster.SelectedRows[0].Cells["colEnabled"].Value = "No";
                    }
                    else
                    {
                        var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                        File.Move(originalPath, enabledPath);
                        dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value = enabledPath;
                        dgvSongsMaster.SelectedRows[0].Cells["colFileName"].Value = originalFile.Replace("_p.disabled.psarc", "_p.psarc");
                        dgvSongsMaster.SelectedRows[0].Cells["colEnabled"].Value = "Yes";
                    }

                    UpdateToolStrip();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(string.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er, Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                }
            }
            else
                Globals.Log(String.Format(Properties.Resources.ThisIsARocksmith1CompatiblitySongX0RS1Comp, Environment.NewLine));
        }

        private void cmsEditSong_Click(object sender, EventArgs e)
        {
            DgvExtensions.SaveSorting(dgvSongsMaster);
            var selection = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
            var filePath = selection.FilePath;
            var enabled = selection.Enabled == "Yes";

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

        private void cmsOpenDLCPage_Click(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SelectedRows.Count == 1)
            {
                var song = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster.SelectedRows[0]);
                if (song != null)
                {
                    if (song.IgnitionID == null || song.IgnitionID == "No Results")
                        song.IgnitionID = Ignition.GetDLCInfoFromURL(song.GetInfoURL(), "id");

                    if (song.IgnitionID == null || song.IgnitionID == "No Results")
                        Globals.Log("<ERROR>: Song doesn't exist in Ignition anymore.");
                    else
                        Process.Start(string.Format("{0}/{1}", Constants.DefaultDetailsURL, song.IgnitionID));
                }
            }
        }

        private void cmsPlaySelectedSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastSelectedSongPath != string.Empty && lastSelectedSongPath != dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString())
                Globals.AudioEngine.Stop();

            PlaySongFunction.DynamicInvoke();
        }

        private void cmsShowDLCInfo_Click(object sender, EventArgs e)
        {
            ShowSongInfo();
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ResetDetail();

            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(masterSongCollection);
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
                    var songDetails = masterSongCollection.Where(master => (master.DLCKey == songKey)).ToList();
                    if (!songDetails.Any())
                        MessageBox.Show("No Song Details Found");
                    else
                    {
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = "TRUE";
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = MinusBitmap;

                        // CRITICAL EXECUTION ORDER - Workaround for intermitent bug not displaying horizscrollbar when last row is selected
                        dgvSongsDetail.Visible = true;
                        dgvSongsDetail.ScrollBars = ScrollBars.Horizontal;

                        // CRITICAL CODE AREA - CAREFUL - No Filtering KISS
                        dgvSongsDetail.AutoGenerateColumns = false;
                        dgvSongsDetail.DataSource = songDetails;
                        dgvSongsDetail.DataMember = "Arrangements2D";

                        // apply some fixed formatting
                        // any column/row sizing triggers SubclassedDataGridView
                        dgvSongsDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
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
                        dgvSongsDetail.Columns["colDetailChords"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgvSongsDetail.Columns["colDetailChords"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells; // work around forces horiz scroll to display

                        // calculate the height and width of dgvSongsDetail
                        var colHeaderHeight = dgvSongsMaster.Columns[e.ColumnIndex].HeaderCell.Size.Height;
                        var rowHeight = dgvSongsMaster.Rows[e.RowIndex].Height;
                        var colWidth = dgvSongsMaster.Columns[e.ColumnIndex].Width;
                        var horizScrollHeight = SystemInformation.HorizontalScrollBarHeight;

                        var vertScrollWidth = 0;
                        if (dgvSongsMaster.Controls.OfType<VScrollBar>().First().Visible)
                            vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

                        dgvSongsDetail.Width = dgvSongsMaster.Width - vertScrollWidth - dgvSongsDetail.Location.X + 5; // tweak
                        dgvSongsDetail.Height = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height + 1) + colHeaderHeight + horizScrollHeight;

                        // height tweak
                        if (dgvSongsDetail.RowCount < 3)
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 4;

                        // given current formatting what is the real width
                        //var rowsWidth = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height);
                        var colsWidth = dgvSongsDetail.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width);
                        if (colsWidth < dgvSongsDetail.Width)
                        {
                            dgvSongsDetail.Columns["colDetailChords"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                    string arrInit = song.Arrangements.ToUpper();

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
                    var s = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, e.RowIndex);
                    s.Selected = !s.Selected;
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
                    var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, e.RowIndex);
                    cmsEditSong.Enabled = !sng.OfficialDLC;
                    cmsDeleteSong.Enabled = !sng.OfficialDLC;
                    cmsTaggerPreview.Enabled = !sng.OfficialDLC;
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
                // beyound current scope of CFM
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

            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);
            // filter applied
            if (!String.IsNullOrEmpty(filterStatus) && dgvPainted)
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
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSongsMaster.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvSongsMaster_KeyDown(object sender, KeyEventArgs e)
        {
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
                        var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvSongsMaster, i);
                        // beyound current scope of CFM
                        if (sng.IsRsCompPack)
                            Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                        else
                        {
                            sng.Selected = !sng.Selected;
                        }
                    }
                }
            }
        }

        private void dgvSongsMaster_KeyUp(object sender, KeyEventArgs e)
        {
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

        private void dgvSongsMaster_Sorted(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SortedColumn != null)
            {
                AppSettings.Instance.SortColumn = dgvSongsMaster.SortedColumn.DataPropertyName;
                AppSettings.Instance.SortAscending = dgvSongsMaster.SortOrder == SortOrder.Ascending ? true : false;

                if (lastSelectedSongPath != string.Empty) //Reselect last selected row after sorting
                {
                    int newRowIndex = dgvSongsMaster.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.Cells["colFilePath"].Value.ToString() == lastSelectedSongPath).Index;
                    dgvSongsMaster.Rows[newRowIndex].Selected = true;
                    dgvSongsMaster.FirstDisplayedScrollingRowIndex = newRowIndex;
                }
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            RemoveFilter();
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

        private void tsmiFilesBackup_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            FileTools.CreateBackupOfType(selection, Constants.BackupsFolder, Constants.EXT_BAK);
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

        private void tsmiFilesCheckUpdates_Click(object sender, EventArgs e)
        {
            // TODO: need to remove RS1 Compatiblity DLCs from this check
            GenExtensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Enabled = true; });
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += checkAllForUpdates;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();

            GenExtensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Visible = false; });
        }

        private void tsmiFilesCleanDlc_Click(object sender, EventArgs e)
        {
            FileTools.CleanDlcFolder();
        }

        private void tsmiFilesDelete_Click(object sender, EventArgs e)
        {
            // reset safety interlock ... must come first
            tsmiFilesSafetyInterlock.Checked = false;
            DeleteSelection();
        }

        private void tsmiFilesDisableEnable_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value) || row.Selected)
                {
                    var originalPath = row.Cells["colFilePath"].Value.ToString();
                    var originalFile = row.Cells["colFileName"].Value.ToString();

                    if (!originalPath.ToLower().Contains(Constants.RS1COMP))
                    {
                        try
                        {
                            if (row.Cells["colEnabled"].Value.ToString() == "Yes")
                            {
                                var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(originalPath, disabledPath);
                                row.Cells["colFilePath"].Value = disabledPath;
                                row.Cells["colFileName"].Value = originalFile.Replace("_p.psarc", "_p.disabled.psarc");
                                row.Cells["colEnabled"].Value = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(originalPath, enabledPath);
                                row.Cells["colFilePath"].Value = enabledPath;
                                row.Cells["colFileName"].Value = originalFile.Replace("_p.disabled.psarc", "_p.psarc");
                                row.Cells["colEnabled"].Value = "Yes";
                            }

                            UpdateToolStrip();
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er, Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                        }
                    }
                    else
                        Globals.Log(String.Format(Properties.Resources.ThisIsARocksmith1CompatiblitySongX0RS1Comp, Environment.NewLine));
                }
            }
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
                selection.AddRange(Globals.SongCollection.Where(x => x.CharterName == "Ubisoft").ToList());
                // prevent double selecting ODLC files
                selection = selection.GroupBy(x => x.FilePath).Select(s => s.First()).ToList();
            }

            // never try to rename dlc compatibility files ... doh!
            selection = selection.Where(fi => !fi.FileName.ToLower().Contains(Constants.RS1COMP) && !fi.FileName.ToLower().Contains(Constants.SONGPACK) && !fi.FileName.ToLower().Contains(Constants.ABVSONGPACK)).ToList();

            if (!selection.Any()) return;

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

        private void tsmiFilesSafetyInterlock_CheckStateChanged(object sender, EventArgs e)
        {
            tsmiFilesDelete.Enabled = tsmiFilesSafetyInterlock.Checked;

            if (tsmiFilesDelete.Enabled)
                tsmiFilesDelete.BackColor = Color.Red;
            else
                tsmiFilesDelete.BackColor = SystemColors.Control;

            tsmiFiles.ShowDropDown();
            menuStrip.Focus();
        }

        private void tsmiFilesUnorganize_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);

            if (tsmiFilesIncludeODLC.Checked)
            {
                selection.AddRange(Globals.SongCollection.Where(x => x.CharterName == "Ubisoft").ToList());
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
                    LoadFilteredBindingList(masterSongCollection.Where(x => x.CharterName == charterName).ToList());
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

            // quickly reload the SongCollection to the dgv
            if (Globals.ReloadSongManager)
            {
                LoadFilteredBindingList(Globals.SongCollection);
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

        private void tsmiModsPitchShifterCheckbox_Click(object sender, EventArgs e)
        {
            tsmiMods.ShowDropDown();
            tsmiModsPitchShifter.ShowDropDown();
            menuStrip.Focus();
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

            // new 'Downloads' CDLC added or corrupt CDLC were removed
            // quickly reload the SongCollection to the dgv
            if (Globals.ReloadSongManager)
            {
                LoadFilteredBindingList(Globals.SongCollection);
                Globals.ReloadSongManager = false;
            }

            // deselect the repaired songs
            //allSelected = true;
            //SelectAllNone();
        }

        private void tsmiRescanFull_Click(object sender, EventArgs e)
        {
            if (AppSettings.Instance.IncludeAnalyzerData)
            {
                if (MessageBox.Show("You are about to run a full rescan with extra metadata, " +
                    "which is fairly longer than the normal scan?" + Environment.NewLine +
                    "Do you want to proceed?", "Include Analyzer Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)

                    return;
            }

            RefreshDgv(true, AppSettings.Instance.IncludeAnalyzerData);
        }

        private void tsmiRescanQuick_Click(object sender, EventArgs e)
        {
            RefreshDgv(false, AppSettings.Instance.IncludeAnalyzerData);
        }

        public DataGridView GetGrid()
        {
            return dgvSongsMaster;
        }

        public void TabEnter()
        {
            Globals.Log("SongManager GUI TabEnter ...");
            Globals.DgvCurrent = dgvSongsMaster;

            // fixed bug ... index 0 has no value is thrown when flipping between tabs and back
            // is caused by method TabEnter() with call to PopulateSongManager();
        }

        public void TabLeave()
        {
            SetRepairOptions();
            Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
            Globals.Log("SongManager GUI TabLeave ...");
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

        private void cmsGetAnalyzerData_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any())
                return;

            Globals.Log("Please wait ...");
            Globals.Log("Getting Analyzer Data for selected songs ...");
            DoWork(Constants.GWORKER_ANALYZE, selection);

            dgvSongsMaster.Refresh();
        }


    }
}

