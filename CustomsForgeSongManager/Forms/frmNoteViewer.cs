using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GenTools;
using System.Text;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmNoteViewer : Form
    {
        public frmNoteViewer()
        {
            InitializeComponent();

            // display Christmas stocking hat icon
            if (DateTimeExtensions.TisTheSeason())
            {
                this.BackgroundImage = CustomsForgeSongManager.Properties.Resources.hat;
                var bm = CustomsForgeSongManager.Properties.Resources.hat;
                this.Icon = Icon.FromHandle(bm.GetHicon());
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceHelpPath"></param>
        /// <param name="windowText">Custom Form Titlebar Message.</param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="windowText">Custom Form Titlebar Message.</param>
        /// <param name="startLine">Extract a section of text. Only applies to text files.</param>
        /// <param name="stopLine">Extract a section of text. Only applies to text files.</param>
        public static void ViewExternalFile(string filePath, string windowText = "", int startLine = 1, int stopLine = 0)
        {
            using (var noteViewer = new frmNoteViewer())
            {
                noteViewer.Text = String.Format("{0} . . . {1}", noteViewer.Text, windowText);

                if (filePath.EndsWith(".rtf")) // view rtf file
                {
                    if (startLine != 1 && stopLine != 0)
                        throw new Exception("<ERROR> Improper use of ViewExternalFile for an RTF file ...");

                    using (Stream stream = File.OpenRead(filePath))
                        noteViewer.PopulateRichText(stream);
                }
                else // view simple text file
                {
                    if (startLine == 1 && stopLine == 0)
                        noteViewer.PopulateText(File.ReadAllText(filePath));
                    else
                    {
                        // TODO: make this a generic external method
                        // extract a section from file by using line numbers
                        var sb = new StringBuilder();
                        var lines = File.ReadAllText(filePath).Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                       
                        if (startLine > lines.Length)
                            throw new Exception("<ERROR> Improper use of ViewExternalFile startLine > lines.Length ...");
                        
                        if (stopLine == 0 || stopLine > lines.Length)
                            stopLine = lines.Length;

                        for (int i = startLine -1 ; i < stopLine; i++)
                            sb.AppendLine(lines[i]);

                        noteViewer.PopulateText(sb.ToString());
                    }
                }

                noteViewer.ShowDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="windowText">Custom Form Titlebar Message.</param>
        public static void ViewExternalImageFile(string filePath, string windowText = "")
        {
            using (Form f = new Form())
            {
                f.Text = windowText;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowIcon = false;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.AutoSize = true;
                PictureBox pb = new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage, Dock = DockStyle.Fill, Image = new Bitmap(filePath) };
                f.Controls.Add(pb);
                f.ShowDialog();
            }
        }

    }
}