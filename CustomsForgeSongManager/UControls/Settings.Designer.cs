using System.ComponentModel;
using System.Windows.Forms;
using CustomControls;

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
            this.btnResetDownloads = new System.Windows.Forms.Button();
            this.chkIncludeRS1CompSongs = new System.Windows.Forms.CheckBox();
            this.chkIncludeRS2BaseSongs = new System.Windows.Forms.CheckBox();
            this.chkIncludeCustomPacks = new System.Windows.Forms.CheckBox();
            this.chkEnableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.chkEnableNotifications = new System.Windows.Forms.CheckBox();
            this.chkEnableQuarantine = new System.Windows.Forms.CheckBox();
            this.chkValidateD3D = new System.Windows.Forms.CheckBox();
            this.chkMacMode = new System.Windows.Forms.CheckBox();
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
            this.tlpSettings_Wrapper.Controls.Add(this.rbCleanOnClosing, 0, 13);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 16);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 0, 16);
            this.tlpSettings_Wrapper.Controls.Add(this.btnEmptyLogs, 0, 15);
            this.tlpSettings_Wrapper.Controls.Add(this.btnResetDownloads, 1, 15);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS1CompSongs, 0, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS2BaseSongs, 0, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeCustomPacks, 0, 4);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableAutoUpdate, 0, 5);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableNotifications, 0, 6);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableQuarantine, 0, 7);
            this.tlpSettings_Wrapper.Controls.Add(this.chkValidateD3D, 0, 8);
            this.tlpSettings_Wrapper.Controls.Add(this.chkMacMode, 0, 9);
            this.tlpSettings_Wrapper.Controls.Add(this.txtCharterName, 0, 10);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 18;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
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
            this.tlpSettings_Wrapper.SetRowSpan(this.panel5, 17);
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
            this.tlpSettings_Wrapper.SetColumnSpan(this.rbCleanOnClosing, 2);
            this.rbCleanOnClosing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCleanOnClosing.Location = new System.Drawing.Point(3, 345);
            this.rbCleanOnClosing.Name = "rbCleanOnClosing";
            this.rbCleanOnClosing.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.rbCleanOnClosing.Size = new System.Drawing.Size(166, 19);
            this.rbCleanOnClosing.TabIndex = 10;
            this.rbCleanOnClosing.Text = "Reset CFSM On Closing";
            this.rbCleanOnClosing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.rbCleanOnClosing, resources.GetString("rbCleanOnClosing.ToolTip"));
            this.rbCleanOnClosing.UseVisualStyleBackColor = true;
            this.rbCleanOnClosing.Click += new System.EventHandler(this.rbCleanOnClosing_Click);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettingsSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSettingsSave.Image = global::CustomsForgeSongManager.Properties.Resources.save;
            this.btnSettingsSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsSave.Location = new System.Drawing.Point(192, 444);
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
            this.btnSettingsLoad.Location = new System.Drawing.Point(35, 444);
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
            this.btnEmptyLogs.Location = new System.Drawing.Point(35, 399);
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
            // btnResetDownloads
            // 
            this.btnResetDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetDownloads.Image = global::CustomsForgeSongManager.Properties.Resources.clear;
            this.btnResetDownloads.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetDownloads.Location = new System.Drawing.Point(192, 399);
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
            // chkIncludeRS1CompSongs
            // 
            this.chkIncludeRS1CompSongs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeRS1CompSongs.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeRS1CompSongs, 2);
            this.chkIncludeRS1CompSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeRS1CompSongs.Location = new System.Drawing.Point(3, 57);
            this.chkIncludeRS1CompSongs.Name = "chkIncludeRS1CompSongs";
            this.chkIncludeRS1CompSongs.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeRS1CompSongs.Size = new System.Drawing.Size(211, 19);
            this.chkIncludeRS1CompSongs.TabIndex = 3;
            this.chkIncludeRS1CompSongs.Text = "Include RS1 Compatibility Packs";
            this.chkIncludeRS1CompSongs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkIncludeRS1CompSongs, "If checked, include RS1 Compatibility \r\nPacks with the scanned data.\r\n\r\nNOTE: Ini" +
                    "tial load is very slow.");
            this.chkIncludeRS1CompSongs.UseVisualStyleBackColor = true;
            this.chkIncludeRS1CompSongs.Click += new System.EventHandler(this.chkIncludeRS1CompSongs_Click);
            // 
            // chkIncludeRS2BaseSongs
            // 
            this.chkIncludeRS2BaseSongs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeRS2BaseSongs.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeRS2BaseSongs, 2);
            this.chkIncludeRS2BaseSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chkIncludeRS2BaseSongs.Location = new System.Drawing.Point(3, 85);
            this.chkIncludeRS2BaseSongs.Name = "chkIncludeRS2BaseSongs";
            this.chkIncludeRS2BaseSongs.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeRS2BaseSongs.Size = new System.Drawing.Size(192, 19);
            this.chkIncludeRS2BaseSongs.TabIndex = 15;
            this.chkIncludeRS2BaseSongs.Text = "Include RS2014 Base Songs";
            this.chkIncludeRS2BaseSongs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkIncludeRS2BaseSongs, "If checked, include RS2014 Base\r\nSongs with the scanned data.\r\n\r\nNOTE: Initial lo" +
                    "ad is very slow.");
            this.chkIncludeRS2BaseSongs.UseVisualStyleBackColor = true;
            this.chkIncludeRS2BaseSongs.Click += new System.EventHandler(this.chkIncludeRS2BaseSongs_Click);
            // 
            // chkIncludeCustomPacks
            // 
            this.chkIncludeCustomPacks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeCustomPacks.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeCustomPacks, 2);
            this.chkIncludeCustomPacks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chkIncludeCustomPacks.Location = new System.Drawing.Point(3, 113);
            this.chkIncludeCustomPacks.Name = "chkIncludeCustomPacks";
            this.chkIncludeCustomPacks.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeCustomPacks.Size = new System.Drawing.Size(188, 19);
            this.chkIncludeCustomPacks.TabIndex = 18;
            this.chkIncludeCustomPacks.Text = "Include Custom Song Packs";
            this.chkIncludeCustomPacks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkIncludeCustomPacks, "If checked, include Custom\r\nSongPacks with the scanned data.");
            this.chkIncludeCustomPacks.UseVisualStyleBackColor = true;
            this.chkIncludeCustomPacks.Click += new System.EventHandler(this.chkIncludeCustomPacks_Click);
            // 
            // chkEnableAutoUpdate
            // 
            this.chkEnableAutoUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableAutoUpdate.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableAutoUpdate, 2);
            this.chkEnableAutoUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableAutoUpdate.Location = new System.Drawing.Point(3, 141);
            this.chkEnableAutoUpdate.Name = "chkEnableAutoUpdate";
            this.chkEnableAutoUpdate.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableAutoUpdate.Size = new System.Drawing.Size(144, 19);
            this.chkEnableAutoUpdate.TabIndex = 12;
            this.chkEnableAutoUpdate.Text = "Enable Auto Update";
            this.chkEnableAutoUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkEnableAutoUpdate, "If checked the application will automatically\r\nupdate to the latest version if th" +
                    "ere is\r\nan internet connection.");
            this.chkEnableAutoUpdate.UseVisualStyleBackColor = true;
            this.chkEnableAutoUpdate.Click += new System.EventHandler(this.chkEnableAutoUpdate_Click);
            // 
            // chkEnableNotifications
            // 
            this.chkEnableNotifications.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableNotifications.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableNotifications, 2);
            this.chkEnableNotifications.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableNotifications.Location = new System.Drawing.Point(3, 169);
            this.chkEnableNotifications.Name = "chkEnableNotifications";
            this.chkEnableNotifications.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableNotifications.Size = new System.Drawing.Size(144, 19);
            this.chkEnableNotifications.TabIndex = 16;
            this.chkEnableNotifications.Text = "Enable Notifications";
            this.chkEnableNotifications.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkEnableNotifications, "If checked, shows popup notification\r\nballon messages in the System Tray.");
            this.chkEnableNotifications.UseVisualStyleBackColor = true;
            this.chkEnableNotifications.Click += new System.EventHandler(this.chkEnableNotifications_Click);
            // 
            // chkEnableQuarantine
            // 
            this.chkEnableQuarantine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableQuarantine.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableQuarantine, 2);
            this.chkEnableQuarantine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableQuarantine.Location = new System.Drawing.Point(3, 197);
            this.chkEnableQuarantine.Name = "chkEnableQuarantine";
            this.chkEnableQuarantine.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkEnableQuarantine.Size = new System.Drawing.Size(165, 19);
            this.chkEnableQuarantine.TabIndex = 21;
            this.chkEnableQuarantine.Text = "Enable Auto Quarantine";
            this.chkEnableQuarantine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkEnableQuarantine, "If checked, corrupt CDLC will \r\nautomatically be moved to quarantine\r\nand be remo" +
                    "ved from the \'dlc\' folder.");
            this.chkEnableQuarantine.UseVisualStyleBackColor = true;
            this.chkEnableQuarantine.Click += new System.EventHandler(this.chkEnableQuarantine_Click);
            // 
            // chkValidateD3D
            // 
            this.chkValidateD3D.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkValidateD3D.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkValidateD3D, 2);
            this.chkValidateD3D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkValidateD3D.Location = new System.Drawing.Point(3, 225);
            this.chkValidateD3D.Name = "chkValidateD3D";
            this.chkValidateD3D.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkValidateD3D.Size = new System.Drawing.Size(159, 19);
            this.chkValidateD3D.TabIndex = 19;
            this.chkValidateD3D.Text = "Validate D3DX9_42.dll";
            this.chkValidateD3D.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkValidateD3D, "If checked, CFSM will validate the\r\nuser\'s \'D3DX9_42.dll\' file installation.");
            this.chkValidateD3D.UseVisualStyleBackColor = true;
            this.chkValidateD3D.Click += new System.EventHandler(this.chkValidateD3D_Click);
            // 
            // chkMacMode
            // 
            this.chkMacMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkMacMode.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkMacMode, 2);
            this.chkMacMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMacMode.Location = new System.Drawing.Point(3, 253);
            this.chkMacMode.Name = "chkMacMode";
            this.chkMacMode.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkMacMode.Size = new System.Drawing.Size(167, 19);
            this.chkMacMode.TabIndex = 20;
            this.chkMacMode.Text = "Mac Compatibility Mode";
            this.chkMacMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkMacMode, resources.GetString("chkMacMode.ToolTip"));
            this.chkMacMode.UseVisualStyleBackColor = true;
            this.chkMacMode.Click += new System.EventHandler(this.chkMacMode_Click);
            // 
            // txtCharterName
            // 
            this.txtCharterName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpSettings_Wrapper.SetColumnSpan(this.txtCharterName, 2);
            this.txtCharterName.Cue = "Enter a CDLC Charter\'s Name";
            this.txtCharterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCharterName.ForeColor = System.Drawing.Color.Gray;
            this.txtCharterName.Location = new System.Drawing.Point(12, 281);
            this.txtCharterName.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.txtCharterName.Name = "txtCharterName";
            this.txtCharterName.Size = new System.Drawing.Size(179, 20);
            this.txtCharterName.TabIndex = 9;
            this.toolTip.SetToolTip(this.txtCharterName, "Enter your charter name or the name\r\nof any charter you would like to show \r\nquic" +
                    "kly when checkbox \'Show My CDLC\' \r\nis checked in Song Manager, Custom Mods.\r\n");
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
        public CheckBox chkIncludeRS1CompSongs;
        private CueTextBox txtCharterName;
        private RadioButton rbCleanOnClosing;
        private ToolTip toolTip;
        private Button btnEmptyLogs;
        public CheckBox chkEnableAutoUpdate;
        private Button btnResetDownloads;
        private CheckBox chkEnableNotifications;
        public CheckBox chkIncludeRS2BaseSongs;
        public CheckBox chkIncludeCustomPacks;
        private CheckBox chkValidateD3D;
        private CheckBox chkMacMode;
        private CheckBox chkEnableQuarantine;
    }
}
