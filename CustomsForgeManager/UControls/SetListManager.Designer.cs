using System.Windows.Forms;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gbSetlistSongs = new System.Windows.Forms.GroupBox();
            this.gbSetlist = new System.Windows.Forms.GroupBox();
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
            this.btnToggleSetlist = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.gbDlcSongs = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnToggleDlcSongs = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnEnDiDlcSongs = new System.Windows.Forms.Button();
            this.btnAddDlcSongs = new System.Windows.Forms.Button();
            this.gbSongPacks = new System.Windows.Forms.GroupBox();
            this.chkEnableDelete = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEnDiSongPack = new System.Windows.Forms.Button();
            this.gbSearch = new System.Windows.Forms.GroupBox();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRunRSWithSetlist = new System.Windows.Forms.Button();
            this.lnkOpenSngMgrHelp = new System.Windows.Forms.LinkLabel();
            this.dgvSetlistSongs = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.dgvSetlists = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSetlistSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDlcSongs = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colBass = new System.Windows.Forms.DataGridViewImageColumn();
            this.colLead = new System.Windows.Forms.DataGridViewImageColumn();
            this.colRhythm = new System.Windows.Forms.DataGridViewImageColumn();
            this.colVocals = new System.Windows.Forms.DataGridViewImageColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIgnitionVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrangements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToolkitVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colATA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSongPacks = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSongPackSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSongPackEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongPackPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cueDlcSongsSearch = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.colSetlistSongsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSetlistSongsBass = new System.Windows.Forms.DataGridViewImageColumn();
            this.colSetlistSongsLead = new System.Windows.Forms.DataGridViewImageColumn();
            this.colSetlistSongsRhythm = new System.Windows.Forms.DataGridViewImageColumn();
            this.colSetlistSongsVocals = new System.Windows.Forms.DataGridViewImageColumn();
            this.colSetlistSongsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsSongYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsIgnitionID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsIgnitionUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsIgnitionAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsIgnitionVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsArrangements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsToolkitVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistSongsATA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbSetlistSongs.SuspendLayout();
            this.gbSetlist.SuspendLayout();
            this.gbButtons.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.gbDlcSongs.SuspendLayout();
            this.gbSongPacks.SuspendLayout();
            this.gbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistSongs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDlcSongs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).BeginInit();
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.06482F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.93518F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 536F));
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
            this.gbSetlistSongs.Text = "SetlistsSongs";
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
            this.gbButtons.Controls.Add(this.btnToggleSetlist);
            this.gbButtons.Controls.Add(this.label14);
            this.gbButtons.Controls.Add(this.label10);
            this.gbButtons.Controls.Add(this.label13);
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
            this.btnCreateSetlist.Location = new System.Drawing.Point(10, 10);
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
            this.btnToggleSetlistSong.Location = new System.Drawing.Point(10, 106);
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
            this.label16.Location = new System.Drawing.Point(38, 110);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(154, 13);
            this.label16.TabIndex = 60;
            this.label16.Text = "Toggle selected songs in setlist";
            // 
            // btnEnDiSetlist
            // 
            this.btnEnDiSetlist.Location = new System.Drawing.Point(10, 58);
            this.btnEnDiSetlist.Name = "btnEnDiSetlist";
            this.btnEnDiSetlist.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiSetlist.TabIndex = 49;
            this.btnEnDiSetlist.Text = "E";
            this.btnEnDiSetlist.UseVisualStyleBackColor = true;
            this.btnEnDiSetlist.Click += new System.EventHandler(this.btnEnDiSetlist_Click);
            // 
            // btnRemoveSetlist
            // 
            this.btnRemoveSetlist.Location = new System.Drawing.Point(10, 82);
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
            this.label15.Location = new System.Drawing.Point(38, 134);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(158, 13);
            this.label15.TabIndex = 58;
            this.label15.Text = "Enable/disable selected song(s)";
            // 
            // btnEnDiSetlistSong
            // 
            this.btnEnDiSetlistSong.Location = new System.Drawing.Point(10, 130);
            this.btnEnDiSetlistSong.Name = "btnEnDiSetlistSong";
            this.btnEnDiSetlistSong.Size = new System.Drawing.Size(22, 22);
            this.btnEnDiSetlistSong.TabIndex = 57;
            this.btnEnDiSetlistSong.Text = "E";
            this.btnEnDiSetlistSong.UseVisualStyleBackColor = true;
            this.btnEnDiSetlistSong.Click += new System.EventHandler(this.btnEnDiSetlistSong_Click);
            // 
            // btnToggleSetlist
            // 
            this.btnToggleSetlist.Location = new System.Drawing.Point(10, 34);
            this.btnToggleSetlist.Name = "btnToggleSetlist";
            this.btnToggleSetlist.Size = new System.Drawing.Size(22, 22);
            this.btnToggleSetlist.TabIndex = 52;
            this.btnToggleSetlist.Text = "T";
            this.btnToggleSetlist.UseVisualStyleBackColor = true;
            this.btnToggleSetlist.Click += new System.EventHandler(this.btnToggleSetlists_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 86);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 13);
            this.label14.TabIndex = 56;
            this.label14.Text = "Remove selected setlist(s)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Create new setlist";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(38, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 13);
            this.label13.TabIndex = 55;
            this.label13.Text = "Toggle selected setlists";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(38, 62);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(161, 13);
            this.label12.TabIndex = 54;
            this.label12.Text = "Enable/disable selected setlist(s)";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.65517F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.34483F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 258F));
            this.tableLayoutPanel3.Controls.Add(this.gbDlcSongs, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.gbSongPacks, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.gbSearch, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 229);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(988, 260);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // gbDlcSongs
            // 
            this.gbDlcSongs.Controls.Add(this.label5);
            this.gbDlcSongs.Controls.Add(this.btnToggleDlcSongs);
            this.gbDlcSongs.Controls.Add(this.label9);
            this.gbDlcSongs.Controls.Add(this.label8);
            this.gbDlcSongs.Controls.Add(this.btnEnDiDlcSongs);
            this.gbDlcSongs.Controls.Add(this.btnAddDlcSongs);
            this.gbDlcSongs.Controls.Add(this.dgvDlcSongs);
            this.gbDlcSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDlcSongs.Location = new System.Drawing.Point(1, 1);
            this.gbDlcSongs.Margin = new System.Windows.Forms.Padding(1);
            this.gbDlcSongs.Name = "gbDlcSongs";
            this.gbDlcSongs.Size = new System.Drawing.Size(564, 258);
            this.gbDlcSongs.TabIndex = 8;
            this.gbDlcSongs.TabStop = false;
            this.gbDlcSongs.Text = "DLC Songs";
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
            // gbSearch
            // 
            this.gbSearch.Controls.Add(this.lnkClearSearch);
            this.gbSearch.Controls.Add(this.label1);
            this.gbSearch.Controls.Add(this.btnRunRSWithSetlist);
            this.gbSearch.Controls.Add(this.cueDlcSongsSearch);
            this.gbSearch.Controls.Add(this.lnkOpenSngMgrHelp);
            this.gbSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSearch.Location = new System.Drawing.Point(569, 3);
            this.gbSearch.Name = "gbSearch";
            this.gbSearch.Size = new System.Drawing.Size(157, 254);
            this.gbSearch.TabIndex = 7;
            this.gbSearch.TabStop = false;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.Color.Black;
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
            this.label1.Location = new System.Drawing.Point(22, 212);
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
            this.btnRunRSWithSetlist.Location = new System.Drawing.Point(28, 162);
            this.btnRunRSWithSetlist.Name = "btnRunRSWithSetlist";
            this.btnRunRSWithSetlist.Size = new System.Drawing.Size(94, 44);
            this.btnRunRSWithSetlist.TabIndex = 46;
            this.btnRunRSWithSetlist.UseVisualStyleBackColor = true;
            this.btnRunRSWithSetlist.Click += new System.EventHandler(this.btnRunRSWithSetlist_Click);
            // 
            // lnkOpenSngMgrHelp
            // 
            this.lnkOpenSngMgrHelp.AutoSize = true;
            this.lnkOpenSngMgrHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkOpenSngMgrHelp.ForeColor = System.Drawing.Color.Black;
            this.lnkOpenSngMgrHelp.LinkColor = System.Drawing.Color.Black;
            this.lnkOpenSngMgrHelp.Location = new System.Drawing.Point(42, 98);
            this.lnkOpenSngMgrHelp.Name = "lnkOpenSngMgrHelp";
            this.lnkOpenSngMgrHelp.Size = new System.Drawing.Size(70, 16);
            this.lnkOpenSngMgrHelp.TabIndex = 45;
            this.lnkOpenSngMgrHelp.TabStop = true;
            this.lnkOpenSngMgrHelp.Text = "Open help";
            this.lnkOpenSngMgrHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenSngMgrHelp_LinkClicked);
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
            this.colSetlistSongsBass,
            this.colSetlistSongsLead,
            this.colSetlistSongsRhythm,
            this.colSetlistSongsVocals,
            this.colSetlistSongsEnabled,
            this.colSetlistSongsSongArtist,
            this.colSetlistSongsSongTitle,
            this.colSetlistSongsSongAlbum,
            this.colSetlistSongsSongTuning,
            this.colSetlistSongsDD,
            this.colSetlistSongsSongYear,
            this.colSetlistSongsUpdated,
            this.colSetlistSongsIgnitionID,
            this.colSetlistSongsIgnitionUpdated,
            this.colSetlistSongsIgnitionAuthor,
            this.colSetlistSongsIgnitionVersion,
            this.colSetlistSongsVersion,
            this.colSetlistSongsStatus,
            this.colSetlistSongsArrangements,
            this.colSetlistSongsAuthor,
            this.colSetlistSongsPath,
            this.colSetlistSongsFileName,
            this.colSetlistSongsToolkitVersion,
            this.colSetlistSongsATA});
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
            // dgvDlcSongs
            // 
            this.dgvDlcSongs.AllowUserToAddRows = false;
            this.dgvDlcSongs.AllowUserToDeleteRows = false;
            this.dgvDlcSongs.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDlcSongs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvDlcSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDlcSongs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvDlcSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colBass,
            this.colLead,
            this.colRhythm,
            this.colVocals,
            this.colEnabled,
            this.colSongArtist,
            this.colSongTitle,
            this.colSongAlbum,
            this.colSongTuning,
            this.colDD,
            this.colSongYear,
            this.colUpdated,
            this.colIgnitionID,
            this.colIgnitionUpdated,
            this.colIgnitionAuthor,
            this.colIgnitionVersion,
            this.colVersion,
            this.colStatus,
            this.colArrangements,
            this.colAuthor,
            this.colPath,
            this.colFileName,
            this.colToolkitVersion,
            this.colATA});
            this.dgvDlcSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDlcSongs.Location = new System.Drawing.Point(6, 16);
            this.dgvDlcSongs.Name = "dgvDlcSongs";
            this.dgvDlcSongs.RowHeadersVisible = false;
            this.dgvDlcSongs.Size = new System.Drawing.Size(552, 212);
            this.dgvDlcSongs.TabIndex = 32;
            this.dgvDlcSongs.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDlcSongs_CellMouseUp);
            this.dgvDlcSongs.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDlcSongs_ColumnHeaderMouseClick);
            this.dgvDlcSongs.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDlcSongs_DataBindingComplete);
            this.dgvDlcSongs.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDlcSongs_Paint);
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
            // colBass
            // 
            this.colBass.DataPropertyName = "Bass";
            this.colBass.HeaderText = "B";
            this.colBass.Image = global::CustomsForgeManager.Properties.Resources.Letter_B;
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
            this.colLead.Image = global::CustomsForgeManager.Properties.Resources.Letter_L;
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
            this.colRhythm.Image = global::CustomsForgeManager.Properties.Resources.Letter_R;
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
            this.colVocals.Image = global::CustomsForgeManager.Properties.Resources.Letter_V;
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
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEnabled.DefaultCellStyle = dataGridViewCellStyle9;
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Width = 50;
            // 
            // colSongArtist
            // 
            this.colSongArtist.DataPropertyName = "Artist";
            this.colSongArtist.HeaderText = "Artist";
            this.colSongArtist.Name = "colSongArtist";
            this.colSongArtist.ReadOnly = true;
            // 
            // colSongTitle
            // 
            this.colSongTitle.DataPropertyName = "Song";
            this.colSongTitle.HeaderText = "Song Title";
            this.colSongTitle.Name = "colSongTitle";
            this.colSongTitle.ReadOnly = true;
            // 
            // colSongAlbum
            // 
            this.colSongAlbum.DataPropertyName = "Album";
            this.colSongAlbum.HeaderText = "Album";
            this.colSongAlbum.Name = "colSongAlbum";
            this.colSongAlbum.ReadOnly = true;
            // 
            // colSongTuning
            // 
            this.colSongTuning.DataPropertyName = "Tuning";
            this.colSongTuning.HeaderText = "Tuning";
            this.colSongTuning.Name = "colSongTuning";
            this.colSongTuning.ReadOnly = true;
            this.colSongTuning.Width = 70;
            // 
            // colDD
            // 
            this.colDD.DataPropertyName = "DD";
            this.colDD.HeaderText = "DD";
            this.colDD.Name = "colDD";
            this.colDD.ReadOnly = true;
            this.colDD.Visible = false;
            this.colDD.Width = 50;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
            this.colSongYear.HeaderText = "Year";
            this.colSongYear.Name = "colSongYear";
            this.colSongYear.ReadOnly = true;
            this.colSongYear.Visible = false;
            this.colSongYear.Width = 50;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "Updated";
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Visible = false;
            this.colUpdated.Width = 50;
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
            this.colIgnitionUpdated.HeaderText = "Ignition Updated";
            this.colIgnitionUpdated.Name = "colIgnitionUpdated";
            this.colIgnitionUpdated.ReadOnly = true;
            this.colIgnitionUpdated.Visible = false;
            this.colIgnitionUpdated.Width = 50;
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
            // colIgnitionVersion
            // 
            this.colIgnitionVersion.DataPropertyName = "IgnitionVersion";
            this.colIgnitionVersion.HeaderText = "Ignition Version";
            this.colIgnitionVersion.Name = "colIgnitionVersion";
            this.colIgnitionVersion.ReadOnly = true;
            this.colIgnitionVersion.Visible = false;
            this.colIgnitionVersion.Width = 50;
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
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Visible = false;
            this.colStatus.Width = 50;
            // 
            // colArrangements
            // 
            this.colArrangements.DataPropertyName = "Arrangements";
            this.colArrangements.HeaderText = "Arrangements";
            this.colArrangements.Name = "colArrangements";
            this.colArrangements.ReadOnly = true;
            this.colArrangements.Visible = false;
            this.colArrangements.Width = 50;
            // 
            // colAuthor
            // 
            this.colAuthor.DataPropertyName = "Author";
            this.colAuthor.HeaderText = "Author";
            this.colAuthor.Name = "colAuthor";
            this.colAuthor.ReadOnly = true;
            this.colAuthor.Visible = false;
            this.colAuthor.Width = 50;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 350;
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
            // colToolkitVersion
            // 
            this.colToolkitVersion.DataPropertyName = "ToolkitVer";
            this.colToolkitVersion.HeaderText = "Toolkit Version";
            this.colToolkitVersion.Name = "colToolkitVersion";
            this.colToolkitVersion.ReadOnly = true;
            this.colToolkitVersion.Visible = false;
            this.colToolkitVersion.Width = 50;
            // 
            // colATA
            // 
            this.colATA.DataPropertyName = "ArtistTitleAlbum";
            this.colATA.HeaderText = "ArtistTitleAlbum";
            this.colATA.Name = "colATA";
            this.colATA.Visible = false;
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
            this.colSongPackPath.HeaderText = "Path";
            this.colSongPackPath.Name = "colSongPackPath";
            this.colSongPackPath.ReadOnly = true;
            this.colSongPackPath.Width = 300;
            // 
            // cueDlcSongsSearch
            // 
            this.cueDlcSongsSearch.Cue = "Search";
            this.cueDlcSongsSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueDlcSongsSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueDlcSongsSearch.Location = new System.Drawing.Point(6, 16);
            this.cueDlcSongsSearch.Name = "cueDlcSongsSearch";
            this.cueDlcSongsSearch.Size = new System.Drawing.Size(144, 20);
            this.cueDlcSongsSearch.TabIndex = 44;
            this.cueDlcSongsSearch.TextChanged += new System.EventHandler(this.cueDlcSongsSearch_TextChanged);
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
            // colSetlistSongsBass
            // 
            this.colSetlistSongsBass.DataPropertyName = "Bass";
            this.colSetlistSongsBass.HeaderText = "B";
            this.colSetlistSongsBass.Image = global::CustomsForgeManager.Properties.Resources.Letter_B;
            this.colSetlistSongsBass.MinimumWidth = 21;
            this.colSetlistSongsBass.Name = "colSetlistSongsBass";
            this.colSetlistSongsBass.ReadOnly = true;
            this.colSetlistSongsBass.ToolTipText = "Bass";
            this.colSetlistSongsBass.Visible = false;
            this.colSetlistSongsBass.Width = 21;
            // 
            // colSetlistSongsLead
            // 
            this.colSetlistSongsLead.DataPropertyName = "Lead";
            this.colSetlistSongsLead.HeaderText = "L";
            this.colSetlistSongsLead.Image = global::CustomsForgeManager.Properties.Resources.Letter_L;
            this.colSetlistSongsLead.MinimumWidth = 21;
            this.colSetlistSongsLead.Name = "colSetlistSongsLead";
            this.colSetlistSongsLead.ReadOnly = true;
            this.colSetlistSongsLead.ToolTipText = "Lead";
            this.colSetlistSongsLead.Visible = false;
            this.colSetlistSongsLead.Width = 21;
            // 
            // colSetlistSongsRhythm
            // 
            this.colSetlistSongsRhythm.DataPropertyName = "Rhythm";
            this.colSetlistSongsRhythm.HeaderText = "R";
            this.colSetlistSongsRhythm.Image = global::CustomsForgeManager.Properties.Resources.Letter_R;
            this.colSetlistSongsRhythm.MinimumWidth = 21;
            this.colSetlistSongsRhythm.Name = "colSetlistSongsRhythm";
            this.colSetlistSongsRhythm.ReadOnly = true;
            this.colSetlistSongsRhythm.ToolTipText = "Rhythm";
            this.colSetlistSongsRhythm.Visible = false;
            this.colSetlistSongsRhythm.Width = 21;
            // 
            // colSetlistSongsVocals
            // 
            this.colSetlistSongsVocals.DataPropertyName = "Vocals";
            this.colSetlistSongsVocals.HeaderText = "V";
            this.colSetlistSongsVocals.Image = global::CustomsForgeManager.Properties.Resources.Letter_V;
            this.colSetlistSongsVocals.MinimumWidth = 21;
            this.colSetlistSongsVocals.Name = "colSetlistSongsVocals";
            this.colSetlistSongsVocals.ReadOnly = true;
            this.colSetlistSongsVocals.ToolTipText = "Vocals";
            this.colSetlistSongsVocals.Visible = false;
            this.colSetlistSongsVocals.Width = 21;
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
            this.colSetlistSongsSongTitle.DataPropertyName = "Song";
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
            this.colSetlistSongsDD.Visible = false;
            this.colSetlistSongsDD.Width = 50;
            // 
            // colSetlistSongsSongYear
            // 
            this.colSetlistSongsSongYear.DataPropertyName = "SongYear";
            this.colSetlistSongsSongYear.HeaderText = "Year";
            this.colSetlistSongsSongYear.Name = "colSetlistSongsSongYear";
            this.colSetlistSongsSongYear.ReadOnly = true;
            this.colSetlistSongsSongYear.Visible = false;
            this.colSetlistSongsSongYear.Width = 50;
            // 
            // colSetlistSongsUpdated
            // 
            this.colSetlistSongsUpdated.DataPropertyName = "Updated";
            this.colSetlistSongsUpdated.HeaderText = "Updated";
            this.colSetlistSongsUpdated.Name = "colSetlistSongsUpdated";
            this.colSetlistSongsUpdated.ReadOnly = true;
            this.colSetlistSongsUpdated.Visible = false;
            this.colSetlistSongsUpdated.Width = 50;
            // 
            // colSetlistSongsIgnitionID
            // 
            this.colSetlistSongsIgnitionID.DataPropertyName = "IgnitionID";
            this.colSetlistSongsIgnitionID.HeaderText = "Ignition ID";
            this.colSetlistSongsIgnitionID.Name = "colSetlistSongsIgnitionID";
            this.colSetlistSongsIgnitionID.ReadOnly = true;
            this.colSetlistSongsIgnitionID.Visible = false;
            this.colSetlistSongsIgnitionID.Width = 50;
            // 
            // colSetlistSongsIgnitionUpdated
            // 
            this.colSetlistSongsIgnitionUpdated.DataPropertyName = "IgnitionUpdated";
            this.colSetlistSongsIgnitionUpdated.HeaderText = "Ignition Updated";
            this.colSetlistSongsIgnitionUpdated.Name = "colSetlistSongsIgnitionUpdated";
            this.colSetlistSongsIgnitionUpdated.ReadOnly = true;
            this.colSetlistSongsIgnitionUpdated.Visible = false;
            this.colSetlistSongsIgnitionUpdated.Width = 50;
            // 
            // colSetlistSongsIgnitionAuthor
            // 
            this.colSetlistSongsIgnitionAuthor.DataPropertyName = "IgnitionAuthor";
            this.colSetlistSongsIgnitionAuthor.HeaderText = "Ignition Author";
            this.colSetlistSongsIgnitionAuthor.Name = "colSetlistSongsIgnitionAuthor";
            this.colSetlistSongsIgnitionAuthor.ReadOnly = true;
            this.colSetlistSongsIgnitionAuthor.Visible = false;
            this.colSetlistSongsIgnitionAuthor.Width = 50;
            // 
            // colSetlistSongsIgnitionVersion
            // 
            this.colSetlistSongsIgnitionVersion.DataPropertyName = "IgnitionVersion";
            this.colSetlistSongsIgnitionVersion.HeaderText = "Ignition Version";
            this.colSetlistSongsIgnitionVersion.Name = "colSetlistSongsIgnitionVersion";
            this.colSetlistSongsIgnitionVersion.ReadOnly = true;
            this.colSetlistSongsIgnitionVersion.Visible = false;
            this.colSetlistSongsIgnitionVersion.Width = 50;
            // 
            // colSetlistSongsVersion
            // 
            this.colSetlistSongsVersion.DataPropertyName = "Version";
            this.colSetlistSongsVersion.HeaderText = "Version";
            this.colSetlistSongsVersion.Name = "colSetlistSongsVersion";
            this.colSetlistSongsVersion.ReadOnly = true;
            this.colSetlistSongsVersion.Visible = false;
            this.colSetlistSongsVersion.Width = 50;
            // 
            // colSetlistSongsStatus
            // 
            this.colSetlistSongsStatus.DataPropertyName = "Status";
            this.colSetlistSongsStatus.HeaderText = "Status";
            this.colSetlistSongsStatus.Name = "colSetlistSongsStatus";
            this.colSetlistSongsStatus.ReadOnly = true;
            this.colSetlistSongsStatus.Visible = false;
            this.colSetlistSongsStatus.Width = 50;
            // 
            // colSetlistSongsArrangements
            // 
            this.colSetlistSongsArrangements.DataPropertyName = "Arrangements";
            this.colSetlistSongsArrangements.HeaderText = "Arrangements";
            this.colSetlistSongsArrangements.Name = "colSetlistSongsArrangements";
            this.colSetlistSongsArrangements.ReadOnly = true;
            this.colSetlistSongsArrangements.Visible = false;
            this.colSetlistSongsArrangements.Width = 50;
            // 
            // colSetlistSongsAuthor
            // 
            this.colSetlistSongsAuthor.DataPropertyName = "Author";
            this.colSetlistSongsAuthor.HeaderText = "Author";
            this.colSetlistSongsAuthor.Name = "colSetlistSongsAuthor";
            this.colSetlistSongsAuthor.ReadOnly = true;
            this.colSetlistSongsAuthor.Visible = false;
            this.colSetlistSongsAuthor.Width = 50;
            // 
            // colSetlistSongsPath
            // 
            this.colSetlistSongsPath.DataPropertyName = "Path";
            this.colSetlistSongsPath.HeaderText = "Path";
            this.colSetlistSongsPath.Name = "colSetlistSongsPath";
            this.colSetlistSongsPath.ReadOnly = true;
            this.colSetlistSongsPath.Width = 350;
            // 
            // colSetlistSongsFileName
            // 
            this.colSetlistSongsFileName.DataPropertyName = "FileName";
            this.colSetlistSongsFileName.HeaderText = "File Name";
            this.colSetlistSongsFileName.Name = "colSetlistSongsFileName";
            this.colSetlistSongsFileName.ReadOnly = true;
            this.colSetlistSongsFileName.Visible = false;
            this.colSetlistSongsFileName.Width = 50;
            // 
            // colSetlistSongsToolkitVersion
            // 
            this.colSetlistSongsToolkitVersion.DataPropertyName = "ToolkitVer";
            this.colSetlistSongsToolkitVersion.HeaderText = "Toolkit Version";
            this.colSetlistSongsToolkitVersion.Name = "colSetlistSongsToolkitVersion";
            this.colSetlistSongsToolkitVersion.ReadOnly = true;
            this.colSetlistSongsToolkitVersion.Visible = false;
            this.colSetlistSongsToolkitVersion.Width = 50;
            // 
            // colSetlistSongsATA
            // 
            this.colSetlistSongsATA.DataPropertyName = "ArtistTitleAlbum";
            this.colSetlistSongsATA.HeaderText = "ATA";
            this.colSetlistSongsATA.Name = "colSetlistSongsATA";
            this.colSetlistSongsATA.Visible = false;
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
            this.gbSetlist.ResumeLayout(false);
            this.gbButtons.ResumeLayout(false);
            this.gbButtons.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.gbDlcSongs.ResumeLayout(false);
            this.gbDlcSongs.PerformLayout();
            this.gbSongPacks.ResumeLayout(false);
            this.gbSongPacks.PerformLayout();
            this.gbSearch.ResumeLayout(false);
            this.gbSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlistSongs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetlists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDlcSongs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gbSetlist;
        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvSetlists;
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
        private System.Windows.Forms.Button btnToggleSetlist;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gbSetlistSongs;
        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvSetlistSongs;
        private System.Windows.Forms.GroupBox gbSongPacks;
        private System.Windows.Forms.CheckBox chkEnableDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnEnDiSongPack;
        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvSongPacks;
        private System.Windows.Forms.GroupBox gbSearch;
        private System.Windows.Forms.GroupBox gbDlcSongs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnToggleDlcSongs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnEnDiDlcSongs;
        private System.Windows.Forms.Button btnAddDlcSongs;
        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvDlcSongs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRunRSWithSetlist;
        private CustomsForgeManagerLib.CustomControls.CueTextBox cueDlcSongsSearch;
        private System.Windows.Forms.LinkLabel lnkOpenSngMgrHelp;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSongPackSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongPackEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongPackPath;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewImageColumn colBass;
        private DataGridViewImageColumn colLead;
        private DataGridViewImageColumn colRhythm;
        private DataGridViewImageColumn colVocals;
        private DataGridViewTextBoxColumn colEnabled;
        private DataGridViewTextBoxColumn colSongArtist;
        private DataGridViewTextBoxColumn colSongTitle;
        private DataGridViewTextBoxColumn colSongAlbum;
        private DataGridViewTextBoxColumn colSongTuning;
        private DataGridViewTextBoxColumn colDD;
        private DataGridViewTextBoxColumn colSongYear;
        private DataGridViewTextBoxColumn colUpdated;
        private DataGridViewTextBoxColumn colIgnitionID;
        private DataGridViewTextBoxColumn colIgnitionUpdated;
        private DataGridViewTextBoxColumn colIgnitionAuthor;
        private DataGridViewTextBoxColumn colIgnitionVersion;
        private DataGridViewTextBoxColumn colVersion;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colArrangements;
        private DataGridViewTextBoxColumn colAuthor;
        private DataGridViewTextBoxColumn colPath;
        private DataGridViewTextBoxColumn colFileName;
        private DataGridViewTextBoxColumn colToolkitVersion;
        private DataGridViewTextBoxColumn colATA;
        private DataGridViewCheckBoxColumn colSetlistSongsSelect;
        private DataGridViewImageColumn colSetlistSongsBass;
        private DataGridViewImageColumn colSetlistSongsLead;
        private DataGridViewImageColumn colSetlistSongsRhythm;
        private DataGridViewImageColumn colSetlistSongsVocals;
        private DataGridViewTextBoxColumn colSetlistSongsEnabled;
        private DataGridViewTextBoxColumn colSetlistSongsSongArtist;
        private DataGridViewTextBoxColumn colSetlistSongsSongTitle;
        private DataGridViewTextBoxColumn colSetlistSongsSongAlbum;
        private DataGridViewTextBoxColumn colSetlistSongsSongTuning;
        private DataGridViewTextBoxColumn colSetlistSongsDD;
        private DataGridViewTextBoxColumn colSetlistSongsSongYear;
        private DataGridViewTextBoxColumn colSetlistSongsUpdated;
        private DataGridViewTextBoxColumn colSetlistSongsIgnitionID;
        private DataGridViewTextBoxColumn colSetlistSongsIgnitionUpdated;
        private DataGridViewTextBoxColumn colSetlistSongsIgnitionAuthor;
        private DataGridViewTextBoxColumn colSetlistSongsIgnitionVersion;
        private DataGridViewTextBoxColumn colSetlistSongsVersion;
        private DataGridViewTextBoxColumn colSetlistSongsStatus;
        private DataGridViewTextBoxColumn colSetlistSongsArrangements;
        private DataGridViewTextBoxColumn colSetlistSongsAuthor;
        private DataGridViewTextBoxColumn colSetlistSongsPath;
        private DataGridViewTextBoxColumn colSetlistSongsFileName;
        private DataGridViewTextBoxColumn colSetlistSongsToolkitVersion;
        private DataGridViewTextBoxColumn colSetlistSongsATA;




    }
}
