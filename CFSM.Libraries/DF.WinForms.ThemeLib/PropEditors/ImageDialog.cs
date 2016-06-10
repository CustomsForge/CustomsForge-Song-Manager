using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DF.WinForms.ThemeLib.PropEditors
{
    public partial class ImageDialog : Form
    {
        public ImageDialog()
        {
            InitializeComponent();
        }

        public Image Image
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog od = new OpenFileDialog() {Filter = "Image Files|*.bmp;*.gif;*.jpeg;*.jpg;*.png"})
            {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(od.FileName);
                }
            }
        }
    }
}