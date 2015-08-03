using System;
using System.Windows.Forms;

namespace CustomsForgeManager.CustomsForgeManagerLib.CustomControls
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
