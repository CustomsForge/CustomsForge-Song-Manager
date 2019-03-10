using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Data;

namespace DataGridViewTools
{
    public static class DgvExtensions
    {
        [Obfuscation(Exclude = false, Feature = "-rename")]
        public enum TristateSelect : byte
        {
            NotSelected = 0,
            Selected = 1,
            All = 2
        }

        /// <summary>
        /// Get the data object of first selected (highlighted) row
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dgvCurrent"></param>
        /// <returns></returns>
        public static T GetObjectFromFirstSelectedRow<T>(DataGridView dgvCurrent)
        {
            if (dgvCurrent.SelectedRows.Count > 0)
                return GetObjectFromRow<T>(dgvCurrent, dgvCurrent.SelectedRows[0].Index);

            return default(T);
        }

        public static T GetObjectFromRow<T>(DataGridView dgvCurrent, int rowIndex)
        {
            var debugMe = dgvCurrent.Name;

            if (rowIndex == -1)
                return default(T);

            return (T)dgvCurrent.Rows[rowIndex].DataBoundItem;
        }

        public static T GetObjectFromRow<T>(DataGridViewRow dataGridViewRow)
        {
            return (T)dataGridViewRow.DataBoundItem;
        }

        public static List<T> GetObjectsFromRows<T>(DataGridView dgvCurrent, TristateSelect selected = TristateSelect.Selected, string dataPropertyName = "Selected")
        {
            List<T> selectedObjects = new List<T>();

            // determine DataProperty Selected column index
            int colNdx = GetDataPropertyColumnIndex(dgvCurrent, dataPropertyName);

            // the checkbox value changes but not detected here (known VS issue)
            // DO NOT USE - 'row.Selected' returns incorrect value  
            // so added check of row cell value which returns correct value
            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                var sd = GetObjectFromRow<T>(row);

                switch (selected)
                {
                    case TristateSelect.NotSelected:
                        if (sd != null && !(bool)row.Cells[colNdx].Value)
                            selectedObjects.Add(sd);
                        break;

                    case TristateSelect.Selected:
                        if (sd != null && (bool)row.Cells[colNdx].Value)
                            selectedObjects.Add(sd);
                        break;

                    case TristateSelect.All:
                        if (sd != null)
                            selectedObjects.Add(sd);
                        break;
                }
            }

            //if (selectedObjects.Count == 0 && selected == TristateSelect.Selected)
            //    selectedObjects.Add(GetObjectFromFirstSelectedRow<T>(dgvCurrent));

            return selectedObjects;
        }

        public static int GetDataPropertyColumnIndex(DataGridView dgvCurrent, string dataPropertyName)
        {
            // determine DataProperty column index
            int colNdx = -1;
            for (int i = 0; i < dgvCurrent.Columns.Count; i++)
            {
                //var debugHere = dgvCurrent.Columns[i].DataPropertyName;
                if (dgvCurrent.Columns[i].DataPropertyName == dataPropertyName)
                {
                    colNdx = i;
                    break;
                }
            }

            if (colNdx == -1)
                throw new Exception(dgvCurrent.Name + " column DataProperty '" + dataPropertyName + "' can not be found.");

            return colNdx;
        }

        // may reduce DataGridView flickering
        public static void DoubleBuffered(DataGridView dgv, bool setting = true)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            // switch off/on or on/off to activate
            pi.SetValue(dgv, !setting, null);
            pi.SetValue(dgv, setting, null);
        }

        public static void RowsAllNone(DataGridView dgvCurrent, string dataPropertyName = "Selected")
        {
            var colNdxSelected = GetDataPropertyColumnIndex(dgvCurrent, dataPropertyName);
            // use condition of first row to determine selection state
            var selected = Convert.ToBoolean(dgvCurrent.Rows[0].Cells[colNdxSelected].Value);

            foreach (DataGridViewRow row in dgvCurrent.Rows)
                row.Cells[colNdxSelected].Value = !selected;

            dgvCurrent.Refresh();
        }

        public static void RowsToggle(DataGridView dgvCurrent, string dataPropertyName = "Selected")
        {
            var colNdxSelected = GetDataPropertyColumnIndex(dgvCurrent, dataPropertyName);

            foreach (DataGridViewRow row in dgvCurrent.Rows)
                row.Cells[colNdxSelected].Value = !Convert.ToBoolean(row.Cells[colNdxSelected].Value);

            dgvCurrent.Refresh();
        }

        public static void RowsCheckboxValue(DataGridView dgvCurrent, bool value, string dataPropertyName = "Selected")
        {
            var colNdxSelected = GetDataPropertyColumnIndex(dgvCurrent, dataPropertyName);

            foreach (DataGridViewRow row in dgvCurrent.Rows)
                row.Cells[colNdxSelected].Value = value;

            dgvCurrent.Refresh();
        }

        public static int RowsSelectedCount(DataGridView dgvCurrent, string dataPropertyName = "Selected")
        {
            var colNdxSelected = GetDataPropertyColumnIndex(dgvCurrent, dataPropertyName);
            return dgvCurrent.Rows.Cast<DataGridViewRow>().Count(r => Convert.ToBoolean(r.Cells[colNdxSelected].Value));
        }

    }
}