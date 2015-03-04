using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Forms
{
    public partial class frmSongListExport : Form
    {
        public frmSongListExport()
        {
            InitializeComponent();
        }

        public string SongList
        {
            set { txtSongList.Text = value; }
        }

        public string FormTitle
        {
            set { this.Text = value; }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSongList.Text);
        }
    }
}
