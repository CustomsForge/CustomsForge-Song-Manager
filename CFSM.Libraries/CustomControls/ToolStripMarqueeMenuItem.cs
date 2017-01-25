using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace CustomControls
{
    public class ToolStripMarqueeMenuItem : ToolStripEnhancedMenuItem
    {

        #region Local Fields

        // Constants
        const MarqueeScrollDirection DEFAULT_MARQUEE_SCROLL_DIRECTION = MarqueeScrollDirection.RightToLeft;
        const int DEFAULT_REFRESH_INTERVAL = 30;
        const int DEFAULT_SCROLL_STEP = 1;
        const bool DEFAULT_STOP_SCROLL_ON_MOUSE_OVER = true;

        //If <=0, minimum text width is equal text width. If value is greater than zero, this value is used as text area width.
        int m_MinimumTextWidth;

        // Marquee text.
        string m_Text;

        //Every time new text is assigned, text is measured ans stored here
        Size m_TextSize;

        //Timer used to repaint scrolled text
        Timer m_Timer;

        //Value modified in Timer tick event. Used to represent ever changing text offset.
        int m_PixelOffest;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Instantiates Timer used to tick scrolling events and initializes default values.
        /// </summary>
        public ToolStripMarqueeMenuItem()
        {
            MarqueeScrollDirection = DEFAULT_MARQUEE_SCROLL_DIRECTION;
            m_Timer = new Timer();
            m_Timer.Interval = DEFAULT_REFRESH_INTERVAL;
            m_Timer.Tick += new EventHandler(m_Timer_Tick);
            m_Timer.Enabled = true;

            if (MarqueeScrollDirection == MarqueeScrollDirection.RightToLeft)
                m_PixelOffest = -m_TextSize.Width;
            else
                m_PixelOffest = m_TextSize.Width;

            CheckMarkDisplayStyle = CheckMarkDisplayStyle.None;
            CheckOnClick = false;
            CheckState = System.Windows.Forms.CheckState.Unchecked;

            StopScrollOnMouseOver = DEFAULT_STOP_SCROLL_ON_MOUSE_OVER;
            ScrollStep = DEFAULT_SCROLL_STEP;

            MinimumTextWidth = -1;
        }

        #endregion

        #region Timer event

        /// <summary>
        /// Recalculate new text position and and calls Invalidate to repaint.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Timer_Tick(object sender, EventArgs e)
        {
            //Change offset only when menu item is visible, mouse is not hovering over or StopScrollOnMouseOver is not set to 'false'
            if ((Visible) && ((!Selected) || (!StopScrollOnMouseOver)))
            {
                m_PixelOffest = (m_PixelOffest + ScrollStep + m_TextSize.Width) % (2 * m_TextSize.Width + 1) - m_TextSize.Width;
                Invalidate();
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if text is scrolled left-to-right or right-to-left.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_MARQUEE_SCROLL_DIRECTION)]
        [Description("Determines if text is scrolled left-to-right or right-to-left.")]
        public MarqueeScrollDirection MarqueeScrollDirection { set; get; }

        /// <summary>
        /// Value less or equal zero indicates that text area width is defined by with of Text string.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(-1)]
        [Description("Value less or equal zero indicates that text area width is defined by with of Text string.")]
        public int MinimumTextWidth
        {
            set
            {
                m_MinimumTextWidth = value;
                MeasureText();
            }
            get
            {
                return m_MinimumTextWidth;
            }
        }

        /// <summary>
        /// Determines how often new text position is recalculated. 
        /// Together with 'ScrollStep' property defines speed of moving text.
        /// Smaller value means faster moving text.
        /// Text scrolling speed in pixels per seconds can be expressed with the following formula:
        /// <br></br>
        /// 1000 * ScrolStep/RefreshInterval
        /// </summary>
        /// <remarks>
        /// On most computers fastest scrolling speed is achieved for property value around 20 milliseconds.
        /// Values smaller than 20 will not increase speed. If faster scrolling is needed,
        /// it can be achieved by increasing value of 'ScrollStep' property.
        /// </remarks>
        [Browsable(true)]
        [DefaultValue(DEFAULT_REFRESH_INTERVAL)]
        [Description("Interval in milliseconds when new position is calculated and refreshed.")]
        public int RefreshInterval
        {
            get { return m_Timer.Interval; }
            set { m_Timer.Interval = value; }
        }

        /// <summary>
        /// Determines how many pixels text shifts when position is recalculated. 
        /// Together with 'RefreshInterval' property defines speed of moving text.
        /// Bigger value means faster moving text.
        /// Text scrolling speed in pixels per seconds can be expressed with the following formula:
        /// <br></br>
        /// 1000 * ScrolStep/RefreshInterval
        /// </summary>
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_SCROLL_STEP)]
        [Description("Number of pixels text position shifts when new position is calculated and refreshed")]
        public int ScrollStep { set; get; }

        /// <summary>
        /// When sets to 'true', every time mouse pointer moves over tool strip item, scrolling stops.
        /// Otherwise scrolling never stops.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DEFAULT_STOP_SCROLL_ON_MOUSE_OVER)]
        [Description(" When sets to 'true', every time mouse pointer moves over scrolling stops.  Otherwise scrolling never stops.")]
        public bool StopScrollOnMouseOver { set; get; }

        /// <summary>
        /// Sets or gets text value of menu item text.
        /// </summary>
        [Browsable(true)]
        [Description("Sets or gets text value of menu item text.")]
        public new string Text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;

                MeasureText();

                //Assign only spaces text to the base;
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Every time Text or Font properties change this method is called to recalculate
        /// commonly used text metrics.
        /// </summary>
        private void MeasureText()
        {
            m_TextSize = TextRenderer.MeasureText(m_Text, Font);

            //Calculate size of masked text passed to the base class. Base class doesn't know
            //real value of Text property. It  uses only white spaced string with length
            //equal to length of Text.
            StringBuilder allSpacesString = new StringBuilder(" ");

            int maxWidth = MinimumTextWidth > 0 ? MinimumTextWidth : m_TextSize.Width;

            while (TextRenderer.MeasureText(allSpacesString.ToString(), Font).Width < maxWidth)
                allSpacesString.Append(' ');

            base.Text = allSpacesString.ToString();
        }

        #endregion

        #region Override

        /// <summary>
        /// Dispose override to stop and dispose Timer object as soon as possible.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            m_Timer.Enabled = false;
            m_Timer.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Every time Font changes text metrics have to be recalculated.
        /// </summary>
        /// <param name="e">Standard EventArgs for OnFontChanged</param>
        protected override void OnFontChanged(EventArgs e)
        {
            MeasureText();
            base.OnFontChanged(e);
        }

        /// <summary>
        /// Method responsible for painting text every time new text offset is recalculated.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);  //Paint text (blank) and check box/radio button (if required)

            //Paint scrolling text
            ToolStrip parent = GetCurrentParent();
            Rectangle displayRect = parent.DisplayRectangle;
            int horizPadding = parent.Padding.Horizontal;

            Rectangle clipRectangle = new Rectangle(displayRect.X, displayRect.Y, displayRect.Width - horizPadding, displayRect.Height);

            e.Graphics.FillRectangle(Brushes.Transparent, e.ClipRectangle);

            int textYPosition = (this.Size.Height - m_TextSize.Height) / 2;

            Region savedClip = e.Graphics.Clip;
            Region clipRegion = new Region(clipRectangle);
            e.Graphics.Clip = clipRegion;
            if (MarqueeScrollDirection == MarqueeScrollDirection.RightToLeft)
                e.Graphics.DrawString(m_Text, Font, Brushes.Black, -m_PixelOffest + horizPadding, textYPosition);
            else
                e.Graphics.DrawString(m_Text, Font, Brushes.Black, +m_PixelOffest + horizPadding, textYPosition);

            clipRegion.Dispose();
            e.Graphics.Clip = savedClip;
        }

        #endregion
    }

    [Obfuscation(Exclude = false, Feature = "-rename")]
    public enum MarqueeScrollDirection
    {
        RightToLeft,
        LeftToRight
    }
}
