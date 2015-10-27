using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace CustomsForgeManager.Forms
{
    public partial class frmHtmlViewer : Form
    {
        public frmHtmlViewer()
        {
            InitializeComponent();
        }


        public void PopulateHtml(string notes2View)
        {
            try
            {
                if (String.IsNullOrEmpty(notes2View))
                    htmlPanel.Text = @"Could not find any notes to view";
                else
                {
                    htmlPanel.Text = notes2View;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Failed to render HTML");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();

            if (!String.IsNullOrEmpty(htmlPanel.SelectedHtml))
                Clipboard.SetText(htmlPanel.SelectedHtml, TextDataFormat.UnicodeText);
            else
                Clipboard.SetText(htmlPanel.Text, TextDataFormat.UnicodeText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sd = new SaveFileDialog() { Filter = "Html Files | *.html;*.htm", AddExtension = true, OverwritePrompt = true })
            {
                if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var FS = File.Create(sd.FileName);
                    using (var tw = new StreamWriter(FS))
                        tw.Write(htmlPanel.Text);
                }
            }
        }
    }
}
