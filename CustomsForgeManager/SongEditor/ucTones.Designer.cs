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
            this.raDataGridView1 = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.raDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // raDataGridView1
            // 
            this.raDataGridView1.AllowUserToAddRows = false;
            this.raDataGridView1.AllowUserToDeleteRows = false;
            this.raDataGridView1.AllowUserToResizeRows = false;
            this.raDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.raDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colVolume});
            this.raDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.raDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.raDataGridView1.MultiSelect = false;
            this.raDataGridView1.Name = "raDataGridView1";
            this.raDataGridView1.Size = new System.Drawing.Size(450, 349);
            this.raDataGridView1.TabIndex = 0;
            this.raDataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.raDataGridView1_CellBeginEdit);
            this.raDataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.raDataGridView1_CellContentClick);
            this.raDataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.raDataGridView1_CellEndEdit);
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
            this.Controls.Add(this.raDataGridView1);
            this.Name = "ucTones";
            this.Size = new System.Drawing.Size(450, 349);
            ((System.ComponentModel.ISupportInitialize)(this.raDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView raDataGridView1;
        private System.Windows.Forms.DataGridViewLinkColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
    }
}
