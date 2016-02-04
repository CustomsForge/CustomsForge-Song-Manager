using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using RocksmithToolkitLib;
using RocksmithToolkitLib.XmlRepository;

namespace CFSM.GenTools.Objects
{
    public static class GlobalsTools
    {
        private static List<TuningDefinition> _tuningXml;

        public static List<TuningDefinition> TuningXml
        {
            // auto load _tuningXml if null
            get { return _tuningXml ?? (_tuningXml = TuningDefinitionRepository.LoadTuningDefinitions(GameVersion.RS2014)); }
            set { _tuningXml = value; }
        }

        private static DataGridView _dgvCurrent;
        public static DataGridView DgvCurrent
        {
            get { return _dgvCurrent ?? (_dgvCurrent = new DataGridView()); }
            set { _dgvCurrent = value; }
        }

        // for sharing log messages with caller
        public static string SharedLog { get; private set; }
        public static void Log(string message)
        {
            SharedLog = message;
        }


        private static string _charterName;
        public static string CharterName
        {
            get { return _charterName ?? (_charterName = "CFSM Tools"); }
            set { _charterName = value; }
        }

        private static string _appVersion;

        public static string AppVersion
        {
            get { return _appVersion ?? (_appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()); }
            set { _appVersion = value; }
        }

        private static string _rs1Comp;

        public static string Rs1Comp
        {
            get { return _rs1Comp ?? (_rs1Comp = "rs1compatibility"); }
            set { _rs1Comp = value; }
        }

        private static string _songPack;

        public static string SongPack
        {
            get { return _songPack ?? (_songPack = "songpack"); }
            set { _songPack = value; }
        }

        private static string _workDirectory;

        public static string WorkDirectory
        {
            get
            {
                if (_workDirectory == null)
                    throw new InvalidDataException("CFSM Work Directory was not specified by the caller.");

                return _workDirectory;
            }
            set { _workDirectory = value; }
        }

        private static string _rsProfileDir;

        public static string RsProfileDir
        {
            get
            {
                if (_rsProfileDir == null)
                    throw new InvalidDataException("Rocksmith 2014 Profile Directory was not specified by the caller.");

                return _rsProfileDir;
            }
            set { _rsProfileDir = value; }
        }

        private static string _applicatonName;

        public static string ApplicationName
        {
            get { return _applicatonName ?? (_applicatonName = Assembly.GetExecutingAssembly().GetName().FullName); }
            set { _applicatonName = value; }
        }
    }
}