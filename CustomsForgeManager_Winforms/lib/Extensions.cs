using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CustomsForgeManager_Winforms.Controls;
using CustomsForgeManager_Winforms.lib;
using System.Net;
using System.Reflection;

namespace CustomsForgeManager_Winforms.Utilities
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
        public static void Serialze(this object obj, FileStream Stream)
        {
            BinaryFormatter bin = new BinaryFormatter();
            bin.FilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Low;
            bin.Serialize(Stream, obj);
        }
        public static object DeSerialize(this FileStream Stream)
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
            var result = text.Replace("/", "_1_").Replace("\\","_2_"); //WebUtility.HtmlEncode(text);
            return result; //WebUtility.HtmlDecode(text);
        }

        public static string GetInfoURL(this SongData song)
        {
            var url = Constants.DefaultInfoURL + "/" + song.Artist.CleanForAPI() + "/" + song.Album.CleanForAPI() + "/" + song.Song.CleanForAPI();
            return url;
        }

        public static void FetchInfo(this SongData song)
        {
            string url = song.GetInfoURL();
            //= client.DownloadString(url);
            string response = "";
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

        public static string GetVersionFromFileName(this SongData song)
        {
            if (song.FileName.Contains("_v"))
                return song.FileName.Split(new string[] { "_v", "_p" }, StringSplitOptions.None)[1].Replace("_DD", "").Replace("_", ".");
            else
                return "";
           // return song.Path.Split(new string[] { "_v", "_"}, StringSplitOptions.None)[1];  
        }
    }
}
