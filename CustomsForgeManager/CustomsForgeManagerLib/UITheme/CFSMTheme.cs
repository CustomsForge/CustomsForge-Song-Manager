using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DF.WinForms.ThemeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace CustomsForgeManager.CustomsForgeManagerLib.UITheme
{
    [ThemeFileVersion("1.0"), ThemeFileExtension(".cfsmtheme")]
    public class CFSMTheme : Theme
    {
        public CFSMTheme()
            : base()
        {
            ThemeDirectory = Constants.ThemeDirectory;
            if (!Directory.Exists(ThemeDirectory))
                Directory.CreateDirectory(ThemeDirectory);
        }

        public static void InitializeDgvAppearance(DataGridView dgvTheme)
        {
            // set all columns to read only except colSelect
            foreach (DataGridViewColumn col in dgvTheme.Columns)
                col.ReadOnly = true;

            dgvTheme.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.LightSteelBlue };
            dgvTheme.AllowUserToAddRows = false; // removes empty row at bottom
            dgvTheme.AllowUserToDeleteRows = false;
            dgvTheme.AllowUserToOrderColumns = true;
            dgvTheme.AllowUserToResizeColumns = true;
            dgvTheme.AllowUserToResizeRows = false;
            dgvTheme.AutoGenerateColumns = false; // true to display all columns, overrides Visible setting
            dgvTheme.BackgroundColor = SystemColors.AppWorkspace;
            dgvTheme.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // required when using FilteredBindingList
            dgvTheme.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgvTheme.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            // set custom selection (highlighting) color
            dgvTheme.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvLook.DefaultCellStyle.BackColor; // or removes selection highlight
            dgvTheme.DefaultCellStyle.SelectionForeColor = dgvTheme.DefaultCellStyle.ForeColor;
            // this overrides any user ability to make changes 
            // dgvLook.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvTheme.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTheme.EnableHeadersVisualStyles = true;
            dgvTheme.Font = new Font("Arial", 8);
            dgvTheme.GridColor = SystemColors.ActiveCaption;
            dgvTheme.MultiSelect = false;
            // dgvTheme.Name = "dgvLook";
            dgvTheme.RowHeadersVisible = false; // remove row arrow
            dgvTheme.ScrollBars = ScrollBars.Both;
            dgvTheme.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // always visible on restart in case user changed
            if (dgvTheme.Columns["colSelect"] != null)
            {
                dgvTheme.Columns["colSelect"].ReadOnly = false; // is overridden by EditProgrammatically
                dgvTheme.Columns["colSelect"].Visible = true;
                dgvTheme.Columns["colSelect"].Width = 50;
            }

            if (dgvTheme.Columns["colEnabled"] != null)
            {
                dgvTheme.Columns["colEnabled"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTheme.Columns["colEnabled"].Width = 72;
            }

            // prevents double line headers on filtered columns
            if (dgvTheme.Columns["colKey"] != null)
            {
                dgvTheme.Columns["colKey"].Width = 95;
                dgvTheme.Columns["colKey"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            if (dgvTheme.Columns["colAppID"] != null)
            {
                dgvTheme.Columns["colAppID"].Width = 80;
                dgvTheme.Columns["colAppID"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // must come before AutoResizeColumns
            dgvTheme.Visible = true;
            dgvTheme.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            dgvTheme.Refresh();
            // dgvTheme.ClearSelection(); // removes selection highlighting
        }

        // may reduce DataGridView flickering
        public static void DoubleBuffered(DataGridView dgv, bool setting = true)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            
            // switch off/on or on/off to activate
            pi.SetValue(dgv, !setting, null);
            pi.SetValue(dgv, setting, null);
        }

    }
}
