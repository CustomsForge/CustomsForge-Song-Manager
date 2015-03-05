using CustomsForgeManager_Winforms.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.Controls
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
                    foreach (var item in sorted)
                    {
                        if (item != null)
                        {
                            this.InvokeIfRequired(delegate
                            {
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