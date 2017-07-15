using System;
using System.Drawing;
using System.Windows.Forms;

namespace DataGridViewTools
{
    public partial class SubclassedDataGridView : DataGridView
    {
        private bool vertScrollVisible;
        private bool horizScrollVisible;

        public SubclassedDataGridView()
        {
            VerticalScrollBar.VisibleChanged += VerticalScrollBar_VisibleChanged;
            HorizontalScrollBar.VisibleChanged += HorizontalScrollBar_VisibleChanged;
        }

        void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            // prevents continous loop error
            if (!VerticalScrollBar.Parent.Visible)
                return;

            if (!vertScrollVisible)
                return;

            int width = VerticalScrollBar.Width;
            VerticalScrollBar.Location = new Point(ClientRectangle.Width - width - 2, 21);

            if (horizScrollVisible)
                VerticalScrollBar.Size = new Size(width, ClientRectangle.Height - 21 - 2);
            else
                VerticalScrollBar.Size = new Size(width, ClientRectangle.Height - 2);

            VerticalScrollBar.Show();
        }

        void HorizontalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            // prevents continous loop error
            if (!HorizontalScrollBar.Parent.Visible)
                return;

            if (!horizScrollVisible)
                return;

            int height = HorizontalScrollBar.Height;
            HorizontalScrollBar.Location = new Point(2, ClientRectangle.Height - 18);

            if (vertScrollVisible)
                HorizontalScrollBar.Size = new Size(ClientRectangle.Width - 21, height);
            else
                HorizontalScrollBar.Size = new Size(ClientRectangle.Width - 3, height);

            HorizontalScrollBar.Show();
        }

        // added to the control property choices
        public bool VerticalScrollBarVisible
        {
            get { return vertScrollVisible; }
            set { vertScrollVisible = value; }
        }

        // added to the control property choices
        public bool HorizontalScrollBarVisible
        {
            get { return horizScrollVisible; }
            set { horizScrollVisible = value; }
        }


    }
}
