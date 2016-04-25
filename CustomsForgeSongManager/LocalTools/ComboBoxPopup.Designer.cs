namespace CustomsForgeSongManager.LocalTools
{
    partial class frmComboBoxPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComboBoxPopup));
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox
            // 
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(37, 25);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(264, 21);
            this.comboBox.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(136, 66);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(46, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 13);
            this.lblTitle.TabIndex = 2;
            // 
            // frmComboBoxPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 101);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.comboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmComboBoxPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblTitle;
    }
}