using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.ToolkitTone;
using RocksmithToolkitLib.Xml;

namespace CustomsForgeSongManager.LocalTools
{
    public class PitchShiftTools
    {
        #region Class Methods

        public static DLCPackageData AddExtensionToSongName(DLCPackageData packageData, string ext)
        {
            packageData.Name = packageData.Name + ext;
            packageData.SongInfo.SongDisplayName = packageData.SongInfo.SongDisplayName + ext;

            return packageData;
        }

        public static DLCPackageData AddPitchShiftPedalToTones(DLCPackageData packageData, int gitShift, int bassShift, int mixVal, int toneVal)
        {
            List<Tone2014> tones = new List<Tone2014>();
            packageData.TonesRS2014.ForEach(t => tones.Add(t.XmlClone()));

            Pedal2014 pedal = new Pedal2014();
            var pedals = ToolkitPedal.LoadFromResource(RocksmithToolkitLib.GameVersion.RS2014);
            var bassPitchShiftPedal = pedals.FirstOrDefault(p => p.Type == "Pedals" && p.DisplayName.Contains("MultiPitch"));
            var gitPitchShiftPedal = bassPitchShiftPedal; //Only one pich shift pedal for both guitar and bass

            bassPitchShiftPedal.Knobs[0].DefaultValue = bassShift; //Pitch
            bassPitchShiftPedal.Knobs[1].DefaultValue = mixVal; //Mix
            bassPitchShiftPedal.Knobs[2].DefaultValue = toneVal; //Tone

            gitPitchShiftPedal.Knobs[0].DefaultValue = gitShift;
            gitPitchShiftPedal.Knobs[1].DefaultValue = mixVal;
            gitPitchShiftPedal.Knobs[2].DefaultValue = toneVal;

            foreach (var tone in tones) //Move all tones up one slot and add pitch shifter to the first slot
            {
                if (tone.GearList.PrePedal3 != null) tone.GearList.PrePedal4 = tone.GearList.PrePedal3;
                if (tone.GearList.PrePedal2 != null) tone.GearList.PrePedal3 = tone.GearList.PrePedal2;
                if (tone.GearList.PrePedal1 != null) tone.GearList.PrePedal2 = tone.GearList.PrePedal1;

                if (tone.ToneDescriptors.Any(d => d.ToLower().Contains("bass")))
                    pedal = bassPitchShiftPedal.MakePedalSetting(RocksmithToolkitLib.GameVersion.RS2014);
                else
                    pedal = gitPitchShiftPedal.MakePedalSetting(RocksmithToolkitLib.GameVersion.RS2014);

                tone.GearList.PrePedal1 = pedal;
            }

            return packageData;
        }

        public static DLCPackageData GetSetArrInfo(DLCPackageData packageData, ref int gitShift, ref int bassShift, ref string ext)
        {
            foreach (var arr in packageData.Arrangements)
            {
                if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                    continue;

                if (!arr.Tuning.Contains("Bonus") && arr.ArrangementType != ArrangementType.Bass)
                    gitShift = arr.TuningStrings.String0;

                if (!arr.Tuning.Contains("Bonus") && arr.ArrangementType == ArrangementType.Bass)
                    bassShift = arr.TuningStrings.String0;

                if (arr.Tuning.Contains("Standard"))
                {
                    arr.Tuning = "E Standard";
                    arr.TuningStrings = new RocksmithToolkitLib.Xml.TuningStrings { String0 = 0, String1 = 0, String2 = 0, String3 = 0, String4 = 0, String5 = 0 };

                    ext = "-e-std";
                }
                else if (arr.Tuning.Contains("Drop"))
                {
                    arr.Tuning = "Drop D";
                    arr.TuningStrings = new RocksmithToolkitLib.Xml.TuningStrings { String0 = -2, String1 = 0, String2 = 0, String3 = 0, String4 = 0, String5 = 0 };

                    ext = "-drop-d";
                }

                if (arr.TuningPitch < 400) //If bass fix has been applied, reset reference pitch back to 440
                    arr.TuningPitch = 440;
            }

            return packageData;
        }

