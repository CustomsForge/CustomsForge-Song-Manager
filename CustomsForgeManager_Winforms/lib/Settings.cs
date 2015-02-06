using System;

namespace CustomsForgeManager_Winforms.Utilities
{
    [Serializable]
    public class SettingsData
    {
        public string LogFilePath { get; set; }
        public string RSInstalledDir { get; set; }

    }

    [Serializable]
    public class Settings
    {
        private SettingsData _settingsData;
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
