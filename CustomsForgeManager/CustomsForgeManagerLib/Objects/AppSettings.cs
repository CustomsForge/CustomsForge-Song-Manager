﻿using System;
using System.Collections.Generic;

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
    public class AppSettings  : NotifyPropChangedBase
    {
        string FLogFilePath;
        string FRSInstalledDir;
        bool FRescanOnStartup;
        bool FRescanNewSongs;
        bool FIncludeRS1DLCs;
        bool FEnabledLogBaloon;
        bool FCheckForUpdateOnScan;
        string FRenameTemplate;
        bool FFullScreen;


        public string LogFilePath { get { return FLogFilePath; } set { SetPropertyField("LogFilePath", ref FLogFilePath, value); } }
        public string RSInstalledDir { get { return FRSInstalledDir; } set { SetPropertyField("RSInstalledDir", ref FRSInstalledDir, value); } }
        public bool RescanOnStartup { get { return FRescanOnStartup; } set { SetPropertyField("RescanOnStartup", ref FRescanOnStartup, value); } }
        public bool RescanNewSongs { get { return FRescanNewSongs; } set { SetPropertyField("RescanNewSongs", ref FRescanNewSongs, value); } }
        public bool IncludeRS1DLCs { get { return FIncludeRS1DLCs; } set { SetPropertyField("IncludeRS1DLCs", ref FIncludeRS1DLCs, value); } }
        public bool EnabledLogBaloon { get { return FEnabledLogBaloon; } set { SetPropertyField("EnabledLogBaloon", ref FEnabledLogBaloon, value); } }
        public bool CheckForUpdateOnScan { get { return FCheckForUpdateOnScan; } set { SetPropertyField("CheckForUpdateOnScan", ref FCheckForUpdateOnScan, value); } }
        public RADataGridViewSettings ManagerGridSettings { get; set; }
        // TODO: need to impliment saving/loading this
        public string RenameTemplate { get { return FRenameTemplate; } set { SetPropertyField("RenameTemplate", ref FRenameTemplate, value); } }

        public bool FullScreen { get { return FFullScreen; } set { SetPropertyField("FullScreen", ref FFullScreen, value); } }
      
        /// <summary>
        /// Initialise settings with default values
        /// </summary>
        public AppSettings()
        {
          LogFilePath = Constants.LogFilePath;
        }
    }
}