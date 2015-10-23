using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CustomsForgeManager.CustomsForgeManagerLib.Objects
{
    #if AUTOUPDATE
    public class Autoupdater
    {
        const string UpdateURL = "http://dfcrs.com/CFSM";
        const string VersionURL = UpdateURL + "/vinfo.txt";//"/VersionInfo.txt";
        const string SetupURL = UpdateURL + "/CFSMSetup.exe";
        private static bool hasServerInfo = false;
        public static string ReleaseNotes { get; private set; }
        private static Version FLatestVersion;
        private static DateTime FLastChecked = default(DateTime);

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
            return LatestVersion > new Version(Constants.CustomVersion());
        }

        private static void DownloadCurrent()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(VersionURL);
            webRequest.Method = "GET";
            webRequest.Timeout = 3000;
            webRequest.BeginGetResponse(new AsyncCallback(GetVersionInfo), webRequest);  
        }

        private static void GotVersionInfo()
        {
            FLastChecked = DateTime.Now;
            hasServerInfo = true;
            if (OnInfoRecieved != null)
                OnInfoRecieved(null, EventArgs.Empty);
        }

        private static void GetVersionInfo(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult))
                {
                    StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
                    string s = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(s))
                    {
                        string[] lines = s.Split('\n');
                        FLatestVersion = new Version(lines[0]);
                        var l = lines.ToList();
                        l.RemoveAt(0);
                        if (l.Count > 0)
                        {
                            ReleaseNotes = String.Join("\n", l.ToArray());
                        }


                        GotVersionInfo();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
#endif
}
