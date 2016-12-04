using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.LocalTools;
using System.IO;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmRepairOptions : Form
    {
        private byte _checkByte;
        public SettingsDDC _settingsDD;
        private bool _userClosed, _btnOKClicked, _showDDTab;

        public frmRepairOptions()
        {
            InitializeComponent();

            LoadSavedSettings();

            _userClosed = false;
            _btnOKClicked = false;
            _showDDTab = false;
        }

        #region General repairs
        //public bool RepairMastery
        //{
        //    get { return rbRepairMastery.Checked; }
        //}

        public bool SkipRepaired
        {
            get { return rbSkipRepaired.Checked; }
        }

        //public bool RepairMasteryBug
        //{
        //    get { return rbRepairMastery.Checked; }
        //}

        public bool PreserveStats
        {
            get { return chkPreserve.Checked; }
        }

        public bool IgnoreMultiToneExceptions
        {
            get { return chkIgnoreMultitoneEx.Checked; }
        }
        #endregion
        #region DD
        public bool ReapplyDD
        {
            get { return rbReapplyDD.Checked; }
        }

        public int PhraseLenght
        {
            get { return Convert.ToInt32(tbPhraseLength.Value); }
        }

        public string CFGPath
        {
            get { return tbCFGPath.Text; }
        }

        public string RampUpPath
        {
            get { return tbRampUpPath.Text; }
        }

        public bool RemoveSus
        {
            get { return chkRemoveSus.Checked; }
        }

        public SettingsDDC SettingsDD
        {
            get
            {
                _settingsDD.PhraseLen = PhraseLenght;
                _settingsDD.RemoveSus = RemoveSus;
                _settingsDD.RampPath = RampUpPath;
                _settingsDD.CfgPath = CFGPath;
                return _settingsDD;
            }
        }
        #endregion
        #region Max5Arr
        public bool RepairMax5Arr
        {
            get { return rbRepairMaxFive.Checked; }
        }

        public bool RemoveNDDArr
        {
            get { return chkRemoveNdd.Checked; }
        }

        public bool RemoveBassArr
        {
            get { return chkRemoveBass.Checked; }
        }

        public bool RemoveGuitarArr
        {
            get { return chkRemoveGuitar.Checked; }
        }

        public bool RemoveBonusArr
        {
            get { return chkRemoveBonus.Checked; }
        }

        public bool RemoveMetronomeArr
        {
            get { return chkRemoveMetronome.Checked; }
        }

        public bool IgnoreStopLimit
        {
            get { return chkIgnoreLimit.Checked; }
        }

        public byte CheckByte
        {
            get
            {
                _checkByte = 0x00;
                if (chkRemoveNdd.Checked) _checkByte += 0x01;
                if (chkRemoveBass.Checked) _checkByte += 0x02;
                if (chkRemoveGuitar.Checked) _checkByte += 0x04;
                if (chkRemoveBonus.Checked) _checkByte += 0x08;
                if (chkRemoveMetronome.Checked) _checkByte += 0x16;

                return _checkByte;
            }
        }
        #endregion

        public bool FormClosedByUser
        {
            get { return _userClosed; }
            set { _userClosed = value; }
        }

        public bool ShowDDTab
        {
            get { return _showDDTab; }
            set { _showDDTab = value; }
        }

        private void LoadSavedSettings()
        {
            rbReapplyDD.Checked = AppSettings.Instance.ReapplyDD;
            rbRepairMaxFive.Checked = AppSettings.Instance.RepairMax5;
            rbSkipRepaired.Checked = AppSettings.Instance.SkipRepaired;
            chkIgnoreLimit.Checked = AppSettings.Instance.IgnoreStopLimit;
            chkIgnoreMultitoneEx.Checked = AppSettings.Instance.IgnoreMultiToneExceptions;
            chkPreserve.Checked = AppSettings.Instance.PreserveStats;
            chkRemoveBass.Checked = AppSettings.Instance.RemoveBass;
            chkRemoveBonus.Checked = AppSettings.Instance.RemoveBass;
            chkRemoveGuitar.Checked = AppSettings.Instance.RemoveGuitar;
            chkRemoveMetronome.Checked = AppSettings.Instance.RemoveMetronome;
            chkRemoveNdd.Checked = AppSettings.Instance.RemoveNDD;
            chkRemoveSus.Checked = AppSettings.Instance.RemoveSus;
            tbPhraseLength.Value = AppSettings.Instance.PhraseLenght;

            SettingsDDC.Instance.LoadConfigXml();

            if (AppSettings.Instance.CFGPath != null)
                tbCFGPath.Text = SettingsDDC.Instance.CfgPath;
            else
                tbCFGPath.Text = AppSettings.Instance.CFGPath;

            if (AppSettings.Instance.RampUpPath != null)
                tbCFGPath.Text = SettingsDDC.Instance.RampPath;
            else
                tbCFGPath.Text = AppSettings.Instance.RampUpPath;

            _settingsDD = new SettingsDDC();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AppSettings.Instance.RemoveSus = chkRemoveSus.Checked;
            AppSettings.Instance.RemoveNDD = chkRemoveNdd.Checked;
            AppSettings.Instance.RemoveMetronome = chkRemoveMetronome.Checked;
            AppSettings.Instance.RemoveGuitar = chkRemoveGuitar.Checked;
            AppSettings.Instance.RemoveBass = chkRemoveBonus.Checked;
            AppSettings.Instance.RemoveBass = chkRemoveBass.Checked;
            AppSettings.Instance.PreserveStats = chkPreserve.Checked;
            AppSettings.Instance.IgnoreMultiToneExceptions = chkIgnoreMultitoneEx.Checked;
            AppSettings.Instance.IgnoreStopLimit = chkIgnoreLimit.Checked;
            AppSettings.Instance.SkipRepaired = rbSkipRepaired.Checked;
            AppSettings.Instance.RepairMax5 = rbRepairMaxFive.Checked;
            AppSettings.Instance.ReapplyDD = rbReapplyDD.Checked;
            AppSettings.Instance.CFGPath = tbCFGPath.Text;
            AppSettings.Instance.RampUpPath = tbRampUpPath.Text;
            AppSettings.Instance.PhraseLenght = Convert.ToInt32(tbPhraseLength.Value);

            _btnOKClicked = true;
            this.Close();
        }

        private void tbCFGPath_MouseClick(object sender, MouseEventArgs e)
        {
            //TODO: fix multithreading issues
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "CFG Files (*.cfg)|*.cfg";
                ofd.InitialDirectory = Path.GetDirectoryName(SettingsDDC.Instance.CfgPath);

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                tbCFGPath.Text = ofd.FileName;
            }
        }

        private void tbRampUpPath_MouseClick(object sender, MouseEventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML Files (*.xml)|*.xml";
                ofd.InitialDirectory = Path.GetDirectoryName(SettingsDDC.Instance.RampPath);
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                tbRampUpPath.Text = ofd.FileName;
            }
        }

        private void rbSkipRepaired_MouseClick(object sender, MouseEventArgs e)
        {
            rbSkipRepaired.Checked = !rbSkipRepaired.Checked;
        }

        private void rbReapplyDD_MouseClick(object sender, MouseEventArgs e)
        {
            rbReapplyDD.Checked = !rbReapplyDD.Checked;
        }

        private void rbRepairMaxFive_Click(object sender, EventArgs e)
        {
            rbRepairMaxFive.Checked = !rbRepairMaxFive.Checked;
        }

        private void btnNextTabRepairs_Click(object sender, EventArgs e)
        {
            if (_showDDTab)
                tcRepairOptions.SelectedIndex = 1;
            else
                tcRepairOptions.SelectedIndex = 2;
        }

        private void btnNextTabDD_Click(object sender, EventArgs e)
        {
            tcRepairOptions.SelectedIndex = 2;
        }

        private void frmRepairOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_btnOKClicked)
                _userClosed = true;
        }
    }
}
