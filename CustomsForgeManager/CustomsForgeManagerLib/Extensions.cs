using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;

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

        public static string TuningToName(this string tuningStrings)
        {
            var root = XElement.Load("tunings.xml");
            var tuningName = root.Elements("Tuning").Where(tuning => tuning.Attribute("Strings").Value == tuningStrings).Select(tuning => tuning.Attribute("Name")).ToList();
            return tuningName.Count == 0 ? "Other" : tuningName[0].Value;
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

        public static void SerializeXml<T>(this T obj, FileStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        public static T DeserializeXml<T>(this FileStream stream, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        public static List<string> FilesList(string path, bool includeRS1Pack = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("<ERROR>: No path provided for file scanning");
            
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

    }
}