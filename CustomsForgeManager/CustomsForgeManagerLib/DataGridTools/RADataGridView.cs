﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DF.WinForms.ThemeLib;
using System.Drawing;
using System.Drawing.Drawing2D;
using DataGridViewTools;
using CustomsForgeManager.CustomsForgeManagerLib.UITheme;

namespace CustomsForgeManager.CustomsForgeManagerLib.DataGridTools
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
            if (ColumnOrderCollection != null && ColumnOrderCollection.Count != 0 && Columns.Count > 1)
            {
                DataGridViewColumnCollection orgDgvColumns = Columns;
                var sorted = ColumnOrderCollection.OrderBy(i => i.DisplayIndex);

                // smooth column swapping operation when equal
                if (sorted.Count() == Columns.Count)
                    foreach (var item in sorted)
                    {
                        if (item == null)
                            continue;

                        this.InvokeIfRequired(delegate
                            {
                                Columns[item.ColumnIndex].Name = item.ColumnName;
                                Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                                Columns[item.ColumnIndex].Visible = item.Visible;
                                Columns[item.ColumnIndex].Width = item.Width;
                            });
                    }

                //if (orgDgvColumns.Contains(item.ColumnName))
                //    orgDgvColumns.Remove(item.ColumnName);

                    //if (orgDgvColumns.Count > 0)
                    //    foreach (DataGridViewColumn customColumn in orgDgvColumns)
                    //        Columns.Add(customColumn);
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
                    e.Graphics.FillRectangle(new LinearGradientBrush(e.CellBounds, gvs.ColumnHeaderBackColor,
                        gvs.ColumnHeaderBackColor2, LinearGradientMode.Vertical), e.CellBounds);
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
            string s = "";
            if (CustomsForgeManager.Forms.frmCustomFilter.EditCustomFilter(ref s,
                typeof(SongData).GetProperty(ColumnName)))
                return "Expression:" + s;
            return string.Empty;
        }

        public bool CanFilter(string ColumnName)
        {
            var p = typeof(SongData).GetProperty(ColumnName);
            if (p != null)
            {
                if (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) ||
                    p.PropertyType == typeof(double) ||
                    p.PropertyType == typeof(float))
                    return true;
                //todo: Enums
            }
            return false;
        }
    }
}