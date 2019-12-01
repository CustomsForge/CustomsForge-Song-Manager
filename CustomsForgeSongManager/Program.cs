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
using RocksmithToolkitLib.Extensions;


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
            // prevent multiple occurrence of this application from running
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
            }

            RunApp();
        }

        private static void RunApp()
        {
            // check for correct version of .NET
            if (!File.Exists(Constants.SongsInfoPath))
                SysExtensions.IsDotNet4();

            DLogger myLog = new DLogger();
            myLog.AddTargetFile(AppSettings.Instance.LogFilePath);
            myLog.Write("==== This is the start of a new CFSM run log =====");

            if (!Constants.DebugMode)
            {
                // non-UI thread exceptions handling.
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    var exception = e.ExceptionObject as Exception;
                    Globals.MyLog.Write(String.Format("<ERROR> Unhandled.Exception:\nSource: {0}\nTarget: {1}\n{2}\n", exception.Source, exception.TargetSite, exception.ToString()));
                    if (MessageBox.Show(String.Format("Unhandled.Exception:\n\n{0}\nPlease send us the {1} file if you need help.  Open log file now?",
                        exception.Message.ToString(), Path.GetFileName(AppSettings.Instance.LogFilePath)),
                        "Please Read This Important Message Completely ...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        Process.Start(AppSettings.Instance.LogFilePath);
                    }
                };

                // UI thread exceptions handling.
                Application.ThreadException += (s, e) =>
                {
                    var exception = e.Exception;
                    Globals.MyLog.Write(String.Format("<ERROR> Application.ThreadException\nSource: {0}\nTarget: {1}\n{2}\n", exception.Source, exception.TargetSite, exception.ToString()));

                    if (MessageBox.Show(String.Format("Application.ThreadException:\n\n{0}\nPlease send us the {1} file if you need help.  Open log file now?",
                        exception.Message.ToString(), Path.GetFileName(AppSettings.Instance.LogFilePath)),
                        "Please Read This Important Message Completely ...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        Process.Start(AppSettings.Instance.LogFilePath);
                    }
                };
            }

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.Run(new frmMain(myLog));

            //try
            //{
            //}
            //catch (Exception ex)
            //{
            //    // a more detailed exception message
            //    var exMessage = String.Format("Exception({0}): {1}", ex.GetType().Name, ex.Message);
            //    if (ex.InnerException != null)
            //        exMessage += String.Format(", InnerException({0}): {1}", ex.InnerException.GetType().Name, ex.InnerException.Message);
            //    Globals.MyLog.Write(exMessage);
            //    Process.Start(AppSettings.Instance.LogFilePath);
            //}

        }

    }
}