using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using UserProfileLib;
using CustomControls;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.Forms
{
    public partial class frmLocalProfiles : Form
    {
        private ToolTip tt = new ToolTip(); // prevents tt trails

        public frmLocalProfiles(string localProfilesPath)
        {
            InitializeComponent();
            tt.IsBalloon = true;
            tt.InitialDelay = 200;
            tt.ReshowDelay = 2000;
            LocalProfilesPath = localProfilesPath;
        }

        public string LocalProfilesPath { get; set; }
        public string PlayerName { get; set; }
        public string PrfldbPath { get; set; }

        private void LoadLocalProfilesGrid(string localProfilesPath)
        {
            var localProfiles = UserProfiles.ReadLocalProfiles(localProfilesPath);
            dgvLocalProfiles.AutoGenerateColumns = false;
            dgvLocalProfiles.DataSource = localProfiles;
        }

        private string SelectLocalProfiles()
        {
            string output;
            if (!Steam.IsSteamInstallationValid(out output))
            {
                var diaMsg = "CFSM did not find a valid Steam installation ..." + Environment.NewLine +
                             "Do you want to try the manual update method?" + Environment.NewLine +
                             "Did you manually shut down and exit Steam?" + Environment.NewLine + Environment.NewLine +
                             "Answer 'No' to leave Profile Song Lists.";

                if (DialogResult.Yes != BetterDialog2.ShowDialog(diaMsg, "Manual Update Mode ...", null, "Yes", "No", Bitmap.FromHicon(SystemIcons.Hand.Handle), "Warning", 0, 150))
                {
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                    // selects SongManager tabmenu even if tab order is changed
                    var tabIndex = Globals.MainForm.tcMain.TabPages.IndexOf(Globals.MainForm.tpSongManager);
                    Globals.MainForm.tcMain.SelectedIndex = tabIndex;
                    // show HelpManualSongLists
                    frmNoteViewer.ViewResourcesFile("CustomsForgeSongManager.Resources.HelpSongListsManual.rtf", "Profile Song Lists - Manual Mode");
                    return null;
                }
            }

            using (var ofd = new OpenFileDialog())
            {
                //if (Constants.DebugMode)
                //    LocalProfilesPath = "D:\\Temp"; // for dev testing

                ofd.Filter = "All Files (*.*)|*.*|Local Profiles File (LocalProfiles.json)|LocalProfiles.json";
                ofd.Title = "Select the Rocksmith 2014 Local Profiles file";
                ofd.FilterIndex = 2;
                ofd.InitialDirectory = LocalProfilesPath;
                ofd.CheckPathExists = true;
                ofd.Multiselect = false;

                if (ofd.ShowDialog() != DialogResult.OK)
                    return null;

                var fileName = ofd.FileName;

                return fileName;
            }
        }

        private void btnLocalProfiles_Click(object sender, EventArgs e)
        {
            var profilePath = SelectLocalProfiles();
            if (!String.IsNullOrEmpty(profilePath))
            {
                txtLocalProfiles.ForeColor = Color.Black;
                txtLocalProfiles.Text = profilePath;

                if (UserProfiles.IsLocalProfilesFile(profilePath))
                {
                    LocalProfilesPath = profilePath;
                    LoadLocalProfilesGrid(LocalProfilesPath);
                }
                else
                    profilePath = null;
            }

            if (String.IsNullOrEmpty(profilePath))
            {
                PrfldbPath = null;
                PlayerName = null;
                txtLocalProfiles.ForeColor = Color.Gray;
                txtLocalProfiles.Text = "Select the 'LocalProfiles.json' file from the folder:" + Environment.NewLine + ">Program Files (x86)/Steam/userdata/*/221860/remotecache";
            }
        }

        private void dgvLocalProfiles_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = (DataGridView)sender;
            var rowIndex = e.RowIndex;
            var colIndex = e.ColumnIndex;

            if (rowIndex == -1)
                return;

            var uniqueId = grid.Rows[rowIndex].Cells["colUniqueID"].Value.ToString();
            PlayerName = grid.Rows[rowIndex].Cells["colPlayerName"].Value.ToString();
            PrfldbPath = Path.Combine(Path.GetDirectoryName(LocalProfilesPath), uniqueId + "_prfldb");

            if (!File.Exists(PrfldbPath))
            {
                PrfldbPath = null;
                grid.Rows.Clear();
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvLocalProfiles_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // use CellMouseEnter event to trigger tooltip popups         
            var grid = (DataGridView)sender;
            var rowIndex = e.RowIndex;
            var colIndex = e.ColumnIndex;

            grid.ShowCellToolTips = false; // force tt to show
            tt.Hide(this);
            tt.ReshowDelay = 400;
            tt.AutoPopDelay = 5000; // default tooltip display
            tt.SetToolTip(grid, null); // reset tooltip

            if (rowIndex == -1)
                return;

            // show tooltip only every 5th row
            //if (rowIndex % 5 != 0 || rowIndex == 0) 
            //    return;

            if (colIndex == colPlayerName.Index)
            {
                var ttText = "Double Click a Player Name to" + Environment.NewLine +
                             "select a UniqueID (User Profile)";
                tt.SetToolTip(grid, ttText);
                // tt.Show(ttText, grid, grid.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y)));
            }
        }


    }
}
