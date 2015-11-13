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
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using CustomsForgeManager.SongEditor;
using DataGridViewTools;
using Newtonsoft.Json;
using System.Xml;
using Extensions = CustomsForgeManager.CustomsForgeManagerLib.Extensions;
using CFSM.Utils;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls.DataGridEditors;


namespace CustomsForgeManager.UControls
{
    public partial class SongManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        #region private fields
        private bool allSelected = true;
        private AbortableBackgroundWorker bWorker;
        private bool bindingCompleted = false;
        private Stopwatch counterStopwatch = new Stopwatch();
        private bool dgvPainted = false;
        private BindingList<SongData> masterSongCollection = new BindingList<SongData>();
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        Color _Enabled = Color.Lime;
        Color _Disabled = Color.Red;
        Bitmap PlusBitmap = new Bitmap(Properties.Resources.plus);
        Bitmap MinusBitmap = new Bitmap(Properties.Resources.minus);
        private Font OfficialDLCFont;
        #endregion

        public SongManager()
        {
            InitializeComponent();
            OfficialDLCFont = new Font("Arial", 8, FontStyle.Bold);
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            dgvSongsDetail.Visible = false;
            PopulateSongManager();
            cmsTaggerPreview.Visible = true;
            //DataGridViewCheckBoxHeaderCell cbHeader = new DataGridViewCheckBoxHeaderCell();
            //colSelect.HeaderCell = cbHeader;
            //colSelect.HeaderText = String.Empty;
            //colSelect.SortMode = DataGridViewColumnSortMode.NotSortable;
            //colSelect.HeaderCell.ToolTipText = String.Empty;
            //cbHeader.OnCheckBoxClicked += (Checked) =>
            //{
            //    TemporaryDisableDatabindEvent(() =>
            //    {
            //        for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
            //            GetSongByRow(i).Selected = Checked;
            //    });
            //    allSelected = Checked;
            //};
        }

        public void LeaveSongManager()
        {
            Globals.Log("Leaving SongManager GUI ...");
            Globals.DgvSongs = dgvSongsMaster;
        }

        public void LoadSongCollectionFromFile()
        {
            masterSongCollection.Clear();
            var songsInfoPath = Constants.SongsInfoPath;
            // load songs into memory
            try
            {
                bool correctVersion = File.Exists(songsInfoPath);
                XmlDocument dom = new XmlDocument();
                if (correctVersion)
                {
                    correctVersion = false;
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

                    if (correctVersion)
                        masterSongCollection = UtilExtensions.XmlDeserialize<BindingList<SongData>>(listNode.OuterXml);
                    else
                        Globals.Log("New song collection version found, rescanning songs.");
                }

                // pop Arrangment DLCKey info
                masterSongCollection.ToList().ForEach(a => { a.Arrangements2D.ToList().ForEach(arr => arr.Parent = a); });

                Globals.SongCollection = masterSongCollection;
                Globals.ReloadDuplicates = false;
                Globals.ReloadRenamer = false;
                Globals.ReloadSetlistManager = false;

                //rescan should be called AFTER setting Globals.SongCollection
                Rescan();

                if (masterSongCollection == null || masterSongCollection.Count == 0)
                    throw new SongCollectionException(masterSongCollection == null ? "Master Collection = null" : "Master Collection.Count = 0");

                Globals.Log("Loaded song collection file ...");
                PopulateDataGridView();
            }
            catch (Exception e)
            {
                // failsafe ... delete CFM folder and start over
                string err = e.Message;
                if (e.InnerException != null)
                    err += ", Inner: " + e.InnerException.Message;

                Globals.Log("Error: " + e.Message);
                Globals.Log("Deleted CFSM folder from My Documents ...");

                if (Directory.Exists(Constants.WorkDirectory))
                    if (Directory.Exists(Constants.WorkDirectory))
                    {
                        File.Delete(Constants.SettingsPath);
                        File.Delete(Constants.SongsInfoPath);
                    }
                MessageBox.Show(string.Format("{0}{1}{1}The program will now shut down.", err, Environment.NewLine), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void PopulateTagger()
        {
            cmsTaggerPreview.DropDownItems.Clear();
            foreach (string tagPreview in Globals.Tagger.Themes)
            {
                var tsi = cmsTaggerPreview.DropDownItems.Add(tagPreview);
                tsi.Click += (s, e) =>
                {
                    var sel = GetFirstSelected();
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
                                PictureBox pb = new PictureBox()
                                {
                                    SizeMode = PictureBoxSizeMode.CenterImage,
                                    Dock = DockStyle.Fill,
                                    Image = img
                                };
                                f.Controls.Add(pb);
                                f.ShowDialog(this.FindForm());
                            }
                        }
                        else
                            Globals.Log(String.Format("ERROR: Unable to preview {0}.", sel.Title));
                    }
                };
            }
        }

