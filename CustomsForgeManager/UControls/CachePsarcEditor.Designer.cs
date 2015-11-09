namespace CustomsForgeManager.UControls
{
    partial class CachePsarcEditor
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
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.btnSelectAllNone = new System.Windows.Forms.Button();
            this.tlpCachePsarcEditor = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkRefreshAll = new System.Windows.Forms.LinkLabel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.checkDeleteExtractedOnClose = new System.Windows.Forms.CheckBox();
            this.comboGameChoice = new System.Windows.Forms.ComboBox();
            this.btnSaveSongs = new System.Windows.Forms.Button();
            this.btnEnableSongs = new System.Windows.Forms.Button();
            this.btnDisableSongs = new System.Windows.Forms.Button();
            this.dgvSongs = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.lblSongsFrom = new System.Windows.Forms.Label();
            this.tlpCachePsarcEditor.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(89, 13);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(269, 20);
            this.tbSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(39, 16);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "Search:";
            // 
            // btnSelectAllNone
            // 
            this.btnSelectAllNone.Location = new System.Drawing.Point(20, 42);
            this.btnSelectAllNone.Name = "btnSelectAllNone";
            this.btnSelectAllNone.Size = new System.Drawing.Size(90, 23);
            this.btnSelectAllNone.TabIndex = 6;
            this.btnSelectAllNone.Text = "Select all/none";
            this.btnSelectAllNone.UseVisualStyleBackColor = true;
            this.btnSelectAllNone.Click += new System.EventHandler(this.btnSelectAllNone_Click);
            // 
            // tlpCachePsarcEditor
            // 
            this.tlpCachePsarcEditor.ColumnCount = 2;
            this.tlpCachePsarcEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCachePsarcEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCachePsarcEditor.Controls.Add(this.panel2, 1, 0);
            this.tlpCachePsarcEditor.Controls.Add(this.dgvSongs, 0, 1);
            this.tlpCachePsarcEditor.Controls.Add(this.panel1, 0, 0);
            this.tlpCachePsarcEditor.Location = new System.Drawing.Point(3, 3);
            this.tlpCachePsarcEditor.Name = "tlpCachePsarcEditor";
            this.tlpCachePsarcEditor.RowCount = 2;
            this.tlpCachePsarcEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpCachePsarcEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tlpCachePsarcEditor.Size = new System.Drawing.Size(984, 400);
            this.tlpCachePsarcEditor.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lnkRefreshAll);
            this.panel2.Controls.Add(this.btnRestoreBackup);
            this.panel2.Controls.Add(this.checkDeleteExtractedOnClose);
            this.panel2.Controls.Add(this.comboGameChoice);
            this.panel2.Controls.Add(this.btnSaveSongs);
            this.panel2.Controls.Add(this.btnEnableSongs);
            this.panel2.Controls.Add(this.btnDisableSongs);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(495, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(486, 94);
            this.panel2.TabIndex = 6;
            // 
            // lnkRefreshAll
            // 
            this.lnkRefreshAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkRefreshAll.AutoSize = true;
            this.lnkRefreshAll.ForeColor = System.Drawing.Color.DimGray;
            this.lnkRefreshAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkRefreshAll.Location = new System.Drawing.Point(-1, 15);
            this.lnkRefreshAll.Name = "lnkRefreshAll";
            this.lnkRefreshAll.Size = new System.Drawing.Size(57, 13);
            this.lnkRefreshAll.TabIndex = 18;
            this.lnkRefreshAll.TabStop = true;
            this.lnkRefreshAll.Text = "Refresh all";
            this.lnkRefreshAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefreshAll_LinkClicked);
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Location = new System.Drawing.Point(390, 45);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(75, 43);
            this.btnRestoreBackup.TabIndex = 15;
            this.btnRestoreBackup.Text = "Restore backup";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // checkDeleteExtractedOnClose
            // 
            this.checkDeleteExtractedOnClose.AutoSize = true;
            this.checkDeleteExtractedOnClose.Location = new System.Drawing.Point(16, 48);
            this.checkDeleteExtractedOnClose.Name = "checkDeleteExtractedOnClose";
            this.checkDeleteExtractedOnClose.Size = new System.Drawing.Size(356, 17);
            this.checkDeleteExtractedOnClose.TabIndex = 15;
            this.checkDeleteExtractedOnClose.Text = "Delete extracted workdirs on exit - takes more time, saves HDD space";
            this.checkDeleteExtractedOnClose.UseVisualStyleBackColor = true;
            // 
            // comboGameChoice
            // 
            this.comboGameChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboGameChoice.FormattingEnabled = true;
            this.comboGameChoice.Items.AddRange(new object[] {
            "Rocksmith 1",
            "Rocksmith 1 DLC",
            "Rocksmith 2014"});
            this.comboGameChoice.Location = new System.Drawing.Point(2, 71);
            this.comboGameChoice.Name = "comboGameChoice";
            this.comboGameChoice.Size = new System.Drawing.Size(121, 21);
            this.comboGameChoice.TabIndex = 15;
            this.comboGameChoice.SelectedIndexChanged += new System.EventHandler(this.comboGameChoice_SelectedIndexChanged);
            // 
            // btnSaveSongs
            // 
            this.btnSaveSongs.Location = new System.Drawing.Point(350, 11);
            this.btnSaveSongs.Name = "btnSaveSongs";
            this.btnSaveSongs.Size = new System.Drawing.Size(123, 23);
            this.btnSaveSongs.TabIndex = 15;
            this.btnSaveSongs.Text = "Save songs";
            this.btnSaveSongs.UseVisualStyleBackColor = true;
            this.btnSaveSongs.Click += new System.EventHandler(this.btnSaveSongs_Click);
            // 
            // btnEnableSongs
            // 
            this.btnEnableSongs.Location = new System.Drawing.Point(205, 10);
            this.btnEnableSongs.Name = "btnEnableSongs";
            this.btnEnableSongs.Size = new System.Drawing.Size(139, 23);
            this.btnEnableSongs.TabIndex = 14;
            this.btnEnableSongs.Text = "Enable selected songs";
            this.btnEnableSongs.UseVisualStyleBackColor = true;
            this.btnEnableSongs.Click += new System.EventHandler(this.btnEnableSongs_Click);
            // 
            // btnDisableSongs
            // 
            this.btnDisableSongs.Location = new System.Drawing.Point(60, 10);
            this.btnDisableSongs.Name = "btnDisableSongs";
            this.btnDisableSongs.Size = new System.Drawing.Size(139, 23);
            this.btnDisableSongs.TabIndex = 13;
            this.btnDisableSongs.Text = "Disable selected songs";
            this.btnDisableSongs.UseVisualStyleBackColor = true;
            this.btnDisableSongs.Click += new System.EventHandler(this.btnDisableSongs_Click);
            // 
            // dgvSongs
            // 
            this.dgvSongs.AllowUserToAddRows = false;
            this.dgvSongs.AllowUserToDeleteRows = false;
            this.dgvSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colEnabled,
            this.colSong,
            this.colArtist,
            this.colAlbum,
            this.colTuning,
            this.colSongKey});
            this.tlpCachePsarcEditor.SetColumnSpan(this.dgvSongs, 2);
            this.dgvSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSongs.Location = new System.Drawing.Point(3, 103);
            this.dgvSongs.Name = "dgvSongs";
            this.dgvSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongs.Size = new System.Drawing.Size(978, 294);
            this.dgvSongs.TabIndex = 4;
            this.dgvSongs.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongs_CellMouseUp);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "Select";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 60;
            // 
            // colEnabled
            // 
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Width = 60;
            // 
            // colSong
            // 
            this.colSong.HeaderText = "Song";
            this.colSong.Name = "colSong";
            this.colSong.ReadOnly = true;
            this.colSong.Width = 165;
            // 
            // colArtist
            // 
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Width = 165;
            // 
            // colAlbum
            // 
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Width = 165;
            // 
            // colTuning
            // 
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Width = 165;
            // 
            // colSongKey
            // 
            this.colSongKey.HeaderText = "Song Key";
            this.colSongKey.Name = "colSongKey";
            this.colSongKey.ReadOnly = true;
            this.colSongKey.Width = 165;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lnkClearSearch);
            this.panel1.Controls.Add(this.lblSongsFrom);
            this.panel1.Controls.Add(this.tbSearch);
            this.panel1.Controls.Add(this.btnSelectAllNone);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 94);
            this.panel1.TabIndex = 5;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(364, 16);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 17;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // lblSongsFrom
            // 
            this.lblSongsFrom.AutoSize = true;
            this.lblSongsFrom.Location = new System.Drawing.Point(423, 75);
            this.lblSongsFrom.Name = "lblSongsFrom";
            this.lblSongsFrom.Size = new System.Drawing.Size(63, 13);
            this.lblSongsFrom.TabIndex = 16;
            this.lblSongsFrom.Text = "Songs from:";
            // 
            // CachePsarcEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpCachePsarcEditor);
            this.Name = "CachePsarcEditor";
            this.Size = new System.Drawing.Size(990, 403);
            this.tlpCachePsarcEditor.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnSelectAllNone;
        private System.Windows.Forms.TableLayoutPanel tlpCachePsarcEditor;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.CheckBox checkDeleteExtractedOnClose;
        private System.Windows.Forms.Button btnSaveSongs;
        private System.Windows.Forms.Button btnEnableSongs;
        private System.Windows.Forms.Button btnDisableSongs;
        private System.Windows.Forms.DataGridView dgvSongs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSongsFrom;
        private System.Windows.Forms.ComboBox comboGameChoice;
        private System.Windows.Forms.LinkLabel lnkRefreshAll;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongKey;
    }
}