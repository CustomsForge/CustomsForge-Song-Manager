namespace CustomsForgeManager.Forms
{
    partial class frmNoteViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNoteViewer));
            this.rtbNotes = new System.Windows.Forms.RichTextBox();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbNotes
            // 
            this.rtbNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbNotes.Location = new System.Drawing.Point(0, 0);
            this.rtbNotes.Name = "rtbNotes";
            this.rtbNotes.Size = new System.Drawing.Size(557, 394);
            this.rtbNotes.TabIndex = 1;
            this.rtbNotes.Text = "This is a generic Rich Textbox Note Viewer";
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCopyToClipboard.Location = new System.Drawing.Point(221, 411);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(115, 23);
            this.btnCopyToClipboard.TabIndex = 2;
            this.btnCopyToClipboard.Text = "Copy to Clipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // frmNoteViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(557, 449);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.rtbNotes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNoteViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CFSM Note Viewer";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox rtbNotes;
        public System.Windows.Forms.Button btnCopyToClipboard;


    }
}