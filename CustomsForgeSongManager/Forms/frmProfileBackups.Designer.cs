namespace CustomsForgeSongManager.Forms
{
    partial class frmProfileBackups
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfileBackups));
            this.tlpProfileBackups = new System.Windows.Forms.TableLayoutPanel();
            this.panelBackupedProfiles = new System.Windows.Forms.Panel();
            this.dgvProfileBackups = new System.Windows.Forms.DataGridView();
            this.colProfileSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colProfileDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.panelProfileBackups2 = new System.Windows.Forms.Panel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnDeleteBackup = new System.Windows.Forms.Button();
            this.tlpProfileBackups.SuspendLayout();
            this.panelBackupedProfiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileBackups)).BeginInit();
            this.panelProfileBackups2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProfileBackups
            // 
            this.tlpProfileBackups.ColumnCount = 1;
            this.tlpProfileBackups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProfileBackups.Controls.Add(this.panelBackupedProfiles, 0, 0);
            this.tlpProfileBackups.Controls.Add(this.panelProfileBackups2, 0, 1);
            this.tlpProfileBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProfileBackups.Location = new System.Drawing.Point(0, 0);
            this.tlpProfileBackups.Name = "tlpProfileBackups";
            this.tlpProfileBackups.RowCount = 2;
            this.tlpProfileBackups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tlpProfileBackups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpProfileBackups.Size = new System.Drawing.Size(581, 488);
            this.tlpProfileBackups.TabIndex = 0;
            // 
            // panelBackupedProfiles
            // 
            this.panelBackupedProfiles.Controls.Add(this.dgvProfileBackups);
            this.panelBackupedProfiles.Controls.Add(this.button1);
            this.panelBackupedProfiles.Location = new System.Drawing.Point(3, 3);
            this.panelBackupedProfiles.Name = "panelBackupedProfiles";
            this.panelBackupedProfiles.Size = new System.Drawing.Size(575, 433);
            this.panelBackupedProfiles.TabIndex = 1;
            // 
            // dgvProfileBackups
            // 
            this.dgvProfileBackups.AllowUserToAddRows = false;
            this.dgvProfileBackups.AllowUserToDeleteRows = false;
            this.dgvProfileBackups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProfileBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProfileBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProfileSelect,
            this.colProfileDate,
            this.colProfilePath});
            this.dgvProfileBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProfileBackups.Location = new System.Drawing.Point(0, 0);
            this.dgvProfileBackups.MultiSelect = false;
            this.dgvProfileBackups.Name = "dgvProfileBackups";
            this.dgvProfileBackups.ReadOnly = true;
            this.dgvProfileBackups.RowHeadersVisible = false;
            this.dgvProfileBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileBackups.Size = new System.Drawing.Size(575, 433);
            this.dgvProfileBackups.TabIndex = 0;
            // 
            // colProfileSelect
            // 
            this.colProfileSelect.HeaderText = "Select";
            this.colProfileSelect.Name = "colProfileSelect";
            this.colProfileSelect.ReadOnly = true;
            // 
            // colProfileDate
            // 
            this.colProfileDate.HeaderText = "Date";
            this.colProfileDate.Name = "colProfileDate";
            this.colProfileDate.ReadOnly = true;
            this.colProfileDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colProfileDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colProfilePath
            // 
            this.colProfilePath.HeaderText = "Path";
            this.colProfilePath.Name = "colProfilePath";
            this.colProfilePath.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panelProfileBackups2
            // 
            this.panelProfileBackups2.Controls.Add(this.btnRestoreBackup);
            this.panelProfileBackups2.Controls.Add(this.btnDeleteBackup);
            this.panelProfileBackups2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProfileBackups2.Location = new System.Drawing.Point(3, 442);
            this.panelProfileBackups2.Name = "panelProfileBackups2";
            this.panelProfileBackups2.Size = new System.Drawing.Size(575, 43);
            this.panelProfileBackups2.TabIndex = 2;
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Location = new System.Drawing.Point(109, 11);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(144, 23);
            this.btnRestoreBackup.TabIndex = 1;
            this.btnRestoreBackup.Text = "Restore selected backup";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnDeleteBackup
            // 
            this.btnDeleteBackup.Location = new System.Drawing.Point(298, 11);
            this.btnDeleteBackup.Name = "btnDeleteBackup";
            this.btnDeleteBackup.Size = new System.Drawing.Size(144, 23);
            this.btnDeleteBackup.TabIndex = 0;
            this.btnDeleteBackup.Text = "Delete selected backup";
            this.btnDeleteBackup.UseVisualStyleBackColor = true;
            this.btnDeleteBackup.Click += new System.EventHandler(this.btnDeleteBackup_Click);
            // 
            // frmProfileBackups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 488);
            this.Controls.Add(this.tlpProfileBackups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProfileBackups";
            this.Text = "Profile Backups";
            this.tlpProfileBackups.ResumeLayout(false);
            this.panelBackupedProfiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileBackups)).EndInit();
            this.panelProfileBackups2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProfileBackups;
        private System.Windows.Forms.Panel panelBackupedProfiles;
        private System.Windows.Forms.DataGridView dgvProfileBackups;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colProfileSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfileDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfilePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelProfileBackups2;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnDeleteBackup;

    }
}