namespace CustomsForgeManager.SongEditor
{
    partial class ucTones
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
            this.dgvTones = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTones)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTones
            // 
            this.dgvTones.AllowUserToAddRows = false;
            this.dgvTones.AllowUserToDeleteRows = false;
            this.dgvTones.AllowUserToResizeRows = false;
            this.dgvTones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colVolume});
            this.dgvTones.Location = new System.Drawing.Point(0, 0);
            this.dgvTones.MultiSelect = false;
            this.dgvTones.Name = "dgvTones";
            this.dgvTones.Size = new System.Drawing.Size(450, 349);
            this.dgvTones.TabIndex = 0;
            this.dgvTones.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.raDataGridView1_CellBeginEdit);
            this.dgvTones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.raDataGridView1_CellContentClick);
            this.dgvTones.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.raDataGridView1_CellEndEdit);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 300;
            // 
            // colVolume
            // 
            this.colVolume.DataPropertyName = "Volume";
            this.colVolume.HeaderText = "Volume";
            this.colVolume.Name = "colVolume";
            // 
            // ucTones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvTones);
            this.Name = "ucTones";
            this.Size = new System.Drawing.Size(494, 349);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvTones;
        private System.Windows.Forms.DataGridViewLinkColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
    }
}
