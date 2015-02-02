using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage.Manifest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CFCDLCManager
{
    class Otherpieces
    {
        private void ProcessFiles(List<string> filesList)
        {
            //bool cleanupTempFolder = false;
            //if (!Directory.Exists(workingFolder))
            //{
            //    Directory.CreateDirectory(workingFolder);
            //    cleanupTempFolder = true;
            //    button1.Content = "Button2";
            //}
            //else
            //{
            //    //button1.Content = "Button3";
            //}

            foreach (string filePathAndName in filesList)
            {
                string unpackedDir = " ";
                string filePath = Path.GetDirectoryName(filePathAndName) + "\\";
                TuningDefinition tun = new TuningDefinition();
                tun.GameVersion = GameVersion.RS2014;
               

                var preview = "";
                var artist = "";
                var songName = "";
                var album = "";
                var tuning = "";
                var creator = "";
                var currentVersion = "";
                var newestVersion = "";
                var updated = "";
                //if (!filePath.IsValidPSARC()) continue;
                try
                {
                   // unpackedDir = Packer.Unpack(filePathAndName, workingFolder, false, false, false);
                }
                catch (IOException ex)
                {
                  //  LogError(filePathAndName, ex);
                    continue;
                }

                Attributes2014 attrs = null;
                var jsonFiles = Directory.GetFiles(unpackedDir, String.Format("*.json"), SearchOption.AllDirectories);
                if (jsonFiles.Length > 0 && !String.IsNullOrEmpty(jsonFiles[0]))
                    attrs = Manifest2014<Attributes2014>.LoadFromFile(jsonFiles[0]).Entries.ToArray()[0].Value.ToArray()[0].Value;

                if (attrs == null) continue;

                artist = attrs.ArtistName;
                songName = attrs.SongName;
                album = attrs.AlbumName;

                tuning = tun.NameFromStrings(attrs.Tuning, false);
            //    creator = GetAuthorFromMetadata(unpackedDir);
                updated = attrs.LastConversionDateTime;
                //   newestVersion = client.DownloadString();
               // currentVersion = GetVersionFromFileName(filePathAndName);

           //     _SongCollection.Add(new SongData
                //{
                //    Preview = "xyz",
                //    Artist = artist,
                //    Song = songName,
                //    Album = album,
                //    Tuning = tuning,
                //    Updated = updated,
                //    User = creator,
                //    NewestVersion = "TBA",
                //    CurrentVersion = currentVersion
              //  });
            }
        }
    }
}
