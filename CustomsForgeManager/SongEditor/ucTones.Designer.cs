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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dgvTones.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTones.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colVolume});
            this.dgvTones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTones.Location = new System.Drawing.Point(0, 0);
            this.dgvTones.MultiSelect = false;
            this.dgvTones.Name = "dgvTones";
            this.dgvTones.RowHeadersVisible = false;
            this.dgvTones.Size = new System.Drawing.Size(430, 294);
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
            this.Size = new System.Drawing.Size(430, 294);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvTones;
        private System.Windows.Forms.DataGridViewLinkColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
    }
}
