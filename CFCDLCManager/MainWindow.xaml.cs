using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using CFCDLCManager;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.DLCPackage.Manifest;
using RocksmithToolkitLib.Extensions;
using System.IO;
using RocksmithToolkitLib;
using System.Text.RegularExpressions;
using System.Net;
using System.Security.Cryptography;
using System.ComponentModel;

namespace CDLCManagerCF
{
    /// <summary>
    /// CDLC Manager Customs Forge
    /// Credits:Darkken(borrowed a few functions:), Rocksmith Custom Toolkit Team, Lovroman
    /// </summary>
    public partial class MainWindow : Window
    {
        public string rocksmithPath = @"C:\Program Files (x86)\Steam\steamapps\common\Rocksmith";
        public string rocksmith2014Path = @"C:\Program Files (x86)\Steam\steamapps\common\Rocksmith2014";
        public string workingFolder = @"E:\testing";
        public string backupFolder = @"C:\Program Files (x86)\Steam\steamapps\common\Rocksmith2014\backup";

        private static string _artistSongSeparator = "_";
        private static string _spaceSeparator = "-";

        ObservableCollection<SongData> _SongCollection = new ObservableCollection<SongData>();
        public ObservableCollection<SongData> SongCollection
        { get { return _SongCollection; } }
        public class SongData
        {
            public string Preview { get; set; }
            public string Artist { get; set; }
            public string Song { get; set; }
            public string Album { get; set; }
            public string Tuning { get; set; }
            public string Updated { get; set; }
            public string User { get; set; }
            public string NewAvailable { get; set; }
            public string Arrangements { get; set; }
            public string Author { get; set; }
        }
        public MainWindow()
        {
            Loaded += WindowLoaded;
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }
        private void PopulateList()
        {
            List<string> filesList = new List<string>(FilesList(rocksmith2014Path));
            foreach (string file in filesList)
            {
                var browser = new PsarcBrowser(file);
                var songList = browser.GetSongList();
                foreach (var song in songList)
                {
                    var arrangements = "";
                    foreach (string arrangement in song.Arrangements)
                    {
                        arrangements += "," + arrangement;
                    }
                    arrangements = arrangements.Remove(0, 1);
                    _SongCollection.Add(new SongData
                                        {
                                            Song = song.Title,
                                            Artist = song.Artist,
                                            Album = song.Album,
                                            Updated = song.Updated,
                                            Tuning = TuningToName(song.Tuning),
                                            Arrangements = arrangements,
                                            Author = song.Author,
                                            NewAvailable = ""
                                        });
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // List<string> filesList = new List<string>(FilesList(rocksmith2014Path));
            //  ProcessFiles(filesList);
            PopulateList();
        }
        private List<string> FilesList(string path)
        {
            List<string> files = new List<string>(Directory.GetFiles(path, "*_p.psarc", SearchOption.AllDirectories));
            return files;
        }
        private string TuningToName(string tuning)
        {
            switch (tuning)
            {
                case "000000":
                    return "E Standard";
                case "-100000":
                    return "E Drop D";
                case "-2-2-2-2-2-2":
                    return "D Standard";
                case "-3-1-1-1-1-1":
                    return "Eb Drop Db";
                case "-3-3-3-3-3-3":
                    return "C# Standard";
                case "-4-4-4-4-4-4":
                    return "C Standard";
                case "-4-2-2-2-2-2":
                    return "D Drop C";
                case "-5-5-5-5-5-5":
                    return "B Standard";
                case "-5-3-3-3-3-3":
                    return "C# Drop B";
                case "-7-7-7-7-7-7":
                    return "A Standard";
                case "-7-5-5-5-5-5":
                    return "B Drop A";
                default:
                    return "Other";
            }
        }
        private void LogError(string fileName, Exception ex)
        {

        }
        private void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            rocksmith2014Path = txtLocation.Text;
        }
        private void btnTestLocation_Click(object sender, RoutedEventArgs e)
        {
            workingFolder = txtTestLocation.Text;
        }
        private void btnBackupLocation_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }
            foreach (ListViewItem item in listSongs.Items)
            {
                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
         
        }
    }
}
