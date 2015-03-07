namespace CustomsForgeManager_Winforms.Forms
{
    partial class frmOutdatedSongs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOutdatedSongs));
            this.dgvOutdatedSongs = new System.Windows.Forms.DataGridView();
            this.btnOpenSongInBrowser = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutdatedSongs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOutdatedSongs
            // 
            this.dgvOutdatedSongs.AllowUserToAddRows = false;
            this.dgvOutdatedSongs.AllowUserToDeleteRows = false;
            this.dgvOutdatedSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutdatedSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOutdatedSongs.Location = new System.Drawing.Point(12, 12);
            this.dgvOutdatedSongs.MultiSelect = false;
            this.dgvOutdatedSongs.Name = "dgvOutdatedSongs";
            this.dgvOutdatedSongs.ReadOnly = true;
            this.dgvOutdatedSongs.RowHeadersVisible = false;
            this.dgvOutdatedSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOutdatedSongs.Size = new System.Drawing.Size(544, 374);
            this.dgvOutdatedSongs.TabIndex = 0;
            // 
            // btnOpenSongInBrowser
            // 
            this.btnOpenSongInBrowser.Location = new System.Drawing.Point(215, 392);
            this.btnOpenSongInBrowser.Name = "btnOpenSongInBrowser";
            this.btnOpenSongInBrowser.Size = new System.Drawing.Size(134, 23);
            this.btnOpenSongInBrowser.TabIndex = 1;
            this.btnOpenSongInBrowser.Text = "Open song in browser";
            this.btnOpenSongInBrowser.UseVisualStyleBackColor = true;
            this.btnOpenSongInBrowser.Click += new System.EventHandler(this.btnOpenSongInBrowser_Click);
            // 
            // frmOutdatedSongs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 422);
            this.Controls.Add(this.btnOpenSongInBrowser);
            this.Controls.Add(this.dgvOutdatedSongs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmOutdatedSongs";
            this.Text = "Outdated Songs";
            this.Load += new System.EventHandler(this.frmOutdatedSongs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutdatedSongs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOutdatedSongs;
        private System.Windows.Forms.Button btnOpenSongInBrowser;
    }
}