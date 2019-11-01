using System.Diagnostics;
using CFSM.AudioTools;
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
using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Header;
using RocksmithToolkitLib.PSARC;
using MiscUtil.IO;
using RocksmithToolkitLib.Ogg;


namespace CustomsForgeSongManager.LocalTools
{
    public sealed class PsarcBrowser : IDisposable
    {
        private PSARC _archive;
        private string _filePath;
        private string _fileName;
        private Stream _fileStream;

        // Loads song archive file to memory.
        public PsarcBrowser(string filePath)
        {
            _filePath = filePath;
            _fileName = Path.GetFileName(_filePath);
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
            Platform platform = _filePath.GetPlatform();
            var toolkitVersion = String.Empty;
            var packageAuthor = String.Empty;
            var packageVersion = String.Empty;
            var packageComment = String.Empty;
            var packageRating = String.Empty;
            var appId = String.Empty;

            var tagged = _archive.TOC.Any(entry => entry.Name == "tagger.org");

            var toolkitVersionFile = _archive.TOC.FirstOrDefault(x => x.Name.Equals("toolkit.version"));
            if (toolkitVersionFile != null)
            {
                _archive.InflateEntry(toolkitVersionFile);
                ToolkitInfo tkInfo = GeneralExtension.GetToolkitInfo(new StreamReader(toolkitVersionFile.Data));
                toolkitVersion = tkInfo.ToolkitVersion ?? "Null";
                packageAuthor = tkInfo.PackageAuthor ?? "Null";
                packageVersion = tkInfo.PackageVersion ?? "Null";
                packageComment = tkInfo.PackageComment ?? "Null";
                packageRating = tkInfo.PackageRating ?? "Null";
            }

            var appIdFile = _archive.TOC.FirstOrDefault(x => (x.Name.Equals("appid.appid")));
            if (appIdFile != null)
            {
                _archive.InflateEntry(appIdFile);
                using (var reader = new StreamReader(appIdFile.Data))
                    appId = reader.ReadLine();
            }

            // every song contains gamesxblock but may not contain showlights.xml
            var xblockEntries = _archive.TOC.Where(x => x.Name.StartsWith("gamexblocks/nsongs") && x.Name.EndsWith(".xblock")).ToList();
            if (!xblockEntries.Any())
                throw new Exception("Could not find valid xblock file : " + _filePath);

            if (_filePath.ToLower().EndsWith(Constants.BASESONGS) || _filePath.ToLower().EndsWith(Constants.BASESONGSDISABLED))
                xblockEntries = xblockEntries.Where(s => !s.Name.Contains("rs2")).ToList();

            var jsonData = new List<Manifest2014<Attributes2014>>();
            // this foreach loop addresses song packs otherwise it is only done one time
            foreach (var xblockEntry in xblockEntries)
            {
                var arrangements = new List<Arrangement>();
                bool gotSongInfo = false;
                var song = new SongData
                {
                    ToolkitVersion = toolkitVersion,
                    PackageAuthor = packageAuthor,
                    PackageVersion = packageVersion,
                    PackageComment = packageComment,
                    PackageRating = packageRating,
                    AppID = appId,
                    FilePath = _filePath,
                    FileDate = fInfo.LastWriteTime,
                    FileSize = (int)fInfo.Length
                };

                if (toolkitVersionFile == null || packageAuthor == "Ubisoft")
                {
                    song.PackageAuthor = "Ubisoft";
                    song.Tagged = SongTaggerStatus.ODLC;
                    song.RepairStatus = RepairStatus.ODLC;

                    if (String.IsNullOrEmpty(packageRating) || packageRating == "Null")
                        song.PackageRating = "5";
                }
                else
                {
                    song.Tagged = tagged ? SongTaggerStatus.True : SongTaggerStatus.False;

                    // address old songpack files with unknown repair status
                    if (packageComment.Contains("SongPack Maker v1.1") || (packageVersion.Contains("N/A") || packageVersion.Contains("Null") && (_filePath.Contains("_sp_") || _filePath.Contains("_songpack_"))))
                        song.RepairStatus = RepairStatus.Unknown;
                    else if (packageComment.Contains("N/A") || packageComment.Contains("Null"))
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
                // may be it is a songpack file
                if (jsonEntries.Count > 6) // Remastered CDLC max with vocals
                    Debug.WriteLine("<WARNING> Manifest Count > 6 : " + _filePath);

                int songBPMChangeCount = -1;
                float songMinBPM = -1;
                float songMaxBPM = -1;
                int songTimeSignatureChangeCount = -1;

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
                            // permafix for LastConversionDateTime string to DateTime conversion
                            // LastConversionDateTime stored as string in en-US format, e.g. 08-15-13 16:13
                            // convert to culture independent DateTime {8/15/2013 4:13:00 PM}             
                            CultureInfo cultureInfo = new CultureInfo("en-US");
                            DateTime dt = DateTime.Parse(attributes.LastConversionDateTime, cultureInfo, DateTimeStyles.NoCurrentDateDefault);
                            song.LastConversionDateTime = dt;
                            song.SongYear = attributes.SongYear;
                            song.SongLength = (double)attributes.SongLength;
                            song.SongAverageTempo = attributes.SongAverageTempo;
                            // NOTE: older CDLC do not have AlbumNameSort or SongVolume
                            song.AlbumSort = attributes.AlbumNameSort;
                            song.SongVolume = attributes.SongVolume;

                            // try to get SongVolume from main audio bnk file 
                            if (song.SongVolume == null && !song.IsRsCompPack && !song.IsSongPack && !song.IsSongsPsarc)
                            {
                                var bnkEntry = _archive.TOC.FirstOrDefault(x => x.Name.StartsWith("audio/") && x.Name.EndsWith(".bnk") && !x.Name.EndsWith("_preview.bnk"));
                                if (bnkEntry == null)
                                    throw new Exception("Could not find valid bnk file : " + _filePath);

                                _archive.InflateEntry(bnkEntry);

                                var bnkPath = Path.GetTempFileName();
                                using (var fs = File.Create(bnkPath))
                                {
                                    bnkEntry.Data.Seek(0, SeekOrigin.Begin);
                                    bnkEntry.Data.CopyTo(fs);
                                }

                                song.SongVolume = SoundBankGenerator2014.ReadVolumeFactor(bnkPath, platform);
                                File.Delete(bnkPath);
                            }

                            // get main wem audio bitrate (kbps)
                            var wems = _archive.TOC.Where(entry => entry.Name.StartsWith("audio/") && entry.Name.EndsWith(".wem")).ToList();
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

                            // assumes main audio is larger wem file (may not always be correct)
                            if (wems.Count > 0)
                            {
                                var top = wems[0];
                                _archive.InflateEntry(top);
                                top.Data.Position = 0;
                                var wemSize = top.Data.Length;
                                
                                // slow actual kbps
                                // WemFile wemFile = new WemFile(top.Data, platform);
                                // var kbpsActual = (int)wemFile.AverageBytesPerSecond * 8 / 1000;
                                
                                // fast approximate kbps (very close)
                                var kbpsApprox = (int)(wemSize * 8 / song.SongLength / 1000);
                                song.AudioBitRate = kbpsApprox;
                            }

                            // does the CDLC have a CustomFont
                            var hasCustomFont = _archive.TOC.Any(x => x.Name.Contains("/lyrics_") && x.Name.EndsWith(".dds"));
                            song.HasCustomFont = hasCustomFont;

                            // REM - the above is done only 1X for max speed
                        }
                        catch (Exception ex) // CDLC may still be usable
                        {
                            Globals.Log("<WARNING> CDLC is missing some basic song information meta data ...");
                            Globals.Log(" - " + ex.Message + " : " + ex.InnerException);
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
                        // Arrangement Attributes used by SongManager            
                        arr.Tuning = PsarcExtensions.TuningToName(attributes.Tuning, Globals.TuningXml);
                        arr.TuningPitch = Convert.ToDouble(attributes.CentOffset).Cents2Frequency();
                        arr.DDMax = attributes.MaxPhraseDifficulty;
                        // ScrollSpeed as defined by toolkit
                        arr.ScrollSpeed = attributes.DynamicVisualDensity.Last();

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

                        // parse Arrangment Analyzer data (slow process, only done if requested by user)
                        if (AppSettings.Instance.IncludeArrangementData)
                        {
                            // quick load HSAN file data to get SongDifficulty
                            var hsanEntries = new ManifestHeader2014<AttributesHeader2014>(platform);
                            var hsanEntry = _archive.TOC.FirstOrDefault(x => x.Name.StartsWith("manifests/songs") && x.Name.EndsWith(".hsan"));

                            if (hsanEntry == null)
                                throw new Exception("Could not find valid hsan manifest in archive.");

                            using (var ms = ExtractEntryData(x => x.Name.Equals(hsanEntry.Name)))
                            using (var readerJson = new StreamReader(ms, new UTF8Encoding(), true, 65536))
                                hsanEntries = JsonConvert.DeserializeObject<ManifestHeader2014<AttributesHeader2014>>(readerJson.ReadToEnd());

                            // use unique PID instead of ArrangementName which may not be unique in ODLC
                            var arrPID = attributes.PersistentID;
                            arr.SongDifficulty = hsanEntries.Entries[arrPID].ToArray()[0].Value.SongDifficulty;

                            // loading SNG is 5X faster than loading XML and ODLC do not have XML
                            var song2014 = new Song2014();
                            var sngEntry = _archive.TOC.FirstOrDefault(x => x.Name.EndsWith(".sng") && x.Name.ToLower().Contains(arrName.ToLower() + ".sng") && x.Name.Contains(strippedName));
                            using (var ms = ExtractEntryData(x => x.Name.Equals(sngEntry.Name)))
                            {
                                // Platform platform = _filePath.GetPlatform();
                                var sng2014File = Sng2014File.ReadSng(ms, platform);
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
                            var eBeats = song2014.Ebeats;
                            var maxLevelNotes = new List<SongNote2014>();
                            var maxLevelChords = new List<SongChord2014>();
                            var maxLevelHandShapes = new List<SongHandShape>();
                            var chordNames = new List<string>();
                            var chordCounts = new List<int>();
                            int bassPick = 0;
                            int thumbCount = 0;
                            int pitchedChordSlideCount = 0;
                            int bpmChangeCount = 0;
                            float maxBPM = 0;
                            float minBPM = 999;
                            int timeSignatureChangeCount = 0;

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

                                foreach (var hs in allLevelData[i].HandShapes)
                                {
                                    if (!maxLevelHandShapes.Any(h => h.StartTime == hs.StartTime))
                                        maxLevelHandShapes.Add(hs);
                                }
                            }

                            pitchedChordSlideCount = maxLevelChords.Count(c => c.LinkNext == 1);

                            if (songTimeSignatureChangeCount == -1 && eBeats.Count(b => b.Measure != -1) > 1) //no need to rescan for each arrangement, because songs should have same beatmap for all of them
                            {
                                var secondMeasure = eBeats.Skip(1).FirstOrDefault(b => b.Measure != -1);
                                int? secondMeasureIdx = Array.IndexOf(eBeats, secondMeasure);
                                int difference = (int)secondMeasureIdx;
                                int currentIdx = (int)secondMeasureIdx;
                                int beatCount = eBeats.Count();
                                int nextIdx = currentIdx + difference;

                                var third = eBeats[currentIdx + difference * 2];

                                var currentMeasure = eBeats[0];
                                var nextMeasure = secondMeasure;

                                float currentBPM = 0;
                                float oldBPM = 0;
                                List<float> currentBPMs = new List<float>();

                                while (nextIdx < beatCount)
                                {

                                    if (nextIdx != -1 && eBeats[nextIdx].Measure == -1)
                                    {
                                        nextMeasure = eBeats.FirstOrDefault(b => b.Time > nextMeasure.Time && b.Measure != -1);
                                        nextIdx = Array.IndexOf(eBeats, nextMeasure);
                                        currentIdx = Array.IndexOf(eBeats, currentMeasure);
                                        difference = nextIdx - currentIdx;

                                        if (nextMeasure == null) //should mean we are out of bounds (or that the song doesn't end on a full measure)
                                            break;

                                        timeSignatureChangeCount++;
                                    }

                                    if (nextIdx == -1)
                                        minBPM = 0;

                                    currentBPM = (60 / ((nextMeasure.Time - currentMeasure.Time) / difference));

                                    // using toolkit range for tempo validation
                                    if (currentBPM > 0 && currentBPM < 999)
                                    {
                                        maxBPM = Math.Max(currentBPM, maxBPM);
                                        minBPM = Math.Min(currentBPM, minBPM);
                                        currentBPMs.Add(currentBPM);
                                    }

                                    if (currentBPM != oldBPM && Math.Abs(currentBPM - oldBPM) > AppSettings.Instance.BPMThreshold)
                                    {
                                        oldBPM = currentBPM;
                                        bpmChangeCount++;
                                    }

                                    currentMeasure = nextMeasure;
                                    nextMeasure = eBeats[nextIdx];
                                    nextIdx += difference;
                                }

                                songBPMChangeCount = bpmChangeCount;
                                songMaxBPM = maxBPM;
                                songMinBPM = minBPM;
                                songTimeSignatureChangeCount = timeSignatureChangeCount;

                                // confirm SongAverageTempo is within calculated range, and ODLC is not defaulted (120.0)
                                if (song.SongAverageTempo < minBPM || song.SongAverageTempo > maxBPM || (song.IsODLC && song.SongAverageTempo == 120.0))
                                    song.SongAverageTempo = currentBPMs.Average();
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

                            foreach (var hs in maxLevelHandShapes) //TODO: check for performance impact and optimize
                            {
                                if (chordTemplates[hs.ChordId].Finger0 != 0)
                                    continue;

                                var chords = maxLevelChords.Where(c => c.Time >= hs.StartTime && c.Time < hs.EndTime);
                                var notes = maxLevelNotes.Where(n => n.Time >= hs.StartTime && n.Time < hs.EndTime && n.String == 0);

                                thumbCount += chords.Count() + notes.Count();
                            }

                            arr.ChordNames = chordNames;
                            arr.ChordCounts = chordCounts;

                            // Arrangement Levels
                            arr.NoteCount = maxLevelNotes.Count();
                            arr.ChordCount = maxLevelChords.Count();
                            arr.AccentCount = maxLevelNotes.Count(n => n.Accent > 0);
                            arr.BendCount = maxLevelNotes.Count(n => n.Bend > 0.0f);
                            arr.FretHandMuteCount = maxLevelNotes.Count(n => n.Mute > 0) + maxLevelChords.Count(c => c.FretHandMute > 0);
                            arr.HammerOnCount = maxLevelNotes.Count(n => n.HammerOn > 0);
                            arr.HarmonicCount = maxLevelNotes.Count(n => n.Harmonic > 0);
                            arr.HarmonicPinchCount = maxLevelNotes.Count(n => n.HarmonicPinch > 0);
                            arr.HighestFretUsed = highestFretUsed;
                            // FIXME: toolkit hopo is always zero
                            //var hopoNote = maxLevelNotes.Count(n => n.Hopo > 0);
                            //var hopoChord = maxLevelChords.Count(n => n.Hopo > 0);
                            //arr.HopoCount = hopoNote + hopoChord;
                            //if (arr.HopoCount > 0)
                            //    Debug.WriteLine("DebuMe");
                            //                        
                            arr.IgnoreCount = maxLevelNotes.Count(n => n.Ignore > 0);
                            arr.LinkNextCount = maxLevelNotes.Count(n => n.LinkNext > 0);
                            arr.OctaveCount = octaveCount;
                            arr.PalmMuteCount = maxLevelNotes.Count(n => n.PalmMute > 0) + maxLevelChords.Count(c => c.PalmMute > 0);
                            arr.PluckCount = maxLevelNotes.Count(n => n.Pluck > 0);
                            arr.PullOffCount = maxLevelNotes.Count(n => n.PullOff > 0);
                            arr.SlapCount = maxLevelNotes.Count(n => n.Slap > 0);
                            arr.SlideCount = maxLevelNotes.Count(n => n.SlideTo > 0);
                            arr.SlideUnpitchToCount = maxLevelNotes.Count(n => n.SlideUnpitchTo > 0);
                            arr.SustainCount = maxLevelNotes.Count(n => n.Sustain > 0.0f);
                            arr.TapCount = maxLevelNotes.Count(n => n.Tap > 0);
                            arr.TremoloCount = maxLevelNotes.Count(n => n.Tremolo > 0);
                            arr.VibratoCount = maxLevelNotes.Count(n => n.Vibrato > 0);
                            arr.ThumbCount = thumbCount;
                            arr.PitchedChordSlideCount = pitchedChordSlideCount;
                            arr.TimeSignatureChangeCount = songTimeSignatureChangeCount;
                            arr.BPMChangeCount = songBPMChangeCount;
                            arr.MaxBPM = songMaxBPM;
                            arr.MinBPM = songMinBPM;

                            // Arrangement Properties
                            if (arrName.ToLower().Equals("bass"))
                                arr.BassPick = bassPick;

                            // using XML arrangement represent to avoid JSON represent bug found in old toolkit versions                  
                            arr.Represent = attributes.ArrangementProperties.Represent;
                            arr.BonusArr = attributes.ArrangementProperties.BonusArr;

                            // TODO: maybe extract all AP (not sure how useful data is though) 

                            arr.SectionsCount = song2014.Sections.ToList().Count();
                            arr.TonesCount = song2014.Tones.ToList().Count;
                            arr.CapoFret = song2014.Capo == 0xFF ? 0 : Convert.ToInt16(song2014.Capo);
                        }
                    }

                    // add a smidge of Arrangement Attributes for vocals too
                    arr.PersistentID = attributes.PersistentID;
                    arr.ArrangementName = arrName;

                    arrangements.Add(arr);
                }

                // log some songpacks parsing info
                if (_fileName.ToLower().EndsWith(Constants.BASESONGS) ||
                    _fileName.ToLower().EndsWith(Constants.BASESONGSDISABLED) ||
                    _fileName.ToLower().Contains(Constants.RS1COMP) ||
                    _fileName.ToLower().Contains(Constants.SONGPACK) ||
                    _fileName.ToLower().Contains(Constants.ABVSONGPACK))
                {
                    // ignore any non-song data from songpacks
                    if (song.Album == null || song.Album.Contains("Rocksmith") || song.ArtistTitleAlbum.Contains(";;"))
                        continue;

                    Globals.Log(String.Format(" + Parsed Song Pack: {0};{1}", _fileName, song.ArtistTitleAlbumDate));
                }

                song.Arrangements2D = arrangements;
                songsData.Add(song);
            }

            sw.Stop();
            // elimanted multiple log messages per users request
            Globals.Log(String.Format(" - Parsing took {1} (msec): {0}", _filePath, sw.ElapsedMilliseconds));

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

            // get contents of archive
            using (var archive = new PSARC(true))
            using (var stream = File.OpenRead(archiveName))
            {
                archive.Read(stream, true);
                var wems = archive.TOC.Where(entry => entry.Name.StartsWith("audio/") && entry.Name.EndsWith(".wem")).ToList();

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
                    Application.DoEvents();
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