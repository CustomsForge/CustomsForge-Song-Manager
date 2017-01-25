using System;
using System.Diagnostics;
using System.IO;
using RocksmithToolkitLib.XmlRepository;
using RocksmithToolkitLib.DLCPackage;

namespace CustomsForgeSongManager.LocalTools
{
    public class DynamicDifficulty
    {
        public static int ApplyDD(string filePath, int phraseLen, bool removeSus, string rampPath, string cfgPath, out string consoleOutput, bool overWrite = false, bool keepLog = false)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var ddcDir = Path.Combine(baseDir, "ddc");
            var ddcExePath = Path.Combine(ddcDir, "ddc.exe");

            if (String.IsNullOrEmpty(rampPath))
                rampPath = Path.Combine(ddcDir, "ddc_default.xml");

            if (String.IsNullOrEmpty(cfgPath))
                cfgPath = Path.Combine(ddcDir, "ddc_default.cfg");

            if (!File.Exists(ddcExePath))
            {
                consoleOutput = "ddc.exe file is misssing.  Reinstall application and try again.";
                return -1; // application error
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = ddcExePath,
                WorkingDirectory = Path.GetDirectoryName(filePath),
                Arguments = String.Format("\"{0}\" -l {1} -s {2} -m \"{3}\" -c \"{4}\" -p {5} -t {6}", Path.GetFileName(filePath), (UInt16)phraseLen, removeSus ? "Y" : "N", rampPath, cfgPath, overWrite ? "Y" : "N", keepLog ? "Y" : "N"),
                UseShellExecute = false,
                CreateNoWindow = true, // hide command window
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var DDC = new Process())
            {
                DDC.StartInfo = startInfo;
                DDC.Start();
                consoleOutput = DDC.StandardOutput.ReadToEnd();
                consoleOutput += DDC.StandardError.ReadToEnd();
                DDC.WaitForExit(1000 * 60 * 15); //wait for 15 minutes, crunchy solution for AV-sandboxing issues
                return DDC.ExitCode;
            }
        }
    }

    public class SettingsDDC
    {
        public int PhraseLen { get; set; }
        public bool RemoveSus { get; set; }
        public string RampPath { get; set; }
        public string CfgPath { get; set; }

        // not used for now
        // public bool CleanProcess { get; set; }
        // public bool KeepLog { get; set; }

        private static SettingsDDC _instance;

        public static SettingsDDC Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SettingsDDC();
                return _instance;
            }
        }

        public void LoadConfigXml()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var ddcDir = Path.Combine(baseDir, "ddc");
            var configFile = ConfigRepository.Instance()["ddc_config"] + ".cfg";
            var rampupFile = ConfigRepository.Instance()["ddc_rampup"] + ".xml";

            PhraseLen = (int)ConfigRepository.Instance().GetDecimal("ddc_phraselength");
            RemoveSus = ConfigRepository.Instance().GetBoolean("ddc_removesustain");
            RampPath = Path.Combine(ddcDir, rampupFile);
            CfgPath = Path.Combine(ddcDir, configFile);

            if (!File.Exists(RampPath) || !File.Exists(CfgPath))
                throw new FileNotFoundException("DDC support files are missing");

            //    // -m "D:\Documents and Settings\Administrator\My Documents\Visual Studio 2010\Projects\rocksmith-custom-song-toolkit\RocksmithTookitGUI\bin\Debug\ddc\ddc_default.xml"
            //    // -c "D:\Documents and Settings\Administrator\My Documents\Visual Studio 2010\Projects\rocksmith-custom-song-toolkit\RocksmithTookitGUI\bin\Debug\ddc\ddc_default.cfg"
        }

    }
}
