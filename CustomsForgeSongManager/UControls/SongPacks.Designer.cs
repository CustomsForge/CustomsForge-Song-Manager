using DataGridViewTools;

namespace CustomsForgeSongManager.UControls
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lnkRefreshAll = new System.Windows.Forms.LinkLabel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.cmbSongPacks = new System.Windows.Forms.ComboBox();
            this.lblSongPack = new System.Windows.Forms.Label();
            this.dgvSongPacks = new DataGridViewTools.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTuning = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongYear = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongLength = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colSongKey = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkClearSearch = new System.Windows.Forms.LinkLabel();
            this.cueSearch = new DataGridViewTools.CueTextBox();
            this.btnSaveSongs = new System.Windows.Forms.Button();
            this.btnDisableSongs = new System.Windows.Forms.Button();
            this.btnEnableSongs = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lnkToggle = new System.Windows.Forms.LinkLabel();
            this.lnkSelectAll = new System.Windows.Forms.LinkLabel();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectAllNone
            // 
            this.btnSelectAllNone.AutoSize = true;
            this.btnSelectAllNone.Location = new System.Drawing.Point(21, 38);
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
            this.tlpSongPacks.Controls.Add(this.panel3, 0, 2);
            this.tlpSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSongPacks.Location = new System.Drawing.Point(0, 0);
            this.tlpSongPacks.Name = "tlpSongPacks";
            this.tlpSongPacks.RowCount = 3;
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSongPacks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSongPacks.Size = new System.Drawing.Size(990, 403);
            this.tlpSongPacks.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblFileName);
            this.panel2.Controls.Add(this.txtFileName);
            this.panel2.Controls.Add(this.lnkRefreshAll);
            this.panel2.Controls.Add(this.btnRestoreBackup);
            this.panel2.Controls.Add(this.cmbSongPacks);
            this.panel2.Controls.Add(this.lblSongPack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(498, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(489, 67);
            this.panel2.TabIndex = 6;
            // 
            // lblFileName
            // 
            this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(34, 41);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(57, 13);
            this.lblFileName.TabIndex = 20;
            this.lblFileName.Text = "File Name:";
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(97, 38);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(177, 20);
            this.txtFileName.TabIndex = 19;
            // 
            // lnkRefreshAll
            // 
            this.lnkRefreshAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkRefreshAll.AutoSize = true;
            this.lnkRefreshAll.ForeColor = System.Drawing.Color.DimGray;
            this.lnkRefreshAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkRefreshAll.Location = new System.Drawing.Point(324, 12);
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
            this.btnRestoreBackup.Location = new System.Drawing.Point(327, 36);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(137, 23);
            this.btnRestoreBackup.TabIndex = 15;
            this.btnRestoreBackup.Text = "Restore Original Backups";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // cmbSongPacks
            // 
            this.cmbSongPacks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSongPacks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSongPacks.FormattingEnabled = true;
            this.cmbSongPacks.Location = new System.Drawing.Point(97, 8);
            this.cmbSongPacks.Name = "cmbSongPacks";
            this.cmbSongPacks.Size = new System.Drawing.Size(177, 21);
            this.cmbSongPacks.TabIndex = 15;
            this.cmbSongPacks.SelectedIndexChanged += new System.EventHandler(this.cmbSongPacks_SelectedIndexChanged);
            // 
            // lblSongPack
            // 
            this.lblSongPack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSongPack.AutoSize = true;
            this.lblSongPack.Location = new System.Drawing.Point(23, 12);
            this.lblSongPack.Name = "lblSongPack";
            this.lblSongPack.Size = new System.Drawing.Size(68, 13);
            this.lblSongPack.TabIndex = 16;
            this.lblSongPack.Text = "Song Packs:";
            this.lblSongPack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.colTitle,
            this.colAlbum,
            this.colTuning,
            this.colSongYear,
            this.colSongLength,
            this.colSongKey});
            this.tlpSongPacks.SetColumnSpan(this.dgvSongPacks, 2);
            this.dgvSongPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSongPacks.Location = new System.Drawing.Point(3, 76);
            this.dgvSongPacks.Name = "dgvSongPacks";
            this.dgvSongPacks.RowHeadersVisible = false;
            this.dgvSongPacks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSongPacks.Size = new System.Drawing.Size(984, 304);
            this.dgvSongPacks.TabIndex = 4;
            this.dgvSongPacks.Tag = "Song Packs";
            this.dgvSongPacks.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongPacks_CellMouseDown);
            this.dgvSongPacks.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSongs_CellMouseUp);
            this.dgvSongPacks.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSongPacks_DataBindingComplete);
            this.dgvSongPacks.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSongPacks_Paint);
            // 
            // colSelect
            // 
            this.colSelect.DataPropertyName = "Selected";
            this.colSelect.FalseValue = "false";
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.TrueValue = "true";
            this.colSelect.ToolTipText = "Left mouse click the \'Select\' checkbox to select a row\r\nRight mouse click on row to show file operation options";
            this.colSelect.Width = 52;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnabled.Width = 72;
            // 
            // colArtist
            // 
            this.colArtist.DataPropertyName = "Artist";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArtist.Width = 120;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Song Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTitle.Width = 120;
            // 
            // colAlbum
            // 
            this.colAlbum.DataPropertyName = "Album";
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAlbum.Width = 120;
            // 
            // colTuning
            // 
            this.colTuning.DataPropertyName = "Tuning";
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            this.colTuning.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTuning.Width = 120;
            // 
            // colSongYear
            // 
            this.colSongYear.DataPropertyName = "SongYear";
            this.colSongYear.HeaderText = "Song Year";
            this.colSongYear.Name = "colSongYear";
            this.colSongYear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongYear.Width = 120;
            // 
            // colSongLength
            // 
            this.colSongLength.DataPropertyName = "SongLength";
            this.colSongLength.HeaderText = "Song Length";
            this.colSongLength.Name = "colSongLength";
            this.colSongLength.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongLength.Width = 120;
            // 
            // colSongKey
            // 
            this.colSongKey.DataPropertyName = "SongKey";
            this.colSongKey.HeaderText = "DLC Key";
            this.colSongKey.Name = "colSongKey";
            this.colSongKey.ReadOnly = true;
            this.colSongKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSongKey.Width = 120;
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
            this.panel1.Size = new System.Drawing.Size(489, 67);
            this.panel1.TabIndex = 5;
            // 
            // lnkClearSearch
            // 
            this.lnkClearSearch.AutoSize = true;
            this.lnkClearSearch.ForeColor = System.Drawing.Color.DimGray;
            this.lnkClearSearch.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkClearSearch.Location = new System.Drawing.Point(402, 12);
            this.lnkClearSearch.Name = "lnkClearSearch";
            this.lnkClearSearch.Size = new System.Drawing.Size(68, 13);
            this.lnkClearSearch.TabIndex = 17;
            this.lnkClearSearch.TabStop = true;
            this.lnkClearSearch.Text = "Clear Search";
            this.lnkClearSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearSearch_LinkClicked);
            // 
            // cueSearch
            // 
            this.cueSearch.Cue = "Type characters to search for ...";
            this.cueSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueSearch.ForeColor = System.Drawing.Color.Gray;
            this.cueSearch.Location = new System.Drawing.Point(21, 8);
            this.cueSearch.Name = "cueSearch";
            this.cueSearch.Size = new System.Drawing.Size(366, 20);
            this.cueSearch.TabIndex = 16;
            this.cueSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cueSearch_KeyUp);
            // 
            // btnSaveSongs
            // 
            this.btnSaveSongs.AutoSize = true;
            this.btnSaveSongs.Location = new System.Drawing.Point(317, 38);
            this.btnSaveSongs.Name = "btnSaveSongs";
            this.btnSaveSongs.Size = new System.Drawing.Size(69, 23);
            this.btnSaveSongs.TabIndex = 15;
            this.btnSaveSongs.Text = "Save";
            this.btnSaveSongs.UseVisualStyleBackColor = true;
            this.btnSaveSongs.Click += new System.EventHandler(this.btnSaveSongs_Click);
            // 
            // btnDisableSongs
            // 
            this.btnDisableSongs.AutoSize = true;
            this.btnDisableSongs.Location = new System.Drawing.Point(227, 38);
            this.btnDisableSongs.Name = "btnDisableSongs";
            this.btnDisableSongs.Size = new System.Drawing.Size(69, 23);
            this.btnDisableSongs.TabIndex = 13;
            this.btnDisableSongs.Text = "Disable";
            this.btnDisableSongs.UseVisualStyleBackColor = true;
            this.btnDisableSongs.Click += new System.EventHandler(this.btnDisableSongs_Click);
            // 
            // btnEnableSongs
            // 
            this.btnEnableSongs.AutoSize = true;
            this.btnEnableSongs.Location = new System.Drawing.Point(137, 38);
            this.btnEnableSongs.Name = "btnEnableSongs";
            this.btnEnableSongs.Size = new System.Drawing.Size(69, 23);
            this.btnEnableSongs.TabIndex = 14;
            this.btnEnableSongs.Text = "Enable";
            this.btnEnableSongs.UseVisualStyleBackColor = true;
            this.btnEnableSongs.Click += new System.EventHandler(this.btnEnableSongs_Click);
            // 
            // panel3
            // 
            this.tlpSongPacks.SetColumnSpan(this.panel3, 2);
            this.panel3.Controls.Add(this.lnkToggle);
            this.panel3.Controls.Add(this.lnkSelectAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 386);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(984, 14);
            this.panel3.TabIndex = 8;
            // 
            // lnkToggle
            // 
            this.lnkToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkToggle.AutoSize = true;
            this.lnkToggle.ForeColor = System.Drawing.Color.Black;
            this.lnkToggle.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkToggle.Location = new System.Drawing.Point(134, -1);
            this.lnkToggle.Name = "lnkToggle";
            this.lnkToggle.Size = new System.Drawing.Size(87, 13);
            this.lnkToggle.TabIndex = 8;
            this.lnkToggle.TabStop = true;
            this.lnkToggle.Text = "Toggle Selection";
            this.lnkToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkToggle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkToggle_LinkClicked);
            // 
            // lnkSelectAll
            // 
            this.lnkSelectAll.AutoSize = true;
            this.lnkSelectAll.ForeColor = System.Drawing.Color.Black;
            this.lnkSelectAll.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkSelectAll.Location = new System.Drawing.Point(18, -1);
            this.lnkSelectAll.Name = "lnkSelectAll";
            this.lnkSelectAll.Size = new System.Drawing.Size(82, 13);
            this.lnkSelectAll.TabIndex = 7;
            this.lnkSelectAll.TabStop = true;
            this.lnkSelectAll.Text = "Select All/None";
            this.lnkSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lnkSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelectAll_LinkClicked);
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
            this.Tag = "Song Packs";
            this.tlpSongPacks.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSongPacks)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
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
        private CueTextBox cueSearch;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.LinkLabel lnkSelectAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.LinkLabel lnkToggle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colEnabled;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTuning;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongYear;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongLength;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colSongKey;
    }
}