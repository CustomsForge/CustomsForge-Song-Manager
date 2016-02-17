//------------------------------------------------------------------------------ 
// <copyright file="DataGridViewComboBoxEditingControl.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//----------------------------------------------------------------------------- 

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DataGridViewTools
{
    public class ComboBoxEditingControl : ComboBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;
        private int rowIndex;

        public ComboBoxEditingControl() : base()
        {
            this.TabStop = false;
        }

        // IDataGridViewEditingControl interface implementation 
        public virtual DataGridView EditingControlDataGridView
        {
            get { return this.dataGridView; }
            set { this.dataGridView = value; }
        }

        public virtual object EditingControlFormattedValue
        {
            get { return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting); }
            set
            {
                string valueStr = value as string;
                if (valueStr != null)
                {
                    this.Text = valueStr;
                    if (String.Compare(valueStr, this.Text, true, CultureInfo.CurrentCulture) != 0)
                    {
                        this.SelectedIndex = -1;
                    }
                }
            }
        }

        public virtual int EditingControlRowIndex
        {
            get { return this.rowIndex; }
            set { this.rowIndex = value; }
        }

        public virtual bool EditingControlValueChanged
        {
            get { return this.valueChanged; }
            set { this.valueChanged = value; }
        }

        public virtual Cursor EditingPanelCursor
        {
            get { return Cursors.Default; }
        }

        public virtual bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 255)
            {
                // Our ComboBox does not support transparent back colors 
                Color opaqueBackColor = Color.FromArgb(255, dataGridViewCellStyle.BackColor);
                this.BackColor = opaqueBackColor;
                this.dataGridView.EditingPanel.BackColor = opaqueBackColor;
            }
            else
            {
                this.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.ForeColor = dataGridViewCellStyle.ForeColor;
        }

        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            if ((keyData & Keys.KeyCode) == Keys.Down || (keyData & Keys.KeyCode) == Keys.Up || (this.DroppedDown && ((keyData & Keys.KeyCode) == Keys.Escape) || (keyData & Keys.KeyCode) == Keys.Enter))
            {
                return true;
            }
            return !dataGridViewWantsInputKey;
        }

        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        public virtual void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                SelectAll();
            }
        }

        private void NotifyDataGridViewOfValueChange()
        {
            this.valueChanged = true;
            this.dataGridView.NotifyCurrentCellDirty(true);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (this.SelectedIndex != -1)
            {
                NotifyDataGridViewOfValueChange();
            }
        }
    }
}