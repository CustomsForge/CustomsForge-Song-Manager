namespace CustomsForgeManager.Forms
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
            this.btnOpenAllOutdatedSongs = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutdatedSongs)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOutdatedSongs
            // 
            this.dgvOutdatedSongs.AllowUserToAddRows = false;
            this.dgvOutdatedSongs.AllowUserToDeleteRows = false;
            this.dgvOutdatedSongs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutdatedSongs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOutdatedSongs.Location = new System.Drawing.Point(12, 15);
            this.dgvOutdatedSongs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvOutdatedSongs.MultiSelect = false;
            this.dgvOutdatedSongs.Name = "dgvOutdatedSongs";
            this.dgvOutdatedSongs.ReadOnly = true;
            this.dgvOutdatedSongs.RowHeadersVisible = false;
            this.dgvOutdatedSongs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOutdatedSongs.Size = new System.Drawing.Size(848, 609);
            this.dgvOutdatedSongs.TabIndex = 0;
            // 
            // btnOpenSongInBrowser
            // 
            this.btnOpenSongInBrowser.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.btnOpenSongInBrowser.Location = new System.Drawing.Point(300, 631);
            this.btnOpenSongInBrowser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenSongInBrowser.Name = "btnOpenSongInBrowser";
            this.btnOpenSongInBrowser.Size = new System.Drawing.Size(134, 28);
            this.btnOpenSongInBrowser.TabIndex = 1;
            this.btnOpenSongInBrowser.Text = "Open song in browser";
            this.btnOpenSongInBrowser.UseVisualStyleBackColor = true;
            this.btnOpenSongInBrowser.Click += new System.EventHandler(this.btnOpenSongInBrowser_Click);
            // 
            // btnOpenAllOutdatedSongs
            // 
            this.btnOpenAllOutdatedSongs.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.btnOpenAllOutdatedSongs.Location = new System.Drawing.Point(440, 631);
            this.btnOpenAllOutdatedSongs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenAllOutdatedSongs.Name = "btnOpenAllOutdatedSongs";
            this.btnOpenAllOutdatedSongs.Size = new System.Drawing.Size(144, 28);
            this.btnOpenAllOutdatedSongs.TabIndex = 2;
            this.btnOpenAllOutdatedSongs.Text = "Open all songs in browser";
            this.btnOpenAllOutdatedSongs.UseVisualStyleBackColor = true;
            this.btnOpenAllOutdatedSongs.Click += new System.EventHandler(this.btnOpenAllOutdatedSongs_Click);
            // 
            // frmOutdatedSongs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 674);
            this.Controls.Add(this.btnOpenAllOutdatedSongs);
            this.Controls.Add(this.btnOpenSongInBrowser);
            this.Controls.Add(this.dgvOutdatedSongs);
            this.Font = new System.Drawing.Font("Trebuchet MS", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOutdatedSongs";
            this.Text = "Outdated Songs";
            this.Load += new System.EventHandler(this.frmOutdatedSongs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutdatedSongs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOutdatedSongs;
        private System.Windows.Forms.Button btnOpenSongInBrowser;
        private System.Windows.Forms.Button btnOpenAllOutdatedSongs;
    }
}