        public void PopulateSongManager()
        {
            Globals.Log("Populating SongManager GUI ...");
            PopulateTagger();

            // Hide main dgvSongsMaster until load completes
            dgvSongsMaster.Visible = false;
            //Load Song Collection from file must be called before
            LoadSongCollectionFromFile();

            //Worker actually does the sorting after parsing, this is just to tell the grid that it is sorted.
            if (!String.IsNullOrEmpty(AppSettings.Instance.SortColumn))
            {
                var colX = dgvSongsMaster.Columns.Cast<DataGridViewColumn>().Where(
                  col => col.DataPropertyName == AppSettings.Instance.SortColumn).FirstOrDefault();
                if (colX != null)
                    dgvSongsMaster.Sort(colX, AppSettings.Instance.SortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }
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
                Rescan();
            }

            if (Globals.ReloadSongManager)
            {
                Globals.ReloadSongManager = false;
                LoadSongCollectionFromFile();
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", masterSongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            numberOfDisabledDLC = masterSongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;
            var tsldcMsg = String.Format("Outdated: {0} | Disabled DLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void CheckForUpdatesEvent(object o, DoWorkEventArgs args)
        {
            // part of ContextMenuStrip action
            Extensions.InvokeIfRequired(dgvSongsMaster, delegate
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
                var currentSong = GetSongByRow(dataGridViewRow);
                if (currentSong != null)
                {
                    //currentSong.IgnitionVersion = Ignition.GetDLCInfoFromURL(currentSong.GetInfoURL(), "version");
                    string url = currentSong.GetInfoURL();
                    string response = "";
                    string cfUrl = "";
                    int version = 0;

                    string auth_token = "";

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
                        var columnSetting = AppSettings.Instance.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvSongsMaster.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                            //   dgvSongsMaster.Invalidate();
                        }
                    }

                    //foreach (var item in dgvSongsMaster.Columns.Cast<DataGridViewColumn>())
                    //    if (item.Visible)
                    //        dgvSongsMaster.InvalidateCell(item.HeaderCell);

                }
            }
        }

        private void DgvSongsAppearance()
        {
            // set all columns to read only except colSelect
            foreach (DataGridViewColumn col in dgvSongsMaster.Columns)
                col.ReadOnly = true;

            dgvSongsMaster.Visible = true; // needs to come now so settings apply correctly

            // see SongManager.Designer for custom appearance settings
            dgvSongsMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            dgvSongsMaster.AllowUserToAddRows = false; // removes empty row at bottom
            dgvSongsMaster.AllowUserToDeleteRows = false;
            dgvSongsMaster.AllowUserToOrderColumns = true;
            dgvSongsMaster.AllowUserToResizeColumns = true;
            dgvSongsMaster.AllowUserToResizeRows = false;
            dgvSongsMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvSongsMaster.BackgroundColor = SystemColors.AppWorkspace;
            dgvSongsMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // required when using FilteredBindingList
            dgvSongsMaster.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvSongsMaster.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // set custom selection (highlighting) color
            dgvSongsMaster.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongsMaster.DefaultCellStyle.BackColor; // or removes selection highlight
            dgvSongsMaster.DefaultCellStyle.SelectionForeColor = dgvSongsMaster.DefaultCellStyle.ForeColor;
            // this overrides any user ability to make changes 
            // dgvSongsMaster.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvSongsMaster.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSongsMaster.EnableHeadersVisualStyles = true;
            dgvSongsMaster.Font = new Font("Arial", 8);
            dgvSongsMaster.GridColor = SystemColors.ActiveCaption;
            dgvSongsMaster.MultiSelect = false;
            dgvSongsMaster.Name = "dgvSongsMaster";
            dgvSongsMaster.RowHeadersVisible = false; // remove row arrow
            dgvSongsMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // always visible on restart in case user changed
            dgvSongsMaster.Columns["colSelect"].ReadOnly = false; // is overridden by EditProgrammatically
            dgvSongsMaster.Columns["colSelect"].Visible = true;
            dgvSongsMaster.Columns["colSelect"].Width = 50;
            dgvSongsMaster.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSongsMaster.Columns["colEnabled"].Width = 54;
            // prevents double line headers on filtered columns
            dgvSongsMaster.Columns["colKey"].Width = 95;
            dgvSongsMaster.Columns["colKey"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvSongsMaster.Columns["colAppID"].Width = 80;
            dgvSongsMaster.Columns["colAppID"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvSongsMaster.Refresh();
        }

        private void DisableEnabled()
        {
            // TODO: impliment this if RS1 Compatiblity Songs are included by default
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                if (row.Cells["colPath"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    row.Cells["colEnabled"].Value = false;
            }
        }

        public SongData GetSongByRow(int RowIndex)
        {
            if (RowIndex == -1)
                return null;
            return (SongData)dgvSongsMaster.Rows[RowIndex].DataBoundItem;
        }

        private SongData GetSongByRow(DataGridViewRow dataGridViewRow)
        {
            return (SongData)dataGridViewRow.DataBoundItem;
        }

        private void LoadFilteredBindingList(IList<SongData> list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSongsMaster.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvSongsMaster.DataSource = bs;
        }

        private void PopulateDataGridView() // binding data to grid
        {
            LoadFilteredBindingList(masterSongCollection);

            // update datagrid appearance
            DgvSongsAppearance();

            // reload column order, width, visibility
            if (AppSettings.Instance.ManagerGridSettings != null)
                dgvSongsMaster.ReLoadColumnOrder(AppSettings.Instance.ManagerGridSettings.ColumnOrder);

            // start fresh ... clear Selected in dgvSongsMaster and object SongData
            for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
            {
                DataGridViewRow row = dgvSongsMaster.Rows[i];
                row.Cells["colSelect"].Value = false;
                masterSongCollection[i].Selected = false;
            }
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {
            // save current column status
            Globals.DgvSongs = dgvSongsMaster;
            Globals.Settings.SaveSettingsToFile();

            if (AppSettings.Instance.ManagerGridSettings == null)
                return;

            contextMenuStrip.Items.Clear();
            RADataGridViewSettings gridSettings = AppSettings.Instance.ManagerGridSettings;
            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
            {
                var cn = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name;
                if (cn.ToLower().StartsWith("col"))
                    cn = cn.Remove(0, 3);
                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(cn, null, ColumnMenuItemClick)
                        {
                            Checked = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Visible,
                            Tag = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name
                        };
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void Rescan(bool fullRescan = false)
        {
            dgvSongsMaster.DataSource = null;
            // save settings (column widths) in case user has modified
            Globals.Settings.SaveSettingsToFile();
            ResetDetail();

            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show("Error: Rocksmith 2014 installation directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // this is done here in case user decided to manually delete all songs
            // the default initial load condition does not include RS1 Compatiblity files
            var dlcFiles = Directory.EnumerateFiles(Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"), "*.psarc", SearchOption.AllDirectories)
                .Where(fi => !fi.ToLower().Contains(Constants.RS1COMP)).ToArray();

            if (!dlcFiles.Any())
            {
                var msgText =
                   string.Format("Houston ... we have a problem!{0}There are no Rocksmith 2014 songs in:" +
                   "{0}{1}{0}{0}Please select a valid Rocksmith 2014{0}installation directory when you restart CFSM.  ",
                   Environment.NewLine, Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"));
                MessageBox.Show(msgText, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                if (Directory.Exists(Constants.WorkDirectory))
                {
                    File.Delete(Constants.SongsInfoPath);
                }

                // prevents write log attempt and shutsdown app
                Environment.Exit(0);
            }
            if (fullRescan)
                Globals.SongCollection.Clear();

            ToggleUIControls(false);
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

            masterSongCollection = Globals.SongCollection;
            SaveSongCollectionToFile();

            LoadFilteredBindingList(masterSongCollection);

            Globals.RescanSetlistManager = false;
            Globals.RescanDuplicates = false;
            Globals.RescanSongManager = false;
            Globals.RescanRenamer = false;
            Globals.ReloadSongManager = false;
            Globals.ReloadDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
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
            var results = masterSongCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(criteria.ToLower()) ||
                x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Arrangements.ToLower().Contains(criteria.ToLower()) ||
                x.Charter.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower())) ||
                x.SongYear.ToString().Contains(criteria) ||
                x.Path.ToLower().Contains(criteria.ToLower()))).ToList();

            LoadFilteredBindingList(results);
        }

        private void ShowSongInfo()
        {
            if (dgvSongsMaster.SelectedRows.Count > 0)
            {
                var song = GetFirstSelected();
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show(string.Format("Please select (highlight) the song that  {0}you would like information about.", Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public SongData GetFirstSelected()
        {
            if (dgvSongsMaster.SelectedRows.Count > 0)
                return GetSongByRow(dgvSongsMaster.SelectedRows[0]);
            return null;
        }



        public List<SongData> GetSelectedSongs()
        {
            List<SongData> SelectedSongs = new List<SongData>();

            // the checkbox value change was not being detected here so (known VS issue)
            // uncommented code from dgvSongsMaster_CellMouseUp to make it detectable
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                var sd = GetSongByRow(row);
                if (sd != null && sd.Selected)
                    SelectedSongs.Add(sd);
            }
            return SelectedSongs;
        }

        private void ToggleUIControls(bool enable)
        {
            Extensions.InvokeIfRequired(btnRescan, delegate { btnRescan.Enabled = enable; });
            Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = enable; });
            Extensions.InvokeIfRequired(cueSearch, delegate { cueSearch.Enabled = enable; });
            Extensions.InvokeIfRequired(btnDisableEnableSongs, delegate { btnDisableEnableSongs.Enabled = enable; });
            Extensions.InvokeIfRequired(cmsSelection, delegate { cmsSelection.Enabled = enable; });
            Extensions.InvokeIfRequired(lnkLblSelectAll, delegate { lnkLblSelectAll.Enabled = enable; });
            Extensions.InvokeIfRequired(lnkClearSearch, delegate { lnkClearSearch.Enabled = enable; });
        }

        private void UpdateCharter(DataGridViewRow selectedRow)
        {
            try
            {
                var currentSong = GetSongByRow(selectedRow);
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

        private void btnBackupSelectedDLCs_Click(object sender, EventArgs e)
        {
            string backupDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "backups");
            string fileName = String.Empty;
            string filePath = String.Empty;

            try
            {
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                {
                    if (row.Selected)
                    {
                        // beyound current scope of CFM
                        if (row.Cells["colSelect"].Value.ToString().Contains(Constants.RS1COMP))
                        {
                            Globals.Log("Can not 'Backup' individual RS1 Compatiblity DLC");
                            continue;
                        }

                        filePath = row.Cells["Path"].Value.ToString();
                        fileName = Path.GetFileName(filePath);

                        if (File.Exists(Path.Combine(backupDir, fileName)))
                            File.Delete(filePath);
                        File.Copy(filePath, Path.Combine(backupDir, fileName));
                        continue;
                    }

                    if (row.Cells["colSelect"].Value == null)
                        row.Cells["colSelect"].Value = false;

                    if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    {
                        filePath = row.Cells["Path"].Value.ToString();
                        fileName = Path.GetFileName(filePath);

                        if (File.Exists(Path.Combine(backupDir, fileName)))
                            File.Delete(filePath);
                        File.Copy(filePath, Path.Combine(backupDir, fileName));
                    }
                }
            }
            catch (IOException ex)
            {
                Globals.Log("<ERROR>: " + ex.Message);
            }
        }

        private void btnCheckAllForUpdates_Click(object sender, EventArgs e)
        {
            // TODO: need to remove RS1 Compatiblity DLCs from this check
            Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Enabled = true; });
            bWorker = new AbortableBackgroundWorker();
            bWorker.SetDefaults();
            bWorker.DoWork += checkAllForUpdates;

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();

            Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Visible = false; });
        }

        private void btnDeleteSongs_Click(object sender, EventArgs e)
        {
            bool safe2Delete = false;

            // remove rows from datagridview going backward to avoid index issues
            for (int ndx = dgvSongsMaster.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSongsMaster.Rows[ndx];
                var SD = GetSongByRow(row);


                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    string songPath = SD.Path;

                    // redundant for file safety
                    if (chkEnableDelete.Checked && !safe2Delete)
                    {
                        // DANGER ZONE
                        if (MessageBox.Show(string.Format(
                            Properties.Resources.YouAreAboutToPermanentlyDeleteAllSelectedS,
                            Environment.NewLine), Constants.ApplicationName + " ... Warning ... Warning",
                                            MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        safe2Delete = true;
                    }

                    // redundant for file safety
                    if (safe2Delete)
                    {
                        try
                        {
                            SD.Delete();
                            dgvSongsMaster.Rows.Remove(row);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToDeleteSongX0X1ErrorX2, songPath, Environment.NewLine, ex.Message), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            // rescan on tabpage change
            // Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
        }

        private void PrepareDropdown()
        {
            tagToolStripMenuItem.DropDownItems.Clear();

            //SongData sdata = null;
            //var sngs = GetSelectedSongs().Where(sd => !sd.Tagged).ToArray(); // 
            //if (sngs.Length == 0)
            //    sngs = Globals.SongCollection.Where(sd => !sd.Tagged).ToArray();
            //if (sngs.Length > 0)
            //    sdata = sngs[Globals.random.Next(sngs.Length - 1)];
            foreach (var x in Globals.Tagger.Themes)
            {
                ToolStripMenuItem mi = new ToolStripMenuItem(x);
                mi.Click += (s, ev) =>
                {
                    var selection = GetSelectedSongs(); //.Where(sd => sd.Tagged == false);
                    if (selection.Count > 0)
                    {
                        Globals.Tagger.ThemeName = ((ToolStripMenuItem)s).Text;
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
                };

                //if (sdata != null)
                //{
                //    var img = Globals.Tagger.Preview(sdata,x).ResizeImage(100,100);
                //    PictureBox pb = new PictureBox() { Image = img, Width = img.Width, Height = img.Height,Tag = x,
                //                                       SizeMode = PictureBoxSizeMode.CenterImage,
                //                                       Dock = DockStyle.Fill
                //    };

                //    ToolStripControlHost mi2 = new ToolStripControlHost(pb) {  Height = pb.Height, Tag = x };
                //    pb.Click += (s, ev) =>
                //    {
                //        var selection = GetSelectedSongs(); 
                //        if (selection.Count > 0)
                //        {
                //            Globals.Tagger.ThemeName = ((PictureBox)s).Tag.ToString();
                //            Globals.Tagger.OnProgress += TaggerProgress;
                //            try
                //            {
                //                Globals.Tagger.TagSongs(selection.ToArray());
                //            }
                //            finally
                //            {
                //                Globals.Tagger.OnProgress -= TaggerProgress;
                //                // force dgvSongsMaster data to refresh after Tagging
                //                GetGrid().Invalidate();
                //                GetGrid().Refresh();
                //            }
                //        }

                //    };
                //    mi.DropDownItems.Add(mi2);
                //}
                // ToolStripItem
                tagToolStripMenuItem.DropDownItems.Add(mi);
            }
        }

        public void btnSelectionClick(object sender, EventArgs e)
        {
            if (tagToolStripMenuItem.DropDownItems.Count == 0)
            {
                Cursor = Cursors.WaitCursor;
                PrepareDropdown();
                Cursor = Cursors.Default;
            }
            cmsSelection.Show(btnDisableEnableSongs, 0, btnDisableEnableSongs.Height + 2);
        }

        private void btnDisableEnableSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    var originalPath = row.Cells["colPath"].Value.ToString();
                    var originalFile = row.Cells["colFileName"].Value.ToString();

                    if (!originalPath.ToLower().Contains(Constants.RS1COMP))
                    {
                        try
                        {
                            if (row.Cells["colEnabled"].Value.ToString() == "Yes")
                            {
                                var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(originalPath, disabledPath);
                                row.Cells["colPath"].Value = disabledPath;
                                row.Cells["colFileName"].Value = originalFile.Replace("_p.psarc", "_p.disabled.psarc");
                                row.Cells["colEnabled"].Value = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(originalPath, enabledPath);
                                row.Cells["colPath"].Value = enabledPath;
                                row.Cells["colFileName"].Value = originalFile.Replace("_p.disabled.psarc", "_p.psarc");
                                row.Cells["colEnabled"].Value = "Yes";
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(Properties.Resources.UnableToEnableDisableSongX0InDlcFolderX1Er,
                                Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                        }

                        // row.Cells["colSelect"].Value = false;
                        numberOfDisabledDLC = masterSongCollection.Where(song => song.Enabled == "No").ToList().Count();
                        var tsldcMsg = String.Format("Outdated: {0} | Disabled DLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
                        Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_DisabledCounter.Text = tsldcMsg; });
                    }
                    else
                        Globals.Log(
                            string.Format(Properties.Resources.ThisIsARocksmith1CompatiblitySongX0RS1Comp, Environment.NewLine));
                }
            }

            dgvSongsMaster.Refresh();

            // rescan on tabpage change
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan(Control.ModifierKeys == Keys.Control);
            UpdateToolStrip();
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
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

                Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_Cancel.Visible = false; });
            }

            //Thread.Sleep(3000);
            counterStopwatch.Restart();
            Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = false; });

            Extensions.InvokeIfRequired(dgvSongsMaster, delegate
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

        private void chkEnableDelete_CheckedChanged(object sender, EventArgs e)
        {
            btnDeleteSongs.Enabled = chkEnableDelete.Checked;

            if (btnDeleteSongs.Enabled)
                btnDeleteSongs.BackColor = Color.Red;
            else
                btnDeleteSongs.BackColor = SystemColors.Control;

        }

        private void chkTheMover_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvSongsMaster.DataSource == null)
                return;

            for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
            {
                DataGridViewRow row = dgvSongsMaster.Rows[i];
                var artist = (string)row.Cells["colArtist"].Value;

                // 'The' mover
                if (chkTheMover.Checked)
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

        private void cmsBackupDLC_Click(object sender, EventArgs e)
        {
            string backupPath = Path.Combine(AppSettings.Instance.RSInstalledDir, "backup");

            try
            {
                if (!Directory.Exists(backupPath))
                    Directory.CreateDirectory(backupPath);

                var filePath = masterSongCollection[dgvSongsMaster.SelectedRows[0].Index].Path;
                var fileName = Path.GetFileName(filePath);

                if (File.Exists(Path.Combine(backupPath, fileName)))
                    File.Delete(filePath);

                File.Copy(filePath, Path.Combine(backupPath, fileName));

                Globals.Log("Backup: " + fileName);
                Globals.Log("Successfully saved to: " + Path.Combine(backupPath, fileName));
            }
            catch (IOException ex)
            {
                MessageBox.Show(string.Format(Properties.Resources.PleaseSelectHighlightTheSongX0ThatYouWould, Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Globals.Log("<ERROR>: " + ex.Message);
            }
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
            if (dgvSongsMaster.SelectedRows[0].Cells["colPath"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
            {
                Globals.Log("Can not delete individual RS1 Compatibility DLC");
                Globals.Log("Go to Setting tab and uncheck 'Include RS1 Compatibility Pack'");
                Globals.Log("Then click 'Rescan' to remove all RS1 Compatibility DLCs");
                return;
            }

            try
            {
                if (MessageBox.Show(string.Format(Properties.Resources.DoYouReallyWantToRemoveThisCDLCX0Warning,
                    Environment.NewLine), Constants.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    SongData sd = GetSongByRow(dgvSongsMaster.SelectedRows[0]);
                    sd.Delete();
                    //  File.Delete(masterSongCollection[dgvSongsMaster.SelectedRows[0].Index].Path);
                    masterSongCollection.RemoveAt(dgvSongsMaster.SelectedRows[0].Index);
                }
            }
            catch (IOException ex)
            {
                Globals.Log("<ERROR>:" + ex.Message);
            }
        }

        private void cmsEditSong_Click(object sender, EventArgs e)
        {
            var filePath = dgvSongsMaster.SelectedRows[0].Cells["colPath"].Value.ToString();

            using (var songEditor = new frmSongEditor(filePath))
            {
                songEditor.Text = String.Format("{0}{1}", Properties.Resources.SongEditorLoaded,
                    Path.GetFileName(filePath));
                songEditor.ShowDialog();
            }

            if (Globals.RescanSongManager)
                UpdateToolStrip();
        }

        private void cmsGetCharterName_Click(object sender, EventArgs e)
        {
            // TODO: add image for GetCharterName to Context Menu Strip item
            Extensions.InvokeIfRequired(dgvSongsMaster, delegate
                {
                    if (dgvSongsMaster.SelectedRows.Count > 0)
                        UpdateCharter(dgvSongsMaster.SelectedRows[0]);
                });
        }

        private void cmsOpenDLCLocation_Click(object sender, EventArgs e)
        {
            var path = dgvSongsMaster.SelectedRows[0].Cells["colPath"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
        }

        private void cmsOpenDLCPage_Click(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SelectedRows.Count == 1)
            {
                var song = GetSongByRow(dgvSongsMaster.SelectedRows[0]);
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
                        if (dgvSongsDetail.Rows.Count == 1) // extra tweak for single row
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 5;

                        dgvSongsDetail.Invalidate();
                        dgvSongsDetail.Visible = true;
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

        private void dgvSongsMaster_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick 
            var grid = (DataGridView)sender;

            // for debugging
            //var erow = e.RowIndex;
            //var ecol = grid.Columns[e.ColumnIndex].Name;
            //Globals.Log("erow = " + erow + "  ecol = " + ecol);

            if (e.Button == MouseButtons.Right)
                if (e.RowIndex != -1)
                {
                    grid.Rows[e.RowIndex].Selected = true;
                    var sng = GetSongByRow(e.RowIndex);
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

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // beyound current scope of CFM
                if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                else // required to force selected row change
                {
                    TemporaryDisableDatabindEvent(() =>
                        {
                            dgvSongsMaster.EndEdit();
                        });
                }
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvSongsMaster.Refresh();
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

        //TODO: need to find a better way to do this. DataBindingComplete get's called ALOT!!!!
        private void dgvSongsMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (e.ListChangedType != ListChangedType.Reset)
                return;

            // workaround to catch DataBindingComplete called by other UC's
            //why would another UC call this event??? it's not assigned to any other UC events.
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongsMaster")
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
            }

            // filter removed
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSongsMaster.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvSongsMaster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
            {
                DataGridViewAutoFilterColumnHeaderCell filterCell =
                    this.dgvSongsMaster.CurrentCell.OwningColumn.HeaderCell as
                    DataGridViewAutoFilterColumnHeaderCell;

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
                    DataGridViewRow row = dgvSongsMaster.Rows[i];

                    if (row.Selected)
                    {
                        // beyound current scope of CFM
                        if (row.Cells["colSelect"].Value.ToString().Contains(Constants.RS1COMP))
                            Globals.Log(Properties.Resources.CanNotSelectIndividualRS1CompatiblityDLC);
                        else
                        {
                            if (row.Cells["colSelect"].Value == null)
                                row.Cells["colSelect"].Value = false;

                            if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                                row.Cells["colSelect"].Value = false;
                            else
                                row.Cells["colSelect"].Value = true;
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
                        GetSongByRow(i).Selected = allSelected;
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

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            RemoveFilter();
        }

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

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
            {
                for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
                {
                    GetSongByRow(i).Selected = allSelected;
                }
            });
            allSelected = !allSelected;

        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void RemoveFilter()
        {
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongsMaster);
            ResetDetail();
            LoadFilteredBindingList(masterSongCollection);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSongsMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        #region IDataGridViewHolder
        public DataGridView GetGrid()
        {
            return dgvSongsMaster;
        }
        #endregion

        private void cbMyCDLC_CheckedChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(AppSettings.Instance.CreatorName))
            {
                if (cbMyCDLC.Checked)
                    LoadFilteredBindingList(masterSongCollection.Where(x => x.IsMine).ToList());
                else
                    PopulateDataGridView();
            }
        }

        private void dgvSongsMaster_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            var x = (SongData)dgvSongsMaster.Rows[e.RowIndex].DataBoundItem;

            if (x != null)
            {
                if (x.OfficialDLC)
                    e.CellStyle.Font = OfficialDLCFont;


                if (e.ColumnIndex == colBass.Index ||
                    e.ColumnIndex == colVocals.Index ||
                    e.ColumnIndex == colLead.Index ||
                    e.ColumnIndex == colRhythm.Index)
                {
                    string arrInit = x.Arrangements.ToUpper();

                    if (e.ColumnIndex == colBass.Index)
                        e.CellStyle.BackColor = arrInit.Contains("BASS") ? _Enabled : _Disabled;
                    else
                        if (e.ColumnIndex == colVocals.Index)
                            e.CellStyle.BackColor = arrInit.Contains("VOCAL") ? _Enabled : _Disabled;
                        else
                            if (e.ColumnIndex == colLead.Index)
                            {
                                if (arrInit.Contains("COMBO"))
                                    e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                                else
                                    e.CellStyle.BackColor = arrInit.Contains("LEAD") ? _Enabled : _Disabled;
                            }
                            else
                                if (e.ColumnIndex == colRhythm.Index)
                                {
                                    if (arrInit.Contains("COMBO"))
                                        e.CellStyle.BackColor = arrInit.Contains("COMBO") ? _Enabled : _Disabled;
                                    else
                                        e.CellStyle.BackColor = arrInit.Contains("RHYTHM") ? _Enabled : _Disabled;
                                }
                }
            }
        }

        private void dgvSongsMaster_Sorted(object sender, EventArgs e)
        {
            if (dgvSongsMaster.SortedColumn != null)
            {
                AppSettings.Instance.SortColumn = dgvSongsMaster.SortedColumn.DataPropertyName;
                AppSettings.Instance.SortAscending = dgvSongsMaster.SortOrder == SortOrder.Ascending ? true : false;
            }
        }


        public void PlaySelectedSong()
        {
            var sng = GetFirstSelected();
            if (sng != null)
            {
                if (String.IsNullOrEmpty(sng.AudioCache))
                {
                    sng.AudioCache = string.Format("{0}_{1}",
                        Guid.NewGuid().ToString().Replace("-", ""), sng.FileSize);
                }

                var Dir = Path.Combine(Constants.WorkDirectory, "AudioCache");
                if (!Directory.Exists(Dir))
                    Directory.CreateDirectory(Dir);

                var fullname = Path.Combine(Dir, sng.AudioCache + ".ogg");

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
                    if (!PsarcBrowser.ExtractAudio(sng.Path, fullname, ""))
                    {
                        Globals.Log(Properties.Resources.CouldNotExtractTheAudio);
                        sng.AudioCache = "";
                        return;
                    }
                }
                if (!File.Exists(fullname))
                {
                    sng.AudioCache = "";
                    return;
                }

                if (Globals.AudioEngine.OpenAudioFile(fullname))
                {
                    Globals.AudioEngine.Play();
                    Globals.Log(String.Format("Playing {0} by {1}. ({2})", sng.Title, sng.Artist, Path.GetFileName(sng.Path)));
                }
                else
                    Globals.Log("Unable to open audio file.");
            }
        }

        private void TaggerProgress(object sender, TaggerProgress e)
        {
            Globals.TsProgressBar_Main.Value = e.Progress;
            Application.DoEvents();
        }

        private void unTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selection = Globals.SongManager.GetSelectedSongs(); //.Where(sd => sd.Tagged);

            if (selection.Count > 0)
            {
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
        }

        private void changePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selection = Globals.SongManager.GetSelectedSongs(); //.Where(sd => sd.Tagged);

            if (selection.Count > 0)
            {
                frmSongBatchEdit.BatchEdit(selection.ToArray());
            }
        }

        public void TabEnter()
        {
            Globals.Log("SongManager GUI Activated...");
            //  PopulateSongManager();
        }

        public void TabLeave()
        {
            LeaveSongManager();
        }

        private void dgvSongsMaster_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (ModifierKeys == Keys.Control)
                {
                    var s = GetSongByRow(e.RowIndex);
                    s.Selected = !s.Selected;
                }
                else
                    if (ModifierKeys == Keys.Shift)
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
                                end = start;
                            }
                            TemporaryDisableDatabindEvent(() =>
                            {
                                for (int i = start; i < end; i++)
                                {
                                    var s = GetSongByRow(i);
                                    s.Selected = !s.Selected;
                                }
                            });
                        }
                    }
            }
        }

        private void lnklblToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TemporaryDisableDatabindEvent(() =>
            {
                for (int i = 0; i < dgvSongsMaster.Rows.Count; i++)
                {
                    GetSongByRow(i).Selected = !GetSongByRow(i).Selected;
                }

            });
        }

    }
}