using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using GenTools;
using RocksmithToolkitLib.DLCPackage;
using Globals = CustomsForgeSongManager.DataObjects.Globals;
using System;
using System.Xml.Serialization;
using Newtonsoft.Json;


namespace CustomsForgeSongManager.SongEditor
{
    public partial class ucArrangements : DLCPackageEditorControlBase //, IAttributesEditor
    {
        //TODO: allow users to create new tones.
        public List<LocalArrangement> LocalArrangements = new List<LocalArrangement>();
        public List<Arrangement> NewArrangements = new List<Arrangement>();

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

            LocalArrangements.Clear();
            NewArrangements.Clear();
            SongData.Arrangements.ForEach(t => NewArrangements.Add(t.XmlClone()));

            // ScrollSpeed as used by Rocksmith is 10X the CFSM ScrollSpeed
            // using reflection to convert it so it displays properly in CFSM
            foreach (var newArrangement in NewArrangements)
            {
                var localArrangement = new LocalArrangement();
                localArrangement = newArrangement.CopyTo(localArrangement);
                LocalArrangements.Add(localArrangement);
            }

            dgvArrangements.DataSource = LocalArrangements;

            // use standard custom selection (highlighting) color
            dgvArrangements.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            dgvArrangements.DefaultCellStyle.SelectionForeColor = dgvArrangements.DefaultCellStyle.ForeColor;
            dgvArrangements.ClearSelection();
        }

        public override void Save()
        {
            if (!Dirty)
                return;

            NewArrangements.Clear();
            SongData.Arrangements.Clear();

            // CFSM SrollSpeed is 1/10 of the toolkit ScrollSpeed 
            // using reflection to convert it so it displays properly
            foreach (var localArrangement in LocalArrangements)
            {
                var newArrangement = new Arrangement();
                newArrangement = localArrangement.CopyTo(newArrangement);
                NewArrangements.Add(newArrangement);
            }

            SongData.Arrangements.AddRange(NewArrangements);
            base.Save();
        }

        private bool EditArrangement(Arrangement arrangement)
        {
            if (arrangement == null)
                return false;
            //if ((arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight || arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal))
            //  return false;

            using (var form = new frmArrangement(arrangement, this) { Text = "Edit Arrangement" })
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
            var arrangement = (Arrangement)dgvArrangements.Rows[e.RowIndex].DataBoundItem;
            if (arrangement != null && (arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight || arrangement.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal))
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

        private void dgvArrangements_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var dgv = (DataGridView)sender;
            var dgvName = dgv.Name;
            var colNdx = e.ColumnIndex;
            var colHeader = dgv.Columns[colNdx].HeaderText;
            var colProperty = dgv.Columns[colNdx].DataPropertyName;
            var rowNdx = e.RowIndex;
            var rowName = dgv.Rows[rowNdx].Cells[0].Value.ToString();

            if (dgv.Rows[e.RowIndex].IsNewRow)
                return;

            if (rowName == "ShowLights")
            {
                dgv.Rows[rowNdx].Cells["colScrollSpeed"].Value = 0;
                return;
            }

            // validate user entry
            if (colProperty == "ScrollSpeedTenth")
            {
                double d;
                if (double.TryParse(e.FormattedValue.ToString(), out d))
                {
                    if (d < 0.5 || d > 4.5)
                    {
                        e.Cancel = true;
                        Globals.DebugLog(String.Format("<ERROR> (Row: {0}, Col: {1}), Please enter a value between 0.5 and 4.5 inclusive ...", rowName, colHeader));
                        this.HaltOnError = true;
                    }
                    else
                        this.HaltOnError = false;
                }
                else
                    this.HaltOnError = true;
            }
        }

        private void dgvArrangements_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Globals.DebugLog(String.Format("<ERROR> (Row: {0}, Col: {1}), {2} ...", e.RowIndex, e.ColumnIndex, e.Exception.Message));
            e.Cancel = true;
        }

        public class LocalArrangement : Arrangement
        {
            // the toolkit stores ScrollSpeed as an integer
            // what is being displayed in CFSM as ScrollSpeed is
            // actually the DynamicVisualDensity which is 1/10 the
            // value of the integer value of ScrollSpeed

            public double ScrollSpeedTenth
            {
                get
                {
                    return Math.Round(((double)ScrollSpeed / 10), 1);
                }
                set
                {
                    ScrollSpeed = (int)(Math.Round(value, 1) * 10);
                }
            }
        }

    }
}