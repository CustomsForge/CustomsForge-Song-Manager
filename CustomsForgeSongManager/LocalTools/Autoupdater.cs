using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CustomControls;
using GenTools;
using CustomsForgeSongManager.DataObjects;
using CustomsForgeSongManager.Forms;
using RocksmithToolkitLib;

namespace CustomsForgeSongManager.LocalTools
{
    public class AutoUpdater
    {
        private const string NO_INTERNET = "No Internet Connection";
        private const string NOT_FOUND = "Not Found";
        private static string _versOnline;
        private static DateTime _lastCheck = default(DateTime);
        private static string _versInfoUrl;

        public static string ReleaseNotes { get; private set; }

        public static string VersOnline
        {
            get
            {
                if (_versInfoUrl == null)
                    throw new Exception("This should never occure ... no versUrl was specified.");

                if (_versOnline == null)
                {
                    DownloadVersInfo(_versInfoUrl);
                }

                return _versOnline;
            }

            set
            {
                _versOnline = value;
            }
        }

        public static string VersInstalled(string appExePath)
        {
            if (!File.Exists(appExePath))
                return NOT_FOUND;

            var versInfo = FileVersionInfo.GetVersionInfo(appExePath);
            var fileVersion = versInfo.FileVersion;
            var productVersion = versInfo.ProductVersion;

            if (String.IsNullOrEmpty(productVersion))
                return NOT_FOUND;

            return productVersion;
        }

        public static bool UpdateWithInno(string appSetupPath)
        {
            // updating is handled by Inno Setup Installer
            // the installer forces the user to close the program before installing
            if (File.Exists(appSetupPath))
            {
                GenExtensions.RunExtExe(appSetupPath, false, arguments: "-appupdate");
                return true;
            }

            MessageBox.Show(Path.GetFileName(appSetupPath) + " not found, please download the program again.");
            return false;
        }

        public static bool NeedsUpdate(string appExePath, string versInfoUrl)
        {
            _versInfoUrl = versInfoUrl; // share versInfoUrl
            VersOnline = null; // reset OnlineVersion data

            // RSTK stores VersionInfo differently than Inno apps
            if (versInfoUrl.StartsWith(Constants.RSToolkitURL))
            {
                if (IsTlsCompat("Rocksmith Custom Song Toolkit"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            var versOnlineRSTK = ToolkitVersionOnline.Load();
                            VersOnline = versOnlineRSTK.Revision; // returns github commit (4 bytes)
                            break;
                        }
                        catch (WebException ex)
                        {
                            // Globals.Log("NeedsUpdate Web Exception: " + ex.Message + " ...");
                        }
                        catch (NotSupportedException ex)
                        {
                            // Globals.Log("NeedsUpdate Not Supported Exception: " + ex.Message + " ...");
                        }
                        Thread.Sleep(200);
                    }

                    if (VersOnline == NO_INTERNET)
                        return false;
                }
                else
                    return false;
            }

            var versInstalled = VersInstalled(appExePath).Trim();
            var versOnline = VersOnline.Trim();

            if (versOnline != versInstalled && versOnline != NO_INTERNET)
            {
                Globals.Log(Path.GetFileName(appExePath) + "[" + versInstalled + "] needs updating ...");
                Debug.WriteLine("OnlineVers: " + versOnline + "  InstalledVers: " + versInstalled);
                return true;
            }

            if (versOnline == versInstalled)
            {
                Globals.Log(Path.GetFileName(appExePath) + " [" + versInstalled + "] does not need updating ...");
                Debug.WriteLine("OnlineVers: " + versOnline + "  InstalledVers: " + versInstalled);
                return false;
            }

            return false;
        }

