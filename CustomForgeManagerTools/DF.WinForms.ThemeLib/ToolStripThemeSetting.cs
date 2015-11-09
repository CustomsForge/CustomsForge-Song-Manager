using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DF.WinForms.ThemeLib
{

    [ThemeKey("Tool Strip")]
    public class ToolStripThemeSetting : ThemeSetting
    {
        public override void ResetSettings()
        {
            RaftingContainerGradientBegin = SystemColors.ControlLight;
            RaftingContainerGradientEnd = SystemColors.ControlDark;
            ToolStripPanelGradientBegin = SystemColors.ControlLight;
            ToolStripPanelGradientEnd = SystemColors.ControlDark;
            ToolStripGradientBegin = SystemColors.ControlLight;
            ToolStripGradientMiddle = SystemColors.ControlLight;
            ToolStripGradientEnd = SystemColors.ControlDark;
            ToolStripBorder = SystemColors.ControlDark;
            ButtonPressedGradientBegin = SystemColors.Highlight;
            ButtonPressedGradientMiddle = SystemColors.Highlight;
            ButtonPressedGradientEnd = SystemColors.Highlight;
            ButtonPressedBorder = SystemColors.HotTrack;
            ButtonPressedHighlight = SystemColors.Highlight;
            ButtonPressedHighlightBorder = SystemColors.HotTrack;
            ButtonSelectedGradientBegin = SystemColors.Highlight;
            ButtonSelectedGradientMiddle = SystemColors.Highlight;
            ButtonSelectedGradientEnd = SystemColors.Highlight;
            ButtonSelectedBorder = SystemColors.HotTrack;
            ButtonSelectedHighlight = SystemColors.Highlight;
            ButtonSelectedHighlightBorder = SystemColors.HotTrack;
            ButtonSelectedText = Color.White;
            MenuBorder = SystemColors.ActiveBorder;
            MenuItemBorder = SystemColors.ActiveBorder;
            GripDark = SystemColors.ControlDarkDark;
            GripLight = SystemColors.ControlLightLight;
            SeparatorDark = SystemColors.ControlDarkDark;
            SeparatorLight = SystemColors.ControlLightLight;
            MenuStripGradientBegin = SystemColors.ControlDarkDark;
            MenuStripGradientEnd = SystemColors.ControlLightLight;
            base.ResetSettings();
        }

        #region Button Selected
        [Category("Button Selected")]
        public Color ButtonSelectedBorder { get; set; }
        [Category("Button Selected")]
        public Color ButtonSelectedGradientBegin { get; set; }
        [Category("Button Selected")]
        public Color ButtonSelectedGradientEnd { get; set; }
        [Category("Button Selected")]
        public Color ButtonSelectedGradientMiddle { get; set; }
        [Category("Button Selected")]
        public Color ButtonSelectedHighlight { get; set; }
        [Category("Button Selected")]
        public Color ButtonSelectedHighlightBorder { get; set; }

        [Category("Button Selected")]
        public Color ButtonSelectedText { get; set; }

        #endregion

        #region Button Pressed
        [Category("Button Pressed")]
        public Color ButtonPressedBorder { get; set; }
        [Category("Button Pressed")]
        public Color ButtonPressedGradientBegin { get; set; }
        [Category("Button Pressed")]
        public Color ButtonPressedGradientEnd { get; set; }
        [Category("Button Pressed")]
        public Color ButtonPressedGradientMiddle { get; set; }
        [Category("Button Pressed")]
        public Color ButtonPressedHighlight { get; set; }
        [Category("Button Pressed")]
        public Color ButtonPressedHighlightBorder { get; set; }
        #endregion

        public Color GripDark { get; set; }
        public Color GripLight { get; set; }

        public Color SeparatorDark { get; set; }
        public Color SeparatorLight { get; set; }


        public Color MenuBorder { get; set; }
        public Color MenuItemBorder { get; set; }

        public Color ToolStripBorder { get; set; }
        public Color ToolStripGradientBegin { get; set; }
        public Color ToolStripGradientEnd { get; set; }
        public Color ToolStripGradientMiddle { get; set; }

        public Color ToolStripPanelGradientBegin { get; set; }
        public Color ToolStripPanelGradientEnd { get; set; }

        public Color RaftingContainerGradientBegin { get; set; }
        public Color RaftingContainerGradientEnd { get; set; }
        public Color MenuStripGradientBegin { get; set; }
        public Color MenuStripGradientEnd { get; set; }


    }


    class ThemeColorTable : ProfessionalColorTable
    {
        private Theme theme;
        private ToolStripThemeSetting setting;
        public ThemeColorTable(Theme atheme)
        {
            theme = atheme;
            setting = theme.GetThemeSetting<ToolStripThemeSetting>();
        }

        #region Button Selected
        public override Color ButtonSelectedBorder
        {
            get
            {
                return setting.ButtonSelectedBorder;
            }
        }

        public override Color ButtonSelectedGradientBegin
        {
            get
            {
                return setting.ButtonSelectedGradientBegin;
            }
        }

        public override Color ButtonSelectedGradientEnd
        {
            get
            {
                return setting.ButtonSelectedGradientEnd;
            }
        }

        public override Color ButtonSelectedGradientMiddle
        {
            get
            {
                return setting.ButtonSelectedGradientMiddle;
            }
        }

        public override Color ButtonSelectedHighlight
        {
            get
            {
                return setting.ButtonSelectedHighlight;
            }
        }

        public override Color ButtonSelectedHighlightBorder
        {
            get
            {
                return setting.ButtonSelectedHighlightBorder;
            }
        }
        #endregion


        #region Button Pressed
        public override Color ButtonPressedBorder { get { return setting.ButtonPressedBorder; } }
        public override Color ButtonPressedGradientBegin { get { return setting.ButtonPressedGradientBegin; } }
        public override Color ButtonPressedGradientEnd { get { return setting.ButtonPressedGradientEnd; } }
        public override Color ButtonPressedGradientMiddle { get { return setting.ButtonPressedGradientMiddle; } }
        public override Color ButtonPressedHighlight { get { return setting.ButtonPressedHighlight; } }
        public override Color ButtonPressedHighlightBorder { get { return setting.ButtonPressedHighlightBorder; } }
        #endregion


        public override Color GripDark { get { return setting.GripDark; } }
        public override Color GripLight { get { return setting.GripLight; } }

        public override Color SeparatorDark { get { return setting.SeparatorDark; } }
        public override Color SeparatorLight { get { return setting.SeparatorLight; } }

        public override Color ToolStripPanelGradientBegin { get { return setting.ToolStripPanelGradientBegin; } }
        public override Color ToolStripPanelGradientEnd { get { return setting.ToolStripPanelGradientEnd; } }
        public override Color ToolStripBorder { get { return setting.ToolStripBorder; } }
        public override Color ToolStripGradientBegin { get { return setting.ToolStripGradientBegin; } }
        public override Color ToolStripGradientEnd { get { return setting.ToolStripGradientEnd; } }
        public override Color ToolStripGradientMiddle { get { return setting.ToolStripGradientMiddle; } }
        public override Color RaftingContainerGradientBegin { get { return setting.RaftingContainerGradientBegin; } }
        public override Color RaftingContainerGradientEnd { get { return setting.RaftingContainerGradientEnd; } }
        public override Color MenuBorder { get { return setting.MenuBorder; } }
        public override Color MenuItemBorder { get { return setting.MenuItemBorder; } }

        public override Color MenuItemSelected
        {
            get
            {
                return setting.ButtonSelectedHighlight;
            }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return setting.ButtonSelectedGradientBegin;
            }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return setting.ButtonSelectedGradientEnd;
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return setting.MenuStripGradientBegin;
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return setting.MenuStripGradientEnd;
            }
        }





    }

    class ThemeToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected ThemeToolStripRenderer() { }

        Theme ftheme;
        ToolStripThemeSetting ts;

        public ThemeToolStripRenderer(Theme theme)
            : base(new ThemeColorTable(theme))
        {
            ftheme = theme;
            ts = theme.GetThemeSetting<ToolStripThemeSetting>();
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {

            if (e.Item.Selected || e.Item.Pressed)
            {
                e.TextColor = ts.ButtonSelectedText;
                // e.TextFont = new Font("Helvetica", 7, FontStyle.Bold);
            }
            base.OnRenderItemText(e);
        }
    }

}
