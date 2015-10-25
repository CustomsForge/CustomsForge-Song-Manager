using System.Windows.Forms;

namespace CustomsForgeManager.UControls
{
    partial class Duplicates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Duplicates));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmsDuplicate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exploreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.panelActions = new System.Windows.Forms.Panel();
            this.lnkPersistentId = new System.Windows.Forms.LinkLabel();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnDeleteSong = new System.Windows.Forms.Button();
            this.btnEnableDisable = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.txtNoDuplicates = new System.Windows.Forms.TextBox();
            this.dgvDups = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArrangementPID = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colArtist = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colTitle = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colAlbum = new DataGridViewTools.DataGridViewAutoFilterTextBoxColumn();
            this.colUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsDuplicate.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDups)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsDuplicate
            // 
            this.cmsDuplicate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exploreToolStripMenuItem});
            this.cmsDuplicate.Name = "cmsDuplicate";
            this.cmsDuplicate.Size = new System.Drawing.Size(113, 26);
            // 
            // exploreToolStripMenuItem
            // 
            this.exploreToolStripMenuItem.Name = "exploreToolStripMenuItem";
            this.exploreToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exploreToolStripMenuItem.Text = "Explore";
            this.exploreToolStripMenuItem.Click += new System.EventHandler(this.exploreToolStripMenuItem_Click);
            // 
            // gbActions
            // 
            this.gbActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActions.Controls.Add(this.panelActions);
            this.gbActions.Location = new System.Drawing.Point(5, 457);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(864, 60);
            this.gbActions.TabIndex = 14;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions:";
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.lnkPersistentId);
            this.panelActions.Controls.Add(this.btnMove);
            this.panelActions.Controls.Add(this.btnDeleteSong);
            this.panelActions.Controls.Add(this.btnEnableDisable);
            this.panelActions.Controls.Add(this.btnRescan);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(3, 16);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(858, 41);
            this.panelActions.TabIndex = 3;
            // 
            // lnkPersistentId
            // 
            this.lnkPersistentId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkPersistentId.AutoSize = true;
            this.lnkPersistentId.ForeColor = System.Drawing.Color.Black;
            this.lnkPersistentId.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.Location = new System.Drawing.Point(698, 13);
            this.lnkPersistentId.Name = "lnkPersistentId";
            this.lnkPersistentId.Size = new System.Drawing.Size(145, 13);
            this.lnkPersistentId.TabIndex = 18;
            this.lnkPersistentId.TabStop = true;
            this.lnkPersistentId.Text = "Select SongInfo/PersistentID";
            this.lnkPersistentId.VisitedLinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkPersistentId.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPersistentId_LinkClicked);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMove.Image = global::CustomsForgeManager.Properties.Resources.export;
            this.btnMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMove.Location = new System.Drawing.Point(416, 6);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(112, 27);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "Move Selected";
            this.btnMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMove.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSong
            // 
            this.btnDeleteSong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSong.Image = global::CustomsForgeManager.Properties.Resources.delete;
            this.btnDeleteSong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSong.Location = new System.Drawing.Point(280, 6);
            this.btnDeleteSong.Name = "btnDeleteSong";
            this.btnDeleteSong.Size = new System.Drawing.Size(112, 27);
            this.btnDeleteSong.TabIndex = 11;
            this.btnDeleteSong.Text = "Delete Selected";
            this.btnDeleteSong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteSong.UseVisualStyleBackColor = true;
            // 
            // btnEnableDisable
            // 
            this.btnEnableDisable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnableDisable.Image = global::CustomsForgeManager.Properties.Resources.disable;
            this.btnEnableDisable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnableDisable.Location = new System.Drawing.Point(106, 5);
            this.btnEnableDisable.Name = "btnEnableDisable";
            this.btnEnableDisable.Size = new System.Drawing.Size(150, 29);
            this.btnEnableDisable.TabIndex = 14;
            this.btnEnableDisable.Text = "Enable/Disable Selected";
            this.btnEnableDisable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnableDisable.UseVisualStyleBackColor = true;
            this.btnEnableDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRescan.Image = ((System.Drawing.Image)(resources.GetObject("btnRescan.Image")));
            this.btnRescan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRescan.Location = new System.Drawing.Point(8, 5);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(75, 29);
            this.btnRescan.TabIndex = 4;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // gbResults
            // 
            this.gbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbResults.Controls.Add(this.txtNoDuplicates);
            this.gbResults.Controls.Add(this.dgvDups);
            this.gbResults.Location = new System.Drawing.Point(4, 3);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(864, 454);
            this.gbResults.TabIndex = 16;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results Grid:";
            // 
            // txtNoDuplicates
            // 
            this.txtNoDuplicates.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoDuplicates.Location = new System.Drawing.Point(279, 157);
            this.txtNoDuplicates.Multiline = true;
            this.txtNoDuplicates.Name = "txtNoDuplicates";
            this.txtNoDuplicates.Size = new System.Drawing.Size(307, 104);
            this.txtNoDuplicates.TabIndex = 16;
            this.txtNoDuplicates.Text = "\r\nGood News ...\r\nNo Duplicates Found";
            this.txtNoDuplicates.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dgvDups
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDups.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colEnabled,
            this.colPID,
            this.colArrangementPID,
            this.colArtist,
            this.colTitle,
            this.colAlbum,
            this.colUpdated,
            this.colPath});
            this.dgvDups.ContextMenuStrip = this.cmsDuplicate;
            this.dgvDups.Location = new System.Drawing.Point(6, 19);
            this.dgvDups.Name = "dgvDups";
            this.dgvDups.RowHeadersVisible = false;
            this.dgvDups.Size = new System.Drawing.Size(852, 429);
            this.dgvDups.TabIndex = 15;
            this.dgvDups.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDups_CellDoubleClick);
            this.dgvDups.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDups_CellFormatting);
            this.dgvDups.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDups_CellMouseUp);
            this.dgvDups.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvDups_DataBindingComplete);
            this.dgvDups.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDups_DataError);
            this.dgvDups.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvDups_Paint);
            this.dgvDups.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDups_KeyDown);
            // 
            // colSelect
            // 
            this.colSelect.DataPropertyName = "Select";
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.ReadOnly = true;
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 54;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "Enabled";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.ReadOnly = true;
            this.colEnabled.Width = 71;
            // 
            // colPID
            // 
            this.colPID.DataPropertyName = "PID";
            this.colPID.HeaderText = "Persistent ID";
            this.colPID.Name = "colPID";
            this.colPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPID.Width = 110;
            // 
            // colArrangementPID
            // 
            this.colArrangementPID.DataPropertyName = "ArrangementPID";
            this.colArrangementPID.HeaderText = "Arrangement";
            this.colArrangementPID.Name = "colArrangementPID";
            this.colArrangementPID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArrangementPID.Width = 110;
            // 
            // colArtist
            // 
            this.colArtist.DataPropertyName = "Artist";
            this.colArtist.HeaderText = "Artist";
            this.colArtist.Name = "colArtist";
            this.colArtist.ReadOnly = true;
            this.colArtist.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArtist.Width = 73;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Song Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTitle.Width = 98;
            // 
            // colAlbum
            // 
            this.colAlbum.DataPropertyName = "Album";
            this.colAlbum.HeaderText = "Album";
            this.colAlbum.Name = "colAlbum";
            this.colAlbum.ReadOnly = true;
            this.colAlbum.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAlbum.Width = 79;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "LastConversionDateTime";
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Width = 73;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 54;
            // 
            // Duplicates
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.gbActions);
            this.Name = "Duplicates";
            this.Size = new System.Drawing.Size(872, 525);
            this.cmsDuplicate.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.panelActions.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDeleteSong;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnEnableDisable;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnMove;
        private ContextMenuStrip cmsDuplicate;
        private ToolStripMenuItem exploreToolStripMenuItem;
        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvDups;
        private GroupBox gbResults;
        private DataGridViewCheckBoxColumn colSelect;
        private DataGridViewTextBoxColumn colEnabled;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colPID;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArrangementPID;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colArtist;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colTitle;
        private DataGridViewTools.DataGridViewAutoFilterTextBoxColumn colAlbum;
        private DataGridViewTextBoxColumn colUpdated;
        private DataGridViewTextBoxColumn colPath;
        private LinkLabel lnkPersistentId;
        private TextBox txtNoDuplicates;
    }
}
