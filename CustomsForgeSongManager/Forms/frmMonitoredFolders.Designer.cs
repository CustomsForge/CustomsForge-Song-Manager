namespace CustomsForgeSongManager.Forms
{
    partial class frmMonitoredFolders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonitoredFolders));
            this.lvMonitoredFolders = new System.Windows.Forms.ListView();
            this.Folders = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddNewMonitoredFolder = new System.Windows.Forms.Button();
            this.btnRemoveMonitoredFolder = new System.Windows.Forms.Button();
            this.btnCloseMonitored = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvMonitoredFolders
            // 
            this.lvMonitoredFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Folders});
            this.lvMonitoredFolders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvMonitoredFolders.Location = new System.Drawing.Point(12, 12);
            this.lvMonitoredFolders.Name = "lvMonitoredFolders";
            this.lvMonitoredFolders.Size = new System.Drawing.Size(424, 146);
            this.lvMonitoredFolders.TabIndex = 0;
            this.lvMonitoredFolders.UseCompatibleStateImageBehavior = false;
            this.lvMonitoredFolders.View = System.Windows.Forms.View.Details;
            // 
            // btnAddNewMonitoredFolder
            // 
            this.btnAddNewMonitoredFolder.Location = new System.Drawing.Point(12, 164);
            this.btnAddNewMonitoredFolder.Name = "btnAddNewMonitoredFolder";
            this.btnAddNewMonitoredFolder.Size = new System.Drawing.Size(75, 23);
            this.btnAddNewMonitoredFolder.TabIndex = 1;
            this.btnAddNewMonitoredFolder.Text = "Add";
            this.btnAddNewMonitoredFolder.UseVisualStyleBackColor = true;
            this.btnAddNewMonitoredFolder.Click += new System.EventHandler(this.btnAddNewMonitoredFolder_Click);
            // 
            // btnRemoveMonitoredFolder
            // 
            this.btnRemoveMonitoredFolder.Location = new System.Drawing.Point(190, 164);
            this.btnRemoveMonitoredFolder.Name = "btnRemoveMonitoredFolder";
            this.btnRemoveMonitoredFolder.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveMonitoredFolder.TabIndex = 2;
            this.btnRemoveMonitoredFolder.Text = "Remove";
            this.btnRemoveMonitoredFolder.UseVisualStyleBackColor = true;
            this.btnRemoveMonitoredFolder.Click += new System.EventHandler(this.btnRemoveMonitoredFolder_Click);
            // 
            // btnCloseMonitored
            // 
            this.btnCloseMonitored.Location = new System.Drawing.Point(361, 164);
            this.btnCloseMonitored.Name = "btnCloseMonitored";
            this.btnCloseMonitored.Size = new System.Drawing.Size(75, 23);
            this.btnCloseMonitored.TabIndex = 3;
            this.btnCloseMonitored.Text = "Close";
            this.btnCloseMonitored.UseVisualStyleBackColor = true;
            this.btnCloseMonitored.Click += new System.EventHandler(this.btnCloseMonitored_Click);
            // 
            // frmMonitoredFolders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 198);
            this.Controls.Add(this.btnCloseMonitored);
            this.Controls.Add(this.btnRemoveMonitoredFolder);
            this.Controls.Add(this.btnAddNewMonitoredFolder);
            this.Controls.Add(this.lvMonitoredFolders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMonitoredFolders";
            this.Text = "Monitored Folders";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvMonitoredFolders;
        private System.Windows.Forms.Button btnAddNewMonitoredFolder;
        private System.Windows.Forms.Button btnRemoveMonitoredFolder;
        private System.Windows.Forms.Button btnCloseMonitored;
        private System.Windows.Forms.ColumnHeader Folders;

    }
}