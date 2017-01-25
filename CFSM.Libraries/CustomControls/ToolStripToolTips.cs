using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    // can be applied to MenuStrip ... just change ': ToolStrip' to ': MenuStrip'
    public class ToolStripToolTips : ToolStrip // : MenuStrip
    {
        private ToolStripItem mouseOverItem = null;
        private Point mouseOverPoint;
        private Timer timer;
        private ToolTip tt;
        private const int DEFAULT_TOOLTIP_INTERVAL = 32767;
        public string ToolTipText;

        #region Constructor

        /// <summary>
        /// Constructor: initiates default settings. 
        /// </summary>
        public ToolStripToolTips()
            : base()
        {
            ShowItemToolTips = false; // overrides the internal value
            timer = new Timer();
            timer.Enabled = false;
            timer.Interval = SystemInformation.MouseHoverTime;
            timer.Tick += new EventHandler(timer_Tick);
            tt = new ToolTip();
            tt.IsBalloon = m_IsBallon;
            tt.InitialDelay = m_InitialDelay;
            tt.ShowAlways = m_ShowAlways;
        }

        #endregion

        #region Public properties

        public int m_ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;
        [Browsable(true)]
        [DefaultValue(DEFAULT_TOOLTIP_INTERVAL)]
        [Description("Sets ToolTip display interval in mSecs (max 32767")]
        public int ToolTipInterval
        {
            get { return m_ToolTipInterval; }
            set { m_ToolTipInterval = value; }
        }

        public bool m_IsBallon = true;
        [Browsable(true)]
        [DefaultValue(true)]
        [Description("Sets ToolTip IsBallon")]
        public bool IsBalloon
        {
            get { return m_IsBallon; }
            set { m_IsBallon = value; }
        }

        public int m_InitialDelay = 0;
        [Browsable(true)]
        [DefaultValue(false)]
        [Description("Sets ToolTip InitialDelay in mSecs")]
        public int InitialDelay
        {
            get { return m_InitialDelay; }
            set { m_InitialDelay = value; }
        }

        public bool m_ShowAlways = true;
        [Browsable(true)]
        [DefaultValue(true)]
        [Description("Sets ToolTip ShowAlways")]
        public bool ShowAlways
        {
            get { return m_ShowAlways; }
            set { m_ShowAlways = value; }
        }

        public bool m_ShowAbove = true;
        [Browsable(true)]
        [DefaultValue(true)]
        [Description("Show ToolTip Above")]
        public bool ShowAbove
        {
            get { return m_ShowAbove; }
            set { m_ShowAbove = value; }
        }

        #endregion

        #region Overrides

        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);
            ToolStripItem newMouseOverItem = this.GetItemAt(mea.Location);
            if (mouseOverItem != newMouseOverItem)
            // || (Math.Abs(mouseOverPoint.X - mea.X) > SystemInformation.MouseHoverSize.Width || (Math.Abs(mouseOverPoint.Y - mea.Y) > SystemInformation.MouseHoverSize.Height)))
            // commented out becauses this was leaving tooltip tracks
            {
                mouseOverItem = newMouseOverItem;
                mouseOverPoint = mea.Location;
                if (tt != null)
                    tt.Hide(this);
                timer.Stop();
                timer.Start();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ToolStripItem newMouseOverItem = this.GetItemAt(e.Location);
            if (newMouseOverItem != null && tt != null)
            {
                tt.Hide(this);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            ToolStripItem newMouseOverItem = this.GetItemAt(mea.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            timer.Stop();
            if (tt != null)
                tt.Hide(this);
            mouseOverPoint = new Point(-50, -50);
            mouseOverItem = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                timer.Dispose();
                tt.Dispose();
            }
        }

        #endregion


        void timer_Tick(object sender, EventArgs e)
        {
            // not my code ... kinda pointless ... don't you think
            timer.Stop();
            try
            {
                Point currentMouseOverPoint;
                if (ShowAbove)
                    currentMouseOverPoint = this.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y - Cursor.Current.Size.Height - Cursor.Current.HotSpot.Y));
                else
                    currentMouseOverPoint = this.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y + Cursor.Current.HotSpot.Y));

                if (mouseOverItem == null)
                {
                    if (ToolTipText != null && ToolTipText.Length > 0)
                    {
                        if (tt == null)
                            tt = new ToolTip();
                        tt.Show(ToolTipText, this, currentMouseOverPoint, ToolTipInterval);
                    }
                }
                else if ((!(mouseOverItem is ToolStripDropDownButton) && !(mouseOverItem is ToolStripSplitButton)) ||
                    ((mouseOverItem is ToolStripDropDownButton) && !((ToolStripDropDownButton)mouseOverItem).DropDown.Visible) ||
                    (((mouseOverItem is ToolStripSplitButton) && !((ToolStripSplitButton)mouseOverItem).DropDown.Visible)))
                {
                    if (mouseOverItem.ToolTipText != null && mouseOverItem.ToolTipText.Length > 0 && tt != null)
                    {
                        if (tt == null)
                            tt = new ToolTip();
                        tt.Show(mouseOverItem.ToolTipText, this, currentMouseOverPoint, ToolTipInterval);
                    }
                }
            }
            catch
            { }
         }
 
    }
}
