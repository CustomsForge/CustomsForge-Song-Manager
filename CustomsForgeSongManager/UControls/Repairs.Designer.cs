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
            this.components = new System.ComponentModel.Container();
            this.tlpRepairs = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteCorruptFiles = new System.Windows.Forms.Button();
            this.btnArchiveCorruptCDLC = new System.Windows.Forms.Button();
            this.chkRepairOrg = new System.Windows.Forms.CheckBox();
            this.chkPreserve = new System.Windows.Forms.CheckBox();
            this.btnCleanupDlcFolder = new System.Windows.Forms.Button();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnRemasterAllCDLC = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblBetaFeature = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.dgvLog = new DataGridViewTools.SubclassedDataGridView();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnViewErrorLog = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tlpRepairs.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.gbLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpRepairs
            // 
            this.tlpRepairs.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tlpRepairs.ColumnCount = 1;
            this.tlpRepairs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRepairs.Controls.Add(this.panel1, 0, 1);
            this.tlpRepairs.Controls.Add(this.panel3, 0, 0);
            this.tlpRepairs.Controls.Add(this.panel4, 0, 2);
            this.tlpRepairs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRepairs.Location = new System.Drawing.Point(0, 0);
            this.tlpRepairs.Name = "tlpRepairs";
            this.tlpRepairs.RowCount = 3;
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRepairs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRepairs.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDeleteCorruptFiles);
            this.panel1.Controls.Add(this.btnArchiveCorruptCDLC);
            this.panel1.Controls.Add(this.chkRepairOrg);
            this.panel1.Controls.Add(this.chkPreserve);
            this.panel1.Controls.Add(this.btnCleanupDlcFolder);
            this.panel1.Controls.Add(this.btnRestoreBackup);
            this.panel1.Controls.Add(this.btnRemasterAllCDLC);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(889, 94);
            this.panel1.TabIndex = 4;
            // 
            // btnDeleteCorruptFiles
            // 
            this.btnDeleteCorruptFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnDeleteCorruptFiles.Image = global::CustomsForgeSongManager.Properties.Resources.delete2;
            this.btnDeleteCorruptFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteCorruptFiles.Location = new System.Drawing.Point(676, 53);
            this.btnDeleteCorruptFiles.Name = "btnDeleteCorruptFiles";
            this.btnDeleteCorruptFiles.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnDeleteCorruptFiles.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDeleteCorruptFiles.Size = new System.Drawing.Size(182, 32);
            this.btnDeleteCorruptFiles.TabIndex = 9;
            this.btnDeleteCorruptFiles.Text = "     Delete Corrupt CDLC";
            this.toolTip.SetToolTip(this.btnDeleteCorruptFiles, "Delete corrupt CDLC (.cor.psarc) files.");
            this.btnDeleteCorruptFiles.UseVisualStyleBackColor = true;
            this.btnDeleteCorruptFiles.Click += new System.EventHandler(this.btnDeleteCorruptFiles_Click);
            // 
            // btnArchiveCorruptCDLC
            // 
            this.btnArchiveCorruptCDLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnArchiveCorruptCDLC.Image = global::CustomsForgeSongManager.Properties.Resources.zip;
            this.btnArchiveCorruptCDLC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArchiveCorruptCDLC.Location = new System.Drawing.Point(458, 53);
            this.btnArchiveCorruptCDLC.Name = "btnArchiveCorruptCDLC";
            this.btnArchiveCorruptCDLC.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnArchiveCorruptCDLC.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnArchiveCorruptCDLC.Size = new System.Drawing.Size(182, 32);
            this.btnArchiveCorruptCDLC.TabIndex = 8;
            this.btnArchiveCorruptCDLC.Text = "     Archive Corrupt CDLC";
            this.toolTip.SetToolTip(this.btnArchiveCorruptCDLC, "Archive corrupt CDLC to a zip file.");
            this.btnArchiveCorruptCDLC.UseVisualStyleBackColor = true;
            this.btnArchiveCorruptCDLC.Click += new System.EventHandler(this.btnArchiveCorruptCDLC_Click);
            // 
            // chkRepairOrg
            // 
            this.chkRepairOrg.AutoSize = true;
            this.chkRepairOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRepairOrg.Location = new System.Drawing.Point(676, 16);
            this.chkRepairOrg.Name = "chkRepairOrg";
            this.chkRepairOrg.Size = new System.Drawing.Size(185, 19);
            this.chkRepairOrg.TabIndex = 7;
            this.chkRepairOrg.Text = "Run Repair Using (.org) Files";
            this.toolTip.SetToolTip(this.chkRepairOrg, "WARNING:\r\nExisting CDLC in the \'dlc\' folder that have\r\nthe same name will be over" +
                    "written.\r\n\r\nIf checked run repair using the (.org.psarc) files.");
            this.chkRepairOrg.UseVisualStyleBackColor = true;
            // 
            // chkPreserve
            // 
            this.chkPreserve.AutoSize = true;
            this.chkPreserve.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreserve.Location = new System.Drawing.Point(458, 16);
            this.chkPreserve.Name = "chkPreserve";
            this.chkPreserve.Size = new System.Drawing.Size(182, 19);
            this.chkPreserve.TabIndex = 6;
            this.chkPreserve.Text = "Preserve Existing Song Stats";
            this.toolTip.SetToolTip(this.chkPreserve, "If checked preserves the existing song\r\nstats for CDLC that have not been\r\nplayed" +
                    " in Rocksmith 2014 Remastered.");
            this.chkPreserve.UseVisualStyleBackColor = true;
            // 
            // btnCleanupDlcFolder
            // 
            this.btnCleanupDlcFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCleanupDlcFolder.Image = global::CustomsForgeSongManager.Properties.Resources.broom;
            this.btnCleanupDlcFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCleanupDlcFolder.Location = new System.Drawing.Point(22, 53);
            this.btnCleanupDlcFolder.Name = "btnCleanupDlcFolder";
            this.btnCleanupDlcFolder.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnCleanupDlcFolder.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCleanupDlcFolder.Size = new System.Drawing.Size(182, 32);
            this.btnCleanupDlcFolder.TabIndex = 5;
            this.btnCleanupDlcFolder.Text = "     Cleanup \'dlc\' Folder";
            this.toolTip.SetToolTip(this.btnCleanupDlcFolder, "Removes (.org.psarc) files if they\r\nexist from the \'dlc\' folder and saves\r\nthem t" +
                    "o the \'backup\' folder.");
            this.btnCleanupDlcFolder.UseVisualStyleBackColor = true;
            this.btnCleanupDlcFolder.Click += new System.EventHandler(this.btnCleanupDlcFolder_Click);
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRestoreBackup.Image = global::CustomsForgeSongManager.Properties.Resources.restorewindow;
            this.btnRestoreBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestoreBackup.Location = new System.Drawing.Point(240, 53);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnRestoreBackup.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRestoreBackup.Size = new System.Drawing.Size(182, 32);
            this.btnRestoreBackup.TabIndex = 4;
            this.btnRestoreBackup.Text = "     Restore (.org) Backups";
            this.toolTip.SetToolTip(this.btnRestoreBackup, "WARNING:\r\nOverwrites files that have the same name\r\n\r\nRestore (.org.psarc) files " +
                    "to the \'dlc\' folder\r\nso that a full repair may be run again.\r\n");
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnRemasterAllCDLC
            // 
            this.btnRemasterAllCDLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRemasterAllCDLC.Image = global::CustomsForgeSongManager.Properties.Resources.maintenance;
            this.btnRemasterAllCDLC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemasterAllCDLC.Location = new System.Drawing.Point(22, 9);
            this.btnRemasterAllCDLC.Name = "btnRemasterAllCDLC";
            this.btnRemasterAllCDLC.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnRemasterAllCDLC.Size = new System.Drawing.Size(400, 32);
            this.btnRemasterAllCDLC.TabIndex = 3;
            this.btnRemasterAllCDLC.Text = "Remaster All CDLC Files (remove 100% mastery bug)";
            this.toolTip.SetToolTip(this.btnRemasterAllCDLC, "Remaster any CDLC that are located\r\nin the \'dlc\' folder or subfolders.\r\n\r\nCheck t" +
                    "he appropriate boxes first.");
            this.btnRemasterAllCDLC.UseVisualStyleBackColor = true;
            this.btnRemasterAllCDLC.Click += new System.EventHandler(this.btnRemasterAllCDLC_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblBetaFeature);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(889, 34);
            this.panel3.TabIndex = 6;
            // 
            // lblBetaFeature
            // 
            this.lblBetaFeature.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblBetaFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblBetaFeature.Location = new System.Drawing.Point(26, 5);
            this.lblBetaFeature.Name = "lblBetaFeature";
            this.lblBetaFeature.Size = new System.Drawing.Size(836, 24);
            this.lblBetaFeature.TabIndex = 3;
            this.lblBetaFeature.Text = "NOTE: This is a beta feature, use it carefully and please report every bug you en" +
                "counter!";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gbLog);
            this.panel4.Controls.Add(this.btnViewErrorLog);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(889, 336);
            this.panel4.TabIndex = 7;
            // 
            // gbLog
            // 
            this.gbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLog.Controls.Add(this.dgvLog);
            this.gbLog.Location = new System.Drawing.Point(3, 3);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(883, 295);
            this.gbLog.TabIndex = 11;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Log:";
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFileName,
            this.colMessage});
            this.dgvLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLog.HorizontalScrollBarVisible = false;
            this.dgvLog.Location = new System.Drawing.Point(6, 19);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.Size = new System.Drawing.Size(871, 270);
            this.dgvLog.TabIndex = 9;
            this.dgvLog.VerticalScrollBarVisible = false;
            // 
            // colFileName
            // 
            this.colFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colFileName.HeaderText = "CDLC File Name";
            this.colFileName.Name = "colFileName";
            this.colFileName.Width = 450;
            // 
            // colMessage
            // 
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            // 
            // btnViewErrorLog
            // 
            this.btnViewErrorLog.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnViewErrorLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnViewErrorLog.Image = global::CustomsForgeSongManager.Properties.Resources.notes;
            this.btnViewErrorLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewErrorLog.Location = new System.Drawing.Point(353, 304);
            this.btnViewErrorLog.Name = "btnViewErrorLog";
            this.btnViewErrorLog.Padding = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.btnViewErrorLog.Size = new System.Drawing.Size(182, 32);
            this.btnViewErrorLog.TabIndex = 10;
            this.btnViewErrorLog.Text = "     View Error Log";
            this.toolTip.SetToolTip(this.btnViewErrorLog, "Show \'remastered_error.log\' on the screen.");
            this.btnViewErrorLog.UseVisualStyleBackColor = true;
            this.btnViewErrorLog.Click += new System.EventHandler(this.btnViewErrorLog_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 12000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // Repairs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRepairs);
            this.Name = "Repairs";
            this.Size = new System.Drawing.Size(899, 490);
            this.tlpRepairs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.gbLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpRepairs;
        private System.Windows.Forms.Label lblBetaFeature;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemasterAllCDLC;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnCleanupDlcFolder;
        private System.Windows.Forms.Button btnViewErrorLog;
        private System.Windows.Forms.CheckBox chkRepairOrg;
        private System.Windows.Forms.CheckBox chkPreserve;
        private System.Windows.Forms.Button btnDeleteCorruptFiles;
        private System.Windows.Forms.Button btnArchiveCorruptCDLC;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.ToolTip toolTip;
        private DataGridViewTools.SubclassedDataGridView dgvLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;

   }
}
