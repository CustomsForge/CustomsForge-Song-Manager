using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Controls
{
    public partial class frmComboBoxPopup : Form
    {
        public frmComboBoxPopup()
        {
            InitializeComponent();
        }

        public ComboBox Combo
        {
           get { return comboBox; }
        }

        public ComboBox.ObjectCollection ComboBoxItems
        {
            set { comboBox.DataSource = value; }
            get { return comboBox.Items; }
        }

        public string LblText
        {
            set { lblTitle.Text = value; }
        }

        public string BtnText
        {
            set { btnOK.Text = value; }
        }

        public string FrmText
        {
            set { this.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
