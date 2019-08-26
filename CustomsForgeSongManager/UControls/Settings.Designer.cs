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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.cueRsDir = new CustomControls.CueTextBox();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.rbCleanOnClosing = new System.Windows.Forms.RadioButton();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.btnResetDownloads = new System.Windows.Forms.Button();
            this.chkIncludeCustomPacks = new System.Windows.Forms.CheckBox();
            this.txtCharterName = new CustomControls.CueTextBox();
            this.chkMacMode = new System.Windows.Forms.CheckBox();
            this.chkValidateD3D = new System.Windows.Forms.CheckBox();
            this.chkEnableQuarantine = new System.Windows.Forms.CheckBox();
            this.chkEnableNotifications = new System.Windows.Forms.CheckBox();
            this.chkEnableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.chkIncludeArrangementData = new System.Windows.Forms.CheckBox();
            this.chkIncludeRS1CompSongs = new System.Windows.Forms.CheckBox();
            this.chkIncludeRS2BaseSongs = new System.Windows.Forms.CheckBox();
            this.btnResetThreading = new System.Windows.Forms.Button();
            this.btnEmptyLogs = new System.Windows.Forms.Button();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvColumns = new System.Windows.Forms.DataGridView();
            this.colVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDisplayIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHeaderText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
            this.lblDgvColumns = new System.Windows.Forms.Label();
            this.lblDgvSettingsPath = new System.Windows.Forms.Label();
            this.cueDgvSettingsPath = new CustomControls.CueTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tlpSettings_Wrapper.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpSettings_Wrapper
            // 
            this.tlpSettings_Wrapper.ColumnCount = 3;
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.88513F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.94813F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.16674F));
            this.tlpSettings_Wrapper.Controls.Add(this.cueRsDir, 2, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 0, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.rbCleanOnClosing, 0, 13);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 0, 17);
            this.tlpSettings_Wrapper.Controls.Add(this.btnResetDownloads, 1, 16);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeCustomPacks, 0, 4);
            this.tlpSettings_Wrapper.Controls.Add(this.txtCharterName, 0, 11);
            this.tlpSettings_Wrapper.Controls.Add(this.chkMacMode, 0, 10);
            this.tlpSettings_Wrapper.Controls.Add(this.chkValidateD3D, 0, 9);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableQuarantine, 0, 8);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableNotifications, 0, 7);
            this.tlpSettings_Wrapper.Controls.Add(this.chkEnableAutoUpdate, 0, 6);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeArrangementData, 0, 5);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS1CompSongs, 0, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.chkIncludeRS2BaseSongs, 0, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.btnResetThreading, 1, 15);
            this.tlpSettings_Wrapper.Controls.Add(this.btnEmptyLogs, 1, 17);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 0, 16);
            this.tlpSettings_Wrapper.Controls.Add(this.panel5, 2, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.lblDgvSettingsPath, 0, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.cueDgvSettingsPath, 2, 1);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 20;
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
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(866, 490);
            this.tlpSettings_Wrapper.TabIndex = 1;
            // 
            // cueRsDir
            // 
            this.cueRsDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cueRsDir.Cue = "Click here and specify Rocksmith installation directory";
            this.cueRsDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueRsDir.ForeColor = System.Drawing.Color.Gray;
            this.cueRsDir.Location = new System.Drawing.Point(347, 3);
            this.cueRsDir.Multiline = true;
            this.cueRsDir.Name = "cueRsDir";
            this.cueRsDir.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cueRsDir.Size = new System.Drawing.Size(516, 19);
            this.cueRsDir.TabIndex = 8;
            this.cueRsDir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cueRsDir_MouseClick);
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSettingsRSDir.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.lblSettingsRSDir, 2);
            this.lblSettingsRSDir.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingsRSDir.Location = new System.Drawing.Point(141, 3);
            this.lblSettingsRSDir.Margin = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Padding = new System.Windows.Forms.Padding(3);
            this.lblSettingsRSDir.Size = new System.Drawing.Size(200, 19);
            this.lblSettingsRSDir.TabIndex = 7;
            this.lblSettingsRSDir.Text = "RS2014 Installation Directory:";
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
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSettingsLoad.Image = global::CustomsForgeSongManager.Properties.Resources.load;
            this.btnSettingsLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsLoad.Location = new System.Drawing.Point(35, 428);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnSettingsLoad.Size = new System.Drawing.Size(118, 25);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Reload Settings  ";
            this.btnSettingsLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnSettingsLoad, "HINT: Resets the current grid settings to the \r\nlast saved grid setting condition" +
                    ". Great for\r\nfixing your mistakes. Do this before exiting\r\nthe Settings tabmenu " +
                    "to prevent auto saving\r\nany mistakes.");
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // btnResetDownloads
            // 
            this.btnResetDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetDownloads.Image = global::CustomsForgeSongManager.Properties.Resources.clear;
            this.btnResetDownloads.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetDownloads.Location = new System.Drawing.Point(192, 396);
            this.btnResetDownloads.Name = "btnResetDownloads";
            this.btnResetDownloads.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnResetDownloads.Size = new System.Drawing.Size(118, 25);
            this.btnResetDownloads.TabIndex = 13;
            this.btnResetDownloads.Text = "Reset DL Folder";
            this.btnResetDownloads.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnResetDownloads, "Reset the \'Downloads\' folder path to empty ...\r\nYou will be asked later on to spe" +
                    "cify the new path\r\nof the \'Downloads\' folder when it is needed.");
            this.btnResetDownloads.UseVisualStyleBackColor = true;
            this.btnResetDownloads.Click += new System.EventHandler(this.btnResetDownloads_Click);
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
            // txtCharterName
            // 
            this.txtCharterName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tlpSettings_Wrapper.SetColumnSpan(this.txtCharterName, 2);
            this.txtCharterName.Cue = "Enter a CDLC Charter\'s Name";
            this.txtCharterName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCharterName.ForeColor = System.Drawing.Color.Gray;
            this.txtCharterName.Location = new System.Drawing.Point(12, 309);
            this.txtCharterName.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.txtCharterName.Name = "txtCharterName";
            this.txtCharterName.Size = new System.Drawing.Size(179, 20);
            this.txtCharterName.TabIndex = 9;
            this.toolTip.SetToolTip(this.txtCharterName, "Enter your charter name or the name\r\nof any charter you would like to show \r\nquic" +
                    "kly when checkbox \'Show My CDLC\' \r\nis checked in Song Manager, Custom Mods.\r\n");
            this.txtCharterName.TextChanged += new System.EventHandler(this.tbCreator_TextChanged);
            // 
            // chkMacMode
            // 
            this.chkMacMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkMacMode.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkMacMode, 2);
            this.chkMacMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMacMode.Location = new System.Drawing.Point(3, 281);
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
            // chkValidateD3D
            // 
            this.chkValidateD3D.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkValidateD3D.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkValidateD3D, 2);
            this.chkValidateD3D.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkValidateD3D.Location = new System.Drawing.Point(3, 253);
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
            // chkEnableQuarantine
            // 
            this.chkEnableQuarantine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableQuarantine.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableQuarantine, 2);
            this.chkEnableQuarantine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableQuarantine.Location = new System.Drawing.Point(3, 225);
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
            // chkEnableNotifications
            // 
            this.chkEnableNotifications.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableNotifications.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableNotifications, 2);
            this.chkEnableNotifications.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableNotifications.Location = new System.Drawing.Point(3, 197);
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
            // chkEnableAutoUpdate
            // 
            this.chkEnableAutoUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkEnableAutoUpdate.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkEnableAutoUpdate, 2);
            this.chkEnableAutoUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableAutoUpdate.Location = new System.Drawing.Point(3, 169);
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
            // chkIncludeArrangementData
            // 
            this.chkIncludeArrangementData.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeArrangementData.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeArrangementData, 2);
            this.chkIncludeArrangementData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chkIncludeArrangementData.Location = new System.Drawing.Point(3, 141);
            this.chkIncludeArrangementData.Name = "chkIncludeArrangementData";
            this.chkIncludeArrangementData.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.chkIncludeArrangementData.Size = new System.Drawing.Size(227, 19);
            this.chkIncludeArrangementData.TabIndex = 22;
            this.chkIncludeArrangementData.Text = "Include Arrangement Analyzer Data";
            this.chkIncludeArrangementData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkIncludeArrangementData, "If checked, include complete\r\narrangement data with scan.  \r\n\r\nNOTE:  A full resc" +
                    "an of CDLC \r\ntakes 5x longer when checked.");
            this.chkIncludeArrangementData.UseVisualStyleBackColor = true;
            this.chkIncludeArrangementData.Click += new System.EventHandler(this.chkIncludeArrangementData_Click);
            // 
            // chkIncludeRS1CompSongs
            // 
            this.chkIncludeRS1CompSongs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeRS1CompSongs.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.chkIncludeRS1CompSongs, 2);
            this.chkIncludeRS1CompSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIncludeRS1CompSongs.Location = new System.Drawing.Point(3, 85);
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
            this.chkIncludeRS2BaseSongs.Location = new System.Drawing.Point(3, 57);
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
            // btnResetThreading
            // 
            this.btnResetThreading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetThreading.Image = global::CustomsForgeSongManager.Properties.Resources.restorewindow;
            this.btnResetThreading.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetThreading.Location = new System.Drawing.Point(192, 364);
            this.btnResetThreading.Name = "btnResetThreading";
            this.btnResetThreading.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnResetThreading.Size = new System.Drawing.Size(118, 25);
            this.btnResetThreading.TabIndex = 23;
            this.btnResetThreading.Text = "Reset Threading";
            this.btnResetThreading.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnResetThreading, "Reset CFSM CPU threading usage.\r\nYou will be asked later to specify the \r\ntype of" +
                    " threading used if needed.");
            this.btnResetThreading.UseVisualStyleBackColor = true;
            this.btnResetThreading.Click += new System.EventHandler(this.btnResetThreading_Click);
            // 
            // btnEmptyLogs
            // 
            this.btnEmptyLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEmptyLogs.Image = global::CustomsForgeSongManager.Properties.Resources.delete;
            this.btnEmptyLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmptyLogs.Location = new System.Drawing.Point(192, 428);
            this.btnEmptyLogs.Name = "btnEmptyLogs";
            this.btnEmptyLogs.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnEmptyLogs.Size = new System.Drawing.Size(118, 25);
            this.btnEmptyLogs.TabIndex = 11;
            this.btnEmptyLogs.Text = "Empty Log Files";
            this.btnEmptyLogs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnEmptyLogs, "Empty the log files");
            this.btnEmptyLogs.UseVisualStyleBackColor = true;
            this.btnEmptyLogs.Click += new System.EventHandler(this.btnEmptyLogs_Click);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSettingsSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSettingsSave.Image = global::CustomsForgeSongManager.Properties.Resources.save;
            this.btnSettingsSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSettingsSave.Location = new System.Drawing.Point(35, 396);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnSettingsSave.Size = new System.Drawing.Size(118, 25);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings  ";
            this.btnSettingsSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnSettingsSave, "Settings are saved automatically when\r\nyou exit the Setting tabmenu.  You can\r\nma" +
                    "nually save settings at any time by\r\nusing this button.");
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.dgvColumns);
            this.panel5.Controls.Add(this.lnkSelectAll);
            this.panel5.Controls.Add(this.lblDgvColumns);
            this.panel5.Location = new System.Drawing.Point(347, 56);
            this.panel5.Name = "panel5";
            this.tlpSettings_Wrapper.SetRowSpan(this.panel5, 18);
            this.panel5.Size = new System.Drawing.Size(516, 431);
            this.panel5.TabIndex = 4;
            // 
            // dgvColumns
            // 
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToDeleteRows = false;
            this.dgvColumns.AllowUserToResizeColumns = false;
            this.dgvColumns.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvColumns.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvColumns.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvColumns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVisible,
            this.colDisplayIndex,
            this.colWidth,
            this.colHeaderText,
            this.colColumnIndex,
            this.colColumnName});
            this.dgvColumns.Location = new System.Drawing.Point(3, 26);
            this.dgvColumns.MultiSelect = false;
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvColumns.Size = new System.Drawing.Size(510, 402);
            this.dgvColumns.TabIndex = 7;
            this.dgvColumns.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvColumns_CellEndEdit);
            this.dgvColumns.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvColumns_CellValidating);
            // 
            // colVisible
            // 
            this.colVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colVisible.DataPropertyName = "Visible";
            this.colVisible.HeaderText = "Visible";
            this.colVisible.Name = "colVisible";
            this.colVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colVisible.ToolTipText = "Click to Sort (Read/Write)";
            this.colVisible.Width = 62;
            // 
            // colDisplayIndex
            // 
            this.colDisplayIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colDisplayIndex.DataPropertyName = "DisplayIndex";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.colDisplayIndex.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDisplayIndex.HeaderText = "DisplayIndex";
            this.colDisplayIndex.Name = "colDisplayIndex";
            this.colDisplayIndex.ToolTipText = "Click to Sort (Read/Write)\r\nEither manually edit \'DisplayIndex\', or\r\ndrag and dro" +
                "p a \'HeaderText\' row\r\nto change grid column display order";
            this.colDisplayIndex.Width = 92;
            // 
            // colWidth
            // 
            this.colWidth.DataPropertyName = "Width";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.colWidth.DefaultCellStyle = dataGridViewCellStyle4;
            this.colWidth.HeaderText = "Width";
            this.colWidth.Name = "colWidth";
            this.colWidth.ToolTipText = "Click to Sort (Read/Write)";
            this.colWidth.Width = 80;
            // 
            // colHeaderText
            // 
            this.colHeaderText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHeaderText.DataPropertyName = "HeaderText";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colHeaderText.DefaultCellStyle = dataGridViewCellStyle5;
            this.colHeaderText.HeaderText = "HeaderText";
            this.colHeaderText.Name = "colHeaderText";
            this.colHeaderText.ReadOnly = true;
            this.colHeaderText.ToolTipText = "Click to Sort (Read Only)\r\nDrag and drop a \'HeaderText\' row\r\nto change grid colum" +
                "n display order";
            // 
            // colColumnIndex
            // 
            this.colColumnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colColumnIndex.DataPropertyName = "ColumnIndex";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N0";
            dataGridViewCellStyle6.NullValue = null;
            this.colColumnIndex.DefaultCellStyle = dataGridViewCellStyle6;
            this.colColumnIndex.HeaderText = "ColumnIndex";
            this.colColumnIndex.Name = "colColumnIndex";
            this.colColumnIndex.ReadOnly = true;
            this.colColumnIndex.ToolTipText = "Click to Sort\r\n(For Reference Only)";
            this.colColumnIndex.Width = 93;
            // 
            // colColumnName
            // 
            this.colColumnName.DataPropertyName = "ColumnName";
            this.colColumnName.HeaderText = "ColumnName";
            this.colColumnName.Name = "colColumnName";
            this.colColumnName.ReadOnly = true;
            this.colColumnName.ToolTipText = "Click to Sort (Read Only)";
            this.colColumnName.Visible = false;
            this.colColumnName.Width = 98;
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.Location = new System.Drawing.Point(3, 5);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(98, 13);
            this.lnkSelectAll.TabIndex = 6;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "Deselect/Select All";
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
            this.lblDgvColumns.Size = new System.Drawing.Size(169, 13);
            this.lblDgvColumns.TabIndex = 1;
            this.lblDgvColumns.Text = "Grid Settings for {0} from {1}";
            this.lblDgvColumns.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDgvSettingsPath
            // 
            this.lblDgvSettingsPath.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDgvSettingsPath.AutoSize = true;
            this.tlpSettings_Wrapper.SetColumnSpan(this.lblDgvSettingsPath, 2);
            this.lblDgvSettingsPath.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDgvSettingsPath.Location = new System.Drawing.Point(188, 29);
            this.lblDgvSettingsPath.Margin = new System.Windows.Forms.Padding(3);
            this.lblDgvSettingsPath.Name = "lblDgvSettingsPath";
            this.lblDgvSettingsPath.Padding = new System.Windows.Forms.Padding(3);
            this.lblDgvSettingsPath.Size = new System.Drawing.Size(153, 20);
            this.lblDgvSettingsPath.TabIndex = 24;
            this.lblDgvSettingsPath.Text = "Grid Settings File Path:";
            this.lblDgvSettingsPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.lblDgvSettingsPath, resources.GetString("lblDgvSettingsPath.ToolTip"));
            // 
            // cueDgvSettingsPath
            // 
            this.cueDgvSettingsPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cueDgvSettingsPath.Cue = "Click here to override the setttings selection and manually choose a grid setting" +
                "s file to load and edit";
            this.cueDgvSettingsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueDgvSettingsPath.ForeColor = System.Drawing.Color.Gray;
            this.cueDgvSettingsPath.Location = new System.Drawing.Point(347, 28);
            this.cueDgvSettingsPath.Multiline = true;
            this.cueDgvSettingsPath.Name = "cueDgvSettingsPath";
            this.cueDgvSettingsPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cueDgvSettingsPath.Size = new System.Drawing.Size(516, 19);
            this.cueDgvSettingsPath.TabIndex = 25;
            this.toolTip.SetToolTip(this.cueDgvSettingsPath, "The grid settings files are initially created\r\nwhen the matching tabmenu is selec" +
                    "ted.\r\nSubsequently the grid settings file path\r\nwill then be available for selec" +
                    "tion here.");
            this.cueDgvSettingsPath.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cueDgvSettingsPath_MouseClick);
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tlpSettings_Wrapper;
        private Button btnSettingsLoad;
        private Panel panel5;
        private LinkLabel lnkSelectAll;
        private Label lblDgvColumns;
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
        public CheckBox chkIncludeArrangementData;
        private Button btnResetThreading;
        private DataGridView dgvColumns;
        private Label lblDgvSettingsPath;
        private CueTextBox cueDgvSettingsPath;
        private DataGridViewCheckBoxColumn colVisible;
        private DataGridViewTextBoxColumn colDisplayIndex;
        private DataGridViewTextBoxColumn colWidth;
        private DataGridViewTextBoxColumn colHeaderText;
        private DataGridViewTextBoxColumn colColumnIndex;
        private DataGridViewTextBoxColumn colColumnName;
    }
}
