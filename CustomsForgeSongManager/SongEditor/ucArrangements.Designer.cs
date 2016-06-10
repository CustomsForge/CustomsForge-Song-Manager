using DataGridViewTools;

namespace CustomsForgeSongManager.SongEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvArrangements = new RADataGridView();
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
            this.dgvArrangements.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvArrangements.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
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
            this.dgvArrangements.RowHeadersVisible = false;
            this.dgvArrangements.Size = new System.Drawing.Size(736, 354);
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
            this.colScrollSpeed.Width = 90;
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.dgvArrangements);
            this.Name = "ucArrangements";
            this.Size = new System.Drawing.Size(736, 354);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArrangements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RADataGridView dgvArrangements;
        private System.Windows.Forms.DataGridViewLinkColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrangementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRouteMask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScrollSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuning;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuningPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneBase;
    }
}
