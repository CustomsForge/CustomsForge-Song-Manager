using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    public static class Constants
    {

#if (DEBUG)
        public static bool DebugMode { get { return true; } }
#else
        public static bool DebugMode { get { return false; } }
#endif

        public const string RS1COMP = "rs1compatibility";
 
        public const string ApplicationName = "CustomsForge Song Manager";
        public static string ApplicationVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public static string WorkDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFSM"); } }
        public static string LogFilePath { get { return Path.Combine(WorkDirectory, "debug.log"); } }
        public static string SettingsPath { get { return Path.Combine(WorkDirectory, "settings.xml"); } }
        public static string GridSettingsPath { get { return Path.Combine(WorkDirectory, "gridSettings.xml"); } }
        public static string SongsInfoPath { get { return Path.Combine(WorkDirectory, "songsinfo.xml"); } }
        public static string ApplicationDirectory { get { return Path.GetDirectoryName(Application.ExecutablePath); } }
        public static string AppIdFilePath { get { return Path.Combine(ApplicationDirectory, "RocksmithToolkitLib.SongAppId.xml"); } }
        public static string TuningDefFilePath { get { return Path.Combine(ApplicationDirectory, "RocksmithToolkitLib.TuningDefinition.xml"); } }

        public static string TaggerWorkingFolder { get { return Path.Combine(WorkDirectory, "Tagger"); } }
        public static string TaggerTemplatesFolder { get { return Path.Combine(TaggerWorkingFolder, "templates"); } }
        public static string TaggerExtractedFolder { get { return Path.Combine(TaggerWorkingFolder, "extracted"); } }
        public static string TaggerPreviewsFolder { get { return Path.Combine(TaggerWorkingFolder, "previews"); } }

        public static string CachePsarcPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "cache.psarc"); } }
        public static string RS1PackPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.psarc"); } }
        public static string RS1DLCPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.psarc"); } }
        public static string CachePsarcBackupPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "backup", "cache.psarc.backup"); } }
        public static string RS1PackBackupPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", "rs1compatibilitydisc_p.psarc.backup"); } }
        public static string RS1DLCBackupPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "dlc", "rs1compatibilitydlc_p.psarc.backup"); } }
        public static string BackupFolderPath { get { return Path.Combine(AppSettings.Instance.RSInstalledDir, "backup"); } }

        public static string CFEWorkingFolder { get { return Path.Combine(WorkDirectory, "CachePsarcEditor"); } }
        public static string ExtractedRSPackPath { get { return Path.Combine(CFEWorkingFolder, "cache_Pc"); } }
        public static string ExtractedRS1PackPath { get { return Path.Combine(CFEWorkingFolder, "rs1compatibilitydisc_p_Pc", "manifests", "songs_rs1disc"); } }
        public static string ExtractedRS1DLCPackPath { get { return Path.Combine(CFEWorkingFolder, "rs1compatibilitydlc_p_Pc", "manifests", "songs_rs1dlc"); } }

        public static string ManifestsFolderPath { get { return Path.Combine("manifests", "songs"); } }
        public static string Cache7zPath { get { return Path.Combine(ExtractedRSPackPath, "cache7.7z"); } }
        public static string RSSongsFilePath { get { return Path.Combine("manifests", "songs", "songs.hsan"); } }
        public static string RS1SongsFilePath { get { return Path.Combine(ExtractedRSPackPath, "songs_rs1disc.hsan"); } }
        public static string RS1DLCSongsFilePath { get { return Path.Combine(ExtractedRSPackPath, "songs_rs1dlc.hsan"); } }

        #region URL constants
        public const string CustomsForgeURL = "http://customsforge.com/";
        public const string CustomsForgeUserURL_Format = CustomsForgeURL + "user/{0}/";
        public const string IgnitionURL = "http://ignition.customsforge.com/";
        public const string EOFURL = IgnitionURL + "/eof";
        public const string RequestURL = "http://requests.customsforge.com/";
        public const string DefaultInfoURL =
#if (DEBUG)
                 @"http://ignition.dev.customsforge.com/api/search";
#else
                 @"http://ignition.dev.customsforge.com/api/search";
#endif


        public const string DefaultAuthURL = "http://ignition.dev.customsforge.com/api/auth";
        public const string DefaultDetailsURL = "http://ignition.dev.customsforge.com/api/details";
        public const string DefaultCFSongUrl = CustomsForgeURL + "page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/";


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
