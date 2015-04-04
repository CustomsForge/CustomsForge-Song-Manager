using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antlr4.StringTemplate;
using CustomsForgeManager_Winforms.Controls;
using CustomsForgeManager_Winforms.Utilities;

namespace CustomsForgeManager_Winforms.Forms
{

    //GUI class for renamer program.
    //TODO: currently contains tons of logic/functions that should be refactored into another class.
    public partial class frmRenamer : Form
    {
        private Logging.Log myLog;
        private AbortableBackgroundWorker renameWorker = new AbortableBackgroundWorker();
        private List<SongData> songList;
        private Settings settings;

        public frmRenamer(Logging.Log myLog, Settings settings, List<SongData> songList)
        {
            // TODO: Complete member initialization
            this.myLog = myLog;
            this.songList = songList;
            this.settings = settings;
            InitializeComponent();
            myLog.Write("Renamer opened");
        }


        //TODO: Refactor this somewhere else?
        private void renameSongs(List<SongData> songList, String templateString)
        {
            try
            {
                foreach (SongData data in songList)
                {
                    myLog.Write(data.FileName);
                    Template template = new Template(templateString);
                    template.Add("title", data.Song);
                    template.Add("artist", data.Artist);
                    template.Add("version", data.Version);
                    template.Add("author", data.Author);
                    template.Add("album", data.Album);
                    template.Add("dd", data.DD);
                    template.Add("author", data.Updated);
                    String renameFilePath = settings.RSInstalledDir + "\\dlc\\" + template.Render() + "_p.psarc";
                    System.IO.File.Move(settings.RSInstalledDir + "\\dlc\\" + data.FileName, renameFilePath);
                }
            }
            //lazy exception catch for now.
            catch (Exception e)
            {
                myLog.Write(e.Message);
            }
        }

        private void doRenameSongs(object sender, DoWorkEventArgs e)
        {
            if (!renameWorker.CancellationPending)
            {
                renameSongs(songList, renameTemplateTextBox.Text);
            }
        }



        private void renameAllButton_Click(object sender, EventArgs e)
        {
            if (!renameTemplateTextBox.Text.Contains("<title>"))
            {
                MessageBox.Show("Rename Template requires <title> atleast once to prevent overwriting songs.");
            }
            else
            {
                if (!renameTemplateTextBox.Text.Contains("<title>"))
                {
                    MessageBox.Show("Rename Template requires <title> atleast once to prevent overwriting songs.");
                }
                else
                {
                    renameWorker.DoWork += doRenameSongs;
                    renameWorker.RunWorkerAsync();
                }
            }
        }

        private void frmRenamer_Load(object sender, EventArgs e)
        {

        }

        private void songPropertiesGroupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}