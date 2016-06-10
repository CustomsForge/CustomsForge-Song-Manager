using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFMPostBuilder
{
    class Program
    {
        static string ftpUsername = "anonymous";
        static string ftpPass = "password";
        static string ftpURL = "\\";


        public static void UploadStream(Stream stream, string Filename)
        {
            Console.WriteLine("Uploading " + Filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpURL + Filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPass);
            request.ContentLength = stream.Length;
            Stream requestStream = request.GetRequestStream();
            stream.CopyTo(requestStream);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload File {0}, status {1}, size {2}", Filename, response.StatusDescription, stream.Length);
            response.Close();
        }


        static void Main(string[] args)
        {

            // dumby CLI data for test
#if DEBUG
            args = new[] { "CREATEVERSIONINFO" }; // , "CONVERT" };
#endif

            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Invalid command line input");
                Console.WriteLine("");
                Console.WriteLine("Usage: CFMPostBuilder.exe UPLOAD 'FTP ADDRESS' 'USERNAME' 'PASSWORD' ");
                Console.WriteLine("       CFMPostBuilder.exe CONVERT");
                Console.ReadLine();
                Environment.Exit(-10);
                return;
            }
            Console.WriteLine(args[0]);

            // common variables are here
            const string relNotesFile = "ReleaseNotes.txt";
            const string verInfoFile = "VersionInfo.txt";

            var appExe = Assembly.GetExecutingAssembly().Location;
            var appPath = Path.GetDirectoryName(appExe);
            var parentPath = Path.GetDirectoryName(appPath);
            var relNotesPath = Path.Combine(parentPath, "CustomsForgeSongManager", relNotesFile);
            var verInfoPath = Path.Combine(appPath, "CFSM.Setup", "Output", verInfoFile);
            var setupFilePath = Path.Combine(appPath, "CFSM.Setup", "Output", "CFSMSetup.exe");


            switch (args[0].ToUpper())
            {
                case "SETINSTALLERTYPE":
                    {
                        if (args.Length < 2 || string.IsNullOrEmpty(args[0]))
                        {
                            Console.WriteLine("Invalid command line input");
                            Console.WriteLine("");
                            Console.WriteLine("SETINSTALLERTYPE needs a second paramerter.");
                            Console.WriteLine("Usage: CFSMPostBuilder.exe SETINSTALLERTYPE 'BETA' ");
                            Environment.Exit(-10);
                        }

                        try
                        {
                            Directory.SetCurrentDirectory(Path.Combine(appPath, "CFSM.Setup"));
                            if (File.Exists("isbeta.iss"))
                            {
                                Console.WriteLine("iss installer version file found. deleting...");
                                File.Delete("isbeta.iss");

                            }
                            if (File.Exists(setupFilePath))
                            {
                                File.Delete(setupFilePath);
                                Console.WriteLine("Old setup deleted");
                            }
                            bool isBeta = args[1].ToUpper() == "BETA";


                            File.WriteAllText("isbeta.iss", isBeta ? "#define BETA" : "#define RELEASE");
                            Console.WriteLine("Installer type set to: " + (isBeta ? "BETA" : "RELEASE"));

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Environment.Exit(-1);
                        }

                        Console.WriteLine("SETINSTALLERTYPE was sucessful");
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    }
                case "CREATEVERSIONINFO":
                    {
                        if (!File.Exists(relNotesPath))
                        {
                            Console.WriteLine("ReleaseNotes not found");
                            Environment.Exit(-5);
                        }

                        if (!File.Exists(setupFilePath))
                        {
                            Console.WriteLine("Setup.exe installer not found");
                            Environment.Exit(-5);
                        }

                        var vi = FileVersionInfo.GetVersionInfo(setupFilePath);
                        var txt = vi.ProductVersion.Trim();

                        Console.WriteLine("Current Application Version: " + txt);

                        // commented out ... old version info may be confusing updater
                        //if (File.Exists(verInfoPath))
                        //    txt += Environment.NewLine + File.ReadAllText(verInfoPath);

                        File.WriteAllText(verInfoPath, txt);

                        Console.WriteLine("CREATEVERSIONINFO was sucessful");
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                        break;
                    }

                case "UPLOAD":
                    {
                        if (args.Length < 4 || string.IsNullOrEmpty(args[0]))
                        {
                            Console.WriteLine("Invalid command line input");
                            Console.WriteLine("");
                            Console.WriteLine("UPLOAD requires an ftp address,username and password.");
                            Console.WriteLine("Usage: CFMPostBuilder.exe UPLOAD 'FTP ADDRESS' 'USERNAME' 'PASSWORD' ");
                            Environment.Exit(-10);
                        }

                        if (!File.Exists(relNotesPath))
                        {
                            Console.WriteLine("ReleaseNotes not found");
                            Environment.Exit(-5);
                        }

                        if (args.Count() > 1)
                            ftpURL = "ftp://" + args[1];
                        if (args.Count() > 2)
                            ftpUsername = args[2];
                        if (args.Count() > 3)
                            ftpPass = args[3];

                        Console.WriteLine("URL:" + ftpURL);
                        Console.WriteLine("USER:" + ftpUsername);
                        Console.WriteLine("PASS:" + ftpPass);

                        if (MessageBox.Show("Upload the files?", "Question", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            FileVersionInfo vi = FileVersionInfo.GetVersionInfo(setupFilePath);


                            using (var fs = File.OpenRead(setupFilePath))
                            {
                                UploadStream(fs, "CFSMSetup.exe");
                            }


                            Console.WriteLine(File.Exists(relNotesFile));
                            String txt = vi.ProductVersion.Trim() + Environment.NewLine;
                            txt += File.ReadAllText(relNotesFile);

                            MemoryStream ms = new MemoryStream();
                            using (var sw = new StreamWriter(ms))
                            {
                                sw.Write(txt);
                                sw.Flush();
                                ms.Position = 0;
                                UploadStream(ms, "VersionInfo.txt");
                            }
                            Console.WriteLine("Uploaded all files.");
                            MessageBox.Show("Uploading files seemed sucessful", "Uploading Finished ...", MessageBoxButtons.OK);
                        }
                        else
                            Console.WriteLine("Upload Canceled.");

                        Console.WriteLine("");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadLine();
                    }
                    break;

                case "CONVERT":

                    Console.WindowWidth = 85;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;

                    var path = @".\"; // CAREFUL - ONLY GO UP ONE DIRECTORY
                    if (args.Count() > 1)
                        path = args[1];

                    var convertedCount = 0;
                    var fileCount = 0;
                    var slnFiles = Directory.EnumerateFiles(path, "*.sln", SearchOption.AllDirectories).ToList();
                    fileCount = slnFiles.Count;

                    foreach (string slnFile in slnFiles)
                    {
                        Console.WriteLine(slnFile);
                        var lines = File.ReadAllLines(slnFile).ToList();
                        if (lines.Count() > 0)
                        {
                            var z = lines.Where(str => str.Contains("Format Version 12.00")).FirstOrDefault();
                            if (!string.IsNullOrEmpty(z))
                            {
                                var idx = lines.IndexOf(z);
                                if (idx > -1)
                                {
                                    lines[idx] = "Microsoft Visual Studio Solution File, Format Version 11.00";
                                    lines[idx + 1] = "# Visual Studio 2010";
                                    lines.RemoveAll(m => m.StartsWith("VisualStudioVersion"));
                                    lines.RemoveAll(m => m.StartsWith("MinimumVisualStudioVersion"));
                                    File.WriteAllLines(slnFile, lines.ToArray());
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Converted :" + Path.GetFileName(slnFile));
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    convertedCount++;
                                }
                            }
                        }
                    }

                    var csprojFiles = Directory.EnumerateFiles(path, "*.csproj", SearchOption.AllDirectories).ToList();
                    fileCount = fileCount + csprojFiles.Count;

                    foreach (string csprojFile in csprojFiles)
                    {
                        Console.WriteLine(csprojFile);
                        var lines = File.ReadAllLines(csprojFile).ToList();
                        if (lines.Count() > 0)
                        {
                            var z = lines.Where(str => str.Contains("Project ToolsVersion=\"12.0\"")).FirstOrDefault();
                            if (!string.IsNullOrEmpty(z))
                            {
                                var idx = lines.IndexOf(z);
                                if (idx > -1)
                                {
                                    lines[idx] = "<Project DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\" ToolsVersion=\"4.0\">";
                                    lines.RemoveAll(m => m.StartsWith("<Import Project=\"$(MSBuildExtensionsPath)"));
                                    File.WriteAllLines(csprojFile, lines.ToArray());
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Converted :" + Path.GetFileName(csprojFile));
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    convertedCount++;
                                }
                            }
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Converted: " + convertedCount + " of " + fileCount + " files.");
                    Console.WriteLine("Done converting");
                    Console.WriteLine("");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                    break;
            }

        }


        private static String ThisDirectory()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static void ResetCurrentDirectory()
        {
            Directory.SetCurrentDirectory(ThisDirectory());
        }


    }
}
