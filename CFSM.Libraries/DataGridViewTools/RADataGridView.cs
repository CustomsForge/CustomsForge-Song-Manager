using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            // instantiate an instance of the object from the dataObject's remote assembly
            var dataObj = dataObject.ToString();
            var moduleName = dataObject.GetType().GetProperty("Module").GetValue(dataObject, null);
            var remoteAssembly = Assembly.LoadFrom(moduleName.ToString());
            var handle = Activator.CreateInstance(remoteAssembly.ToString(), dataObj);
            var obj = handle.Unwrap();

            string s = String.Empty;
            if (frmCustomFilter.EditCustomFilter(ref s,
                obj.GetType().GetProperty(ColumnName)))
                return "Expression:" + s;
            return string.Empty;
        }

        public bool CanFilter(object dataObject, string ColumnName)
        {
            // test bed for debugging reflection (save)
            // instantiate an instance of the object from the caller assembly
            //var currentAssembly = Assembly.GetExecutingAssembly();
            //var callerAssemblies = new StackTrace().GetFrames()
            //    .Select(x => x.GetMethod().ReflectedType.Assembly)
            //    .Distinct().Where(x => x.GetReferencedAssemblies()
            //        .Any(y => y.FullName == currentAssembly.FullName));
            //var initialAssembly = callerAssemblies.Last();

            //foreach (var prop in dataObject.GetType().GetProperties())
            //{
            //    try
            //    {
            //        Debug.WriteLine("{0} = {1}", prop.Name, prop.GetValue(dataObject, null));
            //    }
            //    catch (Exception)
            //    {
            //        Console.WriteLine("Error: " + prop);
            //    }
            //}

            // instantiate an instance of the object from the dataObject's remote assembly
            var dataObj = dataObject.ToString();
            var moduleName = dataObject.GetType().GetProperty("Module").GetValue(dataObject, null);
            var remoteAssembly = Assembly.LoadFrom(moduleName.ToString());
            var handle = Activator.CreateInstance(remoteAssembly.ToString(), dataObj);
            var obj = handle.Unwrap();

            // TODO: get custom filter to work with Enums, i.e. p.PropertyType.IsEnum
            // determine the property type
            var p = obj.GetType().GetProperty(ColumnName);
            if (p != null)
            {
                if (p.PropertyType == typeof(string) ||
                    p.PropertyType == typeof(int) ||
                    p.PropertyType == typeof(double) ||
                    p.PropertyType == typeof(float))
                    return true;
            }
            return false;
        }
    }

    [Serializable]
    public class ColumnOrderItem
    {
        public string ColumnName { get; set; }
        public string HeaderText { get; set; }
        public int DisplayIndex { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public int ColumnIndex { get; set; }
    }

    [Serializable]
    public class RADataGridViewSettings
    {
        // ver 1.1 - added column HeaderText to DataGridViewTools column settings
        public const string gridViewSettingsVersion = "1.1";

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

    public static class RAExtensions
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
            if (columnOrderCollection.Count == 0)
                return;

            DataGridViewColumnCollection orgDgvColumns = raDataGridView.Columns;
            var sorted = columnOrderCollection.OrderBy(i => i.DisplayIndex);

            // smooth column swapping operation when equal
            var debugHere1 = sorted.Count();
            var debugHere2 = raDataGridView.Columns.Count;

            if (sorted.Count() == raDataGridView.Columns.Count)
            {
                foreach (var item in sorted)
                {
                    if (item == null)
                        continue;

                    raDataGridView.InvokeIfRequired(delegate
                        {
                            raDataGridView.Columns[item.ColumnIndex].Name = item.ColumnName;
                            raDataGridView.Columns[item.ColumnIndex].HeaderText = item.HeaderText;
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

        public static RADataGridViewSettings ManagerGridSettings { get; set; }

        public static RADataGridViewSettings SaveColumnOrder(DataGridView dgvCurrent)
        {
            var settings = new RADataGridViewSettings();

            if (String.IsNullOrEmpty(dgvCurrent.Name))
                return settings;

            var columns = dgvCurrent.Columns;
            if (columns.Count > 1)
            {
                Debug.WriteLine("Saving DataGridview Column Order: " + dgvCurrent.Name);
                try
                {
                    for (int i = 0; i < columns.Count; i++)
                        settings.ColumnOrder.Add(new ColumnOrderItem
                            {
                                ColumnIndex = i,
                                DisplayIndex = columns[i].DisplayIndex,
                                Visible = columns[i].Visible,
                                Width = columns[i].Width,
                                ColumnName = columns[i].Name,
                                HeaderText = columns[i].HeaderText
                            });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(String.Format("<Error>: {0}", ex.Message));
                }
            }

            return settings;
        }

    }
}