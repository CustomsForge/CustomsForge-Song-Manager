using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    [Serializable]
    public class RADataGridViewSettings
    {
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
        bool FShowLogwindow;
        string FCreator;

        public string LogFilePath { get; set; }
        public string RSInstalledDir { get { return FRSInstalledDir; } set { SetPropertyField("RSInstalledDir", ref FRSInstalledDir, value); } }
        public bool IncludeRS1DLCs { get { return FIncludeRS1DLCs; } set { SetPropertyField("IncludeRS1DLCs", ref FIncludeRS1DLCs, value); } }
        public bool EnabledLogBaloon { get { return FEnabledLogBaloon; } set { SetPropertyField("EnabledLogBaloon", ref FEnabledLogBaloon, value); } }
        public bool CheckForUpdateOnScan { get { return FCheckForUpdateOnScan; } set { SetPropertyField("CheckForUpdateOnScan", ref FCheckForUpdateOnScan, value); } }
        public RADataGridViewSettings ManagerGridSettings { get; set; }
        // TODO: need to impliment saving/loading this
        public string RenameTemplate { get; set; }
        public bool FullScreen { get { return FFullScreen; } set { SetPropertyField("FullScreen", ref FFullScreen, value); } }
        public bool ShowLogWindow { get { return FShowLogwindow; } set { SetPropertyField("ShowLogWindow", ref FShowLogwindow, value); } }

        public string CreatorName { get { return FCreator; } set { SetPropertyField("CreatorName", ref FCreator, value); } }

        //property template
        //public type PropName { get { return propName; } set { SetPropertyField("PropName", ref propName, value); } }

        private static AppSettings instance;

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new AppSettings();
                return instance;
            }
        }

        public void LoadFromFile(string Filename)
        {
            using (var fs = File.OpenRead(Filename))
                LoadFromStream(fs);
        }

        public void LoadFromStream(Stream stream)
        {
            var x = stream.DeserializeXml<AppSettings>();
            var props = GetType().GetProperties(
                System.Reflection.BindingFlags.Public |  System.Reflection.BindingFlags.Instance);

            var emptyObjParams = new object[] { };

            foreach (var p in props)
                if (p.CanRead && p.CanWrite)
                    p.SetValue(this, p.GetValue(x, emptyObjParams), emptyObjParams);

            Globals.Log("Loaded settings file ...");
        }

        public void Reset()
        {
            Instance.LogFilePath = Constants.LogFilePath;
            Instance.RSInstalledDir = Extensions.GetSteamDirectory();
            Instance.IncludeRS1DLCs = false;  // changed to false (fewer issues)
            Instance.EnabledLogBaloon = false; // fewer notfication issues
            Instance.ShowLogWindow = Constants.DebugMode;
        }

        /// Initialise settings with default values
        /// </summary>
        private AppSettings()
        {
            LogFilePath = Constants.LogFilePath;
        }


    }
}
