using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using System.Linq;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using CustomsForgeManager.Forms;

// optimized for simple directory and/or file I/O operations
// no fancy psarc upacking and repacking is done here :^(
//
// Setlist Manager Safety Rules:
// only one copy of a song may exist and it can be enabled or disabled
// a song can only be loaded into the game one time because if
// multiple versions of the same song are loaded it will hang the game
// so when a song is added to any setlist it removed from dgvSetlistsSongs
// and added to the cooresponding dgvDlcSongs
// cache.psarc may not be renamed

namespace CustomsForgeManager.UControls
{
    public partial class SetlistManager : UserControl
    {
        private const string MESSAGE_CAPTION = "Setlist Manager";
        private bool bindingCompleted = false;
        private bool dgvPainted = false;
        private string dlcDir;
        private BindingList<SongData> dlcSongCollection = new BindingList<SongData>();
        private List<SongData> dlcSongs = new List<SongData>();
        private List<SongData> dlcSongsSearch = new List<SongData>();
        private string rs1CompDiscPath;
        private string rs1CompDlcPath;

        public SetlistManager()
        {
            InitializeComponent();
            PopulateSetlistManager();
        }

        public void PopulateSetlistManager()
        {
            Globals.Log("Populating SetlistManager GUI ...");

            // do error check here and it does not need to be done again
            // theoretically this error condition should never exist
            if (String.IsNullOrEmpty(Globals.MySettings.RSInstalledDir) || !Directory.Exists(Globals.MySettings.RSInstalledDir))
            {
                MessageBox.Show(@"Please fix the Rocksmith installation directory!  " + Environment.NewLine + @"This can be changed in the 'Settings' menu tab.", MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dlcDir = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");
            dgvDlcSongs.Rows.Clear();
            dgvSetlistSongs.Rows.Clear();
            dgvSetlists.Rows.Clear();
            dgvSongPacks.Rows.Clear();

            if (LoadDlcSongs())
            {
                LoadSetLists();
                LoadSetlistSongs();
                LoadSongPacks();
            }

            Globals.ReloadSetlistManager = false;
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

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
        }

        private void DgvDlcSongsAppearance()
        {
            // overrides SetlistManager.Desinger.cs, easier to change setting here than in IDE
            foreach (DataGridViewColumn col in dgvDlcSongs.Columns)
            {
                col.ReadOnly = true;
                col.Visible = false;
            }

            // always visible and first
            dgvDlcSongs.Columns["colSelect"].ReadOnly = false;
            dgvDlcSongs.Columns["colSelect"].Visible = true;
            dgvDlcSongs.Columns["colSelect"].Width = 43;
            dgvDlcSongs.Columns["colSelect"].DisplayIndex = 0;
            dgvDlcSongs.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDlcSongs.Columns["colEnabled"].Visible = true;
            dgvDlcSongs.Columns["colEnabled"].Width = 50;
            dgvDlcSongs.Columns["colSongArtist"].Visible = true;
            dgvDlcSongs.Columns["colSongArtist"].Width = 100;
            dgvDlcSongs.Columns["colSongTitle"].Visible = true;
            dgvDlcSongs.Columns["colSongTitle"].Width = 100;
            dgvDlcSongs.Columns["colSongAlbum"].Visible = true;
            dgvDlcSongs.Columns["colSongAlbum"].Width = 100;
            dgvDlcSongs.Columns["colSongTuning"].Visible = true;
            dgvDlcSongs.Columns["colSongTuning"].Width = 70;
            dgvDlcSongs.Columns["colPath"].Visible = true;
            dgvDlcSongs.Columns["colPath"].Width = 350;

            dgvDlcSongs.Visible = true; // must come first for setting to apply correctly
            dgvDlcSongs.AllowUserToAddRows = false; // removes empty row at bottom
            dgvDlcSongs.AllowUserToDeleteRows = false;
            dgvDlcSongs.AllowUserToOrderColumns = true;
            dgvDlcSongs.AllowUserToResizeColumns = true;
            dgvDlcSongs.AllowUserToResizeRows = true;
            // dgvDlcSongs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            dgvDlcSongs.BackgroundColor = SystemColors.AppWorkspace;
            dgvDlcSongs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDlcSongs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDlcSongs.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDlcSongs.EnableHeadersVisualStyles = true;
            dgvDlcSongs.Font = new Font("Arial", 8);
            dgvDlcSongs.GridColor = SystemColors.ActiveCaption;
            dgvDlcSongs.MultiSelect = false;
            dgvDlcSongs.Name = "dgvDlcSongs";
            dgvDlcSongs.RowHeadersVisible = false; // remove row arrow
            dgvDlcSongs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvDlcSongs.ClearSelection();
        }

        private void HighlightUsedSongs(Color color)
        {
            foreach (DataGridViewRow row in dgvDlcSongs.Rows)
            {
                var dlcSongPath = row.Cells["colPath"].Value.ToString();
                // check if song has already been added to a setlist
                if (dlcDir.ToLower() != Path.GetDirectoryName(dlcSongPath).ToLower())
                    row.DefaultCellStyle.BackColor = color;
            }
        }

        private bool LoadDlcSongs()
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

            // CDLC song collection is loaded in Globals.SongCollection
            dlcSongCollection = Globals.SongCollection;

            // check for duplicates
            var dups = dlcSongCollection.GroupBy(x => new { x.Song, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            if (dups.Any())
            {
                // recommend that duplicates be removed before using SetlistManager
                MessageBox.Show("Found duplicates in the song collection." + Environment.NewLine +
                                "Please use the 'Duplicates' menu tab to" + Environment.NewLine +
                                "remove them before working in SetlistManager.  ", MESSAGE_CAPTION,
                                MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }

            var dlcSorted = dlcSongCollection.GroupBy(x => new { x.Artist, x.Song, x.Album, }).SelectMany(group => group).ToList();
            dlcSongs.Clear();
            dlcSongs.AddRange(dlcSorted);

            // binding source creates issues will cell formating
            // wait for binding and paint to complete before changing color
            BindingSource bs = new BindingSource { DataSource = dlcSongs };
            dgvDlcSongs.DataSource = bs;
            DgvDlcSongsAppearance();
            return true;
        }

        private void LoadSetLists()
        {
            // TODO: revise to get setlist info from local dlcSongCollection
            dgvSetlists.Rows.Clear();
            dgvSetlistSongs.Rows.Clear();

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
        }

        private void LoadSetlistSongs(string search = "")
        {
            // TODO: revise to get setlist songs info from local dlcSongCollection

            dgvSetlistSongs.Rows.Clear();

            if (dgvSetlists.Rows.Count == 0)
                return;

            // this loads songs into dgvSetlistSongs frow all selected dgvSetlists ... wow, cool
            foreach (DataGridViewRow row in dgvSetlists.SelectedRows)
            {
                string setlistPath = Path.Combine(dlcDir, row.Cells["colSetlistName"].Value.ToString());

                if (Directory.Exists(setlistPath))
                {
                    // TODO: Globals.SongCollection may be different than dgvDlcSongs by now need to check this
                    var matchingSongs = Globals.SongCollection.Where(sng => (sng.Artist.ToLower().Contains(search) || sng.Album.ToLower().Contains(search) || sng.Song.ToLower().Contains(search) || sng.Path.ToLower().Contains(search)) && sng.Path.Contains(setlistPath)).ToList();
                    foreach (SongData song in matchingSongs)
                        dgvSetlistSongs.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Tuning, song.Path);
                }
            }

            // alternate method
            // populate dgvDlcSongs with songs from first SetList 
            //if (setlistDirs.Any())
            //{
            //    var setlistDirFiles = Directory.EnumerateFiles(setlistDirs[0], "*.psarc", SearchOption.AllDirectories).ToArray();

            //    foreach (string dlcFilePath in setlistDirFiles)
            //    {
            //        // TODO: using LINQ here could be a bottle neck ... bench mark this
            //        // this is not going to be accurate if there are multiple files with same name
            //        var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == dlcFilePath);
            //        if (song != null)
            //            dgvSetlistSongs.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Tuning, song.Path);
            //    }

            //    dgvSetlists.Rows[0].Selected = true;
            //}

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
                var rs1CompDiscName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(rs1CompDiscPath));
                dgvSongPacks.Rows.Add(false, rs1CompDiscPath.Contains("disabled") ? "No" : "Yes", rs1CompDiscName, rs1CompDiscPath);
            }

            string[] rs1CompDlcFiles = Directory.GetFiles(dlcDir, "rs1compatibilitydlc*.psarc");
            if (rs1CompDlcFiles.Any())
            {
                rs1CompDlcPath = rs1CompDlcFiles.First(x => x.ToLower().Contains("rs1compatibilitydlc"));
                var rs1CompDlcName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(rs1CompDlcPath));
                dgvSongPacks.Rows.Add(false, rs1CompDlcPath.Contains("disabled") ? "No" : "Yes", rs1CompDlcName, rs1CompDlcPath);
            }
        }

