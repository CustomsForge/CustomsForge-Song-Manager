using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using GenTools;
using DF.WinForms.ThemeLib;
using DLogNet;
using DataGridViewTools;
using Mutex = System.Threading.Mutex;
using System.Threading;

#if WINDOWS

#else
using Mutex = DF.WinForms.ThemeLib.Mutex;
#endif

namespace CustomsForgeSongManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
         {
            // must come first if it is used
            Application.SetCompatibleTextRenderingDefault(false);

            // prevent multiple occurrence of this application from running
            // using a global mutex for the installer
            using (Mutex mutex = new Mutex(false, @"Global\CUSTOMSFORGESONGMANAGER"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    var pHandle = CrossPlatform.GetHandleFromProcessName(Application.ExecutablePath);
                    if (pHandle != IntPtr.Zero)
                    {
                        //restore the window if minimized
                        CrossPlatform.ShowWindow(pHandle, 9);
                        //bring it to the front
                        CrossPlatform.SetForegroundWindow(pHandle);
                    }
                    return;
                }

                RunApp();
            }
        }

        private static void RunApp()
        {
            // check for correct version of .NET
            if (!File.Exists(Constants.SongsInfoPath))
                SysExtensions.IsDotNet4();

            DLogger myLog = new DLogger();
            myLog.AddTargetFile(AppSettings.Instance.LogFilePath);

            if (Constants.DebugMode) // have VS handle the exception
            {
                Application.EnableVisualStyles();
                // this is throwing an error so commented out and moved
                //  Application.SetCompatibleTextRenderingDefault(false);    
                Application.Run(new frmMain(myLog));
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    // Application.SetCompatibleTextRenderingDefault(false);
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

        public static bool RemoveGridSettings()
        {
            if (String.IsNullOrEmpty(Globals.DgvCurrent.Name))
                Globals.DgvCurrent.Name = "dgvSongsMaster";

            if (!File.Exists(Constants.GridSettingsPath))
                return false;

            using (var fs = File.OpenRead(Constants.GridSettingsPath))
            {
                try
                {
                    var gs = fs.DeserializeXml<RADataGridViewSettings>();
                    if (gs.LoadedVersion == null)
                        return true;
                    if (gs.LoadedVersion != RADataGridViewSettings.gridViewSettingsVersion)
                        return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }

            return false;
        }
    }
}