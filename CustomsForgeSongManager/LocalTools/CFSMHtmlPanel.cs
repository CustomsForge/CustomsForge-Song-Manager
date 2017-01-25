using System.IO;
using CustomsForgeSongManager.DataObjects;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace CustomsForgeSongManager.LocalTools
{
    public class CFSMHtmlPanel : HtmlPanel
    {
        public CFSMHtmlPanel() : base()
        {
            UseSystemCursors = true;
        }

        protected override void OnStylesheetLoad(TheArtOfDev.HtmlRenderer.Core.Entities.HtmlStylesheetLoadEventArgs e)
        {
            if (File.Exists(Path.Combine(Constants.WorkFolder, e.Src)))
            {
                e.SetStyleSheet = File.ReadAllText(Path.Combine(Constants.WorkFolder, e.Src));
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