using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Diagnostics;

// FIXME: IDE does not display properties or event handlers in the dropdown
// FIXME: Tooltip and overrides are not working

namespace CustomControls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    [DefaultProperty("Items")]
    [ToolboxBitmap(typeof(Button))]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ToolStripButton : ToolStripControlHost
    {
        #region Private fields

        private ToolStripItem _mouseOverItem = null;
        private Point _mouseOverPoint;
        private const int DEFAULT_TOOLTIP_INTERVAL = 3000;  // aka AutoPopDelay
        private const int DEFAULT_RESHOW_DELAY = 100;
        private const int DEFAULT_INITIAL_DELAY = 100;
        private const bool DEFAULT_SHOW_ALWAYS = false;
        private bool _firstEntry = true;
        private ToolTip tt;

        # endregion

        #region Constructors

        public ToolStripButton()
            : base(new FlowLayoutPanel { AutoSize = true, BackColor = Color.Transparent, Margin = new Padding(1, 1, 0, 2) })
        {
            tt = new ToolTip();
            ShowAlways = DEFAULT_SHOW_ALWAYS;
            ReshowDelay = DEFAULT_RESHOW_DELAY;
            InitialDelay = DEFAULT_INITIAL_DELAY;
            ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;

            Button = new Button
            {
                Anchor = AnchorStyles.Left,
                AutoSize = true,
                Margin = new Padding { All = 0 },
                Text = "Enter Button Text"
            };

            Panel.Controls.AddRange(new Control[] { Button });
        }

        #endregion

        public event EventHandler Click
        {
            add { Button.Click += value; }
            remove { Button.Click -= value; }
        }

        //public event EventHandler MouseEnter
        //{
        //    add { Button.MouseEnter += value; }
        //    remove { Button.MouseEnter -= value; }
        //}

        #region Properties

        public Button Button { get; private set; }
        public string ButtonText
        {
            get { return Button.Text; }
            set { Button.Text = value; }
        }

        private FlowLayoutPanel Panel
        {
            get { return Control as FlowLayoutPanel; }
        }

        public int m_InitialDelay = DEFAULT_INITIAL_DELAY;
        [Browsable(true)]
        [DefaultValue(DEFAULT_INITIAL_DELAY)]
        [Description("Sets ToolTip Initial Delay (when first entered)")]
        public int InitialDelay
        {
            get { return m_InitialDelay; }
            set { m_InitialDelay = value; }
        }

        public int m_ReshowDelay = DEFAULT_RESHOW_DELAY;
        [Browsable(true)]
        [DefaultValue(DEFAULT_RESHOW_DELAY)]
        [Description("Sets ToolTip Reshow Delay interval in mSecs (max 32767")]
        public int ReshowDelay
        {
            get { return m_ReshowDelay; }
            set { m_ReshowDelay = value; }
        }

        public int m_ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;
        [Browsable(true)]
        [DefaultValue(DEFAULT_TOOLTIP_INTERVAL)]
        [Description("Sets ToolTip display interval in mSecs (max 32767")]
        public int ToolTipInterval
        {
            get { return m_ToolTipInterval; }
            set { m_ToolTipInterval = value; }
        }

        public string m_ToolTipText = "";
        [Browsable(true)]
        [DefaultValue("")]
        [Description("Enter ToolTip Text")]
        public string ToolTipText
        {
            get { return m_ToolTipText; }
            set { m_ToolTipText = value; }
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

        public bool m_ShowAlways = DEFAULT_SHOW_ALWAYS;
        [Browsable(true)]
        [DefaultValue(DEFAULT_SHOW_ALWAYS)]
        [Description("Sets ToolTip ShowAlways (forces display independant of form activity)")]
        public bool ShowAlways
        {
            get { return m_ShowAlways; }
            set { m_ShowAlways = value; }
        }

        #endregion

        #region Overrides
        // FIXME: only the Dispose override is working
        // all other overrides are not working ... don't know why???

        protected override void OnMouseEnter(EventArgs e)
        {
            // for debugging override issue
            base.OnMouseEnter(e);
            base.BackColor = Color.Blue;
        }

        // using OnMouseMove instead OnMouseEnter to get mea location
        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);
            ToolStrip parent = GetCurrentParent();
            ToolStripItem newMouseOverItem = parent.GetItemAt(mea.Location);

            if (_firstEntry && !String.IsNullOrEmpty(m_ToolTipText))
            {
                // these get set one time on first entry
                tt.Active = true;
                tt.IsBalloon = IsBalloon;
                tt.ShowAlways = ShowAlways;
                tt.InitialDelay = InitialDelay;
                tt.ReshowDelay = ReshowDelay;
                tt.AutoPopDelay = ToolTipInterval;
                _firstEntry = false;

                Debug.WriteLine("_mouseOverItem is null");
                Debug.WriteLine("IsBallon: " + IsBalloon);
                Debug.WriteLine("ShowAlways: " + ShowAlways);
                Debug.WriteLine("InitialDelay: " + InitialDelay);
                Debug.WriteLine("ReshowDelay: " + ReshowDelay);
                Debug.WriteLine("AutoPopDelay: " + ToolTipInterval);
            }

            if ((_mouseOverItem != newMouseOverItem) ||
            (Math.Abs(_mouseOverPoint.X - mea.X) > SystemInformation.MouseHoverSize.Width || (Math.Abs(_mouseOverPoint.Y - mea.Y) > SystemInformation.MouseHoverSize.Height)))
            // TODO: monitor here ... may create tooltip tracks
            {
                _mouseOverItem = newMouseOverItem;
                _mouseOverPoint = mea.Location;

                if (!String.IsNullOrEmpty(m_ToolTipText))
                {
                    Debug.WriteLine("_mouseOverItem != newMouseOverItem");

                    tt.Active = true;
                    tt.IsBalloon = IsBalloon;
                    // pretty balloon
                    // Point currentMouseOverPoint = parent.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y + Cursor.Current.HotSpot.Y));
                    // tt.Show(ToolTipText, parent, currentMouseOverPoint, ToolTipInterval);
                    tt.SetToolTip(Button, ToolTipText);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);
            ToolStrip parent = GetCurrentParent();
            ToolStripItem newMouseOverItem = parent.GetItemAt(mea.Location);
            if (newMouseOverItem != null && !String.IsNullOrEmpty(m_ToolTipText))
            {
                // once clicked do not display again until leave and come back
                tt.Active = false;
                tt.Hide(parent);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            ToolStrip parent = GetCurrentParent();
            if (parent == null)
                return;

            ToolStripItem newMouseOverItem = parent.GetItemAt(mea.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            base.BackColor = Color.LightGray;

            ToolStrip parent = GetCurrentParent();
            var mea = parent.PointToClient(Control.MousePosition);

            // detect if mouse moved off target
            if (Math.Abs(_mouseOverPoint.X - mea.X) < SystemInformation.MouseHoverSize.Width && (Math.Abs(_mouseOverPoint.Y - mea.Y) < SystemInformation.MouseHoverSize.Height))
            {
                if (!String.IsNullOrEmpty(m_ToolTipText))
                {
                    tt.Active = false;
                    tt.Hide(parent);
                    _mouseOverPoint = new Point(-50, -50);
                    _mouseOverItem = null;
                }

                Debug.WriteLine("Detected mouse leave event");
            }
            else
                Debug.WriteLine("Suppressed mouse leave event");
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && tt != null)
                tt.Dispose();
        }

        #endregion

    }
}









