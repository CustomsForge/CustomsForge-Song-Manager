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
            this.colArrangementType = new CustomsForgeManager.SongEditor.ArrangementTypeColumn();
            this.colRouteMask = new CustomsForgeManager.SongEditor.RouteMaskColumn();
            this.colCapoFret = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuningPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScrollSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colToneBase = new CustomsForgeManager.SongEditor.ToneColumn();
            this.colToneMultiplayer = new CustomsForgeManager.SongEditor.ToneColumn();
            this.colToneA = new CustomsForgeManager.SongEditor.ToneColumn();
            this.colToneB = new CustomsForgeManager.SongEditor.ToneColumn();
            this.colToneC = new CustomsForgeManager.SongEditor.ToneColumn();
            this.colToneD = new CustomsForgeManager.SongEditor.ToneColumn();
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
            this.gridArrangements.Size = new System.Drawing.Size(1359, 366);
            this.gridArrangements.TabIndex = 0;            
            this.gridArrangements.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridArrangements_CellValueChanged);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.MaxInputLength = 50;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colArrangementType
            // 
            this.colArrangementType.DataPropertyName = "ArrangementType";
            this.colArrangementType.HeaderText = "Arrangement Type";
            this.colArrangementType.Name = "colArrangementType";
            this.colArrangementType.ReadOnly = true;
            this.colArrangementType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colRouteMask
            // 
            this.colRouteMask.DataPropertyName = "RouteMask";
            this.colRouteMask.HeaderText = "Route Mask";
            this.colRouteMask.Name = "colRouteMask";
            this.colRouteMask.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colCapoFret
            // 
            this.colCapoFret.DataPropertyName = "CapoFret";
            this.colCapoFret.HeaderText = "Capo Fret";
            this.colCapoFret.MaxInputLength = 4;
            this.colCapoFret.Name = "colCapoFret";
            this.colCapoFret.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colTuningPitch
            // 
            this.colTuningPitch.DataPropertyName = "TuningPitch";
            this.colTuningPitch.HeaderText = "Tuning Pitch";
            this.colTuningPitch.MaxInputLength = 10;
            this.colTuningPitch.Name = "colTuningPitch";
            // 
            // colScrollSpeed
            // 
            this.colScrollSpeed.DataPropertyName = "ScrollSpeed";
            this.colScrollSpeed.HeaderText = "Scroll Speed";
            this.colScrollSpeed.MaxInputLength = 10;
            this.colScrollSpeed.Name = "colScrollSpeed";
            // 
            // colToneBase
            // 
            this.colToneBase.DataPropertyName = "ToneBase";
            this.colToneBase.HeaderText = "Tone Base";
            this.colToneBase.Name = "colToneBase";
            this.colToneBase.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToneMultiplayer
            // 
            this.colToneMultiplayer.DataPropertyName = "ToneMultiplayer";
            this.colToneMultiplayer.HeaderText = "Tone Multiplayer";
            this.colToneMultiplayer.Name = "colToneMultiplayer";
            this.colToneMultiplayer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToneA
            // 
            this.colToneA.DataPropertyName = "ToneA";
            this.colToneA.HeaderText = "ToneA";
            this.colToneA.Name = "colToneA";
            this.colToneA.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToneB
            // 
            this.colToneB.DataPropertyName = "ToneB";
            this.colToneB.HeaderText = "ToneB";
            this.colToneB.Name = "colToneB";
            this.colToneB.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToneC
            // 
            this.colToneC.DataPropertyName = "ToneC";
            this.colToneC.HeaderText = "ToneC";
            this.colToneC.Name = "colToneC";
            this.colToneC.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colToneD
            // 
            this.colToneD.DataPropertyName = "ToneD";
            this.colToneD.HeaderText = "ToneD";
            this.colToneD.Name = "colToneD";
            this.colToneD.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            this.Size = new System.Drawing.Size(1359, 366);
            ((System.ComponentModel.ISupportInitialize)(this.gridArrangements)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomsForgeManagerLib.CustomControls.RADataGridView gridArrangements;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private ArrangementTypeColumn colArrangementType;
        private RouteMaskColumn colRouteMask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapoFret;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTuningPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScrollSpeed;
        private ToneColumn colToneBase;
        private ToneColumn colToneMultiplayer;
        private ToneColumn colToneA;
        private ToneColumn colToneB;
        private ToneColumn colToneC;
        private ToneColumn colToneD;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colBonusArr;
    }
}
