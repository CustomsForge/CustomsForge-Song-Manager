namespace CustomsForgeManager.SongEditor
{
    partial class frmSongEditor
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
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.tslSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSaveAs = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslExit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpSongInfo = new System.Windows.Forms.TabPage();
            this.tpArrangements = new System.Windows.Forms.TabPage();
            this.tpTones = new System.Windows.Forms.TabPage();
            this.tpVersionInfo = new System.Windows.Forms.TabPage();
            this.tpAlbumArt = new System.Windows.Forms.TabPage();
            this.statusStripMain.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSave,
            this.tslSaveAs,
            this.tslExit});
            this.statusStripMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripMain.Location = new System.Drawing.Point(0, 459);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(673, 23);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 4;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // tslSave
            // 
            this.tslSave.AutoSize = false;
            this.tslSave.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslSave.IsLink = true;
            this.tslSave.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tslSave.Name = "tslSave";
            this.tslSave.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tslSave.Size = new System.Drawing.Size(68, 18);
            this.tslSave.Text = "Save";
            this.tslSave.Click += new System.EventHandler(this.tslSave_Click);
            // 
            // tslSaveAs
            // 
            this.tslSaveAs.AutoSize = false;
            this.tslSaveAs.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslSaveAs.IsLink = true;
            this.tslSaveAs.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tslSaveAs.Name = "tslSaveAs";
            this.tslSaveAs.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tslSaveAs.Size = new System.Drawing.Size(68, 18);
            this.tslSaveAs.Text = "Save As";
            this.tslSaveAs.Click += new System.EventHandler(this.tslSaveAs_Click);
            // 
            // tslExit
            // 
            this.tslExit.AutoSize = false;
            this.tslExit.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslExit.IsLink = true;
            this.tslExit.LinkColor = System.Drawing.SystemColors.ActiveCaption;
            this.tslExit.Name = "tslExit";
            this.tslExit.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tslExit.Size = new System.Drawing.Size(68, 18);
            this.tslExit.Text = "Exit";
            this.tslExit.Click += new System.EventHandler(this.tslExit_Click);
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpSongInfo);
            this.tcMain.Controls.Add(this.tpArrangements);
            this.tcMain.Controls.Add(this.tpTones);
            this.tcMain.Controls.Add(this.tpVersionInfo);
            this.tcMain.Controls.Add(this.tpAlbumArt);
            this.tcMain.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcMain.Location = new System.Drawing.Point(8, 12);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(653, 431);
            this.tcMain.TabIndex = 5;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            // 
            // tpSongInfo
            // 
            this.tpSongInfo.Location = new System.Drawing.Point(4, 25);
            this.tpSongInfo.Name = "tpSongInfo";
            this.tpSongInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpSongInfo.Size = new System.Drawing.Size(645, 402);
            this.tpSongInfo.TabIndex = 6;
            this.tpSongInfo.Text = "Song Info";
            this.tpSongInfo.UseVisualStyleBackColor = true;
            // 
            // tpArrangements
            // 
            this.tpArrangements.Location = new System.Drawing.Point(4, 25);
            this.tpArrangements.Name = "tpArrangements";
            this.tpArrangements.Size = new System.Drawing.Size(645, 402);
            this.tpArrangements.TabIndex = 4;
            this.tpArrangements.Text = "Arrangements";
            this.tpArrangements.UseVisualStyleBackColor = true;
            // 
            // tpTones
            // 
            this.tpTones.Location = new System.Drawing.Point(4, 25);
            this.tpTones.Name = "tpTones";
            this.tpTones.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.tpTones.Size = new System.Drawing.Size(645, 402);
            this.tpTones.TabIndex = 0;
            this.tpTones.Text = "Tones";
            this.tpTones.UseVisualStyleBackColor = true;
            // 
            // tpVersionInfo
            // 
            this.tpVersionInfo.Location = new System.Drawing.Point(4, 25);
            this.tpVersionInfo.Name = "tpVersionInfo";
            this.tpVersionInfo.Size = new System.Drawing.Size(645, 402);
            this.tpVersionInfo.TabIndex = 7;
            this.tpVersionInfo.Text = "Version Info";
            this.tpVersionInfo.UseVisualStyleBackColor = true;
            // 
            // tpAlbumArt
            // 
            this.tpAlbumArt.Location = new System.Drawing.Point(4, 25);
            this.tpAlbumArt.Name = "tpAlbumArt";
            this.tpAlbumArt.Size = new System.Drawing.Size(645, 402);
            this.tpAlbumArt.TabIndex = 5;
            this.tpAlbumArt.Text = "Album Art";
            this.tpAlbumArt.UseVisualStyleBackColor = true;
            // 
            // frmSongEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 482);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.tcMain);
            this.Name = "frmSongEditor";
            this.Text = "frmSongEditor";
            this.TopMost = true;
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpTones;
        private System.Windows.Forms.TabPage tpArrangements;
        private System.Windows.Forms.TabPage tpSongInfo;
        private System.Windows.Forms.TabPage tpVersionInfo;
        private System.Windows.Forms.TabPage tpAlbumArt;
        public System.Windows.Forms.ToolStripStatusLabel tslSave;
        public System.Windows.Forms.ToolStripStatusLabel tslSaveAs;
        public System.Windows.Forms.ToolStripStatusLabel tslExit;

    }
}