using System.ComponentModel;
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

// Profile Song Lists tab menu is under development
// TODO Items:
// 1. Establish the names of the six in-game SongLists and update songListsNames array variable
// 2. Populate the Song List Songs based on user SongListsRoot selection
//    Involves cross matching DLC Key from SongList and MasterSongData
// 3. Save any user changes made to SongListSongs back to songListsRoot variable
// 4. Develop a User Profile SongListsRoot packer or memory popper method to write the new SongListsRoot
// 5. (optional) determine User Profile Name in case multiple profiles exist

namespace CustomsForgeSongManager.UControls
{
    public partial class ProfileSongLists : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private const string MESSAGE_CAPTION = "Profile Song Lists";
        private bool allSelected = false;
        private bool bindingCompleted = false;
        private Color cdlcColor = Color.Cyan;
        private string cdlcDir;
        private string curSongListsRootName;
        private string dlcDir;
        private bool dgvPainted = false;
        private string prfldbFile;
        private DataGridViewRow selectedRow;
        private Color songListColor = Color.Yellow;
        private BindingList<SongData> songListMaster = new BindingList<SongData>(); // prevents filtering from being inherited
        private SongListsRoot songListsRoot = new SongListsRoot();
        private List<SongData> songSearch = new List<SongData>();

        public ProfileSongLists()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
        }

        public void PopulateProfileSongLists()
        {
            Globals.Log("Populating Profile Song Lists GUI ...");
            DgvExtensions.DoubleBuffered(dgvSongListMaster);
            Globals.Settings.LoadSettingsFromFile(dgvSongListMaster, true);

            dlcDir = Constants.Rs2DlcFolder;
            cdlcDir = Path.Combine(dlcDir, "CDLC").ToLower();

            LoadSongListMaster();
            LoadSongListsRoot();
            UpdateToolStrip();
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanProfileSongLists)
            {
                Rescan();
                IncludeSubfolders();
                PopulateProfileSongLists();
            }
            else if (Globals.ReloadProfileSongLists)
            {
                IncludeSubfolders();
                PopulateProfileSongLists();
            }

            Globals.RescanSetlistManager = false;
            Globals.ReloadSetlistManager = false;
            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, songListMaster.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs in '{0}' Song List: {1}", curSongListsRootName, dgvSongListSongs.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void IncludeSubfolders()
        {
            cueSearch.Text = String.Empty;
            // filter out any SongPacks
            songListMaster = new BindingList<SongData>(Globals.MasterCollection.Where(x => !x.IsRsCompPack && !x.IsSongsPsarc && !x.IsSongPack).ToList());

            if (!chkIncludeSubfolders.Checked)
            {
                var results = songListMaster.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
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

            var debugMe = dgvSongListMaster.RowCount;
        }

        private bool LoadSongListMaster()
        {
            bindingCompleted = false;
            dgvPainted = false;

            // TODO: determine how in-game Song Lists handle SongPacks

            // double check that SongPacks have been filtered out ... JIC
            if (songListMaster.Any(x => x.IsRsCompPack || x.IsSongsPsarc || x.IsSongPack))
            {
                AppSettings.Instance.IncludeRS1CompSongs = false;
                AppSettings.Instance.IncludeRS2BaseSongs = false;
                AppSettings.Instance.IncludeCustomPacks = false;
                // ask user to rescan song collection to remove all Song Pack songs
                var diaMsg = "Can not include RS2014 Base Song file or" + Environment.NewLine +
                             "RS1 Compatibility files as individual songs" + Environment.NewLine +
                             "in a song list.  Please return to Song Manager " + Environment.NewLine +
                             "and perform a Full Rescan before resuming." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Profile Song Lists ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return false;
            }

            DgvExtensions.DoubleBuffered(dgvSongListMaster);
            CFSMTheme.InitializeDgvAppearance(dgvSongListMaster);

            return true;
        }

        // TODO: FIXME
        private void LoadSongListSongs(string songListName, string search = "")
        {
            var selectedRow = dgvSongListsRoot.Rows.Cast<DataGridViewRow>().Where(slr => Convert.ToBoolean(slr.Cells["colSongListsRootSelect"].Value)).FirstOrDefault();
            if (selectedRow == null || dgvSongListsRoot.Rows.Count == 0)
            {
                // preserve custom column headers and clear the table
                dgvSongListSongs.AutoGenerateColumns = false;
                dgvSongListSongs.DataSource = null;
                gbSongListSongs.Text = "Song List Songs";
                Globals.TsLabel_DisabledCounter.Text = "Song List Songs: 0";
                curSongListsRootName = String.Empty;
                return;
            }

            //
            // TODO: populate Song List Songs based on user selection from SongListsRoot
            //

            //dgvSongList.AutoGenerateColumns = false;
            //dgvSongList.DataSource = new FilteredBindingList<SongData>(songListSongs);

            gbSongListSongs.Text = String.Format("Song List Songs: {0}", songListName);
            Globals.TsLabel_DisabledCounter.Text = String.Format("Song List '{0}', Song Count: {1}", songListName, dgvSongListSongs.Rows.Count);
            RefreshAllDgv(false);
        }

        private void LoadSongListsRoot()
        {
            // TODO: maybe store prfldbFile as AppSettings.Instance            
            if (String.IsNullOrEmpty(prfldbFile))
                prfldbFile = RocksmithProfile.SelectPrfldb();

            if (String.IsNullOrEmpty(prfldbFile))
                return;

            gbSongListsRoot.Text = String.Format("User Profile: {0}", Path.GetFileName(prfldbFile));

            // reads the six SongLists from a prfldb file
            songListsRoot = RocksmithProfile.ReadProfileSongLists(prfldbFile);

            dgvSongListsRoot.Rows.Clear();

            // TODO: determine Profile Song Lists proper naming
            var songListsRootNames = new string[] { "Favorites TBD1", "Sorted by Artist Name TBD2", "Sorted by Album Name TBD3", "Sorted by TBD4", "Sorted by TBD5", "Sorted by TBD6" };

            // populate dgvSongListsRoot
            for (int i = 0; i < songListsRoot.SongLists.Count; i++)
                dgvSongListsRoot.Rows.Add(false, songListsRootNames[i]);

            if (dgvSongListsRoot.Rows.Count > 0)
            {
                dgvSongListsRoot.Rows[0].Cells["colSongListsRootSelect"].Value = true;
                curSongListsRootName = dgvSongListsRoot.Rows[0].Cells["colSongListsRootName"].Value.ToString();

                LoadSongListSongs(curSongListsRootName);
            }
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
                        if (sd != null && sd.IsODLC)
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
            dgvSongListsRoot.Refresh();
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

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                Globals.Log(Resources.UserCancelledProcess);
                return;
            }

            // filter out any SongPacks
            songListMaster = new BindingList<SongData>(Globals.MasterCollection.Where(x => !x.IsRsCompPack && !x.IsSongsPsarc && !x.IsSongPack).ToList());
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            var results = songListMaster.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tunings1D.ToLower().Contains(lowerCriteria) && Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
            songSearch.Clear();
            songSearch.AddRange(results);
            LoadSongListSongs(curSongListsRootName, lowerCriteria);
        }

        private void SelectionCopyRemove(string mode, DataGridView dgvCurrent)
        {
            if (dgvCurrent == null || dgvCurrent.Rows.Count == 0)
                return;

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any())
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the select checkbox and then" + Environment.NewLine + "right mouse click to quickly Copy, Move or Delete.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (String.IsNullOrEmpty(curSongListsRootName))
            {
                MessageBox.Show("Please select a Song List.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            mode = mode.ToLower();

            foreach (var song in selection)
            {
                var srcPath = song.FilePath;
                var dgvDest = new DataGridView();

                if (mode == "copy")
                {
                    // add song from dgvDest
                    SongData newSong = new SongData();
                    var propInfo = song.GetType().GetProperties();
                    foreach (var item in propInfo)
                        if (item.CanWrite)
                            newSong.GetType().GetProperty(item.Name).SetValue(newSong, item.GetValue(song, null), null);
                }

                if (mode == "remove")
                {
                    // remove rows from datagridview going backward to avoid index issues
                    for (int rowIndex = dgvCurrent.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, rowIndex);
                        if (sd.FilePath == srcPath)
                        {
                            dgvCurrent.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }

                    //for (int rowIndex = dgvDest.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                    //{
                    //    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvDest, rowIndex);
                    //    if (sd.FilePath == srcPath)
                    //    {
                    //        dgvDest.Rows.RemoveAt(rowIndex);
                    //        break;
                    //    }
                    //}
                }
            }

            UpdateToolStrip();
            RefreshAllDgv(true);
        }

        private void SelectionEnableDisable(DataGridView dgvCurrent)
        {
            var debugMe = dgvCurrent.Name;
            var colNdxSelected = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Selected");
            var selectedCount = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value));
            if (selectedCount == 0)
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the select checkbox and" + Environment.NewLine + "then right mouse click to quickly Enable/Disable.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var colNdxEnabled = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Enabled");
            var colNdxPath = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "FilePath");
            var selectedDisabled = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value) && r.Cells[colNdxEnabled].Value.ToString() == "No" && Path.GetFileName(Path.GetDirectoryName(r.Cells[colNdxPath].Value.ToString().ToLower())).Contains("dlc"));

            if (dgvCurrent.Name == "dgvSongListsMaster" && selectedDisabled > 0)
                if (MessageBox.Show("Enabling Master Songs DLC/CDLC can cause Setlists   " + Environment.NewLine + "to not work as expected.  Do you want to continue?", "Enable Master Songs DLC/CDLC ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

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
                        MessageBox.Show(@"Unable to enable/disable " + dgvCurrent.Name + ": " + Path.GetFileName(originalPath) + Environment.NewLine + "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // update Song Lists Songs
            if (dgvCurrent.Name == "dgvSongListsMaster")
                LoadSongListSongs(curSongListsRootName);

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
                if (sd != null && sd.IsODLC && chkProtectODLC.Checked)
                    dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;
                else
                    row.Cells[colSelect.Index].Value = !Convert.ToBoolean(row.Cells[colSelect.Index].Value);
            }
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
            var dgvCurrent = (DataGridView)sender;
            var debugMe = dgvCurrent.Name;

            // speed hacks ...
            if (e.RowIndex == -1)
                return;
            if (dgvCurrent.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvCurrent.Rows[e.RowIndex].IsNewRow) // || !dgvCurrent.IsCurrentRowDirty)
                return;
            if (dgvCurrent.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            var sd = dgvCurrent.Rows[e.RowIndex].DataBoundItem as SongData;
            if (sd != null)
            {
                if (sd.IsODLC)
                {
                    e.CellStyle.Font = Constants.OfficialDLCFont;
                    DataGridViewCell cell = dgvCurrent.Rows[e.RowIndex].Cells[colSelect.Index];
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

                if (dgvCurrent == dgvSongListMaster)
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
        }

        private void dgvCurrent_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgvCurrent = (DataGridView)sender;
            var debugMe = dgvCurrent.Name;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;

            if (dgvCurrent.Rows.Count == 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                dgvCurrent.Rows[e.RowIndex].Selected = true;
                cmsCopy.Enabled = true;
                cmsRemove.Enabled = true;
                cmsEnableDisable.Enabled = true;

                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, e.RowIndex);
                if (sd != null && sd.IsODLC)
                {
                    cmsCopy.Enabled = !chkProtectODLC.Checked;
                    cmsRemove.Enabled = !chkProtectODLC.Checked;
                    cmsEnableDisable.Enabled = !chkProtectODLC.Checked;
                }

                // known VS bug .. SourceControl returns null ... using tag for work around
                cmsProfileSongLists.Tag = dgvCurrent;
                cmsProfileSongLists.Show(Cursor.Position);
            }

            // TODO: FIXME so that only one SongListsRoot can be selected

            // user complained that clicking a row should not autocheck select
            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                try
                {
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, e.RowIndex);
                    if (sd != null && sd.IsODLC && chkProtectODLC.Checked)
                        dgvCurrent.Rows[e.RowIndex].Cells[colSelect.Index].Value = false;
                    else
                        dgvCurrent.Rows[e.RowIndex].Cells[colSelect.Index].Value = !(bool)(dgvCurrent.Rows[e.RowIndex].Cells[colSelect.Index].Value);
                }
                catch
                {
                    // debounce excess clicking
                    Thread.Sleep(50);
                }
            }

            TemporaryDisableDatabindEvent(() => { dgvCurrent.EndEdit(); });
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

        private void dgvSongListsRoot_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgvCurrent = (DataGridView)sender;
            var debugMe = dgvCurrent.Name;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;
            if (dgvCurrent.Rows.Count == 0)
                return;

            // programmatic left clicking on Select 
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                // limit to a single song list selection
                foreach (DataGridViewRow row in dgvCurrent.Rows)
                    row.Cells["colSongListsRootSelect"].Value = false;

                try
                {
                    if (Convert.ToBoolean(dgvCurrent.Rows[e.RowIndex].Cells["colSongListsRootSelect"].Value))
                    {
                        dgvCurrent.Rows[e.RowIndex].Cells["colSongListsRootSelect"].Value = false;
                        var selected = dgvSongListsRoot.Rows.Cast<DataGridViewRow>().FirstOrDefault(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value));

                        if (selected == null)
                            curSongListsRootName = String.Empty;
                        else
                            curSongListsRootName = selected.Cells["colSongListsRootName"].Value.ToString();
                    }
                    else
                    {
                        dgvCurrent.Rows[e.RowIndex].Cells["colSongListsRootSelect"].Value = true;
                        curSongListsRootName = dgvCurrent.Rows[e.RowIndex].Cells["colSongListsRootName"].Value.ToString();
                    }

                    if (!String.IsNullOrEmpty(cueSearch.Text))
                        LoadSongListSongs(curSongListsRootName, cueSearch.Text);
                    else
                        LoadSongListSongs(curSongListsRootName);
                }
                catch
                {
                    Thread.Sleep(50); // debounce multiple clicks
                }
            }

            dgvCurrent.EndEdit();
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

        private void cmsCopy_Click(object sender, EventArgs e)
        {
            // know VS bug SourceControl returns null
            // ContextMenuStrip cms = (ContextMenuStrip) ((ToolStripMenuItem) sender).Owner;
            // DataGridView dgv = (DataGridView) cms.SourceControl;
            // use tsmi custom tag to store current dgv object

            var tsmi = sender as ToolStripMenuItem; // Copy
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionCopyRemove("Copy", tsmi.Owner.Tag as DataGridView);
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
                SelectionCopyRemove("Remove", tsmi.Owner.Tag as DataGridView);
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

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Search";
            RemoveFilter();

            // save current sorting before clearing search
            DgvExtensions.SaveSorting(dgvSongListMaster);
            IncludeSubfolders();
            UpdateToolStrip();
            DgvExtensions.RestoreSorting(dgvSongListMaster);
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        private void lnkSongListsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: develop help
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSetlistMgr.rtf", "Profile Song Lists Help");
        }

        public DataGridView GetGrid()
        {
            return dgvSongListMaster;
        }

        public void TabEnter()
        {
            chkIncludeSubfolders.Checked = AppSettings.Instance.IncludeSubfolders;
            IncludeSubfolders();
            chkProtectODLC.Checked = AppSettings.Instance.ProtectODLC;
            ProtectODLC();
            PopulateProfileSongLists();
            Globals.DgvCurrent = dgvSongListMaster;
            Globals.Log("Profile Song Lists GUI Activated...");
        }

        public void TabLeave()
        {
            UpdateProfileSongLists();
            Globals.Settings.SaveSettingsToFile(dgvSongListMaster);
            Globals.Log("Profile Song Lists GUI Deactivated ...");
        }

        private void UpdateProfileSongLists()
        {
            // TODO: Impliment
            var debugMe = songListsRoot;
        }

        private void lnkLoadPrfldb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            prfldbFile = String.Empty;
            LoadSongListsRoot();
        }
    }
}