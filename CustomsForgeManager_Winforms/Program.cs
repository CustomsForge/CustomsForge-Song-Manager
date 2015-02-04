using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomsForgeManager_Winforms.Forms;
using CustomsForgeManager_Winforms.Logging;
using CustomsForgeManager_Winforms.Utilities;

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
            Log myLog = new Log();
            Settings mySettings = new Settings();
            SettingsData settingsData = new SettingsData();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(myLog, mySettings, settingsData));
        }
    }
}
