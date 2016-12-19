using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using GenTools;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class ucTones : DLCPackageEditorControlBase //, IAttributesEditor
    {
        //TODO: allow users to create new tones.

        public List<Tone2014> NewTonesRS2014 = new List<Tone2014>();

        public ucTones()
        {
            InitializeComponent();
            dgvTones.AutoGenerateColumns = false;
            this.BackColor = dgvTones.BackgroundColor;
        }

        public override void DoInit()
        {
            if (SongData == null)
                return;
            SongData.TonesRS2014.ForEach(t => NewTonesRS2014.Add(t.XmlClone()));
            dgvTones.DataSource = NewTonesRS2014;

            // black on blue is not readable on devs CRT monitor   
            // use standard custom selection (highlighting) color
            dgvTones.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            dgvTones.DefaultCellStyle.SelectionForeColor = dgvTones.DefaultCellStyle.ForeColor;
            dgvTones.ClearSelection();
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
            frmTone f = new frmTone() { Tone = tone, StartPosition = FormStartPosition.CenterParent };
            return f.ShowDialog(this.ParentForm) == DialogResult.OK;
        }

        private void raDataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.dgvTones.Columns["colName"].Index)
                e.Cancel = true;
        }

        private void raDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.dgvTones.Columns["colName"].Index)
            {
                if (EditTone((Tone2014)dgvTones.Rows[e.RowIndex].DataBoundItem))
                {
                    this.dgvTones.Refresh();
                    this.Dirty = true;
                }
            }
        }

        private void raDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.dgvTones.Columns["colName"].Index)
                this.Dirty = true;
        }
    }
}