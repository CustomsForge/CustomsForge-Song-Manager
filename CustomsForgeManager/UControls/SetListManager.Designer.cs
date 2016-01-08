using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;

namespace CustomsForgeManager.UControls
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
            this.dgvSetlistSongs = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
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
            this.gbSetlist = new System.Windows.Forms.GroupBox();
            this.dgvSetlists = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSetlistSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbButtons = new System.Windows.Forms.GroupBox();
            this.btnCreateSetlist = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.chkDeleteSetlistOrSetlistSongs = new System.Windows.Forms.CheckBox();
            this.btnRemoveSetlistSong = new System.Windows.Forms.Button();
            this.btnToggleSetlistSong = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.btnEnDiSetlist = new System.Windows.Forms.Button();
            this.btnRemoveSetlist = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btnEnDiSetlistSong = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.gbSongs = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnToggleDlcSongs = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnEnDiDlcSongs = new System.Windows.Forms.Button();
            this.btnAddDlcSongs = new System.Windows.Forms.Button();
            this.dgvSetlistMaster = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangements = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDD = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtistTitleAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSongPacks = new System.Windows.Forms.GroupBox();
            this.chkEnableDelete = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEnDiSongPack = new System.Windows.Forms.Button();
            this.dgvSongPacks = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSongPackSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSongPackEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongPackPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.chkSubFolders = new System.Windows.Forms.CheckBox();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRunRSWithSetlist = new System.Windows.Forms.Button();
            this.cueSearch = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.lnkSetlistMgrHelp = new System.Windows.Forms.LinkLabel();
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
            this.colSetlistSongsArtistTitleAlbum});
            this.dgvSetlistSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSetlistSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSetlistSongs.Location = new System.Drawing.Point(3, 16);
            this.dgvSetlistSongs.Name = "dgvSetlistSongs";
            this.dgvSetlistSongs.RowHeadersVisible = false;
            this.dgvSetlistSongs.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvSetlistSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSetlistSongs.Size = new System.Drawing.Size(525, 201);
            this.dgvSetlistSongs.TabIndex = 34;
            this.dgvSetlistSongs.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlistSongs_CellMouseUp);
            // 
            // colSetlistSongsSelect
            // 
            this.colSetlistSongsSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
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
            this.colSetlistSongsPath.DataPropertyName = "Path";
            this.colSetlistSongsPath.HeaderText = "Path";
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
            this.dgvSetlists.MultiSelect = false;
            this.dgvSetlists.Name = "dgvSetlists";
            this.dgvSetlists.RowHeadersVisible = false;
            this.dgvSetlists.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvSetlists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSetlists.Size = new System.Drawing.Size(196, 201);
            this.dgvSetlists.TabIndex = 35;
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
            this.gbButtons.Controls.Add(this.btnCreateSetlist);
            this.gbButtons.Controls.Add(this.label11);
            this.gbButtons.Controls.Add(this.chkDeleteSetlistOrSetlistSongs);
            this.gbButtons.Controls.Add(this.btnRemoveSetlistSong);
            this.gbButtons.Controls.Add(this.btnToggleSetlistSong);
            this.gbButtons.Controls.Add(this.label16);
            this.gbButtons.Controls.Add(this.btnEnDiSetlist);
            this.gbButtons.Controls.Add(this.btnRemoveSetlist);
            this.gbButtons.Controls.Add(this.label15);
            this.gbButtons.Controls.Add(this.btnEnDiSetlistSong);
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
            // btnCreateSetlist
            // 
            this.btnCreateSetlist.Location = new System.Drawing.Point(10, 12);
            this.btnCreateSetlist.Name = "btnCreateSetlist";
            this.btnCreateSetlist.Size = new System.Drawing.Size(22, 22);
            this.btnCreateSetlist.TabIndex = 51;
            this.btnCreateSetlist.Text = "C";
            this.btnCreateSetlist.UseVisualStyleBackColor = true;
            this.btnCreateSetlist.Click += new System.EventHandler(this.btnCreateSetlist_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(38, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(179, 13);
            this.label11.TabIndex = 63;
            this.label11.Text = "Remove selected song(s) from setlist";
            // 
            // chkDeleteSetlistOrSetlistSongs
            // 
            this.chkDeleteSetlistOrSetlistSongs.AutoSize = true;
            this.chkDeleteSetlistOrSetlistSongs.BackColor = System.Drawing.SystemColors.Control;
            this.chkDeleteSetlistOrSetlistSongs.Enabled = false;
            this.chkDeleteSetlistOrSetlistSongs.Location = new System.Drawing.Point(9, 179);
            this.chkDeleteSetlistOrSetlistSongs.Margin = new System.Windows.Forms.Padding(0);
            this.chkDeleteSetlistOrSetlistSongs.Name = "chkDeleteSetlistOrSetlistSongs";
            this.chkDeleteSetlistOrSetlistSongs.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.chkDeleteSetlistOrSetlistSongs.Size = new System.Drawing.Size(212, 30);
            this.chkDeleteSetlistOrSetlistSongs.TabIndex = 61;
            this.chkDeleteSetlistOrSetlistSongs.Text = "  Delete setlists/songs without moving \r\nsongs back to the main dlc folder";
            this.chkDeleteSetlistOrSetlistSongs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDeleteSetlistOrSetlistSongs.UseVisualStyleBackColor = false;
            this.chkDeleteSetlistOrSetlistSongs.CheckedChanged += new System.EventHandler(this.chkDeleteSetlistOrSetlistSongs_CheckedChanged);
            // 
            // btnRemoveSetlistSong
            // 
            this.btnRemoveSetlistSong.Location = new System.Drawing.Point(10, 154);
            this.btnRemoveSetlistSong.Name = "btnRemoveSetlistSong";
            this.btnRemoveSetlistSong.Size = new System.Drawing.Size(22, 22);
            this.btnRemoveSetlistSong.TabIndex = 62;
            this.btnRemoveSetlistSong.Text = "R";
            this.btnRemoveSetlistSong.UseVisualStyleBackColor = true;
            this.btnRemoveSetlistSong.Click += new System.EventHandler(this.btnRemoveSetlistSong_Click);
            // 
            // btnToggleSetlistSong
            // 
            this.btnToggleSetlistSong.Location = new System.Drawing.Point(10, 96);
            this.btnToggleSetlistSong.Name = "btnToggleSetlistSong";
            this.btnToggleSetlistSong.Size = new System.Drawing.Size(22, 22);
            this.btnToggleSetlistSong.TabIndex = 59;
            this.btnToggleSetlistSong.Text = "T";
            this.btnToggleSetlistSong.UseVisualStyleBackColor = true;
            this.btnToggleSetlistSong.Click += new System.EventHandler(this.btnToggleSongsInSetlist_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(38, 100);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(154, 13);
            this.label16.TabIndex = 60;
            this.label16.Text = "Toggle selected songs in setlist";
            // 
            // btnEnDiSetlist
            // 
            this.btnEnDiSetlist.Location = new System.Drawing.Point(10, 40);
            this.btnEnDiSetlist.Name = "btnEnDiSetlist";
            this.btnEnDiSetlist.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiSetlist.TabIndex = 49;
            this.btnEnDiSetlist.Text = "E";
            this.btnEnDiSetlist.UseVisualStyleBackColor = true;
            this.btnEnDiSetlist.Click += new System.EventHandler(this.btnEnDiSetlist_Click);
            // 
            // btnRemoveSetlist
            // 
            this.btnRemoveSetlist.Location = new System.Drawing.Point(10, 68);
            this.btnRemoveSetlist.Name = "btnRemoveSetlist";
            this.btnRemoveSetlist.Size = new System.Drawing.Size(22, 22);
            this.btnRemoveSetlist.TabIndex = 50;
            this.btnRemoveSetlist.Text = "R";
            this.btnRemoveSetlist.UseVisualStyleBackColor = true;
            this.btnRemoveSetlist.Click += new System.EventHandler(this.btnRemoveSetlist_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(38, 128);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(158, 13);
            this.label15.TabIndex = 58;
            this.label15.Text = "Enable/disable selected song(s)";
            // 
            // btnEnDiSetlistSong
            // 
            this.btnEnDiSetlistSong.Location = new System.Drawing.Point(10, 124);
            this.btnEnDiSetlistSong.Name = "btnEnDiSetlistSong";
            this.btnEnDiSetlistSong.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiSetlistSong.TabIndex = 57;
            this.btnEnDiSetlistSong.Text = "E";
            this.btnEnDiSetlistSong.UseVisualStyleBackColor = true;
            this.btnEnDiSetlistSong.Click += new System.EventHandler(this.btnEnDiSetlistSong_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 72);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 13);
            this.label14.TabIndex = 56;
            this.label14.Text = "Remove selected setlist(s)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Create new setlist";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(38, 44);
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
            this.gbSongs.Controls.Add(this.label5);
            this.gbSongs.Controls.Add(this.btnToggleDlcSongs);
            this.gbSongs.Controls.Add(this.label9);
            this.gbSongs.Controls.Add(this.label8);
            this.gbSongs.Controls.Add(this.btnEnDiDlcSongs);
            this.gbSongs.Controls.Add(this.btnAddDlcSongs);
            this.gbSongs.Controls.Add(this.dgvSetlistMaster);
            this.gbSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongs.Location = new System.Drawing.Point(1, 1);
            this.gbSongs.Margin = new System.Windows.Forms.Padding(1);
            this.gbSongs.Name = "gbSongs";
            this.gbSongs.Size = new System.Drawing.Size(564, 258);
            this.gbSongs.TabIndex = 8;
            this.gbSongs.TabStop = false;
            this.gbSongs.Text = "Songs";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(444, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Toggle selected songs";
            // 
            // btnToggleDlcSongs
            // 
            this.btnToggleDlcSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnToggleDlcSongs.Location = new System.Drawing.Point(416, 230);
            this.btnToggleDlcSongs.Name = "btnToggleDlcSongs";
            this.btnToggleDlcSongs.Size = new System.Drawing.Size(22, 22);
            this.btnToggleDlcSongs.TabIndex = 37;
            this.btnToggleDlcSongs.Text = "T";
            this.btnToggleDlcSongs.UseVisualStyleBackColor = true;
            this.btnToggleDlcSongs.Click += new System.EventHandler(this.btnToggleDlcSongs_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 235);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Add selected song(s) to setlist";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(241, 235);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Enable/disable selected song(s)";
            // 
            // btnEnDiDlcSongs
            // 
            this.btnEnDiDlcSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnDiDlcSongs.Location = new System.Drawing.Point(213, 230);
            this.btnEnDiDlcSongs.Name = "btnEnDiDlcSongs";
            this.btnEnDiDlcSongs.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiDlcSongs.TabIndex = 34;
            this.btnEnDiDlcSongs.Text = "E";
            this.btnEnDiDlcSongs.UseVisualStyleBackColor = true;
            this.btnEnDiDlcSongs.Click += new System.EventHandler(this.btnEnDiDlcSongs_Click);
            // 
            // btnAddDlcSongs
            // 
            this.btnAddDlcSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDlcSongs.Location = new System.Drawing.Point(9, 230);
            this.btnAddDlcSongs.Name = "btnAddDlcSongs";
            this.btnAddDlcSongs.Size = new System.Drawing.Size(22, 22);
            this.btnAddDlcSongs.TabIndex = 33;
            this.btnAddDlcSongs.Text = "A";
            this.btnAddDlcSongs.UseVisualStyleBackColor = true;
            this.btnAddDlcSongs.Click += new System.EventHandler(this.btnAddDlcSong_Click);
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
            this.colPath,
            this.colArtistTitleAlbum});
            this.dgvSetlistMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSetlistMaster.Location = new System.Drawing.Point(6, 16);
            this.dgvSetlistMaster.Name = "dgvSetlistMaster";
            this.dgvSetlistMaster.RowHeadersVisible = false;
            this.dgvSetlistMaster.Size = new System.Drawing.Size(552, 212);
            this.dgvSetlistMaster.TabIndex = 32;
            this.dgvSetlistMaster.Tag = "Setlist Manager";
            this.dgvSetlistMaster.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlistMaster_CellMouseUp);
            this.dgvSetlistMaster.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSetlistMaster_ColumnHeaderMouseClick);
            this.dgvSetlistMaster.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSetlistMaster_DataBindingComplete);
            this.dgvSetlistMaster.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSetlistMaster_Paint);
            // 
            // colSelect
            // 
            this.colSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
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
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 350;
            // 
            // colArtistTitleAlbum
            // 
            this.colArtistTitleAlbum.DataPropertyName = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.HeaderText = "ArtistTitleAlbum";
            this.colArtistTitleAlbum.Name = "colArtistTitleAlbum";
            this.colArtistTitleAlbum.Visible = false;
            // 
            // gbSongPacks
            // 
            this.gbSongPacks.Controls.Add(this.chkEnableDelete);
            this.gbSongPacks.Controls.Add(this.label4);
            this.gbSongPacks.Controls.Add(this.btnEnDiSongPack);
            this.gbSongPacks.Controls.Add(this.dgvSongPacks);
            this.gbSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongPacks.Location = new System.Drawing.Point(732, 3);
            this.gbSongPacks.Name = "gbSongPacks";
            this.gbSongPacks.Size = new System.Drawing.Size(253, 254);
            this.gbSongPacks.TabIndex = 6;
            this.gbSongPacks.TabStop = false;
            this.gbSongPacks.Text = "Song Packs";
            // 
            // chkEnableDelete
            // 
            this.chkEnableDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnableDelete.AutoSize = true;
            this.chkEnableDelete.Location = new System.Drawing.Point(21, 205);
            this.chkEnableDelete.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.chkEnableDelete.Name = "chkEnableDelete";
            this.chkEnableDelete.Size = new System.Drawing.Size(192, 43);
            this.chkEnableDelete.TabIndex = 37;
            this.chkEnableDelete.Text = "  Enable the \'Delete setlist/songs \r\n  without moving songs\' checkbox. \r\n  Hazrdo" +
                "us to any CDLC collection!";
            this.chkEnableDelete.UseVisualStyleBackColor = true;
            this.chkEnableDelete.CheckedChanged += new System.EventHandler(this.chkEnableDelete_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Enable/disable selected song packs";
            // 
            // btnEnDiSongPack
            // 
            this.btnEnDiSongPack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnDiSongPack.Location = new System.Drawing.Point(17, 169);
            this.btnEnDiSongPack.Name = "btnEnDiSongPack";
            this.btnEnDiSongPack.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiSongPack.TabIndex = 35;
            this.btnEnDiSongPack.Text = "E";
            this.btnEnDiSongPack.UseVisualStyleBackColor = true;
            this.btnEnDiSongPack.Click += new System.EventHandler(this.btnEnDiSongPack_Click);
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
            this.colSongPackPath});
            this.dgvSongPacks.Location = new System.Drawing.Point(6, 16);
            this.dgvSongPacks.Name = "dgvSongPacks";
            this.dgvSongPacks.RowHeadersVisible = false;
            this.dgvSongPacks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongPacks.Size = new System.Drawing.Size(241, 141);
            this.dgvSongPacks.TabIndex = 34;
            // 
            // colSongPackSelect
            // 
            this.colSongPackSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSongPackSelect.DataPropertyName = "Select";
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
            this.colSongPackPath.DataPropertyName = "Path";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colSongPackPath.DefaultCellStyle = dataGridViewCellStyle13;
            this.colSongPackPath.HeaderText = "Path";
            this.colSongPackPath.Name = "colSongPackPath";
            this.colSongPackPath.ReadOnly = true;
            this.colSongPackPath.Width = 300;
            // 
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.chkSubFolders);
            this.gbSearch.Controls.Add(this.lnkClearSearch);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Controls.Add(this.btnRunRSWithSetlist);
            this.gbSearch.Controls.Add(this.cueSearch);
            this.gbSearch.Controls.Add(this.lnkSetlistMgrHelp);
            this.gbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSearch.Location = new System.Drawing.Point(569, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(157, 254);
            this.gbSearch.TabIndex = 7;
            this.gbSearch.TabStop = false;
            // 
            // chkSubFolders
            // 
            this.chkSubFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSubFolders.AutoSize = true;
            this.chkSubFolders.Checked = true;
            this.chkSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubFolders.Location = new System.Drawing.Point(15, 232);
            this.chkSubFolders.Name = "chkSubFolders";
            this.chkSubFolders.Size = new System.Drawing.Size(125, 17);
            this.chkSubFolders.TabIndex = 49;
            this.chkSubFolders.Text = "Include Setlist Songs";
            this.chkSubFolders.UseVisualStyleBackColor = true;
            this.chkSubFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkSubFolders_MouseUp);
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(42, 39);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 48;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "with enabled setlist(s)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRunRSWithSetlist
            // 
            this.btnRunRSWithSetlist.Enabled = false;
            this.btnRunRSWithSetlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunRSWithSetlist.Image = global::CustomsForgeManager.Properties.Resources.StartRs;
            this.btnRunRSWithSetlist.Location = new System.Drawing.Point(28, 126);
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
            this.lnkSetlistMgrHelp.Location = new System.Drawing.Point(12, 85);
            this.lnkSetlistMgrHelp.Name = "lnkSetlistMgrHelp";
            this.lnkSetlistMgrHelp.Size = new System.Drawing.Size(133, 16);
            this.lnkSetlistMgrHelp.TabIndex = 45;
            this.lnkSetlistMgrHelp.TabStop = true;
            this.lnkSetlistMgrHelp.Text = "Setlist Manager Help";
            this.lnkSetlistMgrHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSetlistMgrHelp_LinkClicked);
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
            this.gbSongs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistMaster)).EndInit();
            this.gbSongPacks.ResumeLayout(false);
            this.gbSongPacks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).EndInit();
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkDeleteSetlistOrSetlistSongs;
        private System.Windows.Forms.Button btnRemoveSetlistSong;
        private System.Windows.Forms.Button btnToggleSetlistSong;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnEnDiSetlist;
        private System.Windows.Forms.Button btnRemoveSetlist;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnEnDiSetlistSong;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gbSetlistSongs;
        private RADataGridView dgvSetlistSongs;
        private System.Windows.Forms.GroupBox gbSongPacks;
        private System.Windows.Forms.CheckBox chkEnableDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEnDiSongPack;
        private RADataGridView dgvSongPacks;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.GroupBox gbSongs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnToggleDlcSongs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEnDiDlcSongs;
        private System.Windows.Forms.Button btnAddDlcSongs;
        private RADataGridView dgvSetlistMaster;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRunRSWithSetlist;
        private CustomsForgeManagerLib.CustomControls.CueTextBox cueSearch;
        private System.Windows.Forms.LinkLabel lnkSetlistMgrHelp;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
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
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colEnabled;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArrangements;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongTuning;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colDD;
        private DataGridViewTextBoxColumn colPath;
        private DataGridViewTextBoxColumn colArtistTitleAlbum;
        private DataGridViewCheckBoxColumn colSongPackSelect;
        private DataGridViewTextBoxColumn colSongPackEnabled;
        private DataGridViewTextBoxColumn colSongPackPath;
        private CheckBox chkSubFolders;




    }
}
