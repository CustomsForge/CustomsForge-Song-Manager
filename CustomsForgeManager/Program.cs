using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using DLogNet;

namespace CustomsForgeManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // prevent multiple occurrence of this application from running
            if (!Constants.DebugMode)
                if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                    return;

            if (FirstRun() && !Constants.DebugMode)
            {
                if (Directory.Exists(Constants.WorkDirectory))
                {
                    File.Delete(Constants.LogFilePath);
                    File.Delete(Constants.SettingsPath);
                    File.Delete(Constants.SongFilesPath);
                    File.Delete(Constants.SongsInfoPath);
                } 

                using (TextWriter tw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ReleaseNotes.txt"), true))
                {
                    tw.Write("notfirstrun"); // IMPORTANT no CRLF added to end
                    tw.WriteLine(Environment.NewLine + Environment.NewLine + "notfirstrun");  // causes CRLF to be added
                   // tw.Close(); Close not needed when using the "using" statement
                }
            }

            DLogger myLog = new DLogger();
            AppSettings mySettings = new AppSettings();
            myLog.AddTargetFile(mySettings.LogFilePath);

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(myLog, mySettings));
            }
            catch (Exception ex)
            {
                Globals.MyLog.Write(String.Format("{0} {1}", "Exception:", ex.Message));
                Process.Start(Globals.MySettings.LogFilePath);
            }
        }

       public static bool FirstRun()
        {
            if (Path.GetDirectoryName(Application.ExecutablePath) == null)
                throw new Exception("Can not find application directory.");

            if (!File.ReadAllText(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ReleaseNotes.txt")).Contains("notfirstrun"))
                return true;

            return false;
        }

    }
}
