using System.ComponentModel;
using System.Windows.Forms;

namespace CustomsForgeManager.Forms
{
    // TODO: improve loading efficiency through seperation of code into forms/classes
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.timerAutoUpdate = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon_Main = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_Tray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpSongManager = new System.Windows.Forms.TabPage();
            this.tpDuplicates = new System.Windows.Forms.TabPage();
            this.tpRenamer = new System.Windows.Forms.TabPage();
            this.tpSetlistManager = new System.Windows.Forms.TabPage();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.toolStripUtilities = new System.Windows.Forms.ToolStrip();
            this.tsBtnLaunchRS = new System.Windows.Forms.ToolStripButton();
            this.tsBtnBackup = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBtnUpload = new System.Windows.Forms.ToolStripButton();
            this.tsBtnRequest = new System.Windows.Forms.ToolStripButton();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.tsLabel_ShowHideLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsProgressBar_Main = new System.Windows.Forms.ToolStripProgressBar();
            this.tsLabel_ClearLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel_MainMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel_StatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel_Cancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLabel_DisabledCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.gbLog.SuspendLayout();
            this.contextMenuStrip_Tray.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.toolStripUtilities.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.tbLog);
            this.gbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLog.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.gbLog.Location = new System.Drawing.Point(0, 0);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(150, 46);
            this.gbLog.TabIndex = 1;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Log";
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.SystemColors.Window;
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(144, 27);
            this.tbLog.TabIndex = 0;
            // 
            // timerMain
            // 
            this.timerMain.Interval = 1000;
            // 
            // timerAutoUpdate
            // 
            this.timerAutoUpdate.Enabled = true;
            this.timerAutoUpdate.Interval = 600000;
            // 
            // notifyIcon_Main
            // 
            this.notifyIcon_Main.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon_Main.ContextMenuStrip = this.contextMenuStrip_Tray;
            this.notifyIcon_Main.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon_Main.Icon")));
            this.notifyIcon_Main.Text = "Notification";
            this.notifyIcon_Main.Visible = true;
            // 
            // contextMenuStrip_Tray
            // 
            this.contextMenuStrip_Tray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.contextMenuStrip_Tray.Name = "contextMenuStrip_Tray";
            this.contextMenuStrip_Tray.Size = new System.Drawing.Size(104, 26);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::CustomsForgeManager.Properties.Resources.close;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.toolStripContainer1);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.gbLog);
            this.scMain.Panel2Collapsed = true;
            this.scMain.Size = new System.Drawing.Size(1011, 636);
            this.scMain.SplitterDistance = 526;
            this.scMain.TabIndex = 2;
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tcMain);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1011, 611);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1011, 636);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripUtilities);
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpSongManager);
            this.tcMain.Controls.Add(this.tpDuplicates);
            this.tcMain.Controls.Add(this.tpRenamer);
            this.tcMain.Controls.Add(this.tpSetlistManager);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Controls.Add(this.tpAbout);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1011, 611);
            this.tcMain.TabIndex = 2;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpSongManager
            // 
            this.tpSongManager.Location = new System.Drawing.Point(4, 25);
            this.tpSongManager.Name = "tpSongManager";
            this.tpSongManager.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tpSongManager.Size = new System.Drawing.Size(1003, 582);
            this.tpSongManager.TabIndex = 0;
            this.tpSongManager.Text = "Song Manager";
            this.tpSongManager.UseVisualStyleBackColor = true;
            // 
            // tpDuplicates
            // 
            this.tpDuplicates.Location = new System.Drawing.Point(4, 25);
            this.tpDuplicates.Name = "tpDuplicates";
            this.tpDuplicates.Size = new System.Drawing.Size(1003, 582);
            this.tpDuplicates.TabIndex = 4;
            this.tpDuplicates.Text = "Duplicates";
            this.tpDuplicates.UseVisualStyleBackColor = true;
            // 
            // tpRenamer
            // 
            this.tpRenamer.Location = new System.Drawing.Point(4, 25);
            this.tpRenamer.Name = "tpRenamer";
            this.tpRenamer.Padding = new System.Windows.Forms.Padding(3);
            this.tpRenamer.Size = new System.Drawing.Size(1003, 582);
            this.tpRenamer.TabIndex = 6;
            this.tpRenamer.Text = "Renamer";
            this.tpRenamer.UseVisualStyleBackColor = true;
            // 
            // tpSetlistManager
            // 
            this.tpSetlistManager.Location = new System.Drawing.Point(4, 25);
            this.tpSetlistManager.Name = "tpSetlistManager";
            this.tpSetlistManager.Size = new System.Drawing.Size(1003, 582);
            this.tpSetlistManager.TabIndex = 7;
            this.tpSetlistManager.Text = "Setlist Manger";
            this.tpSetlistManager.UseVisualStyleBackColor = true;
            // 
            // tpSettings
            // 
            this.tpSettings.Location = new System.Drawing.Point(4, 25);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(1003, 582);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            this.tpSettings.UseVisualStyleBackColor = true;
            // 
            // tpAbout
            // 
            this.tpAbout.Location = new System.Drawing.Point(4, 25);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Size = new System.Drawing.Size(1003, 582);
            this.tpAbout.TabIndex = 8;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            // 
            // toolStripUtilities
            // 
            this.toolStripUtilities.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripUtilities.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnLaunchRS,
            this.toolStripSeparator1,
            this.tsBtnBackup,
            this.toolStripSeparator2,
            this.tsBtnUpload,
            this.toolStripSeparator3,
            this.tsBtnRequest});
            this.toolStripUtilities.Location = new System.Drawing.Point(3, 0);
            this.toolStripUtilities.Name = "toolStripUtilities";
            this.toolStripUtilities.Size = new System.Drawing.Size(347, 25);
            this.toolStripUtilities.TabIndex = 0;
            // 
            // tsBtnLaunchRS
            // 
            this.tsBtnLaunchRS.Image = global::CustomsForgeManager.Properties.Resources.launch;
            this.tsBtnLaunchRS.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnLaunchRS.Name = "tsBtnLaunchRS";
            this.tsBtnLaunchRS.Size = new System.Drawing.Size(112, 22);
            this.tsBtnLaunchRS.Text = "Launch Rocksmith";
            this.tsBtnLaunchRS.Click += new System.EventHandler(this.tsBtnLaunchRS_Click);
            // 
            // tsBtnBackup
            // 
            this.tsBtnBackup.Image = global::CustomsForgeManager.Properties.Resources.backup;
            this.tsBtnBackup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnBackup.Name = "tsBtnBackup";
            this.tsBtnBackup.Size = new System.Drawing.Size(94, 22);
            this.tsBtnBackup.Text = "Backup Profile";
            this.tsBtnBackup.Click += new System.EventHandler(this.tsBtnBackup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBtnUpload
            // 
            this.tsBtnUpload.Image = global::CustomsForgeManager.Properties.Resources.upload;
            this.tsBtnUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUpload.Name = "tsBtnUpload";
            this.tsBtnUpload.Size = new System.Drawing.Size(60, 22);
            this.tsBtnUpload.Text = "Upload";
            this.tsBtnUpload.ToolTipText = "Upload CDLC to CustomsForge";
            this.tsBtnUpload.Click += new System.EventHandler(this.tsBtnUpload_Click);
            // 
            // tsBtnRequest
            // 
            this.tsBtnRequest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnRequest.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnRequest.Image")));
            this.tsBtnRequest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRequest.Name = "tsBtnRequest";
            this.tsBtnRequest.Size = new System.Drawing.Size(51, 22);
            this.tsBtnRequest.Text = "Request";
            this.tsBtnRequest.ToolTipText = "Request a song on CustomsForge";
            this.tsBtnRequest.Click += new System.EventHandler(this.tsBtnRequest_Click);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLabel_ShowHideLog,
            this.tsProgressBar_Main,
            this.tsLabel_ClearLog,
            this.tsLabel_MainMsg,
            this.tsLabel_StatusMsg,
            this.tsLabel_Cancel,
            this.tsLabel_DisabledCounter});
            this.statusStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripMain.Location = new System.Drawing.Point(0, 636);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1011, 23);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // tsLabel_ShowHideLog
            // 
            this.tsLabel_ShowHideLog.AutoSize = false;
            this.tsLabel_ShowHideLog.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_ShowHideLog.IsLink = true;
            this.tsLabel_ShowHideLog.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsLabel_ShowHideLog.Name = "tsLabel_ShowHideLog";
            this.tsLabel_ShowHideLog.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_ShowHideLog.Size = new System.Drawing.Size(68, 18);
            this.tsLabel_ShowHideLog.Text = "Show Log ";
            this.tsLabel_ShowHideLog.Click += new System.EventHandler(this.tsLabelShowHideLog_Click);
            // 
            // tsProgressBar_Main
            // 
            this.tsProgressBar_Main.Name = "tsProgressBar_Main";
            this.tsProgressBar_Main.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsProgressBar_Main.Size = new System.Drawing.Size(408, 17);
            // 
            // tsLabel_ClearLog
            // 
            this.tsLabel_ClearLog.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_ClearLog.IsLink = true;
            this.tsLabel_ClearLog.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsLabel_ClearLog.Name = "tsLabel_ClearLog";
            this.tsLabel_ClearLog.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_ClearLog.Size = new System.Drawing.Size(64, 18);
            this.tsLabel_ClearLog.Text = "Clear log";
            this.tsLabel_ClearLog.Click += new System.EventHandler(this.tsLabelClearLog_Click);
            // 
            // tsLabel_MainMsg
            // 
            this.tsLabel_MainMsg.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_MainMsg.Margin = new System.Windows.Forms.Padding(9, 3, 0, 2);
            this.tsLabel_MainMsg.Name = "tsLabel_MainMsg";
            this.tsLabel_MainMsg.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_MainMsg.Size = new System.Drawing.Size(42, 18);
            this.tsLabel_MainMsg.Text = "Main";
            this.tsLabel_MainMsg.ToolTipText = "Main";
            // 
            // tsLabel_StatusMsg
            // 
            this.tsLabel_StatusMsg.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_StatusMsg.Name = "tsLabel_StatusMsg";
            this.tsLabel_StatusMsg.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_StatusMsg.Size = new System.Drawing.Size(50, 18);
            this.tsLabel_StatusMsg.Spring = true;
            this.tsLabel_StatusMsg.Text = "Status";
            // 
            // tsLabel_Cancel
            // 
            this.tsLabel_Cancel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_Cancel.IsLink = true;
            this.tsLabel_Cancel.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tsLabel_Cancel.Name = "tsLabel_Cancel";
            this.tsLabel_Cancel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_Cancel.Size = new System.Drawing.Size(53, 18);
            this.tsLabel_Cancel.Text = "Cancel";
            this.tsLabel_Cancel.Click += new System.EventHandler(this.tsLabelCancel_Click);
            // 
            // tsLabel_DisabledCounter
            // 
            this.tsLabel_DisabledCounter.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsLabel_DisabledCounter.Name = "tsLabel_DisabledCounter";
            this.tsLabel_DisabledCounter.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tsLabel_DisabledCounter.Size = new System.Drawing.Size(65, 18);
            this.tsLabel_DisabledCounter.Text = "Disabled:";
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 150);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // frmMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1011, 659);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.statusStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomsForge Song Manager";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.contextMenuStrip_Tray.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.toolStripUtilities.ResumeLayout(false);
            this.toolStripUtilities.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox gbLog;
        private Timer timerMain;
        private Timer timerAutoUpdate;
        private NotifyIcon notifyIcon_Main;
        private ContextMenuStrip contextMenuStrip_Tray;
        private ToolStripMenuItem closeToolStripMenuItem;
        private SplitContainer scMain;
        public TextBox tbLog;
        private ToolStripContainer toolStripContainer1;
        private ToolStripPanel BottomToolStripPanel;
        private StatusStrip statusStripMain;
        public ToolStripStatusLabel tsLabel_ShowHideLog;
        public ToolStripProgressBar tsProgressBar_Main;
        public ToolStripStatusLabel tsLabel_ClearLog;
        public ToolStripStatusLabel tsLabel_MainMsg;
        public ToolStripStatusLabel tsLabel_StatusMsg;
        public ToolStripStatusLabel tsLabel_Cancel;
        public ToolStripStatusLabel tsLabel_DisabledCounter;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private TabControl tcMain;
        private TabPage tpSongManager;
        private TabPage tpDuplicates;
        private TabPage tpRenamer;
        private TabPage tpSetlistManager;
        private TabPage tpSettings;
        private TabPage tpAbout;
        private ToolStrip toolStripUtilities;
        private ToolStripButton tsBtnLaunchRS;
        private ToolStripButton tsBtnBackup;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsBtnUpload;
        private ToolStripButton tsBtnRequest;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
    }
}