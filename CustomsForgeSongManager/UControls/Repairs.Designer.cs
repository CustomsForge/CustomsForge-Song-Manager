namespace CustomsForgeSongManager.UControls
{
    partial class Repairs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Repairs));
            this.tlpRepairs = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblFailedRepairs = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveBackup = new System.Windows.Forms.Button();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnRepairCDLCs = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblBetaFeature = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnShowLogOfFailedRepairs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.colRepairsCurrDLC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpRepairs.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpRepairs
            // 
            this.tlpRepairs.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tlpRepairs.ColumnCount = 1;
            this.tlpRepairs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRepairs.Controls.Add(this.panel2, 0, 2);
            this.tlpRepairs.Controls.Add(this.panel1, 0, 1);
            this.tlpRepairs.Controls.Add(this.panel3, 0, 0);
            this.tlpRepairs.Controls.Add(this.panel4, 0, 3);
            this.tlpRepairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRepairs.Location = new System.Drawing.Point(0, 0);
            this.tlpRepairs.Name = "tlpRepairs";
            this.tlpRepairs.RowCount = 4;
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRepairs.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblFailedRepairs);
            this.panel2.Controls.Add(this.lblProgress);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 201);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(889, 42);
            this.panel2.TabIndex = 5;
            // 
            // lblFailedRepairs
            // 
            this.lblFailedRepairs.AutoSize = true;
            this.lblFailedRepairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblFailedRepairs.Location = new System.Drawing.Point(154, 13);
            this.lblFailedRepairs.Name = "lblFailedRepairs";
            this.lblFailedRepairs.Size = new System.Drawing.Size(59, 16);
            this.lblFailedRepairs.TabIndex = 1;
            this.lblFailedRepairs.Text = "Failed: 0";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblProgress.Location = new System.Drawing.Point(23, 13);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(87, 16);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "Progress: 0/0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveBackup);
            this.panel1.Controls.Add(this.btnRestoreBackup);
            this.panel1.Controls.Add(this.btnRepairCDLCs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(889, 138);
            this.panel1.TabIndex = 4;
            // 
            // btnRemoveBackup
            // 
            this.btnRemoveBackup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemoveBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemoveBackup.Image = global::CustomsForgeSongManager.Properties.Resources.deleteBackup;
            this.btnRemoveBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveBackup.Location = new System.Drawing.Point(216, 86);
            this.btnRemoveBackup.Name = "btnRemoveBackup";
            this.btnRemoveBackup.Size = new System.Drawing.Size(187, 47);
            this.btnRemoveBackup.TabIndex = 5;
            this.btnRemoveBackup.Text = "Remove backup";
            this.btnRemoveBackup.UseVisualStyleBackColor = true;
            this.btnRemoveBackup.Click += new System.EventHandler(this.btnRemoveBackup_Click);
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRestoreBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRestoreBackup.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreBackup.Image")));
            this.btnRestoreBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestoreBackup.Location = new System.Drawing.Point(448, 86);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(187, 47);
            this.btnRestoreBackup.TabIndex = 4;
            this.btnRestoreBackup.Text = "Restore backup";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnRepairCDLCs
            // 
            this.btnRepairCDLCs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRepairCDLCs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRepairCDLCs.Image = global::CustomsForgeSongManager.Properties.Resources.maintenance_48;
            this.btnRepairCDLCs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRepairCDLCs.Location = new System.Drawing.Point(115, 20);
            this.btnRepairCDLCs.Name = "btnRepairCDLCs";
            this.btnRepairCDLCs.Size = new System.Drawing.Size(641, 57);
            this.btnRepairCDLCs.TabIndex = 3;
            this.btnRepairCDLCs.Text = "Repair all your CDLCs (remove 100% mastery bug)";
            this.btnRepairCDLCs.UseVisualStyleBackColor = true;
            this.btnRepairCDLCs.Click += new System.EventHandler(this.btnRepairCDLCs_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblBetaFeature);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(889, 42);
            this.panel3.TabIndex = 6;
            // 
            // lblBetaFeature
            // 
            this.lblBetaFeature.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBetaFeature.AutoSize = true;
            this.lblBetaFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblBetaFeature.Location = new System.Drawing.Point(20, 10);
            this.lblBetaFeature.Name = "lblBetaFeature";
            this.lblBetaFeature.Size = new System.Drawing.Size(836, 24);
            this.lblBetaFeature.TabIndex = 3;
            this.lblBetaFeature.Text = "NOTE: This is a beta feature, use it carefully and please report every bug you en" +
    "counter!";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnShowLogOfFailedRepairs);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.dgvLog);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 251);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(889, 234);
            this.panel4.TabIndex = 7;
            // 
            // btnShowLogOfFailedRepairs
            // 
            this.btnShowLogOfFailedRepairs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowLogOfFailedRepairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnShowLogOfFailedRepairs.Image = global::CustomsForgeSongManager.Properties.Resources.log_icon;
            this.btnShowLogOfFailedRepairs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowLogOfFailedRepairs.Location = new System.Drawing.Point(310, 192);
            this.btnShowLogOfFailedRepairs.Name = "btnShowLogOfFailedRepairs";
            this.btnShowLogOfFailedRepairs.Size = new System.Drawing.Size(238, 37);
            this.btnShowLogOfFailedRepairs.TabIndex = 10;
            this.btnShowLogOfFailedRepairs.Text = "Show the log of failed repairs";
            this.btnShowLogOfFailedRepairs.UseVisualStyleBackColor = true;
            this.btnShowLogOfFailedRepairs.Click += new System.EventHandler(this.btnShowLogOfFailedRepairs_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Log:";
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRepairsCurrDLC,
            this.Message});
            this.dgvLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLog.Location = new System.Drawing.Point(24, 28);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.Size = new System.Drawing.Size(832, 158);
            this.dgvLog.TabIndex = 8;
            // 
            // colRepairsCurrDLC
            // 
            this.colRepairsCurrDLC.HeaderText = "CDLC";
            this.colRepairsCurrDLC.MinimumWidth = 100;
            this.colRepairsCurrDLC.Name = "colRepairsCurrDLC";
            // 
            // Message
            // 
            this.Message.HeaderText = "Repairs";
            this.Message.Name = "Message";
            // 
            // Repairs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRepairs);
            this.Name = "Repairs";
            this.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRepairs;
        private System.Windows.Forms.Label lblBetaFeature;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRepairCDLCs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblFailedRepairs;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnRemoveBackup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepairsCurrDLC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnShowLogOfFailedRepairs;

    }
}
