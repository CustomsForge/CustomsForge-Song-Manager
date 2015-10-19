using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls.DataGridEditors;
using CustomsForgeManager.CustomsForgeManagerLib;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucArrangements : DLCPackageEditorControlBase//, IAttributesEditor
    {
        //TODO: allow users to create new tones.

        public List<Arrangement> NewArrangement = new List<Arrangement>();

        public ucArrangements()
        {
            InitializeComponent();
            dgvArrangements.AutoGenerateColumns = false;
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

            using (var form = new frmArrangement(arrangement, this, GameVersion.RS2014) { Text = "Edit Arrangement" })
            {
                form.EditMode = true;
                form.StartPosition = FormStartPosition.CenterParent;

                if (DialogResult.OK != form.ShowDialog())
                    return false;

                return true;
            }
        }

        private void dgvArrangements_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == this.dgvArrangements.Columns["colName"].Index)
                e.Cancel = true;
        }

        private void dgvArrangements_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.dgvArrangements.Columns["colName"].Index)
            {
                var arrangement = (Arrangement)dgvArrangements.Rows[e.RowIndex].DataBoundItem;
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
