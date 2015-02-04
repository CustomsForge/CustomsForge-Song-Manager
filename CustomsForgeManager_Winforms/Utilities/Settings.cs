using System;

namespace CustomsForgeManager_Winforms.Utilities
{
    [Serializable]
    public class SettingsData
    {
        public string LogFilePath { get; set; }
        public string RSInstalledDir { get; set; }

    }

    public class Settings
    {
        private SettingsData _settingsData;
        private string _path;

        public string Path { get { return _path; } }

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
            _path = Constants.DefaultWorkingDirectory;
            _settingsData = new SettingsData();
            _settingsData.LogFilePath = Constants.DefaultLogName;
        }
    }



}
