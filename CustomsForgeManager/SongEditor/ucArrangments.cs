using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls.DataGridEditors;
using CustomsForgeManager.CustomsForgeManagerLib;

namespace CustomsForgeManager.SongEditor
{
    public partial class ucArrangments : DLCPackageEditorControlBase
    {
        //TODO: validators and editors for capofret,tuningpitch,scrollspeed
        public ucArrangments()
        {
            InitializeComponent();
            gridArrangements.AutoGenerateColumns = false;
        }


        public frmSongEditor SongEditor
        {
            get;
            private set;
        }

        public ucArrangments(frmSongEditor ParentForm)
            : this()
        {
            SongEditor = ParentForm;
        }

        List<Arrangement> NewData = new List<Arrangement>();

        public override void DoInit()
        {
            base.DoInit();
            NewData = new List<Arrangement>();
            var oldData = SongData.Arrangements.Where(x =>
                    x.ArrangementType != RocksmithToolkitLib.Sng.ArrangementType.ShowLight).ToList();
            //create a copy of each object and add to the new datasource
            oldData.ForEach(sa => NewData.Add(sa.XmlClone()));
            gridArrangements.DataSource = NewData;

            for (int i = 0; i < gridArrangements.Rows.Count; i++)
            {
                var r = gridArrangements.Rows[i];
                var a = (Arrangement)r.DataBoundItem;
                switch (a.ArrangementType)
                {
                    case RocksmithToolkitLib.Sng.ArrangementType.Guitar:
                        r.DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                        // black on blue is not readable on devs CRT monitor   
                        // r.DefaultCellStyle.BackColor = Color.FromArgb(0x66, 0x66, 0xFF);
                        break;
                    case RocksmithToolkitLib.Sng.ArrangementType.Bass:
                        r.DefaultCellStyle.BackColor = Color.Lime;
                        // not readable on devs CRT monitor   
                        // r.DefaultCellStyle.BackColor = Color.FromArgb(0x33, 0xCC, 0x33);
                        break;
                    case RocksmithToolkitLib.Sng.ArrangementType.Vocal:
                        r.ReadOnly = true;
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                        // differentable from custom seletion color
                        //r.DefaultCellStyle.BackColor = Color.FromArgb(0xFF, 0xCC, 0x00);
                        break;
                }
            }
            // use standard custom selection (highlighting) color
            gridArrangements.DefaultCellStyle.SelectionBackColor = Color.Gold; // dgvSongs.DefaultCellStyle.BackColor;
            gridArrangements.DefaultCellStyle.SelectionForeColor = gridArrangements.DefaultCellStyle.ForeColor;
            gridArrangements.ClearSelection();
        }

        public override void Save()
        {
            SongData.Arrangements.Clear();
            SongData.Arrangements.AddRange(NewData);
            base.Save();
        }


