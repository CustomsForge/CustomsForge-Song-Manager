﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RocksmithToTabLib;
using RocksmithToolkitLib.DLCPackage.Manifest;
using RocksmithToolkitLib.DLCPackage.Manifest.Functions;
using RocksmithToolkitLib.Xml;


namespace RocksmithToolkitLib.Song2014ToTab
{
    public class Gp5Converter : IDisposable
    {
        public void Dispose() { }

        #region PSARC to Song2014 to GuitarPro *.gp5 file

        /// <summary>
        /// Load a PSARC file into memory and
        /// convert to GuitarPro file(s)
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputDir"></param>
        /// <param name="songListShort"></param>
        /// <param name="outputFormat"></param>
        /// <param name="allDif"></param>
        public void PsarcToGp5(string inputFilePath, string outputDir, IList<SongInfoShort> songListShort = null, string outputFormat = "gp5", bool allDif = false)
        {
            Console.WriteLine("Opening archive {0} ...", inputFilePath);
            Console.WriteLine();

            try
            {
                var browser = new PsarcBrowser(inputFilePath);
                var songList = browser.GetSongList();
                var toolkitInfo = browser.GetToolkitInfo();

                // collect all songs to convert
                var toConvert = new List<SongInfo>();
                // if nothing was specified, convert all songs
                if (songListShort == null || songListShort.Count == 0)
                    toConvert = toConvert.Concat(songList).ToList();
                else
                    // convert only the user selected songs and arrangements
                    toConvert = SongInfoShortToSongInfo(songListShort, songList);

                foreach (var song in toConvert)
                {
                    var score = new Score();
                    // get all default or user specified arrangements for the song 
                    var arrangements = song.Arrangements;
                    Console.WriteLine("Converting song " + song.Identifier + "...");

                    foreach (var arr in arrangements)
                    {
                        var arrangement = (Song2014)browser.GetArrangement(song.Identifier, arr);
                        // get maximum difficulty for the arrangement
                        var mf = new ManifestFunctions(GameVersion.RS2014);
                        int maxDif = mf.GetMaxDifficulty(arrangement);

                        if (allDif) // create seperate file for each difficulty
                        {
                            for (int difLevel = 0; difLevel <= maxDif; difLevel++)
                            {
                                ExportArrangement(score, arrangement, difLevel, inputFilePath, toolkitInfo);
                                Console.WriteLine("Difficulty Level: {0}", difLevel);

                                var baseFileName = CleanFileName(
                                    String.Format("{0} - {1}", score.Artist, score.Title));
                                baseFileName += String.Format(" ({0})", arr);
                                baseFileName += String.Format(" (level {0:D2})", difLevel);

                                SaveScore(score, baseFileName, outputDir, outputFormat);
                                // remember to remove the track from the score again
                                score.Tracks.Clear();
                            }
                        }
                        else // combine maximum difficulty arrangements into one file
                        {
                            Console.WriteLine("Maximum Difficulty Level: {0}", maxDif);
                            ExportArrangement(score, arrangement, maxDif, inputFilePath, toolkitInfo);
                        }
                    }

                    if (!allDif) // only maximum difficulty
                    {
                        var baseFileName = CleanFileName(
                            String.Format("{0} - {1}", score.Artist, score.Title));
                        SaveScore(score, baseFileName, outputDir, outputFormat);
                    }
                }

                Console.WriteLine();
            }

            catch (IOException e)
            {
                Console.WriteLine("Error encountered:");
                Console.WriteLine(e.Message);
            }

        }


        static void ExportArrangement(Score score, Song2014 arrangement, int difficulty,
               string originalFile, ToolkitInfo toolkitInfo)
        {
            var track = Converter.ConvertArrangement(arrangement, arrangement.Part.ToString(), difficulty);
            score.Tracks.Add(track);
            score.Title = arrangement.Title;
            score.Artist = arrangement.ArtistName;
            score.Album = arrangement.AlbumName;
            score.Year = arrangement.AlbumYear;
            score.Comments = new List<string>();
            score.Comments.Add("Generated by RocksmithToTab v" + VersionInfo.VERSION);
            score.Comments.Add("=> https://github.com/fholger/RocksmithToTab");
            score.Comments.Add("Created from archive: " + Path.GetFileName(originalFile));
            if (toolkitInfo != null && toolkitInfo.PackageAuthor != string.Empty)
            {
                score.Comments.Add("CDLC author:  " + toolkitInfo.PackageAuthor);
                score.Tabber = toolkitInfo.PackageAuthor;
            }
            if (toolkitInfo != null && toolkitInfo.PackageVersion != string.Empty)
                score.Comments.Add("CDLC version: " + toolkitInfo.PackageVersion);
        }


        static GpxExporter gpxExporter = new GpxExporter();
        static GP5File gp5Exporter = new GP5File();

        static void SaveScore(Score score, string baseFileName, string outputDirectory, string outputFormat)
        {
            string basePath = Path.Combine(outputDirectory, baseFileName);
            // create a separate file for each arrangement
            if (outputFormat == "gp5")
            {
                gp5Exporter.ExportScore(score, basePath + ".gp5");
            }
            else if (outputFormat == "gpif")
            {
                gpxExporter.ExportGpif(score, basePath + ".gpif");
            }
            else
            {
                gpxExporter.ExportGPX(score, basePath + ".gpx");
            }

        }


        static string CleanFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var cleaned = fileName.Where(x => !invalidChars.Contains(x)).ToArray();
            return new string(cleaned);
        }

        #endregion

        #region SongInfoShort to SongInfo

        /// <summary>
        /// Convert SongInfoShort to SongInfo that contains only user selections or defualts
        /// </summary>
        /// <param name="songListShort"></param>
        /// <param name="songList"></param>
        /// <returns></returns>
        public List<SongInfo> SongInfoShortToSongInfo(IList<SongInfoShort> songListShort, IList<SongInfo> songList)
        {
            var songIdPre = String.Empty;
            var newSongList = new List<SongInfo>();
            var newSongNdx = 0;

            for (var i = 0; i < songListShort.Count(); i++)
            {
                var songIdShort = songListShort[i].Identifier;
                var arrangementShort = songListShort[i].Arrangement;

                if (songIdPre != songIdShort)
                {
                    // add the new song info
                    var songInfo = songList.FirstOrDefault(x => x.Identifier == songIdShort);
                    newSongList.Add(songInfo);
                    newSongNdx++;

                    // clear arrangments so we can add user selections
                    if (arrangementShort != null)
                    {
                        newSongList[newSongNdx - 1].Arrangements.Clear();
                        newSongList[newSongNdx - 1].Arrangements.Add(arrangementShort);
                    }
                }
                else if (songIdPre == songIdShort && arrangementShort != null)
                    newSongList[newSongNdx - 1].Arrangements.Add(arrangementShort);

                songIdPre = songIdShort;
            }

            return newSongList;
        }
        #endregion

    }

    /// <summary>
    /// Struct containing song Identifier and 
    /// song Arrangement information for a PSARC file
    /// </summary>
    public class SongInfoShort
    {
        public string Identifier { get; set; }
        public string Arrangement { get; set; }
    }

}
