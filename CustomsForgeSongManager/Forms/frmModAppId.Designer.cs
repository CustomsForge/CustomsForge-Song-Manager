using DF.WinForms.ThemeLib;

namespace CustomsForgeSongManager.Forms
{
    partial class frmModAppId
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModAppId));
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.cmbAppId = new System.Windows.Forms.ComboBox();
            this.lblAppID = new System.Windows.Forms.Label();
            this.themedProgressBar1 = new DF.WinForms.ThemeLib.ThemedProgressBar();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtAppId
            // 
            this.txtAppId.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtAppId.Location = new System.Drawing.Point(299, 24);
            this.txtAppId.MaxLength = 6;
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAppId.Size = new System.Drawing.Size(61, 20);
            this.txtAppId.TabIndex = 85;
            this.txtAppId.Text = "AppID";
            this.txtAppId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip.SetToolTip(this.txtAppId, "Hint:\r\nType in a valid AppID of\r\nany DLC that you own.");
            // 
            // cmbAppId
            // 
            this.cmbAppId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAppId.FormattingEnabled = true;
            this.cmbAppId.Location = new System.Drawing.Point(15, 24);
            this.cmbAppId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbAppId.Name = "cmbAppId";
            this.cmbAppId.Size = new System.Drawing.Size(279, 21);
            this.cmbAppId.TabIndex = 84;
            this.cmbAppId.SelectedIndexChanged += new System.EventHandler(this.cmbAppId_SelectedIndexChanged);
            // 
            // lblAppID
            // 
            this.lblAppID.AutoSize = true;
            this.lblAppID.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblAppID.Location = new System.Drawing.Point(12, 6);
            this.lblAppID.Margin = new System.Windows.Forms.Padding(3);
            this.lblAppID.Name = "lblAppID";
            this.lblAppID.Size = new System.Drawing.Size(40, 13);
            this.lblAppID.TabIndex = 83;
            this.lblAppID.Text = "AppID:";
            this.lblAppID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // themedProgressBar1
            // 
            this.themedProgressBar1.Location = new System.Drawing.Point(15, 57);
            this.themedProgressBar1.Name = "themedProgressBar1";
            this.themedProgressBar1.Size = new System.Drawing.Size(345, 23);
            this.themedProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.themedProgressBar1.TabIndex = 86;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(204, 97);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 87;
            this.btnApply.Text = global::CustomsForgeSongManager.Properties.Resources.Apply;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 88;
            this.btnCancel.Text = global::CustomsForgeSongManager.Properties.Resources.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.Location = new System.Drawing.Point(12, 97);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(171, 22);
            this.lblMsg.TabIndex = 89;
            this.lblMsg.Text = "Please wait ... Processing Files";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMsg.Visible = false;
            // 
            // frmModAppId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 128);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.themedProgressBar1);
            this.Controls.Add(this.txtAppId);
            this.Controls.Add(this.cmbAppId);
            this.Controls.Add(this.lblAppID);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmModAppId";
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
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ToolTip toolTip;
    }
}