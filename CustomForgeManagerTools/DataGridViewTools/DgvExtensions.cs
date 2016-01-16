using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataGridViewTools
{
    public static class DgvExtensions
    {
        public enum TristateSelect : byte
        {
            NotSelected = 0,
            Selected = 1,
            All = 2
        }

        public static T GetObjectFromFirstSelectedRow<T>(DataGridView dgvCurrent)
        {
            if (dgvCurrent.SelectedRows.Count > 0)
                return GetObjectFromRow<T>(dgvCurrent, dgvCurrent.SelectedRows[0].Index);

            return default(T);
        }

        public static T GetObjectFromRow<T>(DataGridView dgvCurrent, int rowIndex)
        {
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

            // checkbox value changes but not detected here (known VS issue)
            // so added extra check of row cell value
            foreach (DataGridViewRow row in dgvCurrent.Rows)
            {
                var sd = GetObjectFromRow<T>(row);

                switch (selected)
                {
                    case TristateSelect.NotSelected:
                        if (sd != null && (!row.Selected && !(bool)row.Cells[colNdx].Value))
                            selectedObjects.Add(sd);
                        break;

                    case TristateSelect.Selected:
                        if (sd != null && (row.Selected || (bool)row.Cells[colNdx].Value))
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
                var stophere = dgvCurrent.Columns[i].DataPropertyName;
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

    }
}