        private void RemoveHighlightUsedSongs()
        {
            BindingSource bs = new BindingSource { DataSource = dlcSongs };
            dgvDlcSongs.DataSource = bs;
            DgvDlcSongsAppearance();
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

            // determine currently selected setlist name
            string curSetlist = dgvSetlists.Rows[dgvSetlists.SelectedRows[0].Index].Cells["colSetlistName"].Value.ToString();
            if (String.IsNullOrEmpty(curSetlist))
                return;

            for (int ndx = dgvDlcSongs.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvDlcSongs.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    string dlcSongPath = row.Cells["colPath"].Value.ToString();
                    // double GetFileNameWithoutExtenstion is required to complete remove double extension
                    var dlcSongName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(dlcSongPath));
                    string setlistSongPath = Path.Combine(dlcDir, curSetlist, dlcSongName);

                    if (curSetlist.Contains("disabled"))
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
                        // if the move bombs then what follows is not done
                        dgvSetlistSongs.Rows.Add(false, "NA", row.Cells["colSongArtist"].Value, row.Cells["colSongTitle"].Value, row.Cells["colSongAlbum"].Value, row.Cells["colSongTuning"].Value, setlistSongPath);
                        int lastRowNdx = dgvSetlistSongs.Rows.Count - 1;
                        if (curSetlist.Contains("disabled"))
                            dgvSetlistSongs.Rows[lastRowNdx].Cells["colSetlistSongsEnabled"].Value = "No";
                        else
                            dgvSetlistSongs.Rows[lastRowNdx].Cells["colSetlistSongsEnabled"].Value = "Yes";

                        // update dgvDlcSongs
                        row.Cells["colEnabled"].Value = curSetlist.Contains("disabled") ? "No" : "Yes";
                        row.Cells["colPath"].Value = setlistSongPath;
                        row.Cells["colSelect"].Value = false;
                        row.DefaultCellStyle.BackColor = Color.Red;

                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to move song:" + Path.GetFileName(dlcSongPath) + @", to setlist: " + curSetlist + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            dgvDlcSongs.ClearSelection();
            dgvSetlistSongs.ClearSelection();

            // force a rescan for data safety on next entry
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
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
            foreach (DataGridViewRow row in dgvDlcSongs.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                {
                    var originalPath = row.Cells["colPath"].Value.ToString();

                    try
                    {
                        if (row.Cells["colEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledDLCPath = originalPath.Replace("_p.psarc", "_p.disabled.psarc");
                            File.Move(originalPath, disabledDLCPath);
                            row.Cells["colPath"].Value = disabledDLCPath;
                            row.Cells["colEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledDLCPath = originalPath.Replace("_p.disabled.psarc", "_p.psarc");
                            File.Move(originalPath, enabledDLCPath);
                            row.Cells["colPath"].Value = enabledDLCPath;
                            row.Cells["colEnabled"].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable song: " + Path.GetFileName(originalPath) + @" in 'dlc' folder." + Environment.NewLine + "Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnEnDiSetlistSong_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlistSongs.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value))
                {
                    var setlistSongPath = row.Cells["colSetlistSongsPath"].Value.ToString();
                    try
                    {
                        if (row.Cells["colSetlistSongsEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledSetlistSongsPath = setlistSongPath.Replace(".psarc", ".disabled.psarc");
                            File.Move(setlistSongPath, disabledSetlistSongsPath);
                            row.Cells["colSetlistSongsPath"].Value = disabledSetlistSongsPath;
                            row.Cells["colSetlistSongsEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledSetlistSongsPath = setlistSongPath.Replace(".disabled.psarc", ".psarc");
                            File.Move(setlistSongPath, enabledSetlistSongsPath);
                            row.Cells["colSetlistSongsPath"].Value = enabledSetlistSongsPath;
                            row.Cells["colSetlistSongsEnabled"].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable setlist song: " + Path.GetFileName(setlistSongPath) + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEnDiSetlist_Click(object sender, EventArgs e)
        {
            bool clearDgvSetlistSongs = false;

            foreach (DataGridViewRow row in dgvSetlists.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value))
                {
                    string setlistName = row.Cells["colSetlistName"].Value.ToString();
                    string setlistDirPath = Path.Combine(dlcDir, setlistName);

                    try
                    {
                        if (row.Cells["colSetlistEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledSetlistDirPath = String.Format("{0}_disabled", setlistDirPath);
                            Directory.Move(setlistDirPath, disabledSetlistDirPath);
                            row.Cells["colSetlistName"].Value = String.Format("{0}_disabled", setlistName);
                            row.Cells["colSetlistEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledSetlistDirPath = setlistDirPath.Replace("_disabled", "");
                            Directory.Move(setlistDirPath, enabledSetlistDirPath);
                            row.Cells["colSetlistName"].Value = setlistName.Replace("_disabled", "");
                            row.Cells["colSetlistEnabled"].Value = "Yes";
                        }

                        clearDgvSetlistSongs = true;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable setlist: " + setlistName + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (clearDgvSetlistSongs)
                dgvSetlistSongs.Rows.Clear();
        }

        private void btnEnDiSongPack_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSongPacks.Rows)
            {
                if (Convert.ToBoolean(row.Cells["colSongPackSelect"].Value))
                {
                    var songPackPath = row.Cells["colSongPackPath"].Value.ToString();
                    try
                    {
                        if (row.Cells["colSongPackEnabled"].Value.ToString() == "Yes")
                        {
                            var disabledSongPackPath = songPackPath.Replace(".psarc", ".disabled.psarc");
                            File.Move(songPackPath, disabledSongPackPath);
                            row.Cells["colSongPackPath"].Value = disabledSongPackPath;
                            row.Cells["colSongPackEnabled"].Value = "No";
                        }
                        else
                        {
                            var enabledSongPackPath = songPackPath.Replace(".disabled.psarc", ".psarc");
                            File.Move(songPackPath, enabledSongPackPath);
                            row.Cells["colSongPackPath"].Value = enabledSongPackPath;
                            row.Cells["colSongPackEnabled"].Value = "Yes";
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(@"Unable to enable/disable Song Pack: " + Path.GetFileName(songPackPath) + Environment.NewLine + "Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }

        private void btnRemoveSetlistSong_Click(object sender, EventArgs e)
        {
            bool safe2Delete = false;

            // remove rows from datagridview going backward to avoid index issues
            // no need to go Casting unless we are fishing
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

                    var dlcNdx = dgvDlcSongs.Rows.Cast<DataGridViewRow>()
                        .Where(r => r.Cells["colPath"].Value.ToString() == setlistSongPath)
                        .Select(r => r.Index).First();  // this will throw exception if not found

                    if (safe2Delete) // this is redundant for safety
                        try
                        {
                            // be really sure you want to do this
                            File.Delete(setlistSongPath);
                            // update dgvDlcSongs
                            dgvDlcSongs.Rows.RemoveAt(dlcNdx);
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
                            // update dgvDlcSongs
                            dgvDlcSongs.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                            dgvDlcSongs.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                            dgvDlcSongs.Rows[dlcNdx].Cells["colSelect"].Value = false;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show(@"Unable to move song from setlist:" + Environment.NewLine + Path.GetDirectoryName(setlistSongPath) + Environment.NewLine + @"Error: " + ex.Message, MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    dgvSetlistSongs.Rows.Remove(row);
                }
            }

            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.SkyBlue); // for testing will be yellow for production
        }

        private void btnRemoveSetlist_Click(object sender, System.EventArgs e)
        {
            // remove rows from datagridview going backward to avoid index issues
            for (int ndx = dgvSetlists.Rows.Count - 1; ndx >= 0; ndx--)
            {
                DataGridViewRow row = dgvSetlists.Rows[ndx];

                if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value))
                {
                    string setlistName = row.Cells["colSetlistName"].Value.ToString();

                    if (chkDeleteSetlistOrSetlistSongs.Checked)
                    {
                        // DANGER ZONE ... Confirm deletion for every setlist selected .. redundant safety interlock
                        if (MessageBox.Show("You are about to permanently delete setlist '" + setlistName + "'" + Environment.NewLine +
                                             "Including all songs contained in the setlist!" + Environment.NewLine + Environment.NewLine +
                                             "Are you sure you want to permanently delete setlist '" + setlistName + "' and its' songs?",
                                             MESSAGE_CAPTION + " ... Warning ... Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    // enumerate everything in setlist dirctory
                    string setlistPath = Path.Combine(dlcDir, setlistName);
                    var setlistSongsPath = Directory.EnumerateFiles(setlistPath, "*", SearchOption.AllDirectories);
                    bool safeDelete = true;

                    foreach (var setlistSongPath in setlistSongsPath)
                    {
                        // move setlist song back to dlc folder
                        var dlcSongPath = Path.Combine(dlcDir, Path.GetFileName(setlistSongPath));
                        try
                        {
                            File.Move(setlistSongPath, dlcSongPath);
                            var dlcNdx = dgvDlcSongs.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["colPath"].Value.ToString() == setlistSongPath).Select(r => r.Index).First();

                            // update dgvDlcSongs
                            if (chkDeleteSetlistOrSetlistSongs.Checked)
                                dgvDlcSongs.Rows.RemoveAt(dlcNdx);
                            else
                            {
                                dgvDlcSongs.Rows[dlcNdx].Cells["colPath"].Value = dlcSongPath;
                                dgvDlcSongs.Rows[dlcNdx].Cells["colEnabled"].Value = dlcSongPath.Contains("disabled") ? "No" : "Yes";
                                dgvDlcSongs.Rows[dlcNdx].Cells["colSelect"].Value = false;
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
                }
            }

            RemoveHighlightUsedSongs();
            HighlightUsedSongs(Color.Magenta); // for testing will be yellow for production
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
            foreach (DataGridViewRow row in dgvDlcSongs.Rows)
                if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                    row.Cells["colSelect"].Value = false;
                else
                    row.Cells["colSelect"].Value = true;
        }

        private void btnToggleSetlists_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlists.Rows)
                if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value))
                    row.Cells["colSetlistSelect"].Value = false;
                else
                    row.Cells["colSetlistSelect"].Value = true;
        }

        private void btnToggleSongsInSetlist_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvSetlistSongs.Rows)
                if (Convert.ToBoolean(row.Cells["colSetlistSongsSelect"].Value))
                    row.Cells["colSetlistSongsSelect"].Value = false;
                else
                    row.Cells["colSetlistSongsSelect"].Value = true;
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

            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
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

        private void cueDlcSongsSearch_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cueDlcSongsSearch.Text))
                return;

            string search = cueDlcSongsSearch.Text.ToLower();
            var matchingSongs = Globals.SongCollection.Where(sng => sng.Artist.ToLower().Contains(search) || sng.Song.ToLower().Contains(search) || sng.Tuning.ToLower().Contains(search) && Path.GetFileName(Path.GetDirectoryName(sng.Path)) == "dlc");
            dgvDlcSongs.Rows.Clear();
            dlcSongsSearch.Clear();
            dlcSongsSearch.AddRange(matchingSongs);
            BindingSource bs = new BindingSource { DataSource = dlcSongsSearch };
            dgvDlcSongs.DataSource = bs;
            DgvDlcSongsAppearance();

            LoadSetlistSongs(cueDlcSongsSearch.Text.ToLower());
        }

        private void dgvDlcSongs_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing color (cell formating) on initial loading
            if (!bindingCompleted)
                bindingCompleted = true;
        }

        private void dgvDlcSongs_Paint(object sender, PaintEventArgs e)
        {
            // wait for DataBinding and DataGridView Paint to complete before  
            // changing cell color (formating) on initial loading
            if (bindingCompleted && !dgvPainted)
            {
                bindingCompleted = false;
                dgvPainted = true;
                HighlightUsedSongs(Color.Yellow);
            }
        }

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cueDlcSongsSearch.Text))
                LoadSetlistSongs(cueDlcSongsSearch.Text);
            else
                LoadSetlistSongs();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PopulateSetlistManager();
            HighlightUsedSongs(Color.Yellow);
            cueDlcSongsSearch.Text = String.Empty;
            cueDlcSongsSearch.Cue = "Search";
        }

        private void lnkOpenSngMgrHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ensures proper disposal of objects and variables
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("CustomsForgeManager.Resources.SetlistHelp.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string songManagerHelp = reader.ReadToEnd();

                using (var noteViewer = new frmNoteViewer())
                {
                    noteViewer.PopulateText(songManagerHelp);
                    noteViewer.ShowDialog();
                }
            }
        }
    }
}

