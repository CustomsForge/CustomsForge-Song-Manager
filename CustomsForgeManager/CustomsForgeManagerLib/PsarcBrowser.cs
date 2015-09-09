using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using Newtonsoft.Json.Linq;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PSARC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public class PsarcBrowser : IDisposable
    {
        private string FilePath;
        private PSARC archive;
 
       // Loads song archive file to memory.
        public PsarcBrowser(string fileName)
        {
            FilePath = fileName;
            archive = new PSARC();
            var stream = File.OpenRead(FilePath);
            archive.Read(stream, true);
        }

         public IEnumerable<SongData> GetSongs()
        {
            string author = String.Empty;
            string version = String.Empty;
            string tkversion = String.Empty;
            var songsFromPsarcFileList = new List<SongData>();

            var toolkitVersionFiles = archive.TOC.Where(x => (x.Name.Equals("toolkit.version")));
            foreach (var toolkitVersionFile in toolkitVersionFiles)
            {
                if (toolkitVersionFile.Name.Equals("toolkit.version"))
                {
                    //if (toolkitVersionFile.Compressed)// it's planned
                    archive.InflateEntry(toolkitVersionFile);
                    ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                    author = tkInfo.PackageAuthor ?? "N/A";
                    version = tkInfo.PackageVersion ?? "N/A";
                    tkversion = tkInfo.ToolkitVersion ?? "N/A";
                }
            }

            var singleSongCount = archive.TOC.Where(x => x.Name.Contains("showlights.xml") && x.Name.Contains("arr"));
            foreach (var singleSong in singleSongCount)
            {

                string strippedName = singleSong.Name.Replace("_showlights.xml", "").Replace("songs/arr/", "");
                var infoFiles = archive.TOC.Where(x =>
                    x.Name.StartsWith("manifests/songs")
                    && !x.Name.Contains("vocals")
                    && x.Name.EndsWith(".json")
                    && x.Name.Contains(strippedName)
                ).OrderBy(x => x.Name);

                var currentSong = new SongData { Author = author, Version = version, ToolkitVer = tkversion, Path = FilePath };
                foreach (var entry in infoFiles)
                {
                    archive.InflateEntry(entry);
                    using (var ms = new MemoryStream())
                    using (var reader = new StreamReader(ms, new UTF8Encoding(), false, 65536))//4Kb is default alloc sise for windows.. 64Kb is default PSARC alloc
                    {
                        entry.Data.CopyTo(ms);
                        entry.Data.Position = 0;
                        ms.Position = 0;

                        var o = JObject.Parse(reader.ReadToEnd());
                        var attributes = o["Entries"].First.Last["Attributes"];

                        currentSong.Song = attributes["SongName"].ToString();
                        currentSong.Artist = attributes["ArtistName"].ToString();
                        currentSong.Album = attributes["AlbumName"].ToString();
                        currentSong.SongYear = attributes["SongYear"].ToString();
                        currentSong.Updated = attributes["LastConversionDateTime"].ToString();
                        currentSong.DD = attributes["MaxPhraseDifficulty"].ToString().DifficultyToDD();
                        currentSong.Tuning =
                            Regex.Replace(attributes["Tuning"].ToString(), @"""(?:\\.|[^""\r\n\\])*""", "")
                                .Replace(@"\s+", "")
                                .Replace("{", "")
                                .Replace("}", "")
                                .Replace(",", "")
                                .Replace(": ", "")
                                .Replace(Environment.NewLine, string.Empty)
                                .Replace(" ", String.Empty).TuningToName();
                      
                        // TODO: fix Vocals parsing and display
                        currentSong.AddArrangement(new SongDataArrangement
                        {
                            Name = attributes["ArrangementName"].ToString()
                        });
                    }
                    songsFromPsarcFileList.Add(currentSong);
                }
            }

            return songsFromPsarcFileList;
        }



        public void Dispose()
        {
            archive.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
