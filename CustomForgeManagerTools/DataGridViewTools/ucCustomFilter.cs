using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataGridViewTools
{
    // TODO: use a generic UserControl

    public partial class ucCustomFilter : UserControl
    {
        public ucCustomFilter()
        {
            InitializeComponent();
        }

        public bool IsAnd
        {
            get
            {
                return radioButton1.Checked;
            }
        }

        Control oldEdit;

        private void cbExpression_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbExpression.SelectedItem != null)
            {
                if (oldEdit != null)
                {
                    Controls.Remove(oldEdit);
                }
                Expression x = (Expression)cbExpression.SelectedItem;
                if (x != null)
                {
                    var c = x.GetEditor();
                    if (c != null)
                    {
                        Controls.Add(c);
                        c.Location = new Point(198, 20);
                        c.Size = new Size(157, 20);
                        oldEdit = c;
                    }
                }
            }
        }


    }
}
