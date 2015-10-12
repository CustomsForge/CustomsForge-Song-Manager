﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CustomsForgeManager.CustomsForgeManagerLib.CustomControls;
using CustomsForgeManager.UControls;
using DLogNet;
using DataGridViewTools;
using RocksmithToolkitLib;


namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    static class Globals
    {
        internal enum Tristate : byte
        {
            False = 0,
            True = 1,
            Cancelled = 2
        }

        // application data variables
        private static About _about;
        private static List<string> _fileCollection;
        private static DataGridView _dgvSongs;
        private static Duplicates _duplicates;
        private static Dictionary<string, SongData> _outdatedSongList;
        private static Renamer _renamer;
        private static SetlistManager _SetlistManager;
        private static Settings _settings;
        private static BindingList<SongData> _songCollection;
        private static SongManager _songManager;
        private static Utilities _utilities;
 
        public static About About
        {
            get { return _about ?? (_about = new About()); }
            set { _about = value; }
        }

        public static List<string> FileCollection
        {
            get { return _fileCollection ?? (_fileCollection = new List<string>()); }
            set { _fileCollection = value; }
        }

        public static DataGridView DgvSongs
        {
            get { return _dgvSongs ?? (_dgvSongs = new DataGridView()); }
            set { _dgvSongs = value; }
        }

        public static Duplicates Duplicates
        {
            get { return _duplicates ?? (_duplicates = new Duplicates()); }
            set { _duplicates = value; }
        }

        public static List<TuningDefinition> TuningXml { get; set; }
        public static DLogger MyLog { get; set; }
        public static AppSettings MySettings { get; set; }
        public static NotifyIcon Notifier { get; set; }

        public static Dictionary<string, SongData> OutdatedSongList
        {
            get { return _outdatedSongList ?? (_outdatedSongList = new Dictionary<string, SongData>()); }
            set { _outdatedSongList = value; }
        }

        public static Renamer Renamer
        {
            get { return _renamer ?? (_renamer = new Renamer()); }
            set { _renamer = value; }
        }

        public static bool RescanAbout { get; set; }
        public static bool RescanDuplicates { get; set; }
        public static bool RescanRenamer { get; set; }
        public static bool RescanSetlistManager { get; set; }
        public static bool RescanSettings { get; set; }
        public static bool RescanSongManager { get; set; }
        public static bool RescanUtilities { get; set; }
        public static bool ReloadAbout { get; set; }
        public static bool ReloadDuplicates { get; set; }
        public static bool ReloadRenamer { get; set; }
        public static bool ReloadSetlistManager { get; set; }
        public static bool ReloadSettings { get; set; }
        public static bool ReloadSongManager { get; set; }
        public static bool ReloadUtilities { get; set; }

        public static SetlistManager SetlistManager
        {
            get { return _SetlistManager ?? (_SetlistManager = new SetlistManager()); }
            set { _SetlistManager = value; }
        }

        public static Settings Settings
        {
            get { return _settings ?? (_settings = new Settings()); }
            set { _settings = value; }
        }

        public static BindingList<SongData> SongCollection
        {
            get { return _songCollection ?? (_songCollection = new BindingList<SongData>()); }
            set { _songCollection = value; }
        }

        public static SongManager SongManager
        {
            get { return _songManager ?? (_songManager = new SongManager()); }
            set { _songManager = value; }
        }

        public static ToolStripStatusLabel TsLabel_Cancel { get; set; }
        public static ToolStripStatusLabel TsLabel_DisabledCounter { get; set; }
        public static ToolStripStatusLabel TsLabel_MainMsg { get; set; }
        public static ToolStripStatusLabel TsLabel_StatusMsg { get; set; }
        public static ToolStripProgressBar TsProgressBar_Main { get; set; }

        public static Utilities Utilities
        {
            get { return _utilities ?? (_utilities = new Utilities()); }
            set { _utilities = value; }
        }
        
        public static Tristate WorkerFinished { get; set; } // True = 0, False = 1, Cancelled = 2

        public static void Log(string message)
        {
             MyLog.Write(message);
        }

        public static void ResetToolStripGlobals()
        {
            TsProgressBar_Main.Value = 0;
            TsLabel_MainMsg.Visible = false;
            TsLabel_StatusMsg.Visible = false;
            TsLabel_Cancel.Visible = false;
            TsLabel_Cancel.Text = "Cancel";
            TsLabel_DisabledCounter.Visible = false;
        }

    }
}