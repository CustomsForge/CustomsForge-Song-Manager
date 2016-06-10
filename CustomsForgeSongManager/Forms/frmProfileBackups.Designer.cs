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
            this.button1 = new System.Windows.Forms.Button();
            this.panelProfileBackups2 = new System.Windows.Forms.Panel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnDeleteBackup = new System.Windows.Forms.Button();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.dgvProfileBackups.ColumnHeadersHeight = 22;
            this.dgvProfileBackups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colDate,
            this.colName,
            this.colPath});
            this.dgvProfileBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProfileBackups.Location = new System.Drawing.Point(0, 0);
            this.dgvProfileBackups.Name = "dgvProfileBackups";
            this.dgvProfileBackups.RowHeadersVisible = false;
            this.dgvProfileBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileBackups.Size = new System.Drawing.Size(575, 433);
            this.dgvProfileBackups.TabIndex = 0;
            this.dgvProfileBackups.SelectionChanged += new System.EventHandler(this.dgvProfileBackups_SelectionChanged);
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
            // colSelect
            // 
            this.colSelect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colSelect.FalseValue = "false";
            this.colSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.colSelect.HeaderText = "Select";
            this.colSelect.IndeterminateValue = "false";
            this.colSelect.Name = "colSelect";
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 43;
            // 
            // colDate
            // 
            this.colDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDate.Width = 36;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.Width = 60;
            // 
            // colPath
            // 
            this.colPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 500;
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelProfileBackups2;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnDeleteBackup;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;

    }
}