using System.Diagnostics;
using CFSM.AudioTools;
using CFSM.RSTKLib.PSARC;
using CustomsForgeSongManager.DataObjects;
using DataGridViewTools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using Arrangement = CustomsForgeSongManager.DataObjects.Arrangement;

namespace CustomsForgeSongManager.ClassMethods
{
    public sealed class PsarcBrowser : IDisposable
    {
        private string _filePath;
        private PSARC _archive;
        private Stream _fileStream;

        // Loads song archive file to memory.
        public PsarcBrowser(string fileName)
        {
            _filePath = fileName;
            _archive = new PSARC();
            _fileStream = File.OpenRead(_filePath);
            _archive.Read(_fileStream, true);
        }

        public IEnumerable<SongData> GetSongData()
        {
            Stopwatch sw = null;
            sw = new Stopwatch();
            sw.Restart();

            var songsFromPsarc = new List<SongData>();
            var fInfo = new FileInfo(_filePath);
            var author = String.Empty;
            var version = String.Empty;
            var tkversion = String.Empty;
            var appId = String.Empty;

            var tagged = _archive.TOC.Any(entry => entry.Name == "tagger.org");

            var toolkitVersionFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("toolkit.version")));
            if (toolkitVersionFile != null)
            {
                _archive.InflateEntry(toolkitVersionFile);
                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                author = tkInfo.PackageAuthor ?? "N/A";
                version = tkInfo.PackageVersion ?? "N/A";
                tkversion = tkInfo.ToolkitVersion ?? "N/A";
            }

            var appIdFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("appid.appid")));
            if (appIdFile != null)
            {
                _archive.InflateEntry(appIdFile);
                using (var reader = new StreamReader(appIdFile.Data))
                    appId = reader.ReadLine();
            }

            // every song contains gamesxblock but may not contain showlights.xml
            var singleSongCount = _archive.TOC.Where(x => x.Name.Contains(".xblock") && x.Name.Contains("nsongs"));
            // this foreach loop addresses song packs otherwise it is only done one time
            foreach (var singleSong in singleSongCount)
            {
                var currentSong = new SongData
                {
                    CharterName = author,
                    Version = version,
                    ToolkitVer = tkversion,
                    AppID = appId,
                    FilePath = _filePath,
                    FileDate = fInfo.LastWriteTimeUtc,
                    FileSize = (int)fInfo.Length
                };

                if (toolkitVersionFile == null)
                {
                    currentSong.Tagged = SongTaggerStatus.ODLC;
                    if (String.IsNullOrEmpty(author))
                        currentSong.CharterName = "Ubisoft";
                }
                else
                    currentSong.Tagged = tagged ? SongTaggerStatus.True : SongTaggerStatus.False;

                var strippedName = singleSong.Name.Replace(".xblock", "").Replace("gamexblocks/nsongs/", "");
                if (strippedName.Contains("_fcp_dlc"))
                    strippedName = strippedName.Replace("_fcp_dlc", "");

                var infoFiles = _archive.TOC.Where(x => x.Name.StartsWith("manifests/songs") && x.Name.EndsWith(".json") && x.Name.Contains(strippedName)).OrderBy(x => x.Name);

                // speed hack ... some song info is only needed one time
                bool gotSongInfo = false;
                var arrangmentsFromPsarc = new FilteredBindingList<Arrangement>();

                // looping through song multiple times gathering each arrangement
                foreach (var entry in infoFiles)
                {
                    _archive.InflateEntry(entry);
                    var ms = new MemoryStream();
                    using (var reader = new StreamReader(ms, new UTF8Encoding(), false, 65536)) //4Kb is default alloc sise for windows.. 64Kb is default PSARC alloc
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
                            currentSong.DLCKey = attributes["SongKey"].ToString();
                            currentSong.Artist = attributes["ArtistName"].ToString();
                            currentSong.ArtistSort = attributes["ArtistNameSort"].ToString();
                            currentSong.Title = attributes["SongName"].ToString();
                            currentSong.TitleSort = attributes["SongNameSort"].ToString();
                            currentSong.Album = attributes["AlbumName"].ToString();
                            currentSong.AlbumSort = attributes["AlbumNameSort"].ToString();
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

                        // get vocal arrangment info
                        if (arrName.ToLower().Contains("vocal"))
                            arrangmentsFromPsarc.Add(new Arrangement(currentSong)
                                {
                                    PersistentID = attributes["PersistentID"].ToString(),
                                    Name = arrName
                                });
                        else
                        {
                            arrangmentsFromPsarc.Add(new Arrangement(currentSong)
                                {
                                    PersistentID = attributes["PersistentID"].ToString(),
                                    Name = arrName,
                                    Tuning = PsarcExtensions.TuningToName(attributes["Tuning"].ToString(), Globals.TuningXml),
                                    DMax = Convert.ToInt32(attributes["MaxPhraseDifficulty"].ToString()),
                                    ToneBase = attributes["Tone_Base"].ToString(),
                                    SectionCount = attributes["Sections"].ToArray().Count()
                                });
                        }
                    }
                }

                currentSong.Arrangements2D = arrangmentsFromPsarc;
                songsFromPsarc.Add(currentSong);
            }

            sw.Stop();
            Globals.Log(String.Format("{0} parsing took: {1} (msec)", Path.GetFileName(_filePath), sw.ElapsedMilliseconds));

            return songsFromPsarc;
        }

        public void Dispose()
        {
            if (_fileStream != null)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
            if (_archive != null)
            {
                _archive.Dispose();
                _archive = null;
            }

            GC.SuppressFinalize(this);
        }

        public static bool ExtractAudio(string archiveName, string audioName, string previewName)
        {
            bool result = false;
            if (String.IsNullOrEmpty(audioName))
                return false;

            Globals.Log("Extracting Audio ... Please wait ...");
            // TODO: maintain app responsiveness during audio extraction

            using (var archive = new PSARC(true))
            using (var stream = File.OpenRead(archiveName))
            {
                archive.Read(stream, true);
                var wems = archive.TOC.Where(entry => entry.Name.StartsWith("audio/windows") && entry.Name.EndsWith(".wem")).ToList();

                if (wems.Count > 1)
                {
                    wems.Sort((e1, e2) =>
                        {
                            if (e1.Length < e2.Length)
                                return 1;
                            if (e1.Length > e2.Length)
                                return -1;
                            return 0;
                        });
                }

                if (wems.Count > 0)
                {
                    var top = wems[0];
                    archive.InflateEntry(top);
                    top.Data.Position = 0;
                    using (var FS = File.Create(audioName))
                    {
                        WwiseToOgg w2o = new WwiseToOgg(top.Data, FS);
                        result = w2o.ConvertToOgg();
                    }
                }

                if (!String.IsNullOrEmpty(previewName) && result && wems.Count > 0)
                {
                    var bottom = wems.Last();
                    archive.InflateEntry(bottom);
                    bottom.Data.Position = 0;
                    using (var FS = File.Create(previewName))
                    {
                        WwiseToOgg w2o = new WwiseToOgg(bottom.Data, FS);
                        result = w2o.ConvertToOgg();
                    }
                }
            }
            return result;
        }

    }
}