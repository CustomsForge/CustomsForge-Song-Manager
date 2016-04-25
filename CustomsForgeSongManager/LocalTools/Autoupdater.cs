using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading;
using CustomsForgeSongManager.DataObjects;

namespace CustomsForgeSongManager.LocalTools
{
    //#if AUTOUPDATE
    public class Autoupdater
    {
#if RELEASE
        const string UpdateURL = "http://appdev.cfmanager.com/release";
#else
        private const string UpdateURL = "http://appdev.cfmanager.com/beta";
#endif

        private const string VersionURL = UpdateURL + "/VersionInfo.txt";
        //   const string SetupURL = UpdateURL + "/CFSMSetup.exe";
        private static bool hasServerInfo = false;
        public static string ReleaseNotes { get; private set; }
        private static Version FLatestVersion;
        private static DateTime FLastChecked = default(DateTime);
        private static int attemptCount = 0;

        static Autoupdater()
        {
            DownloadCurrent();
        }

        public static void RunUpdater()
        {
            if (!hasServerInfo)
                return;
        }

        public static event EventHandler OnInfoRecieved;

        public static Version LatestVersion
        {
            get
            {
                if (!hasServerInfo)
                    return new Version(Constants.CustomVersion());
                return FLatestVersion;
            }
        }

        public static bool NeedsUpdate()
        {
            if (LatestVersion > new Version(Constants.CustomVersion()))
                Globals.Log("CFSM needs updating ...");

            return LatestVersion > new Version(Constants.CustomVersion());
        }

        // stackoverflow 14192993
        private static void DownloadCurrent()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(VersionURL);
            webRequest.Method = "GET";
            webRequest.Timeout = 5000;
            webRequest.KeepAlive = true;
            try
            {
                // this will throw and error if there is no internet connection
                var webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode == HttpStatusCode.OK)
                    webRequest.BeginGetResponse(new AsyncCallback(GetVersionInfo), webRequest);
                else
                {
                    // try to connect a bunch of times
                    if (attemptCount < 15)
                    {
                        attemptCount++;
                        Thread.Sleep(500);
                        DownloadCurrent();
                    }
                    else
                        Globals.Log("Unable to check for CFSM updates, server not responding ...");
                }
            }
            catch
            {
                // try a couple of times
                if (attemptCount < 4)
                {
                    attemptCount++;
                    Thread.Sleep(500);
                    DownloadCurrent();
                }
                else
                    Globals.Log("Unable to check for CFSM updates, no internet connection detected ...");
            }
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
                        FLatestVersion = new Version(lines[0]);
                        var l = lines.ToList();
                        l.RemoveAt(0);
                        if (l.Count > 0)
                        {
                            ReleaseNotes = String.Join("\n", l.ToArray());
                        }
                        GotVersionInfo();
                        Globals.Log("Auto update feature made a good connection with the server ...");
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Log("ERROR: Auto update failed " + ex.Message);
            }
        }

        private static void GotVersionInfo()
        {
            FLastChecked = DateTime.Now;
            hasServerInfo = true;
            if (OnInfoRecieved != null)
                OnInfoRecieved(null, EventArgs.Empty);
        }
    }

    //#endif
}