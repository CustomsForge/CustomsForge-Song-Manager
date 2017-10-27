using System;
using System.Diagnostics;
using System.IO;
using GenTools;

namespace CustomsForgeSongManager.LocalTools
{
    class VersionInfo
    {
        /// <summary>
        /// Creates the VersionInfo.txt file used by InnoSetup 
        /// </summary>
        public static void CreateVersionInfo()
        {
            // only is performed when IDE Mode is running
            if (!GenExtensions.IsInDesignMode)
            {
                // MessageBox.Show("The debugger is not attached");
                return;
            }

            const string relNotesFile = "ReleaseNotes.txt";
            const string verInfoFile = "VersionInfo.txt";

            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var relNotesPath = Path.Combine(projectDir, relNotesFile);
            var verInfoPath = Path.Combine(Path.GetDirectoryName(projectDir), "ThirdParty", verInfoFile);

            if (!Directory.Exists(Path.GetDirectoryName(verInfoPath)))
                throw new Exception("Directory: " + Path.GetDirectoryName(projectDir)+ "\\ThirdParty can not be found");

            if (!File.Exists(relNotesPath))
                throw new Exception("ReleaseNotes not found");

            var txt = GenExtensions.GetFullAppVersion();
            Debug.WriteLine("Current Application Version: " + txt);
            File.WriteAllText(verInfoPath, txt);
            Debug.WriteLine("CreateVersionInfo was sucessful");
        }
    }
}

