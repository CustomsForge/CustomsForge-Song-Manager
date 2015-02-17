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
        protected override void Dispose(bool disposing)
        {
            SaveColumnOrder();
            base.Dispose(disposing);
        }


        private void SaveColumnOrder()
        {
            //string path = Constants.DefaultWorkingDirectory + "\\settings.bin";
            //if (this.AllowUserToOrderColumns)
            //{
            //    List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
            //    DataGridViewColumnCollection columns = this.Columns;
            //    RADataGridViewSettings settings = new RADataGridViewSettings();

            //    for (int i = 0; i < columns.Count; i++)
            //    {
            //        columnOrder.Add(new ColumnOrderItem
            //        {
            //            ColumnIndex = i,
            //            DisplayIndex = columns[i].DisplayIndex,
            //            Visible = columns[i].Visible,
            //            Width = columns[i].Width
            //        });
            //    }
            //}
        }

        public void LoadColumnOrder()
        {
            string path = Constants.DefaultWorkingDirectory + "\\settings.bin";
            List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
            RADataGridViewSettings settings = new RADataGridViewSettings();

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                RADataGridViewSettings deserialized = fs.DeSerialize() as RADataGridViewSettings;
                if (deserialized != null)
                {
                    try
                    {
                        if (deserialized.ColumnOrder[this.Name] != null)
                        {
                            columnOrder = deserialized.ColumnOrder[this.Name];
                            var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
                            foreach (var item in sorted)
                            {
                                this.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                                this.Columns[item.ColumnIndex].Visible = item.Visible;
                                this.Columns[item.ColumnIndex].Width = item.Width;
                            }
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                }
                fs.Close();
            }
        }

        public void LoadColumnOrder(Dictionary<string, List<ColumnOrderItem>> ColumnOrderCollection)
        {
            string path = Constants.DefaultWorkingDirectory + "\\settings.bin";
            List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
            try
            {
                columnOrder = ColumnOrderCollection[this.Name];
                var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
                foreach (var item in sorted)
                {
                    if (item != null)
                    {
                        this.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                        this.Columns[item.ColumnIndex].Visible = item.Visible;
                        this.Columns[item.ColumnIndex].Width = item.Width;
                    }
                }
            }
            catch (KeyNotFoundException)
            {
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}