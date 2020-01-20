using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

// FIXME: IDE does not display properties or event handlers in the dropdown

namespace CustomControls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    [DefaultProperty("Items")]
    [ToolboxBitmap(typeof(NumericUpDown))]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        private FlowLayoutPanel controlPanel = null;
        private NumericUpDown num = null;
        private Label txt = null;
        private ToolStripItem _mouseOverItem = null;
        private Point _mouseOverPoint;
        private const int DEFAULT_TOOLTIP_INTERVAL = 6000;  // aka AutoPopDelay
        private const int DEFAULT_RESHOW_DELAY = 200;
        private const int DEFAULT_INITIAL_DELAY = 200;
        private const bool DEFAULT_SHOW_ALWAYS = true;
        private bool _firstEntry = true;
        private ToolTip tt;

        public event EventHandler ValueChanged
        {
            add { this.num.ValueChanged += value; }
            remove { this.num.ValueChanged -= value; }
        }

        private FlowLayoutPanel Panel
        {
            get { return Control as FlowLayoutPanel; }
        }

        #region Properties

        [DefaultValue(true)]
        public bool TextVisible
        {
            get { return this.txt.Visible; }
            set
            {
                if (value == false) // force visiblity
                    value = true;

                this.txt.Visible = value;
                this.UpdateAutoSize();
            }
        }

        public override string Text
        {
            get { return this.txt.Text; }
            set
            {
                this.txt.Text = value;
                this.UpdateAutoSize();
            }
        }

        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (base.Size != value && !this.AutoSize)
                {
                    base.Size = value;
                    this.OnSizeChanged();
                }
            }
        }

        [DefaultValue(50)]
        public int NumericWidth
        {
            get { return this.num.Width; }
            set
            {
                this.num.Width = value;
                this.UpdateAutoSize();
            }
        }

        // [DefaultValue(typeof(decimal), "100")]
        public decimal Maximum
        {
            get { return this.num.Maximum; }
            set { this.num.Maximum = value; }
        }

        // [DefaultValue(typeof(decimal), "0")]
        public decimal Minimum
        {
            get { return this.num.Minimum; }
            set { this.num.Minimum = value; }
        }

        // [DefaultValue(typeof(decimal), "0.0")]
        public decimal DecimalValue
        {
            get { return this.num.Value; }
            set
            {
                if (value < Minimum)
                    this.num.Value = Minimum;
                else if (value > Maximum)
                    this.num.Value = Maximum;
                else
                    this.num.Value = value;
            }
        }

        // [DefaultValue(0)]
        public int DecimalPlaces
        {
            get { return this.num.DecimalPlaces; }
            set { this.num.DecimalPlaces = value; }
        }

        // [DefaultValue(typeof(decimal), "1")]
        public decimal Increment
        {
            get { return this.num.Increment; }
            set { this.num.Increment = value; }
        }

        [DefaultValue(false)]
        public bool Hexadecimal
        {
            get { return this.num.Hexadecimal; }
            set { this.num.Hexadecimal = value; }
        }

        [DefaultValue(typeof(HorizontalAlignment), "Center")]
        public HorizontalAlignment NumericTextAlign
        {
            get { return this.num.TextAlign; }
            set { this.num.TextAlign = value; }
        }

        public Color NumBackColor
        {
            get { return this.num.BackColor; }
            set { this.num.BackColor = value; }
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

        public ToolStripNumericUpDown()
            : base(new FlowLayoutPanel())
        {
            tt = new ToolTip();
            ShowAlways = DEFAULT_SHOW_ALWAYS;
            ReshowDelay = DEFAULT_RESHOW_DELAY;
            InitialDelay = DEFAULT_INITIAL_DELAY;
            ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;

            // Set up the FlowLayouPanel.
            this.controlPanel = (FlowLayoutPanel)Control;
            this.controlPanel.BackColor = Color.Transparent;
            this.controlPanel.WrapContents = false;
            this.controlPanel.AutoSize = true;
            this.controlPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Add child controls.
            this.num = new NumericUpDown();
            this.num.Width = 50;
            this.num.Height = this.num.PreferredHeight;
            this.num.Margin = new Padding(0, 1, 3, 1);
            this.num.Value = 0;
            this.num.Minimum = 0;
            this.num.Maximum = 100;
            this.num.DecimalPlaces = 0;
            this.num.Increment = 1;
            this.num.Hexadecimal = false;
            this.num.TextAlign = HorizontalAlignment.Center;
            this.num.Visible = true;

            this.txt = new Label();
            this.txt.Text = "NumericUpDown";
            this.txt.TextAlign = ContentAlignment.MiddleRight;
            this.txt.AutoSize = true;
            this.txt.Dock = DockStyle.Left;
            this.txt.Visible = true;

            this.controlPanel.Controls.Add(this.txt);
            this.controlPanel.Controls.Add(this.num);
        }

        #region Overrides

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
                    tt.SetToolTip(num, ToolTipText);
                    tt.SetToolTip(txt, ToolTipText);
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

            // Commented out becuase tooltip shows/hides properly without this          
            //ToolStrip parent = GetCurrentParent();
            //var mea = parent.PointToClient(Control.MousePosition);

            //// detect if mouse moved off target
            //if (Math.Abs(_mouseOverPoint.X - mea.X) < SystemInformation.MouseHoverSize.Width && (Math.Abs(_mouseOverPoint.Y - mea.Y) < SystemInformation.MouseHoverSize.Height))
            //{
            //    if (!String.IsNullOrEmpty(m_ToolTipText))
            //    {
            //        tt.Active = false;
            //        tt.Hide(parent);
            //        _mouseOverPoint = new Point(-50, -50);
            //        _mouseOverItem = null;
            //    }

            //    Debug.WriteLine("Detected mouse leave event");
            //}
            //else
            //    Debug.WriteLine("Suppressed mouse leave event");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && tt != null)
                tt.Dispose();
        }

        protected void UpdateAutoSize()
        {
            if (!this.AutoSize) return;
            this.AutoSize = false;
            this.AutoSize = true;
        }

        protected void OnSizeChanged()
        {
            if (this.num != null && this.controlPanel != null && this.txt != null)
            {
                this.num.Width = this.controlPanel.ClientSize.Width - this.txt.Width - this.controlPanel.Margin.Horizontal - this.controlPanel.Margin.Horizontal;
            }
        }

        #endregion

    }
}








