﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using DataGridViewTools;
using CFSM.Utils;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    [Serializable]
    public class RADataGridViewSettings
    {
        public const string gridViewSettingsVersion = "1.0";
        [XmlIgnore]
        public string LoadedVersion { get; private set; }

        [XmlAttribute]
        public string GridViewSettingsVersion
        {
            get { return gridViewSettingsVersion; }
            set { this.LoadedVersion = value; }
        }

        List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
        public List<ColumnOrderItem> ColumnOrder
        {
            get { return columnOrder as List<ColumnOrderItem>; }
            set { columnOrder = value; }
        }
    }

    [Serializable]
    public class ColumnOrderItem
    {
        public string ColumnName { get; set; }
        public int DisplayIndex { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public int ColumnIndex { get; set; }
    }

    [Serializable]
    public class AppSettings : NotifyPropChangedBase
    {
        string FRSInstalledDir;
        bool FIncludeRS1DLCs;
        bool FEnabledLogBaloon;
        bool FCheckForUpdateOnScan;
        bool FFullScreen;
        int FWindowWidth;
        int FWindowHeight;
        int FWindowTop;
        int FWindowLeft;
        bool FShowLogwindow;
        string FCreator;
        string FRenameTemplate;
        string FSortColumn;
        bool FSortAscending;

        [Browsable(false)]
        public string FirstRun { get; set; }
        [Browsable(false)]
        public string LogFilePath { get; set; }
        public string RSInstalledDir { get { return FRSInstalledDir; } set { SetPropertyField("RSInstalledDir", ref FRSInstalledDir, value); } }
        public bool IncludeRS1DLCs { get { return FIncludeRS1DLCs; } set { SetPropertyField("IncludeRS1DLCs", ref FIncludeRS1DLCs, value); } }
        public bool EnabledLogBaloon { get { return FEnabledLogBaloon; } set { SetPropertyField("EnabledLogBaloon", ref FEnabledLogBaloon, value); } }
        public bool CheckForUpdateOnScan { get { return FCheckForUpdateOnScan; } set { SetPropertyField("CheckForUpdateOnScan", ref FCheckForUpdateOnScan, value); } }

        //[XmlArray("UISettings")] // provides proper xml serialization
        //[XmlArrayItem("UISetting")] // provides proper xml serialization
        //public List<UISetting> UISettings { get; set; }

        [XmlIgnore, Browsable(false)]
        public RADataGridViewSettings ManagerGridSettings { get; set; }
        [Browsable(false)]
        public string RenameTemplate { get { return FRenameTemplate; } set { SetPropertyField("RenameTemplate", ref FRenameTemplate, value); } }

        [Browsable(false)]
        public bool FullScreen { get { return FFullScreen; } set { SetPropertyField("FullScreen", ref FFullScreen, value); } }
        public bool ShowLogWindow { get { return FShowLogwindow; } set { SetPropertyField("ShowLogWindow", ref FShowLogwindow, value); } }
        public string CreatorName { get { return FCreator; } set { SetPropertyField("CreatorName", ref FCreator, value); } }

        public string ThemeName { get { return FThemeName; } set { SetPropertyField("ThemeName", ref FThemeName, value); } }

        [Browsable(false)]
        public int WindowWidth { get { return FWindowWidth; } set { SetPropertyField("WindowWidth", ref FWindowWidth, value); } }

        [Browsable(false)]
        public int WindowHeight { get { return FWindowHeight; } set { SetPropertyField("WindowHeight", ref FWindowHeight, value); } }

        [Browsable(false)]
        public int WindowTop { get { return FWindowTop; } set { SetPropertyField("WindowTop", ref FWindowTop, value); } }

        [Browsable(false)]
        public int WindowLeft { get { return FWindowLeft; } set { SetPropertyField("WindowLeft", ref FWindowLeft, value); } }

        [Browsable(false)]
        public string SortColumn { get { return FSortColumn; } set { SetPropertyField("SortColumn", ref FSortColumn, value); } }

        [Browsable(false)]
        public bool SortAscending { get { return FSortAscending; } set { SetPropertyField("SortAscending", ref FSortAscending, value); } }

        [XmlArray("CustomSettings")] // provides proper xml serialization
        [XmlArrayItem("CustomSetting")] // provides proper xml serialization
        public List<CustomSetting> CustomSettings { get; set; }

        public bool MoveToQuarantine { get; set; }

        //property template
        //public type PropName { get { return propName; } set { SetPropertyField("PropName", ref propName, value); } }

        private static AppSettings instance;
        private string FThemeName;

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new AppSettings();
                return instance;
            }
        }

        public void LoadFromFile(string settingsPath, string gridSettingsPath)
        {
            if (!String.IsNullOrEmpty(settingsPath) && File.Exists(settingsPath))
                using (var fs = File.OpenRead(settingsPath))
                    LoadFromStream(fs);

            if (!String.IsNullOrEmpty(gridSettingsPath) && File.Exists(gridSettingsPath))
            {
                using (var fs = File.OpenRead(gridSettingsPath))
                    ManagerGridSettings = fs.DeserializeXml<RADataGridViewSettings>();
            }
        }

        public void LoadFromStream(Stream stream)
        {
            var x = stream.DeserializeXml<AppSettings>();
            var props = GetType().GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

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
            Instance.RSInstalledDir = Extensions.GetSteamDirectory();
            Instance.IncludeRS1DLCs = false;  // changed to false (fewer issues)
            Instance.EnabledLogBaloon = false; // fewer notfication issues
            Instance.ShowLogWindow = Constants.DebugMode;
            Instance.ManagerGridSettings = new RADataGridViewSettings();
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
