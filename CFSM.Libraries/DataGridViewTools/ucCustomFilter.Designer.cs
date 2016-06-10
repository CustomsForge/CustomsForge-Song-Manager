using System.Windows.Forms;

namespace DataGridViewTools
{
    partial class ucCustomFilter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.cbExpression = new System.Windows.Forms.ComboBox();
            this.cbNot = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.cbExpression);
            this.groupBox1.Controls.Add(this.cbNot);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "[FILTER]";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(68, 73);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(36, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Or";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // cbExpression
            // 
            this.cbExpression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExpression.FormattingEnabled = true;
            this.cbExpression.Location = new System.Drawing.Point(18, 45);
            this.cbExpression.Name = "cbExpression";
            this.cbExpression.Size = new System.Drawing.Size(221, 21);
            this.cbExpression.TabIndex = 0;
            // 
            // cbNot
            // 
            this.cbNot.AutoSize = true;
            this.cbNot.Location = new System.Drawing.Point(18, 22);
            this.cbNot.Name = "cbNot";
            this.cbNot.Size = new System.Drawing.Size(71, 17);
            this.cbNot.TabIndex = 2;
            this.cbNot.Text = "Does Not";
            this.cbNot.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 73);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "And";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // ucCustomFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucCustomFilter";
            this.Size = new System.Drawing.Size(264, 104);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        public ComboBox cbExpression;
        public CheckBox cbNot;


    }
}
