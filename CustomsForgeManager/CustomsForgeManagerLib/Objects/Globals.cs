using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.UControls;
using DLogNet;
using DataGridViewTools;
using RocksmithToolkitLib;
using System;
using CustomsForgeManager.CustomsForgeManagerLib.UITheme;


namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    public class ScannerEventHandler : EventArgs
    {
        public ScannerEventHandler(bool isScaning)
        {
            IsScanning = isScaning;
        }

        public bool IsScanning
        {
            get;
            private set;
        }
    }

    static class Globals
    {
        internal enum Tristate : byte
        {
            False = 0,
            True = 1,
            Cancelled = 2
        }

        // application data variables
        private static About _about;
        private static DataGridView _dgvSongs;
        private static Duplicates _duplicates;
        private static Dictionary<string, SongData> _outdatedSongList;
        private static Renamer _renamer;
        private static SetlistManager _SetlistManager;
        private static CachePsarcEditor _cachePsarcEditor;
        private static Settings _settings;
        private static BindingList<SongData> _songCollection;
        private static SongManager _songManager;
        private static DF.WinForms.ThemeLib.Theme _theme;
 		private static SongTagger _tagger;


        public static Random random = new Random();

        public static event EventHandler<ScannerEventHandler> OnScanEvent;

        private static bool FIsScanning;
        public static bool IsScanning
        {
            get { return FIsScanning; }
            set
            {
                if (FIsScanning != value)
                {
                    FIsScanning = value;
                    if (OnScanEvent != null)
                        OnScanEvent(null, new ScannerEventHandler(FIsScanning));
                }
            }
        }

        public static CFMAudioTools.AudioEngine AudioEngine
        {
            get { return CFMAudioTools.AudioEngine.GetDefaultEngine(); }
        }

        public static DF.WinForms.ThemeLib.Theme CFMTheme { get { return _theme ?? (_theme = new CFSMTheme()); ; } set { _theme = value; } }
 
 
        public static About About
        {
            get { return _about ?? (_about = new About()); }
            set { _about = value; }
        }


        public static DataGridView DgvSongs
        {
            get { return _dgvSongs ?? (_dgvSongs = new DataGridView()); }
            set { _dgvSongs = value; }
        }

        public static Duplicates Duplicates
        {
            get { return _duplicates ?? (_duplicates = new Duplicates()); }
            set { _duplicates = value; }
        }

        public static List<TuningDefinition> TuningXml { get; set; }
        public static DLogger MyLog { get; set; }
        // public static AppSettings MySettings { get; set; }
        public static NotifyIcon Notifier { get; set; }

        public static Dictionary<string, SongData> OutdatedSongList
        {
            get { return _outdatedSongList ?? (_outdatedSongList = new Dictionary<string, SongData>()); }
            set { _outdatedSongList = value; }
        }

        public static Renamer Renamer
        {
            get { return _renamer ?? (_renamer = new Renamer()); }
            set { _renamer = value; }
        }
#if TAGGER
        public static SongTagger Tagger
        {
            get { return _tagger ?? (_tagger = new SongTagger()); }
            set { _tagger = value; }
        }
#endif
        public static CachePsarcEditor CachePsarcEditor
        {
            get { return _cachePsarcEditor ?? (_cachePsarcEditor = new CachePsarcEditor()); }
            set { _cachePsarcEditor = value; }
        }

        public static bool RescanDuplicates { get; set; }
        public static bool RescanRenamer { get; set; }
        public static bool RescanSetlistManager { get; set; }
        public static bool RescanSongManager { get; set; }
        public static bool ReloadDuplicates { get; set; }
        public static bool ReloadRenamer { get; set; }
        public static bool ReloadSetlistManager { get; set; }
        public static bool ReloadSongManager { get; set; }
        public static bool ReloadCachePsarcEditor { get; set; }

        public static SetlistManager SetlistManager
        {
            get { return _SetlistManager ?? (_SetlistManager = new SetlistManager()); }
            set { _SetlistManager = value; }
        }

        public static Settings Settings
        {
            get { return _settings ?? (_settings = new Settings()); }
            set { _settings = value; }
        }

        public static BindingList<SongData> SongCollection
        {
            get { return _songCollection ?? (_songCollection = new BindingList<SongData>()); }
            set { _songCollection = value; }
        }

        public static SongManager SongManager
        {
            get { return _songManager ?? (_songManager = new SongManager()); }
            set { _songManager = value; }
        }

        public static bool CancelBackgroundScan { get; set; }

        public static ToolStripStatusLabel TsLabel_Cancel { get; set; }
        public static ToolStripStatusLabel TsLabel_DisabledCounter { get; set; }
        public static ToolStripStatusLabel TsLabel_MainMsg { get; set; }
        public static ToolStripStatusLabel TsLabel_StatusMsg { get; set; }
        public static ToolStripProgressBar TsProgressBar_Main { get; set; }


        public static CustomsForgeManager.Forms.frmMain MainForm
        {
            get { return (CustomsForgeManager.Forms.frmMain)Application.OpenForms["frmMain"]; }
        }

        public static IMainForm iMainForm
        {
            get { return (IMainForm)Application.OpenForms["frmMain"]; }
        }


        public static Tristate WorkerFinished { get; set; } // True = 0, False = 1, Cancelled = 2

        public static void Log(string message)
        {
            MyLog.Write(message);
        }

        public static void DebugLog(string message)
        {
            if (Constants.DebugMode)
                Log(message);
        }

        public static void ResetToolStripGlobals()
        {
            TsProgressBar_Main.Value = 0;
            TsLabel_MainMsg.Visible = false;
            TsLabel_StatusMsg.Visible = false;
            TsLabel_Cancel.Visible = false;
            TsLabel_Cancel.Text = "Cancel";
            TsLabel_DisabledCounter.Visible = false;
        }

    }
}
