using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace CustomControls
{
    /// <summary>
    /// Custom LinkLabel control that displays a popup image next to the link
    ///
    [RefreshProperties(RefreshProperties.Repaint)]
    [Browsable(true)]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public class LinkLabelPopup : LinkLabel
    {
        private Control parent;
        private PictureBox pbProfile;
        private LinkLabel linkLabel;

        /// <summary>
        /// Constructor: initiates any special default settings.
        /// </summary>
        public LinkLabelPopup()
            : base()
        {
            PopupImage = null;
            PopupImageSize = new Size(72, 72);
            UrlLink = "";
        }

        public Image m_PopupImage = null;
        [Browsable(true)]
        [DefaultValue("")]
        [Description("Select Image")]
        public Image PopupImage
        {
            get { return m_PopupImage; }
            set { m_PopupImage = value; }
        }

        public Size m_PopupImageSize;
        [Browsable(true)]
        [DefaultValue("")]
        [Description("Image Size")]
        public Size PopupImageSize
        {
            get { return m_PopupImageSize; }
            set { m_PopupImageSize = value; }
        }

        public string m_UrlLink = "";
        [Browsable(true)]
        [DefaultValue("")]
        [Description("Select URL Link")]
        public string UrlLink
        {
            get { return m_UrlLink; }
            set { m_UrlLink = value; }
        }


        //protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(LinkUrl))
        //            Process.Start(LinkUrl);
        //}

        protected override void OnMouseEnter(EventArgs e)
        {
            parent = GetOwner();

            if (parent == null)
                return;

            if (PopupImage != null)
            {
                pbProfile = new PictureBox();
                pbProfile.Size = PopupImageSize;
                parent.Controls.Add(pbProfile);
                pbProfile.Image = PopupImage;
                var p = GetAbsLocation(linkLabel);
                p.X = p.X + Width;
                p.Y = p.Y - Height - 40;
                pbProfile.Location = p;
                pbProfile.Visible = true;
                pbProfile.BringToFront();
            }

            Invalidate();
            base.OnMouseEnter(e); 
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (GetOwner() == null)
                return;

            if (pbProfile != null)
            {
                pbProfile.Visible = false;
                pbProfile = null;
            }

            Invalidate();
            base.OnMouseLeave(e);
        }

        private ContainerControl GetOwner()
        {
            var parent = Parent;
            linkLabel = this;

            while (parent != null)
            {
                if (parent is ContainerControl)
                    return (ContainerControl)parent;

                parent = parent.Parent;
            }

            return null;
        }

        /// <summary>
        /// Gets the absolute location of the LinkLabel on the Owner (ContainerControl)
        /// </summary>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private Point GetAbsLocation(Control ctrl)
        {
            Point p;

            for (p = ctrl.Location; ctrl.Parent != null; ctrl = ctrl.Parent)
                p.Offset((ctrl.Parent.Location));

            return p;
        }

    }
}

