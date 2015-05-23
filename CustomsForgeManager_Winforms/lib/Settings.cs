using System;
using System.Collections.Generic;

namespace CustomsForgeManager_Winforms.Utilities
{
    [Serializable]
    public class SettingsData
    {
        public string LogFilePath { get; set; }
        public string RSInstalledDir { get; set; }
        public bool RescanOnStartup { get; set; }
        public bool IncludeRS1DLCs { get; set; }
        public bool EnableLogBaloon { get; set; }
        public RADataGridViewSettings ManagerGridSettings { get; set; }
        public string RenameTemplate { get; set; }
    }

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
        public int DisplayIndex { get; set; }
        public int Width { get; set; }
        public bool Visible { get; set; }
        public int ColumnIndex { get; set; }
    }

    [Serializable]
    public class Settings
    {
        private SettingsData _settingsData;
        
        //Dictionary<string, List<ColumnOrderItem>> columnOrder = new Dictionary<string, List<ColumnOrderItem>>();
        //public Dictionary<string, List<ColumnOrderItem>> ColumnOrder
        //{
        //    get { return columnOrder as Dictionary<string, List<ColumnOrderItem>>; }
        //    set { columnOrder = value; }
        //}

        public RADataGridViewSettings ManagerGridSettings
        {
            get
            {
                return _settingsData.ManagerGridSettings;
            }
            set
            {
                if (_settingsData == null) _settingsData = new SettingsData();
                _settingsData.ManagerGridSettings = value;
            }
        }

        public string LogFilePath
        {
            get { return _settingsData.LogFilePath; }
            set
            {
                if (_settingsData == null) 
                    _settingsData = new SettingsData();
                _settingsData.LogFilePath = value;
            }
        }

        public string RSInstalledDir
        {
            get { return _settingsData.RSInstalledDir; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new SettingsData();
                _settingsData.RSInstalledDir = value;
            }
        }

        public bool RescanOnStartup
        {
            get { return _settingsData.RescanOnStartup; }
            set 
            {
                if (_settingsData == null)
                    _settingsData = new SettingsData();
                _settingsData.RescanOnStartup = value;
            }
        }

        public bool IncludeRS1DLCs
        {
            get { return _settingsData.IncludeRS1DLCs; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new SettingsData();
                _settingsData.IncludeRS1DLCs = value;
            }
        }

        public bool EnabledLogBaloon
        {
            get { return _settingsData.EnableLogBaloon; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new SettingsData();
                _settingsData.EnableLogBaloon = value;
            }
        }

        /// <summary>
        /// Initialise settings with default values
        /// </summary>
        public Settings()
        {
            _settingsData = new SettingsData();
            _settingsData.LogFilePath = Constants.DefaultLogName;
        }
    }
}
