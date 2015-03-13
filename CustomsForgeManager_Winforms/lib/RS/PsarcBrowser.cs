using CustomsForgeManager_Winforms.Utilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PSARC;
using RocksmithToolkitLib.Sng2014HSL;
using RocksmithToolkitLib.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomsForgeManager_Winforms
{
    public class PsarcBrowser : IDisposable
    {
        private string FilePath;
        private PSARC archive;
        //private Platform platform;

        /// <summary>
        /// Loads archive file to memory.
        /// </summary>
        /// <param name="fileName">Path of the .psarc file to open.</param>
        public PsarcBrowser(string fileName)
        {
            FilePath = fileName;
            archive = new PSARC();
            //platform = FilePath.GetPlatform();
            var stream = File.OpenRead(FilePath);
            {
                archive.Read(stream, true);
            }
        }

        /// <summary>
        /// Retrieve a list of all song contained in the archive.
        /// Returned info includes song title, artist, album and year,
        /// as well as the available arrangements.
        /// </summary>
        /// <returns>List of included songs.</returns>
        public IEnumerable<SongData> GetSongs()
        {
            string author = "";
            string version = "";
            string tkversion = "";
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

                string strippedName = singleSong.Name.Replace("_showlights.xml", "").Replace("songs/arr/","");
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

        /// <summary>
        /// Extract a particular arrangement of a song from the archive
        /// and return the corresponding Song2014 object.
        /// </summary>
        /// <param name="identifier">Identifier (short title, aka key) of the song to load.</param>
        /// <param name="arrangement">The arrangement (lead, rythum, bass) to use.</param>
        /// <returns>A Song2014 object containing the arrangement.</returns>
        //public Song2014 GetArrangement(string identifier, string arrangement)
        //{
        //    // In order to instantiate a Rocksmith Song2014 object, we need both
        //    // the binary .sng file and the attributes contained in the corresponding
        //    // .json manifest.
        //    Console.WriteLine("GetArrangement called with identifier [{0}], arrangement {{{1}}}", identifier,
        //                      arrangement);
        //    var sngFile = archive.TOC.FirstOrDefault(x => x.Name == "songs/bin/generic/" +
        //                                                                identifier + "_" + arrangement + ".sng");
        //    var jsonFile = archive.TOC.FirstOrDefault(x => x.Name.StartsWith("manifests/songs") &&
        //                                                       x.Name.EndsWith("/" + identifier + "_" + arrangement +
        //                                                                       ".json"));
        //    if (sngFile == null || jsonFile == null)
        //    {
        //        if (sngFile == null)
        //            Console.WriteLine("sngFile is null.");
        //        if (jsonFile == null)
        //            Console.WriteLine("jsonFile is null.");
        //        return null;
        //    }

        //    // read out attributes from .json manifest
        //    Attributes2014 attr;
        //    using (var ms = new MemoryStream())
        //    using (var reader = new StreamReader(ms, new UTF8Encoding(), false, 1024))
        //    {
        //        jsonFile.Data.CopyTo(ms);
        //        ms.Position = 0;
        //        var manifest = JsonConvert.DeserializeObject<Manifest2014<Attributes2014>>(
        //            reader.ReadToEnd());
        //        if (manifest == null)
        //            return null;
        //        attr = manifest.Entries.ToArray()[0].Value.ToArray()[0].Value;
        //    }

        //    // get contents of .sng file
        //    Sng2014File sng = Sng2014File.ReadSng(sngFile.Data, platform);

        //    return new Song2014(sng, attr);
        //}

        public void Dispose()
        {
            archive.Dispose();
        }
    }

    ///// <summary>
    ///// Struct containing info about a single track.
    ///// </summary>
    //public class SongInfo
    //{
    //    public string Title { get; set; }
    //    public string Artist { get; set; }
    //    public string Album { get; set; }
    //    public string Year { get; set; }
    //    public string DD { get; set; }
    //    public string Identifier { get; set; }
    //    public string Tuning { get; set; }
    //    public string Updated { get; set; }
    //    public string Author { get; set; }
    //    public IList<string> Arrangements { get; set; }
    //}

    ///// <summary>
    ///// Struct containing short info about a single track.
    ///// </summary>
    //public class SongInfoShort
    //{
    //    public string Identifier { get; set; }
    //    public string Arrangement { get; set; }
    //}
}
