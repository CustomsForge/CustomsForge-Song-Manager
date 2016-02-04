using System;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using DF.WinForms.ThemeLib;

namespace CustomsForgeSongManager.UITheme
{
    public partial class ThemeDesigner : Form
    {
        private CFSMTheme theme;

        public ThemeDesigner()
        {
            InitializeComponent();
            theme = new CFSMTheme();
            if (!this.DesignMode)
                refreshTheme();
        }

        private void refreshTheme()
        {
            listBox1.BeginUpdate();
            try
            {
                listBox1.Items.Clear();
                listBox1.Items.Add(new ValueObject("Theme", theme));
                var objs = theme.AllThemeSettings();
                foreach (var obj in objs)
                {
                    string caption = obj.Key;
                    if (caption.ToLower().EndsWith("themesetting"))
                        caption = caption.Remove(caption.Length - 12);
                    listBox1.Items.Add(new ValueObject(obj.Key, obj));
                }
            }
            finally
            {
                listBox1.EndUpdate();
            }
            listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ValueObject v = (ValueObject) listBox1.SelectedItem;
                propertyGrid1.SelectedObject = v.Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var sd = new SaveFileDialog() {InitialDirectory = Constants.ThemeDirectory, AddExtension = true, Filter = String.Format("CFSM Theme Files (*{0})|*{0}", Theme.GetThemeExt<CFSMTheme>()), FileName = theme.ThemeName + Theme.GetThemeExt<CFSMTheme>()})
            {
                if (sd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    theme.SaveToFile(sd.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Globals.CFMTheme.BeginPreview();
            try
            {
                using (ThemePreview tm = new ThemePreview(theme))
                    tm.ShowDialog();
                theme.RemoveListeners();
            }
            finally
            {
                Globals.CFMTheme.EndPreview();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            using (var od = new OpenFileDialog() {InitialDirectory = Constants.ThemeDirectory, Filter = String.Format("CFSM Theme Files (*{0})|*{0}|All Files|*.*", Theme.GetThemeExt<CFSMTheme>())})
            {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    theme.LoadFromFile(od.FileName);
                    refreshTheme();
                }
            }
        }
    }

    internal class ValueObject
    {
        private string name;
        public object Value { get; set; }

        public ValueObject(string name, object value)
        {
            this.name = name;
            this.Value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}