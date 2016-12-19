using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using GenTools;

namespace DF.WinForms.ThemeLib
{
    public class ThemedForm : Form, IThemeListener
    {
        [Browsable(false)]
        public Theme Theme { get; protected set; }

        public ThemedForm()
            : base()
        {
            AutoAddListeners = true;
        }

        public ThemedForm(Theme theme)
            : this()
        {
            this.Theme = theme;
            theme.AddListener(this);
            ToolStripManager.VisualStylesEnabled = false;
            ToolStripManager.Renderer = new ThemeToolStripRenderer(theme);
        }

        /*  var frm = Form.ActiveForm;
        using (var bmp = new Bitmap(Width, Height)) {
            frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp.Save(@"c:\temp\screenshot.png");
        }*/

        [DefaultValue(true)]
        public bool AutoAddListeners { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Theme != null)
                Theme.ApplyTheme();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (AutoAddListeners && Theme != null)
            {
                if (e.Control is IThemeListener)
                    Theme.AddListener(e.Control as IThemeListener);
                var c = e.Control.FindAllChildrenByType<IThemeListener>().ToArray();
                if (c.Length > 0)
                    Theme.AddListeners(c);

                var tsc = e.Control.FindAllChildrenByType<ToolStrip>().ToList();
                if (tsc.Count > 0)
                {
                    var r = new ThemeToolStripRenderer(Theme);
                    tsc.ForEach(ts =>
                    {
                        var tsi = ts.Items.Cast<ToolStripItem>().Where(x => x is IThemeListener);
                        if (tsi.Count() > 0)
                            foreach (var t in tsi)
                                Theme.AddListener(t as IThemeListener);
                        if (Theme.Enabled)
                            ts.SetTheme(Theme);
                    });
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (AutoAddListeners && Theme != null)
            {
                if (e.Control is IThemeListener)
                    Theme.RemoveListener(e.Control as IThemeListener);
                var c = e.Control.FindAllChildrenByType<IThemeListener>().ToArray();
                if (c.Length > 0)
                    Theme.RemoveListeners(c);

                var tsc = e.Control.FindAllChildrenByType<ToolStrip>().ToList();
                if (tsc.Count > 0)
                    tsc.ForEach(ts =>
                    {
                        var tsi = ts.Items.Cast<ToolStripItem>().Where(x => x is IThemeListener);
                        if (tsi.Count() > 0)
                            foreach (var t in tsi)
                                Theme.RemoveListener(t as IThemeListener);
                    });
            }
            base.OnControlRemoved(e);
        }

        public virtual void ApplyTheme(Theme sender)
        {
            Theme = sender;
            if (Theme.Enabled)
            {
                this.SetTheme(sender);
                this.FindAllChildrenByType<Control>().ToList().ForEach(
                    c =>
                    {
                        if (Theme.Enabled)
                            c.SetTheme(sender);
                        if (c is GroupBox)
                        {
                            var gbox = (GroupBox)c;
                            gbox.Paint -= GBPaint;
                            if (Theme.Enabled)
                                gbox.Paint += GBPaint;

                        }
                    }
                );
            }
        }

        private void GBPaint(object o, PaintEventArgs p)
        {
            var box = (GroupBox)o;
            if (box != null)
            {
                Brush textBrush = new SolidBrush(Theme.TextColor);
                Brush borderBrush = new SolidBrush(Theme.BorderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = p.Graphics.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                p.Graphics.Clear(this.BackColor);
                // Draw text
                p.Graphics.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                //Left
                p.Graphics.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                p.Graphics.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                p.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                p.Graphics.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                p.Graphics.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
    }




}
