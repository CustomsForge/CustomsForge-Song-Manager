using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

//parts of this taken from http://www.codeproject.com/Articles/91387/Painting-Your-Own-Tabs-Second-Edition

namespace DF.WinForms.ThemeLib
{
    public enum TabStyle
    {
        None = 0,
        Default = 1,
        VisualStudio = 2,
        Rounded = 3,
        Angled = 4,
        Chrome = 5,
        IE8 = 6,
        VS2010 = 7
    }

    public class ThemedTabControl : TabControl, IThemeListener
    {
        [Browsable(false)]
        public TabControlThemeSetting settings { get; private set; }

        [Browsable(false)]
        public Theme currentTheme { get; private set; }

        const ControlStyles PaintStyle = ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.Opaque | ControlStyles.ResizeRedraw;
        #region Private variables

        private Bitmap _BackImage;
        private Bitmap _BackBuffer;
        private Graphics _BackBufferGraphics;
        private Bitmap _TabBuffer;
        private Graphics _TabBufferGraphics;

        private int _oldValue;
        private Point _dragStartPosition = Point.Empty;

        private TabStyle _Style;
        private TabStyleProvider _StyleProvider;

        private List<TabPage> _TabPages;

        #endregion

        #region	Construction
        public ThemedTabControl()
            : base()
        {
            settings = new TabControlThemeSetting();
          //  this.SetStyle(PaintStyle, true);            
            this._BackBuffer = new Bitmap(this.Width, this.Height);
            this._BackBufferGraphics = Graphics.FromImage(this._BackBuffer);
            this._TabBuffer = new Bitmap(this.Width, this.Height);
            this._TabBufferGraphics = Graphics.FromImage(this._TabBuffer);
            this.DisplayStyle = TabStyle.Default;
            ShowTabCloser = false;
            this.HotTrack = false;
            //   Globals.CFMTheme.AddListener(this);
        }

        #region IThemeListener
        public void ApplyTheme(Theme sender)
        {
            currentTheme = sender;
            sender.PropertyChanged -= sender_PropertyChanged;
            sender.PropertyChanged += sender_PropertyChanged;
            themeEnabled = sender.Enabled;
            SetStyle(PaintStyle, themeEnabled);

            var xsettings = sender.GetThemeSetting<TabControlThemeSetting>();
            if (xsettings != null)
                settings = xsettings;
            this.Font = settings.Font;
            DisplayStyle = settings.Style;
            _StyleProvider.FocusColor = settings.FocusColor;
            _StyleProvider.HotTrack = settings.HotTrack;
            _StyleProvider.BorderColor = settings.BorderColor;
            _StyleProvider.TextColorSelected = settings.ActiveTextColor;
            _StyleProvider.TextColor = settings.TextColor;

            foreach (TabPage tp in TabPages)
                tp.SetTheme(sender);
        }
        #endregion

        void sender_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Enabled")
            {
                themeEnabled = ((Theme)sender).Enabled;
                SetStyle(PaintStyle, themeEnabled);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.OnFontChanged(EventArgs.Empty);
        }


        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams cp = base.CreateParams;
                if (this.RightToLeftLayout)
                    cp.ExStyle = cp.ExStyle | NativeMethods.WS_EX_LAYOUTRTL | NativeMethods.WS_EX_NOINHERITLAYOUT;
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this._BackImage != null)
                {
                    this._BackImage.Dispose();
                }
                if (this._BackBufferGraphics != null)
                {
                    this._BackBufferGraphics.Dispose();
                }
                if (this._BackBuffer != null)
                {
                    this._BackBuffer.Dispose();
                }
                if (this._TabBufferGraphics != null)
                {
                    this._TabBufferGraphics.Dispose();
                }
                if (this._TabBuffer != null)
                {
                    this._TabBuffer.Dispose();
                }

