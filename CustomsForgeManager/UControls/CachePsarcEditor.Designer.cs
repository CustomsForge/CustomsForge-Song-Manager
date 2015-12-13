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
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.chkDeleteWorkDir = new System.Windows.Forms.CheckBox();
            this.cmbGameChoice = new System.Windows.Forms.ComboBox();
            this.lblSongsFrom = new System.Windows.Forms.Label();
            this.dgvSongs = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkRefreshAll = new System.Windows.Forms.LinkLabel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.btnSaveSongs = new System.Windows.Forms.Button();
            this.btnDisableSongs = new System.Windows.Forms.Button();
            this.btnEnableSongs = new System.Windows.Forms.Button();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpCachePsarcEditor.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongs)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(67, 13);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(269, 20);
            this.tbSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(17, 16);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 5;
            this.lblSearch.Text = "Search:";
            // 
            // btnSelectAllNone
            // 
            this.btnSelectAllNone.AutoSize = true;
            this.btnSelectAllNone.Location = new System.Drawing.Point(20, 52);
            this.btnSelectAllNone.Name = "btnSelectAllNone";
            this.btnSelectAllNone.Size = new System.Drawing.Size(92, 23);
            this.btnSelectAllNone.TabIndex = 6;
            this.btnSelectAllNone.Text = "Select All/None";
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
            this.panel2.Controls.Add(this.btnRestoreBackup);
            this.panel2.Controls.Add(this.chkDeleteWorkDir);
            this.panel2.Controls.Add(this.cmbGameChoice);
            this.panel2.Controls.Add(this.lblSongsFrom);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(495, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(486, 94);
            this.panel2.TabIndex = 6;
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.AutoSize = true;
            this.btnRestoreBackup.Location = new System.Drawing.Point(324, 14);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(137, 23);
            this.btnRestoreBackup.TabIndex = 15;
            this.btnRestoreBackup.Text = "Restore Original Backups";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // chkDeleteWorkDir
            // 
            this.chkDeleteWorkDir.AutoSize = true;
            this.chkDeleteWorkDir.Location = new System.Drawing.Point(16, 56);
            this.chkDeleteWorkDir.Name = "chkDeleteWorkDir";
            this.chkDeleteWorkDir.Size = new System.Drawing.Size(362, 17);
            this.chkDeleteWorkDir.TabIndex = 15;
            this.chkDeleteWorkDir.Text = "Delete CachePsarcEditor work directories on leave - saves HDD space";
            this.chkDeleteWorkDir.UseVisualStyleBackColor = true;
            // 
            // cmbGameChoice
            // 
            this.cmbGameChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGameChoice.FormattingEnabled = true;
            this.cmbGameChoice.Items.AddRange(new object[] {
            "All Available Tracks",
            "Rocksmith Main Tracks",
            "RS1 Compatibility Disc",
            "RS1 Compatibility DLC"});
            this.cmbGameChoice.Location = new System.Drawing.Point(115, 16);
            this.cmbGameChoice.Name = "cmbGameChoice";
            this.cmbGameChoice.Size = new System.Drawing.Size(156, 21);
            this.cmbGameChoice.TabIndex = 15;
            this.cmbGameChoice.SelectedIndexChanged += new System.EventHandler(this.cmbGameChoice_SelectedIndexChanged);
            // 
            // lblSongsFrom
            // 
            this.lblSongsFrom.AutoSize = true;
            this.lblSongsFrom.Location = new System.Drawing.Point(13, 18);
            this.lblSongsFrom.Name = "lblSongsFrom";
            this.lblSongsFrom.Size = new System.Drawing.Size(96, 13);
            this.lblSongsFrom.TabIndex = 16;
            this.lblSongsFrom.Text = "Show Songs From:";
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
            this.colSongKey,
            this.colSongSource});
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
            this.colSelect.Width = 43;
            // 
            // colEnabled
            // 
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Width = 53;
            // 
            // colSong
            // 
            this.colSong.HeaderText = "Song";
            this.colSong.Name = "colSong";
            this.colSong.ReadOnly = true;
            this.colSong.Width = 150;
            // 
            // colArtist
            // 
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Width = 150;
            // 
            // colAlbum
            // 
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Width = 150;
            // 
            // colTuning
            // 
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Width = 150;
            // 
            // colSongKey
            // 
            this.colSongKey.HeaderText = "Song Key";
            this.colSongKey.Name = "colSongKey";
            this.colSongKey.ReadOnly = true;
            this.colSongKey.Width = 150;
            // 
            // colSongSource
            // 
            this.colSongSource.HeaderText = "Song Source";
            this.colSongSource.Name = "colSongSource";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lnkRefreshAll);
            this.panel1.Controls.Add(this.lnkClearSearch);
            this.panel1.Controls.Add(this.tbSearch);
            this.panel1.Controls.Add(this.btnSaveSongs);
            this.panel1.Controls.Add(this.btnDisableSongs);
            this.panel1.Controls.Add(this.btnEnableSongs);
            this.panel1.Controls.Add(this.btnSelectAllNone);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 94);
            this.panel1.TabIndex = 5;
            // 
            // lnkRefreshAll
            // 
            this.lnkRefreshAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkRefreshAll.AutoSize = true;
            this.lnkRefreshAll.ForeColor = System.Drawing.Color.DimGray;
            this.lnkRefreshAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkRefreshAll.Location = new System.Drawing.Point(358, 57);
            this.lnkRefreshAll.Name = "lnkRefreshAll";
            this.lnkRefreshAll.Size = new System.Drawing.Size(57, 13);
            this.lnkRefreshAll.TabIndex = 18;
            this.lnkRefreshAll.TabStop = true;
            this.lnkRefreshAll.Text = "Refresh all";
            this.lnkRefreshAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefreshAll_LinkClicked);
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(358, 16);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 17;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // btnSaveSongs
            // 
            this.btnSaveSongs.AutoSize = true;
            this.btnSaveSongs.Location = new System.Drawing.Point(268, 52);
            this.btnSaveSongs.Name = "btnSaveSongs";
            this.btnSaveSongs.Size = new System.Drawing.Size(66, 23);
            this.btnSaveSongs.TabIndex = 15;
            this.btnSaveSongs.Text = "Save";
            this.btnSaveSongs.UseVisualStyleBackColor = true;
            this.btnSaveSongs.Click += new System.EventHandler(this.btnSaveSongs_Click);
            // 
            // btnDisableSongs
            // 
            this.btnDisableSongs.AutoSize = true;
            this.btnDisableSongs.Location = new System.Drawing.Point(194, 52);
            this.btnDisableSongs.Name = "btnDisableSongs";
            this.btnDisableSongs.Size = new System.Drawing.Size(66, 23);
            this.btnDisableSongs.TabIndex = 13;
            this.btnDisableSongs.Text = "Disable";
            this.btnDisableSongs.UseVisualStyleBackColor = true;
            this.btnDisableSongs.Click += new System.EventHandler(this.btnDisableSongs_Click);
            // 
            // btnEnableSongs
            // 
            this.btnEnableSongs.AutoSize = true;
            this.btnEnableSongs.Location = new System.Drawing.Point(120, 52);
            this.btnEnableSongs.Name = "btnEnableSongs";
            this.btnEnableSongs.Size = new System.Drawing.Size(66, 23);
            this.btnEnableSongs.TabIndex = 14;
            this.btnEnableSongs.Text = "Enable";
            this.btnEnableSongs.UseVisualStyleBackColor = true;
            this.btnEnableSongs.Click += new System.EventHandler(this.btnEnableSongs_Click);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Enabled";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Song";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 165;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Artist";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 165;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Album";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 165;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Tuning";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 165;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Song Key";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 165;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Song Source";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
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
        private System.Windows.Forms.CheckBox chkDeleteWorkDir;
        private System.Windows.Forms.Button btnSaveSongs;
        private System.Windows.Forms.Button btnEnableSongs;
        private System.Windows.Forms.Button btnDisableSongs;
        private System.Windows.Forms.DataGridView dgvSongs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSongsFrom;
        private System.Windows.Forms.ComboBox cmbGameChoice;
        private System.Windows.Forms.LinkLabel lnkRefreshAll;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}