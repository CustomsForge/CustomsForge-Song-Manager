using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucArrangments : DLCPackageEditorControlBase
    {
        public ucArrangments()
        {
            InitializeComponent();
            gridArrangements.AutoGenerateColumns = false;
        }

        public override void DoInit()
        {
            base.DoInit();
            gridArrangements.DataSource = SongData.Arrangements.Where(x => x.ArrangementType != RocksmithToolkitLib.Sng.ArrangementType.ShowLight).ToList();
        }
    }


    public class ToneColumn : DataGridViewColumn
    {
        public ToneColumn()
            : base(new ToneCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(ToneCell)))
                {
                    throw new InvalidCastException("Must be a ToneCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class ToneCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object
        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            //ComboBox ctl =
            //    DataGridView.EditingControl as ComboBoxEditingControl;
            //// Use the default row value when Value property is null.
            //if (this.Value == null)
            //{
            //   // ctl.Value = (DateTime)this.DefaultNewRowValue;
            //}
            //else
            //{
            //  //  ctl.Value = (DateTime)this.Value;
            //}
        }

        public override Type EditType
        {
            get
            {
                return typeof(ComboBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return "";
            }
        }
    }

    class ComboBoxEditingControl : ComboBox, IDataGridViewEditingControl
    {

        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

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
