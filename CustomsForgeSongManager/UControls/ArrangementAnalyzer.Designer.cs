using System.Windows.Forms;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class ArrangementAnalyzer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrangementAnalyzer));
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lbl_Search = new System.Windows.Forms.Label();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.gb_Main_Search = new System.Windows.Forms.GroupBox();
            this.lnkLblSelectAll = new System.Windows.Forms.LinkLabel();
            this.gb_Main_Grid = new System.Windows.Forms.GroupBox();
            this.lnklblToggle = new System.Windows.Forms.LinkLabel();
            this.dgvArrangements = new DataGridViewTools.RADataGridView();
            this.colDLCKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPersistentID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangementName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitleSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbumSort = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongAverageTempo = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongVolume = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colLastConversionDateTime = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAppID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colToolkitVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPackageComment = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFilePath = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileName = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFileSize = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionVersion = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionAuthor = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnitionDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTagged = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colRepairStatus = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIsOfficialDLC = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIsRsCompPack = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colBassPick = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colCapoFret = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colDDMax = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuningPitch = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colToneBase = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTones = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTonesCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSectionsCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colNoteCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colChordCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colChordNamesCounts = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAccentCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colBendCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colFretHandMuteCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colHammerOnCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colHarmonicCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colHarmonicPinchCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colHighestFretUsed = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colIgnoreCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colLinkNextCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colOctaveCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPalmMuteCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPluckCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colPullOffCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSlapCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSlideCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSlideUnpitchToCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSustainCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTapCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTremoloCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colVibratoCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtistTitleAlbumDate = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colHopoCount = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.cmsArrangementsColumns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.testToolStripMenuItem = new CustomControls.ToolStripEnhancedMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.tsmiRescan = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanQuick = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiRescanFull = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelp = new CustomControls.ToolStripEnhancedMenuItem();
            this.tsmiHelpGeneral = new CustomControls.ToolStripEnhancedMenuItem();
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
            this.colDLCKey,
            this.colSelect,
            this.colPersistentID,
            this.colArrangementName,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colArtistSort,
            this.colTitleSort,
            this.colAlbumSort,
            this.colSongYear,
            this.colSongLength,
            this.colSongAverageTempo,
            this.colSongVolume,
            this.colLastConversionDateTime,
            this.colAppID,
            this.colToolkitVersion,
            this.colPackageAuthor,
            this.colPackageVersion,
            this.colPackageComment,
            this.colFilePath,
            this.colFileName,
            this.colFileDate,
            this.colFileSize,
            this.colIgnitionID,
            this.colIgnitionVersion,
            this.colIgnitionAuthor,
            this.colIgnitionDate,
            this.colStatus,
            this.colTagged,
            this.colRepairStatus,
            this.colIsOfficialDLC,
            this.colIsRsCompPack,
            this.colBassPick,
            this.colCapoFret,
            this.colDDMax,
            this.colTuning,
            this.colTuningPitch,
            this.colToneBase,
            this.colTones,
            this.colTonesCount,
            this.colSectionsCount,
            this.colNoteCount,
            this.colChordCount,
            this.colChordNamesCounts,
            this.colAccentCount,
            this.colBendCount,
            this.colFretHandMuteCount,
            this.colHammerOnCount,
            this.colHarmonicCount,
            this.colHarmonicPinchCount,
            this.colHighestFretUsed,
            this.colIgnoreCount,
            this.colLinkNextCount,
            this.colOctaveCount,
            this.colPalmMuteCount,
            this.colPluckCount,
            this.colPullOffCount,
            this.colSlapCount,
            this.colSlideCount,
            this.colSlideUnpitchToCount,
            this.colSustainCount,
            this.colTapCount,
            this.colTremoloCount,
            this.colVibratoCount,
            this.colArtistTitleAlbum,
            this.colArtistTitleAlbumDate});
            this.dgvArrangements.Location = new System.Drawing.Point(6, 19);
            this.dgvArrangements.Name = "dgvArrangements";
            this.dgvArrangements.RowHeadersVisible = false;
            this.dgvArrangements.Size = new System.Drawing.Size(879, 348);
            this.dgvArrangements.TabIndex = 1;
            this.dgvArrangements.Tag = "Arrangement Analyzer";
            this.dgvArrangements.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrangements_CellContentClick);
            this.dgvArrangements.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrangements_CellDoubleClick);
            this.dgvArrangements.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvArrangements_CellFormatting);
            this.dgvArrangements.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvArrangements_CellMouseDown);
            this.dgvArrangements.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvArrangements_CellMouseUp);
            this.dgvArrangements.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvArrangements_ColumnHeaderMouseClick);
            this.dgvArrangements.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvArrangements_KeyDown);
            this.dgvArrangements.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvArrangements_KeyUp);
            // 
            // colDLCKey
            // 
            this.colDLCKey.DataPropertyName = "DLCKey";
            this.colDLCKey.HeaderText = "DLCKey";
            this.colDLCKey.Name = "colDLCKey";
            this.colDLCKey.ReadOnly = true;
            this.colDLCKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDLCKey.Width = 95;
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
            this.colPersistentID.HeaderText = "PersistentID";
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
            // colArtistSort
            // 
            this.colArtistSort.DataPropertyName = "ArtistSort";
            this.colArtistSort.HeaderText = "ArtistSort";
            this.colArtistSort.Name = "colArtistSort";
            this.colArtistSort.ReadOnly = true;
            this.colArtistSort.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArtistSort.Visible = false;
            // 
            // colTitleSort
            // 
            this.colTitleSort.DataPropertyName = "TitleSort";
            this.colTitleSort.HeaderText = "SongTitleSort";
            this.colTitleSort.Name = "colTitleSort";
            this.colTitleSort.ReadOnly = true;
            this.colTitleSort.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTitleSort.Visible = false;
            // 
            // colAlbumSort
            // 
            this.colAlbumSort.DataPropertyName = "AlbumSort";
            this.colAlbumSort.HeaderText = "AlbumSort";
            this.colAlbumSort.Name = "colAlbumSort";
            this.colAlbumSort.ReadOnly = true;
            this.colAlbumSort.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAlbumSort.Visible = false;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
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
            this.colSongAverageTempo.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSongAverageTempo.HeaderText = "BPM";
            this.colSongAverageTempo.Name = "colSongAverageTempo";
            this.colSongAverageTempo.ReadOnly = true;
            // 
            // colSongVolume
            // 
            this.colSongVolume.DataPropertyName = "SongVolume";
            this.colSongVolume.DefaultCellStyle = dataGridViewCellStyle4;
            this.colSongVolume.HeaderText = "SongVolume (LF)";
            this.colSongVolume.Name = "colSongVolume";
            this.colSongVolume.ReadOnly = true;
            this.colSongVolume.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongVolume.ToolTipText = "(Loudness Factor)";
            this.colSongVolume.Visible = false;
            // 
            // colLastConversionDateTime
            // 
            this.colLastConversionDateTime.DataPropertyName = "LastConversionDateTime";
            this.colLastConversionDateTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.colLastConversionDateTime.HeaderText = "ConversionDate";
            this.colLastConversionDateTime.Name = "colLastConversionDateTime";
            this.colLastConversionDateTime.ReadOnly = true;
            this.colLastConversionDateTime.Visible = false;
            this.colLastConversionDateTime.Width = 50;
            // 
            // colAppID
            // 
            this.colAppID.DataPropertyName = "AppID";
            this.colAppID.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAppID.HeaderText = "AppID";
            this.colAppID.Name = "colAppID";
            this.colAppID.ReadOnly = true;
            this.colAppID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAppID.Visible = false;
            this.colAppID.Width = 80;
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
            this.colPackageAuthor.ReadOnly = true;
            this.colPackageAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageAuthor.Visible = false;
            // 
            // colPackageVersion
            // 
            this.colPackageVersion.DataPropertyName = "PackageVersion";
            this.colPackageVersion.HeaderText = "PackageVersion";
            this.colPackageVersion.Name = "colPackageVersion";
            this.colPackageVersion.ReadOnly = true;
            this.colPackageVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageVersion.Visible = false;
            // 
            // colPackageComment
            // 
            this.colPackageComment.DataPropertyName = "PackageComment";
            this.colPackageComment.HeaderText = "PackageComment";
            this.colPackageComment.Name = "colPackageComment";
            this.colPackageComment.ReadOnly = true;
            this.colPackageComment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPackageComment.Visible = false;
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
            // colFileDate
            // 
            this.colFileDate.DataPropertyName = "FileDate";
            this.colFileDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colFileDate.HeaderText = "FileDate";
            this.colFileDate.Name = "colFileDate";
            this.colFileDate.ReadOnly = true;
            this.colFileDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colFileDate.Visible = false;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "FileSize";
            this.colFileSize.DefaultCellStyle = dataGridViewCellStyle3;
            this.colFileSize.HeaderText = "FileSize (bytes)";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            this.colFileSize.Visible = false;
            // 
            // colIgnitionID
            // 
            this.colIgnitionID.DataPropertyName = "IgnitionID";
            this.colIgnitionID.HeaderText = "IgnitionID";
            this.colIgnitionID.Name = "colIgnitionID";
            this.colIgnitionID.ReadOnly = true;
            this.colIgnitionID.ToolTipText = "For Future Expansion";
            this.colIgnitionID.Visible = false;
            this.colIgnitionID.Width = 50;
            // 
            // colIgnitionVersion
            // 
            this.colIgnitionVersion.DataPropertyName = "IgnitionVersion";
            this.colIgnitionVersion.HeaderText = "IgnitionVersion";
            this.colIgnitionVersion.Name = "colIgnitionVersion";
            this.colIgnitionVersion.ReadOnly = true;
            this.colIgnitionVersion.ToolTipText = "For Future Expansion";
            this.colIgnitionVersion.Visible = false;
            this.colIgnitionVersion.Width = 50;
            // 
            // colIgnitionAuthor
            // 
            this.colIgnitionAuthor.DataPropertyName = "IgnitionAuthor";
            this.colIgnitionAuthor.HeaderText = "IgnitionAuthor";
            this.colIgnitionAuthor.Name = "colIgnitionAuthor";
            this.colIgnitionAuthor.ReadOnly = true;
            this.colIgnitionAuthor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIgnitionAuthor.ToolTipText = "For Future Expansion";
            this.colIgnitionAuthor.Visible = false;
            this.colIgnitionAuthor.Width = 50;
            // 
            // colIgnitionDate
            // 
            this.colIgnitionDate.DataPropertyName = "IgnitionDate";
            this.colIgnitionDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colIgnitionDate.HeaderText = "IgnitionDate";
            this.colIgnitionDate.Name = "colIgnitionDate";
            this.colIgnitionDate.ReadOnly = true;
            this.colIgnitionDate.ToolTipText = "For Future Expansion";
            this.colIgnitionDate.Visible = false;
            this.colIgnitionDate.Width = 50;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = false;
            // 
            // colTagged
            // 
            this.colTagged.DataPropertyName = "Tagged";
            this.colTagged.HeaderText = "Tagged";
            this.colTagged.Name = "colTagged";
            this.colTagged.ReadOnly = true;
            this.colTagged.Visible = false;
            // 
            // colRepairStatus
            // 
            this.colRepairStatus.DataPropertyName = "RepairStatus";
            this.colRepairStatus.HeaderText = "RepairStatus";
            this.colRepairStatus.Name = "colRepairStatus";
            this.colRepairStatus.ReadOnly = true;
            this.colRepairStatus.Visible = false;
            // 
            // colIsOfficialDLC
            // 
            this.colIsOfficialDLC.DataPropertyName = "IsOfficialDLC";
            this.colIsOfficialDLC.HeaderText = "IsOfficialDLC";
            this.colIsOfficialDLC.Name = "colIsOfficialDLC";
            this.colIsOfficialDLC.ReadOnly = true;
            this.colIsOfficialDLC.Visible = false;
            // 
            // colIsRsCompPack
            // 
            this.colIsRsCompPack.DataPropertyName = "IsRsCompPack";
            this.colIsRsCompPack.HeaderText = "IsRsCompPack";
            this.colIsRsCompPack.Name = "colIsRsCompPack";
            this.colIsRsCompPack.ReadOnly = true;
            this.colIsRsCompPack.Visible = false;
            // 
            // colBassPick
            // 
            this.colBassPick.DataPropertyName = "IsBassPick";
            this.colBassPick.HeaderText = "BassPickedFingered";
            this.colBassPick.Name = "colBassPick";
            this.colBassPick.ReadOnly = true;
            this.colBassPick.Visible = false;
            // 
            // colCapoFret
            // 
            this.colCapoFret.DataPropertyName = "CapoFret";
            this.colCapoFret.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCapoFret.HeaderText = "CapoFret";
            this.colCapoFret.Name = "colCapoFret";
            this.colCapoFret.ReadOnly = true;
            this.colCapoFret.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCapoFret.Visible = false;
            // 
            // colDDMax
            // 
            this.colDDMax.DataPropertyName = "DDMax";
            this.colDDMax.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDDMax.HeaderText = "DDMaxLevel";
            this.colDDMax.Name = "colDDMax";
            this.colDDMax.ReadOnly = true;
            this.colDDMax.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTuning
            // 
            this.colTuning.DataPropertyName = "Tuning";
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTuningPitch
            // 
            this.colTuningPitch.DataPropertyName = "TuningPitch";
            this.colTuningPitch.DefaultCellStyle = dataGridViewCellStyle4;
            this.colTuningPitch.HeaderText = "TuningPitch";
            this.colTuningPitch.Name = "colTuningPitch";
            // 
            // colToneBase
            // 
            this.colToneBase.DataPropertyName = "ToneBase";
            this.colToneBase.HeaderText = "ToneBase";
            this.colToneBase.Name = "colToneBase";
            // 
            // colTones
            // 
            this.colTones.DataPropertyName = "Tones";
            this.colTones.HeaderText = "Tones";
            this.colTones.Name = "colTones";
            this.colTones.ReadOnly = true;
            // 
            // colTonesCount
            // 
            this.colTonesCount.DataPropertyName = "TonesCount";
            this.colTonesCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTonesCount.HeaderText = "ToneSwitchCount";
            this.colTonesCount.Name = "colTonesCount";
            this.colTonesCount.ReadOnly = true;
            this.colTonesCount.ToolTipText = "Number of tone changes\r\nin the arrangement.";
            // 
            // colSectionsCount
            // 
            this.colSectionsCount.DataPropertyName = "SectionsCount";
            this.colSectionsCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSectionsCount.HeaderText = "SectionsCount";
            this.colSectionsCount.Name = "colSectionsCount";
            this.colSectionsCount.ReadOnly = true;
            // 
            // colNoteCount
            // 
            this.colNoteCount.DataPropertyName = "NoteCount";
            this.colNoteCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colNoteCount.HeaderText = "NoteCount";
            this.colNoteCount.Name = "colNoteCount";
            this.colNoteCount.ReadOnly = true;
            // 
            // colChordCount
            // 
            this.colChordCount.DataPropertyName = "ChordCount";
            this.colChordCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colChordCount.HeaderText = "ChordCount";
            this.colChordCount.Name = "colChordCount";
            this.colChordCount.ReadOnly = true;
            // 
            // colChordNamesCounts
            // 
            this.colChordNamesCounts.DataPropertyName = "ChordNamesCounts";
            this.colChordNamesCounts.HeaderText = "ChordNamesCounts";
            this.colChordNamesCounts.Name = "colChordNamesCounts";
            this.colChordNamesCounts.ReadOnly = true;
            // 
            // colAccentCount
            // 
            this.colAccentCount.DataPropertyName = "AccentCount";
            this.colAccentCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAccentCount.HeaderText = "AccentCount";
            this.colAccentCount.Name = "colAccentCount";
            this.colAccentCount.ReadOnly = true;
            this.colAccentCount.Visible = false;
            // 
            // colBendCount
            // 
            this.colBendCount.DataPropertyName = "BendCount";
            this.colBendCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colBendCount.HeaderText = "BendCount";
            this.colBendCount.Name = "colBendCount";
            this.colBendCount.ReadOnly = true;
            this.colBendCount.Visible = false;
            // 
            // colFretHandMuteCount
            // 
            this.colFretHandMuteCount.DataPropertyName = "FretHandMuteCount";
            this.colFretHandMuteCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colFretHandMuteCount.HeaderText = "FrethandMuteCount";
            this.colFretHandMuteCount.Name = "colFretHandMuteCount";
            this.colFretHandMuteCount.ReadOnly = true;
            this.colFretHandMuteCount.Visible = false;
            // 
            // colHammerOnCount
            // 
            this.colHammerOnCount.DataPropertyName = "HammerOnCount";
            this.colHammerOnCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHammerOnCount.HeaderText = "HammerOnCount";
            this.colHammerOnCount.Name = "colHammerOnCount";
            this.colHammerOnCount.ReadOnly = true;
            this.colHammerOnCount.Visible = false;
            // 
            // colHarmonicCount
            // 
            this.colHarmonicCount.DataPropertyName = "HarmonicCount";
            this.colHarmonicCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHarmonicCount.HeaderText = "HarmonicCount";
            this.colHarmonicCount.Name = "colHarmonicCount";
            this.colHarmonicCount.ReadOnly = true;
            this.colHarmonicCount.Visible = false;
            // 
            // colHarmonicPinchCount
            // 
            this.colHarmonicPinchCount.DataPropertyName = "HarmonicPinchCount";
            this.colHarmonicPinchCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHarmonicPinchCount.HeaderText = "HarmonicPinchCount";
            this.colHarmonicPinchCount.Name = "colHarmonicPinchCount";
            this.colHarmonicPinchCount.ReadOnly = true;
            this.colHarmonicPinchCount.Visible = false;
            // 
            // colHighestFretUsed
            // 
            this.colHighestFretUsed.DataPropertyName = "HighestFretUsed";
            this.colHighestFretUsed.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHighestFretUsed.HeaderText = "HighestFretUsed";
            this.colHighestFretUsed.Name = "colHighestFretUsed";
            this.colHighestFretUsed.ReadOnly = true;
            this.colHighestFretUsed.Visible = false;
            // 
            // colIgnoreCount
            // 
            this.colIgnoreCount.DataPropertyName = "IgnoreCount";
            this.colIgnoreCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colIgnoreCount.HeaderText = "IgnoreCount";
            this.colIgnoreCount.Name = "colIgnoreCount";
            this.colIgnoreCount.ReadOnly = true;
            this.colIgnoreCount.Visible = false;
            // 
            // colLinkNextCount
            // 
            this.colLinkNextCount.DataPropertyName = "LinkNextCount";
            this.colLinkNextCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colLinkNextCount.HeaderText = "LinkNextCount";
            this.colLinkNextCount.Name = "colLinkNextCount";
            this.colLinkNextCount.Visible = false;
            // 
            // colOctaveCount
            // 
            this.colOctaveCount.DataPropertyName = "OctaveCount";
            this.colOctaveCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colOctaveCount.HeaderText = "OctaveCount";
            this.colOctaveCount.Name = "colOctaveCount";
            this.colOctaveCount.ReadOnly = true;
            this.colOctaveCount.Visible = false;
            // 
            // colPalmMuteCount
            // 
            this.colPalmMuteCount.DataPropertyName = "PalmMuteCount";
            this.colPalmMuteCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPalmMuteCount.HeaderText = "PalmMuteCount";
            this.colPalmMuteCount.Name = "colPalmMuteCount";
            this.colPalmMuteCount.ReadOnly = true;
            this.colPalmMuteCount.Visible = false;
            // 
            // colPluckCount
            // 
            this.colPluckCount.DataPropertyName = "PluckCount";
            this.colPluckCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPluckCount.HeaderText = "PluckCount";
            this.colPluckCount.Name = "colPluckCount";
            this.colPluckCount.ReadOnly = true;
            this.colPluckCount.Visible = false;
            // 
            // colPullOffCount
            // 
            this.colPullOffCount.DataPropertyName = "PullOffCount";
            this.colPullOffCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colPullOffCount.HeaderText = "PullOffCount";
            this.colPullOffCount.Name = "colPullOffCount";
            this.colPullOffCount.ReadOnly = true;
            this.colPullOffCount.Visible = false;
            // 
            // colSlapCount
            // 
            this.colSlapCount.DataPropertyName = "SlapCount";
            this.colSlapCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSlapCount.HeaderText = "SlapCount";
            this.colSlapCount.Name = "colSlapCount";
            this.colSlapCount.ReadOnly = true;
            this.colSlapCount.Visible = false;
            // 
            // colSlideCount
            // 
            this.colSlideCount.DataPropertyName = "SlideCount";
            this.colSlideCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSlideCount.HeaderText = "SlideCount";
            this.colSlideCount.Name = "colSlideCount";
            this.colSlideCount.ReadOnly = true;
            this.colSlideCount.Visible = false;
            // 
            // colSlideUnpitchToCount
            // 
            this.colSlideUnpitchToCount.DataPropertyName = "SlideUnpitchToCount";
            this.colSlideUnpitchToCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSlideUnpitchToCount.HeaderText = "SlideUnpitchToCount";
            this.colSlideUnpitchToCount.Name = "colSlideUnpitchToCount";
            this.colSlideUnpitchToCount.ReadOnly = true;
            this.colSlideUnpitchToCount.Visible = false;
            // 
            // colSustainCount
            // 
            this.colSustainCount.DataPropertyName = "SustainCount";
            this.colSustainCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSustainCount.HeaderText = "SustainCount";
            this.colSustainCount.Name = "colSustainCount";
            this.colSustainCount.ReadOnly = true;
            this.colSustainCount.Visible = false;
            // 
            // colTapCount
            // 
            this.colTapCount.DataPropertyName = "TapCount";
            this.colTapCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTapCount.HeaderText = "TapCount";
            this.colTapCount.Name = "colTapCount";
            this.colTapCount.ReadOnly = true;
            this.colTapCount.Visible = false;
            // 
            // colTremoloCount
            // 
            this.colTremoloCount.DataPropertyName = "TremoloCount";
            this.colTremoloCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTremoloCount.HeaderText = "TremoloCount";
            this.colTremoloCount.Name = "colTremoloCount";
            this.colTremoloCount.ReadOnly = true;
            this.colTremoloCount.Visible = false;
            // 
            // colVibratoCount
            // 
            this.colVibratoCount.DataPropertyName = "VibratoCount";
            this.colVibratoCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colVibratoCount.HeaderText = "VibratoCount";
            this.colVibratoCount.Name = "colVibratoCount";
            this.colVibratoCount.ReadOnly = true;
            this.colVibratoCount.Visible = false;
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
            // colHopoCount
            // 
            this.colHopoCount.DataPropertyName = "HopoCount";
            this.colHopoCount.DefaultCellStyle = dataGridViewCellStyle3;
            this.colHopoCount.HeaderText = "HopoCount";
            this.colHopoCount.Name = "colHopoCount";
            this.colHopoCount.Visible = false;
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
            this.tsmiRescan,
            this.tsmiHelp});
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
            // tsmiHelp
            // 
            this.tsmiHelp.AssociatedEnumValue = null;
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpGeneral});
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
            this.tsmiHelpGeneral.Size = new System.Drawing.Size(146, 22);
            this.tsmiHelpGeneral.Text = "General Help";
            this.tsmiHelpGeneral.Click += new System.EventHandler(this.tsmiHelpGeneral_Click);
            // 
            // ArrangementAnalyzer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gb_Main_Grid);
            this.Controls.Add(this.gb_Main_Search);
            this.Controls.Add(this.menuStrip);
            this.Name = "ArrangementAnalyzer";
            this.Size = new System.Drawing.Size(899, 490);
            this.Resize += new System.EventHandler(this.Arrangements_Resize);
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
        private DataGridViewAutoFilterTextBoxColumn colDLCKey;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewAutoFilterTextBoxColumn colPersistentID;
        private DataGridViewAutoFilterTextBoxColumn colArrangementName;
        private DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistSort;
        private DataGridViewAutoFilterTextBoxColumn colTitleSort;
        private DataGridViewAutoFilterTextBoxColumn colAlbumSort;
        private DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewAutoFilterTextBoxColumn colSongLength;
        private DataGridViewAutoFilterTextBoxColumn colSongAverageTempo;
        private DataGridViewAutoFilterTextBoxColumn colSongVolume;
        private DataGridViewAutoFilterTextBoxColumn colLastConversionDateTime;
        private DataGridViewAutoFilterTextBoxColumn colAppID;
        private DataGridViewAutoFilterTextBoxColumn colToolkitVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageAuthor;
        private DataGridViewAutoFilterTextBoxColumn colPackageVersion;
        private DataGridViewAutoFilterTextBoxColumn colPackageComment;
        private DataGridViewAutoFilterTextBoxColumn colFilePath;
        private DataGridViewAutoFilterTextBoxColumn colFileName;
        private DataGridViewAutoFilterTextBoxColumn colFileDate;
        private DataGridViewAutoFilterTextBoxColumn colFileSize;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionID;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionVersion;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionAuthor;
        private DataGridViewAutoFilterTextBoxColumn colIgnitionDate;
        private DataGridViewAutoFilterTextBoxColumn colStatus;
        private DataGridViewAutoFilterTextBoxColumn colTagged;
        private DataGridViewAutoFilterTextBoxColumn colRepairStatus;
        private DataGridViewAutoFilterTextBoxColumn colIsOfficialDLC;
        private DataGridViewAutoFilterTextBoxColumn colIsRsCompPack;
        private DataGridViewAutoFilterTextBoxColumn colBassPick;
        private DataGridViewAutoFilterTextBoxColumn colCapoFret;
        private DataGridViewAutoFilterTextBoxColumn colDDMax;
        private DataGridViewAutoFilterTextBoxColumn colTuning;
        private DataGridViewAutoFilterTextBoxColumn colTuningPitch;
        private DataGridViewAutoFilterTextBoxColumn colToneBase;
        private DataGridViewAutoFilterTextBoxColumn colTones;
        private DataGridViewAutoFilterTextBoxColumn colTonesCount;
        private DataGridViewAutoFilterTextBoxColumn colSectionsCount;
        private DataGridViewAutoFilterTextBoxColumn colNoteCount;
        private DataGridViewAutoFilterTextBoxColumn colChordCount;
        private DataGridViewAutoFilterTextBoxColumn colChordNamesCounts;
        private DataGridViewAutoFilterTextBoxColumn colAccentCount;
        private DataGridViewAutoFilterTextBoxColumn colBendCount;
        private DataGridViewAutoFilterTextBoxColumn colFretHandMuteCount;
        private DataGridViewAutoFilterTextBoxColumn colHammerOnCount;
        private DataGridViewAutoFilterTextBoxColumn colHarmonicCount;
        private DataGridViewAutoFilterTextBoxColumn colHarmonicPinchCount;
        private DataGridViewAutoFilterTextBoxColumn colHighestFretUsed;
        private DataGridViewAutoFilterTextBoxColumn colHopoCount;
        private DataGridViewAutoFilterTextBoxColumn colIgnoreCount;
        private DataGridViewAutoFilterTextBoxColumn colLinkNextCount;
        private DataGridViewAutoFilterTextBoxColumn colOctaveCount;
        private DataGridViewAutoFilterTextBoxColumn colPalmMuteCount;
        private DataGridViewAutoFilterTextBoxColumn colPluckCount;
        private DataGridViewAutoFilterTextBoxColumn colPullOffCount;
        private DataGridViewAutoFilterTextBoxColumn colSlapCount;
        private DataGridViewAutoFilterTextBoxColumn colSlideCount;
        private DataGridViewAutoFilterTextBoxColumn colSlideUnpitchToCount;
        private DataGridViewAutoFilterTextBoxColumn colSustainCount;
        private DataGridViewAutoFilterTextBoxColumn colTapCount;
        private DataGridViewAutoFilterTextBoxColumn colTremoloCount;
        private DataGridViewAutoFilterTextBoxColumn colVibratoCount;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbum;
        private DataGridViewAutoFilterTextBoxColumn colArtistTitleAlbumDate;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescan;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanQuick;
        private CustomControls.ToolStripEnhancedMenuItem tsmiRescanFull;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelp;
        private CustomControls.ToolStripEnhancedMenuItem tsmiHelpGeneral;

    }
}
