using System.IO;
using CustomsForgeSongManager.DataObjects;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace CustomsForgeSongManager.CustomControls
{
    public class CFSMHtmlPanel : HtmlPanel
    {
        public CFSMHtmlPanel() : base()
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