        private void gridArrangements_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.Dirty = true;
        }
    }

    #region Grid Editors
    public class UArrangementCell : DataGridViewTextBoxCell
    {
        protected ucArrangments GetParentArrangement()
        {
            return (ucArrangments)DataGridView.Parent;
        }
    }

    public class EnumColumn<T> : DataGridViewColumn where T : struct, IConvertible
    {
        public EnumColumn()
            : base(new EnumCell<T>())
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(EnumCell<T>)))
                    throw new InvalidCastException("Must be a EnumCell<T>");
                base.CellTemplate = value;
            }
        }

    }

    public class EnumCell<T> : UArrangementCell where T : struct, IConvertible
    {
        public override void InitializeEditingControl(int rowIndex, object
        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
            ComboBoxEditingControl ctl = DataGridView.EditingControl as ComboBoxEditingControl;
            ctl.SelectedValueChanged -= ctl_SelectedValueChanged;
            ctl.Items.Clear();

            ctl.Items.AddRange(Enum.GetNames(typeof(T)));
            ctl.Text = Enum.GetName(typeof(T), this.Value);
            ctl.SelectedValueChanged += ctl_SelectedValueChanged;

        }


        void ctl_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBoxEditingControl ctl = (ComboBoxEditingControl)sender;
            string s = ctl.Text;
            if (string.IsNullOrEmpty(s))
            {
                this.DataGridView.CurrentCell.Value = null;
                return;
            }
            var x = Enum.Parse(typeof(T), s);
            if ((int)x != (int)this.DataGridView.CurrentCell.Value)
            {
                this.DataGridView.CurrentCell.Value = x;
                GetParentArrangement().Dirty = true;
                this.DataGridView.EndEdit();
            }

            // TEnum  x;
            // if (Enum.TryParse<T>(s, out x))
            // {



            //  }


            //if (NewTone != null && this.Value != NewTone)
            //{
            //    if (NewTone.Name == "[NONE]")
            //        this.DataGridView.CurrentCell.Value = null;
            //    else
            //        this.DataGridView.CurrentCell.Value = NewTone;

            //}
        }


        public override Type EditType
        {
            get
            {
                if (ReadOnly)
                    return null;
                return typeof(ComboBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(T);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return default(T);
            }
        }
    }


    public class RouteMaskColumn : EnumColumn<RouteMask> { }

    public class ArrangementTypeColumn : EnumColumn<RocksmithToolkitLib.Sng.ArrangementType> { }


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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(ToneCell)))
                    throw new InvalidCastException("Must be a ToneCell");
                base.CellTemplate = value;
            }
        }
    }

    public class ToneCell : UArrangementCell
    {

        public Tone2014 NoTone { get; private set; }

        public override void InitializeEditingControl(int rowIndex, object
        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);

            ComboBoxEditingControl ctl = DataGridView.EditingControl as ComboBoxEditingControl;
            ctl.SelectedValueChanged -= ctl_SelectedValueChanged;
            ctl.Items.Clear();
            ctl.Items.AddRange(GetAllTones());
            if (this.Value != null)
            {
                ctl.SelectedValue = this.Value;
            }
            else
                ctl.Text = String.Empty;

            ctl.SelectedValueChanged += ctl_SelectedValueChanged;
        }

        //public override bool ReadOnly
        //{
        //    get
        //    {
        //        var x = (Arrangement)DataGridView.Rows[RowIndex].DataBoundItem;
        //        if (x != null)
        //            if (x.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.Vocal || x.ArrangementType == RocksmithToolkitLib.Sng.ArrangementType.ShowLight)
        //                return true;
        //        return base.ReadOnly;
        //    }
        //    set
        //    {
        //        base.ReadOnly = value;
        //    }
        //}

        Tone2014[] GetAllTones()
        {
            if (NoTone == null)
                NoTone = new Tone2014() { Key = "[NONE]", Name = "[NONE]" };

            List<Tone2014> resultList = new List<Tone2014>();
            resultList.Add(NoTone);
            var cc = GetParentArrangement().SongEditor.GetEditorControl<ucTones>();
            if (cc != null)
                resultList.AddRange(cc.NewTonesRS2014);
            else
                resultList.AddRange(GetParentArrangement().SongData.TonesRS2014);
            return resultList.ToArray();
        }


        void ctl_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBoxEditingControl ctl = (ComboBoxEditingControl)sender;
            string s = ctl.Text;
            if (string.IsNullOrEmpty(s))
            {
                this.DataGridView.CurrentCell.Value = null;
                return;
            }

            var NewTone = GetAllTones().FirstOrDefault(x => x.ToString() == s);
            if (NewTone != null && this.Value != NewTone)
            {
                if (NewTone.Name == "[NONE]")
                    this.DataGridView.CurrentCell.Value = null;
                else
                    this.DataGridView.CurrentCell.Value = NewTone;

                this.DataGridView.NotifyCurrentCellDirty(true);
                this.DataGridView.EndEdit();
            }
        }

        public override Type EditType
        {
            get
            {
                if (ReadOnly)
                    return null;
                return typeof(ComboBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(Tone2014);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
    }


    #endregion



}
