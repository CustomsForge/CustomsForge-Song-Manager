using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using DLogNet;
using System.Threading;
using System.Linq;

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
            //using a global mutex for the installer
            using (Mutex mutex = new Mutex(false, @"Global\CUSTOMSFORGESONGMANAGER"))
            {
                if (!mutex.WaitOne(0, false))
                {                    
                    Process proc = Process.GetProcesses().Where(
                        p => Path.GetFileName(p.ProcessName) == 
                            Path.GetFileName(Path.GetFileNameWithoutExtension(Application.ExecutablePath))).FirstOrDefault();

                    if (proc != null)
                    {
                        //restore the window if minimized
                        ShowWindow(proc.MainWindowHandle, 9);
                        //bring it to the front
                        SetForegroundWindow(proc.MainWindowHandle);
                    }
                    return;
                }

                if (!Directory.Exists(Constants.WorkDirectory))
                    Directory.CreateDirectory(Constants.WorkDirectory);


                if (!Constants.DebugMode && FirstRun())
                {
                    if (Directory.Exists(Constants.WorkDirectory))
                    {
                        File.Delete(Constants.LogFilePath);
                        File.Delete(Constants.SettingsPath);
                        File.Delete(Constants.SongsInfoPath);
                    }

                    using (var tw = File.CreateText(Path.Combine(Constants.WorkDirectory, string.Format("firstrun.dat"))))
                    {
                        tw.Write(Constants.ApplicationVersion);
                    }
                }
                RunApp();
            }
        }


        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

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
            var txtFile = Path.Combine(Constants.WorkDirectory, string.Format("firstrun.dat"));
            if (!File.Exists(txtFile))
                return true;

            if (!File.ReadAllText(txtFile).Contains(Constants.ApplicationVersion))
                return true;
            return false;
        }

    }
}
