using System;
using System.Diagnostics;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using CustomsForgeManager.Forms;
using DLogNet;

namespace CustomsForgeManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // prevent multiple occurrence of this application from running
            if (!Constants.DebugMode)
                if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                    return;

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
                Globals.Log(String.Format("{0} {1}", "Exception:", ex.Message));
                Process.Start(Globals.MySettings.LogFilePath);
            }
        }
    }
}
