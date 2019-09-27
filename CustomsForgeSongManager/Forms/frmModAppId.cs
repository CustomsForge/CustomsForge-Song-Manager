using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using DF.WinForms.ThemeLib;
using RocksmithToolkitLib;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.XmlRepository;
using RocksmithToolkitLib.PSARC;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmModAppId : ThemedForm
    {
        private SongData[] DataFiles;
        private bool isCancelled;

        protected frmModAppId()
        {
            InitializeComponent();
            foreach (var song in SongAppIdRepository.Instance().Select(GameVersion.RS2014))
                cmbAppId.Items.Add(song);

            Icon = Properties.Resources.cfsm_48x48;
        }

        public static bool BatchEdit(SongData[] dataFiles)
        {
            if (dataFiles.Any())
            {
                using (var f = new frmModAppId())
                {
                    f.DataFiles = dataFiles;
                    f.cmbAppId.SelectedItem = SongAppIdRepository.Instance().Select(dataFiles[0].AppID, GameVersion.RS2014);
                    return f.ShowDialog() == DialogResult.OK;
                }
            }
            return false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;
            this.Refresh();
            themedProgressBar1.Maximum = DataFiles.Length;
            themedProgressBar1.Value = 0;
            var newID = txtAppId.Text.Trim().GetValidAppIdSixDigits();

            // social engineering code
            if (newID.Equals("221680"))
                throw new InvalidDataException("<WARNING> Sentinel has detected futile human resistance ..." + Environment.NewLine +
                    "Buy Cherub Rock and you wont have to bother changing AppId's.");

            foreach (var song in DataFiles)
            {
                if (song.IsODLC)
                    continue;

                NoCloseStream dataStream = null;
                using (PSARC p = new PSARC(true))
                {
                    using (var fs = File.OpenRead(song.FilePath))
                        p.Read(fs);

                    dataStream = p.ReplaceData(x => x.Name.Equals("appid.appid"), newID);

                    using (var fs = File.Create(song.FilePath))
                        p.Write(fs, true);
                }

                if (dataStream != null)
                    dataStream.CloseEx();

                song.AppID = txtAppId.Text;
                song.UpdateFileInfo();
                themedProgressBar1.Value++;
                Application.DoEvents();

                if (isCancelled)
                    break;
            }

            lblMsg.Visible = false;
            DialogResult = DialogResult.OK;

            this.Close();
        }

        private void cmbAppId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAppId.SelectedItem != null)
            {
                try
                {
                    txtAppId.Text = cmbAppId.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[2];
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("song 'Name' in the AppId repository is missing the required ' - '");
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancelled = true;
        }
    }

    public class BatchEditor
    {
        public virtual bool Edit(PSARC archive)
        {
            return false;
        }
    }
}