using DF.WinForms.ThemeLib;

namespace CustomsForgeSongManager.Forms
{
    partial class frmSongBatchEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSongBatchEdit));
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.cmbAppId = new System.Windows.Forms.ComboBox();
            this.lblAppID = new System.Windows.Forms.Label();
            this.themedProgressBar1 = new DF.WinForms.ThemeLib.ThemedProgressBar();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtAppId
            // 
            this.txtAppId.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtAppId.Location = new System.Drawing.Point(247, 38);
            this.txtAppId.MaxLength = 6;
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.ReadOnly = true;
            this.txtAppId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAppId.Size = new System.Drawing.Size(61, 20);
            this.txtAppId.TabIndex = 85;
            this.txtAppId.Text = "AppID";
            this.txtAppId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbAppId
            // 
            this.cmbAppId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppId.FormattingEnabled = true;
            this.cmbAppId.Location = new System.Drawing.Point(14, 37);
            this.cmbAppId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbAppId.Name = "cmbAppId";
            this.cmbAppId.Size = new System.Drawing.Size(228, 21);
            this.cmbAppId.TabIndex = 84;
            this.cmbAppId.SelectedIndexChanged += new System.EventHandler(this.cmbAppId_SelectedIndexChanged);
            // 
            // lblAppID
            // 
            this.lblAppID.AutoSize = true;
            this.lblAppID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblAppID.Location = new System.Drawing.Point(11, 15);
            this.lblAppID.Margin = new System.Windows.Forms.Padding(3);
            this.lblAppID.Name = "lblAppID";
            this.lblAppID.Size = new System.Drawing.Size(40, 13);
            this.lblAppID.TabIndex = 83;
            this.lblAppID.Text = "AppID:";
            this.lblAppID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // themedProgressBar1
            // 
            this.themedProgressBar1.Location = new System.Drawing.Point(14, 70);
            this.themedProgressBar1.Name = "themedProgressBar1";
            this.themedProgressBar1.Size = new System.Drawing.Size(294, 23);
            this.themedProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.themedProgressBar1.TabIndex = 86;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(152, 117);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 87;
            this.btnApply.Text = global::CustomsForgeSongManager.Properties.Resources.Apply;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(233, 117);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 88;
            this.btnCancel.Text = global::CustomsForgeSongManager.Properties.Resources.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmSongBatchEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 154);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.themedProgressBar1);
            this.Controls.Add(this.txtAppId);
            this.Controls.Add(this.cmbAppId);
            this.Controls.Add(this.lblAppID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSongBatchEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song Batch Editor";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAppId;
        private System.Windows.Forms.ComboBox cmbAppId;
        private System.Windows.Forms.Label lblAppID;
        private ThemedProgressBar themedProgressBar1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
    }
}