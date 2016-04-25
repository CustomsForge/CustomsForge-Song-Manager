using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using CFSM.GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UITheme;
using DataGridViewTools;

// cache.psarc may not be renamed

namespace CustomsForgeSongManager.UControls
{
    public partial class SetlistManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        #region Constants

        private const string MESSAGE_CAPTION = "Setlist Manager";

        #endregion

        private bool allSelected = false;
        private bool bindingCompleted = false;
        private Color cdlcColor = Color.Cyan;
        private string cdlcDir;
        private string curSetlistName;
        private bool dgvPainted = false;
        private string dlcDir;
        private string rs1CompDiscPath;
        private string rs1CompDlcPath;
        private DataGridViewRow selectedRow;
        private Color setlistColor = Color.Yellow;
        private BindingList<SongData> setlistCollection = new BindingList<SongData>();
        private List<SongData> songSearch = new List<SongData>();

        public SetlistManager()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            PopulateSetlistManager();
        }

        public void PopulateSetlistManager()
        {
            Globals.Log("Populating SetlistManager GUI ...");
            DgvExtensions.DoubleBuffered(dgvSetlistMaster);
            Globals.Settings.LoadSettingsFromFile(dgvSetlistMaster);
            chkShowSetlistSongs.Checked = AppSettings.Instance.ShowSetlistSongs;

            // theoretically this error condition should never exist
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir) || !Directory.Exists(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show(@"Please fix the Rocksmith installation directory!  " + Environment.NewLine + @"This can be changed in the 'Settings' menu tab.", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // for testing
            // NOTE: changes to MultiSelect must come before data is bound to grid
            // otherwise ShowAll will throw an error if filter is removed from grid
            dgvSetlistMaster.MultiSelect = false;
            dgvSetlistSongs.MultiSelect = false;
            dgvSetlists.MultiSelect = true;
            dgvSongPacks.MultiSelect = false;

            dlcDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc");
            cdlcDir = Path.Combine(dlcDir, "CDLC").ToLower();
            if (!LoadSetlistMaster())
                return;

            LoadSetLists(); // this generates a selection change
            // LoadSetlistSongs(); // so this is not needed
            LoadSongPacks();

            // directory/file manipulation requires forced rescan
            // TODO: check if user made any actual changes
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanRenamer = true;
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSetlistManager)
            {
                Globals.RescanSetlistManager = false;
                Rescan();
                PopulateSetlistManager();
            }

            if (Globals.ReloadSetlistManager)
            {
                Globals.ReloadSetlistManager = false;
                PopulateSetlistManager();
            }

            Globals.DgvCurrent = dgvSetlistMaster;
            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, setlistCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs in '{0}' Setlist: {1}", curSetlistName, dgvSetlistSongs.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private string CreateSetlist()
        {
            string newSetlistName = Microsoft.VisualBasic.Interaction.InputBox(Properties.Resources.PleaseEnterSetlistName, "Setlist name");
            if (String.IsNullOrEmpty(newSetlistName))
                return null;

            if (newSetlistName.ToLower().Contains("disabled"))
            {
                MessageBox.Show(Properties.Resources.DisabledIsAReservedWord, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return null;
            }

            try
            {
                string setlistPath = Path.Combine(dlcDir, newSetlistName);
                if (!Directory.Exists(setlistPath))
                {
                    Directory.CreateDirectory(setlistPath);
                    // NOTE: 'Add' auto generates a call to dgvSetlists_SelectionChanged
                    if (Directory.Exists(setlistPath))
                    {
                        dgvSetlists.Rows.Add(false, "Yes", newSetlistName);
                        dgvSetlists.Rows[dgvSetlists.Rows.Count - 1].Selected = true;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format(Properties.Resources.ASetlistNamedX0AlreadyExists, newSetlistName), MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return null;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(Properties.Resources.UnableToCreateANewSetlistError + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return newSetlistName;
        }

        private void FileEnableDisable(DataGridView dgvCurrent)
        {
            var colNdxSelected = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Selected");
            var selectedCount = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value));

            if (selectedCount == 0)
                return;

            var colNdxEnabled = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Enabled");
            var colNdxPath = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "FilePath");
            var selectedDisabled = dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value) && r.Cells[colNdxEnabled].Value.ToString() == "No" && Path.GetFileName(Path.GetDirectoryName(r.Cells[colNdxPath].Value.ToString().ToLower())).Contains("dlc"));

            if (dgvCurrent.Name == "dgvSetlistMaster" && selectedDisabled > 0)
                if (MessageBox.Show("Enabling Master DLC/CDLC songs can cause Setlists   " + Environment.NewLine + "to not work as expected.  Do you want to continue?", "Enable Master DLC/CDLC ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colNdxSelected].Value))
                {
                    var originalPath = row.Cells[colNdxPath].Value.ToString();

                    try
                    {
                        if (row.Cells[colNdxEnabled].Value.ToString() == "Yes")
                        {
                            var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledPath);
                            row.Cells[colNdxPath].Value = disabledPath;
                            row.Cells[colNdxEnabled].Value = "No";
                        }
                        else
                        {
                            var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledPath);
                            row.Cells[colNdxPath].Value = enabledPath;
                            row.Cells[colNdxEnabled].Value = "Yes";
                        }

                        // row.Cells[colNdxSelected].Value = false;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable " + dgvCurrent.Name + ": " + Path.GetFileName(originalPath) + Environment.NewLine + "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // update setlist
            if (dgvCurrent.Name == "dgvSetlistMaster")
                LoadSetlistSongs(curSetlistName);

            RefreshAllDgv();
        }

        private void FileOperations(string mode, DataGridView dgvCurrent, DgvExtensions.TristateSelect selected = DgvExtensions.TristateSelect.Selected)
        {
            if (dgvCurrent == null || dgvCurrent.Rows.Count == 0)
                return;

            var debugHere = dgvCurrent.Name;
            var songsList = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent, selected).ToList();
            if (!songsList.Any() || songsList[0] == null)
            {
                MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the row to select it then" + Environment.NewLine + "right mouse click to quickly Copy, Move or Delete.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (String.IsNullOrEmpty(curSetlistName))
            {
                MessageBox.Show("Please select or create a Setlist.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var setlistDir = Path.Combine(dlcDir, curSetlistName);
            var isSetlistDisabled = curSetlistName.ToLower().Contains("disabled");
            mode = mode.ToLower();

            if (mode == "delete")
                if (MessageBox.Show("Are you sure you want to delete the selected songs?  " + Environment.NewLine + "Warning ... This action is can not be undone!", "File Deletion Warning ...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

            foreach (var song in songsList)
            {
                var srcPath = song.FilePath;
                var srcDir = Path.GetDirectoryName(srcPath);
                var srcFileName = Path.GetFileName(srcPath);
                var srcFnEnabled = srcFileName.Replace("_p.disabled.psarc", "_p.psarc");
                var srcFnDisabled = srcFileName.Replace("_p.psarc", "_p.disabled.psarc");
                bool isSrcDlcDir = Path.GetFileName(srcDir).ToLower().Contains("dlc");
                bool isSrcDisabled = srcFileName.ToLower().Contains("disabled");
                var destPath = String.Empty;

                if (mode == "copy" || mode == "move")
                {
                    if (isSrcDlcDir)
                    {
                        if (isSetlistDisabled)
                            destPath = Path.Combine(setlistDir, srcFnDisabled);
                        else
                            destPath = Path.Combine(setlistDir, srcFnEnabled);

                        if (File.Exists(destPath))
                            File.Delete(destPath);

                        File.Copy(srcPath, destPath);

                        // add song from dgvCurrent
                        SongData newSong = new SongData();
                        var propInfo = song.GetType().GetProperties();

                        foreach (var item in propInfo)
                            if (item.CanWrite)
                                newSong.GetType().GetProperty(item.Name).SetValue(newSong, item.GetValue(song, null), null);

                        newSong.FilePath = destPath;
                        setlistCollection.Add(newSong);

                        if (!isSrcDisabled && mode == "copy")
                        {
                            song.FilePath = Path.Combine(dlcDir, srcFnDisabled);
                            File.Copy(srcPath, song.FilePath);
                            File.Delete(srcPath);
                        }
                    }
                }

                if (mode == "move" || mode == "delete")
                {
                    File.Delete(srcPath);

                    // remove song from dgvSetlistMaster if it exists
                    foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
                    {
                        var sdRow = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, row.Index);
                        if (sdRow.FilePath == srcPath)
                            dgvSetlistMaster.Rows.RemoveAt(row.Index);
                    }
                }
            }

            // do seperately to prevent clearing selections
            LoadSetlistMaster();
            LoadSetlistSongs(curSetlistName);
            UpdateToolStrip();
            RefreshAllDgv(false);
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSetlistMaster.AutoGenerateColumns = false;
            var fbl = new FilteredBindingList<SongData>(list);
            var bs = new BindingSource { DataSource = fbl };
            dgvSetlistMaster.DataSource = bs;
        }

        private void LoadSetLists()
        {
            dgvSetlists.Rows.Clear();

            // find all existing Setlists (directories) in the dlc directory
            string[] setlistDirs = Directory.GetDirectories(dlcDir, "*", SearchOption.TopDirectoryOnly);
            // ignore the inlay(s) folder
            setlistDirs = setlistDirs.Where(x => !x.ToLower().Contains("inlay")).ToArray();

            // populate dgvSetlists
            foreach (var setlistDir in setlistDirs)
            {
                if (setlistDir.Contains("disabled"))
                    dgvSetlists.Rows.Add(false, "No", Path.GetFileName(setlistDir));
                else
                    dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlistDir));
            }

            if (dgvSetlists.Rows.Count > 0)
            {
                //dgvSetlists.Rows[0].Selected = true; // this triggers selection change
                dgvSetlists.Rows[0].Cells["colSetlistSelect"].Value = true;
                curSetlistName = dgvSetlists.Rows[0].Cells["colSetlistName"].Value.ToString();
                LoadSetlistSongs(curSetlistName);
            }
        }

        private bool LoadSetlistMaster()
        {
            bindingCompleted = false;
            dgvPainted = false;

            if (AppSettings.Instance.IncludeRS1DLCs)
            {
                Globals.Settings.chkIncludeRS1DLC.Checked = false;
                // ask user to rescan song collection to remove all RS1 Compatibility songs
                MessageBox.Show(Properties.Resources.CanNotIncludeRS1CompatibilityFiles, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }

            // commented out for testing ... do duplicates really cause game hangs?
            // check for duplicates
            //var dups = Globals.SongCollection.GroupBy(x => new { ArtistSongAlbum = x.ArtistTitleAlbum }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            //// this was a requested feature but it comes with a WARNING ...
            //// if user enables a duplicate in SetlistManager it will likely crash the game
            //dups.RemoveAll(x => x.FileName.ToLower().Contains("disabled"));
            //var dupsEnabled = dups.GroupBy(x => new { ArtistSongAlbum = x.ArtistTitleAlbum }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            //if (dupsEnabled.Any())
            //{
            //    // recommend that duplicates be removed before using SetlistManager
            //    MessageBox.Show("Found duplicates in the song collection." + Environment.NewLine +
            //                    "Please use the 'Duplicates' menu tab to remove" + Environment.NewLine +
            //                    "or disable songs before working in SetlistManager.  ", MESSAGE_CAPTION,
            //                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //    return false;
            //}

            //if (dups.Count - dupsEnabled.Count > 0)
            //{
            //    MessageBox.Show("Found disabled duplicates in the song collection.  " + Environment.NewLine + Environment.NewLine +
            //                    "Warning:  The game will freeze if multiple" + Environment.NewLine +
            //                    "duplicates are enabled in SetlistManager.  ", MESSAGE_CAPTION,
            //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            // local setlistCollection is loaded with Globals SongCollection
            setlistCollection = Globals.SongCollection;
            LoadFilteredBindingList(setlistCollection);
            CFSMTheme.InitializeDgvAppearance(dgvSetlistMaster);

            return true;
        }

        private void LoadSetlistSongs(string setlistName, string search = "")
        {
            var selectedRows = dgvSetlists.Rows.Cast<DataGridViewRow>().Where(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value)).ToList();
            if (dgvSetlists.Rows.Count == 0 || !selectedRows.Any())
            {
                // preserve custom column headers and clear the table
                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = null;
                gbSetlistSongs.Text = "Setlist Songs";
                Globals.TsLabel_DisabledCounter.Text = "Setlist Songs: 0";
                curSetlistName = String.Empty;
                return;
            }

            // var setlistName = selectedRows.First().Cells["colSetlistName"].Value.ToString();
            var setlistPath = Path.Combine(dlcDir, setlistName);

            if (Directory.Exists(setlistPath))
            {
                // var setlistSongs = Directory.EnumerateFiles(setlistPath, "*.psarc", SearchOption.AllDirectories).ToList();

                // CAREFUL - brain damage area
                // the use of LINQ 'Select' defeats the FilteredBindingList feature and locks data                
                var setlistSongs = setlistCollection.Where(sng => (sng.ArtistTitleAlbum.ToLower().Contains(search) || sng.Tuning.ToLower().Contains(search) || sng.FilePath.ToLower().Contains(search)) && Path.GetDirectoryName(sng.FilePath) == setlistPath).ToList();

                // the use of .Select breaks binding
                // .Select(x => new { x.Selected, x.Enabled, x.Artist, x.Song, x.Album, x.Tuning, x.Path }).ToList();

                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = new FilteredBindingList<SongData>(setlistSongs);
            }

            gbSetlistSongs.Text = String.Format("Setlist Songs from: {0}", setlistName);
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs in '{0}' Setlist: {1}", setlistName, dgvSetlistSongs.Rows.Count);
            RefreshAllDgv();
        }

        private void LoadSongPacks()
        {
            // populate dgvSongPacks that can be enabled/disabled (not all can be)           
            dgvSongPacks.Rows.Clear();

            // Directory.GetFiles is not case sensitive so pick up case through LINQ
            string[] rs1CompDiscFiles = Directory.GetFiles(dlcDir, "rs1compatibilitydisc*.psarc");
            if (rs1CompDiscFiles.Any())
            {
                rs1CompDiscPath = rs1CompDiscFiles.First(x => x.ToLower().Contains("rs1compatibilitydisc"));
                dgvSongPacks.Rows.Add(false, rs1CompDiscPath.Contains("disabled") ? "No" : "Yes", rs1CompDiscPath);
            }

            string[] rs1CompDlcFiles = Directory.GetFiles(dlcDir, "rs1compatibilitydlc*.psarc");
            if (rs1CompDlcFiles.Any())
            {
                rs1CompDlcPath = rs1CompDlcFiles.First(x => x.ToLower().Contains("rs1compatibilitydlc"));
                dgvSongPacks.Rows.Add(false, rs1CompDlcPath.Contains("disabled") ? "No" : "Yes", rs1CompDlcPath);
            }

            string[] customSongPacks = Directory.GetFiles(dlcDir, "*.*").Where(file => file.ToLower().EndsWith(".psarc") && file.ToLower().Contains("songpack")).ToArray();
            if (customSongPacks.Any())
                foreach (var customPackPath in customSongPacks)
                    dgvSongPacks.Rows.Add(false, customPackPath.Contains("disabled") ? "No" : "Yes", customPackPath);

            dgvSongPacks.Columns["colSongPackPath"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void RefreshAllDgv(bool unSelectAll = false)
        {
            // uncheck (deselect)
            if (unSelectAll)
            {
                foreach (var song in setlistCollection)
                    song.Selected = false;

                foreach (DataGridViewRow row in dgvSongPacks.Rows)
                    row.Cells["colSongPackSelect"].Value = false;
            }

            dgvSetlistMaster.Refresh();
            dgvSetlists.Refresh();
            dgvSetlistSongs.Refresh();
            dgvSongPacks.Refresh();
        }

        private void RemoveFilter()
        {
            Globals.Settings.SaveSettingsToFile(dgvSetlistMaster);
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSetlistMaster);
            LoadFilteredBindingList(setlistCollection);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSetlistMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
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

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
                return;

            Globals.ReloadSetlistManager = false;
            Globals.RescanDuplicates = false;
            Globals.RescanSongManager = false;
            Globals.RescanRenamer = false;
            Globals.ReloadSetlistManager = true;
            Globals.ReloadDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSongManager = true;
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            var results = setlistCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tuning.ToLower().Contains(lowerCriteria) && Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
            songSearch.Clear();
            songSearch.AddRange(results);
            LoadSetlistSongs(curSetlistName, lowerCriteria);
        }

        private void ToggleSongs(DataGridView dgvCurrent)
        {
            var colNdxSelected = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "Selected");

            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                row.Cells[colNdxSelected].Value = !Convert.ToBoolean(row.Cells[colNdxSelected].Value);
                dgvCurrent.Refresh();
            }
        }

        private void btnCopySongs_Click(object sender, EventArgs e)
        {
            FileOperations("Copy", dgvSetlistMaster);
        }

        private void btnMoveSongs_Click(object sender, EventArgs e)
        {
            FileOperations("Move", dgvSetlistMaster);
        }

        private void btnCombineSetlists_Click(object sender, EventArgs e)
        {
            var comboSetlistName = CreateSetlist();

            if (String.IsNullOrEmpty(comboSetlistName))
                return;

            var combinedSetlistDir = Path.Combine(dlcDir, comboSetlistName);

            if (!Directory.Exists(combinedSetlistDir))
                Directory.CreateDirectory(combinedSetlistDir);

            foreach (DataGridViewRow row in dgvSetlists.Rows.Cast<DataGridViewRow>().Where(row => Convert.ToBoolean(row.Cells["colSetlistSelect"].Value))) //row.Selected || 
            {
                // kickout clause ... don't add combo to itself
                if (row.Cells["colSetlistName"].Value.ToString() == comboSetlistName)
                    continue;

                var setlistPath = Path.Combine(dlcDir, row.Cells["colSetlistName"].Value.ToString());

                foreach (var songPath in Directory.GetFiles(setlistPath, "*.psarc"))
                {
                    var newSongPath = Path.Combine(combinedSetlistDir, Path.GetFileName(songPath).Replace(".disabled", ""));
                    DataGridViewRow masterRow = dgvSetlistMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colFilePath"].Value.ToString() == songPath).First();
                    SongData oldSong = DgvExtensions.GetObjectFromRow<SongData>(masterRow);

                    if (oldSong != null)
                    {
                        SongData newSong = new SongData();
                        var propInfo = oldSong.GetType().GetProperties();

                        foreach (var item in propInfo)
                        {
                            if (item.CanWrite)
                            {
                                newSong.GetType().GetProperty(item.Name).SetValue(newSong, item.GetValue(oldSong, null), null);
                            }
                        }

                        newSong.FilePath = newSongPath;

                        if (File.Exists(newSongPath))
                            File.Delete(newSongPath);

                        File.Copy(songPath, newSongPath);
                        Globals.Log("Copied: " + oldSong.FilePath);
                        Globals.Log("To: " + newSong.FilePath);
                        setlistCollection.Add(newSong);
                    }
                }
            }

            RefreshAllDgv();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            FileOperations("Copy", dgvSetlistMaster);
        }

        private void btnCreateSetlist_Click(object sender, System.EventArgs e)
        {
            CreateSetlist();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            FileOperations("Delete", dgvSetlistMaster);
        }

        private void btnEnDiSetlistMaster_Click(object sender, EventArgs e)
        {
            FileEnableDisable(dgvSetlistMaster);
        }

        private void btnEnDiSetlistSong_Click(object sender, EventArgs e)
        {
            FileEnableDisable(dgvSetlistSongs);
        }

        private void btnEnDiSetlist_Click(object sender, EventArgs e)
        {
            // required to catch otherwise error causing conditons
            if (dgvSetlists.Rows.Count == 0 || dgvSetlists.SelectedRows.Count.Equals(0))
                return;

            // only one setlist seleted at a time, less code confusion
            // determine currently selected setlist name, make sure it is checked
            var rowNdx = dgvSetlists.SelectedRows[0].Index;
            DataGridViewRow slRow = dgvSetlists.Rows[rowNdx];
            var setlistName = slRow.Cells["colSetlistName"].Value.ToString();
            var setlistChecked = Convert.ToBoolean(slRow.Cells["colSetlistSelect"].Value);

            if (String.IsNullOrEmpty(setlistName) || !setlistChecked)
                return;

            var setlistDir = Path.Combine(dlcDir, setlistName);
            var setlistEnabled = !setlistName.Contains("disabled");

            // rename setlist directory with current songs
            var newSetlistDir = String.Empty;

            if (setlistEnabled)
                newSetlistDir = String.Format("{0}_disabled", setlistDir);
            else
                newSetlistDir = setlistDir.Replace("_disabled", "");

            try
            {
                Directory.Move(setlistDir, newSetlistDir);
                slRow.Cells["colSetlistName"].Value = Path.GetFileName(newSetlistDir);
                slRow.Cells["colSetlistEnabled"].Value = setlistEnabled ? "No" : "Yes";
                slRow.Cells["colSetlistSelect"].Value = false;
            }
            catch (IOException ex)
            {
                MessageBox.Show(@"Unable to enable/disable setlist: " + setlistName + Environment.NewLine + "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // rename setlist songs to match new directory name enabled/disabled setting
            foreach (DataGridViewRow row in dgvSetlistSongs.Rows)
            {
                var songName = Path.GetFileName(row.Cells["colSetlistSongsPath"].Value.ToString());
                var newSongName = String.Empty;

                if (setlistEnabled)
                    newSongName = songName.Replace("_p.psarc", "_p.disabled.psarc");
                else
                    newSongName = songName.Replace("_p.disabled.psarc", "_p.psarc");

                var songPath = Path.Combine(newSetlistDir, songName);
                var newSongPath = Path.Combine(newSetlistDir, newSongName);

                try
                {
                    File.Move(songPath, newSongPath);
                    row.Cells["colSetlistSongsPath"].Value = newSongPath;
                    row.Cells["colSetlistSongsEnabled"].Value = setlistEnabled ? "No" : "Yes";
                }
                catch (IOException ex)
                {
                    MessageBox.Show(@"Unable to enable/disable setlist song: " + songName + Environment.NewLine + "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            RefreshAllDgv();
        }

        private void btnEnDiSongPack_Click(object sender, EventArgs e)
        {
            FileEnableDisable(dgvSongPacks);
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            FileOperations("Move", dgvSetlistMaster);
        }

        private void btnRemoveSetlistSong_Click(object sender, EventArgs e)
        {
            // remove rows from datagridview going backward to avoid index issues
            for (int ndx = dgvSetlistSongs.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSetlistSongs.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value))
                {
                    string setlistSongPath = row.Cells["colSetlistSongsPath"].Value.ToString();

                    // try to prevent the user from deleting their entire CDLC collection if possible
                    if (row.Cells["colSetlistSongsPath"].Value.ToString().ToLower().Contains("cdlc"))
                    {
                        var errMsg = "You are about to permanently delete all selected songs.  " + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete the songs?  ";

                        if (row.Cells["colSetlistSongsPath"].Value.ToString().ToLower().Contains("cdlc"))
                            errMsg = "Oops ... Looks like this could be your CDLC folder." + Environment.NewLine + errMsg;

                        // DANGER ZONE
                        if (MessageBox.Show(errMsg, MESSAGE_CAPTION + " ... Warning ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    // this will throw exception if not found
                    var dlcNdx = dgvSetlistMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colFilePath"].Value.ToString() == setlistSongPath).Select(r => r.Index).First();

                    try
                    {
                        File.Delete(setlistSongPath);
                        dgvSetlistMaster.Rows.RemoveAt(dlcNdx);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to remove song :" + setlistSongPath + Environment.NewLine + "<Error>: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            LoadSetlistSongs(curSetlistName);
        }

        private void btnRemoveSetlist_Click(object sender, System.EventArgs e)
        {
            var selectedRows = dgvSetlists.Rows.Cast<DataGridViewRow>().Where(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value)).ToList();
            if (dgvSetlists.Rows.Count == 0 || !selectedRows.Any())
                return;

            foreach (DataGridViewRow row in selectedRows)
            {
                curSetlistName = row.Cells["colSetlistName"].Value.ToString();
                var setlistChecked = Convert.ToBoolean(row.Cells["colSetlistSelect"].Value);
                var setlistPath = Path.Combine(dlcDir, curSetlistName);

                if (String.IsNullOrEmpty(curSetlistName) || !setlistChecked)
                    return;

                // try to prevent the user from deleting their entire CDLC collection if possible
                if (row.Cells["colSetlistName"].Value.ToString().ToLower().Contains("cdlc"))
                {
                    var errMsg = "You are about to permanently delete setlist '" + curSetlistName + "'" + Environment.NewLine + "Including all songs contained in the setlist!" + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete setlist '" + curSetlistName + "' and its' songs?  ";

                    if (row.Cells["colSetlistName"].Value.ToString().ToLower().Contains("cdlc"))
                        errMsg = "Oops ... Looks like this might be your CDLC folder." + Environment.NewLine + errMsg;

                    // DANGER ZONE ... Confirm deletion for every setlist selected .. redundant safety interlock
                    if (MessageBox.Show(errMsg, MESSAGE_CAPTION + " ... Warning ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                // enumerate CDLC in setlist dirctory if no CDLC just delete the directory and continue
                var setlistSongsPath = Directory.EnumerateFiles(setlistPath, "*.psarc", SearchOption.AllDirectories).ToList();
                bool safeDelete = true;

                foreach (var setlistSongPath in setlistSongsPath)
                {

                    // searching for song that contains the current setlist path
                    var dlcNdx = dgvSetlistMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colFilePath"].Value.ToString() == setlistSongPath).Select(r => r.Index).FirstOrDefault();

                    try
                    {
                        dgvSetlistMaster.Rows.RemoveAt(dlcNdx);
                        File.Delete(setlistSongPath);


                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to remove all songs from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistPath) + Environment.NewLine + "<Error>: " + ex.Message);
                        safeDelete = false;
                    }
                }

                // after all files have been removed then delete the directory
                if (safeDelete)
                    try
                    {
                        ZipUtilities.DeleteDirectory(setlistPath);
                        //Directory.Delete(setlistPath, true);
                        dgvSetlists.Rows.Remove(row);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to delete setlist directory:" + Environment.NewLine +
                            Path.GetDirectoryName(setlistPath) + Environment.NewLine +
                            "<Error>: " + ex.Message);
                    }
            }

            // reset dgvSetlistSongs
            if (dgvSetlistSongs.Rows.Count > 0)
                dgvSetlistSongs.Rows.Clear();

            // LoadSetlistSongs(curSetlistName);
        }

        private void btnRunRSWithSetlist_Click(object sender, EventArgs e)
        {
            // TODO: confirm this method works correctly and then enable button
            string rs2014Pack = String.Empty;
            string rs1MainPack = String.Empty;
            string rs1DLCPack = String.Empty;
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");

            // TODO: determine if EnumerateFiles is case insensitive
            List<string> rs1DLCFiles = Directory.EnumerateFiles(dlcDir, "rs1compatibilitydlc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs1Files = Directory.EnumerateFiles(dlcDir, "rs1compatibilitydisc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs2014Files = Directory.EnumerateFiles(dlcDir, "cache.psarc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();

            frmComboBoxPopup comboPopup = new frmComboBoxPopup();

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
                        if (Path.GetDirectoryName(rs2014File) != Path.Combine(AppSettings.Instance.RSInstalledDir, rs2014Pack))
                            File.Move(rs2014File, rs2014File + ".disabled");

                    if (File.Exists(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc")))
                        File.Copy(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc"), Path.Combine(AppSettings.Instance.RSInstalledDir, "cache.psarc"), true);
                    else if (File.Exists(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled")))
                        File.Copy(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled"), Path.Combine(AppSettings.Instance.RSInstalledDir, "cache.psarc"), true);
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
                    foreach (string rs1DLCFile in rs1DLCFiles)
                        if (Path.GetDirectoryName(rs1DLCFile) != Path.Combine(dlcDir, rs1DLCPack))
                            File.Move(rs1DLCFile, rs1DLCFile.Replace("_p.psarc", "_p.disabled.psarc"));
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
                        if (Path.GetDirectoryName(rs1File) != Path.Combine(dlcDir, rs1MainPack))
                            File.Move(rs1File, rs1File.Replace("_p.psarc", "_p.disabled.psarc"));
                }
            }

            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
            {
                Application.DoEvents();
                Process.Start("steam://rungameid/221680");
            }
        }

        private void btnToggleSetlistMasterSongs_Click(object sender, EventArgs e)
        {
            ToggleSongs(dgvSetlistMaster);
        }

        private void btnToggleSetlistSongs_Click(object sender, EventArgs e)
        {
            ToggleSongs(dgvSetlistSongs);
        }

        private void chkShowSetlistSongs_CheckedChanged(object sender, EventArgs e)
        {
            AppSettings.Instance.ShowSetlistSongs = chkShowSetlistSongs.Checked;
        }

        private void chkShowSetlistSongs_MouseUp(object sender, MouseEventArgs e)
        {
            if (!chkShowSetlistSongs.Checked)
            {
                var results = setlistCollection.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

                LoadFilteredBindingList(results);
            }
            else
                RemoveFilter();
        }

        private void cmsCopy_Click(object sender, EventArgs e)
        {
            // know VS bug SourceControl returns null
            // ContextMenuStrip cms = (ContextMenuStrip) ((ToolStripMenuItem) sender).Owner;
            // DataGridView dgv = (DataGridView) cms.SourceControl;
            // use tsmi custom tag to store current dgv object

            var tsmi = sender as ToolStripMenuItem; // Copy
            if (tsmi != null && tsmi.Owner.Tag != null)
                FileOperations("Copy", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Delete
            if (tsmi != null && tsmi.Owner.Tag != null)
                FileOperations("Delete", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsEnableDisable_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            FileEnableDisable(dgvCurrent);
        }

        private void cmsMove_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi != null && tsmi.Owner.Tag != null)
                FileOperations("Move", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsSelectAllNone_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Select All/None
            if (tsmi != null && tsmi.Owner.Tag != null)
            {
                var dgvCurrent = tsmi.Owner.Tag as DataGridView;
                foreach (DataGridViewRow row in dgvCurrent.Rows)
                {
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colSelect"];
                    chk.Value = !allSelected;
                }

                allSelected = !allSelected;
                dgvCurrent.Refresh();
            }
        }

        private void cmsShow_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            var colNdxPath = DgvExtensions.GetDataPropertyColumnIndex(dgvCurrent, "FilePath");
            var path = selectedRow.Cells[colNdxPath].Value.ToString();
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
                LoadFilteredBindingList(setlistCollection);
        }

        private void dgvSetlistMaster_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // HACK: data from other grids ends up here when filter is removed causing error ... figure out why?
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSetlistMaster")
                return;


            // speed hacks ...
            if (e.RowIndex == -1)
                return;
            if (dgvSetlistMaster.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                return;
            if (dgvSetlistMaster.Rows[e.RowIndex].IsNewRow) // || !dgvSetlistMaster.IsCurrentRowDirty)
                return;
            if (dgvSetlistMaster.Rows.Count < 1) // needed in case filter was set that returns no items
                return;

            SongData song = new SongData();
            try
            {
                song = dgvSetlistMaster.Rows[e.RowIndex].DataBoundItem as SongData;
            }
            catch (Exception ex)
            {
                var debugMe = ""; //
            }

            if (song != null && song.DLCKey != null)
            {
                if (song.OfficialDLC)
                {
                    e.CellStyle.Font = Constants.OfficialDLCFont;
                    // prevent checking (selecting) ODCL all together ... evil genious code
                    //DataGridViewCell cell = dgvSetlistMaster.Rows[e.RowIndex].Cells["colSelect"];
                    //DataGridViewCheckBoxCell chkCell = cell as DataGridViewCheckBoxCell;
                    //chkCell.Value = false;
                    //chkCell.FlatStyle = FlatStyle.Flat;
                    //chkCell.Style.ForeColor = Color.DarkGray;
                    //cell.ReadOnly = true;
                }

                // colorize the enabled and path columns depending on cdlc location
                if (e.ColumnIndex == colEnabled.Index || e.ColumnIndex == colFilePath.Index)
                {
                    var songPath = Path.GetDirectoryName(song.FilePath).ToLower();
                    if (songPath == cdlcDir)
                        e.CellStyle.BackColor = cdlcColor;
                    else if (songPath != dlcDir.ToLower())
                        e.CellStyle.BackColor = setlistColor;
                }
            }
        }
        private void dgvSetlistMaster_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            // same in all grids
            if (e.Button == MouseButtons.Left)
            {
                // select a single row by Ctrl-Click
                if (ModifierKeys == Keys.Control)
                {
                    var s = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, e.RowIndex);
                    s.Selected = !s.Selected;
                }
                // select multiple rows by Shift-Click on two outer rows
                else if (ModifierKeys == Keys.Shift)
                {
                    if (dgvSetlistMaster.SelectedRows.Count > 0)
                    {
                        var first = dgvSetlistMaster.SelectedRows[0];
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
                                var s = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, i);
                                s.Selected = !s.Selected;
                            }
                        });
                    }
                }
            }
        }

        // use to manipulate data with causing error
        private void TemporaryDisableDatabindEvent(Action action)
        {
            dgvSetlistMaster.DataBindingComplete -= dgvSetlistMaster_DataBindingComplete;
            try
            {
                action();
            }
            finally
            {
                dgvSetlistMaster.DataBindingComplete += dgvSetlistMaster_DataBindingComplete;
            }
        }

        private void dgvSetlistMaster_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
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
                    cmsCopy.Enabled = true;
                    cmsMove.Enabled = true;
                    cmsDelete.Enabled = true;

                    if (chkProtectODLC.Checked)
                    {
                        var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, e.RowIndex);
                        // cmsCopy.Enabled = !sng.OfficialDLC;
                        cmsMove.Enabled = !sng.OfficialDLC;
                        cmsDelete.Enabled = !sng.OfficialDLC;
                    }

                    // known VS bug .. SourceControl returns null .. using tag for work around
                    cmsSetlistManager.Tag = dgvSetlistMaster;
                    cmsSetlistManager.Show(Cursor.Position);

                }
                else
                {
                    // TODO: impliment ContextMenu for columns
                    //PopulateMenuWithColumnHeaders(cmsSetlistManagerColumns);
                    //cmsSetlistManagerColumns.Show(Cursor.Position);
                }
            }

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                TemporaryDisableDatabindEvent(() => { dgvSetlistMaster.EndEdit(); });
            }

            Thread.Sleep(50); // debounce multiple clicks
            dgvSetlistMaster.Refresh();
        }

        private void dgvSetlistMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dgvSetlistMaster.Invalidate();
        }

        private void dgvSetlistMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // HACK: catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSetlistMaster")
                return;

            // wait for DataBinding and DataGridView Paint to complete before  
            // changing color (cell formating) on initial loading
            var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSetlistMaster);
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
            //if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSetlistMaster.CurrentCell != null)
            //    RemoveFilter();
        }

        private void dgvSetlistMaster_Paint(object sender, PaintEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing cell color (formating) on initial loading
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.Log("dgvSongs Painted ... ");
            }
        }

        private void dgvSetlistMaster_SelectionChanged(object sender, EventArgs e)
        {
            return; // for testing

            // save selected row info
            if (dgvSetlistMaster.SelectedRows.Count > 0)
                selectedRow = dgvSetlistMaster.SelectedRows[0];
            // remove row selection highlighting for cells of different colors
            dgvSetlistMaster.ClearSelection();
        }

        private void dgvSetlistSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;

            if (grid.Rows.Count == 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                grid.Rows[e.RowIndex].Selected = true;
                cmsCopy.Enabled = false;
                cmsMove.Enabled = true;
                cmsDelete.Enabled = true;

                //if (chkProtectODLC.Checked)
                //{
                //    var sng = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistSongs, e.RowIndex);
                //    // cmsCopy.Enabled = !sng.OfficialDLC;
                //    cmsMove.Enabled = !sng.OfficialDLC;
                //    cmsDelete.Enabled = !sng.OfficialDLC;
                //}

                // known VS bug .. SourceControl returns null ... using tag for work around
                cmsSetlistManager.Tag = dgvSetlistSongs;
                cmsSetlistManager.Show(Cursor.Position);
            }

            // programmatic left clicking row to check/uncheck 'Select'
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    grid.Rows[e.RowIndex].Cells["colSetlistSongsSelect"].Value = !(bool)(grid.Rows[e.RowIndex].Cells["colSetlistSongsSelect"].Value);
                    RefreshAllDgv();
                }
                catch
                {
                    Thread.Sleep(50); // debounce multiple clicks
                }
            }
        }

        private void dgvSetlistSongs_SelectionChanged(object sender, EventArgs e)
        {
            return; // for testing

            if (dgvSetlistSongs.SelectedRows.Count > 0)
                selectedRow = dgvSetlistSongs.SelectedRows[0];
            dgvSetlistSongs.ClearSelection();
        }

        private void dgvSetlists_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;
            if (grid.Rows.Count == 0)
                return;

            // programmatic left clicking row to check/uncheck 'Select'
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    if (Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colSetlistSelect"].Value))
                    {
                        grid.Rows[e.RowIndex].Cells["colSetlistSelect"].Value = false;
                        var selected = dgvSetlists.Rows.Cast<DataGridViewRow>().FirstOrDefault(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value));

                        if (selected == null)
                            curSetlistName = String.Empty;
                        else
                            curSetlistName = selected.Cells["colSetlistName"].Value.ToString();
                    }
                    else
                    {
                        grid.Rows[e.RowIndex].Cells["colSetlistSelect"].Value = true;
                        curSetlistName = grid.Rows[e.RowIndex].Cells["colSetlistName"].Value.ToString();
                    }

                    if (!String.IsNullOrEmpty(cueSearch.Text))
                        LoadSetlistSongs(curSetlistName, cueSearch.Text);
                    else
                        LoadSetlistSongs(curSetlistName);
                }
                catch
                {
                    Thread.Sleep(50); // debounce multiple clicks
                }
            }
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            return; // for testing

            dgvSetlists.ClearSelection();
        }

        private void dgvSongPacks_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;
            if (grid.Rows.Count == 0)
                return;

            // programmatic left clicking row to check/uncheck 'Select'
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    grid.Rows[e.RowIndex].Cells["colSongPackSelect"].Value = !(bool)(grid.Rows[e.RowIndex].Cells["colSongPackSelect"].Value);
                }
                catch
                {
                    Thread.Sleep(50); // debounce multiple clicks
                }
            }
        }

        private void dgvSongPacks_SelectionChanged(object sender, EventArgs e)
        {
            return; // for testing

            dgvSongPacks.ClearSelection();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Search";
            RemoveFilter();
        }

        private void lnkSetlistMgrHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CustomsSongForgeManager.Resources.HelpSetlistMgr.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                var helpSetlistManager = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "Setlist Manager Help");
                    noteViewer.PopulateText(helpSetlistManager);
                    noteViewer.ShowDialog();
                }
            }
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }

        public DataGridView GetGrid()
        {
            return dgvSetlistMaster;
        }

        public void TabEnter()
        {
            Globals.Log("Setlist Manager GUI Activated...");
            Globals.DgvCurrent = dgvSetlistMaster;
        }

        public void TabLeave()
        {
            Globals.Log("Leaving Setlist Manager GUI ...");
            Globals.Settings.SaveSettingsToFile(dgvSetlistMaster);
        }

    }
}