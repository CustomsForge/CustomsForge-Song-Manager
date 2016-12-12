using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace CustomsForgeSongManager.LocalTools
{
    /// <summary>
    /// Enhanced ToolStripMenuItem with the ability to display radio button for checked item.
    /// Set individual tooltip and display intervals for each ToolStripMenuItem
    /// <br></br>
    /// If CheckOnClick property is set to true and CheckMarkDisplayStyle is set to RadioButton, context
    /// menu strip behaves similar way as GroupBox with RadioButton controls.
    /// Within same group only one item can be selected.
    /// 
    /// REM Set 'ShowItemToolTips' in MenuStrip to false to prevent showing double tips

    /// </summary>
    [ToolboxBitmap(typeof(ToolStripMenuItem))]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class ToolStripEnhancedMenuItem : ToolStripMenuItem
    {
        #region Private fields

        private CheckMarkDisplayStyle m_CheckMarkDisplayStyle;
        private ToolStripItem _mouseOverItem = null;
        private Point _mouseOverPoint;
        private const int DEFAULT_TOOLTIP_INTERVAL = 20000;  // aka AutoPopDelay
        private const int DEFAULT_RESHOW_DELAY = 2000;
        private const int DEFAULT_INITIAL_DELAY = 300;
        private const bool DEFAULT_SHOW_ALWAYS = false;

        private bool _firstEntry = true;
        private ToolTip tt;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor: initiates default settings.
        /// Defualt is normal menu item that has blank tooltip 
        /// </summary>
        public ToolStripEnhancedMenuItem()
            : base()
        {
            CheckMarkDisplayStyle = CheckMarkDisplayStyle.CheckBox;
            CheckOnClick = false;
            CheckState = CheckState.Unchecked;
            // create single new instance prevents tooltip tracks
            tt = new ToolTip();
            ShowAlways = DEFAULT_SHOW_ALWAYS;
            ReshowDelay = DEFAULT_RESHOW_DELAY;
            InitialDelay = DEFAULT_INITIAL_DELAY;
            ToolTipInterval = DEFAULT_TOOLTIP_INTERVAL;
        }

        #endregion

        #region Public properties

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

        /// <summary>
        /// Menu items with radio button display can be used to bind enum values.
        /// This property can be used to store associated Enum value.
        /// </summary>
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public Enum AssociatedEnumValue { get; set; }

        /// <summary>
        /// Switches between CheckBox or RadioButton style.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [DefaultValue(CheckMarkDisplayStyle.CheckBox)]
        [Description("If set to 'None' then no action is taken when clicked!")]
        public CheckMarkDisplayStyle CheckMarkDisplayStyle
        {
            get { return m_CheckMarkDisplayStyle; }
            set
            {
                m_CheckMarkDisplayStyle = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// In order to provide behavior similar to RadioButton group, we need a way to mark groups. 
        /// This property is used for this purpose. All menu items with identical RadioButtonGroupName belong to the same group.
        /// It means that clicking one item within group de-selects previously selected item and 
        /// selects clicked item (only one item can be selected).
        /// </summary>
        [DefaultValue("")]
        public string RadioButtonGroupName { set; get; }

        #endregion

        #region Overrides

        /// <summary>
        /// If menu item belongs to the radio group, this override ensures proper functionality 
        /// (select clicked item and de-select all others from the same group).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            if (CheckMarkDisplayStyle == CheckMarkDisplayStyle.None) // do nothing
                return;

            if ((CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton) && (CheckOnClick))
            {
                //Un-click all radio buttons different than the clicked one
                ToolStrip toolStrip = this.GetCurrentParent();

                //Iterate all siblings of clicked item and make them unchecked
                foreach (ToolStripItem toolStripItem in toolStrip.Items)
                {
                    if (toolStripItem is ToolStripEnhancedMenuItem)
                    {
                        ToolStripEnhancedMenuItem toolStripEnhancedItem = (ToolStripEnhancedMenuItem)toolStripItem;
                        if ((toolStripEnhancedItem.CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton) &&
                            (toolStripEnhancedItem.CheckOnClick) &&
                            (toolStripEnhancedItem.RadioButtonGroupName == RadioButtonGroupName))
                            toolStripEnhancedItem.Checked = false;
                    }
                }
            }
            //If CheckOnClick is 'true', base.OnClick will make clicked item selected.
            base.OnClick(e);
        }

        /// <summary>
        /// if CheckMarkDisplayStyle is equal RadioButton OnPaint override paints radio button images. 
        /// </summary>
        /// <param name="e">Standard event arguments for OnPaint method.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint will render menu item.
            base.OnPaint(e);

            if (CheckMarkDisplayStyle == CheckMarkDisplayStyle.None) // do nothing
                return;

            //if CheckMarkDisplayStyle is equal RadioButton additional paining or radio button is needed
            if ((CheckMarkDisplayStyle == CheckMarkDisplayStyle.RadioButton))
            {
                //Find location of radio button
                Size radioButtonSize = RadioButtonRenderer.GetGlyphSize(e.Graphics, RadioButtonState.CheckedNormal);
                int radioButtonX = ContentRectangle.X + 3;
                int radioButtonY = ContentRectangle.Y + (ContentRectangle.Height - radioButtonSize.Height) / 2;

                //Find state of radio button
                RadioButtonState state = RadioButtonState.CheckedNormal;
                if (this.Checked)
                {
                    if (Pressed)
                        state = RadioButtonState.CheckedPressed;
                    else if (Selected)
                        state = RadioButtonState.CheckedHot;
                }
                else
                {
                    if (Pressed)
                        state = RadioButtonState.UncheckedPressed;
                    else if (Selected)
                        state = RadioButtonState.UncheckedHot;
                    else
                        state = RadioButtonState.UncheckedNormal;
                }

                //Draw RadioButton in proper state (Checked/Unchecked; Hot/Normal/Pressed)
                RadioButtonRenderer.DrawRadioButton(e.Graphics, new Point(radioButtonX, radioButtonY), state);
            }
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
                    // Point currentMouseOverPoint = parent.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y + Cursor.Current.HotSpot.Y));
                    // tt.Show(ToolTipText, parent, currentMouseOverPoint, ToolTipInterval);
                    tt.SetToolTip(parent, ToolTipText);
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
                // once click does not display again to leave and come back
                tt.Active = false;
                tt.Hide(parent);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            ToolStrip parent = GetCurrentParent();
            ToolStripItem newMouseOverItem = parent.GetItemAt(mea.Location);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ToolStrip parent = GetCurrentParent();

            if (!String.IsNullOrEmpty(m_ToolTipText))
            {
                tt.Active = false;
                tt.Hide(parent);
            }

            _mouseOverPoint = new Point(-50, -50);
            _mouseOverItem = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                tt.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Check mark display style. 
    /// </summary>
    [Obfuscation(Exclude = false, Feature = "-rename")]
    public enum CheckMarkDisplayStyle
    {
        None = 0,
        CheckBox = 1,
        RadioButton = 2
    }

    // tsmiFileInfo.Image = new Bitmap(Resources.greencheckmark);

}
