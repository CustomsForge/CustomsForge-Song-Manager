using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = true;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPass);
            request.ContentLength = stream.Length;
            Stream requestStream = request.GetRequestStream();
            stream.CopyTo(requestStream);
            requestStream.Close();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload File {0}, status {1}", Filename , response.StatusDescription);
            response.Close();
        }


        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "UPLOAD":
                    {
                        const string rnotes = @"..\CustomsForgeManager\ReleaseNotes.txt";
                        if (!File.Exists(rnotes))
                        {
                            Console.WriteLine("ReleaseNotes not found");
                            Environment.Exit(-5);
                        }

                        if (args.Count() > 1)
                            ftpURL = args[1];
                        if (args.Count() > 2)
                            ftpUsername = args[2];
                        if (args.Count() > 3)
                            ftpPass = args[3];

                        const string SetupFile = @"CFMSetup\Output\CFSMSetup.exe";
                        FileVersionInfo vi = FileVersionInfo.GetVersionInfo(SetupFile);


                        using (var fs = File.OpenRead(SetupFile))
                        {
                            UploadStream(fs, "CFSMSetup.exe");
                        }


                        Console.WriteLine(File.Exists(rnotes));
                        String txt = vi.ProductVersion.Trim() + '\n';
                        txt += File.ReadAllText(rnotes);

                        MemoryStream ms = new MemoryStream();
                        using (var sw = new StreamWriter(ms))
                        {
                            sw.Write(txt);
                            UploadStream(ms, "UpdateInfo.txt");
                        }
                    }
                    break;



                case "ConvertToVS2010":
                    var path = @"..\";
                    if (args.Count() > 1)
                        path = args[1];

                    var sol = Directory.GetFiles(path, "*.sln", SearchOption.TopDirectoryOnly);// SearchOption.AllDirectories);
                    foreach (string s in sol)
                    {
                        Console.WriteLine(s);
                        var lines = File.ReadAllLines(s).ToList();
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
                                    File.WriteAllLines(s, lines.ToArray());
                                    Console.WriteLine("converted :" + Path.GetFileName(s));
                                }
                            }
                        }
                    }
                    Console.WriteLine("done converting");
                    break;
            }
            Console.ReadLine();

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
