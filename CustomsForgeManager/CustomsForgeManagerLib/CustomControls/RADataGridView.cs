using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;

namespace CustomsForgeManager.CustomsForgeManagerLib.CustomControls
{
    class RADataGridView : DataGridView
    {
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


    }
}