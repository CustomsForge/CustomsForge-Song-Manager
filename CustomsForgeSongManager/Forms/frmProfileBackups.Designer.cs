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
            this.panelProfileBackups2 = new System.Windows.Forms.Panel();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnDeleteBackup = new System.Windows.Forms.Button();
            this.dgvProfileBackups = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpProfileBackups.SuspendLayout();
            this.panelProfileBackups2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileBackups)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpProfileBackups
            // 
            this.tlpProfileBackups.ColumnCount = 1;
            this.tlpProfileBackups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProfileBackups.Controls.Add(this.panelProfileBackups2, 0, 1);
            this.tlpProfileBackups.Controls.Add(this.dgvProfileBackups, 0, 0);
            this.tlpProfileBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProfileBackups.Location = new System.Drawing.Point(0, 0);
            this.tlpProfileBackups.Name = "tlpProfileBackups";
            this.tlpProfileBackups.RowCount = 2;
            this.tlpProfileBackups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProfileBackups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpProfileBackups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpProfileBackups.Size = new System.Drawing.Size(489, 271);
            this.tlpProfileBackups.TabIndex = 0;
            // 
            // panelProfileBackups2
            // 
            this.panelProfileBackups2.Controls.Add(this.btnRestoreBackup);
            this.panelProfileBackups2.Controls.Add(this.btnDeleteBackup);
            this.panelProfileBackups2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProfileBackups2.Location = new System.Drawing.Point(3, 234);
            this.panelProfileBackups2.Name = "panelProfileBackups2";
            this.panelProfileBackups2.Size = new System.Drawing.Size(483, 34);
            this.panelProfileBackups2.TabIndex = 2;
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRestoreBackup.Location = new System.Drawing.Point(63, 4);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(144, 23);
            this.btnRestoreBackup.TabIndex = 1;
            this.btnRestoreBackup.Text = "Restore Selected Backup";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnDeleteBackup
            // 
            this.btnDeleteBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteBackup.Location = new System.Drawing.Point(271, 4);
            this.btnDeleteBackup.Name = "btnDeleteBackup";
            this.btnDeleteBackup.Size = new System.Drawing.Size(144, 23);
            this.btnDeleteBackup.TabIndex = 0;
            this.btnDeleteBackup.Text = "Delete Selected Backup";
            this.btnDeleteBackup.UseVisualStyleBackColor = true;
            this.btnDeleteBackup.Click += new System.EventHandler(this.btnDeleteBackup_Click);
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
            this.dgvProfileBackups.Location = new System.Drawing.Point(3, 3);
            this.dgvProfileBackups.Name = "dgvProfileBackups";
            this.dgvProfileBackups.RowHeadersVisible = false;
            this.dgvProfileBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProfileBackups.Size = new System.Drawing.Size(483, 225);
            this.dgvProfileBackups.TabIndex = 3;
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
            this.colPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPath.HeaderText = "Path";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 54;
            // 
            // frmProfileBackups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 271);
            this.Controls.Add(this.tlpProfileBackups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProfileBackups";
            this.Text = "Profile Backups";
            this.tlpProfileBackups.ResumeLayout(false);
            this.panelProfileBackups2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProfileBackups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProfileBackups;
        private System.Windows.Forms.Panel panelProfileBackups2;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnDeleteBackup;
        private System.Windows.Forms.DataGridView dgvProfileBackups;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;

    }
}