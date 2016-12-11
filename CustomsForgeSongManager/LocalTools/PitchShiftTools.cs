using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CFSM.GenTools;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest2014.Tone;
using RocksmithToolkitLib.Sng;
using RocksmithToolkitLib.ToolkitTone;
using RocksmithToolkitLib.Xml;

namespace CustomsForgeSongManager.LocalTools
{
    class PitchShiftTools
    {
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

        public static DLCPackageData AddExtensionToSongName(DLCPackageData packageData, string ext)
        {
            packageData.Name = packageData.Name + ext;
            packageData.SongInfo.SongDisplayName = packageData.SongInfo.SongDisplayName + ext;

            return packageData;
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

        public static DLCPackageData AddPitchShiftedMsg(DLCPackageData packageData)
        {
            string pitchShiftedMessage = "Pitch Shifted by CFSM";

            var pitchShiftedComment = packageData.ToolkitInfo.PackageComment;
            if (String.IsNullOrEmpty(pitchShiftedComment))
                pitchShiftedComment = pitchShiftedMessage;
            else if (!pitchShiftedComment.Contains(pitchShiftedMessage))
                pitchShiftedComment = pitchShiftedComment + " " + pitchShiftedMessage;

            packageData.ToolkitInfo.PackageComment = pitchShiftedComment;

            return packageData;
        }
    }
}
