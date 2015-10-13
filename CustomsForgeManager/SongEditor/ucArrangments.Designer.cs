namespace CustomsForgeManager.SongEditor
{
    partial class ucArrangments
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
            this.gridArrangements = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.RADataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArrangementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRouteMask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCapoFret = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuningPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScrollSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneBase = new ToneColumn();
            this.colToneMultiplayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBonusArr = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridArrangements)).BeginInit();
            this.SuspendLayout();
            // 
            // gridArrangements
            // 
            this.gridArrangements.AllowUserToAddRows = false;
            this.gridArrangements.AllowUserToDeleteRows = false;
            this.gridArrangements.AllowUserToOrderColumns = true;
            this.gridArrangements.AllowUserToResizeRows = false;
            this.gridArrangements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArrangements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colArrangementType,
            this.colRouteMask,
            this.colCapoFret,
            this.colTuningPitch,
            this.colScrollSpeed,
            this.colToneBase,
            this.colToneMultiplayer,
            this.colToneA,
            this.colToneB,
            this.colToneC,
            this.colToneD,
            this.colBonusArr});
            this.gridArrangements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridArrangements.Location = new System.Drawing.Point(0, 0);
            this.gridArrangements.Name = "gridArrangements";
            this.gridArrangements.Size = new System.Drawing.Size(550, 366);
            this.gridArrangements.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colArrangementType
            // 
            this.colArrangementType.DataPropertyName = "ArrangementType";
            this.colArrangementType.HeaderText = "Arrangement Type";
            this.colArrangementType.Name = "colArrangementType";
            this.colArrangementType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colArrangementType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colRouteMask
            // 
            this.colRouteMask.DataPropertyName = "RouteMask";
            this.colRouteMask.HeaderText = "Route Mask";
            this.colRouteMask.Name = "colRouteMask";
            this.colRouteMask.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRouteMask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colCapoFret
            // 
            this.colCapoFret.DataPropertyName = "CapoFret";
            this.colCapoFret.HeaderText = "Capo Fret";
            this.colCapoFret.Name = "colCapoFret";
            // 
            // colTuningPitch
            // 
            this.colTuningPitch.DataPropertyName = "TuningPitch";
            this.colTuningPitch.HeaderText = "Tuning Pitch";
            this.colTuningPitch.Name = "colTuningPitch";
            // 
            // colScrollSpeed
            // 
            this.colScrollSpeed.DataPropertyName = "ScrollSpeed";
            this.colScrollSpeed.HeaderText = "Scroll Speed";
            this.colScrollSpeed.Name = "colScrollSpeed";
            // 
            // colToneBase
            // 
            this.colToneBase.DataPropertyName = "ToneBase";
            this.colToneBase.HeaderText = "Tone Base";
            this.colToneBase.Name = "colToneBase";
            this.colToneBase.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneBase.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colToneMultiplayer
            // 
            this.colToneMultiplayer.DataPropertyName = "ToneMultiplayer";
            this.colToneMultiplayer.HeaderText = "Tone Multiplayer";
            this.colToneMultiplayer.Name = "colToneMultiplayer";
            this.colToneMultiplayer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneMultiplayer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colToneA
            // 
            this.colToneA.DataPropertyName = "ToneA";
            this.colToneA.HeaderText = "ToneA";
            this.colToneA.Name = "colToneA";
            this.colToneA.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colToneB
            // 
            this.colToneB.DataPropertyName = "ToneB";
            this.colToneB.HeaderText = "ToneB";
            this.colToneB.Name = "colToneB";
            this.colToneB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colToneC
            // 
            this.colToneC.DataPropertyName = "ToneC";
            this.colToneC.HeaderText = "ToneC";
            this.colToneC.Name = "colToneC";
            this.colToneC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colToneD
            // 
            this.colToneD.DataPropertyName = "ToneD";
            this.colToneD.HeaderText = "ToneD";
            this.colToneD.Name = "colToneD";
            this.colToneD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colToneD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colBonusArr
            // 
            this.colBonusArr.DataPropertyName = "BonusArr";
            this.colBonusArr.HeaderText = "Bonus Arr";
            this.colBonusArr.Name = "colBonusArr";
            // 
            // ucArrangments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridArrangements);
            this.Name = "ucArrangments";
            this.Size = new System.Drawing.Size(550, 366);
            ((System.ComponentModel.ISupportInitialize)(this.gridArrangements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView gridArrangements;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArrangementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRouteMask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapoFret;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuningPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScrollSpeed;
        private ToneColumn colToneBase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneMultiplayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneB;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneC;
        private System.Windows.Forms.DataGridViewTextBoxColumn colToneD;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBonusArr;
    }
}
