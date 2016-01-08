using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using System.Linq;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using CustomsForgeManager.CustomsForgeManagerLib.UITheme;
using CustomsForgeManager.Forms;
using DataGridViewTools;
using Newtonsoft.Json;

// cache.psarc may not be renamed

namespace CustomsForgeManager.UControls
{
    public partial class SetlistManager : UserControl, IDataGridViewHolder, INotifyTabChanged
    {
        private const string MESSAGE_CAPTION = "Setlist Manager";
        private bool bindingCompleted = false;
        private bool dgvPainted = false;
        private string dlcDir;
        private string rs1CompDiscPath;
        private string rs1CompDlcPath;
        private BindingList<SongData> songCollection = new BindingList<SongData>();
        private List<SongData> songSearch = new List<SongData>();
        private int songsInSetlists;

        public SetlistManager()
        {
            InitializeComponent();
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
            PopulateSetlistManager();
        }

        public void PopulateSetlistManager()
        {
            Globals.Log("Populating SetlistManager GUI ...");
            CFSMTheme.DoubleBuffered(dgvSetlistMaster);
            Globals.Settings.LoadSettingsFromFile(dgvSetlistMaster);
            chkSubFolders.Checked = true;

            // theoretically this error condition should never exist
            if (String.IsNullOrEmpty(AppSettings.Instance.RSInstalledDir) || !Directory.Exists(AppSettings.Instance.RSInstalledDir))
            {
                MessageBox.Show(@"Please fix the Rocksmith installation directory!  " + Environment.NewLine + @"This can be changed in the 'Settings' menu tab.", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dlcDir = Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc");

            if (!LoadSetlistMaster())
                return;

            LoadSetLists(); // this generates a selection change
            LoadSongPacks();

            // set custom selection (highlighting) color
            dgvSetlistMaster.DefaultCellStyle.SelectionBackColor = Color.Gold;
            dgvSetlistMaster.DefaultCellStyle.SelectionForeColor = dgvSetlistMaster.DefaultCellStyle.ForeColor;
            dgvSetlistSongs.DefaultCellStyle.SelectionBackColor = Color.Gold;
            dgvSetlistSongs.DefaultCellStyle.SelectionForeColor = dgvSetlistSongs.DefaultCellStyle.ForeColor;
            dgvSetlists.DefaultCellStyle.SelectionBackColor = Color.Gold;
            dgvSetlists.DefaultCellStyle.SelectionForeColor = dgvSetlists.DefaultCellStyle.ForeColor;
            dgvSongPacks.DefaultCellStyle.SelectionBackColor = Color.Gold;
            dgvSongPacks.DefaultCellStyle.SelectionForeColor = dgvSongPacks.DefaultCellStyle.ForeColor;

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
            }

            if (Globals.ReloadSetlistManager)
            {
                Globals.ReloadSetlistManager = false;
                PopulateSetlistManager();
            }

            Globals.TsLabel_MainMsg.Text = string.Format(Properties.Resources.RocksmithSongsCountFormat, songCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format(Properties.Resources.SongsUsedInSetlistsCountX0, "0");
            Globals.TsLabel_DisabledCounter.Visible = true;
            Globals.TsLabel_StatusMsg.Visible = false;
        }

        private void HighlightUsedSongs(Color color)
        {
            songsInSetlists = 0;
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
            {
                var dlcSongPath = row.Cells["colPath"].Value.ToString();
                // check if song has already been added to a setlist
                if (dlcDir.ToLower() != Path.GetDirectoryName(dlcSongPath).ToLower())
                {
                    row.DefaultCellStyle.BackColor = color;
                    songsInSetlists++;
                }
            }

            if (String.IsNullOrEmpty(DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSetlistMaster)))
                Globals.TsLabel_DisabledCounter.Text = String.Format(Properties.Resources.SongsUsedInSetlistsCountX0, songsInSetlists);
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSetlistMaster.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
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
                dgvSetlists.Rows[0].Selected = true; // this triggers selection change
                // dgvSetlists.Rows[0].Cells["colSetlistSelect"].Value = true;
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

            // local songCollection is loaded with Globals SongCollection
            songCollection = Globals.SongCollection;
            LoadFilteredBindingList(songCollection);
            CFSMTheme.InitializeDgvAppearance(dgvSetlistMaster);

            return true;
        }

        private void LoadSetlistSongs(string search = "")
        {
            if (dgvSetlists.Rows.Count == 0 || dgvSetlists.SelectedRows.Count.Equals(0))
            {
                // preserve custom column headers and clear the table
                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = null;
                return;
            }

            // determine currently selected setlist name, make sure it is checked
            var curSetlistName = dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistName"].Value.ToString();
            // var curSetlistChecked = Convert.ToBoolean(dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistSelect"].Value);
            if (String.IsNullOrEmpty(curSetlistName)) // || !curSetlistChecked)
                return;

            var setlistPath = Path.Combine(dlcDir, curSetlistName);

            if (Directory.Exists(setlistPath))
            {
                // CAREFUL - brain damage area
                // the use of LINQ 'Select' defeats the FilteredBindingList feature and locks data                
                var setlistSongs = songCollection.Where(sng => (sng.ArtistTitleAlbum.ToLower().Contains(search) ||
                    sng.Tuning.ToLower().Contains(search) ||
                    sng.Path.ToLower().Contains(search)) && sng.Path.Contains(setlistPath)).ToList();
                // use of .Select breaks binding
                // .Select(x => new { x.Selected, x.Enabled, x.Artist, x.Song, x.Album, x.Tuning, x.Path }).ToList();

                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = new FilteredBindingList<SongData>(setlistSongs);
            }

            // belt and suspenders refresh
            dgvSetlistMaster.ClearSelection();
            dgvSetlistSongs.ClearSelection();
            dgvSetlistMaster.Refresh();
            dgvSetlistSongs.Refresh();
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

            dgvSongPacks.Columns["colSongPackPath"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void RemoveFilter()
        {
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSetlistMaster);
            LoadFilteredBindingList(songCollection);

            // reset alternating row color
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSetlistMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            UpdateToolStrip();
        }

        private void RemoveHighlightUsedSongs()
        {
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            dgvSetlistMaster.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            dgvSetlistMaster.Refresh();
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

            PopulateSetlistManager();
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
            var results = songCollection.Where(x => x.ArtistTitleAlbum.ToLower().Contains(lowerCriteria) ||
                                                    x.Tuning.ToLower().Contains(lowerCriteria) &&
                                                    Path.GetFileName(Path.GetDirectoryName(x.Path)) == "dlc").ToList();

            LoadFilteredBindingList(results);
            songSearch.Clear();
            songSearch.AddRange(results);
            LoadSetlistSongs(lowerCriteria);
        }

        private void btnAddDlcSong_Click(object sender, EventArgs e)
        {
            // see Setlist Manager Safety Rules!
            if (dgvSetlists.Rows.Count == 0)
                return;

            // determine currently selected setlist name, make sure it is checked
            var curSetlistName = dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistName"].Value.ToString();
            var curSetlistChecked = Convert.ToBoolean(dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistSelect"].Value);
            if (String.IsNullOrEmpty(curSetlistName) || !curSetlistChecked)
                return;

            for (int ndx = dgvSetlistMaster.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSetlistMaster.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    string dlcSongPath = row.Cells["colPath"].Value.ToString();
                    // double GetFileNameWithoutExtenstion is required to complete remove double extension
                    var dlcSongName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(dlcSongPath));
                    string setlistSongPath = Path.Combine(dlcDir, curSetlistName, dlcSongName);

                    if (curSetlistName.Contains("disabled"))
                        setlistSongPath = String.Format("{0}.disabled.psarc", setlistSongPath);
                    else
                        setlistSongPath = String.Format("{0}.psarc", setlistSongPath);

                    // check if song has already been added to a setlist
                    if (!string.Equals(dlcDir, Path.GetDirectoryName(dlcSongPath), StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageBox.Show(@"Warning ... Prevented game hanging" + Environment.NewLine + Environment.NewLine +
                                         @"Song file: " + Path.GetFileName(dlcSongPath) + Environment.NewLine +
                                         @"Has already been added to setlest: " + Path.GetDirectoryName(dlcSongPath) + @"  " + Environment.NewLine +
                                         @"First remove the song from the setlist and try again.", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    try
                    {
                        // update dgvSongs
                        //row.Cells["colEnabled"].Value = curSetlistName.Contains("disabled") ? "No" : "Yes";
                        //row.Cells["colPath"].Value = setlistSongPath;
                        row.Cells["colSelect"].Value = false;
                        row.DefaultCellStyle.BackColor = Color.Yellow;

                        SongData oldSong = row.DataBoundItem as SongData;

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

                            newSong.Path = setlistSongPath;
                            File.Copy(dlcSongPath, setlistSongPath);
                            Globals.Log("Copied: " + oldSong.Path);
                            Globals.Log("To: " + newSong.Path);
                            songCollection.Add(newSong);
                        }

                        // update songsInSetlist count
                        songsInSetlists++;
                        Globals.TsLabel_DisabledCounter.Text = String.Format(Properties.Resources.SongsUsedInSetlistsCountFormat, songsInSetlists);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to copy song:" + Path.GetFileName(dlcSongPath) + @", to setlist: " + curSetlistName + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            LoadFilteredBindingList(songCollection);
            // update dgvSetlistSongs
            LoadSetlistSongs();
            HighlightUsedSongs(Color.Yellow);

            // force a rescan for data safety on next entry
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanRenamer = true;
        }

        private void btnCreateSetlist_Click(object sender, System.EventArgs e)
        {
            string setlistName = Microsoft.VisualBasic.Interaction.InputBox(Properties.Resources.PleaseEnterSetlistName, "Setlist name");
            if (String.IsNullOrEmpty(setlistName))
                return;

            if (setlistName.ToLower().Contains("disabled"))
            {
                MessageBox.Show(Properties.Resources.DisabledIsAReservedWord, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            try
            {
                string setlistPath = Path.Combine(dlcDir, setlistName);
                if (!Directory.Exists(setlistPath))
                {
                    Directory.CreateDirectory(setlistPath);
                    if (Directory.Exists(setlistPath))
                        dgvSetlists.Rows.Add(false, "Yes", setlistName); // interesting feature ;)
                }
                else
                    MessageBox.Show(string.Format(Properties.Resources.ASetlistNamedX0AlreadyExists, setlistName), MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (IOException ex)
            {
                MessageBox.Show(Properties.Resources.UnableToCreateANewSetlistError + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnDiDlcSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    var originalPath = row.Cells["colPath"].Value.ToString();

                    if (Path.GetDirectoryName(originalPath) == Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"))
                    {
                        try
                        {
                            if (row.Cells["colEnabled"].Value.ToString() == "Yes")
                            {
                                var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                                File.Move(originalPath, disabledPath);
                                row.Cells["colPath"].Value = disabledPath;
                                row.Cells["colEnabled"].Value = "No";
                            }
                            else
                            {
                                var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                                File.Move(originalPath, enabledPath);
                                row.Cells["colPath"].Value = enabledPath;
                                row.Cells["colEnabled"].Value = "Yes";
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(string.Format(@"Unable to enable/disable song: {0} in 'dlc' folder.{1}Error: {2}", Path.GetFileName(originalPath), Environment.NewLine, ex.Message));
                        }
                    }
                    else
                        MessageBox.Show(Properties.Resources.ThisSongIsIncludedInASetlist);

                }
            }

            // update setlist
            LoadSetlistSongs();

            dgvSetlistMaster.Refresh();
            dgvSetlistSongs.Refresh();
        }

        private void btnEnDiSetlistSong_Click(object sender, EventArgs e)
        {
            var selectedCount = dgvSetlistSongs.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells["colSetlistSongsSelect"].Value));
            if (selectedCount == 0)
                return;

            foreach (DataGridViewRow row in dgvSetlistSongs.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value))
                {
                    var originalPath = row.Cells["colSetlistSongsPath"].Value.ToString();

                    try
                    {
                        if (row.Cells["colSetlistSongsEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledPath);
                            row.Cells["colSetlistSongsPath"].Value = disabledPath;
                            row.Cells["colSetlistSongsEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledPath);
                            row.Cells["colSetlistSongsPath"].Value = enabledPath;
                            row.Cells["colSetlistSongsEnabled"].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable setlist song: " + Path.GetFileName(originalPath) + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            dgvSetlistMaster.Refresh();
            dgvSetlistSongs.Refresh();
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
            }
            catch (IOException ex)
            {
                MessageBox.Show(@"Unable to enable/disable setlist: " + setlistName + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show(@"Unable to enable/disable setlist song: " + songName + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            dgvSetlistMaster.Refresh();
            dgvSetlistSongs.Refresh();
        }

        private void btnEnDiSongPack_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongPacks.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSongPackSelect"].Value))
                {
                    var originalPath = row.Cells["colSongPackPath"].Value.ToString();

                    try
                    {
                        if (row.Cells["colSongPackEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledPath);
                            row.Cells["colSongPackPath"].Value = disabledPath;
                            row.Cells["colSongPackEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledPath);
                            row.Cells["colSongPackPath"].Value = enabledPath;
                            row.Cells["colSongPackEnabled"].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable Song Pack: " + Path.GetFileName(originalPath) + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnRemoveSetlistSong_Click(object sender, EventArgs e)
        {
            bool safe2Delete = false;

            // remove rows from datagridview going backward to avoid index issues
            for (int ndx = dgvSetlistSongs.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSetlistSongs.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value))
                {
                    string setlistSongPath = row.Cells["colSetlistSongsPath"].Value.ToString();

                    if (chkDeleteSetlistOrSetlistSongs.Checked && !safe2Delete)
                    {
                        // DANGER ZONE
                        if (MessageBox.Show("You are about to permanently delete all 'Selected' songs(s).  " + Environment.NewLine + Environment.NewLine +
                                            "Are you sure you want to permanently delete the(se) songs(s)", MESSAGE_CAPTION + " ... Warning ... Warning",
                                            MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        safe2Delete = true;
                    }

                    var dlcNdx = dgvSetlistMaster.Rows.Cast<DataGridViewRow>()
                        .Where(r => r.Cells["colPath"].Value.ToString() == setlistSongPath)
                        .Select(r => r.Index).First();  // this will throw exception if not found

                    if (safe2Delete) // this is redundant for safety
                        try
                        {
                            // be really sure you want to do this
                            File.Delete(setlistSongPath);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(@"Unable to delete song :" + setlistSongPath + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    else
                        try
                        {
                            dgvSetlistMaster.Rows.RemoveAt(dlcNdx);
                            File.Delete(setlistSongPath);
                            // alt: move song back to dlc folder
                            //var dlcSongPath = Path.Combine(dlcDir, Path.GetFileName(setlistSongPath));
                            //File.Move(setlistSongPath, dlcSongPath);
                            // update dgvSetlistMaster
                            //dgvSetlistMaster.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                            //dgvSetlistMaster.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                            //dgvSetlistMaster.Rows[dlcNdx].Cells["colSelect"].Value = false;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(@"Unable to remove song from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistSongPath) + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                }
            }

            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.Yellow);
            LoadSetlistSongs();
        }

        private void btnRemoveSetlist_Click(object sender, System.EventArgs e)
        {
            // required to catch otherwise error causing conditons
            if (dgvSetlists.Rows.Count == 0 || dgvSetlists.SelectedRows.Count.Equals(0))
                return;

            // only one setlist seleted at a time, less code confusion
            // determine currently selected setlist name, make sure it is checked
            var rowNdx = dgvSetlists.SelectedRows[0].Index;
            DataGridViewRow row = dgvSetlists.Rows[rowNdx];
            var setlistName = row.Cells["colSetlistName"].Value.ToString();
            var setlistChecked = Convert.ToBoolean(row.Cells["colSetlistSelect"].Value);
            var setlistPath = Path.Combine(dlcDir, setlistName);

            if (String.IsNullOrEmpty(setlistName) || !setlistChecked)
                return;

            if (chkDeleteSetlistOrSetlistSongs.Checked)
            {
                // DANGER ZONE ... Confirm deletion for every setlist selected .. redundant safety interlock
                if (MessageBox.Show("You are about to permanently delete setlist '" + setlistName + "'" + Environment.NewLine + "Including all songs contained in the setlist!" + Environment.NewLine + Environment.NewLine + "Are you sure you want to permanently delete setlist '" + setlistName + "' and its' songs?", MESSAGE_CAPTION + " ... Warning ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            // enumerate everything in setlist dirctory
            var setlistSongsPath = Directory.EnumerateFiles(setlistPath, "*", SearchOption.AllDirectories);
            bool safeDelete = true;

            foreach (var setlistSongPath in setlistSongsPath)
            {
                // searching for song that contains the current setlist path
                var dlcNdx = dgvSetlistMaster.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colPath"].Value.ToString() == setlistSongPath).Select(r => r.Index).FirstOrDefault();
                var dlcSongPath = Path.Combine(dlcDir, Path.GetFileName(setlistSongPath));

                try
                {
                    // update dgvSongs
                    if (chkDeleteSetlistOrSetlistSongs.Checked)
                        dgvSetlistMaster.Rows.RemoveAt(dlcNdx);
                    else
                    {
                        File.Delete(setlistSongPath);
                        dgvSetlistMaster.Rows.RemoveAt(dlcNdx);
                        // alt: move setlist song back to dlc folder
                        // File.Move(setlistSongPath, dlcSongPath);
                        //dgvSetlistMaster.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                        //dgvSetlistMaster.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                        //dgvSetlistMaster.Rows[dlcNdx].Cells["colSelect"].Value = false;
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(@"Unable to remove all songs from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistPath) + Environment.NewLine + "Error: " + ex.Message);
                    safeDelete = false;
                }
            }

            // after all files have been removed then delete the directory
            if (safeDelete)
                try
                {
                    Directory.Delete(setlistPath, true);
                    dgvSetlists.Rows.Remove(row);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(@"Unable to delete setlist directory:" + Environment.NewLine + Path.GetDirectoryName(setlistPath) + Environment.NewLine + "Error: " + ex.Message);
                }

            // reset dgvSetlistSongs
            LoadSetlistSongs();
            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.Yellow);
        }

        private void btnRunRSWithSetlist_Click(object sender, EventArgs e)
        {
            // TODO: confirm this method works correctly and then enable button
            // String.Empty may be more transportable across platforms
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

        private void btnToggleDlcSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlistMaster.Rows)
                row.Cells["colSelect"].Value = !Convert.ToBoolean(row.Cells["colSelect"].Value);
        }

        private void btnToggleSongsInSetlist_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlistSongs.Rows)
                row.Cells["colSetlistSongsSelect"].Value = !Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value);
        }

        private void chkDeleteSetlistOrSetlistSongs_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeleteSetlistOrSetlistSongs.Checked)
            {
                btnRemoveSetlist.BackColor = Color.Red;
                btnRemoveSetlistSong.BackColor = Color.Red;
            }
            else
            {
                btnRemoveSetlist.BackColor = SystemColors.Control;
                btnRemoveSetlistSong.BackColor = SystemColors.Control;
            }
        }

        private void chkEnableDelete_CheckedChanged(object sender, EventArgs e)
        {
            chkDeleteSetlistOrSetlistSongs.Enabled = chkEnableDelete.Checked;

            if (chkDeleteSetlistOrSetlistSongs.Enabled)
                chkDeleteSetlistOrSetlistSongs.BackColor = Color.Yellow;
            else
            {
                chkDeleteSetlistOrSetlistSongs.BackColor = SystemColors.Control;
                chkDeleteSetlistOrSetlistSongs.Checked = false;
            }
        }

        private void chkSubFolders_MouseUp(object sender, MouseEventArgs e)
        {
            if (!chkSubFolders.Checked)
            {
                var results = songCollection
                    .Where(x => Path.GetFileName(Path.GetDirectoryName(x.Path)) == "dlc")
                    .ToList();

                LoadFilteredBindingList(results);
            }
            else
                RemoveFilter();
        }

        private void cueSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (cueSearch.Text.Length > 0) // && e.KeyCode == Keys.Enter)
                SearchCDLC(cueSearch.Text);
            else
                LoadFilteredBindingList(songCollection);
        }

        private void dgvSetlistMaster_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            bindingCompleted = false;
            dgvPainted = false;

            var grid = (DataGridView)sender;

            // for debugging
            //var erow = e.RowIndex;
            //var ecol = grid.Columns[e.ColumnIndex].Name;
            //Globals.Log("erow = " + erow + "  ecol = " + ecol);

            // removed for testing works better without this
            // programmatic left clicking on colSelect (toggling)
            //if (e.Button == MouseButtons.Left)
            //    if (e.RowIndex != -1)
            //        if (grid.Columns[e.ColumnIndex].Name == "colSelect")
            //            grid.Rows[e.RowIndex].Cells["colSelect"].Value = !Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colSelect"].Value);

            Thread.Sleep(50); // debounce multiple clicks
            dgvSetlistSongs.Refresh();
            dgvSetlistMaster.Refresh();
        }

        private void dgvSetlistMaster_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.Yellow);
        }

        private void dgvSetlistMaster_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // workaround to catch DataBindingComplete called by other UC's
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

            // filter removed 
            if (String.IsNullOrEmpty(filterStatus) && dgvPainted && this.dgvSetlistMaster.CurrentCell != null)
                RemoveFilter();
        }

        private void dgvSetlistMaster_Paint(object sender, PaintEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing cell color (formating) on initial loading
            if (bindingCompleted && !dgvPainted)
            {
                dgvPainted = true;
                // Globals.Log("dgvSongs Painted ... ");
                HighlightUsedSongs(Color.Yellow);
            }
        }

        private void dgvSetlistSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;

            // programmatic left clicking on colSelect
            if (e.Button == MouseButtons.Left)
                if (e.RowIndex != -1)
                    if (grid.Columns[e.ColumnIndex].Name == "colSetlistSongsSelect")
                        grid.Rows[e.RowIndex].Cells["colSetlistSongsSelect"].Value = !Convert.ToBoolean(grid.Rows[e.RowIndex].Cells["colSetlistSongsSelect"].Value);

            Thread.Sleep(50); // debounce multiple clicks
            dgvSetlistSongs.Refresh();
            dgvSetlistMaster.Refresh();
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            // required to catch otherwise error causing conditions
            if (dgvSetlists.Rows.Count == 0 || dgvSetlists.SelectedRows.Count.Equals(0))
                return;

            // ensures only one setlist is selected (checked) at a time
            foreach (DataGridViewRow row in dgvSetlists.Rows)
                row.Cells["colSetlistSelect"].Value = false;

            dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistSelect"].Value = true;

            if (!Convert.ToBoolean(dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistSelect"].Value))
                return;

            if (!String.IsNullOrEmpty(cueSearch.Text))
                LoadSetlistSongs(cueSearch.Text);
            else
                LoadSetlistSongs();

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
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.HelpSetlistMgr.txt");
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
            LeaveSetlistManager();
        }

        public void LeaveSetlistManager()
        {
            Globals.Log("Leaving Setlist Manager GUI ...");
            Globals.Settings.SaveSettingsToFile(dgvSetlistMaster);
        }



    }
}

