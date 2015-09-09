using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Ionic.Zip;
using zlib;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    internal static class DotNetZip
    {
        public static void ZipFile(string inFile, string outFile)
        {
            string[] fileNames = new string[1] { inFile };
            ZipFiles(fileNames, outFile);
        }

        public static void ZipDirectory(string inDirectory, string outFile, bool includeSubDirs = false)
        {
            string[] fileNames = Directory.GetFiles(inDirectory, "*", includeSubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            ZipFiles(fileNames, outFile);
        }

        public static void ZipFiles(string[] inFiles, string outFile)
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (var inFile in inFiles)
                    zip.AddFile(inFile);

                zip.Save(outFile);
            }
        }


    }
}

