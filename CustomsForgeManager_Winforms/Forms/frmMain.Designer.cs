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
            this.tlp_MainForm_Wrappper = new System.Windows.Forms.TableLayoutPanel();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpManager = new System.Windows.Forms.TabPage();
            this.tlpSongListWrapper = new System.Windows.Forms.TableLayoutPanel();
            this.panelSongListButtons = new System.Windows.Forms.Panel();
            this.btnSaveDLC = new System.Windows.Forms.Button();
            this.btnBackupDLC = new System.Windows.Forms.Button();
            this.btnDLCPage = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnEditDLC = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvSongs = new System.Windows.Forms.DataGridView();
            this.colBackup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPreview = new System.Windows.Forms.DataGridViewLinkColumn();
            this.contextMenuStrip_MainManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDLCPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDLCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tpEditor = new System.Windows.Forms.TabPage();
            this.tpDuplicates = new System.Windows.Forms.TabPage();
            this.tlpDuplicates = new System.Windows.Forms.TableLayoutPanel();
            this.listDupeSongs = new System.Windows.Forms.ListView();
            this.colDupeSelect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeSong = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeSongOnePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDupeSongTwoPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDupeRescan = new System.Windows.Forms.Button();
            this.btnDeleteSongOne = new System.Windows.Forms.Button();
            this.btnDeleteSongTwo = new System.Windows.Forms.Button();
            this.tpUtilities = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnBackupRSProfile = new System.Windows.Forms.Button();
            this.tpSettings = new System.Windows.Forms.TabPage();
            this.tlpSettings_Wrapper = new System.Windows.Forms.TableLayoutPanel();
            this.lblSettingsRSDir = new System.Windows.Forms.Label();
            this.tbSettingsRSDir = new System.Windows.Forms.TextBox();
            this.checkRescanOnStartup = new System.Windows.Forms.CheckBox();
            this.btnSettingsLoad = new System.Windows.Forms.Button();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lnkAboutCF = new System.Windows.Forms.LinkLabel();
            this.pbAboutLogo = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel_Credits = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Credits = new System.Windows.Forms.Label();
            this.lbl_Credits_Description = new System.Windows.Forms.Label();
            this.lbl_UnleashedRole = new System.Windows.Forms.Label();
            this.lbl_DarjuszRole = new System.Windows.Forms.Label();
            this.lbl_LovromanRole = new System.Windows.Forms.Label();
            this.lbl_AlexRole = new System.Windows.Forms.Label();
            this.link_UnleashedProfile = new System.Windows.Forms.LinkLabel();
            this.link_DarjuszProfile = new System.Windows.Forms.LinkLabel();
            this.link_LovromanProfile = new System.Windows.Forms.LinkLabel();
            this.link_Alex360Profile = new System.Windows.Forms.LinkLabel();
            this.lbl_ForgeRole = new System.Windows.Forms.Label();
            this.link_ForgeOnProfile = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bWorker = new System.ComponentModel.BackgroundWorker();
            this.timerMain = new System.Windows.Forms.Timer(this.components);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBarMain = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel_Main = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerAutoUpdate = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog_SettingsRSPath = new System.Windows.Forms.FolderBrowserDialog();
            this.btnLaunchRocksmith = new System.Windows.Forms.Button();
            this.tlp_MainForm_Wrappper.SuspendLayout();
            this.gbLog.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpManager.SuspendLayout();
            this.tlpSongListWrapper.SuspendLayout();
            this.panelSongListButtons.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).BeginInit();
            this.contextMenuStrip_MainManager.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tpDuplicates.SuspendLayout();
            this.tlpDuplicates.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpUtilities.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tpSettings.SuspendLayout();
            this.tlpSettings_Wrapper.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutLogo)).BeginInit();
            this.tableLayoutPanel_Credits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStripMain.SuspendLayout();
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
            this.tcMain.Controls.Add(this.tpDuplicates);
            this.tcMain.Controls.Add(this.tpUtilities);
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
            this.tpManager.Text = "Song Manager";
            // 
            // tlpSongListWrapper
            // 
            this.tlpSongListWrapper.ColumnCount = 1;
            this.tlpSongListWrapper.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSongListWrapper.Controls.Add(this.panelSongListButtons, 0, 2);
            this.tlpSongListWrapper.Controls.Add(this.panel2, 0, 1);
            this.tlpSongListWrapper.Controls.Add(this.panel3, 0, 0);
            this.tlpSongListWrapper.Location = new System.Drawing.Point(0, 0);
            this.tlpSongListWrapper.Name = "tlpSongListWrapper";
            this.tlpSongListWrapper.RowCount = 3;
            this.tlpSongListWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.071867F));
            this.tlpSongListWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.32058F));
            this.tlpSongListWrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.60755F));
            this.tlpSongListWrapper.Size = new System.Drawing.Size(770, 393);
            this.tlpSongListWrapper.TabIndex = 2;
            // 
            // panelSongListButtons
            // 
            this.panelSongListButtons.Controls.Add(this.btnSaveDLC);
            this.panelSongListButtons.Controls.Add(this.btnBackupDLC);
            this.panelSongListButtons.Controls.Add(this.btnDLCPage);
            this.panelSongListButtons.Controls.Add(this.btnRescan);
            this.panelSongListButtons.Controls.Add(this.btnEditDLC);
            this.panelSongListButtons.Location = new System.Drawing.Point(3, 334);
            this.panelSongListButtons.Name = "panelSongListButtons";
            this.panelSongListButtons.Size = new System.Drawing.Size(764, 52);
            this.panelSongListButtons.TabIndex = 3;
            // 
            // btnSaveDLC
            // 
            this.btnSaveDLC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveDLC.Enabled = false;
            this.btnSaveDLC.Location = new System.Drawing.Point(597, 13);
            this.btnSaveDLC.Name = "btnSaveDLC";
            this.btnSaveDLC.Size = new System.Drawing.Size(150, 27);
            this.btnSaveDLC.TabIndex = 7;
            this.btnSaveDLC.Text = "Save Song";
            this.btnSaveDLC.UseVisualStyleBackColor = true;
            // 
            // btnBackupDLC
            // 
            this.btnBackupDLC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBackupDLC.Enabled = false;
            this.btnBackupDLC.Location = new System.Drawing.Point(441, 13);
            this.btnBackupDLC.Name = "btnBackupDLC";
            this.btnBackupDLC.Size = new System.Drawing.Size(150, 27);
            this.btnBackupDLC.TabIndex = 6;
            this.btnBackupDLC.Text = "Backup Song";
            this.btnBackupDLC.UseVisualStyleBackColor = true;
            // 
            // btnDLCPage
            // 
            this.btnDLCPage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDLCPage.Enabled = false;
            this.btnDLCPage.Location = new System.Drawing.Point(285, 13);
            this.btnDLCPage.Name = "btnDLCPage";
            this.btnDLCPage.Size = new System.Drawing.Size(150, 27);
            this.btnDLCPage.TabIndex = 5;
            this.btnDLCPage.Text = "Open Song Page";
            this.btnDLCPage.UseVisualStyleBackColor = true;
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRescan.Location = new System.Drawing.Point(36, 13);
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
            this.btnEditDLC.Location = new System.Drawing.Point(129, 13);
            this.btnEditDLC.Name = "btnEditDLC";
            this.btnEditDLC.Size = new System.Drawing.Size(150, 27);
            this.btnEditDLC.TabIndex = 4;
            this.btnEditDLC.Text = "Edit Song";
            this.btnEditDLC.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvSongs);
            this.panel2.Location = new System.Drawing.Point(3, 38);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(764, 290);
            this.panel2.TabIndex = 4;
            // 
            // dgvSongs
            // 
            this.dgvSongs.AllowUserToAddRows = false;
            this.dgvSongs.AllowUserToDeleteRows = false;
            this.dgvSongs.AllowUserToResizeColumns = false;
            this.dgvSongs.AllowUserToResizeRows = false;
            this.dgvSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBackup,
            this.colPreview});
            this.dgvSongs.ContextMenuStrip = this.contextMenuStrip_MainManager;
            this.dgvSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSongs.Location = new System.Drawing.Point(0, 0);
            this.dgvSongs.MultiSelect = false;
            this.dgvSongs.Name = "dgvSongs";
            this.dgvSongs.RowHeadersVisible = false;
            this.dgvSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongs.Size = new System.Drawing.Size(764, 290);
            this.dgvSongs.TabIndex = 0;
            // 
            // colBackup
            // 
            this.colBackup.HeaderText = "Backup";
            this.colBackup.Name = "colBackup";
            this.colBackup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBackup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBackup.Visible = false;
            // 
            // colPreview
            // 
            this.colPreview.HeaderText = "Preview";
            this.colPreview.Name = "colPreview";
            this.colPreview.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPreview.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colPreview.Text = "Preview";
            this.colPreview.Visible = false;
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
            // panel3
            // 
            this.panel3.Controls.Add(this.lbl_Search);
            this.panel3.Controls.Add(this.tbSearch);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(764, 29);
            this.panel3.TabIndex = 5;
            // 
            // lbl_Search
            // 
            this.lbl_Search.AutoSize = true;
            this.lbl_Search.Location = new System.Drawing.Point(126, 8);
            this.lbl_Search.Name = "lbl_Search";
            this.lbl_Search.Size = new System.Drawing.Size(59, 13);
            this.lbl_Search.TabIndex = 2;
            this.lbl_Search.Text = "Search for:";
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(191, 5);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(319, 20);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(516, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 21);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tpEditor
            // 
            this.tpEditor.Location = new System.Drawing.Point(4, 22);
            this.tpEditor.Name = "tpEditor";
            this.tpEditor.Size = new System.Drawing.Size(770, 389);
            this.tpEditor.TabIndex = 3;
            this.tpEditor.Text = "Song Editor";
            this.tpEditor.UseVisualStyleBackColor = true;
            // 
            // tpDuplicates
            // 
            this.tpDuplicates.Controls.Add(this.tlpDuplicates);
            this.tpDuplicates.Location = new System.Drawing.Point(4, 22);
            this.tpDuplicates.Name = "tpDuplicates";
            this.tpDuplicates.Size = new System.Drawing.Size(770, 389);
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
            this.tlpDuplicates.Location = new System.Drawing.Point(1, 3);
            this.tlpDuplicates.Name = "tlpDuplicates";
            this.tlpDuplicates.RowCount = 2;
            this.tlpDuplicates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.73267F));
            this.tlpDuplicates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.26733F));
            this.tlpDuplicates.Size = new System.Drawing.Size(768, 383);
            this.tlpDuplicates.TabIndex = 2;
            // 
            // listDupeSongs
            // 
            this.listDupeSongs.CheckBoxes = true;
            this.listDupeSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDupeSelect,
            this.colDupeArtist,
            this.colDupeSong,
            this.colDupeAlbum,
            this.colDupeSongOnePath,
            this.colDupeSongTwoPath});
            this.listDupeSongs.ContextMenuStrip = this.contextMenuStrip_MainManager;
            this.listDupeSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDupeSongs.FullRowSelect = true;
            this.listDupeSongs.GridLines = true;
            this.listDupeSongs.HideSelection = false;
            this.listDupeSongs.Location = new System.Drawing.Point(3, 3);
            this.listDupeSongs.Name = "listDupeSongs";
            this.listDupeSongs.Size = new System.Drawing.Size(762, 303);
            this.listDupeSongs.TabIndex = 2;
            this.listDupeSongs.UseCompatibleStateImageBehavior = false;
            this.listDupeSongs.View = System.Windows.Forms.View.Details;
            // 
            // colDupeSelect
            // 
            this.colDupeSelect.Text = "Select";
            // 
            // colDupeArtist
            // 
            this.colDupeArtist.Text = "Artist";
            // 
            // colDupeSong
            // 
            this.colDupeSong.Text = "Song";
            // 
            // colDupeAlbum
            // 
            this.colDupeAlbum.Text = "Album";
            // 
            // colDupeSongOnePath
            // 
            this.colDupeSongOnePath.Text = "First Song Path";
            this.colDupeSongOnePath.Width = 122;
            // 
            // colDupeSongTwoPath
            // 
            this.colDupeSongTwoPath.Text = "Second Song Path";
            this.colDupeSongTwoPath.Width = 139;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDupeRescan);
            this.panel1.Controls.Add(this.btnDeleteSongOne);
            this.panel1.Controls.Add(this.btnDeleteSongTwo);
            this.panel1.Location = new System.Drawing.Point(3, 312);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(762, 68);
            this.panel1.TabIndex = 0;
            // 
            // btnDupeRescan
            // 
            this.btnDupeRescan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDupeRescan.Location = new System.Drawing.Point(129, 22);
            this.btnDupeRescan.Name = "btnDupeRescan";
            this.btnDupeRescan.Size = new System.Drawing.Size(150, 27);
            this.btnDupeRescan.TabIndex = 9;
            this.btnDupeRescan.Text = "Rescan duplicates";
            this.btnDupeRescan.UseVisualStyleBackColor = true;
            this.btnDupeRescan.Click += new System.EventHandler(this.btnDupeRescan_Click);
            // 
            // btnDeleteSongOne
            // 
            this.btnDeleteSongOne.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteSongOne.Location = new System.Drawing.Point(285, 22);
            this.btnDeleteSongOne.Name = "btnDeleteSongOne";
            this.btnDeleteSongOne.Size = new System.Drawing.Size(150, 27);
            this.btnDeleteSongOne.TabIndex = 8;
            this.btnDeleteSongOne.Text = "Delete dupe song #1";
            this.btnDeleteSongOne.UseVisualStyleBackColor = true;
            this.btnDeleteSongOne.Click += new System.EventHandler(this.btnDeleteSongOne_Click);
            // 
            // btnDeleteSongTwo
            // 
            this.btnDeleteSongTwo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteSongTwo.Location = new System.Drawing.Point(441, 22);
            this.btnDeleteSongTwo.Name = "btnDeleteSongTwo";
            this.btnDeleteSongTwo.Size = new System.Drawing.Size(150, 27);
            this.btnDeleteSongTwo.TabIndex = 7;
            this.btnDeleteSongTwo.Text = "Delete dupe song #2";
            this.btnDeleteSongTwo.UseVisualStyleBackColor = true;
            this.btnDeleteSongTwo.Click += new System.EventHandler(this.btnDeleteSongTwo_Click);
            // 
            // tpUtilities
            // 
            this.tpUtilities.Controls.Add(this.tableLayoutPanel2);
            this.tpUtilities.Location = new System.Drawing.Point(4, 22);
            this.tpUtilities.Name = "tpUtilities";
            this.tpUtilities.Size = new System.Drawing.Size(770, 389);
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
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 389F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(770, 389);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnLaunchRocksmith);
            this.panel4.Controls.Add(this.btnBackupRSProfile);
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(764, 383);
            this.panel4.TabIndex = 0;
            // 
            // btnBackupRSProfile
            // 
            this.btnBackupRSProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBackupRSProfile.Location = new System.Drawing.Point(294, 15);
            this.btnBackupRSProfile.Name = "btnBackupRSProfile";
            this.btnBackupRSProfile.Size = new System.Drawing.Size(150, 23);
            this.btnBackupRSProfile.TabIndex = 2;
            this.btnBackupRSProfile.Text = "Backup Rocksmith Profile";
            this.btnBackupRSProfile.UseVisualStyleBackColor = true;
            this.btnBackupRSProfile.Click += new System.EventHandler(this.btnBackupRSProfile_Click);
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
            this.tlpSettings_Wrapper.Controls.Add(this.lblSettingsRSDir, 1, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.tbSettingsRSDir, 2, 1);
            this.tlpSettings_Wrapper.Controls.Add(this.checkRescanOnStartup, 1, 2);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsLoad, 2, 4);
            this.tlpSettings_Wrapper.Controls.Add(this.btnSettingsSave, 1, 4);
            this.tlpSettings_Wrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings_Wrapper.Location = new System.Drawing.Point(3, 3);
            this.tlpSettings_Wrapper.Name = "tlpSettings_Wrapper";
            this.tlpSettings_Wrapper.RowCount = 5;
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.832898F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.86262F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.46965F));
            this.tlpSettings_Wrapper.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpSettings_Wrapper.Size = new System.Drawing.Size(764, 383);
            this.tlpSettings_Wrapper.TabIndex = 0;
            // 
            // lblSettingsRSDir
            // 
            this.lblSettingsRSDir.AutoSize = true;
            this.lblSettingsRSDir.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblSettingsRSDir.Location = new System.Drawing.Point(110, 20);
            this.lblSettingsRSDir.Name = "lblSettingsRSDir";
            this.lblSettingsRSDir.Size = new System.Drawing.Size(272, 24);
            this.lblSettingsRSDir.TabIndex = 1;
            this.lblSettingsRSDir.Text = "Rocksmith installation directory (double-click to change):";
            this.lblSettingsRSDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSettingsRSDir
            // 
            this.tbSettingsRSDir.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbSettingsRSDir.Location = new System.Drawing.Point(388, 23);
            this.tbSettingsRSDir.Name = "tbSettingsRSDir";
            this.tbSettingsRSDir.Size = new System.Drawing.Size(373, 20);
            this.tbSettingsRSDir.TabIndex = 2;
            this.tbSettingsRSDir.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbSettingsRSDir_MouseDoubleClick);
            // 
            // checkRescanOnStartup
            // 
            this.checkRescanOnStartup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkRescanOnStartup.AutoSize = true;
            this.checkRescanOnStartup.Checked = true;
            this.checkRescanOnStartup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRescanOnStartup.Location = new System.Drawing.Point(183, 52);
            this.checkRescanOnStartup.Name = "checkRescanOnStartup";
            this.checkRescanOnStartup.Size = new System.Drawing.Size(113, 17);
            this.checkRescanOnStartup.TabIndex = 3;
            this.checkRescanOnStartup.Text = "Rescan on startup";
            this.checkRescanOnStartup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRescanOnStartup.UseVisualStyleBackColor = true;
            // 
            // btnSettingsLoad
            // 
            this.btnSettingsLoad.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSettingsLoad.Location = new System.Drawing.Point(499, 346);
            this.btnSettingsLoad.Name = "btnSettingsLoad";
            this.btnSettingsLoad.Size = new System.Drawing.Size(150, 23);
            this.btnSettingsLoad.TabIndex = 0;
            this.btnSettingsLoad.Text = "Load Settings";
            this.btnSettingsLoad.UseVisualStyleBackColor = true;
            this.btnSettingsLoad.Click += new System.EventHandler(this.btnSettingsLoad_Click);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSettingsSave.Location = new System.Drawing.Point(165, 346);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(150, 23);
            this.btnSettingsSave.TabIndex = 0;
            this.btnSettingsSave.Text = "Save Settings";
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.btnSettingsSave_Click);
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel_Credits, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
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
            // tableLayoutPanel_Credits
            // 
            this.tableLayoutPanel_Credits.ColumnCount = 2;
            this.tableLayoutPanel_Credits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Credits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_Credits, 0, 0);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_Credits_Description, 1, 0);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_UnleashedRole, 0, 1);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_DarjuszRole, 0, 2);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_LovromanRole, 0, 3);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_AlexRole, 0, 4);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_UnleashedProfile, 1, 1);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_DarjuszProfile, 1, 2);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_LovromanProfile, 1, 3);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_Alex360Profile, 1, 4);
            this.tableLayoutPanel_Credits.Controls.Add(this.lbl_ForgeRole, 0, 5);
            this.tableLayoutPanel_Credits.Controls.Add(this.link_ForgeOnProfile, 1, 5);
            this.tableLayoutPanel_Credits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Credits.Location = new System.Drawing.Point(3, 197);
            this.tableLayoutPanel_Credits.Name = "tableLayoutPanel_Credits";
            this.tableLayoutPanel_Credits.RowCount = 6;
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Credits.Size = new System.Drawing.Size(379, 189);
            this.tableLayoutPanel_Credits.TabIndex = 2;
            // 
            // lbl_Credits
            // 
            this.lbl_Credits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Credits.AutoSize = true;
            this.lbl_Credits.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Credits.Location = new System.Drawing.Point(54, 29);
            this.lbl_Credits.Name = "lbl_Credits";
            this.lbl_Credits.Size = new System.Drawing.Size(81, 30);
            this.lbl_Credits.TabIndex = 0;
            this.lbl_Credits.Text = "Credits";
            this.lbl_Credits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Credits_Description
            // 
            this.lbl_Credits_Description.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Credits_Description.AutoSize = true;
            this.lbl_Credits_Description.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Credits_Description.Location = new System.Drawing.Point(194, 7);
            this.lbl_Credits_Description.Name = "lbl_Credits_Description";
            this.lbl_Credits_Description.Size = new System.Drawing.Size(179, 75);
            this.lbl_Credits_Description.TabIndex = 0;
            this.lbl_Credits_Description.Text = "Thanks to these people this app was created:";
            this.lbl_Credits_Description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UnleashedRole
            // 
            this.lbl_UnleashedRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UnleashedRole.AutoSize = true;
            this.lbl_UnleashedRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UnleashedRole.Location = new System.Drawing.Point(23, 89);
            this.lbl_UnleashedRole.Name = "lbl_UnleashedRole";
            this.lbl_UnleashedRole.Size = new System.Drawing.Size(143, 19);
            this.lbl_UnleashedRole.TabIndex = 0;
            this.lbl_UnleashedRole.Text = "CustomsForge owner:";
            this.lbl_UnleashedRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_DarjuszRole
            // 
            this.lbl_DarjuszRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_DarjuszRole.AutoSize = true;
            this.lbl_DarjuszRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DarjuszRole.Location = new System.Drawing.Point(41, 109);
            this.lbl_DarjuszRole.Name = "lbl_DarjuszRole";
            this.lbl_DarjuszRole.Size = new System.Drawing.Size(107, 19);
            this.lbl_DarjuszRole.TabIndex = 0;
            this.lbl_DarjuszRole.Text = "Lead Developer:";
            this.lbl_DarjuszRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_LovromanRole
            // 
            this.lbl_LovromanRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_LovromanRole.AutoSize = true;
            this.lbl_LovromanRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LovromanRole.Location = new System.Drawing.Point(57, 129);
            this.lbl_LovromanRole.Name = "lbl_LovromanRole";
            this.lbl_LovromanRole.Size = new System.Drawing.Size(74, 19);
            this.lbl_LovromanRole.TabIndex = 0;
            this.lbl_LovromanRole.Text = "Developer:";
            this.lbl_LovromanRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_AlexRole
            // 
            this.lbl_AlexRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_AlexRole.AutoSize = true;
            this.lbl_AlexRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_AlexRole.Location = new System.Drawing.Point(57, 149);
            this.lbl_AlexRole.Name = "lbl_AlexRole";
            this.lbl_AlexRole.Size = new System.Drawing.Size(74, 19);
            this.lbl_AlexRole.TabIndex = 0;
            this.lbl_AlexRole.Text = "Developer:";
            this.lbl_AlexRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // link_UnleashedProfile
            // 
            this.link_UnleashedProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_UnleashedProfile.AutoSize = true;
            this.link_UnleashedProfile.LinkColor = System.Drawing.Color.Red;
            this.link_UnleashedProfile.Location = new System.Drawing.Point(249, 92);
            this.link_UnleashedProfile.Name = "link_UnleashedProfile";
            this.link_UnleashedProfile.Size = new System.Drawing.Size(70, 13);
            this.link_UnleashedProfile.TabIndex = 1;
            this.link_UnleashedProfile.TabStop = true;
            this.link_UnleashedProfile.Text = "Unleashed2k";
            this.link_UnleashedProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_UnleashedProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_UnleashedProfile_LinkClicked);
            // 
            // link_DarjuszProfile
            // 
            this.link_DarjuszProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_DarjuszProfile.AutoSize = true;
            this.link_DarjuszProfile.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.link_DarjuszProfile.Location = new System.Drawing.Point(263, 112);
            this.link_DarjuszProfile.Name = "link_DarjuszProfile";
            this.link_DarjuszProfile.Size = new System.Drawing.Size(42, 13);
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
            this.link_LovromanProfile.Location = new System.Drawing.Point(257, 132);
            this.link_LovromanProfile.Name = "link_LovromanProfile";
            this.link_LovromanProfile.Size = new System.Drawing.Size(54, 13);
            this.link_LovromanProfile.TabIndex = 1;
            this.link_LovromanProfile.TabStop = true;
            this.link_LovromanProfile.Text = "Lovroman";
            this.link_LovromanProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_LovromanProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LovromanProfile_LinkClicked);
            // 
            // link_Alex360Profile
            // 
            this.link_Alex360Profile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_Alex360Profile.AutoSize = true;
            this.link_Alex360Profile.Location = new System.Drawing.Point(261, 152);
            this.link_Alex360Profile.Name = "link_Alex360Profile";
            this.link_Alex360Profile.Size = new System.Drawing.Size(45, 13);
            this.link_Alex360Profile.TabIndex = 1;
            this.link_Alex360Profile.TabStop = true;
            this.link_Alex360Profile.Text = "Alex360";
            this.link_Alex360Profile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_Alex360Profile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_Alex360Profile_LinkClicked);
            // 
            // lbl_ForgeRole
            // 
            this.lbl_ForgeRole.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_ForgeRole.AutoSize = true;
            this.lbl_ForgeRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ForgeRole.Location = new System.Drawing.Point(70, 169);
            this.lbl_ForgeRole.Name = "lbl_ForgeRole";
            this.lbl_ForgeRole.Size = new System.Drawing.Size(49, 19);
            this.lbl_ForgeRole.TabIndex = 0;
            this.lbl_ForgeRole.Text = "Tester:";
            this.lbl_ForgeRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // link_ForgeOnProfile
            // 
            this.link_ForgeOnProfile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.link_ForgeOnProfile.AutoSize = true;
            this.link_ForgeOnProfile.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.link_ForgeOnProfile.Location = new System.Drawing.Point(260, 172);
            this.link_ForgeOnProfile.Name = "link_ForgeOnProfile";
            this.link_ForgeOnProfile.Size = new System.Drawing.Size(48, 13);
            this.link_ForgeOnProfile.TabIndex = 1;
            this.link_ForgeOnProfile.TabStop = true;
            this.link_ForgeOnProfile.Text = "ForgeOn";
            this.link_ForgeOnProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link_ForgeOnProfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_ForgeOnProfile_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::CustomsForgeManager_Winforms.Properties.Resources.logo_black;
            this.pictureBox1.Location = new System.Drawing.Point(67, 69);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 56);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
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
            this.toolStripProgressBarMain,
            this.toolStripStatusLabel_Main});
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
            // toolStripStatusLabel_Main
            // 
            this.toolStripStatusLabel_Main.Name = "toolStripStatusLabel_Main";
            this.toolStripStatusLabel_Main.Size = new System.Drawing.Size(0, 17);
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
            // btnLaunchRocksmith
            // 
            this.btnLaunchRocksmith.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLaunchRocksmith.Location = new System.Drawing.Point(294, 54);
            this.btnLaunchRocksmith.Name = "btnLaunchRocksmith";
            this.btnLaunchRocksmith.Size = new System.Drawing.Size(150, 23);
            this.btnLaunchRocksmith.TabIndex = 3;
            this.btnLaunchRocksmith.Text = "Launch Rocksmith";
            this.btnLaunchRocksmith.UseVisualStyleBackColor = true;
            this.btnLaunchRocksmith.Click += new System.EventHandler(this.btnLaunchRocksmith_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tlp_MainForm_Wrappper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomsForge Song Manager";
            this.tlp_MainForm_Wrappper.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            this.gbLog.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpManager.ResumeLayout(false);
            this.tlpSongListWrapper.ResumeLayout(false);
            this.panelSongListButtons.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).EndInit();
            this.contextMenuStrip_MainManager.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tpDuplicates.ResumeLayout(false);
            this.tlpDuplicates.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tpUtilities.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tpSettings.ResumeLayout(false);
            this.tlpSettings_Wrapper.ResumeLayout(false);
            this.tlpSettings_Wrapper.PerformLayout();
            this.tpAbout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAboutLogo)).EndInit();
            this.tableLayoutPanel_Credits.ResumeLayout(false);
            this.tableLayoutPanel_Credits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
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
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_MainManager;
        private System.Windows.Forms.ToolStripMenuItem openDLCPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editDLCToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Main;
        private System.Windows.Forms.TabPage tpDuplicates;
        private System.Windows.Forms.TableLayoutPanel tlpDuplicates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeleteSongTwo;
        private System.Windows.Forms.Button btnDeleteDupeSongOme;
        private System.Windows.Forms.Button btnDeleteSongOne;
        private System.Windows.Forms.Button btnDupeRescan;
        private System.Windows.Forms.Panel panelSongListButtons;
        private System.Windows.Forms.Button btnSaveDLC;
        private System.Windows.Forms.Button btnBackupDLC;
        private System.Windows.Forms.Button btnDLCPage;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnEditDLC;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvSongs;
        private System.Windows.Forms.ListView listDupeSongs;
        private System.Windows.Forms.ColumnHeader colDupeSelect;
        private System.Windows.Forms.ColumnHeader colDupeArtist;
        private System.Windows.Forms.ColumnHeader colDupeSong;
        private System.Windows.Forms.ColumnHeader colDupeAlbum;
        private System.Windows.Forms.ColumnHeader colDupeSongOnePath;
        private System.Windows.Forms.ColumnHeader colDupeSongTwoPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBackup;
        private System.Windows.Forms.DataGridViewLinkColumn colPreview;
        private System.Windows.Forms.Label lbl_Search;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Credits;
        private System.Windows.Forms.Label lbl_Credits;
        private System.Windows.Forms.Label lbl_Credits_Description;
        private System.Windows.Forms.Label lbl_UnleashedRole;
        private System.Windows.Forms.Label lbl_DarjuszRole;
        private System.Windows.Forms.Label lbl_LovromanRole;
        private System.Windows.Forms.Label lbl_AlexRole;
        private System.Windows.Forms.LinkLabel link_UnleashedProfile;
        private System.Windows.Forms.LinkLabel link_DarjuszProfile;
        private System.Windows.Forms.LinkLabel link_LovromanProfile;
        private System.Windows.Forms.LinkLabel link_Alex360Profile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_SettingsRSPath;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_ForgeRole;
        private System.Windows.Forms.LinkLabel link_ForgeOnProfile;
        private System.Windows.Forms.CheckBox checkRescanOnStartup;
        private System.Windows.Forms.TabPage tpUtilities;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnBackupRSProfile;
        private System.Windows.Forms.Button btnLaunchRocksmith;
    }
}

