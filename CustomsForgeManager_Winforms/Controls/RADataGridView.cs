using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.lib;

namespace CustomsForgeManager_Winforms.Controls
{
    class RADataGridView : DataGridView
    {
        public void ReLoadColumnOrder(List<ColumnOrderItem> ColumnOrderCollection)
        {
            int errNdx = 0;
            try
            {
                if (ColumnOrderCollection != null)
                {
                    if (Columns.Count > 1)
                    {
                        var sorted = ColumnOrderCollection.OrderBy(i => i.DisplayIndex);

                        // alt method to catch error
                        // if (sorted.Count() != Columns.Count)
                        //    return;

                        foreach (var item in sorted)
                        {
                            if (item != null)
                            {
                                // for debugging
                                // Console.WriteLine(item.DisplayIndex + " : " + item.Visible + " : " + item.Width);

                                this.InvokeIfRequired(delegate
                                    {
                                        Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                                        Columns[item.ColumnIndex].Visible = item.Visible;
                                        Columns[item.ColumnIndex].Width = item.Width;
                                    });
                            }
                            ++errNdx; // for debugging
                        }
                    }
                }
            }
            catch (Exception)
            {
                // usually cause by calling method before dgv has columns
                Console.WriteLine("Failure at Item.ColumnIndex: " + errNdx);
            }

        }
    }
}