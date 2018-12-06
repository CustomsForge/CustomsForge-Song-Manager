using System.Windows.Forms;
using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
{
    partial class ProfileSongLists
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
            this.cmsProfileSongLists = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsActions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsToggle = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSelectAllNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsShow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEnableDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.chkProtectODLC = new System.Windows.Forms.CheckBox();
            this.chkIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.dgvSongListSongs = new DataGridViewTools.RADataGridView();
            this.colSongListSongsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsSongArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsSongTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsSongAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsArrangements = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsSongTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsDD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsArtistTitleAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongListSongsFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSongListMaster = new DataGridViewTools.RADataGridView();
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
            this.gbSongListSongs = new System.Windows.Forms.GroupBox();
            this.gbSongListMaster = new System.Windows.Forms.GroupBox();
            this.gbSongLists = new System.Windows.Forms.GroupBox();
            this.dgvGameSongLists = new DataGridViewTools.RADataGridView();
            this.colGameSongListsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colGameSongLists = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSetlistManager = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.gbButtons = new System.Windows.Forms.GroupBox();
            this.lnkLoadPrfldb = new System.Windows.Forms.LinkLabel();
            this.lnkSongListsHelp = new System.Windows.Forms.LinkLabel();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.cmsProfileSongLists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongListSongs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongListMaster)).BeginInit();
            this.gbSongListSongs.SuspendLayout();
            this.gbSongListMaster.SuspendLayout();
            this.gbSongLists.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameSongLists)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.gbButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsProfileSongLists
            // 
            this.cmsProfileSongLists.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsActions,
            this.toolStripSeparator2,
            this.cmsToggle,
            this.cmsSelectAllNone,
            this.toolStripSeparator1,
            this.cmsShow,
            this.cmsEnableDisable,
            this.cmsRemove,
            this.cmsAdd});
            this.cmsProfileSongLists.Name = "contextMenuStrip_MainManager";
            this.cmsProfileSongLists.Size = new System.Drawing.Size(158, 170);
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
            // cmsRemove
            // 
            this.cmsRemove.Image = global::CustomsForgeSongManager.Properties.Resources.delete;
            this.cmsRemove.Name = "cmsRemove";
            this.cmsRemove.Size = new System.Drawing.Size(157, 22);
            this.cmsRemove.Text = "Remove";
            this.cmsRemove.ToolTipText = "WARNING\r\nDeletion can not be undone.\r\nSelect must be checked.";
            this.cmsRemove.Click += new System.EventHandler(this.cmsRemove_Click);
            // 
            // cmsAdd
            // 
            this.cmsAdd.Image = global::CustomsForgeSongManager.Properties.Resources.copy;
            this.cmsAdd.Name = "cmsAdd";
            this.cmsAdd.Size = new System.Drawing.Size(157, 22);
            this.cmsAdd.Text = "Add";
            this.cmsAdd.ToolTipText = "Select must be checked.";
            this.cmsAdd.Click += new System.EventHandler(this.cmsAdd_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 100;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 50;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(20, 88);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 48;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.toolTip.SetToolTip(this.lnkClearSearch, "HINT:\r\nQuickly clears any Filters, \r\nas well as, any Search, \r\nand preserves exis" +
                    "ting sort.");
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // chkProtectODLC
            // 
            this.chkProtectODLC.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkProtectODLC.AutoSize = true;
            this.chkProtectODLC.BackColor = System.Drawing.Color.LightGray;
            this.chkProtectODLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkProtectODLC.ForeColor = System.Drawing.Color.Red;
            this.chkProtectODLC.Location = new System.Drawing.Point(23, 134);
            this.chkProtectODLC.Name = "chkProtectODLC";
            this.chkProtectODLC.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.chkProtectODLC.Size = new System.Drawing.Size(124, 23);
            this.chkProtectODLC.TabIndex = 50;
            this.chkProtectODLC.Text = "Protect Official DLC";
            this.toolTip.SetToolTip(this.chkProtectODLC, "If checked, prevents ODLC\r\nfrom being selected, enabled,\r\ndisabled, copied, moved" +
                    ", or deleted.");
            this.chkProtectODLC.UseVisualStyleBackColor = false;
            this.chkProtectODLC.CheckedChanged += new System.EventHandler(this.chkProtectODLC_CheckedChanged);
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkIncludeSubfolders.AutoSize = true;
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(23, 163);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Padding = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(119, 23);
            this.chkIncludeSubfolders.TabIndex = 49;
            this.chkIncludeSubfolders.Text = "Include Subfolders";
            this.toolTip.SetToolTip(this.chkIncludeSubfolders, "If checked, shows and highlights\r\nsongs in the Master Songs grid\r\nthat are from s" +
                    "etlist subfolders.");
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            this.chkIncludeSubfolders.CheckStateChanged += new System.EventHandler(this.chkIncludeSubfolders_CheckedChanged);
            // 
            // dgvSongListSongs
            // 
            this.dgvSongListSongs.AllowUserToAddRows = false;
            this.dgvSongListSongs.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongListSongs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSongListSongs.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongListSongs.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSongListSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSongListSongsSelect,
            this.colKey,
            this.colSongListSongsEnabled,
            this.colSongListSongsSongArtist,
            this.colSongListSongsSongTitle,
            this.colSongListSongsSongAlbum,
            this.colSongListSongsArrangements,
            this.colSongListSongsSongTuning,
            this.colSongListSongsDD,
            this.colSongListSongsPath,
            this.colSongListSongsArtistTitleAlbum,
            this.colSongListSongsFileName});
            this.dgvSongListSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSongListSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSongListSongs.Location = new System.Drawing.Point(3, 16);
            this.dgvSongListSongs.MultiSelect = false;
            this.dgvSongListSongs.Name = "dgvSongListSongs";
            this.dgvSongListSongs.RowHeadersVisible = false;
            this.dgvSongListSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongListSongs.Size = new System.Drawing.Size(423, 459);
            this.dgvSongListSongs.TabIndex = 34;
            this.dgvSongListSongs.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrent_CellFormatting);
            this.dgvSongListSongs.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseEnter);
            this.dgvSongListSongs.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseLeave);
            this.dgvSongListSongs.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrent_CellMouseUp);
            // 
            // colSongListSongsSelect
            // 
            this.colSongListSongsSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colSongListSongsSelect.DataPropertyName = "Selected";
            this.colSongListSongsSelect.FalseValue = "false";
            this.colSongListSongsSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSongListSongsSelect.HeaderText = "Select";
            this.colSongListSongsSelect.IndeterminateValue = "false";
            this.colSongListSongsSelect.Name = "colSongListSongsSelect";
            this.colSongListSongsSelect.TrueValue = "true";
            this.colSongListSongsSelect.Width = 43;
            // 
            // colKey
            // 
            this.colKey.DataPropertyName = "DLCKey";
            this.colKey.HeaderText = "DLC Key";
            this.colKey.Name = "colKey";
            this.colKey.ReadOnly = true;
            // 
            // colSongListSongsEnabled
            // 
            this.colSongListSongsEnabled.DataPropertyName = "Enabled";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colSongListSongsEnabled.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSongListSongsEnabled.HeaderText = "Enabled";
            this.colSongListSongsEnabled.Name = "colSongListSongsEnabled";
            this.colSongListSongsEnabled.ReadOnly = true;
            this.colSongListSongsEnabled.Width = 50;
            // 
            // colSongListSongsSongArtist
            // 
            this.colSongListSongsSongArtist.DataPropertyName = "Artist";
            this.colSongListSongsSongArtist.HeaderText = "Artist";
            this.colSongListSongsSongArtist.Name = "colSongListSongsSongArtist";
            this.colSongListSongsSongArtist.ReadOnly = true;
            // 
            // colSongListSongsSongTitle
            // 
            this.colSongListSongsSongTitle.DataPropertyName = "Title";
            this.colSongListSongsSongTitle.HeaderText = "Song Title";
            this.colSongListSongsSongTitle.Name = "colSongListSongsSongTitle";
            this.colSongListSongsSongTitle.ReadOnly = true;
            // 
            // colSongListSongsSongAlbum
            // 
            this.colSongListSongsSongAlbum.DataPropertyName = "Album";
            this.colSongListSongsSongAlbum.HeaderText = "Album";
            this.colSongListSongsSongAlbum.Name = "colSongListSongsSongAlbum";
            this.colSongListSongsSongAlbum.ReadOnly = true;
            // 
            // colSongListSongsArrangements
            // 
            this.colSongListSongsArrangements.DataPropertyName = "Arrangements";
            this.colSongListSongsArrangements.HeaderText = "Arrangements";
            this.colSongListSongsArrangements.Name = "colSongListSongsArrangements";
            this.colSongListSongsArrangements.ReadOnly = true;
            this.colSongListSongsArrangements.Width = 50;
            // 
            // colSongListSongsSongTuning
            // 
            this.colSongListSongsSongTuning.DataPropertyName = "Tuning";
            this.colSongListSongsSongTuning.HeaderText = "Tuning";
            this.colSongListSongsSongTuning.Name = "colSongListSongsSongTuning";
            this.colSongListSongsSongTuning.ReadOnly = true;
            this.colSongListSongsSongTuning.Width = 70;
            // 
            // colSongListSongsDD
            // 
            this.colSongListSongsDD.DataPropertyName = "DD";
            this.colSongListSongsDD.HeaderText = "DD";
            this.colSongListSongsDD.Name = "colSongListSongsDD";
            this.colSongListSongsDD.ReadOnly = true;
            this.colSongListSongsDD.Width = 50;
            // 
            // colSongListSongsPath
            // 
            this.colSongListSongsPath.DataPropertyName = "FilePath";
            this.colSongListSongsPath.HeaderText = "File Path";
            this.colSongListSongsPath.Name = "colSongListSongsPath";
            this.colSongListSongsPath.ReadOnly = true;
            this.colSongListSongsPath.Width = 350;
            // 
            // colSongListSongsArtistTitleAlbum
            // 
            this.colSongListSongsArtistTitleAlbum.DataPropertyName = "ArtistTitleAlbum";
            this.colSongListSongsArtistTitleAlbum.HeaderText = "ArtistTitleAlbum";
            this.colSongListSongsArtistTitleAlbum.Name = "colSongListSongsArtistTitleAlbum";
            this.colSongListSongsArtistTitleAlbum.ReadOnly = true;
            this.colSongListSongsArtistTitleAlbum.Visible = false;
            // 
            // colSongListSongsFileName
            // 
            this.colSongListSongsFileName.DataPropertyName = "FileName";
            this.colSongListSongsFileName.HeaderText = "File Name";
            this.colSongListSongsFileName.Name = "colSongListSongsFileName";
            this.colSongListSongsFileName.ReadOnly = true;
            this.colSongListSongsFileName.Width = 140;
            // 
            // dgvSongListMaster
            // 
            this.dgvSongListMaster.AllowUserToAddRows = false;
            this.dgvSongListMaster.AllowUserToDeleteRows = false;
            this.dgvSongListMaster.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvSongListMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSongListMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSongListMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSongListMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dgvSongListMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSongListMaster.Location = new System.Drawing.Point(6, 16);
            this.dgvSongListMaster.MultiSelect = false;
            this.dgvSongListMaster.Name = "dgvSongListMaster";
            this.dgvSongListMaster.RowHeadersVisible = false;
            this.dgvSongListMaster.Size = new System.Drawing.Size(529, 213);
            this.dgvSongListMaster.TabIndex = 32;
            this.dgvSongListMaster.Tag = "Song List Master";
            this.dgvSongListMaster.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCurrent_CellFormatting);
            this.dgvSongListMaster.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongListMaster_CellMouseDown);
            this.dgvSongListMaster.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseEnter);
            this.dgvSongListMaster.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseLeave);
            this.dgvSongListMaster.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrent_CellMouseUp);
            this.dgvSongListMaster.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSongListMaster_DataBindingComplete);
            this.dgvSongListMaster.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSongListMaster_Paint);
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
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEnabled.DefaultCellStyle = dataGridViewCellStyle6;
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
            // gbSongListSongs
            // 
            this.gbSongListSongs.Controls.Add(this.dgvSongListSongs);
            this.gbSongListSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongListSongs.Location = new System.Drawing.Point(552, 3);
            this.gbSongListSongs.Name = "gbSongListSongs";
            this.gbSongListSongs.Size = new System.Drawing.Size(429, 478);
            this.gbSongListSongs.TabIndex = 6;
            this.gbSongListSongs.TabStop = false;
            this.gbSongListSongs.Text = "Song List Songs";
            // 
            // gbSongListMaster
            // 
            this.gbSongListMaster.Controls.Add(this.dgvSongListMaster);
            this.gbSongListMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongListMaster.Location = new System.Drawing.Point(1, 240);
            this.gbSongListMaster.Margin = new System.Windows.Forms.Padding(1);
            this.gbSongListMaster.Name = "gbSongListMaster";
            this.gbSongListMaster.Size = new System.Drawing.Size(541, 237);
            this.gbSongListMaster.TabIndex = 9;
            this.gbSongListMaster.TabStop = false;
            this.gbSongListMaster.Text = "Master Songs";
            // 
            // gbSongLists
            // 
            this.gbSongLists.Controls.Add(this.dgvGameSongLists);
            this.gbSongLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSongLists.Location = new System.Drawing.Point(3, 3);
            this.gbSongLists.Name = "gbSongLists";
            this.gbSongLists.Size = new System.Drawing.Size(334, 227);
            this.gbSongLists.TabIndex = 5;
            this.gbSongLists.TabStop = false;
            this.gbSongLists.Text = "Song Lists";
            // 
            // dgvGameSongLists
            // 
            this.dgvGameSongLists.AllowUserToAddRows = false;
            this.dgvGameSongLists.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvGameSongLists.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvGameSongLists.BackgroundColor = System.Drawing.SystemColors.InactiveBorder;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGameSongLists.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvGameSongLists.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGameSongListsSelect,
            this.colGameSongLists,
            this.colSetlistManager});
            this.dgvGameSongLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGameSongLists.Location = new System.Drawing.Point(3, 16);
            this.dgvGameSongLists.MultiSelect = false;
            this.dgvGameSongLists.Name = "dgvGameSongLists";
            this.dgvGameSongLists.RowHeadersVisible = false;
            this.dgvGameSongLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGameSongLists.Size = new System.Drawing.Size(328, 208);
            this.dgvGameSongLists.TabIndex = 35;
            this.dgvGameSongLists.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseEnter);
            this.dgvGameSongLists.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCurrent_CellMouseLeave);
            this.dgvGameSongLists.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGameSongLists_CellMouseUp);
            this.dgvGameSongLists.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGameSongLists_CellValueChanged);
            this.dgvGameSongLists.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvGameSongLists_CurrentCellDirtyStateChanged);
            // 
            // colGameSongListsSelect
            // 
            this.colGameSongListsSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colGameSongListsSelect.FalseValue = "false";
            this.colGameSongListsSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colGameSongListsSelect.HeaderText = "Select";
            this.colGameSongListsSelect.IndeterminateValue = "false";
            this.colGameSongListsSelect.Name = "colGameSongListsSelect";
            this.colGameSongListsSelect.TrueValue = "true";
            this.colGameSongListsSelect.Width = 43;
            // 
            // colGameSongLists
            // 
            this.colGameSongLists.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colGameSongLists.HeaderText = "In-Game Song Lists";
            this.colGameSongLists.Name = "colGameSongLists";
            this.colGameSongLists.ReadOnly = true;
            this.colGameSongLists.Width = 124;
            // 
            // colSetlistManager
            // 
            this.colSetlistManager.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSetlistManager.HeaderText = "Setlist Manager Setlists";
            this.colSetlistManager.Name = "colSetlistManager";
            this.colSetlistManager.ToolTipText = "Add Setlist Manager created setlist to an in-game song list.";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(990, 490);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.81015F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.18985F));
            this.tableLayoutPanel5.Controls.Add(this.gbSongListSongs, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(984, 484);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.gbSongListMaster, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(543, 478);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.41463F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.58537F));
            this.tableLayoutPanel7.Controls.Add(this.gbButtons, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.gbSongLists, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(537, 233);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // gbButtons
            // 
            this.gbButtons.Controls.Add(this.lnkLoadPrfldb);
            this.gbButtons.Controls.Add(this.lnkClearSearch);
            this.gbButtons.Controls.Add(this.lnkSongListsHelp);
            this.gbButtons.Controls.Add(this.chkProtectODLC);
            this.gbButtons.Controls.Add(this.chkIncludeSubfolders);
            this.gbButtons.Controls.Add(this.cueSearch);
            this.gbButtons.Location = new System.Drawing.Point(343, 3);
            this.gbButtons.Name = "gbButtons";
            this.gbButtons.Size = new System.Drawing.Size(191, 227);
            this.gbButtons.TabIndex = 6;
            this.gbButtons.TabStop = false;
            // 
            // lnkLoadPrfldb
            // 
            this.lnkLoadPrfldb.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lnkLoadPrfldb.AutoSize = true;
            this.lnkLoadPrfldb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLoadPrfldb.ForeColor = System.Drawing.Color.Black;
            this.lnkLoadPrfldb.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkLoadPrfldb.Location = new System.Drawing.Point(16, 27);
            this.lnkLoadPrfldb.Name = "lnkLoadPrfldb";
            this.lnkLoadPrfldb.Size = new System.Drawing.Size(158, 15);
            this.lnkLoadPrfldb.TabIndex = 51;
            this.lnkLoadPrfldb.TabStop = true;
            this.lnkLoadPrfldb.Text = "Reload Song Lists Root";
            this.lnkLoadPrfldb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLoadPrfldb_LinkClicked);
            // 
            // lnkSongListsHelp
            // 
            this.lnkSongListsHelp.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lnkSongListsHelp.AutoSize = true;
            this.lnkSongListsHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSongListsHelp.ForeColor = System.Drawing.Color.Black;
            this.lnkSongListsHelp.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSongListsHelp.Location = new System.Drawing.Point(39, 200);
            this.lnkSongListsHelp.Name = "lnkSongListsHelp";
            this.lnkSongListsHelp.Size = new System.Drawing.Size(113, 13);
            this.lnkSongListsHelp.TabIndex = 45;
            this.lnkSongListsHelp.TabStop = true;
            this.lnkSongListsHelp.Text = "Profile Song Lists Help";
            this.lnkSongListsHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSongListsHelp_LinkClicked);
            // 
            // cueSearch
            // 
            this.cueSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cueSearch.Cue = "Search";
            this.cueSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueSearch.Location = new System.Drawing.Point(23, 65);
            this.cueSearch.Name = "cueSearch";
            this.cueSearch.Size = new System.Drawing.Size(144, 20);
            this.cueSearch.TabIndex = 44;
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
            // 
            // ProfileSongLists
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Name = "ProfileSongLists";
            this.Size = new System.Drawing.Size(990, 490);
            this.cmsProfileSongLists.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongListSongs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongListMaster)).EndInit();
            this.gbSongListSongs.ResumeLayout(false);
            this.gbSongListMaster.ResumeLayout(false);
            this.gbSongLists.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGameSongLists)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.gbButtons.ResumeLayout(false);
            this.gbButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContextMenuStrip cmsProfileSongLists;
        private ToolStripMenuItem cmsRemove;
        private ToolStripMenuItem cmsAdd;
        private ToolStripMenuItem cmsShow;
        private ToolTip toolTip;
        private ToolStripMenuItem cmsEnableDisable;
        private ToolStripMenuItem cmsToggle;
        private ToolStripMenuItem cmsSelectAllNone;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem cmsActions;
        private ToolStripSeparator toolStripSeparator2;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private TableLayoutPanel tableLayoutPanel7;
        private GroupBox gbSongListSongs;
        private RADataGridView dgvSongListSongs;
        private GroupBox gbSongListMaster;
        private RADataGridView dgvSongListMaster;
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
        private GroupBox gbButtons;
        private LinkLabel lnkClearSearch;
        private LinkLabel lnkSongListsHelp;
        private CheckBox chkProtectODLC;
        private CheckBox chkIncludeSubfolders;
        private CueTextBox cueSearch;
        private GroupBox gbSongLists;
        private RADataGridView dgvGameSongLists;
        private LinkLabel lnkLoadPrfldb;
        private DataGridViewCheckBoxColumn colSongListSongsSelect;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colSongListSongsEnabled;
        private DataGridViewTextBoxColumn colSongListSongsSongArtist;
        private DataGridViewTextBoxColumn colSongListSongsSongTitle;
        private DataGridViewTextBoxColumn colSongListSongsSongAlbum;
        private DataGridViewTextBoxColumn colSongListSongsArrangements;
        private DataGridViewTextBoxColumn colSongListSongsSongTuning;
        private DataGridViewTextBoxColumn colSongListSongsDD;
        private DataGridViewTextBoxColumn colSongListSongsPath;
        private DataGridViewTextBoxColumn colSongListSongsArtistTitleAlbum;
        private DataGridViewTextBoxColumn colSongListSongsFileName;
        private DataGridViewCheckBoxColumn colGameSongListsSelect;
        private DataGridViewTextBoxColumn colGameSongLists;
        private DataGridViewComboBoxColumn colSetlistManager;




    }
}
