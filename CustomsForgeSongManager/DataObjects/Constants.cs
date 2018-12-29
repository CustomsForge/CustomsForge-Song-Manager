using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CustomsForgeSongManager.DataObjects
{
    public static class Constants
    {
#if (DEBUG)
        public static bool DebugMode
        {
            get { return true; }
        }
#else
        public static bool DebugMode { get { return false; } }
#endif

        public static readonly string TKI_ARRID = "(Arrangement ID by CFSM)";
        public static readonly string TKI_DDC = "(DD by CFSM)";
        public static readonly string TKI_MAX5 = "(Max5 by CFSM)";
        public static readonly string TKI_REMASTER = "(Remastered by CFSM)";
        public static readonly string TKI_PITCHSHIFT = "(Pitch Shifted by CFSM)";

        public static string EXT_BAK = ".bak";
        public static string EXT_COR = ".cor";
        public static string EXT_MAX = ".max";
        public static string EXT_ORG = ".org";
        public static string EXT_DUP = ".dup";

        public static string GWORKER_REPAIR = "repairing";
        public static string GWORKER_ACHRIVE = "archiving";
        public static string GWORKER_PITCHSHIFT = "pitch shifting";
        public static string GWORKER_ORGANIZE = "organizing";
        public static string GWORKER_TAG = "tagging";
        public static string GWORKER_UNTAG = "untagging";

        public static readonly string BASESONGS = "songs.psarc";
        public static readonly string RS1COMP = "rs1compatibility";
        public static readonly string SONGPACK = "songpack";
        public static readonly string ABVSONGPACK = "_sp_"; // abbreviation for songpack
        public static readonly string ApplicationName = "CustomsForge Song Manager";

        public static Font OfficialDLCFont { get { return new Font("Arial", 8, FontStyle.Bold | FontStyle.Italic); } }
        public static string AppTitle { get; set; }
        public static string ApplicationVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public static string WorkFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFSM"); } }
        public static string TempWorkFolder { get { return Path.Combine(Path.GetTempPath(), "CFSM"); } }
        public static string ThemeFolder { get { return Path.Combine(WorkFolder, "Themes"); } }
        public static string LogFilePath { get { return Path.Combine(WorkFolder, "debug.log"); } }
        public static string AppSettingsPath { get { return Path.Combine(WorkFolder, "appSettings.xml"); } }
        public static string SongsInfoPath { get { return Path.Combine(WorkFolder, "songsInfo.xml"); } }
        public static string ProfileDataPath { get { return Path.Combine(WorkFolder, "profileData.xml"); } }
        public static string GridSettingsFolder { get { return Path.Combine(WorkFolder, "DgvSettings"); } }
        public static string GridSettingsPath { get { return Path.Combine(GridSettingsFolder, String.Format("{0}{1}", Globals.DgvCurrent.Name, ".xml")); } }
        public static string ApplicationFolder { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public static string AppIdFilePath { get { return Path.Combine(ApplicationFolder, "RocksmithToolkitLib.SongAppId.xml"); } }
        public static string TuningDefFilePath { get { return Path.Combine(ApplicationFolder, "RocksmithToolkitLib.TuningDefinition.xml"); } }
        public static string AudioCacheFolder { get { return Path.Combine(WorkFolder, "AudioCache"); } }
        public static string ProfileBackupsFolder { get { return Path.Combine(WorkFolder, "ProfileBackups"); } }
        public static string TaggerWorkingFolder { get { return Path.Combine(WorkFolder, "Tagger"); } }
        public static string TaggerTemplatesFolder { get { return Path.Combine(TaggerWorkingFolder, "templates"); } }
        public static string TaggerExtractedFolder { get { return Path.Combine(TaggerWorkingFolder, "extracted"); } }
        public static string TaggerPreviewsFolder { get { return Path.Combine(TaggerWorkingFolder, "previews"); } }

        public static bool OnMac
        {
            get
            {
                if (Rs2DlcFolder.Contains("Application Support")) //Rather unreliable, but other methods have been proven not to work in all cases on Wineskin
                {
                    AppSettings.Instance.MacMode = true;
                    return true;
                }

                // run Mac compatiblity mode even when on a PC
                if (AppSettings.Instance.MacMode)
                    return true;

                return false;
            }
        }

        public static string EnabledExtension
        {
            get
            {
                if (OnMac)
                    return "_m.psarc";
                else
                    return "_p.psarc";
            }
        }

        public static string DisabledExtension
        {
            get
            {
                if (OnMac)
                    return "_m.disabled.psarc";
                else
                    return "_p.disabled.psarc";
            }
        }

        public static string Rs1DiscPsarcPath
        {
            get
            {
                var rs1PackName = "rs1compatibilitydisc" + EnabledExtension;
                var dlcPath = Rs2DlcFolder;

                // TODO: determine if GetFiles is case sensitive
                var files = Directory.GetFiles(dlcPath, rs1PackName, SearchOption.AllDirectories);
                if (files.Length > 0)
                    return files[0];
                return Path.Combine(dlcPath, rs1PackName);
            }
        }

        public static string Rs1DlcPsarcPath
        {
            get
            {
                var rs1DlcPackName = "rs1compatibilitydlc" + EnabledExtension;
                var dlcPath = Rs2DlcFolder;

                // TODO: determine if GetFiles is case sensitive
                var files = Directory.GetFiles(dlcPath, rs1DlcPackName, SearchOption.AllDirectories);
                if (files.Length > 0)
                    return files[0];
                return Path.Combine(dlcPath, rs1DlcPackName);
            }
        }

        // write access to the Steam RSInstallDir is provided by the code 
        public static string Rs2DlcFolder { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"); } }
        public static string Rs2CdlcFolder { get { return Path.Combine(Rs2DlcFolder, "cdlc"); } }
        public static string CachePsarcPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "cache.psarc"); } }
        public static string SongsPsarcPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "songs.psarc"); } }

        // not a good practice for app to use rocksmith2014 folder because of issues with OS Permissions and some AV
        [Obsolete("Depricated, please use 'My Documents/CFSM' folder", false)]
        public static string Rs2CfsmFolder { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "cfsm"); } }

        public static string Rs2OriginalsFolder { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "originals"); } }
        public static string Rs1DiscPsarcBackupPath { get { return Path.Combine(Rs2OriginalsFolder, "rs1compatibilitydisc_p.org.psarc"); } }
        public static string Rs1DlcPsarcBackupPath { get { return Path.Combine(Rs2OriginalsFolder, "rs1compatibilitydlc_p.org.psarc"); } }
        public static string CachePsarcBackupPath { get { return Path.Combine(Rs2OriginalsFolder, "cache.org.psarc"); } }
        public static string ExtractedSongsHsanPath { get { return Path.Combine(SongPacksFolder, "songs.hsan"); } }
        public static string ExtractedRs1DiscHsanPath { get { return Path.Combine(SongPacksFolder, "songs_rs1disc.hsan"); } }
        public static string ExtractedRs1DlcHsanPath { get { return Path.Combine(SongPacksFolder, "songs_rs1dlc.hsan"); } }
        // TODO: address Mac unpacked directory, i.e., cache.psarc_RS2014_Mac
        public static string Cache7zPath { get { return Path.Combine(SongPacksFolder, "cache.psarc_RS2014_Pc", "cache7.7z"); } }
        public static string CachePcPath { get { return Path.Combine(SongPacksFolder, "cache.psarc_RS2014_Pc"); } } 
        // cache7.7z internal paths uses back slashes (normal path mode)
        public static string SongsHsanInternalPath { get { return Path.Combine("manifests", "songs", "songs.hsan"); } }
        // this is not a mistake archive internal paths use forward slashes (internal path mode)
        public static string SongsRs1DiscInternalPath { get { return @"manifests/songs_rs1disc/songs_rs1disc.hsan"; } }
        public static string SongsRs1DlcInternalPath { get { return @"manifests/songs_rs1dlc/songs_rs1dlc.hsan"; } }

        // not a good practice to use Rocksmith 2014 root for CFSM content     
        public static string SongPacksFolder { get { return Path.Combine(WorkFolder, "SongPacks"); } }
        public static string BackupsFolder { get { return Path.Combine(WorkFolder, "Backups"); } }
        public static string DuplicatesFolder { get { return Path.Combine(WorkFolder, "Duplicates"); } }
        public static string RemasteredFolder { get { return Path.Combine(WorkFolder, "Remastered"); } }
        public static string RepairsErrorLogPath { get { return Path.Combine(RemasteredFolder, "remastered_error.log"); } }
        public static string RemasteredArcFolder { get { return Path.Combine(RemasteredFolder, "archives"); } }
        public static string RemasteredCorFolder { get { return Path.Combine(RemasteredFolder, "corrupt"); } }
        public static string RemasteredOrgFolder { get { return Path.Combine(RemasteredFolder, "original"); } }
        public static string RemasteredMaxFolder { get { return Path.Combine(RemasteredFolder, "maxfive"); } }
        public static string QuarantineFolder { get { return Path.Combine(Constants.WorkFolder, "Quarantine"); } }

        #region URL constants

        public const string RSToolkitURL = "https://www.rscustom.net/";
        public const string CustomsForgeURL = "http://customsforge.com/";
        public const string CustomsForgeUserURL_Format = CustomsForgeURL + "user/{0}/";
        public const string EOFURL = IgnitionURL + "/eof";
        public const string RequestURL = "http://requests.customsforge.com/"; // discontinued
        public const string DefaultCFSongUrl = CustomsForgeURL + "page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/";
        // TODO: update these 
        public const string IgnitionURL = "http://ignition.customsforge.com/"; // valid link to ignition search page
        public const string DefaultInfoURL = "http://ignition.dev.customsforge.com/api/search"; // dead
        public const string DefaultDetailsURL = "http://ignition.dev.customsforge.com/api/details"; // dead
        public const string DefaultAuthURL = "http://ignition.dev.customsforge.com/api/auth"; // dead

        #endregion

        public static string CustomVersion()
        {
            // this can be customized
            return String.Format("{0}.{1}.{2}.{3}",
                                 Assembly.GetExecutingAssembly().GetName().Version.Major,
                                 Assembly.GetExecutingAssembly().GetName().Version.Minor,
                                 Assembly.GetExecutingAssembly().GetName().Version.Build,
                                 Assembly.GetExecutingAssembly().GetName().Version.Revision);
        }
    }
}
