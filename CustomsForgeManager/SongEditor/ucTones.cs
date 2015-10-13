﻿using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using System.Collections.Generic;
using System.Linq;
using System;
using CustomsForgeManager.CustomsForgeManagerLib;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucTones : DLCPackageEditorControlBase//, IAttributesEditor
    {
        public List<Tone2014> NewTonesRS2014 = new List<Tone2014>();

        public ucTones()
        {
            InitializeComponent();
            raDataGridView1.AutoGenerateColumns = false;
        }

        
        public override void DoInit()
        {
            if (SongData == null)
                return;
            SongData.TonesRS2014.ForEach(t => NewTonesRS2014.Add((Tone2014)Extensions.XmlDeserialize(t.XmlSerialize(), typeof(Tone2014))));
            raDataGridView1.DataSource = NewTonesRS2014;
        }

        public override void Save()
        {
            if (!Dirty)
                return;
            SongData.TonesRS2014.Clear();
            SongData.TonesRS2014.AddRange(NewTonesRS2014);
            base.Save();
        }


        private bool EditTone(Tone2014 tone)
        {
            ToneControlDialog f = new ToneControlDialog()
            {
                Tone = tone,
                StartPosition = FormStartPosition.CenterParent
            };
            return f.ShowDialog(this.ParentForm) == DialogResult.OK;
        }

        private void raDataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.raDataGridView1.Columns["colName"].Index)
                e.Cancel = true;
        }

        private void raDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.raDataGridView1.Columns["colName"].Index)
            {
                var tone = (Tone2014)raDataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (EditTone(tone))
                {
                    this.raDataGridView1.Refresh();
                    this.Dirty = true;
                }
            }
        }

        private void raDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.raDataGridView1.Columns["colName"].Index)
                this.Dirty = true;
        }

    }


}
