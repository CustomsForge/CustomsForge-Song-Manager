#define DEBUG_LOG_PARSING

using System.Diagnostics;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.CustomsForgeManagerLib.Objects;
using DataGridViewTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.PSARC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arrangement = CustomsForgeManager.CustomsForgeManagerLib.Objects.Arrangement;

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
            var sw = new Stopwatch();
            sw.Restart();

            var author = String.Empty;
            var version = String.Empty;
            var tkversion = String.Empty;
            var appId = String.Empty;
            var songsFromPsarc = new List<SongData>();
            var arrangmentsFromPsarc = new FilteredBindingList<Arrangement>();


            var toolkitVersionFile = archive.TOC.FirstOrDefault(x => (x.Name.Equals("toolkit.version")));
            if (toolkitVersionFile != null)
            {
                archive.InflateEntry(toolkitVersionFile);
                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                author = tkInfo.PackageAuthor ?? "N/A";
                version = tkInfo.PackageVersion ?? "N/A";
                tkversion = tkInfo.ToolkitVersion ?? "N/A";
            }

            var appIdFile = archive.TOC.FirstOrDefault(x => (x.Name.Equals("appid.appid")));
            if (appIdFile != null)
            {
                archive.InflateEntry(appIdFile);
                using (var reader = new StreamReader(appIdFile.Data))
                    appId = reader.ReadLine();
            }

            bool vocals = archive.TOC.Any(x => (x.Name.Contains("_vocals.sng")));

            // speed hack - this only needs to be done one time
            var fInfo = new FileInfo(FilePath);
            var currentSong = new SongData
            {
                Charter = author,
                Version = version,
                ToolkitVer = tkversion,
                AppID = appId,
                Path = FilePath,
                FileDate = fInfo.LastWriteTimeUtc,
                FileSize = (int)fInfo.Length,
            };

            // it is incorrect to assume that each song contains showlights
            // var singleSongCount = archive.TOC.Where(x => x.Name.Contains("showlights.xml") && x.Name.Contains("arr"));
            // use gamexblock which every song must contain
            var singleSongCount = archive.TOC.Where(x => x.Name.Contains(".xblock") && x.Name.Contains("nsongs"));
            // this foreach loop addresses song packs otherwise it is only done one time
            foreach (var singleSong in singleSongCount)
            {
                string strippedName = singleSong.Name.Replace(".xblock", "").Replace("gamexblocks/nsongs/", "");
                //string strippedName = singleSong.Name.Replace("_showlights.xml", "").Replace("songs/arr/", "");

                // get vocal arrangment info too
                var infoFiles = archive.TOC.Where(x =>
                     x.Name.StartsWith("manifests/songs")
                         // && !x.Name.Contains("vocals") // commented out to gather vocals info
                    && x.Name.EndsWith(".json")
                    && x.Name.Contains(strippedName)
                ).OrderBy(x => x.Name); // bass, lead, rhythm, vocal

                // speed hack ... some song info only needed one time
                bool gotSongInfo = false;

                // looping through song multiple times gathering each arrangment
                foreach (var entry in infoFiles)
                {
                    archive.InflateEntry(entry);
                    var ms = new MemoryStream();  // ?? Why remove disposal ??
                    using (var reader = new StreamReader(ms, new UTF8Encoding(), false, 65536))//4Kb is default alloc sise for windows.. 64Kb is default PSARC alloc
                    {
                        entry.Data.CopyTo(ms);
                        entry.Data.Position = 0;
                        ms.Position = 0;

                        // generic json object parsing
                        var o = JObject.Parse(reader.ReadToEnd());
                        var attributes = o["Entries"].First.Last["Attributes"];

                        // mini speed hack - these don't change so skip after first pass
                        if (!gotSongInfo)
                        {
                            currentSong.SongKey = attributes["SongKey"].ToString();
                            currentSong.Title = attributes["SongName"].ToString();
                            currentSong.Artist = attributes["ArtistName"].ToString();
                            currentSong.Album = attributes["AlbumName"].ToString();
                            currentSong.LastConversionDateTime = Convert.ToDateTime(attributes["LastConversionDateTime"]);
                            currentSong.SongYear = Convert.ToInt32(attributes["SongYear"]);
                            currentSong.SongLength = Convert.ToSingle(attributes["SongLength"]);
                            currentSong.SongAverageTempo = Convert.ToSingle(attributes["SongAverageTempo"]);

                            // some CDLC may not have SongVolume info
                            if (attributes["SongVolume"] != null)
                                currentSong.SongVolume = Convert.ToSingle(attributes["SongVolume"]);

                            gotSongInfo = true;
                        }

                        var arrName = attributes["ArrangementName"].ToString();

                        if (arrName.ToLower().Contains("vocal"))
                            arrangmentsFromPsarc.Add(new Arrangement
                            {
                                SongKey = attributes["DLCKey"].ToString(),
                                PersistentID = attributes["PersistentID"].ToString(),
                                Name = attributes["ArrangementName"].ToString(),
                             });
                        else
                            arrangmentsFromPsarc.Add(new Arrangement
                           {
                               SongKey = attributes["DLCKey"].ToString(),
                               PersistentID = attributes["PersistentID"].ToString(),
                               Name = arrName,
                               Tuning = Extensions.TuningToName(attributes["Tuning"].ToString()),
                               DMax = Convert.ToInt32(attributes["MaxPhraseDifficulty"].ToString()),
                               ToneBase = attributes["Tone_Base"].ToString(),
                               SectionCount = attributes["Sections"].ToArray().Count()
                           });
                    }
                }

                currentSong.Arrangements2D = arrangmentsFromPsarc;
                songsFromPsarc.Add(currentSong);
            }

            sw.Stop();
            if (Constants.DebugMode)
                Globals.Log(Path.GetFileName(FilePath) + " parsing took: " + sw.ElapsedMilliseconds + " (msec)");

            return songsFromPsarc;
        }


        public void Dispose()
        {
            archive.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
