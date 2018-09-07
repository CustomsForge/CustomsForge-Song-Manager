using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace CustomControls
{
    public partial class FormUserInput : Form
    {
        public bool IsProfileId = false;

        public FormUserInput(bool validateInput = false)
        {
            InitializeComponent();
            if (validateInput)
            {
                txtCustomInput.TextChanged += CustomInputText_TextChanged;
                txtCustomInput.KeyDown += CustomInputText_KeyDown;
            }
        }

        public string BtnText
        {
            set { btnOk.Text = value; }
        }

        public string CustomInputLabel
        {
            set { lblCustomInput.Text = value; }
        }

        public string CustomInputText
        {
            get { return txtCustomInput.Text; }
            set { txtCustomInput.Text = value; }
        }

        public string FrmHeaderText
        {
            set { this.Text = value; }
        }

        private void CustomInputText_KeyDown(object sender, KeyEventArgs e)
        {
            // modify this as needed
            if (e.KeyCode == Keys.Delete)
                txtCustomInput.Text = "";
            if ((e.KeyCode == Keys.Back) || (e.KeyCode == Keys.Delete))
                return;

            if (e.KeyData == (Keys.Control | Keys.V))
                (sender as TextBox).Paste();

            if (e.KeyCode > Keys.F && e.KeyCode <= Keys.Z)
                e.SuppressKeyPress = true;

            if (IsProfileId)
            {
                // must be >= to work properly
                if (txtCustomInput.Text.Length >= 11)
                    e.SuppressKeyPress = true;
                // validate entry as it is typed for nice action
                CustomInputText = ValidateProfileId(txtCustomInput.Text.ToUpper());
            }
        }

        private void CustomInputText_TextChanged(object sender, EventArgs e)
        {
            CustomInputText = txtCustomInput.Text;
            // move cursor to end
            txtCustomInput.SelectionStart = txtCustomInput.Text.Length;
        }

        private void FormUserInput_Load(object sender, EventArgs e)
        {
            // autoadjust the width of the UserInput form
            Font f = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular);
            Bitmap b = new Bitmap(2200, 2200);
            Graphics g = Graphics.FromImage(b);
            SizeF sizeOfString = new SizeF();
            sizeOfString = g.MeasureString(lblCustomInput.Text, f);
            this.Width = (int)sizeOfString.Width;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        #region Class Methods

        public static string ValidateProfileId(string userInput)
        {
            string okOutput = String.Empty;
            if (!String.IsNullOrEmpty(userInput))
            {
                userInput = userInput.Replace(" ", ""); // remove spaces
                string[] hexParts = userInput.Split('-');

                Regex rgx = new Regex("[^a-zA-Z0-9\\-]");
                foreach (string part in hexParts)
                {
                    okOutput = okOutput + rgx.Replace(part, "");
                    if (okOutput.Length == 2) okOutput = okOutput + "-";
                    if (okOutput.Length == 5) okOutput = okOutput + "-";
                    if (okOutput.Length == 8) okOutput = okOutput + "-";
                }
            }
            return okOutput;
        }

        #endregion
    }
}
