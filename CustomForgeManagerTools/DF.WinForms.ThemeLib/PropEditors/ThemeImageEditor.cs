
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;

namespace DF.WinForms.ThemeLib.PropEditors
{
    public class ThemeImageEditor : UITypeEditor
    {
        public static void UTILS_INIT()
        {
            TypeDescriptor.AddAttributes(typeof(ThemeImage),
                new EditorAttribute(typeof(ThemeImageEditor), typeof(UITypeEditor)));
        }

        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value != null && value.GetType() == typeof(ThemeImage))
            {
                ThemeImage ti = (ThemeImage)value;
                using (ImageDialog id = new ImageDialog())
                {
                    if (ti.Image != null)
                        id.Image = ti.Image;
                    if (id.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        ti.SetImage(id.Image);
                }
            }
            return value;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value != null)
            {
                ThemeImage ti = (ThemeImage)e.Value;               
                if (ti != null && ti.Image != null)
                {
                    Rectangle bounds = e.Bounds;
                    int num = bounds.Width;
                    bounds.Width = num - 1;
                    num = bounds.Height;
                    bounds.Height = num - 1;                    
                    e.Graphics.DrawRectangle(SystemPens.WindowFrame, bounds);
                    e.Graphics.DrawImage(ti.Image, e.Bounds);
                }
            }
        }

        // Indicates whether the UITypeEditor supports painting a 
        // representation of a property's value.
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

     
}
