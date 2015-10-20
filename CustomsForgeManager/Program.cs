using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
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

            if (!Constants.DebugMode && FirstRun())
            {
                if (Directory.Exists(Constants.WorkDirectory))
                {
                    File.Delete(Constants.LogFilePath);
                    File.Delete(Constants.SettingsPath);
                    File.Delete(Constants.SongsInfoPath);
                }
                using (TextWriter tw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ReleaseNotes.txt"), true))
                {
                    tw.Write("notfirstrun"); // IMPORTANT no CRLF added to end
                    tw.WriteLine(string.Format("{0}{0}notfirstrun", Environment.NewLine));  // causes CRLF to be added
                    // tw.Close(); Close not needed when using the "using" statement
                }
            }
            RunApp();
        }

        private static void RunApp()
        {
            DLogger myLog = new DLogger();
            myLog.AddTargetFile(AppSettings.Instance.LogFilePath);

            if (Constants.DebugMode)// have VS handle the exception
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(myLog));
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmMain(myLog));
                }
                catch (Exception ex)
                {
                    //a more detailed exception message
                    var exMessage = String.Format("Exception({0}): {1}", ex.GetType().Name, ex.Message);
                    if (ex.InnerException != null)
                        exMessage += String.Format(", InnerException({0}): {1}", ex.InnerException.GetType().Name, ex.InnerException.Message);
                    Globals.MyLog.Write(exMessage);
                    Process.Start(AppSettings.Instance.LogFilePath);
                }
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
