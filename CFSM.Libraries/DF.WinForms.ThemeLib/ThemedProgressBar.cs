using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DF.WinForms.ThemeLib
{
    public class ThemedProgressBar : ProgressBar, IThemeListener
    {
        Color Chunk2 = Color.Green;
        bool Border3d = true;
        Pen BorderPen = Pens.Black;
        int LastPaintValue = 0;
        LinearGradientMode GradientMode = LinearGradientMode.Horizontal;
        bool themeEnabled = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!themeEnabled)
                base.OnPaint(e);
            else
            {
                var valueLength = ((float)Value / Maximum) * Width;
                LastPaintValue = Value;
                var chunkRect = new RectangleF(2, 2, valueLength, Height - 4);
                Brush BackGroundBrush = new SolidBrush(BackColor);
                Brush ChunkBrush = null;
                if (ForeColor == Chunk2)
                    ChunkBrush = new SolidBrush(ForeColor);
                else
                    ChunkBrush = new LinearGradientBrush(ClientRectangle, ForeColor, Chunk2, GradientMode);
                using (BackGroundBrush)
                using (ChunkBrush)
                {
                    e.Graphics.FillRectangle(BackGroundBrush, ClientRectangle);
                    e.Graphics.FillRectangle(ChunkBrush, chunkRect);
                }
                if (Border3d)
                    ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.SunkenOuter);
                else
                {
                    e.Graphics.DrawRectangle(BorderPen, ClientRectangle);
                }
            }
        }

        public void ApplyTheme(Theme sender)
        {
            sender.PropertyChanged -= sender_PropertyChanged;
            sender.PropertyChanged += sender_PropertyChanged;
            themeEnabled = sender.Enabled;
            SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, themeEnabled);
            var s = sender.GetThemeSetting<ProgressBarThemeSetting>();
            if (s != null)
            {
                ForeColor = s.Chunk;
                Chunk2 = s.Chunk2;
                BackColor = s.BackGround;
                GradientMode = s.GradientMode;
                Border3d = s.Border3d;
                BorderPen = new Pen(s.Border, s.BorderWidth);
                Invalidate();
            }
        }

        void sender_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Enabled")
            {
                themeEnabled = ((Theme)sender).Enabled;
                SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, themeEnabled);
            }
        }

    }


  


    public class ProgressBarThemeSetting : ThemeSetting
    {
        public ProgressBarThemeSetting():base()
        {
            Key = "ProgressBar";
            GradientMode = LinearGradientMode.Horizontal;
            Chunk = Color.Green;
            Chunk2 = Color.Green;
            Border3d = true;
            BackGround = SystemColors.Control;
            Border = Color.Black;
            BorderWidth = 1.0f;
        }

        public Color BackGround { get; set; }
        public Color Chunk { get; set; }
        public Color Chunk2 { get; set; }
        public LinearGradientMode GradientMode { get; set; }
        public Color Border { get; set; }
        public bool Border3d { get; set; }
        public float BorderWidth { get; set; }
    }

    public class ToolStripThemedProgressBar : ToolStripControlHost, IThemeListener  
    {
        internal static readonly object EventRightToLeftLayoutChanged = new object();
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event KeyEventHandler KeyDown
        {
            add
            {
                base.KeyDown += value;
            }
            remove
            {
                base.KeyDown -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event KeyPressEventHandler KeyPress
        {
            add
            {
                base.KeyPress += value;
            }
            remove
            {
                base.KeyPress -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event KeyEventHandler KeyUp
        {
            add
            {
                base.KeyUp += value;
            }
            remove
            {
                base.KeyUp -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler LocationChanged
        {
            add
            {
                base.LocationChanged += value;
            }
            remove
            {
                base.LocationChanged -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler OwnerChanged
        {
            add
            {
                base.OwnerChanged += value;
            }
            remove
            {
                base.OwnerChanged -= value;
            }
        }

        public event EventHandler RightToLeftLayoutChanged
        {
            add
            {
                base.Events.AddHandler(EventRightToLeftLayoutChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventRightToLeftLayoutChanged, value);
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TextChanged
        {
            add
            {
                base.TextChanged += value;
            }
            remove
            {
                base.TextChanged -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler Validated
        {
            add
            {
                base.Validated += value;
            }
            remove
            {
                base.Validated -= value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event CancelEventHandler Validating
        {
            add
            {
                base.Validating += value;
            }
            remove
            {
                base.Validating -= value;
            }
        }
        
        public ToolStripThemedProgressBar() : base(CreateControlInstance())
        {
        }

        public ToolStripThemedProgressBar(string name)
            : this()
        {
            base.Name = name;
        }
        
        private static Control CreateControlInstance()
        {
            return new ThemedProgressBar { Size = new Size(100, 15) };
        }
        
        private void HandleRightToLeftLayoutChanged(object sender, EventArgs e)
        {
            this.OnRightToLeftLayoutChanged(e);
        }
        
        public void Increment(int value)
        {
            this.ProgressBar.Increment(value);
        }
        
        protected virtual void OnRightToLeftLayoutChanged(EventArgs e)
        {
           // base.RaiseEvent(EventRightToLeftLayoutChanged, e);
        }
        
        protected override void OnSubscribeControlEvents(Control control)
        {
            ThemedProgressBar bar = control as ThemedProgressBar;
            if (bar != null)
            {
                bar.RightToLeftLayoutChanged += new EventHandler(this.HandleRightToLeftLayoutChanged);
            }
            base.OnSubscribeControlEvents(control);
        }
        
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            System.Windows.Forms.ProgressBar bar = control as System.Windows.Forms.ProgressBar;
            if (bar != null)
            {
                bar.RightToLeftLayoutChanged -= new EventHandler(this.HandleRightToLeftLayoutChanged);
            }
            base.OnUnsubscribeControlEvents(control);
        }
        
        public void PerformStep()
        {
            this.ProgressBar.PerformStep();
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }
        
        protected override Padding DefaultMargin
        {
            get
            {
                if ((base.Owner != null) && (base.Owner is StatusStrip))
                {
                    return new Padding(1, 3, 1, 3);
                }
                return new Padding(1, 2, 1, 1);
            }
        }
        
        protected override Size DefaultSize
        {
            get
            {
                return new Size(100, 15);
            }
        }
        
        [DefaultValue(100), Category("Behavior")]
        public int MarqueeAnimationSpeed
        {
            get
            {
                return this.ProgressBar.MarqueeAnimationSpeed;
            }
            set
            {
                this.ProgressBar.MarqueeAnimationSpeed = value;
            }
        }
        
        [DefaultValue(100), Category("Behavior"), RefreshProperties(RefreshProperties.Repaint)]
        public int Maximum
        {
            get
            {
                return this.ProgressBar.Maximum;
            }
            set
            {
                this.ProgressBar.Maximum = value;
            }
        }
        
        [DefaultValue(0), Category("Behavior"), RefreshProperties(RefreshProperties.Repaint)]
        public int Minimum
        {
            get
            {
                return this.ProgressBar.Minimum;
            }
            set
            {
                this.ProgressBar.Minimum = value;
            }
        }
        
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ThemedProgressBar ProgressBar
        {
            get
            {
                return (base.Control as ThemedProgressBar);
            }
        }
        
        [Category("Appearance"), Localizable(true), DefaultValue(false)]
        public virtual bool RightToLeftLayout
        {
            get
            {
                return this.ProgressBar.RightToLeftLayout;
            }
            set
            {
                this.ProgressBar.RightToLeftLayout = value;
            }
        }
        
        [DefaultValue(10), Category("Behavior")]
        public int Step
        {
            get
            {
                return this.ProgressBar.Step;
            }
            set
            {
                this.ProgressBar.Step = value;
            }
        }
        
        [DefaultValue(0), Category("Behavior")]
        public ProgressBarStyle Style
        {
            get
            {
                return this.ProgressBar.Style;
            }
            set
            {
                this.ProgressBar.Style = value;
            }
        }
        
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get
            {
                return base.Control.Text;
            }
            set
            {
                base.Control.Text = value;
            }
        }
        
        [DefaultValue(0), Category("Behavior"), Bindable(true)]
        public int Value
        {
            get
            {
                return this.ProgressBar.Value;
            }
            set
            {
                this.ProgressBar.Value = value;
            }
        }

        public void ApplyTheme(Theme sender)
        {
            this.ProgressBar.ApplyTheme(sender);
        }
    }
}
