namespace CustomsForgeManager.UControls
{
    partial class Utilities
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Utilities));
            this.panel4 = new System.Windows.Forms.Panel();
            this.gb_Utilities = new System.Windows.Forms.GroupBox();
            this.btnUploadSong = new System.Windows.Forms.Button();
            this.btnRequestSong = new System.Windows.Forms.Button();
            this.btnLaunchSteam = new System.Windows.Forms.Button();
            this.btnBackupRSProfile = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.gb_Utilities.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gb_Utilities);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(990, 490);
            this.panel4.TabIndex = 1;
            // 
            // gb_Utilities
            // 
            this.gb_Utilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Utilities.Controls.Add(this.btnUploadSong);
            this.gb_Utilities.Controls.Add(this.btnRequestSong);
            this.gb_Utilities.Controls.Add(this.btnLaunchSteam);
            this.gb_Utilities.Controls.Add(this.btnBackupRSProfile);
            this.gb_Utilities.Location = new System.Drawing.Point(3, 3);
            this.gb_Utilities.Name = "gb_Utilities";
            this.gb_Utilities.Size = new System.Drawing.Size(984, 42);
            this.gb_Utilities.TabIndex = 6;
            this.gb_Utilities.TabStop = false;
            this.gb_Utilities.Text = "Utilities";
            // 
            // btnUploadSong
            // 
            this.btnUploadSong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUploadSong.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadSong.Image")));
            this.btnUploadSong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadSong.Location = new System.Drawing.Point(761, 12);
            this.btnUploadSong.Name = "btnUploadSong";
            this.btnUploadSong.Size = new System.Drawing.Size(100, 26);
            this.btnUploadSong.TabIndex = 4;
            this.btnUploadSong.Text = "Upload a Song";
            this.btnUploadSong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUploadSong.UseVisualStyleBackColor = true;
            this.btnUploadSong.Click += new System.EventHandler(this.btnUploadSong_Click);
            // 
            // btnRequestSong
            // 
            this.btnRequestSong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRequestSong.Image = ((System.Drawing.Image)(resources.GetObject("btnRequestSong.Image")));
            this.btnRequestSong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRequestSong.Location = new System.Drawing.Point(867, 12);
            this.btnRequestSong.Name = "btnRequestSong";
            this.btnRequestSong.Size = new System.Drawing.Size(110, 26);
            this.btnRequestSong.TabIndex = 4;
            this.btnRequestSong.Text = "Request a Song";
            this.btnRequestSong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRequestSong.UseVisualStyleBackColor = true;
            this.btnRequestSong.Click += new System.EventHandler(this.btnRequestSong_Click);
            // 
            // btnLaunchSteam
            // 
            this.btnLaunchSteam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunchSteam.Image = global::CustomsForgeManager.Properties.Resources.launch;
            this.btnLaunchSteam.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLaunchSteam.Location = new System.Drawing.Point(382, 12);
            this.btnLaunchSteam.Name = "btnLaunchSteam";
            this.btnLaunchSteam.Size = new System.Drawing.Size(175, 26);
            this.btnLaunchSteam.TabIndex = 4;
            this.btnLaunchSteam.Text = "Launch Rocksmith via Steam";
            this.btnLaunchSteam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaunchSteam.UseVisualStyleBackColor = true;
            this.btnLaunchSteam.Click += new System.EventHandler(this.btnLaunchSteam_Click);
            // 
            // btnBackupRSProfile
            // 
            this.btnBackupRSProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackupRSProfile.Image = global::CustomsForgeManager.Properties.Resources.backup;
            this.btnBackupRSProfile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackupRSProfile.Location = new System.Drawing.Point(563, 12);
            this.btnBackupRSProfile.Name = "btnBackupRSProfile";
            this.btnBackupRSProfile.Size = new System.Drawing.Size(160, 26);
            this.btnBackupRSProfile.TabIndex = 2;
            this.btnBackupRSProfile.Text = "Backup Rocksmith Profile";
            this.btnBackupRSProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBackupRSProfile.UseVisualStyleBackColor = true;
            this.btnBackupRSProfile.Click += new System.EventHandler(this.btnBackupRSProfile_Click);
            // 
            // Utilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel4);
            this.Name = "Utilities";
            this.Size = new System.Drawing.Size(990, 490);
            this.panel4.ResumeLayout(false);
            this.gb_Utilities.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox gb_Utilities;
        private System.Windows.Forms.Button btnUploadSong;
        private System.Windows.Forms.Button btnRequestSong;
        private System.Windows.Forms.Button btnLaunchSteam;
        private System.Windows.Forms.Button btnBackupRSProfile;
    }
}
