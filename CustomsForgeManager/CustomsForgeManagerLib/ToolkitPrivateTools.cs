using System;
using System.IO;
using System.Linq;
using CFSM.Utils.PSARC;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using RocksmithToolkitLib;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public static class ToolkitPrivateTools
    {
        public static void GenerateToolkitVersion(Stream output, string version = null)
        {
            var author = ConfigRepository.Instance()["general_defaultauthor"];

            var writer = new StreamWriter(output);
            writer.WriteLine(String.Format("Toolkit version: {0}", ToolkitVersion.version));
            if (!String.IsNullOrEmpty(author))
                writer.WriteLine(String.Format("Package Author: {0}", author));
            if (!String.IsNullOrEmpty(version))
                writer.Write(String.Format("Package Version: {0}", version));

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

            if (File.Exists(psarcPath))
            {
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

            return false;
        }

        public static bool InjectArchiveEntry(string psarcPath, string entryNamePath = null, string sourcePath = null)
        {
            if (File.Exists(psarcPath))
            {
                using (PSARC archive = new PSARC(true))
                using (var psarcStream = File.OpenRead(psarcPath))
                using (var entryStream = new MemoryStream())
                {
                    switch (entryNamePath)
                    {
                        case "APP_ID":
                            GenerateAppId(entryStream, null, new Platform(GamePlatform.Pc, GameVersion.RS2014));
                            break;
                        case "toolkit.version":
                            string version = String.Format("CFSM v{0}", Constants.ApplicationVersion);

                            if (!String.IsNullOrEmpty(sourcePath))
                                version = File.ReadAllText(sourcePath);

                            GenerateToolkitVersion(entryStream, version);
                            break;
                        default:
                            if (String.IsNullOrEmpty(sourcePath))
                                return false;

                            using (var sourceStream = File.OpenRead(sourcePath))
                                sourceStream.CopyTo(entryStream);
                            break;
                    }

                    if (String.IsNullOrEmpty(entryNamePath))
                        entryNamePath = Path.GetFileName(sourcePath);

                    entryStream.Position = 0;
                    archive.Read(psarcStream);
                    psarcStream.Dispose();

                    var tocEntry = archive.TOC.FirstOrDefault(entry => entry.Name == entryNamePath);

                    if (tocEntry != null)
                    {
                        tocEntry.Data.Dispose();
                        tocEntry.Data = entryStream;
                    }
                    else
                    {
                        archive.AddEntry(entryNamePath, entryStream);

                        // evil genius ... ;) => forces archive update
                        archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                        using (var fs = File.Create(psarcPath))
                            archive.Write(fs, true);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool ExtractArchiveFile(string psarcPath, string entryNamePath, string outputDir)
        {
            using (PSARC archive = new PSARC(true))
            using (var psarcStream = File.OpenRead(psarcPath))
            {
                archive.Read(psarcStream, true);
                var tocEntry = archive.TOC.Where(entry => entry.Name.Contains(entryNamePath)).FirstOrDefault();
                if (tocEntry != null)
                {
                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    archive.InflateEntry(tocEntry, Path.Combine(outputDir, Path.GetFileName(Path.GetFullPath(entryNamePath))));
                }
                else
                    return false;
            }

            return true;
        }
    }
}
