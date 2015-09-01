using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;

namespace CustomsForgeManager.UControls
{
    partial class SetListManager
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupSetlists = new System.Windows.Forms.GroupBox();
            this.radioEnableDisableSetlists = new System.Windows.Forms.RadioButton();
            this.radioEnableDisableSongs = new System.Windows.Forms.RadioButton();
            this.dgvDLCsInSetlist = new System.Windows.Forms.DataGridView();
            this.colDLCSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDLCEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDLCArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDLCSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDLCAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDLCTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDLCPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEnableAllSetlists = new System.Windows.Forms.Button();
            this.dgvSetlists = new System.Windows.Forms.DataGridView();
            this.colSetlistSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkDeleteSongsAndSetlists = new System.Windows.Forms.CheckBox();
            this.btnCreateNewSetlist = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveSongsFromSetlist = new System.Windows.Forms.Button();
            this.btnDeleteSelectedSetlist = new System.Windows.Forms.Button();
            this.btnEnableDisableSelectedSM = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRunRSWithSetlist = new System.Windows.Forms.Button();
            this.linkOpenSngMgrHelp = new System.Windows.Forms.LinkLabel();
            this.groupOfficialSongs = new System.Windows.Forms.GroupBox();
            this.btnRestoreOfficialsBackup = new System.Windows.Forms.Button();
            this.btnSngPackToSetlist = new System.Windows.Forms.Button();
            this.btnEnblDisblOfficialSongPack = new System.Windows.Forms.Button();
            this.dgvOfficialSongs = new System.Windows.Forms.DataGridView();
            this.colOfficialSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colOfficialEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOfficialSongPack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOfficialPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupUnsortedDLCs = new System.Windows.Forms.GroupBox();
            this.checkSearchInAllSetlists = new System.Windows.Forms.CheckBox();
            this.btnDeleteSelectedSongs = new System.Windows.Forms.Button();
            this.btnEnableDisableSelectedSongs = new System.Windows.Forms.Button();
            this.dgvUnsortedDLCs = new System.Windows.Forms.DataGridView();
            this.colUnsortedSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colUnsortedEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnsortedArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnsortedSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnsortedTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnsortedPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMoveSongToSetlist = new System.Windows.Forms.Button();
            this.tbUnsortedSearch = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.bWorker = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.AbortableBackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupSetlists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDLCsInSetlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupOfficialSongs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfficialSongs)).BeginInit();
            this.groupUnsortedDLCs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsortedDLCs)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.68588F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.63262F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.6815F));
            this.tableLayoutPanel1.Controls.Add(this.groupSetlists, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupOfficialSongs, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupUnsortedDLCs, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(990, 490);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // groupSetlists
            // 
            this.groupSetlists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupSetlists, 3);
            this.groupSetlists.Controls.Add(this.radioEnableDisableSetlists);
            this.groupSetlists.Controls.Add(this.radioEnableDisableSongs);
            this.groupSetlists.Controls.Add(this.dgvDLCsInSetlist);
            this.groupSetlists.Controls.Add(this.btnEnableAllSetlists);
            this.groupSetlists.Controls.Add(this.dgvSetlists);
            this.groupSetlists.Controls.Add(this.checkDeleteSongsAndSetlists);
            this.groupSetlists.Controls.Add(this.btnCreateNewSetlist);
            this.groupSetlists.Controls.Add(this.label2);
            this.groupSetlists.Controls.Add(this.label3);
            this.groupSetlists.Controls.Add(this.btnRemoveSongsFromSetlist);
            this.groupSetlists.Controls.Add(this.btnDeleteSelectedSetlist);
            this.groupSetlists.Controls.Add(this.btnEnableDisableSelectedSM);
            this.groupSetlists.Location = new System.Drawing.Point(3, 3);
            this.groupSetlists.Name = "groupSetlists";
            this.groupSetlists.Size = new System.Drawing.Size(984, 239);
            this.groupSetlists.TabIndex = 29;
            this.groupSetlists.TabStop = false;
            this.groupSetlists.Text = "Setlists";
            // 
            // radioEnableDisableSetlists
            // 
            this.radioEnableDisableSetlists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioEnableDisableSetlists.AutoSize = true;
            this.radioEnableDisableSetlists.Location = new System.Drawing.Point(903, 149);
            this.radioEnableDisableSetlists.Name = "radioEnableDisableSetlists";
            this.radioEnableDisableSetlists.Size = new System.Drawing.Size(56, 17);
            this.radioEnableDisableSetlists.TabIndex = 0;
            this.radioEnableDisableSetlists.Text = "setlists";
            // 
            // radioEnableDisableSongs
            // 
            this.radioEnableDisableSongs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.radioEnableDisableSongs.AutoSize = true;
            this.radioEnableDisableSongs.Location = new System.Drawing.Point(844, 149);
            this.radioEnableDisableSongs.Name = "radioEnableDisableSongs";
            this.radioEnableDisableSongs.Size = new System.Drawing.Size(53, 17);
            this.radioEnableDisableSongs.TabIndex = 1;
            this.radioEnableDisableSongs.Text = "songs";
            // 
            // dgvDLCsInSetlist
            // 
            this.dgvDLCsInSetlist.AllowUserToAddRows = false;
            this.dgvDLCsInSetlist.AllowUserToDeleteRows = false;
            this.dgvDLCsInSetlist.AllowUserToOrderColumns = true;
            this.dgvDLCsInSetlist.AllowUserToResizeRows = false;
            this.dgvDLCsInSetlist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvDLCsInSetlist.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvDLCsInSetlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDLCsInSetlist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDLCSelect,
            this.colDLCEnabled,
            this.colDLCArtist,
            this.colDLCSong,
            this.colDLCAlbum,
            this.colDLCTuning,
            this.colDLCPath});
            this.dgvDLCsInSetlist.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvDLCsInSetlist.Location = new System.Drawing.Point(310, 32);
            this.dgvDLCsInSetlist.Name = "dgvDLCsInSetlist";
            this.dgvDLCsInSetlist.RowHeadersVisible = false;
            this.dgvDLCsInSetlist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDLCsInSetlist.Size = new System.Drawing.Size(374, 194);
            this.dgvDLCsInSetlist.TabIndex = 21;
            // 
            // colDLCSelect
            // 
            this.colDLCSelect.HeaderText = "Select";
            this.colDLCSelect.Name = "colDLCSelect";
            this.colDLCSelect.Width = 45;
            // 
            // colDLCEnabled
            // 
            this.colDLCEnabled.HeaderText = "Enabled";
            this.colDLCEnabled.Name = "colDLCEnabled";
            this.colDLCEnabled.Width = 55;
            // 
            // colDLCArtist
            // 
            this.colDLCArtist.HeaderText = "Artist";
            this.colDLCArtist.Name = "colDLCArtist";
            this.colDLCArtist.Width = 90;
            // 
            // colDLCSong
            // 
            this.colDLCSong.HeaderText = "Song";
            this.colDLCSong.Name = "colDLCSong";
            this.colDLCSong.Width = 90;
            // 
            // colDLCAlbum
            // 
            this.colDLCAlbum.HeaderText = "Album";
            this.colDLCAlbum.Name = "colDLCAlbum";
            this.colDLCAlbum.Width = 90;
            // 
            // colDLCTuning
            // 
            this.colDLCTuning.HeaderText = "Tuning";
            this.colDLCTuning.Name = "colDLCTuning";
            // 
            // colDLCPath
            // 
            this.colDLCPath.HeaderText = "Path";
            this.colDLCPath.Name = "colDLCPath";
            this.colDLCPath.Visible = false;
            // 
            // btnEnableAllSetlists
            // 
            this.btnEnableAllSetlists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEnableAllSetlists.Location = new System.Drawing.Point(846, 79);
            this.btnEnableAllSetlists.Name = "btnEnableAllSetlists";
            this.btnEnableAllSetlists.Size = new System.Drawing.Size(113, 46);
            this.btnEnableAllSetlists.TabIndex = 20;
            this.btnEnableAllSetlists.Text = "Enable all setlists";
            this.btnEnableAllSetlists.UseVisualStyleBackColor = true;
            this.btnEnableAllSetlists.Click += new System.EventHandler(this.btnEnableAllSetlists_Click);
            // 
            // dgvSetlists
            // 
            this.dgvSetlists.AllowUserToAddRows = false;
            this.dgvSetlists.AllowUserToDeleteRows = false;
            this.dgvSetlists.AllowUserToOrderColumns = true;
            this.dgvSetlists.AllowUserToResizeRows = false;
            this.dgvSetlists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvSetlists.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvSetlists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSetlists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSetlistSelect,
            this.colSetlistEnabled,
            this.colSetlist});
            this.dgvSetlists.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvSetlists.Location = new System.Drawing.Point(22, 32);
            this.dgvSetlists.Name = "dgvSetlists";
            this.dgvSetlists.RowHeadersVisible = false;
            this.dgvSetlists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSetlists.Size = new System.Drawing.Size(243, 194);
            this.dgvSetlists.TabIndex = 13;
            // 
            // colSetlistSelect
            // 
            this.colSetlistSelect.HeaderText = "Select";
            this.colSetlistSelect.Name = "colSetlistSelect";
            this.colSetlistSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSetlistSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSetlistSelect.Width = 50;
            // 
            // colSetlistEnabled
            // 
            this.colSetlistEnabled.HeaderText = "Enabled";
            this.colSetlistEnabled.Name = "colSetlistEnabled";
            this.colSetlistEnabled.Width = 60;
            // 
            // colSetlist
            // 
            this.colSetlist.HeaderText = "Setlist";
            this.colSetlist.Name = "colSetlist";
            this.colSetlist.Width = 130;
            // 
            // checkDeleteSongsAndSetlists
            // 
            this.checkDeleteSongsAndSetlists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkDeleteSongsAndSetlists.AutoSize = true;
            this.checkDeleteSongsAndSetlists.Location = new System.Drawing.Point(750, 194);
            this.checkDeleteSongsAndSetlists.Name = "checkDeleteSongsAndSetlists";
            this.checkDeleteSongsAndSetlists.Size = new System.Drawing.Size(201, 30);
            this.checkDeleteSongsAndSetlists.TabIndex = 12;
            this.checkDeleteSongsAndSetlists.Text = "Delete setlists/songs without moving \r\nsongs to the main dlc folder";
            this.checkDeleteSongsAndSetlists.UseVisualStyleBackColor = true;
            // 
            // btnCreateNewSetlist
            // 
            this.btnCreateNewSetlist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCreateNewSetlist.Location = new System.Drawing.Point(710, 27);
            this.btnCreateNewSetlist.Name = "btnCreateNewSetlist";
            this.btnCreateNewSetlist.Size = new System.Drawing.Size(113, 46);
            this.btnCreateNewSetlist.TabIndex = 19;
            this.btnCreateNewSetlist.Text = "Create new setlist";
            this.btnCreateNewSetlist.UseVisualStyleBackColor = true;
            this.btnCreateNewSetlist.Click += new System.EventHandler(this.btnCreateNewSetlist_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(452, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "DLCs in the setlist";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Setlists";
            // 
            // btnRemoveSongsFromSetlist
            // 
            this.btnRemoveSongsFromSetlist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemoveSongsFromSetlist.Location = new System.Drawing.Point(710, 81);
            this.btnRemoveSongsFromSetlist.Name = "btnRemoveSongsFromSetlist";
            this.btnRemoveSongsFromSetlist.Size = new System.Drawing.Size(113, 46);
            this.btnRemoveSongsFromSetlist.TabIndex = 15;
            this.btnRemoveSongsFromSetlist.Text = "Remove selected song(s) from setlist";
            this.btnRemoveSongsFromSetlist.UseVisualStyleBackColor = true;
            this.btnRemoveSongsFromSetlist.Click += new System.EventHandler(this.btnRemoveSongsFromSetlist_Click);
            // 
            // btnDeleteSelectedSetlist
            // 
            this.btnDeleteSelectedSetlist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteSelectedSetlist.Location = new System.Drawing.Point(846, 27);
            this.btnDeleteSelectedSetlist.Name = "btnDeleteSelectedSetlist";
            this.btnDeleteSelectedSetlist.Size = new System.Drawing.Size(113, 46);
            this.btnDeleteSelectedSetlist.TabIndex = 3;
            this.btnDeleteSelectedSetlist.Text = "Delete selected setlist(s)";
            this.btnDeleteSelectedSetlist.UseVisualStyleBackColor = true;
            this.btnDeleteSelectedSetlist.Click += new System.EventHandler(this.btnDeleteSelectedSetlist_Click);
            // 
            // btnEnableDisableSelectedSM
            // 
            this.btnEnableDisableSelectedSM.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEnableDisableSelectedSM.Location = new System.Drawing.Point(710, 135);
            this.btnEnableDisableSelectedSM.Name = "btnEnableDisableSelectedSM";
            this.btnEnableDisableSelectedSM.Size = new System.Drawing.Size(113, 46);
            this.btnEnableDisableSelectedSM.TabIndex = 2;
            this.btnEnableDisableSelectedSM.Text = "Enable/disable selected ";
            this.btnEnableDisableSelectedSM.UseVisualStyleBackColor = true;
            this.btnEnableDisableSelectedSM.Click += new System.EventHandler(this.btnEnableDisableSelectedSM_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnRunRSWithSetlist);
            this.panel1.Controls.Add(this.linkOpenSngMgrHelp);
            this.panel1.Location = new System.Drawing.Point(415, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 236);
            this.panel1.TabIndex = 32;
            // 
            // btnRunRSWithSetlist
            // 
            this.btnRunRSWithSetlist.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRunRSWithSetlist.Location = new System.Drawing.Point(7, 63);
            this.btnRunRSWithSetlist.Name = "btnRunRSWithSetlist";
            this.btnRunRSWithSetlist.Size = new System.Drawing.Size(136, 46);
            this.btnRunRSWithSetlist.TabIndex = 34;
            this.btnRunRSWithSetlist.Text = "Run RS with selected setlist(s)";
            this.btnRunRSWithSetlist.UseVisualStyleBackColor = true;
            this.btnRunRSWithSetlist.Click += new System.EventHandler(this.btnRunRSWithSetlist_Click);
            // 
            // linkOpenSngMgrHelp
            // 
            this.linkOpenSngMgrHelp.AutoSize = true;
            this.linkOpenSngMgrHelp.ForeColor = System.Drawing.Color.Black;
            this.linkOpenSngMgrHelp.LinkColor = System.Drawing.Color.Black;
            this.linkOpenSngMgrHelp.Location = new System.Drawing.Point(47, 141);
            this.linkOpenSngMgrHelp.Name = "linkOpenSngMgrHelp";
            this.linkOpenSngMgrHelp.Size = new System.Drawing.Size(56, 13);
            this.linkOpenSngMgrHelp.TabIndex = 33;
            this.linkOpenSngMgrHelp.TabStop = true;
            this.linkOpenSngMgrHelp.Text = "Open help";
            // 
            // groupOfficialSongs
            // 
            this.groupOfficialSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOfficialSongs.Controls.Add(this.btnRestoreOfficialsBackup);
            this.groupOfficialSongs.Controls.Add(this.btnSngPackToSetlist);
            this.groupOfficialSongs.Controls.Add(this.btnEnblDisblOfficialSongPack);
            this.groupOfficialSongs.Controls.Add(this.dgvOfficialSongs);
            this.groupOfficialSongs.Location = new System.Drawing.Point(579, 248);
            this.groupOfficialSongs.Name = "groupOfficialSongs";
            this.groupOfficialSongs.Size = new System.Drawing.Size(408, 236);
            this.groupOfficialSongs.TabIndex = 33;
            this.groupOfficialSongs.TabStop = false;
            this.groupOfficialSongs.Text = "Official songs";
            // 
            // btnRestoreOfficialsBackup
            // 
            this.btnRestoreOfficialsBackup.Location = new System.Drawing.Point(278, 179);
            this.btnRestoreOfficialsBackup.Name = "btnRestoreOfficialsBackup";
            this.btnRestoreOfficialsBackup.Size = new System.Drawing.Size(105, 46);
            this.btnRestoreOfficialsBackup.TabIndex = 22;
            this.btnRestoreOfficialsBackup.Text = "Restore official song pack backup";
            this.btnRestoreOfficialsBackup.UseVisualStyleBackColor = true;
            this.btnRestoreOfficialsBackup.Click += new System.EventHandler(this.btnRestoreOfficialsBackup_Click);
            // 
            // btnSngPackToSetlist
            // 
            this.btnSngPackToSetlist.Location = new System.Drawing.Point(150, 179);
            this.btnSngPackToSetlist.Name = "btnSngPackToSetlist";
            this.btnSngPackToSetlist.Size = new System.Drawing.Size(116, 46);
            this.btnSngPackToSetlist.TabIndex = 19;
            this.btnSngPackToSetlist.Text = "Add song packs to selected setlist";
            this.btnSngPackToSetlist.UseVisualStyleBackColor = true;
            this.btnSngPackToSetlist.Click += new System.EventHandler(this.btnSngPackToSetlist_Click);
            // 
            // btnEnblDisblOfficialSongPack
            // 
            this.btnEnblDisblOfficialSongPack.Location = new System.Drawing.Point(22, 179);
            this.btnEnblDisblOfficialSongPack.Name = "btnEnblDisblOfficialSongPack";
            this.btnEnblDisblOfficialSongPack.Size = new System.Drawing.Size(116, 46);
            this.btnEnblDisblOfficialSongPack.TabIndex = 18;
            this.btnEnblDisblOfficialSongPack.Text = "Enable/disable selected song packs";
            this.btnEnblDisblOfficialSongPack.UseVisualStyleBackColor = true;
            this.btnEnblDisblOfficialSongPack.Click += new System.EventHandler(this.btnEnblDisblOfficialSongPack_Click);
            // 
            // dgvOfficialSongs
            // 
            this.dgvOfficialSongs.AllowUserToAddRows = false;
            this.dgvOfficialSongs.AllowUserToDeleteRows = false;
            this.dgvOfficialSongs.AllowUserToOrderColumns = true;
            this.dgvOfficialSongs.AllowUserToResizeRows = false;
            this.dgvOfficialSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOfficialSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOfficialSelect,
            this.colOfficialEnabled,
            this.colOfficialSongPack,
            this.colOfficialPath});
            this.dgvOfficialSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvOfficialSongs.Location = new System.Drawing.Point(88, 22);
            this.dgvOfficialSongs.Name = "dgvOfficialSongs";
            this.dgvOfficialSongs.RowHeadersVisible = false;
            this.dgvOfficialSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOfficialSongs.Size = new System.Drawing.Size(243, 143);
            this.dgvOfficialSongs.TabIndex = 17;
            // 
            // colOfficialSelect
            // 
            this.colOfficialSelect.HeaderText = "Select";
            this.colOfficialSelect.Name = "colOfficialSelect";
            this.colOfficialSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colOfficialSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colOfficialSelect.Width = 50;
            // 
            // colOfficialEnabled
            // 
            this.colOfficialEnabled.HeaderText = "Enabled";
            this.colOfficialEnabled.Name = "colOfficialEnabled";
            this.colOfficialEnabled.Width = 60;
            // 
            // colOfficialSongPack
            // 
            this.colOfficialSongPack.HeaderText = "Song Pack";
            this.colOfficialSongPack.Name = "colOfficialSongPack";
            this.colOfficialSongPack.Width = 130;
            // 
            // colOfficialPath
            // 
            this.colOfficialPath.HeaderText = "Path";
            this.colOfficialPath.Name = "colOfficialPath";
            this.colOfficialPath.Visible = false;
            // 
            // groupUnsortedDLCs
            // 
            this.groupUnsortedDLCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupUnsortedDLCs.Controls.Add(this.checkSearchInAllSetlists);
            this.groupUnsortedDLCs.Controls.Add(this.tbUnsortedSearch);
            this.groupUnsortedDLCs.Controls.Add(this.btnDeleteSelectedSongs);
            this.groupUnsortedDLCs.Controls.Add(this.btnEnableDisableSelectedSongs);
            this.groupUnsortedDLCs.Controls.Add(this.dgvUnsortedDLCs);
            this.groupUnsortedDLCs.Controls.Add(this.btnMoveSongToSetlist);
            this.groupUnsortedDLCs.Location = new System.Drawing.Point(3, 248);
            this.groupUnsortedDLCs.Name = "groupUnsortedDLCs";
            this.groupUnsortedDLCs.Size = new System.Drawing.Size(406, 236);
            this.groupUnsortedDLCs.TabIndex = 31;
            this.groupUnsortedDLCs.TabStop = false;
            this.groupUnsortedDLCs.Text = "Unsorted DLCs";
            // 
            // checkSearchInAllSetlists
            // 
            this.checkSearchInAllSetlists.AutoSize = true;
            this.checkSearchInAllSetlists.Location = new System.Drawing.Point(275, 48);
            this.checkSearchInAllSetlists.Name = "checkSearchInAllSetlists";
            this.checkSearchInAllSetlists.Size = new System.Drawing.Size(118, 17);
            this.checkSearchInAllSetlists.TabIndex = 20;
            this.checkSearchInAllSetlists.Text = "Search in all setlists";
            this.checkSearchInAllSetlists.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSelectedSongs
            // 
            this.btnDeleteSelectedSongs.Location = new System.Drawing.Point(275, 179);
            this.btnDeleteSelectedSongs.Name = "btnDeleteSelectedSongs";
            this.btnDeleteSelectedSongs.Size = new System.Drawing.Size(115, 46);
            this.btnDeleteSelectedSongs.TabIndex = 18;
            this.btnDeleteSelectedSongs.Text = "Delete selected songs";
            this.btnDeleteSelectedSongs.UseVisualStyleBackColor = true;
            this.btnDeleteSelectedSongs.Click += new System.EventHandler(this.btnDeleteSelectedSongs_Click);
            // 
            // btnEnableDisableSelectedSongs
            // 
            this.btnEnableDisableSelectedSongs.Location = new System.Drawing.Point(275, 127);
            this.btnEnableDisableSelectedSongs.Name = "btnEnableDisableSelectedSongs";
            this.btnEnableDisableSelectedSongs.Size = new System.Drawing.Size(115, 46);
            this.btnEnableDisableSelectedSongs.TabIndex = 17;
            this.btnEnableDisableSelectedSongs.Text = "Enable/disable selected songs";
            this.btnEnableDisableSelectedSongs.UseVisualStyleBackColor = true;
            this.btnEnableDisableSelectedSongs.Click += new System.EventHandler(this.btnEnableDisableSelectedSongs_Click);
            // 
            // dgvUnsortedDLCs
            // 
            this.dgvUnsortedDLCs.AllowUserToAddRows = false;
            this.dgvUnsortedDLCs.AllowUserToDeleteRows = false;
            this.dgvUnsortedDLCs.AllowUserToOrderColumns = true;
            this.dgvUnsortedDLCs.AllowUserToResizeRows = false;
            this.dgvUnsortedDLCs.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvUnsortedDLCs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUnsortedDLCs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUnsortedSelect,
            this.colUnsortedEnabled,
            this.colUnsortedArtist,
            this.colUnsortedSong,
            this.colUnsortedTuning,
            this.colUnsortedPath});
            this.dgvUnsortedDLCs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvUnsortedDLCs.Location = new System.Drawing.Point(6, 22);
            this.dgvUnsortedDLCs.Name = "dgvUnsortedDLCs";
            this.dgvUnsortedDLCs.RowHeadersVisible = false;
            this.dgvUnsortedDLCs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUnsortedDLCs.Size = new System.Drawing.Size(259, 203);
            this.dgvUnsortedDLCs.TabIndex = 16;
            // 
            // colUnsortedSelect
            // 
            this.colUnsortedSelect.HeaderText = "Select";
            this.colUnsortedSelect.Name = "colUnsortedSelect";
            this.colUnsortedSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnsortedSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colUnsortedSelect.Width = 45;
            // 
            // colUnsortedEnabled
            // 
            this.colUnsortedEnabled.HeaderText = "Enabled";
            this.colUnsortedEnabled.Name = "colUnsortedEnabled";
            this.colUnsortedEnabled.Width = 55;
            // 
            // colUnsortedArtist
            // 
            this.colUnsortedArtist.HeaderText = "Artist";
            this.colUnsortedArtist.Name = "colUnsortedArtist";
            this.colUnsortedArtist.Width = 75;
            // 
            // colUnsortedSong
            // 
            this.colUnsortedSong.HeaderText = "Song";
            this.colUnsortedSong.Name = "colUnsortedSong";
            this.colUnsortedSong.Width = 81;
            // 
            // colUnsortedTuning
            // 
            this.colUnsortedTuning.HeaderText = "Tuning";
            this.colUnsortedTuning.Name = "colUnsortedTuning";
            // 
            // colUnsortedPath
            // 
            this.colUnsortedPath.HeaderText = "Path";
            this.colUnsortedPath.Name = "colUnsortedPath";
            this.colUnsortedPath.Visible = false;
            // 
            // btnMoveSongToSetlist
            // 
            this.btnMoveSongToSetlist.Location = new System.Drawing.Point(275, 75);
            this.btnMoveSongToSetlist.Name = "btnMoveSongToSetlist";
            this.btnMoveSongToSetlist.Size = new System.Drawing.Size(115, 46);
            this.btnMoveSongToSetlist.TabIndex = 14;
            this.btnMoveSongToSetlist.Text = "Add selected song(s) to setlist";
            this.btnMoveSongToSetlist.UseVisualStyleBackColor = true;
            this.btnMoveSongToSetlist.Click += new System.EventHandler(this.btnMoveSongToSetlist_Click);
            // 
            // tbUnsortedSearch
            // 
            this.tbUnsortedSearch.Cue = "Search";
            this.tbUnsortedSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tbUnsortedSearch.ForeColor = System.Drawing.Color.Gray;
            this.tbUnsortedSearch.Location = new System.Drawing.Point(272, 22);
            this.tbUnsortedSearch.Name = "tbUnsortedSearch";
            this.tbUnsortedSearch.Size = new System.Drawing.Size(127, 20);
            this.tbUnsortedSearch.TabIndex = 19;
            this.tbUnsortedSearch.TextChanged += new System.EventHandler(this.tbUnsortedSearch_TextChanged);
            // 
            // SetListManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SetListManager";
            this.Size = new System.Drawing.Size(990, 490);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupSetlists.ResumeLayout(false);
            this.groupSetlists.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDLCsInSetlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupOfficialSongs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfficialSongs)).EndInit();
            this.groupUnsortedDLCs.ResumeLayout(false);
            this.groupUnsortedDLCs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnsortedDLCs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupSetlists;
        private System.Windows.Forms.RadioButton radioEnableDisableSetlists;
        private System.Windows.Forms.RadioButton radioEnableDisableSongs;
        private System.Windows.Forms.DataGridView dgvDLCsInSetlist;
        private System.Windows.Forms.Button btnEnableAllSetlists;
        private System.Windows.Forms.DataGridView dgvSetlists;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSetlistSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetlistEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSetlist;
        private System.Windows.Forms.CheckBox checkDeleteSongsAndSetlists;
        private System.Windows.Forms.Button btnCreateNewSetlist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveSongsFromSetlist;
        private System.Windows.Forms.Button btnDeleteSelectedSetlist;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRunRSWithSetlist;
        private System.Windows.Forms.LinkLabel linkOpenSngMgrHelp;
        private System.Windows.Forms.GroupBox groupOfficialSongs;
        private System.Windows.Forms.Button btnRestoreOfficialsBackup;
        private System.Windows.Forms.Button btnSngPackToSetlist;
        private System.Windows.Forms.Button btnEnblDisblOfficialSongPack;
        private System.Windows.Forms.DataGridView dgvOfficialSongs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colOfficialSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOfficialEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOfficialSongPack;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOfficialPath;
        private AbortableBackgroundWorker bWorker;
        private System.Windows.Forms.Button btnEnableDisableSelectedSM;
        private System.Windows.Forms.GroupBox groupUnsortedDLCs;
        private System.Windows.Forms.CheckBox checkSearchInAllSetlists;
        private CueTextBox tbUnsortedSearch;
        private System.Windows.Forms.Button btnDeleteSelectedSongs;
        private System.Windows.Forms.Button btnEnableDisableSelectedSongs;
        private System.Windows.Forms.DataGridView dgvUnsortedDLCs;
        private System.Windows.Forms.Button btnMoveSongToSetlist;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDLCSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDLCPath;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colUnsortedSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnsortedEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnsortedArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnsortedSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnsortedTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnsortedPath;
    }
}
