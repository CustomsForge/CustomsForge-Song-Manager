namespace CustomControls
{
    partial class FormUserInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserInput));
            this.lblCustomInput = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtCustomInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblCustomInput
            // 
            this.lblCustomInput.AutoSize = true;
            this.lblCustomInput.Location = new System.Drawing.Point(8, 7);
            this.lblCustomInput.Name = "lblCustomInput";
            this.lblCustomInput.Size = new System.Drawing.Size(72, 13);
            this.lblCustomInput.TabIndex = 0;
            this.lblCustomInput.Text = "Custom Input:";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.Location = new System.Drawing.Point(138, 53);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtCustomInput
            // 
            this.txtCustomInput.Location = new System.Drawing.Point(10, 25);
            this.txtCustomInput.Name = "txtCustomInput";
            this.txtCustomInput.Size = new System.Drawing.Size(329, 20);
            this.txtCustomInput.TabIndex = 2;
            this.txtCustomInput.TextChanged += new System.EventHandler(this.txtCustomInput_TextChanged);
            this.txtCustomInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCustomInput_KeyDown);
            // 
            // FormUserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(349, 84);
            this.Controls.Add(this.txtCustomInput);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblCustomInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormUserInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom User Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCustomInput;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtCustomInput;
    }
}