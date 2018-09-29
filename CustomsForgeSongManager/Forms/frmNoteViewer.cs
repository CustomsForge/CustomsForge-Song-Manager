using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmNoteViewer : Form
    {
        public frmNoteViewer()
        {
            InitializeComponent();
        }

        public void PopulateText(string notes2View, bool wordWrap = true)
        {
            if (String.IsNullOrEmpty(notes2View))
                rtbText.Text = @"Could not find any notes to view";
            else
                rtbText.Text = notes2View;

            rtbText.WordWrap = wordWrap;
            rtbText.Select(0, 0);
        }

        private void PopulateRichText(Stream streamRtfNotes = null)
        {
            if (streamRtfNotes == null)
            {
                this.Size = new Size(550, 200);
                rtbText.Text = "Additional help will be displayed here when available.";
            }
            else
            {
                this.Size = new Size(800, 640);
                rtbText.LoadFile(streamRtfNotes, RichTextBoxStreamType.RichText);
            }
        }
      
        public void PopulateAsciiText(string textNotes = "")
        {
            if (String.IsNullOrEmpty(textNotes))
            {
                this.Size = new Size(550, 132);
                rtbText.Text = "Additional help will be displayed here when available.";
            }
            else
            {
                rtbText.Text = textNotes;
            }
        }

        private void RemoveButtonHandler()
        {
            btnCopyToClipboard.Click -= btnCopyToClipboard_Click;
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();

            if (rtbText.SelectionLength > 0)
                Clipboard.SetText(rtbText.SelectedText, TextDataFormat.Text);
            else
                Clipboard.SetText(rtbText.Text, TextDataFormat.Text);
        }


        public static void ViewResourcesFile(string resourceHelpPath = "CustomsForgeSongManager.Resources.HelpGeneral.rtf", string windowText = "Default")
        {
            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, windowText);
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(resourceHelpPath))
                {
                    if (resourceHelpPath.EndsWith(".rtf")) // view rtf notes
                    {
                        noteViewer.PopulateRichText(stream);
                    }
                    else // view simple text notes
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var helpGeneral = reader.ReadToEnd();
                            noteViewer.PopulateText(helpGeneral);
                        }
                    }

                    noteViewer.ShowDialog();
                }
            }
        }

        public static void ViewExternalFile(string filePath, string windowText = "")
        {
            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, windowText);

                if (filePath.EndsWith(".rtf")) // view rtf file
                {
                    using (Stream stream = File.OpenRead(filePath))
                        noteViewer.PopulateRichText(stream);
                }
                else // view simple text file
                    noteViewer.PopulateText(File.ReadAllText(filePath));

                noteViewer.ShowDialog();
            }
        }




    }
}