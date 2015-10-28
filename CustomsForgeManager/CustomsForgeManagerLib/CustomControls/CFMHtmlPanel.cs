using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace CustomsForgeManager.CustomsForgeManagerLib.CustomControls
{
    public class CFMHtmlPanel : HtmlPanel
    {

        public CFMHtmlPanel():base()
        {
            UseSystemCursors = true;
        }


        protected override void OnStylesheetLoad(TheArtOfDev.HtmlRenderer.Core.Entities.HtmlStylesheetLoadEventArgs e)
        {
            if (File.Exists(Path.Combine(Constants.WorkDirectory, e.Src)))
            {
                e.SetStyleSheet = File.ReadAllText(Path.Combine(Constants.WorkDirectory, e.Src));
                return;
            }
            if (e.Src == "htmExport.css")
            {
                e.SetStyleSheet = Properties.Resources.htmExport;
                return;
            }

            base.OnStylesheetLoad(e);
        }


    }
}
