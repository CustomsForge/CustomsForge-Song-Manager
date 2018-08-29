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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Duplicates));
            this.cmsDuplicateColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsColumnHeaders = new System.Windows.Forms.ToolStripMenuItem();
            this.linkSelectOlderVersions = new System.Windows.Forms.LinkLabel();
            this.chkSubFolders = new System.Windows.Forms.CheckBox();
            this.lnkPersistentId = new System.Windows.Forms.LinkLabel();
            this.gbResults = new System.Windows.Forms.GroupBox();
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
            this.colSongLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvgTempo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
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
            this.colArtistTitleAlbumDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsDuplicates = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsActions = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsShowSongInfo = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsOpenLocation = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsEnableDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelete = new CustomControls.ToolStripEnhancedMenuItem();
            this.cmsMove = new CustomControls.ToolStripEnhancedMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiRescan = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanAll = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanEnabledDisabled = new CustomControls.ToolStripEnhancedMenuItem();
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
            // linkSelectOlderVersions
            // 
            this.linkSelectOlderVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkSelectOlderVersions.AutoSize = true;
            this.linkSelectOlderVersions.ForeColor = System.Drawing.Color.Black;
            this.linkSelectOlderVersions.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.linkSelectOlderVersions.Location = new System.Drawing.Point(216, 492);
            this.linkSelectOlderVersions.Name = "linkSelectOlderVersions";
            this.linkSelectOlderVersions.Size = new System.Drawing.Size(122, 13);
            this.linkSelectOlderVersions.TabIndex = 26;
            this.linkSelectOlderVersions.TabStop = true;
            this.linkSelectOlderVersions.Text = "Select All Older Versions";
            this.toolTip.SetToolTip(this.linkSelectOlderVersions, "Select all duplicate songs excluding the newest versions ");
            this.linkSelectOlderVersions.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.linkSelectOlderVersions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectOlderVersions_LinkClicked);
            // 
            // chkSubFolders
            // 
            this.chkSubFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSubFolders.AutoSize = true;
            this.chkSubFolders.Checked = true;
            this.chkSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubFolders.Location = new System.Drawing.Point(522, 492);
            this.chkSubFolders.Name = "chkSubFolders";
            this.chkSubFolders.Size = new System.Drawing.Size(114, 17);
            this.chkSubFolders.TabIndex = 25;
            this.chkSubFolders.Text = "Include Subfolders";
            this.chkSubFolders.UseVisualStyleBackColor = true;
            this.chkSubFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkSubFolders_MouseUp);
            // 
            // lnkPersistentId
            // 
            this.lnkPersistentId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPersistentId.AutoSize = true;
            this.lnkPersistentId.ForeColor = System.Drawing.Color.Black;
            this.lnkPersistentId.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.Location = new System.Drawing.Point(351, 492);
            this.lnkPersistentId.Name = "lnkPersistentId";
            this.lnkPersistentId.Size = new System.Drawing.Size(142, 13);
            this.lnkPersistentId.TabIndex = 18;
            this.lnkPersistentId.TabStop = true;
            this.lnkPersistentId.Text = "Show SongInfo/PersistentID";
            this.toolTip.SetToolTip(this.lnkPersistentId, "Show the bad boy CDLC that are reusing Persistent IDs\r\nThese causing game hangs a" +
                    "nd need to be deleted.");
            this.lnkPersistentId.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPersistentId_LinkClicked);
            // 
            // gbResults
            // 
            this.gbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResults.Controls.Add(this.lnkLblSelectAll);
            this.gbResults.Controls.Add(this.lnklblToggle);
            this.gbResults.Controls.Add(this.chkSubFolders);
            this.gbResults.Controls.Add(this.linkSelectOlderVersions);
            this.gbResults.Controls.Add(this.lnkPersistentId);
            this.gbResults.Controls.Add(this.txtNoDuplicates);
            this.gbResults.Controls.Add(this.dgvDuplicates);
            this.gbResults.Location = new System.Drawing.Point(4, 3);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(864, 517);
            this.gbResults.TabIndex = 16;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results Grid:";
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
            this.toolTip.SetToolTip(this.lnkLblSelectAll, "ODLC are not selectable");
            this.lnkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSelectAll_LinkClicked);
            // 
            // lnklblToggle
            // 
            this.lnklblToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnklblToggle.AutoSize = true;
            this.lnklblToggle.ForeColor = System.Drawing.Color.Black;
            this.lnklblToggle.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnklblToggle.Location = new System.Drawing.Point(116, 492);
            this.lnklblToggle.Name = "lnklblToggle";
            this.lnklblToggle.Size = new System.Drawing.Size(87, 13);
            this.lnklblToggle.TabIndex = 28;
            this.lnklblToggle.TabStop = true;
            this.lnklblToggle.Text = "Toggle Selection";
            this.lnklblToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.lnklblToggle, "ODLC are not toggleable");
            this.lnklblToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToggle_LinkClicked);
            // 
            // txtNoDuplicates
            // 
            this.txtNoDuplicates.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtNoDuplicates.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDuplicates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDuplicates.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
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
            this.colAppID,
            this.colFileName,
            this.colFilePath,
            this.colRepairStatus,
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
            this.colArtistTitleAlbumDate});
            this.dgvDuplicates.Location = new System.Drawing.Point(6, 19);
            this.dgvDuplicates.Name = "dgvDuplicates";
            this.dgvDuplicates.RowHeadersVisible = false;
            this.dgvDuplicates.Size = new System.Drawing.Size(852, 460);
            this.dgvDuplicates.TabIndex = 17;
            this.dgvDuplicates.Tag = "Duplicates";
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
            this.colKey.Visible = false;
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
            this.colSelect.Width = 43;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            this.colSongYear.Width = 50;
            // 
            // colSongLength
            // 
            this.colSongLength.DataPropertyName = "SongLength";
            dataGridViewCellStyle21.Format = "N2";
            dataGridViewCellStyle21.NullValue = null;
            this.colSongLength.DefaultCellStyle = dataGridViewCellStyle21;
            this.colSongLength.HeaderText = "Length Seconds";
            this.colSongLength.Name = "colSongLength";
            this.colSongLength.ReadOnly = true;
            this.colSongLength.Visible = false;
            // 
            // colAvgTempo
            // 
            this.colAvgTempo.DataPropertyName = "SongAverageTempo";
            dataGridViewCellStyle22.Format = "N2";
            dataGridViewCellStyle22.NullValue = null;
            this.colAvgTempo.DefaultCellStyle = dataGridViewCellStyle22;
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
            this.colAppID.Visible = false;
            this.colAppID.Width = 80;
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            // 
            // colFilePath
            // 
            this.colFilePath.DataPropertyName = "FilePath";
            this.colFilePath.HeaderText = "File Path";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilePath.Width = 50;
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
            dataGridViewCellStyle23.NullValue = null;
            this.colUpdated.DefaultCellStyle = dataGridViewCellStyle23;
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
            this.colToolkitVersion.Visible = false;
            this.colToolkitVersion.Width = 50;
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
            dataGridViewCellStyle24.NullValue = null;
            this.colIgnitionUpdated.DefaultCellStyle = dataGridViewCellStyle24;
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
            this.cmsShowSongInfo,
            this.cmsOpenLocation,
            this.cmsEnableDisable,
            this.cmsDelete,
            this.cmsMove});
            this.cmsDuplicates.Name = "contextMenuStrip_MainManager";
            this.cmsDuplicates.Size = new System.Drawing.Size(188, 142);
            this.cmsDuplicates.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsDuplicates_Closing);
            this.cmsDuplicates.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsDuplicates_ItemClicked);
            // 
            // cmsActions
            // 
            this.cmsActions.AssociatedEnumValue = null;
            this.cmsActions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsActions.Name = "cmsActions";
            this.cmsActions.RadioButtonGroupName = null;
            this.cmsActions.Size = new System.Drawing.Size(187, 22);
            this.cmsActions.Text = "Actions:";
            this.cmsActions.ToolTipText = resources.GetString("cmsActions.ToolTipText");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // cmsShowSongInfo
            // 
            this.cmsShowSongInfo.AssociatedEnumValue = null;
            this.cmsShowSongInfo.Image = ((System.Drawing.Image)(resources.GetObject("cmsShowSongInfo.Image")));
            this.cmsShowSongInfo.Name = "cmsShowSongInfo";
            this.cmsShowSongInfo.RadioButtonGroupName = null;
            this.cmsShowSongInfo.Size = new System.Drawing.Size(187, 22);
            this.cmsShowSongInfo.Text = "Show Song Info";
            this.cmsShowSongInfo.Click += new System.EventHandler(this.cmsShowSongInfo_Click);
            // 
            // cmsOpenLocation
            // 
            this.cmsOpenLocation.AssociatedEnumValue = null;
            this.cmsOpenLocation.Image = ((System.Drawing.Image)(resources.GetObject("cmsOpenLocation.Image")));
            this.cmsOpenLocation.Name = "cmsOpenLocation";
            this.cmsOpenLocation.RadioButtonGroupName = null;
            this.cmsOpenLocation.Size = new System.Drawing.Size(187, 22);
            this.cmsOpenLocation.Text = "Open Song Location";
            this.cmsOpenLocation.Click += new System.EventHandler(this.cmsOpenLocation_Click);
            // 
            // cmsEnableDisable
            // 
            this.cmsEnableDisable.Image = global::CustomsForgeSongManager.Properties.Resources.disable;
            this.cmsEnableDisable.Name = "cmsEnableDisable";
            this.cmsEnableDisable.Size = new System.Drawing.Size(187, 22);
            this.cmsEnableDisable.Text = "Enable/Disable Songs";
            this.cmsEnableDisable.Click += new System.EventHandler(this.cmsEnableDisable_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.AssociatedEnumValue = null;
            this.cmsDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmsDelete.Image")));
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.RadioButtonGroupName = null;
            this.cmsDelete.Size = new System.Drawing.Size(187, 22);
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
            this.cmsMove.Size = new System.Drawing.Size(187, 22);
            this.cmsMove.Text = "Move Songs";
            this.cmsMove.ToolTipText = "Select must be checked.\r\nSee Log for moved file location.\r\n";
            this.cmsMove.Click += new System.EventHandler(this.cmsMove_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRescan,
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
            this.tsmiRescanEnabledDisabled});
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
            // tsmiRescanEnabledDisabled
            // 
            this.tsmiRescanEnabledDisabled.AssociatedEnumValue = null;
            this.tsmiRescanEnabledDisabled.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRescanEnabledDisabled.Image")));
            this.tsmiRescanEnabledDisabled.Name = "tsmiRescanEnabledDisabled";
            this.tsmiRescanEnabledDisabled.RadioButtonGroupName = null;
            this.tsmiRescanEnabledDisabled.Size = new System.Drawing.Size(196, 22);
            this.tsmiRescanEnabledDisabled.Text = "Show Enabled/Disabled";
            this.tsmiRescanEnabledDisabled.ToolTipText = "Toggles showing enabled or disabled duplicates";
            this.tsmiRescanEnabledDisabled.Click += new System.EventHandler(this.tsmiRescanEnabledDisabled_Click);
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
        private LinkLabel lnkPersistentId;
        private TextBox txtNoDuplicates;
        private RADataGridView dgvDuplicates;
        private ToolTip toolTip;
        private CheckBox chkSubFolders;
        private LinkLabel linkSelectOlderVersions;
        private ContextMenuStrip cmsDuplicates;
        private CustomControls.ToolStripEnhancedMenuItem cmsOpenLocation;
        private ToolStripMenuItem cmsEnableDisable;
        private CustomControls.ToolStripEnhancedMenuItem cmsDelete;
        private CustomControls.ToolStripEnhancedMenuItem cmsMove;
        private MenuStrip menuStrip;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescan;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanEnabledDisabled;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanAll;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelp;
        private LinkLabel lnkLblSelectAll;
        private LinkLabel lnklblToggle;
        private CustomControls.ToolStripEnhancedMenuItem cmsActions;
        private ToolStripSeparator toolStripSeparator1;
        private CustomControls.ToolStripEnhancedMenuItem cmsShowSongInfo;
        private DataGridViewAutoFilterTextBoxColumn colPID;
        private DataGridViewAutoFilterTextBoxColumn colPIDArrangement;
        private DataGridViewAutoFilterTextBoxColumn colKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewTextBoxColumn colSongLength;
        private DataGridViewTextBoxColumn colAvgTempo;
        private DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewAutoFilterTextBoxColumn colCharter;
        private DataGridViewTextBoxColumn colUpdated;
        private DataGridViewTextBoxColumn colVersion;
        private DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewTextBoxColumn colIgnitionID;
        private DataGridViewTextBoxColumn colIgnitionUpdated;
        private DataGridViewTextBoxColumn colIgnitionVersion;
        private DataGridViewTextBoxColumn colIgnitionAuthor;
        private DataGridViewTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbumDate;
    }
}
