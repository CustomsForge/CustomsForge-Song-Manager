using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Forms
{
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
            var outdatedSongsInfo = outdatedSongList.Select(song => new { Song = song.Value.Song, Artist = song.Value.Artist, Album = song.Value.Album, Link = song.Key }).ToList();
            dgvOutdatedSongs.DataSource = outdatedSongsInfo;
        }
    }
}
