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
        // setlistMaster declared as BindingList causes update when dgv is edited
        private BindingList<SongData> setlistMaster = new BindingList<SongData>(); // prevents filtering from being inherited
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
            Globals.Settings.LoadSettingsFromFile(dgvSetlistMaster, true);

            // theoretically this error condition should not exist
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

            dlcDir = Constants.Rs2DlcFolder;
            cdlcDir = Path.Combine(dlcDir, "CDLC").ToLower();

            IncludeSubfolders();
            ProtectODLC();

            if (!LoadSetlistMaster())
                return;

            LoadSetLists(); // this generates a selection change
            // LoadSetlistSongs(); // so this is not needed
            LoadSongPacks();
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanSetlistManager)
            {
                Rescan();
                PopulateSetlistManager();
            }
            else if (Globals.ReloadSetlistManager)
            {
                PopulateSetlistManager();
            }

            Globals.RescanSetlistManager = false;
            Globals.ReloadSetlistManager = false;
            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, setlistMaster.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs in Setlist '{0}': {1}", curSetlistName, dgvSetlistSongs.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private string CreateSetlist(bool uncheckExisting, string customLabel = "Enter a new setlist name:")
        {
            var newSetlistName = String.Empty;
            using (var userInput = new FormUserInput(false))
            {
                userInput.CustomInputLabel = customLabel;
                userInput.FrmHeaderText = "Create New Setlist ...";

                if (DialogResult.OK != userInput.ShowDialog())
                    return null;

                newSetlistName = userInput.CustomInputText;
            }

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
                        // uncheck and unselect existing rows
                        foreach (DataGridViewRow row in dgvSetlists.Rows)
                        {
                            if (uncheckExisting)
                                dgvSetlists.Rows[row.Index].Cells[colSelect.Index].Value = false;

                            dgvSetlists.Rows[row.Index].Selected = false;
                        }

                        // add new row and select it
                        dgvSetlists.Rows.Add(true, "Yes", newSetlistName);
                        dgvSetlists.Rows[dgvSetlists.Rows.Count - 1].Selected = true;
                        curSetlistName = dgvSetlists.Rows[dgvSetlists.Rows.Count - 1].Cells["colSetlistName"].Value.ToString();
                        LoadSetlistSongs(curSetlistName);
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

        private void IncludeSubfolders()
        {
            cueSearch.Text = String.Empty;
            // filter out any SongPacks
            setlistMaster = new BindingList<SongData>(Globals.MasterCollection.Where(x => !x.IsRsCompPack && !x.IsSongsPsarc && !x.IsSongPack).ToList());

            if (!chkIncludeSubfolders.Checked)
            {
                var results = setlistMaster.Where(x => Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();
                LoadFilteredBindingList(results);
            }
            else
                LoadFilteredBindingList(setlistMaster);
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

            var debugMe = dgvSetlistMaster.RowCount;
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
                // dgvSetlists.Rows[0].Selected = true; // this triggers selection change
                dgvSetlists.Rows[0].Cells["colSetlistSelect"].Value = true;
                curSetlistName = dgvSetlists.Rows[0].Cells["colSetlistName"].Value.ToString();
                LoadSetlistSongs(curSetlistName);
            }
        }

        private bool LoadSetlistMaster()
        {
            bindingCompleted = false;
            dgvPainted = false;

            // double check that SongPacks have been filtered out ... JIC
            if (setlistMaster.Any(x => x.IsRsCompPack || x.IsSongsPsarc || x.IsSongPack))
            {
                AppSettings.Instance.IncludeRS1CompSongs = false;
                AppSettings.Instance.IncludeRS2BaseSongs = false;
                AppSettings.Instance.IncludeCustomPacks = false;
                // ask user to rescan song collection to remove all Song Pack songs
                var diaMsg = "Can not include RS2014 Base Song file or" + Environment.NewLine +
                             "RS1 Compatibility files as individual songs" + Environment.NewLine +
                             "in a setlist.  Please return to Song Manager " + Environment.NewLine +
                             "and perform a Full Rescan before resuming." + Environment.NewLine;
                BetterDialog2.ShowDialog(diaMsg, "Setlist Manager ...", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "ReadMe", 0, 150);
                return false;
            }

            DgvExtensions.DoubleBuffered(dgvSetlistMaster);
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
                var setlistSongs = setlistMaster.Where(sng => (sng.ArtistTitleAlbum.ToLower().Contains(search) ||
                    sng.Tunings1D.ToLower().Contains(search) || sng.FilePath.ToLower().Contains(search)) && 
                    Path.GetDirectoryName(sng.FilePath) == setlistPath).ToList();

                // the use of .Select breaks binding
                // .Select(x => new { x.Selected, x.Enabled, x.Artist, x.Song, x.Album, x.Tuning, x.Path }).ToList();

                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = new FilteredBindingList<SongData>(setlistSongs);
            }

            gbSetlistSongs.Text = String.Format("Setlist Songs from: {0}", setlistName);
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs in Setlist '{0}': {1}", setlistName, dgvSetlistSongs.Rows.Count);
            RefreshAllDgv(false);
        }

        private void LoadSongPacks()
        {
            // populate dgvSongPacks that can be enabled/disabled
            dgvSongPacks.Rows.Clear();

            var baseSongFiles = Directory.GetFiles(AppSettings.Instance.RSInstalledDir, "*").Where(file => file.ToLower().Contains("songs") && file.ToLower().EndsWith(".psarc")).ToList();
            if (baseSongFiles.Any())
                foreach (var baseSongFile in baseSongFiles)
                    dgvSongPacks.Rows.Add(false, baseSongFile.Contains("disabled") ? "No" : "Yes", baseSongFile, Path.GetFileName(baseSongFile));

            var rs1CompFiles = Directory.GetFiles(dlcDir, "*").Where(file => file.ToLower().Contains(Constants.RS1COMP)).ToList();
            if (rs1CompFiles.Any())
                foreach (var rs1CompFile in rs1CompFiles)
                    dgvSongPacks.Rows.Add(false, rs1CompFile.Contains("disabled") ? "No" : "Yes", rs1CompFile, Path.GetFileName(rs1CompFile));

            var songPackFiles = Directory.GetFiles(dlcDir, "*", SearchOption.AllDirectories).Where(file => file.ToLower().Contains(Constants.SONGPACK) || file.ToLower().Contains(Constants.ABVSONGPACK)).ToList();
            if (songPackFiles.Any())
                foreach (var songPackFile in songPackFiles)
                    dgvSongPacks.Rows.Add(false, songPackFile.Contains("disabled") ? "No" : "Yes", songPackFile, Path.GetFileName(songPackFile));

            dgvSongPacks.Columns["colSongPackPath"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void RefreshAllDgv(bool unSelectAll)
        {
            // uncheck (deselect)
            if (unSelectAll)
            {
                foreach (var song in setlistMaster)
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

            // force reload
            //Globals.ReloadSetlistManager = true;
            Globals.ReloadDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSongManager = true;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                Globals.Log(Resources.UserCancelledProcess);
                return;
            }

            // filter out any SongPacks
            setlistMaster = new BindingList<SongData>(Globals.MasterCollection.Where(x => !x.IsRsCompPack && !x.IsSongsPsarc && !x.IsSongPack).ToList());
        }

        private void SearchCDLC(string criteria)
        {
            var lowerCriteria = criteria.ToLower();
            var results = setlistMaster.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) || x.Tunings1D.ToLower().Contains(lowerCriteria) && Path.GetFileName(Path.GetDirectoryName(x.FilePath)) == "dlc").ToList();

            LoadFilteredBindingList(results);
            songSearch.Clear();
            songSearch.AddRange(results);
            LoadSetlistSongs(curSetlistName, lowerCriteria);
        }

        private void SelectionCopyMoveDelete(string mode, DataGridView dgvCurrent)
        {
            if (dgvCurrent == null || dgvCurrent.Rows.Count == 0)
                return;

            var selection = DgvExtensions.GetObjectsFromRows<SongData>(dgvCurrent);
            if (!selection.Any())
            {
                if (dgvCurrent != dgvSongPacks)
                    MessageBox.Show("Please select the checkbox next to song(s)." + Environment.NewLine + "First left mouse click the select checkbox and then" + Environment.NewLine + "right mouse click to quickly Copy, Move or Delete.  ", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (String.IsNullOrEmpty(curSetlistName))
            {
                MessageBox.Show("Please select or create a Setlist.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var setlistDir = Path.Combine(dlcDir, curSetlistName);
            mode = mode.ToLower();

            if (mode == "delete")
            {
                var diaMsg = "You are about to permenantly delete the selected songs." + Environment.NewLine + "This action can not be undone." + Environment.NewLine + Environment.NewLine + "Are you sure you want to continue?";
                if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Delete Song(s) ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
                    return;
            }

            foreach (var song in selection)
            {
                var srcPath = song.FilePath;
                var srcDir = Path.GetDirectoryName(srcPath);
                var srcFileName = Path.GetFileName(srcPath);
                var destPath = String.Empty;
                var dgvDest = new DataGridView();

                if (dgvCurrent == dgvSetlistMaster)
                {
                    destPath = Path.Combine(setlistDir, srcFileName);
                    dgvDest = dgvSetlistSongs;
                }
                else if (dgvCurrent == dgvSetlistSongs)
                {
                    destPath = Path.Combine(dlcDir, srcFileName);
                    dgvDest = dgvSetlistMaster;
                }

                if (mode == "copy" || mode == "move")
                {
                    GenExtensions.CopyFile(srcPath, destPath, true);

                    // add song from dgvDest
                    SongData newSong = new SongData();
                    var propInfo = song.GetType().GetProperties();
                    foreach (var item in propInfo)
                        if (item.CanWrite)
                            newSong.GetType().GetProperty(item.Name).SetValue(newSong, item.GetValue(song, null), null);

                    newSong.FilePath = destPath; // triggers update of dgvSetlistSongs
                    setlistMaster.Add(newSong);
                }

                if (mode == "delete" || mode == "move")
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

                    for (int rowIndex = dgvDest.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvDest, rowIndex);
                        if (sd.FilePath == srcPath)
                        {
                            dgvDest.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }

                    GenExtensions.DeleteFile(srcPath);
                }
            }

            // force reload/rescan
            Globals.ReloadSongManager = true; // full reload here
            Globals.RescanDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.RescanSetlistManager = true;
            Globals.RescanProfileSongLists = true;
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

            if (dgvCurrent.Name == "dgvSetlistMaster" && selectedDisabled > 0)
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

            // update setlist
            if (dgvCurrent.Name == "dgvSetlistMaster")
                LoadSetlistSongs(curSetlistName);

            dgvCurrent.Refresh();
            this.Refresh();
        }

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

        private void ToggleSongs(DataGridView dgvCurrent)
        {
            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, row.Index);
                if (sd == null)
                    continue;
                else if((sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc) && chkProtectODLC.Checked)
                    dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;
                else if (dgvCurrent == dgvSongPacks && chkProtectODLC.Checked)
                    dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;
                else
                    row.Cells[colSelect.Index].Value = !Convert.ToBoolean(row.Cells[colSelect.Index].Value);
            }
        }

        private void btnEnDiSetlistSong_Click(object sender, EventArgs e)
        {
            SelectionEnableDisable(dgvSetlistSongs);
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

        private void btnToggleSetlistMasterSongs_Click(object sender, EventArgs e)
        {
            ToggleSongs(dgvSetlistMaster);
        }

        private void btnToggleSetlistSongs_Click(object sender, EventArgs e)
        {
            ToggleSongs(dgvSetlistSongs);
        }

        private void btnCombineSetlists_Click(object sender, EventArgs e)
        {
            var comboSetlistName = CreateSetlist(false, "Enter name of combined setlist:");

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
                        setlistMaster.Add(newSong);
                    }
                }
            }

            foreach (DataGridViewRow row in dgvSetlists.Rows)
            {
                if (curSetlistName != dgvSetlists.Rows[dgvSetlists.Rows.Count - 1].Cells["colSetlistName"].Value.ToString())
                    dgvSetlists.Rows[row.Index].Cells[colSelect.Index].Value = false;
            }

            LoadSetlistSongs(curSetlistName);
            RefreshAllDgv(false);
        }

        private void btnCreateSetlist_Click(object sender, System.EventArgs e)
        {
            CreateSetlist(true);
        }

        private void btnDeleteSetlist_Click(object sender, System.EventArgs e)
        {
            var selectedRows = dgvSetlists.Rows.Cast<DataGridViewRow>().Where(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value)).ToList();
            if (dgvSetlists.Rows.Count == 0 || !selectedRows.Any())
                return;

            // DANGER ZONE ... give user adequate warning
            var diaMsg = "You are about to permanently delete setlist: '" + curSetlistName + "'" + Environment.NewLine + "Including all songs contained in the setlist!" + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete the setlist?";
            if (selectedRows.Count > 1)
                diaMsg = "You are about to permanently delete multiple setlists." + Environment.NewLine + "Including all songs contained in these setlists!" + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete these setlists?";

            if (DialogResult.No == BetterDialog2.ShowDialog(diaMsg, "Permanent Deletion Warning ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 0, 150))
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
                    var errMsg = "You are about to permanently delete setlist '" + curSetlistName + "'" + Environment.NewLine + "Including all songs contained in the setlist!" + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete setlist:" + Environment.NewLine + curSetlistName + " and its' songs?";

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
            LoadSetlistSongs(null);
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
                // do not deselect the Setlist
                // slRow.Cells["colSetlistSelect"].Value = false;
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
                    newSongName = songName.Replace(Constants.EnabledExtension, Constants.DisabledExtension);
                else
                    newSongName = songName.Replace(Constants.DisabledExtension, Constants.EnabledExtension);

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

            RefreshAllDgv(false);
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
                            File.Move(rs1DLCFile, rs1DLCFile.Replace(Constants.EnabledExtension, Constants.DisabledExtension));
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
                            File.Move(rs1File, rs1File.Replace(Constants.EnabledExtension, Constants.DisabledExtension));
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

        private void ProtectODLC()
        {
            // deselect and protect ODLC 
            if (chkProtectODLC.Checked)
            {
                var dgvList = new List<DataGridView>() { dgvSetlistMaster, dgvSetlistSongs, dgvSongPacks };
                foreach (var dgvCurrent in dgvList)
                {
                    var debugMe = dgvCurrent.Name;

                    foreach (DataGridViewRow row in dgvCurrent.Rows)
                    {
                        var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, row.Index);
                        if (sd != null && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                            dgvCurrent.Rows[row.Index].Cells[colSelect.Index].Value = false;

                        if (dgvCurrent == dgvSongPacks)
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

        private void cmsCopy_Click(object sender, EventArgs e)
        {
            // know VS bug SourceControl returns null
            // ContextMenuStrip cms = (ContextMenuStrip) ((ToolStripMenuItem) sender).Owner;
            // DataGridView dgv = (DataGridView) cms.SourceControl;
            // use tsmi custom tag to store current dgv object

            var tsmi = sender as ToolStripMenuItem; // Copy
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionCopyMoveDelete("Copy", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Delete
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionCopyMoveDelete("Delete", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsEnableDisable_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi == null || tsmi.Owner.Tag == null)
                return;

            var dgvCurrent = tsmi.Owner.Tag as DataGridView;
            SelectionEnableDisable(dgvCurrent);
        }

        private void cmsMove_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Move
            if (tsmi != null && tsmi.Owner.Tag != null)
                SelectionCopyMoveDelete("Move", tsmi.Owner.Tag as DataGridView);
        }

        private void cmsSelectAllNone_Click(object sender, EventArgs e)
        {
            var tsmi = sender as ToolStripMenuItem; // Select All/None
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
                LoadFilteredBindingList(setlistMaster);
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

                if (dgvCurrent == dgvSetlistMaster)
                {
                    // colorize the enabled and path columns depending on cdlc location
                    if (e.ColumnIndex == colEnabled.Index || e.ColumnIndex == colFilePath.Index)
                    {
                        var songPath = Path.GetDirectoryName(sd.FilePath).ToLower();
                        if (songPath == cdlcDir)
                            e.CellStyle.BackColor = cdlcColor;
                        else if (songPath != dlcDir.ToLower())
                            e.CellStyle.BackColor = setlistColor;
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

            var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvCurrent, e.RowIndex);
            if (sd == null)
                return;

            if (e.Button == MouseButtons.Right)
            {
                dgvCurrent.Rows[e.RowIndex].Selected = true;
                cmsCopy.Enabled = true;
                cmsDelete.Enabled = true;
                cmsMove.Enabled = true;
                cmsEnableDisable.Enabled = true;
                // known VS bug .. SourceControl returns null ... using tag for work around
                cmsSetlistManager.Tag = dgvCurrent;
                cmsSetlistManager.Show(Cursor.Position);
         
                if (dgvCurrent == dgvSongPacks)
                {
                    cmsCopy.Enabled = false;
                    cmsDelete.Enabled = false;
                    cmsMove.Enabled = false;

                    if (chkProtectODLC.Checked)
                        cmsEnableDisable.Enabled = false;
                }
                else
                {
                    if (chkProtectODLC.Checked && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                    {
                        cmsCopy.Enabled = false;
                        cmsDelete.Enabled = false;
                        cmsMove.Enabled = false;
                        cmsEnableDisable.Enabled = false;
                    }
                }
            }

            // user complained that clicking a row should not autocheck select
            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                try
                {
                    if (chkProtectODLC.Checked && (sd.IsODLC || sd.IsRsCompPack || sd.IsSongsPsarc))
                        dgvCurrent.Rows[e.RowIndex].Cells[colSelect.Index].Value = false;
                    else if (dgvCurrent == dgvSongPacks && chkProtectODLC.Checked)
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
                    var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, e.RowIndex);
                    sd.Selected = !sd.Selected;
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
                                var sd = DgvExtensions.GetObjectFromRow<SongData>(dgvSetlistMaster, i);
                                sd.Selected = !sd.Selected;
                            }
                        });
                    }
                }
            }
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

        private void dgvSetlists_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgvCurrent = (DataGridView)sender;
            var debugMe = dgvCurrent.Name;
            // work around for Win10 right click header hang ... check seperate and first
            if (e.RowIndex == -1)
                return;
            if (dgvCurrent.Rows.Count == 0)
                return;

            // user complained that clicking a row should not autocheck select
            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left && e.RowIndex != -1 && e.ColumnIndex == colSelect.Index)
            {
                try
                {
                    if (Convert.ToBoolean(dgvCurrent.Rows[e.RowIndex].Cells["colSetlistSelect"].Value))
                    {
                        dgvCurrent.Rows[e.RowIndex].Cells["colSetlistSelect"].Value = false;
                        var selected = dgvSetlists.Rows.Cast<DataGridViewRow>().FirstOrDefault(slr => Convert.ToBoolean(slr.Cells["colSetlistSelect"].Value));

                        if (selected == null)
                            curSetlistName = String.Empty;
                        else
                            curSetlistName = selected.Cells["colSetlistName"].Value.ToString();
                    }
                    else
                    {
                        dgvCurrent.Rows[e.RowIndex].Cells["colSetlistSelect"].Value = true;
                        curSetlistName = dgvCurrent.Rows[e.RowIndex].Cells["colSetlistName"].Value.ToString();
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

            dgvCurrent.EndEdit();
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            return; // for testing

            dgvSetlists.ClearSelection();
        }

        private void dgvSongPacks_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // makes checkbox mark appear correctly in unbound datagrid
            dgvSongPacks.EndEdit();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Search";
            RemoveFilter();

            // save current sorting before clearing search
            DgvExtensions.SaveSorting(dgvSetlistMaster);
            IncludeSubfolders();
            UpdateToolStrip();
            DgvExtensions.RestoreSorting(dgvSetlistMaster);
        }

        private void lnkSetlistMgrHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSetlistMgr.rtf", "Setlist Manager Help");
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
            Globals.DgvCurrent = dgvSetlistMaster;
            Globals.Log("Setlist Manager GUI Activated...");
            chkIncludeSubfolders.Checked = AppSettings.Instance.IncludeSubfolders;
            chkProtectODLC.Checked = AppSettings.Instance.ProtectODLC;
        }

        public void TabLeave()
        {
            Globals.Settings.SaveSettingsToFile(dgvSetlistMaster);
            Globals.Log("Setlist Manager GUI Deactivated ...");
        }

    }
}