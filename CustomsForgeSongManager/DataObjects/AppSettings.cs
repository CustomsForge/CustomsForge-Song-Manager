﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using CFSM.GenTools;
using CustomsForgeSongManager.ClassMethods;
using DataGridViewTools;

namespace CustomsForgeSongManager.DataObjects
{
    [Serializable]
    public class AppSettings : NotifyPropChangedBase
    {
        private string _rsInstalledDir;
        private string _rsProfileDir;
        private bool _includeRs1DlCs;
        private bool _enabledLogBaloon;
        private bool _cleanOnClosing;
        private bool _checkForUpdateOnScan;
        private bool _fullScreen;
        private int _windowWidth;
        private int _windowHeight;
        private int _windowTop;
        private int _windowLeft;
        private bool _showLogWindow;
        private string _charterName;
        private string _renameTemplate;
        private string _sortColumn;
        private bool _sortAscending;
        private bool _showSetlistSongs;

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

        public bool IncludeRS1DLCs
        {
            get { return _includeRs1DlCs; }
            set { SetPropertyField("IncludeRS1DLCs", ref _includeRs1DlCs, value); }
        }

        public bool EnabledLogBaloon
        {
            get { return _enabledLogBaloon; }
            set { SetPropertyField("EnabledLogBaloon", ref _enabledLogBaloon, value); }
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

        public bool MoveToQuarantine { get; set; }

        //property template
        //public type PropName { get { return propName; } set { SetPropertyField("PropName", ref propName, value); } }

        private static AppSettings _instance;
        private string _themeName;

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AppSettings();
                return _instance;
            }
        }

        public void LoadFromFile(string settingsPath, DataGridView dgvCurrent)
        {
            if (!String.IsNullOrEmpty(settingsPath) && File.Exists(settingsPath))
                using (var fs = File.OpenRead(settingsPath))
                    LoadFromStream(fs);

            if (dgvCurrent != null)
                if (File.Exists(Constants.GridSettingsPath))
                    RAExtensions.ManagerGridSettings = SerialExtensions.LoadFromFile<RADataGridViewSettings>(Constants.GridSettingsPath);
        }

        public void LoadFromStream(Stream stream)
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
            Globals.Log("Loaded settings file ...");
        }

        public void Reset()
        {
            Instance.MoveToQuarantine = false;
            Instance.LogFilePath = Constants.LogFilePath;
            Instance.RSInstalledDir = LocalExtensions.GetSteamDirectory();
            Instance.RSProfileDir = String.Empty;
            Instance.IncludeRS1DLCs = false; // changed to false (fewer issues)
            Instance.EnabledLogBaloon = false; // fewer notfication issues
            Instance.CleanOnClosing = false;
            Instance.ShowLogWindow = Constants.DebugMode;
            RAExtensions.ManagerGridSettings = new RADataGridViewSettings();
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