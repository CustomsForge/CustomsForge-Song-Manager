using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;

namespace CustomsForgeManager.UControls
{
    public partial class Duplicates : UserControl
    {
        private BindingList<SongData> dupSongCollection = new BindingList<SongData>();
        private List<SongData> duplicates = new List<SongData>();

        public Duplicates()
        {
            InitializeComponent();
            btnMove.Click += DeleteMoveSelected;
            btnDeleteSong.Click += DeleteMoveSelected;
            PopulateDuplicates();
        }

        public void PopulateDuplicates()
        {
            Globals.Log("Populating Duplicates GUI ...");
            dgvDups.Visible = false;

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
            {
                MessageBox.Show("Duplicates need to be rescanned!", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // populate ArtistSongAlbum for finding duplicates
            foreach (var songData in Globals.SongCollection)
                songData.ArtistSongAlbum = String.Format("{0}{1}{2}", songData.Artist, songData.Song, songData.Album);

            dupSongCollection = Globals.SongCollection;
            // var dups = dupSongCollection.GroupBy(x => new { x.Song, x.Album, x.Artist }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            var dups = dupSongCollection.GroupBy(x => new { x.ArtistSongAlbum }).Where(group => group.Count() > 1).SelectMany(group => group).ToList();
            dups.RemoveAll(x => x.FileName.Contains(Constants.RS1COMP));
            duplicates.Clear();
            duplicates.AddRange(dups);
            Globals.DupeCollection = duplicates;

            // must use binding source here
            BindingSource bs = new BindingSource { DataSource = duplicates };
            dgvDups.DataSource = bs;

            // update datagrid appearance
            DgvDupsAppearance();

            // set custom selection (highlighting) color
            dgvDups.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            dgvDups.DefaultCellStyle.SelectionForeColor = dgvDups.DefaultCellStyle.ForeColor;
            dgvDups.ClearSelection();
            Globals.ReloadDuplicates = false;
        }

        public void UpdateToolStrip()
        {
            if (Globals.RescanDuplicates)
            {
                Globals.RescanDuplicates = false;
                Rescan();
            }

            if (Globals.ReloadDuplicates)
            {
                Globals.ReloadDuplicates = false;
                PopulateDuplicates();
            }

            Globals.TsLabel_MainMsg.Text = string.Format("Rocksmith Songs Count: {0}", Globals.SongCollection.Count);
            Globals.TsLabel_MainMsg.Visible = true;
            Globals.TsLabel_DisabledCounter.Alignment = ToolStripItemAlignment.Right;
            Globals.TsLabel_DisabledCounter.Text = String.Format("Duplicates Count: {0}", dgvDups.Rows.Count);
            Globals.TsLabel_DisabledCounter.Visible = true;
        }

        private void DgvDupsAppearance() // overrides Duplicates.Desinger.cs
        {
            foreach (DataGridViewColumn col in dgvDups.Columns)
            {
                col.ReadOnly = true;
                col.Visible = false;
            }

            // always visible and first
            dgvDups.Columns["colSelect"].ReadOnly = false;
            dgvDups.Columns["colSelect"].Visible = true;
            dgvDups.Columns["colSelect"].Width = 43;
            dgvDups.Columns["colSelect"].DisplayIndex = 0;
            dgvDups.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDups.Columns["colEnabled"].Visible = true;
            dgvDups.Columns["colEnabled"].Width = 50;
            dgvDups.Columns["colSongArtist"].Visible = true;
            dgvDups.Columns["colSongArtist"].Width = 140;
            dgvDups.Columns["colSongTitle"].Visible = true;
            dgvDups.Columns["colSongTitle"].Width = 140;
            dgvDups.Columns["colSongAlbum"].Visible = true;
            dgvDups.Columns["colSongAlbum"].Width = 200;
            dgvDups.Columns["colUpdated"].Visible = true;
            dgvDups.Columns["colUpdated"].Width = 100;
            dgvDups.Columns["colPath"].Visible = true;
            dgvDups.Columns["colPath"].Width = 305;

            dgvDups.Visible = true; // must come first for setting to apply correctly
            dgvDups.AllowUserToAddRows = false; // removes empty row at bottom
            dgvDups.AllowUserToDeleteRows = false;
            dgvDups.AllowUserToOrderColumns = true;
            dgvDups.AllowUserToResizeColumns = true;
            dgvDups.AllowUserToResizeRows = true;
            // dgvDups.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            dgvDups.BackgroundColor = SystemColors.AppWorkspace;
            dgvDups.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDups.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDups.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDups.EnableHeadersVisualStyles = true;
            dgvDups.Font = new Font("Arial", 8);
            dgvDups.GridColor = SystemColors.ActiveCaption;
            dgvDups.MultiSelect = false;
            dgvDups.Name = "dgvDups";
            dgvDups.RowHeadersVisible = false; // remove row arrow
            dgvDups.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvDups.ClearSelection();
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
                worker.BackgroundScan(this);
                while (Globals.WorkerFinished == Globals.Tristate.False)
                    Application.DoEvents();
            }

            if (Globals.WorkerFinished == Globals.Tristate.Cancelled)
                return;

            PopulateDuplicates();
            Globals.RescanSetlistManager = false;
            Globals.RescanDuplicates = false;
            Globals.RescanSongManager = false;
            Globals.RescanRenamer = false;
            Globals.ReloadDuplicates = false;
            Globals.ReloadSongManager = true;
            Globals.ReloadRenamer = true;
            Globals.ReloadSetlistManager = true;
        }

        private void ShowSongInfo()
        {
            if (dgvDups.SelectedRows.Count > 0)
            {
                var selectedRow = dgvDups.SelectedRows[0];
                var title = selectedRow.Cells["colSongTitle"].Value.ToString();
                var artist = selectedRow.Cells["colSongArtist"].Value.ToString();
                var album = selectedRow.Cells["colSongAlbum"].Value.ToString();
                var path = selectedRow.Cells["colPath"].Value.ToString();

                var song = dupSongCollection.FirstOrDefault(x => x.Song == title && x.Album == album && x.Artist == artist && x.Path == path);
                if (song != null)
                {
                    frmSongInfo infoWindow = new frmSongInfo(song);
                    infoWindow.Show();
                }
            }
            else
                MessageBox.Show("Please select (highlight) the song that  " + Environment.NewLine + "you would like information about.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteMoveSelected(object sender, System.EventArgs e)
        {
            if (dgvDups.Rows.Count == 0)
                return;

            var count = dgvDups.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToBoolean(row.Cells["colSelect"].Value));
            if (count == 0)
            {
                MessageBox.Show("Please select the checkbox next to songs  " + Environment.NewLine +
                                "that you would like to delete or move.", Constants.ApplicationName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var buttonName = (Button)sender;

            if (buttonName.Text.ToLower().Contains("delete"))
                if (MessageBox.Show("Do you really want to delete the selected duplicate CDLC?  " + Environment.NewLine + "Warning:  This can not be undone!", Constants.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

            string duplicatesDir = Path.Combine(Globals.MySettings.RSInstalledDir, "duplicates");

            if (!Directory.Exists(duplicatesDir))
                Directory.CreateDirectory(duplicatesDir);

            for (int i = 0; i < dgvDups.Rows.Count; i++)
            {
                if (dgvDups.Rows[i].Cells["colSelect"].Value == null)
                    dgvDups.Rows[i].Cells["colSelect"].Value = false;

                if (Convert.ToBoolean(dgvDups.Rows[i].Cells["colSelect"].Value))
                {
                    try
                    {
                        if (File.Exists(duplicates[i].Path))//In case that the user manually (re)moved the song & hasn't rescanned since he did that 
                        {
                            if (buttonName.Text.ToLower().Contains("move"))
                            {
                                var filePath = duplicates[i].Path;
                                var dupFileName = String.Format("{0}{1}", Path.GetFileName(filePath), ".dup");
                                var dupFilePath = Path.Combine(duplicatesDir, dupFileName);
                                File.Move(duplicates[i].Path, dupFilePath);
                            }
                            else
                                File.Delete(duplicates[i].Path);
                        }
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Globals.Log(ex.Message);
                    }
                }
            }

            // safely remove rows now that file(s) are deleted/moved 
            for (int ndx = 0; ndx < dgvDups.Rows.Count; ++ndx)
                if (Convert.ToBoolean(dgvDups.Rows[ndx].Cells["colSelect"].Value))
                {
                    dgvDups.Rows.RemoveAt(ndx);
                    ndx--;
                }

            if (dgvDups.Rows.Count == 1)
                dgvDups.Rows.Clear();

            UpdateToolStrip();
            Globals.RescanSongManager = true;
            Globals.RescanDuplicates = true;
            Globals.RescanSetlistManager = true;
        }


        private void btnEnableDisable_Click(object sender, EventArgs e)
        {
            bool updateSongCollection = false;

            try
            {
                foreach (DataGridViewRow row in dgvDups.Rows)
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
                        }
                        else
                            Globals.Log("This is a Rocksmith 1 song. It can't be disabled at this moment. (You just can disable all of them!)");
                    }
                }
            }
            catch (IOException)
            {
                Globals.Log("<ERROR>: Unable to disable the song(s)!");
            }

            if (updateSongCollection)
            {
                UpdateToolStrip();
                Globals.RescanSongManager = true;
                Globals.RescanSetlistManager = true;
                Globals.RescanDuplicates = true;
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            Rescan();
        }

        private void dgvDups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // triggered by any key
            if (e.RowIndex != -1)
                ShowSongInfo();
        }

        private void dgvDups_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // has precedent over a ColumnHeader_MouseClick
            var rowIndex = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                // fancy way to decide when context menu pops up
                dgvDups.ContextMenuStrip.Opening += (s, i) =>
                    {
                        if (rowIndex != -1)
                        {
                            dgvDups.Rows[rowIndex].Selected = true;
                            i.Cancel = false; // resets e.RowIndex
                            cmsDuplicate.Show(Cursor.Position);
                        }
                        else
                            i.Cancel = true;
                    };
            }
        }

        private void dgvDups_KeyDown(object sender, KeyEventArgs e)
        {
            // space bar used to select a song (w/ checkbox "Select")
            if (e.KeyCode == Keys.Space)
                foreach (DataGridViewRow row in dgvDups.Rows)
                    if (row.Selected)
                    {
                        if (row.Cells["colSelect"].Value == null)
                            row.Cells["colSelect"].Value = false;

                        if (Convert.ToBoolean(row.Cells["colSelect"].Value))
                            row.Cells["colSelect"].Value = false;
                        else
                            row.Cells["colSelect"].Value = true;
                    }
        }

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var filePath = duplicates[dgvDups.SelectedRows[0].Index].Path;

            if (File.Exists(filePath))
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
        }


    }
}
