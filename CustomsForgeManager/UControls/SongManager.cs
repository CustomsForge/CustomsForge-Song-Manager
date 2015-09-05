﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
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
        private int numberOfDLCPendingUpdate = 0;
        private int numberOfDisabledDLC = 0;
        private List<string> smFileCollection = new List<string>();
        private BindingList<SongData> smSongCollection = new BindingList<SongData>();
        private string sortColumnName = String.Empty;
        private bool sortDescending = true;
        private List<SongData> sortedSongCollection = new List<SongData>();

        public SongManager()
        {
            InitializeComponent();
            Leave += SongManager_Leave;
            PopulateSongManager();
        }

        public void LeaveSongManager()
        {
            Globals.Log("Leaving SongManager GUI ...");
            Globals.DgvSongs = dgvSongs;
        }

        public void LoadSongCollectionFromFile()
        {
            var songsInfoPath = Constants.SongsInfoPath;
            var songFilesPath = Constants.SongFilesPath;
            smSongCollection.Clear();
            smFileCollection.Clear();

            if (!File.Exists(songsInfoPath) || !File.Exists(songFilesPath))
            {
                // load songs into memory
                Rescan();
            }

            try
            {
                using (var fsSongCollection = new FileStream(songsInfoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    smSongCollection = fsSongCollection.DeserializeXml(new BindingList<SongData>());
                    fsSongCollection.Flush(); // seems redundant?
                }

                using (var fsFileCollection = new FileStream(songFilesPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    smFileCollection = fsFileCollection.DeserializeXml(new List<string>());
                    fsFileCollection.Flush(); // seems redundant?
                }

                if (smFileCollection == null || smFileCollection.Count == 0)
                    throw new Exception();

                if (smSongCollection == null || smSongCollection.Count == 0)
                    throw new Exception();

                Globals.FileCollection = smFileCollection;
                Globals.SongCollection = smSongCollection;
                Globals.Log("Loaded song collection file ...");
                PopulateDataGridView();
            }
            catch (Exception e)
            {
                Globals.Log("Error: " + e.Message);
            }
        }

        public void PopulateSongManager()
        {
            Globals.Log("Populating SongManager GUI ...");

            // Hide main dgvSongs until load completes
            dgvSongs.Visible = false;

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
                smSongCollection.SerializeXml(fsSc);
                fsSc.Flush();
            }

            using (var fsFc = new FileStream(songFilesPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                smFileCollection.SerializeXml(fsFc);
                fsFc.Flush();
            }

            Globals.Log("Saved song collection file ...");
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSongManager)
            {
                Globals.RescanSongManager = false;

                if (Globals.RescanDuplicates || Globals.RescanSongManager)
                {
                    Globals.RescanDuplicates = false;
                    Globals.RescanSongManager = false;
                    Rescan();
                }

                LoadSongCollectionFromFile();
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", smSongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            numberOfDisabledDLC = smSongCollection.Where(song => song.Enabled == "No").ToList().Count();
            numberOfDLCPendingUpdate = 0;
            var tsldcMsg = String.Format("Outdated: {0} | Disabled DLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = tsldcMsg;
            Globals.TsLabel_DisabledCounter.Visible = true;
        }

        private void ArrangementColumnsColors()
        {
            // Color.Green does not show on Dev's desktop over a dgvSongs blue background
            DataGridViewCellStyle style1 = new DataGridViewCellStyle();
            style1.BackColor = Color.Lime;
            DataGridViewCellStyle style2 = new DataGridViewCellStyle();
            style2.BackColor = Color.Lime;
            DataGridViewCellStyle style3 = new DataGridViewCellStyle();
            style3.BackColor = Color.Red;

            foreach (DataGridViewRow row in dgvSongs.Rows)
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
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    if (dgvSongs.SelectedRows.Count > 0)
                    {
                        CheckRowForUpdate(dgvSongs.SelectedRows[0]);
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
                        string myParameters = myParametersTemplate.Replace("{artist}", currentSong.Artist).Replace("{album}", currentSong.Album).Replace("{title}", currentSong.Song);

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
                    var dataGridViewColumn = dgvSongs.Columns[currentContextMenuItem.Tag.ToString()];
                    if (dataGridViewColumn != null)
                    {
                        var columnIndex = dataGridViewColumn.Index;
                        var columnSetting = Globals.MySettings.ManagerGridSettings.ColumnOrder.SingleOrDefault(x => x.ColumnIndex == columnIndex);
                        if (columnSetting != null)
                        {
                            columnSetting.Visible = !columnSetting.Visible;
                            dgvSongs.Columns[columnIndex].Visible = columnSetting.Visible;
                            currentContextMenuItem.Checked = columnSetting.Visible;
                        }
                    }
                }
            }
        }

        private void DgvSongsAppearance()
        {
            // set all columns to read only except colSelect
            foreach (DataGridViewColumn col in dgvSongs.Columns)
                col.ReadOnly = true;

            // always visible and first
            dgvSongs.Columns["colSelect"].ReadOnly = false; // overridden by EditProgrammatically
            dgvSongs.Columns["colSelect"].Visible = true;
            dgvSongs.Columns["colSelect"].Width = 43;
            dgvSongs.Columns["colSelect"].DisplayIndex = 0;
            dgvSongs.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvSongs.Visible = true;
            // see SongManager.Designer for custom appearance settings
            dgvSongs.AllowUserToAddRows = false; // removes empty row at bottom
            dgvSongs.AllowUserToDeleteRows = false;
            dgvSongs.AllowUserToOrderColumns = true;
            dgvSongs.AllowUserToResizeColumns = true;
            dgvSongs.AllowUserToResizeRows = false;
            dgvSongs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            dgvSongs.BackgroundColor = SystemColors.AppWorkspace;
            dgvSongs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSongs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // set custom selection (highlighting) color
            dgvSongs.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor; // or removes selection highlight
            dgvSongs.DefaultCellStyle.SelectionForeColor = dgvSongs.DefaultCellStyle.ForeColor;
            // this overrides any user ability to make changes 
            dgvSongs.EditMode = DataGridViewEditMode.EditProgrammatically;
            // dgvSongs.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSongs.EnableHeadersVisualStyles = true;
            dgvSongs.Font = new Font("Arial", 8);
            dgvSongs.GridColor = SystemColors.ActiveCaption;
            dgvSongs.MultiSelect = false;
            dgvSongs.Name = "dgvSongs";
            // dgvSongs.ReadOnly = true;
            dgvSongs.RowHeadersVisible = false; // remove row arrow
            dgvSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private SongData GetSongByRow(DataGridViewRow dataGridViewRow)
        {
            return smSongCollection.Distinct().FirstOrDefault(x => x.Song == dataGridViewRow.Cells["Song"].Value.ToString() && x.Artist == dataGridViewRow.Cells["Artist"].Value.ToString() && x.Album == dataGridViewRow.Cells["Album"].Value.ToString() && x.Path == dataGridViewRow.Cells["Path"].Value.ToString());
        }

        private void PopulateDataGridView() // binding data to grid
        {
            bindingCompleted = false;
            dgvPainted = false;

            dgvSongs.DataSource = smSongCollection;
            sortedSongCollection = smSongCollection.ToList();

            // update datagrid appearance
            DgvSongsAppearance();

            // reload column order, width, visibility
            if (Globals.MySettings.ManagerGridSettings != null)
                dgvSongs.ReLoadColumnOrder(Globals.MySettings.ManagerGridSettings.ColumnOrder);

            // start fresh ... clear Selected in dgvSongs and object SongData
            for (int i = 0; i < dgvSongs.Rows.Count; i++)
            {
                DataGridViewRow row = dgvSongs.Rows[i];
                row.Cells["colSelect"].Value = false;
                smSongCollection[i].Selected = false;
            }
        }

        private void DisableEnabled()
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                if (row.Cells["colPath"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
                    row.Cells["colEnabled"].Value = false;
            }
        }

        private void PopulateMenuWithColumnHeaders(ContextMenuStrip contextMenuStrip)
        {
            // save current column status
            Globals.DgvSongs = dgvSongs;
            Globals.Settings.SaveSettingsToFile();

            if (Globals.MySettings == null || Globals.MySettings.ManagerGridSettings == null)
                return;

            contextMenuStrip.Items.Clear();
            RADataGridViewSettings gridSettings = Globals.MySettings.ManagerGridSettings;
            foreach (ColumnOrderItem columnOrderItem in gridSettings.ColumnOrder)
            {
                ToolStripMenuItem columnsMenuItem = new ToolStripMenuItem(dgvSongs.Columns[columnOrderItem.ColumnIndex].Name, null, ColumnMenuItemClick);
                columnsMenuItem.Checked = dgvSongs.Columns[columnOrderItem.ColumnIndex].Visible;
                columnsMenuItem.Tag = dgvSongs.Columns[columnOrderItem.ColumnIndex].Name;
                contextMenuStrip.Items.Add(columnsMenuItem);
            }
        }

        private void Rescan()
        {
            // save settings (column widths) in case user has modified
            Globals.Settings.SaveSettingsToFile();

            // this should never happen
            if (String.IsNullOrEmpty(Globals.MySettings.RSInstalledDir))
            {
                MessageBox.Show("Error: Rocksmith 2014 installation directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // run new worker
            using (Worker worker = new Worker())
            {
                ToggleUIControls();
                dgvSongs.Rows.Clear();
                worker.BackgroundScan(this, bWorker);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                {
                    Application.DoEvents();
                    // updates display while working
                    dgvSongs.DataSource = worker.bwSongCollection;
                    //dgvSongs.DataSource = worker.bwSongCollection.Select(x => new
                    //{
                    //    colEnabled = x.Enabled,
                    //    colPath = x.Path
                    //}).ToList();
                }

                ToggleUIControls();

                if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
                    return;

                smSongCollection = Globals.SongCollection;
                smFileCollection = Globals.FileCollection;
                sortedSongCollection = smSongCollection.ToList();
                SaveSongCollectionToFile();
            }
        }

        private void SearchDLC(string criteria)
        {
            var results = smSongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) || x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower()) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower())) || x.SongYear.Contains(criteria) || x.Path.ToLower().Contains(criteria.ToLower()))).ToList();
            sortedSongCollection = smSongCollection.Where(x => x.Artist.ToLower().Contains(criteria.ToLower()) || x.Album.ToLower().Contains(criteria.ToLower()) || x.Song.ToLower().Contains(criteria.ToLower()) || x.Tuning.ToLower().Contains(criteria.ToLower()) || x.Author.ToLower().Contains(criteria.ToLower()) || (x.IgnitionAuthor != null && x.IgnitionAuthor.ToLower().Contains(criteria.ToLower())) || (x.IgnitionID != null && x.IgnitionID.ToLower().Contains(criteria.ToLower()) || x.SongYear.Contains(criteria) || x.Path.ToLower().Contains(criteria.ToLower()))).ToList();

            Extensions.InvokeIfRequired(dgvSongs, delegate { dgvSongs.DataSource = results; });
        }

        private void ShowSongInfo()
        {
            if (dgvSongs.SelectedRows.Count > 0)
            {
                var selectedRow = dgvSongs.SelectedRows[0];
                var title = selectedRow.Cells["colSongTitle"].Value.ToString();
                var artist = selectedRow.Cells["colSongArtist"].Value.ToString();
                var album = selectedRow.Cells["colSongAlbum"].Value.ToString();
                var path = selectedRow.Cells["colPath"].Value.ToString();

                var song = smSongCollection.FirstOrDefault(x => x.Song == title && x.Album == album && x.Artist == artist && x.Path == path);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show("Please select (highlight) the song that  " + Environment.NewLine + "you would like information about.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SongListToBBCode()
        {
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("Song - Artist, Album, Updated, Tuning, DD, Arangements, Author");
            sbTXT.AppendLine("[LIST=1]");

            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            if (checkedRows.Count == 0)
            {
                foreach (var song in smSongCollection)
                    if (song.Author == null)
                        sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + song.DD.DifficultyToDD() + ", " + song.Arrangements + "[/*]");
                    else
                        sbTXT.AppendLine("[*]" + song.Song + " - " + song.Artist + ", " + song.Album + ", " + song.Updated + ", " + song.Tuning + ", " + song.DD.DifficultyToDD() + ", " + song.Arrangements + ", " + song.Author + "[/*]");
            }
            else
            {
                foreach (var row in checkedRows)
                    if (row.Cells["Author"].Value == null)
                        sbTXT.AppendLine("[*]" + row.Cells["Song"].Value + " - " + row.Cells["Artist"].Value + ", " + row.Cells["Album"].Value + ", " + row.Cells["Updated"].Value + ", " + row.Cells["Tuning"].Value + ", " + row.Cells["DD"].Value == "0" ? "No" : "Yes" + ", " + row.Cells["Arrangements"].Value + "[/*]");
                    else
                        sbTXT.AppendLine("[*]" + row.Cells["Song"].Value + " - " + row.Cells["Artist"].Value + ", " + row.Cells["Album"].Value + ", " + row.Cells["Updated"].Value + ", " + row.Cells["Tuning"].Value + ", " + row.Cells["DD"].Value + ", " + row.Cells["Arrangements"].Value + ", " + row.Cells["Author"].Value);
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
            string path = Constants.WorkDirectory + @"\SongListCSV.csv";
            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();

            sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
            sfdSongListToCSV.FileName = "SongListCSV";

            if (sfdSongListToCSV.ShowDialog() == DialogResult.OK)
                path = sfdSongListToCSV.FileName;

            sbCSV.AppendLine(@"sep=;");
            sbCSV.AppendLine("Artist;Song;Album;Year;Tuning;Arrangements");

            if (checkedRows.Count == 0)
            {
                foreach (var song in smSongCollection)
                    if (song.Author == null)
                        sbCSV.AppendLine(song.Song + ";" + song.Artist + ";" + song.Album + ";" + song.Updated + ";" + song.Tuning + ";" + song.DD.DifficultyToDD() + ";" + song.Arrangements);
                    else
                        sbCSV.AppendLine(song.Song + ";" + song.Artist + ";" + song.Album + ";" + song.Updated + ";" + song.Tuning + ";" + song.DD.DifficultyToDD() + ";" + song.Arrangements + ";" + song.Author);
            }
            else
            {
                foreach (var row in checkedRows)
                    if (row.Cells["Author"].Value == null)
                        sbCSV.AppendLine(row.Cells["Song"].Value + ";" + row.Cells["Artist"].Value + ";" + row.Cells["Album"].Value + ";" + row.Cells["Updated"].Value + ";" + row.Cells["Tuning"].Value + ";" + row.Cells["DD"].Value == "0" ? "No" : "Yes" + ";" + row.Cells["Arrangements"].Value);
                    else
                        sbCSV.AppendLine(row.Cells["Song"].Value + ";" + row.Cells["Artist"].Value + ";" + row.Cells["Album"].Value + ";" + row.Cells["Updated"].Value + ";" + row.Cells["Tuning"].Value + ";" + row.Cells["DD"].Value + ";" + row.Cells["Arrangements"].Value + ";" + row.Cells["Author"].Value);
            }

            try
            {
                using (StreamWriter file = new StreamWriter(path, false, Encoding.UTF8))
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
            var checkedRows = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colSelect"].Value != null).Where(r => Convert.ToBoolean(r.Cells["colSelect"].Value)).ToList();
            var sbTXT = new StringBuilder();
            sbTXT.AppendLine("<table>");
            sbTXT.AppendLine("<tr>");
            sbTXT.AppendLine("<th>Song</th><th>Artist</th><th>Album</th><th>Updated</th><th>Tuning</th><th>DD</th><th>Arangements</th><th>Author</th>");
            sbTXT.AppendLine("</tr>");

            if (checkedRows.Count == 0)
            {
                foreach (var song in smSongCollection)
                {
                    sbTXT.AppendLine("<tr>");
                    if (song.Author == null)
                        sbTXT.AppendLine("<td>" + song.Song + "</td><td>" + song.Artist + "</td><td>" + song.Album + "</td><td>" + song.Updated + "</td><td>" + song.Tuning + "</td><td>" + song.DD.DifficultyToDD() + "</td><td>" + song.Arrangements + "</td>");
                    else
                        sbTXT.AppendLine("<td>" + song.Song + "</td><td>" + song.Artist + "</td><td>" + song.Album + "</td><td>" + song.Updated + "</td><td>" + song.Tuning + "</td><td>" + song.DD.DifficultyToDD() + "</td><td>" + song.Arrangements + "</td><td>" + song.Author + "</td>");

                    sbTXT.AppendLine("</tr>");
                }
            }
            else
            {
                foreach (var row in checkedRows)
                {
                    sbTXT.AppendLine("<tr>");
                    if (row.Cells["Author"].Value == null)
                        sbTXT.AppendLine("<td>" + row.Cells["Song"].Value + "</td><td>" + row.Cells["Artist"].Value + "</td><td>" + row.Cells["Album"].Value + "</td><td>" + row.Cells["Updated"].Value + "</td><td>" + row.Cells["Tuning"].Value + "</td><td>" + (row.Cells["DD"].Value == "0" ? "No" : "Yes") + "</td><td>" + row.Cells["Arrangements"].Value + "</td>");
                    else
                        sbTXT.AppendLine("<td>" + row.Cells["Song"].Value + "</td><td>" + row.Cells["Artist"].Value + "</td><td>" + row.Cells["Album"].Value + "</td><td>" + row.Cells["Updated"].Value + "</td><td>" + row.Cells["Tuning"].Value + "</td><td>" + (row.Cells["DD"].Value == "0" ? "No" : "Yes") + "</td><td>" + row.Cells["Arrangements"].Value + "</td><td>" + row.Cells["Author"].Value + "</td>");

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
            Extensions.InvokeIfRequired(tbSearch, delegate { tbSearch.Enabled = !tbSearch.Enabled; });
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

        private void UpdateAuthor(DataGridViewRow selectedRow)
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

                foreach (DataGridViewRow row in dgvSongs.Rows)
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

        private void btnDisableEnableSongs_Click(object sender, EventArgs e)
        {
            bool updateSongCollection = false;

            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                var cell = (DataGridViewCheckBoxCell)row.Cells["colSelect"];

                if (cell.Value == null)
                    cell.Value = false;

                if (Convert.ToBoolean(cell.Value))
                {
                    var originalPath = row.Cells["colPath"].Value.ToString();
                    if (!originalPath.ToLower().Contains(String.Format("{0}{1}", Constants.RS1COMP, "disc")))
                    {
                        // confirmed CDLC is disabled in game when using this file naming method
                        if (row.Cells["colEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledDLCPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledDLCPath);
                            row.Cells["colPath"].Value = disabledDLCPath;
                            row.Cells["colEnabled"].Value = "No";
                            updateSongCollection = true;
                        }
                        else
                        {
                            var enabledDLCPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledDLCPath);
                            row.Cells["colPath"].Value = enabledDLCPath;
                            row.Cells["colEnabled"].Value = "Yes";
                            updateSongCollection = true;
                        }

                        cell.Value = "false";
                        numberOfDisabledDLC = smSongCollection.Where(song => song.Enabled == "No").ToList().Count();
                        var tsldcMsg = String.Format("Outdated: {0} | Disabled DLC: {1}", numberOfDLCPendingUpdate, numberOfDisabledDLC);
                        Extensions.InvokeIfRequired(this, delegate { Globals.TsLabel_DisabledCounter.Text = tsldcMsg; });
                    }
                    else
                        Globals.Log("This is a Rocksmith 1 song. It can't be disabled at this moment. (You just can disable all of them!)");
                }
            }

            if (updateSongCollection)
            {
                SaveSongCollectionToFile();
                Globals.RescanSongManager = true;
                Globals.RescanDuplicates = true;
            }
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
                MessageBox.Show("Export format not selected" + Environment.NewLine + "Please select export format!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;
            Rescan();
            ArrangementColumnsColors();
            UpdateToolStrip();
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

        private void chkTheMover_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvSongs.DataSource == null)
                return;

            for (int i = 0; i < dgvSongs.Rows.Count; i++)
            {
                DataGridViewRow row = dgvSongs.Rows[i];
                var artist = (string)row.Cells["colSongArtist"].Value;

                // 'The' mover
                if (chkTheMover.Checked)
                {
                    if (artist.StartsWith("The ", StringComparison.CurrentCultureIgnoreCase))
                        row.Cells["colSongArtist"].Value = String.Format("{0}, The", artist.Substring(4, artist.Length - 4)).Trim();
                }
                else
                {
                    if (artist.EndsWith(", The", StringComparison.CurrentCultureIgnoreCase))
                        row.Cells["colSongArtist"].Value = String.Format("The {0}", artist.Substring(0, artist.Length - 5)).Trim();
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

                var filePath = smSongCollection[dgvSongs.SelectedRows[0].Index].Path;
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
            if (dgvSongs.SelectedRows[0].Cells["colPath"].Value.ToString().ToLower().Contains(Constants.RS1COMP))
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
                    File.Delete(smSongCollection[dgvSongs.SelectedRows[0].Index].Path);
                    smSongCollection.RemoveAt(dgvSongs.SelectedRows[0].Index);
                    dgvSongs.Rows.RemoveAt(dgvSongs.SelectedRows[0].Index);
                }
            }
            catch (IOException ex)
            {
                Globals.Log("<ERROR>:" + ex.Message);
            }
        }

        private void cmsEditDLC_Click(object sender, EventArgs e)
        {
            // TODO: open toolkit if installed
        }

        private void cmsGetAuthorName_Click(object sender, EventArgs e)
        {
            // TODO: add image for GetAuthorName to Context Menu Strip item
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    if (dgvSongs.SelectedRows.Count > 0)
                        UpdateAuthor(dgvSongs.SelectedRows[0]);
                });
        }

        private void cmsOpenDLCLocation_Click(object sender, EventArgs e)
        {
            var path = dgvSongs.SelectedRows[0].Cells["Path"].Value.ToString();
            var directory = new FileInfo(path);
            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
        }

        private void cmsOpenDLCPage_Click(object sender, EventArgs e)
        {
            if (dgvSongs.SelectedRows.Count == 1)
            {
                var song = GetSongByRow(dgvSongs.SelectedRows[0]);
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

        private void dgvSongs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1) //if it's not header
                ShowSongInfo();
        }

        private void dgvSongs_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            var grid = (DataGridView)sender;
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
                        if (grid.Rows[e.RowIndex].Cells["colSelect"].Value.ToString().Contains(Constants.RS1COMP))
                            Globals.Log("Can not 'Select' individual RS1 Compatiblity DLC");
                        else
                        {
                            if (grid.Rows[e.RowIndex].Cells["colSelect"].Value == null)
                                grid.Rows[e.RowIndex].Cells["colSelect"].Value = false;

                            if (Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colSelect"].Value))
                                grid.Rows[e.RowIndex].Cells["colSelect"].Value = false;
                            else
                                grid.Rows[e.RowIndex].Cells["colSelect"].Value = true;

                            // as long as the data is bound this should be OK to do
                            smSongCollection[e.RowIndex].Selected = (bool)grid.Rows[e.RowIndex].Cells["colSelect"].Value;
                        }
                    }
        }

        private void dgvSongs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // right mouse click anywhere popup Context Control
            // left mouse click on header sort routines with BLRV refresh           

            if (dgvSongs.DataSource == null)
                return;

            // Ctrl Key w/ left mouse click turn off column visiblity
            if (ModifierKeys == Keys.Control)
            {
                dgvSongs.Columns[e.ColumnIndex].Visible = false;
                return;
            }

            int scrollHorizontalOffset = 0;
            int scrollVerticalOffset = 0;
            BindingSource bs = new BindingSource { DataSource = smSongCollection };
            var songsToShow = sortedSongCollection;

            Dictionary<SongData, Color> currentRows = new Dictionary<SongData, Color>();

            foreach (DataGridViewRow row in dgvSongs.Rows)
                if (row.DefaultCellStyle.BackColor != Color.White || row.DefaultCellStyle.BackColor != Color.Gray)
                    currentRows.Add((SongData)row.DataBoundItem, row.DefaultCellStyle.BackColor);

            // track sorted column and direction
            var columnName = dgvSongs.Columns[e.ColumnIndex].Name;
            if (columnName != sortColumnName)
                sortDescending = false;
            else
                sortDescending = !sortDescending;

            sortColumnName = columnName;

            switch (sortColumnName)
            {
                case "colSelect":
                    bs.DataSource = songsToShow.ToList();
                    break;

                case "colEnabled":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Enabled);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Enabled);
                    break;

                case "colSongTitle":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Song);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Song);
                    break;

                case "colSongArtist":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Artist);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Artist);
                    break;

                case "colSongAlbum":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Album);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Album);
                    break;

                case "colUpdated":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => DateTime.ParseExact(song.Updated, "M-d-y H:m", System.Globalization.CultureInfo.InvariantCulture));
                    else
                        bs.DataSource = songsToShow.OrderBy(song => DateTime.ParseExact(song.Updated, "M-d-y H:m", System.Globalization.CultureInfo.InvariantCulture));
                    break;

                case "colSongTuning":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Tuning);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Tuning);
                    break;

                case "colDD":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.DD);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.DD);
                    break;

                case "colArrangements":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Arrangements);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Arrangements);
                    break;

                case "colAuthor":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Author);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Author);
                    break;

                case "colVersion":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Version);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Version);
                    break;

                case "colToolkitVer":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.ToolkitVer);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.ToolkitVer);
                    break;

                case "colPath":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Path);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Path);
                    break;

                case "colFileName":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.FileName);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.FileName);
                    break;

                case "colSongYear":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.SongYear);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.SongYear);
                    break;

                case "colIgntionVersion":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionVersion);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.IgnitionVersion);
                    break;

                case "colIgnitionID":
                    if (sortDescending)

                        bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionID);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.IgnitionID);
                    break;

                case "colIgnitionUpdated":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.Updated);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.Updated);
                    break;

                case "colIgnitionAuthor":
                    if (sortDescending)
                        bs.DataSource = songsToShow.OrderByDescending(song => song.IgnitionAuthor);
                    else
                        bs.DataSource = songsToShow.OrderBy(song => song.IgnitionAuthor);
                    break;
            }

            scrollHorizontalOffset = dgvSongs.HorizontalScrollingOffset;
            scrollVerticalOffset = dgvSongs.VerticalScrollingOffset;
            dgvSongs.DataSource = bs;
            dgvSongs.HorizontalScrollingOffset = scrollHorizontalOffset;

            if (scrollVerticalOffset != 0)
            {
                PropertyInfo verticalOffset = dgvSongs.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                verticalOffset.SetValue(dgvSongs, scrollVerticalOffset, null);
            }

            foreach (KeyValuePair<SongData, Color> row in currentRows)
                dgvSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == row.Key).DefaultCellStyle.BackColor = row.Value;

            // enables column header sort glyph (icon)
            dgvSongs.Columns[e.ColumnIndex].SortMode = DataGridViewColumnSortMode.Automatic;
            if (sortDescending)
                dgvSongs.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            else
                dgvSongs.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;

            //  ArrangementColumnsColors();
        }

        private void dgvSongs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;
            }
        }

        private void dgvSongs_KeyDown(object sender, KeyEventArgs e)
        {
            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
            {
                for (int i = 0; i < dgvSongs.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvSongs.Rows[i];

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
                            smSongCollection[i].Selected = (bool)row.Cells["colSelect"].Value;
                        }
                    }
                }
            }
        }

        private void dgvSongs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                foreach (DataGridViewRow row in dgvSongs.Rows)
                {
                    if (allSelected)
                        row.Cells["colSelect"].Value = false;
                    else
                        row.Cells["colSelect"].Value = true;
                }
                allSelected = !allSelected;
            }
        }

        private void dgvSongs_Paint(object sender, PaintEventArgs e)
        {
            // need to wait for DataBinding and DataGridView Paint to complete before  
            // changing BLRV column color (cell formating) on initial loading

            if (bindingCompleted && !dgvPainted)
            {
                bindingCompleted = false;
                dgvPainted = true;
                Debug.WriteLine("dgvSongs Painted ... ");
                ArrangementColumnsColors();
                UpdateToolStrip();
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Extensions.InvokeIfRequired(dgvSongs, delegate
                {
                    Dictionary<SongData, Color> currentRows = new Dictionary<SongData, Color>();
                    foreach (DataGridViewRow row in dgvSongs.Rows)
                        if (row.DefaultCellStyle.BackColor != Color.White || row.DefaultCellStyle.BackColor != Color.Gray)
                            currentRows.Add((SongData)row.DataBoundItem, row.DefaultCellStyle.BackColor);

                    sortedSongCollection = smSongCollection.ToList();
                    dgvSongs.DataSource = new BindingSource().DataSource = smSongCollection;

                    foreach (KeyValuePair<SongData, Color> row in currentRows)
                        dgvSongs.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == row.Key).DefaultCellStyle.BackColor = row.Value;
                });

            Extensions.InvokeIfRequired(tbSearch, delegate { tbSearch.Text = ""; });
        }

        private void lnkLblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
                {
                    foreach (DataGridViewRow row in dgvSongs.Rows)
                        row.Cells["colSelect"].Value = !allSelected;
                };

            bWorker.RunWorkerCompleted += (se, ev) => { allSelected = !allSelected; };

            if (!bWorker.IsBusy)
                bWorker.RunWorkerAsync();
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchDLC(tbSearch.Text);
            else
                dgvSongs.DataSource = new BindingSource().DataSource = smSongCollection;
        }


    }
}