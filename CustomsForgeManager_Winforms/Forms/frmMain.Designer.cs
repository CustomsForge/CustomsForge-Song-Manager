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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlp_MainForm_Wrappper = new System.Windows.Forms.TableLayoutPanel();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpManager = new System.Windows.Forms.TabPage();
            this.gb_Main_Grid = new System.Windows.Forms.GroupBox();
            this.linkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip_MainManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDLCInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDLCPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDLCLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAuthorNameStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSongToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupDLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Main_Search = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.link_MainClearResults = new System.Windows.Forms.LinkLabel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gb_Main_Actions = new System.Windows.Forms.GroupBox();
            this.panelSongListButtons = new System.Windows.Forms.Panel();
            this.btnBackupSelectedDLCs = new System.Windows.Forms.Button();
            this.btnBatchRenamer = new System.Windows.Forms.Button();
            this.radioBtn_ExportToHTML = new System.Windows.Forms.RadioButton();
            this.radioBtn_ExportToCSV = new System.Windows.Forms.RadioButton();
            this.btnExportSongList = new System.Windows.Forms.Button();
            this.lbl_ExportTo = new System.Windows.Forms.Label();
            this.radioBtn_ExportToBBCode = new System.Windows.Forms.RadioButton();
            this.btnDisableEnableSongs = new System.Windows.Forms.Button();
            this.btnCheckAllForUpdates = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.tpDuplicates = new System.Windows.Forms.TabPage();
            this.tlpDuplicates = new System.Windows.Forms.TableLayoutPanel();
            this.listDupeSongs = new System.Windows.Forms.ListView();
            this.colDupeSelect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeLastUpdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeSongPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDisableEnableSong = new System.Windows.Forms.Button();
            this.btnDupeRescan = new System.Windows.Forms.Button();
            this.btnDeleteDupeSong = new System.Windows.Forms.Button();
            this.tpBatchRenamer = new System.Windows.Forms.TabPage();
            this.deleteEmptyDirCheckBox = new System.Windows.Forms.CheckBox();
            this.slashLabel = new System.Windows.Forms.Label();
            this.propertiesGroupBox = new System.Windows.Forms.GroupBox();
            this.renamerPropertyDataGridView = new System.Windows.Forms.DataGridView();
            this.howToGroupBox = new System.Windows.Forms.GroupBox();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.renameTemplateLabel = new System.Windows.Forms.Label();
            this.renameTemplateTextBox = new System.Windows.Forms.TextBox();
            this.renameAllButton = new System.Windows.Forms.Button();
            this.tpUtilities = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gb_Utilities = new System.Windows.Forms.GroupBox();
            this.btnLaunchSteam = new System.Windows.Forms.Button();
            this.btnBackupRSProfile = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.checkRescanOnStartup = new System.Windows.Forms.CheckBox();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.checkIncludeRS1DLC = new System.Windows.Forms.CheckBox();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.tpCreators = new System.Windows.Forms.TabPage();
            this.btnRSTKSite = new System.Windows.Forms.Button();
            this.btnEOFSite = new System.Windows.Forms.Button();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxCF = new System.Windows.Forms.PictureBox();
            this.group_CFquickLinks = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_CFQuicklinks = new System.Windows.Forms.TableLayoutPanel();
            this.linkOpenCFHomePage = new System.Windows.Forms.LinkLabel();
            this.linkOpenIgnition = new System.Windows.Forms.LinkLabel();
            this.linkOpenRequests = new System.Windows.Forms.LinkLabel();
            this.linkDontainsPage = new System.Windows.Forms.LinkLabel();
            this.linkOpenCFVideos = new System.Windows.Forms.LinkLabel();
            this.linkCFFAQ = new System.Windows.Forms.LinkLabel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_AppVersion = new System.Windows.Forms.Label();
            this.link_CFManager = new System.Windows.Forms.LinkLabel();
            this.lnk_ReleaseNotes = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel_Credits = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lbl_Credits = new System.Windows.Forms.Label();
            this.lbl_Credits_Description = new System.Windows.Forms.Label();
            this.lbl_UnleashedRole = new System.Windows.Forms.Label();
            this.lbl_DarjuszRole = new System.Windows.Forms.Label();
            this.lbl_LovromanRole = new System.Windows.Forms.Label();
            this.lbl_ZerkzRole = new System.Windows.Forms.Label();
            this.link_UnleashedProfile = new System.Windows.Forms.LinkLabel();
            this.link_DarjuszProfile = new System.Windows.Forms.LinkLabel();
            this.link_LovromanProfile = new System.Windows.Forms.LinkLabel();
            this.link_ZerkzProfile = new System.Windows.Forms.LinkLabel();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarMain = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_ClearLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_MainCancel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSpringer = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_DisabledCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerAutoUpdate = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog_SettingsRSPath = new System.Windows.Forms.FolderBrowserDialog();
            this.sfdSongListToCSV = new System.Windows.Forms.SaveFileDialog();
            this.notifyIcon_Main = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_Tray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelAlphaTesters = new System.Windows.Forms.Label();
            this.tbSettingsRSDir = new System.Windows.Forms.TextBox();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.checkEnableLogBaloon = new System.Windows.Forms.CheckBox();
            this.listDisabledColumns = new System.Windows.Forms.ListView();
            this.columnSelect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSettingsColumnEnabled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblDisabledColumns = new System.Windows.Forms.Label();
            this.btnEnableColumns = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgvSongs = new CustomsForgeManager_Winforms.Controls.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.frmMainBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tlp_MainForm_Wrappper.SuspendLayout();
            this.gbLog.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpManager.SuspendLayout();
            this.gb_Main_Grid.SuspendLayout();
            this.contextMenuStrip_MainManager.SuspendLayout();
            this.gb_Main_Search.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gb_Main_Actions.SuspendLayout();
            this.panelSongListButtons.SuspendLayout();
            this.tpDuplicates.SuspendLayout();
            this.tlpDuplicates.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpBatchRenamer.SuspendLayout();
            this.propertiesGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataGridView)).BeginInit();
            this.howToGroupBox.SuspendLayout();
            this.tpUtilities.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.gb_Utilities.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tlpSettings_Wrapper.SuspendLayout();
            this.tpCreators.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCF)).BeginInit();
            this.group_CFquickLinks.SuspendLayout();
            this.tableLayoutPanel_CFQuicklinks.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel_Credits.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.contextMenuStrip_Tray.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmMainBindingSource)).BeginInit();
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
            this.tlp_MainForm_Wrappper.Size = new System.Drawing.Size(1011, 629);
            this.tlp_MainForm_Wrappper.TabIndex = 0;
            // 
            // gbLog
            // 
            this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLog.Controls.Add(this.tbLog);
            this.gbLog.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.gbLog.Location = new System.Drawing.Point(3, 474);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(1005, 152);
            this.gbLog.TabIndex = 1;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Log";
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.BackColor = System.Drawing.SystemColors.Window;
            this.tbLog.Location = new System.Drawing.Point(3, 16);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(999, 94);
            this.tbLog.TabIndex = 0;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpManager);
            this.tcMain.Controls.Add(this.tpDuplicates);
            this.tcMain.Controls.Add(this.tpBatchRenamer);
            this.tcMain.Controls.Add(this.tpUtilities);
            this.tcMain.Controls.Add(this.tpSettings);
            this.tcMain.Controls.Add(this.tpCreators);
            this.tcMain.Controls.Add(this.tpAbout);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMain.Location = new System.Drawing.Point(3, 3);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1005, 465);
            this.tcMain.TabIndex = 2;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpManager
            // 
            this.tpManager.BackColor = System.Drawing.SystemColors.Control;
            this.tpManager.Controls.Add(this.gb_Main_Grid);
            this.tpManager.Controls.Add(this.gb_Main_Search);
            this.tpManager.Controls.Add(this.gb_Main_Actions);
            this.tpManager.Location = new System.Drawing.Point(4, 25);
            this.tpManager.Name = "tpManager";
            this.tpManager.Padding = new System.Windows.Forms.Padding(3);
            this.tpManager.Size = new System.Drawing.Size(997, 436);
            this.tpManager.TabIndex = 0;
            this.tpManager.Text = "Song Manager";
            // 
            // gb_Main_Grid
            // 
            this.gb_Main_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Grid.Controls.Add(this.linkLblSelectAll);
            this.gb_Main_Grid.Controls.Add(this.dgvSongs);
            this.gb_Main_Grid.Location = new System.Drawing.Point(6, 79);
            this.gb_Main_Grid.Name = "gb_Main_Grid";
            this.gb_Main_Grid.Size = new System.Drawing.Size(985, 261);
            this.gb_Main_Grid.TabIndex = 5;
            this.gb_Main_Grid.TabStop = false;
            this.gb_Main_Grid.Text = "Results Grid:";
            // 
            // linkLblSelectAll
            // 
            this.linkLblSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLblSelectAll.AutoSize = true;
            this.linkLblSelectAll.ForeColor = System.Drawing.Color.Black;
            this.linkLblSelectAll.LinkColor = System.Drawing.Color.Black;
            this.linkLblSelectAll.Location = new System.Drawing.Point(6, 233);
            this.linkLblSelectAll.Name = "linkLblSelectAll";
            this.linkLblSelectAll.Size = new System.Drawing.Size(113, 16);
            this.linkLblSelectAll.TabIndex = 2;
            this.linkLblSelectAll.TabStop = true;
            this.linkLblSelectAll.Text = "Select All/Deselect All";
            this.linkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblSelectAll_LinkClicked);
            // 
            // contextMenuStrip_MainManager
            // 
            this.contextMenuStrip_MainManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDLCInfoToolStripMenuItem,
            this.openDLCPageToolStripMenuItem,
            this.checkForUpdateToolStripMenuItem,
            this.openDLCLocationToolStripMenuItem,
            this.editDLCToolStripMenuItem,
            this.getAuthorNameStripMenuItem,
            this.deleteSongToolStripMenuItem,
            this.backupDLCToolStripMenuItem});
            this.contextMenuStrip_MainManager.Name = "contextMenuStrip_MainManager";
            this.contextMenuStrip_MainManager.Size = new System.Drawing.Size(178, 180);
            // 
            // showDLCInfoToolStripMenuItem
            // 
            this.showDLCInfoToolStripMenuItem.Name = "showDLCInfoToolStripMenuItem";
            this.showDLCInfoToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.showDLCInfoToolStripMenuItem.Text = "Show DLC Info";
            this.showDLCInfoToolStripMenuItem.Click += new System.EventHandler(this.showDLCInfoToolStripMenuItem_Click);
            // 
            // openDLCPageToolStripMenuItem
            // 
            this.openDLCPageToolStripMenuItem.Name = "openDLCPageToolStripMenuItem";
            this.openDLCPageToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openDLCPageToolStripMenuItem.Text = "Open DLC Page";
            this.openDLCPageToolStripMenuItem.Click += new System.EventHandler(this.openDLCPageToolStripMenuItem_Click);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // openDLCLocationToolStripMenuItem
            // 
            this.openDLCLocationToolStripMenuItem.Name = "openDLCLocationToolStripMenuItem";
            this.openDLCLocationToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openDLCLocationToolStripMenuItem.Text = "Open DLC Location";
            this.openDLCLocationToolStripMenuItem.Click += new System.EventHandler(this.openDLCLocationToolStripMenuItem_Click);
            // 
            // editDLCToolStripMenuItem
            // 
            this.editDLCToolStripMenuItem.Name = "editDLCToolStripMenuItem";
            this.editDLCToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.editDLCToolStripMenuItem.Text = "Edit DLC";
            this.editDLCToolStripMenuItem.Click += new System.EventHandler(this.editDLCToolStripMenuItem_Click);
            // 
            // getAuthorNameStripMenuItem
            // 
            this.getAuthorNameStripMenuItem.Name = "getAuthorNameStripMenuItem";
            this.getAuthorNameStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.getAuthorNameStripMenuItem.Text = "Get author name";
            this.getAuthorNameStripMenuItem.Click += new System.EventHandler(this.getAuthorNameStripMenuItem_Click);
            // 
            // deleteSongToolStripMenuItem
            // 
            this.deleteSongToolStripMenuItem.Name = "deleteSongToolStripMenuItem";
            this.deleteSongToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.deleteSongToolStripMenuItem.Text = "Delete song";
            this.deleteSongToolStripMenuItem.Click += new System.EventHandler(this.deleteSongToolStripMenuItem_Click);
            // 
            // backupDLCToolStripMenuItem
            // 
            this.backupDLCToolStripMenuItem.Name = "backupDLCToolStripMenuItem";
            this.backupDLCToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.backupDLCToolStripMenuItem.Text = "Backup DLC";
            this.backupDLCToolStripMenuItem.Click += new System.EventHandler(this.backupDLCToolStripMenuItem_Click);
            // 
            // gb_Main_Search
            // 
            this.gb_Main_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Search.Controls.Add(this.panel3);
            this.gb_Main_Search.Location = new System.Drawing.Point(6, 6);
            this.gb_Main_Search.Name = "gb_Main_Search";
            this.gb_Main_Search.Size = new System.Drawing.Size(985, 67);
            this.gb_Main_Search.TabIndex = 4;
            this.gb_Main_Search.TabStop = false;
            this.gb_Main_Search.Text = "Search:";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.link_MainClearResults);
            this.panel3.Controls.Add(this.lbl_Search);
            this.panel3.Controls.Add(this.tbSearch);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Location = new System.Drawing.Point(6, 19);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(973, 35);
            this.panel3.TabIndex = 5;
            // 
            // link_MainClearResults
            // 
            this.link_MainClearResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.link_MainClearResults.AutoSize = true;
            this.link_MainClearResults.ForeColor = System.Drawing.Color.DimGray;
            this.link_MainClearResults.LinkColor = System.Drawing.Color.Black;
            this.link_MainClearResults.Location = new System.Drawing.Point(894, 9);
            this.link_MainClearResults.Name = "link_MainClearResults";
            this.link_MainClearResults.Size = new System.Drawing.Size(71, 16);
            this.link_MainClearResults.TabIndex = 3;
            this.link_MainClearResults.TabStop = true;
            this.link_MainClearResults.Text = "Clear Search";
            this.link_MainClearResults.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_MainClearResults_LinkClicked);
            // 
            // lbl_Search
            // 
            this.lbl_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Search.AutoSize = true;
            this.lbl_Search.Location = new System.Drawing.Point(3, 9);
            this.lbl_Search.Name = "lbl_Search";
            this.lbl_Search.Size = new System.Drawing.Size(64, 16);
            this.lbl_Search.TabIndex = 2;
            this.lbl_Search.Text = "Search for:";
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(68, 6);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(739, 20);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(813, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 21);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gb_Main_Actions
            // 
            this.gb_Main_Actions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Actions.Controls.Add(this.panelSongListButtons);
            this.gb_Main_Actions.Location = new System.Drawing.Point(6, 346);
            this.gb_Main_Actions.Name = "gb_Main_Actions";
            this.gb_Main_Actions.Size = new System.Drawing.Size(985, 84);
            this.gb_Main_Actions.TabIndex = 3;
            this.gb_Main_Actions.TabStop = false;
            this.gb_Main_Actions.Text = "Actions:";
            // 
            // panelSongListButtons
            // 
            this.panelSongListButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSongListButtons.Controls.Add(this.btnBackupSelectedDLCs);
            this.panelSongListButtons.Controls.Add(this.btnBatchRenamer);
            this.panelSongListButtons.Controls.Add(this.radioBtn_ExportToHTML);
            this.panelSongListButtons.Controls.Add(this.radioBtn_ExportToCSV);
            this.panelSongListButtons.Controls.Add(this.btnExportSongList);
            this.panelSongListButtons.Controls.Add(this.lbl_ExportTo);
            this.panelSongListButtons.Controls.Add(this.radioBtn_ExportToBBCode);
            this.panelSongListButtons.Controls.Add(this.btnDisableEnableSongs);
            this.panelSongListButtons.Controls.Add(this.btnCheckAllForUpdates);
            this.panelSongListButtons.Controls.Add(this.btnRescan);
            this.panelSongListButtons.Location = new System.Drawing.Point(6, 19);
            this.panelSongListButtons.Name = "panelSongListButtons";
            this.panelSongListButtons.Size = new System.Drawing.Size(973, 52);
            this.panelSongListButtons.TabIndex = 3;
            // 
            // btnBackupSelectedDLCs
            // 
            this.btnBackupSelectedDLCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBackupSelectedDLCs.Location = new System.Drawing.Point(472, 15);
            this.btnBackupSelectedDLCs.Name = "btnBackupSelectedDLCs";
            this.btnBackupSelectedDLCs.Size = new System.Drawing.Size(101, 23);
            this.btnBackupSelectedDLCs.TabIndex = 19;
            this.btnBackupSelectedDLCs.Text = "Backup selected";
            this.btnBackupSelectedDLCs.UseVisualStyleBackColor = true;
            this.btnBackupSelectedDLCs.Click += new System.EventHandler(this.btnBackupSelectedDLCs_Click);
            // 
            // btnBatchRenamer
            // 
            this.btnBatchRenamer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchRenamer.Location = new System.Drawing.Point(375, 15);
            this.btnBatchRenamer.Name = "btnBatchRenamer";
            this.btnBatchRenamer.Size = new System.Drawing.Size(91, 23);
            this.btnBatchRenamer.TabIndex = 14;
            this.btnBatchRenamer.Text = "Batch Renamer";
            this.btnBatchRenamer.UseVisualStyleBackColor = true;
            // 
            // radioBtn_ExportToHTML
            // 
            this.radioBtn_ExportToHTML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioBtn_ExportToHTML.AutoSize = true;
            this.radioBtn_ExportToHTML.Location = new System.Drawing.Point(800, 16);
            this.radioBtn_ExportToHTML.Name = "radioBtn_ExportToHTML";
            this.radioBtn_ExportToHTML.Size = new System.Drawing.Size(53, 20);
            this.radioBtn_ExportToHTML.TabIndex = 18;
            this.radioBtn_ExportToHTML.TabStop = true;
            this.radioBtn_ExportToHTML.Text = "HTML";
            this.radioBtn_ExportToHTML.UseVisualStyleBackColor = true;
            // 
            // radioBtn_ExportToCSV
            // 
            this.radioBtn_ExportToCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioBtn_ExportToCSV.AutoSize = true;
            this.radioBtn_ExportToCSV.Location = new System.Drawing.Point(859, 16);
            this.radioBtn_ExportToCSV.Name = "radioBtn_ExportToCSV";
            this.radioBtn_ExportToCSV.Size = new System.Drawing.Size(45, 20);
            this.radioBtn_ExportToCSV.TabIndex = 17;
            this.radioBtn_ExportToCSV.TabStop = true;
            this.radioBtn_ExportToCSV.Text = "CSV";
            this.radioBtn_ExportToCSV.UseVisualStyleBackColor = true;
            // 
            // btnExportSongList
            // 
            this.btnExportSongList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportSongList.Location = new System.Drawing.Point(910, 15);
            this.btnExportSongList.Name = "btnExportSongList";
            this.btnExportSongList.Size = new System.Drawing.Size(55, 23);
            this.btnExportSongList.TabIndex = 12;
            this.btnExportSongList.Text = "Export";
            this.btnExportSongList.UseVisualStyleBackColor = true;
            this.btnExportSongList.Click += new System.EventHandler(this.btnExportSongList_Click);
            // 
            // lbl_ExportTo
            // 
            this.lbl_ExportTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ExportTo.AutoSize = true;
            this.lbl_ExportTo.Location = new System.Drawing.Point(665, 18);
            this.lbl_ExportTo.Name = "lbl_ExportTo";
            this.lbl_ExportTo.Size = new System.Drawing.Size(60, 16);
            this.lbl_ExportTo.TabIndex = 16;
            this.lbl_ExportTo.Text = "Export to:";
            // 
            // radioBtn_ExportToBBCode
            // 
            this.radioBtn_ExportToBBCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioBtn_ExportToBBCode.AutoSize = true;
            this.radioBtn_ExportToBBCode.Location = new System.Drawing.Point(731, 16);
            this.radioBtn_ExportToBBCode.Name = "radioBtn_ExportToBBCode";
            this.radioBtn_ExportToBBCode.Size = new System.Drawing.Size(63, 20);
            this.radioBtn_ExportToBBCode.TabIndex = 15;
            this.radioBtn_ExportToBBCode.TabStop = true;
            this.radioBtn_ExportToBBCode.Text = "BBCode";
            this.radioBtn_ExportToBBCode.UseVisualStyleBackColor = true;
            // 
            // btnDisableEnableSongs
            // 
            this.btnDisableEnableSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDisableEnableSongs.Location = new System.Drawing.Point(249, 15);
            this.btnDisableEnableSongs.Name = "btnDisableEnableSongs";
            this.btnDisableEnableSongs.Size = new System.Drawing.Size(120, 23);
            this.btnDisableEnableSongs.TabIndex = 14;
            this.btnDisableEnableSongs.Text = "Enable/Disable songs";
            this.btnDisableEnableSongs.UseVisualStyleBackColor = true;
            this.btnDisableEnableSongs.Click += new System.EventHandler(this.btnDisableEnableSongs_Click);
            // 
            // btnCheckAllForUpdates
            // 
            this.btnCheckAllForUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckAllForUpdates.Location = new System.Drawing.Point(86, 13);
            this.btnCheckAllForUpdates.Name = "btnCheckAllForUpdates";
            this.btnCheckAllForUpdates.Size = new System.Drawing.Size(120, 27);
            this.btnCheckAllForUpdates.TabIndex = 4;
            this.btnCheckAllForUpdates.Text = "Check All for Update";
            this.btnCheckAllForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckAllForUpdates.Click += new System.EventHandler(this.btnCheckAllForUpdates_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRescan.Location = new System.Drawing.Point(20, 13);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(60, 27);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // tpDuplicates
            // 
            this.tpDuplicates.Controls.Add(this.tlpDuplicates);
            this.tpDuplicates.Location = new System.Drawing.Point(4, 25);
            this.tpDuplicates.Name = "tpDuplicates";
            this.tpDuplicates.Size = new System.Drawing.Size(997, 436);
            this.tpDuplicates.TabIndex = 4;
            this.tpDuplicates.Text = "Duplicates";
            this.tpDuplicates.UseVisualStyleBackColor = true;
            // 
            // tlpDuplicates
            // 
            this.tlpDuplicates.ColumnCount = 1;
            this.tlpDuplicates.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDuplicates.Controls.Add(this.listDupeSongs, 0, 0);
            this.tlpDuplicates.Controls.Add(this.panel1, 0, 1);
            this.tlpDuplicates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDuplicates.Location = new System.Drawing.Point(0, 0);
            this.tlpDuplicates.Name = "tlpDuplicates";
            this.tlpDuplicates.RowCount = 2;
            this.tlpDuplicates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.73267F));
            this.tlpDuplicates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.26733F));
            this.tlpDuplicates.Size = new System.Drawing.Size(997, 436);
            this.tlpDuplicates.TabIndex = 2;
            // 
            // listDupeSongs
            // 
            this.listDupeSongs.CheckBoxes = true;
            this.listDupeSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDupeSelect,
            this.colDupeEnabled,
            this.colDupeArtist,
            this.colDupeSong,
            this.colDupeAlbum,
            this.colDupeLastUpdated,
            this.colDupeSongPath});
            this.listDupeSongs.ContextMenuStrip = this.contextMenuStrip_MainManager;
            this.listDupeSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDupeSongs.FullRowSelect = true;
            this.listDupeSongs.GridLines = true;
            this.listDupeSongs.HideSelection = false;
            this.listDupeSongs.Location = new System.Drawing.Point(3, 3);
            this.listDupeSongs.Name = "listDupeSongs";
            this.listDupeSongs.Size = new System.Drawing.Size(991, 345);
            this.listDupeSongs.TabIndex = 2;
            this.listDupeSongs.UseCompatibleStateImageBehavior = false;
            this.listDupeSongs.View = System.Windows.Forms.View.Details;
            // 
            // colDupeSelect
            // 
            this.colDupeSelect.Text = "Select";
            // 
            // colDupeEnabled
            // 
            this.colDupeEnabled.Text = "Enabled";
            // 
            // colDupeArtist
            // 
            this.colDupeArtist.Text = "Artist";
            this.colDupeArtist.Width = 82;
            // 
            // colDupeSong
            // 
            this.colDupeSong.Text = "Song";
            this.colDupeSong.Width = 96;
            // 
            // colDupeAlbum
            // 
            this.colDupeAlbum.Text = "Album";
            this.colDupeAlbum.Width = 85;
            // 
            // colDupeLastUpdated
            // 
            this.colDupeLastUpdated.Text = "Last Updated";
            this.colDupeLastUpdated.Width = 103;
            // 
            // colDupeSongPath
            // 
            this.colDupeSongPath.Text = "Song Path";
            this.colDupeSongPath.Width = 284;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDisableEnableSong);
            this.panel1.Controls.Add(this.btnDupeRescan);
            this.panel1.Controls.Add(this.btnDeleteDupeSong);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 354);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 79);
            this.panel1.TabIndex = 0;
            // 
            // btnDisableEnableSong
            // 
            this.btnDisableEnableSong.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDisableEnableSong.Location = new System.Drawing.Point(417, 25);
            this.btnDisableEnableSong.Name = "btnDisableEnableSong";
            this.btnDisableEnableSong.Size = new System.Drawing.Size(150, 27);
            this.btnDisableEnableSong.TabIndex = 10;
            this.btnDisableEnableSong.Text = "Enable/disable song";
            this.btnDisableEnableSong.UseVisualStyleBackColor = true;
            this.btnDisableEnableSong.Click += new System.EventHandler(this.btnDisableEnableSong_Click);
            // 
            // btnDupeRescan
            // 
            this.btnDupeRescan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDupeRescan.Location = new System.Drawing.Point(261, 25);
            this.btnDupeRescan.Name = "btnDupeRescan";
            this.btnDupeRescan.Size = new System.Drawing.Size(150, 27);
            this.btnDupeRescan.TabIndex = 9;
            this.btnDupeRescan.Text = "Rescan duplicates";
            this.btnDupeRescan.UseVisualStyleBackColor = true;
            this.btnDupeRescan.Click += new System.EventHandler(this.btnDupeRescan_Click);
            // 
            // btnDeleteDupeSong
            // 
            this.btnDeleteDupeSong.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteDupeSong.Location = new System.Drawing.Point(573, 25);
            this.btnDeleteDupeSong.Name = "btnDeleteDupeSong";
            this.btnDeleteDupeSong.Size = new System.Drawing.Size(150, 27);
            this.btnDeleteDupeSong.TabIndex = 8;
            this.btnDeleteDupeSong.Text = "Delete song";
            this.btnDeleteDupeSong.UseVisualStyleBackColor = true;
            this.btnDeleteDupeSong.Click += new System.EventHandler(this.btnDeleteSongOne_Click);
            // 
            // tpBatchRenamer
            // 
            this.tpBatchRenamer.Controls.Add(this.deleteEmptyDirCheckBox);
            this.tpBatchRenamer.Controls.Add(this.slashLabel);
            this.tpBatchRenamer.Controls.Add(this.propertiesGroupBox);
            this.tpBatchRenamer.Controls.Add(this.howToGroupBox);
            this.tpBatchRenamer.Controls.Add(this.renameTemplateLabel);
            this.tpBatchRenamer.Controls.Add(this.renameTemplateTextBox);
            this.tpBatchRenamer.Controls.Add(this.renameAllButton);
            this.tpBatchRenamer.Location = new System.Drawing.Point(4, 25);
            this.tpBatchRenamer.Name = "tpBatchRenamer";
            this.tpBatchRenamer.Padding = new System.Windows.Forms.Padding(3);
            this.tpBatchRenamer.Size = new System.Drawing.Size(997, 436);
            this.tpBatchRenamer.TabIndex = 6;
            this.tpBatchRenamer.Text = "Batch Renamer";
            this.tpBatchRenamer.UseVisualStyleBackColor = true;
            // 
            // deleteEmptyDirCheckBox
            // 
            this.deleteEmptyDirCheckBox.AutoSize = true;
            this.deleteEmptyDirCheckBox.Location = new System.Drawing.Point(161, 38);
            this.deleteEmptyDirCheckBox.Name = "deleteEmptyDirCheckBox";
            this.deleteEmptyDirCheckBox.Size = new System.Drawing.Size(223, 20);
            this.deleteEmptyDirCheckBox.TabIndex = 10;
            this.deleteEmptyDirCheckBox.Text = "Delete Empty Directories after Rename";
            this.deleteEmptyDirCheckBox.UseVisualStyleBackColor = true;
            // 
            // slashLabel
            // 
            this.slashLabel.AutoSize = true;
            this.slashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slashLabel.Location = new System.Drawing.Point(118, 10);
            this.slashLabel.Name = "slashLabel";
            this.slashLabel.Size = new System.Drawing.Size(37, 20);
            this.slashLabel.TabIndex = 9;
            this.slashLabel.Text = "dlc/";
            // 
            // propertiesGroupBox
            // 
            this.propertiesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertiesGroupBox.Controls.Add(this.renamerPropertyDataGridView);
            this.propertiesGroupBox.Location = new System.Drawing.Point(440, 58);
            this.propertiesGroupBox.Name = "propertiesGroupBox";
            this.propertiesGroupBox.Size = new System.Drawing.Size(551, 372);
            this.propertiesGroupBox.TabIndex = 8;
            this.propertiesGroupBox.TabStop = false;
            this.propertiesGroupBox.Text = "Usuable Properties";
            // 
            // renamerPropertyDataGridView
            // 
            this.renamerPropertyDataGridView.AllowUserToAddRows = false;
            this.renamerPropertyDataGridView.AllowUserToDeleteRows = false;
            this.renamerPropertyDataGridView.AllowUserToResizeColumns = false;
            this.renamerPropertyDataGridView.AllowUserToResizeRows = false;
            this.renamerPropertyDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.renamerPropertyDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.renamerPropertyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.renamerPropertyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renamerPropertyDataGridView.Location = new System.Drawing.Point(3, 16);
            this.renamerPropertyDataGridView.MultiSelect = false;
            this.renamerPropertyDataGridView.Name = "renamerPropertyDataGridView";
            this.renamerPropertyDataGridView.ReadOnly = true;
            this.renamerPropertyDataGridView.RowHeadersVisible = false;
            this.renamerPropertyDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.renamerPropertyDataGridView.Size = new System.Drawing.Size(545, 353);
            this.renamerPropertyDataGridView.TabIndex = 2;
            // 
            // howToGroupBox
            // 
            this.howToGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.howToGroupBox.Controls.Add(this.instructionsLabel);
            this.howToGroupBox.Controls.Add(this.label1);
            this.howToGroupBox.Location = new System.Drawing.Point(9, 58);
            this.howToGroupBox.Name = "howToGroupBox";
            this.howToGroupBox.Size = new System.Drawing.Size(428, 372);
            this.howToGroupBox.TabIndex = 7;
            this.howToGroupBox.TabStop = false;
            this.howToGroupBox.Text = "How To Use:";
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instructionsLabel.Location = new System.Drawing.Point(3, 16);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.instructionsLabel.Size = new System.Drawing.Size(354, 304);
            this.instructionsLabel.TabIndex = 1;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 0;
            // 
            // renameTemplateLabel
            // 
            this.renameTemplateLabel.AutoSize = true;
            this.renameTemplateLabel.Location = new System.Drawing.Point(15, 12);
            this.renameTemplateLabel.Name = "renameTemplateLabel";
            this.renameTemplateLabel.Size = new System.Drawing.Size(98, 16);
            this.renameTemplateLabel.TabIndex = 6;
            this.renameTemplateLabel.Text = "Rename Template:";
            // 
            // renameTemplateTextBox
            // 
            this.renameTemplateTextBox.Location = new System.Drawing.Point(161, 12);
            this.renameTemplateTextBox.Name = "renameTemplateTextBox";
            this.renameTemplateTextBox.Size = new System.Drawing.Size(379, 20);
            this.renameTemplateTextBox.TabIndex = 5;
            this.renameTemplateTextBox.Text = "<title>_<artist>";
            // 
            // renameAllButton
            // 
            this.renameAllButton.Location = new System.Drawing.Point(546, 8);
            this.renameAllButton.Name = "renameAllButton";
            this.renameAllButton.Size = new System.Drawing.Size(218, 30);
            this.renameAllButton.TabIndex = 4;
            this.renameAllButton.Text = "Rename All";
            this.renameAllButton.UseVisualStyleBackColor = true;
            this.renameAllButton.Click += new System.EventHandler(this.renameAllButton_Click);
            // 
            // tpUtilities
            // 
            this.tpUtilities.Controls.Add(this.tableLayoutPanel2);
            this.tpUtilities.Location = new System.Drawing.Point(4, 25);
            this.tpUtilities.Name = "tpUtilities";
            this.tpUtilities.Size = new System.Drawing.Size(997, 436);
            this.tpUtilities.TabIndex = 5;
            this.tpUtilities.Text = "Utilities";
            this.tpUtilities.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 436F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(997, 436);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gb_Utilities);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(991, 430);
            this.panel4.TabIndex = 0;
            // 
            // gb_Utilities
            // 
            this.gb_Utilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Utilities.Controls.Add(this.btnLaunchSteam);
            this.gb_Utilities.Controls.Add(this.btnBackupRSProfile);
            this.gb_Utilities.Location = new System.Drawing.Point(3, 3);
            this.gb_Utilities.Name = "gb_Utilities";
            this.gb_Utilities.Size = new System.Drawing.Size(985, 42);
            this.gb_Utilities.TabIndex = 6;
            this.gb_Utilities.TabStop = false;
            this.gb_Utilities.Text = "Utilities";
            // 
            // btnLaunchSteam
            // 
            this.btnLaunchSteam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunchSteam.Location = new System.Drawing.Point(666, 12);
            this.btnLaunchSteam.Name = "btnLaunchSteam";
            this.btnLaunchSteam.Size = new System.Drawing.Size(157, 26);
            this.btnLaunchSteam.TabIndex = 4;
            this.btnLaunchSteam.Text = "Launch Rocksmith via Steam";
            this.btnLaunchSteam.UseVisualStyleBackColor = true;
            this.btnLaunchSteam.Click += new System.EventHandler(this.btnLaunchSteam_Click);
            // 
            // btnBackupRSProfile
            // 
            this.btnBackupRSProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackupRSProfile.Location = new System.Drawing.Point(829, 12);
            this.btnBackupRSProfile.Name = "btnBackupRSProfile";
            this.btnBackupRSProfile.Size = new System.Drawing.Size(150, 26);
            this.btnBackupRSProfile.TabIndex = 2;
            this.btnBackupRSProfile.Text = "Backup Rocksmith Profile";
            this.btnBackupRSProfile.UseVisualStyleBackColor = true;
            this.btnBackupRSProfile.Click += new System.EventHandler(this.btnBackupRSProfile_Click);
            // 
            // tpSettings
            // 
            this.tpSettings.BackColor = System.Drawing.SystemColors.Control;
            this.tpSettings.Controls.Add(this.tlpSettings_Wrapper);
            this.tpSettings.Location = new System.Drawing.Point(4, 25);
            this.tpSettings.Name = "tpSettings";
            this.tpSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpSettings.Size = new System.Drawing.Size(997, 436);
            this.tpSettings.TabIndex = 1;
            this.tpSettings.Text = "Settings";
            // 
            // tlpSettings_Wrapper
            // 
            this.tlpSettings_Wrapper.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tlpSettings_Wrapper.ColumnCount = 2;
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpSettings_Wrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 0, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.tbSettingsRSDir, 1, 0);
            this.tlpSettings_Wrapper.Controls.Add(this.checkRescanOnStartup, 0, 3);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 0, 5);
            this.tlpSettings_Wrapper.Controls.Add(this.checkIncludeRS1DLC, 0, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.panel5, 1, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.checkEnableLogBaloon, 0, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 5);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(3, 3);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 6;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(991, 430);
            this.tlpSettings_Wrapper.TabIndex = 0;
            // 
            // checkRescanOnStartup
            // 
            this.checkRescanOnStartup.AutoSize = true;
            this.checkRescanOnStartup.Checked = true;
            this.checkRescanOnStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRescanOnStartup.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkRescanOnStartup.Location = new System.Drawing.Point(176, 134);
            this.checkRescanOnStartup.Name = "checkRescanOnStartup";
            this.checkRescanOnStartup.Size = new System.Drawing.Size(118, 35);
            this.checkRescanOnStartup.TabIndex = 3;
            this.checkRescanOnStartup.Text = "Rescan on startup";
            this.checkRescanOnStartup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRescanOnStartup.UseVisualStyleBackColor = true;
            // 
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSettingsLoad.Location = new System.Drawing.Point(5, 387);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Size = new System.Drawing.Size(150, 38);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Load Settings";
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // checkIncludeRS1DLC
            // 
            this.checkIncludeRS1DLC.AutoSize = true;
            this.checkIncludeRS1DLC.Checked = true;
            this.checkIncludeRS1DLC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeRS1DLC.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkIncludeRS1DLC.Location = new System.Drawing.Point(115, 91);
            this.checkIncludeRS1DLC.Name = "checkIncludeRS1DLC";
            this.checkIncludeRS1DLC.Size = new System.Drawing.Size(179, 35);
            this.checkIncludeRS1DLC.TabIndex = 3;
            this.checkIncludeRS1DLC.Text = "Include RS1 Compatibility Pack";
            this.checkIncludeRS1DLC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkIncludeRS1DLC.UseVisualStyleBackColor = true;
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.BackColor = System.Drawing.Color.Chartreuse;
            this.btnSettingsSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSettingsSave.Location = new System.Drawing.Point(836, 387);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(150, 38);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.UseVisualStyleBackColor = false;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
            // 
            // tpCreators
            // 
            this.tpCreators.BackgroundImage = global::CustomsForgeManager_Winforms.Properties.Resources.eof_bg;
            this.tpCreators.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tpCreators.Controls.Add(this.btnRSTKSite);
            this.tpCreators.Controls.Add(this.btnEOFSite);
            this.tpCreators.Location = new System.Drawing.Point(4, 25);
            this.tpCreators.Name = "tpCreators";
            this.tpCreators.Padding = new System.Windows.Forms.Padding(3);
            this.tpCreators.Size = new System.Drawing.Size(997, 436);
            this.tpCreators.TabIndex = 6;
            this.tpCreators.Text = "Creators";
            this.tpCreators.UseVisualStyleBackColor = true;
            // 
            // btnRSTKSite
            // 
            this.btnRSTKSite.Location = new System.Drawing.Point(275, 221);
            this.btnRSTKSite.Name = "btnRSTKSite";
            this.btnRSTKSite.Size = new System.Drawing.Size(195, 25);
            this.btnRSTKSite.TabIndex = 1;
            this.btnRSTKSite.Text = "Rocksmith Toolkit";
            this.btnRSTKSite.UseVisualStyleBackColor = true;
            this.btnRSTKSite.Click += new System.EventHandler(this.btnRSTKSite_Click);
            // 
            // btnEOFSite
            // 
            this.btnEOFSite.Location = new System.Drawing.Point(275, 105);
            this.btnEOFSite.Name = "btnEOFSite";
            this.btnEOFSite.Size = new System.Drawing.Size(195, 25);
            this.btnEOFSite.TabIndex = 0;
            this.btnEOFSite.Text = "Editor on Fire";
            this.btnEOFSite.UseVisualStyleBackColor = true;
            this.btnEOFSite.Click += new System.EventHandler(this.btnEOFSite_Click);
            // 
            // tpAbout
            // 
            this.tpAbout.BackColor = System.Drawing.SystemColors.Control;
            this.tpAbout.Controls.Add(this.tableLayoutPanel1);
            this.tpAbout.Location = new System.Drawing.Point(4, 25);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Size = new System.Drawing.Size(997, 436);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxCF, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.group_CFquickLinks, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(997, 436);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBoxCF
            // 
            this.pictureBoxCF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBoxCF.Image = global::CustomsForgeManager_Winforms.Properties.Resources.logo_black;
            this.pictureBoxCF.Location = new System.Drawing.Point(123, 81);
            this.pictureBoxCF.Name = "pictureBoxCF";
            this.pictureBoxCF.Size = new System.Drawing.Size(251, 56);
            this.pictureBoxCF.TabIndex = 1;
            this.pictureBoxCF.TabStop = false;
            // 
            // group_CFquickLinks
            // 
            this.group_CFquickLinks.Controls.Add(this.tableLayoutPanel_CFQuicklinks);
            this.group_CFquickLinks.Location = new System.Drawing.Point(501, 221);
            this.group_CFquickLinks.Name = "group_CFquickLinks";
            this.group_CFquickLinks.Size = new System.Drawing.Size(379, 187);
            this.group_CFquickLinks.TabIndex = 3;
            this.group_CFquickLinks.TabStop = false;
            this.group_CFquickLinks.Text = "CustomsForge Links";
            // 
            // tableLayoutPanel_CFQuicklinks
            // 
            this.tableLayoutPanel_CFQuicklinks.ColumnCount = 3;
            this.tableLayoutPanel_CFQuicklinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_CFQuicklinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel_CFQuicklinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkOpenCFHomePage, 1, 0);
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkOpenIgnition, 1, 1);
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkOpenRequests, 1, 2);
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkDontainsPage, 1, 3);
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkOpenCFVideos, 1, 4);
            this.tableLayoutPanel_CFQuicklinks.Controls.Add(this.linkCFFAQ, 1, 5);
            this.tableLayoutPanel_CFQuicklinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_CFQuicklinks.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel_CFQuicklinks.Name = "tableLayoutPanel_CFQuicklinks";
            this.tableLayoutPanel_CFQuicklinks.RowCount = 7;
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel_CFQuicklinks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel_CFQuicklinks.Size = new System.Drawing.Size(373, 168);
            this.tableLayoutPanel_CFQuicklinks.TabIndex = 4;
            // 
            // linkOpenCFHomePage
            // 
            this.linkOpenCFHomePage.AutoSize = true;
            this.linkOpenCFHomePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkOpenCFHomePage.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkOpenCFHomePage.LinkColor = System.Drawing.Color.Black;
            this.linkOpenCFHomePage.Location = new System.Drawing.Point(77, 0);
            this.linkOpenCFHomePage.Name = "linkOpenCFHomePage";
            this.linkOpenCFHomePage.Size = new System.Drawing.Size(216, 28);
            this.linkOpenCFHomePage.TabIndex = 9;
            this.linkOpenCFHomePage.TabStop = true;
            this.linkOpenCFHomePage.Text = "CustomsForge Home";
            this.linkOpenCFHomePage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkOpenCFHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOpenCFHomePage_LinkClicked);
            // 
            // linkOpenIgnition
            // 
            this.linkOpenIgnition.AutoSize = true;
            this.linkOpenIgnition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkOpenIgnition.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkOpenIgnition.LinkColor = System.Drawing.Color.Black;
            this.linkOpenIgnition.Location = new System.Drawing.Point(77, 28);
            this.linkOpenIgnition.Name = "linkOpenIgnition";
            this.linkOpenIgnition.Size = new System.Drawing.Size(216, 28);
            this.linkOpenIgnition.TabIndex = 10;
            this.linkOpenIgnition.TabStop = true;
            this.linkOpenIgnition.Text = "Ignition";
            this.linkOpenIgnition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkOpenIgnition.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOpenIgnition_LinkClicked);
            // 
            // linkOpenRequests
            // 
            this.linkOpenRequests.AutoSize = true;
            this.linkOpenRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkOpenRequests.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkOpenRequests.LinkColor = System.Drawing.Color.Black;
            this.linkOpenRequests.Location = new System.Drawing.Point(77, 56);
            this.linkOpenRequests.Name = "linkOpenRequests";
            this.linkOpenRequests.Size = new System.Drawing.Size(216, 28);
            this.linkOpenRequests.TabIndex = 12;
            this.linkOpenRequests.TabStop = true;
            this.linkOpenRequests.Text = "Requests";
            this.linkOpenRequests.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkOpenRequests.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOpenRequests_LinkClicked);
            // 
            // linkDontainsPage
            // 
            this.linkDontainsPage.AutoSize = true;
            this.linkDontainsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkDontainsPage.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkDontainsPage.LinkColor = System.Drawing.Color.Black;
            this.linkDontainsPage.Location = new System.Drawing.Point(77, 84);
            this.linkDontainsPage.Name = "linkDontainsPage";
            this.linkDontainsPage.Size = new System.Drawing.Size(216, 28);
            this.linkDontainsPage.TabIndex = 13;
            this.linkDontainsPage.TabStop = true;
            this.linkDontainsPage.Text = "Donations";
            this.linkDontainsPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkDontainsPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDonationsPage_LinkClicked);
            // 
            // linkOpenCFVideos
            // 
            this.linkOpenCFVideos.AutoSize = true;
            this.linkOpenCFVideos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkOpenCFVideos.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkOpenCFVideos.LinkColor = System.Drawing.Color.Black;
            this.linkOpenCFVideos.Location = new System.Drawing.Point(77, 112);
            this.linkOpenCFVideos.Name = "linkOpenCFVideos";
            this.linkOpenCFVideos.Size = new System.Drawing.Size(216, 28);
            this.linkOpenCFVideos.TabIndex = 14;
            this.linkOpenCFVideos.TabStop = true;
            this.linkOpenCFVideos.Text = "Videos";
            this.linkOpenCFVideos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkOpenCFVideos.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOpenCFVideos_LinkClicked);
            // 
            // linkCFFAQ
            // 
            this.linkCFFAQ.AutoSize = true;
            this.linkCFFAQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkCFFAQ.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.linkCFFAQ.LinkColor = System.Drawing.Color.Black;
            this.linkCFFAQ.Location = new System.Drawing.Point(77, 140);
            this.linkCFFAQ.Name = "linkCFFAQ";
            this.linkCFFAQ.Size = new System.Drawing.Size(216, 28);
            this.linkCFFAQ.TabIndex = 15;
            this.linkCFFAQ.TabStop = true;
            this.linkCFFAQ.Text = "FAQ";
            this.linkCFFAQ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkCFFAQ.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCFFAQ_LinkClicked);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.groupBox3);
            this.panel6.Location = new System.Drawing.Point(501, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(379, 187);
            this.panel6.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(379, 187);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Info";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.lbl_AppVersion, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.link_CFManager, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.lnk_ReleaseNotes, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(373, 168);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // lbl_AppVersion
            // 
            this.lbl_AppVersion.AutoSize = true;
            this.lbl_AppVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_AppVersion.Location = new System.Drawing.Point(77, 126);
            this.lbl_AppVersion.Name = "lbl_AppVersion";
            this.lbl_AppVersion.Size = new System.Drawing.Size(217, 42);
            this.lbl_AppVersion.TabIndex = 3;
            this.lbl_AppVersion.Text = "Version:";
            this.lbl_AppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // link_CFManager
            // 
            this.link_CFManager.AutoSize = true;
            this.link_CFManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.link_CFManager.Font = new System.Drawing.Font("Trebuchet MS", 14F);
            this.link_CFManager.LinkColor = System.Drawing.Color.Black;
            this.link_CFManager.Location = new System.Drawing.Point(77, 42);
            this.link_CFManager.Name = "link_CFManager";
            this.link_CFManager.Size = new System.Drawing.Size(217, 42);
            this.link_CFManager.TabIndex = 2;
            this.link_CFManager.TabStop = true;
            this.link_CFManager.Text = "Song Manager Support";
            this.link_CFManager.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_CFManager.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_CFManager_LinkClicked);
            // 
            // lnk_ReleaseNotes
            // 
            this.lnk_ReleaseNotes.AutoSize = true;
            this.lnk_ReleaseNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnk_ReleaseNotes.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.lnk_ReleaseNotes.LinkColor = System.Drawing.Color.DodgerBlue;
            this.lnk_ReleaseNotes.Location = new System.Drawing.Point(77, 84);
            this.lnk_ReleaseNotes.Name = "lnk_ReleaseNotes";
            this.lnk_ReleaseNotes.Size = new System.Drawing.Size(217, 42);
            this.lnk_ReleaseNotes.TabIndex = 2;
            this.lnk_ReleaseNotes.TabStop = true;
            this.lnk_ReleaseNotes.Text = "Release Notes";
            this.lnk_ReleaseNotes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnk_ReleaseNotes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_ReleaseNotes_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel_Credits);
            this.groupBox1.Location = new System.Drawing.Point(3, 221);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 187);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credits";
            // 
            // tableLayoutPanel_Credits
            // 
            this.tableLayoutPanel_Credits.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tableLayoutPanel_Credits.BackgroundImage = global::CustomsForgeManager_Winforms.Properties.Resources.Rocksmith_2014_Background_Guitar;
            this.tableLayoutPanel_Credits.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel_Credits.ColumnCount = 2;
            this.tableLayoutPanel_Credits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Credits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Credits.Controls.Add(this.linkLabel1, 0, 5);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_Credits, 0, 0);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_Credits_Description, 1, 0);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_UnleashedRole, 0, 1);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_DarjuszRole, 0, 2);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_LovromanRole, 0, 3);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_ZerkzRole, 0, 4);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_UnleashedProfile, 1, 1);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_DarjuszProfile, 1, 2);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_LovromanProfile, 1, 3);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_ZerkzProfile, 1, 4);
            this.tableLayoutPanel_Credits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Credits.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.tableLayoutPanel_Credits.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel_Credits.Name = "tableLayoutPanel_Credits";
            this.tableLayoutPanel_Credits.RowCount = 6;
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.Size = new System.Drawing.Size(373, 168);
            this.tableLayoutPanel_Credits.TabIndex = 3;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.SystemColors.WindowText;
            this.linkLabel1.Font = new System.Drawing.Font("Trebuchet MS", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.HotPink;
            this.linkLabel1.Location = new System.Drawing.Point(4, 126);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(177, 30);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "ForgeOn, rummhamm87, CustomsForge Staff & other users. (thank you!)";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Credits
            // 
            this.lbl_Credits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Credits.AutoSize = true;
            this.lbl_Credits.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.lbl_Credits.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbl_Credits.Location = new System.Drawing.Point(26, 8);
            this.lbl_Credits.Name = "lbl_Credits";
            this.lbl_Credits.Size = new System.Drawing.Size(133, 18);
            this.lbl_Credits.TabIndex = 0;
            this.lbl_Credits.Text = "Song Manager Team";
            this.lbl_Credits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Credits_Description
            // 
            this.lbl_Credits_Description.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Credits_Description.AutoSize = true;
            this.lbl_Credits_Description.BackColor = System.Drawing.Color.Black;
            this.lbl_Credits_Description.Font = new System.Drawing.Font("Trebuchet MS", 12F);
            this.lbl_Credits_Description.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_Credits_Description.Location = new System.Drawing.Point(222, 6);
            this.lbl_Credits_Description.Name = "lbl_Credits_Description";
            this.lbl_Credits_Description.Size = new System.Drawing.Size(115, 22);
            this.lbl_Credits_Description.TabIndex = 0;
            this.lbl_Credits_Description.Text = "Maintained By:";
            this.lbl_Credits_Description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UnleashedRole
            // 
            this.lbl_UnleashedRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UnleashedRole.AutoSize = true;
            this.lbl_UnleashedRole.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.lbl_UnleashedRole.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_UnleashedRole.Location = new System.Drawing.Point(20, 36);
            this.lbl_UnleashedRole.Name = "lbl_UnleashedRole";
            this.lbl_UnleashedRole.Size = new System.Drawing.Size(146, 18);
            this.lbl_UnleashedRole.TabIndex = 0;
            this.lbl_UnleashedRole.Text = "CustomsForge Owner:";
            this.lbl_UnleashedRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_UnleashedRole.Click += new System.EventHandler(this.lbl_UnleashedRole_Click);
            // 
            // lbl_DarjuszRole
            // 
            this.lbl_DarjuszRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_DarjuszRole.AutoSize = true;
            this.lbl_DarjuszRole.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.lbl_DarjuszRole.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_DarjuszRole.Location = new System.Drawing.Point(37, 56);
            this.lbl_DarjuszRole.Name = "lbl_DarjuszRole";
            this.lbl_DarjuszRole.Size = new System.Drawing.Size(111, 18);
            this.lbl_DarjuszRole.TabIndex = 0;
            this.lbl_DarjuszRole.Text = "Lead Developer:";
            this.lbl_DarjuszRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_LovromanRole
            // 
            this.lbl_LovromanRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_LovromanRole.AutoSize = true;
            this.lbl_LovromanRole.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.lbl_LovromanRole.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_LovromanRole.Location = new System.Drawing.Point(54, 76);
            this.lbl_LovromanRole.Name = "lbl_LovromanRole";
            this.lbl_LovromanRole.Size = new System.Drawing.Size(77, 18);
            this.lbl_LovromanRole.TabIndex = 0;
            this.lbl_LovromanRole.Text = "Developer:";
            this.lbl_LovromanRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_ZerkzRole
            // 
            this.lbl_ZerkzRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_ZerkzRole.AutoSize = true;
            this.lbl_ZerkzRole.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.lbl_ZerkzRole.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbl_ZerkzRole.Location = new System.Drawing.Point(54, 96);
            this.lbl_ZerkzRole.Name = "lbl_ZerkzRole";
            this.lbl_ZerkzRole.Size = new System.Drawing.Size(77, 18);
            this.lbl_ZerkzRole.TabIndex = 0;
            this.lbl_ZerkzRole.Text = "Developer:";
            this.lbl_ZerkzRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // link_UnleashedProfile
            // 
            this.link_UnleashedProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_UnleashedProfile.AutoSize = true;
            this.link_UnleashedProfile.LinkColor = System.Drawing.Color.Red;
            this.link_UnleashedProfile.Location = new System.Drawing.Point(244, 37);
            this.link_UnleashedProfile.Name = "link_UnleashedProfile";
            this.link_UnleashedProfile.Size = new System.Drawing.Size(70, 16);
            this.link_UnleashedProfile.TabIndex = 1;
            this.link_UnleashedProfile.TabStop = true;
            this.link_UnleashedProfile.Text = "Unleashed2k";
            this.link_UnleashedProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_UnleashedProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_UnleashedProfile_LinkClicked);
            // 
            // link_DarjuszProfile
            // 
            this.link_DarjuszProfile.ActiveLinkColor = System.Drawing.Color.Purple;
            this.link_DarjuszProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_DarjuszProfile.AutoSize = true;
            this.link_DarjuszProfile.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.link_DarjuszProfile.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.link_DarjuszProfile.Location = new System.Drawing.Point(255, 57);
            this.link_DarjuszProfile.Name = "link_DarjuszProfile";
            this.link_DarjuszProfile.Size = new System.Drawing.Size(48, 16);
            this.link_DarjuszProfile.TabIndex = 1;
            this.link_DarjuszProfile.TabStop = true;
            this.link_DarjuszProfile.Text = "Darjusz";
            this.link_DarjuszProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_DarjuszProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_DarjuszProfile_LinkClicked);
            // 
            // link_LovromanProfile
            // 
            this.link_LovromanProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_LovromanProfile.AutoSize = true;
            this.link_LovromanProfile.LinkColor = System.Drawing.Color.Orange;
            this.link_LovromanProfile.Location = new System.Drawing.Point(251, 77);
            this.link_LovromanProfile.Name = "link_LovromanProfile";
            this.link_LovromanProfile.Size = new System.Drawing.Size(57, 16);
            this.link_LovromanProfile.TabIndex = 1;
            this.link_LovromanProfile.TabStop = true;
            this.link_LovromanProfile.Text = "Lovroman";
            this.link_LovromanProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_LovromanProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LovromanProfile_LinkClicked);
            // 
            // link_ZerkzProfile
            // 
            this.link_ZerkzProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_ZerkzProfile.AutoSize = true;
            this.link_ZerkzProfile.LinkColor = System.Drawing.Color.Orange;
            this.link_ZerkzProfile.Location = new System.Drawing.Point(261, 97);
            this.link_ZerkzProfile.Name = "link_ZerkzProfile";
            this.link_ZerkzProfile.Size = new System.Drawing.Size(37, 16);
            this.link_ZerkzProfile.TabIndex = 1;
            this.link_ZerkzProfile.TabStop = true;
            this.link_ZerkzProfile.Text = "Zerkz";
            this.link_ZerkzProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_ZerkzProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_Alex360Profile_LinkClicked);
            // 
            // timerMain
            // 
            this.timerMain.Interval = 1000;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarMain,
            this.toolStripStatusLabel_Main,
            this.toolStripStatusLabel_ClearLog,
            this.toolStripStatusLabel_MainCancel,
            this.toolStripStatusLabelSpringer,
            this.toolStripStatusLabel_DisabledCounter});
            this.statusStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripMain.Location = new System.Drawing.Point(0, 606);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1011, 23);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 1;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripProgressBarMain
            // 
            this.toolStripProgressBarMain.Name = "toolStripProgressBarMain";
            this.toolStripProgressBarMain.Size = new System.Drawing.Size(200, 17);
            // 
            // toolStripStatusLabel_Main
            // 
            this.toolStripStatusLabel_Main.Name = "toolStripStatusLabel_Main";
            this.toolStripStatusLabel_Main.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel_ClearLog
            // 
            this.toolStripStatusLabel_ClearLog.Font = new System.Drawing.Font("Trebuchet MS", 9F);
            this.toolStripStatusLabel_ClearLog.IsLink = true;
            this.toolStripStatusLabel_ClearLog.LinkColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel_ClearLog.Name = "toolStripStatusLabel_ClearLog";
            this.toolStripStatusLabel_ClearLog.Size = new System.Drawing.Size(56, 18);
            this.toolStripStatusLabel_ClearLog.Text = "Clear log";
            this.toolStripStatusLabel_ClearLog.Click += new System.EventHandler(this.toolStripStatusLabel_ClearLog_Click);
            // 
            // toolStripStatusLabel_MainCancel
            // 
            this.toolStripStatusLabel_MainCancel.Font = new System.Drawing.Font("Trebuchet MS", 9F);
            this.toolStripStatusLabel_MainCancel.IsLink = true;
            this.toolStripStatusLabel_MainCancel.LinkColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel_MainCancel.Name = "toolStripStatusLabel_MainCancel";
            this.toolStripStatusLabel_MainCancel.Size = new System.Drawing.Size(45, 18);
            this.toolStripStatusLabel_MainCancel.Text = "Cancel";
            this.toolStripStatusLabel_MainCancel.Visible = false;
            this.toolStripStatusLabel_MainCancel.Click += new System.EventHandler(this.toolStripStatusLabel_MainCancel_Click);
            // 
            // toolStripStatusLabelSpringer
            // 
            this.toolStripStatusLabelSpringer.Name = "toolStripStatusLabelSpringer";
            this.toolStripStatusLabelSpringer.Size = new System.Drawing.Size(0, 18);
            this.toolStripStatusLabelSpringer.Spring = true;
            // 
            // toolStripStatusLabel_DisabledCounter
            // 
            this.toolStripStatusLabel_DisabledCounter.Font = new System.Drawing.Font("Trebuchet MS", 9F);
            this.toolStripStatusLabel_DisabledCounter.Name = "toolStripStatusLabel_DisabledCounter";
            this.toolStripStatusLabel_DisabledCounter.Size = new System.Drawing.Size(57, 18);
            this.toolStripStatusLabel_DisabledCounter.Text = "Disabled:";
            this.toolStripStatusLabel_DisabledCounter.Visible = false;
            // 
            // timerAutoUpdate
            // 
            this.timerAutoUpdate.Enabled = true;
            this.timerAutoUpdate.Interval = 600000;
            this.timerAutoUpdate.Tick += new System.EventHandler(this.timerAutoUpdate_Tick);
            // 
            // folderBrowserDialog_SettingsRSPath
            // 
            this.folderBrowserDialog_SettingsRSPath.Description = "Browse for your Rocksmith 2014 installed directory";
            this.folderBrowserDialog_SettingsRSPath.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog_SettingsRSPath.ShowNewFolderButton = false;
            // 
            // sfdSongListToCSV
            // 
            this.sfdSongListToCSV.FileName = "songList.csv";
            this.sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
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
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // labelAlphaTesters
            // 
            this.labelAlphaTesters.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAlphaTesters.AutoSize = true;
            this.labelAlphaTesters.Font = new System.Drawing.Font("Trebuchet MS", 10F);
            this.labelAlphaTesters.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelAlphaTesters.Location = new System.Drawing.Point(44, 132);
            this.labelAlphaTesters.Name = "labelAlphaTesters";
            this.labelAlphaTesters.Size = new System.Drawing.Size(97, 18);
            this.labelAlphaTesters.TabIndex = 3;
            this.labelAlphaTesters.Text = "Alpha Testers:";
            this.labelAlphaTesters.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSettingsRSDir
            // 
            this.tbSettingsRSDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSettingsRSDir.Location = new System.Drawing.Point(302, 5);
            this.tbSettingsRSDir.Multiline = true;
            this.tbSettingsRSDir.Name = "tbSettingsRSDir";
            this.tbSettingsRSDir.ReadOnly = true;
            this.tbSettingsRSDir.Size = new System.Drawing.Size(684, 35);
            this.tbSettingsRSDir.TabIndex = 2;
            this.tbSettingsRSDir.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbSettingsRSDir_MouseDoubleClick);
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.AutoSize = true;
            this.lblSettingsRSDir.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSettingsRSDir.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettingsRSDir.Location = new System.Drawing.Point(18, 2);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Size = new System.Drawing.Size(276, 41);
            this.lblSettingsRSDir.TabIndex = 1;
            this.lblSettingsRSDir.Text = "Rocksmith installation directory (double-click to change):";
            this.lblSettingsRSDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkEnableLogBaloon
            // 
            this.checkEnableLogBaloon.AutoSize = true;
            this.checkEnableLogBaloon.Checked = true;
            this.checkEnableLogBaloon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEnableLogBaloon.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkEnableLogBaloon.Location = new System.Drawing.Point(176, 48);
            this.checkEnableLogBaloon.Name = "checkEnableLogBaloon";
            this.checkEnableLogBaloon.Size = new System.Drawing.Size(118, 35);
            this.checkEnableLogBaloon.TabIndex = 5;
            this.checkEnableLogBaloon.Text = "Enable Log Baloon ";
            this.checkEnableLogBaloon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkEnableLogBaloon.UseVisualStyleBackColor = true;
            this.checkEnableLogBaloon.CheckedChanged += new System.EventHandler(this.checkEnableLogBaloon_CheckedChanged);
            // 
            // listDisabledColumns
            // 
            this.listDisabledColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listDisabledColumns.CheckBoxes = true;
            this.listDisabledColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSelect,
            this.colSettingsColumnName,
            this.colSettingsColumnEnabled});
            this.listDisabledColumns.Location = new System.Drawing.Point(3, 22);
            this.listDisabledColumns.Name = "listDisabledColumns";
            this.listDisabledColumns.Size = new System.Drawing.Size(686, 277);
            this.listDisabledColumns.TabIndex = 5;
            this.listDisabledColumns.UseCompatibleStateImageBehavior = false;
            this.listDisabledColumns.View = System.Windows.Forms.View.Details;
            // 
            // columnSelect
            // 
            this.columnSelect.Text = "Select";
            this.columnSelect.Width = 51;
            // 
            // colSettingsColumnName
            // 
            this.colSettingsColumnName.Text = "Column Name";
            this.colSettingsColumnName.Width = 169;
            // 
            // colSettingsColumnEnabled
            // 
            this.colSettingsColumnEnabled.Text = "Enabled";
            this.colSettingsColumnEnabled.Width = 105;
            // 
            // lblDisabledColumns
            // 
            this.lblDisabledColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisabledColumns.AutoSize = true;
            this.lblDisabledColumns.Location = new System.Drawing.Point(247, 2);
            this.lblDisabledColumns.Name = "lblDisabledColumns";
            this.lblDisabledColumns.Size = new System.Drawing.Size(119, 16);
            this.lblDisabledColumns.TabIndex = 1;
            this.lblDisabledColumns.Text = "Song Manager columns";
            // 
            // btnEnableColumns
            // 
            this.btnEnableColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableColumns.Location = new System.Drawing.Point(232, 305);
            this.btnEnableColumns.Name = "btnEnableColumns";
            this.btnEnableColumns.Size = new System.Drawing.Size(150, 23);
            this.btnEnableColumns.TabIndex = 2;
            this.btnEnableColumns.Text = "Enable/disable column(s)";
            this.btnEnableColumns.UseVisualStyleBackColor = true;
            this.btnEnableColumns.Click += new System.EventHandler(this.btnEnableColumns_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnEnableColumns);
            this.panel5.Controls.Add(this.lblDisabledColumns);
            this.panel5.Controls.Add(this.listDisabledColumns);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(302, 48);
            this.panel5.Name = "panel5";
            this.tlpSettings_Wrapper.SetRowSpan(this.panel5, 4);
            this.panel5.Size = new System.Drawing.Size(684, 331);
            this.panel5.TabIndex = 4;
            // 
            // dgvSongs
            // 
            this.dgvSongs.AllowUserToAddRows = false;
            this.dgvSongs.AllowUserToDeleteRows = false;
            this.dgvSongs.AllowUserToOrderColumns = true;
            this.dgvSongs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSongs.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect});
            this.dgvSongs.ContextMenuStrip = this.contextMenuStrip_MainManager;
            this.dgvSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvSongs.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvSongs.Location = new System.Drawing.Point(6, 19);
            this.dgvSongs.MultiSelect = false;
            this.dgvSongs.Name = "dgvSongs";
            this.dgvSongs.RowHeadersVisible = false;
            this.dgvSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongs.Size = new System.Drawing.Size(973, 211);
            this.dgvSongs.TabIndex = 1;
            this.dgvSongs.DataSourceChanged += new System.EventHandler(this.dgvSongs_DataSourceChanged);
            this.dgvSongs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongs_CellDoubleClick);
            this.dgvSongs.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongs_CellMouseDown);
            this.dgvSongs.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongs_ColumnHeaderMouseClick);
            this.dgvSongs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSongs_KeyDown);
            // 
            // colSelect
            // 
            this.colSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.TrueValue = "true";
            this.colSelect.Visible = false;
            // 
            // frmMainBindingSource
            // 
            this.frmMainBindingSource.DataSource = typeof(CustomsForgeManager_Winforms.Forms.frmMain);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 629);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tlp_MainForm_Wrappper);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomsForge Song Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.tlp_MainForm_Wrappper.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpManager.ResumeLayout(false);
            this.gb_Main_Grid.ResumeLayout(false);
            this.gb_Main_Grid.PerformLayout();
            this.contextMenuStrip_MainManager.ResumeLayout(false);
            this.gb_Main_Search.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gb_Main_Actions.ResumeLayout(false);
            this.panelSongListButtons.ResumeLayout(false);
            this.panelSongListButtons.PerformLayout();
            this.tpDuplicates.ResumeLayout(false);
            this.tlpDuplicates.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tpBatchRenamer.ResumeLayout(false);
            this.tpBatchRenamer.PerformLayout();
            this.propertiesGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.renamerPropertyDataGridView)).EndInit();
            this.howToGroupBox.ResumeLayout(false);
            this.howToGroupBox.PerformLayout();
            this.tpUtilities.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.gb_Utilities.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tlpSettings_Wrapper.ResumeLayout(false);
            this.tlpSettings_Wrapper.PerformLayout();
            this.tpCreators.ResumeLayout(false);
            this.tpAbout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCF)).EndInit();
            this.group_CFquickLinks.ResumeLayout(false);
            this.tableLayoutPanel_CFQuicklinks.ResumeLayout(false);
            this.tableLayoutPanel_CFQuicklinks.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel_Credits.ResumeLayout(false);
            this.tableLayoutPanel_Credits.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.contextMenuStrip_Tray.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frmMainBindingSource)).EndInit();
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
        private Controls.AbortableBackgroundWorker bWorker;
        private System.Windows.Forms.Timer timerMain;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarMain;
        private System.Windows.Forms.Timer timerAutoUpdate;
        private System.Windows.Forms.TableLayoutPanel tlpSettings_Wrapper;
        private System.Windows.Forms.Button btnSettingsSave;
        private System.Windows.Forms.Button btnSettingsLoad;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_MainManager;
        private System.Windows.Forms.ToolStripMenuItem openDLCPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editDLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Main;
        private System.Windows.Forms.TabPage tpDuplicates;
        private System.Windows.Forms.TableLayoutPanel tlpDuplicates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeleteDupeSong;
        private System.Windows.Forms.Button btnDupeRescan;
        private System.Windows.Forms.Panel panelSongListButtons;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.ListView listDupeSongs;
        private System.Windows.Forms.ColumnHeader colDupeSelect;
        private System.Windows.Forms.ColumnHeader colDupeArtist;
        private System.Windows.Forms.ColumnHeader colDupeSong;
        private System.Windows.Forms.ColumnHeader colDupeAlbum;
        private System.Windows.Forms.ColumnHeader colDupeSongPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_SettingsRSPath;
        private System.Windows.Forms.CheckBox checkRescanOnStartup;
        private System.Windows.Forms.TabPage tpUtilities;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnBackupRSProfile;
        private System.Windows.Forms.GroupBox gb_Utilities;
        private System.Windows.Forms.Button btnLaunchSteam;
        private System.Windows.Forms.SaveFileDialog sfdSongListToCSV;
        private System.Windows.Forms.Button btnExportSongList;
        private System.Windows.Forms.ToolStripMenuItem showDLCInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.Button btnCheckAllForUpdates;
        private System.Windows.Forms.Button btnDisableEnableSongs;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_MainCancel;
        private System.Windows.Forms.NotifyIcon notifyIcon_Main;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Tray;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkIncludeRS1DLC;
        private System.Windows.Forms.ToolStripMenuItem openDLCLocationToolStripMenuItem;
        private System.Windows.Forms.Button btnBatchRenamer;
        private System.Windows.Forms.ToolStripMenuItem getAuthorNameStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_DisabledCounter;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSpringer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBoxCF;
        private System.Windows.Forms.GroupBox group_CFquickLinks;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.LinkLabel link_CFManager;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Credits;
        private System.Windows.Forms.Label lbl_UnleashedRole;
        private System.Windows.Forms.Label lbl_DarjuszRole;
        private System.Windows.Forms.Label lbl_LovromanRole;
        private System.Windows.Forms.Label lbl_ZerkzRole;
        private System.Windows.Forms.LinkLabel link_UnleashedProfile;
        private System.Windows.Forms.LinkLabel link_DarjuszProfile;
        private System.Windows.Forms.LinkLabel link_LovromanProfile;
        private System.Windows.Forms.LinkLabel link_ZerkzProfile;
        private System.Windows.Forms.Label lbl_AppVersion;
        private System.Windows.Forms.RadioButton radioBtn_ExportToHTML;
        private System.Windows.Forms.RadioButton radioBtn_ExportToCSV;
        private System.Windows.Forms.RadioButton radioBtn_ExportToBBCode;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_ClearLog;
        private System.Windows.Forms.ColumnHeader colDupeLastUpdated;
        private System.Windows.Forms.ToolStripMenuItem deleteSongToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnk_ReleaseNotes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_CFQuicklinks;
        private System.Windows.Forms.LinkLabel linkOpenCFHomePage;
        private System.Windows.Forms.LinkLabel linkOpenIgnition;
        private System.Windows.Forms.LinkLabel linkOpenRequests;
        private System.Windows.Forms.LinkLabel linkDontainsPage;
        private System.Windows.Forms.LinkLabel linkOpenCFVideos;
        private System.Windows.Forms.TabPage tpCreators;
        private System.Windows.Forms.Button btnRSTKSite;
        private System.Windows.Forms.Button btnEOFSite;
        private System.Windows.Forms.Label lbl_ExportTo;
        private System.Windows.Forms.LinkLabel linkCFFAQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lbl_Credits;
        private System.Windows.Forms.Label lbl_Credits_Description;
        private System.Windows.Forms.ToolStripMenuItem backupDLCToolStripMenuItem;
        private System.Windows.Forms.Button btnBackupSelectedDLCs;
        private System.Windows.Forms.TabPage tpBatchRenamer;
        private System.Windows.Forms.Button renameAllButton;
        private System.Windows.Forms.TextBox renameTemplateTextBox;
        private System.Windows.Forms.GroupBox howToGroupBox;
        private System.Windows.Forms.Label renameTemplateLabel;
        private System.Windows.Forms.GroupBox propertiesGroupBox;
        private System.Windows.Forms.Label labelAlphaTesters;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.DataGridView renamerPropertyDataGridView;
        private System.Data.DataSet renamerPropertyDataSet;
        private System.Windows.Forms.BindingSource frmMainBindingSource;
        private System.Windows.Forms.Label slashLabel;
        private System.Windows.Forms.CheckBox deleteEmptyDirCheckBox;
        private Controls.RADataGridView dgvSongs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.GroupBox gb_Main_Grid;
        private System.Windows.Forms.LinkLabel linkLblSelectAll;
        private System.Windows.Forms.GroupBox gb_Main_Search;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.LinkLabel link_MainClearResults;
        private System.Windows.Forms.Label lbl_Search;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gb_Main_Actions;
        private System.Windows.Forms.ColumnHeader colDupeEnabled;
        private System.Windows.Forms.Button btnDisableEnableSong;
        private System.Windows.Forms.Label lblSettingsRSDir;
        private System.Windows.Forms.TextBox tbSettingsRSDir;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnEnableColumns;
        private System.Windows.Forms.Label lblDisabledColumns;
        private System.Windows.Forms.ListView listDisabledColumns;
        private System.Windows.Forms.ColumnHeader columnSelect;
        private System.Windows.Forms.ColumnHeader colSettingsColumnName;
        private System.Windows.Forms.ColumnHeader colSettingsColumnEnabled;
        private System.Windows.Forms.CheckBox checkEnableLogBaloon;
    }
}
