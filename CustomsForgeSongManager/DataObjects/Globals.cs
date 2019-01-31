using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CFSM.AudioTools;
using CustomsForgeSongManager.Forms;
using CustomsForgeSongManager.LocalTools;
using CustomsForgeSongManager.UControls;
using CustomsForgeSongManager.UITheme;
using DF.WinForms.ThemeLib;
using DLogNet;
using System;
using RocksmithToolkitLib.XmlRepository;
using System.Reflection;
using System.Threading.Tasks;


namespace CustomsForgeSongManager.DataObjects
{
    public class ScannerEventHandler : EventArgs
    {
        public ScannerEventHandler(bool isScaning)
        {
            IsScanning = isScaning;
        }

        public bool IsScanning { get; private set; }
    }

    internal static class Globals
    {
        [Obfuscation(Exclude = false, Feature = "-rename")]
        internal enum Tristate : byte
        {
            False = 0,
            True = 1,
            Cancelled = 2
        }

        // application data variables
        private static About _about;
        private static DataGridView _dgvCurrent;
        private static Duplicates _duplicates;
        private static Dictionary<string, SongData> _outdatedSongList;
        private static Renamer _renamer;
        private static SetlistManager _setlistManager;
        private static ProfileSongLists _profileSongLists;
        private static SongPacks _songPacks;
        private static Settings _settings;
        private static BindingList<SongData> _masterCollection;
        private static SongManager _songManager;
        private static ArrangementAnalyzer _arrangementAnalyzer;
        private static Theme _theme;
        private static TaggerTools _tagger;
        private static List<OfficialDLCSong> _oDLCSongList;

        public static Random random = new Random();

        public static event EventHandler<ScannerEventHandler> OnScanEvent;

        private static bool _isScanning;

        public static bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (_isScanning != value)
                {
                    _isScanning = value;
                    if (OnScanEvent != null)
                        OnScanEvent(null, new ScannerEventHandler(_isScanning));
                }
            }
        }

        public static AudioEngine AudioEngine
        {
            get { return AudioEngine.GetDefaultEngine(); }
        }

        public static Theme CFMTheme
        {
            get { return _theme ?? (_theme = new CFSMTheme()); }
            set { _theme = value; }
        }

        public static About About
        {
            get { return _about ?? (_about = new About()); }
            set { _about = value; }
        }

        public static DataGridView DgvCurrent
        {
            get { return _dgvCurrent ?? (_dgvCurrent = new DataGridView()); }
            set { _dgvCurrent = value; }
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

        public static List<OfficialDLCSong> OfficialDLCSongList
        {
            get { return _oDLCSongList ?? (_oDLCSongList = new List<OfficialDLCSong>()); }
            set { _oDLCSongList = value; }
        }

        public static Renamer Renamer
        {
            get { return _renamer ?? (_renamer = new Renamer()); }
            set { _renamer = value; }
        }

#if TAGGER
        public static TaggerTools Tagger
        {
            get { return _tagger ?? (_tagger = new TaggerTools()); }
            set { _tagger = value; }
        }
#endif

        public static SongPacks SongPacks
        {
            get { return _songPacks ?? (_songPacks = new SongPacks()); }
            set { _songPacks = value; }
        }

        public static bool RescanDuplicates { get; set; }
        public static bool RescanRenamer { get; set; }
        public static bool RescanSetlistManager { get; set; }
        public static bool RescanProfileSongLists { get; set; }
        public static bool RescanSongManager { get; set; }
        public static bool RescanArrangements { get; set; }
        public static bool ReloadDuplicates { get; set; }
        public static bool ReloadRenamer { get; set; }
        public static bool ReloadSetlistManager { get; set; }
        public static bool ReloadProfileSongLists { get; set; }
        public static bool ReloadSongManager { get; set; }
        public static bool ReloadArrangements { get; set; }
        public static bool ReloadSongPacks { get; set; }

        public static SetlistManager SetlistManager
        {
            get { return _setlistManager ?? (_setlistManager = new SetlistManager()); }
            set { _setlistManager = value; }
        }

        public static ProfileSongLists ProfileSongLists
        {
            get { return _profileSongLists ?? (_profileSongLists = new ProfileSongLists()); }
            set { _profileSongLists = value; }
        }

        public static Settings Settings
        {
            get { return _settings ?? (_settings = new Settings()); }
            set { _settings = value; }
        }

        public static ArrangementAnalyzer ArrangementAnalyzer
        {
            get { return _arrangementAnalyzer ?? (_arrangementAnalyzer = new ArrangementAnalyzer()); }
            set { _arrangementAnalyzer = value; }
        }

        public static BindingList<SongData> MasterCollection
        {
            get { return _masterCollection ?? (_masterCollection = new BindingList<SongData>()); }
            set { _masterCollection = value; }
        }

        public static SongManager SongManager
        {
            get { return _songManager ?? (_songManager = new SongManager()); }
            set { _songManager = value; }
        }

        public static bool CancelBackgroundScan { get; set; }

        public static TextBox TbLog { get; set; }
        public static ToolStripStatusLabel TsLabel_Cancel { get; set; }
        public static ToolStripStatusLabel TsLabel_DisabledCounter { get; set; }
        public static ToolStripStatusLabel TsLabel_MainMsg { get; set; }
        public static ToolStripStatusLabel TsLabel_StatusMsg { get; set; }
        public static ToolStripProgressBar TsProgressBar_Main { get; set; }

        public static frmMain MainForm
        {
            get { return (frmMain)Application.OpenForms["frmMain"]; }
        }

        public static IMainForm iMainForm
        {
            get { return (IMainForm)Application.OpenForms["frmMain"]; }
        }

        public static Tristate WorkerFinished { get; set; }
        // True = 0, False = 1, Cancelled = 2

        public static void Log(string message)
        {
            try
            {
                MyLog.Write(message);
            }
            catch
            {
                // just ignore it
            }
        }

        public static void DebugLog(string message)
        {
            if (Constants.DebugMode)
                Log(message);
        }

        public static void ResetToolStripGlobals()
        {
            try
            {
                TsProgressBar_Main.Value = 0;
                TsLabel_MainMsg.Visible = false;
                TsLabel_StatusMsg.Visible = false;
                TsLabel_Cancel.Visible = false;
                TsLabel_Cancel.Text = "Cancel";
                TsLabel_DisabledCounter.Visible = false;
            }
            catch {/* DO NOTHING */}
        }

        public static void ClearLog()
        {
            TbLog.Clear();
        }

        // Release version = true, Development version = false            
        public static bool PublicRelease
        {
#if !DEBUG
            get { return true; }
#else
            get { return false; }
#endif
        }

        public static bool PrfldbNeedsUpdate { get; set; }
        public static bool PackageRatingNeedsUpdate { get; set; }
        public static bool UpdateInProgress { get; set; }

    }
}