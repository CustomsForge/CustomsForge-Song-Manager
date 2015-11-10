using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DF.WinForms.ThemeLib;
using RocksmithToolkitLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFSM.Utils;

namespace CustomsForgeManager.Forms
{
    public partial class frmSongBatchEdit : ThemedForm
    {
        SongData[] DataFiles;
        protected frmSongBatchEdit()
        {
            InitializeComponent();
            foreach (var song in SongAppIdRepository.Instance().Select(GameVersion.RS2014))
                cmbAppId.Items.Add(song);
            Icon = Properties.Resources.cfsm_48x48;
        }

        public static bool BatchEdit(SongData[] dataFiles)
        {
            if (dataFiles.Length > 0)
            {
                using (var f = new frmSongBatchEdit())
                {
                    f.DataFiles = dataFiles;
                    f.cmbAppId.SelectedItem =
                        SongAppIdRepository.Instance().Select(dataFiles[0].AppID, GameVersion.RS2014);
                    return f.ShowDialog() == DialogResult.OK;
                }
            }
            return false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            themedProgressBar1.Maximum = DataFiles.Length;
            themedProgressBar1.Value = 0;
            string newID = txtAppId.Text.Trim();
            foreach (var song in DataFiles)
            {
                NoCloseStream dataStream = null;
                using (CFSM.Utils.PSARC.PSARC p = new CFSM.Utils.PSARC.PSARC(true))
                {
                    using (var FS = File.OpenRead(song.Path))
                        p.Read(FS);

                    dataStream = p.ReplaceData(x => x.Name.Equals("appid.appid"), newID);

                    using (var FS = File.Create(song.Path))
                        p.Write(FS,true);
                }
                if (dataStream != null)
                    dataStream.CloseEx();

                song.AppID = txtAppId.Text;
                song.UpdateFileInfo();
                themedProgressBar1.Value++;
                Application.DoEvents();
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmbAppId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAppId.SelectedItem != null)
                txtAppId.Text = cmbAppId.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[2];
        }
    }

    public class BatchEditor
    {
        public virtual bool Edit(CFSM.Utils.PSARC.PSARC archive) { return false; }
    }


}
