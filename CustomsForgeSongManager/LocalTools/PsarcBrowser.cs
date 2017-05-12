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
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.XmlRepository;
using Arrangement = CustomsForgeSongManager.DataObjects.Arrangement;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.Xml;
using RocksmithToolkitLib.Sng2014HSL;
using RocksmithToolkitLib.DLCPackage.Manifest2014;
using Newtonsoft.Json;
using GenTools;
using System.Windows.Forms;

namespace CustomsForgeSongManager.LocalTools
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

        public IEnumerable<SongData> GetSongData(bool getAnalyzerData = false)
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
            var packageComment = String.Empty;

            var toolkitVersionFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("toolkit.version")));
            if (toolkitVersionFile != null)
            {
                _archive.InflateEntry(toolkitVersionFile);
                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                author = tkInfo.PackageAuthor ?? "N/A";
                version = tkInfo.PackageVersion ?? "N/A";
                tkversion = tkInfo.ToolkitVersion ?? "N/A";

                packageComment = tkInfo.PackageComment;
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

            if (_filePath.Contains("songs.psarc"))
                singleSongCount = singleSongCount.Where(s => !s.Name.Contains("rs2"));

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
                    currentSong.CharterName = "Ubisoft";
                    currentSong.Tagged = SongTaggerStatus.ODLC;
                    currentSong.RepairStatus = RepairStatus.ODLC;
                }
                else
                {
                    currentSong.Tagged = tagged ? SongTaggerStatus.True : SongTaggerStatus.False;

                    // TODO: reconsider/simplify this
                    if (packageComment == null)
                        currentSong.RepairStatus = RepairStatus.NotRepaired;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("DD") && packageComment.Contains("Max5"))
                        currentSong.RepairStatus = RepairStatus.RepairedDDMaxFive;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("DD"))
                        currentSong.RepairStatus = RepairStatus.RepairedDD;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("Max5"))
                        currentSong.RepairStatus = RepairStatus.RepairedMaxFive;
                    else if (packageComment.Contains("Remastered"))
                        currentSong.RepairStatus = RepairStatus.Repaired;
                    else
                        currentSong.RepairStatus = RepairStatus.NotRepaired;
                }


                var strippedName = singleSong.Name.Replace(".xblock", "").Replace("gamexblocks/nsongs/", "");
                if (strippedName.Contains("_fcp_dlc"))
                    strippedName = strippedName.Replace("_fcp_dlc", "");

                var infoFiles = _archive.TOC.Where(x => x.Name.StartsWith("manifests/songs") && x.Name.EndsWith(".json") && x.Name.Contains(strippedName)).OrderBy(x => x.Name);

                if (_filePath.Contains("rs1comp")) //there seems to be a problem with single-word names (like Pearl Jam's 'Black'), causing multiple songs to be stacked together
                    if (infoFiles.Where(x => x.Name.Contains(strippedName + "_")).ToList().Count() != 0)
                        infoFiles = infoFiles.Where(x => x.Name.Contains(strippedName + "_")).OrderBy(x => x.Name);

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

                        if (Char.IsNumber(entry.Name[entry.Name.IndexOf(".json") - 1]))
                            arrName = arrName + entry.Name[entry.Name.IndexOf(".json") - 1];

                        // get vocal arrangment info
                        if (arrName.ToLower().Contains("vocal"))
                            arrangmentsFromPsarc.Add(new Arrangement(currentSong)
                                {
                                    PersistentID = attributes["PersistentID"].ToString(),
                                    Name = arrName
                                });
                        else
                        {
                            // fix for tuning 'Other' issue
                            if (Globals.TuningXml == null || Globals.TuningXml.Count == 0)
                                Globals.TuningXml = TuningDefinitionRepository.Instance.LoadTuningDefinitions(GameVersion.RS2014);

                            Arrangement arr = new Arrangement(currentSong);

                            if (getAnalyzerData) //this is a very lenghty process compared to the "regular" loading, seems to be mainly related to loading of XML/SNG files
                            {
                                var song2014Data = new Song2014();

                                if (currentSong.OfficialDLC)
                                {
                                    var arrSngEntry = _archive.TOC.FirstOrDefault(x => x.Name.EndsWith(".sng") && x.Name.ToLower().Contains(arrName.ToLower() + ".sng") && x.Name.Contains(strippedName));

                                    _archive.InflateEntry(arrSngEntry);

                                    var sngMS = new MemoryStream();
                                    using (var sngReader = new StreamReader(sngMS, new UTF8Encoding(), false, 65536))
                                    {
                                        arrSngEntry.Data.CopyTo(sngMS);
                                        arrSngEntry.Data.Position = 0;
                                        sngMS.Position = 0;

                                        var sngFile = Sng2014File.ReadSng(sngMS, new Platform(GamePlatform.Pc, GameVersion.RS2014));

                                        entry.Data.Position = 0;
                                        ms.Position = 0;
                                        var man = JsonConvert.DeserializeObject<Manifest2014<Attributes2014>>(reader.ReadToEnd());

                                        var atr = new Attributes2014();
                                        atr = man.Entries.ToArray()[0].Value.ToArray()[0].Value;

                                        song2014Data = new Song2014(sngFile, atr);
                                    }
                                }
                                else
                                {
                                    var arrXmlEntry = _archive.TOC.FirstOrDefault(x => x.Name.EndsWith(".xml") && x.Name.ToLower().Contains(arrName.ToLower()) && x.Name.Contains(strippedName));

                                    _archive.InflateEntry(arrXmlEntry);

                                    var xmlMS = new MemoryStream();
                                    using (var xmlReader = new StreamReader(xmlMS, new UTF8Encoding(), false, 65536))
                                    {
                                        arrXmlEntry.Data.CopyTo(xmlMS);
                                        arrXmlEntry.Data.Position = 0;
                                        xmlMS.Position = 0;

                                        using (var fileStream = File.Create(Path.Combine(Constants.WorkFolder, "sng.xml")))
                                        {
                                            xmlMS.Seek(0, SeekOrigin.Begin);
                                            xmlMS.CopyTo(fileStream);
                                        }

                                        song2014Data = Song2014.LoadFromFile(Path.Combine(Constants.WorkFolder, "sng.xml"));
                                    }
                                }



                                int octaveCount = 0;
                                int chordCount = 0;
                                bool isOctave = false;

                                var chordTemplates = song2014Data.ChordTemplates;
                                var arrProperties = song2014Data.ArrangementProperties;

                                var allLevelData = song2014Data.Levels;
                                var maxLevelNotes = new List<SongNote2014>();
                                var maxLevelChords = new List<SongChord2014>();
                                //var chordCountDict = new Dictionary<string, int>();
                                var chordNames = new List<string>();
                                var chordCounts = new List<int>();

                                for (int i = allLevelData.Count() - 1; i > 0; i--) //go from the highest level to prevent adding the lower level notes
                                {
                                    foreach (var note in allLevelData[i].Notes)
                                    {
                                        if (!maxLevelNotes.Any(n => n.Time == note.Time) && !maxLevelChords.Any(c => c.Time == note.Time))
                                            maxLevelNotes.Add(note);
                                    }

                                    foreach (var chord in allLevelData[i].Chords)
                                    {
                                        if (!maxLevelChords.Any(c => c.Time == chord.Time) && !maxLevelNotes.Any(n => n.Time == chord.Time))
                                            maxLevelChords.Add(chord);
                                    }
                                }

                                foreach (var chord in maxLevelChords)
                                {
                                    string chordName = song2014Data.ChordTemplates[chord.ChordId].ChordName;

                                    chordCount = 0;

                                    if (chordName == "")
                                        continue;

                                    if (chordNames.Where(c => c == chordName).Count() > 0)
                                        chordCounts[chordNames.IndexOf(chordName)] += 1;
                                    else
                                    {
                                        chordNames.Add(chordName);
                                        chordCounts.Add(1);
                                    }
                                }

                                foreach (var chord in maxLevelChords)
                                {
                                    var chordTemplate = chordTemplates[chord.ChordId];

                                    if (chordTemplate.ChordName != "") //check if the current chord has no name (those who don't usually are either double stops or octaves)
                                        continue;

                                    var chordFrets = chordTemplate.GetType().GetProperties().Where(p => p.Name.Contains("Fret")).ToList();

                                    for (int i = 0; i < chordFrets.Count() - 2; i++)
                                    {
                                        sbyte firstFret = (sbyte)chordFrets[i].GetValue(chordTemplate, null);
                                        sbyte secondFret = (sbyte)chordFrets[i + 1].GetValue(chordTemplate, null);
                                        sbyte thirdFret = (sbyte)chordFrets[i + 2].GetValue(chordTemplate, null);

                                        if (firstFret != -1 && secondFret == -1 || thirdFret != -1)
                                            isOctave = true;
                                    }

                                    if (isOctave)
                                        octaveCount++;
                                }

                                arr = new Arrangement(currentSong)
                                 {
                                     NoteCount = maxLevelNotes.Count(),
                                     ChordCount = maxLevelChords.Count(),
                                     HammerOnCount = maxLevelNotes.Count(n => n.HammerOn > 0),
                                     PullOffCount = maxLevelNotes.Count(n => n.PullOff > 0),
                                     HarmonicCount = maxLevelNotes.Count(n => n.Harmonic > 0),
                                     HarmonicPinchCount = maxLevelNotes.Count(n => n.HarmonicPinch > 0),
                                     FretHandMuteCount = maxLevelNotes.Count(n => n.Mute > 0) + maxLevelChords.Count(c => c.FretHandMute > 0),
                                     PalmMuteCount = maxLevelNotes.Count(n => n.PalmMute > 0) + maxLevelChords.Count(c => c.PalmMute > 0),
                                     PluckCount = maxLevelNotes.Count(n => n.Pluck > 0),
                                     SlapCount = maxLevelNotes.Count(n => n.Slap > 0),
                                     //PopCount = noteList.Count(n=>n.P > 0),
                                     SlideCount = maxLevelNotes.Count(n => n.SlideTo > 0),
                                     UnpitchedSlideCount = maxLevelNotes.Count(n => n.SlideUnpitchTo > 0),
                                     TremoloCount = maxLevelNotes.Count(n => n.Tremolo > 0),
                                     TapCount = maxLevelNotes.Count(n => n.Tap > 0),
                                     VibratoCount = maxLevelNotes.Count(n => n.Vibrato > 0),
                                     SustainCount = maxLevelNotes.Count(n => n.Sustain > 0.0f),
                                     BendCount = maxLevelNotes.Count(n => n.Bend > 0.0f),

                                     OctaveCount = octaveCount,
                                     ChordNames = chordNames,
                                     ChordCounts = chordCounts
                                 };
                            }

                            arr.PersistentID = attributes["PersistentID"].ToString();
                            arr.Name = arrName;
                            arr.Tuning = PsarcExtensions.TuningToName(attributes["Tuning"].ToString(), Globals.TuningXml);
                            arr.DMax = Convert.ToInt32(attributes["MaxPhraseDifficulty"].ToString());
                            arr.ToneBase = attributes["Tone_Base"].ToString();
                            arr.SectionCount = attributes["Sections"].ToArray().Count();

                            arrangmentsFromPsarc.Add(arr);
                        }
                    }
                }

                if (_filePath.Contains("songs.psarc"))
                {
                    if (currentSong.Album == null || currentSong.Album.Contains("Rocksmith") || currentSong.ArtistTitleAlbum.Contains(";;") || currentSong.LastConversionDateTime.Year == 1)
                        continue;
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
            // get contents of archive
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