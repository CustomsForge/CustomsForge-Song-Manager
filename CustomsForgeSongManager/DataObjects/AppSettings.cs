using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using CustomsForgeSongManager.LocalTools;
using GenTools;
using DataGridViewTools;

namespace CustomsForgeSongManager.DataObjects
{
    [Serializable]
    public class AppSettings : NotifyPropChangedBase
    {
        private string _rsInstalledDir = String.Empty;
        private string _rsProfileDir = String.Empty;
        private bool _includeRS1CompSongs;
        private bool _includeRS2BaseSongs;
        private bool _includeCustomPacks;
        private bool _includeAnalyzerData;
        private bool _enableAutoUpdate = false;
        private bool _enableNotifications = false;
        private bool _validateD3D = false;
        private bool _macMode;
        private bool _cleanOnClosing;
        private bool _checkForUpdateOnScan;
        private bool _fullScreen;
        private int _windowWidth;
        private int _windowHeight;
        private int _windowTop;
        private int _windowLeft;
        private bool _showLogWindow;
        private string _charterName = String.Empty;
        private string _renameTemplate = String.Empty;
        private string _searchString = String.Empty;
        private string _filterString = String.Empty;
        private string _sortColumn = "Artist"; // set default sort column (retains selection)
        private bool _sortAscending = true;
        private bool _showSetlistSongs;
        private string _downloadsDir;
        private DateTime _lastODLCCheckDate;
        private RepairOptions _repairOptions;

        [Browsable(false)]
        public string LogFilePath { get; set; }

        public string RSInstalledDir
        {
            get { return _rsInstalledDir; }
            set { SetPropertyField("RSInstalledDir", ref _rsInstalledDir, value); }
        }

        public string RSProfileDir
        {
            get { return _rsProfileDir; }
            set { SetPropertyField("RSProfileDir", ref _rsProfileDir, value); }
        }

        public bool IncludeRS1CompSongs
        {
            get { return _includeRS1CompSongs; }
            set { SetPropertyField("IncludeRS1CompSongs", ref _includeRS1CompSongs, value); }
        }

        public bool IncludeRS2BaseSongs
        {
            get { return _includeRS2BaseSongs; }
            set { SetPropertyField("IncludeRS2BaseSongs", ref _includeRS2BaseSongs, value); }
        }

        public bool IncludeCustomPacks
        {
            get { return _includeCustomPacks; }
            set { SetPropertyField("IncludeCustomPacks", ref _includeCustomPacks, value); }
        }

        public bool IncludeAnalyzerData
        {
            get { return _includeAnalyzerData; }
            set { SetPropertyField("IncludeAnalyzerData", ref _includeAnalyzerData, value); }
        }

        public bool EnableAutoUpdate
        {
            get { return _enableAutoUpdate; }
            set { SetPropertyField("EnableAutoUpdate", ref _enableAutoUpdate, value); }
        }

        public bool EnableNotifications
        {
            get { return _enableNotifications; }
            set { SetPropertyField("EnableNotifications", ref _enableNotifications, value); }
        }

        public bool ValidateD3D
        {
            get { return _validateD3D; }
            set { SetPropertyField("ValidateD3D", ref _validateD3D, value); }
        }

        public bool MacMode
        {
            get { return _macMode; }
            set { SetPropertyField("MacMode", ref _macMode, value); }
        }

        public bool CleanOnClosing
        {
            get { return _cleanOnClosing; }
            set { SetPropertyField("CleanOnClosing", ref _cleanOnClosing, value); }
        }

        public bool CheckForUpdateOnScan
        {
            get { return _checkForUpdateOnScan; }
            set { SetPropertyField("CheckForUpdateOnScan", ref _checkForUpdateOnScan, value); }
        }

        public DateTime LastODLCCheckDate
        {
            get { return _lastODLCCheckDate; }
            set { SetPropertyField("LastODLCCheckDate", ref _lastODLCCheckDate, value); }
        }

        public string DownloadsDir
        {
            get { return _downloadsDir; }
            set { SetPropertyField("DownloadsDir", ref _downloadsDir, value); }
        }

        //[XmlArray("UISettings")] // provides proper xml serialization
        //[XmlArrayItem("UISetting")] // provides proper xml serialization
        //public List<UISetting> UISettings { get; set; }

        [Browsable(false)]
        public string RenameTemplate
        {
            get { return _renameTemplate; }
            set { SetPropertyField("RenameTemplate", ref _renameTemplate, value); }
        }

        [Browsable(false)]
        public bool FullScreen
        {
            get { return _fullScreen; }
            set { SetPropertyField("FullScreen", ref _fullScreen, value); }
        }

        public bool ShowLogWindow
        {
            get { return _showLogWindow; }
            set { SetPropertyField("ShowLogWindow", ref _showLogWindow, value); }
        }

        public string CharterName
        {
            get { return _charterName; }
            set { SetPropertyField("CreatorName", ref _charterName, value); }
        }

