using System;
using System.IO;
using RocksmithToolkitLib;
using RocksmithToolkitLib.DLCPackage;
using RocksmithToolkitLib.Extensions;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public class PsarcPackage : IDisposable
    {
        private string packageDir;
 
        public DLCPackageData ReadPackage(string inputPath)
        {
            // UNPACK
            packageDir = Packer.Unpack(inputPath, Path.GetTempPath(), true, true, false);
           
            // REORGANIZE
            packageDir = DLCPackageData.DoLikeProject(packageDir);

            // LOAD DATA
            DLCPackageData info = null;
            var packagePlatform = inputPath.GetPlatform();
            info = DLCPackageData.LoadFromFolder(packageDir, packagePlatform, packagePlatform);

            return info;
        }

        public void WritePackage(string outputPath, DLCPackageData packageData)
        {
            DLCPackageCreator.Generate(outputPath, packageData, new Platform(GamePlatform.Pc, GameVersion.RS2014));
        }

        public void Dispose()
        {
            // DirectoryExtension.SafeDelete(packageDir);            
        }
    }
}
