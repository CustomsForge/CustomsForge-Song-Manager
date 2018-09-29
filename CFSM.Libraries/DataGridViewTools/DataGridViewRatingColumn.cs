using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DataGridViewTools
{
    /// <summary>
    /// Represents a sortable/filterable DataGridViewColumn with a 5-Star Rating System.  
    /// Ref: https://stackoverflow.com/questions/19914880/using-a-custom-control-in-a-datagridview
    /// </summary>
    public class DataGridViewRatingColumn : DataGridViewColumn
    {
        private Color grayStarColor;
        private Color ratedStarColor;
        private bool readOnly;
        private float starScale;
        private int starOffset;

        public DataGridViewRatingColumn()
            : base(new DataGridViewRatingCell())
        {
            base.ReadOnly = true;
            RatedStarColor = Color.Green;
            GrayStarColor = Color.LightGray;
            StarScale = 0.9F;
        
            // make rating column sortable
            base.DefaultHeaderCellType = typeof(DataGridViewAutoFilterColumnHeaderCell);
            base.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        // FIXME: ToolTipText property default not working with filtering/sorting
        //"To remove a rating completely ...\r\nFirst click on the first star, then\r\nclick on the first star again, next\r\ncompletely move the mouse off.";

        [Browsable(true)]
        [Description("Sets gray star color")]
        public Color GrayStarColor
        {
            get { return grayStarColor; }
            set
            {
                if (grayStarColor != value)
                {
                    grayStarColor = value;
                    if (DataGridView != null)
                        DataGridView.InvalidateColumn(Index);
                }
            }
        }

        [Browsable(true)]
        [Description("Sets rated star color")]
        public Color RatedStarColor
        {
            get { return ratedStarColor; }
            set
            {
                if (ratedStarColor != value)
                {
                    ratedStarColor = value;
                    if (DataGridView != null)
                        DataGridView.InvalidateColumn(Index);
                }
            }
        }

        public new bool ReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }

        [Browsable(true)]
        [Description("Sets star scale")]
        public float StarScale
        {
            get { return starScale; }
            set
            {
                if (starScale != value)
                {
                    starScale = value;
                    DataGridViewRatingCell.UpdateBrushes(value);

                    if (DataGridView != null)
                        DataGridView.InvalidateColumn(Index);
                }
            }
        }


        #region public properties that hide inherited, non-virtual properties: DefaultHeaderCellType and SortMode

        /// <summary>
        /// Returns the AutoFilter header cell type. This property hides the 
        /// non-virtual DefaultHeaderCellType property inherited from the 
        /// DataGridViewBand class. The inherited property is set in the 
        /// DataGridViewAutoFilterTextBoxColumn constructor. 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Type DefaultHeaderCellType
        {
            get { return typeof(DataGridViewAutoFilterColumnHeaderCell); }
        }

        /// <summary>
        /// Gets or sets the sort mode for the column and prevents it from being 
        /// set to Automatic, which would interfere with the proper functioning 
        /// of the drop-down button. This property hides the non-virtual 
        /// DataGridViewColumn.SortMode property from the designer. The inherited 
        /// property is set in the DataGridViewAutoFilterTextBoxColumn constructor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false)]
        [DefaultValue(DataGridViewColumnSortMode.Programmatic)]
        public new DataGridViewColumnSortMode SortMode
        {
            get { return base.SortMode; }
            set
            {
                if (value == DataGridViewColumnSortMode.Automatic)
                {
                    throw new InvalidOperationException("A SortMode value of Automatic is incompatible with " + "the DataGridViewAutoFilterColumnHeaderCell type. " + "Use the AutomaticSortingEnabled property instead.");
                }
                else
                {
                    base.SortMode = value;
                }
            }
        }

        #endregion

        #region public properties: FilteringEnabled, AutomaticSortingEnabled, DropDownListBoxMaxLines

        /// <summary>
        /// Gets or sets a value indicating whether filtering is enabled for this column. 
        /// </summary>
        [DefaultValue(true)]
        public Boolean FilteringEnabled
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).FilteringEnabled;
            }
            set
            {
                // Set the header-cell property. 
                ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).FilteringEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic sorting is enabled for this column. 
        /// </summary>
        [DefaultValue(true)]
        public Boolean AutomaticSortingEnabled
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).AutomaticSortingEnabled;
            }
            set
            {
                // Set the header-cell property.
                ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).AutomaticSortingEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum height of the drop-down filter list for this column. 
        /// </summary>
        [DefaultValue(20)]
        public Int32 DropDownListBoxMaxLines
        {
            get
            {
                // Return the header-cell value.
                return ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).DropDownListBoxMaxLines;
            }
            set
            {
                // Set the header-cell property.
                ((DataGridViewAutoFilterColumnHeaderCell)HeaderCell).DropDownListBoxMaxLines = value;
            }
        }

        #endregion public properties

        #region public, static, convenience methods: RemoveFilter and GetFilterStatus

        /// <summary>
        /// Removes the filter from the BindingSource bound to the specified DataGridView. 
        /// </summary>
        /// <param name="dataGridView">The DataGridView bound to the BindingSource to unfilter.</param>
        public static void RemoveFilter(DataGridView dataGridView)
        {
            DataGridViewAutoFilterColumnHeaderCell.RemoveFilter(dataGridView);
        }

        /// <summary>
        /// Gets a status string for the specified DataGridView indicating the 
        /// number of visible rows in the bound, filtered BindingSource, or 
        /// String.Empty if all rows are currently visible. 
        /// </summary>
        /// <param name="dataGridView">The DataGridView bound to the 
        /// BindingSource to return the filter status for.</param>
        /// <returns>A string in the format "x of y records found" where x is 
        /// the number of rows currently displayed and y is the number of rows 
        /// available, or String.Empty if all rows are currently displayed.</returns>
        public static String GetFilterStatus(DataGridView dataGridView)
        {
            return DataGridViewAutoFilterColumnHeaderCell.GetFilterStatus(dataGridView);
        }

        #endregion

    }


    public class DataGridViewRatingCell : DataGridViewTextBoxCell
    {
        private static GraphicsPath star = new GraphicsPath();
        private static GraphicsPath[] stars = new GraphicsPath[5];
        private static LinearGradientBrush[] brushes = new LinearGradientBrush[5];
        private static Point center;
        private static int R;
        private static int r;
        private int currentValue = -1;
        private bool mouseOver;
 
        static DataGridViewRatingCell()
        {
            //Init star            
            List<PointF> points = new List<PointF>();
            bool largeArc = true;
            R = 10 + starOffset;
            r = 4;
            center = new Point(R, R);

            for (float alpha = 90; alpha <= 414; alpha += 36)
            {
                int d = largeArc ? R : r;
                double radAlpha = alpha * Math.PI / 180;
                float x = (float)(d * Math.Cos(radAlpha));
                float y = (float)(d * Math.Sin(radAlpha));
                points.Add(new PointF(center.X + x, center.Y + y));
                largeArc = !largeArc;
            }

            star.AddPolygon(points.ToArray());
            star.Transform(new Matrix(1, 0, 0, -1, 0, center.Y * 2));
            //Init stars
            UpdateBrushes(1);
        }

        public DataGridViewRatingCell()
        {
            ValueType = typeof(int);
            ratedStarColor = Color.Green;
            grayStarColor = Color.LightGray;
            starScale = 0.9F;
            starOffset = 0;
            UseColumnStarColor = true;
            UseColumnStarScale = true;
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return 0;
            }
        }

        internal static void UpdateBrushes(float scale)
        {
            int space = 2 * R;
            for (int i = 0; i < 5; i++)
            {
                if (stars[i] != null) stars[i].Dispose();
                stars[i] = (GraphicsPath)star.Clone();
                stars[i].Transform(new Matrix(scale, 0, 0, scale, space * i * scale, 0));
                brushes[i] = CreateBrush(new RectangleF(center.X - R + space * i * scale, center.Y - R, R * 2 * scale, R * 2 * scale));
            }
        }

        private static LinearGradientBrush CreateBrush(RectangleF bounds)
        {
            var brush = new LinearGradientBrush(bounds, Color.White, Color.Yellow, LinearGradientMode.ForwardDiagonal);
            ColorBlend cb = new ColorBlend();
            Color c = Color.Green;
            Color lightColor = Color.White;
            cb.Colors = new Color[] { c, c, lightColor, c, c };
            cb.Positions = new float[] { 0, 0.4f, 0.5f, 0.6f, 1 };
            brush.InterpolationColors = cb;
            return brush;
        }

        private void AdjustBrushColors(LinearGradientBrush brush, Color baseColor, Color lightColor)
        {
            //Note how we adjust the colors, using brush.InterpolationColors directly won't work.
            ColorBlend cb = brush.InterpolationColors;
            cb.Colors = new Color[] { baseColor, baseColor, lightColor, baseColor, baseColor };
            brush.InterpolationColors = cb;
        }    

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue,
            string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts & ~DataGridViewPaintParts.SelectionBackground & ~DataGridViewPaintParts.ContentForeground);
            
            if (rowIndex == RowIndex && (paintParts & DataGridViewPaintParts.ContentForeground) != 0)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (Value != null) Value = Math.Min(Math.Max(0, (int)Value), 5);
                if (!mouseOver) currentValue = (int)(Value ?? 0);
                PaintStars(graphics, cellBounds, 0, currentValue, true);
                PaintStars(graphics, cellBounds, currentValue, 5 - currentValue, false);
                graphics.SmoothingMode = SmoothingMode.Default;
            }
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!mouseOver) 
                mouseOver = true;
            if (IsReadOnly())
                return;
            
            var lastStar = stars.Select((x, i) => new { x, i })
                .LastOrDefault(x => x.x.IsVisible(e.Location));
            
            if (lastStar != null)
            {
                currentValue = lastStar.i + 1;
                DataGridView.Cursor = Cursors.Hand;
            }
            else if (RowIndex > -1)
            {
                currentValue = (int)(Value ?? 0);
                DataGridView.Cursor = Cursors.Default;
            }

            DataGridView.InvalidateCell(this);
        }

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);

            if (IsReadOnly()) 
                return;

            Value = currentValue == 1 && (int?)Value == 1 ? 0 : currentValue;
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            mouseOver = false;
            if (IsReadOnly()) 
                return;
            
            if (rowIndex == RowIndex)
            {
                currentValue = (int)(Value ?? 0);
                DataGridView.InvalidateCell(this);
            }
        }

        private bool IsReadOnly()
        {
            var col = OwningColumn as DataGridViewRatingColumn;
            return col != null ? col.ReadOnly : false;
        }

        private void PaintStars(Graphics g, Rectangle bounds, int startIndex, int count, bool rated)
        {
            GraphicsState gs = g.Save();
            // g.TranslateTransform(bounds.Left, bounds.Top);
            g.TranslateTransform(bounds.Left + starOffset, bounds.Top);
            var col = OwningColumn as DataGridViewRatingColumn;
            // some weird shit going on here hurts my head
            Color ratedColor = col == null ? Color.Yellow : UseColumnStarColor ? col.RatedStarColor : RatedStarColor;
            Color grayColor = col == null ? Color.LightGray : UseColumnStarColor ? col.GrayStarColor : GrayStarColor;
            float starScale = col == null ? 1 : UseColumnStarScale ? col.StarScale : StarScale;
            UpdateBrushes(starScale);
            for (int i = startIndex; i < startIndex + count; i++)
            {
                AdjustBrushColors(brushes[i], rated ? ratedColor : grayColor, rated ? Color.White : grayColor);
                g.FillPath(brushes[i], stars[i]);
                //g.DrawPath(Pens.Green, stars[i]);
            }
            g.Restore(gs);
        }

        Color ratedStarColor;
        Color grayStarColor;
        float starScale;
        static int starOffset;

        public Color RatedStarColor
        {
            get { return ratedStarColor; }
            set
            {
                if (ratedStarColor != value)
                {
                    ratedStarColor = value;
                    var col = OwningColumn as DataGridViewRatingColumn;
                    if (col != null && col.RatedStarColor != value)
                    {
                        UseColumnStarColor = false;
                        DataGridView.InvalidateCell(this);
                    }
                }
            }
        }

        public Color GrayStarColor
        {
            get { return grayStarColor; }
            set
            {
                if (grayStarColor != value)
                {
                    grayStarColor = value;
                    var col = OwningColumn as DataGridViewRatingColumn;
                    if (col != null && col.GrayStarColor != value)
                    {
                        UseColumnStarColor = false;
                        DataGridView.InvalidateCell(this);
                    }
                }
            }
        }

        //Change the star size via scaling factor (default by 1)
        public float StarScale
        {
            get { return starScale; }
            set
            {
                if (starScale != value)
                {
                    starScale = value;
                    var col = OwningColumn as DataGridViewRatingColumn;
                    if (col != null && col.StarScale != value)
                    {
                        UseColumnStarScale = false;
                        DataGridView.InvalidateCell(this);
                    }
                }
            }
        }

        public bool UseColumnStarColor { get; set; }
        public bool UseColumnStarScale { get; set; }
    }

}