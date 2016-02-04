using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CFSM.AudioTools;
using CFSM.RSTKLib.PSARC;
using CustomsForgeSongManager.CustomControls;
using CustomsForgeSongManager.DataObjects;
using Microsoft.Win32;

namespace CustomsForgeSongManager.ClassMethods
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
                  Process.Start("steam://rungameid/221680");
              }
              catch (Exception)
              {
                  Globals.Log("Can not find Steam version of Rocksmith 2014");
              }
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
          const string installValueName = "InstallLocation";
          const string steamRegPath = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";

          const string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
          const string rsX64Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

          // TODO: confirm the following constants for x86 machines
          const string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Ubisoft\Rocksmith2014";
          const string rsX86Steam = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

          string result = GetStringValueFromRegistry(steamRegPath, "SteamPath");
          if (!String.IsNullOrEmpty(result))
              return Path.Combine(result.Replace('/', '\\'), "SteamApps\\common\\Rocksmith2014");

          result = GetStringValueFromRegistry(rsX64Path, "installdir");
          if (!String.IsNullOrEmpty(result))
              return result;

          result = GetStringValueFromRegistry(rsX64Steam, installValueName);
          if (!String.IsNullOrEmpty(result))
              return result;

          result = GetStringValueFromRegistry(rsX86Path, installValueName);
          if (!String.IsNullOrEmpty(result))
              return result;

          result = GetStringValueFromRegistry(rsX86Steam, installValueName);
          if (!String.IsNullOrEmpty(result))
              return result;

          Globals.Log("RS2014 Installation Directory not found in Registry");

          return String.Empty;
      }

      public static void SetDefaults(this AbortableBackgroundWorker bWorker)
      {
          bWorker.WorkerSupportsCancellation = true;
          bWorker.WorkerReportsProgress = true;
      }
    }    
}
