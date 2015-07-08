using System;
using System.Diagnostics;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Forms;
using CustomsForgeManager_Winforms.lib;
using DLogNet;

namespace CustomsForgeManager_Winforms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DLogger myLog = new DLogger();
            Settings mySettings = new Settings();
            
            myLog.AddTargetFile(mySettings.LogFilePath);
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain(myLog, mySettings));
            }
            catch (Exception)
            {
                Process.Start(mySettings.LogFilePath);
            }
        }
    }
}
