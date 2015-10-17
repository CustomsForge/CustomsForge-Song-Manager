using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json.Linq;
using RocksmithToolkitLib;
using RocksmithToolkitLib.Xml;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.Serialization;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public static class Extensions
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
                c.Invoke(new Action(() => action(c)));
            else
                action(c);
        }

        public static void SerializeBin(this object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
            bin.Serialize(Stream, obj);
        }

        public static object DeserializeBin(this FileStream Stream)
        {
            object x = null;
            if (Stream.Length > 0)
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
                x = bin.Deserialize(Stream);
            }
            return x;
        }

        public static string TuningToName(string tolkenTuning)
        {
            var jObj = JObject.Parse(tolkenTuning);
            TuningStrings songTuning = jObj.ToObject<TuningStrings>();

            foreach (var tuning in Globals.TuningXml)
                if (tuning.Tuning.String0 == songTuning.String5 &&
                    tuning.Tuning.String1 == songTuning.String5 &&
                    tuning.Tuning.String2 == songTuning.String5 &&
                    tuning.Tuning.String3 == songTuning.String5 &&
                    tuning.Tuning.String4 == songTuning.String5 &&
                    tuning.Tuning.String5 == songTuning.String5)
                {
                    return tuning.UIName;
                }

            return "Other";
        }

        public static string DifficultyToDD(this string maxDifficulty)
        {
            return maxDifficulty == "0" ? "No" : "Yes";
        }

        public static void SetDefaults(this AbortableBackgroundWorker bWorker)
        {
            bWorker.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
        }

        public static string CleanForAPI(this string text)
        {
            //return text.Replace("/", "_"); //.Replace("\\","");
            var result = text.Replace("/", "_1_").Replace("\\", "_2_"); //WebUtility.HtmlEncode(text);
            return result; //WebUtility.HtmlDecode(text);
        }

        public static string GetInfoURL(this SongData song)
        {
            var url = Constants.DefaultInfoURL; // + "/" + song.Artist.CleanForAPI() + "/" + song.Album.CleanForAPI() + "/" + song.Song.CleanForAPI();
            return url;
        }

        public static string GetAuthURL()
        {
            return Constants.DefaultAuthURL;
        }

        public static void FetchInfo(this SongData song)
        {
            string url = song.GetInfoURL();
            //= client.DownloadString(url);
            string response = String.Empty;
            var client = new WebClient();
            client.DownloadStringCompleted += (sender, e) =>
                {
                    response = e.Result;

                    song.IgnitionID = Ignition.GetDLCInfoFromResponse(response, "id");
                    song.IgnitionUpdated = Ignition.GetDLCInfoFromResponse(response, "updated");
                    song.IgnitionVersion = Ignition.GetDLCInfoFromResponse(response, "version");
                    song.IgnitionAuthor = Ignition.GetDLCInfoFromResponse(response, "name");
                };
            client.DownloadStringAsync(new Uri(url));
        }

        public static void CheckForUpdateStatus(this SongData currentSong, bool isAsync = false)
        {
            currentSong.Status = SongDataStatus.None;

            string url = currentSong.GetInfoURL();
            string response = String.Empty;
            string cfUrl = String.Empty;
            var client = new WebClient();
            int version = 0;

            //isAsync = false;

            if (!isAsync)
            {
                response = client.DownloadString(new Uri(url));

                currentSong.IgnitionID = Ignition.GetDLCInfoFromResponse(response, "id");
                currentSong.IgnitionUpdated = Ignition.GetDLCInfoFromResponse(response, "updated");
                currentSong.IgnitionVersion = Ignition.GetDLCInfoFromResponse(response, "version");
                currentSong.IgnitionAuthor = Ignition.GetDLCInfoFromResponse(response, "name");

                if (int.TryParse(currentSong.Version, out version))
                {
                    currentSong.Version += ".0";
                }

                if (int.TryParse(currentSong.IgnitionVersion, out version))
                {
                    currentSong.IgnitionVersion += ".0";
                }

                if (currentSong.IgnitionVersion == "No Results")
                {
                    currentSong.Status = SongDataStatus.NotFound;
                }
                else if (currentSong.Version == "N/A")
                {
                    //TODO: Check for updates by release/update date
                }
                else if (currentSong.IgnitionVersion != currentSong.Version)
                {
                    currentSong.Status = SongDataStatus.OutDated;
                }
                else if (currentSong.IgnitionVersion == currentSong.Version)
                {
                    currentSong.Status = SongDataStatus.UpToDate;
                }
            }
            else
            {
                client.DownloadStringCompleted += (sender, e) =>
                    {
                        response = e.Result;

                        currentSong.IgnitionID = Ignition.GetDLCInfoFromResponse(response, "id");
                        currentSong.IgnitionUpdated = Ignition.GetDLCInfoFromResponse(response, "updated");
                        currentSong.IgnitionVersion = Ignition.GetDLCInfoFromResponse(response, "version");
                        currentSong.IgnitionAuthor = Ignition.GetDLCInfoFromResponse(response, "name");

                        if (int.TryParse(currentSong.Version, out version))
                        {
                            currentSong.Version += ".0";
                        }

                        if (int.TryParse(currentSong.IgnitionVersion, out version))
                        {
                            currentSong.IgnitionVersion += ".0";
                        }

                        if (currentSong.IgnitionVersion == "No Results")
                        {
                            currentSong.Status = SongDataStatus.NotFound;
                        }
                        else if (currentSong.Version == "N/A")
                        {
                            //TODO: Check for updates by release/update date
                        }
                        else if (currentSong.IgnitionVersion != currentSong.Version)
                        {
                            currentSong.Status = SongDataStatus.OutDated;
                        }
                        else if (currentSong.IgnitionVersion == currentSong.Version)
                        {
                            currentSong.Status = SongDataStatus.UpToDate;
                        }
                    };

                client.DownloadStringAsync(new Uri(url));
            }
        }

        public static string GetVersionFromFileName(this SongData song)
        {
            if (song.FileName.Contains("_v"))
                return song.FileName.Split(new string[] { "_v", "_p" }, StringSplitOptions.None)[1].Replace("_DD", "").Replace("_", ".");
            else
                return String.Empty;
        }

        public static void DeleteEmptyDirs(this DirectoryInfo dir)
        {
            foreach (DirectoryInfo d in dir.GetDirectories())
                d.DeleteEmptyDirs();

            try
            {
                dir.Delete();
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static void SerializeXml<T>(this T obj, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        public static T DeserializeXml<T>(this Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        public static string XmlSerialize(this object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            using (StreamReader reader = new StreamReader(memoryStream))
                return reader.ReadToEnd();
        }

        public static System.Xml.XmlDocument XmlSerializeToDom(this object obj)
        {
            var result = new System.Xml.XmlDocument();
            result.LoadXml(XmlSerialize(obj));
            return result;
        }

        public static object XmlDeserialize(string xml, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(toType);
                using (TextReader tr = new StringReader(xml))
                    return serializer.Deserialize(tr);
            }
        }

        public static T XmlDeserialize<T>(string xml)
        {
            return (T)XmlDeserialize(xml, typeof(T));
        }

        public static T XmlClone<T>(this T obj)
        {
            return (T)XmlDeserialize(obj.XmlSerialize(), typeof(T));
        }

        public static List<string> FilesList(string path, bool includeRS1Pack = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception(CustomsForgeManager.Properties.Resources.ERRORNoPathProvidedForFileScanning);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var files = Directory.EnumerateFiles(path, "*_p.psarc", SearchOption.AllDirectories).ToList();
            files.AddRange(Directory.EnumerateFiles(path, "*_p.disabled.psarc", SearchOption.AllDirectories).ToList());

            if (!includeRS1Pack)
            {
                files = files.Where(file => !file.Contains(Constants.RS1COMP)).ToList();
            }
            return files;
        }

        // Convert List to Data Table
        public static DataTable ConvertList<T>(IEnumerable<T> objectList)
        {
            Type type = typeof(T);
            var typeproperties = type.GetProperties();

            DataTable list2DataTable = new DataTable();
            foreach (PropertyInfo propInfo in typeproperties)
            {
                list2DataTable.Columns.Add(new DataColumn(propInfo.Name, propInfo.PropertyType));
            }

            foreach (T listItem in objectList)
            {
                object[] values = new object[typeproperties.Length];
                for (int i = 0; i < typeproperties.Length; i++)
                {
                    values[i] = typeproperties[i].GetValue(listItem, null);
                }

                list2DataTable.Rows.Add(values);
            }

            return list2DataTable;
        }

        public static void LaunchRocksmith2014()
        {
            var rocksmithProcess = Process.GetProcessesByName("Rocksmith2014.exe");
            if (rocksmithProcess.Length > 0)
                MessageBox.Show(CustomsForgeManager.Properties.Resources.RocksmithIsAlreadyRunning);
            else
                Process.Start("steam://rungameid/221680");
        }

        private static string GetStringValueFromRegistry(string keyName, string valueName)
        {
            try
            {
                return (string)Registry.GetValue(keyName, valueName, "");
            }
            catch (Exception)
            {
                return string.Empty;
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

            Globals.Log(CustomsForgeManager.Properties.Resources.RS2014InstallationDirectoryNotFoundInRegis);


            return String.Empty;
        }


        public static void BackupRocksmithProfile()
        {
            // TODO: confirm this works
            try
            {
                string timestamp = string.Format("{0}-{1}-{2}.{3}-{4}-{5}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                string backupPath = string.Format("{0}\\profile.backup.{1}.zip", Constants.WorkDirectory, timestamp);
                string userProfilePath = String.Empty;
                string steamProfileDir = String.Empty;

                string rsX64Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
                // TODO: confirm the following constant for x86 machines
                string rsX86Path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";

                // for WinXP SP3 x86 compatiblity
                try
                {
                    if (!String.IsNullOrEmpty(Registry.GetValue(rsX64Path, "userdata", null).ToString()))
                        steamProfileDir = Registry.GetValue(rsX64Path, "userdata", null).ToString();
                    // TODO: confirm the following is correct for x86 machines
                    if (!String.IsNullOrEmpty(Registry.GetValue(rsX86Path, "UserData", null).ToString()))
                        steamProfileDir = Registry.GetValue(rsX86Path, "UserData", null).ToString();
                }
                catch (NullReferenceException)
                {
                    // needed for WinXP SP3 which throws NullReferenceException when registry not found
                    Globals.Log("RS2014 User Profile Directory not found in Registry");
                    Globals.Log("You will need to manually locate the directory");
                }

                if (String.IsNullOrEmpty(steamProfileDir))
                    using (var fbd = new FolderBrowserDialog())
                    {
                        var srcDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Steam\userdata\YOUR_USER_ID\221680\remote");
                        fbd.SelectedPath = srcDir;
                        fbd.Description = "Select Rocksmith 2014 user profile directory location";

                        if (fbd.ShowDialog() != DialogResult.OK) return;
                        steamProfileDir = fbd.SelectedPath;
                    }

                var subdirs = new DirectoryInfo(steamProfileDir).GetDirectories("*", SearchOption.AllDirectories).ToArray();

                if (!subdirs.Any())
                {
                    List<string> files = new List<string>();
                    string[] filePatterns = new string[] { "*_prfldb", "localprofiles.json", "crd" };

                    foreach (var pattern in filePatterns)
                    {
                        var partial = Directory.GetFiles(steamProfileDir, pattern, SearchOption.AllDirectories);
                        files.AddRange(partial);
                    }

                    if (files.Count > 1)
                        userProfilePath = steamProfileDir;
                }
                else
                    foreach (DirectoryInfo info in subdirs)
                        if (info.FullName.Contains(@"221680\remote"))
                        {
                            userProfilePath = info.FullName;
                            break;
                        }

                if (Directory.Exists(userProfilePath))
                {
                    // zip using ICSharpCode.SharpZipLib.dll (consistent w/ toolkit dependency)
                    FastZip fz = new FastZip();
                    fz.CreateZip(backupPath, userProfilePath, true, "");

                    Globals.Log(CustomsForgeManager.Properties.Resources.CreatedUserProfileBackup);
                    Globals.Log(backupPath);
                }
                else
                    Globals.Log(CustomsForgeManager.Properties.Resources.Rocksmith2014UserProfileNotFound);
            }
            catch (Exception ex)
            {
                Globals.Log("<Error>:" + ex.Message);
            }
        }


        public static void UploadToCustomsForge()
        {
            Process.Start(Constants.IgnitionURL + "/creators/submit");
        }

        public static void RequestSongOnCustomsForge()
        {
            Process.Start(Constants.RequestURL);
        }


        public static void ExtractEmbeddedResource(string outputDir, string resourceLocation, string[] files)
        {
            string resourcePath = "";
            foreach (string file in files)
            {
                resourcePath = Path.Combine(outputDir, file);
                if (!File.Exists(resourcePath))
                {
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocation + @"." + file);                  
                    using (FileStream fileStream = new FileStream(resourcePath, FileMode.Create))
                        for (int i = 0; i < stream.Length; i++)
                            fileStream.WriteByte((byte)stream.ReadByte());                   
                }
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        


        public static void Benchmark(Action act, int iterations)
        {
            // usage example: CustomsForgeManagerLib.Extensions.Benchmark(LoadSongManager, 1);

            GC.Collect();
            act.Invoke(); // run once outside loop to avoid initialization costs
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
                act.Invoke();

            sw.Stop();
            if (Constants.DebugMode)
                Globals.Log(act.Method.Name + " took: " + sw.ElapsedMilliseconds + " (msec)");
        }

    }
}