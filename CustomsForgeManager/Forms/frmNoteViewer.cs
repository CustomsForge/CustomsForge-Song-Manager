using System;
using System.Windows.Forms;

namespace CustomsForgeManager.Forms
{
    public partial class frmNoteViewer : Form
    {
        public frmNoteViewer()
        {
            InitializeComponent();
        }

        public void PopulateText(string notes2View)
        {
            if (String.IsNullOrEmpty(notes2View))
                rtbNotes.Text = @"Could not find any notes to view";
            else
                rtbNotes.Text = notes2View;

            rtbNotes.Select(0, 0);
        }

        public void RemoveButtonHandler()
        {
            btnCopyToClipboard.Click -= btnCopyToClipboard_Click;
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();

            if (rtbNotes.SelectionLength > 0)
                Clipboard.SetText(rtbNotes.SelectedText, TextDataFormat.Text);
            else
                Clipboard.SetText(rtbNotes.Text, TextDataFormat.Text);
        }


    }
}
