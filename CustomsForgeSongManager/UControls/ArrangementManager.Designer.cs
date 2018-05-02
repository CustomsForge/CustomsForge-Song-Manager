using System.Windows.Forms;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class ArrangementManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrangementManager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.gb_Main_Search = new System.Windows.Forms.GroupBox();
            this.lnkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.gb_Main_Grid = new System.Windows.Forms.GroupBox();
            this.lnklblToggle = new System.Windows.Forms.LinkLabel();
            this.dgvArrangements = new DataGridViewTools.RADataGridView();
            this.cmsArrangementsColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiHelp = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.colKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPersistentID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangementName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colCapoFret = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuningPitch = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTones = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDDMax = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAvgTempo = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colUpdated = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colCharter = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colToolkitVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageComment = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colBassPick = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colNoteCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChordCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCordNamesCounts = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colOctaveCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVibratoCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHammerOnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBendCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPullOffCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHarmonicCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFretHandMuteCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPalmMuteCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPluckCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSlapCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSlideCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSustainCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTremoloCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPinchHarmonicCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnpitchedSlideCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalTapCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHighestFretUsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileSize = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colOfficialDLC = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIsRsCompPack = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTagged = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbumDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.panelSearch.SuspendLayout();
            this.gb_Main_Search.SuspendLayout();
            this.gb_Main_Grid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrangements)).BeginInit();
            this.cmsArrangementsColumns.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSearch
            // 
            this.panelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearch.Controls.Add(this.lnkClearSearch);
            this.panelSearch.Controls.Add(this.lbl_Search);
            this.panelSearch.Controls.Add(this.cueSearch);
            this.panelSearch.Location = new System.Drawing.Point(6, 19);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(879, 33);
            this.panelSearch.TabIndex = 5;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(411, 8);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 3;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
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
            this.toolTip.SetToolTip(this.lnkLblSelectAll, "ODLC are not selectable");
            this.lnkLblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSelectAll_LinkClicked);
            // 
            // gb_Main_Grid
            // 
            this.gb_Main_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main_Grid.Controls.Add(this.lnkLblSelectAll);
            this.gb_Main_Grid.Controls.Add(this.lnklblToggle);
            this.gb_Main_Grid.Controls.Add(this.dgvArrangements);
            this.gb_Main_Grid.Location = new System.Drawing.Point(3, 27);
            this.gb_Main_Grid.Name = "gb_Main_Grid";
            this.gb_Main_Grid.Size = new System.Drawing.Size(891, 394);
            this.gb_Main_Grid.TabIndex = 8;
            this.gb_Main_Grid.TabStop = false;
            this.gb_Main_Grid.Text = "Arrangements Grid (Read Only):";
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
            this.toolTip.SetToolTip(this.lnklblToggle, "ODLC are not toggleable");
            this.lnklblToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblToggle_LinkClicked);
            // 
            // dgvArrangements
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvArrangements.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvArrangements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvArrangements.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvArrangements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colKey,
            this.colSelect,
            this.colPersistentID,
            this.colArrangementName,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colSongYear,
            this.colCapoFret,
            this.colTuningPitch,
            this.colTuning,
            this.colTones,
            this.colDDMax,
            this.colSongLength,
            this.colAvgTempo,
            this.colUpdated,
            this.colAppID,
            this.colCharter,
            this.colVersion,
            this.colToolkitVersion,
            this.colPackageComment,
            this.colBassPick,
            this.colNoteCount,
            this.colChordCount,
            this.colCordNamesCounts,
            this.colOctaveCount,
            this.colVibratoCount,
            this.colHammerOnCount,
            this.colBendCount,
            this.colPullOffCount,
            this.colHarmonicCount,
            this.colFretHandMuteCount,
            this.colPalmMuteCount,
            this.colPluckCount,
            this.colSlapCount,
            this.colSlideCount,
            this.colSustainCount,
            this.colTremoloCount,
            this.colPinchHarmonicCount,
            this.colUnpitchedSlideCount,
            this.colTotalTapCount,
            this.colHighestFretUsed,
            this.colFilePath,
            this.colFileName,
            this.colFileDate,
            this.colFileSize,
            this.colOfficialDLC,
            this.colIsRsCompPack,
            this.colTagged,
            this.colRepairStatus,
            this.colArtistTitleAlbum,
            this.colArtistTitleAlbumDate});
            this.dgvArrangements.Location = new System.Drawing.Point(6, 19);
            this.dgvArrangements.Name = "dgvArrangements";
            this.dgvArrangements.RowHeadersVisible = false;
            this.dgvArrangements.Size = new System.Drawing.Size(879, 348);
            this.dgvArrangements.TabIndex = 1;
            this.dgvArrangements.Tag = "Song Manager";
            // 
            // cmsArrangementsColumns
            // 
            this.cmsArrangementsColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.cmsArrangementsColumns.Name = "cmsSongManagerColumns";
            this.cmsArrangementsColumns.Size = new System.Drawing.Size(107, 26);
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
            this.tsmiHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(899, 24);
            this.menuStrip.TabIndex = 11;
            this.menuStrip.Text = "menuStrip1";
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.AssociatedEnumValue = null;
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3});
            this.tsmiHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelp.Image")));
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.RadioButtonGroupName = null;
            this.tsmiHelp.Size = new System.Drawing.Size(56, 20);
            this.tsmiHelp.Text = "Help";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(57, 6);
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
            // colPersistentID
            // 
            this.colPersistentID.DataPropertyName = "PersistentID";
            this.colPersistentID.HeaderText = "Persistent ID";
            this.colPersistentID.Name = "colPersistentID";
            this.colPersistentID.ReadOnly = true;
            // 
            // colArrangementName
            // 
            this.colArrangementName.DataPropertyName = "Name";
            this.colArrangementName.HeaderText = "Arrangement";
            this.colArrangementName.Name = "colArrangementName";
            this.colArrangementName.ReadOnly = true;
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
            this.colAlbum.Visible = false;
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
            // colCapoFret
            // 
            this.colCapoFret.DataPropertyName = "CapoFret";
            this.colCapoFret.HeaderText = "CapoFret";
            this.colCapoFret.Name = "colCapoFret";
            this.colCapoFret.ReadOnly = true;
            this.colCapoFret.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTuningPitch
            // 
            this.colTuningPitch.DataPropertyName = "TuningPitch";
            this.colTuningPitch.HeaderText = "TuningPitch";
            this.colTuningPitch.Name = "colTuningPitch";
            // 
            // colTuning
            // 
            this.colTuning.DataPropertyName = "Tuning";
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTones
            // 
            this.colTones.DataPropertyName = "Tones";
            this.colTones.HeaderText = "Tones";
            this.colTones.Name = "colTones";
            this.colTones.ReadOnly = true;
            // 
            // colDDMax
            // 
            this.colDDMax.DataPropertyName = "DDMax";
            this.colDDMax.HeaderText = "DD Max";
            this.colDDMax.Name = "colDDMax";
            this.colDDMax.ReadOnly = true;
            this.colDDMax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "LastConversionDateTime";
            dataGridViewCellStyle5.NullValue = null;
            this.colUpdated.DefaultCellStyle = dataGridViewCellStyle5;
            this.colUpdated.HeaderText = "Last Conversion";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Width = 50;
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
            // colCharter
            // 
            this.colCharter.DataPropertyName = "CharterName";
            this.colCharter.HeaderText = "Charter";
            this.colCharter.Name = "colCharter";
            this.colCharter.ReadOnly = true;
            this.colCharter.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCharter.Visible = false;
            this.colCharter.Width = 50;
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
            this.colToolkitVersion.Width = 110;
            // 
            // colPackageComment
            // 
            this.colPackageComment.DataPropertyName = "PackageComment";
            this.colPackageComment.HeaderText = "Package Comment";
            this.colPackageComment.Name = "colPackageComment";
            this.colPackageComment.ReadOnly = true;
            this.colPackageComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageComment.Visible = false;
            // 
            // colBassPick
            // 
            this.colBassPick.DataPropertyName = "BassPick";
            this.colBassPick.HeaderText = "BassPick";
            this.colBassPick.Name = "colBassPick";
            this.colBassPick.ReadOnly = true;
            this.colBassPick.Visible = false;
            // 
            // colNoteCount
            // 
            this.colNoteCount.DataPropertyName = "NoteCount";
            this.colNoteCount.HeaderText = "Note Count";
            this.colNoteCount.Name = "colNoteCount";
            this.colNoteCount.ReadOnly = true;
            // 
            // colChordCount
            // 
            this.colChordCount.DataPropertyName = "ChordCount";
            this.colChordCount.HeaderText = "Chord Count";
            this.colChordCount.Name = "colChordCount";
            this.colChordCount.ReadOnly = true;
            // 
            // colCordNamesCounts
            // 
            this.colCordNamesCounts.DataPropertyName = "ChordNamesCounts";
            this.colCordNamesCounts.HeaderText = "Chord Names Counts";
            this.colCordNamesCounts.Name = "colCordNamesCounts";
            this.colCordNamesCounts.ReadOnly = true;
            // 
            // colOctaveCount
            // 
            this.colOctaveCount.DataPropertyName = "OctaveCount";
            this.colOctaveCount.HeaderText = "Octave Count";
            this.colOctaveCount.Name = "colOctaveCount";
            this.colOctaveCount.ReadOnly = true;
            this.colOctaveCount.Visible = false;
            // 
            // colVibratoCount
            // 
            this.colVibratoCount.DataPropertyName = "VibratoCount";
            this.colVibratoCount.HeaderText = "Vibrato Count";
            this.colVibratoCount.Name = "colVibratoCount";
            this.colVibratoCount.ReadOnly = true;
            this.colVibratoCount.Visible = false;
            // 
            // colHammerOnCount
            // 
            this.colHammerOnCount.DataPropertyName = "HammerOnCount";
            this.colHammerOnCount.HeaderText = "Hammer-On Count";
            this.colHammerOnCount.Name = "colHammerOnCount";
            this.colHammerOnCount.ReadOnly = true;
            this.colHammerOnCount.Visible = false;
            // 
            // colBendCount
            // 
            this.colBendCount.DataPropertyName = "BendCount";
            this.colBendCount.HeaderText = "Bend Count";
            this.colBendCount.Name = "colBendCount";
            this.colBendCount.ReadOnly = true;
            this.colBendCount.Visible = false;
            // 
            // colPullOffCount
            // 
            this.colPullOffCount.DataPropertyName = "PullOffCount";
            this.colPullOffCount.HeaderText = "Pull-Off Count";
            this.colPullOffCount.Name = "colPullOffCount";
            this.colPullOffCount.ReadOnly = true;
            this.colPullOffCount.Visible = false;
            // 
            // colHarmonicCount
            // 
            this.colHarmonicCount.DataPropertyName = "HarmonicCount";
            this.colHarmonicCount.HeaderText = "Harmonic Count";
            this.colHarmonicCount.Name = "colHarmonicCount";
            this.colHarmonicCount.ReadOnly = true;
            this.colHarmonicCount.Visible = false;
            // 
            // colFretHandMuteCount
            // 
            this.colFretHandMuteCount.DataPropertyName = "FretHandMuteCount";
            this.colFretHandMuteCount.HeaderText = "Frethand Mute Count";
            this.colFretHandMuteCount.Name = "colFretHandMuteCount";
            this.colFretHandMuteCount.ReadOnly = true;
            this.colFretHandMuteCount.Visible = false;
            // 
            // colPalmMuteCount
            // 
            this.colPalmMuteCount.DataPropertyName = "PalmMuteCount";
            this.colPalmMuteCount.HeaderText = "PalmMute Count";
            this.colPalmMuteCount.Name = "colPalmMuteCount";
            this.colPalmMuteCount.ReadOnly = true;
            this.colPalmMuteCount.Visible = false;
            // 
            // colPluckCount
            // 
            this.colPluckCount.DataPropertyName = "PluckCount";
            this.colPluckCount.HeaderText = "Pluck Count";
            this.colPluckCount.Name = "colPluckCount";
            this.colPluckCount.ReadOnly = true;
            this.colPluckCount.Visible = false;
            // 
            // colSlapCount
            // 
            this.colSlapCount.DataPropertyName = "SlapCount";
            this.colSlapCount.HeaderText = "Slap Count";
            this.colSlapCount.Name = "colSlapCount";
            this.colSlapCount.ReadOnly = true;
            this.colSlapCount.Visible = false;
            // 
            // colSlideCount
            // 
            this.colSlideCount.DataPropertyName = "SlideCount";
            this.colSlideCount.HeaderText = "Slide Count";
            this.colSlideCount.Name = "colSlideCount";
            this.colSlideCount.ReadOnly = true;
            this.colSlideCount.Visible = false;
            // 
            // colSustainCount
            // 
            this.colSustainCount.DataPropertyName = "SustainCount";
            this.colSustainCount.HeaderText = "Sustain Count";
            this.colSustainCount.Name = "colSustainCount";
            this.colSustainCount.ReadOnly = true;
            this.colSustainCount.Visible = false;
            // 
            // colTremoloCount
            // 
            this.colTremoloCount.DataPropertyName = "TremoloCount";
            this.colTremoloCount.HeaderText = "Tremolo Count";
            this.colTremoloCount.Name = "colTremoloCount";
            this.colTremoloCount.ReadOnly = true;
            this.colTremoloCount.Visible = false;
            // 
            // colPinchHarmonicCount
            // 
            this.colPinchHarmonicCount.DataPropertyName = "HarmonicPinchCount";
            this.colPinchHarmonicCount.HeaderText = "Pinch Harmonic Count";
            this.colPinchHarmonicCount.Name = "colPinchHarmonicCount";
            this.colPinchHarmonicCount.ReadOnly = true;
            this.colPinchHarmonicCount.Visible = false;
            // 
            // colUnpitchedSlideCount
            // 
            this.colUnpitchedSlideCount.DataPropertyName = "UnpitchedSlideCount";
            this.colUnpitchedSlideCount.HeaderText = "Unpitched Slide Count";
            this.colUnpitchedSlideCount.Name = "colUnpitchedSlideCount";
            this.colUnpitchedSlideCount.ReadOnly = true;
            this.colUnpitchedSlideCount.Visible = false;
            // 
            // colTotalTapCount
            // 
            this.colTotalTapCount.DataPropertyName = "TapCount";
            this.colTotalTapCount.HeaderText = "Tap Count";
            this.colTotalTapCount.Name = "colTotalTapCount";
            this.colTotalTapCount.ReadOnly = true;
            this.colTotalTapCount.Visible = false;
            // 
            // colHighestFretUsed
            // 
            this.colHighestFretUsed.DataPropertyName = "HighestFretUsed";
            this.colHighestFretUsed.HeaderText = "Highest Fret Used";
            this.colHighestFretUsed.Name = "colHighestFretUsed";
            this.colHighestFretUsed.ReadOnly = true;
            this.colHighestFretUsed.Visible = false;
            // 
            // colFilePath
            // 
            this.colFilePath.DataPropertyName = "FilePath";
            this.colFilePath.HeaderText = "File Path";
            this.colFilePath.Name = "colFilePath";
            this.colFilePath.ReadOnly = true;
            this.colFilePath.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFilePath.Visible = false;
            this.colFilePath.Width = 150;
            // 
            // colFileName
            // 
            this.colFileName.DataPropertyName = "FileName";
            this.colFileName.HeaderText = "File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            this.colFileName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileName.Visible = false;
            this.colFileName.Width = 50;
            // 
            // colFileDate
            // 
            this.colFileDate.DataPropertyName = "FileDate";
            this.colFileDate.HeaderText = "File Date";
            this.colFileDate.Name = "colFileDate";
            this.colFileDate.ReadOnly = true;
            this.colFileDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileDate.Visible = false;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "FileSize";
            this.colFileSize.HeaderText = "File Size (bytes)";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            this.colFileSize.Visible = false;
            // 
            // colOfficialDLC
            // 
            this.colOfficialDLC.DataPropertyName = "OfficialDLC";
            this.colOfficialDLC.HeaderText = "OfficialDLC";
            this.colOfficialDLC.Name = "colOfficialDLC";
            this.colOfficialDLC.ReadOnly = true;
            // 
            // colIsRsCompPack
            // 
            this.colIsRsCompPack.DataPropertyName = "IsRsCompPack";
            this.colIsRsCompPack.HeaderText = "IsRsCompPack";
            this.colIsRsCompPack.Name = "colIsRsCompPack";
            this.colIsRsCompPack.ReadOnly = true;
            // 
            // colTagged
            // 
            this.colTagged.DataPropertyName = "Tagged";
            this.colTagged.HeaderText = "Tagged";
            this.colTagged.Name = "colTagged";
            this.colTagged.ReadOnly = true;
            // 
            // colRepairStatus
            // 
            this.colRepairStatus.DataPropertyName = "RepairStatus";
            this.colRepairStatus.HeaderText = "RepairStatus";
            this.colRepairStatus.Name = "colRepairStatus";
            this.colRepairStatus.ReadOnly = true;
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
            this.colArtistTitleAlbumDate.ReadOnly = true;
            // 
            // ArrangementManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gb_Main_Grid);
            this.Controls.Add(this.gb_Main_Search);
            this.Controls.Add(this.menuStrip);
            this.Name = "ArrangementManager";
            this.Size = new System.Drawing.Size(899, 490);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.gb_Main_Search.ResumeLayout(false);
            this.gb_Main_Grid.ResumeLayout(false);
            this.gb_Main_Grid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrangements)).EndInit();
            this.cmsArrangementsColumns.ResumeLayout(false);
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
        private RADataGridView dgvArrangements;
        private System.Windows.Forms.LinkLabel lnkLblSelectAll;
        private System.Windows.Forms.GroupBox gb_Main_Grid;
        private System.Windows.Forms.ContextMenuStrip cmsArrangementsColumns;
        private CustomControls.ToolStripEnhancedMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.LinkLabel lnklblToggle;
        private System.Windows.Forms.MenuStrip menuStrip;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewAutoFilterTextBoxColumn colKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewAutoFilterTextBoxColumn colPersistentID;
        private DataGridViewAutoFilterTextBoxColumn colArrangementName;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewAutoFilterTextBoxColumn colCapoFret;
        private DataGridViewAutoFilterTextBoxColumn colTuningPitch;
        private DataGridViewAutoFilterTextBoxColumn colTuning;
        private DataGridViewAutoFilterTextBoxColumn colTones;
        private DataGridViewAutoFilterTextBoxColumn colDDMax;
        private DataGridViewAutoFilterTextBoxColumn colSongLength;
        private DataGridViewAutoFilterTextBoxColumn colAvgTempo;
        private DataGridViewAutoFilterTextBoxColumn colUpdated;
        private DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewAutoFilterTextBoxColumn colCharter;
        private DataGridViewAutoFilterTextBoxColumn colVersion;
        private DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageComment;
        private DataGridViewAutoFilterTextBoxColumn colBassPick;
        private DataGridViewTextBoxColumn colNoteCount;
        private DataGridViewTextBoxColumn colChordCount;
        private DataGridViewAutoFilterTextBoxColumn colCordNamesCounts;
        private DataGridViewTextBoxColumn colOctaveCount;
        private DataGridViewTextBoxColumn colVibratoCount;
        private DataGridViewTextBoxColumn colHammerOnCount;
        private DataGridViewTextBoxColumn colBendCount;
        private DataGridViewTextBoxColumn colPullOffCount;
        private DataGridViewTextBoxColumn colHarmonicCount;
        private DataGridViewTextBoxColumn colFretHandMuteCount;
        private DataGridViewTextBoxColumn colPalmMuteCount;
        private DataGridViewTextBoxColumn colPluckCount;
        private DataGridViewTextBoxColumn colSlapCount;
        private DataGridViewTextBoxColumn colSlideCount;
        private DataGridViewTextBoxColumn colSustainCount;
        private DataGridViewTextBoxColumn colTremoloCount;
        private DataGridViewTextBoxColumn colPinchHarmonicCount;
        private DataGridViewTextBoxColumn colUnpitchedSlideCount;
        private DataGridViewTextBoxColumn colTotalTapCount;
        private DataGridViewTextBoxColumn colHighestFretUsed;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colFileDate;
        private DataGridViewAutoFilterTextBoxColumn colFileSize;
        private DataGridViewAutoFilterTextBoxColumn colOfficialDLC;
        private DataGridViewAutoFilterTextBoxColumn colIsRsCompPack;
        private DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbumDate;

    }
}
