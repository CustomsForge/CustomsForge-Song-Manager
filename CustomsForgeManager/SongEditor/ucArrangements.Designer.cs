namespace CustomsForgeManager.SongEditor
{
    partial class ucArrangements
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
            this.dgvArrangements = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colArrangementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRouteMask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScrollSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuning = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuningPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneBase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrangements)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvArrangements
            // 
            this.dgvArrangements.AllowUserToAddRows = false;
            this.dgvArrangements.AllowUserToDeleteRows = false;
            this.dgvArrangements.AllowUserToOrderColumns = true;
            this.dgvArrangements.AllowUserToResizeRows = false;
            this.dgvArrangements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArrangements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colArrangementType,
            this.colRouteMask,
            this.colScrollSpeed,
            this.colTuning,
            this.colTuningPitch,
            this.colToneBase});
            this.dgvArrangements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArrangements.Location = new System.Drawing.Point(0, 0);
            this.dgvArrangements.Name = "dgvArrangements";
            this.dgvArrangements.Size = new System.Drawing.Size(810, 409);
            this.dgvArrangements.TabIndex = 0;
            this.dgvArrangements.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvArrangements_CellBeginEdit);
            this.dgvArrangements.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrangements_CellContentClick);
            this.dgvArrangements.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArrangements_CellEndEdit);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colArrangementType
            // 
            this.colArrangementType.DataPropertyName = "ArrangementType";
            this.colArrangementType.HeaderText = "Arrangement Type";
            this.colArrangementType.Name = "colArrangementType";
            this.colArrangementType.ReadOnly = true;
            this.colArrangementType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArrangementType.Width = 120;
            // 
            // colRouteMask
            // 
            this.colRouteMask.DataPropertyName = "RouteMask";
            this.colRouteMask.HeaderText = "Route Mask";
            this.colRouteMask.Name = "colRouteMask";
            this.colRouteMask.ReadOnly = true;
            this.colRouteMask.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colScrollSpeed
            // 
            this.colScrollSpeed.DataPropertyName = "ScrollSpeed";
            this.colScrollSpeed.HeaderText = "Scroll Speed";
            this.colScrollSpeed.Name = "colScrollSpeed";
            // 
            // colTuning
            // 
            this.colTuning.DataPropertyName = "Tuning";
            this.colTuning.HeaderText = "Tuning";
            this.colTuning.Name = "colTuning";
            this.colTuning.ReadOnly = true;
            // 
            // colTuningPitch
            // 
            this.colTuningPitch.DataPropertyName = "TuningPitch";
            this.colTuningPitch.HeaderText = "Tuning Pitch";
            this.colTuningPitch.Name = "colTuningPitch";
            // 
            // colToneBase
            // 
            this.colToneBase.DataPropertyName = "ToneBase";
            this.colToneBase.HeaderText = "Tone Base";
            this.colToneBase.Name = "colToneBase";
            this.colToneBase.ReadOnly = true;
            this.colToneBase.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ucArrangements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvArrangements);
            this.Name = "ucArrangements";
            this.Size = new System.Drawing.Size(810, 409);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrangements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView dgvArrangements;
        private System.Windows.Forms.DataGridViewLinkColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrangementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRouteMask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScrollSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuningPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneBase;
    }
}
