using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataGridViewTools
{
    public class DgvStatus
    {
        //Usage:
        //GridUtility.SaveSorting(grid);    
        //grid.DataSource = databaseFetch(); // or whatever
        //GridUtility.RestoreSorting(grid);

        private ListSortDirection _oldSortOrder;
        private DataGridViewColumn _oldSortCol;

        /// <summary>
        /// Saves information about sorting column, to be restored later by calling RestoreSorting
        /// on the same DataGridView
        /// </summary>
        /// <param name="grid"></param>
        public void SaveSorting(DataGridView grid)
        {
            _oldSortCol = null;
            _oldSortOrder = grid.SortOrder == SortOrder.Ascending ?
                ListSortDirection.Ascending : ListSortDirection.Descending;
            _oldSortCol = grid.SortedColumn;
        }

        /// <summary>
        /// Restores column sorting and sort glpyh to a DataGridView.
        ///<para>Call this AFTER calling SaveSorting on the same DataGridView.</para>   
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="toogleSort">If TRUE toggles column sorting from Ascending to Desending and viseversa</param>
        public void RestoreSorting(DataGridView grid, bool toggleSort = false)
        {
            if (_oldSortCol != null)
            {
                if (toggleSort)
                {
                    _oldSortOrder = _oldSortOrder == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    DataGridViewColumn newCol = grid.Columns[_oldSortCol.Name];
                    grid.Sort(newCol, _oldSortOrder);
                }
                else
                {
                    DataGridViewColumn newCol = grid.Columns[_oldSortCol.Name];
                    grid.Sort(newCol, _oldSortOrder);
                }
            }
        }

        private static DgvStatus _instance;
        public static DgvStatus Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DgvStatus();

                return _instance;
            }
        }
    }

}
