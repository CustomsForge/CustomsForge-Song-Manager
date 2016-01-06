using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.DataGridTools;

namespace CustomsForgeManager.UControls
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
            this.cmsDuplicate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exploreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.panelActions = new System.Windows.Forms.Panel();
            this.chkSubFolders = new System.Windows.Forms.CheckBox();
            this.lnkPersistentId = new System.Windows.Forms.LinkLabel();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnDeleteSong = new System.Windows.Forms.Button();
            this.btnEnableDisable = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.txtNoDuplicates = new System.Windows.Forms.TextBox();
            this.dgvDuplicates = new CustomsForgeManager.CustomsForgeManagerLib.DataGridTools.RADataGridView();
            this.colPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPIDArrangement = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmsDuplicate.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuplicates)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsDuplicate
            // 
            this.cmsDuplicate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exploreToolStripMenuItem});
            this.cmsDuplicate.Name = "cmsDuplicate";
            this.cmsDuplicate.Size = new System.Drawing.Size(122, 26);
            // 
            // exploreToolStripMenuItem
            // 
            this.exploreToolStripMenuItem.Name = "exploreToolStripMenuItem";
            this.exploreToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.exploreToolStripMenuItem.Text = "Explore";
            this.exploreToolStripMenuItem.Click += new System.EventHandler(this.exploreToolStripMenuItem_Click);
            // 
            // gbActions
            // 
            this.gbActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActions.Controls.Add(this.panelActions);
            this.gbActions.Location = new System.Drawing.Point(5, 457);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(864, 60);
            this.gbActions.TabIndex = 14;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions:";
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.chkSubFolders);
            this.panelActions.Controls.Add(this.lnkPersistentId);
            this.panelActions.Controls.Add(this.btnMove);
            this.panelActions.Controls.Add(this.btnDeleteSong);
            this.panelActions.Controls.Add(this.btnEnableDisable);
            this.panelActions.Controls.Add(this.btnRescan);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(3, 16);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(858, 41);
            this.panelActions.TabIndex = 3;
            // 
            // chkSubFolders
            // 
            this.chkSubFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSubFolders.AutoSize = true;
            this.chkSubFolders.Checked = true;
            this.chkSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubFolders.Location = new System.Drawing.Point(523, 13);
            this.chkSubFolders.Name = "chkSubFolders";
            this.chkSubFolders.Size = new System.Drawing.Size(152, 17);
            this.chkSubFolders.TabIndex = 25;
            this.chkSubFolders.Text = "Include Subfolders/Setlists";
            this.chkSubFolders.UseVisualStyleBackColor = true;
            this.chkSubFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkSubFolders_MouseUp);
            // 
            // lnkPersistentId
            // 
            this.lnkPersistentId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkPersistentId.AutoSize = true;
            this.lnkPersistentId.ForeColor = System.Drawing.Color.Black;
            this.lnkPersistentId.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.Location = new System.Drawing.Point(698, 14);
            this.lnkPersistentId.Name = "lnkPersistentId";
            this.lnkPersistentId.Size = new System.Drawing.Size(145, 13);
            this.lnkPersistentId.TabIndex = 18;
            this.lnkPersistentId.TabStop = true;
            this.lnkPersistentId.Text = "Select SongInfo/PersistentID";
            this.toolTip.SetToolTip(this.lnkPersistentId, "Show the bad boy CDLC that are reusing Persistent IDs\r\nThese causing game hangs a" +
                    "nd need to be deleted.");
            this.lnkPersistentId.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPersistentId_LinkClicked);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMove.Image = global::CustomsForgeManager.Properties.Resources.export;
            this.btnMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMove.Location = new System.Drawing.Point(396, 7);
            this.btnMove.Name = "btnMove";
            this.btnMove.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnMove.Size = new System.Drawing.Size(112, 27);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "Move Selected";
            this.btnMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMove.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSong
            // 
            this.btnDeleteSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSong.Image = global::CustomsForgeManager.Properties.Resources.delete;
            this.btnDeleteSong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSong.Location = new System.Drawing.Point(266, 7);
            this.btnDeleteSong.Name = "btnDeleteSong";
            this.btnDeleteSong.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnDeleteSong.Size = new System.Drawing.Size(117, 27);
            this.btnDeleteSong.TabIndex = 11;
            this.btnDeleteSong.Text = "Delete Selected";
            this.btnDeleteSong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteSong.UseVisualStyleBackColor = true;
            // 
            // btnEnableDisable
            // 
            this.btnEnableDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnableDisable.Image = global::CustomsForgeManager.Properties.Resources.disable;
            this.btnEnableDisable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnableDisable.Location = new System.Drawing.Point(99, 6);
            this.btnEnableDisable.Name = "btnEnableDisable";
            this.btnEnableDisable.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnEnableDisable.Size = new System.Drawing.Size(154, 29);
            this.btnEnableDisable.TabIndex = 14;
            this.btnEnableDisable.Text = "Enable/Disable Selected";
            this.btnEnableDisable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnableDisable.UseVisualStyleBackColor = true;
            this.btnEnableDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRescan.Image = ((System.Drawing.Image)(resources.GetObject("btnRescan.Image")));
            this.btnRescan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRescan.Location = new System.Drawing.Point(8, 6);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.btnRescan.Size = new System.Drawing.Size(78, 29);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // gbResults
            // 
            this.gbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResults.Controls.Add(this.txtNoDuplicates);
            this.gbResults.Controls.Add(this.dgvDuplicates);
            this.gbResults.Location = new System.Drawing.Point(4, 3);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(864, 454);
            this.gbResults.TabIndex = 16;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results Grid:";
            // 
            // txtNoDuplicates
            // 
            this.txtNoDuplicates.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoDuplicates.Location = new System.Drawing.Point(279, 157);
            this.txtNoDuplicates.Multiline = true;
            this.txtNoDuplicates.Name = "txtNoDuplicates";
            this.txtNoDuplicates.Size = new System.Drawing.Size(307, 104);
            this.txtNoDuplicates.TabIndex = 16;
            this.txtNoDuplicates.Text = "\r\nGood News ...\r\nNo Duplicates Found";
            this.txtNoDuplicates.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvDuplicates
            // 
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
            this.colAppID,
            this.colPath,
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
            this.colArtistTitleAlbum});
            this.dgvDuplicates.Location = new System.Drawing.Point(6, 19);
            this.dgvDuplicates.Name = "dgvDuplicates";
            this.dgvDuplicates.RowHeadersVisible = false;
            this.dgvDuplicates.Size = new System.Drawing.Size(852, 429);
            this.dgvDuplicates.TabIndex = 17;
            this.dgvDuplicates.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDuplicates_CellDoubleClick);
            this.dgvDuplicates.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDups_CellFormatting);
            this.dgvDuplicates.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDuplicates_CellMouseUp);
            this.dgvDuplicates.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDuplicates_DataBindingComplete);
            this.dgvDuplicates.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDups_DataError);
            this.dgvDuplicates.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDuplicates_Paint);
            this.dgvDuplicates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDuplicates_KeyDown);
            // 
            // colPID
            // 
            this.colPID.DataPropertyName = "PID";
            this.colPID.HeaderText = "Persistent ID";
            this.colPID.Name = "colPID";
            // 
            // colPIDArrangement
            // 
            this.colPIDArrangement.DataPropertyName = "PIDArrangement";
            this.colPIDArrangement.HeaderText = "Arrangement";
            this.colPIDArrangement.Name = "colPIDArrangement";
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
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 50;
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
            dataGridViewCellStyle5.NullValue = null;
            this.colUpdated.DefaultCellStyle = dataGridViewCellStyle5;
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
            dataGridViewCellStyle6.NullValue = null;
            this.colIgnitionUpdated.DefaultCellStyle = dataGridViewCellStyle6;
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
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 200;
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 200;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // Duplicates
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.gbActions);
            this.Name = "Duplicates";
            this.Size = new System.Drawing.Size(872, 525);
            this.Resize += new System.EventHandler(this.Duplicates_Resize);
            this.cmsDuplicate.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuplicates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteSong;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnEnableDisable;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnMove;
        private ContextMenuStrip cmsDuplicate;
        private ToolStripMenuItem exploreToolStripMenuItem;
        private GroupBox gbResults;
        private LinkLabel lnkPersistentId;
        private TextBox txtNoDuplicates;
        private RADataGridView dgvDuplicates;
        private ToolTip toolTip;
        private DataGridViewTextBoxColumn colPID;
        private DataGridViewTextBoxColumn colPIDArrangement;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewTextBoxColumn colSongLength;
        private DataGridViewTextBoxColumn colAvgTempo;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewTextBoxColumn colPath;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colStatus;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colCharter;
        private DataGridViewTextBoxColumn colUpdated;
        private DataGridViewTextBoxColumn colVersion;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewTextBoxColumn colIgnitionID;
        private DataGridViewTextBoxColumn colIgnitionUpdated;
        private DataGridViewTextBoxColumn colIgnitionVersion;
        private DataGridViewTextBoxColumn colIgnitionAuthor;
        private DataGridViewTextBoxColumn colArtistTitleAlbum;
        private CheckBox chkSubFolders;
    }
}