                if (this._StyleProvider != null)
                {
                    this._StyleProvider.Dispose();
                }
            }
        }
        #endregion

        #region Public properties

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabStyleProvider DisplayStyleProvider
        {
            get
            {
                if (this._StyleProvider == null)
                {
                    this.DisplayStyle = TabStyle.Default;
                }

                return this._StyleProvider;
            }
            set
            {
                this._StyleProvider = value;
            }
        }

        public bool ShowTabCloser
        {
            get;
            set;
        }

        [Category("Appearance"), DefaultValue(typeof(TabStyle), "Default"), RefreshProperties(RefreshProperties.All)]
        public TabStyle DisplayStyle
        {
            get { return this._Style; }
            set
            {
                if (this._Style != value)
                {
                    this._Style = value;
                    this._StyleProvider = TabStyleProvider.CreateProvider(this);
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance"), RefreshProperties(RefreshProperties.All)]
        public new bool Multiline
        {
            get
            {
                return base.Multiline;
            }
            set
            {
                base.Multiline = value;
                this.Invalidate();
            }
        }


        //	Hide the Padding attribute so it can not be changed
        //	We are handling this on the Style Provider
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Point Padding
        {
            get { return this.DisplayStyleProvider.Padding; }
            set
            {
                this.DisplayStyleProvider.Padding = value;
            }
        }

        public override bool RightToLeftLayout
        {
            get { return base.RightToLeftLayout; }
            set
            {
                base.RightToLeftLayout = value;
                this.UpdateStyles();
            }
        }


        //	Hide the HotTrack attribute so it can not be changed
        //	We are handling this on the Style Provider
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HotTrack
        {
            get { return this.DisplayStyleProvider.HotTrack; }
            set
            {
                this.DisplayStyleProvider.HotTrack = value;
            }
        }

        [Category("Appearance")]
        public new TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                base.Alignment = value;
                switch (value)
                {
                    case TabAlignment.Top:
                    case TabAlignment.Bottom:
                        this.Multiline = false;
                        break;
                    case TabAlignment.Left:
                    case TabAlignment.Right:
                        this.Multiline = true;
                        break;
                }

            }
        }

        //	Hide the Appearance attribute so it can not be changed
        //	We don't want it as we are doing all the painting.
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public new TabAppearance Appearance
        {
            get
            {
                return base.Appearance;
            }
            set
            {
                //	Don't permit setting to other appearances as we are doing all the painting
                base.Appearance = TabAppearance.Normal;
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {

                //	Special processing to hide tabs
                if (this._Style == TabStyle.None)
                {
                    return new Rectangle(0, 0, Width, Height);
                }
                else
                {
                    int tabStripHeight = 0;
                    int itemHeight = 0;

                    if (this.Alignment <= TabAlignment.Bottom)
                    {
                        itemHeight = this.ItemSize.Height;
                    }
                    else
                    {
                        itemHeight = this.ItemSize.Width;
                    }

                    tabStripHeight = 5 + (itemHeight * this.RowCount);

                    Rectangle rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
                    switch (this.Alignment)
                    {
                        case TabAlignment.Top:
                            rect = new Rectangle(4, tabStripHeight, Width - 8, Height - tabStripHeight - 4);
                            break;
                        case TabAlignment.Bottom:
                            rect = new Rectangle(4, 4, Width - 8, Height - tabStripHeight - 4);
                            break;
                        case TabAlignment.Left:
                            rect = new Rectangle(tabStripHeight, 4, Width - tabStripHeight - 4, Height - 8);
                            break;
                        case TabAlignment.Right:
                            rect = new Rectangle(4, 4, Width - tabStripHeight - 4, Height - 8);
                            break;
                    }
                    return rect;
                }
            }
        }

        [Browsable(false)]
        public int ActiveIndex
        {
            get
            {
                NativeMethods.TCHITTESTINFO hitTestInfo = new NativeMethods.TCHITTESTINFO(this.PointToClient(Control.MousePosition));
                int index = NativeMethods.SendMessage(this.Handle, NativeMethods.TCM_HITTEST, IntPtr.Zero, NativeMethods.ToIntPtr(hitTestInfo)).ToInt32();
                if (index == -1)
                {
                    return -1;
                }
                else
                {
                    if (this.TabPages[index].Enabled)
                    {
                        return index;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        [Browsable(false)]
        public TabPage ActiveTab
        {
            get
            {
                int activeIndex = this.ActiveIndex;
                if (activeIndex > -1)
                {
                    return this.TabPages[activeIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region	Extension methods

        public void HideTab(TabPage page)
        {
            if (page != null && this.TabPages.Contains(page))
            {
                this.BackupTabPages();
                this.TabPages.Remove(page);
            }
        }

        public void HideTab(int index)
        {
            if (this.IsValidTabIndex(index))
            {
                this.HideTab(this._TabPages[index]);
            }
        }

        public void HideTab(string key)
        {
            if (this.TabPages.ContainsKey(key))
            {
                this.HideTab(this.TabPages[key]);
            }
        }

        public void ShowTab(TabPage page)
        {
            if (page != null)
            {
                if (this._TabPages != null)
                {
                    if (!this.TabPages.Contains(page)
                        && this._TabPages.Contains(page))
                    {

                        //	Get insert point from backup of pages
                        int pageIndex = this._TabPages.IndexOf(page);
                        if (pageIndex > 0)
                        {
                            int start = pageIndex - 1;

                            //	Check for presence of earlier pages in the visible tabs
                            for (int index = start; index >= 0; index--)
                            {
                                if (this.TabPages.Contains(this._TabPages[index]))
                                {

                                    //	Set insert point to the right of the last present tab
                                    pageIndex = this.TabPages.IndexOf(this._TabPages[index]) + 1;
                                    break;
                                }
                            }
                        }

                        //	Insert the page, or add to the end
                        if ((pageIndex >= 0) && (pageIndex < this.TabPages.Count))
                        {
                            this.TabPages.Insert(pageIndex, page);
                        }
                        else
                        {
                            this.TabPages.Add(page);
                        }
                    }
                }
                else
                {

                    //	If the page is not found at all then just add it
                    if (!this.TabPages.Contains(page))
                    {
                        this.TabPages.Add(page);
                    }
                }
            }
        }

        public void ShowTab(int index)
        {
            if (this.IsValidTabIndex(index))
            {
                this.ShowTab(this._TabPages[index]);
            }
        }

        public void ShowTab(string key)
        {
            if (this._TabPages != null)
            {
                TabPage tab = this._TabPages.Find(delegate(TabPage page) { return page.Name.Equals(key, StringComparison.OrdinalIgnoreCase); });
                this.ShowTab(tab);
            }
        }

        private bool IsValidTabIndex(int index)
        {
            this.BackupTabPages();
            return ((index >= 0) && (index < this._TabPages.Count));
        }

        private void BackupTabPages()
        {
            if (this._TabPages == null)
            {
                this._TabPages = new List<TabPage>();
                foreach (TabPage page in this.TabPages)
                {
                    this._TabPages.Add(page);
                }
            }
        }

        #endregion

        #region Drag 'n' Drop

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.AllowDrop)
            {
                this._dragStartPosition = new Point(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this.AllowDrop)
            {
                this._dragStartPosition = Point.Empty;
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
                if (drgevent.Data.GetDataPresent(typeof(TabPage)))
                {
                    drgevent.Effect = DragDropEffects.Move;
                }
                else
                {
                    drgevent.Effect = DragDropEffects.None;
                }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if (drgevent.Data.GetDataPresent(typeof(TabPage)))
            {
                drgevent.Effect = DragDropEffects.Move;

                TabPage dragTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));

                if (this.ActiveTab == dragTab)
                {
                    return;
                }

                //	Capture insert point and adjust for removal of tab
                //	We cannot assess this after removal as differeing tab sizes will cause
                //	inaccuracies in the activeTab at insert point.
                int insertPoint = this.ActiveIndex;
                if (dragTab.Parent.Equals(this) && this.TabPages.IndexOf(dragTab) < insertPoint)
                {
                    insertPoint--;
                }
                if (insertPoint < 0)
                {
                    insertPoint = 0;
                }

                //	Remove from current position (could be another tabcontrol)
                ((TabControl)dragTab.Parent).TabPages.Remove(dragTab);

                //	Add to current position
                this.TabPages.Insert(insertPoint, dragTab);
                this.SelectedTab = dragTab;

                //	deal with hidden tab handling?
            }
        }

        private void StartDragDrop()
        {

            if (!this._dragStartPosition.IsEmpty)
            {
                TabPage dragTab = this.SelectedTab;
                if (dragTab != null)
                {
                    //	Test for movement greater than the drag activation trigger area
                    Rectangle dragTestRect = new Rectangle(this._dragStartPosition, Size.Empty);
                    dragTestRect.Inflate(SystemInformation.DragSize);
                    Point pt = this.PointToClient(Control.MousePosition);
                    if (!dragTestRect.Contains(pt))
                    {
                        this.DoDragDrop(dragTab, DragDropEffects.All);
                        this._dragStartPosition = Point.Empty;
                    }
                }
            }
        }

        #endregion

        #region Events

        [Category("Action")]
        public event ScrollEventHandler HScroll;
        [Category("Action")]
        public event EventHandler<TabControlEventArgs> TabImageClick;
        [Category("Action")]
        public event EventHandler<TabControlCancelEventArgs> TabClosing;

        #endregion

        #region	Base class event processing

        protected override void OnFontChanged(EventArgs e)
        {
                IntPtr hFont = this.Font.ToHfont();
                NativeMethods.SendMessage(this.Handle, NativeMethods.WM_SETFONT, hFont, (IntPtr)(-1));
                NativeMethods.SendMessage(this.Handle, NativeMethods.WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);
                this.UpdateStyles();
                if (this.Visible)
                {
                    this.Invalidate();
                }
        }

        protected override void OnResize(EventArgs e)
        {
            //	Recreate the buffer for manual double buffering
            if (this.Width > 0 && this.Height > 0)
            {
                if (this._BackImage != null)
                {
                    this._BackImage.Dispose();
                    this._BackImage = null;
                }
                if (this._BackBufferGraphics != null)
                {
                    this._BackBufferGraphics.Dispose();
                }
                if (this._BackBuffer != null)
                {
                    this._BackBuffer.Dispose();
                }

                this._BackBuffer = new Bitmap(this.Width, this.Height);
                this._BackBufferGraphics = Graphics.FromImage(this._BackBuffer);

                if (this._TabBufferGraphics != null)
                {
                    this._TabBufferGraphics.Dispose();
                }
                if (this._TabBuffer != null)
                {
                    this._TabBuffer.Dispose();
                }

                this._TabBuffer = new Bitmap(this.Width, this.Height);
                this._TabBufferGraphics = Graphics.FromImage(this._TabBuffer);

                if (this._BackImage != null)
                {
                    this._BackImage.Dispose();
                    this._BackImage = null;
                }

            }
            base.OnResize(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            if (this._BackImage != null)
            {
                this._BackImage.Dispose();
                this._BackImage = null;
            }
            base.OnParentBackColorChanged(e);
        }

        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            if (this._BackImage != null)
            {
                this._BackImage.Dispose();
                this._BackImage = null;
            }
            base.OnParentBackgroundImageChanged(e);
        }

        private void OnParentResize(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Invalidate();
            }
        }


        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e); 
            if (this.Parent != null)
            {
                this.Parent.Resize += this.OnParentResize;
            }
        }

        protected override void OnSelecting(TabControlCancelEventArgs e)
        {
            base.OnSelecting(e);
            //	Do not allow selecting of disabled tabs
            if (e.Action == TabControlAction.Selecting && e.TabPage != null && !e.TabPage.Enabled)
            {
                e.Cancel = true;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            if (this.Width > 0 && this.Height > 0)
            {
                if (this._BackImage != null)
                {
                    this._BackImage.Dispose();
                    this._BackImage = null;
                }
            }
            base.OnMove(e);
            this.Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (this.Visible)
            {
                this.Invalidate();
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (this.Visible)
            {
                this.Invalidate();
            }
        }


        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessMnemonic(char charCode)
        {
            foreach (TabPage page in this.TabPages)
            {
                if (IsMnemonic(charCode, page.Text))
                {
                    this.SelectedTab = page;
                    return true;
                }
            }
            return base.ProcessMnemonic(charCode);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref Message m)
        {

            switch (m.Msg)
            {
                case NativeMethods.WM_HSCROLL:

                    //	Raise the scroll event when the scroller is scrolled
                    base.WndProc(ref m);
                        this.OnHScroll(new ScrollEventArgs(((ScrollEventType)NativeMethods.LoWord(m.WParam)), _oldValue, NativeMethods.HiWord(m.WParam), ScrollOrientation.HorizontalScroll));
                    break;
                default:
                    base.WndProc(ref m);
                    break;

            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int index = this.ActiveIndex;

            //	If we are clicking on an image then raise the ImageClicked event before raising the standard mouse click event
            //	if there if a handler.
            if (index > -1 && this.TabImageClick != null
                && (this.TabPages[index].ImageIndex > -1 || !string.IsNullOrEmpty(this.TabPages[index].ImageKey))
                && this.GetTabImageRect(index).Contains(this.MousePosition))
            {
                this.OnTabImageClick(new TabControlEventArgs(this.TabPages[index], index, TabControlAction.Selected));

                //	Fire the base event
                base.OnMouseClick(e);

            }
            else if (!this.DesignMode && index > -1 && ShowTabCloser && this._StyleProvider.ShowTabCloser && this.GetTabCloserRect(index).Contains(this.MousePosition))
            {

                //	If we are clicking on a closer then remove the tab instead of raising the standard mouse click event
                //	But raise the tab closing event first
                TabPage tab = this.ActiveTab;
                TabControlCancelEventArgs args = new TabControlCancelEventArgs(tab, index, false, TabControlAction.Deselecting);
                this.OnTabClosing(args);

                if (!args.Cancel)
                {
                    this.TabPages.Remove(tab);
                    tab.Dispose();
                }
            }
            else
            {
                //	Fire the base event
                base.OnMouseClick(e);
            }
        }

        protected virtual void OnTabImageClick(TabControlEventArgs e)
        {
            if (this.TabImageClick != null)
            {
                this.TabImageClick(this, e);
            }
        }

        protected virtual void OnTabClosing(TabControlCancelEventArgs e)
        {
            if (this.TabClosing != null)
            {
                this.TabClosing(this, e);
            }
        }

        protected virtual void OnHScroll(ScrollEventArgs e)
        {
            //	repaint the moved tabs
            this.Invalidate();

            //	Raise the event
            if (this.HScroll != null)
            {
                this.HScroll(this, e);
            }

            if (e.Type == ScrollEventType.EndScroll)
            {
                this._oldValue = e.NewValue;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
                if (this._StyleProvider.ShowTabCloser && ShowTabCloser)
                {
                    Rectangle tabRect = this._StyleProvider.GetTabRect(this.ActiveIndex);
                    if (tabRect.Contains(this.MousePosition))
                    {
                        this.Invalidate();
                    }
                }

                //	Initialise Drag Drop
                if (this.AllowDrop && e.Button == MouseButtons.Left)
                {
                    this.StartDragDrop();
                }
        }

        #endregion

        #region	Basic drawing methods

        private bool nopaintbounce = false;
        private bool themeEnabled;
        protected override void OnPaint(PaintEventArgs e)
        {

            if (!themeEnabled)
            {
                base.OnPaint(e);
                return;
            }

            //If "nopaintbounce" is set, the last OnPaint call Invalidate,
            //we have the surface, then we paint it now.
            if (nopaintbounce)
            {
                this.CustomPaint(e.Graphics);
                nopaintbounce = false;
                return;                     //And return in this point.
            }

            //We must always paint the entire area of the tab control
            if (e.ClipRectangle.Equals(this.ClientRectangle))
            {

                this.CustomPaint(e.Graphics);

            }
            else
            {

                //The Invalidate method will recall this OnPaint method, and... loop forever,
                //to fix it we go to set "notpaintbounce".
                nopaintbounce = true;

                //it is less intensive to just reinvoke the paint with the whole surface available to draw on.
                this.Invalidate();

            }
        }

        private void CustomPaint(Graphics screenGraphics)
        {
            //	We render into a bitmap that is then drawn in one shot rather than using
            //	double buffering built into the control as the built in buffering
            // 	messes up the background painting.
            //	Equally the .Net 2.0 BufferedGraphics object causes the background painting
            //	to mess up, which is why we use this .Net 1.1 buffering technique.

            //	Buffer code from Gil. Schmidt http://www.codeproject.com/KB/graphics/DoubleBuffering.aspx

            if (this.Width > 0 && this.Height > 0)
            {
                if (this._BackImage == null)
                {
                    //	Cached Background Image
                    this._BackImage = new Bitmap(this.Width, this.Height);
                    Graphics backGraphics = Graphics.FromImage(this._BackImage);
                    backGraphics.Clear(Color.Transparent);
                    this.PaintTransparentBackground(backGraphics, this.ClientRectangle);
                }

                this._BackBufferGraphics.Clear(Color.Transparent);
                this._BackBufferGraphics.DrawImageUnscaled(this._BackImage, 0, 0);

                this._TabBufferGraphics.Clear(Color.Transparent);

                if (this.TabCount > 0)
                {

                    //	When top or bottom and scrollable we need to clip the sides from painting the tabs.
                    //	Left and right are always multiline.
                    if (this.Alignment <= TabAlignment.Bottom && !this.Multiline)
                    {
                        this._TabBufferGraphics.Clip = new Region(new RectangleF(this.ClientRectangle.X + 3, this.ClientRectangle.Y, this.ClientRectangle.Width - 6, this.ClientRectangle.Height));
                    }

                    //	Draw each tabpage from right to left.  We do it this way to handle
                    //	the overlap correctly.
                    if (this.Multiline)
                    {
                        for (int row = 0; row < this.RowCount; row++)
                        {
                            for (int index = this.TabCount - 1; index >= 0; index--)
                            {
                                if (index != this.SelectedIndex && (this.RowCount == 1 || this.GetTabRow(index) == row))
                                {
                                    this.DrawTabPage(index, this._TabBufferGraphics);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int index = this.TabCount - 1; index >= 0; index--)
                        {
                            if (index != this.SelectedIndex)
                            {
                                this.DrawTabPage(index, this._TabBufferGraphics);
                            }
                        }
                    }

                    //	The selected tab must be drawn last so it appears on top.
                    if (this.SelectedIndex > -1)
                    {
                        this.DrawTabPage(this.SelectedIndex, this._TabBufferGraphics);
                    }
                }
                this._TabBufferGraphics.Flush();

                //	Paint the tabs on top of the background

                // Create a new color matrix and set the alpha value to 0.5
                ColorMatrix alphaMatrix = new ColorMatrix();
                alphaMatrix.Matrix00 = alphaMatrix.Matrix11 = alphaMatrix.Matrix22 = alphaMatrix.Matrix44 = 1;
                alphaMatrix.Matrix33 = this._StyleProvider.Opacity;

                // Create a new image attribute object and set the color matrix to
                // the one just created
                using (ImageAttributes alphaAttributes = new ImageAttributes())
                {

                    alphaAttributes.SetColorMatrix(alphaMatrix);

                    // Draw the original image with the image attributes specified
                    this._BackBufferGraphics.DrawImage(this._TabBuffer,
                                                       new Rectangle(0, 0, this._TabBuffer.Width, this._TabBuffer.Height),
                                                       0, 0, this._TabBuffer.Width, this._TabBuffer.Height, GraphicsUnit.Pixel,
                                                       alphaAttributes);
                }

                this._BackBufferGraphics.Flush();

                //	Now paint this to the screen


                //	We want to paint the whole tabstrip and border every time
                //	so that the hot areas update correctly, along with any overlaps

                //	paint the tabs etc.
                if (this.RightToLeftLayout)
                {
                    screenGraphics.DrawImageUnscaled(this._BackBuffer, -1, 0);
                }
                else
                {
                    screenGraphics.DrawImageUnscaled(this._BackBuffer, 0, 0);
                }
            }
        }

        protected void PaintTransparentBackground(Graphics graphics, Rectangle clipRect)
        {

            if ((this.Parent != null))
            {

                //	Set the cliprect to be relative to the parent
                clipRect.Offset(this.Location);

                //	Save the current state before we do anything.
                GraphicsState state = graphics.Save();

                //	Set the graphicsobject to be relative to the parent
                graphics.TranslateTransform((float)-this.Location.X, (float)-this.Location.Y);
                graphics.SmoothingMode = SmoothingMode.HighSpeed;

                //	Paint the parent
                PaintEventArgs e = new PaintEventArgs(graphics, clipRect);
                try
                {
                    this.InvokePaintBackground(this.Parent, e);
                    this.InvokePaint(this.Parent, e);
                }
                finally
                {
                    //	Restore the graphics state and the clipRect to their original locations
                    graphics.Restore(state);
                    clipRect.Offset(-this.Location.X, -this.Location.Y);
                }
            }
        }


        private Brush GetPageBackgroundBrush(int index)
        {
            return this._StyleProvider.GetPageBackgroundBrush(index);
        }

        private void DrawTabPage(int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighSpeed;

            //	Get TabPageBorder
            using (GraphicsPath tabPageBorderPath = this.GetTabPageBorder(index))
            {

                //	Paint the background
                using (Brush fillBrush = GetPageBackgroundBrush(index))
                {
                    graphics.FillPath(fillBrush, tabPageBorderPath);
                }


                //	Paint the tab
                this._StyleProvider.PaintTab(index, graphics);

                //	Draw any image
                this.DrawTabImage(index, graphics);

                //	Draw the text
                this.DrawTabText(index, graphics);


                //	Paint the border
                this.DrawTabBorder(tabPageBorderPath, index, graphics);

            }
        }

        private void DrawTabBorder(GraphicsPath path, int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            Color borderColor;
            if (index == this.SelectedIndex)
            {
                borderColor = this._StyleProvider.BorderColorSelected;
            }
            else if (this._StyleProvider.HotTrack && index == this.ActiveIndex)
            {
                borderColor = this._StyleProvider.BorderColorHot;
            }
            else
            {
                borderColor = this._StyleProvider.BorderColor;
            }

            using (Pen borderPen = new Pen(borderColor))
            {
                graphics.DrawPath(borderPen, path);
            }
        }

        private void DrawTabText(int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle tabBounds = this.GetTabTextRect(index);

            if (this.SelectedIndex == index)
            {
                using (Brush textBrush = new SolidBrush(this._StyleProvider.TextColorSelected))
                {
                    graphics.DrawString(this.TabPages[index].Text, settings.ActiveFont, textBrush, tabBounds, this.GetStringFormat());
                }
            }
            else
            {
                if (this.TabPages[index].Enabled)
                {
                    using (Brush textBrush = new SolidBrush(this._StyleProvider.TextColor))
                    {
                        graphics.DrawString(this.TabPages[index].Text, this.Font, textBrush, tabBounds, this.GetStringFormat());
                    }
                }
                else
                {
                    using (Brush textBrush = new SolidBrush(this._StyleProvider.TextColorDisabled))
                    {
                        graphics.DrawString(this.TabPages[index].Text, this.Font, textBrush, tabBounds, this.GetStringFormat());
                    }
                }
            }
        }

        private void DrawTabImage(int index, Graphics graphics)
        {
            Image tabImage = null;
            if (this.TabPages[index].ImageIndex > -1 && this.ImageList != null && this.ImageList.Images.Count > this.TabPages[index].ImageIndex)
            {
                tabImage = this.ImageList.Images[this.TabPages[index].ImageIndex];
            }
            else if ((!string.IsNullOrEmpty(this.TabPages[index].ImageKey) && !this.TabPages[index].ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase))
                     && this.ImageList != null && this.ImageList.Images.ContainsKey(this.TabPages[index].ImageKey))
            {
                tabImage = this.ImageList.Images[this.TabPages[index].ImageKey];
            }

            if (tabImage != null)
            {
                if (this.RightToLeftLayout)
                {
                    tabImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                Rectangle imageRect = this.GetTabImageRect(index);
                if (this.TabPages[index].Enabled)
                {
                    graphics.DrawImage(tabImage, imageRect);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(graphics, tabImage, imageRect.X, imageRect.Y, Color.Transparent);
                }
            }
        }

        #endregion

        #region String formatting

        private StringFormat GetStringFormat()
        {
            StringFormat format = null;

            //	Rotate Text by 90 degrees for left and right tabs
            switch (this.Alignment)
            {
                case TabAlignment.Top:
                case TabAlignment.Bottom:
                    format = new StringFormat();
                    break;
                case TabAlignment.Left:
                case TabAlignment.Right:
                    format = new StringFormat(StringFormatFlags.DirectionVertical);
                    break;
            }
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if (this.FindForm() != null && this.FindForm().KeyPreview)
            {
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            }
            else
            {
                format.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            }
            if (this.RightToLeft == RightToLeft.Yes)
            {
                format.FormatFlags = format.FormatFlags | StringFormatFlags.DirectionRightToLeft;
            }
            return format;
        }

        #endregion

        #region Tab borders and bounds properties

        private GraphicsPath GetTabPageBorder(int index)
        {

            GraphicsPath path = new GraphicsPath();
            Rectangle pageBounds = this.GetPageBounds(index);
            Rectangle tabBounds = this._StyleProvider.GetTabRect(index);
            this._StyleProvider.AddTabBorder(path, tabBounds);
            this.AddPageBorder(path, pageBounds, tabBounds);

            path.CloseFigure();
            return path;
        }

        public Rectangle GetPageBounds(int index)
        {
            Rectangle pageBounds = this.TabPages[index].Bounds;
            pageBounds.Width += 1;
            pageBounds.Height += 1;
            pageBounds.X -= 1;
            pageBounds.Y -= 1;

            if (pageBounds.Bottom > this.Height - 4)
            {
                pageBounds.Height -= (pageBounds.Bottom - this.Height + 4);
            }
            return pageBounds;
        }

        private Rectangle GetTabTextRect(int index)
        {
            Rectangle textRect = new Rectangle();
            using (GraphicsPath path = this._StyleProvider.GetTabBorder(index))
            {
                RectangleF tabBounds = path.GetBounds();

                textRect = new Rectangle((int)tabBounds.X, (int)tabBounds.Y, (int)tabBounds.Width,
                    (int)tabBounds.Height);

                //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        textRect.Y += 4;
                        textRect.Height -= 6;
                        break;
                    case TabAlignment.Bottom:
                        textRect.Y += 2;
                        textRect.Height -= 6;
                        break;
                    case TabAlignment.Left:
                        textRect.X += 4;
                        textRect.Width -= 6;
                        break;
                    case TabAlignment.Right:
                        textRect.X += 2;
                        textRect.Width -= 6;
                        break;
                }

                //	If there is an image allow for it
                if (this.ImageList != null && (this.TabPages[index].ImageIndex > -1
                                               || (!string.IsNullOrEmpty(this.TabPages[index].ImageKey)
                                                   && !this.TabPages[index].ImageKey.Equals("(none)", StringComparison.OrdinalIgnoreCase))))
                {
                    Rectangle imageRect = this.GetTabImageRect(index);
                    if ((this._StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                    {
                        if (this.Alignment <= TabAlignment.Bottom)
                        {
                            textRect.X = imageRect.Right + 4;
                            textRect.Width -= (textRect.Right - (int)tabBounds.Right);
                        }
                        else
                        {
                            textRect.Y = imageRect.Y + 4;
                            textRect.Height -= (textRect.Bottom - (int)tabBounds.Bottom);
                        }
                        //	If there is a closer allow for it
                        if (this._StyleProvider.ShowTabCloser && ShowTabCloser)
                        {
                            Rectangle closerRect = this.GetTabCloserRect(index);
                            if (this.Alignment <= TabAlignment.Bottom)
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                    else if ((this._StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                    {
                        //	If there is a closer allow for it
                        if (this._StyleProvider.ShowTabCloser && ShowTabCloser)
                        {
                            Rectangle closerRect = this.GetTabCloserRect(index);
                            if (this.Alignment <= TabAlignment.Bottom)
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (this.Alignment <= TabAlignment.Bottom)
                        {
                            textRect.Width -= ((int)tabBounds.Right - imageRect.X + 4);
                        }
                        else
                        {
                            textRect.Height -= ((int)tabBounds.Bottom - imageRect.Y + 4);
                        }
                        //	If there is a closer allow for it
                        if (this._StyleProvider.ShowTabCloser && ShowTabCloser)
                        {
                            Rectangle closerRect = this.GetTabCloserRect(index);
                            if (this.Alignment <= TabAlignment.Bottom)
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                    textRect.X = closerRect.Right + 4;
                                }
                                else
                                {
                                    textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                                }
                            }
                            else
                            {
                                if (this.RightToLeftLayout)
                                {
                                    textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                    textRect.Y = closerRect.Bottom + 4;
                                }
                                else
                                {
                                    textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //	If there is a closer allow for it
                    if (this._StyleProvider.ShowTabCloser && ShowTabCloser)
                    {
                        Rectangle closerRect = this.GetTabCloserRect(index);
                        if (this.Alignment <= TabAlignment.Bottom)
                        {
                            if (this.RightToLeftLayout)
                            {
                                textRect.Width -= (closerRect.Right + 4 - textRect.X);
                                textRect.X = closerRect.Right + 4;
                            }
                            else
                            {
                                textRect.Width -= ((int)tabBounds.Right - closerRect.X + 4);
                            }
                        }
                        else
                        {
                            if (this.RightToLeftLayout)
                            {
                                textRect.Height -= (closerRect.Bottom + 4 - textRect.Y);
                                textRect.Y = closerRect.Bottom + 4;
                            }
                            else
                            {
                                textRect.Height -= ((int)tabBounds.Bottom - closerRect.Y + 4);
                            }
                        }
                    }
                }


                //	Ensure it fits inside the path at the centre line
                if (this.Alignment <= TabAlignment.Bottom)
                {
                    while (!path.IsVisible(textRect.Right, textRect.Y) && textRect.Width > 0)
                    {
                        textRect.Width -= 1;
                    }
                    while (!path.IsVisible(textRect.X, textRect.Y) && textRect.Width > 0)
                    {
                        textRect.X += 1;
                        textRect.Width -= 1;
                    }
                }
                else
                {
                    while (!path.IsVisible(textRect.X, textRect.Bottom) && textRect.Height > 0)
                    {
                        textRect.Height -= 1;
                    }
                    while (!path.IsVisible(textRect.X, textRect.Y) && textRect.Height > 0)
                    {
                        textRect.Y += 1;
                        textRect.Height -= 1;
                    }
                }
            }
            return textRect;
        }

        public int GetTabRow(int index)
        {
            //	All calculations will use this rect as the base point
            //	because the itemsize does not return the correct width.
            Rectangle rect = this.GetTabRect(index);

            int row = -1;

            switch (this.Alignment)
            {
                case TabAlignment.Top:
                    row = (rect.Y - 2) / rect.Height;
                    break;
                case TabAlignment.Bottom:
                    row = ((this.Height - rect.Y - 2) / rect.Height) - 1;
                    break;
                case TabAlignment.Left:
                    row = (rect.X - 2) / rect.Width;
                    break;
                case TabAlignment.Right:
                    row = ((this.Width - rect.X - 2) / rect.Width) - 1;
                    break;
            }
            return row;
        }

        public Point GetTabPosition(int index)
        {

            //	If we are not multiline then the column is the index and the row is 0.
            if (!this.Multiline)
            {
                return new Point(0, index);
            }

            //	If there is only one row then the column is the index
            if (this.RowCount == 1)
            {
                return new Point(0, index);
            }

            //	We are in a true multi-row scenario
            int row = this.GetTabRow(index);
            Rectangle rect = this.GetTabRect(index);
            int column = -1;

            //	Scan from left to right along rows, skipping to next row if it is not the one we want.
            for (int testIndex = 0; testIndex < this.TabCount; testIndex++)
            {
                Rectangle testRect = this.GetTabRect(testIndex);
                if (this.Alignment <= TabAlignment.Bottom)
                {
                    if (testRect.Y == rect.Y)
                    {
                        column += 1;
                    }
                }
                else
                {
                    if (testRect.X == rect.X)
                    {
                        column += 1;
                    }
                }

                if (testRect.Location.Equals(rect.Location))
                {
                    return new Point(row, column);
                }
            }

            return new Point(0, 0);
        }

        public bool IsFirstTabInRow(int index)
        {
            if (index < 0)
            {
                return false;
            }
            bool firstTabinRow = (index == 0);
            if (!firstTabinRow)
            {
                if (this.Alignment <= TabAlignment.Bottom)
                {
                    if (this.GetTabRect(index).X == 2)
                    {
                        firstTabinRow = true;
                    }
                }
                else
                {
                    if (this.GetTabRect(index).Y == 2)
                    {
                        firstTabinRow = true;
                    }
                }
            }
            return firstTabinRow;
        }

        private void AddPageBorder(GraphicsPath path, Rectangle pageBounds, Rectangle tabBounds)
        {
            switch (this.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, tabBounds.X, pageBounds.Y);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, tabBounds.Right, pageBounds.Bottom);
                    break;
                case TabAlignment.Left:
                    path.AddLine(pageBounds.X, tabBounds.Y, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, tabBounds.Bottom);
                    break;
                case TabAlignment.Right:
                    path.AddLine(pageBounds.Right, tabBounds.Bottom, pageBounds.Right, pageBounds.Bottom);
                    path.AddLine(pageBounds.Right, pageBounds.Bottom, pageBounds.X, pageBounds.Bottom);
                    path.AddLine(pageBounds.X, pageBounds.Bottom, pageBounds.X, pageBounds.Y);
                    path.AddLine(pageBounds.X, pageBounds.Y, pageBounds.Right, pageBounds.Y);
                    path.AddLine(pageBounds.Right, pageBounds.Y, pageBounds.Right, tabBounds.Y);
                    break;
            }
        }

        private Rectangle GetTabImageRect(int index)
        {
            using (GraphicsPath tabBorderPath = this._StyleProvider.GetTabBorder(index))
            {
                return this.GetTabImageRect(tabBorderPath);
            }
        }
        private Rectangle GetTabImageRect(GraphicsPath tabBorderPath)
        {
            Rectangle imageRect = new Rectangle();
            RectangleF rect = tabBorderPath.GetBounds();

            //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
            switch (this.Alignment)
            {
                case TabAlignment.Top:
                    rect.Y += 4;
                    rect.Height -= 6;
                    break;
                case TabAlignment.Bottom:
                    rect.Y += 2;
                    rect.Height -= 6;
                    break;
                case TabAlignment.Left:
                    rect.X += 4;
                    rect.Width -= 6;
                    break;
                case TabAlignment.Right:
                    rect.X += 2;
                    rect.Width -= 6;
                    break;
            }

            //	Ensure image is fully visible
            if (this.Alignment <= TabAlignment.Bottom)
            {
                if ((this._StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Y))
                    {
                        imageRect.X += 1;
                    }
                    imageRect.X += 4;

                }
                else if ((this._StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)(((int)rect.Right - (int)rect.X - (int)rect.Height + 2) / 2)), (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                }
                else
                {
                    imageRect = new Rectangle((int)rect.Right, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 16) / 2), 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.Right, imageRect.Y))
                    {
                        imageRect.X -= 1;
                    }
                    imageRect.X -= 4;

                    //	Move it in further to allow for the tab closer
                    if (this._StyleProvider.ShowTabCloser && ShowTabCloser && !this.RightToLeftLayout)
                    {
                        imageRect.X -= 10;
                    }
                }
            }
            else
            {
                if ((this._StyleProvider.ImageAlign & NativeMethods.AnyLeftAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Y, 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Y))
                    {
                        imageRect.Y += 1;
                    }
                    imageRect.Y += 4;
                }
                else if ((this._StyleProvider.ImageAlign & NativeMethods.AnyCenterAlign) != ((ContentAlignment)0))
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Y + (int)Math.Floor((double)(((int)rect.Bottom - (int)rect.Y - (int)rect.Width + 2) / 2)), 16, 16);
                }
                else
                {
                    imageRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 16) / 2), (int)rect.Bottom, 16, 16);
                    while (!tabBorderPath.IsVisible(imageRect.X, imageRect.Bottom))
                    {
                        imageRect.Y -= 1;
                    }
                    imageRect.Y -= 4;

                    //	Move it in further to allow for the tab closer
                    if (this._StyleProvider.ShowTabCloser && ShowTabCloser && !this.RightToLeftLayout)
                    {
                        imageRect.Y -= 10;
                    }
                }
            }
            return imageRect;
        }

        public Rectangle GetTabCloserRect(int index)
        {
            Rectangle closerRect = new Rectangle();
            using (GraphicsPath path = this._StyleProvider.GetTabBorder(index))
            {
                RectangleF rect = path.GetBounds();

                //	Make it shorter or thinner to fit the height or width because of the padding added to the tab for painting
                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        rect.Y += 4;
                        rect.Height -= 6;
                        break;
                    case TabAlignment.Bottom:
                        rect.Y += 2;
                        rect.Height -= 6;
                        break;
                    case TabAlignment.Left:
                        rect.X += 4;
                        rect.Width -= 6;
                        break;
                    case TabAlignment.Right:
                        rect.X += 2;
                        rect.Width -= 6;
                        break;
                }
                if (this.Alignment <= TabAlignment.Bottom)
                {
                    if (this.RightToLeftLayout)
                    {
                        closerRect = new Rectangle((int)rect.Left, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 6) / 2), 6, 6);
                        while (!path.IsVisible(closerRect.Left, closerRect.Y) && closerRect.Right < this.Width)
                        {
                            closerRect.X += 1;
                        }
                        closerRect.X += 4;
                    }
                    else
                    {
                        closerRect = new Rectangle((int)rect.Right, (int)rect.Y + (int)Math.Floor((double)((int)rect.Height - 6) / 2), 6, 6);
                        while (!path.IsVisible(closerRect.Right, closerRect.Y) && closerRect.Right > -6)
                        {
                            closerRect.X -= 1;
                        }
                        closerRect.X -= 4;
                    }
                }
                else
                {
                    if (this.RightToLeftLayout)
                    {
                        closerRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 6) / 2), (int)rect.Top, 6, 6);
                        while (!path.IsVisible(closerRect.X, closerRect.Top) && closerRect.Bottom < this.Height)
                        {
                            closerRect.Y += 1;
                        }
                        closerRect.Y += 4;
                    }
                    else
                    {
                        closerRect = new Rectangle((int)rect.X + (int)Math.Floor((double)((int)rect.Width - 6) / 2), (int)rect.Bottom, 6, 6);
                        while (!path.IsVisible(closerRect.X, closerRect.Bottom) && closerRect.Top > -6)
                        {
                            closerRect.Y -= 1;
                        }
                        closerRect.Y -= 4;
                    }
                }
            }
            return closerRect;
        }

        public new Point MousePosition
        {
            get
            {
                Point loc = this.PointToClient(Control.MousePosition);
                if (this.RightToLeftLayout)
                {
                    loc.X = (this.Width - loc.X);
                }
                return loc;
            }
        }

        #endregion


      
    }

    #region Providers

    [System.ComponentModel.ToolboxItem(false)]
    public abstract class TabStyleProvider : Component
    {
        #region Constructor

        protected TabStyleProvider(ThemedTabControl tabControl)
        {
            this._TabControl = tabControl;

            this._BorderColor = Color.Empty;
            this._BorderColorSelected = Color.Empty;
            this._FocusColor = Color.Orange;

            if (this._TabControl.RightToLeftLayout)
            {
                this._ImageAlign = ContentAlignment.MiddleRight;
            }
            else
            {
                this._ImageAlign = ContentAlignment.MiddleLeft;
            }

            this.HotTrack = true;

            //	Must set after the _Overlap as this is used in the calculations of the actual padding
            this.Padding = new Point(6, 3);
        }

        #endregion

        #region Factory Methods

        public static TabStyleProvider CreateProvider(ThemedTabControl tabControl)
        {
            TabStyleProvider provider;

            //	Depending on the display style of the tabControl generate an appropriate provider.
            switch (tabControl.DisplayStyle)
            {
                case TabStyle.None:
                    provider = new TabStyleNoneProvider(tabControl);
                    break;

                case TabStyle.Default:
                    provider = new TabStyleDefaultProvider(tabControl);
                    break;

                case TabStyle.Angled:
                    provider = new TabStyleAngledProvider(tabControl);
                    break;

                case TabStyle.Rounded:
                    provider = new TabStyleRoundedProvider(tabControl);
                    break;

                case TabStyle.VisualStudio:
                    provider = new TabStyleVisualStudioProvider(tabControl);
                    break;

                case TabStyle.Chrome:
                    provider = new TabStyleChromeProvider(tabControl);
                    break;

                case TabStyle.IE8:
                    provider = new TabStyleIE8Provider(tabControl);
                    break;

                case TabStyle.VS2010:
                    provider = new TabStyleVS2010Provider(tabControl);
                    break;

                default:
                    provider = new TabStyleDefaultProvider(tabControl);
                    break;
            }

            provider._Style = tabControl.DisplayStyle;
            return provider;
        }

        #endregion

        #region	Protected variables

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected ThemedTabControl _TabControl;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Point _Padding;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected bool _HotTrack;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected TabStyle _Style = TabStyle.Default;


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected ContentAlignment _ImageAlign;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected int _Radius = 1;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected int _Overlap;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected bool _FocusTrack;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected float _Opacity = 1;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected bool _ShowTabCloser;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _BorderColorSelected = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _BorderColor = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _BorderColorHot = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]

        protected Color _CloserColorActive = Color.Black;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _CloserColor = Color.DarkGray;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]

        protected Color _FocusColor = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]

        protected Color _TextColor = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _TextColorSelected = Color.Empty;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        protected Color _TextColorDisabled = Color.Empty;

        #endregion

        #region overridable Methods

        public abstract void AddTabBorder(GraphicsPath path, Rectangle tabBounds);

        public virtual Rectangle GetTabRect(int index)
        {

            if (index < 0)
            {
                return new Rectangle();
            }
            Rectangle tabBounds = this._TabControl.GetTabRect(index);
            if (this._TabControl.RightToLeftLayout)
            {
                tabBounds.X = this._TabControl.Width - tabBounds.Right;
            }
            bool firstTabinRow = this._TabControl.IsFirstTabInRow(index);

            //	Expand to overlap the tabpage
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    tabBounds.Height += 2;
                    break;
                case TabAlignment.Bottom:
                    tabBounds.Height += 2;
                    tabBounds.Y -= 2;
                    break;
                case TabAlignment.Left:
                    tabBounds.Width += 2;
                    break;
                case TabAlignment.Right:
                    tabBounds.X -= 2;
                    tabBounds.Width += 2;
                    break;
            }


            //	Greate Overlap unless first tab in the row to align with tabpage
            if ((!firstTabinRow || this._TabControl.RightToLeftLayout) && this._Overlap > 0)
            {
                if (this._TabControl.Alignment <= TabAlignment.Bottom)
                {
                    tabBounds.X -= this._Overlap;
                    tabBounds.Width += this._Overlap;
                }
                else
                {
                    tabBounds.Y -= this._Overlap;
                    tabBounds.Height += this._Overlap;
                }
            }

            //	Adjust first tab in the row to align with tabpage
            this.EnsureFirstTabIsInView(ref tabBounds, index);

            return tabBounds;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected virtual void EnsureFirstTabIsInView(ref Rectangle tabBounds, int index)
        {
            //	Adjust first tab in the row to align with tabpage
            //	Make sure we only reposition visible tabs, as we may have scrolled out of view.

            bool firstTabinRow = this._TabControl.IsFirstTabInRow(index);

            if (firstTabinRow)
            {
                if (this._TabControl.Alignment <= TabAlignment.Bottom)
                {
                    if (this._TabControl.RightToLeftLayout)
                    {
                        if (tabBounds.Left < this._TabControl.Right)
                        {
                            int tabPageRight = this._TabControl.GetPageBounds(index).Right;
                            if (tabBounds.Right > tabPageRight)
                            {
                                tabBounds.Width -= (tabBounds.Right - tabPageRight);
                            }
                        }
                    }
                    else
                    {
                        if (tabBounds.Right > 0)
                        {
                            int tabPageX = this._TabControl.GetPageBounds(index).X;
                            if (tabBounds.X < tabPageX)
                            {
                                tabBounds.Width -= (tabPageX - tabBounds.X);
                                tabBounds.X = tabPageX;
                            }
                        }
                    }
                }
                else
                {
                    if (this._TabControl.RightToLeftLayout)
                    {
                        if (tabBounds.Top < this._TabControl.Bottom)
                        {
                            int tabPageBottom = this._TabControl.GetPageBounds(index).Bottom;
                            if (tabBounds.Bottom > tabPageBottom)
                            {
                                tabBounds.Height -= (tabBounds.Bottom - tabPageBottom);
                            }
                        }
                    }
                    else
                    {
                        if (tabBounds.Bottom > 0)
                        {
                            int tabPageY = this._TabControl.GetPageBounds(index).Location.Y;
                            if (tabBounds.Y < tabPageY)
                            {
                                tabBounds.Height -= (tabPageY - tabBounds.Y);
                                tabBounds.Y = tabPageY;
                            }
                        }
                    }
                }
            }
        }

        protected virtual Brush GetTabBackgroundBrush(int index)
        {
            LinearGradientBrush fillBrush = null;

            //	Capture the colours dependant on selection state of the tab
            Color dark = _TabControl.settings.BackColor;
            Color light = _TabControl.settings.BackColor2;

            if (this._TabControl.SelectedIndex == index)
            {
                dark = _TabControl.settings.ActiveBackColor;
                light = _TabControl.settings.ActiveBackColor2;
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = dark;
            }
            else if (this._HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = _TabControl.settings.HotTrackColor;
                dark = _TabControl.settings.HotTrackColor2;
            }

            //	Get the correctly aligned gradient
            Rectangle tabBounds = this.GetTabRect(index);
            tabBounds.Inflate(3, 3);
            tabBounds.X -= 1;
            tabBounds.Y -= 1;
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    if (this._TabControl.SelectedIndex == index)
                    {
                        dark = light;
                    }
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Bottom:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Left:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal);
                    break;
                case TabAlignment.Right:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal);
                    break;
            }

            //	Add the blend
            fillBrush.Blend = this.GetBackgroundBlend();

            return fillBrush;
        }

        #endregion

        #region	Base Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TabStyle DisplayStyle
        {
            get { return this._Style; }
            set { this._Style = value; }
        }

        [Category("Appearance")]
        public ContentAlignment ImageAlign
        {
            get { return this._ImageAlign; }
            set
            {
                this._ImageAlign = value;
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance")]
        public Point Padding
        {
            get { return this._Padding; }
            set
            {
                this._Padding = value;
                //	This line will trigger the handle to recreate, therefore invalidating the control
                if (this._ShowTabCloser)
                {
                    if (value.X + (int)(this._Radius / 2) < -6)
                    {
                        ((TabControl)this._TabControl).Padding = new Point(0, value.Y);
                    }
                    else
                    {
                        ((TabControl)this._TabControl).Padding = new Point(value.X + (int)(this._Radius / 2) + 6, value.Y);
                    }
                }
                else
                {
                    if (value.X + (int)(this._Radius / 2) < 1)
                    {
                        ((TabControl)this._TabControl).Padding = new Point(0, value.Y);
                    }
                    else
                    {
                        ((TabControl)this._TabControl).Padding = new Point(value.X + (int)(this._Radius / 2) - 1, value.Y);
                    }
                }
            }
        }


        [Category("Appearance"), DefaultValue(1), Browsable(true)]
        public int Radius
        {
            get { return this._Radius; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("The radius must be greater than 1", "value");
                }
                this._Radius = value;
                //	Adjust padding
                this.Padding = this._Padding;
            }
        }

        [Category("Appearance")]
        public int Overlap
        {
            get { return this._Overlap; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The tabs cannot have a negative overlap", "value");
                }
                this._Overlap = value;

            }
        }


        [Category("Appearance")]
        public bool FocusTrack
        {
            get { return this._FocusTrack; }
            set
            {
                this._FocusTrack = value;
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance")]
        public bool HotTrack
        {
            get { return this._HotTrack; }
            set
            {
                this._HotTrack = value;
                ((TabControl)this._TabControl).HotTrack = value;
            }
        }

        [Category("Appearance")]
        public bool ShowTabCloser
        {
            get { return this._ShowTabCloser; }
            set
            {
                this._ShowTabCloser = value;
                //	Adjust padding
                this.Padding = this._Padding;
            }
        }

        [Category("Appearance")]
        public float Opacity
        {
            get { return this._Opacity; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The opacity must be between 0 and 1", "value");
                }
                if (value > 1)
                {
                    throw new ArgumentException("The opacity must be between 0 and 1", "value");
                }
                this._Opacity = value;
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color BorderColorSelected
        {
            get
            {
                //if (this._BorderColorSelected.IsEmpty)
                //{
                //    return ThemedColors.ToolBorder;
                //}
                // else
                {
                    return this._BorderColorSelected;
                }
            }
            set
            {
                //if (value.Equals(ThemedColors.ToolBorder))
                //{
                //    this._BorderColorSelected = Color.Empty;
                //}
                //else
                {
                    this._BorderColorSelected = value;
                }
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color BorderColorHot
        {
            get
            {
                //if (this._BorderColorHot.IsEmpty)
                //{
                //    return SystemColors.ControlDark;
                //}
                //else
                {
                    return this._BorderColorHot;
                }
            }
            set
            {
                //if (value.Equals(SystemColors.ControlDark))
                //{
                //    this._BorderColorHot = Color.Empty;
                //}
                //else
                {
                    this._BorderColorHot = value;
                }
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color BorderColor
        {
            get
            {
                if (this._BorderColor.IsEmpty)
                {
                    return _TabControl.settings.BorderColor;
                }
                else
                {
                    return this._BorderColor;
                }
            }
            set
            {
                if (value.Equals(SystemColors.ControlDark))
                {
                    this._BorderColor = Color.Empty;
                }
                else
                {
                    this._BorderColor = value;
                }
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color TextColor
        {
            get
            {
                if (this._TextColor.IsEmpty)
                {
                    return _TabControl.settings.TextColor;
                }
                else
                {
                    return this._TextColor;
                }
            }
            set
            {
                if (value.Equals(SystemColors.ControlText))
                {
                    this._TextColor = Color.Empty;
                }
                else
                {
                    this._TextColor = value;
                }
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color TextColorSelected
        {
            get
            {
                if (this._TextColorSelected.IsEmpty)
                {
                    return _TabControl.settings.ActiveTextColor;
                }
                else
                {
                    return this._TextColorSelected;
                }
            }
            set
            {
                if (value.Equals(SystemColors.ControlText))
                {
                    this._TextColorSelected = Color.Empty;
                }
                else
                {
                    this._TextColorSelected = value;
                }
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "")]
        public Color TextColorDisabled
        {
            get
            {
                if (this._TextColor.IsEmpty)
                {
                    return SystemColors.ControlDark;
                }
                else
                {
                    return this._TextColorDisabled;
                }
            }
            set
            {
                if (value.Equals(SystemColors.ControlDark))
                {
                    this._TextColorDisabled = Color.Empty;
                }
                else
                {
                    this._TextColorDisabled = value;
                }
                this._TabControl.Invalidate();
            }
        }


        [Category("Appearance"), DefaultValue(typeof(Color), "Orange")]
        public Color FocusColor
        {
            get { return this._FocusColor; }
            set
            {
                this._FocusColor = value;
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "Black")]
        public Color CloserColorActive
        {
            get { return this._CloserColorActive; }
            set
            {
                this._CloserColorActive = value;
                this._TabControl.Invalidate();
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Color), "DarkGrey")]
        public Color CloserColor
        {
            get { return this._CloserColor; }
            set
            {
                this._CloserColor = value;
                this._TabControl.Invalidate();
            }
        }

        #endregion

        #region Painting

        public void PaintTab(int index, Graphics graphics)
        {
            using (GraphicsPath tabpath = this.GetTabBorder(index))
            {
                using (Brush fillBrush = this.GetTabBackgroundBrush(index))
                {
                    //	Paint the background
                    graphics.FillPath(fillBrush, tabpath);

                    //	Paint a focus indication
                    if (this._TabControl.Focused)
                    {
                        this.DrawTabFocusIndicator(tabpath, index, graphics);
                    }

                    //	Paint the closer
                    this.DrawTabCloser(index, graphics);

                }
            }
        }

        protected virtual void DrawTabCloser(int index, Graphics graphics)
        {
            if (this._ShowTabCloser)
            {
                Rectangle closerRect = this._TabControl.GetTabCloserRect(index);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath closerPath = TabStyleProvider.GetCloserPath(closerRect))
                {
                    if (closerRect.Contains(this._TabControl.MousePosition))
                    {
                        using (Pen closerPen = new Pen(this._CloserColorActive))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                    else
                    {
                        using (Pen closerPen = new Pen(this._CloserColor))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }

                }
            }
        }

        protected static GraphicsPath GetCloserPath(Rectangle closerRect)
        {
            GraphicsPath closerPath = new GraphicsPath();
            closerPath.AddLine(closerRect.X, closerRect.Y, closerRect.Right, closerRect.Bottom);
            closerPath.CloseFigure();
            closerPath.AddLine(closerRect.Right, closerRect.Y, closerRect.X, closerRect.Bottom);
            closerPath.CloseFigure();

            return closerPath;
        }

        private void DrawTabFocusIndicator(GraphicsPath tabpath, int index, Graphics graphics)
        {
            if (this._FocusTrack && this._TabControl.Focused && index == this._TabControl.SelectedIndex)
            {
                Brush focusBrush = null;
                RectangleF pathRect = tabpath.GetBounds();
                Rectangle focusRect = Rectangle.Empty;
                switch (this._TabControl.Alignment)
                {
                    case TabAlignment.Top:
                        focusRect = new Rectangle((int)pathRect.X, (int)pathRect.Y, (int)pathRect.Width, 4);
                        focusBrush = new LinearGradientBrush(focusRect, this._FocusColor, SystemColors.Window, LinearGradientMode.Vertical);
                        break;
                    case TabAlignment.Bottom:
                        focusRect = new Rectangle((int)pathRect.X, (int)pathRect.Bottom - 4, (int)pathRect.Width, 4);
                        focusBrush = new LinearGradientBrush(focusRect, SystemColors.ControlLight, this._FocusColor, LinearGradientMode.Vertical);
                        break;
                    case TabAlignment.Left:
                        focusRect = new Rectangle((int)pathRect.X, (int)pathRect.Y, 4, (int)pathRect.Height);
                        focusBrush = new LinearGradientBrush(focusRect, this._FocusColor, SystemColors.ControlLight, LinearGradientMode.Horizontal);
                        break;
                    case TabAlignment.Right:
                        focusRect = new Rectangle((int)pathRect.Right - 4, (int)pathRect.Y, 4, (int)pathRect.Height);
                        focusBrush = new LinearGradientBrush(focusRect, SystemColors.ControlLight, this._FocusColor, LinearGradientMode.Horizontal);
                        break;
                }

                //	Ensure the focus stip does not go outside the tab
                Region focusRegion = new Region(focusRect);
                focusRegion.Intersect(tabpath);
                graphics.FillRegion(focusBrush, focusRegion);
                focusRegion.Dispose();
                focusBrush.Dispose();
            }
        }

        #endregion

        #region Background brushes

        private Blend GetBackgroundBlend()
        {
            float[] relativeIntensities = new float[] { 0f, 0.7f, 1f };
            float[] relativePositions = new float[] { 0f, 0.6f, 1f };

            //	Glass look to top aligned tabs
            if (this._TabControl.Alignment == TabAlignment.Top)
            {
                relativeIntensities = new float[] { 0f, 0.5f, 1f, 1f };
                relativePositions = new float[] { 0f, 0.5f, 0.51f, 1f };
            }

            Blend blend = new Blend();
            blend.Factors = relativeIntensities;
            blend.Positions = relativePositions;

            return blend;
        }

        public virtual Brush GetPageBackgroundBrush(int index)
        {

            //	Capture the colours dependant on selection state of the tab
            Color light = _TabControl.settings.BackColor;
           // Color dark = _TabControl.settings.BackColor2;
            //if (this._TabControl.Alignment == TabAlignment.Top)
            //{
            //    light = Color.FromArgb(207, 207, 207);
            //}

            if (this._TabControl.SelectedIndex == index)
            {
                light = _TabControl.settings.ActiveBackColor;
          //      dark = _TabControl.settings.ActiveBackColor2;
            }
            //else 
            //if (!this._TabControl.TabPages[index].Enabled)
            //{
            //    light = _TabControl.settings.BackColor;
            //}
            else if (this._HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = _TabControl.settings.HotTrackColor;
           //     dark = _TabControl.settings.HotTrackColor2;
            }

            return new SolidBrush(light);
        }

        #endregion

        #region Tab border and rect

        public GraphicsPath GetTabBorder(int index)
        {

            GraphicsPath path = new GraphicsPath();
            Rectangle tabBounds = this.GetTabRect(index);

            this.AddTabBorder(path, tabBounds);

            path.CloseFigure();
            return path;
        }

        #endregion

    }


    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleDefaultProvider : TabStyleProvider
    {
        public TabStyleDefaultProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._FocusTrack = true;
            this._Radius = 2;
        }

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y);
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right, tabBounds.Y);
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom);
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }

        public override Rectangle GetTabRect(int index)
        {
            if (index < 0)
            {
                return new Rectangle();
            }

            Rectangle tabBounds = base.GetTabRect(index);
            bool firstTabinRow = this._TabControl.IsFirstTabInRow(index);

            //	Make non-SelectedTabs smaller and selected tab bigger
            if (index != this._TabControl.SelectedIndex)
            {
                switch (this._TabControl.Alignment)
                {
                    case TabAlignment.Top:
                        tabBounds.Y += 1;
                        tabBounds.Height -= 1;
                        break;
                    case TabAlignment.Bottom:
                        tabBounds.Height -= 1;
                        break;
                    case TabAlignment.Left:
                        tabBounds.X += 1;
                        tabBounds.Width -= 1;
                        break;
                    case TabAlignment.Right:
                        tabBounds.Width -= 1;
                        break;
                }
            }
            else
            {
                switch (this._TabControl.Alignment)
                {
                    case TabAlignment.Top:
                        if (tabBounds.Y > 0)
                        {
                            tabBounds.Y -= 1;
                            tabBounds.Height += 1;
                        }

                        if (firstTabinRow)
                        {
                            tabBounds.Width += 1;
                        }
                        else
                        {
                            tabBounds.X -= 1;
                            tabBounds.Width += 2;
                        }
                        break;
                    case TabAlignment.Bottom:
                        if (tabBounds.Bottom < this._TabControl.Bottom)
                        {
                            tabBounds.Height += 1;
                        }
                        if (firstTabinRow)
                        {
                            tabBounds.Width += 1;
                        }
                        else
                        {
                            tabBounds.X -= 1;
                            tabBounds.Width += 2;
                        }
                        break;
                    case TabAlignment.Left:
                        if (tabBounds.X > 0)
                        {
                            tabBounds.X -= 1;
                            tabBounds.Width += 1;
                        }

                        if (firstTabinRow)
                        {
                            tabBounds.Height += 1;
                        }
                        else
                        {
                            tabBounds.Y -= 1;
                            tabBounds.Height += 2;
                        }
                        break;
                    case TabAlignment.Right:
                        if (tabBounds.Right < this._TabControl.Right)
                        {
                            tabBounds.Width += 1;
                        }
                        if (firstTabinRow)
                        {
                            tabBounds.Height += 1;
                        }
                        else
                        {
                            tabBounds.Y -= 1;
                            tabBounds.Height += 2;
                        }
                        break;
                }
            }

            //	Adjust first tab in the row to align with tabpage
            this.EnsureFirstTabIsInView(ref tabBounds, index);

            return tabBounds;
        }
    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleNoneProvider : TabStyleProvider
    {
        public TabStyleNoneProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeRush", "Member is not implemented")]
        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {
        }
    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleChromeProvider : TabStyleProvider
    {
        public TabStyleChromeProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._Overlap = 16;
            this._Radius = 16;
            this._ShowTabCloser = true;
            this._CloserColorActive = Color.White;

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(7, 5);
        }

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {

            int spread;
            int eigth;
            int sixth;
            int quarter;

            if (this._TabControl.Alignment <= TabAlignment.Bottom)
            {
                spread = (int)Math.Floor((decimal)tabBounds.Height * 2 / 3);
                eigth = (int)Math.Floor((decimal)tabBounds.Height * 1 / 8);
                sixth = (int)Math.Floor((decimal)tabBounds.Height * 1 / 6);
                quarter = (int)Math.Floor((decimal)tabBounds.Height * 1 / 4);
            }
            else
            {
                spread = (int)Math.Floor((decimal)tabBounds.Width * 2 / 3);
                eigth = (int)Math.Floor((decimal)tabBounds.Width * 1 / 8);
                sixth = (int)Math.Floor((decimal)tabBounds.Width * 1 / 6);
                quarter = (int)Math.Floor((decimal)tabBounds.Width * 1 / 4);
            }

            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:

                    path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Bottom)
					              		,new Point(tabBounds.X + sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X + spread, tabBounds.Y)});
                    path.AddLine(tabBounds.X + spread, tabBounds.Y, tabBounds.Right - spread, tabBounds.Y);
                    path.AddCurve(new Point[] {  new Point(tabBounds.Right - spread, tabBounds.Y)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right, tabBounds.Bottom)});
                    break;
                case TabAlignment.Bottom:
                    path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Y)
					              		,new Point(tabBounds.Right - sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.Right - spread + quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.Right - spread, tabBounds.Bottom)});
                    path.AddLine(tabBounds.Right - spread, tabBounds.Bottom, tabBounds.X + spread, tabBounds.Bottom);
                    path.AddCurve(new Point[] {  new Point(tabBounds.X + spread, tabBounds.Bottom)
					              		,new Point(tabBounds.X + spread - quarter, tabBounds.Bottom - eigth)
					              		,new Point(tabBounds.X + sixth, tabBounds.Y + eigth)
					              		,new Point(tabBounds.X, tabBounds.Y)});
                    break;
                case TabAlignment.Left:
                    path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X, tabBounds.Bottom - spread)});
                    path.AddLine(tabBounds.X, tabBounds.Bottom - spread, tabBounds.X, tabBounds.Y + spread);
                    path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y + spread)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right, tabBounds.Y)});

                    break;
                case TabAlignment.Right:
                    path.AddCurve(new Point[] {  new Point(tabBounds.X, tabBounds.Y)
					              		,new Point(tabBounds.X + eigth, tabBounds.Y + sixth)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Y + spread - quarter)
					              		,new Point(tabBounds.Right, tabBounds.Y + spread)});
                    path.AddLine(tabBounds.Right, tabBounds.Y + spread, tabBounds.Right, tabBounds.Bottom - spread);
                    path.AddCurve(new Point[] {  new Point(tabBounds.Right, tabBounds.Bottom - spread)
					              		,new Point(tabBounds.Right - eigth, tabBounds.Bottom - spread + quarter)
					              		,new Point(tabBounds.X + eigth, tabBounds.Bottom - sixth)
					              		,new Point(tabBounds.X, tabBounds.Bottom)});
                    break;
            }
        }

        protected override void DrawTabCloser(int index, Graphics graphics)
        {
            if (this._ShowTabCloser)
            {
                Rectangle closerRect = this._TabControl.GetTabCloserRect(index);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (closerRect.Contains(this._TabControl.MousePosition))
                {
                    using (GraphicsPath closerPath = GetCloserButtonPath(closerRect))
                    {
                        using (SolidBrush closerBrush = new SolidBrush(Color.FromArgb(193, 53, 53)))
                        {
                            graphics.FillPath(closerBrush, closerPath);
                        }
                    }
                    using (GraphicsPath closerPath = GetCloserPath(closerRect))
                    {
                        using (Pen closerPen = new Pen(this._CloserColorActive))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                }
                else
                {
                    using (GraphicsPath closerPath = GetCloserPath(closerRect))
                    {
                        using (Pen closerPen = new Pen(this._CloserColor))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                }


            }
        }

        private static GraphicsPath GetCloserButtonPath(Rectangle closerRect)
        {
            GraphicsPath closerPath = new GraphicsPath();
            closerPath.AddEllipse(new Rectangle(closerRect.X - 2, closerRect.Y - 2, closerRect.Width + 4, closerRect.Height + 4));
            closerPath.CloseFigure();
            return closerPath;
        }
    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleIE8Provider : TabStyleRoundedProvider
    {
        public TabStyleIE8Provider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._Radius = 3;
            this._ShowTabCloser = true;
            this._CloserColorActive = Color.Red;

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(6, 5);

        }

        public override Rectangle GetTabRect(int index)
        {
            if (index < 0)
            {
                return new Rectangle();
            }
            Rectangle tabBounds = base.GetTabRect(index);
            bool firstTabinRow = this._TabControl.IsFirstTabInRow(index);

            //	Make non-SelectedTabs smaller and selected tab bigger
            if (index != this._TabControl.SelectedIndex)
            {
                switch (this._TabControl.Alignment)
                {
                    case TabAlignment.Top:
                        tabBounds.Y += 1;
                        tabBounds.Height -= 1;
                        break;
                    case TabAlignment.Bottom:
                        tabBounds.Height -= 1;
                        break;
                    case TabAlignment.Left:
                        tabBounds.X += 1;
                        tabBounds.Width -= 1;
                        break;
                    case TabAlignment.Right:
                        tabBounds.Width -= 1;
                        break;
                }
            }
            else
            {
                switch (this._TabControl.Alignment)
                {
                    case TabAlignment.Top:
                        tabBounds.Y -= 1;
                        tabBounds.Height += 1;

                        if (firstTabinRow)
                        {
                            tabBounds.Width += 1;
                        }
                        else
                        {
                            tabBounds.X -= 1;
                            tabBounds.Width += 2;
                        }
                        break;
                    case TabAlignment.Bottom:
                        tabBounds.Height += 1;

                        if (firstTabinRow)
                        {
                            tabBounds.Width += 1;
                        }
                        else
                        {
                            tabBounds.X -= 1;
                            tabBounds.Width += 2;
                        }
                        break;
                    case TabAlignment.Left:
                        tabBounds.X -= 1;
                        tabBounds.Width += 1;

                        if (firstTabinRow)
                        {
                            tabBounds.Height += 1;
                        }
                        else
                        {
                            tabBounds.Y -= 1;
                            tabBounds.Height += 2;
                        }
                        break;
                    case TabAlignment.Right:
                        tabBounds.Width += 1;
                        if (firstTabinRow)
                        {
                            tabBounds.Height += 1;
                        }
                        else
                        {
                            tabBounds.Y -= 1;
                            tabBounds.Height += 2;
                        }
                        break;
                }
            }

            //	Adjust first tab in the row to align with tabpage
            this.EnsureFirstTabIsInView(ref tabBounds, index);
            return tabBounds;
        }

        protected override Brush GetTabBackgroundBrush(int index)
        {
            LinearGradientBrush fillBrush = null;

            //	Capture the colours dependant on selection state of the tab
            Color dark = Color.FromArgb(227, 238, 251);
            Color light = Color.FromArgb(227, 238, 251);

            if (this._TabControl.SelectedIndex == index)
            {
                dark = Color.FromArgb(196, 222, 251);
                light = SystemColors.Window;
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = dark;
            }
            else if (this.HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = SystemColors.Window;
                dark = Color.FromArgb(166, 203, 248);
            }

            //	Get the correctly aligned gradient
            Rectangle tabBounds = this.GetTabRect(index);
            tabBounds.Inflate(3, 3);
            tabBounds.X -= 1;
            tabBounds.Y -= 1;
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Bottom:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Left:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal);
                    break;
                case TabAlignment.Right:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal);
                    break;
            }

            //	Add the blend
            fillBrush.Blend = this.GetBackgroundBlend(index);

            return fillBrush;
        }

        private Blend GetBackgroundBlend(int index)
        {
            float[] relativeIntensities = new float[] { 0f, 0.7f, 1f };
            float[] relativePositions = new float[] { 0f, 0.8f, 1f };

            if (this._TabControl.SelectedIndex != index)
            {
                relativeIntensities = new float[] { 0f, 0.3f, 1f };
                relativePositions = new float[] { 0f, 0.2f, 1f };
            }

            Blend blend = new Blend();
            blend.Factors = relativeIntensities;
            blend.Positions = relativePositions;

            return blend;
        }

        protected override void DrawTabCloser(int index, Graphics graphics)
        {
            if (this._ShowTabCloser)
            {
                Rectangle closerRect = this._TabControl.GetTabCloserRect(index);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (closerRect.Contains(this._TabControl.MousePosition))
                {
                    using (GraphicsPath closerPath = GetCloserButtonPath(closerRect))
                    {
                        graphics.FillPath(Brushes.White, closerPath);
                        using (Pen closerPen = new Pen(this.BorderColor))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                    using (GraphicsPath closerPath = GetCloserPath(closerRect))
                    {
                        using (Pen closerPen = new Pen(this._CloserColorActive))
                        {
                            closerPen.Width = 2;
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                }
                else
                {
                    using (GraphicsPath closerPath = GetCloserPath(closerRect))
                    {
                        using (Pen closerPen = new Pen(this._CloserColor))
                        {
                            closerPen.Width = 2;
                            graphics.DrawPath(closerPen, closerPath);

                        }
                    }
                }

            }
        }

        private static GraphicsPath GetCloserButtonPath(Rectangle closerRect)
        {
            GraphicsPath closerPath = new GraphicsPath();
            closerPath.AddLine(closerRect.X - 1, closerRect.Y - 2, closerRect.Right + 1, closerRect.Y - 2);
            closerPath.AddLine(closerRect.Right + 2, closerRect.Y - 1, closerRect.Right + 2, closerRect.Bottom + 1);
            closerPath.AddLine(closerRect.Right + 1, closerRect.Bottom + 2, closerRect.X - 1, closerRect.Bottom + 2);
            closerPath.AddLine(closerRect.X - 2, closerRect.Bottom + 1, closerRect.X - 2, closerRect.Y - 1);
            closerPath.CloseFigure();
            return closerPath;
        }

        public override Brush GetPageBackgroundBrush(int index)
        {

            //	Capture the colours dependant on selection state of the tab
            Color light = Color.FromArgb(227, 238, 251);

            if (this._TabControl.SelectedIndex == index)
            {
                light = SystemColors.Window;
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = Color.FromArgb(207, 207, 207);
            }
            else if (this._HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = Color.FromArgb(234, 246, 253);
            }

            return new SolidBrush(light);
        }

    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleVisualStudioProvider : TabStyleProvider
    {
        public TabStyleVisualStudioProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._ImageAlign = ContentAlignment.MiddleRight;
            this._Overlap = 7;

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(14, 1);
        }

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + tabBounds.Height - 4, tabBounds.Y + 2);
                    path.AddLine(tabBounds.X + tabBounds.Height, tabBounds.Y, tabBounds.Right - 3, tabBounds.Y);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Y, 6, 6, 270, 90);
                    path.AddLine(tabBounds.Right, tabBounds.Y + 3, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - 3);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
                    path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X + tabBounds.Height, tabBounds.Bottom);
                    path.AddLine(tabBounds.X + tabBounds.Height - 4, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 3, tabBounds.Bottom);
                    path.AddArc(tabBounds.X, tabBounds.Bottom - 6, 6, 6, 90, 90);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - 3, tabBounds.X, tabBounds.Y + tabBounds.Width);
                    path.AddLine(tabBounds.X + 2, tabBounds.Y + tabBounds.Width - 4, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + tabBounds.Width - 4);
                    path.AddLine(tabBounds.Right, tabBounds.Y + tabBounds.Width, tabBounds.Right, tabBounds.Bottom - 3);
                    path.AddArc(tabBounds.Right - 6, tabBounds.Bottom - 6, 6, 6, 0, 90);
                    path.AddLine(tabBounds.Right - 3, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }

    }


    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleVS2010Provider : TabStyleRoundedProvider
    {
        public TabStyleVS2010Provider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._Radius = 3;
            this._ShowTabCloser = true;
            this._CloserColorActive = Color.Black;
            this._CloserColor = Color.FromArgb(117, 99, 61);
            this._TextColor = Color.White;
            this._TextColorDisabled = Color.WhiteSmoke;
            this._BorderColor = Color.Transparent;
            this._BorderColorHot = Color.FromArgb(155, 167, 183);

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(6, 5);

        }

        protected override Brush GetTabBackgroundBrush(int index)
        {
            LinearGradientBrush fillBrush = null;

            //	Capture the colours dependant on selection state of the tab
            Color dark = Color.Transparent;
            Color light = Color.Transparent;

            if (this._TabControl.SelectedIndex == index)
            {
                dark = Color.FromArgb(229, 195, 101);
                light = SystemColors.Window;
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = dark;
            }
            else if (this.HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                dark = Color.FromArgb(108, 116, 118);
                light = dark;
            }

            //	Get the correctly aligned gradient
            Rectangle tabBounds = this.GetTabRect(index);
            tabBounds.Inflate(3, 3);
            tabBounds.X -= 1;
            tabBounds.Y -= 1;
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Bottom:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Vertical);
                    break;
                case TabAlignment.Left:
                    fillBrush = new LinearGradientBrush(tabBounds, light, dark, LinearGradientMode.Horizontal);
                    break;
                case TabAlignment.Right:
                    fillBrush = new LinearGradientBrush(tabBounds, dark, light, LinearGradientMode.Horizontal);
                    break;
            }

            //	Add the blend
            fillBrush.Blend = GetBackgroundBlend();

            return fillBrush;
        }

        private static Blend GetBackgroundBlend()
        {
            float[] relativeIntensities = new float[] { 0f, 0.5f, 1f, 1f };
            float[] relativePositions = new float[] { 0f, 0.5f, 0.51f, 1f };


            Blend blend = new Blend();
            blend.Factors = relativeIntensities;
            blend.Positions = relativePositions;

            return blend;
        }

        public override Brush GetPageBackgroundBrush(int index)
        {

            //	Capture the colours dependant on selection state of the tab
            Color light = Color.Transparent;

            if (this._TabControl.SelectedIndex == index)
            {
                light = Color.FromArgb(229, 195, 101);
            }
            else if (!this._TabControl.TabPages[index].Enabled)
            {
                light = Color.Transparent;
            }
            else if (this._HotTrack && index == this._TabControl.ActiveIndex)
            {
                //	Enable hot tracking
                light = Color.Transparent;
            }

            return new SolidBrush(light);
        }

        protected override void DrawTabCloser(int index, Graphics graphics)
        {
            if (this._ShowTabCloser)
            {
                Rectangle closerRect = this._TabControl.GetTabCloserRect(index);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                if (closerRect.Contains(this._TabControl.MousePosition))
                {
                    using (GraphicsPath closerPath = GetCloserButtonPath(closerRect))
                    {
                        graphics.FillPath(Brushes.White, closerPath);
                        using (Pen closerPen = new Pen(Color.FromArgb(229, 195, 101)))
                        {
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                    using (GraphicsPath closerPath = GetCloserPath(closerRect))
                    {
                        using (Pen closerPen = new Pen(this._CloserColorActive))
                        {
                            closerPen.Width = 2;
                            graphics.DrawPath(closerPen, closerPath);
                        }
                    }
                }
                else
                {
                    if (index == this._TabControl.SelectedIndex)
                    {
                        using (GraphicsPath closerPath = GetCloserPath(closerRect))
                        {
                            using (Pen closerPen = new Pen(this._CloserColor))
                            {
                                closerPen.Width = 2;
                                graphics.DrawPath(closerPen, closerPath);
                            }
                        }
                    }
                    else if (index == this._TabControl.ActiveIndex)
                    {
                        using (GraphicsPath closerPath = GetCloserPath(closerRect))
                        {
                            using (Pen closerPen = new Pen(Color.FromArgb(155, 167, 183)))
                            {
                                closerPen.Width = 2;
                                graphics.DrawPath(closerPen, closerPath);
                            }
                        }
                    }
                }

            }
        }

        private static GraphicsPath GetCloserButtonPath(Rectangle closerRect)
        {
            GraphicsPath closerPath = new GraphicsPath();
            closerPath.AddLine(closerRect.X - 1, closerRect.Y - 2, closerRect.Right + 1, closerRect.Y - 2);
            closerPath.AddLine(closerRect.Right + 2, closerRect.Y - 1, closerRect.Right + 2, closerRect.Bottom + 1);
            closerPath.AddLine(closerRect.Right + 1, closerRect.Bottom + 2, closerRect.X - 1, closerRect.Bottom + 2);
            closerPath.AddLine(closerRect.X - 2, closerRect.Bottom + 1, closerRect.X - 2, closerRect.Y - 1);
            closerPath.CloseFigure();
            return closerPath;
        }

    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleAngledProvider : TabStyleProvider
    {
        public TabStyleAngledProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._ImageAlign = ContentAlignment.MiddleRight;
            this._Overlap = 7;
            this._Radius = 10;

            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(10, 3);

        }

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {
            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X + this._Radius - 2, tabBounds.Y + 2);
                    path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
                    path.AddLine(tabBounds.Right - this._Radius + 2, tabBounds.Y + 2, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right - this._Radius + 2, tabBounds.Bottom - 2);
                    path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
                    path.AddLine(tabBounds.X + this._Radius - 2, tabBounds.Bottom - 2, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + 2, tabBounds.Bottom - this._Radius + 2);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y + this._Radius);
                    path.AddLine(tabBounds.X + 2, tabBounds.Y + this._Radius - 2, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - 2, tabBounds.Y + this._Radius - 2);
                    path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom - this._Radius);
                    path.AddLine(tabBounds.Right - 2, tabBounds.Bottom - this._Radius + 2, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }

    }

    [System.ComponentModel.ToolboxItem(false)]
    public class TabStyleRoundedProvider : TabStyleProvider
    {
        public TabStyleRoundedProvider(ThemedTabControl tabControl)
            : base(tabControl)
        {
            this._Radius = 10;
            //	Must set after the _Radius as this is used in the calculations of the actual padding
            this.Padding = new Point(6, 3);
        }

        public override void AddTabBorder(GraphicsPath path, Rectangle tabBounds)
        {

            switch (this._TabControl.Alignment)
            {
                case TabAlignment.Top:
                    path.AddLine(tabBounds.X, tabBounds.Bottom, tabBounds.X, tabBounds.Y + this._Radius);
                    path.AddArc(tabBounds.X, tabBounds.Y, this._Radius * 2, this._Radius * 2, 180, 90);
                    path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
                    path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Y, this._Radius * 2, this._Radius * 2, 270, 90);
                    path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom);
                    break;
                case TabAlignment.Bottom:
                    path.AddLine(tabBounds.Right, tabBounds.Y, tabBounds.Right, tabBounds.Bottom - this._Radius);
                    path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 0, 90);
                    path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
                    path.AddArc(tabBounds.X, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 90, 90);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y);
                    break;
                case TabAlignment.Left:
                    path.AddLine(tabBounds.Right, tabBounds.Bottom, tabBounds.X + this._Radius, tabBounds.Bottom);
                    path.AddArc(tabBounds.X, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 90, 90);
                    path.AddLine(tabBounds.X, tabBounds.Bottom - this._Radius, tabBounds.X, tabBounds.Y + this._Radius);
                    path.AddArc(tabBounds.X, tabBounds.Y, this._Radius * 2, this._Radius * 2, 180, 90);
                    path.AddLine(tabBounds.X + this._Radius, tabBounds.Y, tabBounds.Right, tabBounds.Y);
                    break;
                case TabAlignment.Right:
                    path.AddLine(tabBounds.X, tabBounds.Y, tabBounds.Right - this._Radius, tabBounds.Y);
                    path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Y, this._Radius * 2, this._Radius * 2, 270, 90);
                    path.AddLine(tabBounds.Right, tabBounds.Y + this._Radius, tabBounds.Right, tabBounds.Bottom - this._Radius);
                    path.AddArc(tabBounds.Right - this._Radius * 2, tabBounds.Bottom - this._Radius * 2, this._Radius * 2, this._Radius * 2, 0, 90);
                    path.AddLine(tabBounds.Right - this._Radius, tabBounds.Bottom, tabBounds.X, tabBounds.Bottom);
                    break;
            }
        }
    }

    #endregion

    
    [ThemeKey("Tab Control")]
    public class TabControlThemeSetting : ThemeSetting, IDisposable
    {
        public void Dispose()
        {
            if (Font != null)
            {
                Font.Dispose();
                Font = null;
            }
            if (ActiveFont != null)
            {
                ActiveFont.Dispose();
                ActiveFont = null;
            }
        }

        public TabControlThemeSetting():base()
        {
            Key = "TabControl";
            Style = TabStyle.Default;

            ActiveBackColor = SystemColors.ControlLight;
            ActiveBackColor2 = SystemColors.Window;

            BackColor = Color.FromArgb(0xCF, 0xCF, 0xCF);
            BackColor2 = Color.FromArgb(242, 242, 242);

            ActiveTextColor = SystemColors.WindowText;
            TextColor = SystemColors.WindowText;
            FocusColor = Color.Orange;

            BorderColor = SystemColors.ControlLight;

            HotTrackColor = Color.FromArgb(167, 217, 245);
            HotTrackColor2 = Color.FromArgb(234, 246, 253);
            HotTrack = false;

            Font = new Font("Arial", 8);
            ActiveFont = new Font("Arial", 8);
        }
        [Category("Appearance"), DefaultValue(TabStyle.Default)]
        public TabStyle Style { get; set; }
        [Category("Appearance")]
        public Font Font { get; set; }
        [Category("Appearance")]
        public Font ActiveFont { get; set; }
        [Category("Appearance"), DefaultValue(typeof(Color), "#CFCFCF")]//
        public Color BackColor { get; set; }
        [Category("Appearance")]
        public Color BackColor2 { get; set; }
        [Category("Appearance")]
        public Color TextColor { get; set; }
        [Category("Appearance")]
        public Color ActiveBackColor { get; set; }
        [Category("Appearance")]
        public Color ActiveBackColor2 { get; set; }
        [Category("Appearance")]
        public Color ActiveTextColor { get; set; }
        [Category("Appearance"), DefaultValue(false)]
        public bool HotTrack { get; set; }
        [Category("Appearance")]
        public Color HotTrackColor { get; set; }
        [Category("Appearance")]
        public Color HotTrackColor2 { get; set; }
        [Category("Appearance")]
        public Color HotTrackTextColor { get; set; }
        [Category("Appearance"), DefaultValue(typeof(Color), "Orange")]
        public Color FocusColor { get; set; }
        [Category("Appearance"), DefaultValue(typeof(Color), "ControlLight")]
        public Color BorderColor { get; set; }
    }
}
