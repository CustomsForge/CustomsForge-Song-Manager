using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CustomControls;

namespace GenTools
{
    public static class WebExtensions
    {
        private static bool _downloadComplete;
        private static bool _downloadError;

        public static bool DownloadSync(string webUrl, string fileName, string downloadDir, int attempts = 4)
        {
            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        // alt method - direct download
                        //webClient.DownloadFile(webUrl, Path.Combine(downloadDir, appFileName));

                        // pooling data prevents writing an empty file - no feedback
                        byte[] downloadedBytes = webClient.DownloadData(webUrl);

                        if (downloadedBytes.Length != 0)
                        {
                            Stream file = File.Open(Path.Combine(downloadDir, fileName), FileMode.Create);
                            file.Write(downloadedBytes, 0, downloadedBytes.Length);
                            file.Close();
                            return true;
                        }
                    }
                }
                catch (WebException ex)
                {
                    GlobalExtensions.Log("DownloadWebApp Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    GlobalExtensions.Log("DownloadWebApp Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            GlobalExtensions.Log("DownloadWebApp no internet connection detected ...");
            return false;
        }


        public static bool DownloadAsync(string webUrl, string fileName, string downloadDir, int attempts = 4)
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
                        webClient.DownloadFileAsync(new Uri(webUrl), Path.Combine(downloadDir, fileName));

                        while (!_downloadComplete && !_downloadError)
                        {
                            Thread.Sleep(100);
                        }

                        if (_downloadError)
                            throw new WebException("The remote name could not be resolved: " + webUrl);

                        return true;
                    }
                }
                catch (WebException ex)
                {
                    GlobalExtensions.Log("DownloadWebApp Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    GlobalExtensions.Log("DownloadWebApp Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            GlobalExtensions.Log("DownloadWebApp no internet connection detected ...");
            return false;
        }

        private static void wcProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var bytesReceived = e.BytesReceived;
            var totalBytes = e.TotalBytesToReceive;
            var percentage = totalBytes == -1 ? 0 : (int)((double)bytesReceived / totalBytes * 100.0);

            GlobalExtensions.UpdateProgress.Value = percentage;
            Debug.WriteLine("Bytes Received: " + bytesReceived);
            Debug.WriteLine("Total Bytes: " + totalBytes);
            Debug.WriteLine("Percentage: " + percentage);
        }

        private static void wcCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                GlobalExtensions.UpdateProgress.Value = 100;
                _downloadComplete = true;
                GlobalExtensions.Log("Download Completed ...");
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
                    GlobalExtensions.Log("ExtractUrlData Web Exception: " + ex.Message + " ...");
                }
                catch (NotSupportedException ex)
                {
                    GlobalExtensions.Log("ExtractUrlData Not Supported Exception: " + ex.Message + " ...");
                }
                Thread.Sleep(200);
            }

            GlobalExtensions.Log("ExtractUrlData no internet connection detected ...");
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
