using System;
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
        public static string WorkDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFM"); } }
        public static string LogFilePath { get { return Path.Combine(WorkDirectory, "debug.log"); } }
        public static string SettingsPath { get { return Path.Combine(WorkDirectory, "settings.xml"); } }
        public static string SongsInfoPath { get { return Path.Combine(WorkDirectory, "songsinfo.xml"); } }
        public static string ApplicationDirectory{get { return Path.GetDirectoryName(Application.ExecutablePath); }}
        public static string AppIdFilePath { get { return Path.Combine(ApplicationDirectory, "RocksmithToolkitLib.SongAppId.xml"); } }
        public static string TuningDefFilePath { get { return Path.Combine(ApplicationDirectory, "RocksmithToolkitLib.TuningDefinition.xml"); } }

        #region URL constants
        public const string CustomsForgeURL= "http://customsforge.com/";
        public const string CustomsForgeUserURL_Format = CustomsForgeURL + "user/{0}/";
        public const string CustomsForgeDonateURL= "http://goo.gl/jREeJF"; 
        public const string IgnitionURL= "http://ignition.customsforge.com/";
        public const string EOFURL = IgnitionURL + "/eof"; 
        public const string RequestURL = "http://requests.customsforge.com/";
        public const string DefaultInfoURL =       
#if (DEBUG)
                 @"http://ignition.dev.customsforge.com/api/search";
#else
                 @"http://ignition.dev.customsforge.com/api/search";
#endif
                //return @"http://ignition.customsforge.com/api/search";
        public const string DefaultAuthURL = "http://ignition.dev.customsforge.com/api/auth";
        public const string DefaultDetailsURL = "http://ignition.dev.customsforge.com/api/details";         
        public const string DefaultCFSongUrl =  CustomsForgeURL +  "page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/"; 
        

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
