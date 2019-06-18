using System.Windows.Forms;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class Duplicates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Duplicates));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmsDuplicateColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsColumnHeaders = new System.Windows.Forms.ToolStripMenuItem();
            this.lnkSelectByATAD = new System.Windows.Forms.LinkLabel();
            this.chkIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.lnkSelectByToolkitVersion = new System.Windows.Forms.LinkLabel();
            this.lnkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.lnklblToggle = new System.Windows.Forms.LinkLabel();
            this.txtNoDuplicates = new System.Windows.Forms.TextBox();
            this.dgvDuplicates = new DataGridViewTools.RADataGridView();
            this.colPID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPIDArrangement = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAvgTempo = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colLastConversionDateTime = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileSize = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colUpdated = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colToolkitVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageComment = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageRating = new DataGridViewTools.DataGridViewRatingColumn();
            this.colTagged = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionUpdated = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbumDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsDuplicates = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsActions = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsEdit = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsShowSongInfo = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsOpenLocation = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsEnableDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelete = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsMove = new CustomControls.ToolStripEnhancedMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiRescan = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanAll = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiShowEnabledDisabled = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDuplicateType = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDuplicateTypeDLCKeyATA = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiDuplicateTypePID = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelp = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsDuplicateColumns.SuspendLayout();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuplicates)).BeginInit();
            this.cmsDuplicates.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsDuplicateColumns
            // 
            this.cmsDuplicateColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsColumnHeaders});
            this.cmsDuplicateColumns.Name = "cmsDuplicate";
            this.cmsDuplicateColumns.Size = new System.Drawing.Size(107, 26);
            // 
            // cmsColumnHeaders
            // 
            this.cmsColumnHeaders.Name = "cmsColumnHeaders";
            this.cmsColumnHeaders.Size = new System.Drawing.Size(106, 22);
            this.cmsColumnHeaders.Text = "Test";
            this.cmsColumnHeaders.Click += new System.EventHandler(this.exploreToolStripMenuItem_Click);
            // 
            // lnkSelectByATAD
            // 
            this.lnkSelectByATAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkSelectByATAD.AutoSize = true;
            this.lnkSelectByATAD.ForeColor = System.Drawing.Color.Black;
            this.lnkSelectByATAD.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSelectByATAD.Location = new System.Drawing.Point(232, 492);
            this.lnkSelectByATAD.Name = "lnkSelectByATAD";
            this.lnkSelectByATAD.Size = new System.Drawing.Size(192, 13);
            this.lnkSelectByATAD.TabIndex = 26;
            this.lnkSelectByATAD.TabStop = true;
            this.lnkSelectByATAD.Text = "Select All Older By ArtistTitleAlbumDate";
            this.toolTip.SetToolTip(this.lnkSelectByATAD, "Select all duplicate CDLC ordered\r\nby ArtistTitleAlbumDate excluding\r\nthe newest " +
                    "version and ODLC");
            this.lnkSelectByATAD.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSelectByATAD.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectOlderVersions_LinkClicked);
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIncludeSubfolders.AutoSize = true;
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(720, 491);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(114, 17);
            this.chkIncludeSubfolders.TabIndex = 25;
            this.chkIncludeSubfolders.Text = "Include Subfolders";
            this.toolTip.SetToolTip(this.chkIncludeSubfolders, resources.GetString("chkIncludeSubfolders.ToolTip"));
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            this.chkIncludeSubfolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkIncludeSubfolders_MouseUp);
            // 
            // gbResults
            // 
            this.gbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResults.Controls.Add(this.lnkSelectByToolkitVersion);
            this.gbResults.Controls.Add(this.lnkLblSelectAll);
            this.gbResults.Controls.Add(this.lnklblToggle);
            this.gbResults.Controls.Add(this.chkIncludeSubfolders);
            this.gbResults.Controls.Add(this.lnkSelectByATAD);
            this.gbResults.Controls.Add(this.txtNoDuplicates);
            this.gbResults.Controls.Add(this.dgvDuplicates);
            this.gbResults.Location = new System.Drawing.Point(4, 3);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(864, 517);
            this.gbResults.TabIndex = 16;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results Grid:";
            // 
            // lnkSelectByToolkitVersion
            // 
            this.lnkSelectByToolkitVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkSelectByToolkitVersion.AutoSize = true;
            this.lnkSelectByToolkitVersion.ForeColor = System.Drawing.Color.Black;
            this.lnkSelectByToolkitVersion.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSelectByToolkitVersion.Location = new System.Drawing.Point(469, 492);
            this.lnkSelectByToolkitVersion.Name = "lnkSelectByToolkitVersion";
            this.lnkSelectByToolkitVersion.Size = new System.Drawing.Size(164, 13);
            this.lnkSelectByToolkitVersion.TabIndex = 29;
            this.lnkSelectByToolkitVersion.TabStop = true;
            this.lnkSelectByToolkitVersion.Text = "Select All Older By ToolkitVersion";
            this.toolTip.SetToolTip(this.lnkSelectByToolkitVersion, "Select all duplicate CDLC ordered by\r\nToolkitVersion excluding the newest\r\nversio" +
                    "n and ODLC");
            this.lnkSelectByToolkitVersion.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSelectByToolkitVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectOlderVersions_LinkClicked);
            // 
            // lnkLblSelectAll
            // 
            this.lnkLblSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkLblSelectAll.AutoSize = true;
            this.lnkLblSelectAll.ForeColor = System.Drawing.Color.Black;
            this.lnkLblSelectAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkLblSelectAll.Location = new System.Drawing.Point(21, 492);
            this.lnkLblSelectAll.Name = "lnkLblSelectAll";
            this.lnkLblSelectAll.Size = new System.Drawing.Size(82, 13);
            this.lnkLblSelectAll.TabIndex = 27;
            this.lnkLblSelectAll.TabStop = true;
            this.lnkLblSelectAll.Text = "Select All/None";
            this.lnkLblSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSelectAll_LinkClicked);
            // 
            // lnklblToggle
            // 
            this.lnklblToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnklblToggle.AutoSize = true;
            this.lnklblToggle.ForeColor = System.Drawing.Color.Black;
            this.lnklblToggle.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnklblToggle.Location = new System.Drawing.Point(124, 492);
            this.lnklblToggle.Name = "lnklblToggle";
            this.lnklblToggle.Size = new System.Drawing.Size(87, 13);
            this.lnklblToggle.TabIndex = 28;
            this.lnklblToggle.TabStop = true;
            this.lnklblToggle.Text = "Toggle Selection";
            this.lnklblToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnklblToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToggle_LinkClicked);
            // 
            // txtNoDuplicates
            // 
            this.txtNoDuplicates.BackColor = System.Drawing.Color.Chartreuse;
            this.txtNoDuplicates.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoDuplicates.ForeColor = System.Drawing.Color.Black;
            this.txtNoDuplicates.Location = new System.Drawing.Point(248, 200);
            this.txtNoDuplicates.Multiline = true;
            this.txtNoDuplicates.Name = "txtNoDuplicates";
            this.txtNoDuplicates.Size = new System.Drawing.Size(355, 119);
            this.txtNoDuplicates.TabIndex = 16;
            this.txtNoDuplicates.Text = "\r\nGood News ...\r\nNo Duplicates Found";
            this.txtNoDuplicates.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNoDuplicates.Visible = false;
            // 
            // dgvDuplicates
            // 
            this.dgvDuplicates.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDuplicates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDuplicates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDuplicates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPID,
            this.colPIDArrangement,
            this.colKey,
            this.colSelect,
            this.colEnabled,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colSongYear,
            this.colSongLength,
            this.colAvgTempo,
            this.colLastConversionDateTime,
            this.colAppID,
            this.colFileName,
            this.colFilePath,
            this.colFileDate,
            this.colFileSize,
            this.colRepairStatus,
            this.colUpdated,
            this.colToolkitVersion,
            this.colPackageAuthor,
            this.colPackageVersion,
            this.colPackageComment,
            this.colPackageRating,
            this.colTagged,
            this.colIgnitionID,
            this.colIgnitionUpdated,
            this.colIgnitionVersion,
            this.colIgnitionAuthor,
            this.colArtistTitleAlbum,
            this.colArtistTitleAlbumDate});
            this.dgvDuplicates.Location = new System.Drawing.Point(6, 19);
            this.dgvDuplicates.Name = "dgvDuplicates";
            this.dgvDuplicates.RowHeadersVisible = false;
            this.dgvDuplicates.Size = new System.Drawing.Size(852, 460);
            this.dgvDuplicates.TabIndex = 17;
            this.dgvDuplicates.Tag = "Duplicates";
            this.toolTip.SetToolTip(this.dgvDuplicates, "Left mouse click the \'Select\' checkbox to select a row\r\nRight mouse click on row " +
                    "to show file operation options");
            this.dgvDuplicates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDuplicates_CellContentClick);
            this.dgvDuplicates.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDuplicates_CellDoubleClick);
            this.dgvDuplicates.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDuplicates_CellFormatting);
            this.dgvDuplicates.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDuplicates_CellMouseDown);
            this.dgvDuplicates.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDuplicates_CellMouseUp);
            this.dgvDuplicates.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDuplicates_DataBindingComplete);
            this.dgvDuplicates.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDuplicates_DataError);
            this.dgvDuplicates.Sorted += new System.EventHandler(this.dgvDuplicates_Sorted);
            this.dgvDuplicates.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDuplicates_Paint);
            this.dgvDuplicates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDuplicates_KeyDown);
            // 
            // colPID
            // 
            this.colPID.DataPropertyName = "PID";
            this.colPID.HeaderText = "Persistent ID";
            this.colPID.Name = "colPID";
            this.colPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colPIDArrangement
            // 
            this.colPIDArrangement.DataPropertyName = "PIDArrangement";
            this.colPIDArrangement.HeaderText = "Arrangement";
            this.colPIDArrangement.Name = "colPIDArrangement";
            this.colPIDArrangement.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            this.colSelect.Width = 43;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnabled.Visible = false;
            this.colEnabled.Width = 54;
            // 
            // colArtist
            // 
            this.colArtist.DataPropertyName = "Artist";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArtist.Width = 50;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Song Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTitle.Width = 50;
            // 
            // colAlbum
            // 
            this.colAlbum.DataPropertyName = "Album";
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAlbum.Width = 50;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
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
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.colSongLength.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSongLength.HeaderText = "Length Seconds";
            this.colSongLength.Name = "colSongLength";
            this.colSongLength.ReadOnly = true;
            this.colSongLength.Visible = false;
            // 
            // colAvgTempo
            // 
            this.colAvgTempo.DataPropertyName = "SongAverageTempo";
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.colAvgTempo.DefaultCellStyle = dataGridViewCellStyle4;
            this.colAvgTempo.HeaderText = "BPM";
            this.colAvgTempo.Name = "colAvgTempo";
            this.colAvgTempo.ReadOnly = true;
            this.colAvgTempo.Visible = false;
            // 
            // colLastConversionDateTime
            // 
            this.colLastConversionDateTime.DataPropertyName = "LastConversionDateTime";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.NullValue = null;
            this.colLastConversionDateTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.colLastConversionDateTime.HeaderText = "LastConversionDateTime";
            this.colLastConversionDateTime.Name = "colLastConversionDateTime";
            this.colLastConversionDateTime.ReadOnly = true;
            this.colLastConversionDateTime.Visible = false;
            this.colLastConversionDateTime.Width = 50;
            // 
            // colAppID
            // 
            this.colAppID.DataPropertyName = "AppID";
            this.colAppID.HeaderText = "App ID";
            this.colAppID.Name = "colAppID";
            this.colAppID.ReadOnly = true;
            this.colAppID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAppID.Visible = false;
            this.colAppID.Width = 80;
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileName.Width = 50;
            // 
            // colFilePath
            // 
            this.colFilePath.DataPropertyName = "FilePath";
            this.colFilePath.HeaderText = "FilePath";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilePath.Visible = false;
            this.colFilePath.Width = 150;
            // 
            // colFileDate
            // 
            this.colFileDate.DataPropertyName = "FileDate";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.NullValue = null;
            this.colFileDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colFileDate.HeaderText = "FileDate";
            this.colFileDate.Name = "colFileDate";
            this.colFileDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "FileSize";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.NullValue = null;
            this.colFileSize.DefaultCellStyle = dataGridViewCellStyle7;
            this.colFileSize.HeaderText = "FileSize (bytes)";
            this.colFileSize.Name = "colFileSize";
            // 
            // colRepairStatus
            // 
            this.colRepairStatus.DataPropertyName = "RepairStatus";
            this.colRepairStatus.HeaderText = "Repair Status";
            this.colRepairStatus.Name = "colRepairStatus";
            this.colRepairStatus.ReadOnly = true;
            this.colRepairStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRepairStatus.Visible = false;
            this.colRepairStatus.Width = 50;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "LastConversionDateTime";
            dataGridViewCellStyle8.NullValue = null;
            this.colUpdated.DefaultCellStyle = dataGridViewCellStyle8;
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Visible = false;
            this.colUpdated.Width = 50;
            // 
            // colToolkitVersion
            // 
            this.colToolkitVersion.DataPropertyName = "ToolkitVersion";
            this.colToolkitVersion.HeaderText = "ToolkitVersion";
            this.colToolkitVersion.Name = "colToolkitVersion";
            this.colToolkitVersion.ReadOnly = true;
            this.colToolkitVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToolkitVersion.Visible = false;
            this.colToolkitVersion.Width = 110;
            // 
            // colPackageAuthor
            // 
            this.colPackageAuthor.DataPropertyName = "PackageAuthor";
            this.colPackageAuthor.HeaderText = "PackageAuthor";
            this.colPackageAuthor.Name = "colPackageAuthor";
            this.colPackageAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colPackageVersion
            // 
            this.colPackageVersion.DataPropertyName = "PackageVersion";
            this.colPackageVersion.HeaderText = "PackageVersion";
            this.colPackageVersion.Name = "colPackageVersion";
            this.colPackageVersion.ReadOnly = true;
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
            this.colPackageRating.Visible = false;
            // 
            // colTagged
            // 
            this.colTagged.DataPropertyName = "Tagged";
            this.colTagged.HeaderText = "Tagged";
            this.colTagged.Name = "colTagged";
            this.colTagged.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTagged.Visible = false;
            this.colTagged.Width = 50;
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
            dataGridViewCellStyle9.NullValue = null;
            this.colIgnitionUpdated.DefaultCellStyle = dataGridViewCellStyle9;
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
            // colArtistTitleAlbumDate
            // 
            this.colArtistTitleAlbumDate.DataPropertyName = "ArtistTitleAlbumDate";
            this.colArtistTitleAlbumDate.HeaderText = "ArtistTitleAlbumDate";
            this.colArtistTitleAlbumDate.Name = "colArtistTitleAlbumDate";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // cmsDuplicates
            // 
            this.cmsDuplicates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsActions,
            this.toolStripSeparator1,
            this.cmsEdit,
            this.toolStripSeparator2,
            this.cmsShowSongInfo,
            this.cmsOpenLocation,
            this.cmsEnableDisable,
            this.cmsDelete,
            this.cmsMove});
            this.cmsDuplicates.Name = "contextMenuStrip_MainManager";
            this.cmsDuplicates.Size = new System.Drawing.Size(190, 170);
            this.cmsDuplicates.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsDuplicates_Closing);
            this.cmsDuplicates.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsDuplicates_ItemClicked);
            // 
            // cmsActions
            // 
            this.cmsActions.AssociatedEnumValue = null;
            this.cmsActions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsActions.Name = "cmsActions";
            this.cmsActions.RadioButtonGroupName = null;
            this.cmsActions.Size = new System.Drawing.Size(189, 22);
            this.cmsActions.Text = "Actions:";
            this.cmsActions.ToolTipText = resources.GetString("cmsActions.ToolTipText");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // cmsEdit
            // 
            this.cmsEdit.AssociatedEnumValue = null;
            this.cmsEdit.Image = ((System.Drawing.Image)(resources.GetObject("cmsEdit.Image")));
            this.cmsEdit.Name = "cmsEdit";
            this.cmsEdit.RadioButtonGroupName = null;
            this.cmsEdit.Size = new System.Drawing.Size(189, 22);
            this.cmsEdit.Text = "Edit Song Information";
            this.cmsEdit.ToolTipText = "<CAUTION> For Expert User Use Only\r\nData revisions have limited or no validation!" +
                "";
            this.cmsEdit.Click += new System.EventHandler(this.cmsEdit_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(186, 6);
            // 
            // cmsShowSongInfo
            // 
            this.cmsShowSongInfo.AssociatedEnumValue = null;
            this.cmsShowSongInfo.Image = ((System.Drawing.Image)(resources.GetObject("cmsShowSongInfo.Image")));
            this.cmsShowSongInfo.Name = "cmsShowSongInfo";
            this.cmsShowSongInfo.RadioButtonGroupName = null;
            this.cmsShowSongInfo.Size = new System.Drawing.Size(189, 22);
            this.cmsShowSongInfo.Text = "Show Song Info";
            this.cmsShowSongInfo.Click += new System.EventHandler(this.cmsShowSongInfo_Click);
            // 
            // cmsOpenLocation
            // 
            this.cmsOpenLocation.AssociatedEnumValue = null;
            this.cmsOpenLocation.Image = ((System.Drawing.Image)(resources.GetObject("cmsOpenLocation.Image")));
            this.cmsOpenLocation.Name = "cmsOpenLocation";
            this.cmsOpenLocation.RadioButtonGroupName = null;
            this.cmsOpenLocation.Size = new System.Drawing.Size(189, 22);
            this.cmsOpenLocation.Text = "Open Song Location";
            this.cmsOpenLocation.Click += new System.EventHandler(this.cmsOpenLocation_Click);
            // 
            // cmsEnableDisable
            // 
            this.cmsEnableDisable.Image = global::CustomsForgeSongManager.Properties.Resources.enabledisable;
            this.cmsEnableDisable.Name = "cmsEnableDisable";
            this.cmsEnableDisable.Size = new System.Drawing.Size(189, 22);
            this.cmsEnableDisable.Text = "Enable/Disable Songs";
            this.cmsEnableDisable.Click += new System.EventHandler(this.cmsEnableDisable_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.AssociatedEnumValue = null;
            this.cmsDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmsDelete.Image")));
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.RadioButtonGroupName = null;
            this.cmsDelete.Size = new System.Drawing.Size(189, 22);
            this.cmsDelete.Text = "Delete Songs";
            this.cmsDelete.ToolTipText = "WARNING\r\nDeletion can not be undone.\r\nSelect must be checked.";
            this.cmsDelete.Click += new System.EventHandler(this.cmsDelete_Click);
            // 
            // cmsMove
            // 
            this.cmsMove.AssociatedEnumValue = null;
            this.cmsMove.Image = global::CustomsForgeSongManager.Properties.Resources.export;
            this.cmsMove.Name = "cmsMove";
            this.cmsMove.RadioButtonGroupName = null;
            this.cmsMove.Size = new System.Drawing.Size(189, 22);
            this.cmsMove.Text = "Move Songs";
            this.cmsMove.ToolTipText = "Select must be checked.\r\nSee Log for moved file location.\r\n";
            this.cmsMove.Click += new System.EventHandler(this.cmsMove_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRescan,
            this.tsmiDuplicateType,
            this.tsmiHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(872, 24);
            this.menuStrip.TabIndex = 17;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmiRescan
            // 
            this.tsmiRescan.AssociatedEnumValue = null;
            this.tsmiRescan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRescanAll,
            this.tsmiShowEnabledDisabled});
            this.tsmiRescan.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescan.Image")));
            this.tsmiRescan.Name = "tsmiRescan";
            this.tsmiRescan.RadioButtonGroupName = null;
            this.tsmiRescan.Size = new System.Drawing.Size(70, 20);
            this.tsmiRescan.Text = "Rescan";
            // 
            // tsmiRescanAll
            // 
            this.tsmiRescanAll.AssociatedEnumValue = null;
            this.tsmiRescanAll.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescanAll.Image")));
            this.tsmiRescanAll.Name = "tsmiRescanAll";
            this.tsmiRescanAll.RadioButtonGroupName = null;
            this.tsmiRescanAll.Size = new System.Drawing.Size(196, 22);
            this.tsmiRescanAll.Text = "Show All Duplicates";
            this.tsmiRescanAll.ToolTipText = "Rescan songs for all duplcates";
            this.tsmiRescanAll.Click += new System.EventHandler(this.tsmiRescanAll_Click);
            // 
            // tsmiShowEnabledDisabled
            // 
            this.tsmiShowEnabledDisabled.AssociatedEnumValue = null;
            this.tsmiShowEnabledDisabled.Image = ((System.Drawing.Image)(resources.GetObject("tsmiShowEnabledDisabled.Image")));
            this.tsmiShowEnabledDisabled.Name = "tsmiShowEnabledDisabled";
            this.tsmiShowEnabledDisabled.RadioButtonGroupName = null;
            this.tsmiShowEnabledDisabled.Size = new System.Drawing.Size(196, 22);
            this.tsmiShowEnabledDisabled.Text = "Show Enabled/Disabled";
            this.tsmiShowEnabledDisabled.ToolTipText = "Toggles showing enabled or disabled duplicates";
            this.tsmiShowEnabledDisabled.Click += new System.EventHandler(this.tsmiShowEnabledDisabled_Click);
            // 
            // tsmiDuplicateType
            // 
            this.tsmiDuplicateType.AssociatedEnumValue = null;
            this.tsmiDuplicateType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDuplicateTypeDLCKeyATA,
            this.tsmiDuplicateTypePID});
            this.tsmiDuplicateType.Name = "tsmiDuplicateType";
            this.tsmiDuplicateType.RadioButtonGroupName = null;
            this.tsmiDuplicateType.Size = new System.Drawing.Size(90, 20);
            this.tsmiDuplicateType.Text = "Duplicate Type";
            this.tsmiDuplicateType.ToolTipText = "Show duplicates, select a type ...";
            // 
            // tsmiDuplicateTypeDLCKeyATA
            // 
            this.tsmiDuplicateTypeDLCKeyATA.AssociatedEnumValue = null;
            this.tsmiDuplicateTypeDLCKeyATA.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiDuplicateTypeDLCKeyATA.CheckOnClick = true;
            this.tsmiDuplicateTypeDLCKeyATA.Name = "tsmiDuplicateTypeDLCKeyATA";
            this.tsmiDuplicateTypeDLCKeyATA.RadioButtonGroupName = null;
            this.tsmiDuplicateTypeDLCKeyATA.Size = new System.Drawing.Size(283, 22);
            this.tsmiDuplicateTypeDLCKeyATA.Text = "DLC Key or Artist, Title, Album Duplicates";
            this.tsmiDuplicateTypeDLCKeyATA.ToolTipText = "Show CDLC that have duplicate DLC Key, or\r\nduplicate Artist, Title, and Album inf" +
                "ormation.\r\n\r\nNOTE: The search is not case sensitive and \r\nignores any non-alphnu" +
                "meric characters.";
            this.tsmiDuplicateTypeDLCKeyATA.Click += new System.EventHandler(this.tsmiDuplicateTypeDLCKeyATA_Click);
            // 
            // tsmiDuplicateTypePID
            // 
            this.tsmiDuplicateTypePID.AssociatedEnumValue = null;
            this.tsmiDuplicateTypePID.CheckMarkDisplayStyle = CustomControls.CheckMarkDisplayStyle.RadioButton;
            this.tsmiDuplicateTypePID.CheckOnClick = true;
            this.tsmiDuplicateTypePID.Name = "tsmiDuplicateTypePID";
            this.tsmiDuplicateTypePID.RadioButtonGroupName = null;
            this.tsmiDuplicateTypePID.Size = new System.Drawing.Size(283, 22);
            this.tsmiDuplicateTypePID.Text = "Persistent ID Duplicates (Game Crashers)";
            this.tsmiDuplicateTypePID.ToolTipText = "Show CDLC that are reusing Persistent IDs.\r\nThese cause in-game hangs and need to" +
                " be deleted!";
            this.tsmiDuplicateTypePID.Click += new System.EventHandler(this.tsmiDuplicateTypePID_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.AssociatedEnumValue = null;
            this.tsmiHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelp.Image")));
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.RadioButtonGroupName = null;
            this.tsmiHelp.Size = new System.Drawing.Size(56, 20);
            this.tsmiHelp.Text = "Help";
            this.tsmiHelp.Click += new System.EventHandler(this.tsmiHelp_Click);
            // 
            // Duplicates
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.gbResults);
            this.Name = "Duplicates";
            this.Size = new System.Drawing.Size(872, 525);
            this.Resize += new System.EventHandler(this.Duplicates_Resize);
            this.cmsDuplicateColumns.ResumeLayout(false);
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuplicates)).EndInit();
            this.cmsDuplicates.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextMenuStrip cmsDuplicateColumns;
        private ToolStripMenuItem cmsColumnHeaders;
        private GroupBox gbResults;
        private TextBox txtNoDuplicates;
        private RADataGridView dgvDuplicates;
        private ToolTip toolTip;
        private CheckBox chkIncludeSubfolders;
        private LinkLabel lnkSelectByATAD;
        private ContextMenuStrip cmsDuplicates;
        private CustomControls.ToolStripEnhancedMenuItem cmsOpenLocation;
        private ToolStripMenuItem cmsEnableDisable;
        private CustomControls.ToolStripEnhancedMenuItem cmsDelete;
        private CustomControls.ToolStripEnhancedMenuItem cmsMove;
        private MenuStrip menuStrip;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescan;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanAll;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelp;
        private LinkLabel lnkLblSelectAll;
        private LinkLabel lnklblToggle;
        private CustomControls.ToolStripEnhancedMenuItem cmsActions;
        private ToolStripSeparator toolStripSeparator1;
        private CustomControls.ToolStripEnhancedMenuItem cmsShowSongInfo;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDuplicateType;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDuplicateTypeDLCKeyATA;
        private CustomControls.ToolStripEnhancedMenuItem tsmiDuplicateTypePID;
        private CustomControls.ToolStripEnhancedMenuItem tsmiShowEnabledDisabled;
        private DataGridViewAutoFilterTextBoxColumn colPID;
        private DataGridViewAutoFilterTextBoxColumn colPIDArrangement;
        private DataGridViewAutoFilterTextBoxColumn colKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewAutoFilterTextBoxColumn colSongLength;
        private DataGridViewAutoFilterTextBoxColumn colAvgTempo;
        private DataGridViewAutoFilterTextBoxColumn colLastConversionDateTime;
        private DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colFileDate;
        private DataGridViewAutoFilterTextBoxColumn colFileSize;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewAutoFilterTextBoxColumn colUpdated;
        private DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageAuthor;
        private DataGridViewAutoFilterTextBoxColumn colPackageVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageComment;
        private DataGridViewRatingColumn colPackageRating;
        private DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionID;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionUpdated;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionVersion;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionAuthor;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbumDate;
        private CustomControls.ToolStripEnhancedMenuItem cmsEdit;
        private ToolStripSeparator toolStripSeparator2;
        private LinkLabel lnkSelectByToolkitVersion;
    }
}