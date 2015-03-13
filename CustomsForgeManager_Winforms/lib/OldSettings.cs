using System;
using System.Collections.Generic;

namespace CustomsForgeManager_Winforms.Utilities
{
    [Serializable]
    public class OldSettingsData
    {
        public string LogFilePath { get; set; }
        public string RSInstalledDir { get; set; }
        public bool RescanOnStartup { get; set; }
        public bool IncludeRS1DLCs { get; set; }
        public RADataGridViewSettings ManagerGridSettings { get; set; }
    }

    [Serializable]
    public class OldSettings
    {
        private OldSettingsData _settingsData;
        
        public RADataGridViewSettings ManagerGridSettings
        {
            get
            {
                return _settingsData.ManagerGridSettings;
            }
            set
            {
                if (_settingsData == null) _settingsData = new OldSettingsData();
                _settingsData.ManagerGridSettings = value;
            }
        }

        public string LogFilePath
        {
            get { return _settingsData.LogFilePath; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new OldSettingsData();
                _settingsData.LogFilePath = value;
            }
        }

        public string RSInstalledDir
        {
            get { return _settingsData.RSInstalledDir; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new OldSettingsData();
                _settingsData.RSInstalledDir = value;
            }
        }

        public bool RescanOnStartup
        {
            get { return _settingsData.RescanOnStartup; }
            set 
            {
                if (_settingsData == null)
                    _settingsData = new OldSettingsData();
                _settingsData.RescanOnStartup = value;
            }
        }

        public bool IncludeRS1DLCs
        {
            get { return _settingsData.IncludeRS1DLCs; }
            set
            {
                if (_settingsData == null)
                    _settingsData = new OldSettingsData();
                _settingsData.IncludeRS1DLCs = value;
            }
        }

        /// <summary>
        /// Initialise settings with default values
        /// </summary>
        public OldSettings()
        {
            _settingsData = new OldSettingsData();
            _settingsData.LogFilePath = Constants.DefaultLogName;
        }
    }
}
