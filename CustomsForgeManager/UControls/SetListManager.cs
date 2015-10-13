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
using CustomsForgeManager.Forms;
using DataGridViewTools;

// optimized for simple directory/file I/O operations
// no fancy psarc upacking and repacking is done here :^(
//
// Setlist Manager Safety Rules:
// only one copy of a song may exist and it can be enabled or disabled
// a song can only be loaded into the game one time because if
// multiple versions of the same song are loaded it will hang the game
// when a song is added to any setlist it is highlighted in dgvSongs
// cache.psarc may not be renamed

namespace CustomsForgeManager.UControls
{
    public partial class SetlistManager : UserControl
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
            PopulateSetlistManager();
        }

        public void PopulateSetlistManager()
        {
            Globals.Log("Populating SetlistManager GUI ...");

            // theoretically this error condition should never exist
            if (String.IsNullOrEmpty(Globals.MySettings.RSInstalledDir) || !Directory.Exists(Globals.MySettings.RSInstalledDir))
            {
                MessageBox.Show(@"Please fix the Rocksmith installation directory!  " + Environment.NewLine + @"This can be changed in the 'Settings' menu tab.", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dlcDir = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");

            if (LoadSongs())
            {
                LoadSetLists(); // this generates a selection change
                LoadSongPacks();

                // set custom selection (highlighting) color
                dgvSongs.DefaultCellStyle.SelectionBackColor = Color.Gold;
                dgvSongs.DefaultCellStyle.SelectionForeColor = dgvSongs.DefaultCellStyle.ForeColor;
                dgvSetlistSongs.DefaultCellStyle.SelectionBackColor = Color.Gold;
                dgvSetlistSongs.DefaultCellStyle.SelectionForeColor = dgvSetlistSongs.DefaultCellStyle.ForeColor;
                dgvSetlists.DefaultCellStyle.SelectionBackColor = Color.Gold;
                dgvSetlists.DefaultCellStyle.SelectionForeColor = dgvSetlists.DefaultCellStyle.ForeColor;
                dgvSongPacks.DefaultCellStyle.SelectionBackColor = Color.Gold;
                dgvSongPacks.DefaultCellStyle.SelectionForeColor = dgvSongPacks.DefaultCellStyle.ForeColor;
            }

            // directory/file manipulation requires forced rescan
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

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", songCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Songs Used In Setlists Count: {0}", "0");
            Globals.TsLabel_DisabledCounter.Visible = true;

            Globals.TsLabel_StatusMsg.Visible = false;
            Globals.TsLabel_StatusMsg.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_StatusMsg.Text = "Show &All";
            Globals.TsLabel_StatusMsg.IsLink = true;
            Globals.TsLabel_StatusMsg.LinkBehavior = LinkBehavior.HoverUnderline;
            Globals.TsLabel_StatusMsg.Click += lnkShowAll_Click;
        }

        private void DgvSongsAppearance()
        {
            // easier to keep track of appearance setting here
            dgvSongs.Visible = true; // must come first for setting to apply correctly
            dgvSongs.AllowUserToAddRows = false; // removes empty row at bottom
            dgvSongs.AllowUserToDeleteRows = false;
            dgvSongs.AllowUserToOrderColumns = true;
            dgvSongs.AllowUserToResizeColumns = true;
            dgvSongs.AllowUserToResizeRows = true;
            // dgvSongs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            dgvSongs.BackgroundColor = SystemColors.AppWorkspace;
            dgvSongs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSongs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSongs.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvSongs.EnableHeadersVisualStyles = true;
            dgvSongs.Font = new Font("Arial", 8);
            dgvSongs.GridColor = SystemColors.ActiveCaption;
            dgvSongs.MultiSelect = false;
            dgvSongs.Name = "dgvSongs";
            dgvSongs.RowHeadersVisible = false; // remove row arrow
            dgvSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSongs.ClearSelection();
        }

        private void HighlightUsedSongs(Color color)
        {
            songsInSetlists = 0;
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                var dlcSongPath = row.Cells["colPath"].Value.ToString();
                // check if song has already been added to a setlist
                if (dlcDir.ToLower() != Path.GetDirectoryName(dlcSongPath).ToLower())
                {
                    row.DefaultCellStyle.BackColor = color;
                    songsInSetlists++;
                }
            }

            if (String.IsNullOrEmpty(DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongs)))
                Globals.TsLabel_DisabledCounter.Text = String.Format("Songs Used In Setlists Count: {0}", songsInSetlists);
        }

        private void LoadFilteredBindingList(dynamic list)
        {
            bindingCompleted = false;
            dgvPainted = false;
            // sortable binding list with drop down filtering
            dgvSongs.AutoGenerateColumns = false;
            FilteredBindingList<SongData> fbl = new FilteredBindingList<SongData>(list);
            BindingSource bs = new BindingSource { DataSource = fbl };
            dgvSongs.DataSource = bs;
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
                // .Select(x => new { x.Selected, x.Enabled, x.Artist, x.Song, x.Album, x.Tuning, x.Path }).ToList();

                dgvSetlistSongs.AutoGenerateColumns = false;
                dgvSetlistSongs.DataSource = new FilteredBindingList<SongData>(setlistSongs);
            }

            // belt and suspenders refresh
            dgvSongs.ClearSelection();
            dgvSetlistSongs.ClearSelection();
            dgvSongs.Refresh();
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

        private bool LoadSongs()
        {
            bindingCompleted = false;
            dgvPainted = false;

            if (Globals.MySettings.IncludeRS1DLCs)
            {
                Globals.Settings.chkIncludeRS1DLC.Checked = false;
                // ask user to rescan song collection to remove all RS1 Compatiblity songs
                MessageBox.Show("Can not include RS1 compatiblity files as individual" + Environment.NewLine +
                                "songs in a setlist.  Please return to SongManager and  " + Environment.NewLine +
                                "rescan before returning to Setlist Manager. ", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }

            // local smSongCollection is loaded with Globals SongCollection
            songCollection = Globals.SongCollection;

            // check for duplicates
            var dups = songCollection.GroupBy(x => new { ArtistSongAlbum = x.ArtistTitleAlbum }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            // this was a requested feature but it comes with a WARNING ...
            // if user enables a duplicate in SetlistManager it will likely crash the game
            dups.RemoveAll(x => x.FileName.ToLower().Contains("disabled"));
            var dupsEnabled = dups.GroupBy(x => new { ArtistSongAlbum = x.ArtistTitleAlbum }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dupsEnabled.Any())
            {
                // recommend that duplicates be removed before using SetlistManager
                MessageBox.Show("Found duplicates in the song collection." + Environment.NewLine +
                                "Please use the 'Duplicates' menu tab to remove" + Environment.NewLine +
                                "or disable songs before working in SetlistManager.  ", MESSAGE_CAPTION,
                                MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }

            if (dups.Count - dupsEnabled.Count > 0)
            {
                MessageBox.Show("Found disabled duplicates in the song collection.  " + Environment.NewLine + Environment.NewLine +
                                "Warning:  The game will freeze if multiple" + Environment.NewLine +
                                "duplicates are enabled in SetlistManager.  ", MESSAGE_CAPTION,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // binding source creates issues will cell formating
            // wait for binding and paint to complete before changing color
            LoadFilteredBindingList(songCollection);
            DgvSongsAppearance();

            return true;
        }

        private void RemoveFilter()
        {
            DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvSongs);
            LoadSongs();
            // UpdateToolStrip();
        }

        private void RemoveHighlightUsedSongs()
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
                row.DefaultCellStyle.BackColor = Color.Empty;

            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.BackColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dgvSongs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;

            dgvSongs.Refresh();
        }

        private void Rescan()
        {
            // this should never happen
            if (String.IsNullOrEmpty(Globals.MySettings.RSInstalledDir))
            {
                MessageBox.Show("Error: Rocksmith 2014 installation directory setting is null or empty.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Globals.ReloadSetlistManager = false;
            Globals.ReloadDuplicates = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSongManager = true;
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

            for (int ndx = dgvSongs.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSongs.Rows[ndx];

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
                        // move song to setlist folder
                        File.Move(dlcSongPath, setlistSongPath);

                        // update dgvSongs
                        row.Cells["colEnabled"].Value = curSetlistName.Contains("disabled") ? "No" : "Yes";
                        row.Cells["colPath"].Value = setlistSongPath;
                        row.Cells["colSelect"].Value = false;
                        row.DefaultCellStyle.BackColor = Color.Yellow;

                        // update songsInSetlist count
                        songsInSetlists++;
                        Globals.TsLabel_DisabledCounter.Text = String.Format("Songs Used In Setlists Count: {0}", songsInSetlists);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to move song:" + Path.GetFileName(dlcSongPath) + @", to setlist: " + curSetlistName + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // update dgvSetlistSongs
            LoadSetlistSongs();

            // force a rescan for data safety on next entry
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanRenamer = true;
        }

        private void btnCreateSetlist_Click(object sender, System.EventArgs e)
        {
            string setlistName = Microsoft.VisualBasic.Interaction.InputBox("Please enter setlist name", "Setlist name");
            if (String.IsNullOrEmpty(setlistName))
                return;

            if (setlistName.ToLower().Contains("disabled"))
            {
                MessageBox.Show(@"'Disabled' is a Reserved Word ..." + Environment.NewLine +
                    @"Setlist names may not contain" + Environment.NewLine +
                    @"any form of the word 'disabled'", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                    MessageBox.Show(@"A setlist named '" + setlistName + @"' already exists!", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (IOException ex)
            {
                MessageBox.Show(@"Unable to create a new setlist: " + setlistName + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnDiDlcSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongs.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    var originalPath = row.Cells["colPath"].Value.ToString();
                    var originalFile = row.Cells["colFileName"].Value.ToString();

                    if (Path.GetDirectoryName(originalPath) == Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"))
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
                    }
                    else
                        MessageBox.Show("This song is included in a setlist." + Environment.NewLine +
                                        "Please use Setlist Songs to enable/disable setlist songs.  ");

                }
            }

            // update setlist
            LoadSetlistSongs();
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

            dgvSongs.ClearSelection();
            dgvSetlistSongs.ClearSelection();
            dgvSongs.Refresh();
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

            dgvSongs.Refresh();
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

                    var dlcNdx = dgvSongs.Rows.Cast<DataGridViewRow>()
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
                            // move song back to dlc folder
                            var dlcSongPath = Path.Combine(dlcDir, Path.GetFileName(setlistSongPath));
                            File.Move(setlistSongPath, dlcSongPath);
                            // update dgvSongs
                            dgvSongs.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                            dgvSongs.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                            dgvSongs.Rows[dlcNdx].Cells["colSelect"].Value = false;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(@"Unable to move song from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistSongPath) + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var dlcNdx = dgvSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colPath"].Value.ToString() == setlistSongPath).Select(r => r.Index).First();
                var dlcSongPath = Path.Combine(dlcDir, Path.GetFileName(setlistSongPath));

                try
                {
                    // update dgvSongs
                    if (chkDeleteSetlistOrSetlistSongs.Checked)
                        dgvSongs.Rows.RemoveAt(dlcNdx);
                    else
                    {
                        // move setlist song back to dlc folder
                        File.Move(setlistSongPath, dlcSongPath);
                        dgvSongs.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                        dgvSongs.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                        dgvSongs.Rows[dlcNdx].Cells["colSelect"].Value = false;
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(@"Unable to move all songs from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistPath) + Environment.NewLine + "Error: " + ex.Message);
                    safeDelete = false;
                }
            }

            // after all files have been moved then delete the directory
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
                        if (Path.GetDirectoryName(rs2014File) != Path.Combine(Globals.MySettings.RSInstalledDir, rs2014Pack))
                            File.Move(rs2014File, rs2014File + ".disabled");

                    if (File.Exists(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc")))
                        File.Copy(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc"), Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc"), true);
                    else if (File.Exists(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled")))
                        File.Copy(Path.Combine(dlcDir, rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled"), Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc"), true);
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
            foreach (DataGridViewRow row in dgvSongs.Rows)
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

        private void cueSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cueSearch.Text))
                return;

            string search = cueSearch.Text.ToLower();
            var matchingSongs = songCollection.Where(sng => sng.ArtistTitleAlbum.ToLower().Contains(search)
                || sng.Tuning.ToLower().Contains(search)
                && Path.GetFileName(Path.GetDirectoryName(sng.Path)) == "dlc");
            dgvSongs.Rows.Clear();
            songSearch.Clear();
            songSearch.AddRange(matchingSongs);

            LoadFilteredBindingList(matchingSongs);
            LoadSetlistSongs(cueSearch.Text.ToLower());
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
            dgvSongs.Refresh();
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

        private void dgvSongs_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
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
            dgvSongs.Refresh();
        }

        private void dgvSongs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.Yellow);
        }

        private void dgvSongs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // workaround to catch DataBindingComplete called by other UC's
            var grid = (DataGridView)sender;
            if (grid.Name != "dgvSongs")
                return;

            // wait for DataBinding and DataGridView Paint to complete before  
            // changing color (cell formating) on initial loading
            if (!bindingCompleted)
            {
                Debug.WriteLine("DataBinding Complete ... ");
                bindingCompleted = true;

                var filterStatus = DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dgvSongs);
                if (!String.IsNullOrEmpty(filterStatus))
                {
                    Globals.TsLabel_StatusMsg.Visible = true;
                    Globals.TsLabel_DisabledCounter.Text = filterStatus;
                    // ensures rows are recolored correctly
                    dgvPainted = false;
                }
            }
        }

        private void dgvSongs_Paint(object sender, PaintEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing cell color (formating) on initial loading
            if (bindingCompleted && !dgvPainted)
            {
                bindingCompleted = false;
                dgvPainted = true;
                Debug.WriteLine("dgvSongs Painted ... ");
                HighlightUsedSongs(Color.Yellow);
            }
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cueSearch.Text = String.Empty;
            cueSearch.Cue = "Search";
            RemoveFilter();
        }

        private void lnkOpenSngMgrHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ensures proper disposal of objects and variables
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.SetlistHelp.txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                string setlistManagerHelp = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, "SetlistManager Help");
                    noteViewer.PopulateText(setlistManagerHelp);
                    noteViewer.ShowDialog();
                }
            }
        }

        private void lnkShowAll_Click(object sender, EventArgs e)
        {
            RemoveFilter();
        }
    }
}

