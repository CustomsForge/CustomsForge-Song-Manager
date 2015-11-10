using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DF.WinForms.ThemeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace CustomsForgeManager.CustomsForgeManagerLib.UITheme
{
    [ThemeFileVersion("1.0"), ThemeFileExtension(".cfsmtheme")]
    public class CFSMTheme : Theme
    {
        public CFSMTheme()
            : base()
        {
            ThemeDirectory = Constants.ThemeDirectory;
            if (!Directory.Exists(ThemeDirectory))
                Directory.CreateDirectory(ThemeDirectory);
        }
    }
}