        // TODO: Debug this code and confirm proper operation
        public static bool PitchShiftSong(List<SongData> songs, bool overwriteFile = false)
        {
            // this method can also be used for a single song
            var srcFilePaths = FileTools.SongFilePaths(songs);
            var total = srcFilePaths.Count;
            var processed = 0;
            var failed = 0;
            var skipped = 0;
            GenericWorker.InitReportProgress();
            GenericWorker.ReportProgress(processed, total, skipped, failed);

            foreach (var srcFilePath in srcFilePaths)
            {
                var isSkipped = false;
                Globals.Log("Processing: " + Path.GetFileName(srcFilePath));
                processed++;

                var officialOrRepaired = FileTools.OfficialOrRepaired(srcFilePath);
                if (!String.IsNullOrEmpty(officialOrRepaired))
                {
                    if (officialOrRepaired.Contains("Official"))
                    {
                        Globals.Log(" - Skipped ODLC File");
                        skipped++;
                        isSkipped = true;
                    }
                }

                try
                {
                    var ext = string.Empty;
                    var finalPath = srcFilePath;
                    Globals.Log(" - Extracting CDLC artifacts");
                    DLCPackageData packageData;

                    //Try unpacking and if it throws InvalidDataException - fix arrangement XMLs
                    packageData = PackageDataTools.GetDataWithFixedTones(srcFilePath);

                    // TODO: need to confirm desired action
                    var alreadyPitchShifted = packageData.ToolkitInfo.PackageComment.Contains(Constants.TKI_PITCHSHIFT);
                    if (!overwriteFile && alreadyPitchShifted)
                    {
                        Globals.Log(" - Song is already pitch shifted");
                        skipped++;
                        isSkipped = true;
                    }

                    if (!isSkipped)
                    {
                        Globals.Log(" - Adding pitch shifting effect");
                        int gitShift = 0, bassShift = 0;
                        int mix = 100;
                        int tone = 50;

                        //Get info (amount of steps) and set correct tunings
                        packageData = GetSetArrInfo(packageData, ref gitShift, ref bassShift, ref ext);

                        packageData = AddPitchShiftPedalToTones(packageData, gitShift, bassShift, mix, tone);

                        //Add a comment
                        packageData = packageData.AddPackageComment(Constants.TKI_PITCHSHIFT);

                        //Add extension to the names and validate
                        packageData = AddExtensionToSongName(packageData, ext);
                        packageData = PackageDataTools.ValidatePackageDataName(packageData);

                        //Set correct names and regenerate xml
                        packageData = RegenerateXML(packageData);

                        Globals.Log(" - Repackaging");

                        if (overwriteFile)
                            finalPath = srcFilePath.Replace("_p.psarc", ext + "_p.psarc");

                        using (var psarcNew = new PsarcPackager(true))
                            psarcNew.WritePackage(finalPath, packageData, srcFilePath);

                        if (File.Exists(finalPath))
                        {
                            //TemporaryDisableDatabindEvent(() =>
                            //{
                            using (var browser = new PsarcBrowser(finalPath))
                            {
                                var songInfo = browser.GetSongData();

                                if (songInfo != null && Globals.SongCollection.Where(sng => sng.FilePath == finalPath).Count() == 0)
                                    Globals.SongCollection.Add(songInfo.First());
                            }
                            //  });

                            //  GenExtensions.InvokeIfRequired(dgvSongsMaster, delegate { dgvSongsMaster.Refresh(); });
                        }

                        Globals.Log(" - Pitch shifting was sucessful");
                    }
                }
                catch (Exception ex)
                {
                    Globals.Log(" - Pitch shifting failed: " + ex.Message);
                    failed++;

                }

                GenericWorker.ReportProgress(processed, total, skipped, failed);
            }


            if (processed > 0)
            {
                Globals.Log("CDLC pitch shifting completed ...");
                Globals.RescanSongManager = true;

                if (!Constants.DebugMode)
                    GenExtensions.CleanLocalTemp();
            }
            else
            {
                Globals.Log("No CDLC were pitch shifted ...");
                return false;
            }

            return true;
        }

        public static DLCPackageData RegenerateXML(DLCPackageData packageData)
        {
            foreach (var arr in packageData.Arrangements)
            {
                arr.Id = IdGenerator.Guid();
                arr.MasterId = RandomGenerator.NextInt();

                if (arr.ArrangementType == ArrangementType.Vocal || arr.ArrangementType == ArrangementType.ShowLight)
                    continue;

                var songXml = Song2014.LoadFromFile(arr.SongXml.File);
                arr.ClearCache();
                songXml.Title = packageData.SongInfo.SongDisplayName;
                songXml.Tuning = arr.TuningStrings;
                if (!String.IsNullOrEmpty(arr.ToneBase)) songXml.ToneBase = arr.ToneBase; //This tone stuff doesn't have to be set again, because it's not changed
                if (!String.IsNullOrEmpty(arr.ToneA)) songXml.ToneA = arr.ToneA;
                if (!String.IsNullOrEmpty(arr.ToneB)) songXml.ToneB = arr.ToneB;
                if (!String.IsNullOrEmpty(arr.ToneC)) songXml.ToneC = arr.ToneC;
                if (!String.IsNullOrEmpty(arr.ToneD)) songXml.ToneD = arr.ToneD;

                using (var stream = File.Open(arr.SongXml.File, FileMode.Create))
                    songXml.Serialize(stream);
            }

            return packageData;
        }

        #endregion
    }
}
