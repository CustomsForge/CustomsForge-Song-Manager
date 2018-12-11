using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using CustomControls;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UITheme;
using GenTools;
using DataGridViewTools;
using CustomsForgeSongManager.Properties;
using UserProfileLib;
using System.Security.Principal;
using System.Reflection;


namespace CustomsForgeSongManager.UControls
{
    public partial class ProfileSongLists : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private string MESSAGE_CAPTION = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private bool allSelected = false;
        private bool bindingCompleted = false;
        private Color cdlcColor = Color.Cyan;
        private string cdlcDir;
        private List<string> cfsmSetlists;
        private int curSongListsIndex = -1;
        private string curSongListsName;
        private bool dgvPainted = false;
        private string dlcDir;
        private FavoritesListRoot favoritesListRoot;
        private List<List<string>> gameSongLists;
        private string prfldbPath;
        private DataGridViewRow selectedRow;
        private Color songListColor = Color.Yellow;
        private BindingList<SongData> songListMaster = new BindingList<SongData>(); // prevents filtering from being inherited
        private List<SongData> songListSongs;
        private SongListsRoot songListsRoot;
        private List<SongData> songSearch = new List<SongData>();

        public ProfileSongLists()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            PopulateProfileSongLists();
        }

        public void PopulateProfileSongLists()
        {
            Globals.Log("Populating Profile Song Lists GUI ...");
            DgvExtensions.DoubleBuffered(dgvSongListMaster);
            Globals.Settings.LoadSettingsFromFile(dgvSongListMaster, true);

            dlcDir = Constants.Rs2DlcFolder;
            cdlcDir = Path.Combine(dlcDir, "CDLC").ToLower();

            // bind datasource to grid
            IncludeSubfolders();
            ProtectODLC();

            LoadSongListMaster();
            LoadGameSongLists();
            UpdateToolStrip();
        }

        public void UpdateProfileSongLists()
        {
            if (Globals.PrfldbNeedsUpdate)
            {
                try
                {
                    Extensions.WriteFavoritesListRoot(favoritesListRoot, prfldbPath);
                    Globals.Log(" - User Profile FavoriteListRoot has been updated ...");
                    Extensions.WriteSongListsRoot(songListsRoot, prfldbPath);
                    Globals.Log(" - User Profile SongListsRoot has been updated ...");
                }
                catch (Exception ex)
                {
                    Globals.Log("<ERROR> User Profile FavoriteListRoot and/or SongListsRoot failed to updated ...");
                    Globals.Log(" - " + ex.Message);
                }

                Globals.PrfldbNeedsUpdate = false;
            }
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanProfileSongLists)
            {
                Globals.RescanProfileSongLists = false;
                Rescan();
                PopulateProfileSongLists();
            }
            else if (Globals.ReloadProfileSongLists)
            {
                Globals.ReloadProfileSongLists = false;
                PopulateProfileSongLists();
            }

            chkIncludeSubfolders.Checked = AppSettings.Instance.IncludeSubfolders;
            chkProtectODLC.Checked = AppSettings.Instance.ProtectODLC;

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, dgvSongListMaster.Rows.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Song List Song Count: {0}", dgvSongListSongs.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private List<string> GetSetlistManagerLists()
        {
            var setlistDirs = Directory.GetDirectories(dlcDir, "*", SearchOption.TopDirectoryOnly).ToList();
            setlistDirs = setlistDirs.Where(x => !x.ToLower().Contains("inlay")).ToList();
            setlistDirs = setlistDirs.OrderBy(x => x).ToList();

            var setlists = new List<string>();
            setlists.Add("-");

            foreach (string dir in setlistDirs)
            {
                if (Directory.GetFiles(dir, "*psarc").Count() > 0)
                    setlists.Add(new DirectoryInfo(dir).Name);
            }

            return setlists;
        }

        private void IncludeSubfolders()
        {
            cueSearch.Text = String.Empty;
            // bind songListMaster to MasterCollection
            songListMaster = new BindingList<SongData>(Globals.MasterCollection);

            if (!chkIncludeSubfolders.Checked)
                songListMaster = new BindingList<SongData>(songListMaster.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList());

            LoadFilteredBindingList(songListMaster);
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSongListMaster.AutoGenerateColumns = false;
            var fbl = new FilteredBindingList<SongData>(list);
            var bs = new BindingSource { DataSource = fbl };
            dgvSongListMaster.DataSource = bs;
        }

        private void LoadGameSongLists()
        {
            prfldbPath = AppSettings.Instance.RSProfilePath;

            // first time user
            if (String.IsNullOrEmpty(prfldbPath))
            {
                // show ProfileSongList help
                frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSongLists.rtf", "Profile Song Lists Help");
                prfldbPath = RocksmithProfile.SelectPrfldb();
            }

            if (String.IsNullOrEmpty(prfldbPath))
                return;

            Globals.Log("Loading User Profile In-Game Song Lists ...");
            Globals.Log(" - " + prfldbPath);
            // make complete backup of user profile files in case something goes wrong
            var timestamp = DateTime.Now.ToString("yyyyMM"); // make monthly backups
            var backupFileName = String.Format("ProfileBackup_{0}.zip", timestamp);
            var backupPath = Path.Combine(Constants.ProfileBackupsFolder, backupFileName);
            RocksmithProfile.BackupProfiles(prfldbPath, backupPath);

            // display current _prfldb file name in groupbox title
            gbSongLists.Text = String.Format("User Profile: {0}", Path.GetFileName(prfldbPath));

            // read FavoritesListRoot from prfldb file
            favoritesListRoot = Extensions.ReadFavoritesListRoot(prfldbPath);

            // read SongListsRoots from a prfldb file
            songListsRoot = Extensions.ReadSongListsRoot(prfldbPath);

            // create composite gameSongLists
            gameSongLists = new List<List<string>>();
            gameSongLists.Add(favoritesListRoot.FavoritesList); // index 0 will always be used for FavoritesList             
            foreach (var songList in songListsRoot.SongLists)
                gameSongLists.Add(songList);

            // count the songLists
            var gameSongListsCount = gameSongLists.Count();
            if (gameSongListsCount != 7)
                throw new DataException("<ERROR> User Profile In-Game Song Lists Count is incorrect ...");

            // count the songs in all lists
            var gameSongListsSongsCount = 0;
            foreach (var songList in gameSongLists)
                gameSongListsSongsCount += songList.Count;

            // constant names of gameSongLists
            var gameSongListsNames = new string[] { "Favorites", "Song List 1", "Song List 2", "Song List 3", "Song List 4", "Song List 5", "Song List 6" };

            // populate the SetlistManager comboboxcolumn
            cfsmSetlists = GetSetlistManagerLists();
            ((DataGridViewComboBoxColumn)dgvGameSongLists.Columns["colSetlistManager"]).DataSource = cfsmSetlists;

            // populate dgvGameSongLists
            dgvGameSongLists.Rows.Clear();
            for (int i = 0; i < gameSongListsNames.Count(); i++)
            {
                dgvGameSongLists.Rows.Add(false, gameSongListsNames[i]);
                dgvGameSongLists.Rows[i].Cells["colSetlistManager"].Value = "-";
            }

            if (dgvGameSongLists.Rows.Count > 0)
            {
                dgvGameSongLists.Rows[0].Cells["colGameSongListsSelect"].Value = true;
                curSongListsName = dgvGameSongLists.Rows[0].Cells["colGameSongLists"].Value.ToString();
                curSongListsIndex = dgvGameSongLists.Rows[0].Index;
                LoadSongListSongs(curSongListsName);
            }
        }

        private bool LoadSongListMaster()
        {
            bindingCompleted = false;
            dgvPainted = false;

            DgvExtensions.DoubleBuffered(dgvSongListMaster);
            CFSMTheme.InitializeDgvAppearance(dgvSongListMaster);

            return true;
        }

        private void LoadSongListSongs(string songListName)
        {
            var selectedRow = dgvGameSongLists.Rows.Cast<DataGridViewRow>().Where(slr => Convert.ToBoolean(slr.Cells["colGameSongListsSelect"].Value)).FirstOrDefault();
            if (selectedRow == null || dgvGameSongLists.Rows.Count == 0)
            {
                // preserve custom column headers and clear the table
                dgvSongListSongs.AutoGenerateColumns = false;
                dgvSongListSongs.DataSource = null;
                gbSongListSongs.Text = "In-Game Song List Songs";
                Globals.TsLabel_DisabledCounter.Text = "Song List Song Count: 0";
                curSongListsName = String.Empty;
                curSongListsIndex = -1;
                return;
            }

            if (curSongListsIndex == -1)
                throw new IndexOutOfRangeException("<ERROR> SongListRootIndex");

            // populate Song List Songs based on user selection from SongListsRoot
            const string noMatchingSongs = "Unknown";
            songListSongs = new List<SongData>();

            // gameSongLists zero index is always FavoritesList
            foreach (var dlcKey in gameSongLists[curSongListsIndex])
            {
                SongData sd = Globals.MasterCollection.FirstOrDefault(x => x.DLCKey == dlcKey);
                // DLCKey in combinedSongList[ndx] may not match any in songsListMaster 
                if (sd == null)
                {
                    sd = new SongData()
                       {
                           DLCKey = dlcKey,
                           Artist = noMatchingSongs,
                           Album = noMatchingSongs,
                           Title = noMatchingSongs,
                           // define these to prevent data exceptions
                           Arrangements2D = new List<Arrangement>(),
                           FilePath = noMatchingSongs
                       };
                }

                songListSongs.Add(sd);
            }

            dgvSongListSongs.Rows.Clear();
            dgvSongListSongs.AutoGenerateColumns = false;
            dgvSongListSongs.DataSource = new FilteredBindingList<SongData>(songListSongs);

            RefreshAllDgv(false);
            gbSongListSongs.Text = String.Format("In-Game Song List Songs: {0}", songListName);
            Globals.TsLabel_DisabledCounter.Text = String.Format("Song List Song Count: {0}", songListSongs.Count);
        }

        private void ProtectODLC()
        {
            // deselect and protect ODLC 
            if (chkProtectODLC.Checked)
            {
                var dgvList = new List<DataGridView>() { dgvSongListMaster, dgvSongListSongs };
                foreach (var dgvCurrent in dgvList)
                {
                    var debugMe = dgvCurrent.Name;

                    foreach (DataGridViewRow row in dgvCurrent.Rows)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, row.Index);
                        if (sd != null && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                            dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;
                    }
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

        private void RefreshAllDgv(bool unSelectAll)
        {
            // uncheck (deselect)
            if (unSelectAll)
            {
                foreach (var song in songListMaster)
                    song.Selected = false;
            }

            dgvSongListMaster.Refresh();
            dgvGameSongLists.Refresh();
            dgvSongListSongs.Refresh();
        }

        private void RemoveFilter()
        {
            Globals.Settings.SaveSettingsToFile(dgvSongListMaster);
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongListMaster);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvSongListMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSongListMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void Rescan()
        {
            // this should never happen
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show(Properties.Resources.ErrorRocksmith2014InstallationDirectorySet, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // run new worker
            using (Worker worker = new Worker())
            {
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            // force reload
            Globals.ReloadDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSongManager = true;
            Globals.ReloadSetlistManager = true;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                Globals.Log(Resources.UserCancelledProcess);
                return;
            }

            // bind songListMaster to MasterCollection
            songListMaster = new BindingList<SongData>(Globals.MasterCollection);
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            var results = songListMaster.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tunings1D.ToLower().Contains(lowerCriteria) && Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
            songSearch.Clear();
            songSearch.AddRange(results);
        }

        private void SelectionAddRemove(string mode, DataGridView dgvCurrent, List<SongData> songsFromSetlist = null)
        {
            if (dgvCurrent == null || dgvCurrent.Rows.Count == 0)
                return;

            var selection = new List<SongData>();

            if (songsFromSetlist != null && songsFromSetlist.Count() > 0)
                selection = songsFromSetlist;
            else
                selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);

            if (!selection.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the select checkbox and then" + Environment.NewLine + "right mouse click to quickly Copy, Move or Delete.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (String.IsNullOrEmpty(curSongListsName))
            {
                MessageBox.Show("Please select a Song List.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            mode = mode.ToLower();

            foreach (var song in selection)
            {
                var srcPath = song.FilePath;
                var dgvDest = new DataGridView();

                if (dgvCurrent == dgvSongListMaster)
                    dgvDest = dgvSongListSongs;
                else if (dgvCurrent == dgvSongListSongs)
                    dgvDest = dgvSongListMaster;

                if (mode == "add")
                {
                    // don't add duplicates to SongListSongs
                    SongData sd = songListSongs.FirstOrDefault(x => x.DLCKey == song.DLCKey);
                    if (sd != null)
                    {
                        Globals.Log(" - SongList " + curSongListsName + " already contains " + song.FileName);
                        break;
                    }

                    // add song
                    SongData newSong = new SongData();
                    var propInfo = song.GetType().GetProperties();
                    foreach (var item in propInfo)
                        if (item.CanWrite)
                            newSong.GetType().GetProperty(item.Name).SetValue(newSong, item.GetValue(song, null), null);

                    songListSongs.Add(newSong);

                    // reset bound DataSource to refesh dgvSongListSongs
                    dgvSongListSongs.DataSource = null;
                    dgvSongListSongs.AutoGenerateColumns = false;
                    dgvSongListSongs.DataSource = new FilteredBindingList<SongData>(songListSongs);
                }

                if (mode == "remove")
                {
                    // remove rows from datagridview going backward to avoid index issues
                    for (int rowIndex = dgvCurrent.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, rowIndex);
                        if (sd.FilePath == srcPath)
                        {
                            // the bound datagridview also removes the item from songListSongs
                            dgvSongListSongs.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                }
            }

            RefreshAllDgv(true);
            UpdateToolStrip();
            gbSongListSongs.Text = String.Format("In-Game Song List Songs: {0}", curSongListsName);

            // update local gameSongList
            var songList = songListSongs.Select(x => x.DLCKey).ToList();
            gameSongLists[curSongListsIndex] = songList;

            // update FavoritesListsRoot or SongListsRoot
            if (curSongListsIndex == 0)
                favoritesListRoot.FavoritesList = songList;
            else
                songListsRoot.SongLists[curSongListsIndex - 1] = songList;

            Globals.PrfldbNeedsUpdate = true;
        }

        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            var debugMe = dgvCurrent.Name;
            var colNdxSelected = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Selected");
            var selectedCount = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value));
            if (selectedCount == 0)
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine +
                                "First left mouse click the select checkbox and" + Environment.NewLine +
                                "then right mouse click to quickly Enable/Disable.  ",
                                Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var colNdxEnabled = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Enabled");
            var colNdxPath = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "FilePath");
            var selectedDisabled = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value) && r.Cells[colNdxEnabled].Value.ToString() == "No" && Path.GetFileName(Path.GetDirectoryName(r.Cells[colNdxPath].Value.ToString().ToLower())).Contains("dlc"));

            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colNdxSelected].Value))
                {
                    if (chkProtectODLC.Checked)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, row.Index);
                        if (sd != null && sd.IsODLC)
                            continue;
                    }

                    var originalPath = row.Cells[colNdxPath].Value.ToString();

                    try
                    {
                        if (row.Cells[colNdxEnabled].Value.ToString() == "Yes")
                        {
                            var disabledPath = originalPath.Replace(Constants.EnabledExtension, Constants.DisabledExtension);
                            if (originalPath.EndsWith(Constants.BASESONGS))
                                disabledPath = originalPath.Replace(Constants.BASESONGS, "songs.disabled.psarc");
                            File.Move(originalPath, disabledPath);
                            row.Cells[colNdxPath].Value = disabledPath;
                            row.Cells[colNdxEnabled].Value = "No";
                        }
                        else
                        {
                            var enabledPath = originalPath.Replace(Constants.DisabledExtension, Constants.EnabledExtension);
                            if (originalPath.EndsWith("songs.disabled.psarc"))
                                enabledPath = originalPath.Replace("songs.disabled.psarc", Constants.BASESONGS);
                            File.Move(originalPath, enabledPath);
                            row.Cells[colNdxPath].Value = enabledPath;
                            row.Cells[colNdxEnabled].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable " + dgvCurrent.Name + ": " + Path.GetFileName(originalPath) + Environment.NewLine +
                            "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // update Song Lists Songs
            if (dgvCurrent.Name == "dgvSongListsMaster")
                LoadSongListSongs(curSongListsName);

            dgvCurrent.Refresh();
            this.Refresh();
        }

        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvSongListMaster.DataBindingComplete -= dgvSongListMaster_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvSongListMaster.DataBindingComplete += dgvSongListMaster_DataBindingComplete;
            }
        }

        private void ToggleSongs(DataGridView dgvCurrent)
        {
            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, row.Index);
                if (sd == null)
                    continue;

                if ((sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc) && chkProtectODLC.Checked)
                    dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;
                else
                    row.Cells[colSelect.Index].Value = !Convert.ToBoolean(row.Cells[colSelect.Index].Value);
            }
        }

        private void chkIncludeSubfolders_MouseUp(object sender, MouseEventArgs e)
        {
            AppSettings.Instance.IncludeSubfolders = chkIncludeSubfolders.Checked;
            IncludeSubfolders();
        }

        private void chkProtectODLC_MouseUp(object sender, MouseEventArgs e)
        {
            AppSettings.Instance.ProtectODLC = chkProtectODLC.Checked;
            ProtectODLC();
        }

        private void chkIncludeSubfolders_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.IncludeSubfolders = chkIncludeSubfolders.Checked;
            IncludeSubfolders();
        }

        private void chkProtectODLC_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.ProtectODLC = chkProtectODLC.Checked;
            ProtectODLC();
        }

        private void cmsAdd_Click(object sender, EventArgs e)
        {
            // know VS bug SourceControl returns null
            // ContextMenuStrip cms = (ContextMenuStrip) ((ToolStripMenuItem) sender).Owner;
            // DataGridView dgv = (DataGridView) cms.SourceControl;
            // use tsmi custom tag to store current dgv object

            var tsmi = sender as ToolStripMenuItem; // Copy
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionAddRemove("Add", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsEnableDisable_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            SelectionEnableDisable(dgvCurrent);
        }

        private void cmsRemove_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionAddRemove("Remove", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsSelectAllNone_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem;
            if (tsmi != null && tsmi.Owner.Tag != null)
            {
                var dgvCurrent = tsmi.Owner.Tag as DataGridView;
                foreach (DataGridViewRow row in dgvCurrent.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[colSelect.Index];
                    chk.Value = !allSelected;
                }

                allSelected = !allSelected;
            }
        }

        private void cmsShow_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            var colNdxPath = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "FilePath");
            var path = dgvCurrent.SelectedRows[0].Cells[colNdxPath].Value.ToString();
            var directory = new FileInfo(path);

            if (directory.DirectoryName != null)
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", directory.FullName));
        }

        private void cmsToggle_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            ToggleSongs(dgvCurrent);
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(songListMaster);
        }

        private void dgvCurrent_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // HACK: data from other grids ends up here when filter is removed causing error ... figure out why?
            var grid = (DataGridView)sender;
   
            // speed hacks ...
            if (e.RowIndex == -1)
                return;
            if (grid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (grid.Rows[e.RowIndex].IsNewRow) // || !dgvCurrent.IsCurrentRowDirty)
                return;
            if (grid.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            var sd = grid.Rows[e.RowIndex].DataBoundItem as SongData;
            if (sd == null)
                return;

            if (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc)
            {
                e.CellStyle.Font = Constants.OfficialDLCFont;
                DataGridViewCell cell = grid.Rows[e.RowIndex].Cells[colSelect.Index];
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

            if (grid == dgvSongListMaster)
            {
                // colorize the enabled and path columns depending on cdlc location
                if (e.ColumnIndex == colEnabled.Index || e.ColumnIndex == colFilePath.Index)
                {
                    var songPath = Path.GetDirectoryName(sd.FilePath).ToLower();
                    if (songPath == cdlcDir)
                        e.CellStyle.BackColor = cdlcColor;
                    else if (songPath != dlcDir.ToLower())
                        e.CellStyle.BackColor = songListColor;
                }
            }
        }

        private void dgvCurrent_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // create some nicer tooltips  
            var grid = (DataGridView)sender;
            var tt = String.Empty;
            var duration = 6000;
            grid.ShowCellToolTips = false;

            if (grid == dgvGameSongLists)
            {
                if (e.RowIndex == -1) // header
                {
                    if (e.ColumnIndex == 0)
                        tt = "Click on the 'Select' checkbox" + Environment.NewLine +
                             "to load an in-game song list";

                    if (e.ColumnIndex == 2)
                        tt = "These are the user defined" + Environment.NewLine +
                             "CFSM Setlist Manager setlists";
                }
                else
                {
                    DataGridViewCell cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (e.ColumnIndex == 0 && cell.Value.Equals(false))
                        tt = "Click on the 'Select' checkbox" + Environment.NewLine +
                             "to load an in-game song list";

                    if (e.ColumnIndex == 2 && cell.Value.Equals("-"))
                        tt = "Click on dropdown arrow to select an existing" + Environment.NewLine +
                             "CFSM setlist to add to an in-game song list." + Environment.NewLine +
                             "Multiple setlist can be added by selecting" + Environment.NewLine +
                             "a different setlist from one of the other" + Environment.NewLine +
                             "available unused dropdown boxes.";
                }
            }
            else
            {
                if (e.ColumnIndex == 0)
                    tt = "Left mouse click the 'Select' checkbox to select a row" + Environment.NewLine +
                         "Right mouse click on row to show file operation options";
            }

            if (!String.IsNullOrEmpty(tt))
            {
                // Cozy's whacky work around prevents ballons that are too small for the size of the tip
                toolTip.IsBalloon = true;
                toolTip.Show(tt, grid, grid.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y)), duration);
            }
            else
                toolTip.SetToolTip(grid, null); // part of work around
        }

        private void dgvCurrent_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            // part of nicer tooltips
            var grid = (DataGridView)sender;
            toolTip.Hide(grid);
            toolTip.SetToolTip(grid, null); // part of work around
            toolTip.IsBalloon = false; // part of work around
            grid.ShowCellToolTips = true;
        }

        private void dgvCurrent_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;

            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;

            if (grid.Rows.Count == 0)
                return;

            var sd = DgvExtensions.GetObjectFromRow<SongData>(grid, e.RowIndex);
            if (sd == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                grid.Rows[e.RowIndex].Selected = true;
                cmsAdd.Enabled = grid == dgvSongListMaster ? true : false;
                cmsRemove.Enabled = grid == dgvSongListMaster ? false : true;
                cmsEnableDisable.Enabled = true;
                // known VS bug .. SourceControl returns null ... using tag for work around
                cmsProfileSongLists.Tag = grid;
                cmsProfileSongLists.Show(Cursor.Position);

                if (chkProtectODLC.Checked && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                {
                    cmsAdd.Enabled = false;
                    cmsRemove.Enabled = false;
                    cmsEnableDisable.Enabled = false;
                }
            }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                try
                {
                    if (chkProtectODLC.Checked && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                        grid.Rows[e.RowIndex].Cells[colSelect.Index].Value = false;
                    else
                        grid.Rows[e.RowIndex].Cells[colSelect.Index].Value = !(bool)(grid.Rows[e.RowIndex].Cells[colSelect.Index].Value);
                }
                catch
                {
                    // debounce excess clicking
                    Thread.Sleep(50);
                }
            }

            TemporaryDisableDatabindEvent(() => { grid.EndEdit(); });
        }

        private void dgvGameSongLists_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;
            if (grid.Rows.Count == 0)
                return;

            // programmatic left clicking on Select 
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // limit to a single song list selection
                foreach (DataGridViewRow row in grid.Rows)
                {
                    row.Cells["colGameSongListsSelect"].Value = false;
                    // reset the combobox column
                    row.Cells["colSetlistManager"].Value = "-";
                }

                try
                {
                    if (Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colGameSongListsSelect"].Value))
                    {
                        grid.Rows[e.RowIndex].Cells["colGameSongListsSelect"].Value = false;
                        var selected = dgvGameSongLists.Rows.Cast<DataGridViewRow>().FirstOrDefault(slr => Convert.ToBoolean(slr.Cells["colGameSongListsSelect"].Value));

                        if (selected == null)
                            curSongListsName = String.Empty;
                        else
                            curSongListsName = selected.Cells["colGameSongLists"].Value.ToString();
                    }
                    else
                    {
                        grid.Rows[e.RowIndex].Cells["colGameSongListsSelect"].Value = true;
                        curSongListsName = grid.Rows[e.RowIndex].Cells["colGameSongLists"].Value.ToString();
                    }

                    curSongListsIndex = e.RowIndex;
                    LoadSongListSongs(curSongListsName);
                }
                catch
                {
                    Thread.Sleep(50); // debounce multiple clicks
                }

                grid.EndEdit();
            }
        }

        private void dgvGameSongLists_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (e.ColumnIndex != 2)
                return;

            var setlistName = dgvGameSongLists.Rows[e.RowIndex].Cells["colSetlistManager"].Value.ToString();
            if (setlistName == "-")
                return;

            var inGameSetlist = dgvGameSongLists.Rows[0].Cells["colGameSongLists"].Value.ToString();
            var diaMsg = "Songs from CFSM SetlistManager created setlist: '" + setlistName + "'" + Environment.NewLine +
                         "will be added to the Rocksmith in-game song list: '" + inGameSetlist + "'" + Environment.NewLine + Environment.NewLine +
                         "Do you want to continue?";

            if (DialogResult.Yes != BetterDialog2.ShowDialog(diaMsg, "Add songs to the in-game song list?", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
            {
                // reset the combobox column
                // dgvGameSongLists.Rows[e.RowIndex].Cells["colSetlistManager"].Value = ((DataGridViewComboBoxColumn)dgvGameSongLists.Columns["colSetlistManager"]).Items[0];
                dgvGameSongLists.Rows[e.RowIndex].Cells["colSetlistManager"].Value = "-";
                return;
            }

            var setlistSongs = new List<SongData>();
            string dlcDir = Constants.Rs2DlcFolder;
            string setlistPath = Path.Combine(dlcDir, setlistName);

            foreach (var songPath in Directory.GetFiles(setlistPath, "*.psarc"))
            {
                var song = Globals.MasterCollection.FirstOrDefault(s => s.FilePath == songPath);

                if (song != null)
                    setlistSongs.Add(song);
            }

            SelectionAddRemove("add", dgvGameSongLists, setlistSongs);
        }

        private void dgvGameSongLists_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvGameSongLists.IsCurrentCellDirty)
                dgvGameSongLists.CommitEdit(DataGridViewDataErrorContexts.Commit);

            // resets the dgv combobox cell
            if (dgvGameSongLists.IsCurrentCellInEditMode)
                dgvGameSongLists.EndEdit();

            dgvGameSongLists.Invalidate();
        }

        private void dgvSongListMaster_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongListMaster, e.RowIndex);
                    sd.Selected = !sd.Selected;
                }
                // select multiple rows by Shift-Click on two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvSongListMaster.SelectedRows.Count > 0)
                    {
                        var first = dgvSongListMaster.SelectedRows[0];
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
                                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSongListMaster, i);
                                    sd.Selected = !sd.Selected;
                                }
                            });
                    }
                }
            }
        }

        private void dgvSongListMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongListsMaster")
                return;

            // wait for DataBinding and DataGridView Paint to complete before  
            // changing color (cell formating) on initial loading
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongListMaster);
            if (!bindingCompleted)
            {
                // Globals.Log("DataBinding Complete ... ");
                bindingCompleted = true;
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
                }
            }

            // filter removed ... does not seem need here so commented out
            //if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSongListsMaster.CurrentCell != null)
            //    RemoveFilter();
        }

        private void dgvSongListMaster_Paint(object sender, PaintEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing cell color (formating) on initial loading
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.Log("dgvSongs Painted ... ");
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Search";
            RemoveFilter();

            // save current sorting before clearing search
            DgvExtensions.SaveSorting(dgvSongListMaster);
            UpdateToolStrip();
            DgvExtensions.RestoreSorting(dgvSongListMaster);
        }

        private void lnkLoadPrfldb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AppSettings.Instance.RSProfilePath = String.Empty;
            prfldbPath = String.Empty;
            LoadGameSongLists();
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void lnkSongListsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSongLists.rtf", "Profile Song Lists Help");
        }

        public DataGridView GetGrid()
        {
            return dgvSongListMaster;
        }

        public void TabEnter()
        {
            Globals.DgvCurrent = dgvSongListMaster;
            Globals.Log("Profile Song Lists GUI Activated ...");
        }

        public void TabLeave()
        {
            UpdateProfileSongLists();
            Globals.Settings.SaveSettingsToFile(dgvSongListMaster);
            Globals.Log("Profile Song Lists GUI Deactivated ...");
        }
    }
}