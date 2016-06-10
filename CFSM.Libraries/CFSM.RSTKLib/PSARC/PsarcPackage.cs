﻿using System;
using System.IO;
using System.Linq;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;

namespace CFSM.RSTKLib.PSARC
{
    public class PsarcPackage : IDisposable
    {
        private string packageDir;
        private bool _deleteOnClose;

        public PsarcPackage(bool deleteOnClose = false)
        {
            _deleteOnClose = deleteOnClose;
        }

        public DLCPackageData ReadPackage(string inputPath)
        {
            // UNPACK
            packageDir = Packer.Unpack(inputPath, Path.GetTempPath(), true);

            // REORGANIZE
            packageDir = DLCPackageData.DoLikeProject(packageDir);

            // LOAD DATA
            DLCPackageData info = new DLCPackageData();
            var packagePlatform = inputPath.GetPlatform();
            info = DLCPackageData.LoadFromFolder(packageDir, packagePlatform, packagePlatform);
            var foundShowlights = Directory.EnumerateFiles(packageDir, "*_showlights.xml", SearchOption.AllDirectories).Any();

            // toolkit will generate showlights if none was found
            if (!foundShowlights)
                info.Showlights = true;

            return info;
        }

        public void WritePackage(string outputPath, DLCPackageData packageData)
        {
            DLCPackageCreator.Generate(outputPath, packageData, new Platform(GamePlatform.Pc, GameVersion.RS2014));
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (disposing)
                if (_deleteOnClose)
                    DirectoryExtension.SafeDelete(packageDir);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}