namespace CustomsForgeManager.SongEditor
{
    partial class ucAlbumArt
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
            this.picAlbumArt = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picAlbumArt)).BeginInit();
            this.SuspendLayout();
            // 
            // picAlbumArt
            // 
            this.picAlbumArt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picAlbumArt.Location = new System.Drawing.Point(0, 0);
            this.picAlbumArt.Name = "picAlbumArt";
            this.picAlbumArt.Size = new System.Drawing.Size(507, 411);
            this.picAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picAlbumArt.TabIndex = 0;
            this.picAlbumArt.TabStop = false;
            // 
            // ucAlbumArt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picAlbumArt);
            this.Name = "ucAlbumArt";
            this.Size = new System.Drawing.Size(507, 411);
            ((System.ComponentModel.ISupportInitialize)(this.picAlbumArt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picAlbumArt;
    }
}
