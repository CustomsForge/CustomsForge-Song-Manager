﻿using System;
using System.Diagnostics;
using System.IO;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using RocksmithToolkitLib.Extensions;

namespace CustomsForgeSongManager.LocalTools
{
    class VersionInfo
    {
        /// <summary>
        /// Creates the VersionInfo.txt file used by InnoSetup 
        /// </summary>
        public static void CreateVersionInfo()
        {
            // return if not IDE Mode
            if (!GeneralExtension.IsInDesignMode)
                return;

            const string relNotesFile = "ReleaseNotes.txt";
            const string verInfoFile = "VersionInfo.txt";

            var projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var verInfoPath = Path.Combine(projectDir, verInfoFile);
            var relNotesPath = Path.Combine(projectDir, relNotesFile);

            if (!Directory.Exists(Path.GetDirectoryName(verInfoPath)))
                throw new Exception("<ERROR> Could not find directory: " + Path.GetDirectoryName(verInfoPath));

            if (!File.Exists(relNotesPath))
                throw new Exception("<ERROR> Could not find file: " + relNotesPath);

            var txt = GenExtensions.GetFullAppVersion();
            Globals.Log("<DEV ONLY> Current CFSM Version: " + txt);
            File.WriteAllText(verInfoPath, txt);
            Globals.Log("<DEV ONLY> CreateVersionInfo was sucessful: " + verInfoPath);
        }
    }
}

