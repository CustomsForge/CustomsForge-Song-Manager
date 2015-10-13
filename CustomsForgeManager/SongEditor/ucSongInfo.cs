using System;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucSongInfo : DLCPackageEditorControlBase
    {
        public ucSongInfo()
        {
            InitializeComponent();
        }

        public override void DoInit()
        {
            // this will throw exception if SongFilePath not found
            var song = Globals.SongCollection.First(x => x.Path == FilePath);

            txtKey.Text = SongData.Name;
            txtArtist.Text = SongData.SongInfo.Artist;
            txtArtistSort.Text = SongData.SongInfo.ArtistSort;
            txtTitle.Text = SongData.SongInfo.SongDisplayName;
            txtTitleSort.Text = SongData.SongInfo.SongDisplayNameSort;
            txtAlbum.Text = SongData.SongInfo.Album;
            txtAppId.Text = SongData.AppId;
            txtVersion.Text = SongData.PackageVersion;
            txtYear.Text = SongData.SongInfo.SongYear.ToString();
            txtAvgTempo.Text = SongData.SongInfo.AverageTempo.ToString();
            cmbSongVolume.Value = Convert.ToDecimal(SongData.Volume);
            cmbPreviewVolume.Value = Convert.ToDecimal(SongData.PreviewVolume);
            txtCharter.Text = song.Charter; // not editable

            txtKey.TextChanged += TextValueChanged;
            txtArtist.TextChanged += TextValueChanged;
            txtArtistSort.TextChanged += TextValueChanged;
            txtTitle.TextChanged += TextValueChanged;
            txtTitleSort.TextChanged += TextValueChanged;
            txtAlbum.TextChanged += TextValueChanged;
            txtAppId.TextChanged += TextValueChanged;
            txtVersion.TextChanged += TextValueChanged;
            txtYear.TextChanged += TextValueChanged;
            txtAvgTempo.TextChanged += TextValueChanged;
            cmbSongVolume.TextChanged += TextValueChanged;
            cmbPreviewVolume.TextChanged += TextValueChanged;
            //set up events
            PopulateAppIdCombo(SongData.AppId, GameVersion.RS2014);
        }

        void TextValueChanged(object sender, EventArgs e)
        {
            this.Dirty = true;
        }


        //TODO: validate editors
        public override void Save()
        {
            SongData.Name = txtKey.Text;
            SongData.SongInfo.Artist = txtArtist.Text;
            SongData.SongInfo.ArtistSort = txtArtistSort.Text;
            SongData.SongInfo.SongDisplayName = txtTitle.Text;
            SongData.SongInfo.SongDisplayNameSort = txtTitleSort.Text;
            SongData.SongInfo.Album = txtAlbum.Text;
            SongData.AppId = txtAppId.Text;
            SongData.PackageVersion = txtVersion.Text;
            SongData.SongInfo.SongYear = Convert.ToInt32(txtYear.Text);
            SongData.SongInfo.AverageTempo = Convert.ToInt32(txtAvgTempo.Text);
            SongData.Volume = Convert.ToSingle(cmbSongVolume.Value);
            SongData.PreviewVolume = Convert.ToSingle(cmbPreviewVolume.Value);
            base.Save();
        }

        private void PopulateAppIdCombo(string appId, GameVersion gameVersion)
        {
            cmbAppId.Items.Clear();
            try
            {
                foreach (var song in SongAppIdRepository.Instance().Select(gameVersion))
                    cmbAppId.Items.Add(song);
            }
            catch (Exception)
            {
                var msg = "Can not find file: RocksmithToolkitLib.SongAppId.xml" + Environment.NewLine + "Try reinstalling the complete CFM Applicton.";
                MessageBox.Show(msg, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var songAppId = SongAppIdRepository.Instance().Select(appId, gameVersion);
                cmbAppId.SelectedItem = songAppId; // triggers SelectedIndexChanged
            }
            catch (Exception ex)
            {
                var msg = "Please select a new AppId from available selection." + Environment.NewLine + "Can not find: " + appId;
                MessageBox.Show(msg, Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbAppId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAppId.SelectedItem != null)
            {
                txtAppId.Text = cmbAppId.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[2];
            }

        }


    }
}
