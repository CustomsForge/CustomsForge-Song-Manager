namespace CustomsForgeManager_Winforms.Forms
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlp_MainForm_Wrappper = new System.Windows.Forms.TableLayoutPanel();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpManager = new System.Windows.Forms.TabPage();
            this.tlpSongListWrapper = new System.Windows.Forms.TableLayoutPanel();
            this.listSongs = new System.Windows.Forms.ListView();
            this.colBackup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPreview = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTuning = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArrangements = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUpdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNewVer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelSongListButtons = new System.Windows.Forms.Panel();
            this.btnSaveDLC = new System.Windows.Forms.Button();
            this.btnBackupDLC = new System.Windows.Forms.Button();
            this.btnDLCPage = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnEditDLC = new System.Windows.Forms.Button();
            this.tpEditor = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.tbSettingsRSDir = new System.Windows.Forms.TextBox();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lnkAboutCF = new System.Windows.Forms.LinkLabel();
            this.pbAboutLogo = new System.Windows.Forms.PictureBox();
            this.bWorker = new System.ComponentModel.BackgroundWorker();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarMain = new System.Windows.Forms.ToolStripProgressBar();
            this.timerAutoUpdate = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip_MainManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDLCPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tlp_MainForm_Wrappper.SuspendLayout();
            this.gbLog.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpManager.SuspendLayout();
            this.tlpSongListWrapper.SuspendLayout();
            this.panelSongListButtons.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tlpSettings_Wrapper.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutLogo)).BeginInit();
            this.statusStripMain.SuspendLayout();
            this.contextMenuStrip_MainManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_MainForm_Wrappper
            // 
            this.tlp_MainForm_Wrappper.ColumnCount = 1;
            this.tlp_MainForm_Wrappper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_MainForm_Wrappper.Controls.Add(this.gbLog, 0, 1);
            this.tlp_MainForm_Wrappper.Controls.Add(this.tcMain, 0, 0);
            this.tlp_MainForm_Wrappper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_MainForm_Wrappper.Location = new System.Drawing.Point(0, 0);
            this.tlp_MainForm_Wrappper.Name = "tlp_MainForm_Wrappper";
            this.tlp_MainForm_Wrappper.RowCount = 2;
            this.tlp_MainForm_Wrappper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlp_MainForm_Wrappper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_MainForm_Wrappper.Size = new System.Drawing.Size(784, 562);
            this.tlp_MainForm_Wrappper.TabIndex = 0;
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.tbLog);
            this.gbLog.Location = new System.Drawing.Point(3, 424);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(778, 113);
            this.gbLog.TabIndex = 1;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Log";
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.Window;
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(772, 94);
            this.tbLog.TabIndex = 0;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpManager);
            this.tcMain.Controls.Add(this.tpEditor);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Controls.Add(this.tpAbout);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(3, 3);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(778, 415);
            this.tcMain.TabIndex = 2;
            // 
            // tpManager
            // 
            this.tpManager.BackColor = System.Drawing.SystemColors.Control;
            this.tpManager.Controls.Add(this.tlpSongListWrapper);
            this.tpManager.Location = new System.Drawing.Point(4, 22);
            this.tpManager.Name = "tpManager";
            this.tpManager.Padding = new System.Windows.Forms.Padding(3);
            this.tpManager.Size = new System.Drawing.Size(770, 389);
            this.tpManager.TabIndex = 0;
            this.tpManager.Text = "CLDC Manager";
            // 
            // tlpSongListWrapper
            // 
            this.tlpSongListWrapper.ColumnCount = 1;
            this.tlpSongListWrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSongListWrapper.Controls.Add(this.listSongs, 0, 0);
            this.tlpSongListWrapper.Controls.Add(this.panelSongListButtons, 0, 1);
            this.tlpSongListWrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSongListWrapper.Name = "tlpSongListWrapper";
            this.tlpSongListWrapper.RowCount = 2;
            this.tlpSongListWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.31017F));
            this.tlpSongListWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.68984F));
            this.tlpSongListWrapper.Size = new System.Drawing.Size(765, 393);
            this.tlpSongListWrapper.TabIndex = 2;
            // 
            // listSongs
            // 
            this.listSongs.CheckBoxes = true;
            this.listSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBackup,
            this.colPreview,
            this.colArtist,
            this.colSong,
            this.colAlbum,
            this.colTuning,
            this.colArrangements,
            this.colUpdated,
            this.colUser,
            this.colNewVer});
            this.listSongs.ContextMenuStrip = this.contextMenuStrip_MainManager;
            this.listSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSongs.FullRowSelect = true;
            this.listSongs.GridLines = true;
            this.listSongs.HideSelection = false;
            this.listSongs.Location = new System.Drawing.Point(3, 3);
            this.listSongs.Name = "listSongs";
            this.listSongs.Size = new System.Drawing.Size(759, 317);
            this.listSongs.TabIndex = 1;
            this.listSongs.UseCompatibleStateImageBehavior = false;
            this.listSongs.View = System.Windows.Forms.View.Details;
            // 
            // colBackup
            // 
            this.colBackup.Text = "Backup";
            // 
            // colPreview
            // 
            this.colPreview.Text = "Preview";
            this.colPreview.Width = 84;
            // 
            // colArtist
            // 
            this.colArtist.Text = "Artist";
            this.colArtist.Width = 84;
            // 
            // colSong
            // 
            this.colSong.Text = "Song";
            this.colSong.Width = 84;
            // 
            // colAlbum
            // 
            this.colAlbum.Text = "Album";
            this.colAlbum.Width = 84;
            // 
            // colTuning
            // 
            this.colTuning.Text = "Tuning";
            this.colTuning.Width = 84;
            // 
            // colArrangements
            // 
            this.colArrangements.Text = "Arrangements";
            this.colArrangements.Width = 84;
            // 
            // colUpdated
            // 
            this.colUpdated.Text = "Updated";
            this.colUpdated.Width = 84;
            // 
            // colUser
            // 
            this.colUser.Text = "Creator";
            this.colUser.Width = 84;
            // 
            // colNewVer
            // 
            this.colNewVer.Text = "New version available";
            this.colNewVer.Width = 84;
            // 
            // panelSongListButtons
            // 
            this.panelSongListButtons.Controls.Add(this.btnSaveDLC);
            this.panelSongListButtons.Controls.Add(this.btnBackupDLC);
            this.panelSongListButtons.Controls.Add(this.btnDLCPage);
            this.panelSongListButtons.Controls.Add(this.btnRescan);
            this.panelSongListButtons.Controls.Add(this.btnEditDLC);
            this.panelSongListButtons.Location = new System.Drawing.Point(3, 326);
            this.panelSongListButtons.Name = "panelSongListButtons";
            this.panelSongListButtons.Size = new System.Drawing.Size(759, 64);
            this.panelSongListButtons.TabIndex = 2;
            // 
            // btnSaveDLC
            // 
            this.btnSaveDLC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveDLC.Enabled = false;
            this.btnSaveDLC.Location = new System.Drawing.Point(594, 18);
            this.btnSaveDLC.Name = "btnSaveDLC";
            this.btnSaveDLC.Size = new System.Drawing.Size(150, 27);
            this.btnSaveDLC.TabIndex = 7;
            this.btnSaveDLC.Text = "Save DLC";
            this.btnSaveDLC.UseVisualStyleBackColor = true;
            // 
            // btnBackupDLC
            // 
            this.btnBackupDLC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBackupDLC.Enabled = false;
            this.btnBackupDLC.Location = new System.Drawing.Point(438, 18);
            this.btnBackupDLC.Name = "btnBackupDLC";
            this.btnBackupDLC.Size = new System.Drawing.Size(150, 27);
            this.btnBackupDLC.TabIndex = 6;
            this.btnBackupDLC.Text = "Backup DLC";
            this.btnBackupDLC.UseVisualStyleBackColor = true;
            // 
            // btnDLCPage
            // 
            this.btnDLCPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDLCPage.Enabled = false;
            this.btnDLCPage.Location = new System.Drawing.Point(282, 18);
            this.btnDLCPage.Name = "btnDLCPage";
            this.btnDLCPage.Size = new System.Drawing.Size(150, 27);
            this.btnDLCPage.TabIndex = 5;
            this.btnDLCPage.Text = "Open DLC Page";
            this.btnDLCPage.UseVisualStyleBackColor = true;
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRescan.Location = new System.Drawing.Point(17, 18);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(60, 27);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // btnEditDLC
            // 
            this.btnEditDLC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditDLC.Enabled = false;
            this.btnEditDLC.Location = new System.Drawing.Point(126, 18);
            this.btnEditDLC.Name = "btnEditDLC";
            this.btnEditDLC.Size = new System.Drawing.Size(150, 27);
            this.btnEditDLC.TabIndex = 4;
            this.btnEditDLC.Text = "Edit DLC";
            this.btnEditDLC.UseVisualStyleBackColor = true;
            // 
            // tpEditor
            // 
            this.tpEditor.Location = new System.Drawing.Point(4, 22);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Size = new System.Drawing.Size(770, 389);
            this.tpEditor.TabIndex = 3;
            this.tpEditor.Text = "CDLC Editor";
            this.tpEditor.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tpSettings.Controls.Add(this.tlpSettings_Wrapper);
            this.tpSettings.Location = new System.Drawing.Point(4, 22);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(770, 389);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            // 
            // tlpSettings_Wrapper
            // 
            this.tlpSettings_Wrapper.ColumnCount = 3;
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.56544F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.089F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.47644F));
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 2, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 1, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.tbSettingsRSDir, 2, 1);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(3, 3);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 4;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.832898F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.93472F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.97128F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(764, 383);
            this.tlpSettings_Wrapper.TabIndex = 0;
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSettingsSave.Location = new System.Drawing.Point(165, 340);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(150, 23);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSettingsLoad.Location = new System.Drawing.Point(499, 340);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Size = new System.Drawing.Size(150, 23);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Load Settings";
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.AutoSize = true;
            this.lblSettingsRSDir.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSettingsRSDir.Location = new System.Drawing.Point(110, 20);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Size = new System.Drawing.Size(272, 28);
            this.lblSettingsRSDir.TabIndex = 1;
            this.lblSettingsRSDir.Text = "Rocksmith installation directory (double-click to change):";
            this.lblSettingsRSDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSettingsRSDir
            // 
            this.tbSettingsRSDir.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbSettingsRSDir.Location = new System.Drawing.Point(388, 24);
            this.tbSettingsRSDir.Name = "tbSettingsRSDir";
            this.tbSettingsRSDir.Size = new System.Drawing.Size(373, 20);
            this.tbSettingsRSDir.TabIndex = 2;
            this.tbSettingsRSDir.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbSettingsRSDir_MouseDoubleClick);
            // 
            // tpAbout
            // 
            this.tpAbout.BackColor = System.Drawing.SystemColors.Control;
            this.tpAbout.Controls.Add(this.tableLayoutPanel1);
            this.tpAbout.Location = new System.Drawing.Point(4, 22);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Size = new System.Drawing.Size(770, 389);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lnkAboutCF, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbAboutLogo, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(770, 389);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lnkAboutCF
            // 
            this.lnkAboutCF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkAboutCF.AutoSize = true;
            this.lnkAboutCF.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkAboutCF.Location = new System.Drawing.Point(489, 84);
            this.lnkAboutCF.Name = "lnkAboutCF";
            this.lnkAboutCF.Size = new System.Drawing.Size(177, 25);
            this.lnkAboutCF.TabIndex = 0;
            this.lnkAboutCF.TabStop = true;
            this.lnkAboutCF.Text = "CustomsForge 2015";
            this.lnkAboutCF.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAboutCF_LinkClicked);
            // 
            // pbAboutLogo
            // 
            this.pbAboutLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbAboutLogo.Image = global::CustomsForgeManager_Winforms.Properties.Resources.logo_black;
            this.pbAboutLogo.Location = new System.Drawing.Point(452, 263);
            this.pbAboutLogo.Name = "pbAboutLogo";
            this.pbAboutLogo.Size = new System.Drawing.Size(251, 56);
            this.pbAboutLogo.TabIndex = 1;
            this.pbAboutLogo.TabStop = false;
            // 
            // bWorker
            // 
            this.bWorker.WorkerReportsProgress = true;
            // 
            // timerMain
            // 
            this.timerMain.Interval = 1000;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarMain});
            this.statusStripMain.Location = new System.Drawing.Point(0, 540);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(784, 22);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripProgressBarMain
            // 
            this.toolStripProgressBarMain.Name = "toolStripProgressBarMain";
            this.toolStripProgressBarMain.Size = new System.Drawing.Size(200, 16);
            // 
            // timerAutoUpdate
            // 
            this.timerAutoUpdate.Enabled = true;
            this.timerAutoUpdate.Interval = 600000;
            this.timerAutoUpdate.Tick += new System.EventHandler(this.timerAutoUpdate_Tick);
            // 
            // contextMenuStrip_MainManager
            // 
            this.contextMenuStrip_MainManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDLCPageToolStripMenuItem,
            this.editDLCToolStripMenuItem});
            this.contextMenuStrip_MainManager.Name = "contextMenuStrip_MainManager";
            this.contextMenuStrip_MainManager.Size = new System.Drawing.Size(158, 48);
            // 
            // openDLCPageToolStripMenuItem
            // 
            this.openDLCPageToolStripMenuItem.Name = "openDLCPageToolStripMenuItem";
            this.openDLCPageToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.openDLCPageToolStripMenuItem.Text = "Open DLC Page";
            // 
            // editDLCToolStripMenuItem
            // 
            this.editDLCToolStripMenuItem.Name = "editDLCToolStripMenuItem";
            this.editDLCToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.editDLCToolStripMenuItem.Text = "Edit DLC";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tlp_MainForm_Wrappper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomsForge Manager";
            this.tlp_MainForm_Wrappper.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpManager.ResumeLayout(false);
            this.tlpSongListWrapper.ResumeLayout(false);
            this.panelSongListButtons.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tlpSettings_Wrapper.ResumeLayout(false);
            this.tlpSettings_Wrapper.PerformLayout();
            this.tpAbout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutLogo)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.contextMenuStrip_MainManager.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_MainForm_Wrappper;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpManager;
        private System.Windows.Forms.TabPage tpSettings;
        private System.Windows.Forms.TabPage tpAbout;
        private System.ComponentModel.BackgroundWorker bWorker;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel lnkAboutCF;
        private System.Windows.Forms.PictureBox pbAboutLogo;
        private System.Windows.Forms.Timer timerAutoUpdate;
        private System.Windows.Forms.TableLayoutPanel tlpSettings_Wrapper;
        private System.Windows.Forms.Button btnSettingsSave;
        private System.Windows.Forms.Button btnSettingsLoad;
        private System.Windows.Forms.Label lblSettingsRSDir;
        private System.Windows.Forms.TextBox tbSettingsRSDir;
        private System.Windows.Forms.TabPage tpEditor;
        private System.Windows.Forms.TableLayoutPanel tlpSongListWrapper;
        private System.Windows.Forms.ListView listSongs;
        private System.Windows.Forms.ColumnHeader colPreview;
        private System.Windows.Forms.ColumnHeader colArtist;
        private System.Windows.Forms.ColumnHeader colSong;
        private System.Windows.Forms.ColumnHeader colAlbum;
        private System.Windows.Forms.ColumnHeader colTuning;
        private System.Windows.Forms.ColumnHeader colArrangements;
        private System.Windows.Forms.ColumnHeader colUpdated;
        private System.Windows.Forms.ColumnHeader colUser;
        private System.Windows.Forms.ColumnHeader colNewVer;
        private System.Windows.Forms.Panel panelSongListButtons;
        private System.Windows.Forms.Button btnSaveDLC;
        private System.Windows.Forms.Button btnBackupDLC;
        private System.Windows.Forms.Button btnDLCPage;
        private System.Windows.Forms.Button btnEditDLC;
        private System.Windows.Forms.ColumnHeader colBackup;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_MainManager;
        private System.Windows.Forms.ToolStripMenuItem openDLCPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editDLCToolStripMenuItem;
    }
}

