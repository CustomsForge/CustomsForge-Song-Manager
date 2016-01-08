using System;
using System.IO;
using System.Linq;
using RocksmithToolkitLib;
using Constants = CustomsForgeManager.CustomsForgeManagerLib.Objects.Constants;

namespace CFSM.Utils.PSARC
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

        public static bool RemovePsarcEntry(string psarcPath, string entryName)
        {

            if (File.Exists(psarcPath))
            {
                using (PSARC archive = new PSARC())
                {
                    using (var fs = File.OpenRead(psarcPath))
                        archive.Read(fs);

                    var tocEntry = archive.TOC.FirstOrDefault(entry => entry.Name == entryName);
                    if (tocEntry == null)
                        return true;

                    archive.TOC.Remove(tocEntry);
                    archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                    using (var FS = File.Create(psarcPath))
                        archive.Write(FS, true);

                    return true;
                }
            }

            return false;
        }

        public static bool AddPsarcEntry(string psarcPath, string sourcePath, string entryName)
        {
            if (File.Exists(psarcPath))
            {
                using (PSARC archive = new PSARC())
                using (var psarcStream = File.OpenRead(psarcPath))
                using (var sourceStream = File.OpenRead(sourcePath))
                using (var entryStream = new MemoryStream())
                {
                    switch (entryName)
                    {
                        case "APP_ID":
                            GenerateAppId(entryStream, null, new Platform(GamePlatform.Pc, GameVersion.RS2014));
                            break;
                        case "toolkit.version":
                            string version = String.Format("CFSM v{0}", Constants.ApplicationVersion);
                            GenerateToolkitVersion(entryStream, version);
                            break;
                        default:
                            if (String.IsNullOrEmpty(sourcePath))
                                return false;

                            sourceStream.CopyTo(entryStream);
                            break;
                    }

                    if (String.IsNullOrEmpty(entryName))
                        entryName = Path.GetFileName(sourcePath);

                    entryStream.Position = 0;
                    archive.Read(psarcStream);
                    var tocEntry = archive.TOC.FirstOrDefault(entry => entry.Name == entryName);

                    if (tocEntry != null)
                    {
                        tocEntry.Data.Dispose();
                        tocEntry.Data = entryStream;
                    }
                    else
                    {
                        archive.AddEntry(entryName, entryStream);

                        // evil genius ... ;) => force valid update?
                        archive.TOC.Insert(0, new Entry() { Name = "NamesBlock.bin" });

                        using (var fs = File.Create(psarcPath))
                            archive.Write(fs, true);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool ExtractArchiveFile(string psarcPath, string entryName, string outPath)
        {
            using (PSARC p = new PSARC(true))
            using (var FS = File.OpenRead(psarcPath))
            {
                p.Read(FS, true);
                var e = p.TOC.Where(entry => entry.Name.Contains(entryName)).FirstOrDefault();
                if (e != null)
                {
                    if (!Directory.Exists(outPath))
                        Directory.CreateDirectory(outPath);

                    p.InflateEntry(e, Path.Combine(outPath, entryName));
                }
                else
                    return false;
            }

            return true;
        }
    }
}
