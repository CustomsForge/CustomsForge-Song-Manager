using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace DataGridViewTools
{
    public static class DgvConversion
    {
        public static DataTable DataGridViewToDataTable(this DataGridView dgv, bool ignoreHiddenColumns = false)
        {
            if (dgv.ColumnCount == 0)
                return null;

            DataTable dtSource = new DataTable();
            DataColumn idCol = new DataColumn("rowId", typeof(int));
            idCol.AutoIncrement = true;
            dtSource.Columns.Add(idCol);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (ignoreHiddenColumns & !col.Visible)
                    continue;
                if (col.Name == string.Empty)
                    continue;

                if (col.ValueType == null)
                {
                    if (col.CellType.Name == "DataGridViewCheckBoxCell")
                        col.ValueType = typeof(bool);
                    else if (col.CellType.Name == "DataGridViewTextBoxCell" || col.CellType.Name == "DataGridViewAutoFilterTextBoxColumn")
                        col.ValueType = typeof(string);
                    else if (col.CellType.Name == "DataGridViewLinkCell")
                        col.ValueType = typeof(string);
                    else if (col.CellType.Name == "DataGridViewImageCell")
                        col.ValueType = typeof(Bitmap);
                    else if (col.CellType.Name == "DataGridViewRatingCell")
                        col.ValueType = typeof(int);
                    else
                        throw new DataException("Column ValueType is not defined: " + col.CellType.Name);
                }

                // proper handling of nullables
                dtSource.Columns.Add(col.Name, Nullable.GetUnderlyingType(col.ValueType) ?? col.ValueType);
                dtSource.Columns[col.Name].Caption = col.HeaderText;
            }

            if (dtSource.Columns.Count == 0)
                return null;

            try
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.ErrorText.Length > 0)
                        break;

                    DataRow drNewRow = dtSource.NewRow();
                    foreach (DataColumn dtCol in dtSource.Columns)
                    {
                        if (dtCol.DataType == typeof(bool) && row.Cells[dtCol.ColumnName].Value == null)
                            row.Cells[dtCol.ColumnName].Value = false;

                        // skip over rowId and proper handling of nullables
                        if (dtCol.ColumnName != "rowId")
                            drNewRow[dtCol.ColumnName] = row.Cells[dtCol.ColumnName].Value ?? DBNull.Value;
                    }

                    dtSource.Rows.Add(drNewRow);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return dtSource;
        }

        public static DataGridView DataTableToDataGridView(this DataTable dt)
        {
            // does not play well with an existing dgv because of type mismatches
            var dgv = new DataGridView();
            dgv.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource { DataSource = dt };
            dgv.DataSource = bs;

            return dgv;
        }

        public static DataGridView ListToDataGridView<T>(this T obj)
        {
            var dgv = new DataGridView();
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = obj;

            return dgv;
        }

        public static List<T> DataGridViewToList<T>(this DataGridView dgv) where T : new()
        {
            var dataList = new List<T>();

            // alt method ... could be used to get specific row
            //for (int i = 0; i < dgv.Rows.Count; i++)
            //    dataList.Add((T)dgv.Rows[i].DataBoundItem);

            dataList = ((IEnumerable)dgv.DataSource).OfType<T>().ToList();

            return dataList;
        }

        /// <summary>
        /// Convert a List to a DataTable
        /// </summary>
        /// <param name="data">Class Object List</param>
        /// <param name="columns">Specify column names to include (
        /// e.g. new string[] { "Artist", "Title", "Album" } or defaults to all columns if 'null'</param>
        public static DataTable ListToDataTable<T>(this List<T> data, string[] columns)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            if (columns == null)
            {
                columns = new string[0];
                columns[0] = "ALL_COLUMNS";
            }

            foreach (var column in columns)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    if (prop.Name == column || columns[0] == "ALL_COLUMNS")
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                        else
                            table.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            object[] values;
            if (columns[0] == "ALL_COLUMNS")
                values = new object[props.Count];
            else
                values = new object[columns.Length];

            foreach (T item in data)
            {
                int n = 0;
                foreach (var column in columns)
                {
                    for (int i = 0; i < props.Count; i++)
                    {
                        PropertyDescriptor prop = props[i];
                        if (prop.Name == column || columns[0] == "ALL_COLUMNS")
                        {
                            values[n] = props[i].GetValue(item);
                            n++;
                        }
                    }
                }

                table.Rows.Add(values);
            }
            return table;
        }

        public static DataTable ConvertList<T>(IEnumerable<T> objectList)
        {
            Type type = typeof(T);
            var typeproperties = type.GetProperties();

            DataTable list2DataTable = new DataTable();
            foreach (PropertyInfo propInfo in typeproperties)
            {
                list2DataTable.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
            }

            foreach (T listItem in objectList)
            {
                object[] values = new object[typeproperties.Length];
                for (int i = 0; i < typeproperties.Length; i++)
                {
                    values[i] = typeproperties[i].GetValue(listItem, null);
                }

                list2DataTable.Rows.Add(values);
            }

            return list2DataTable;
        }
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// Usage: List<Doctor> doctors = dtDoctors.ToList<Doctor>();
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="dataTable">DataTable </param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable dataTable) where T : new()
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var dataList = new List<T>();
            var dtColumnNames = (dataTable.Columns.Cast<DataColumn>().Select(aHeader => new { Name = aHeader.ColumnName, Type = aHeader.DataType })).ToList();
            var objFieldNames = (typeof(T).GetProperties(flags).Cast<PropertyInfo>().Select(aProp => new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType })).ToList();
            //var intersectNames = objFieldNames.Intersect(dtColumnNames).ToList();
            //var unionNames = objFieldNames.Union(dtColumnNames).ToList();

            if (dtColumnNames.Count > objFieldNames.Count)
                throw new DataException("DataTable Columns > Object Fields");

            // ------------------------------------------
            //
            // datatable/datagridview column header names 
            // MUST match exactly the object field names
            //
            // ------------------------------------------

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new T();
                foreach (var aField in dtColumnNames)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    if (propertyInfos == null)
                        continue;

                    object value = dataRow[aField.Name] == DBNull.Value ? null : dataRow[aField.Name].ToString();
                    if (value == null)
                        continue;

                    try
                    {
                        if (propertyInfos.PropertyType == typeof(string))
                            value = dataRow[aField.Name].ToString();
                        if (propertyInfos.PropertyType == typeof(bool))
                            value = !dataRow[aField.Name].ToString().ToLower().Contains("false");
                        if (propertyInfos.PropertyType == typeof(int))
                            value = int.Parse(dataRow[aField.Name].ToString());
                        if (propertyInfos.PropertyType == typeof(long))
                            value = long.Parse(dataRow[aField.Name].ToString());
                        if (propertyInfos.PropertyType == typeof(double))
                            value = double.Parse(dataRow[aField.Name].ToString());
                        if (propertyInfos.PropertyType == typeof(float))
                            value = float.Parse(dataRow[aField.Name].ToString());
                        if (propertyInfos.PropertyType == typeof(DateTime))
                            value = DateTime.Parse(dataRow[aField.Name].ToString());
                        try
                        {
                            propertyInfos.SetValue(aTSource, value, null);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("SetValue Exception" + Environment.NewLine + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(propertyInfos.PropertyType + Environment.NewLine + ex.Message);
                    }

                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        /// <summary>
        /// BETA METHOD ONLY: NEEDS FURTHER TESTING/DEBUGGING
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public static DataGridView DataTableToDGV(this DataTable dataTable, DataGridView dgv)
        {
            var dtColumnNames = (dataTable.Columns.Cast<DataColumn>().Select(aHeader => new { Name = aHeader.ColumnName, Type = aHeader.DataType })).ToList();
            var dgvColumnNames = (dgv.Columns.Cast<DataGridViewColumn>().Select(aHeader => new { Name = aHeader.HeaderText, Type = aHeader.ValueType })).ToList();
            var dtColumnCount = dtColumnNames.Count;
            var dgvColumnCount = dgvColumnNames.Count;
            var dtRowCount = dataTable.Rows.Count;

            if (dtColumnCount > dgvColumnCount)
                throw new Exception("Error: datatable has more columns than the datagridview");

            var dgvOutput = dgv;
            dgvOutput.DataSource = null;

            for (int dtRowIndex = 0; dtRowIndex < dtRowCount; dtRowIndex++)
            {
                dgvOutput.Rows.Add();
                int rowIndex = dgvOutput.RowCount - 1;
                var dgvRow = dgvOutput.Rows[rowIndex];

                // var dtRowList = dataTable.Rows[dtRowIndex].ItemArray.Select(x => x.ToString()).ToList();
                var dtRow = dataTable.Rows[dtRowIndex];


                for (int dtColIndex = 0; dtColIndex < dtColumnCount; dtColIndex++)
                {
                    var rcValue = dtRow[dtColIndex];
                    var rcName = dtColumnNames[dtColIndex].Name;

                    for (int dgvColNdx = 0; dgvColNdx < dgvColumnCount; dgvColNdx++)
                    {
                        if (dgvColumnNames[dgvColNdx].Name == rcName)
                        {
                            if (dgvColumnNames[dgvColNdx].Type == typeof(string))
                            {
                                dgvRow.Cells[dgvColNdx].Value = rcValue;
                                break;
                            }

                            if (dgvColumnNames[dgvColNdx].Type == typeof(bool))
                            {
                                var boolValue = Convert.ToBoolean(rcValue);
                                dgvRow.Cells[dgvColNdx].Value = boolValue;
                                break;
                            }
                        }
                    }
                }
            }

            return dgvOutput;
        }


        ///// <summary>
        ///// Converts a DataTable to a list with generic objects
        ///// Usage: List<Doctor> doctors = dtDoctors.ToList<Doctor>();
        ///// </summary>
        ///// <typeparam name="T">Generic object</typeparam>
        ///// <param name="dataTable">DataTable </param>
        ///// <returns>List with generic objects</returns>
        //public static List<T> DataTableToList<T>(this DataTable dataTable) where T : new()
        //{
        //    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        //    var dataList = new List<T>();
        //    var dtColumnNames = (dataTable.Columns.Cast<DataColumn>().Select(aHeader => new { Name = aHeader.ColumnName, Type = aHeader.DataType })).ToList();
        //    var objFieldNames = (typeof(T).GetProperties(flags).Cast<PropertyInfo>().Select(aProp => new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType })).ToList();
        //    var commonFields = objFieldNames.Intersect(dtColumnNames).ToList();

        //    if (dtColumnNames.Count > objFieldNames.Count)
        //        throw new DataException("DataTable Columns > Object Fields");

        //    Debug.WriteLine("StopHere");

        //    foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
        //    {
        //        var aTSource = new T();
        //        foreach (var aField in commonFields)
        //        {
        //            PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
        //            var value = (dataRow[aField.Name] == DBNull.Value) ? null : dataRow[aField.Name]; //if database field is nullable
        //            propertyInfos.SetValue(aTSource, value, null);
        //        }
        //        dataList.Add(aTSource);
        //    }
        //    return dataList;
        //}



    }
}