        public string ThemeName
        {
            get { return _themeName; }
            set { SetPropertyField("ThemeName", ref _themeName, value); }
        }

        [Browsable(false)]
        public int WindowWidth
        {
            get { return _windowWidth; }
            set { SetPropertyField("WindowWidth", ref _windowWidth, value); }
        }

        [Browsable(false)]
        public int WindowHeight
        {
            get { return _windowHeight; }
            set { SetPropertyField("WindowHeight", ref _windowHeight, value); }
        }

        [Browsable(false)]
        public int WindowTop
        {
            get { return _windowTop; }
            set { SetPropertyField("WindowTop", ref _windowTop, value); }
        }

        [Browsable(false)]
        public int WindowLeft
        {
            get { return _windowLeft; }
            set { SetPropertyField("WindowLeft", ref _windowLeft, value); }
        }

        [Browsable(false)]
        public string SearchString
        {
            get { return _searchString; }
            set { SetPropertyField("SearchString", ref _searchString, value); }
        }

        [Browsable(false)]
        public string FilterString
        {
            get { return _filterString; }
            set { SetPropertyField("FilterString", ref _filterString, value); }
        }

        [Browsable(false)]
        public string SortColumn
        {
            get { return _sortColumn; }
            set { SetPropertyField("SortColumn", ref _sortColumn, value); }
        }

        [Browsable(false)]
        public bool SortAscending
        {
            get { return _sortAscending; }
            set { SetPropertyField("SortAscending", ref _sortAscending, value); }
        }

        public bool ShowSetlistSongs
        {
            get { return _showSetlistSongs; }
            set { SetPropertyField("ShowSetlistSongs", ref _showSetlistSongs, value); }
        }

        [XmlArray("CustomSettings")] // provides proper xml serialization
        [XmlArrayItem("CustomSetting")] // provides proper xml serialization
        public List<CustomSetting> CustomSettings { get; set; }

        public RepairOptions RepairOptions
        {
            get { return _repairOptions ?? (_repairOptions = new RepairOptions()); }
            set { _repairOptions = value; }
        }

        public bool MoveToQuarantine { get; set; }

        //property template
        //public type PropName { get { return propName; } set { SetPropertyField("PropName", ref propName, value); } }

        private string _themeName;

        private static AppSettings _instance;
        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public void LoadFromFile(string settingsPath, bool verbose = false)
        {
            if (!String.IsNullOrEmpty(settingsPath) && File.Exists(settingsPath))
            {
                using (var fs = File.OpenRead(settingsPath))
                    LoadSettingsFromStream(fs);

                if (verbose)
                    Globals.Log("Loaded File: " + Path.GetFileName(Constants.AppSettingsPath));
            }

            if (String.IsNullOrEmpty(Globals.DgvCurrent.Name))
                return;

            if (File.Exists(Constants.GridSettingsPath))
            {
                Globals.Log("Loaded File: " + Path.GetFileName(Constants.GridSettingsPath));
                RAExtensions.ManagerGridSettings = SerialExtensions.LoadFromFile<RADataGridViewSettings>(Constants.GridSettingsPath);
            }
            else
            {
                Globals.Log("<WARNING> Did not find file: " + Path.GetFileName(Constants.GridSettingsPath));
                RAExtensions.ManagerGridSettings = null; // reset
            }
        }

        public void LoadSettingsFromStream(Stream stream)
        {
            var x = stream.DeserializeXml<AppSettings>();
            var props = GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            var emptyObjParams = new object[] { };

            foreach (var p in props)
                if (p.CanRead && p.CanWrite)
                {
                    var ignore = p.GetCustomAttributes(typeof(XmlIgnoreAttribute), true).Length > 0;
                    if (!ignore)
                        p.SetValue(this, p.GetValue(x, emptyObjParams), emptyObjParams);
                }
        }

        public void Reset()
        {
            Instance.MoveToQuarantine = false;
            Instance.LogFilePath = Constants.LogFilePath;
            Instance.RSInstalledDir = LocalExtensions.GetSteamDirectory();
            Instance.RSProfileDir = String.Empty;
            Instance.IncludeRS1CompSongs = false; // changed to false (fewer issues)
            Instance.IncludeRS2BaseSongs = false;
            Instance.IncludeCustomPacks = false;
            Instance.IncludeAnalyzerData = false;
            Instance.EnableAutoUpdate = false;
            Instance.EnableNotifications = false; // fewer notfication issues
            Instance.ValidateD3D = true;
            Instance.CleanOnClosing = false;
            Instance.ShowLogWindow = Constants.DebugMode;
            RAExtensions.ManagerGridSettings = new RADataGridViewSettings();
            Instance.RepairOptions = new RepairOptions();
        }

        /// Initialise settings with default values
        /// </summary>
        internal AppSettings()
        {
            LogFilePath = Constants.LogFilePath;
        }

        [XmlRoot("CustomSetting")]
        public class CustomSetting
        {
            public string ControlName { get; set; }
            public string ControlKey { get; set; }
            public string ControlValue { get; set; }
        }
    }
}