        private static void DownloadVersInfo(string versionUrl, int attempts = 4)
        {
            for (int i = 0; i < attempts; i++)
            {
                // this will throw an exception if there is no internet connection
                try
                {
                    var webRequest = (HttpWebRequest)WebRequest.Create(versionUrl);
                    webRequest.Method = "GET";
                    webRequest.Timeout = 5000;
                    webRequest.KeepAlive = true;

                    using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        if (webResponse.StatusCode == HttpStatusCode.OK)
                        {
                            webRequest.BeginGetResponse(new AsyncCallback(GetVersionInfo), webRequest);
                            return;
                        }
                    }
                }
                catch (WebException ex)
                {
                    Globals.Log("DownloadVersionInfo Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    Globals.Log("DownloadVersionInfo Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            Globals.Log("DownloadVersionInfo Web Exception: Connection Timed Out ...");
            VersOnline = NO_INTERNET;
        }

        private static void GetVersionInfo(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
            try
            {
                using (var webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var s = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(s))
                    {
                        var lines = s.Split('\n');
                        //_latestVers = new Version(lines[0]);
                        VersOnline = lines[0];
                        var l = lines.ToList();
                        l.RemoveAt(0);
                        if (l.Count > 0)
                        {
                            ReleaseNotes = String.Join("\n", l.ToArray());
                        }

                        _lastCheck = DateTime.Now;
                        Globals.Log("GetVersionInfo made a good connection with the server ...");
                    }
                }
            }
            catch (WebException ex)
            {
                Globals.Log("GetVersionInfo Web Exception: " + ex.Message + " ...");
            }
            catch (NotSupportedException ex)
            {
                Globals.Log("GetVersionInfo Not Supported Exception: " + ex.Message + " ...");
            }
        }

        private static bool _downloadComplete;
        private static bool _downloadError;

        public static bool DownloadWebApp(string webUrl, string appFileName, string downloadDir, int attempts = 4)
        {
            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        // async download with progress
                        _downloadComplete = false;
                        _downloadError = false;
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(wcCompleted);
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcProgressChanged);
                        webClient.DownloadFileAsync(new Uri(webUrl), Path.Combine(downloadDir, appFileName));

                        while (!_downloadComplete && !_downloadError)
                        {
                            Application.DoEvents();
                            Thread.Sleep(100);
                        }

                        if (_downloadError)
                            throw new WebException("The remote name could not be resolved: " + webUrl);

                        Globals.Log("Successfully Downloaded WebApp: " + appFileName);
                        return true;

                        // alternate methods of download
                        // direct download - no feedback
                        //webClient.DownloadFile(webUrl, Path.Combine(downloadDir, appFileName));

                        // pooling data prevents writing an empty file - no feedback
                        //byte[] downloadedBytes = webClient.DownloadData(webUrl);

                        //if (downloadedBytes.Length != 0)
                        //{
                        //    Stream file = File.Open(Path.Combine(downloadDir, appFileName), FileMode.Create);
                        //    file.Write(downloadedBytes, 0, downloadedBytes.Length);
                        //    file.Close();
                        //    return true;
                        //}
                    }
                }
                catch (WebException ex)
                {
                    Globals.Log("DownloadWebApp Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    Globals.Log("DownloadWebApp Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            Globals.Log("DownloadWebApp no internet connection detected ...");
            return false;
        }

        private static void wcProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
            {
                Globals.TsProgressBar_Main.Value = e.ProgressPercentage;
            });
        }

        private static void wcCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GenExtensions.InvokeIfRequired(Globals.TsProgressBar_Main.GetCurrentParent(), delegate
                {
                    Globals.TsProgressBar_Main.Value = 100;
                });

                _downloadComplete = true;
                Globals.Log("Download Completed ...");
            }
            else
                _downloadError = true;
        }

        public static List<string> ExtractUrlData(string webUrl, string extractSwitch = "", int attempts = 4)
        {
            var urlLinks = new List<string>();
            var webClient = new WebClient();

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    byte[] buffer = webClient.DownloadData(webUrl);
                    string htmlSource = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    List<string> links = DataExtractor.Extract(htmlSource, extractSwitch);

                    foreach (var link in links)
                    {
                        urlLinks.Add(link);
                    }

                    return urlLinks;
                }
                catch (WebException ex)
                {
                    Globals.Log("ExtractUrlData Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    Globals.Log("ExtractUrlData Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            Globals.Log("ExtractUrlData no internet connection detected ...");
            return urlLinks;
        }

        public static bool IsTlsCompat(string appName)
        {
            // requires Net 4.5, or Win7 and IE8
            // use TLS 1.2 protocol if available
            // ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            // check system config, some websites e.g. http://www.rscustom.net/ require TSL 1.2 compatible browswer
            var errMsg = String.Empty;
            var ieVers = SysExtensions.GetBrowserVersion(SysExtensions.GetInternetExplorerVersion());
            if (ieVers < 8.0)
                errMsg = "Internet Explorer 8 or greater is required";

            var sysVers = SysExtensions.MajorVersion + (double)SysExtensions.MinorVersion / 10;
            if (sysVers < 6.1)
                errMsg = !String.IsNullOrEmpty(errMsg) ? errMsg + Environment.NewLine + "and OS Windows 7 or greater is required" : "OS Windows 7 or greater is required";

            if (!String.IsNullOrEmpty(errMsg))
            {
                errMsg = errMsg + Environment.NewLine + "to download " + appName;
                BetterDialog2.ShowDialog(errMsg, "Incompatible System Configuration", null, null, "Ok", Bitmap.FromHicon(SystemIcons.Warning.Handle), "Warning", 150, 150);
                return false;
            }

            return true;
        }

    }

    public class DataExtractor
    {
        /// <summary>
        /// Extracts all src and href links from a HTML string.
        /// </summary>
        /// <param name="htmlSource">The html source</param>
        /// <param name="extractSwitch">defaults to href/src links</param>
        /// <returns>A list of links - these will be all links including javascript ones.</returns>
        public static List<string> Extract(string htmlSource, string extractSwitch = "")
        {
            List<string> list = new List<string>();
            Regex regex;

            switch (extractSwitch)
            {
                case "div":
                    regex = new Regex(@"<div [^>]*>(.*?)</div>", RegexOptions.Singleline | RegexOptions.CultureInvariant);
                    break;

                default:
                    regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
                    break;
            }

            if (regex.IsMatch(htmlSource))
            {
                foreach (Match match in regex.Matches(htmlSource))
                {
                    var extractedValue = match.Groups[1].Value;
                    list.Add(match.Groups[1].Value);
                }
            }

            return list;
        }
    }


}