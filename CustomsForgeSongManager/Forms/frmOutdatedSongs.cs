﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.Forms
{
    // TODO: use reusable NoteViewer and depricate this code

    public partial class frmOutdatedSongs : Form
    {
        private Dictionary<string, SongData> outdatedSongList = new Dictionary<string, SongData>();

        public Dictionary<string, SongData> OutdatedSongList
        {
            set { outdatedSongList = value; }
        }

        public frmOutdatedSongs()
        {
            InitializeComponent();
        }

        private void btnOpenSongInBrowser_Click(object sender, EventArgs e)
        {
            Process.Start(dgvOutdatedSongs.SelectedRows[0].Cells["Link"].Value.ToString());
        }

        private void frmOutdatedSongs_Load(object sender, EventArgs e)
        {
            var outdatedSongsInfo = outdatedSongList.Select(song => new {Song = song.Value.Title, Artist = song.Value.Artist, Album = song.Value.Album, Link = song.Key}).ToList();
            dgvOutdatedSongs.DataSource = outdatedSongsInfo;
            dgvOutdatedSongs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void btnOpenAllOutdatedSongs_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvOutdatedSongs.Rows)
                Process.Start(row.Cells["Link"].Value.ToString());
        }
    }
}