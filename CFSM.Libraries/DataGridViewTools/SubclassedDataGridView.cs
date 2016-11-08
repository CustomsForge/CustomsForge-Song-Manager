using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataGridViewTools
{
    public partial class SubclassedDataGridView : DataGridView
    {
        public SubclassedDataGridView()
        {
            // VerticalScrollBar.Visible = true;
            VerticalScrollBar.VisibleChanged += VerticalScrollBar_VisibleChanged;

            // HorizontalScrollBar.Visible = true;
            HorizontalScrollBar.VisibleChanged += HorizontalScrollBar_VisibleChanged;
        }

        void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            if (VerticalScrollBar.Visible)
                return;

            int width = VerticalScrollBar.Width;

            VerticalScrollBar.Location = new Point(ClientRectangle.Width - width - 2, 21);
            VerticalScrollBar.Size = new Size(width, ClientRectangle.Height - 21 - 2);

            VerticalScrollBar.Show();
        }

        void HorizontalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            if (HorizontalScrollBar.Visible)
                return;

            int height = HorizontalScrollBar.Height;

            HorizontalScrollBar.Location = new Point(2, ClientRectangle.Height - 18);
            HorizontalScrollBar.Size = new Size(ClientRectangle.Width - 21, height);

            HorizontalScrollBar.Show();
        }

        // added to the control property choices
        public bool VerticalScrollBarVisible
        {
            get { return VerticalScrollBar.Visible; }
            set { VerticalScrollBar.Visible = value; }
        }

        // added to the control property choices
        public bool HorizontalScrollBarVisible
        {
            get { return HorizontalScrollBar.Visible; }
            set { HorizontalScrollBar.Visible = value; }
        }
    }
}
