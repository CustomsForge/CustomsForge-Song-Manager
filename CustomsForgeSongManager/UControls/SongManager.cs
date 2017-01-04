﻿using System;
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

// Think twice before adding any code to this already bloated UC.

namespace CustomsForgeSongManager.UControls
{
    public partial class SongManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private Bitmap MinusBitmap = new Bitmap(Properties.Resources.minus);
        private Bitmap PlusBitmap = new Bitmap(Properties.Resources.plus);
        private Color _Disabled = Color.Red;
        private Color _Enabled = Color.Lime;
        private bool allSelected = false;
        private AbortableBackgroundWorker bWorker;
        private bool bindingCompleted = false;
        private Stopwatch counterStopwatch = new Stopwatch();
        private bool dgvPainted = false;
        private BindingList<SongData> masterSongCollection = new BindingList<SongData>();
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private string lastSelectedSongPath = string.Empty;
        public Delegate PlaySongFunction;

        public SongManager()
        {
            InitializeComponent();

            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            dgvSongsDetail.Visible = false;

            PopulateTagger();
            PopulateSongManager();
            cmsTaggerPreview.Visible = true; // ???
            FileTools.VerifyCfsmFolders();
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
                    Globals.AudioEngine.Play();
                    Globals.Log(String.Format("Playing {0} by {1} ... ({2})", sng.Title, sng.Artist, Path.GetFileName(sng.FilePath)));
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
                dom.Save(Constants.SongsInfoPath);
                Globals.Log("Saved song collection file ...");
            }
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

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    TemporaryDisableDatabindEvent(() => dgvSongsMaster.Rows.Remove(row));
            }

            FileTools.DeleteFiles(selection);
            UpdateToolStrip();
        }

        private void GetRepairOptions()
        {
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
        }

        private void InitializeRepairMenu()
        {
            // restore saved repair options
            GetRepairOptions();

            var items = tsmiRepairs.DropDownItems;

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
                ToggleRepairMenu(item);
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
            var songsInfoPath = Constants.SongsInfoPath;
            bool correctVersion = false;

            try
            {
                // load songs from file into memory
                if (File.Exists(songsInfoPath))
                {
                    XmlDocument dom = new XmlDocument();
                    dom.Load(songsInfoPath);
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
                    }

                    masterSongCollection = SerialExtensions.XmlDeserialize<BindingList<SongData>>(listNode.OuterXml);

                    if (masterSongCollection == null || masterSongCollection.Count == 0)
                        throw new SongCollectionException(masterSongCollection == null ? "Master Collection = null" : "Master Collection.Count = 0");

                    Globals.SongCollection = masterSongCollection;

                    if (correctVersion)
                        Rescan(false); // smart scan
                    else
                        Globals.Log("Incorrect song collection version found, rescanning songs.");
                }

                if (!correctVersion || !File.Exists(songsInfoPath))
                    Rescan(true); // full scan
                else
                    Globals.Log("Loaded saved song collection file ...");

                PopulateDataGridView();
            }
            catch (Exception e)
            {
                // failsafe ... delete CFM folder files and start over
                string err = e.Message;
                if (e.InnerException != null)
                    err += ", Inner: " + e.InnerException.Message;

                Globals.Log("<Error>: " + e.Message);
                // log needs to written before it is deleted ... Bazinga
                Globals.Log("Deleted CFSM folder and subfolders from My Documents ...");

                if (Directory.Exists(Constants.WorkFolder))
                    ZipUtilities.DeleteDirectory(Constants.WorkFolder);

                MessageBox.Show(string.Format("{0}{1}{1}The program will now shut down.", err, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            // var debugHere = masterSongCollection;
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

        private void RefreshDgv(bool fullRescan)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan(fullRescan);
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

        private void Rescan(bool fullRescan)
        {
            dgvSongsMaster.DataSource = null;
            dgvSongsDetail.Visible = false;

            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show("<Error>: Rocksmith 2014 installation directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // this is done here in case user decided to manually delete all songs
            // the default initial load condition does not include RS1 Compatiblity or SongPack files
            var dlcFiles = Directory.EnumerateFiles(Constants.Rs2DlcFolder, "*.psarc", SearchOption.AllDirectories).ToList();
            dlcFiles = dlcFiles.Where(fi => !fi.ToLower().Contains(Constants.RS1COMP) && !fi.ToLower().Contains(Constants.SONGPACK) && !fi.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            if (!dlcFiles.Any())
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

            Globals.TuningXml = null;// fix for Tunings 'Other'
            var sw = new Stopwatch();
            sw.Restart();

            // run new worker
            using (Worker worker = new Worker())
            {
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
            Globals.Log("Loaded fresh song collection file ...");
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

        private RepairOptions SetRepairOptions()
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

            foreach (var item in items.OfType<ToolStripEnhancedMenuItem>())
            {
                if (item.RadioButtonGroupName == clickedItemGroup && item.CheckMarkDisplayStyle == CheckMarkDisplayStyle.CheckBox)
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
            // to make a RadioButton work like a CheckBox ... set the
            // RadioButtonGroupName for each RadioButton to a unique name

            if (tsmiRepairsUsingOrg.Checked)
            {
                tsmiProcessDLFolder.Enabled = false;
                tsmiProcessDLFolder.Checked = false;
            }
            else
                tsmiProcessDLFolder.Enabled = false;

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

        private void cmsPlaySelectedSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlaySongFunction.DynamicInvoke();
        }

        private void cmsBackupSelection_Click(object sender, EventArgs e)
        {
            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvSongsMaster);
            if (!selection.Any()) return;

            FileTools.CreateBackupOfType(selection, Constants.BackupFolder, "");
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

        private void cmsEditSong_Click(object sender, EventArgs e)
        {
            var filePath = dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString();

            DgvExtensions.SaveSorting(dgvSongsMaster);
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);


            using (var songEditor = new frmSongEditor(filePath))
            {
                songEditor.Text = String.Format("{0}{1}", Properties.Resources.SongEditorLoaded, Path.GetFileName(filePath));
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
                    else // TODO: change positioning if near bottom of dgvSongsMaster
                    {
                        // apply some formatting
                        dgvSongsMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
                        dgvSongsDetail.Columns["colDetailPID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.Columns["colDetailSections"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvSongsDetail.Columns["colDetailDMax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        var rowHeight = dgvSongsMaster.Rows[e.RowIndex].Height + 0; // height tweak
                        var colWidth = dgvSongsMaster.Columns[e.ColumnIndex].Width - 16; // width tweak
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = "TRUE";
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = MinusBitmap;
                        Rectangle dgvRectangle = dgvSongsMaster.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        dgvSongsDetail.Location = new Point(dgvRectangle.Right + colWidth, dgvRectangle.Bottom + rowHeight - 2);

                        // CRITICAL CODE AREA - CAREFUL - No Filtering
                        dgvSongsDetail.AutoGenerateColumns = false;
                        dgvSongsDetail.DataSource = songDetails;
                        dgvSongsDetail.DataMember = "Arrangements2D";

                        // calculate the height and width of dgvSongsDetail
                        dgvSongsDetail.Columns["colDetailKey"].Width = dgvSongsMaster.Columns["colKey"].Width;
                        var colHeaderHeight = dgvSongsDetail.Columns[e.ColumnIndex].HeaderCell.Size.Height;
                        dgvSongsDetail.Height = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height) + colHeaderHeight - 3;
                        dgvSongsDetail.Width = dgvSongsDetail.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width) + colWidth;
                        if (dgvSongsDetail.Rows.Count < 3) // need extra tweak 
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 4;

                        dgvSongsDetail.Refresh();
                        //dgvSongsDetail.Invalidate();
                        dgvSongsDetail.Visible = true;
                        dgvSongsMaster.ScrollBars = ScrollBars.Horizontal;
                    }
                }
                else
                {
                    dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = PlusBitmap;
                    dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                    dgvSongsMaster.ScrollBars = ScrollBars.Both;
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

                //if (dgvSongsMaster.SelectedRows.Count > 0)
                //    lastSelectedSongPath = dgvSongsMaster.SelectedRows[0].Cells["colFilePath"].Value.ToString();
                //else
                //    lastSelectedSongPath = string.Empty;
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
            TemporaryDisableDatabindEvent(() =>
                {
                    foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                        row.Cells["colSelect"].Value = !allSelected;
                });

            allSelected = !allSelected;
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

        private void tsmiFilesArcBak_Click(object sender, EventArgs e)
        {
            this.Refresh();
            DoWork(Constants.GWORKER_ACHRIVE, Constants.EXT_BAK, Constants.BackupFolder, tsmiFilesArcDeleteAfter.Checked);
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

            FileTools.CreateBackupOfType(selection, Constants.BackupFolder, Constants.EXT_BAK);
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
            DeleteSelection();
            // reset safety interlock
            tsmiFilesSafetyInterlock.Checked = false;
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

        private void tsmiFilesRestoreBak_Click(object sender, EventArgs e)
        {
            FileTools.RestoreBackups(Constants.EXT_BAK, Constants.BackupFolder);
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

        private void tsmiHelpErrorLog_Click(object sender, EventArgs e)
        {
            RepairTools.ViewErrorLog();
        }

        private void tsmiHelpGeneral_Click(object sender, EventArgs e)
        {
            RepairTools.ShowNoteViewer("CustomsForgeSongManager.Resources.HelpSongMgr.txt", "Song Manager Help");
        }

        private void tsmiHelpRepairs_Click(object sender, EventArgs e)
        {
            RepairTools.ShowNoteViewer("CustomsForgeSongManager.Resources.HelpRepairs.txt", "Repairs Help");
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
            if (!selection.Any()) return;

            this.Refresh();
            DoWork(Constants.GWORKER_PITCHSHIFT, selection, tsmiModsPitchShiftOverwrite.Checked);

            // TODO: determine better way to refresh the dgv contents
            RefreshDgv(false);
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

        private void tsmiOverwriteCDLC_Click(object sender, EventArgs e)
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
            // start new generic worker
            DoWork("repairing", selection, SetRepairOptions());

            // RepairTools.RepairSongs(selection, SetRepairOptions());
            // TODO: determine better way to refresh the dgv contents
          
            // TODO: fix this ^ - commented for now because it's not intutive
           RefreshDgv(false);

        }

        private void tsmiRescanFull_Click(object sender, EventArgs e)
        {
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
            Globals.Log("SongManager GUI TabEnter ...");
            Globals.DgvCurrent = dgvSongsMaster;

            // fixed bug ... index 0 has no value is thrown when flipping between tabs and back
            // is caused by method TabEnter() with call to PopulateSongManager();
        }

        public void TabLeave()
        {
            Globals.Log("SongManager GUI TabLeave ...");
            Globals.Settings.SaveSettingsToFile(dgvSongsMaster);
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
    }
}

