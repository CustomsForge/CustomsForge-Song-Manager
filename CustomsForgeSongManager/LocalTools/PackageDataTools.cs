using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Xml;

namespace CustomsForgeSongManager.LocalTools
{
    class PackageDataTools
    {
        public static void AddDefaultPackageVersion (ref DLCPackageData packageData)
        {
            if (String.IsNullOrEmpty(packageData.PackageVersion))
                packageData.PackageVersion = "1";
            else
                packageData.PackageVersion = packageData.PackageVersion.GetValidVersion();
        }

        public static void ValidatePackageDataName(ref DLCPackageData packageData)
        {
            packageData.Name = packageData.Name.GetValidKey();
        }

        public static void ValidateData(DLCPackageData packageData, Song2014 songXml)
        {
            songXml.AlbumYear = packageData.SongInfo.SongYear.ToString().GetValidYear();
            songXml.ArtistName = packageData.SongInfo.Artist.GetValidAtaSpaceName();
            songXml.Title = packageData.SongInfo.SongDisplayName.GetValidAtaSpaceName();
            songXml.AlbumName = packageData.SongInfo.Album.GetValidAtaSpaceName();
            songXml.ArtistNameSort = packageData.SongInfo.ArtistSort.GetValidSortableName();
            songXml.SongNameSort = packageData.SongInfo.SongDisplayNameSort.GetValidSortableName();
            songXml.AlbumNameSort = packageData.SongInfo.AlbumSort.GetValidSortableName();
            songXml.AverageTempo = Convert.ToSingle(packageData.SongInfo.AverageTempo.ToString().GetValidTempo());
        }

        public static void AddPitchShiftedMsg(ref DLCPackageData packageData)
        {
            string pitchShiftedMessage = "Pitch Shifted by CFSM";

            var pitchShiftedComment = packageData.PackageComment;
            if (String.IsNullOrEmpty(pitchShiftedComment))
                pitchShiftedComment = pitchShiftedMessage;
            else if (!pitchShiftedComment.Contains(pitchShiftedMessage))
                pitchShiftedComment = pitchShiftedComment + " " + pitchShiftedMessage;

            packageData.PackageComment = pitchShiftedComment;
        }
    }
}
