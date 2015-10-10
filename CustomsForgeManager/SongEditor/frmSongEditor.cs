using System;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib.DLCPackage;

namespace CustomsForgeManager.SongEditor
{
    public partial class frmSongEditor : Form
    {
        private DLCPackageData info;
        private string filePath ;

        // TODO: consider revamp frmMain code to be like this
        public frmSongEditor(string songPath)
        {
            if (String.IsNullOrEmpty(songPath))
                return;

            InitializeComponent();

            // start marquee Pbar on frmMain 

            var psarc = new PsarcPackage();
            info = psarc.ReadPackage(songPath);
            filePath = songPath;

            // stop marquee Pbar on frmMain

            LoadSongInfo();
        }

        private void tslSave_Click(object sender, EventArgs e)
        {

        }

        private void tslSaveAs_Click(object sender, EventArgs e)
        {

        }

        private void tslExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSongInfo()
        {
            var songInfo = new ucSongInfo();
            this.tpSongInfo.Controls.Clear();
            this.tpSongInfo.Controls.Add(songInfo);
            songInfo.Dock = DockStyle.Fill;
            songInfo.FilePath = filePath;
            songInfo.SongData = info;
            songInfo.InitSongInfo();
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Docking.Fill causes screen flicker so only use if needed
            // reset toolstrip labels
            Globals.ResetToolStripGlobals();

            // get first four charters from tab control text
            switch (tcMain.SelectedTab.Text.Substring(0, 4).ToUpper())
            {
                    // passing variables(objects) by value to UControl
                case "SONG":
                    LoadSongInfo();
                    break;
                case "TONE":
            var tones = new ucTones();
            this.tpTones.Controls.Clear();
            this.tpTones.Controls.Add(tones);
            tones.Dock = DockStyle.Fill;
            tones.FilePath = filePath;
            tones.SongData = info;
            tones.InitTones();
                    break;
 
            }
     

         }
    }
}
