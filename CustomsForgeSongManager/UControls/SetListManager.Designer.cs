using System.Windows.Forms;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class SetlistManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gbSetlistSongs = new System.Windows.Forms.GroupBox();
            this.dgvSetlistSongs = new DataGridViewTools.RADataGridView();
            this.colSetlistSongsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistSongsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsArrangements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsArtistTitleAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSetlist = new System.Windows.Forms.GroupBox();
            this.dgvSetlists = new DataGridViewTools.RADataGridView();
            this.colSetlistSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbButtons = new System.Windows.Forms.GroupBox();
            this.chkProtectODLC = new System.Windows.Forms.CheckBox();
            this.btnCombineSetlists = new System.Windows.Forms.Button();
            this.chkIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreateSetlist = new System.Windows.Forms.Button();
            this.btnEnDiSetlist = new System.Windows.Forms.Button();
            this.btnDeleteSetlist = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.gbSongs = new System.Windows.Forms.GroupBox();
            this.dgvSetlistMaster = new DataGridViewTools.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangements = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDD = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.gbSongPacks = new System.Windows.Forms.GroupBox();
            this.dgvSongPacks = new DataGridViewTools.RADataGridView();
            this.colSongPackSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSongPackEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongPackPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongPackFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.btnRunRSWithSetlist = new System.Windows.Forms.Button();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.lnkSetlistMgrHelp = new System.Windows.Forms.LinkLabel();
            this.cmsSetlistManager = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsActions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSelectAllNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsShow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEnableDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbSetlistSongs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistSongs)).BeginInit();
            this.gbSetlist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).BeginInit();
            this.gbButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.gbSongs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistMaster)).BeginInit();
            this.gbSongPacks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).BeginInit();
            this.gbSearch.SuspendLayout();
            this.cmsSetlistManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 490);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.gbSetlistSongs, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbSetlist, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbButtons, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(988, 226);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // gbSetlistSongs
            // 
            this.gbSetlistSongs.Controls.Add(this.dgvSetlistSongs);
            this.gbSetlistSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSetlistSongs.Location = new System.Drawing.Point(454, 3);
            this.gbSetlistSongs.Name = "gbSetlistSongs";
            this.gbSetlistSongs.Size = new System.Drawing.Size(531, 220);
            this.gbSetlistSongs.TabIndex = 5;
            this.gbSetlistSongs.TabStop = false;
            this.gbSetlistSongs.Text = "Setlists Songs";
            // 
            // dgvSetlistSongs
            // 
            this.dgvSetlistSongs.AllowUserToAddRows = false;
            this.dgvSetlistSongs.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSetlistSongs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSetlistSongs.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetlistSongs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSetlistSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSetlistSongsSelect,
            this.colSetlistSongsEnabled,
            this.colSetlistSongsSongArtist,
            this.colSetlistSongsSongTitle,
            this.colSetlistSongsSongAlbum,
            this.colSetlistSongsArrangements,
            this.colSetlistSongsSongTuning,
            this.colSetlistSongsDD,
            this.colSetlistSongsPath,
            this.colSetlistSongsArtistTitleAlbum,
            this.colSetlistFileName});
            this.dgvSetlistSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSetlistSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSetlistSongs.Location = new System.Drawing.Point(3, 16);
            this.dgvSetlistSongs.Name = "dgvSetlistSongs";
            this.dgvSetlistSongs.RowHeadersVisible = false;
            this.dgvSetlistSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSetlistSongs.Size = new System.Drawing.Size(525, 201);
            this.dgvSetlistSongs.TabIndex = 34;
            this.toolTip.SetToolTip(this.dgvSetlistSongs, "Left mouse click on row to select or check \'Select\' checkbox\r\nRight mouse click o" +
                    "n row to show file operation options");
            this.dgvSetlistSongs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrent_CellFormatting);
            this.dgvSetlistSongs.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrent_CellMouseUp);
            // 
            // colSetlistSongsSelect
            // 
            this.colSetlistSongsSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSetlistSongsSelect.DataPropertyName = "Selected";
            this.colSetlistSongsSelect.FalseValue = "false";
            this.colSetlistSongsSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSetlistSongsSelect.HeaderText = "Select";
            this.colSetlistSongsSelect.IndeterminateValue = "false";
            this.colSetlistSongsSelect.Name = "colSetlistSongsSelect";
            this.colSetlistSongsSelect.TrueValue = "true";
            this.colSetlistSongsSelect.Width = 43;
            // 
            // colSetlistSongsEnabled
            // 
            this.colSetlistSongsEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSetlistSongsEnabled.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSetlistSongsEnabled.HeaderText = "Enabled";
            this.colSetlistSongsEnabled.Name = "colSetlistSongsEnabled";
            this.colSetlistSongsEnabled.ReadOnly = true;
            this.colSetlistSongsEnabled.Width = 50;
            // 
            // colSetlistSongsSongArtist
            // 
            this.colSetlistSongsSongArtist.DataPropertyName = "Artist";
            this.colSetlistSongsSongArtist.HeaderText = "Artist";
            this.colSetlistSongsSongArtist.Name = "colSetlistSongsSongArtist";
            this.colSetlistSongsSongArtist.ReadOnly = true;
            // 
            // colSetlistSongsSongTitle
            // 
            this.colSetlistSongsSongTitle.DataPropertyName = "Title";
            this.colSetlistSongsSongTitle.HeaderText = "Song Title";
            this.colSetlistSongsSongTitle.Name = "colSetlistSongsSongTitle";
            this.colSetlistSongsSongTitle.ReadOnly = true;
            // 
            // colSetlistSongsSongAlbum
            // 
            this.colSetlistSongsSongAlbum.DataPropertyName = "Album";
            this.colSetlistSongsSongAlbum.HeaderText = "Album";
            this.colSetlistSongsSongAlbum.Name = "colSetlistSongsSongAlbum";
            this.colSetlistSongsSongAlbum.ReadOnly = true;
            // 
            // colSetlistSongsArrangements
            // 
            this.colSetlistSongsArrangements.DataPropertyName = "Arrangements";
            this.colSetlistSongsArrangements.HeaderText = "Arrangements";
            this.colSetlistSongsArrangements.Name = "colSetlistSongsArrangements";
            this.colSetlistSongsArrangements.ReadOnly = true;
            this.colSetlistSongsArrangements.Width = 50;
            // 
            // colSetlistSongsSongTuning
            // 
            this.colSetlistSongsSongTuning.DataPropertyName = "Tuning";
            this.colSetlistSongsSongTuning.HeaderText = "Tuning";
            this.colSetlistSongsSongTuning.Name = "colSetlistSongsSongTuning";
            this.colSetlistSongsSongTuning.ReadOnly = true;
            this.colSetlistSongsSongTuning.Width = 70;
            // 
            // colSetlistSongsDD
            // 
            this.colSetlistSongsDD.DataPropertyName = "DD";
            this.colSetlistSongsDD.HeaderText = "DD";
            this.colSetlistSongsDD.Name = "colSetlistSongsDD";
            this.colSetlistSongsDD.ReadOnly = true;
            this.colSetlistSongsDD.Width = 50;
            // 
            // colSetlistSongsPath
            // 
            this.colSetlistSongsPath.DataPropertyName = "FilePath";
            this.colSetlistSongsPath.HeaderText = "File Path";
            this.colSetlistSongsPath.Name = "colSetlistSongsPath";
            this.colSetlistSongsPath.ReadOnly = true;
            this.colSetlistSongsPath.Width = 350;
            // 
            // colSetlistSongsArtistTitleAlbum
            // 
            this.colSetlistSongsArtistTitleAlbum.DataPropertyName = "ArtistTitleAlbum";
            this.colSetlistSongsArtistTitleAlbum.HeaderText = "ArtistTitleAlbum";
            this.colSetlistSongsArtistTitleAlbum.Name = "colSetlistSongsArtistTitleAlbum";
            this.colSetlistSongsArtistTitleAlbum.ReadOnly = true;
            this.colSetlistSongsArtistTitleAlbum.Visible = false;
            // 
            // colSetlistFileName
            // 
            this.colSetlistFileName.DataPropertyName = "FileName";
            this.colSetlistFileName.HeaderText = "File Name";
            this.colSetlistFileName.Name = "colSetlistFileName";
            this.colSetlistFileName.ReadOnly = true;
            this.colSetlistFileName.Width = 140;
            // 
            // gbSetlist
            // 
            this.gbSetlist.Controls.Add(this.dgvSetlists);
            this.gbSetlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSetlist.Location = new System.Drawing.Point(3, 3);
            this.gbSetlist.Name = "gbSetlist";
            this.gbSetlist.Size = new System.Drawing.Size(202, 220);
            this.gbSetlist.TabIndex = 4;
            this.gbSetlist.TabStop = false;
            this.gbSetlist.Text = "Setlists";
            // 
            // dgvSetlists
            // 
            this.dgvSetlists.AllowUserToAddRows = false;
            this.dgvSetlists.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSetlists.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSetlists.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetlists.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSetlists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSetlistSelect,
            this.colSetlistEnabled,
            this.colSetlistName});
            this.dgvSetlists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSetlists.Location = new System.Drawing.Point(3, 16);
            this.dgvSetlists.Name = "dgvSetlists";
            this.dgvSetlists.RowHeadersVisible = false;
            this.dgvSetlists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSetlists.Size = new System.Drawing.Size(196, 201);
            this.dgvSetlists.TabIndex = 35;
            this.toolTip.SetToolTip(this.dgvSetlists, "Left mouse click on row to select or check \'Select\' checkbox");
            this.dgvSetlists.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlists_CellMouseUp);
            this.dgvSetlists.SelectionChanged += new System.EventHandler(this.dgvSetlists_SelectionChanged);
            // 
            // colSetlistSelect
            // 
            this.colSetlistSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSetlistSelect.FalseValue = "false";
            this.colSetlistSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSetlistSelect.HeaderText = "Select";
            this.colSetlistSelect.IndeterminateValue = "false";
            this.colSetlistSelect.Name = "colSetlistSelect";
            this.colSetlistSelect.TrueValue = "true";
            this.colSetlistSelect.Width = 43;
            // 
            // colSetlistEnabled
            // 
            this.colSetlistEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSetlistEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSetlistEnabled.DefaultCellStyle = dataGridViewCellStyle6;
            this.colSetlistEnabled.HeaderText = "Enabled";
            this.colSetlistEnabled.Name = "colSetlistEnabled";
            this.colSetlistEnabled.ReadOnly = true;
            this.colSetlistEnabled.Width = 50;
            // 
            // colSetlistName
            // 
            this.colSetlistName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSetlistName.DataPropertyName = "SetlistName";
            this.colSetlistName.HeaderText = "Setlist Name";
            this.colSetlistName.Name = "colSetlistName";
            this.colSetlistName.ReadOnly = true;
            // 
            // gbButtons
            // 
            this.gbButtons.Controls.Add(this.chkProtectODLC);
            this.gbButtons.Controls.Add(this.btnCombineSetlists);
            this.gbButtons.Controls.Add(this.chkIncludeSubfolders);
            this.gbButtons.Controls.Add(this.label2);
            this.gbButtons.Controls.Add(this.btnCreateSetlist);
            this.gbButtons.Controls.Add(this.btnEnDiSetlist);
            this.gbButtons.Controls.Add(this.btnDeleteSetlist);
            this.gbButtons.Controls.Add(this.label14);
            this.gbButtons.Controls.Add(this.label10);
            this.gbButtons.Controls.Add(this.label12);
            this.gbButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbButtons.Location = new System.Drawing.Point(211, 3);
            this.gbButtons.Name = "gbButtons";
            this.gbButtons.Size = new System.Drawing.Size(237, 220);
            this.gbButtons.TabIndex = 5;
            this.gbButtons.TabStop = false;
            // 
            // chkProtectODLC
            // 
            this.chkProtectODLC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkProtectODLC.AutoSize = true;
            this.chkProtectODLC.BackColor = System.Drawing.Color.LightGray;
            this.chkProtectODLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkProtectODLC.ForeColor = System.Drawing.Color.Red;
            this.chkProtectODLC.Location = new System.Drawing.Point(53, 157);
            this.chkProtectODLC.Name = "chkProtectODLC";
            this.chkProtectODLC.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.chkProtectODLC.Size = new System.Drawing.Size(124, 23);
            this.chkProtectODLC.TabIndex = 50;
            this.chkProtectODLC.Text = "Protect Official DLC";
            this.toolTip.SetToolTip(this.chkProtectODLC, "If checked, prevents ODLC\r\nfrom being selected, enabled,\r\ndisabled, copied, moved" +
                    ", or deleted.");
            this.chkProtectODLC.UseVisualStyleBackColor = false;
            this.chkProtectODLC.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkProtectODLC_MouseUp);
            // 
            // btnCombineSetlists
            // 
            this.btnCombineSetlists.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCombineSetlists.Image = global::CustomsForgeSongManager.Properties.Resources.refresh_plus;
            this.btnCombineSetlists.Location = new System.Drawing.Point(20, 111);
            this.btnCombineSetlists.Name = "btnCombineSetlists";
            this.btnCombineSetlists.Size = new System.Drawing.Size(26, 26);
            this.btnCombineSetlists.TabIndex = 64;
            this.toolTip.SetToolTip(this.btnCombineSetlists, "The \'Select\' checkboxes must be\r\nckecked when combining Setlists.\r\n\r\nIndividual s" +
                    "ongs do not need to \r\nbe selected when using Combine.");
            this.btnCombineSetlists.UseVisualStyleBackColor = true;
            this.btnCombineSetlists.Click += new System.EventHandler(this.btnCombineSetlists_Click);
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIncludeSubfolders.AutoSize = true;
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(53, 188);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(119, 23);
            this.chkIncludeSubfolders.TabIndex = 49;
            this.chkIncludeSubfolders.Text = "Include Subfolders";
            this.toolTip.SetToolTip(this.chkIncludeSubfolders, "If checked, show and highlight\r\nsongs in the Master Songs grid\r\nthat are from set" +
                    "list subfolders.");
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            this.chkIncludeSubfolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkIncludeSubfolders_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 65;
            this.label2.Text = "Combine selected setlist(s)";
            // 
            // btnCreateSetlist
            // 
            this.btnCreateSetlist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCreateSetlist.Image = global::CustomsForgeSongManager.Properties.Resources.plus;
            this.btnCreateSetlist.Location = new System.Drawing.Point(20, 15);
            this.btnCreateSetlist.Name = "btnCreateSetlist";
            this.btnCreateSetlist.Size = new System.Drawing.Size(26, 26);
            this.btnCreateSetlist.TabIndex = 51;
            this.btnCreateSetlist.UseVisualStyleBackColor = true;
            this.btnCreateSetlist.Click += new System.EventHandler(this.btnCreateSetlist_Click);
            // 
            // btnEnDiSetlist
            // 
            this.btnEnDiSetlist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnEnDiSetlist.Image = global::CustomsForgeSongManager.Properties.Resources.enabledisable;
            this.btnEnDiSetlist.Location = new System.Drawing.Point(20, 47);
            this.btnEnDiSetlist.Name = "btnEnDiSetlist";
            this.btnEnDiSetlist.Size = new System.Drawing.Size(26, 26);
            this.btnEnDiSetlist.TabIndex = 49;
            this.btnEnDiSetlist.UseVisualStyleBackColor = true;
            this.btnEnDiSetlist.Click += new System.EventHandler(this.btnEnDiSetlist_Click);
            // 
            // btnDeleteSetlist
            // 
            this.btnDeleteSetlist.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDeleteSetlist.Image = global::CustomsForgeSongManager.Properties.Resources.minus;
            this.btnDeleteSetlist.Location = new System.Drawing.Point(20, 79);
            this.btnDeleteSetlist.Name = "btnDeleteSetlist";
            this.btnDeleteSetlist.Size = new System.Drawing.Size(26, 26);
            this.btnDeleteSetlist.TabIndex = 50;
            this.toolTip.SetToolTip(this.btnDeleteSetlist, "Warning:\r\nDeletes entire setlist and \r\nall setlist songs in the setlist.");
            this.btnDeleteSetlist.UseVisualStyleBackColor = true;
            this.btnDeleteSetlist.Click += new System.EventHandler(this.btnDeleteSetlist_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(52, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 13);
            this.label14.TabIndex = 56;
            this.label14.Text = "Delete selected setlist(s)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(52, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Create new setlist";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(52, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(161, 13);
            this.label12.TabIndex = 54;
            this.label12.Text = "Enable/disable selected setlist(s)";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.gbSongs, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.gbSongPacks, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.gbSearch, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 229);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(988, 260);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // gbSongs
            // 
            this.gbSongs.Controls.Add(this.dgvSetlistMaster);
            this.gbSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongs.Location = new System.Drawing.Point(1, 1);
            this.gbSongs.Margin = new System.Windows.Forms.Padding(1);
            this.gbSongs.Name = "gbSongs";
            this.gbSongs.Size = new System.Drawing.Size(564, 258);
            this.gbSongs.TabIndex = 8;
            this.gbSongs.TabStop = false;
            this.gbSongs.Text = "Master Songs";
            // 
            // dgvSetlistMaster
            // 
            this.dgvSetlistMaster.AllowUserToAddRows = false;
            this.dgvSetlistMaster.AllowUserToDeleteRows = false;
            this.dgvSetlistMaster.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSetlistMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvSetlistMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSetlistMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvSetlistMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colEnabled,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colArrangements,
            this.colSongTuning,
            this.colDD,
            this.colFilePath,
            this.colArtistTitleAlbum,
            this.colFileName,
            this.colRepairStatus});
            this.dgvSetlistMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSetlistMaster.Location = new System.Drawing.Point(6, 16);
            this.dgvSetlistMaster.Name = "dgvSetlistMaster";
            this.dgvSetlistMaster.RowHeadersVisible = false;
            this.dgvSetlistMaster.Size = new System.Drawing.Size(552, 234);
            this.dgvSetlistMaster.TabIndex = 32;
            this.dgvSetlistMaster.Tag = "Setlist Manager";
            this.toolTip.SetToolTip(this.dgvSetlistMaster, "Left mouse click on row to select or check \'Select\' checkbox\r\nRight mouse click o" +
                    "n row to show file operation options");
            this.dgvSetlistMaster.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrent_CellFormatting);
            this.dgvSetlistMaster.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlistMaster_CellMouseDown);
            this.dgvSetlistMaster.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrent_CellMouseUp);
            this.dgvSetlistMaster.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlistMaster_ColumnHeaderMouseClick);
            this.dgvSetlistMaster.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSetlistMaster_DataBindingComplete);
            this.dgvSetlistMaster.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSetlistMaster_Paint);
            // 
            // colSelect
            // 
            this.colSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSelect.DataPropertyName = "Selected";
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 43;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEnabled.DefaultCellStyle = dataGridViewCellStyle9;
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnabled.Width = 50;
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
            // colArrangements
            // 
            this.colArrangements.DataPropertyName = "Arrangements";
            this.colArrangements.HeaderText = "Arrangements";
            this.colArrangements.Name = "colArrangements";
            this.colArrangements.ReadOnly = true;
            this.colArrangements.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colSongTuning
            // 
            this.colSongTuning.DataPropertyName = "Tuning";
            this.colSongTuning.HeaderText = "Tuning";
            this.colSongTuning.Name = "colSongTuning";
            this.colSongTuning.ReadOnly = true;
            this.colSongTuning.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongTuning.Width = 70;
            // 
            // colDD
            // 
            this.colDD.DataPropertyName = "DD";
            this.colDD.HeaderText = "DD";
            this.colDD.Name = "colDD";
            this.colDD.ReadOnly = true;
            this.colDD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDD.Width = 50;
            // 
            // colFilePath
            // 
            this.colFilePath.DataPropertyName = "FilePath";
            this.colFilePath.HeaderText = "File Path";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilePath.Width = 350;
            // 
            // colArtistTitleAlbum
            // 
            this.colArtistTitleAlbum.DataPropertyName = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.HeaderText = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.Name = "colArtistTitleAlbum";
            this.colArtistTitleAlbum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArtistTitleAlbum.Visible = false;
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileName.Width = 140;
            // 
            // colRepairStatus
            // 
            this.colRepairStatus.DataPropertyName = "RepairStatus";
            this.colRepairStatus.HeaderText = "RepairStatus";
            this.colRepairStatus.Name = "colRepairStatus";
            // 
            // gbSongPacks
            // 
            this.gbSongPacks.Controls.Add(this.dgvSongPacks);
            this.gbSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongPacks.Location = new System.Drawing.Point(732, 3);
            this.gbSongPacks.Name = "gbSongPacks";
            this.gbSongPacks.Size = new System.Drawing.Size(253, 254);
            this.gbSongPacks.TabIndex = 6;
            this.gbSongPacks.TabStop = false;
            this.gbSongPacks.Text = "Song Packs";
            // 
            // dgvSongPacks
            // 
            this.dgvSongPacks.AllowUserToAddRows = false;
            this.dgvSongPacks.AllowUserToDeleteRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongPacks.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvSongPacks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSongPacks.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongPacks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvSongPacks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSongPackSelect,
            this.colSongPackEnabled,
            this.colSongPackPath,
            this.colSongPackFileName});
            this.dgvSongPacks.Location = new System.Drawing.Point(6, 16);
            this.dgvSongPacks.Name = "dgvSongPacks";
            this.dgvSongPacks.RowHeadersVisible = false;
            this.dgvSongPacks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongPacks.Size = new System.Drawing.Size(241, 232);
            this.dgvSongPacks.TabIndex = 34;
            this.toolTip.SetToolTip(this.dgvSongPacks, "Left mouse click on row to select or check \'Select\' checkbox");
            this.dgvSongPacks.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrent_CellFormatting);
            this.dgvSongPacks.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrent_CellMouseUp);
            this.dgvSongPacks.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSongPacks_CurrentCellDirtyStateChanged);
            // 
            // colSongPackSelect
            // 
            this.colSongPackSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSongPackSelect.DataPropertyName = "Selected";
            this.colSongPackSelect.FalseValue = "false";
            this.colSongPackSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSongPackSelect.HeaderText = "Select";
            this.colSongPackSelect.IndeterminateValue = "false";
            this.colSongPackSelect.Name = "colSongPackSelect";
            this.colSongPackSelect.TrueValue = "true";
            this.colSongPackSelect.Width = 43;
            // 
            // colSongPackEnabled
            // 
            this.colSongPackEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSongPackEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSongPackEnabled.DefaultCellStyle = dataGridViewCellStyle12;
            this.colSongPackEnabled.HeaderText = "Enabled";
            this.colSongPackEnabled.Name = "colSongPackEnabled";
            this.colSongPackEnabled.ReadOnly = true;
            this.colSongPackEnabled.Width = 50;
            // 
            // colSongPackPath
            // 
            this.colSongPackPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSongPackPath.DataPropertyName = "FilePath";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colSongPackPath.DefaultCellStyle = dataGridViewCellStyle13;
            this.colSongPackPath.HeaderText = "File Path";
            this.colSongPackPath.Name = "colSongPackPath";
            this.colSongPackPath.ReadOnly = true;
            this.colSongPackPath.Width = 300;
            // 
            // colSongPackFileName
            // 
            this.colSongPackFileName.DataPropertyName = "FileName";
            this.colSongPackFileName.HeaderText = "File Name";
            this.colSongPackFileName.Name = "colSongPackFileName";
            this.colSongPackFileName.ReadOnly = true;
            this.colSongPackFileName.Width = 140;
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.lnkClearSearch);
            this.gbSearch.Controls.Add(this.btnRunRSWithSetlist);
            this.gbSearch.Controls.Add(this.cueSearch);
            this.gbSearch.Controls.Add(this.lnkSetlistMgrHelp);
            this.gbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSearch.Location = new System.Drawing.Point(569, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Padding = new System.Windows.Forms.Padding(0);
            this.gbSearch.Size = new System.Drawing.Size(157, 254);
            this.gbSearch.TabIndex = 7;
            this.gbSearch.TabStop = false;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(44, 38);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 48;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.toolTip.SetToolTip(this.lnkClearSearch, "HINT:\r\nQuickly clears any Filters, \r\nas well as, any Search, \r\nand preserves exis" +
                    "ting sort.");
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // btnRunRSWithSetlist
            // 
            this.btnRunRSWithSetlist.Enabled = false;
            this.btnRunRSWithSetlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunRSWithSetlist.Image = global::CustomsForgeSongManager.Properties.Resources.StartRs;
            this.btnRunRSWithSetlist.Location = new System.Drawing.Point(31, 171);
            this.btnRunRSWithSetlist.Name = "btnRunRSWithSetlist";
            this.btnRunRSWithSetlist.Size = new System.Drawing.Size(94, 44);
            this.btnRunRSWithSetlist.TabIndex = 46;
            this.btnRunRSWithSetlist.UseVisualStyleBackColor = true;
            this.btnRunRSWithSetlist.Click += new System.EventHandler(this.btnRunRSWithSetlist_Click);
            // 
            // cueSearch
            // 
            this.cueSearch.Cue = "Search";
            this.cueSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueSearch.Location = new System.Drawing.Point(6, 16);
            this.cueSearch.Name = "cueSearch";
            this.cueSearch.Size = new System.Drawing.Size(144, 20);
            this.cueSearch.TabIndex = 44;
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
            // 
            // lnkSetlistMgrHelp
            // 
            this.lnkSetlistMgrHelp.AutoSize = true;
            this.lnkSetlistMgrHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSetlistMgrHelp.ForeColor = System.Drawing.Color.Black;
            this.lnkSetlistMgrHelp.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSetlistMgrHelp.Location = new System.Drawing.Point(11, 97);
            this.lnkSetlistMgrHelp.Name = "lnkSetlistMgrHelp";
            this.lnkSetlistMgrHelp.Size = new System.Drawing.Size(133, 16);
            this.lnkSetlistMgrHelp.TabIndex = 45;
            this.lnkSetlistMgrHelp.TabStop = true;
            this.lnkSetlistMgrHelp.Text = "Setlist Manager Help";
            this.lnkSetlistMgrHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSetlistMgrHelp_LinkClicked);
            // 
            // cmsSetlistManager
            // 
            this.cmsSetlistManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsActions,
            this.toolStripSeparator2,
            this.cmsToggle,
            this.cmsSelectAllNone,
            this.toolStripSeparator1,
            this.cmsShow,
            this.cmsEnableDisable,
            this.cmsDelete,
            this.cmsMove,
            this.cmsCopy});
            this.cmsSetlistManager.Name = "contextMenuStrip_MainManager";
            this.cmsSetlistManager.Size = new System.Drawing.Size(158, 214);
            // 
            // cmsActions
            // 
            this.cmsActions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmsActions.Name = "cmsActions";
            this.cmsActions.Size = new System.Drawing.Size(157, 22);
            this.cmsActions.Text = "Actions:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // cmsToggle
            // 
            this.cmsToggle.Image = global::CustomsForgeSongManager.Properties.Resources.onoff;
            this.cmsToggle.Name = "cmsToggle";
            this.cmsToggle.Size = new System.Drawing.Size(157, 22);
            this.cmsToggle.Text = "Toggle";
            this.cmsToggle.Click += new System.EventHandler(this.cmsToggle_Click);
            // 
            // cmsSelectAllNone
            // 
            this.cmsSelectAllNone.Image = global::CustomsForgeSongManager.Properties.Resources.SelectCol;
            this.cmsSelectAllNone.Name = "cmsSelectAllNone";
            this.cmsSelectAllNone.Size = new System.Drawing.Size(157, 22);
            this.cmsSelectAllNone.Text = "Select All/None";
            this.cmsSelectAllNone.Click += new System.EventHandler(this.cmsSelectAllNone_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
            // 
            // cmsShow
            // 
            this.cmsShow.Image = global::CustomsForgeSongManager.Properties.Resources.Open;
            this.cmsShow.Name = "cmsShow";
            this.cmsShow.Size = new System.Drawing.Size(157, 22);
            this.cmsShow.Text = "Show";
            this.cmsShow.Click += new System.EventHandler(this.cmsShow_Click);
            // 
            // cmsEnableDisable
            // 
            this.cmsEnableDisable.Image = global::CustomsForgeSongManager.Properties.Resources.enabledisable;
            this.cmsEnableDisable.Name = "cmsEnableDisable";
            this.cmsEnableDisable.Size = new System.Drawing.Size(157, 22);
            this.cmsEnableDisable.Text = "Enable/Disable";
            this.cmsEnableDisable.ToolTipText = "Select must be checked.";
            this.cmsEnableDisable.Click += new System.EventHandler(this.cmsEnableDisable_Click);
            // 
            // cmsDelete
            // 
            this.cmsDelete.Image = global::CustomsForgeSongManager.Properties.Resources.delete;
            this.cmsDelete.Name = "cmsDelete";
            this.cmsDelete.Size = new System.Drawing.Size(157, 22);
            this.cmsDelete.Text = "Delete";
            this.cmsDelete.ToolTipText = "WARNING\r\nDeletion can not be undone.\r\nSelect must be checked.";
            this.cmsDelete.Click += new System.EventHandler(this.cmsDelete_Click);
            // 
            // cmsMove
            // 
            this.cmsMove.Image = global::CustomsForgeSongManager.Properties.Resources.export;
            this.cmsMove.Name = "cmsMove";
            this.cmsMove.Size = new System.Drawing.Size(157, 22);
            this.cmsMove.Text = "Move";
            this.cmsMove.ToolTipText = "Select must be checked.";
            this.cmsMove.Click += new System.EventHandler(this.cmsMove_Click);
            // 
            // cmsCopy
            // 
            this.cmsCopy.Image = global::CustomsForgeSongManager.Properties.Resources.copy;
            this.cmsCopy.Name = "cmsCopy";
            this.cmsCopy.Size = new System.Drawing.Size(157, 22);
            this.cmsCopy.Text = "Copy";
            this.cmsCopy.ToolTipText = "Select must be checked.";
            this.cmsCopy.Click += new System.EventHandler(this.cmsCopy_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 50;
            // 
            // SetlistManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SetlistManager";
            this.Size = new System.Drawing.Size(990, 490);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gbSetlistSongs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistSongs)).EndInit();
            this.gbSetlist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).EndInit();
            this.gbButtons.ResumeLayout(false);
            this.gbButtons.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.gbSongs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistMaster)).EndInit();
            this.gbSongPacks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).EndInit();
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            this.cmsSetlistManager.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gbSetlist;
        private RADataGridView dgvSetlists;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSetlistSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetlistEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetlistName;
        private System.Windows.Forms.GroupBox gbButtons;
        private System.Windows.Forms.Button btnCreateSetlist;
        private System.Windows.Forms.Button btnEnDiSetlist;
        private System.Windows.Forms.Button btnDeleteSetlist;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gbSetlistSongs;
        private RADataGridView dgvSetlistSongs;
        private System.Windows.Forms.GroupBox gbSongPacks;
        private RADataGridView dgvSongPacks;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.GroupBox gbSongs;
        private RADataGridView dgvSetlistMaster;
        private System.Windows.Forms.Button btnRunRSWithSetlist;
        private CueTextBox cueSearch;
        private System.Windows.Forms.LinkLabel lnkSetlistMgrHelp;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private CheckBox chkIncludeSubfolders;
        private ContextMenuStrip cmsSetlistManager;
        private ToolStripMenuItem cmsDelete;
        private ToolStripMenuItem cmsCopy;
        private Button btnCombineSetlists;
        private Label label2;
        private ToolStripMenuItem cmsMove;
        private CheckBox chkProtectODLC;
        private ToolStripMenuItem cmsShow;
        private ToolTip toolTip;
        private ToolStripMenuItem cmsEnableDisable;
        private ToolStripMenuItem cmsToggle;
        private ToolStripMenuItem cmsSelectAllNone;
        private DataGridViewCheckBoxColumn colSetlistSongsSelect;
        private DataGridViewTextBoxColumn colSetlistSongsEnabled;
        private DataGridViewTextBoxColumn colSetlistSongsSongArtist;
        private DataGridViewTextBoxColumn colSetlistSongsSongTitle;
        private DataGridViewTextBoxColumn colSetlistSongsSongAlbum;
        private DataGridViewTextBoxColumn colSetlistSongsArrangements;
        private DataGridViewTextBoxColumn colSetlistSongsSongTuning;
        private DataGridViewTextBoxColumn colSetlistSongsDD;
        private DataGridViewTextBoxColumn colSetlistSongsPath;
        private DataGridViewTextBoxColumn colSetlistSongsArtistTitleAlbum;
        private DataGridViewTextBoxColumn colSetlistFileName;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArrangements;
        private DataGridViewAutoFilterTextBoxColumn colSongTuning;
        private DataGridViewAutoFilterTextBoxColumn colDD;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewCheckBoxColumn colSongPackSelect;
        private DataGridViewTextBoxColumn colSongPackEnabled;
        private DataGridViewTextBoxColumn colSongPackPath;
        private DataGridViewTextBoxColumn colSongPackFileName;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem cmsActions;
        private ToolStripSeparator toolStripSeparator2;




    }
}
