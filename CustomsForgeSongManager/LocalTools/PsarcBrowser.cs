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
using RocksmithToolkitLib.XML;
using RocksmithToolkitLib.Sng2014HSL;
using RocksmithToolkitLib.DLCPackage.Manifest2014;
using Newtonsoft.Json;
using Arrangement = CustomsForgeSongManager.DataObjects.Arrangement;
using System.Threading;
using GenTools;
using System.Globalization;

namespace CustomsForgeSongManager.LocalTools
{
    public sealed class PsarcBrowser : IDisposable
    {
        private PSARC _archive;
        private string _filePath;
        private Stream _fileStream;

        // Loads song archive file to memory.
        public PsarcBrowser(string fileName)
        {
            _filePath = fileName;
            _archive = new PSARC();
            _fileStream = File.OpenRead(_filePath);
            _archive.Read(_fileStream, true);
        }

        public Stream ExtractEntryData(Func<Entry, bool> entryLINQ)
        {
            var entry = _archive.TOC.Where(entryLINQ).FirstOrDefault();
            if (entry != null)
            {
                MemoryStream ms = new MemoryStream();
                _archive.InflateEntry(entry);
                if (entry.Data == null)
                    return null;

                entry.Data.Position = 0;
                entry.Data.CopyTo(ms);
                entry.Dispose();
                ms.Position = 0;
                return ms;
            }
            return null;
        }

        /// <summary>
        /// Retrieves all song and arrangement data from an archive
        /// May cause brain damage but it's effective and fast
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SongData> GetSongData()
        {
            Stopwatch sw = null;
            sw = new Stopwatch();
            sw.Restart();

            // speed hack and fix for tuning 'Other' issue
            if (Globals.TuningXml == null || Globals.TuningXml.Count == 0)
                Globals.TuningXml = TuningDefinitionRepository.Instance.LoadTuningDefinitions(GameVersion.RS2014);

            var songsData = new List<SongData>();
            var fInfo = new FileInfo(_filePath);
            var packageAuthor = String.Empty;
            var packageVersion = String.Empty;
            var packageComment = String.Empty;
            var toolkitVersion = String.Empty;
            var appId = String.Empty;

            var tagged = _archive.TOC.Any(entry => entry.Name == "tagger.org");

            var toolkitVersionFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("toolkit.version")));
            if (toolkitVersionFile != null)
            {
                _archive.InflateEntry(toolkitVersionFile);
                ToolkitInfo tkInfo = GeneralExtensions.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                packageAuthor = tkInfo.PackageAuthor ?? "Null";
                packageVersion = tkInfo.PackageVersion ?? "Null";
                packageComment = tkInfo.PackageComment ?? "Null";
                toolkitVersion = tkInfo.ToolkitVersion ?? "Null";
            }

