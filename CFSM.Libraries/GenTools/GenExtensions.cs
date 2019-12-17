using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using CustomControls;
using System.Security.Cryptography;
using System.ComponentModel;
using ToolkitExtensions = RocksmithToolkitLib.Extensions.StringExtensions;


namespace GenTools
{
    public static class GenExtensions
    {
        #region Constants

        private const int HWND_BOTTOM = 1;
        private const int HWND_NOTOPMOST = -2;
        private const int HWND_TOP = 0;
        private const int HWND_TOPMOST = -1;
        private const string MESSAGEBOX_CAPTION = "GenExtensions from ExtensionsLib";
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_SHOWWINDOW = 0x0040;

        #endregion

        public static bool AddShortcut(Environment.SpecialFolder destDirectory,
                                                            string exeShortcutLink, string exePath, string destSubDirectory = "",
                                                            string shortcutDescription = "", string exeIconPath = "")
        {
            // e.g. "OutlookGoogleCalendarSync.lnk"
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

        public static void CleanDir(this System.IO.DirectoryInfo directory, bool deleteSubDirs = false)
        {
            foreach (FileInfo file in directory.GetFiles())
                file.Delete();

            if (deleteSubDirs)
                foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                    subDirectory.Delete(true);
        }

        public static void CleanLocalTemp()
        {
            var di = new DirectoryInfo(Path.GetTempPath());

            // 'Local Settings\Temp' in WinXp
            // 'AppData\Local\Temp' in Win7
            // confirm this is the correct temp directory before deleting
            if (di.Parent != null)
            {
                if (di.Parent.Name.Contains("Local") && di.Name == "Temp")
                {
                    foreach (FileInfo file in di.GetFiles())
                        try
                        {
                            file.Delete();
                        }
                        catch { /*Don't worry just skip locked file*/ }

                    foreach (DirectoryInfo dir in di.GetDirectories())
                        try
                        {
                            dir.Delete(true);
                        }
                        catch { /*Don't worry just skip locked directory*/ }
                }
            }
        }

        /// <summary>
        /// Moves short words, replaces abbreviations, strips non-alphanumeric and whitespaces
        /// <para>Returned clean string is uppercase invariant suitable for use in making comparisons</para>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string CleanString(this string s)
        {
            // using toolkitlib methods to produce a clean workable string
            return ToolkitExtensions.StripNonAlphaNumeric(ToolkitExtensions.ReplaceAbbreviations(ToolkitExtensions.ShortWordMover(s))).ToUpperInvariant();
        }

        public static string CleanVersion(this string s)
        {
            // 2.9.2.0-bacdb7d3
            var version = "0.0.0.0"; // default ODLC
            var ndxDash = s.IndexOf('-');

            if (ndxDash > 6)
                version = s.Substring(0, ndxDash);
            
            return version;
        }


        public static void ClearFolder(string folderName)
        {
            // useage: ClearFolder(Path.GetTempPath()); to empty the Local Temp folder
            // use this method when DeleteDir does not completely empty a folder
            DirectoryInfo dir = new DirectoryInfo(folderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }

        public static bool CopyDir(string srcFolder, string destFolder, bool isRecursive = true)
        {
            // You can not copy something that does not exist ... doh!  Banging head on desk ...
            if (!Directory.Exists(srcFolder))
                return false;

            if (!Directory.Exists(destFolder))
            {
                try
                {
                    Directory.CreateDirectory(destFolder);
                }
                catch (IOException e)
                {
                    BetterDialog.ShowDialog(
                        "<ERROR> Could not create directory: " + destFolder + Environment.NewLine + e.Message,
                        MESSAGEBOX_CAPTION, MessageBoxButtons.OK, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                    return false;
                }
            }

            // Get Files and Copy
            string[] files = Directory.GetFiles(srcFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);

                string dest = Path.Combine(destFolder, name);
                try
                {
                    File.Copy(file, dest, true);
                }
                catch (IOException e)
                {
                    BetterDialog.ShowDialog(
                        "<ERROR> Could not copy the file: " + file + Environment.NewLine + e.Message,
                        MESSAGEBOX_CAPTION, MessageBoxButtons.OK, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                    return false;
                }
            }

            // Get dirs recursively and copy files
            if (isRecursive)
            {
                string[] folders = Directory.GetDirectories(srcFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Path.Combine(destFolder, name);
                    CopyDir(folder, dest);
                }
            }

            return true;
        }

        public static bool CopyFile(string fileFrom, string fileTo, bool overWrite, bool verbose = true)
        {
            if (!File.Exists(fileFrom))
                return false;

            if (verbose)
                if (!PromptOverwrite(fileTo))
                    return false;
                else
                    overWrite = true;

            var fileToDir = Path.GetDirectoryName(fileTo);
            if (!Directory.Exists(fileToDir)) MakeDir(fileToDir);

            try
            {
                File.SetAttributes(fileFrom, FileAttributes.Normal);
                File.Copy(fileFrom, fileTo, overWrite);
                return true;
            }
            catch (IOException e)
            {
                if (!overWrite || !verbose)
                    return true; // be nice don't throw error
                BetterDialog.ShowDialog(
                    "Could not copy file " + fileFrom + "\r\nError Code: " + e.Message +
                    "\r\nMake sure associated file/folders are closed.",
                    MESSAGEBOX_CAPTION, MessageBoxButtons.OK, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                return false;
            }
        }

        /// <summary>
        /// Find the IndexOf a target string in a source string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int CustomIndexOf(this string source, string target, int position)
        {
            int index = -1;
            for (int i = 0; i < position; i++)
            {
                index = source.IndexOf(target, index + 1, StringComparison.InvariantCultureIgnoreCase);

                if (index == -1)
                    break;
            }
            return index;
        }

        public static bool DeleteDirectory(string dirPath, bool includeSubDirs = true)
        {
            if (!Directory.Exists(dirPath))
                return false;

            const int magicDust = 10;
            for (var gnomes = 1; gnomes <= magicDust; gnomes++)
            {
                try
                {
                    Directory.Delete(dirPath, includeSubDirs);
                    return true;
                }
                catch (DirectoryNotFoundException)
                {
                    return false;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (IOException)
                {
                    // System.IO.IOException: The directory is not empty so delete as many files as possible
                    var files = Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories).ToList();
                    foreach (var file in files)
                        DeleteFile(file);

                    Debug.WriteLine("Gnomes prevent deletion of {0}! Applying magic dust, attempt #{1}.", dirPath, gnomes);
                    Thread.Sleep(50);
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine("Unauthorized access to: " + dirPath);
                    return false;
                }
            }

            return false;
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

        public static bool DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
                return true;

            const int magicDust = 10;
            for (var gnomes = 1; gnomes <= magicDust; gnomes++)
            {
                try
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    return true; // file does not exist
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (IOException)
                {
                    // IOException: The process cannot access the file ...
                    Debug.WriteLine("Gnomes prevent deletion of {0}! Applying magic dust, attempt #{1}.", filePath, gnomes);
                    Thread.Sleep(50);
                    continue;
                }
            }

            return false;
        }

        public static string DifficultyToDD(this string maxDifficulty)
        {
            return maxDifficulty == "0" ? "No" : "Yes";
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

        public static string GetFullAppVersion()
        {
            Debug.WriteLine("Assembly Name: " + Assembly.GetEntryAssembly().GetName());
            // this can be customized
            return String.Format("{0}.{1}.{2}.{3}",
                                 Assembly.GetEntryAssembly().GetName().Version.Major,
                                 Assembly.GetEntryAssembly().GetName().Version.Minor,
                                 Assembly.GetEntryAssembly().GetName().Version.Build,
                                 Assembly.GetEntryAssembly().GetName().Version.Revision);
        }

        public static string GetMD5Hash(string srcPath)
        {
            string strResult = "";
            string strHashData = "";

            byte[] arrbytHashValue;
            FileStream fileStream = null;

            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            try
            {
                fileStream = GetFileStream(srcPath);
                arrbytHashValue = md5Hasher.ComputeHash(fileStream);
                fileStream.Close();

                strHashData = System.BitConverter.ToString(arrbytHashValue);
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }

            return (strResult);
        }

        public static string GetResource(string resName)
        {
            // very usefull, move next line before method call to determine valid resource paths and names 
            // string[] names = this.GetType().Assembly.GetManifestResourceNames();
            // for static methods use the following two lines
            // Assembly assem = Assembly.GetExecutingAssembly();
            // string[] names = assem.GetManifestResourceNames();
            //
            Assembly assem = Assembly.GetExecutingAssembly();
            var stream = assem.GetManifestResourceStream(resName);
            if (stream != null)
            {
                var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            BetterDialog.ShowDialog("Error: Could not access resource file " + resName,
                                    MESSAGEBOX_CAPTION, MessageBoxButtons.OK,
                                    Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
            return null;
        }

        public static void InvokeIfRequired<T>(this T c, Action<T> action) where T : Control
        {
            if (c.InvokeRequired)
                c.Invoke(new Action(() => action(c)));
            else
                action(c);
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static bool IsFilePathLengthValid(this string filePath)
        {
            if (Environment.OSVersion.Version.Major >= 6 && filePath.Length > 260)
                return false;

            if (Environment.OSVersion.Version.Major < 6 && filePath.Length > 215)
                return false;

            return true;
        }

        public static bool IsFilePathNameValid(this string filePath)
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

        public static bool IsFilePathValid(this string filePath)
        {
            if (filePath.IsFilePathLengthValid())
                if (filePath.IsFilePathNameValid())
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

        public static DateTime LastWeekDay(DateTime date, DayOfWeek weekday)
        {
            return (from i in Enumerable.Range(0, 7)
                    where date.AddDays(i * -1).DayOfWeek == weekday
                    select date.AddDays(i * -1)).First();
        }

        public static bool MakeDir(string dirPath)
        {
            try
            {
                Directory.CreateDirectory(dirPath);
                return true;
            }
            catch (IOException e)
            {
                BetterDialog.ShowDialog(
                    "Could not create the directory structure.  Error Code: " + e.Message,
                    MESSAGEBOX_CAPTION, MessageBoxButtons.OK, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                return false;
            }
        }

        public static string MakeValidDirFileName(string name, string replaceWith = "")
        {
            return MakeValidFileName(MakeValidDirName(name, replaceWith), replaceWith);
        }

        public static string MakeValidDirName(string dirName, string replaceWith = "")
        {
            string invalidChars = new string(Path.GetInvalidPathChars());
            if (!String.IsNullOrEmpty(replaceWith) && invalidChars.Contains(replaceWith))
                throw new InvalidDataException("<ERROR> DirName replaceWith charater is invalid ...");

            // remove forward slashes from a directory name, e.g. AC/DC becomes ACDC
            string invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}/]+)", Regex.Escape(invalidChars));
            
            return Regex.Replace(dirName, invalidRegStr, replaceWith);
        }

        public static string MakeValidFileName(string fileName, string replaceWith = "")
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            if (!String.IsNullOrEmpty(replaceWith) && invalidChars.Contains(replaceWith))
                throw new InvalidDataException("<ERROR> FileName replaceWith charater is invalid ...");

            string invalidRegStr = String.Format(@"([{0}]*\.+$)|([{0}]+)", Regex.Escape(invalidChars));

            return Regex.Replace(fileName, invalidRegStr, replaceWith);
        }

        public static bool MoveFile(string fileFrom, string fileTo, bool overWrite, bool verbose = true)
        {
            if (!File.Exists(fileFrom))
                return false;

            if (File.Exists(fileTo))
            {
                if (!verbose)
                    File.Delete(fileTo);
                else if (!PromptOverwrite(fileTo))
                    return false;
                else
                    File.Delete(fileTo);
            }
            else
            {
                var fileToDir = Path.GetDirectoryName(fileTo);
                if (!Directory.Exists(fileToDir))
                    MakeDir(fileToDir);
            }

            try
            {
                CopyFile(fileFrom, fileTo, overWrite, verbose);
                DeleteFile(fileFrom);
                return true;
            }
            catch (IOException e)
            {
                if (!verbose) // be nice don't throw errMsg
                    return true;

                var errMsg = "Could not move the file " + fileFrom + "  Error Code: " + e.Message;
                BetterDialog.ShowDialog(SplitString(errMsg, 50), MESSAGEBOX_CAPTION, MessageBoxButtons.OK, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                return false;
            }
        }

        public static DateTime NextWeekDay(DateTime date, DayOfWeek weekday)
        {
            return (from i in Enumerable.Range(0, 7)
                    where date.AddDays(i).DayOfWeek == weekday
                    select date.AddDays(i)).First();
        }

        public static bool PromptOpen(string destDir, string msgText, string windowTitle = "CustomsForge Song Manager")
        {
            if (BetterDialog.ShowDialog(msgText + Environment.NewLine +
                                        "Would you like to open the folder?", @"Open Directory/File Location",
                                        MessageBoxButtons.YesNo, Bitmap.FromHicon(SystemIcons.Information.Handle),
                                        "Information ...", 150, 150) == DialogResult.Yes)
            {
                if (Directory.Exists(destDir))
                {
                    Process.Start("explorer.exe", String.Format("{0}", destDir));
                    // bring open directory to front
                    var hWnd = WinGetHandle(windowTitle); // e.g. windowTitle = "Custom Game Toolkit"
                    SetWindowPos(hWnd, (IntPtr)HWND_NOTOPMOST, 0, 0, 0, 0,
                                 SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

                    return true;
                }
            }
            return false;
        }

        public static bool PromptOverwrite(string destPath)
        {
            if (File.Exists(destPath))
            {
                if (BetterDialog.ShowDialog("File already exists:" + Environment.NewLine +
                    SplitString(destPath, 50, false) + Environment.NewLine + Environment.NewLine + 
                    "Overwrite the existing file?", @"Warning: Overwrite File Message",
                    MessageBoxButtons.YesNo, Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150)
                    == DialogResult.No)
                    return false;
            }
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

        public static List<string> RsFilesList(string path, bool includeRs1Packs = false, bool includeSongPacks = false, bool includeSubfolders = true)
        {
            if (String.IsNullOrEmpty(path))
                throw new Exception("<ERROR> No path provided for file scanning");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string extension = SysExtensions.OnMac() ? "_m.psarc" : "_p.psarc";
            string disabledExtension = SysExtensions.OnMac() ? "_m.disabled.psarc" : "_p.disabled.psarc";

            var files = Directory.EnumerateFiles(path, "*" + extension, includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
            files.AddRange(Directory.EnumerateFiles(path, "*" + disabledExtension, includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList());
            files = files.Where(file => !file.ToLower().Contains("inlay")).ToList();

            if (!includeRs1Packs)
                files = files.Where(file => !file.ToLower().Contains("rs1compatibility")).ToList();

            if (!includeSongPacks)
                files = files.Where(file => !file.ToLower().Contains("songpack") && !file.ToLower().Contains("_sp_")).ToList();

            return files;
        }

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

            if (waitToFinish)
                process.WaitForExit();

            var output = String.Empty;

            if (runInBackground)
                output = process.StandardOutput.ReadToEnd();

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

        /// <summary>
        /// Splits a big list into a specified number of smaller sublists,
        /// only a single list is returned when 'parts' equals 1 
        /// </summary>
        /// <typeparam name="T">list object type</typeparam>
        /// <param name="list">the big list</param>
        /// <param name="parts">number of sublists desired</param>
        /// <returns></returns>
        public static List<List<T>> SplitList<T>(this List<T> list, int parts)
        {
            List<List<T>> result = new List<List<T>>();
            if (parts > 1)
            {
                var rangeSize = list.Count / parts;
                var firstRangeSize = rangeSize + list.Count % parts;
                var startNdx = 0;
                var endNdx = firstRangeSize;

                for (int i = 0; i < parts; i++)
                {
                    List<T> innerResult = new List<T>();
                    int j = 0;

                    for (j = startNdx; j < endNdx; j++)
                        innerResult.Add(list[j]);

                    result.Add(innerResult);
                    startNdx = j;
                    endNdx = startNdx + rangeSize;
                }

                // linq method works but changes order of lists
                //result = list.Select((item, index) => new { index, item })
                //    .GroupBy(x => x.index % parts)
                //    .Select(x => x.Select(y => y.item).ToList()).ToList();
            }
            else
                result.Add(list);

            return result;
        }

        /// <summary>
        /// Splits a text string so that it wraps to specified line length
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="lineLength"></param>
        /// <param name="splitOnSpace"></param>
        /// <returns></returns>
        public static string SplitString(string inputText, int lineLength, bool splitOnSpace = true)
        {
            var finalString = String.Empty;

            if (splitOnSpace)
            {
                var delimiters = new[] { " " }; // , "\\" };
                var stringSplit = inputText.Split(delimiters, StringSplitOptions.None);
                var charCounter = 0;

                for (int i = 0; i < stringSplit.Length; i++)
                {
                    finalString += stringSplit[i] + " ";
                    charCounter += stringSplit[i].Length;

                    if (charCounter > lineLength)
                    {
                        finalString += Environment.NewLine;
                        charCounter = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < inputText.Length; i += lineLength)
                {
                    if (i + lineLength > inputText.Length)
                        lineLength = inputText.Length - i;

                    finalString += inputText.Substring(i, lineLength) + Environment.NewLine;
                }
                finalString = finalString.TrimEnd(Environment.NewLine.ToCharArray());
            }

            return finalString;
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

        public static string ValidateFileName(this string fileName)
        {
            var validFileName = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), "-"));
            // var validFileName = String.Join("-", valueFileName.Split(Path.GetInvalidFileNameChars()));
            // var validFileName = String.Concat(fileName.Split(Path.GetInvalidFileNameChars()));                        
            return validFileName;
        }

        public static IntPtr WinGetHandle(string windowName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(windowName))
                //if (pList.MainWindowTitle.ToLower() == windowName.ToLower())                   
                {
                    hWnd = pList.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }

        public static void WriteStreamFile(this Stream memoryStream, string fileName)
        {
            var streamFileDir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(streamFileDir)) MakeDir(streamFileDir);

            using (
                FileStream file = new FileStream(fileName, FileMode.Create,
                                                 FileAccess.Write))
            {
                byte[] bytes = new byte[memoryStream.Length];
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                file.Write(bytes, 0, bytes.Length);
            }
        }

        public static bool WriteTextFile(this string strText, string fileName,
                                         bool bAppend = true)
        {
            var textFileDir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(textFileDir))
                MakeDir(textFileDir);

            try
            {
                // File.WriteAllText(fileName, strText); // stream of text
                using (TextWriter tw = new StreamWriter(fileName, bAppend))
                {
                    tw.Write(strText); // IMPORTANT no CRLF added to end
                    // tw.WriteLine(strText);  // causes CRLF to be added
                    tw.Close();
                }
                return true;
            }
            catch (IOException e)
            {
                BetterDialog.ShowDialog(
                    "Could not create text file " + fileName + "  Error Code: " +
                    e.Message, MESSAGEBOX_CAPTION, MessageBoxButtons.OK,
                    Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning ...", 150, 150);
                return false;
            }
        }

        private static FileStream GetFileStream(string pathName)
        {
            return (new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
                                                int Y, int cx, int cy, uint uFlags);

    }
}