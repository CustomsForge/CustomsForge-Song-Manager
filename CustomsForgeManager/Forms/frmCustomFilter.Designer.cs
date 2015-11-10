namespace CustomsForgeManager.Forms
{
    partial class frmCustomFilter
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbExpression1 = new System.Windows.Forms.ComboBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.cbExpression2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cueTextBox1 = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.cueTextBox2 = new CustomsForgeManager.CustomsForgeManagerLib.CustomControls.CueTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show rows where:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cueTextBox2);
            this.groupBox1.Controls.Add(this.cueTextBox1);
            this.groupBox1.Controls.Add(this.cbExpression2);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.cbExpression1);
            this.groupBox1.Location = new System.Drawing.Point(12, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 118);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // cbExpression1
            // 
            this.cbExpression1.FormattingEnabled = true;
            this.cbExpression1.Location = new System.Drawing.Point(18, 28);
            this.cbExpression1.Name = "cbExpression1";
            this.cbExpression1.Size = new System.Drawing.Size(149, 21);
            this.cbExpression1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 55);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "And";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(68, 55);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(36, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Or";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // cbExpression2
            // 
            this.cbExpression2.FormattingEnabled = true;
            this.cbExpression2.Location = new System.Drawing.Point(18, 78);
            this.cbExpression2.Name = "cbExpression2";
            this.cbExpression2.Size = new System.Drawing.Size(149, 21);
            this.cbExpression2.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(224, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(312, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cueTextBox1
            // 
            this.cueTextBox1.Cue = "(Enter a value)";
            this.cueTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueTextBox1.ForeColor = System.Drawing.Color.Gray;
            this.cueTextBox1.Location = new System.Drawing.Point(198, 28);
            this.cueTextBox1.Name = "cueTextBox1";
            this.cueTextBox1.Size = new System.Drawing.Size(157, 20);
            this.cueTextBox1.TabIndex = 4;
            // 
            // cueTextBox2
            // 
            this.cueTextBox2.Cue = "(Enter a value)";
            this.cueTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cueTextBox2.ForeColor = System.Drawing.Color.Gray;
            this.cueTextBox2.Location = new System.Drawing.Point(198, 78);
            this.cueTextBox2.Name = "cueTextBox2";
            this.cueTextBox2.Size = new System.Drawing.Size(157, 20);
            this.cueTextBox2.TabIndex = 5;
            // 
            // frmCustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 203);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "frmCustomFilter";
            this.Text = " Custom Filter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private CustomsForgeManagerLib.CustomControls.CueTextBox cueTextBox2;
        private CustomsForgeManagerLib.CustomControls.CueTextBox cueTextBox1;
        private System.Windows.Forms.ComboBox cbExpression2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox cbExpression1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}