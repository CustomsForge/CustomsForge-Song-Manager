using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using Microsoft.Win32;
using GenTools;

namespace CustomsForgeSongManager.LocalTools
{
    public static class LocalExtensions
    {
        public static string GetVersionFromFileName(this SongData song)
        {
            if (song.FileName.Contains("_v"))
                return song.FileName.Split(new string[] { "_v", "_p" }, StringSplitOptions.None)[1].Replace("_DD", "").Replace("_", ".");
            else
                return String.Empty;
        }

        public static void LaunchRocksmith2014()
        {
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show("Rocksmith is already running!");
            else
                try
                {
                    if (SysExtensions.OnMac(AppSettings.Instance.RSInstalledDir))
                    {
                        string command = "open steam://rungameid/221680";

                        Process proc = new Process();
                        proc.StartInfo.FileName = "/bin/bash";
                        proc.StartInfo.Arguments = "-c \" " + command + " \"";
                        proc.Start();
                    }
                    else
                        Process.Start("steam://rungameid/221680");
                }
                catch (Exception)
                {
                    Globals.Log("Can not find Steam version of Rocksmith 2014");
                }
        }

        public static void LaunchApp(string path)
        {
            Process.Start(path);
        }

        private static string GetStringValueFromRegistry(string keyName, string valueName)
        {
            try
            {
                return (string)Registry.GetValue(keyName, valueName, "");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public static string GetSteamDirectory()
        {
            // for debugging force user to select the RS root
            // return String.Empty;

            const string installValueName = "InstallLocation";
            const string steamRegPath = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";

            const string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
            const string rsX64Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            // TODO: confirm the following constants for x86 machines
            const string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Rocksmith2014";
            const string rsX86Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            string rs2RootDir = String.Empty;
            if (!String.IsNullOrEmpty(GetStringValueFromRegistry(steamRegPath, "SteamPath")))
                rs2RootDir = Path.Combine(rs2RootDir.Replace('/', '\\'), "SteamApps\\common\\Rocksmith2014");
            else if (!String.IsNullOrEmpty(GetStringValueFromRegistry(rsX64Path, "installdir")))
                rs2RootDir = GetStringValueFromRegistry(rsX64Path, "installdir");
            else if (!String.IsNullOrEmpty(GetStringValueFromRegistry(rsX64Steam, installValueName)))
                rs2RootDir = GetStringValueFromRegistry(rsX64Steam, installValueName);
            else if (!String.IsNullOrEmpty(GetStringValueFromRegistry(rsX86Path, installValueName)))
                rs2RootDir = GetStringValueFromRegistry(rsX86Path, installValueName);
            else if (!String.IsNullOrEmpty(GetStringValueFromRegistry(rsX86Steam, installValueName)))
                rs2RootDir = GetStringValueFromRegistry(rsX86Steam, installValueName);

            if (String.IsNullOrEmpty(rs2RootDir))
                Globals.Log("Steam RS2014 Installation Directory not found in Registry ...");
            else
                Globals.Log("Found Steam RS2014 Installation Directory: " + rs2RootDir);

            return rs2RootDir;
        }

        public static void SetDefaults(this AbortableBackgroundWorker bWorker)
        {
            bWorker.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
        }
    }
}
