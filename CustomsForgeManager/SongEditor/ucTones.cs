using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using System.Collections.Generic;
using RocksmithToolkitLib.DLCPackage.Manifest2014;
using System.Linq;
using System;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucTones : DLCPackageEditorControlBase, IAttributesEditor
    {

        public ucTones()
        {
            InitializeComponent();
            raDataGridView1.AutoGenerateColumns = false;
        }

        public override void DoInit()
        {
            if (SongData == null)
                return;
            raDataGridView1.DataSource = SongData.TonesRS2014;
        }


        public void EditSongAttributes(Attributes2014 attr)
        {
            if (!Dirty)
                return;
            foreach (var t in SongData.TonesRS2014)
            {
                if (attr.Tones.Any(tt => tt.Key == t.Key))
                {
                    attr.Tones.RemoveAll(atone => atone.Key == t.Key);
                    attr.Tones.Add(t);
                }
            }
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
