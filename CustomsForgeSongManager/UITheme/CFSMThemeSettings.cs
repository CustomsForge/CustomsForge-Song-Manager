using System;
using System.Drawing;
using DF.WinForms.ThemeLib;

namespace CustomsForgeSongManager.UITheme
{
    [ThemeKey("Column Settings")]
    public class ThemeColumnSettings : ThemeSetting, IDisposable
    {
        public ThemeColumnSettings()
        {
            ColBass = new ThemeImage();
            ColBass.SetImage(Properties.Resources.Letter_B);
            ColLead = new ThemeImage();
            ColLead.SetImage(Properties.Resources.Letter_L);
            ColRhythm = new ThemeImage();
            ColRhythm.SetImage(Properties.Resources.Letter_R);
            ColVocal = new ThemeImage();
            ColVocal.SetImage(Properties.Resources.Letter_V);
            ColAvaliable = Color.Lime;
            ColNotAvaliable = Color.Red;
            FilterOn = new ThemeImage();
            FilterOn.SetImage(DataGridViewTools.DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(true));
            FilterOff = new ThemeImage();
            FilterOff.SetImage(DataGridViewTools.DataGridViewAutoFilterColumnHeaderCell.GetFilterResource(false));
        }

        public void Dispose()
        {
            if (ColBass != null)
            {
                ColBass.Dispose();
                ColBass = null;
            }
            if (ColLead != null)
            {
                ColLead.Dispose();
                ColLead = null;
            }
            if (ColRhythm != null)
            {
                ColRhythm.Dispose();
                ColRhythm = null;
            }
            if (ColVocal != null)
            {
                ColVocal.Dispose();
                ColVocal = null;
            }
        }

        public ThemeImage ColBass { get; set; }
        public ThemeImage ColLead { get; set; }
        public ThemeImage ColRhythm { get; set; }
        public ThemeImage ColVocal { get; set; }
        public ThemeImage FilterOn { get; set; }
        public ThemeImage FilterOff { get; set; }
        public Color ColAvaliable { get; set; }
        public Color ColNotAvaliable { get; set; }
    }

    [ThemeKey("Toolbar Images")]
    public class ThemeToolbarImages : ThemeSetting, IDisposable
    {
        public ThemeToolbarImages()
        {
            Play = new ThemeImage();
            Play.SetImage(Properties.Resources.ap_play);
            Stop = new ThemeImage();
            Stop.SetImage(Properties.Resources.ap_stop);
            Export = new ThemeImage();
            Export.SetImage(Properties.Resources.export);
            Launch = new ThemeImage();
            Launch.SetImage(Properties.Resources.launch);
            Backup = new ThemeImage();
            Backup.SetImage(Properties.Resources.backup);
            Upload = new ThemeImage();
            Upload.SetImage(Properties.Resources.backup);
            Help = new ThemeImage();
            Help.SetImage(Properties.Resources.Help);
            Request = new ThemeImage();
        }

        public void Dispose()
        {
            if (Play != null)
            {
                Play.Dispose();
                Play = null;
            }
            if (Stop != null)
            {
                Stop.Dispose();
                Stop = null;
            }
            if (Export != null)
            {
                Export.Dispose();
                Export = null;
            }
            if (Launch != null)
            {
                Launch.Dispose();
                Launch = null;
            }
            if (Backup != null)
            {
                Backup.Dispose();
                Backup = null;
            }

            if (Upload != null)
            {
                Upload.Dispose();
                Upload = null;
            }

            if (Help != null)
            {
                Help.Dispose();
                Help = null;
            }

            if (Request != null)
            {
                Request.Dispose();
                Request = null;
            }
        }

        public ThemeImage Play { get; set; }
        public ThemeImage Stop { get; set; }
        public ThemeImage Export { get; set; }
        public ThemeImage Launch { get; set; }
        public ThemeImage Backup { get; set; }
        public ThemeImage Upload { get; set; }
        public ThemeImage Help { get; set; }
        public ThemeImage Request { get; set; }
    }
}