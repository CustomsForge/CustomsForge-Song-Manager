using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using CFSM.Utils.PSARC;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public static class ToolkitPrivateTools
    {
        public static void GenerateToolkitVersion(Stream output, string packageAuthor = null, string packageVersion = null)
        {
            if (String.IsNullOrEmpty(packageAuthor))
                packageAuthor = ConfigRepository.Instance()["general_defaultauthor"];

            var writer = new StreamWriter(output);
            writer.WriteLine(String.Format("Toolkit version: {0}", ToolkitVersion.version));
            if (!String.IsNullOrEmpty(packageAuthor))
                writer.WriteLine(String.Format("Package Author: {0}", packageAuthor));
            if (!String.IsNullOrEmpty(packageVersion))
                writer.Write(String.Format("Package Version: {0}", packageVersion));

            writer.Flush();
            output.Seek(0, SeekOrigin.Begin);
        }

        public static void GenerateAppId(Stream output, string appId, Platform platform)
        {
            var writer = new StreamWriter(output);
            var defaultAppId = (platform.version == GameVersion.RS2012) ? "206113" : "248750";
            writer.Write(appId ?? defaultAppId);
            writer.Flush();
            output.Seek(0, SeekOrigin.Begin);
        }

        public static bool RemoveArchiveEntry(string psarcPath, string entryName)
        {

            if (!File.Exists(psarcPath))
                return false;

            using (PSARC archive = new PSARC(true))
            {
                using (var psarcStream = File.OpenRead(psarcPath))
                    archive.Read(psarcStream);

                var tocEntry = archive.TOC.FirstOrDefault(entry => entry.Name == entryName);

                if (tocEntry == null)
                    return true;

                archive.TOC.Remove(tocEntry);
                archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                using (var fs = File.Create(psarcPath))
                    archive.Write(fs, true);

                return true;
            }
        }

        public static bool InjectArchiveEntry(string psarcPath, string entryName, string sourcePath, bool updateToolkitVersion = true)
        {
            if (!File.Exists(psarcPath))
                return false;

            int injectionCount = 2;
            if (!updateToolkitVersion)
                injectionCount = 1;

            using (PSARC archive = new PSARC(true))
            using (var psarcStream = File.OpenRead(psarcPath))
            {
                try
                {
                    archive.Read(psarcStream);
                    psarcStream.Dispose();

                    for (int i = 0; i < injectionCount; i++)
                    {
                        var entryStream = new MemoryStream();

                        switch (i)
                        {
                            case 0:
                                using (var sourceStream = File.OpenRead(sourcePath))
                                    sourceStream.CopyTo(entryStream);
                                break;
                            case 1:
                                var version = String.Format("CFSM v{0}", Constants.ApplicationVersion);
                                GenerateToolkitVersion(entryStream, AppSettings.Instance.CreatorName, version);
                                entryName = "toolkit.version";
                                break;
                        }

                        entryStream.Position = 0;
                        Entry tocEntry = archive.TOC.FirstOrDefault(x => x.Name == entryName);

                        if (tocEntry != null)
                        {
                            tocEntry.Data.Dispose();
                            tocEntry.Data = null;
                            tocEntry.Data = entryStream;
                        }
                        else
                        {
                            archive.AddEntry(entryName, entryStream);

                            // evil genius ... ;) => forces archive update
                            archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
                        }
                    }
                }
                catch
                {
                    return false;
                }

                using (var fs = File.Create(psarcPath))
                    archive.Write(fs, true);

                return true;
            }
        }

        public static string ExtractArchiveFile(string psarcPath, string entryNamePath, string outputDir)
        {
            if (!File.Exists(psarcPath))
                return "";

            using (PSARC archive = new PSARC(true))
            using (var psarcStream = File.OpenRead(psarcPath))
            {
                archive.Read(psarcStream, true);
                var tocEntry = archive.TOC.Where(entry => entry.Name.Contains(entryNamePath)).FirstOrDefault();
                
                if (tocEntry != null)
                {
                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    archive.InflateEntry(tocEntry, Path.Combine(outputDir, Path.GetFileName(tocEntry.ToString())));

                    return tocEntry.ToString();
                }

                return "";
            }
        }

        public static Stream ExtractArchiveFile(string psarcPath, string entryNamePath)
        {
            if (!File.Exists(psarcPath))
                return null;

            using (PSARC archive = new PSARC(true))
            using (var psarcStream = File.OpenRead(psarcPath))
            {
                archive.Read(psarcStream, true);
                var tocEntry = archive.TOC.FirstOrDefault(x => (x.Name.Equals(entryNamePath)));

                if (tocEntry != null)
                {
                    archive.InflateEntry(tocEntry);
                    return tocEntry.Data;
                }
            }

            return null;           
        }

        //public static bool InjectArchiveEntry(string psarcPath, string entryNamePath = null, string sourcePath = null)
        //{
        //    if (!File.Exists(psarcPath))
        //        return false;

        //    using (PSARC archive = new PSARC(true))
        //    using (var psarcStream = File.OpenRead(psarcPath))
        //    using (var entryStream = new MemoryStream())
        //    {
        //        switch (entryNamePath)
        //        {
        //            case "APP_ID":
        //                GenerateAppId(entryStream, null, new Platform(GamePlatform.Pc, GameVersion.RS2014));
        //                break;
        //            case "toolkit.version":
        //                string version = String.Format("CFSM v{0}", Constants.ApplicationVersion);

        //                if (!String.IsNullOrEmpty(sourcePath))
        //                    version = File.ReadAllText(sourcePath);

        //                GenerateToolkitVersion(entryStream, version);
        //                break;
        //            default:
        //                if (String.IsNullOrEmpty(sourcePath))
        //                    return false;

        //                using (var sourceStream = File.OpenRead(sourcePath))
        //                    sourceStream.CopyTo(entryStream);
        //                break;
        //        }

        //        if (String.IsNullOrEmpty(entryNamePath))
        //            entryNamePath = Path.GetFileName(sourcePath);

        //        entryStream.Position = 0;
        //        archive.Read(psarcStream);
        //        psarcStream.Dispose();

        //        Entry tocEntry = archive.TOC.FirstOrDefault(x => x.Name == entryNamePath);

        //        var stophere = tocEntry;

        //        if (tocEntry != null)
        //        {
        //            tocEntry.Data.Dispose();
        //            tocEntry.Data = null;
        //            tocEntry.Data = entryStream;
        //        }
        //        else
        //        {
        //            archive.AddEntry(entryNamePath, entryStream);

        //            // evil genius ... ;) => forces archive update
        //            archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });
        //        }

        //        // File.Delete(psarcPath);

        //        using (var fs = File.Create(psarcPath))
        //            archive.Write(fs, true);

        //        return true;
        //    }
        //}

    }
}
