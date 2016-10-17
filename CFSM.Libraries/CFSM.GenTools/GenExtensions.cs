using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace CFSM.GenTools
{
    public static class GenExtensions
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
                c.Invoke(new Action(() => action(c)));
            else
                action(c);
        }

        public static string DifficultyToDD(this string maxDifficulty)
        {
            return maxDifficulty == "0" ? "No" : "Yes";
        }

        public static string CleanForAPI(this string text)
        {
            //return text.Replace("/", "_"); //.Replace("\\","");
            var result = text.Replace("/", "_1_").Replace("\\", "_2_"); //WebUtility.HtmlEncode(text);
            return result; //WebUtility.HtmlDecode(text);
        }

        public static string CleanName(this string s)
        {
            s = Regex.Replace(s, @"\s+", "");
            s = s.ToLower();

            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("'", "")
                .Replace(".", "")
                .Replace("!", "")
                .Replace("?", "")
                .Replace("|", "")
                .Replace("—", "")
                .Replace("’", "")
                .Replace("...", "");
        }

        public static DateTime NextWeekDay(DateTime date, DayOfWeek weekday)
        {
            return (from i in Enumerable.Range(0, 7)
                    where date.AddDays(i).DayOfWeek == weekday
                    select date.AddDays(i)).First();
        }

        public static DateTime LastWeekDay(DateTime date, DayOfWeek weekday)
        {
            return (from i in Enumerable.Range(0, 7)
                    where date.AddDays(i * -1).DayOfWeek == weekday
                    select date.AddDays(i * -1)).First();
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

        public static void ExtractEmbeddedResource(string outputDir, Assembly resourceAssembly, string resourceLocation, string[] files)
        {
            string resourcePath = String.Empty;
            foreach (string file in files)
            {
                resourcePath = Path.Combine(outputDir, file);
                //always replace with the newest 

                // if (!File.Exists(resourcePath))
                {
                    Stream stream = resourceAssembly.GetManifestResourceStream(String.Format("{0}.{1}", resourceLocation, file));
                    using (FileStream fileStream = new FileStream(resourcePath, FileMode.Create))
                        stream.CopyTo(fileStream);
                    //for (int i = 0; i < stream.Length; i++)
                    //    fileStream.WriteByte((byte)stream.ReadByte());
                }
            }
        }

        public static void ExtractEmbeddedResources(string outputDir, Assembly resourceAssembly, string resourceLocation, bool Overwrite = true)
        {
            var resourcePath = String.Empty;
            var resfiles = resourceAssembly.GetManifestResourceNames().Where(s => s.ToLower().StartsWith(resourceLocation.ToLower()));

            foreach (var file in resfiles)
            {
                // string xFile = file;
                var xFile = file.Replace(resourceLocation + ".", "");
                var parts = xFile.Split('.').ToList();
                var fn = String.Format("{0}.{1}", parts[parts.Count - 2], parts[parts.Count - 1]);
                parts.RemoveRange(parts.Count - 2, 2);
                var xpath = String.Empty;

                foreach (var x in parts)
                    xpath += x + '\\';

                resourcePath = Path.Combine(outputDir, xpath, fn);

                if (!Overwrite && File.Exists(resourcePath))
                    continue;

                var path = Path.GetDirectoryName(resourcePath);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                Stream stream = resourceAssembly.GetManifestResourceStream(file);
                using (FileStream fileStream = new FileStream(resourcePath, FileMode.Create))
                    stream.CopyTo(fileStream);
            }
        }

        public static void BenchmarkAction(Action action, int iterations)
        {
            // usage example: GenExtensions.Extensions.Benchmark(LoadSongManager, 1);

            GC.Collect();
            action.Invoke(); // run once outside loop to avoid initialization costs
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
                action.Invoke();

            sw.Stop();
            Console.WriteLine(action.Method.Name + " took: " + sw.ElapsedMilliseconds + " (msec)");
        }

        // TODO: develop generic benchmarking for extension methods
        public static void BenchmarkExtension(MethodInfo method, int iterations)
        {
            // usage example: var song = Extensions.BenchmarkAction(DgvExtensions.GetObjectFromFirstSelectedRow<SongData>(dgvCurrent), 1);

            GC.Collect();

            method.Invoke(null, new object[] { }); // run once outside loop to avoid initialization costs
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
                method.Invoke(null, new object[] { });

            sw.Stop();
            Console.WriteLine(method.Name + " took: " + sw.ElapsedMilliseconds + " (msec)");
        }

        #region Directory Methods

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

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static void TempChangeDirectory(String directory, Action act)
        {
            String old = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(directory);
                act();
            }
            finally
            {
                Directory.SetCurrentDirectory(old);
            }
        }

        #endregion

        public static string RunExtExe(string exeFileName, bool appRootFolder = true,
                                    bool runInBackground = false,
                                    bool waitToFinish = false, string arguments = null)
        {
            string appRootPath = Path.GetDirectoryName(Application.ExecutablePath);

            var rootPath = (appRootFolder)
                               ? appRootPath
                               : Path.GetDirectoryName(exeFileName);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = (appRootFolder)
                                     ? Path.Combine(rootPath, exeFileName)
                                     : exeFileName;
            startInfo.WorkingDirectory = rootPath;

            if (runInBackground)
            {
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
            }

            if (!String.IsNullOrEmpty(arguments))
                startInfo.Arguments = arguments;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            var output = String.Empty;

            if (runInBackground)
                output = process.StandardOutput.ReadToEnd();

            if (waitToFinish)
                process.WaitForExit();

            return output;
        }

        public static List<string> RunExtExeAlt(string exeFileName, bool appRootFolder = true,
                                    bool runInBackground = false,
                                    bool waitToFinish = false, string arguments = null)
        {
            string appRootPath = Path.GetDirectoryName(Application.ExecutablePath);

            var rootPath = (appRootFolder)
                               ? appRootPath
                               : Path.GetDirectoryName(exeFileName);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = (appRootFolder)
                                     ? Path.Combine(rootPath, exeFileName)
                                     : exeFileName;
            startInfo.WorkingDirectory = rootPath;

            if (runInBackground)
            {
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
            }

            if (!String.IsNullOrEmpty(arguments))
                startInfo.Arguments = arguments;

            Process process = new Process();
            process.StartInfo = startInfo;

            var output = new List<string>();

            if (runInBackground)
            {
                process.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                {
                    output.Add(e.Data);
                });
            }

            process.Start();

            if (waitToFinish)
                process.BeginOutputReadLine();

            return output;
        }

        #region Shortcut Methods

        public static bool AddShortcut(Environment.SpecialFolder destDirectory,
            string exeShortcutLink, string exePath, string destSubDirectory = "",
            string shortcutDescription = "", string exeIconPath = "") // e.g. "OutlookGoogleCalendarSync.lnk"
        {
            Debug.WriteLine("AddShortcut: directory=" + destDirectory.ToString() + "; subdir=" + destSubDirectory);
            if (destSubDirectory != "") destSubDirectory = "\\" + destSubDirectory;
            var shortcutDir = Environment.GetFolderPath(destDirectory) + destSubDirectory;

            if (!Directory.Exists(shortcutDir))
            {
                Debug.WriteLine("Creating directory " + shortcutDir);
                Directory.CreateDirectory(shortcutDir);
            }

            var shortcutLocation = Path.Combine(shortcutDir, exeShortcutLink);
            // IWshRuntimeLibrary is in the COM library "Windows Script Host Object Model"
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(shortcutLocation) as IWshRuntimeLibrary.WshShortcut;

            shortcut.Description = shortcutDescription;
            shortcut.IconLocation = exeIconPath;
            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = Application.StartupPath;
            shortcut.Save();
            Debug.WriteLine("Created shortcut " + exeShortcutLink + " in \"" + shortcutDir + "\"");
            return true;
        }


        public static bool RemoveShortcut(Environment.SpecialFolder destDirectory,
            string exeShortcutLink, string destSubDirectory = "")
        {
            Debug.WriteLine("RemoveShortcut: directory=" + destDirectory.ToString() + "; subdir=" + destSubDirectory);
            if (destSubDirectory != "") destSubDirectory = "\\" + destSubDirectory;
            var shortcutDir = Environment.GetFolderPath(destDirectory) + destSubDirectory;

            if (!Directory.Exists(shortcutDir))
            {
                Debug.WriteLine("Failed to delete shortcut " + exeShortcutLink + " in \"" + shortcutDir + "\" - directory does not exist.");
                return false;
            }

            foreach (var file in Directory.GetFiles(shortcutDir))
            {
                if (file.EndsWith(String.Format("{0}{1}", "\\", exeShortcutLink)))
                {
                    File.Delete(file);
                    Debug.WriteLine("Deleted shortcut " + exeShortcutLink + " in \"" + shortcutDir + "\"");
                    return true;
                }
            }

            return false;
        }

        public static bool CheckShortcut(Environment.SpecialFolder destDirectory, string exeShortcutLink, string destSubDirectory = "")
        {
            Debug.WriteLine("CheckShortcut: directory=" + destDirectory.ToString() + "; subdir=" + destSubDirectory);
            if (destSubDirectory != "") destSubDirectory = "\\" + destSubDirectory;
            var shortcutDir = Environment.GetFolderPath(destDirectory) + destSubDirectory;

            if (!Directory.Exists(shortcutDir)) return false;

            foreach (var file in Directory.GetFiles(shortcutDir))
            {
                if (file.EndsWith(String.Format("{0}{1}", "\\", exeShortcutLink)))
                {
                    Debug.WriteLine("Found shortcut " + exeShortcutLink + " in \"" + shortcutDir + "\"");
                    return true;
                }
            }
            return false;
        }

        public static string ValidateFileName(this string fileName)
        {
            var validFileName = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "-"));
            // var validFileName = String.Join("-", valueFileName.Split(Path.GetInvalidFileNameChars()));
            // var validFileName = String.Concat(fileName.Split(Path.GetInvalidFileNameChars()));                        
            return validFileName;
        }

        public static bool IsFilePathValid(this string filePath)
        {
            if (filePath.IsPathLengthValid())
                if (filePath.IsPathNameValid())
                    return true;

            return false;
        }

        public static bool IsPathLengthValid(this string filePath)
        {
            if (Environment.OSVersion.Version.Major >= 6 && filePath.Length > 260)
                return false;

            if (Environment.OSVersion.Version.Major < 6 && filePath.Length > 215)
                return false;

            return true;
        }

        public static bool IsPathNameValid(this string filePath)
        {
            try
            {
                // check if filePath is null or empty
                if (String.IsNullOrEmpty(filePath))
                    return false;

                // check drive is valid
                var pathRoot = Path.GetPathRoot(filePath);
                if (!Directory.GetLogicalDrives().Contains(pathRoot))
                    return false;

                var fileName = Path.GetFileName(filePath);
                if (String.IsNullOrEmpty(fileName))
                    return false;

                var dirName = Path.GetDirectoryName(filePath);
                if (String.IsNullOrEmpty(dirName))
                    return false;

                if (dirName.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    return false;

                if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}