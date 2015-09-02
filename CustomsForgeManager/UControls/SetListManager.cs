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
using System.Diagnostics;

namespace CustomsForgeManager.UControls
{
    public partial class SetListManager : UserControl
    {
        public SetListManager()
        {
            InitializeComponent();
            PopulateSetListManager();
        }

        public void PopulateSetListManager()
        {
            Globals.Log("Populating SetlistManager GUI ...");
            LoadSetlists();
        }

        #region Misc functions

        private bool ValidateRsDir()
        {
            if (Globals.MySettings.RSInstalledDir == "")
            {
                MessageBox.Show("Please fill RS path textbox!", "RS path empty!");
                return false;
            }
            else
            {
                if (!Directory.Exists(Globals.MySettings.RSInstalledDir))
                {
                    MessageBox.Show("Please fix RS path!", "RS Folder doesn't exist!");
                    return false;
                }
            }
            return true;
        }
        public bool SetlistEnabled(string setlistName)
        {
            string setlistPath = "";
            if (setlistName.Contains(Globals.MySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);

            var songsInSetlist = Directory.EnumerateFiles(setlistPath, "*_p.*psarc", SearchOption.AllDirectories);

            if (songsInSetlist.Where(sng => sng.Contains(".disabled")).Count() == songsInSetlist.Count())
                return false;
            else
                return true;
        }

        public bool SetlistModified(string setlistName)
        {
            string setlistPath = "";
            if (setlistName.Contains(Globals.MySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);

            var disabledCount = Directory.EnumerateFiles(setlistPath, "*_p.*psarc", SearchOption.AllDirectories).Where(sng => sng.Contains(".disabled")).Count();

            if (disabledCount > 0)
                return true;
            else
                return false;
        }

        public bool SetlistContainsSong(string setlistName, string songName)
        {
            string setlistPath = "";
            if (setlistName.Contains(Globals.MySettings.RSInstalledDir))
                setlistPath = setlistName;
            else
                setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);

            var songsInSetlist = Directory.EnumerateFiles(setlistPath, "*_p.*psarc");

            if (songsInSetlist.Where(sng => sng.ToLower().Contains(songName.ToLower())).Count() > 0 || setlistName.ToLower().Contains(songName.ToLower()))
                return true;
            else
                return false;
        }


        public void RefreshSelectedSongs(string search = "")
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                {
                    dgvDLCsInSetlist.Rows.Clear();

                    if (dgvSetlists.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvSetlists.SelectedRows)
                        {
                            string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());

                            if (Directory.Exists(setlistPath))
                            {
                                var matchingSongs = Globals.SongCollection.Where(sng => (sng.Artist.ToLower().Contains(search) || sng.Album.ToLower().Contains(search) || sng.Song.ToLower().Contains(search) || sng.Path.ToLower().Contains(search)) && sng.Path.Contains(setlistPath)).ToList();
                                foreach (SongData song in matchingSongs)
                                {
                                    dgvDLCsInSetlist.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Tuning, song.Path);
                                }
                            }
                        }
                    }
                });
            };

            bWorker.RunWorkerAsync();
        }
        #endregion
        #region Setlists
        private void btnCreateNewSetlist_Click(object sender, System.EventArgs e)
        {
            string setlistName = Microsoft.VisualBasic.Interaction.InputBox("Please enter setlist name", "Setlist name");
            try
            {
                if (ValidateRsDir())
                {
                    string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);
                    if (!Directory.Exists(setlistPath))
                    {
                        Directory.CreateDirectory(setlistPath);
                        if (Directory.Exists(setlistPath))
                            dgvSetlists.Rows.Add(false, "Yes", setlistName);
                    }
                    else
                        MessageBox.Show("Setlist with this name already exists!", "Setlist already exists");
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to create a new setlist! Error: \n\n" + ex.ToString());
            }
        }
        private void btnDeleteSelectedSetlist_Click(object sender, System.EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                try
                {
                    foreach (DataGridViewRow row in dgvSetlists.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                        {
                            if (ValidateRsDir())
                            {
                                string setlistName = row.Cells["colSetlist"].Value.ToString();
                                string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);

                                if (Directory.Exists(setlistPath))
                                {
                                    string[] songs = Directory.GetFiles(setlistPath);

                                    if (checkDeleteSongsAndSetlists.Checked)
                                    {
                                        var confirmResult = MessageBox.Show("Are you sure you want to permanently delete this setlist (" + setlistName + ") and all songs it contains?", "Permanently delete setlist?", MessageBoxButtons.YesNo);
                                        if (confirmResult == DialogResult.Yes)
                                        {
                                            foreach (string song in songs)
                                            {
                                                var songToBeDeleted = Globals.SongCollection.FirstOrDefault(sng => sng.Path == song);
                                                Globals.SongCollection.Remove(songToBeDeleted);
                                            }
                                            Directory.Delete(setlistPath, true);
                                        }
                                    }
                                    else
                                    {
                                        foreach (string songPath in songs)
                                        {
                                            var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                            bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                            if (songPath.Contains("dlc"))
                                            {
                                                string finalSongPath = songPath.Replace(row.Cells["colSetlist"].Value.ToString(), "").Replace("_p.disabled.psarc", "_p.psarc");

                                                if (File.Exists(finalSongPath))
                                                {
                                                    var songDupe = Globals.SongCollection.FirstOrDefault(sng => sng.Path == finalSongPath);

                                                    File.Delete(finalSongPath);

                                                    Globals.SongCollection.Remove(songDupe);
                                                }

                                                if (File.Exists(finalSongPath + ".disabled"))
                                                {
                                                    var songDupe = Globals.SongCollection.FirstOrDefault(sng => sng.Path == (finalSongPath + ".disabled"));

                                                    File.Delete(finalSongPath + ".disabled");

                                                    Globals.SongCollection.Remove(songDupe);
                                                }

                                                if (File.Exists(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc")))
                                                {
                                                    var songDupe = Globals.SongCollection.FirstOrDefault(sng => sng.Path == finalSongPath);

                                                    File.Delete(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc"));

                                                    Globals.SongCollection.Remove(songDupe);
                                                }

                                                File.Move(songPath, finalSongPath);

                                                if (tagged)
                                                    File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                                song.Path = finalSongPath;
                                            }

                                            Extensions.InvokeIfRequired(dgvSetlists, delegate
                                            {
                                                dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Tuning, song.Path);
                                            });
                                        }

                                        Directory.Delete(setlistPath, true);
                                        Extensions.InvokeIfRequired(dgvSetlists, delegate
                                        {
                                            dgvUnsortedDLCs.Sort(dgvUnsortedDLCs.Columns["colUnsortedSong"], ListSortDirection.Ascending);
                                        });
                                    }
                                    Extensions.InvokeIfRequired(dgvSetlists, delegate
                                    {
                                        dgvSetlists.Rows.Remove(row);
                                    });
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to delete the song archive! \n" + ex.ToString(), "IO Error");
                }
            };
            bWorker.RunWorkerAsync();
        }

        private void btnRemoveSongsFromSetlist_Click(object sender, EventArgs e)
        {
            bool permaDelete = false;
            try
            {
                if (ValidateRsDir())
                {
                    foreach (DataGridViewRow row in dgvDLCsInSetlist.Rows)
                    {
                        if (row.Selected || Convert.ToBoolean(row.Cells["colDLCSelect"].Value) == true)
                        {
                            if (checkDeleteSongsAndSetlists.Checked)
                            {
                                var confirmResult = MessageBox.Show("Are you sure you want to permanently delete all selected songs?", "Permanently delete songs?", MessageBoxButtons.YesNo);
                                if (confirmResult == DialogResult.Yes)
                                    permaDelete = true;
                            }

                            string dlcFolderPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");
                            string setlistSongPath = Path.Combine(dlcFolderPath, dgvSetlists.SelectedRows[0].Cells["colSetlist"].Value.ToString());
                            string songPath = row.Cells["colDLCPath"].Value.ToString();
                            string finalSongPath = Path.Combine(dlcFolderPath, Path.GetFileName(songPath)).Replace("_p.disabled.psarc", "_p.psarc");

                            var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                            bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                            if (permaDelete || songPath.Contains("rs1comp") || songPath.Contains("cache.psarc"))
                            {
                                File.Delete(songPath);
                                Globals.SongCollection.Remove(song);
                                dgvDLCsInSetlist.Rows.Remove(row);
                            }
                            else if (!checkDeleteSongsAndSetlists.Checked)
                            {
                                if (File.Exists(finalSongPath))
                                    File.Delete(finalSongPath);

                                if (File.Exists(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc")))
                                    File.Delete(finalSongPath.Replace("_p.psarc", "_p.disabled.psarc"));

                                if (File.Exists(finalSongPath + ".disabled"))
                                    File.Delete(finalSongPath + ".disabled");

                                File.Move(songPath, finalSongPath);

                                if (tagged)
                                    File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Album, song.Tuning, finalSongPath);
                                dgvUnsortedDLCs.Sort(dgvUnsortedDLCs.Columns["colUnsortedArtist"], ListSortDirection.Ascending);

                                dgvDLCsInSetlist.Rows.Remove(row);

                                song.Path = finalSongPath;
                                song.Enabled = "Yes";
                            }
                            else if (permaDelete == false)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n\n" + ex.ToString(), "IO Error");
            }
        }

        private void btnEnableAllSetlists_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += (se, ev) =>
            {
                try
                {
                    if (ValidateRsDir())
                    {
                        foreach (DataGridViewRow row in dgvSetlists.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                            {
                                string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                string finalSongPath = "";
                                List<string> setlistDisabledSongs = Directory.GetFiles(setlistPath).Where(sng => sng.Contains(".disabled")).ToList();

                                foreach (string songPath in setlistDisabledSongs)
                                {
                                    var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                    bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);
                                    finalSongPath = songPath.Replace("_p.psarc", "_p.disabled.psarc");

                                    File.Move(songPath, finalSongPath);

                                    if (tagged)
                                        File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                    song.Path = finalSongPath;
                                    song.Enabled = "Yes";

                                    Extensions.InvokeIfRequired(dgvSetlists, delegate
                                    {
                                        row.Cells["colSetlistEnabled"].Value = "Yes";
                                        row.DefaultCellStyle.BackColor = Color.White;
                                    });
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to enable all setlists! Error: \n\n" + ex.ToString());
                }
            };
            bWorker.RunWorkerAsync();
        }

        private void btnEnableDisableSelectedSM_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                {
                    if (ValidateRsDir())
                    {
                        if (radioEnableDisableSetlists.Checked)
                        {
                            foreach (DataGridViewRow row in dgvSetlists.Rows)
                            {
                                dgvDLCsInSetlist.Rows.Clear();
                                if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                                {
                                    string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                    string setlistEnabledValue = row.Cells["colSetlistEnabled"].Value.ToString();
                                    bool setlistEnabled = setlistEnabledValue == "Yes" || setlistEnabledValue == "Modded" ? true : false;

                                    foreach (string songPath in Directory.GetFiles(setlistPath))
                                    {
                                        var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                        string finalSongPath = setlistEnabled ? song.Path.Replace("_p.psarc", "_p.disabled.psarc") : song.Path.Replace("_p.disabled.psarc", "_p.psarc");
                                        string songEnabled = setlistEnabled ? "No" : "Yes";
                                        bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                        File.Move(song.Path, finalSongPath);

                                        if (tagged)
                                            File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                        song.Path = finalSongPath;
                                        song.Enabled = songEnabled;

                                        dgvDLCsInSetlist.Rows.Add(false, songEnabled, song.Artist, song.Song, song.Album, song.Tuning, Path.GetFileName(finalSongPath));
                                    }

                                    if (setlistEnabled)
                                    {
                                        row.Cells["colSetlistEnabled"].Value = "No";
                                        row.DefaultCellStyle.BackColor = Color.LightGray;
                                    }
                                    else
                                    {
                                        row.Cells["colSetlistEnabled"].Value = "Yes";
                                        row.DefaultCellStyle.BackColor = Color.White;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (DataGridViewRow selectedSong in dgvDLCsInSetlist.SelectedRows)
                            {
                                foreach (DataGridViewRow row in dgvSetlists.SelectedRows)
                                {
                                    string setlistName = row.Cells["colSetlist"].Value.ToString();
                                    string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", setlistName);
                                    string songPath = selectedSong.Cells["colDLCPath"].Value.ToString();

                                    if (SetlistContainsSong(setlistName, songPath))
                                    {
                                        var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                        string finalSongPath = songPath;

                                        bool enabled = !songPath.Contains("_p.disabled.psarc") ? true : false;
                                        bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                        if (!enabled)
                                            finalSongPath = finalSongPath.Replace("_p.disabled.psarc", "_p.psarc");
                                        else
                                            finalSongPath = finalSongPath.Replace("_p.psarc", "_p.disabled.psarc");

                                        File.Move(songPath, finalSongPath);

                                        song.Path = finalSongPath;
                                        song.Enabled = !enabled ? "Yes" : "No";

                                        selectedSong.Cells["colDLCPath"].Value = finalSongPath;
                                        selectedSong.Cells["colDLCEnabled"].Value = !enabled ? "Yes" : "No";

                                        if (tagged)
                                            File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                        if (!SetlistEnabled(setlistName))
                                            row.Cells["colSetlistEnabled"].Value = "No";
                                        else if (SetlistModified(setlistName))
                                            row.Cells["colSetlistEnabled"].Value = "Modded";
                                        else
                                            row.Cells["colSetlistEnabled"].Value = "Yes";
                                    }
                                }
                            }
                        }
                    }
                });
            };
            bWorker.RunWorkerAsync();
        }
        #endregion
        #region Unsorted songs
        private void tbUnsortedSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbUnsortedSearch.Text != "Search")
            {
                string search = tbUnsortedSearch.Text.ToLower();

                dgvUnsortedDLCs.Rows.Clear();
                var matchingSongs = Globals.SongCollection.Where(sng => sng.Artist.ToLower().Contains(search) || sng.Song.ToLower().Contains(search) || sng.Tuning.ToLower().Contains(search)
                    && Path.GetFileName(Path.GetDirectoryName(sng.Path)) == "dlc");

                if (matchingSongs.Where(sng => sng.Path.Contains(".disabled")).Count() > matchingSongs.Where(sng => !sng.Path.Contains(".disabled")).Count())
                {
                    foreach (var song in matchingSongs)
                    {
                        if (song.Path.Contains(".disabled"))
                            dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Tuning, song.Path);
                        else
                            dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Tuning, song.Path);
                    }
                }
                else
                {
                    foreach (var song in matchingSongs)
                    {
                        if (!song.Path.Contains(".disabled"))
                            dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Tuning, song.Path);
                        else
                            dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Tuning, song.Path);
                    }
                }

                foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colUnsortedEnabled"].Value.ToString() == "No"))
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }

                if (checkSearchInAllSetlists.Checked)
                {
                    dgvSetlists.Rows.Clear();
                    string dlcFolderPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");

                    var dirs = Directory.EnumerateDirectories(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*", SearchOption.TopDirectoryOnly);
                    foreach (var setlist in dirs)
                    {
                        if (SetlistContainsSong(setlist, tbUnsortedSearch.Text.ToLower()))
                        {
                            if (!SetlistEnabled(setlist))
                                dgvSetlists.Rows.Add(false, "No", Path.GetFileName(setlist.Replace("-disabled", "")));
                            else if (SetlistModified(setlist))
                                dgvSetlists.Rows.Add(false, "Modded", Path.GetFileName(setlist));
                            else
                                dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlist));
                        }
                    }

                    if (dgvSetlists.Rows.Count > 0)
                        dgvSetlists.Rows[0].Selected = true;

                    RefreshSelectedSongs(tbUnsortedSearch.Text.ToLower());
                }
            }
        }
        private void btnMoveSongToSetlist_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                try
                {
                    if (ValidateRsDir())
                    {
                        foreach (DataGridViewRow row in dgvSetlists.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                            {
                                foreach (DataGridViewRow unsortedRow in dgvUnsortedDLCs.Rows)
                                {
                                    if (unsortedRow.Selected || (bool)unsortedRow.Cells["colUnsortedSelect"].Value == true)
                                    {
                                        string songPath = unsortedRow.Cells["colUnsortedPath"].Value.ToString();
                                        string setlistPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                        string finalSongPath = "";

                                        string setlistEnabledValue = row.Cells["colSetlistEnabled"].Value.ToString();
                                        bool setlistEnabled = setlistEnabledValue == "Yes" || setlistEnabledValue == "Modded" ? true : false;

                                        bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);
                                        var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                        if (setlistEnabled)
                                            finalSongPath = Path.Combine(setlistPath, Path.GetFileName(songPath.Replace(".disabled", "")));
                                        else
                                        {
                                            finalSongPath = Path.Combine(setlistPath, Path.GetFileName(songPath));

                                            if (unsortedRow.Cells["colUnsortedEnabled"].Value.ToString() != "No")
                                                finalSongPath = finalSongPath.Replace("_p.psarc", "_p.disabled.psarc");
                                        }

                                        if (File.Exists(finalSongPath))
                                            File.Delete(finalSongPath);
                                        File.Move(songPath, finalSongPath);

                                        if (tagged)
                                            File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                        Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                                        {
                                            dgvDLCsInSetlist.Rows.Add(false, setlistEnabled ? "Yes" : "No", song.Artist, song.Song, song.Album, song.Tuning, finalSongPath);
                                        });

                                        song.Path = finalSongPath;
                                        song.Enabled = setlistEnabled ? "Yes" : "No";
                                        rowsToDelete.Add(unsortedRow);
                                    }
                                }
                            }
                            //   row.Selected = false;
                            //   row.Cells["colSelect"].Value = false;
                        }
                        Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate
                        {
                            foreach (DataGridViewRow row in rowsToDelete)
                                dgvUnsortedDLCs.Rows.Remove(row);
                        });
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to move the song(s) to the setlist! \n" + ex.ToString(), "IO Error");
                }
            };
            bWorker.RunWorkerAsync();
        }

        private void btnEnableDisableSelectedSongs_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                try
                {
                    if (ValidateRsDir())
                    {
                        foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["colUnsortedSelect"].Value) == true || row.Selected)
                            {
                                string songPath = row.Cells["colUnsortedPath"].Value.ToString();
                                string finalSongPath = "";
                                var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                bool tagged = File.GetCreationTime(songPath) == new DateTime(1990, 1, 1);

                                if (row.Cells["colUnsortedEnabled"].Value.ToString() == "No")
                                {
                                    finalSongPath = songPath.Replace("_p.disabled.psarc", "_p.psarc");

                                    File.Move(songPath, finalSongPath);

                                    row.Cells["colUnsortedEnabled"].Value = "Yes";
                                    row.DefaultCellStyle.BackColor = Color.White;
                                }
                                else
                                {
                                    finalSongPath = songPath.Replace("_p.psarc", "_p.disabled.psarc");

                                    File.Move(songPath, finalSongPath);

                                    row.Cells["colUnsortedEnabled"].Value = "No";
                                    row.DefaultCellStyle.BackColor = Color.LightGray;
                                }

                                if (tagged)
                                    File.SetCreationTime(finalSongPath, new DateTime(1990, 1, 1));

                                song.Path = finalSongPath;
                                song.Enabled = song.Enabled == "Yes" ? "No" : "Yes";
                                row.Cells["colUnsortedPath"].Value = finalSongPath;
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to disalbe/enable selected songs! Error: \n\n" + ex.ToString());
                }
            };
            bWorker.RunWorkerAsync();
        }

        private void btnDeleteSelectedSongs_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                try
                {
                    if (ValidateRsDir())
                    {
                        var confirmResult = MessageBox.Show("Are you sure you want to permanently delete all selected songs?", "Permanently delete songs?", MessageBoxButtons.YesNo);
                        if (confirmResult == DialogResult.Yes)
                        {
                            foreach (DataGridViewRow row in dgvUnsortedDLCs.Rows)
                            {
                                if (Convert.ToBoolean(row.Cells["colUnsortedSelect"].Value) == true || row.Selected)
                                {
                                    string songPath = row.Cells["colUnsortedPath"].Value.ToString();
                                    if (!songPath.Contains("rs1comp"))
                                    {
                                        var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);

                                        File.Delete(songPath);

                                        if (!File.Exists(songPath))
                                            rowsToDelete.Add(row);

                                        // Extensions.InvokeIfRequired(dgvSongs, delegate { });
                                        Globals.SongCollection.Remove(song);
                                    }
                                }
                            }
                        }
                        Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate
                        {
                            foreach (DataGridViewRow dataGridViewRow in rowsToDelete)
                                dgvUnsortedDLCs.Rows.Remove(dataGridViewRow);
                        });
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to delete selected song(s)! Error: \n\n" + ex.ToString());
                }
            };
            bWorker.RunWorkerAsync();
        }
        #endregion
        #region Official song packs
        private void btnEnblDisblOfficialSongPack_Click(object sender, EventArgs e)
        {
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                if (ValidateRsDir())
                {
                    foreach (DataGridViewRow row in dgvOfficialSongs.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colOfficialSelect"].Value) == true || row.Selected)
                        {
                            if (Globals.MySettings.RSInstalledDir != "" && !row.Cells["colOfficialSongPack"].Value.ToString().Contains("cache"))
                            {
                                string currentSongPackPath = "";
                                string finalSongPackPath = "";
                                string currentSPFileName = row.Cells["colOfficialSongPack"].Value.ToString();

                                if (row.Cells["colOfficialEnabled"].Value == "Yes")
                                {
                                    if (currentSPFileName.Contains("cache"))
                                        currentSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, currentSPFileName);
                                    else
                                        currentSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", currentSPFileName);

                                    finalSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", currentSPFileName.Replace("_p.psarc", "_p.disabled.psarc"));
                                }
                                else
                                {
                                    if (currentSPFileName.Contains("cache"))
                                        finalSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, currentSPFileName);
                                    else
                                        finalSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", currentSPFileName);

                                    currentSongPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", currentSPFileName.Replace("_p.psarc", "_p.disabled.psarc"));
                                }

                                try
                                {
                                    File.Move(currentSongPackPath, finalSongPackPath);
                                }
                                catch (IOException ex)
                                {
                                    MessageBox.Show("Unable to disable the offical song pack(s)! Error: \n\n" + ex.ToString());
                                }

                                if (row.Cells["colOfficialEnabled"].Value == "Yes")
                                {
                                    row.Cells["colOfficialEnabled"].Value = "No";
                                    row.DefaultCellStyle.BackColor = Color.LightGray;
                                    row.Selected = false;
                                    row.Cells["colOfficialSelect"].Value = false;
                                }
                                else
                                {
                                    row.Cells["colOfficialEnabled"].Value = "Yes";
                                    row.DefaultCellStyle.BackColor = Color.White;
                                    row.Selected = false;
                                    row.Cells["colOfficialSelect"].Value = false;
                                }
                            }
                        }
                    }
                }
            };
        }

        private void btnSngPackToSetlist_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRsDir())
                {
                    foreach (DataGridViewRow row in dgvSetlists.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["colSetlistSelect"].Value) == true || row.Selected)
                        {
                            foreach (DataGridViewRow officialSPRow in dgvOfficialSongs.Rows)
                            {
                                if (officialSPRow.Selected || (bool)officialSPRow.Cells["colOfficialSelect"].Value == true)
                                {
                                    string songPack = officialSPRow.Cells["colOfficialSongPack"].Value.ToString();
                                    string songPackPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", songPack);
                                    string setlistSongPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", row.Cells["colSetlist"].Value.ToString());
                                    string finalSongPackPath = Path.Combine(setlistSongPath, songPack);

                                    if (officialSPRow.Cells["colOfficialEnabled"].Value == "Yes")
                                    {
                                        if (File.Exists(finalSongPackPath))
                                            File.Delete(finalSongPackPath);
                                        File.Copy(songPackPath, finalSongPackPath);
                                    }
                                    else
                                    {
                                        finalSongPackPath = finalSongPackPath.Replace("_p.psarc", "_p.disabled.psarc");
                                        songPackPath = songPackPath.Replace("_p.psarc", "_p.disabled.psarc");

                                        if (File.Exists(finalSongPackPath))
                                            File.Delete(finalSongPackPath);
                                        File.Copy(songPackPath, finalSongPackPath);
                                    }

                                    dgvOfficialSongs.Rows.Remove(officialSPRow);

                                    var song = new SongData { Song = songPack.Replace(".psarc", ""), Artist = "Ubisoft", Author = "Ubisoft", Path = finalSongPackPath };
                                    Globals.SongCollection.Add(song);
                                }
                            }
                        }
                        //   row.Selected = false;
                        //   row.Cells["colSelect"].Value = false;
                    }
                }
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show("Unable to move the song(s) to the setlist! \n" + ex.ToString(), "IO Error");
            }
        }

        private void btnRestoreOfficialsBackup_Click(object sender, EventArgs e)
        {
            if (ValidateRsDir())
            {
                string backupPath = Path.Combine(Globals.MySettings.RSInstalledDir, "backup");
                string dlcPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");

                string rs1BackupPath = Path.Combine(backupPath, "rs1compatibilitydisc_p.psarc.backup");
                string rs1DLCBackupPath = Path.Combine(backupPath, "rs1compatibilitydlc_p.psarc.backup");
                string rs2014BackupPath = Path.Combine(backupPath, "cache.psarc.backup");

                string rs1FinalPath = Path.Combine(dlcPath, "rs1compatibilitydisc_p.psarc");
                string rs1DLCFinalPath = Path.Combine(dlcPath, "rs1compatibilitydlc_p.psarc");
                string rs2014FinalPath = Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc");

                try
                {
                    if (File.Exists(rs1BackupPath))
                    {
                        if (File.Exists(rs1FinalPath.Replace("_p.psarc", "_p.disabled.psarc")))
                            File.Delete(rs1FinalPath.Replace("_p.psarc", "_p.disabled.psarc"));

                        File.Copy(rs1BackupPath, rs1FinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("disc")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                    }
                    if (File.Exists(rs1DLCBackupPath))
                    {
                        if (File.Exists(rs1DLCFinalPath.Replace("_p.psarc", "_p.disabled.psarc")))
                            File.Delete(rs1DLCFinalPath.Replace("_p.psarc", "_p.disabled.psarc"));

                        File.Copy(rs1DLCBackupPath, rs1DLCFinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("dlc")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydlc_p.psarc");
                    }
                    if (File.Exists(rs2014BackupPath))
                    {
                        File.Copy(rs2014BackupPath, rs2014FinalPath, true);
                        if (dgvOfficialSongs.Rows.Cast<DataGridViewRow>().Where(row => row.Cells["colOfficialSong"].Value.ToString().Contains("cache")).ToList().Count == 0)
                            dgvOfficialSongs.Rows.Add(false, "Yes", "cache.psarc");
                    }
                    if (File.Exists(rs1FinalPath) || File.Exists(rs1DLCFinalPath) || File.Exists(rs2014FinalPath))
                        MessageBox.Show("Backup restored!", "Done");
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Unable to restore backup! Error:\n\n" + ex.Message.ToString(), "Error");
                }

                foreach (DataGridViewRow row in dgvOfficialSongs.Rows)
                    row.Cells["colOfficialEnabled"].Value = "Yes";

                MessageBox.Show("Official songs zipped!");
            }
        }
        #endregion
        #region Other setlist mgr. stuff
        public void LoadSetlists()
        {
            string[] dirs = null;
            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
                if (ValidateRsDir())
                {
                    string dlcFolderPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc");

                    if (Directory.Exists(dlcFolderPath))
                    {
                        dirs = Directory.GetDirectories(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*", SearchOption.TopDirectoryOnly);
                        foreach (var setlistPath in dirs)
                        {
                            bool setlistEnabled = true;
                            Extensions.InvokeIfRequired(dgvSetlists, delegate
                            {
                                if (!SetlistEnabled(setlistPath))
                                    dgvSetlists.Rows.Add(false, "No", Path.GetFileName(setlistPath.Replace("-disabled", "")));
                                else if (SetlistModified(setlistPath))
                                    dgvSetlists.Rows.Add(false, "Modded", Path.GetFileName(setlistPath));
                                else
                                    dgvSetlists.Rows.Add(false, "Yes", Path.GetFileName(setlistPath));
                            });
                        }

                        string[] filesInSetlist = null;
                        if (dirs.Length > 0 && dirs[0] != null)
                        {
                            filesInSetlist = Directory.GetFiles(dirs[0]);
                            string[] unsortedSongs = Directory.GetFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "*_p.*psarc", SearchOption.TopDirectoryOnly);

                            Extensions.InvokeIfRequired(dgvDLCsInSetlist, delegate
                            {
                                foreach (string songPath in filesInSetlist)
                                {
                                    var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                    if (song != null)
                                        dgvDLCsInSetlist.Rows.Add(false, song.Enabled, song.Artist, song.Song, song.Album, song.Tuning, song.Path);
                                }
                            });

                            Extensions.InvokeIfRequired(dgvUnsortedDLCs, delegate
                            {
                                foreach (string songPath in unsortedSongs)
                                {
                                    var song = Globals.SongCollection.FirstOrDefault(sng => sng.Path == songPath);
                                    if (!songPath.Contains("rs1") && song != null)
                                    {
                                        if (songPath.Contains(".disabled"))
                                            dgvUnsortedDLCs.Rows.Add(false, "No", song.Artist, song.Song, song.Tuning, song.Path);
                                        else
                                            dgvUnsortedDLCs.Rows.Add(false, "Yes", song.Artist, song.Song, song.Tuning, song.Path);
                                    }
                                }
                            });

                            Extensions.InvokeIfRequired(dgvOfficialSongs, delegate
                            {
                                string cachePsarcPath = Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc");
                                string cachePsarcBackupPath = Path.Combine(Globals.MySettings.RSInstalledDir, "cache.disabled.psarc");

                                string rs1PsarcPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.psarc");
                                string rs1PsarcBackupPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.disabled.psarc");

                                string rs1DLCPsarcPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.psarc");
                                string rs1DLCPsarcBackupPath = Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.disabled.psarc");

                                if (File.Exists(cachePsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "cache.psarc");
                                else if (File.Exists(cachePsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "cache.psarc");

                                if (File.Exists(rs1PsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydlc_p.psarc");
                                else if (File.Exists(rs1PsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "rs1compatibilitydlc_p.psarc");

                                if (File.Exists(rs1DLCPsarcPath))
                                    dgvOfficialSongs.Rows.Add(false, "Yes", "rs1compatibilitydisc_p.psarc");
                                else if (File.Exists(rs1DLCPsarcBackupPath))
                                    dgvOfficialSongs.Rows.Add(false, "No", "rs1compatibilitydisc_p.psarc");
                            });
                        }
                    }
                }
            };
            bWorker.RunWorkerAsync();
        }
        private void btnRunRSWithSetlist_Click(object sender, EventArgs e)
        {
            string rs2014Pack = "";
            string rs1MainPack = "";
            string rs1DLCPack = "";
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");

            List<string> rs1DLCFiles = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "rs1compatibilitydlc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs1Files = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "rs1compatibilitydisc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();
            List<string> rs2014Files = Directory.EnumerateFiles(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc"), "cache.psarc*", SearchOption.AllDirectories).Where(sp => !sp.Contains(".disabled")).ToList();

            frmComboBoxPopup comboPopup = new frmComboBoxPopup();

            bWorker = new AbortableBackgroundWorker();
            bWorker.DoWork += delegate
            {
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
                        {
                            if (Path.GetDirectoryName(rs2014File) != Path.Combine(Globals.MySettings.RSInstalledDir, rs2014Pack))
                            {
                                File.Move(rs2014File, rs2014File + ".disabled");
                            }
                        }

                        if (File.Exists(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc")))
                            File.Copy(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc"), Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc"), true);
                        else if (File.Exists(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled")))
                            File.Copy(Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs2014Pack.Replace("dlc", ""), "cache.psarc.disabled"), Path.Combine(Globals.MySettings.RSInstalledDir, "cache.psarc"), true);
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
                    {
                        foreach (string rs1DLCFile in rs1DLCFiles)
                        {
                            if (Path.GetDirectoryName(rs1DLCFile) != Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs1DLCPack))
                            {
                                File.Move(rs1DLCFile, rs1DLCFile.Replace("_p.psarc", "_p.disabled.psarc"));
                            }
                        }
                    }
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
                        {
                            if (Path.GetDirectoryName(rs1File) != Path.Combine(Globals.MySettings.RSInstalledDir, "dlc", rs1MainPack))
                            {
                                File.Move(rs1File, rs1File.Replace("_p.psarc", "_p.disabled.psarc"));
                            }
                        }
                    }
                }

                if (rocksmithProcess.Length > 0)
                    MessageBox.Show("Rocksmith is already running!");
                else
                    Process.Start("steam://rungameid/221680");
            };
            bWorker.RunWorkerAsync();
        }
        #endregion

        private void dgvSetlists_SelectionChanged(object sender, EventArgs e)
        {
            if (tbUnsortedSearch.Text != "Search")
                RefreshSelectedSongs(tbUnsortedSearch.Text);
            else
                RefreshSelectedSongs();
        }
    }
}

