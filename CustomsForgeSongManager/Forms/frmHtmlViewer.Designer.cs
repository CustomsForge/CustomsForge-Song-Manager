using CustomsForgeSongManager.LocalTools;

namespace CustomsForgeSongManager.Forms
{
    partial class frmHtmlViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHtmlViewer));
            this.pnlHtmlOwner = new System.Windows.Forms.Panel();
            this.htmlPanel = new CFSMHtmlPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlHtmlOwner.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHtmlOwner
            // 
            this.pnlHtmlOwner.Controls.Add(this.htmlPanel);
            this.pnlHtmlOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHtmlOwner.Location = new System.Drawing.Point(0, 0);
            this.pnlHtmlOwner.Name = "pnlHtmlOwner";
            this.pnlHtmlOwner.Size = new System.Drawing.Size(784, 515);
            this.pnlHtmlOwner.TabIndex = 0;
            // 
            // htmlPanel
            // 
            this.htmlPanel.AutoScroll = true;
            this.htmlPanel.AutoScrollMinSize = new System.Drawing.Size(784, 20);
            this.htmlPanel.BackColor = System.Drawing.SystemColors.Window;
            this.htmlPanel.BaseStylesheet = null;
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlPanel.Location = new System.Drawing.Point(0, 0);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(784, 515);
            this.htmlPanel.TabIndex = 0;
            this.htmlPanel.Text = "cfmHtmlPanel1";
            this.htmlPanel.UseSystemCursors = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 515);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 46);
            this.panel2.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.Location = new System.Drawing.Point(403, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(284, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Copy To Clipboard";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmHtmlViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.pnlHtmlOwner);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmHtmlViewer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CFSM HTML Viewer";
            this.pnlHtmlOwner.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHtmlOwner;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private CFSMHtmlPanel htmlPanel;
    }
}