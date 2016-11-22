using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.Xml;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage.Manifest2014;
using System.Reflection;

namespace CustomsForgeSongManager.LocalTools
{
    class PackageDataTools
    {
        public static void AddDefaultPackageVersion(ref DLCPackageData packageData)
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

        public static void ValidateData(DLCPackageData packageData, ref Song2014 songXml)
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

        public static DLCPackageData GetDataWithFixedTones(string srcFilePath)
        {

            var packageData = new DLCPackageData();
            var fnameWithoutExt = Path.GetFileNameWithoutExtension(srcFilePath);
            Platform platform = Packer.GetPlatform(srcFilePath);
            string unpackedDir = Path.Combine(Path.GetTempPath(), String.Format("{0}_{1}", fnameWithoutExt, platform.platform));

            string songVersion = string.Empty, songName = string.Empty;
            var psarcOld = new PsarcPackager();

            try
            {
                // using (var psarcOld = new PsarcPackager())
                packageData = psarcOld.ReadPackage(srcFilePath);
            }
            catch (InvalidDataException ex)
            {
                if (ex.Message.ToString().Contains("EOF"))
                {
                    var field = typeof(PsarcPackager).GetField("packageDir", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                    var workingDir = field.GetValue(psarcOld).ToString();

                    //packageData = DLCPackageData.LoadFromFolder(workingDir, platform, platform);

                    //foreach (var arr in packageData.Arrangements)
                    //    FixMissingTonesInXML(arr.SongXml.File);

                    foreach (var arr in Directory.EnumerateFiles(workingDir, "*.xml", SearchOption.AllDirectories))
                    {
                        if (!arr.ToLower().Contains("vocals") && !arr.ToLower().Contains("showlights"))
                            FixMissingTonesInXML(arr);
                    }

                    packageData = DLCPackageData.LoadFromFolder(workingDir, platform, platform);
                }
            }

            return packageData;
        }

        public static void FixMissingTonesInXML(string xmlPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(File.ReadAllText(xmlPath));

            var songNode = xmlDoc.SelectSingleNode("//song");
            var ebeatsNode = songNode.SelectSingleNode("//ebeats");
            var tonesNode = xmlDoc.CreateElement("tones");
            tonesNode.SetAttribute("count", "0");

            songNode.InsertAfter(tonesNode, ebeatsNode);

            xmlDoc.Save(xmlPath);
        }
    }
}
