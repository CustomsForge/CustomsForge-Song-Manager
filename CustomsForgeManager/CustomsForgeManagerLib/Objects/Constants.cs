using System;
using System.Globalization;
using System.IO;
using System.Reflection;

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

        public static string ApplicationName { get { return "CustomsForge Song Manager"; } }
        public static string ApplicationVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public static string WorkDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFM"); } }
        public static string LogFilePath { get { return Path.Combine(WorkDirectory, "debug.log"); } }
        public static string SettingsPath { get { return Path.Combine(WorkDirectory, "settings.xml"); } }
        public static string SongsInfoPath { get { return Path.Combine(WorkDirectory, "songsinfo.xml"); } }
        public static string SongFilesPath { get { return Path.Combine(WorkDirectory, "songfiles.xml"); } }

        public static string DefaultInfoURL
        {
            get
            {
#if (DEBUG)
                return @"http://ignition.dev.customsforge.com/api/search";
#else
                return @"http://ignition.dev.customsforge.com/api/search";
#endif
                //return @"http://ignition.customsforge.com/api/search";
            }
        }

        public static string DefaultAuthURL
        {
            get { return @"http://ignition.dev.customsforge.com/api/auth"; }
        }

        public static string DefaultDetailsURL
        {
            get { return @"http://ignition.dev.customsforge.com/api/details"; }
        }

        public static string DefaultCFSongUrl
        {
            get { return @"http://customsforge.com/page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/"; }
        }

        public static string CustomVersion()
        {
            // this can be customized easily wheres ApplicationVersion can not
            return String.Format("{0}.{1}.{2}.{3}",
                                 Assembly.GetExecutingAssembly().GetName().Version.Major,
                                 Assembly.GetExecutingAssembly().GetName().Version.Minor,
                                 Assembly.GetExecutingAssembly().GetName().Version.Build,
                                 Assembly.GetExecutingAssembly().GetName().Version.Revision);
        }

      
    }
}
