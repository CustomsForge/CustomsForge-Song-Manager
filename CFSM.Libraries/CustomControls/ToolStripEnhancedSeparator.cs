using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomControls
{
    /// <summary>
    /// Enhanced menu separator. It act as "standard" menu separator with 
    /// additional ability to display positioned text.
    /// </summary>
    [ToolboxBitmap(typeof(ToolStripSeparator))]
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public partial class ToolStripEnhancedSeparator : ToolStripMenuItem
    {
        #region Private fields
        private bool m_ShowSeparatorLine;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor: initiated default settings. Default color of separator line and text is GrayText.
        /// Also separator by default shows separator line (can be switched off).
        /// </summary>
        public ToolStripEnhancedSeparator()
        {
            ForeColor = SystemColors.GrayText;
            ShowSeparatorLine = true;
        }
        #endregion

        #region Public properties

        /// <summary>
        /// If set to 'true' separator line will be displayed in areas not occupied by text.
        /// Otherwise only text will be displayed.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("If set to 'true' separator line will be displayed in areas not occupied by text. Otherwise only text will be displayed.")]
        public bool ShowSeparatorLine
        {
            get { return m_ShowSeparatorLine; }
            set { m_ShowSeparatorLine = value; this.Invalidate(); }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Separator is not selectable, therefore CanSelect always returns false.
        [EditorBrowsable(EditorBrowsableState.Never)]
        /// </summary>
        [Browsable(false)]
        public override bool CanSelect
        {
            get { return false; }
        }


        /// <summary>
        /// Returns separator preferred size. If text is blank separator height is limited 
        /// to 5 pixels.
        /// </summary>
        /// <param name="constrainingSize"></param>
        /// <returns></returns>
        public override Size GetPreferredSize(Size constrainingSize)
        {
            
            if (string.IsNullOrEmpty(Text))
                return new Size(base.GetPreferredSize(constrainingSize).Width, 5);
            else
                return base.GetPreferredSize(constrainingSize);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ToolStrip ts = this.Owner ?? this.GetCurrentParent();
            int textLeft = ts.Padding.Horizontal;

            if (ts.BackColor != this.BackColor)
            {
                using (SolidBrush sb = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(sb, e.ClipRectangle);
                }
            }



            Size textSize = TextRenderer.MeasureText(Text, Font);

            //Find horizontal text position offset
            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    textLeft = (ContentRectangle.Width + textLeft - textSize.Width) / 2;
                    break;
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    textLeft = ContentRectangle.Right - textSize.Width;
                    break;

            }

            int yLinePosition = (ContentRectangle.Bottom - ContentRectangle.Top) / 2;
            int yTextPosition = (ContentRectangle.Bottom - textSize.Height - ContentRectangle.Top) / 2;

            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomRight:
                    yLinePosition = yTextPosition;
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopRight:
                    yLinePosition = yTextPosition + textSize.Height;
                    break;
            }

            using (Pen pen = new Pen(ForeColor))
            {
                if (ShowSeparatorLine)
                    e.Graphics.DrawLine(pen, ts.Padding.Horizontal, yLinePosition, textLeft, yLinePosition);

                TextRenderer.DrawText(e.Graphics, Text, Font, new Point(textLeft, yTextPosition), ForeColor);

                if (ShowSeparatorLine)
                    e.Graphics.DrawLine(pen, textLeft + textSize.Width, yLinePosition, ContentRectangle.Right, yLinePosition);
            }
        }

        #endregion

        #region properties that should be not visible (they do not have meaning for separator)

        /// <summary>
        /// Checked property doesn't make sense for separator, we need to hide 
        /// base functionality and never call base property in case Checked is used in code. 
        /// Setting Checked property to any value does nothing.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Checked { set; get; }

        /// <summary>
        /// CheckState property doesn't make sense for separator, we need to hide 
        /// base functionality and never call base property in case CheckState is used in code. 
        /// Setting CheckState property to any value does nothing.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new CheckState CheckState { set; get; }

        /// <summary>
        /// DropDown property doesn't make sense for separator, we need to hide 
        /// base functionality and never call base property in case DropDown is used in code. 
        /// Setting DropDown property to any value does nothing.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ToolStripDropDown DropDown { set; get; }


        /// <summary>
        /// DropDownItems property doesn't make sense for separator, we need to hide 
        /// base functionality and never set base property in case DropDownItems is used in code. 
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ToolStripItemCollection DropDownItems
        { get { return base.DropDownItems; } }

        /// <summary>
        /// Separator cannot have DropDown items, therefore <b>HasDropDownItems</b> always returns <i>false</i>.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool HasDropDownItems
        {
            get { return false; }
        }

        #endregion

    }
}
