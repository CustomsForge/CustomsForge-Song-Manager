﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CustomsForgeSongManager.DataObjects;
using Microsoft.Win32;
using GenTools;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        private static List<string> GetCustomSteamappsFolders(string mainSteamPath) //TODO: because it's, for the most part, the same code as for the GetMacPath, test it 
        {
            string libRegex = "(^\\t\"[1-9]\").*(\".*\")";
            var libDirs = new List<string>();

            string steamappsFolder = AppSettings.Instance.MacMode ? mainSteamPath : Path.Combine(mainSteamPath, "steamapps");
            string libVdf = Path.Combine(steamappsFolder, "libraryfolders.vdf");

            if (!File.Exists(libVdf))
                return new List<string>();

            var content = File.ReadAllLines(libVdf);
            foreach (string l in content)
            {
                var reg = Regex.Match(l, libRegex);
                string dir = reg.Groups[2].Value;

                if (dir != string.Empty)
                {
                    string ndir = dir.Trim('\"');
                    libDirs.Add(ndir);
                }
            }

            if (libDirs.Count == 0)
                return new List<string>();

            return libDirs;
        }

        private static string GetCustomRSFolder(string mainSteamPath)
        {
            var customSteamppsFolders = GetCustomSteamappsFolders(mainSteamPath);
            string finalPath = string.Empty;

            customSteamppsFolders.ForEach(dir =>
            {
                string dirPath = Path.Combine(dir, "steamapps", "appmanifest_221680.acf");

                if (File.Exists(dirPath))
                    finalPath = Path.GetDirectoryName(dirPath);
            });

            if (!string.IsNullOrEmpty(finalPath))
                Globals.Log("Found at: " + finalPath);
            else
            {
                Globals.Log("RS path not found.");
                return string.Empty;
            }

            string rsFolderPath = Path.Combine(finalPath, "common", "Rocksmith2014");

            if (Directory.Exists(rsFolderPath))
                return rsFolderPath;

            return String.Empty;
        }

        public static string GetSteamDirectory()
        {
            // for debugging force user to select the RS root
            // return String.Empty;

            if (AppSettings.Instance.MacMode)
                return GetMacSteamPath();

            const string installValueName = "InstallLocation";
            const string steamRegPath = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";

            const string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
            const string rsX64Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            // TODO: confirm the following constants for x86 machines
            const string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Rocksmith2014";
            const string rsX86Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

            string rs2RootDir = GetStringValueFromRegistry(steamRegPath, "SteamPath");
            if (!String.IsNullOrEmpty(rs2RootDir))
            {
                string steamRootPath = rs2RootDir.Replace('/', '\\');

                rs2RootDir = Path.Combine(steamRootPath, "SteamApps\\common\\Rocksmith2014");

                if (!Directory.Exists(rs2RootDir))
                    rs2RootDir = GetCustomRSFolder(steamRootPath);
            }
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

        public static string GetMacSteamPath()
        {
            //string homePath = Environment.GetEnvironmentVariable("HOME"); -> crashes

            //On Wine -> Environment.SpecialFolder.Personal = @"C:\users\username\My Documents";

            string myDocsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string homeDir = @"Z:\users\"; //TODO: since we all use the same Wine wrapper in a release version, this might go through, but it would be better to replace it

            string userName = myDocsPath.Split(new string[] { "users\\" }, StringSplitOptions.None)[1].Split('\\')[0];
            string prefix = homeDir + userName + "\\"; //TODO: figure whether/when needs users/username and when not

            string libVdf = prefix + @"Library\Application Support\Steam\steamapps\libraryfolders.vdf";

            string libRegex = "(^\\t\"[1-9]\").*(\".*\")";
            var libDirs = new List<string>();

            if (!File.Exists(libVdf))
                return " ";

            var content = File.ReadAllLines(libVdf);
            foreach (string l in content)
            {
                var reg = Regex.Match(l, libRegex);
                string dir = reg.Groups[2].Value;

                if (dir != string.Empty)
                {
                    string ndir = dir.Trim('\"'); //TODO: Maybe it should also be normalized
                    libDirs.Add(ndir);
                }
            }

            if (libDirs.Count == 0)
            {
                string defaultDir = prefix + @"Library\Application Support\Steam";
                libDirs.Add(defaultDir);
            }

            bool found = false;
            var finalPath = "";
            foreach (var dir in libDirs)
            {
                string dirPath = Path.Combine(dir, "steamapps", "appmanifest_221680.acf");
                //var rsAcfFiles = new DirectoryInfo(dirPath).GetFiles("appmanifest_221680.acf"); -> crashes...

                if (File.Exists(dirPath))
                {
                    finalPath = Path.GetDirectoryName(dirPath);
                    found = true;
                    break;
                }
            }

            if (found)
                Globals.Log("Found at: " + finalPath);
            else
            {
                Globals.Log("RS path not found.");
                return string.Empty;
            }

            string rsFolderPath = Path.Combine(finalPath, "common", "Rocksmith2014");
            //string rsAppPath = Path.Combine(rsFolderPath, "Rocksmith2014.app", "Contents", "MacOS/");
            //string dylib = Path.Combine(rsAppPath, "libRSBypass.dylib");

            if (Directory.Exists(rsFolderPath))
                return rsFolderPath;

            return String.Empty;
        }


        public static void SetDefaults(this AbortableBackgroundWorker bWorker)
        {
            bWorker.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
        }
    }
}