            var appIdFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("appid.appid")));
            if (appIdFile != null)
            {
                _archive.InflateEntry(appIdFile);
                using (var reader = new StreamReader(appIdFile.Data))
                    appId = reader.ReadLine();
            }

            // every song contains gamesxblock but may not contain showlights.xml
            var xblockEntries = _archive.TOC.Where(x => x.Name.StartsWith("gamexblocks/nsongs") && x.Name.EndsWith(".xblock"));
            if (!xblockEntries.Any())
                throw new Exception("Could not find valid xblock file : " + _filePath);

            if (_filePath.Contains("songs.psarc"))
                xblockEntries = xblockEntries.Where(s => !s.Name.Contains("rs2"));

            var jsonData = new List<Manifest2014<Attributes2014>>();
            // this foreach loop addresses song packs otherwise it is only done one time
            foreach (var xblockEntry in xblockEntries)
            {
                var arrangments = new List<Arrangement>();
                bool gotSongInfo = false;
                var song = new SongData
                    {
                        PackageAuthor = packageAuthor,
                        PackageVersion = packageVersion,
                        PackageComment = packageComment,
                        ToolkitVersion = toolkitVersion,
                        AppID = appId,
                        FilePath = _filePath,
                        FileDate = fInfo.LastWriteTimeUtc,
                        FileSize = (int)fInfo.Length
                    };

                if (toolkitVersionFile == null)
                {
                    song.PackageAuthor = "Ubisoft";
                    song.Tagged = SongTaggerStatus.ODLC;
                    song.RepairStatus = RepairStatus.ODLC;
                }
                else
                {
                    song.Tagged = tagged ? SongTaggerStatus.True : SongTaggerStatus.False;

                    // address old songpack files with unknown repair status
                    if (packageComment.Contains("SongPack Maker v1.1") || (packageVersion.Contains("N/A") && (_filePath.Contains("_sp_") || _filePath.Contains("_songpack_"))))
                        song.RepairStatus = RepairStatus.Unknown;
                    else if (packageComment.Contains("N/A"))
                        song.RepairStatus = RepairStatus.NotRepaired;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("DD") && packageComment.Contains("Max5"))
                        song.RepairStatus = RepairStatus.RepairedDDMaxFive;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("DD"))
                        song.RepairStatus = RepairStatus.RepairedDD;
                    else if (packageComment.Contains("Remastered") && packageComment.Contains("Max5"))
                        song.RepairStatus = RepairStatus.RepairedMaxFive;
                    else if (packageComment.Contains("Remastered"))
                        song.RepairStatus = RepairStatus.Repaired;
                    else
                        song.RepairStatus = RepairStatus.NotRepaired;
                }

                // CAREFUL with use of Contains and Replace to avoid creating duplicates
                var strippedName = xblockEntry.Name.Replace(".xblock", "").Replace("gamexblocks/nsongs", "");
                if (strippedName.Contains("_fcp_dlc"))
                    strippedName = strippedName.Replace("fcp_dlc", "");

                var jsonEntries = _archive.TOC.Where(x => x.Name.StartsWith("manifests/songs") && x.Name.EndsWith(".json") && x.Name.Contains(strippedName)).OrderBy(x => x.Name).ToList();
                if (jsonEntries.Count > 6) // Remastered CDLC max with vocals
                    Debug.WriteLine("<WARNING> Manifest Count > 6 : " + _filePath);

                // looping through song multiple times gathering each arrangement
                foreach (var jsonEntry in jsonEntries)
                {
                    var manifest2014 = new Manifest2014<Attributes2014>();
                    // get song attributes from json entry
                    using (var ms = ExtractEntryData(x => x.Name.Equals(jsonEntry.Name)))
                    using (var readerJson = new StreamReader(ms, new UTF8Encoding(), true, 65536)) //4Kb is default alloc sise for windows.. 64Kb is default PSARC alloc
                        manifest2014 = JsonConvert.DeserializeObject<Manifest2014<Attributes2014>>(readerJson.ReadToEnd());

                    var attributes = manifest2014.Entries.ToArray()[0].Value.ToArray()[0].Value;

                    // speed hack - these don't change so skip after first pass
                    if (!gotSongInfo)
                    {
                        song.DLCKey = attributes.SongKey;
                        song.Artist = attributes.ArtistName;
                        song.Title = attributes.SongName;
                        song.Album = attributes.AlbumName;

                        try
                        {
                            song.TitleSort = attributes.SongNameSort;
                            song.ArtistSort = attributes.ArtistNameSort;
                            // fix for date string to DateTime conversion
                            DateTime dateTime = DateTime.Now;
                            DateTime.TryParse(attributes.LastConversionDateTime, out dateTime);
                            song.LastConversionDateTime = dateTime;
                            song.SongYear = attributes.SongYear;
                            song.SongLength = (double)attributes.SongLength;
                            song.SongAverageTempo = attributes.SongAverageTempo;
                            // NOTE: older CDLC do not have AlbumNameSort or SongVolume
                            // ODLC does not have SongVolume
                            song.AlbumSort = attributes.AlbumNameSort;
                            song.SongVolume = attributes.SongVolume;
                        }
                        catch (Exception ex) // CDLC may still be usable
                        {
                            Globals.Log("<WARNING> CDLC is missing some basic song information meta data ...");
                            Globals.Log(ex.Message + " : " + ex.InnerException);
                            Globals.Log(" - " + Path.GetFileName(_filePath));
                            Globals.Log(" - This CDLC may still be usable but it should be updated if a newer version is available ...");
                        }

                        gotSongInfo = true;
                    }

                    var arr = new Arrangement(song);
                    var arrName = attributes.ArrangementName;
                    if (Char.IsNumber(jsonEntry.Name[jsonEntry.Name.IndexOf(".json") - 1]))
                        arrName = arrName + jsonEntry.Name[jsonEntry.Name.IndexOf(".json") - 1];

                    if (!arrName.ToLower().EndsWith("vocals"))
                    {
                        arr.SectionCount = attributes.Sections.Count();
                        arr.DDMax = attributes.MaxPhraseDifficulty;
                        arr.Tuning = PsarcExtensions.TuningToName(attributes.Tuning, Globals.TuningXml);

                        if (!String.IsNullOrEmpty(attributes.Tone_Base))
                        {
                            arr.ToneBase = attributes.Tone_Base;
                            arr.Tones = String.Format("(Base) {0}", arr.ToneBase);
                        }

                        try
                        {
                            if (!String.IsNullOrEmpty(attributes.Tone_A))
                                arr.Tones = String.Join(", (A) ", arr.Tones, attributes.Tone_A);
                            if (!String.IsNullOrEmpty(attributes.Tone_B))
                                arr.Tones = String.Join(", (B) ", arr.Tones, attributes.Tone_B);
                            if (!String.IsNullOrEmpty(attributes.Tone_C))
                                arr.Tones = String.Join(", (C) ", arr.Tones, attributes.Tone_C);
                            if (!String.IsNullOrEmpty(attributes.Tone_D))
                                arr.Tones = String.Join(", (D) ", arr.Tones, attributes.Tone_D);
                            if (!String.IsNullOrEmpty(attributes.Tone_Multiplayer))
                                arr.Tones = String.Join(", (M) ", arr.Tones, attributes.Tone_Multiplayer);
                        }
                        catch
                        {
                            // DO NOTHING
                        }

                        // loading SNG is 5X faster than loading XML and ODLC does not have XML
                        var song2014 = new Song2014();
                        var sngEntry = _archive.TOC.FirstOrDefault(x => x.Name.EndsWith(".sng") && x.Name.ToLower().Contains(arrName.ToLower() + ".sng") && x.Name.Contains(strippedName));
                        using (var ms = ExtractEntryData(x => x.Name.Equals(sngEntry.Name)))
                        {
                            var sng2014File = Sng2014File.ReadSng(ms, new Platform(GamePlatform.Pc, GameVersion.RS2014));
                            song2014 = new Song2014(sng2014File, attributes);
                        }

                        int octaveCount = 0;
                        int chordCount = 0;
                        int highestFretUsed = 0;
                        int maxChordFret = 0;
                        bool isOctave = false;
                        var chordTemplates = song2014.ChordTemplates;
                        var arrProperties = song2014.ArrangementProperties;
                        var allLevelData = song2014.Levels;
                        var maxLevelNotes = new List<SongNote2014>();
                        var maxLevelChords = new List<SongChord2014>();
                        var chordNames = new List<string>();
                        var chordCounts = new List<int>();
                        int bassPick = 0;
                        if (song2014.ArrangementProperties.PathBass == 1)
                            bassPick = (int)song2014.ArrangementProperties.BassPick;

                        for (int i = allLevelData.Count() - 1; i >= 0; i--) // go from the highest level to prevent adding the lower level notes
                        {
                            foreach (var note in allLevelData[i].Notes)
                            {
                                if (!maxLevelNotes.Any(n => n.Time == note.Time) && !maxLevelChords.Any(c => c.Time == note.Time))
                                    maxLevelNotes.Add(note);

                                if (note.Fret > highestFretUsed)
                                    highestFretUsed = note.Fret;
                            }

                            foreach (var chord in allLevelData[i].Chords)
                            {
                                if (!maxLevelChords.Any(c => c.Time == chord.Time) && !maxLevelNotes.Any(n => n.Time == chord.Time))
                                    maxLevelChords.Add(chord);

                                if (chord.ChordNotes != null)
                                {
                                    maxChordFret = chord.ChordNotes.Max(n => n.Fret);
                                    if (maxChordFret > highestFretUsed)
                                        highestFretUsed = maxChordFret;
                                }
                            }
                        }

                        foreach (var chord in maxLevelChords)
                        {
                            string chordName = song2014.ChordTemplates[chord.ChordId].ChordName.Replace(" ", string.Empty);

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

                        arr.NoteCount = maxLevelNotes.Count();
                        arr.ChordCount = maxLevelChords.Count();
                        arr.HammerOnCount = maxLevelNotes.Count(n => n.HammerOn > 0);
                        arr.PullOffCount = maxLevelNotes.Count(n => n.PullOff > 0);
                        arr.HarmonicCount = maxLevelNotes.Count(n => n.Harmonic > 0);
                        arr.HarmonicPinchCount = maxLevelNotes.Count(n => n.HarmonicPinch > 0);
                        arr.FretHandMuteCount = maxLevelNotes.Count(n => n.Mute > 0) + maxLevelChords.Count(c => c.FretHandMute > 0);
                        arr.PalmMuteCount = maxLevelNotes.Count(n => n.PalmMute > 0) + maxLevelChords.Count(c => c.PalmMute > 0);
                        arr.PluckCount = maxLevelNotes.Count(n => n.Pluck > 0);
                        arr.SlapCount = maxLevelNotes.Count(n => n.Slap > 0);
                        arr.SlideCount = maxLevelNotes.Count(n => n.SlideTo > 0);
                        arr.UnpitchedSlideCount = maxLevelNotes.Count(n => n.SlideUnpitchTo > 0);
                        arr.TremoloCount = maxLevelNotes.Count(n => n.Tremolo > 0);
                        arr.TapCount = maxLevelNotes.Count(n => n.Tap > 0);
                        arr.VibratoCount = maxLevelNotes.Count(n => n.Vibrato > 0);
                        arr.SustainCount = maxLevelNotes.Count(n => n.Sustain > 0.0f);
                        arr.BendCount = maxLevelNotes.Count(n => n.Bend > 0.0f);
                        arr.OctaveCount = octaveCount;
                        arr.ChordNames = chordNames;
                        arr.ChordCounts = chordCounts;
                        arr.HighestFretUsed = highestFretUsed;
                        arr.TuningPitch = Convert.ToDouble(song2014.CentOffset).Cents2Frequency();
                        arr.CapoFret = song2014.Capo == 0xFF ? 0 : Convert.ToInt16(song2014.Capo);

                        arr.AccentsCount = maxLevelNotes.Count(n => n.Accent > 0);

                        if (arrName.ToLower().Equals("bass"))
                            arr.BassPick = bassPick;
                    }

                    // add a smidge of arr info for vocals too!
                    arr.PersistentID = attributes.PersistentID;
                    arr.Name = arrName;
                    arrangments.Add(arr);
                }

                if (_filePath.Contains("songs.psarc"))
                {
                    if (song.Album == null || song.Album.Contains("Rocksmith") || song.ArtistTitleAlbum.Contains(";;") || song.LastConversionDateTime.Year == 1)
                        continue;
                }

                song.Arrangements2D = arrangments;
                songsData.Add(song);
            }

            sw.Stop();
            Globals.Log(String.Format(" - {0} parsing took: {1} (msec)", Path.GetFileName(_filePath), sw.ElapsedMilliseconds));

            return songsData;
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

        #region Class Methods

        public static bool ExtractAudio(string archiveName, string audioName, string previewName)
        {
            bool result = false;
            if (String.IsNullOrEmpty(audioName))
                return false;

            Globals.Log("Extracting Audio ...");
            Globals.Log("Please wait ...");
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

        #endregion
    }
}