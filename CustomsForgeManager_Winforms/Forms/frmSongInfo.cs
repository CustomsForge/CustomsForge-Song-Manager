using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmSongInfo : Form
    {
        private readonly SongData song;

        public frmSongInfo(SongData data)
        {
            this.song = data;
            InitializeComponent();
        }

        private void frmSongInfo_Load(object sender, EventArgs e)
        {
            lbl_PanelSongTitle.Text = song.Song;
            lbl_PanelSongAlbum.Text = song.Album;
            lbl_PanelSongArtist.Text = song.Artist;
            lbl_PanelSongYear.Text = song.SongYear;
            lbl_PanelSongTuning.Text = song.Tuning;
            lbl_PanelSongArrangements.Text = song.Arrangements;
            lbl_PanelSongDD.Text = song.DD == "0" ? "No" : "Yes";
            lbl_PanelSongAuthor.Text = song.Author;
        }
    }
}
