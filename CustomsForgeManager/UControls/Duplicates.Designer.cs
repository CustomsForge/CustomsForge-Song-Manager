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
            this.btnMove = new System.Windows.Forms.Button();
            this.btnDeleteSong = new System.Windows.Forms.Button();
            this.btnEnableDisable = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.dgvDups = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongArtist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSongAlbum = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.cmsDuplicate.Size = new System.Drawing.Size(153, 48);
            // 
            // exploreToolStripMenuItem
            // 
            this.exploreToolStripMenuItem.Name = "exploreToolStripMenuItem";
            this.exploreToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exploreToolStripMenuItem.Text = "Explore";
            this.exploreToolStripMenuItem.Click += new System.EventHandler(this.exploreToolStripMenuItem_Click);
            // 
            // gbActions
            // 
            this.gbActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActions.Controls.Add(this.panelActions);
            this.gbActions.Location = new System.Drawing.Point(5, 422);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(982, 60);
            this.gbActions.TabIndex = 14;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions:";
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnMove);
            this.panelActions.Controls.Add(this.btnDeleteSong);
            this.panelActions.Controls.Add(this.btnEnableDisable);
            this.panelActions.Controls.Add(this.btnRescan);
            this.panelActions.Location = new System.Drawing.Point(6, 13);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(964, 41);
            this.panelActions.TabIndex = 3;
            // 
            // btnMove
            // 
            this.btnMove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMove.Image = global::CustomsForgeManager.Properties.Resources.export;
            this.btnMove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMove.Location = new System.Drawing.Point(425, 7);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(112, 27);
            this.btnMove.TabIndex = 15;
            this.btnMove.Text = "Move Selected";
            this.btnMove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMove.UseVisualStyleBackColor = true;
            // 
            // btnDeleteSong
            // 
            this.btnDeleteSong.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteSong.Image = global::CustomsForgeManager.Properties.Resources.delete;
            this.btnDeleteSong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSong.Location = new System.Drawing.Point(289, 7);
            this.btnDeleteSong.Name = "btnDeleteSong";
            this.btnDeleteSong.Size = new System.Drawing.Size(112, 27);
            this.btnDeleteSong.TabIndex = 11;
            this.btnDeleteSong.Text = "Delete Selected";
            this.btnDeleteSong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteSong.UseVisualStyleBackColor = true;
            // 
            // btnEnableDisable
            // 
            this.btnEnableDisable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEnableDisable.Image = global::CustomsForgeManager.Properties.Resources.disable;
            this.btnEnableDisable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnableDisable.Location = new System.Drawing.Point(115, 6);
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
            this.btnRescan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRescan.Image = ((System.Drawing.Image)(resources.GetObject("btnRescan.Image")));
            this.btnRescan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRescan.Location = new System.Drawing.Point(17, 6);
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
            this.gbResults.Controls.Add(this.dgvDups);
            this.gbResults.Location = new System.Drawing.Point(4, 3);
            this.gbResults.Name = "gbResults";
            this.gbResults.Size = new System.Drawing.Size(982, 419);
            this.gbResults.TabIndex = 16;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "Results Grid:";
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
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colEnabled,
            this.colSongArtist,
            this.colSongTitle,
            this.colSongAlbum,
            this.colUpdated,
            this.colPath});
            this.dgvDups.ContextMenuStrip = this.cmsDuplicate;
            this.dgvDups.Location = new System.Drawing.Point(6, 19);
            this.dgvDups.Name = "dgvDups";
            this.dgvDups.RowHeadersVisible = false;
            this.dgvDups.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvDups.Size = new System.Drawing.Size(970, 394);
            this.dgvDups.TabIndex = 15;
            this.dgvDups.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDups_CellDoubleClick);
            this.dgvDups.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDups_CellMouseUp);
            this.dgvDups.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDups_KeyDown);
            // 
            // colSelect
            // 
            this.colSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.ReadOnly = true;
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 43;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
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
            this.colSongArtist.Width = 50;
            // 
            // colSongTitle
            // 
            this.colSongTitle.DataPropertyName = "Song";
            this.colSongTitle.HeaderText = "Song Title";
            this.colSongTitle.Name = "colSongTitle";
            this.colSongTitle.ReadOnly = true;
            this.colSongTitle.Width = 50;
            // 
            // colSongAlbum
            // 
            this.colSongAlbum.DataPropertyName = "Album";
            this.colSongAlbum.HeaderText = "Album";
            this.colSongAlbum.Name = "colSongAlbum";
            this.colSongAlbum.ReadOnly = true;
            this.colSongAlbum.Width = 50;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "Updated";
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            this.colUpdated.Width = 50;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 50;
            // 
            // Duplicates
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.gbActions);
            this.Name = "Duplicates";
            this.Size = new System.Drawing.Size(990, 490);
            this.cmsDuplicate.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.gbResults.ResumeLayout(false);
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
        private DataGridViewTextBoxColumn colSongArtist;
        private DataGridViewTextBoxColumn colSongTitle;
        private DataGridViewTextBoxColumn colSongAlbum;
        private DataGridViewTextBoxColumn colUpdated;
        private DataGridViewTextBoxColumn colPath;
    }
}
