using System;

namespace CustomsForgeManager_Winforms.Utilities
{
    public static class Constants
    {
        public static string DefaultWorkingDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CFM"; } }
        public static string DefaultLogName { get { return DefaultWorkingDirectory + @"\debug.log"; } }

        public static string DefaultInfoURL
        {
            get
            {
                return @"http://ignition.dev.customsforge.com/api/cdlc";
                //return @"http://ignition.localhost:88/api/cdlc";
            }
        }

        public static string DefaultDetailsURL
        {
            get { return @"http://ignition.dev.customsforge.com/api/details"; }
        }
    }
}
