using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace DF.WinForms.ThemeLib
{
    //theme setting for grid views
    [ThemeKey("Grid View")]
    public class GridViewThemeSetting : ThemeSetting, IDisposable
    {
        public void Dispose()
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
            if (ColumnHeaderFont != null)
            {
                ColumnHeaderFont.Dispose();
                ColumnHeaderFont = null;
            }
        }

        public override void ResetSettings()
        {
            ColumnHeaderHeight = 23;
            SelectionBackColor = Color.Gold;
            SelectionForeColor = Color.Black;
            GridColor = SystemColors.Window;
            EnableHeadersVisualStyles = true;
            CellBackColor = SystemColors.Control;
            CellForeColor = SystemColors.WindowText;
            AlternatingBackColor = SystemColors.ControlDark;
            AlternatingForeColor = SystemColors.WindowText;
            Font = new Font("Arial", 8);
            ColumnHeaderFont = new Font("Arial", 8);
            ColumnHeaderBackColor = SystemColors.Control;
            ColumnHeaderBackColor2 = SystemColors.Control;

        }

        [DefaultValue(23)]
        public int ColumnHeaderHeight { get; set; }
        public Color ColumnHeaderBackColor { get; set; }
        public Color ColumnHeaderBackColor2 { get; set; }
        public Color ColumnHeaderForeColor { get; set; }
        public Font ColumnHeaderFont { get; set; }
        public Color CellBackColor { get; set; }
        public Color CellForeColor { get; set; }
        public Color SelectionBackColor { get; set; }
        public Color SelectionForeColor { get; set; }
        public Color AlternatingBackColor { get; set; }
        public Color AlternatingForeColor { get; set; }
        public Color GridColor { get; set; }
        public Font Font { get; set; }
        [DefaultValue(true)]
        public bool EnableHeadersVisualStyles { get; set; }
    }

    public static class GridViewThemeExtension
    {
        public static void SetTheme(this System.Windows.Forms.DataGridView gridView, Theme theme)
        {            
            gridView.BackgroundColor = theme.ControlColor;
            GridViewThemeSetting gvs = theme.GetThemeSetting<GridViewThemeSetting>();
            if (gvs != null)
            {
                gridView.EnableHeadersVisualStyles = gvs.EnableHeadersVisualStyles;
                gridView.GridColor = gvs.GridColor;

                gridView.ColumnHeadersHeight = gvs.ColumnHeaderHeight;
                gridView.ColumnHeadersDefaultCellStyle.BackColor = gvs.ColumnHeaderBackColor;
                gridView.ColumnHeadersDefaultCellStyle.ForeColor = gvs.ColumnHeaderForeColor;
                gridView.ColumnHeadersDefaultCellStyle.Font = gvs.ColumnHeaderFont;
                

                gridView.DefaultCellStyle.SelectionBackColor = gvs.SelectionBackColor;
                gridView.DefaultCellStyle.SelectionForeColor = gvs.SelectionForeColor;
                gridView.DefaultCellStyle.ForeColor = gvs.CellForeColor;
                gridView.DefaultCellStyle.BackColor = gvs.CellBackColor;
                gridView.DefaultCellStyle.Font = gvs.Font;

                gridView.AlternatingRowsDefaultCellStyle.BackColor = gvs.AlternatingBackColor;
                gridView.AlternatingRowsDefaultCellStyle.ForeColor = gvs.AlternatingForeColor;

            }

        }
    }

}
