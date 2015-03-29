namespace CustomsForgeManager_Winforms.Forms
{
    partial class frmRenamer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRenamer));
            this.renameTemplateLabel = new System.Windows.Forms.Label();
            this.renameTemplateTextBox = new System.Windows.Forms.TextBox();
            this.renameAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // renameTemplateLabel
            // 
            this.renameTemplateLabel.AutoSize = true;
            this.renameTemplateLabel.Location = new System.Drawing.Point(4, 15);
            this.renameTemplateLabel.Name = "renameTemplateLabel";
            this.renameTemplateLabel.Size = new System.Drawing.Size(97, 13);
            this.renameTemplateLabel.TabIndex = 1;
            this.renameTemplateLabel.Text = "Rename Template:";
            // 
            // renameTemplateTextBox
            // 
            this.renameTemplateTextBox.Location = new System.Drawing.Point(107, 12);
            this.renameTemplateTextBox.Name = "renameTemplateTextBox";
            this.renameTemplateTextBox.Size = new System.Drawing.Size(431, 20);
            this.renameTemplateTextBox.TabIndex = 2;
            this.renameTemplateTextBox.Text = "./<title>_<artist>";
            // 
            // renameAllButton
            // 
            this.renameAllButton.Location = new System.Drawing.Point(544, 6);
            this.renameAllButton.Name = "renameAllButton";
            this.renameAllButton.Size = new System.Drawing.Size(121, 30);
            this.renameAllButton.TabIndex = 3;
            this.renameAllButton.Text = "Rename All";
            this.renameAllButton.UseVisualStyleBackColor = true;
            this.renameAllButton.Click += new System.EventHandler(this.renameAllButton_Click);
            // 
            // frmRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 270);
            this.Controls.Add(this.renameAllButton);
            this.Controls.Add(this.renameTemplateTextBox);
            this.Controls.Add(this.renameTemplateLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRenamer";
            this.Text = "zzz";
            this.Load += new System.EventHandler(this.frmRenamer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label renameTemplateLabel;
        private System.Windows.Forms.TextBox renameTemplateTextBox;
        private System.Windows.Forms.Button renameAllButton;
    }
}