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


        public static void SetDefaults(this BackgroundWorker bWorker)
        {
            bWorker.WorkerSupportsCancellation = true;
            bWorker.WorkerReportsProgress = true;
        }


        public static string CleanForAPI(this string text)
        {
            return text.Replace("/", "_");
        }

    }
}
