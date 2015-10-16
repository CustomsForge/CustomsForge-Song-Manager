using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CustomsForgeManager.CustomsForgeManagerLib.CustomControls.DataGridEditors
{
    class ComboBoxEditingControl : ComboBox, IDataGridViewEditingControl
    {

        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public ComboBoxEditingControl()
            : base()
        {

            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SelectedValueChanged += (s, e) =>
            {
                valueChanged = true;
            };
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.SelectedValue;
            }
            set
            {
                this.SelectedValue = value;
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            //
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }
    }
}
