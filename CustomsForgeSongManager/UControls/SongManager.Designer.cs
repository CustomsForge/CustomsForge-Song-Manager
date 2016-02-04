using System;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class SongManager
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.chkSubFolders = new System.Windows.Forms.CheckBox();
            this.btnCheckAllForUpdates = new System.Windows.Forms.Button();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.chkTheMover = new System.Windows.Forms.CheckBox();
            this.gb_Main_Search = new System.Windows.Forms.GroupBox();
            this.panelSongListButtons = new System.Windows.Forms.Panel();
            this.cbMyCDLC = new System.Windows.Forms.CheckBox();
            this.btnDeleteSongs = new System.Windows.Forms.Button();
            this.chkEnableDelete = new System.Windows.Forms.CheckBox();
            this.btnBulkActions = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.gb_Main_Actions = new System.Windows.Forms.GroupBox();
            this.sfdSongListToCSV = new System.Windows.Forms.SaveFileDialog();
            this.cmsGetCharterName = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSongManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsShowSongInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsOpenSongPage = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsOpenSongLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEditSong = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDeleteSong = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsBackupSong = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTaggerPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.gb_Main_Grid = new System.Windows.Forms.GroupBox();
            this.lnklblToggle = new System.Windows.Forms.LinkLabel();
            this.dgvSongsDetail = new System.Windows.Forms.DataGridView();
            this.colDetailKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailArrangement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailSections = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailDMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailToneBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSongsMaster = new DataGridViewTools.RADataGridView();
            this.colShowDetail = new System.Windows.Forms.DataGridViewImageColumn();
            this.colKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBass = new System.Windows.Forms.DataGridViewImageColumn();
            this.colLead = new System.Windows.Forms.DataGridViewImageColumn();
            this.colRhythm = new System.Windows.Forms.DataGridViewImageColumn();
            this.colVocals = new System.Windows.Forms.DataGridViewImageColumn();
            this.colEnabled = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangements = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDD = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvgTempo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colCharter = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToolkitVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTagged = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtistTitleAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtistSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitleSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbumSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.cmsSongManagerColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsSelection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.disableEnableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelSearch.SuspendLayout();
            this.gb_Main_Search.SuspendLayout();
            this.panelSongListButtons.SuspendLayout();
            this.gb_Main_Actions.SuspendLayout();
            this.cmsSongManager.SuspendLayout();
            this.gb_Main_Grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsMaster)).BeginInit();
            this.cmsSongManagerColumns.SuspendLayout();
            this.cmsSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSearch
            // 
            this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearch.Controls.Add(this.chkSubFolders);
            this.panelSearch.Controls.Add(this.btnCheckAllForUpdates);
            this.panelSearch.Controls.Add(this.lnkClearSearch);
            this.panelSearch.Controls.Add(this.lbl_Search);
            this.panelSearch.Controls.Add(this.cueSearch);
            this.panelSearch.Location = new System.Drawing.Point(6, 19);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(879, 33);
            this.panelSearch.TabIndex = 5;
            // 
            // chkSubFolders
            // 
            this.chkSubFolders.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkSubFolders.AutoSize = true;
            this.chkSubFolders.Checked = true;
            this.chkSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubFolders.Location = new System.Drawing.Point(522, 8);
            this.chkSubFolders.Name = "chkSubFolders";
            this.chkSubFolders.Size = new System.Drawing.Size(125, 17);
            this.chkSubFolders.TabIndex = 24;
            this.chkSubFolders.Text = "Include Setlist Songs";
            this.chkSubFolders.UseVisualStyleBackColor = true;
            this.chkSubFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkSubFolders_MouseUp);
            // 
            // btnCheckAllForUpdates
            // 
            this.btnCheckAllForUpdates.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCheckAllForUpdates.Image = global::CustomsForgeSongManager.Properties.Resources.update;
            this.btnCheckAllForUpdates.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCheckAllForUpdates.Location = new System.Drawing.Point(737, 2);
            this.btnCheckAllForUpdates.Name = "btnCheckAllForUpdates";
            this.btnCheckAllForUpdates.Size = new System.Drawing.Size(135, 29);
            this.btnCheckAllForUpdates.TabIndex = 4;
            this.btnCheckAllForUpdates.Text = "Check All for Update";
            this.btnCheckAllForUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCheckAllForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckAllForUpdates.Visible = false;
            this.btnCheckAllForUpdates.Click += new System.EventHandler(this.btnCheckAllForUpdates_Click);
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(411, 10);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 3;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // lbl_Search
            // 
            this.lbl_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Search.AutoSize = true;
            this.lbl_Search.Location = new System.Drawing.Point(3, 9);
            this.lbl_Search.Name = "lbl_Search";
            this.lbl_Search.Size = new System.Drawing.Size(0, 13);
            this.lbl_Search.TabIndex = 2;
            // 
            // cueSearch
            // 
            this.cueSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cueSearch.Cue = "Type characters to search...";
            this.cueSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueSearch.Location = new System.Drawing.Point(9, 6);
            this.cueSearch.Name = "cueSearch";
            this.cueSearch.Size = new System.Drawing.Size(396, 20);
            this.cueSearch.TabIndex = 1;
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
            // 
            // chkTheMover
            // 
            this.chkTheMover.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkTheMover.AutoSize = true;
            this.chkTheMover.Location = new System.Drawing.Point(478, 13);
            this.chkTheMover.Name = "chkTheMover";
            this.chkTheMover.Size = new System.Drawing.Size(241, 17);
            this.chkTheMover.TabIndex = 20;
            this.chkTheMover.Text = "\'The\' Mover e.g., The Beatles -> Beatles, The\r\n";
            this.chkTheMover.UseVisualStyleBackColor = true;
            this.chkTheMover.CheckedChanged += new System.EventHandler(this.chkTheMover_CheckedChanged);
            // 
            // gb_Main_Search
            // 
            this.gb_Main_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Search.Controls.Add(this.panelSearch);
            this.gb_Main_Search.Location = new System.Drawing.Point(3, 3);
            this.gb_Main_Search.Name = "gb_Main_Search";
            this.gb_Main_Search.Size = new System.Drawing.Size(891, 58);
            this.gb_Main_Search.TabIndex = 10;
            this.gb_Main_Search.TabStop = false;
            this.gb_Main_Search.Text = "Search:";
            // 
            // panelSongListButtons
            // 
            this.panelSongListButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSongListButtons.Controls.Add(this.chkTheMover);
            this.panelSongListButtons.Controls.Add(this.cbMyCDLC);
            this.panelSongListButtons.Controls.Add(this.btnDeleteSongs);
            this.panelSongListButtons.Controls.Add(this.chkEnableDelete);
            this.panelSongListButtons.Controls.Add(this.btnBulkActions);
            this.panelSongListButtons.Controls.Add(this.btnRescan);
            this.panelSongListButtons.Location = new System.Drawing.Point(9, 13);
            this.panelSongListButtons.Name = "panelSongListButtons";
            this.panelSongListButtons.Size = new System.Drawing.Size(876, 41);
            this.panelSongListButtons.TabIndex = 3;
            // 
            // cbMyCDLC
            // 
            this.cbMyCDLC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMyCDLC.AutoSize = true;
            this.cbMyCDLC.Location = new System.Drawing.Point(744, 13);
            this.cbMyCDLC.Name = "cbMyCDLC";
            this.cbMyCDLC.Size = new System.Drawing.Size(125, 17);
            this.cbMyCDLC.TabIndex = 23;
            this.cbMyCDLC.Text = "Show MY CDLC only";
            this.cbMyCDLC.UseVisualStyleBackColor = true;
            this.cbMyCDLC.CheckedChanged += new System.EventHandler(this.cbMyCDLC_CheckedChanged);
            // 
            // btnDeleteSongs
            // 
            this.btnDeleteSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSongs.Enabled = false;
            this.btnDeleteSongs.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteSongs.Image")));
            this.btnDeleteSongs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSongs.Location = new System.Drawing.Point(211, 6);
            this.btnDeleteSongs.Name = "btnDeleteSongs";
            this.btnDeleteSongs.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnDeleteSongs.Size = new System.Drawing.Size(118, 29);
            this.btnDeleteSongs.TabIndex = 22;
            this.btnDeleteSongs.Text = "Delete Selection";
            this.btnDeleteSongs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnDeleteSongs, "WARNING!\r\nTHIS IS A PERMENANT DELETE");
            this.btnDeleteSongs.UseVisualStyleBackColor = true;
            this.btnDeleteSongs.Click += new System.EventHandler(this.btnDeleteSongs_Click);
            // 
            // chkEnableDelete
            // 
            this.chkEnableDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnableDelete.AutoSize = true;
            this.chkEnableDelete.Location = new System.Drawing.Point(335, 13);
            this.chkEnableDelete.Name = "chkEnableDelete";
            this.chkEnableDelete.Size = new System.Drawing.Size(100, 17);
            this.chkEnableDelete.TabIndex = 21;
            this.chkEnableDelete.Text = "Safety Interlock";
            this.toolTip.SetToolTip(this.chkEnableDelete, resources.GetString("chkEnableDelete.ToolTip"));
            this.chkEnableDelete.UseVisualStyleBackColor = true;
            this.chkEnableDelete.CheckedChanged += new System.EventHandler(this.chkEnableDelete_CheckedChanged);
            // 
            // btnBulkActions
            // 
            this.btnBulkActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBulkActions.Image = global::CustomsForgeSongManager.Properties.Resources.SelectRow;
            this.btnBulkActions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBulkActions.Location = new System.Drawing.Point(96, 6);
            this.btnBulkActions.Name = "btnBulkActions";
            this.btnBulkActions.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnBulkActions.Size = new System.Drawing.Size(101, 29);
            this.btnBulkActions.TabIndex = 14;
            this.btnBulkActions.Text = "Bulk Actions";
            this.btnBulkActions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnBulkActions, "Select a couple of songs and then apply an action.");
            this.btnBulkActions.UseVisualStyleBackColor = true;
            this.btnBulkActions.Click += new System.EventHandler(this.btnBulkActions_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRescan.Image = ((System.Drawing.Image)(resources.GetObject("btnRescan.Image")));
            this.btnRescan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRescan.Location = new System.Drawing.Point(4, 6);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnRescan.Size = new System.Drawing.Size(78, 29);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.btnRescan, "Ctrl+ Rescan button to force a complete rescan");
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // gb_Main_Actions
            // 
            this.gb_Main_Actions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Actions.Controls.Add(this.panelSongListButtons);
            this.gb_Main_Actions.Location = new System.Drawing.Point(3, 427);
            this.gb_Main_Actions.Name = "gb_Main_Actions";
            this.gb_Main_Actions.Size = new System.Drawing.Size(891, 60);
            this.gb_Main_Actions.TabIndex = 9;
            this.gb_Main_Actions.TabStop = false;
            this.gb_Main_Actions.Text = "Actions:";
            // 
            // sfdSongListToCSV
            // 
            this.sfdSongListToCSV.FileName = "songList.csv";
            this.sfdSongListToCSV.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
            // 
            // cmsGetCharterName
            // 
            this.cmsGetCharterName.Name = "cmsGetCharterName";
            this.cmsGetCharterName.Size = new System.Drawing.Size(189, 22);
            this.cmsGetCharterName.Text = "Get Charter\'s Name";
            this.cmsGetCharterName.Visible = false;
            this.cmsGetCharterName.Click += new System.EventHandler(this.cmsGetCharterName_Click);
            // 
            // cmsSongManager
            // 
            this.cmsSongManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsShowSongInfo,
            this.cmsOpenSongPage,
            this.cmsCheckForUpdate,
            this.cmsOpenSongLocation,
            this.cmsEditSong,
            this.cmsGetCharterName,
            this.cmsDeleteSong,
            this.cmsBackupSong,
            this.cmsTaggerPreview});
            this.cmsSongManager.Name = "contextMenuStrip_MainManager";
            this.cmsSongManager.Size = new System.Drawing.Size(190, 202);
            // 
            // cmsShowSongInfo
            // 
            this.cmsShowSongInfo.Image = global::CustomsForgeSongManager.Properties.Resources.info;
            this.cmsShowSongInfo.Name = "cmsShowSongInfo";
            this.cmsShowSongInfo.Size = new System.Drawing.Size(189, 22);
            this.cmsShowSongInfo.Text = "Show Song Info";
            this.cmsShowSongInfo.Click += new System.EventHandler(this.cmsShowDLCInfo_Click);
            // 
            // cmsOpenSongPage
            // 
            this.cmsOpenSongPage.Image = global::CustomsForgeSongManager.Properties.Resources.internet;
            this.cmsOpenSongPage.Name = "cmsOpenSongPage";
            this.cmsOpenSongPage.Size = new System.Drawing.Size(189, 22);
            this.cmsOpenSongPage.Text = "Open Song Page";
            this.cmsOpenSongPage.Visible = false;
            this.cmsOpenSongPage.Click += new System.EventHandler(this.cmsOpenDLCPage_Click);
            // 
            // cmsCheckForUpdate
            // 
            this.cmsCheckForUpdate.Image = global::CustomsForgeSongManager.Properties.Resources.update;
            this.cmsCheckForUpdate.Name = "cmsCheckForUpdate";
            this.cmsCheckForUpdate.Size = new System.Drawing.Size(189, 22);
            this.cmsCheckForUpdate.Text = "Check for Update";
            this.cmsCheckForUpdate.Visible = false;
            this.cmsCheckForUpdate.Click += new System.EventHandler(this.cmsCheckForUpdate_Click);
            // 
            // cmsOpenSongLocation
            // 
            this.cmsOpenSongLocation.Image = global::CustomsForgeSongManager.Properties.Resources.folder_open;
            this.cmsOpenSongLocation.Name = "cmsOpenSongLocation";
            this.cmsOpenSongLocation.Size = new System.Drawing.Size(189, 22);
            this.cmsOpenSongLocation.Text = "Open Song Location";
            this.cmsOpenSongLocation.Click += new System.EventHandler(this.cmsOpenDLCLocation_Click);
            // 
            // cmsEditSong
            // 
            this.cmsEditSong.Image = global::CustomsForgeSongManager.Properties.Resources.edit;
            this.cmsEditSong.Name = "cmsEditSong";
            this.cmsEditSong.Size = new System.Drawing.Size(189, 22);
            this.cmsEditSong.Text = "Edit Song Information";
            this.cmsEditSong.Click += new System.EventHandler(this.cmsEditSong_Click);
            // 
            // cmsDeleteSong
            // 
            this.cmsDeleteSong.Image = global::CustomsForgeSongManager.Properties.Resources.delete;
            this.cmsDeleteSong.Name = "cmsDeleteSong";
            this.cmsDeleteSong.Size = new System.Drawing.Size(189, 22);
            this.cmsDeleteSong.Text = "Delete Song";
            this.cmsDeleteSong.Click += new System.EventHandler(this.cmsDeleteSong_Click);
            // 
            // cmsBackupSong
            // 
            this.cmsBackupSong.Image = global::CustomsForgeSongManager.Properties.Resources.backup;
            this.cmsBackupSong.Name = "cmsBackupSong";
            this.cmsBackupSong.Size = new System.Drawing.Size(189, 22);
            this.cmsBackupSong.Text = "Backup Song";
            this.cmsBackupSong.Click += new System.EventHandler(this.cmsBackupDLC_Click);
            // 
            // cmsTaggerPreview
            // 
            this.cmsTaggerPreview.Name = "cmsTaggerPreview";
            this.cmsTaggerPreview.Size = new System.Drawing.Size(189, 22);
            this.cmsTaggerPreview.Text = "Tagger Preview";
            this.cmsTaggerPreview.Visible = false;
            // 
            // lnkLblSelectAll
            // 
            this.lnkLblSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkLblSelectAll.AutoSize = true;
            this.lnkLblSelectAll.ForeColor = System.Drawing.Color.Black;
            this.lnkLblSelectAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkLblSelectAll.Location = new System.Drawing.Point(12, 330);
            this.lnkLblSelectAll.Name = "lnkLblSelectAll";
            this.lnkLblSelectAll.Size = new System.Drawing.Size(82, 13);
            this.lnkLblSelectAll.TabIndex = 2;
            this.lnkLblSelectAll.TabStop = true;
            this.lnkLblSelectAll.Text = "Select All/None";
            this.lnkLblSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lnkLblSelectAll, "ODLC are not selectable");
            this.lnkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSelectAll_LinkClicked);
            // 
            // gb_Main_Grid
            // 
            this.gb_Main_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Grid.Controls.Add(this.lnklblToggle);
            this.gb_Main_Grid.Controls.Add(this.dgvSongsDetail);
            this.gb_Main_Grid.Controls.Add(this.lnkLblSelectAll);
            this.gb_Main_Grid.Controls.Add(this.dgvSongsMaster);
            this.gb_Main_Grid.Location = new System.Drawing.Point(3, 67);
            this.gb_Main_Grid.Name = "gb_Main_Grid";
            this.gb_Main_Grid.Size = new System.Drawing.Size(891, 354);
            this.gb_Main_Grid.TabIndex = 8;
            this.gb_Main_Grid.TabStop = false;
            this.gb_Main_Grid.Text = "Results Grid:";
            // 
            // lnklblToggle
            // 
            this.lnklblToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnklblToggle.AutoSize = true;
            this.lnklblToggle.ForeColor = System.Drawing.Color.Black;
            this.lnklblToggle.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnklblToggle.Location = new System.Drawing.Point(104, 330);
            this.lnklblToggle.Name = "lnklblToggle";
            this.lnklblToggle.Size = new System.Drawing.Size(87, 13);
            this.lnklblToggle.TabIndex = 4;
            this.lnklblToggle.TabStop = true;
            this.lnklblToggle.Text = "Toggle Selection";
            this.lnklblToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lnklblToggle, "ODLC are not toggleable");
            this.lnklblToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToggle_LinkClicked);
            // 
            // dgvSongsDetail
            // 
            this.dgvSongsDetail.AllowUserToAddRows = false;
            this.dgvSongsDetail.AllowUserToDeleteRows = false;
            this.dgvSongsDetail.AllowUserToResizeColumns = false;
            this.dgvSongsDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvSongsDetail.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvSongsDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongsDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSongsDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongsDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailKey,
            this.colDetailPID,
            this.colDetailArrangement,
            this.colDetailTuning,
            this.colDetailSections,
            this.colDetailDMax,
            this.colDetailToneBase});
            this.dgvSongsDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSongsDetail.GridColor = System.Drawing.SystemColors.InfoText;
            this.dgvSongsDetail.Location = new System.Drawing.Point(29, 65);
            this.dgvSongsDetail.Name = "dgvSongsDetail";
            this.dgvSongsDetail.ReadOnly = true;
            this.dgvSongsDetail.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvSongsDetail.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSongsDetail.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            this.dgvSongsDetail.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dgvSongsDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dgvSongsDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvSongsDetail.Size = new System.Drawing.Size(813, 70);
            this.dgvSongsDetail.TabIndex = 3;
            // 
            // colDetailKey
            // 
            this.colDetailKey.DataPropertyName = "DLCKey";
            this.colDetailKey.HeaderText = "DLC Key";
            this.colDetailKey.Name = "colDetailKey";
            this.colDetailKey.ReadOnly = true;
            this.colDetailKey.Width = 95;
            // 
            // colDetailPID
            // 
            this.colDetailPID.DataPropertyName = "PersistentID";
            this.colDetailPID.HeaderText = "Persistent ID";
            this.colDetailPID.Name = "colDetailPID";
            this.colDetailPID.ReadOnly = true;
            this.colDetailPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDetailPID.Width = 250;
            // 
            // colDetailArrangement
            // 
            this.colDetailArrangement.DataPropertyName = "Name";
            this.colDetailArrangement.HeaderText = "Arrangement";
            this.colDetailArrangement.Name = "colDetailArrangement";
            this.colDetailArrangement.ReadOnly = true;
            // 
            // colDetailTuning
            // 
            this.colDetailTuning.DataPropertyName = "Tuning";
            this.colDetailTuning.HeaderText = "Tuning";
            this.colDetailTuning.Name = "colDetailTuning";
            this.colDetailTuning.ReadOnly = true;
            // 
            // colDetailSections
            // 
            this.colDetailSections.DataPropertyName = "SectionCount";
            this.colDetailSections.HeaderText = "Sections";
            this.colDetailSections.Name = "colDetailSections";
            this.colDetailSections.ReadOnly = true;
            this.colDetailSections.Width = 60;
            // 
            // colDetailDMax
            // 
            this.colDetailDMax.DataPropertyName = "DMax";
            this.colDetailDMax.HeaderText = "DMax";
            this.colDetailDMax.Name = "colDetailDMax";
            this.colDetailDMax.ReadOnly = true;
            this.colDetailDMax.Width = 55;
            // 
            // colDetailToneBase
            // 
            this.colDetailToneBase.DataPropertyName = "ToneBase";
            this.colDetailToneBase.HeaderText = "Tone Base";
            this.colDetailToneBase.Name = "colDetailToneBase";
            this.colDetailToneBase.ReadOnly = true;
            this.colDetailToneBase.Width = 150;
            // 
            // dgvSongsMaster
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongsMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSongsMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongsMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSongsMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShowDetail,
            this.colKey,
            this.colSelect,
            this.colBass,
            this.colLead,
            this.colRhythm,
            this.colVocals,
            this.colEnabled,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colSongYear,
            this.colArrangements,
            this.colTuning,
            this.colDD,
            this.colSongLength,
            this.colAvgTempo,
            this.colAppID,
            this.colPath,
            this.colFileName,
            this.colStatus,
            this.colCharter,
            this.colUpdated,
            this.colVersion,
            this.colToolkitVersion,
            this.colTagged,
            this.colIgnitionID,
            this.colIgnitionUpdated,
            this.colIgnitionVersion,
            this.colIgnitionAuthor,
            this.colArtistTitleAlbum,
            this.colArtistSort,
            this.colTitleSort,
            this.colAlbumSort});
            this.dgvSongsMaster.Location = new System.Drawing.Point(6, 19);
            this.dgvSongsMaster.Name = "dgvSongsMaster";
            this.dgvSongsMaster.RowHeadersVisible = false;
            this.dgvSongsMaster.Size = new System.Drawing.Size(879, 308);
            this.dgvSongsMaster.TabIndex = 1;
            this.dgvSongsMaster.Tag = "Song Manager";
            this.dgvSongsMaster.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongsMaster_CellContentClick);
            this.dgvSongsMaster.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongsMaster_CellDoubleClick);
            this.dgvSongsMaster.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSongsMaster_CellFormatting);
            this.dgvSongsMaster.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_CellMouseDown);
            this.dgvSongsMaster.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_CellMouseUp);
            this.dgvSongsMaster.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_ColumnHeaderMouseClick);
            this.dgvSongsMaster.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSongsMaster_DataBindingComplete);
            this.dgvSongsMaster.Sorted += new System.EventHandler(this.dgvSongsMaster_Sorted);
            this.dgvSongsMaster.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSongsMaster_Paint);
            this.dgvSongsMaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSongsMaster_KeyDown);
            this.dgvSongsMaster.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvSongsMaster_KeyUp);
            // 
            // colShowDetail
            // 
            this.colShowDetail.DataPropertyName = "ShowDetail";
            this.colShowDetail.HeaderText = "";
            this.colShowDetail.Image = global::CustomsForgeSongManager.Properties.Resources.plus;
            this.colShowDetail.Name = "colShowDetail";
            this.colShowDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colShowDetail.Width = 20;
            // 
            // colKey
            // 
            this.colKey.DataPropertyName = "DLCKey";
            this.colKey.HeaderText = "DLC Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            this.colKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colKey.Width = 95;
            // 
            // colSelect
            // 
            this.colSelect.DataPropertyName = "Selected";
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 50;
            // 
            // colBass
            // 
            this.colBass.DataPropertyName = "Bass";
            this.colBass.HeaderText = "B";
            this.colBass.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_B;
            this.colBass.MinimumWidth = 21;
            this.colBass.Name = "colBass";
            this.colBass.ReadOnly = true;
            this.colBass.ToolTipText = "Bass";
            this.colBass.Visible = false;
            this.colBass.Width = 21;
            // 
            // colLead
            // 
            this.colLead.DataPropertyName = "Lead";
            this.colLead.HeaderText = "L";
            this.colLead.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_L;
            this.colLead.MinimumWidth = 21;
            this.colLead.Name = "colLead";
            this.colLead.ReadOnly = true;
            this.colLead.ToolTipText = "Lead";
            this.colLead.Visible = false;
            this.colLead.Width = 21;
            // 
            // colRhythm
            // 
            this.colRhythm.DataPropertyName = "Rhythm";
            this.colRhythm.HeaderText = "R";
            this.colRhythm.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_R;
            this.colRhythm.MinimumWidth = 21;
            this.colRhythm.Name = "colRhythm";
            this.colRhythm.ReadOnly = true;
            this.colRhythm.ToolTipText = "Rhythm";
            this.colRhythm.Visible = false;
            this.colRhythm.Width = 21;
            // 
            // colVocals
            // 
            this.colVocals.DataPropertyName = "Vocals";
            this.colVocals.HeaderText = "V";
            this.colVocals.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_V;
            this.colVocals.MinimumWidth = 21;
            this.colVocals.Name = "colVocals";
            this.colVocals.ReadOnly = true;
            this.colVocals.ToolTipText = "Vocals";
            this.colVocals.Visible = false;
            this.colVocals.Width = 21;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnabled.Width = 72;
            // 
            // colArtist
            // 
            this.colArtist.DataPropertyName = "Artist";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Song Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colAlbum
            // 
            this.colAlbum.DataPropertyName = "Album";
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
            this.colSongYear.HeaderText = "Year";
            this.colSongYear.Name = "colSongYear";
            this.colSongYear.ReadOnly = true;
            this.colSongYear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongYear.Width = 50;
            // 
            // colArrangements
            // 
            this.colArrangements.DataPropertyName = "Arrangements";
            this.colArrangements.HeaderText = "Arrangements";
            this.colArrangements.Name = "colArrangements";
            this.colArrangements.ReadOnly = true;
            this.colArrangements.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArrangements.Visible = false;
            this.colArrangements.Width = 50;
            // 
            // colTuning
            // 
            this.colTuning.DataPropertyName = "Tuning";
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTuning.Visible = false;
            // 
            // colDD
            // 
            this.colDD.DataPropertyName = "DD";
            this.colDD.HeaderText = "DD";
            this.colDD.Name = "colDD";
            this.colDD.ReadOnly = true;
            this.colDD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDD.Visible = false;
            // 
            // colSongLength
            // 
            this.colSongLength.DataPropertyName = "SongLength";
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.colSongLength.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSongLength.HeaderText = "Length Seconds";
            this.colSongLength.Name = "colSongLength";
            this.colSongLength.ReadOnly = true;
            this.colSongLength.Visible = false;
            // 
            // colAvgTempo
            // 
            this.colAvgTempo.DataPropertyName = "SongAverageTempo";
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.colAvgTempo.DefaultCellStyle = dataGridViewCellStyle6;
            this.colAvgTempo.HeaderText = "BPM";
            this.colAvgTempo.Name = "colAvgTempo";
            this.colAvgTempo.ReadOnly = true;
            this.colAvgTempo.Visible = false;
            // 
            // colAppID
            // 
            this.colAppID.DataPropertyName = "AppID";
            this.colAppID.HeaderText = "App ID";
            this.colAppID.Name = "colAppID";
            this.colAppID.ReadOnly = true;
            this.colAppID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAppID.Width = 80;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPath.Width = 150;
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Visible = false;
            this.colFileName.Width = 50;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStatus.Visible = false;
            this.colStatus.Width = 50;
            // 
            // colCharter
            // 
            this.colCharter.DataPropertyName = "Charter";
            this.colCharter.HeaderText = "Charter";
            this.colCharter.Name = "colCharter";
            this.colCharter.ReadOnly = true;
            this.colCharter.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCharter.Visible = false;
            this.colCharter.Width = 50;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "LastConversionDateTime";
            dataGridViewCellStyle7.NullValue = null;
            this.colUpdated.DefaultCellStyle = dataGridViewCellStyle7;
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Visible = false;
            this.colUpdated.Width = 50;
            // 
            // colVersion
            // 
            this.colVersion.DataPropertyName = "Version";
            this.colVersion.HeaderText = "Version";
            this.colVersion.Name = "colVersion";
            this.colVersion.ReadOnly = true;
            this.colVersion.Visible = false;
            this.colVersion.Width = 50;
            // 
            // colToolkitVersion
            // 
            this.colToolkitVersion.DataPropertyName = "ToolkitVer";
            this.colToolkitVersion.HeaderText = "Toolkit Version";
            this.colToolkitVersion.Name = "colToolkitVersion";
            this.colToolkitVersion.ReadOnly = true;
            this.colToolkitVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToolkitVersion.Width = 110;
            // 
            // colTagged
            // 
            this.colTagged.DataPropertyName = "Tagged";
            this.colTagged.HeaderText = "Tagged";
            this.colTagged.Name = "colTagged";
            this.colTagged.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTagged.Width = 72;
            // 
            // colIgnitionID
            // 
            this.colIgnitionID.DataPropertyName = "IgnitionID";
            this.colIgnitionID.HeaderText = "Ignition ID";
            this.colIgnitionID.Name = "colIgnitionID";
            this.colIgnitionID.ReadOnly = true;
            this.colIgnitionID.Visible = false;
            this.colIgnitionID.Width = 50;
            // 
            // colIgnitionUpdated
            // 
            this.colIgnitionUpdated.DataPropertyName = "IgnitionUpdated";
            dataGridViewCellStyle8.NullValue = null;
            this.colIgnitionUpdated.DefaultCellStyle = dataGridViewCellStyle8;
            this.colIgnitionUpdated.HeaderText = "Ignition Updated";
            this.colIgnitionUpdated.Name = "colIgnitionUpdated";
            this.colIgnitionUpdated.ReadOnly = true;
            this.colIgnitionUpdated.Visible = false;
            this.colIgnitionUpdated.Width = 50;
            // 
            // colIgnitionVersion
            // 
            this.colIgnitionVersion.DataPropertyName = "IgnitionVersion";
            this.colIgnitionVersion.HeaderText = "Ignition Version";
            this.colIgnitionVersion.Name = "colIgnitionVersion";
            this.colIgnitionVersion.ReadOnly = true;
            this.colIgnitionVersion.Visible = false;
            this.colIgnitionVersion.Width = 50;
            // 
            // colIgnitionAuthor
            // 
            this.colIgnitionAuthor.DataPropertyName = "IgnitionAuthor";
            this.colIgnitionAuthor.HeaderText = "Ignition Author";
            this.colIgnitionAuthor.Name = "colIgnitionAuthor";
            this.colIgnitionAuthor.ReadOnly = true;
            this.colIgnitionAuthor.Visible = false;
            this.colIgnitionAuthor.Width = 50;
            // 
            // colArtistTitleAlbum
            // 
            this.colArtistTitleAlbum.DataPropertyName = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.HeaderText = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.Name = "colArtistTitleAlbum";
            this.colArtistTitleAlbum.ReadOnly = true;
            this.colArtistTitleAlbum.Visible = false;
            // 
            // colArtistSort
            // 
            this.colArtistSort.DataPropertyName = "ArtistSort";
            this.colArtistSort.HeaderText = "In-Game Artist Sort";
            this.colArtistSort.Name = "colArtistSort";
            this.colArtistSort.Width = 50;
            // 
            // colTitleSort
            // 
            this.colTitleSort.DataPropertyName = "TitleSort";
            this.colTitleSort.HeaderText = "In-Game Title Sort";
            this.colTitleSort.Name = "colTitleSort";
            this.colTitleSort.Width = 50;
            // 
            // colAlbumSort
            // 
            this.colAlbumSort.DataPropertyName = "AlbumSort";
            this.colAlbumSort.HeaderText = "In-Game Album Sort";
            this.colAlbumSort.Name = "colAlbumSort";
            this.colAlbumSort.Width = 50;
            // 
            // cmsSongManagerColumns
            // 
            this.cmsSongManagerColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.cmsSongManagerColumns.Name = "cmsSongManagerColumns";
            this.cmsSongManagerColumns.Size = new System.Drawing.Size(107, 26);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Checked = true;
            this.testToolStripMenuItem.CheckOnClick = true;
            this.testToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // cmsSelection
            // 
            this.cmsSelection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableEnableToolStripMenuItem,
            this.backupToolStripMenuItem,
            this.changePropertiesToolStripMenuItem,
            this.tagToolStripMenuItem,
            this.unTagToolStripMenuItem});
            this.cmsSelection.Name = "cmsSelection";
            this.cmsSelection.Size = new System.Drawing.Size(156, 114);
            // 
            // disableEnableToolStripMenuItem
            // 
            this.disableEnableToolStripMenuItem.Image = global::CustomsForgeSongManager.Properties.Resources.disable;
            this.disableEnableToolStripMenuItem.Name = "disableEnableToolStripMenuItem";
            this.disableEnableToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.disableEnableToolStripMenuItem.Text = "Disable/Enable";
            this.disableEnableToolStripMenuItem.Click += new System.EventHandler(this.btnDisableEnableSongs_Click);
            // 
            // backupToolStripMenuItem
            // 
            this.backupToolStripMenuItem.Image = global::CustomsForgeSongManager.Properties.Resources.backup;
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.backupToolStripMenuItem.Text = "Backup";
            this.backupToolStripMenuItem.Click += new System.EventHandler(this.btnBackupSelectedDLCs_Click);
            // 
            // changePropertiesToolStripMenuItem
            // 
            this.changePropertiesToolStripMenuItem.Name = "changePropertiesToolStripMenuItem";
            this.changePropertiesToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.changePropertiesToolStripMenuItem.Text = "Edit App IDs";
            this.changePropertiesToolStripMenuItem.Click += new System.EventHandler(this.changePropertiesToolStripMenuItem_Click);
            // 
            // tagToolStripMenuItem
            // 
            this.tagToolStripMenuItem.Name = "tagToolStripMenuItem";
            this.tagToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.tagToolStripMenuItem.Text = "Tag";
            // 
            // unTagToolStripMenuItem
            // 
            this.unTagToolStripMenuItem.Image = global::CustomsForgeSongManager.Properties.Resources.clear;
            this.unTagToolStripMenuItem.Name = "unTagToolStripMenuItem";
            this.unTagToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.unTagToolStripMenuItem.Text = "Un-Tag";
            this.unTagToolStripMenuItem.Click += new System.EventHandler(this.unTagToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "DlcKey";
            this.dataGridViewTextBoxColumn1.HeaderText = "DLC Key";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 95;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PersistentID";
            this.dataGridViewTextBoxColumn2.HeaderText = "Persistent ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Arrangement";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Tuning";
            this.dataGridViewTextBoxColumn4.HeaderText = "Tuning";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "DMax";
            this.dataGridViewTextBoxColumn5.HeaderText = "DMax";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 48;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ToneBase";
            this.dataGridViewTextBoxColumn6.HeaderText = "Tone Base";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 48;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "ToneBase";
            this.dataGridViewTextBoxColumn7.HeaderText = "Tone Base";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 150;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "Bass";
            this.dataGridViewImageColumn1.HeaderText = "B";
            this.dataGridViewImageColumn1.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_B;
            this.dataGridViewImageColumn1.MinimumWidth = 21;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.ToolTipText = "Bass";
            this.dataGridViewImageColumn1.Width = 21;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.DataPropertyName = "Lead";
            this.dataGridViewImageColumn2.HeaderText = "L";
            this.dataGridViewImageColumn2.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_L;
            this.dataGridViewImageColumn2.MinimumWidth = 21;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.ToolTipText = "Lead";
            this.dataGridViewImageColumn2.Width = 21;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.DataPropertyName = "Rhythm";
            this.dataGridViewImageColumn3.HeaderText = "R";
            this.dataGridViewImageColumn3.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_R;
            this.dataGridViewImageColumn3.MinimumWidth = 21;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.ToolTipText = "Rhythm";
            this.dataGridViewImageColumn3.Width = 21;
            // 
            // dataGridViewImageColumn4
            // 
            this.dataGridViewImageColumn4.DataPropertyName = "Vocals";
            this.dataGridViewImageColumn4.HeaderText = "V";
            this.dataGridViewImageColumn4.Image = global::CustomsForgeSongManager.Properties.Resources.Letter_V;
            this.dataGridViewImageColumn4.MinimumWidth = 21;
            this.dataGridViewImageColumn4.Name = "dataGridViewImageColumn4";
            this.dataGridViewImageColumn4.ReadOnly = true;
            this.dataGridViewImageColumn4.ToolTipText = "Vocals";
            this.dataGridViewImageColumn4.Visible = false;
            this.dataGridViewImageColumn4.Width = 21;
            // 
            // SongManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gb_Main_Search);
            this.Controls.Add(this.gb_Main_Actions);
            this.Controls.Add(this.gb_Main_Grid);
            this.Name = "SongManager";
            this.Size = new System.Drawing.Size(899, 490);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.gb_Main_Search.ResumeLayout(false);
            this.panelSongListButtons.ResumeLayout(false);
            this.panelSongListButtons.PerformLayout();
            this.gb_Main_Actions.ResumeLayout(false);
            this.cmsSongManager.ResumeLayout(false);
            this.gb_Main_Grid.ResumeLayout(false);
            this.gb_Main_Grid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsMaster)).EndInit();
            this.cmsSongManagerColumns.ResumeLayout(false);
            this.cmsSelection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.Label lbl_Search;
        private CueTextBox cueSearch;
        private System.Windows.Forms.GroupBox gb_Main_Search;
        private System.Windows.Forms.Panel panelSongListButtons;
        private System.Windows.Forms.Button btnBulkActions;
        private System.Windows.Forms.Button btnCheckAllForUpdates;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.GroupBox gb_Main_Actions;
        private System.Windows.Forms.SaveFileDialog sfdSongListToCSV;
        private System.Windows.Forms.ToolStripMenuItem cmsShowSongInfo;
        private System.Windows.Forms.ToolStripMenuItem cmsOpenSongPage;
        private System.Windows.Forms.ToolStripMenuItem cmsCheckForUpdate;
        private System.Windows.Forms.ToolStripMenuItem cmsOpenSongLocation;
        private System.Windows.Forms.ToolStripMenuItem cmsEditSong;
        private System.Windows.Forms.ToolStripMenuItem cmsGetCharterName;
        private System.Windows.Forms.ToolStripMenuItem cmsDeleteSong;
        private System.Windows.Forms.ToolStripMenuItem cmsBackupSong;
        private System.Windows.Forms.ContextMenuStrip cmsSongManager;
        private RADataGridView dgvSongsMaster;
        private System.Windows.Forms.LinkLabel lnkLblSelectAll;
        private System.Windows.Forms.GroupBox gb_Main_Grid;
        private System.Windows.Forms.ContextMenuStrip cmsSongManagerColumns;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkTheMover;
        private System.Windows.Forms.Button btnDeleteSongs;
        private System.Windows.Forms.CheckBox chkEnableDelete;
        private System.Windows.Forms.ToolTip toolTip;
        // private RADataGridView dgvSongsDetail;
        private System.Windows.Forms.DataGridView dgvSongsDetail;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.ToolStripMenuItem cmsTaggerPreview;
        private System.Windows.Forms.CheckBox cbMyCDLC;
        private System.Windows.Forms.ContextMenuStrip cmsSelection;
        private System.Windows.Forms.ToolStripMenuItem disableEnableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changePropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unTagToolStripMenuItem;
        private System.Windows.Forms.LinkLabel lnklblToggle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailArrangement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailSections;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailDMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailToneBase;
        private System.Windows.Forms.CheckBox chkSubFolders;
        private System.Windows.Forms.DataGridViewImageColumn colShowDetail;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewImageColumn colBass;
        private System.Windows.Forms.DataGridViewImageColumn colLead;
        private System.Windows.Forms.DataGridViewImageColumn colRhythm;
        private System.Windows.Forms.DataGridViewImageColumn colVocals;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArrangements;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTuning;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colDD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvgTempo;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colStatus;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colCharter;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersion;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTagged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIgnitionID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIgnitionUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIgnitionVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIgnitionAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtistTitleAlbum;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtistSort;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitleSort;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbumSort;
    }
}
