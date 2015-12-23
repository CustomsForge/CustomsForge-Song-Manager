using CustomsForgeManager.CustomsForgeManagerLib.DataGridTools;

namespace CustomsForgeManager.UControls
{
    partial class SongPacks
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
            this.btnSelectAllNone = new System.Windows.Forms.Button();
            this.tlpSongPacks = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkRefreshAll = new System.Windows.Forms.LinkLabel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.cmbSongPacks = new System.Windows.Forms.ComboBox();
            this.lblSongPack = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.btnSaveSongs = new System.Windows.Forms.Button();
            this.btnDisableSongs = new System.Windows.Forms.Button();
            this.btnEnableSongs = new System.Windows.Forms.Button();
            this.dgvSongPacks = new CustomsForgeManager.CustomsForgeManagerLib.DataGridTools.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cueSearch = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpSongPacks.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectAllNone
            // 
            this.btnSelectAllNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAllNone.Location = new System.Drawing.Point(22, 42);
            this.btnSelectAllNone.Name = "btnSelectAllNone";
            this.btnSelectAllNone.Size = new System.Drawing.Size(95, 23);
            this.btnSelectAllNone.TabIndex = 6;
            this.btnSelectAllNone.Text = "Select All/None";
            this.btnSelectAllNone.UseVisualStyleBackColor = true;
            this.btnSelectAllNone.Click += new System.EventHandler(this.btnSelectAllNone_Click);
            // 
            // tlpSongPacks
            // 
            this.tlpSongPacks.ColumnCount = 2;
            this.tlpSongPacks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSongPacks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSongPacks.Controls.Add(this.panel2, 1, 0);
            this.tlpSongPacks.Controls.Add(this.dgvSongPacks, 0, 1);
            this.tlpSongPacks.Controls.Add(this.panel1, 0, 0);
            this.tlpSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSongPacks.Location = new System.Drawing.Point(0, 0);
            this.tlpSongPacks.Name = "tlpSongPacks";
            this.tlpSongPacks.RowCount = 3;
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlpSongPacks.Size = new System.Drawing.Size(990, 403);
            this.tlpSongPacks.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lnkRefreshAll);
            this.panel2.Controls.Add(this.btnRestoreBackup);
            this.panel2.Controls.Add(this.cmbSongPacks);
            this.panel2.Controls.Add(this.lblSongPack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(498, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 71);
            this.panel2.TabIndex = 6;
            // 
            // lnkRefreshAll
            // 
            this.lnkRefreshAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkRefreshAll.AutoSize = true;
            this.lnkRefreshAll.ForeColor = System.Drawing.Color.DimGray;
            this.lnkRefreshAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkRefreshAll.Location = new System.Drawing.Point(94, 47);
            this.lnkRefreshAll.Name = "lnkRefreshAll";
            this.lnkRefreshAll.Size = new System.Drawing.Size(116, 13);
            this.lnkRefreshAll.TabIndex = 18;
            this.lnkRefreshAll.TabStop = true;
            this.lnkRefreshAll.Text = "Reload All Song Packs";
            this.lnkRefreshAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefreshAll_LinkClicked);
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestoreBackup.AutoSize = true;
            this.btnRestoreBackup.Location = new System.Drawing.Point(327, 14);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(137, 23);
            this.btnRestoreBackup.TabIndex = 15;
            this.btnRestoreBackup.Text = "Restore Original Backups";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // cmbSongPacks
            // 
            this.cmbSongPacks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSongPacks.FormattingEnabled = true;
            this.cmbSongPacks.Location = new System.Drawing.Point(97, 16);
            this.cmbSongPacks.Name = "cmbSongPacks";
            this.cmbSongPacks.Size = new System.Drawing.Size(166, 21);
            this.cmbSongPacks.TabIndex = 15;
            this.cmbSongPacks.SelectedIndexChanged += new System.EventHandler(this.cmbSongPacks_SelectedIndexChanged);
            // 
            // lblSongPack
            // 
            this.lblSongPack.AutoSize = true;
            this.lblSongPack.Location = new System.Drawing.Point(23, 19);
            this.lblSongPack.Name = "lblSongPack";
            this.lblSongPack.Size = new System.Drawing.Size(68, 13);
            this.lblSongPack.TabIndex = 16;
            this.lblSongPack.Text = "Song Packs:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lnkClearSearch);
            this.panel1.Controls.Add(this.cueSearch);
            this.panel1.Controls.Add(this.btnSaveSongs);
            this.panel1.Controls.Add(this.btnDisableSongs);
            this.panel1.Controls.Add(this.btnEnableSongs);
            this.panel1.Controls.Add(this.btnSelectAllNone);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(489, 71);
            this.panel1.TabIndex = 5;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(403, 19);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 17;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // btnSaveSongs
            // 
            this.btnSaveSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveSongs.Location = new System.Drawing.Point(318, 42);
            this.btnSaveSongs.Name = "btnSaveSongs";
            this.btnSaveSongs.Size = new System.Drawing.Size(69, 23);
            this.btnSaveSongs.TabIndex = 15;
            this.btnSaveSongs.Text = "Save";
            this.btnSaveSongs.UseVisualStyleBackColor = true;
            this.btnSaveSongs.Click += new System.EventHandler(this.btnSaveSongs_Click);
            // 
            // btnDisableSongs
            // 
            this.btnDisableSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDisableSongs.Location = new System.Drawing.Point(228, 42);
            this.btnDisableSongs.Name = "btnDisableSongs";
            this.btnDisableSongs.Size = new System.Drawing.Size(69, 23);
            this.btnDisableSongs.TabIndex = 13;
            this.btnDisableSongs.Text = "Disable";
            this.btnDisableSongs.UseVisualStyleBackColor = true;
            this.btnDisableSongs.Click += new System.EventHandler(this.btnDisableSongs_Click);
            // 
            // btnEnableSongs
            // 
            this.btnEnableSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnableSongs.Location = new System.Drawing.Point(138, 42);
            this.btnEnableSongs.Name = "btnEnableSongs";
            this.btnEnableSongs.Size = new System.Drawing.Size(69, 23);
            this.btnEnableSongs.TabIndex = 14;
            this.btnEnableSongs.Text = "Enable";
            this.btnEnableSongs.UseVisualStyleBackColor = true;
            this.btnEnableSongs.Click += new System.EventHandler(this.btnEnableSongs_Click);
            // 
            // dgvSongPacks
            // 
            this.dgvSongPacks.AllowUserToAddRows = false;
            this.dgvSongPacks.AllowUserToDeleteRows = false;
            this.dgvSongPacks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSongPacks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colEnabled,
            this.colArtist,
            this.colSong,
            this.colAlbum,
            this.colTuning,
            this.colSongKey,
            this.colSongSource});
            this.tlpSongPacks.SetColumnSpan(this.dgvSongPacks, 2);
            this.dgvSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSongPacks.Location = new System.Drawing.Point(3, 80);
            this.dgvSongPacks.Name = "dgvSongPacks";
            this.dgvSongPacks.RowHeadersVisible = false;
            this.dgvSongPacks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongPacks.Size = new System.Drawing.Size(984, 314);
            this.dgvSongPacks.TabIndex = 4;
            this.dgvSongPacks.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongs_CellMouseUp);
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "Select";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 52;
            // 
            // colEnabled
            // 
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Width = 72;
            // 
            // colArtist
            // 
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Width = 150;
            // 
            // colSong
            // 
            this.colSong.HeaderText = "Song";
            this.colSong.Name = "colSong";
            this.colSong.ReadOnly = true;
            this.colSong.Width = 150;
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
            this.colTuning.Width = 120;
            // 
            // colSongKey
            // 
            this.colSongKey.HeaderText = "Song Key";
            this.colSongKey.Name = "colSongKey";
            this.colSongKey.ReadOnly = true;
            this.colSongKey.Width = 120;
            // 
            // colSongSource
            // 
            this.colSongSource.HeaderText = "Song Source";
            this.colSongSource.Name = "colSongSource";
            this.colSongSource.Width = 120;
            // 
            // cueSearch
            // 
            this.cueSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cueSearch.Cue = "Type characters to search...";
            this.cueSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueSearch.Location = new System.Drawing.Point(22, 16);
            this.cueSearch.Name = "cueSearch";
            this.cueSearch.Size = new System.Drawing.Size(366, 20);
            this.cueSearch.TabIndex = 16;
            this.cueSearch.TextChanged += new System.EventHandler(this.cueSearch_TextChanged);
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
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
            this.dataGridViewTextBoxColumn7.Width = 94;
            // 
            // SongPacks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSongPacks);
            this.Name = "SongPacks";
            this.Size = new System.Drawing.Size(990, 403);
            this.tlpSongPacks.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAllNone;
        private System.Windows.Forms.TableLayoutPanel tlpSongPacks;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnSaveSongs;
        private System.Windows.Forms.Button btnEnableSongs;
        private System.Windows.Forms.Button btnDisableSongs;
        private RADataGridView dgvSongPacks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblSongPack;
        private System.Windows.Forms.ComboBox cmbSongPacks;
        private System.Windows.Forms.LinkLabel lnkRefreshAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.LinkLabel lnkClearSearch;
        private CustomsForgeManagerLib.CustomControls.CueTextBox cueSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlbum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSongSource;
    }
}