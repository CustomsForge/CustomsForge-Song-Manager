using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CFSM.GenTools;
using RocksmithToolkitLib.DLCPackage;

namespace CustomsForgeSongManager.SongEditor
{
    public partial class ucArrangements : DLCPackageEditorControlBase //, IAttributesEditor
    {
        //TODO: allow users to create new tones.

        public List<Arrangement> NewArrangement = new List<Arrangement>();

        public ucArrangements()
        {
            InitializeComponent();
            dgvArrangements.AutoGenerateColumns = false;
            BackColor = dgvArrangements.BackgroundColor;
        }

        public override void DoInit()
        {
            if (SongData == null)
                return;

            SongData.Arrangements.ForEach(t => NewArrangement.Add(t.XmlClone()));
            dgvArrangements.DataSource = NewArrangement;

            // black on blue is not readable on devs CRT monitor   
            // use standard custom selection (highlighting) color
            dgvArrangements.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            dgvArrangements.DefaultCellStyle.SelectionForeColor = dgvArrangements.DefaultCellStyle.ForeColor;
            dgvArrangements.ClearSelection();
        }

        public override void Save()
        {
            if (!Dirty)
                return;
            SongData.Arrangements.Clear();
            SongData.Arrangements.AddRange(NewArrangement);
            base.Save();
        }

        private bool EditArrangement(Arrangement arrangement)
        {
            if (arrangement == null)
                return false;
            //if ((arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight || arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal))
            //  return false;

            using (var form = new frmArrangement(arrangement, this) {Text = "Edit Arrangement"})
            {
                form.EditMode = !(arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight || arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal);
                form.StartPosition = FormStartPosition.CenterParent;
                return form.ShowDialog() == DialogResult.OK;
            }
        }

        private void dgvArrangements_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.dgvArrangements.Columns["colName"].Index)
            {
                e.Cancel = true;
                return;
            }
            var arrangement = (Arrangement) dgvArrangements.Rows[e.RowIndex].DataBoundItem;
            if (arrangement != null && (arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight || arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal))
                e.Cancel = true;
        }

        private void dgvArrangements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.dgvArrangements.Columns["colName"].Index)
            {
                var arrangement = (Arrangement) dgvArrangements.Rows[e.RowIndex].DataBoundItem;
                if (EditArrangement(arrangement))
                {
                    this.dgvArrangements.Refresh();
                    this.Dirty = true;
                }
            }
        }

        private void dgvArrangements_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != this.dgvArrangements.Columns["colName"].Index)
                this.Dirty = true;
        }
    }
}