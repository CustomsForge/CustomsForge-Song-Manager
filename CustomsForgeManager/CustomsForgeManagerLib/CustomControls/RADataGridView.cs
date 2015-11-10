using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DF.WinForms.ThemeLib;
using System.Drawing;
using System.Drawing.Drawing2D;
using DataGridViewTools;
using CustomsForgeManager.CustomsForgeManagerLib.UITheme;

namespace CustomsForgeManager.CustomsForgeManagerLib.CustomControls
{
    class RADataGridView : DataGridView, IThemeListener, IGridViewFilterStyle, IGridViewCustomFilter
    {
        Theme theme;
        Image FFilteredImg;
        Image FNormalImg;

        public RADataGridView()
        {
            FFilteredImg = DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(true);
            FNormalImg = DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(false);
        }

        public void ReLoadColumnOrder(List<ColumnOrderItem> ColumnOrderCollection)
        {
            if (ColumnOrderCollection != null)
            {
                if (Columns.Count > 1)
                {
                    var sorted = ColumnOrderCollection.OrderBy(i => i.DisplayIndex);

                    if (sorted.Count() != Columns.Count)
                        return;

                    foreach (var item in sorted)
                    {
                        if (item != null)
                        {
                            this.InvokeIfRequired(delegate
                            {
                                Columns[item.ColumnIndex].Name = item.ColumnName;
                                Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                                Columns[item.ColumnIndex].Visible = item.Visible;
                                Columns[item.ColumnIndex].Width = item.Width;
                            });
                        }
                    }
                }
            }
        }

        public void ApplyTheme(Theme sender)
        {
            theme = sender;
            this.SetTheme(sender);
            ThemeColumnSettings cs = sender.GetThemeSetting<ThemeColumnSettings>();
            if (cs != null)
            {
                FFilteredImg = cs.FilterOn.Image ?? FFilteredImg;
                FNormalImg = cs.FilterOff.Image ?? FNormalImg;
            }
        }


        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && theme != null && Columns[e.ColumnIndex].Visible)
            {
                GridViewThemeSetting gvs = theme.GetThemeSetting<GridViewThemeSetting>();
                if (gvs != null)
                {
                    e.Graphics.FillRectangle(new LinearGradientBrush(e.CellBounds,gvs.ColumnHeaderBackColor,
                        gvs.ColumnHeaderBackColor2,LinearGradientMode.Vertical), e.CellBounds);
                    e.Graphics.DrawRectangle(new Pen(theme.BorderColor), e.CellBounds);
                    e.PaintContent(e.ClipBounds);
                    e.Handled = true;
                }
            } 
            base.OnCellPainting(e);
        }

        public Image filteredImg
        {
            get { return FFilteredImg; }
        }

        public Image normalImg
        {
            get { return FNormalImg; }
        }


        public string GetCustomFilter(string ColumnName)
        {
            return string.Empty;
        }


        public bool CanFilter(string ColumnName)
        {
            return false;


            var p = typeof(SongData).GetProperty(ColumnName);
            if (p != null)
            {
                if (p.PropertyType == typeof(string))
                    return true;
            }

            //var x = Columns[ColumnName];
            //if (x != null)
            //{
            // //   x.DataPropertyName
            //    DataSource


            //}


            return false;
        }
    }
}