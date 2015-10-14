using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using CustomsForgeManager.SongEditor;
using DataGridViewTools;
using Newtonsoft.Json;
using Extensions = CustomsForgeManager.CustomsForgeManagerLib.Extensions;


namespace CustomsForgeManager.UControls
{
    public partial class SongManager : UserControl
    {
        private bool allSelected = false;
        private AbortableBackgroundWorker bWorker;
        private bool bindingCompleted = false;
        private Stopwatch counterStopwatch = new Stopwatch();
        private bool dgvPainted = false;
        private List<string> masterFileCollection = new List<string>();
        private BindingList<SongData> masterSongCollection = new BindingList<SongData>();
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;

        public SongManager()
        {
            InitializeComponent();
            dgvSongsDetail.Visible = false;
            Leave += SongManager_Leave;
            PopulateSongManager();
        }

        public void LeaveSongManager()
        {
            Globals.Log("Leaving SongManager GUI ...");
            Globals.DgvSongs = dgvSongsMaster;
        }

        public void LoadSongCollectionFromFile()
        {
            masterSongCollection.Clear();
            masterFileCollection.Clear();
            var songsInfoPath = Constants.SongsInfoPath;
            var songFilesPath = Constants.SongFilesPath;

            // load songs into memory
            if (!File.Exists(songsInfoPath) || !File.Exists(songFilesPath))
            {
                Rescan();
                Globals.ReloadDuplicates = false;
                Globals.ReloadRenamer = false;
                Globals.ReloadSetlistManager = false;
            }
            try
            {
                using (var fsSongCollection = new FileStream(songsInfoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    masterSongCollection = fsSongCollection.DeserializeXml(new BindingList<SongData>());
                    fsSongCollection.Flush(); // seems redundant?
                }

                using (var fsFileCollection = new FileStream(songFilesPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    masterFileCollection = fsFileCollection.DeserializeXml(new List<string>());
                    fsFileCollection.Flush(); // seems redundant?
                }

                if (masterFileCollection == null || masterFileCollection.Count == 0)
                    throw new Exception();

                if (masterSongCollection == null || masterSongCollection.Count == 0)
                    throw new Exception();

                Globals.FileCollection = masterFileCollection;
                Globals.SongCollection = masterSongCollection;
                Globals.Log("Loaded song collection file ...");
                PopulateDataGridView();
                SmartRescan();
            }
            catch (Exception e)
            {
                // failsafe ... delete CFM folder and start over
                Globals.Log("Error: " + e.Message);
                Globals.Log("Deleted CFM folder from My Documents ...");

                if (Directory.Exists(Constants.WorkDirectory))
                    if (Directory.Exists(Constants.WorkDirectory))
                    {
                        File.Delete(Constants.LogFilePath);
                        File.Delete(Constants.SettingsPath);
                        File.Delete(Constants.SongFilesPath);
                        File.Delete(Constants.SongsInfoPath);
                    }

                Environment.Exit(0);
            }
        }

        public void PopulateSongManager()
        {
            Globals.Log("Populating SongManager GUI ...");

            // Hide main dgvSongsMaster until load completes
            dgvSongsMaster.Visible = false;

            if (Globals.MySettings.RescanOnStartup)
                Rescan();

            LoadSongCollectionFromFile();
        }

        public void SaveSongCollectionToFile()
        {
            var songsInfoPath = Constants.SongsInfoPath;
            var songFilesPath = Constants.SongFilesPath;

            using (var fsSc = new FileStream(songsInfoPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                masterSongCollection.SerializeXml(fsSc);
                fsSc.Flush();
            }

            using (var fsFc = new FileStream(songFilesPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                masterFileCollection.SerializeXml(fsFc);
                fsFc.Flush();
            }

            Globals.Log("Saved song collection file ...");
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
            Globals.TsLabel_StatusMsg.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_StatusMsg.Text = "Show &All";
            Globals.TsLabel_StatusMsg.IsLink = true;
            Globals.TsLabel_StatusMsg.LinkBehavior = LinkBehavior.HoverUnderline;
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
        }

        private void ArrangementColumnsColors()
        {
            // Color.Green does not show on Dev's desktop over a dgvSongsMaster blue background
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            style1.BackColor = Color.Lime;
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            style2.BackColor = Color.Lime;
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            style3.BackColor = Color.Red;

            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
            {
                (row.Cells["colBass"]).Style = style3;
                (row.Cells["colLead"]).Style = style3;
                (row.Cells["colRhythm"]).Style = style3;
                (row.Cells["colVocals"]).Style = style3;

                if (row.Cells["colArrangements"].Value == null)
                    continue;

                // combo's are combinations of lead and rhythm
                if (row.Cells["colArrangements"].Value.ToString().ToUpper().Contains("COMBO"))
                {
                    (row.Cells["colLead"]).Style = style2;
                    (row.Cells["colRhythm"]).Style = style2;
                }

                if (row.Cells["colArrangements"].Value.ToString().ToUpper().Contains("VOCAL"))
                    (row.Cells["colVocals"]).Style = style1;

                if (row.Cells["colArrangements"].Value.ToString().ToUpper().Contains("BASS"))
                    (row.Cells["colBass"]).Style = style1;

                if (row.Cells["colArrangements"].Value.ToString().ToUpper().Contains("LEAD"))
                    (row.Cells["colLead"]).Style = style1;

                if (row.Cells["colArrangements"].Value.ToString().ToUpper().Contains("RHYTHM"))
                    (row.Cells["colRhythm"]).Style = style1;
            }
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
                        string myParametersTemplate = "filters[artist]={artist}&filters[album]={album}&filters[title]={title}&per_page=1";
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
                        var columnSetting = Globals.MySettings.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
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

        private void DgvSongsAppearance()
        {
            // set all columns to read only except colSelect
            foreach (DataGridViewColumn col in dgvSongsMaster.Columns)
                col.ReadOnly = true;

            dgvSongsMaster.Visible = true; // needs to come now so settings apply correctly

            // see SongManager.Designer for custom appearance settings
            dgvSongsMaster.AllowUserToAddRows = false; // removes empty row at bottom
            dgvSongsMaster.AllowUserToDeleteRows = false;
            dgvSongsMaster.AllowUserToOrderColumns = true;
            dgvSongsMaster.AllowUserToResizeColumns = true;
            dgvSongsMaster.AllowUserToResizeRows = false;
            dgvSongsMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvSongsMaster.BackgroundColor = SystemColors.AppWorkspace;
            dgvSongsMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            dgvSongsMaster.Columns["colSelect"].Width = 43;
            dgvSongsMaster.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSongsMaster.Columns["colEnabled"].Width = 47;
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

        private SongData GetSongByRow(DataGridViewRow dataGridViewRow)
        {
            return masterSongCollection.Distinct().FirstOrDefault(x => x.Title == dataGridViewRow.Cells["Song"].Value.ToString() && x.Artist == dataGridViewRow.Cells["Artist"].Value.ToString() && x.Album == dataGridViewRow.Cells["Album"].Value.ToString() && x.Path == dataGridViewRow.Cells["Path"].Value.ToString());
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

        private void PopulateDataGridView() // binding data to grid
        {
            LoadFilteredBindingList(masterSongCollection);

            // update datagrid appearance
            DgvSongsAppearance();

            // reload column order, width, visibility
            if (Globals.MySettings.ManagerGridSettings != null)
                dgvSongsMaster.ReLoadColumnOrder(Globals.MySettings.ManagerGridSettings.ColumnOrder);

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

            if (Globals.MySettings == null || Globals.MySettings.ManagerGridSettings == null)
                return;

            contextMenuStrip.Items.Clear();
            RADataGridViewSettings gridSettings = Globals.MySettings.ManagerGridSettings;
            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
            {
                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name, null, ColumnMenuItemClick);
                columnsMenuItem.Checked = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Visible;
                columnsMenuItem.Tag = dgvSongsMaster.Columns[columnOrderItem.ColumnIndex].Name;
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void Rescan()
        {
            // save settings (column widths) in case user has modified
            Globals.Settings.SaveSettingsToFile();
            ResetDetail();

            // this should never happen
            if (String.IsNullOrEmpty(Globals.MySettings.RSInstalledDir))
            {
                MessageBox.Show("Error: Rocksmith 2014 installation directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // this is done here in case user decided to manually delete all songs
            // the default initial load condition does not include RS1 Compatiblity files
            var dlcFiles = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*.psarc", SearchOption.AllDirectories)
                .Where(fi => !fi.ToLower().Contains(Constants.RS1COMP)).ToArray();

            if (!dlcFiles.Any())
            {
                if (Directory.Exists(Constants.WorkDirectory))
                    if (Directory.Exists(Constants.WorkDirectory))
                    {
                        File.Delete(Constants.LogFilePath);
                        File.Delete(Constants.SettingsPath);
                        File.Delete(Constants.SongFilesPath);
                        File.Delete(Constants.SongsInfoPath);
                    }

                var msgText = "Houston ... we have a problem!" + Environment.NewLine +
                    "There are no Rocksmith 2014 songs in:" + Environment.NewLine +
                    Path.Combine(Globals.MySettings.RSInstalledDir, "dlc") + Environment.NewLine + Environment.NewLine +
                    "Please select a valid Rocksmith 2014" + Environment.NewLine +
                    "installation directory when you restart CFM.  ";
                MessageBox.Show(msgText, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                // prevents write log attempt
                Environment.Exit(0);
            }

            ToggleUIControls();

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(dgvSongsMaster, bWorker);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                {
                    Application.DoEvents();
                    // updates display while working
                    // dgvSongsMaster.DataSource = worker.bwSongCollection;
                    //dgvSongsMaster.DataSource = worker.bwSongCollection.Select(x => new
                    //{
                    //    colEnabled = x.Enabled,
                    //    colPath = x.Path
                    //}).ToList();
                }

                // updates display after rescan
                // dgvSongsMaster.DataSource = worker.bwSongCollection;
            }

            ToggleUIControls();

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
                return;

            masterSongCollection = Globals.SongCollection;
            masterFileCollection = Globals.FileCollection;
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
                    dgvSongsMaster.Rows[ndx].Cells["colShowDetail"].Value = new Bitmap(Properties.Resources.plus);
                    dgvSongsMaster.Rows[ndx].Cells["colShowDetail"].Tag = null;
                    dgvSongsDetail.Visible = false;
                    break;
                }
        }

        private void SearchCDLC(string criteria)
        {
            var results = masterSongCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(criteria.ToLower()) ||
                x.Tuning.ToLower().Contains(criteria.ToLower()) ||
                x.Charter.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) ||
                (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower())) ||
                x.SongYear.ToString().Contains(criteria) ||
                x.Path.ToLower().Contains(criteria.ToLower()))).ToList();

            LoadFilteredBindingList(results);
            //  Extensions.InvokeIfRequired(dgvSongsMaster, delegate { dgvSongsMaster.DataSource = results; });
        }

        private void ShowSongInfo()
        {
            if (dgvSongsMaster.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSongsMaster.SelectedRows[0];
                var title = selectedRow.Cells["colTitle"].Value.ToString();
                var artist = selectedRow.Cells["colArtist"].Value.ToString();
                var album = selectedRow.Cells["colAlbum"].Value.ToString();
                var path = selectedRow.Cells["colPath"].Value.ToString();

                var song = masterSongCollection.FirstOrDefault(x => x.Title == title && x.Album == album && x.Artist == artist && x.Path == path);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show("Please select (highlight) the song that  " + Environment.NewLine + "you would like information about.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SmartRescan()
        {
            // smart rescan technology
            if (!Globals.MySettings.RescanOnStartup)
            {
                var rs1CompFiles = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), Constants.RS1COMP + "*", SearchOption.AllDirectories).ToArray();
                var dlcFiles = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*.psarc", SearchOption.AllDirectories).ToArray();
                int rs1CompSongs;

                if (rs1CompFiles.Count() == 2)
                    rs1CompSongs = 193;
                else if (!rs1CompFiles.Any())
                    rs1CompSongs = 0;
                else // there is only one RS1 Compatibility file
                    if (rs1CompFiles[0].ToLower().Contains("disc"))
                        rs1CompSongs = 52;
                    else
                        rs1CompSongs = 193 - 52;

                if (masterFileCollection.Count != (Globals.MySettings.IncludeRS1DLCs ? dlcFiles.Length + rs1CompSongs - rs1CompFiles.Count() : dlcFiles.Length - rs1CompFiles.Count()))
                    Rescan();
            }
        }

        private void SongListToBBCode()
        {
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("Artist, Song, Album, Year, Tuning, DD, Arangements, Charter");
            sbTXT.AppendLine("[LIST=1]");

            var checkedRows = dgvSongsMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            if (checkedRows.Count == 0)
            {
                foreach (var song in masterSongCollection)
                    if (song.Charter == null)
                        sbTXT.AppendLine("[*]" + song.Artist + ", " + song.Title + ", " + song.Album + ", " + song.SongYear + ", " + song.Tuning + ", " + song.DD + ", " + song.Arrangements + "[/*]");
                    else
                        sbTXT.AppendLine("[*]" + song.Artist + ", " + song.Title + ", " + song.Album + ", " + song.SongYear + ", " + song.Tuning + ", " + song.DD + ", " + song.Arrangements + ", " + song.Charter + "[/*]");
            }
            else
            {
                foreach (var row in checkedRows)
                    if (row.Cells["colCharter"].Value == null)
                        sbTXT.AppendLine("[*]" + row.Cells["colArtist"].Value + " - " + row.Cells["colTitle"].Value + ", " + row.Cells["colAlbum"].Value + ", " + row.Cells["colSongYear"].Value + ", " + row.Cells["colTuning"].Value + ", " + row.Cells["colDD"].Value + ", " + row.Cells["colArrangements"].Value + "[/*]");
                    else
                        sbTXT.AppendLine("[*]" + row.Cells["colArtist"].Value + " - " + row.Cells["colTitle"].Value + ", " + row.Cells["colAlbum"].Value + ", " + row.Cells["colSongYear"].Value + ", " + row.Cells["colTuning"].Value + ", " + row.Cells["colDD"].Value + ", " + row.Cells["colArrangements"].Value + ", " + row.Cells["colCharter"].Value);
            }

            sbTXT.AppendLine("[/LIST]");

            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song list to BBCode");
                noteViewer.PopulateText(sbTXT.ToString());
                noteViewer.ShowDialog();
            }
        }

        private void SongListToCSV()
        {
            var sbCSV = new StringBuilder();
            string path = Path.Combine(Constants.WorkDirectory, "SongListCSV.csv");
            var checkedRows = dgvSongsMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
            sfdSongListToCSV.FileName = Path.Combine(Constants.WorkDirectory, "SongListCSV");

            if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
                path = sfdSongListToCSV.FileName;

            string csvSep = ";";
            sbCSV.AppendLine(String.Format(@"sep={0}", csvSep)); // used by Excel to recognize seperator if Encoding.Unicode is used
            sbCSV.AppendLine(String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", csvSep, "Artist", "Song", "Album", "Year", "Tuning", "DD", "Arrangements", "Author"));

            if (checkedRows.Count == 0)
            {
                foreach (var song in masterSongCollection)
                    if (song.Charter == null)
                        sbCSV.AppendLine(song.Title + csvSep + song.Artist + csvSep + song.Album + csvSep + song.SongYear + csvSep + song.Tuning + csvSep + song.DD + csvSep + song.Arrangements);
                    else
                        sbCSV.AppendLine(song.Title + csvSep + song.Artist + csvSep + song.Album + csvSep + song.SongYear + csvSep + song.Tuning + csvSep + song.DD + csvSep + song.Arrangements + csvSep + song.Charter);
            }
            else
            {
                foreach (var row in checkedRows)
                    if (row.Cells["colCharter"].Value == null)
                        sbCSV.AppendLine(row.Cells["colTitle"].Value + csvSep + row.Cells["colArtist"].Value + csvSep + row.Cells["colAlbum"].Value + csvSep + row.Cells["colSongYear"].Value + csvSep + row.Cells["colTuning"].Value + csvSep + row.Cells["colDD"].Value + csvSep + row.Cells["colArrangements"].Value);
                    else
                        sbCSV.AppendLine(row.Cells["colTitle"].Value + csvSep + row.Cells["colArtist"].Value + csvSep + row.Cells["colAlbum"].Value + csvSep + row.Cells["colSongYear"].Value + csvSep + row.Cells["colTuning"].Value + csvSep + row.Cells["colDD"].Value + csvSep + row.Cells["colArrangements"].Value + csvSep + row.Cells["colCharter"].Value);
            }

            try
            {
                using (StreamWriter file = new StreamWriter(path, false, Encoding.Unicode)) // Excel does not recognize UTF8
                    file.Write(sbCSV.ToString());

                Globals.Log("Song list saved to:" + path);
            }
            catch (IOException ex)
            {
                Globals.Log("<Error>:" + ex.Message);
            }
        }

        private void SongListToHTML()
        {
            var checkedRows = dgvSongsMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("<table>");
            sbTXT.AppendLine("<tr>");
            sbTXT.AppendLine("<th>Artist</th><th>Song</th><th>Album</th><th>Year</th><th>Tuning</th><th>DD</th><th>Arangements</th><th>Author</th>");
            sbTXT.AppendLine("</tr>");

            if (checkedRows.Count == 0)
            {
                foreach (var song in masterSongCollection)
                {
                    sbTXT.AppendLine("<tr>");
                    if (song.Charter == null)
                        sbTXT.AppendLine("<td>" + song.Artist + "</td><td>" + song.Title + "</td><td>" + song.Album + "</td><td>" + song.SongYear + "</td><td>" + song.Tuning + "</td><td>" + song.DD + "</td><td>" + song.Arrangements + "</td>");
                    else
                        sbTXT.AppendLine("<td>" + song.Artist + "</td><td>" + song.Title + "</td><td>" + song.Album + "</td><td>" + song.SongYear + "</td><td>" + song.Tuning + "</td><td>" + song.DD + "</td><td>" + song.Arrangements + "</td><td>" + song.Charter + "</td>");

                    sbTXT.AppendLine("</tr>");
                }
            }
            else
            {
                foreach (var row in checkedRows)
                {
                    sbTXT.AppendLine("<tr>");
                    if (row.Cells["colCharter"].Value == null)
                        sbTXT.AppendLine("<td>" + row.Cells["colTitle"].Value + "</td><td>" + row.Cells["colArtist"].Value + "</td><td>" + row.Cells["colAlbum"].Value + "</td><td>" + row.Cells["colSongYear"].Value + "</td><td>" + row.Cells["colTuning"].Value + "</td><td>" + row.Cells["colDD"].Value + "</td><td>" + row.Cells["colArrangements"].Value + "</td>");
                    else
                        sbTXT.AppendLine("<td>" + row.Cells["colTitle"].Value + "</td><td>" + row.Cells["colArtist"].Value + "</td><td>" + row.Cells["colAlbum"].Value + "</td><td>" + row.Cells["colSongYear"].Value + "</td><td>" + row.Cells["colTuning"].Value + "</td><td>" + row.Cells["colDD"].Value + "</td><td>" + row.Cells["colArrangements"].Value + "</td><td>" + row.Cells["colCharter"].Value + "</td>");

                    sbTXT.AppendLine("</tr>");
                }
            }

            sbTXT.AppendLine("</table>");

            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Song list to HTML");
                noteViewer.PopulateText(sbTXT.ToString());
                noteViewer.ShowDialog();
            }
        }

        private void SongListToJsonOrXml()
        {
            // TODO:
        }

        private void ToggleUIControls()
        {
            Extensions.InvokeIfRequired(btnRescan, delegate { btnRescan.Enabled = !btnRescan.Enabled; });
            Extensions.InvokeIfRequired(btnCheckAllForUpdates, delegate { btnCheckAllForUpdates.Enabled = !btnCheckAllForUpdates.Enabled; });
            Extensions.InvokeIfRequired(cueSearch, delegate { cueSearch.Enabled = !cueSearch.Enabled; });
            Extensions.InvokeIfRequired(btnDisableEnableSongs, delegate { btnDisableEnableSongs.Enabled = !btnDisableEnableSongs.Enabled; });
            Extensions.InvokeIfRequired(btnExportSongList, delegate { btnExportSongList.Enabled = !btnExportSongList.Enabled; });
            Extensions.InvokeIfRequired(btnBackupSelectedDLCs, delegate { btnBackupSelectedDLCs.Enabled = !btnBackupSelectedDLCs.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToBBCode, delegate { radioBtn_ExportToBBCode.Enabled = !radioBtn_ExportToBBCode.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToCSV, delegate { radioBtn_ExportToCSV.Enabled = !radioBtn_ExportToCSV.Enabled; });
            Extensions.InvokeIfRequired(radioBtn_ExportToHTML, delegate { radioBtn_ExportToHTML.Enabled = !radioBtn_ExportToHTML.Enabled; });
            Extensions.InvokeIfRequired(lnkLblSelectAll, delegate { lnkLblSelectAll.Enabled = !lnkLblSelectAll.Enabled; });
            Extensions.InvokeIfRequired(lnkClearSearch, delegate { lnkClearSearch.Enabled = !lnkClearSearch.Enabled; });
            Extensions.InvokeIfRequired(lbl_ExportTo, delegate { lbl_ExportTo.Enabled = !lbl_ExportTo.Enabled; });
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
                MessageBox.Show("Please connect to the internet  " + Environment.NewLine + "to use this feature.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Globals.Log("Need to be connected to the internet to use this feature");
            }
        }

        private void SongManager_Leave(object sender, EventArgs e)
        {
            // TODO: this EH does not get fired after initial load and leave
            // TODO: why???
        }

        private void btnBackupSelectedDLCs_Click(object sender, EventArgs e)
        {
            string backupDir = Path.Combine(Globals.MySettings.RSInstalledDir, "backups");
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

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    string songPath = row.Cells["colPath"].Value.ToString();

                    // redundant for file safety
                    if (chkEnableDelete.Checked && !safe2Delete)
                    {
                        // DANGER ZONE
                        if (MessageBox.Show("You are about to permanently delete all 'Selected' songs(s).  " + Environment.NewLine + Environment.NewLine +
                                            "Are you sure you want to permanently delete the(se) songs(s)", Constants.ApplicationName + " ... Warning ... Warning",
                                            MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        safe2Delete = true;
                    }

                    // redundant for file safety
                    if (safe2Delete)
                    {
                        try
                        {
                            File.Delete(songPath);
                            dgvSongsMaster.Rows.Remove(row);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Unable to delete song :" + songPath + Environment.NewLine + "Error: " + ex.Message, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show(@"Unable to enable/disable song: " + Path.GetFileName(originalPath) + @" in 'dlc' folder." + Environment.NewLine + "Error: " + ex.Message);
                        }

                        // row.Cells["colSelect"].Value = false;
                        numberOfDisabledDLC = masterSongCollection.Where(song => song.Enabled == "No").ToList().Count();
                        var tsldcMsg = String.Format("Outdated: {0} | Disabled DLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
                        Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_DisabledCounter.Text = tsldcMsg; });
                    }
                    else
                        Globals.Log("This is a Rocksmith 1 Compatiblity Song." + Environment.NewLine +
                                    "RS1 Compatiblity Songs can not be disabled individually." + Environment.NewLine +
                                    "Use SetlistManager to disable all RS1 Compatiblity Songs.");
                }
            }

            dgvSongsMaster.Refresh();

            // rescan on tabpage change
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanRenamer = true;
        }

        private void btnExportSongList_Click(object sender, EventArgs e)
        {
            // TODO: make these smart export functions so that only user selected columns and data are exported
            if (radioBtn_ExportToBBCode.Checked)
                SongListToBBCode();
            else if (radioBtn_ExportToHTML.Checked)
                SongListToHTML();
            else if (radioBtn_ExportToCSV.Checked)
                SongListToCSV();
            else
                MessageBox.Show("Export format not selected" + Environment.NewLine + "Please select export format!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan();
            ArrangementColumnsColors();
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
            string backupPath = Path.Combine(Globals.MySettings.RSInstalledDir, "backup");

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
                Globals.Log("Sucessfully saved to: " + Path.Combine(backupPath, fileName));
            }
            catch (IOException ex)
            {
                MessageBox.Show("Please select (highlight) the song  " + Environment.NewLine + "that you would like to backup.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                Globals.Log("Can not delete individual RS1 Compatiblity DLC");
                Globals.Log("Go to Setting tab and uncheck 'Include RS1 Compatibility Pack'");
                Globals.Log("Then click 'Rescan' to remove all RS1 Compatiblity DLCs");
                return;
            }

            try
            {
                if (MessageBox.Show("Do you really want to remove this CDLC?  " + Environment.NewLine + "Warning:  This cannot be undone!", Constants.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    File.Delete(masterSongCollection[dgvSongsMaster.SelectedRows[0].Index].Path);
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
                songEditor.Text = String.Format("{0}{1}", "Song Editor ... Loaded: ", Path.GetFileName(filePath));
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
                        Process.Start(Constants.DefaultDetailsURL + "/" + song.IgnitionID);
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
            // get detail from master
            if (e.ColumnIndex == dgvSongsMaster.Columns["colShowDetail"].Index && e.RowIndex >= 0)
            {
                var songKey = dgvSongsMaster.Rows[e.RowIndex].Cells["colKey"].Value.ToString();
                if (String.IsNullOrEmpty(songKey))
                    return;

                if (dgvSongsDetail.Visible)
                    if (dgvSongsDetail.Rows[0].Cells["colDetailKey"].Value.ToString() != songKey)
                        ResetDetail();

                if (String.IsNullOrEmpty(dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag as String))
                {
                    var songDetails = masterSongCollection.Where(master => (master.SongKey == songKey)).ToList();
                    if (!songDetails.Any())
                        MessageBox.Show("No Song Details Found");
                    else
                    {
                        var rowHeight = dgvSongsMaster.Rows[e.RowIndex].Height + 0; // height tweak
                        var colWidth = dgvSongsMaster.Columns[e.ColumnIndex].Width - 16; // width tweak
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Tag = "TRUE";
                        dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = new Bitmap(Properties.Resources.minus);
                        Rectangle dgvRectangle = dgvSongsMaster.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        dgvSongsDetail.Location = new Point(dgvRectangle.Right + colWidth, dgvRectangle.Bottom + rowHeight - 2);
                        dgvSongsDetail.AutoGenerateColumns = false;
                        dgvSongsMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        // CRITICAL CODE AREA - CAREFUL
                        dgvSongsDetail.DataSource = new FilteredBindingList<SongData>(songDetails);
                        dgvSongsDetail.DataMember = "Arrangements2D";
                        dgvSongsDetail.Columns["colDetailKey"].Width = dgvSongsMaster.Columns["colKey"].Width;

                        // calculate the height and width of dgvSongsDetail
                        var colHeaderHeight = dgvSongsDetail.Columns[e.ColumnIndex].HeaderCell.Size.Height;
                        dgvSongsDetail.Height = dgvSongsDetail.Rows.Cast<DataGridViewRow>().Sum(row => row.Height) + colHeaderHeight - 1;
                        dgvSongsDetail.Width = dgvSongsDetail.Columns.Cast<DataGridViewColumn>().Sum(col => col.Width) + colWidth;
                        if (dgvSongsDetail.Rows.Count == 1) // extra tweak for single row
                            dgvSongsDetail.Height = dgvSongsDetail.Height + 4;

                        dgvSongsDetail.Visible = true;
                    }
                }
                else
                {
                    dgvSongsMaster.Rows[e.RowIndex].Cells["colShowDetail"].Value = new Bitmap(Properties.Resources.plus);
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
                    cmsSongManager.Show(Cursor.Position);
                }
                else
                {
                    PopulateMenuWithColumnHeaders(cmsSongManagerColumns);
                    cmsSongManagerColumns.Show(Cursor.Position);
                }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left)
                if (e.RowIndex != -1)
                    if (grid.Columns[e.ColumnIndex].Name == "colSelect")
                    {
                        // beyound current scope of CFM
                        if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                            Globals.Log("Can not 'Select' individual RS1 Compatiblity DLC");
                        // removed for testing works better without this
                        //else
                        //{
                        //    if (Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colSelect"].Value))
                        //        grid.Rows[e.RowIndex].Cells["colSelect"].Value = false;
                        //    else
                        //        grid.Rows[e.RowIndex].Cells["colSelect"].Value = true;
                        //}
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

            var columnName = dgvSongsMaster.Columns[e.ColumnIndex].Name;

            //if (columnName.Contains("colBass") || columnName.Contains("colLead") ||
            //    columnName.Contains("colRhythm") || columnName.Contains("colVocals"))
            //    return;

            ArrangementColumnsColors();
        }

        private void dgvSongsMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // workaround to catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongsMaster")
                return;

            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;

                var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongsMaster);
                if (!String.IsNullOrEmpty(filterStatus) && dgvPainted)
                {
                    Globals.TsLabel_StatusMsg.Visible = true;
                    Globals.TsLabel_DisabledCounter.Text = filterStatus;
                    // ensures BLRV columns are recolored correctly
                    dgvPainted = false;
                }
                if (String.IsNullOrEmpty(filterStatus) && dgvPainted)
                    RemoveFilter();
            }
        }

        private void dgvSongsMaster_KeyDown(object sender, KeyEventArgs e)
        {
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
                            Globals.Log("Can not 'Select' individual RS1 Compatiblity DLC");
                        else
                        {
                            if (row.Cells["colSelect"].Value == null)
                                row.Cells["colSelect"].Value = false;

                            if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                                row.Cells["colSelect"].Value = false;
                            else
                                row.Cells["colSelect"].Value = true;

                            // as long as the data is bound this should be OK to do
                            masterSongCollection[i].Selected = (bool)row.Cells["colSelect"].Value;
                        }
                    }
                }
            }
        }

        private void dgvSongsMaster_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                {
                    if (allSelected)
                        row.Cells["colSelect"].Value = false;
                    else
                        row.Cells["colSelect"].Value = true;
                }
                allSelected = !allSelected;
            }
        }

        private void dgvSongsMaster_Paint(object sender, PaintEventArgs e)
        {
            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (bindingCompleted && !dgvPainted)
            {
                bindingCompleted = false;
                dgvPainted = true;
                Debug.WriteLine("dgvSongsMaster Painted ... ");
                ArrangementColumnsColors();
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            RemoveFilter();
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongsMaster.Rows)
                row.Cells["colSelect"].Value = !allSelected;
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

            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.BackColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dgvSongsMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            UpdateToolStrip();
        }


    }
}