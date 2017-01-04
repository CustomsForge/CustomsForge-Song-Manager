﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFSM.RSTKLib.PSARC;
using CustomsForgeSongManager.DataObjects;
using DF.WinForms.ThemeLib;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XmlRepository;

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
            var newID = txtAppId.Text.Trim();
            
            foreach (var song in DataFiles)
            {
                NoCloseStream dataStream = null;
                using (PSARC p = new PSARC(true))
                {
                    using (var FS = File.OpenRead(song.FilePath))
                        p.Read(FS);

                    dataStream = p.ReplaceData(x => x.Name.Equals("appid.appid"), newID);

                    using (var FS = File.Create(song.FilePath))
                        p.Write(FS, true);
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
                txtAppId.Text = cmbAppId.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[2];
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