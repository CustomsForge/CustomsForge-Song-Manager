using System;
using System.Drawing;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System.IO;
using CFSM.Utils;

namespace CustomsForgeManager.Forms
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
            lbl_PanelSongTitle.Text = song.Title;
            lbl_PanelSongAlbum.Text = song.Album;
            lbl_PanelSongArtist.Text = song.Artist;
            lbl_PanelSongYear.Text = song.SongYear.ToString();
            lbl_PanelSongTuning.Text = song.Tuning;
            lbl_PanelSongArrangements.Text = song.Arrangements;
            lbl_PanelSongDD.Text = song.DD.ToString();
            lbl_PanelSongAuthor.Text = song.Charter;
            lbl_PanelSongPath.Text = song.Path;

            // TODO: reimpliment
            FillGridWithArrangements(song.Arrangements);

            //
            if (File.Exists(song.Path))
                using (var fs = File.OpenRead(song.Path))
                {
                    var s = fs.ExtractPSARCData(entry => entry.Name.Contains("128.dds"));
                    if (s != null)
                    {
                        var b = ImageExtensions.DDStoBitmap(s);
                        if (b != null)
                            pbArtwork.Image = b;
                        s.Close();
                    }
                }

        }

        private void FillGridWithArrangements(string arrangements)
        {

            string[] splits = arrangements.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (DataGridViewColumn column in dgv_Arrangements.Columns)
            {
                column.Visible = false;
            }

            foreach (string split in splits)
            {
                if (split.ToLower().Contains("combo"))
                {
                    dgv_Arrangements.Columns["colLead"].DefaultCellStyle.BackColor = Color.Lime;
                    dgv_Arrangements.Columns["colRhythm"].DefaultCellStyle.BackColor = Color.Lime;
                    dgv_Arrangements.Columns["colLead"].Visible = true;
                    dgv_Arrangements.Columns["colRhythm"].Visible = true;
                }
                else
                {
                    string column_name = "col" + split;
                    var dataGridViewColumn = dgv_Arrangements.Columns[column_name];
                    if (dataGridViewColumn != null)
                    {
                        dataGridViewColumn.DefaultCellStyle.BackColor = Color.Lime;
                        dataGridViewColumn.Visible = true;
                    }
                }
            }

            dgv_Arrangements.Rows.Add(new[] { Properties.Resources.Letter_B, Properties.Resources.Letter_L, Properties.Resources.Letter_R, Properties.Resources.Letter_V });
            dgv_Arrangements.ClearSelection();
        }

    }
}
