namespace CustomControls
{
    partial class ColorComboBox
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
            this.button = new ColorComboBox.ColorComboButton();
            this.SuspendLayout();
            // 
            // button
            // 
            this.button.Appearance = System.Windows.Forms.Appearance.Button;
            this.button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button.Extended = false;
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Name = "button";
            this.button.SelectedColor = System.Drawing.Color.Black;
            this.button.Size = new System.Drawing.Size(103, 23);
            this.button.TabIndex = 0;
            // 
            // ColorComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button);
            this.Name = "ColorComboBox";
            this.Size = new System.Drawing.Size(103, 23);
            this.SizeChanged += new System.EventHandler(this.ColorComboBox_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private ColorComboButton button;
    }
}
