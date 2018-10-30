using System;
using System.Windows.Forms;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;
using RocksmithToolkitLib.XML;
using System.Xml;
using System.IO;
using RocksmithToolkitLib.PsarcLoader;
using RocksmithToolkitLib;
using System.Reflection;
using CustomsForgeSongManager.DataObjects;
using System.Text;
using System.Drawing;
using CustomsForgeSongManager.Forms;
using System.Threading;
using BetterDialog2 = CustomControls.BetterDialog2;


namespace CustomsForgeSongManager.LocalTools
{
    static class PackageDataTools
    {
        #region Package Info
        public static DLCPackageData AddPackageComment(this DLCPackageData packageData, string packageComment)
        {
            var arrComment = packageData.ToolkitInfo.PackageComment;
            if (String.IsNullOrEmpty(arrComment))
                arrComment = packageComment;
            else if (!arrComment.Contains(packageComment))
                arrComment = arrComment + " " + packageComment;

            packageData.ToolkitInfo.PackageComment = arrComment;

            return packageData;
        }

        public static DLCPackageData AddDefaultPackageVersion(this DLCPackageData packageData)
        {
            if (String.IsNullOrEmpty(packageData.ToolkitInfo.PackageVersion))
                packageData.ToolkitInfo.PackageVersion = "1";
            else
                packageData.ToolkitInfo.PackageVersion = packageData.ToolkitInfo.PackageVersion.GetValidVersion();

            return packageData;
        }

        public static DLCPackageData ValidatePackageDataName(this DLCPackageData packageData)
        {
            packageData.Name = packageData.Name.GetValidKey();

            return packageData;
        }

        public static Song2014 ValidateData(this Song2014 songXml, DLCPackageData packageData)
        {
            songXml.AlbumYear = packageData.SongInfo.SongYear.ToString().GetValidYear();
            songXml.ArtistName = packageData.SongInfo.Artist.GetValidAtaSpaceName();
            songXml.Title = packageData.SongInfo.SongDisplayName.GetValidAtaSpaceName();
            songXml.AlbumName = packageData.SongInfo.Album.GetValidAtaSpaceName();
            songXml.ArtistNameSort = packageData.SongInfo.ArtistSort.GetValidSortableName();
            songXml.SongNameSort = packageData.SongInfo.SongDisplayNameSort.GetValidSortableName();
            songXml.AlbumNameSort = packageData.SongInfo.AlbumSort.GetValidSortableName();
            songXml.AverageTempo = Convert.ToSingle(packageData.SongInfo.AverageTempo.ToString().GetValidTempo());

            return songXml;
        }
        #endregion

        #region Tone Fixing
        public static DLCPackageData GetDataWithFixedTones(string srcFilePath)
        {

            var packageData = new DLCPackageData();
            var fnameWithoutExt = Path.GetFileNameWithoutExtension(srcFilePath);
            Platform platform = Packer.GetPlatform(srcFilePath);
            var unpackedDir = Path.Combine(Path.GetTempPath(), String.Format("{0}_{1}", fnameWithoutExt, platform.platform));
            var songVersion = string.Empty;
            var songName = string.Empty;
            var psarcOld = new PsarcPackager();

            try
            {
                packageData = psarcOld.ReadPackage(srcFilePath);
            }
            catch (InvalidDataException ex)
            {
                if (ex.Message.ToString().Contains("EOF"))
                {
                    var field = typeof(PsarcPackager).GetField("packageDir", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                    var workingDir = field.GetValue(psarcOld).ToString();

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

        #endregion


        #region PackageRating Updater

        public static void UpdatePackageRating()
        {
            if (!Globals.PackageRatingNeedsUpdate)
            {
                Globals.Log("No PackageRating updates to process ...");
                Globals.UpdateInProgress = false;
                return;
            }

            ShowUpdaterWindow();
            // always wait for any PackageRating updates to finish
            Globals.UpdateInProgress = true;

            try
            {
                foreach (var sd in Globals.MasterCollection)
                {
                    if (!sd.NeedsUpdate)
                        continue;

                    // maintains correct ODLC "Ubisoft" author status
                    using (var toolkitVersionStream = new MemoryStream())
                    {
                        DLCPackageCreator.GenerateToolkitVersion(toolkitVersionStream, sd.PackageAuthor, sd.PackageVersion, sd.PackageComment, sd.PackageRating, sd.ToolkitVersion);
                        CFSM.RSTKLib.PSARC.PsarcExtensions.InjectArchiveEntry(sd.FilePath, "toolkit.version", toolkitVersionStream);
                        toolkitVersionStream.Dispose(); // CRITICAL
                        Globals.Log("Updated PackageRating in: " + sd.FileName + " ...");
                    }

                    // reset song UpdateRating
                    sd.NeedsUpdate = false;

                    // for dev debugging
                    //var toolkitVersionPath = CFSM.RSTKLib.PSARC.PsarcExtensions.ExtractArchiveFile(sd.FilePath, "toolkit.version", Path.GetTempPath());
                    //using (var browser = new PsarcLoader(sd.FilePath, true))
                    //{
                    //    var toolkitInfo = browser.ExtractToolkitInfo();
                    //}
                }
            }
            catch (Exception ex)
            {
                CloseUpdaterWindow();
                Globals.Log("<ERROR> PackageRating updates failed ...");
                Globals.Log(" - " + ex.Message);
                Globals.UpdateInProgress = false;
                return;
            }

            CloseUpdaterWindow();
            Globals.Log("PackageRating updates completed successfully ...");
            Globals.UpdateInProgress = false;
            Globals.PackageRatingNeedsUpdate = false;
            // force reload
            Globals.ReloadSetlistManager = true;
            Globals.ReloadDuplicates = true;
            Globals.ReloadSongManager = true;

            return;
        }

        private static CustomControls.BetterDialog2 alert = null;
        public static void ShowUpdaterWindow()
        {
            // wait for any PackageRating updates to finish
            Globals.Log("<CRITICAL WARNING> CDLC PackageRating updates are in progress ...");
            Globals.Log("<CRITICAL WARNING> DO NOT ATTEMPT TO EXIT CFSM ... ");
            Globals.Log("<CRITICAL WARNING> SEVERE CDLC DAMAGE WILL RESULT ...");
            Application.DoEvents(); // keep GUI responsive

            // show nice popup warning message
            var sb = new StringBuilder();
            sb.AppendLine("CDLC PackageRating updates are in progress ...");
            sb.AppendLine("");
            sb.AppendLine("DO NOT ATTEMPT TO EXIT CFSM ...");
            sb.AppendLine("SEVERE CDLC DAMAGE WILL RESULT");
            sb.AppendLine("PLEASE WAIT UNTIL THIS WINDOW CLOSES");
            alert = new BetterDialog2(sb.ToString(), "<CRITICAL README>", null, null, null, Bitmap.FromHicon(SystemIcons.Warning.Handle), "<CRITICAL WARNING>", 100, 100);
            alert.Show();
            Application.DoEvents();
        }

        public static void CloseUpdaterWindow()
        {
            if (alert != null)
            {
                alert.Close();
                alert.Dispose();
                alert = null;
            }
        }

        #endregion
    }
}
