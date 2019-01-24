using System.Windows.Forms;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.chkProtectODLC = new System.Windows.Forms.CheckBox();
            this.chkIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.gb_Main_Search = new System.Windows.Forms.GroupBox();
            this.cmsSongManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsActions = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsOpenSongPage = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsCheckForUpdate = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsGetCharterName = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsPlaySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEdit = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsTaggerPreview = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsShowSongInfo = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsOpenSongLocation = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsEnableDisable = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsDelete = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsBackup = new CustomControls.ToolStripEnhancedMenuItem();
            this.lnkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.gb_Main_Grid = new System.Windows.Forms.GroupBox();
            this.dgvSongsDetail = new DataGridViewTools.SubclassedDataGridView();
            this.colDetailKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailArrangement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailToneBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailDDMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSongsMaster = new DataGridViewTools.RADataGridView();
            this.colShowDetail = new System.Windows.Forms.DataGridViewImageColumn();
            this.colKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBass = new System.Windows.Forms.DataGridViewImageColumn();
            this.colLead = new System.Windows.Forms.DataGridViewImageColumn();
            this.colRhythm = new System.Windows.Forms.DataGridViewImageColumn();
            this.colVocals = new System.Windows.Forms.DataGridViewImageColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colEnabled = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongAverageTempo = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongVolume = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colLastConversionDateTime = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDD = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangements = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTunings = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTones = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileSize = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colToolkitVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageComment = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageRating = new DataGridViewTools.DataGridViewRatingColumn();
            this.colTagged = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbumDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitleSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbumSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.lnklblToggle = new System.Windows.Forms.LinkLabel();
            this.cmsSongManagerColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiRescan = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanQuick = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanFull = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRepairs = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiSkipRemastered = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRepairsAddDD = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDDSettings = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiAddDDNumericUpDown = new CustomControls.ToolStripNumericUpDown();
            this.tsmiAddDDRemoveSustain = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiAddDDCfgPath = new CustomControls.ToolStripSpringTextBox();
            this.tsmiAddDDRampUpPath = new CustomControls.ToolStripSpringTextBox();
            this.tsmiOverwriteDD = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRepairsMastery = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRepairsPreserveStats = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRepairsUsingOrg = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRepairsMultitone = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRepairsMaxFive = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRemoveNDD = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRemoveBass = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRemoveGuitar = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRemoveBonus = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRemoveMetronome = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiIgnoreStopLimit = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAdjustScrollSpeed = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiSSSettings = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiNudScrollSpeed = new CustomControls.ToolStripNumericUpDown();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRemoveSections = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFixLowBass = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDLFolderProcess = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDLFolderMonitor = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDLFolderSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiRepairsRun = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiMods = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsChangeAppId = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiModsPitchShifter = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsPitchShiftStandard = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsPreserveStats = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsPitchShiftOverwrite = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTagStyle = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsTagArtwork = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsUntagArtwork = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiModsTheMover = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiModsMyCDLC = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFiles = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesCheckODLC = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFilesCleanDlc = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesRestore = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesRestoreBak = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesRestoreOrg = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesRestoreMax = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesRestoreCor = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArchive = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArcBak = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArcOrg = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArcMax = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArcCor = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesArcDeleteAfter = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFilesOrganize = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesUnorganize = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiFilesIncludeODLC = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelp = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelpGeneral = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHelpRepairs = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelpErrorLog = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDevDebugUse = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSearch.SuspendLayout();
            this.gb_Main_Search.SuspendLayout();
            this.cmsSongManager.SuspendLayout();
            this.gb_Main_Grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsMaster)).BeginInit();
            this.cmsSongManagerColumns.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSearch
            // 
            this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearch.Controls.Add(this.chkProtectODLC);
            this.panelSearch.Controls.Add(this.chkIncludeSubfolders);
            this.panelSearch.Controls.Add(this.lnkClearSearch);
            this.panelSearch.Controls.Add(this.lbl_Search);
            this.panelSearch.Controls.Add(this.cueSearch);
            this.panelSearch.Location = new System.Drawing.Point(6, 19);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(879, 33);
            this.panelSearch.TabIndex = 5;
            // 
            // chkProtectODLC
            // 
            this.chkProtectODLC.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkProtectODLC.AutoSize = true;
            this.chkProtectODLC.BackColor = System.Drawing.Color.LightGray;
            this.chkProtectODLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkProtectODLC.ForeColor = System.Drawing.Color.Red;
            this.chkProtectODLC.Location = new System.Drawing.Point(739, 4);
            this.chkProtectODLC.Name = "chkProtectODLC";
            this.chkProtectODLC.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.chkProtectODLC.Size = new System.Drawing.Size(124, 23);
            this.chkProtectODLC.TabIndex = 51;
            this.chkProtectODLC.Text = "Protect Official DLC";
            this.toolTip.SetToolTip(this.chkProtectODLC, "If checked, prevents ODLC\r\nfrom being selected, enabled,\r\ndisabled, deleted, or b" +
                    "acked up.");
            this.chkProtectODLC.UseVisualStyleBackColor = false;
            this.chkProtectODLC.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkProtectODLC_MouseUp);
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkIncludeSubfolders.AutoSize = true;
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(538, 7);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(114, 17);
            this.chkIncludeSubfolders.TabIndex = 24;
            this.chkIncludeSubfolders.Text = "Include Subfolders";
            this.toolTip.SetToolTip(this.chkIncludeSubfolders, "If checked, search \'dlc\' folder and \r\nsubfolders for any matching songs.");
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            this.chkIncludeSubfolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkIncludeSubfolders_MouseUp);
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(411, 8);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(100, 13);
            this.lnkClearSearch.TabIndex = 3;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Filters/Search";
            this.toolTip.SetToolTip(this.lnkClearSearch, "HINT:\r\nQuickly clears any Filters, \r\nas well as, any Search, \r\nand preserves exis" +
                    "ting sort.");
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
            this.toolTip.SetToolTip(this.cueSearch, "HINT:\r\nSearching is must faster than filtering.");
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
            // 
            // gb_Main_Search
            // 
            this.gb_Main_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Search.Controls.Add(this.panelSearch);
            this.gb_Main_Search.Location = new System.Drawing.Point(3, 427);
            this.gb_Main_Search.Name = "gb_Main_Search";
            this.gb_Main_Search.Size = new System.Drawing.Size(891, 58);
            this.gb_Main_Search.TabIndex = 10;
            this.gb_Main_Search.TabStop = false;
            this.gb_Main_Search.Text = "Search:";
            // 
            // cmsSongManager
            // 
            this.cmsSongManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsActions,
            this.toolStripSeparator15,
            this.cmsOpenSongPage,
            this.cmsCheckForUpdate,
            this.cmsGetCharterName,
            this.toolStripSeparator11,
            this.cmsPlaySelected,
            this.cmsEdit,
            this.cmsTaggerPreview,
            this.toolStripSeparator2,
            this.cmsShowSongInfo,
            this.cmsOpenSongLocation,
            this.cmsEnableDisable,
            this.cmsDelete,
            this.cmsBackup});
            this.cmsSongManager.Name = "contextMenuStrip_MainManager";
            this.cmsSongManager.Size = new System.Drawing.Size(210, 286);
            this.cmsSongManager.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsSongManager_Closing);
            this.cmsSongManager.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSongManager_ItemClicked);
            // 
            // cmsActions
            // 
            this.cmsActions.AssociatedEnumValue = null;
            this.cmsActions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsActions.Name = "cmsActions";
            this.cmsActions.RadioButtonGroupName = null;
            this.cmsActions.Size = new System.Drawing.Size(209, 22);
            this.cmsActions.Text = "Actions:";
            this.cmsActions.ToolTipText = resources.GetString("cmsActions.ToolTipText");
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(206, 6);
            // 
            // cmsOpenSongPage
            // 
            this.cmsOpenSongPage.AssociatedEnumValue = null;
            this.cmsOpenSongPage.Image = ((System.Drawing.Image)(resources.GetObject("cmsOpenSongPage.Image")));
            this.cmsOpenSongPage.Name = "cmsOpenSongPage";
            this.cmsOpenSongPage.RadioButtonGroupName = null;
            this.cmsOpenSongPage.Size = new System.Drawing.Size(209, 22);
            this.cmsOpenSongPage.Text = "Open Song Page";
            this.cmsOpenSongPage.ToolTipText = "Open Song Page on CF Ignition\r\n(requires an internet connection)";
            this.cmsOpenSongPage.Click += new System.EventHandler(this.cmsOpenSongPage_Click);
            // 
            // cmsCheckForUpdate
            // 
            this.cmsCheckForUpdate.AssociatedEnumValue = null;
            this.cmsCheckForUpdate.Image = ((System.Drawing.Image)(resources.GetObject("cmsCheckForUpdate.Image")));
            this.cmsCheckForUpdate.Name = "cmsCheckForUpdate";
            this.cmsCheckForUpdate.RadioButtonGroupName = null;
            this.cmsCheckForUpdate.Size = new System.Drawing.Size(209, 22);
            this.cmsCheckForUpdate.Text = "Check for Update";
            this.cmsCheckForUpdate.ToolTipText = "Check for Update on CF Ignition\r\n(requires an internet connection)";
            this.cmsCheckForUpdate.Click += new System.EventHandler(this.cmsCheckForUpdate_Click);
            // 
            // cmsGetCharterName
            // 
            this.cmsGetCharterName.AssociatedEnumValue = null;
            this.cmsGetCharterName.Image = global::CustomsForgeSongManager.Properties.Resources.rename;
            this.cmsGetCharterName.Name = "cmsGetCharterName";
            this.cmsGetCharterName.RadioButtonGroupName = null;
            this.cmsGetCharterName.Size = new System.Drawing.Size(209, 22);
            this.cmsGetCharterName.Text = "Get Charter\'s Name";
            this.cmsGetCharterName.Visible = false;
            this.cmsGetCharterName.Click += new System.EventHandler(this.cmsGetCharterName_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(206, 6);
            // 
            // cmsPlaySelected
            // 
            this.cmsPlaySelected.Image = ((System.Drawing.Image)(resources.GetObject("cmsPlaySelected.Image")));
            this.cmsPlaySelected.Name = "cmsPlaySelected";
            this.cmsPlaySelected.Size = new System.Drawing.Size(209, 22);
            this.cmsPlaySelected.Text = "Play/Pause Selected Song";
            this.cmsPlaySelected.Click += new System.EventHandler(this.cmsPlaySelected_Click);
            // 
            // cmsEdit
            // 
            this.cmsEdit.AssociatedEnumValue = null;
            this.cmsEdit.Image = ((System.Drawing.Image)(resources.GetObject("cmsEdit.Image")));
            this.cmsEdit.Name = "cmsEdit";
            this.cmsEdit.RadioButtonGroupName = null;
            this.cmsEdit.Size = new System.Drawing.Size(209, 22);
            this.cmsEdit.Text = "Edit Song Information";
            this.cmsEdit.Click += new System.EventHandler(this.cmsEdit_Click);
            // 
            // cmsTaggerPreview
            // 
            this.cmsTaggerPreview.AssociatedEnumValue = null;
            this.cmsTaggerPreview.Image = global::CustomsForgeSongManager.Properties.Resources.tag;
            this.cmsTaggerPreview.Name = "cmsTaggerPreview";
            this.cmsTaggerPreview.RadioButtonGroupName = null;
            this.cmsTaggerPreview.Size = new System.Drawing.Size(209, 22);
            this.cmsTaggerPreview.Text = "Tagger Preview";
            this.cmsTaggerPreview.ToolTipText = "The full feature to \'Tag Album Artwork\'\r\ncan be found in the \'Custom Mods\' menu.";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(206, 6);
            // 
            // cmsShowSongInfo
            // 
            this.cmsShowSongInfo.AssociatedEnumValue = null;
            this.cmsShowSongInfo.Image = ((System.Drawing.Image)(resources.GetObject("cmsShowSongInfo.Image")));
            this.cmsShowSongInfo.Name = "cmsShowSongInfo";
            this.cmsShowSongInfo.RadioButtonGroupName = null;
            this.cmsShowSongInfo.Size = new System.Drawing.Size(209, 22);
            this.cmsShowSongInfo.Text = "Show Song Info";
            this.cmsShowSongInfo.Click += new System.EventHandler(this.cmsShowSongInfo_Click);
            // 
            // cmsOpenSongLocation
            // 
            this.cmsOpenSongLocation.AssociatedEnumValue = null;
            this.cmsOpenSongLocation.Image = ((System.Drawing.Image)(resources.GetObject("cmsOpenSongLocation.Image")));
            this.cmsOpenSongLocation.Name = "cmsOpenSongLocation";
            this.cmsOpenSongLocation.RadioButtonGroupName = null;
            this.cmsOpenSongLocation.Size = new System.Drawing.Size(209, 22);
            this.cmsOpenSongLocation.Text = "Open Song Location";
            this.cmsOpenSongLocation.Click += new System.EventHandler(this.cmsOpenSongLocation_Click);
            // 
            // cmsEnableDisable
            // 
            this.cmsEnableDisable.AssociatedEnumValue = null;
            this.cmsEnableDisable.Image = global::CustomsForgeSongManager.Properties.Resources.enabledisable;
            this.cmsEnableDisable.Name = "cmsEnableDisable";
            this.cmsEnableDisable.RadioButtonGroupName = null;
            this.cmsEnableDisable.Size = new System.Drawing.Size(209, 22);
            this.cmsEnableDisable.Text = "Enable/Disable Songs";
            this.cmsEnableDisable.ToolTipText = "Select must be checked.";
            this.cmsEnableDisable.Click += new System.EventHandler(this.cmsEnableDisable_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.AssociatedEnumValue = null;
            this.cmsDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmsDelete.Image")));
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.RadioButtonGroupName = null;
            this.cmsDelete.Size = new System.Drawing.Size(209, 22);
            this.cmsDelete.Text = "Delete Songs";
            this.cmsDelete.ToolTipText = "WARNING\r\nDeletion can not be undone.\r\nSelect must be checked.";
            this.cmsDelete.Click += new System.EventHandler(this.cmsDelete_Click);
            // 
            // cmsBackup
            // 
            this.cmsBackup.AssociatedEnumValue = null;
            this.cmsBackup.Image = ((System.Drawing.Image)(resources.GetObject("cmsBackup.Image")));
            this.cmsBackup.Name = "cmsBackup";
            this.cmsBackup.RadioButtonGroupName = null;
            this.cmsBackup.Size = new System.Drawing.Size(209, 22);
            this.cmsBackup.Text = "Backup Songs";
            this.cmsBackup.ToolTipText = "Select must be checked.\r\nSee Log for backup file location.";
            this.cmsBackup.Click += new System.EventHandler(this.cmsBackup_Click);
            // 
            // lnkLblSelectAll
            // 
            this.lnkLblSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkLblSelectAll.AutoSize = true;
            this.lnkLblSelectAll.ForeColor = System.Drawing.Color.Black;
            this.lnkLblSelectAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkLblSelectAll.Location = new System.Drawing.Point(12, 370);
            this.lnkLblSelectAll.Name = "lnkLblSelectAll";
            this.lnkLblSelectAll.Size = new System.Drawing.Size(82, 13);
            this.lnkLblSelectAll.TabIndex = 2;
            this.lnkLblSelectAll.TabStop = true;
            this.lnkLblSelectAll.Text = "Select All/None";
            this.lnkLblSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSelectAll_LinkClicked);
            // 
            // gb_Main_Grid
            // 
            this.gb_Main_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Grid.Controls.Add(this.dgvSongsDetail);
            this.gb_Main_Grid.Controls.Add(this.lnkLblSelectAll);
            this.gb_Main_Grid.Controls.Add(this.dgvSongsMaster);
            this.gb_Main_Grid.Controls.Add(this.lnklblToggle);
            this.gb_Main_Grid.Location = new System.Drawing.Point(3, 27);
            this.gb_Main_Grid.Name = "gb_Main_Grid";
            this.gb_Main_Grid.Size = new System.Drawing.Size(891, 394);
            this.gb_Main_Grid.TabIndex = 8;
            this.gb_Main_Grid.TabStop = false;
            this.gb_Main_Grid.Text = "Songs Grid:";
            // 
            // dgvSongsDetail
            // 
            this.dgvSongsDetail.AllowUserToAddRows = false;
            this.dgvSongsDetail.AllowUserToDeleteRows = false;
            this.dgvSongsDetail.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvSongsDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSongsDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongsDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailKey,
            this.colDetailPID,
            this.colDetailArrangement,
            this.colDetailTuning,
            this.colDetailPitch,
            this.colDetailToneBase,
            this.colDetailDDMax});
            this.dgvSongsDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSongsDetail.HorizontalScrollBarVisible = true;
            this.dgvSongsDetail.Location = new System.Drawing.Point(27, 78);
            this.dgvSongsDetail.Name = "dgvSongsDetail";
            this.dgvSongsDetail.ReadOnly = true;
            this.dgvSongsDetail.RowHeadersVisible = false;
            this.dgvSongsDetail.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvSongsDetail.Size = new System.Drawing.Size(842, 70);
            this.dgvSongsDetail.TabIndex = 5;
            this.dgvSongsDetail.VerticalScrollBarVisible = false;
            // 
            // colDetailKey
            // 
            this.colDetailKey.DataPropertyName = "DLCKey";
            this.colDetailKey.Frozen = true;
            this.colDetailKey.HeaderText = "DLCKey";
            this.colDetailKey.Name = "colDetailKey";
            this.colDetailKey.ReadOnly = true;
            this.colDetailKey.Width = 95;
            // 
            // colDetailPID
            // 
            this.colDetailPID.DataPropertyName = "PersistentID";
            this.colDetailPID.Frozen = true;
            this.colDetailPID.HeaderText = "PersistentID";
            this.colDetailPID.Name = "colDetailPID";
            this.colDetailPID.ReadOnly = true;
            this.colDetailPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colDetailArrangement
            // 
            this.colDetailArrangement.DataPropertyName = "Name";
            this.colDetailArrangement.Frozen = true;
            this.colDetailArrangement.HeaderText = "Arrangement";
            this.colDetailArrangement.Name = "colDetailArrangement";
            this.colDetailArrangement.ReadOnly = true;
            this.colDetailArrangement.Width = 120;
            // 
            // colDetailTuning
            // 
            this.colDetailTuning.DataPropertyName = "Tuning";
            this.colDetailTuning.HeaderText = "Tuning";
            this.colDetailTuning.Name = "colDetailTuning";
            this.colDetailTuning.ReadOnly = true;
            // 
            // colDetailPitch
            // 
            this.colDetailPitch.DataPropertyName = "TuningPitch";
            this.colDetailPitch.HeaderText = "TuningPitch";
            this.colDetailPitch.Name = "colDetailPitch";
            this.colDetailPitch.ReadOnly = true;
            // 
            // colDetailToneBase
            // 
            this.colDetailToneBase.DataPropertyName = "ToneBase";
            this.colDetailToneBase.HeaderText = "ToneBase";
            this.colDetailToneBase.Name = "colDetailToneBase";
            this.colDetailToneBase.ReadOnly = true;
            this.colDetailToneBase.Width = 120;
            // 
            // colDetailDDMax
            // 
            this.colDetailDDMax.DataPropertyName = "DDMax";
            this.colDetailDDMax.HeaderText = "DDMax";
            this.colDetailDDMax.Name = "colDetailDDMax";
            this.colDetailDDMax.ReadOnly = true;
            this.colDetailDDMax.Width = 80;
            // 
            // dgvSongsMaster
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongsMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSongsMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongsMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSongsMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colShowDetail,
            this.colKey,
            this.colSelect,
            this.colBass,
            this.colLead,
            this.colRhythm,
            this.colVocals,
            this.colRepairStatus,
            this.colEnabled,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colSongYear,
            this.colSongLength,
            this.colSongAverageTempo,
            this.colSongVolume,
            this.colLastConversionDateTime,
            this.colDD,
            this.colArrangements,
            this.colTunings,
            this.colTones,
            this.colFileName,
            this.colFilePath,
            this.colFileDate,
            this.colFileSize,
            this.colStatus,
            this.colAppID,
            this.colToolkitVersion,
            this.colPackageAuthor,
            this.colPackageVersion,
            this.colPackageComment,
            this.colPackageRating,
            this.colTagged,
            this.colIgnitionID,
            this.colIgnitionDate,
            this.colIgnitionVersion,
            this.colIgnitionAuthor,
            this.colArtistTitleAlbum,
            this.colArtistTitleAlbumDate,
            this.colArtistSort,
            this.colTitleSort,
            this.colAlbumSort});
            this.dgvSongsMaster.Location = new System.Drawing.Point(6, 19);
            this.dgvSongsMaster.Name = "dgvSongsMaster";
            this.dgvSongsMaster.RowHeadersVisible = false;
            this.dgvSongsMaster.Size = new System.Drawing.Size(879, 348);
            this.dgvSongsMaster.TabIndex = 1;
            this.dgvSongsMaster.Tag = "Song Manager";
            this.toolTip.SetToolTip(this.dgvSongsMaster, "Left mouse click the \'Select\' checkbox to select a row\r\nRight mouse click on row " +
                    "to show file operation options");
            this.dgvSongsMaster.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongsMaster_CellContentClick);
            this.dgvSongsMaster.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongsMaster_CellDoubleClick);
            this.dgvSongsMaster.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSongsMaster_CellFormatting);
            this.dgvSongsMaster.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_CellMouseDown);
            this.dgvSongsMaster.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_CellMouseUp);
            this.dgvSongsMaster.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSongsMaster_CellValueChanged);
            this.dgvSongsMaster.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongsMaster_CellMouseUp);
            this.dgvSongsMaster.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSongsMaster_DataBindingComplete);
            this.dgvSongsMaster.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvSongsMaster_Scroll);
            this.dgvSongsMaster.Sorted += new System.EventHandler(this.dgvSongsMaster_Sorted);
            this.dgvSongsMaster.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSongsMaster_Paint);
            this.dgvSongsMaster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSongsMaster_KeyDown);
            this.dgvSongsMaster.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvSongsMaster_KeyUp);
            // 
            // colShowDetail
            // 
            this.colShowDetail.DataPropertyName = "ShowDetail";
            this.colShowDetail.Frozen = true;
            this.colShowDetail.HeaderText = "";
            this.colShowDetail.Image = ((System.Drawing.Image)(resources.GetObject("colShowDetail.Image")));
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
            this.colSelect.ToolTipText = "Left mouse click the \'Select\' checkbox to select a row\r\nRight mouse click on row " +
                "to show file operation options";
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 50;
            // 
            // colBass
            // 
            this.colBass.DataPropertyName = "Bass";
            this.colBass.HeaderText = "B";
            this.colBass.Image = ((System.Drawing.Image)(resources.GetObject("colBass.Image")));
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
            this.colLead.Image = ((System.Drawing.Image)(resources.GetObject("colLead.Image")));
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
            this.colRhythm.Image = ((System.Drawing.Image)(resources.GetObject("colRhythm.Image")));
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
            this.colVocals.Image = ((System.Drawing.Image)(resources.GetObject("colVocals.Image")));
            this.colVocals.MinimumWidth = 21;
            this.colVocals.Name = "colVocals";
            this.colVocals.ReadOnly = true;
            this.colVocals.ToolTipText = "Vocals";
            this.colVocals.Visible = false;
            this.colVocals.Width = 21;
            // 
            // colRepairStatus
            // 
            this.colRepairStatus.DataPropertyName = "RepairStatus";
            this.colRepairStatus.HeaderText = "RepairStatus";
            this.colRepairStatus.Name = "colRepairStatus";
            this.colRepairStatus.ReadOnly = true;
            this.colRepairStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            this.colTitle.HeaderText = "Title";
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
            this.colAlbum.Visible = false;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = null;
            this.colSongYear.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSongYear.HeaderText = "Year";
            this.colSongYear.Name = "colSongYear";
            this.colSongYear.ReadOnly = true;
            this.colSongYear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongYear.Visible = false;
            this.colSongYear.Width = 50;
            // 
            // colSongLength
            // 
            this.colSongLength.DataPropertyName = "SongLength";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colSongLength.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSongLength.HeaderText = "SongLength (secs)";
            this.colSongLength.Name = "colSongLength";
            this.colSongLength.ReadOnly = true;
            this.colSongLength.Visible = false;
            // 
            // colSongAverageTempo
            // 
            this.colSongAverageTempo.DataPropertyName = "SongAverageTempo";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.colSongAverageTempo.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSongAverageTempo.HeaderText = "BPM";
            this.colSongAverageTempo.Name = "colSongAverageTempo";
            this.colSongAverageTempo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongAverageTempo.Visible = false;
            // 
            // colSongVolume
            // 
            this.colSongVolume.DataPropertyName = "SongVolume";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.colSongVolume.DefaultCellStyle = dataGridViewCellStyle6;
            this.colSongVolume.HeaderText = "SongVolume (LF)";
            this.colSongVolume.Name = "colSongVolume";
            this.colSongVolume.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongVolume.ToolTipText = "(Loudness Factor)";
            this.colSongVolume.Visible = false;
            // 
            // colLastConversionDateTime
            // 
            this.colLastConversionDateTime.DataPropertyName = "LastConversionDateTime";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.NullValue = null;
            this.colLastConversionDateTime.DefaultCellStyle = dataGridViewCellStyle7;
            this.colLastConversionDateTime.HeaderText = "ConversionDate";
            this.colLastConversionDateTime.Name = "colLastConversionDateTime";
            this.colLastConversionDateTime.ReadOnly = true;
            this.colLastConversionDateTime.Visible = false;
            this.colLastConversionDateTime.Width = 50;
            // 
            // colDD
            // 
            this.colDD.DataPropertyName = "DD";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.NullValue = null;
            this.colDD.DefaultCellStyle = dataGridViewCellStyle8;
            this.colDD.HeaderText = "DDMax";
            this.colDD.Name = "colDD";
            this.colDD.ReadOnly = true;
            this.colDD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colArrangements
            // 
            this.colArrangements.DataPropertyName = "Arrangements1D";
            this.colArrangements.HeaderText = "Arrangements";
            this.colArrangements.Name = "colArrangements";
            this.colArrangements.ReadOnly = true;
            this.colArrangements.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArrangements.Width = 50;
            // 
            // colTunings
            // 
            this.colTunings.DataPropertyName = "Tunings1D";
            this.colTunings.HeaderText = "Tunings";
            this.colTunings.Name = "colTunings";
            this.colTunings.ReadOnly = true;
            this.colTunings.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTones
            // 
            this.colTones.DataPropertyName = "Tones1D";
            this.colTones.HeaderText = "Tones";
            this.colTones.Name = "colTones";
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileName.Visible = false;
            this.colFileName.Width = 50;
            // 
            // colFilePath
            // 
            this.colFilePath.DataPropertyName = "FilePath";
            this.colFilePath.HeaderText = "FilePath";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilePath.Width = 150;
            // 
            // colFileDate
            // 
            this.colFileDate.DataPropertyName = "FileDate";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.NullValue = null;
            this.colFileDate.DefaultCellStyle = dataGridViewCellStyle9;
            this.colFileDate.HeaderText = "FileDate";
            this.colFileDate.Name = "colFileDate";
            this.colFileDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileDate.Visible = false;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "FileSize";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.NullValue = null;
            this.colFileSize.DefaultCellStyle = dataGridViewCellStyle10;
            this.colFileSize.HeaderText = "FileSize (bytes)";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.Visible = false;
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
            // colAppID
            // 
            this.colAppID.DataPropertyName = "AppID";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            this.colAppID.DefaultCellStyle = dataGridViewCellStyle11;
            this.colAppID.HeaderText = "AppID";
            this.colAppID.Name = "colAppID";
            this.colAppID.ReadOnly = true;
            this.colAppID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAppID.Width = 80;
            // 
            // colToolkitVersion
            // 
            this.colToolkitVersion.DataPropertyName = "ToolkitVersion";
            this.colToolkitVersion.HeaderText = "ToolkitVersion";
            this.colToolkitVersion.Name = "colToolkitVersion";
            this.colToolkitVersion.ReadOnly = true;
            this.colToolkitVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToolkitVersion.Width = 110;
            // 
            // colPackageAuthor
            // 
            this.colPackageAuthor.DataPropertyName = "PackageAuthor";
            this.colPackageAuthor.HeaderText = "PackageAuthor";
            this.colPackageAuthor.Name = "colPackageAuthor";
            this.colPackageAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageAuthor.Visible = false;
            // 
            // colPackageVersion
            // 
            this.colPackageVersion.DataPropertyName = "PackageVersion";
            this.colPackageVersion.HeaderText = "PackageVersion";
            this.colPackageVersion.Name = "colPackageVersion";
            this.colPackageVersion.ReadOnly = true;
            this.colPackageVersion.Visible = false;
            this.colPackageVersion.Width = 50;
            // 
            // colPackageComment
            // 
            this.colPackageComment.DataPropertyName = "PackageComment";
            this.colPackageComment.HeaderText = "PackageComment";
            this.colPackageComment.Name = "colPackageComment";
            this.colPackageComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageComment.Visible = false;
            // 
            // colPackageRating
            // 
            this.colPackageRating.DataPropertyName = "RatingStars";
            this.colPackageRating.GrayStarColor = System.Drawing.Color.LightGray;
            this.colPackageRating.HeaderText = "PackageRating";
            this.colPackageRating.Name = "colPackageRating";
            this.colPackageRating.RatedStarColor = System.Drawing.Color.Green;
            this.colPackageRating.StarScale = 0.9F;
            this.colPackageRating.ToolTipText = "To remove a rating completely ...\r\nFirst click on the first star, then\r\nclick on " +
                "the first star again, next\r\ncompletely move the mouse off.";
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
            this.colIgnitionID.HeaderText = "IgnitionID";
            this.colIgnitionID.Name = "colIgnitionID";
            this.colIgnitionID.ReadOnly = true;
            this.colIgnitionID.Visible = false;
            this.colIgnitionID.Width = 50;
            // 
            // colIgnitionDate
            // 
            this.colIgnitionDate.DataPropertyName = "IgnitionDate";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            dataGridViewCellStyle12.NullValue = null;
            this.colIgnitionDate.DefaultCellStyle = dataGridViewCellStyle12;
            this.colIgnitionDate.HeaderText = "IgnitionDate";
            this.colIgnitionDate.Name = "colIgnitionDate";
            this.colIgnitionDate.ReadOnly = true;
            this.colIgnitionDate.Visible = false;
            this.colIgnitionDate.Width = 50;
            // 
            // colIgnitionVersion
            // 
            this.colIgnitionVersion.DataPropertyName = "IgnitionVersion";
            this.colIgnitionVersion.HeaderText = "IgnitionVersion";
            this.colIgnitionVersion.Name = "colIgnitionVersion";
            this.colIgnitionVersion.ReadOnly = true;
            this.colIgnitionVersion.Visible = false;
            this.colIgnitionVersion.Width = 50;
            // 
            // colIgnitionAuthor
            // 
            this.colIgnitionAuthor.DataPropertyName = "IgnitionAuthor";
            this.colIgnitionAuthor.HeaderText = "IgnitionAuthor";
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
            // colArtistTitleAlbumDate
            // 
            this.colArtistTitleAlbumDate.DataPropertyName = "ArtistTitleAlbumDate";
            this.colArtistTitleAlbumDate.HeaderText = "ArtistTitleAlbumDate";
            this.colArtistTitleAlbumDate.Name = "colArtistTitleAlbumDate";
            this.colArtistTitleAlbumDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colArtistSort
            // 
            this.colArtistSort.DataPropertyName = "ArtistSort";
            this.colArtistSort.HeaderText = "ArtistSort";
            this.colArtistSort.Name = "colArtistSort";
            this.colArtistSort.ReadOnly = true;
            this.colArtistSort.Visible = false;
            this.colArtistSort.Width = 50;
            // 
            // colTitleSort
            // 
            this.colTitleSort.DataPropertyName = "TitleSort";
            this.colTitleSort.HeaderText = "TitleSort";
            this.colTitleSort.Name = "colTitleSort";
            this.colTitleSort.ReadOnly = true;
            this.colTitleSort.Visible = false;
            this.colTitleSort.Width = 50;
            // 
            // colAlbumSort
            // 
            this.colAlbumSort.DataPropertyName = "AlbumSort";
            this.colAlbumSort.HeaderText = "AlbumSort";
            this.colAlbumSort.Name = "colAlbumSort";
            this.colAlbumSort.ReadOnly = true;
            this.colAlbumSort.Visible = false;
            this.colAlbumSort.Width = 50;
            // 
            // lnklblToggle
            // 
            this.lnklblToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnklblToggle.AutoSize = true;
            this.lnklblToggle.ForeColor = System.Drawing.Color.Black;
            this.lnklblToggle.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnklblToggle.Location = new System.Drawing.Point(109, 370);
            this.lnklblToggle.Name = "lnklblToggle";
            this.lnklblToggle.Size = new System.Drawing.Size(87, 13);
            this.lnklblToggle.TabIndex = 4;
            this.lnklblToggle.TabStop = true;
            this.lnklblToggle.Text = "Toggle Selection";
            this.lnklblToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnklblToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToggle_LinkClicked);
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
            this.testToolStripMenuItem.AssociatedEnumValue = null;
            this.testToolStripMenuItem.Checked = true;
            this.testToolStripMenuItem.CheckOnClick = true;
            this.testToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.testToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.RadioButtonGroupName = null;
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
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRescan,
            this.tsmiRepairs,
            this.tsmiMods,
            this.tsmiFiles,
            this.tsmiHelp,
            this.tsmiDevDebugUse});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(899, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmiRescan
            // 
            this.tsmiRescan.AssociatedEnumValue = null;
            this.tsmiRescan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRescanQuick,
            this.tsmiRescanFull});
            this.tsmiRescan.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescan.Image")));
            this.tsmiRescan.Name = "tsmiRescan";
            this.tsmiRescan.RadioButtonGroupName = null;
            this.tsmiRescan.Size = new System.Drawing.Size(70, 20);
            this.tsmiRescan.Text = "Rescan";
            this.tsmiRescan.ToolTipText = "See \'Settings\' menu for additional\r\ndata rescan and inclusion options.";
            // 
            // tsmiRescanQuick
            // 
            this.tsmiRescanQuick.AssociatedEnumValue = null;
            this.tsmiRescanQuick.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescanQuick.Image")));
            this.tsmiRescanQuick.Name = "tsmiRescanQuick";
            this.tsmiRescanQuick.RadioButtonGroupName = null;
            this.tsmiRescanQuick.Size = new System.Drawing.Size(111, 22);
            this.tsmiRescanQuick.Text = "Quick";
            this.tsmiRescanQuick.ToolTipText = "Quick reload of previously scanned data.\r\nOnly rescans data if necessary.";
            this.tsmiRescanQuick.Click += new System.EventHandler(this.tsmiRescanQuick_Click);
            // 
            // tsmiRescanFull
            // 
            this.tsmiRescanFull.AssociatedEnumValue = null;
            this.tsmiRescanFull.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescanFull.Image")));
            this.tsmiRescanFull.Name = "tsmiRescanFull";
            this.tsmiRescanFull.RadioButtonGroupName = null;
            this.tsmiRescanFull.Size = new System.Drawing.Size(111, 22);
            this.tsmiRescanFull.Text = "Full";
            this.tsmiRescanFull.ToolTipText = "Preliminary scan or after significant changes.\r\n\r\nHint:\r\nTry running a \'Full\' res" +
                "can if the CDLC\r\ncollection does not look as expected.";
            this.tsmiRescanFull.Click += new System.EventHandler(this.tsmiRescanFull_Click);
            // 
            // tsmiRepairs
            // 
            this.tsmiRepairs.AssociatedEnumValue = null;
            this.tsmiRepairs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSkipRemastered,
            this.toolStripSeparator16,
            this.tsmiRepairsAddDD,
            this.tsmiDDSettings,
            this.tsmiOverwriteDD,
            this.toolStripSeparator9,
            this.tsmiRepairsMastery,
            this.tsmiRepairsPreserveStats,
            this.tsmiRepairsUsingOrg,
            this.tsmiRepairsMultitone,
            this.toolStripSeparator8,
            this.tsmiRepairsMaxFive,
            this.tsmiRemoveNDD,
            this.tsmiRemoveBass,
            this.tsmiRemoveGuitar,
            this.tsmiRemoveBonus,
            this.tsmiRemoveMetronome,
            this.tsmiIgnoreStopLimit,
            this.toolStripSeparator7,
            this.tsmiAdjustScrollSpeed,
            this.tsmiSSSettings,
            this.toolStripSeparator12,
            this.tsmiRemoveSections,
            this.tsmiFixLowBass,
            this.toolStripSeparator10,
            this.tsmiDLFolderProcess,
            this.tsmiDLFolderMonitor,
            this.toolStripSeparator13,
            this.tsmiRepairsRun});
            this.tsmiRepairs.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRepairs.Image")));
            this.tsmiRepairs.Name = "tsmiRepairs";
            this.tsmiRepairs.RadioButtonGroupName = null;
            this.tsmiRepairs.Size = new System.Drawing.Size(71, 20);
            this.tsmiRepairs.Text = "Repairs";
            this.tsmiRepairs.ToolTipText = "This tool repairs CDLC files using\nvarious user selectable options.\n\nHINT: First " +
                "use \'Select All/None\' and/or \n\'Toggle Selection\' links to quckly choose\nthe CDLC" +
                " files to repair (aka Bulk Repair).";
            // 
            // tsmiSkipRemastered
            // 
            this.tsmiSkipRemastered.AssociatedEnumValue = null;
            this.tsmiSkipRemastered.AutoCheck = false;
            this.tsmiSkipRemastered.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiSkipRemastered.CheckOnClick = true;
            this.tsmiSkipRemastered.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiSkipRemastered.Name = "tsmiSkipRemastered";
            this.tsmiSkipRemastered.RadioButtonGroupName = "SkipRemastered";
            this.tsmiSkipRemastered.Size = new System.Drawing.Size(246, 22);
            this.tsmiSkipRemastered.Text = "Skip Previously Remastered CDLC";
            this.tsmiSkipRemastered.ToolTipText = "Skipping previously remastered CDLC\r\nwill greatly speed up the repair\r\nprocess.";
            this.tsmiSkipRemastered.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiRepairsAddDD
            // 
            this.tsmiRepairsAddDD.AssociatedEnumValue = null;
            this.tsmiRepairsAddDD.AutoCheck = false;
            this.tsmiRepairsAddDD.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiRepairsAddDD.CheckOnClick = true;
            this.tsmiRepairsAddDD.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsAddDD.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiRepairsAddDD.Name = "tsmiRepairsAddDD";
            this.tsmiRepairsAddDD.RadioButtonGroupName = "AddDD";
            this.tsmiRepairsAddDD.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsAddDD.Text = "Add Dynamic Difficulty (DD)";
            this.tsmiRepairsAddDD.ToolTipText = "Fixes Remastered Play Count tracking\r\nby adding Dynamic Difficulty (DD) to\r\narran" +
                "gements that do not have DD";
            this.tsmiRepairsAddDD.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiDDSettings
            // 
            this.tsmiDDSettings.AssociatedEnumValue = null;
            this.tsmiDDSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddDDNumericUpDown,
            this.tsmiAddDDRemoveSustain,
            this.tsmiAddDDCfgPath,
            this.tsmiAddDDRampUpPath});
            this.tsmiDDSettings.Name = "tsmiDDSettings";
            this.tsmiDDSettings.RadioButtonGroupName = "AddDD";
            this.tsmiDDSettings.Size = new System.Drawing.Size(246, 22);
            this.tsmiDDSettings.Text = "Customizable DD Settings";
            this.tsmiDDSettings.ToolTipText = "Configuration settings for ddc.exe CLI";
            // 
            // tsmiAddDDNumericUpDown
            // 
            this.tsmiAddDDNumericUpDown.BackColor = System.Drawing.Color.Transparent;
            this.tsmiAddDDNumericUpDown.DecimalValue = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.tsmiAddDDNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.tsmiAddDDNumericUpDown.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.tsmiAddDDNumericUpDown.Name = "tsmiAddDDNumericUpDown";
            this.tsmiAddDDNumericUpDown.NumBackColor = System.Drawing.SystemColors.Window;
            this.tsmiAddDDNumericUpDown.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tsmiAddDDNumericUpDown.Size = new System.Drawing.Size(53, 22);
            this.tsmiAddDDNumericUpDown.Text = "Phrase Length";
            this.tsmiAddDDNumericUpDown.TextVisible = false;
            this.tsmiAddDDNumericUpDown.ToolTipText = "Set custom Phrase Length greater than eight";
            this.tsmiAddDDNumericUpDown.Click += new System.EventHandler(this.RepairsAddDDSettings_Click);
            // 
            // tsmiAddDDRemoveSustain
            // 
            this.tsmiAddDDRemoveSustain.AssociatedEnumValue = null;
            this.tsmiAddDDRemoveSustain.CheckOnClick = true;
            this.tsmiAddDDRemoveSustain.Name = "tsmiAddDDRemoveSustain";
            this.tsmiAddDDRemoveSustain.RadioButtonGroupName = "AddDD";
            this.tsmiAddDDRemoveSustain.Size = new System.Drawing.Size(260, 22);
            this.tsmiAddDDRemoveSustain.Text = "Remove Sustain";
            this.tsmiAddDDRemoveSustain.ToolTipText = "If checked, sustain will be removed.";
            this.tsmiAddDDRemoveSustain.Click += new System.EventHandler(this.RepairsAddDDSettings_Click);
            // 
            // tsmiAddDDCfgPath
            // 
            this.tsmiAddDDCfgPath.AssociatedEnumValue = null;
            this.tsmiAddDDCfgPath.AutoSize = false;
            this.tsmiAddDDCfgPath.Name = "tsmiAddDDCfgPath";
            this.tsmiAddDDCfgPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiAddDDCfgPath.RadioButtonGroupName = null;
            this.tsmiAddDDCfgPath.Size = new System.Drawing.Size(170, 22);
            this.tsmiAddDDCfgPath.Spring = true;
            this.tsmiAddDDCfgPath.Text = "Click to set CFG path";
            this.tsmiAddDDCfgPath.ToolTipText = "Set custom CFG path";
            this.tsmiAddDDCfgPath.Click += new System.EventHandler(this.tsmiAddDDCfgPath_Click);
            // 
            // tsmiAddDDRampUpPath
            // 
            this.tsmiAddDDRampUpPath.AssociatedEnumValue = null;
            this.tsmiAddDDRampUpPath.AutoSize = false;
            this.tsmiAddDDRampUpPath.Name = "tsmiAddDDRampUpPath";
            this.tsmiAddDDRampUpPath.Overflow = System.Windows.Forms.ToolStripItemOverflow.AsNeeded;
            this.tsmiAddDDRampUpPath.RadioButtonGroupName = null;
            this.tsmiAddDDRampUpPath.Size = new System.Drawing.Size(170, 22);
            this.tsmiAddDDRampUpPath.Spring = true;
            this.tsmiAddDDRampUpPath.Text = "Click to set RampUp path";
            this.tsmiAddDDRampUpPath.ToolTipText = "Set custom RampUp path";
            this.tsmiAddDDRampUpPath.Click += new System.EventHandler(this.tsmiAddDDRampUpPath_Click);
            // 
            // tsmiOverwriteDD
            // 
            this.tsmiOverwriteDD.AssociatedEnumValue = null;
            this.tsmiOverwriteDD.CheckOnClick = true;
            this.tsmiOverwriteDD.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiOverwriteDD.ForeColor = System.Drawing.Color.Red;
            this.tsmiOverwriteDD.Name = "tsmiOverwriteDD";
            this.tsmiOverwriteDD.RadioButtonGroupName = "AddDD";
            this.tsmiOverwriteDD.Size = new System.Drawing.Size(246, 22);
            this.tsmiOverwriteDD.Text = "Overwrite Existing DD";
            this.tsmiOverwriteDD.ToolTipText = "Overwrite existing Dynamic Difficulty (DD)\r\nwith new DD even if arrangement alrea" +
                "dy\r\nhas DD.";
            this.tsmiOverwriteDD.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiRepairsMastery
            // 
            this.tsmiRepairsMastery.AssociatedEnumValue = null;
            this.tsmiRepairsMastery.AutoCheck = false;
            this.tsmiRepairsMastery.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiRepairsMastery.CheckOnClick = true;
            this.tsmiRepairsMastery.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsMastery.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiRepairsMastery.Name = "tsmiRepairsMastery";
            this.tsmiRepairsMastery.RadioButtonGroupName = "Mastery";
            this.tsmiRepairsMastery.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsMastery.Text = "Mastery 100% Bug";
            this.tsmiRepairsMastery.ToolTipText = resources.GetString("tsmiRepairsMastery.ToolTipText");
            this.tsmiRepairsMastery.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiRepairsPreserveStats
            // 
            this.tsmiRepairsPreserveStats.AssociatedEnumValue = null;
            this.tsmiRepairsPreserveStats.CheckOnClick = true;
            this.tsmiRepairsPreserveStats.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsPreserveStats.Name = "tsmiRepairsPreserveStats";
            this.tsmiRepairsPreserveStats.RadioButtonGroupName = "Mastery";
            this.tsmiRepairsPreserveStats.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsPreserveStats.Text = "Preserve Stats";
            this.tsmiRepairsPreserveStats.ToolTipText = resources.GetString("tsmiRepairsPreserveStats.ToolTipText");
            this.tsmiRepairsPreserveStats.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            this.tsmiRepairsPreserveStats.Click += new System.EventHandler(this.tsmiRepairsPreserveStats_Click);
            // 
            // tsmiRepairsUsingOrg
            // 
            this.tsmiRepairsUsingOrg.AssociatedEnumValue = null;
            this.tsmiRepairsUsingOrg.CheckOnClick = true;
            this.tsmiRepairsUsingOrg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsUsingOrg.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tsmiRepairsUsingOrg.Name = "tsmiRepairsUsingOrg";
            this.tsmiRepairsUsingOrg.RadioButtonGroupName = "Mastery";
            this.tsmiRepairsUsingOrg.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsUsingOrg.Text = "Use (.org) Files";
            this.tsmiRepairsUsingOrg.ToolTipText = resources.GetString("tsmiRepairsUsingOrg.ToolTipText");
            this.tsmiRepairsUsingOrg.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiRepairsMultitone
            // 
            this.tsmiRepairsMultitone.AssociatedEnumValue = null;
            this.tsmiRepairsMultitone.CheckOnClick = true;
            this.tsmiRepairsMultitone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsMultitone.ForeColor = System.Drawing.Color.Red;
            this.tsmiRepairsMultitone.Name = "tsmiRepairsMultitone";
            this.tsmiRepairsMultitone.RadioButtonGroupName = "Mastery";
            this.tsmiRepairsMultitone.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsMultitone.Text = "Prevent Multitone Exceptions";
            this.tsmiRepairsMultitone.ToolTipText = "Prevent multitone exceptions\r\nby converting corrupt multitone\r\narrangements to si" +
                "ngle tone.";
            this.tsmiRepairsMultitone.Click += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiRepairsMaxFive
            // 
            this.tsmiRepairsMaxFive.AssociatedEnumValue = null;
            this.tsmiRepairsMaxFive.AutoCheck = false;
            this.tsmiRepairsMaxFive.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiRepairsMaxFive.CheckOnClick = true;
            this.tsmiRepairsMaxFive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsMaxFive.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiRepairsMaxFive.Name = "tsmiRepairsMaxFive";
            this.tsmiRepairsMaxFive.RadioButtonGroupName = "MaxFive";
            this.tsmiRepairsMaxFive.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsMaxFive.Text = "Max Five Arrangements";
            this.tsmiRepairsMaxFive.ToolTipText = "Select Max5 Repair Options";
            this.tsmiRepairsMaxFive.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiRemoveNDD
            // 
            this.tsmiRemoveNDD.AssociatedEnumValue = null;
            this.tsmiRemoveNDD.CheckOnClick = true;
            this.tsmiRemoveNDD.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRemoveNDD.Name = "tsmiRemoveNDD";
            this.tsmiRemoveNDD.RadioButtonGroupName = "MaxFive";
            this.tsmiRemoveNDD.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveNDD.Text = "Remove NDD";
            this.tsmiRemoveNDD.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiRemoveBass
            // 
            this.tsmiRemoveBass.AssociatedEnumValue = null;
            this.tsmiRemoveBass.CheckOnClick = true;
            this.tsmiRemoveBass.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRemoveBass.Name = "tsmiRemoveBass";
            this.tsmiRemoveBass.RadioButtonGroupName = "MaxFive";
            this.tsmiRemoveBass.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveBass.Text = "Remove Bass";
            this.tsmiRemoveBass.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiRemoveGuitar
            // 
            this.tsmiRemoveGuitar.AssociatedEnumValue = null;
            this.tsmiRemoveGuitar.CheckOnClick = true;
            this.tsmiRemoveGuitar.Name = "tsmiRemoveGuitar";
            this.tsmiRemoveGuitar.RadioButtonGroupName = "MaxFive";
            this.tsmiRemoveGuitar.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveGuitar.Text = "Remove Guitar";
            this.tsmiRemoveGuitar.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiRemoveBonus
            // 
            this.tsmiRemoveBonus.AssociatedEnumValue = null;
            this.tsmiRemoveBonus.CheckOnClick = true;
            this.tsmiRemoveBonus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRemoveBonus.Name = "tsmiRemoveBonus";
            this.tsmiRemoveBonus.RadioButtonGroupName = "MaxFive";
            this.tsmiRemoveBonus.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveBonus.Text = "Remove Bonus";
            this.tsmiRemoveBonus.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiRemoveMetronome
            // 
            this.tsmiRemoveMetronome.AssociatedEnumValue = null;
            this.tsmiRemoveMetronome.CheckOnClick = true;
            this.tsmiRemoveMetronome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRemoveMetronome.Name = "tsmiRemoveMetronome";
            this.tsmiRemoveMetronome.RadioButtonGroupName = "MaxFive";
            this.tsmiRemoveMetronome.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveMetronome.Text = "Remove Metronome";
            this.tsmiRemoveMetronome.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // tsmiIgnoreStopLimit
            // 
            this.tsmiIgnoreStopLimit.AssociatedEnumValue = null;
            this.tsmiIgnoreStopLimit.CheckOnClick = true;
            this.tsmiIgnoreStopLimit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiIgnoreStopLimit.ForeColor = System.Drawing.Color.Red;
            this.tsmiIgnoreStopLimit.Name = "tsmiIgnoreStopLimit";
            this.tsmiIgnoreStopLimit.RadioButtonGroupName = "MaxFive";
            this.tsmiIgnoreStopLimit.Size = new System.Drawing.Size(246, 22);
            this.tsmiIgnoreStopLimit.Text = "Ignore Arrangement Stop Limit";
            this.tsmiIgnoreStopLimit.ToolTipText = "Removes all arrangements of\r\nselected type and ignores\r\nstop limit of 5 arrangeme" +
                "nts.";
            this.tsmiIgnoreStopLimit.CheckStateChanged += new System.EventHandler(this.Repairs_CheckStateChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiAdjustScrollSpeed
            // 
            this.tsmiAdjustScrollSpeed.AssociatedEnumValue = null;
            this.tsmiAdjustScrollSpeed.AutoCheck = false;
            this.tsmiAdjustScrollSpeed.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiAdjustScrollSpeed.CheckOnClick = true;
            this.tsmiAdjustScrollSpeed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiAdjustScrollSpeed.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiAdjustScrollSpeed.Name = "tsmiAdjustScrollSpeed";
            this.tsmiAdjustScrollSpeed.RadioButtonGroupName = "ScrollSpeed";
            this.tsmiAdjustScrollSpeed.Size = new System.Drawing.Size(246, 22);
            this.tsmiAdjustScrollSpeed.Text = "Adjust Scroll Speed (SS)";
            this.tsmiAdjustScrollSpeed.ToolTipText = "Use Arrangement Analyzer\r\nto view Scroll Speed values \r\nbefore and after repair.";
            this.tsmiAdjustScrollSpeed.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiSSSettings
            // 
            this.tsmiSSSettings.AssociatedEnumValue = null;
            this.tsmiSSSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNudScrollSpeed});
            this.tsmiSSSettings.Name = "tsmiSSSettings";
            this.tsmiSSSettings.RadioButtonGroupName = "ScrollSpeed";
            this.tsmiSSSettings.Size = new System.Drawing.Size(246, 22);
            this.tsmiSSSettings.Text = "Customizable SS Setting";
            this.tsmiSSSettings.ToolTipText = "Minimum 0.5, Default 1.3, Maximum 4.5";
            // 
            // tsmiNudScrollSpeed
            // 
            this.tsmiNudScrollSpeed.BackColor = System.Drawing.Color.Transparent;
            this.tsmiNudScrollSpeed.DecimalPlaces = 1;
            this.tsmiNudScrollSpeed.DecimalValue = new decimal(new int[] {
            13,
            0,
            0,
            65536});
            this.tsmiNudScrollSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.tsmiNudScrollSpeed.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            65536});
            this.tsmiNudScrollSpeed.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.tsmiNudScrollSpeed.Name = "tsmiNudScrollSpeed";
            this.tsmiNudScrollSpeed.NumBackColor = System.Drawing.SystemColors.Window;
            this.tsmiNudScrollSpeed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tsmiNudScrollSpeed.Size = new System.Drawing.Size(53, 22);
            this.tsmiNudScrollSpeed.Text = "(Default 1.3)";
            this.tsmiNudScrollSpeed.TextVisible = false;
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiRemoveSections
            // 
            this.tsmiRemoveSections.AssociatedEnumValue = null;
            this.tsmiRemoveSections.AutoCheck = false;
            this.tsmiRemoveSections.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiRemoveSections.CheckOnClick = true;
            this.tsmiRemoveSections.Enabled = false;
            this.tsmiRemoveSections.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiRemoveSections.Name = "tsmiRemoveSections";
            this.tsmiRemoveSections.RadioButtonGroupName = "RemoveSections";
            this.tsmiRemoveSections.Size = new System.Drawing.Size(246, 22);
            this.tsmiRemoveSections.Text = "Remove Sections/Phrases";
            this.tsmiRemoveSections.ToolTipText = "Coming Soon";
            this.tsmiRemoveSections.Visible = false;
            this.tsmiRemoveSections.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiFixLowBass
            // 
            this.tsmiFixLowBass.AssociatedEnumValue = null;
            this.tsmiFixLowBass.AutoCheck = false;
            this.tsmiFixLowBass.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiFixLowBass.CheckOnClick = true;
            this.tsmiFixLowBass.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiFixLowBass.Name = "tsmiFixLowBass";
            this.tsmiFixLowBass.RadioButtonGroupName = "LowBassFix";
            this.tsmiFixLowBass.Size = new System.Drawing.Size(246, 22);
            this.tsmiFixLowBass.Text = "Fix Low Bass Tuning";
            this.tsmiFixLowBass.ToolTipText = "If selected, apply the low bass\r\ntuning fix only if an arragement\r\nneeds to be fi" +
                "xed.";
            this.tsmiFixLowBass.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiDLFolderProcess
            // 
            this.tsmiDLFolderProcess.AssociatedEnumValue = null;
            this.tsmiDLFolderProcess.AutoCheck = false;
            this.tsmiDLFolderProcess.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiDLFolderProcess.CheckOnClick = true;
            this.tsmiDLFolderProcess.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tsmiDLFolderProcess.Name = "tsmiDLFolderProcess";
            this.tsmiDLFolderProcess.RadioButtonGroupName = "ProcessDLFolder";
            this.tsmiDLFolderProcess.Size = new System.Drawing.Size(246, 22);
            this.tsmiDLFolderProcess.Text = "Process Downloads Folder";
            this.tsmiDLFolderProcess.ToolTipText = resources.GetString("tsmiDLFolderProcess.ToolTipText");
            this.tsmiDLFolderProcess.Click += new System.EventHandler(this.RepairsButton_Click);
            // 
            // tsmiDLFolderMonitor
            // 
            this.tsmiDLFolderMonitor.AssociatedEnumValue = null;
            this.tsmiDLFolderMonitor.AutoCheck = false;
            this.tsmiDLFolderMonitor.CheckOnClick = true;
            this.tsmiDLFolderMonitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDLFolderSupport});
            this.tsmiDLFolderMonitor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiDLFolderMonitor.ForeColor = System.Drawing.Color.Red;
            this.tsmiDLFolderMonitor.Name = "tsmiDLFolderMonitor";
            this.tsmiDLFolderMonitor.RadioButtonGroupName = "ProcessDLFolder";
            this.tsmiDLFolderMonitor.Size = new System.Drawing.Size(246, 22);
            this.tsmiDLFolderMonitor.Text = "Auto Monitor Downloads Folder";
            this.tsmiDLFolderMonitor.ToolTipText = resources.GetString("tsmiDLFolderMonitor.ToolTipText");
            this.tsmiDLFolderMonitor.CheckStateChanged += new System.EventHandler(this.tsmiDLFolderMonitor_CheckStateChanged);
            // 
            // tsmiDLFolderSupport
            // 
            this.tsmiDLFolderSupport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiDLFolderSupport.Image = global::CustomsForgeSongManager.Properties.Resources.dollarsign;
            this.tsmiDLFolderSupport.Name = "tsmiDLFolderSupport";
            this.tsmiDLFolderSupport.Size = new System.Drawing.Size(170, 22);
            this.tsmiDLFolderSupport.Text = "Click to Donate";
            this.tsmiDLFolderSupport.Click += new System.EventHandler(this.tsmiDLFolderSupport_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(243, 6);
            // 
            // tsmiRepairsRun
            // 
            this.tsmiRepairsRun.AssociatedEnumValue = null;
            this.tsmiRepairsRun.BackColor = System.Drawing.SystemColors.Control;
            this.tsmiRepairsRun.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiRepairsRun.Name = "tsmiRepairsRun";
            this.tsmiRepairsRun.RadioButtonGroupName = null;
            this.tsmiRepairsRun.Size = new System.Drawing.Size(246, 22);
            this.tsmiRepairsRun.Text = "Run Selected Repair Optons";
            this.tsmiRepairsRun.ToolTipText = "Select repair options first";
            this.tsmiRepairsRun.Click += new System.EventHandler(this.tsmiRepairsRun_Click);
            // 
            // tsmiMods
            // 
            this.tsmiMods.AssociatedEnumValue = null;
            this.tsmiMods.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiModsChangeAppId,
            this.toolStripSeparator4,
            this.tsmiModsPitchShifter,
            this.tsmiModsPitchShiftStandard,
            this.tsmiModsPreserveStats,
            this.tsmiModsPitchShiftOverwrite,
            this.toolStripSeparator5,
            this.tsmiTagStyle,
            this.tsmiModsTagArtwork,
            this.tsmiModsUntagArtwork,
            this.toolStripSeparator14,
            this.tsmiModsTheMover,
            this.tsmiModsMyCDLC});
            this.tsmiMods.Name = "tsmiMods";
            this.tsmiMods.RadioButtonGroupName = null;
            this.tsmiMods.Size = new System.Drawing.Size(83, 20);
            this.tsmiMods.Text = "Custom Mods";
            // 
            // tsmiModsChangeAppId
            // 
            this.tsmiModsChangeAppId.AssociatedEnumValue = null;
            this.tsmiModsChangeAppId.Image = ((System.Drawing.Image)(resources.GetObject("tsmiModsChangeAppId.Image")));
            this.tsmiModsChangeAppId.Name = "tsmiModsChangeAppId";
            this.tsmiModsChangeAppId.RadioButtonGroupName = null;
            this.tsmiModsChangeAppId.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsChangeAppId.Text = "Edit App IDs";
            this.tsmiModsChangeAppId.ToolTipText = "Change the CDLC App ID to\r\nany other DLC that you own.\r\n\r\nHINT:\r\nYou can also typ" +
                "e in any valid\r\nApp ID if it is not on the list.";
            this.tsmiModsChangeAppId.Click += new System.EventHandler(this.tsmiModsChangeAppId_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(200, 6);
            // 
            // tsmiModsPitchShifter
            // 
            this.tsmiModsPitchShifter.AssociatedEnumValue = null;
            this.tsmiModsPitchShifter.Image = ((System.Drawing.Image)(resources.GetObject("tsmiModsPitchShifter.Image")));
            this.tsmiModsPitchShifter.Name = "tsmiModsPitchShifter";
            this.tsmiModsPitchShifter.RadioButtonGroupName = "PitchShifter";
            this.tsmiModsPitchShifter.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsPitchShifter.Text = "Pitch Shifter";
            this.tsmiModsPitchShifter.ToolTipText = resources.GetString("tsmiModsPitchShifter.ToolTipText");
            this.tsmiModsPitchShifter.Click += new System.EventHandler(this.tsmiModsPitchShifter_Click);
            // 
            // tsmiModsPitchShiftStandard
            // 
            this.tsmiModsPitchShiftStandard.AssociatedEnumValue = null;
            this.tsmiModsPitchShiftStandard.CheckOnClick = true;
            this.tsmiModsPitchShiftStandard.Name = "tsmiModsPitchShiftStandard";
            this.tsmiModsPitchShiftStandard.RadioButtonGroupName = null;
            this.tsmiModsPitchShiftStandard.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsPitchShiftStandard.Text = "Force E Standard Tuning";
            this.tsmiModsPitchShiftStandard.ToolTipText = "If checked, forces E Standard Tuning even\r\nif arrangement is currently a Drop tun" +
                "ing.";
            this.tsmiModsPitchShiftStandard.CheckStateChanged += new System.EventHandler(this.ModsPitchShift_CheckStateChanged);
            // 
            // tsmiModsPreserveStats
            // 
            this.tsmiModsPreserveStats.AssociatedEnumValue = null;
            this.tsmiModsPreserveStats.CheckOnClick = true;
            this.tsmiModsPreserveStats.Name = "tsmiModsPreserveStats";
            this.tsmiModsPreserveStats.RadioButtonGroupName = null;
            this.tsmiModsPreserveStats.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsPreserveStats.Text = "Preserve Stats";
            this.tsmiModsPreserveStats.ToolTipText = resources.GetString("tsmiModsPreserveStats.ToolTipText");
            this.tsmiModsPreserveStats.CheckStateChanged += new System.EventHandler(this.ModsPitchShift_CheckStateChanged);
            this.tsmiModsPreserveStats.Click += new System.EventHandler(this.tsmiModPreserveStats_Click);
            // 
            // tsmiModsPitchShiftOverwrite
            // 
            this.tsmiModsPitchShiftOverwrite.AssociatedEnumValue = null;
            this.tsmiModsPitchShiftOverwrite.CheckOnClick = true;
            this.tsmiModsPitchShiftOverwrite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiModsPitchShiftOverwrite.ForeColor = System.Drawing.Color.Red;
            this.tsmiModsPitchShiftOverwrite.Name = "tsmiModsPitchShiftOverwrite";
            this.tsmiModsPitchShiftOverwrite.RadioButtonGroupName = "PitchShift";
            this.tsmiModsPitchShiftOverwrite.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsPitchShiftOverwrite.Text = "Overwrite File";
            this.tsmiModsPitchShiftOverwrite.ToolTipText = "If checked, overwrite existing CDLC\r\nwith pitch shifted arrangements and\r\nreduces" +
                " the risk of in-game hangs.";
            this.tsmiModsPitchShiftOverwrite.CheckStateChanged += new System.EventHandler(this.ModsPitchShift_CheckStateChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(200, 6);
            // 
            // tsmiTagStyle
            // 
            this.tsmiTagStyle.AssociatedEnumValue = null;
            this.tsmiTagStyle.Name = "tsmiTagStyle";
            this.tsmiTagStyle.RadioButtonGroupName = null;
            this.tsmiTagStyle.Size = new System.Drawing.Size(203, 22);
            this.tsmiTagStyle.Text = "Tag Style";
            // 
            // tsmiModsTagArtwork
            // 
            this.tsmiModsTagArtwork.AssociatedEnumValue = null;
            this.tsmiModsTagArtwork.Image = ((System.Drawing.Image)(resources.GetObject("tsmiModsTagArtwork.Image")));
            this.tsmiModsTagArtwork.Name = "tsmiModsTagArtwork";
            this.tsmiModsTagArtwork.RadioButtonGroupName = null;
            this.tsmiModsTagArtwork.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsTagArtwork.Text = "Tag Album Artwork";
            this.tsmiModsTagArtwork.ToolTipText = "Note: CFSM does not tag ODLC.";
            this.tsmiModsTagArtwork.Click += new System.EventHandler(this.tsmiModsTagArtwork_Click);
            // 
            // tsmiModsUntagArtwork
            // 
            this.tsmiModsUntagArtwork.AssociatedEnumValue = null;
            this.tsmiModsUntagArtwork.Image = ((System.Drawing.Image)(resources.GetObject("tsmiModsUntagArtwork.Image")));
            this.tsmiModsUntagArtwork.Name = "tsmiModsUntagArtwork";
            this.tsmiModsUntagArtwork.RadioButtonGroupName = null;
            this.tsmiModsUntagArtwork.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsUntagArtwork.Text = "Un-Tag";
            this.tsmiModsUntagArtwork.Click += new System.EventHandler(this.tsmiModsUntagArtwork_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(200, 6);
            // 
            // tsmiModsTheMover
            // 
            this.tsmiModsTheMover.AssociatedEnumValue = null;
            this.tsmiModsTheMover.CheckOnClick = true;
            this.tsmiModsTheMover.Name = "tsmiModsTheMover";
            this.tsmiModsTheMover.RadioButtonGroupName = null;
            this.tsmiModsTheMover.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsTheMover.Text = "\'The\' mover";
            this.tsmiModsTheMover.ToolTipText = "\'The\' Mover e.g., The Beatles -> Beatles, The";
            this.tsmiModsTheMover.CheckStateChanged += new System.EventHandler(this.tsmiModsTheMover_CheckStateChanged);
            // 
            // tsmiModsMyCDLC
            // 
            this.tsmiModsMyCDLC.AssociatedEnumValue = null;
            this.tsmiModsMyCDLC.Image = global::CustomsForgeSongManager.Properties.Resources.rename;
            this.tsmiModsMyCDLC.Name = "tsmiModsMyCDLC";
            this.tsmiModsMyCDLC.RadioButtonGroupName = null;
            this.tsmiModsMyCDLC.Size = new System.Drawing.Size(203, 22);
            this.tsmiModsMyCDLC.Text = "Show My CDLC";
            this.tsmiModsMyCDLC.CheckStateChanged += new System.EventHandler(this.tsmiModsMyCDLC_CheckStateChanged);
            // 
            // tsmiFiles
            // 
            this.tsmiFiles.AssociatedEnumValue = null;
            this.tsmiFiles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilesCheckODLC,
            this.toolStripSeparator1,
            this.tsmiFilesCleanDlc,
            this.tsmiFilesRestore,
            this.tsmiFilesArchive,
            this.toolStripSeparator6,
            this.tsmiFilesOrganize,
            this.tsmiFilesUnorganize,
            this.tsmiFilesIncludeODLC});
            this.tsmiFiles.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFiles.Image")));
            this.tsmiFiles.Name = "tsmiFiles";
            this.tsmiFiles.RadioButtonGroupName = null;
            this.tsmiFiles.Size = new System.Drawing.Size(56, 20);
            this.tsmiFiles.Text = "Files";
            // 
            // tsmiFilesCheckODLC
            // 
            this.tsmiFilesCheckODLC.AssociatedEnumValue = null;
            this.tsmiFilesCheckODLC.Image = global::CustomsForgeSongManager.Properties.Resources.rescan;
            this.tsmiFilesCheckODLC.Name = "tsmiFilesCheckODLC";
            this.tsmiFilesCheckODLC.RadioButtonGroupName = null;
            this.tsmiFilesCheckODLC.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesCheckODLC.Text = "Check CDLC/ODLC";
            this.tsmiFilesCheckODLC.ToolTipText = "Determines if any of your CDLC\r\nhave been replaced by ODLC.";
            this.tsmiFilesCheckODLC.Click += new System.EventHandler(this.tsmiFilesCheckODLC_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiFilesCleanDlc
            // 
            this.tsmiFilesCleanDlc.AssociatedEnumValue = null;
            this.tsmiFilesCleanDlc.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFilesCleanDlc.Image")));
            this.tsmiFilesCleanDlc.Name = "tsmiFilesCleanDlc";
            this.tsmiFilesCleanDlc.RadioButtonGroupName = null;
            this.tsmiFilesCleanDlc.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesCleanDlc.Text = "Clean \'dlc\' Folder";
            this.tsmiFilesCleanDlc.ToolTipText = "Remove any [.org] [.max] or [.cor]\r\nbackup files from the \'dlc\' folder and\r\nsubfo" +
                "lders.";
            this.tsmiFilesCleanDlc.Click += new System.EventHandler(this.tsmiFilesCleanDlc_Click);
            // 
            // tsmiFilesRestore
            // 
            this.tsmiFilesRestore.AssociatedEnumValue = null;
            this.tsmiFilesRestore.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilesRestoreBak,
            this.tsmiFilesRestoreOrg,
            this.tsmiFilesRestoreMax,
            this.tsmiFilesRestoreCor});
            this.tsmiFilesRestore.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFilesRestore.Image")));
            this.tsmiFilesRestore.Name = "tsmiFilesRestore";
            this.tsmiFilesRestore.RadioButtonGroupName = null;
            this.tsmiFilesRestore.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesRestore.Text = "Restore";
            // 
            // tsmiFilesRestoreBak
            // 
            this.tsmiFilesRestoreBak.AssociatedEnumValue = null;
            this.tsmiFilesRestoreBak.Name = "tsmiFilesRestoreBak";
            this.tsmiFilesRestoreBak.RadioButtonGroupName = null;
            this.tsmiFilesRestoreBak.Size = new System.Drawing.Size(163, 22);
            this.tsmiFilesRestoreBak.Text = "File Type (.bak)";
            this.tsmiFilesRestoreBak.Click += new System.EventHandler(this.tsmiFilesRestoreBak_Click);
            // 
            // tsmiFilesRestoreOrg
            // 
            this.tsmiFilesRestoreOrg.AssociatedEnumValue = null;
            this.tsmiFilesRestoreOrg.Name = "tsmiFilesRestoreOrg";
            this.tsmiFilesRestoreOrg.RadioButtonGroupName = null;
            this.tsmiFilesRestoreOrg.Size = new System.Drawing.Size(163, 22);
            this.tsmiFilesRestoreOrg.Text = "File Type (.org)";
            this.tsmiFilesRestoreOrg.Click += new System.EventHandler(this.tsmiFilesRestoreOrg_Click);
            // 
            // tsmiFilesRestoreMax
            // 
            this.tsmiFilesRestoreMax.AssociatedEnumValue = null;
            this.tsmiFilesRestoreMax.Name = "tsmiFilesRestoreMax";
            this.tsmiFilesRestoreMax.RadioButtonGroupName = null;
            this.tsmiFilesRestoreMax.Size = new System.Drawing.Size(163, 22);
            this.tsmiFilesRestoreMax.Text = "File Type (.max)";
            this.tsmiFilesRestoreMax.Click += new System.EventHandler(this.tsmiFilesRestoreMax_Click);
            // 
            // tsmiFilesRestoreCor
            // 
            this.tsmiFilesRestoreCor.AssociatedEnumValue = null;
            this.tsmiFilesRestoreCor.Name = "tsmiFilesRestoreCor";
            this.tsmiFilesRestoreCor.RadioButtonGroupName = null;
            this.tsmiFilesRestoreCor.Size = new System.Drawing.Size(163, 22);
            this.tsmiFilesRestoreCor.Text = "File Type (.cor)";
            this.tsmiFilesRestoreCor.Click += new System.EventHandler(this.tsmiFilesRestoreCor_Click);
            // 
            // tsmiFilesArchive
            // 
            this.tsmiFilesArchive.AssociatedEnumValue = null;
            this.tsmiFilesArchive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilesArcBak,
            this.tsmiFilesArcOrg,
            this.tsmiFilesArcMax,
            this.tsmiFilesArcCor,
            this.tsmiFilesArcDeleteAfter});
            this.tsmiFilesArchive.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFilesArchive.Image")));
            this.tsmiFilesArchive.Name = "tsmiFilesArchive";
            this.tsmiFilesArchive.RadioButtonGroupName = null;
            this.tsmiFilesArchive.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesArchive.Text = "Archive";
            // 
            // tsmiFilesArcBak
            // 
            this.tsmiFilesArcBak.AssociatedEnumValue = null;
            this.tsmiFilesArcBak.Name = "tsmiFilesArcBak";
            this.tsmiFilesArcBak.RadioButtonGroupName = null;
            this.tsmiFilesArcBak.Size = new System.Drawing.Size(210, 22);
            this.tsmiFilesArcBak.Text = "File Type (.bak)";
            this.tsmiFilesArcBak.Click += new System.EventHandler(this.tsmiFilesArcBak_Click);
            // 
            // tsmiFilesArcOrg
            // 
            this.tsmiFilesArcOrg.AssociatedEnumValue = null;
            this.tsmiFilesArcOrg.Name = "tsmiFilesArcOrg";
            this.tsmiFilesArcOrg.RadioButtonGroupName = null;
            this.tsmiFilesArcOrg.Size = new System.Drawing.Size(210, 22);
            this.tsmiFilesArcOrg.Text = "File Type (.org)";
            this.tsmiFilesArcOrg.Click += new System.EventHandler(this.tsmiFilesArcOrg_Click);
            // 
            // tsmiFilesArcMax
            // 
            this.tsmiFilesArcMax.AssociatedEnumValue = null;
            this.tsmiFilesArcMax.Name = "tsmiFilesArcMax";
            this.tsmiFilesArcMax.RadioButtonGroupName = null;
            this.tsmiFilesArcMax.Size = new System.Drawing.Size(210, 22);
            this.tsmiFilesArcMax.Text = "File Type (.max)";
            this.tsmiFilesArcMax.Click += new System.EventHandler(this.tsmiFilesArcMax_Click);
            // 
            // tsmiFilesArcCor
            // 
            this.tsmiFilesArcCor.AssociatedEnumValue = null;
            this.tsmiFilesArcCor.Name = "tsmiFilesArcCor";
            this.tsmiFilesArcCor.RadioButtonGroupName = null;
            this.tsmiFilesArcCor.Size = new System.Drawing.Size(210, 22);
            this.tsmiFilesArcCor.Text = "File Type (.cor)";
            this.tsmiFilesArcCor.Click += new System.EventHandler(this.tsmiFilesArcCor_Click);
            // 
            // tsmiFilesArcDeleteAfter
            // 
            this.tsmiFilesArcDeleteAfter.AssociatedEnumValue = null;
            this.tsmiFilesArcDeleteAfter.CheckOnClick = true;
            this.tsmiFilesArcDeleteAfter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiFilesArcDeleteAfter.ForeColor = System.Drawing.Color.Red;
            this.tsmiFilesArcDeleteAfter.Name = "tsmiFilesArcDeleteAfter";
            this.tsmiFilesArcDeleteAfter.RadioButtonGroupName = null;
            this.tsmiFilesArcDeleteAfter.Size = new System.Drawing.Size(210, 22);
            this.tsmiFilesArcDeleteAfter.Text = "Delete Files After Arhiving";
            this.tsmiFilesArcDeleteAfter.CheckedChanged += new System.EventHandler(this.tsmiFilesArcDelete_CheckedChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(238, 6);
            // 
            // tsmiFilesOrganize
            // 
            this.tsmiFilesOrganize.AssociatedEnumValue = null;
            this.tsmiFilesOrganize.Name = "tsmiFilesOrganize";
            this.tsmiFilesOrganize.RadioButtonGroupName = null;
            this.tsmiFilesOrganize.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesOrganize.Text = "Organize Artist Name Subfolders";
            this.tsmiFilesOrganize.ToolTipText = resources.GetString("tsmiFilesOrganize.ToolTipText");
            this.tsmiFilesOrganize.Click += new System.EventHandler(this.tsmiFilesOrganize_Click);
            // 
            // tsmiFilesUnorganize
            // 
            this.tsmiFilesUnorganize.AssociatedEnumValue = null;
            this.tsmiFilesUnorganize.Name = "tsmiFilesUnorganize";
            this.tsmiFilesUnorganize.RadioButtonGroupName = null;
            this.tsmiFilesUnorganize.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesUnorganize.Text = "Restore CDLC to the \'dlc\' Folder";
            this.tsmiFilesUnorganize.ToolTipText = resources.GetString("tsmiFilesUnorganize.ToolTipText");
            this.tsmiFilesUnorganize.Click += new System.EventHandler(this.tsmiFilesUnorganize_Click);
            // 
            // tsmiFilesIncludeODLC
            // 
            this.tsmiFilesIncludeODLC.AssociatedEnumValue = null;
            this.tsmiFilesIncludeODLC.CheckOnClick = true;
            this.tsmiFilesIncludeODLC.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsmiFilesIncludeODLC.ForeColor = System.Drawing.Color.Red;
            this.tsmiFilesIncludeODLC.Name = "tsmiFilesIncludeODLC";
            this.tsmiFilesIncludeODLC.RadioButtonGroupName = null;
            this.tsmiFilesIncludeODLC.Size = new System.Drawing.Size(241, 22);
            this.tsmiFilesIncludeODLC.Text = "Include ODLC Files";
            this.tsmiFilesIncludeODLC.ToolTipText = "If checked, ODLC files will be organized\r\ntoo, even though they are not selectabl" +
                "e.";
            this.tsmiFilesIncludeODLC.Click += new System.EventHandler(this.tsmiFilesIncludeODLC_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.AssociatedEnumValue = null;
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpGeneral,
            this.toolStripSeparator3,
            this.tsmiHelpRepairs,
            this.tsmiHelpErrorLog});
            this.tsmiHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelp.Image")));
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.RadioButtonGroupName = null;
            this.tsmiHelp.Size = new System.Drawing.Size(56, 20);
            this.tsmiHelp.Text = "Help";
            // 
            // tsmiHelpGeneral
            // 
            this.tsmiHelpGeneral.AssociatedEnumValue = null;
            this.tsmiHelpGeneral.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelpGeneral.Image")));
            this.tsmiHelpGeneral.Name = "tsmiHelpGeneral";
            this.tsmiHelpGeneral.RadioButtonGroupName = null;
            this.tsmiHelpGeneral.Size = new System.Drawing.Size(154, 22);
            this.tsmiHelpGeneral.Text = "General Help";
            this.tsmiHelpGeneral.Click += new System.EventHandler(this.tsmiHelpGeneral_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
            // 
            // tsmiHelpRepairs
            // 
            this.tsmiHelpRepairs.AssociatedEnumValue = null;
            this.tsmiHelpRepairs.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelpRepairs.Image")));
            this.tsmiHelpRepairs.Name = "tsmiHelpRepairs";
            this.tsmiHelpRepairs.RadioButtonGroupName = null;
            this.tsmiHelpRepairs.Size = new System.Drawing.Size(154, 22);
            this.tsmiHelpRepairs.Text = "Repairs Help";
            this.tsmiHelpRepairs.Click += new System.EventHandler(this.tsmiHelpRepairs_Click);
            // 
            // tsmiHelpErrorLog
            // 
            this.tsmiHelpErrorLog.AssociatedEnumValue = null;
            this.tsmiHelpErrorLog.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelpErrorLog.Image")));
            this.tsmiHelpErrorLog.Name = "tsmiHelpErrorLog";
            this.tsmiHelpErrorLog.RadioButtonGroupName = null;
            this.tsmiHelpErrorLog.Size = new System.Drawing.Size(154, 22);
            this.tsmiHelpErrorLog.Text = "View Error Log";
            this.tsmiHelpErrorLog.Click += new System.EventHandler(this.tsmiHelpErrorLog_Click);
            // 
            // tsmiDevDebugUse
            // 
            this.tsmiDevDebugUse.Name = "tsmiDevDebugUse";
            this.tsmiDevDebugUse.Size = new System.Drawing.Size(98, 20);
            this.tsmiDevDebugUse.Text = "Devs Debug Use";
            this.tsmiDevDebugUse.Click += new System.EventHandler(this.tsmiDevsDebugUse_Click);
            // 
            // SongManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gb_Main_Grid);
            this.Controls.Add(this.gb_Main_Search);
            this.Controls.Add(this.menuStrip);
            this.Name = "SongManager";
            this.Size = new System.Drawing.Size(899, 490);
            this.Resize += new System.EventHandler(this.SongManager_Resize);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.gb_Main_Search.ResumeLayout(false);
            this.cmsSongManager.ResumeLayout(false);
            this.gb_Main_Grid.ResumeLayout(false);
            this.gb_Main_Grid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongsMaster)).EndInit();
            this.cmsSongManagerColumns.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.Label lbl_Search;
        private CueTextBox cueSearch;
        private System.Windows.Forms.GroupBox gb_Main_Search;
        private CustomControls.ToolStripEnhancedMenuItem cmsShowSongInfo;
        private CustomControls.ToolStripEnhancedMenuItem cmsOpenSongPage;
        private CustomControls.ToolStripEnhancedMenuItem cmsCheckForUpdate;
        private CustomControls.ToolStripEnhancedMenuItem cmsOpenSongLocation;
        private CustomControls.ToolStripEnhancedMenuItem cmsEdit;
        private CustomControls.ToolStripEnhancedMenuItem cmsGetCharterName;
        private CustomControls.ToolStripEnhancedMenuItem cmsDelete;
        private CustomControls.ToolStripEnhancedMenuItem cmsBackup;
        private System.Windows.Forms.ContextMenuStrip cmsSongManager;
        private RADataGridView dgvSongsMaster;
        private System.Windows.Forms.LinkLabel lnkLblSelectAll;
        private System.Windows.Forms.GroupBox gb_Main_Grid;
        private System.Windows.Forms.ContextMenuStrip cmsSongManagerColumns;
        private CustomControls.ToolStripEnhancedMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private CustomControls.ToolStripEnhancedMenuItem cmsTaggerPreview;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsChangeAppId;
        private System.Windows.Forms.LinkLabel lnklblToggle;
        private System.Windows.Forms.CheckBox chkIncludeSubfolders;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsPitchShifter;
        private System.Windows.Forms.MenuStrip menuStrip;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescan;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanQuick;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanFull;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairs;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsMastery;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsMaxFive;
        private CustomControls.ToolStripEnhancedMenuItem tsmiMods;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsTagArtwork;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsUntagArtwork;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFiles;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesRestoreOrg;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesRestoreMax;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesRestoreCor;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelp;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelpGeneral;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelpErrorLog;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsTheMover;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsMyCDLC;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesRestore;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsPreserveStats;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsUsingOrg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsMultitone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveNDD;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveBass;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveGuitar;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveBonus;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveMetronome;
        private CustomControls.ToolStripEnhancedMenuItem tsmiIgnoreStopLimit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private CustomControls.ToolStripEnhancedMenuItem tsmiTagStyle;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRemoveSections;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private CustomControls.ToolStripEnhancedMenuItem tsmiSkipRemastered;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private CustomControls.ToolStripEnhancedMenuItem tsmiOverwriteDD;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsRun;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelpRepairs;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArchive;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArcOrg;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArcMax;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArcCor;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArcDeleteAfter;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesCleanDlc;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRepairsAddDD;
        private CustomControls.ToolStripEnhancedMenuItem tsmiAddDDRemoveSustain;
        private CustomControls.ToolStripSpringTextBox tsmiAddDDCfgPath;
        private CustomControls.ToolStripSpringTextBox tsmiAddDDRampUpPath;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDDSettings;
        private CustomControls.ToolStripNumericUpDown tsmiAddDDNumericUpDown;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDLFolderProcess;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDLFolderMonitor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesRestoreBak;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesArcBak;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsPitchShiftOverwrite;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesCheckODLC;
        private CustomControls.ToolStripEnhancedMenuItem cmsEnableDisable;
        private System.Windows.Forms.ToolStripMenuItem cmsPlaySelected;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesOrganize;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesUnorganize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFilesIncludeODLC;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private SubclassedDataGridView dgvSongsDetail;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsPitchShiftStandard;
        private CustomControls.ToolStripEnhancedMenuItem tsmiModsPreserveStats;
        private CustomControls.ToolStripEnhancedMenuItem cmsActions;
        private ToolStripSeparator toolStripSeparator15;
        private ToolStripSeparator toolStripSeparator2;
        private CustomControls.ToolStripEnhancedMenuItem tsmiFixLowBass;
        private ToolStripMenuItem tsmiDevDebugUse;
        private ToolStripSeparator toolStripSeparator11;
        private DataGridViewTextBoxColumn colDetailKey;
        private DataGridViewTextBoxColumn colDetailPID;
        private DataGridViewTextBoxColumn colDetailArrangement;
        private DataGridViewTextBoxColumn colDetailTuning;
        private DataGridViewTextBoxColumn colDetailPitch;
        private DataGridViewTextBoxColumn colDetailToneBase;
        private DataGridViewTextBoxColumn colDetailDDMax;
        private ToolStripMenuItem tsmiDLFolderSupport;
        private CheckBox chkProtectODLC;
        private DataGridViewImageColumn colShowDetail;
        private DataGridViewAutoFilterTextBoxColumn colKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewImageColumn colBass;
        private DataGridViewImageColumn colLead;
        private DataGridViewImageColumn colRhythm;
        private DataGridViewImageColumn colVocals;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewAutoFilterTextBoxColumn colSongLength;
        private DataGridViewAutoFilterTextBoxColumn colSongAverageTempo;
        private DataGridViewAutoFilterTextBoxColumn colSongVolume;
        private DataGridViewAutoFilterTextBoxColumn colLastConversionDateTime;
        private DataGridViewAutoFilterTextBoxColumn colDD;
        private DataGridViewAutoFilterTextBoxColumn colArrangements;
        private DataGridViewAutoFilterTextBoxColumn colTunings;
        private DataGridViewAutoFilterTextBoxColumn colTones;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colFileDate;
        private DataGridViewAutoFilterTextBoxColumn colFileSize;
        private DataGridViewAutoFilterTextBoxColumn colStatus;
        private DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageAuthor;
        private DataGridViewAutoFilterTextBoxColumn colPackageVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageComment;
        private DataGridViewRatingColumn colPackageRating;
        private DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionID;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionDate;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionVersion;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionAuthor;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbumDate;
        private DataGridViewAutoFilterTextBoxColumn colArtistSort;
        private DataGridViewAutoFilterTextBoxColumn colTitleSort;
        private DataGridViewAutoFilterTextBoxColumn colAlbumSort;
        private ToolStripSeparator toolStripSeparator16;
        private CustomControls.ToolStripEnhancedMenuItem tsmiAdjustScrollSpeed;
        private CustomControls.ToolStripEnhancedMenuItem tsmiSSSettings;
        private CustomControls.ToolStripNumericUpDown tsmiNudScrollSpeed;

    }
}
