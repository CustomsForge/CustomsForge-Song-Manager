using System;
using System.ComponentModel;
using System.Windows.Forms;
using CustomControls;
using CustomsForgeSongManager.LocalTools;

namespace CustomsForgeSongManager.UControls
{
    partial class Settings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lblDgvColumns = new System.Windows.Forms.Label();
            this.lstDgvColumns = new System.Windows.Forms.ListView();
            this.columnSelect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cueRsDir = new CustomControls.CueTextBox();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.rbCleanOnClosing = new System.Windows.Forms.RadioButton();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.btnEmptyLogs = new System.Windows.Forms.Button();
            this.chkEnableLogBallon = new System.Windows.Forms.CheckBox();
            this.chkIncludeRS1DLC = new System.Windows.Forms.CheckBox();
            this.chkEnableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.btnResetDownloads = new System.Windows.Forms.Button();
            this.txtCharterName = new CustomControls.CueTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tlpSettings_Wrapper.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSettings_Wrapper
            // 
            this.tlpSettings_Wrapper.ColumnCount = 3;
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.88513F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.94813F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.16674F));
            this.tlpSettings_Wrapper.Controls.Add(this.panel5, 2, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.cueRsDir, 2, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 0, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.rbCleanOnClosing, 0, 8);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 10);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 0, 10);
            this.tlpSettings_Wrapper.Controls.Add(this.btnEmptyLogs, 0, 9);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableLogBallon, 0, 4);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS1DLC, 0, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableAutoUpdate, 0, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.btnResetDownloads, 1, 9);
            this.tlpSettings_Wrapper.Controls.Add(this.txtCharterName, 0, 6);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 12;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 113F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(866, 490);
            this.tlpSettings_Wrapper.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lnkSelectAll);
            this.panel5.Controls.Add(this.lblDgvColumns);
            this.panel5.Controls.Add(this.lstDgvColumns);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(347, 28);
            this.panel5.Name = "panel5";
            this.tlpSettings_Wrapper.SetRowSpan(this.panel5, 11);
            this.panel5.Size = new System.Drawing.Size(516, 459);
            this.panel5.TabIndex = 4;
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.Location = new System.Drawing.Point(3, 5);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(98, 13);
            this.lnkSelectAll.TabIndex = 6;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "Select/Deselect All";
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
            // 
            // lblDgvColumns
            // 
            this.lblDgvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDgvColumns.AutoSize = true;
            this.lblDgvColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDgvColumns.Location = new System.Drawing.Point(119, 5);
            this.lblDgvColumns.Name = "lblDgvColumns";
            this.lblDgvColumns.Size = new System.Drawing.Size(142, 13);
            this.lblDgvColumns.TabIndex = 1;
            this.lblDgvColumns.Text = "Settings for {0} from {1}";
            this.lblDgvColumns.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lstDgvColumns
            // 
            this.lstDgvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstDgvColumns.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lstDgvColumns.CheckBoxes = true;
            this.lstDgvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSelect,
            this.colSettingsColumnName,
            this.colSettingsColumnHeader,
            this.colSettingsWidth});
            this.lstDgvColumns.FullRowSelect = true;
            this.lstDgvColumns.GridLines = true;
            this.lstDgvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstDgvColumns.Location = new System.Drawing.Point(0, 24);
            this.lstDgvColumns.MultiSelect = false;
            this.lstDgvColumns.Name = "lstDgvColumns";
            this.lstDgvColumns.Size = new System.Drawing.Size(513, 438);
            this.lstDgvColumns.TabIndex = 5;
            this.lstDgvColumns.UseCompatibleStateImageBehavior = false;
            this.lstDgvColumns.View = System.Windows.Forms.View.Details;
            this.lstDgvColumns.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listDisabledColumns_ItemChecked);
            // 
            // columnSelect
            // 
            this.columnSelect.Text = "";
            this.columnSelect.Width = 25;
            // 
            // colSettingsColumnName
            // 
            this.colSettingsColumnName.Text = "Column Name";
            this.colSettingsColumnName.Width = 150;
            // 
            // colSettingsColumnHeader
            // 
            this.colSettingsColumnHeader.Text = "Column Header";
            this.colSettingsColumnHeader.Width = 150;
            // 
            // colSettingsWidth
            // 
            this.colSettingsWidth.Text = "Column Width";
            this.colSettingsWidth.Width = 150;
            // 
            // cueRsDir
            // 
            this.cueRsDir.Cue = "Click here and specify Rocksmith installation directory";
            this.cueRsDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueRsDir.ForeColor = System.Drawing.Color.Gray;
            this.cueRsDir.Location = new System.Drawing.Point(347, 3);
            this.cueRsDir.Multiline = true;
            this.cueRsDir.Name = "cueRsDir";
            this.cueRsDir.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cueRsDir.Size = new System.Drawing.Size(515, 19);
            this.cueRsDir.TabIndex = 8;
            this.cueRsDir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cueRsDir_MouseClick);
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSettingsRSDir.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.lblSettingsRSDir, 2);
            this.lblSettingsRSDir.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingsRSDir.Location = new System.Drawing.Point(126, 3);
            this.lblSettingsRSDir.Margin = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Padding = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Size = new System.Drawing.Size(215, 19);
            this.lblSettingsRSDir.TabIndex = 7;
            this.lblSettingsRSDir.Text = "Rocksmith Installation Directory:";
            this.lblSettingsRSDir.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbCleanOnClosing
            // 
            this.rbCleanOnClosing.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbCleanOnClosing.AutoSize = true;
            this.rbCleanOnClosing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCleanOnClosing.Location = new System.Drawing.Point(3, 247);
            this.rbCleanOnClosing.Name = "rbCleanOnClosing";
            this.rbCleanOnClosing.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.rbCleanOnClosing.Size = new System.Drawing.Size(167, 19);
            this.rbCleanOnClosing.TabIndex = 10;
            this.rbCleanOnClosing.Text = "Remove temporary work";
            this.rbCleanOnClosing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.rbCleanOnClosing, resources.GetString("rbCleanOnClosing.ToolTip"));
            this.rbCleanOnClosing.UseVisualStyleBackColor = true;
            this.rbCleanOnClosing.CheckedChanged += new System.EventHandler(this.rbCleanOnClosing_CheckedChanged);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettingsSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSettingsSave.Image = global::CustomsForgeSongManager.Properties.Resources.save;
            this.btnSettingsSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsSave.Location = new System.Drawing.Point(192, 441);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnSettingsSave.Size = new System.Drawing.Size(118, 29);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings  ";
            this.btnSettingsSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSettingsLoad.Image = global::CustomsForgeSongManager.Properties.Resources.load;
            this.btnSettingsLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsLoad.Location = new System.Drawing.Point(35, 441);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnSettingsLoad.Size = new System.Drawing.Size(118, 29);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Load Settings  ";
            this.btnSettingsLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // btnEmptyLogs
            // 
            this.btnEmptyLogs.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEmptyLogs.Image = global::CustomsForgeSongManager.Properties.Resources.delete;
            this.btnEmptyLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmptyLogs.Location = new System.Drawing.Point(35, 354);
            this.btnEmptyLogs.Name = "btnEmptyLogs";
            this.btnEmptyLogs.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnEmptyLogs.Size = new System.Drawing.Size(118, 29);
            this.btnEmptyLogs.TabIndex = 11;
            this.btnEmptyLogs.Text = "Empty Log Files";
            this.btnEmptyLogs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnEmptyLogs, "Empty the log files");
            this.btnEmptyLogs.UseVisualStyleBackColor = true;
            this.btnEmptyLogs.Click += new System.EventHandler(this.btnEmptyLogs_Click);
            // 
            // chkEnableLogBallon
            // 
            this.chkEnableLogBallon.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableLogBallon.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableLogBallon, 2);
            this.chkEnableLogBallon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogBallon.Location = new System.Drawing.Point(3, 119);
            this.chkEnableLogBallon.Name = "chkEnableLogBallon";
            this.chkEnableLogBallon.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableLogBallon.Size = new System.Drawing.Size(143, 19);
            this.chkEnableLogBallon.TabIndex = 5;
            this.chkEnableLogBallon.Text = "Enable Log Baloon ";
            this.chkEnableLogBallon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableLogBallon.UseVisualStyleBackColor = true;
            this.chkEnableLogBallon.CheckedChanged += new System.EventHandler(this.chkEnableLogBaloon_CheckedChanged);
            // 
            // chkIncludeRS1DLC
            // 
            this.chkIncludeRS1DLC.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeRS1DLC.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeRS1DLC, 2);
            this.chkIncludeRS1DLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeRS1DLC.Location = new System.Drawing.Point(3, 87);
            this.chkIncludeRS1DLC.Name = "chkIncludeRS1DLC";
            this.chkIncludeRS1DLC.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeRS1DLC.Size = new System.Drawing.Size(205, 19);
            this.chkIncludeRS1DLC.TabIndex = 3;
            this.chkIncludeRS1DLC.Text = "Include RS1 Compatibility Pack";
            this.chkIncludeRS1DLC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkIncludeRS1DLC.UseVisualStyleBackColor = true;
            this.chkIncludeRS1DLC.CheckedChanged += new System.EventHandler(this.chkIncludeRS1DLC_CheckedChanged);
            // 
            // chkEnableAutoUpdate
            // 
            this.chkEnableAutoUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableAutoUpdate.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableAutoUpdate, 2);
            this.chkEnableAutoUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableAutoUpdate.Location = new System.Drawing.Point(3, 55);
            this.chkEnableAutoUpdate.Name = "chkEnableAutoUpdate";
            this.chkEnableAutoUpdate.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableAutoUpdate.Size = new System.Drawing.Size(144, 19);
            this.chkEnableAutoUpdate.TabIndex = 12;
            this.chkEnableAutoUpdate.Text = "Enable Auto Update";
            this.chkEnableAutoUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkEnableAutoUpdate, "If checked the application will automatically\r\nupdate to the latest version if th" +
                    "ere is\r\nan internet connection.");
            this.chkEnableAutoUpdate.UseVisualStyleBackColor = true;
            this.chkEnableAutoUpdate.CheckedChanged += new System.EventHandler(this.chkEnableAutoUpdate_CheckedChanged);
            // 
            // btnResetDownloads
            // 
            this.btnResetDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetDownloads.Image = global::CustomsForgeSongManager.Properties.Resources.clear;
            this.btnResetDownloads.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetDownloads.Location = new System.Drawing.Point(192, 354);
            this.btnResetDownloads.Name = "btnResetDownloads";
            this.btnResetDownloads.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnResetDownloads.Size = new System.Drawing.Size(118, 29);
            this.btnResetDownloads.TabIndex = 13;
            this.btnResetDownloads.Text = "Reset DL Folder";
            this.btnResetDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnResetDownloads, "Reset the \'Downloads\' folder path to\r\nspecify where new CDLC are stored.");
            this.btnResetDownloads.UseVisualStyleBackColor = true;
            this.btnResetDownloads.Click += new System.EventHandler(this.btnResetDownloads_Click);
            // 
            // txtCharterName
            // 
            this.txtCharterName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpSettings_Wrapper.SetColumnSpan(this.txtCharterName, 2);
            this.txtCharterName.Cue = "Enter CDLC Charter Name";
            this.txtCharterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCharterName.ForeColor = System.Drawing.Color.Gray;
            this.txtCharterName.Location = new System.Drawing.Point(12, 183);
            this.txtCharterName.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.txtCharterName.Name = "txtCharterName";
            this.txtCharterName.Size = new System.Drawing.Size(179, 20);
            this.txtCharterName.TabIndex = 9;
            this.toolTip.SetToolTip(this.txtCharterName, "Enter your charter name or the name\r\nof any charter you would like to show \r\nquic" +
                    "kly when checkbox \'My CDLC Only\' \r\nis checked in Song Manager.\r\n");
            this.txtCharterName.TextChanged += new System.EventHandler(this.tbCreator_TextChanged);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // Settings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tlpSettings_Wrapper);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(866, 490);
            this.tlpSettings_Wrapper.ResumeLayout(false);
            this.tlpSettings_Wrapper.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tlpSettings_Wrapper;
        private Button btnSettingsLoad;
        private Panel panel5;
        private LinkLabel lnkSelectAll;
        private Label lblDgvColumns;
        private ListView lstDgvColumns;
        private ColumnHeader columnSelect;
        private ColumnHeader colSettingsColumnName;
        private ColumnHeader colSettingsColumnHeader;
        private ColumnHeader colSettingsWidth;
        private Button btnSettingsSave;
        private Label lblSettingsRSDir;
        private CueTextBox cueRsDir;
        public CheckBox chkIncludeRS1DLC;
        private CheckBox chkEnableLogBallon;
        private CueTextBox txtCharterName;
        private RadioButton rbCleanOnClosing;
        private ToolTip toolTip;
        private Button btnEmptyLogs;
        public CheckBox chkEnableAutoUpdate;
        private Button btnResetDownloads;
    }
}
