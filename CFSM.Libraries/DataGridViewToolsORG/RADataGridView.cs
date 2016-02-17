using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace DataGridViewTools
{
    public class RADataGridView : DataGridView, IGridViewFilterStyle, IGridViewCustomFilter
    {
        public RADataGridView()
        {
            FilteredImg = DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(true);
            NormalImg = DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(false);
        }

        public Image FilteredImg { get; private set; }
        public Image NormalImg { get; private set; }

        public string GetCustomFilter(object dataObject, string ColumnName)
        {
            // instantiate an instance of the object from the caller assembly
            var currentAssembly = Assembly.GetExecutingAssembly();
            var callerAssemblies = new StackTrace().GetFrames()
                .Select(x => x.GetMethod().ReflectedType.Assembly)
                .Distinct().Where(x => x.GetReferencedAssemblies()
                    .Any(y => y.FullName == currentAssembly.FullName));
            var initialAssembly = callerAssemblies.Last();
            var dataObj = dataObject.ToString();
            var handle = Activator.CreateInstance(initialAssembly.ToString(), dataObj);
            var obj = handle.Unwrap();

            string s = String.Empty;
            if (frmCustomFilter.EditCustomFilter(ref s,
                obj.GetType().GetProperty(ColumnName)))
                return "Expression:" + s;
            return string.Empty;
        }

        public bool CanFilter(object dataObject, string ColumnName)
        {
            // instantiate an instance of the object from the caller assembly
            var currentAssembly = Assembly.GetExecutingAssembly();
            var callerAssemblies = new StackTrace().GetFrames()
                .Select(x => x.GetMethod().ReflectedType.Assembly)
                .Distinct().Where(x => x.GetReferencedAssemblies()
                    .Any(y => y.FullName == currentAssembly.FullName));
            var initialAssembly = callerAssemblies.Last();  
            var dataObj = dataObject.ToString();
            var handle = Activator.CreateInstance(initialAssembly.ToString(), dataObj);
            var obj = handle.Unwrap();
            
            // determine the property type
            var p = obj.GetType().GetProperty(ColumnName);            
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

    [Serializable]
    public class ColumnOrderItem
    {
        public string ColumnName { get; set; }
        public int DisplayIndex { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public int ColumnIndex { get; set; }
    }

    [Serializable]
    public class RADataGridViewSettings
    {
        public const string gridViewSettingsVersion = "1.0";

        [XmlIgnore]
        public string LoadedVersion { get; private set; }

        [XmlAttribute]
        public string GridViewSettingsVersion
        {
            get { return gridViewSettingsVersion; }
            set { this.LoadedVersion = value; }
        }

        private List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();

        public List<ColumnOrderItem> ColumnOrder
        {
            get { return columnOrder as List<ColumnOrderItem>; }
            set { columnOrder = value; }
        }
    }

    public static class RAExtension
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
                c.Invoke(new Action(() => action(c)));
            else
                action(c);
        }

        //useage: radgv.ReLoadColumnOrder(dgvSongPacks, AppSettings.Instance.ManagerGridSettings.ColumnOrder);
        public static void ReLoadColumnOrder(this RADataGridView raDataGridView, List<ColumnOrderItem> columnOrderCollection)
        {
            DataGridViewColumnCollection orgDgvColumns = raDataGridView.Columns;
            var sorted = columnOrderCollection.OrderBy(i => i.DisplayIndex);

            // smooth column swapping operation when equal
            if (sorted.Count() == raDataGridView.Columns.Count)
            {
                foreach (var item in sorted)
                {
                    if (item == null)
                        continue;

                    raDataGridView.InvokeIfRequired(delegate
                        {
                            raDataGridView.Columns[item.ColumnIndex].Name = item.ColumnName;
                            raDataGridView.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
                            raDataGridView.Columns[item.ColumnIndex].Visible = item.Visible;
                            raDataGridView.Columns[item.ColumnIndex].Width = item.Width;
                        });
                }
            }

            //if (orgDgvColumns.Contains(item.ColumnName))
            //    orgDgvColumns.Remove(item.ColumnName);

            //if (orgDgvColumns.Count > 0)
            //    foreach (DataGridViewColumn customColumn in orgDgvColumns)
            //        Columns.Add(customColumn);
        }
    }
}