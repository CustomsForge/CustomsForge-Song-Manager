using System;

namespace CustomsForgeManager_Winforms.Utilities
{
    public static class Constants
    {
        public static string DefaultWorkingDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CFM"; } }
        public static string DefaultLogName { get { return DefaultWorkingDirectory + @"\debug.log"; } }
        public static string DefaultRocksmithPath { get { return @"C:\Program Files (x86)\Steam\steamapps\common\Rocksmith2014"; } }
    }
}
