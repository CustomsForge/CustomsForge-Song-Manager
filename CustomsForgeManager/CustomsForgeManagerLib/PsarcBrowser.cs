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

            string author = String.Empty;
            string version = String.Empty;
            string tkversion = String.Empty;
            string appId = String.Empty;
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

            // is assumption that each song contains showlights??
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

                var currentSong = new SongData { Charter = author, Version = version, ToolkitVer = tkversion, AppID = appId, Path = FilePath };

                // TODO: speed hack ... some song info only needed one time
                bool gotSongInfo = false;

                // looping through song multiple times gathering each arrangment
                foreach (var entry in infoFiles)
                {
                    archive.InflateEntry(entry);
                    using (var ms = new MemoryStream())
                    using (var reader = new StreamReader(ms, new UTF8Encoding(), false, 65536))//4Kb is default alloc sise for windows.. 64Kb is default PSARC alloc
                    {
                        entry.Data.CopyTo(ms);
                        entry.Data.Position = 0;
                        ms.Position = 0;

                        // generic json object parsing
                        var o = JObject.Parse(reader.ReadToEnd());
                        var attributes = o["Entries"].First.Last["Attributes"];
                        var tones = attributes.SelectToken("Tones");

                        //Globals.Log("JSON Attributes " + attributes);
                        // mini speed hack - these don't change so skip after first pass
                        if (!gotSongInfo)
                        {
                            currentSong.SongKey = attributes["SongKey"].ToString();
                            currentSong.Title = attributes["SongName"].ToString();
                            currentSong.Artist = attributes["ArtistName"].ToString();
                            currentSong.Album = attributes["AlbumName"].ToString();
                            currentSong.LastConversionDateTime = attributes["LastConversionDateTime"].ToString();
                            currentSong.SongYear = attributes["SongYear"].ToString();
                            currentSong.SongLength = attributes["SongLength"].ToString();
                            currentSong.SongAverageTempo = attributes["SongAverageTempo"].ToString();

                            // some CDLC may not have SongVolume
                            if (attributes["SongVolume"] != null)
                                currentSong.SongVolume = attributes["SongVolume"].ToString();

                            gotSongInfo = true;
                        }

                        arrangmentsFromPsarc.Add(new Arrangement
                       {
                           SongKey = attributes["SongKey"].ToString(),
                           PersistentID = attributes["PersistentID"].ToString(),
                           Name = attributes["ArrangementName"].ToString(),
                           Tuning = Extensions.TuningToName(attributes["Tuning"].ToString()),
                           DMax = attributes["MaxPhraseDifficulty"].ToString(),
                           ToneBase = attributes["Tone_Base"].ToString()
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
