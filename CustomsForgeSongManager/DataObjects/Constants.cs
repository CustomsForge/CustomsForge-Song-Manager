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

        public static readonly string RS1COMP = "rs1compatibility";
        public static readonly string SONGPACK = "songpack";
        public static readonly string ABVSONGPACK = "_sp_";
        public static readonly string ApplicationName = "CustomsForge Song Manager";
        public static Font OfficialDLCFont { get { return new Font("Arial", 8, FontStyle.Bold | FontStyle.Italic); } }
        public static string ApplicationVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public static string WorkFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFSM"); } }
        public static string ThemeFolder { get { return Path.Combine(WorkFolder, "Themes"); } }
        public static string LogFilePath { get { return Path.Combine(WorkFolder, "debug.log"); } }
        public static string SettingsPath { get { return Path.Combine(WorkFolder, "cfsm.Settings.xml"); } }
        public static string SongsInfoPath { get { return Path.Combine(WorkFolder, "songs.Info.xml"); } }
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

        public static string CachePsarcPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "cache.psarc"); } }

        public static string Rs1DiscPsarcPath
        {
            get
            {
                var dlcPath = Rs2DlcFolder;
                // TODO: determine if GetFiles is case sensitive
                var files = Directory.GetFiles(dlcPath, "rs1compatibilitydisc_p.psarc", SearchOption.AllDirectories);
                if (files.Length > 0)
                    return files[0];
                return Path.Combine(dlcPath, "rs1compatibilitydisc_p.psarc");
            }
        }

        public static string Rs1DlcPsarcPath
        {
            get
            { 
                var dlcPath = Rs2DlcFolder;
                // TODO: determine if GetFiles is case sensitive
                var files = Directory.GetFiles(dlcPath, "rs1compatibilitydlc_p.psarc", SearchOption.AllDirectories);
                if (files.Length > 0)
                    return files[0];
                return Path.Combine(dlcPath, "rs1compatibilitydlc_p.psarc");
            }
        }

        // write access to the Steam RSInstallDir is provided by the code 
        public static string Rs2DlcFolder { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc"); } }
        public static string Rs2BackupFolder { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "backup"); } }
        public static string Rs1DiscPsarcBackupPath { get { return Path.Combine(Rs2BackupFolder, "rs1compatibilitydisc_p.psarc.org"); } }
        public static string Rs1DlcPsarcBackupPath { get { return Path.Combine(Rs2BackupFolder, "rs1compatibilitydlc_p.psarc.org"); } }
        public static string CachePsarcBackupPath { get { return Path.Combine(Rs2BackupFolder, "cache.psarc.org"); } }

        public static string CpeWorkFolder { get { return Path.Combine(WorkFolder, "SongPacks"); } }
        public static string ExtractedSongsHsanPath { get { return Path.Combine(CpeWorkFolder, "songs.hsan"); } }
        public static string ExtractedRs1DiscHsanPath { get { return Path.Combine(CpeWorkFolder, "songs_rs1disc.hsan"); } }
        public static string ExtractedRs1DlcHsanPath { get { return Path.Combine(CpeWorkFolder, "songs_rs1dlc.hsan"); } }

        public static string Cache7zPath { get { return Path.Combine(CpeWorkFolder, "cache_Pc", "cache7.7z"); } }
        public static string CachePcPath { get { return Path.Combine(CpeWorkFolder, "cache_Pc"); } }
        // cache7.7z internal paths uses back slashes (normal path mode)
        public static string SongsHsanInternalPath { get { return Path.Combine("manifests", "songs", "songs.hsan"); } }
        // this is not a mistake archive internal paths use forward slashes (internal path mode)
        public static string SongsRs1DiscInternalPath { get { return @"manifests/songs_rs1disc/songs_rs1disc.hsan"; } }
        public static string SongsRs1DlcInternalPath { get { return @"manifests/songs_rs1dlc/songs_rs1dlc.hsan"; } }

        //Remastered_Folder placed in Rocksmith 2014 root, 'backup' subdirectory
        public static string RemasteredFolder { get { return Path.Combine(Rs2BackupFolder, "remastered"); } }
        public static string RemasteredCorruptFolder { get { return Path.Combine(RemasteredFolder, "corrupt"); } }
        public static string RemasteredOrgFolder { get { return Path.Combine(RemasteredFolder, "original"); } }
        public static string SongPacksFolder { get { return Path.Combine(Rs2BackupFolder, "songpacks"); } }
        public static string SongPacksOrgFolder { get { return Path.Combine(SongPacksFolder, "original"); } }

        #region URL constants

        public const string RSToolkitURL = "https://www.rscustom.net/";
        public const string CustomsForgeURL = "http://customsforge.com/";
        public const string CustomsForgeUserURL_Format = CustomsForgeURL + "user/{0}/";
        public const string IgnitionURL = "http://ignition.customsforge.com/";
        public const string EOFURL = IgnitionURL + "/eof";
        public const string RequestURL = "http://requests.customsforge.com/";
        public const string DefaultAuthURL = "http://ignition.dev.customsforge.com/api/auth";
        public const string DefaultDetailsURL = "http://ignition.dev.customsforge.com/api/details";
        public const string DefaultCFSongUrl = CustomsForgeURL + "page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/";
        public const string DefaultInfoURL =

#if (DEBUG)
 @"http://ignition.dev.customsforge.com/api/search";
#else
 @"http://ignition.dev.customsforge.com/api/search";
#endif

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
