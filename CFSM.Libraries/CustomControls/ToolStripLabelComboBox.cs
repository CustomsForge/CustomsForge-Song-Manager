using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

// TODO: FIXME IDE does not display control events in dropdown

namespace CustomControls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    [DefaultProperty("Items")]
    public class ToolStripLabelCombo : ToolStripControlHost
    {
        #region Private fields

        private ToolStripItem _mouseOverItem = null;
        private Point _mouseOverPoint;
        private const int DEFAULT_TOOLTIP_INTERVAL = 4000;  // aka AutoPopDelay
        private const int DEFAULT_RESHOW_DELAY = 300;
        private const int DEFAULT_INITIAL_DELAY = 100;
        private const bool DEFAULT_SHOW_ALWAYS = false;
        private bool _firstEntry = true;
        private ToolTip tt;

        # endregion

        #region Constructors

        public ToolStripLabelCombo()
            : base(new FlowLayoutPanel { AutoSize = true, BackColor = Color.Transparent, Margin = new Padding(3, 3, 0, 2) })
        {
            tt = new ToolTip();
            ShowAlways = DEFAULT_SHOW_ALWAYS;
            ReshowDelay = DEFAULT_RESHOW_DELAY;
            InitialDelay = DEFAULT_INITIAL_DELAY;
            ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;

            Label = new Label
               {
                   Anchor = AnchorStyles.Left,
                   AutoSize = true,
                   Margin = new Padding { All = 0 },
                   Text = "Combo Label"
               };
            ComboBox = new ComboBox
                {
                    Anchor = AnchorStyles.Left,
                    AutoSize = true,
                    Margin = new Padding { All = 0 }
                };

            Panel.Controls.AddRange(new Control[] { Label, ComboBox });
            ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        #endregion

        public event EventHandler SelectedIndexChanged
        {
            add { ComboBox.SelectedIndexChanged += value; }
            remove { ComboBox.SelectedIndexChanged -= value; }
        }

        #region Properties

        public ComboBox ComboBox { get; private set; }
        public ComboBox.ObjectCollection ComboItems
        {
            get { return ComboBox.Items; }
        }

        public Label Label { get; private set; }
        public string LabelText
        {
            get { return Label.Text; }
            set
            {
                this.UpdateAutoSize();
                Label.Text = value;
            }
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

            if (_mouseOverItem != newMouseOverItem) // ||
            // (Math.Abs(_mouseOverPoint.X - mea.X) > SystemInformation.MouseHoverSize.Width || (Math.Abs(_mouseOverPoint.Y - mea.Y) > SystemInformation.MouseHoverSize.Height)))
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
                    //Point currentMouseOverPoint = parent.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y + Cursor.Current.HotSpot.Y));
                    //tt.Show(ToolTipText, parent, currentMouseOverPoint, ToolTipInterval);
                    tt.SetToolTip(ComboBox, ToolTipText);
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
                // once clicked does not display again to leave and come back
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && tt != null)
                tt.Dispose();
        }

        public override string Text
        {
            get { return ComboBox.Text; }
            set { ComboBox.Text = value; }
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

        protected void UpdateAutoSize()
        {
            if (!this.AutoSize)
                return;
            this.AutoSize = false;
            this.AutoSize = true;
        }

        protected void OnSizeChanged()
        {
            if (ComboBox != null && Panel != null && Label != null)
            {
                ComboBox.Width = Panel.ClientSize.Width - Label.Width - Panel.Margin.Horizontal - Panel.Margin.Horizontal;
            }
        }

        #endregion

    }
}








