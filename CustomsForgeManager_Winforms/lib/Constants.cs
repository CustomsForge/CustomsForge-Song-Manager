using System;
using System.IO;

namespace CustomsForgeManager_Winforms.lib
{
    public static class Constants
    {
        public static string DefaultWorkingDirectory { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFM"); } }
        public static string DefaultLogName { get { return Path.Combine(DefaultWorkingDirectory, "debug.log"); } }

        public static string DefaultInfoURL
        {
            get
            {
#if (DEBUG)
                return @"http://ignition.dev.customsforge.com/api/cdlc";
#else
                return @"http://ignition.dev.customsforge.com/api/cdlc";
#endif
                //return @"http://ignition.localhost:88/api/cdlc";
            }
        }

        public static string DefaultDetailsURL
        {
            get { return @"http://ignition.dev.customsforge.com/api/details"; }
        }

        public static string DefaultCFSongUrl
        {
            get { return @"http://customsforge.com/page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/"; }
        }
    }
}
