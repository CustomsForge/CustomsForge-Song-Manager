using System;
using System.Diagnostics;
using System.Net;
using CustomsForgeSongManager.DataObjects;
using Newtonsoft.Json.Linq;

namespace CustomsForgeSongManager.ClassMethods
{
    public static class Ignition
    {
        public static string GetDLCInfoFromURL(string apiUrl, string fieldName)
        {
            string result = String.Empty;
            WebClient client = new WebClient();
            var response = client.DownloadString(apiUrl);
            if (response == "No Results")
                result = response;
            else if (response == "No album with that name")
                result = "No Results";
            else
            {
                if (response != "")
                {
                    JArray jsonJArray = JArray.Parse(response);
                    JToken jsonJToken = jsonJArray.First;
                    if (jsonJToken != null)
                        result = jsonJToken.SelectToken(fieldName).ToString();
                    else
                        result = "No Results";
                }
                else
                    result = "No Results";
            }
            return result;
        }

        public static string GetDLCInfoFromResponse(string response, string fieldName)
        {
            string result = String.Empty;
            if (response == "No Results")
                result = response;
            else if (response == "No album with that name")
                result = "No Results";
            else
            {
                if (response != "")
                {
                    JArray jsonJArray = JArray.Parse(response);
                    JToken jsonJToken = jsonJArray.First;
                    if (jsonJToken != null)
                        result = jsonJToken.SelectToken(fieldName).ToString();
                    else
                        result = "No Results";
                }
                else
                    result = "No Results";
            }
            return result;
        }

        public static string GetInfoURL(this SongData song)
        {
            var url = Constants.DefaultInfoURL;
            return url;
        }

        public static string GetAuthURL()
        {
            return Constants.DefaultAuthURL;
        }

        public static void FetchInfo(this SongData song)
        {
            string url = song.GetInfoURL();
            //= client.DownloadString(url);
            string response = String.Empty;
            var client = new WebClient();
            client.DownloadStringCompleted += (sender, e) =>
                {
                    response = e.Result;

                    song.IgnitionID = GetDLCInfoFromResponse(response, "id");
                    song.IgnitionUpdated = GetDLCInfoFromResponse(response, "updated");
                    song.IgnitionVersion = GetDLCInfoFromResponse(response, "version");
                    song.IgnitionAuthor = GetDLCInfoFromResponse(response, "name");
                };
            client.DownloadStringAsync(new Uri(url));
        }

        public static void UploadToCustomsForge()
        {
            Process.Start(Constants.IgnitionURL + "/creators/submit");
        }

        public static void RequestSongOnCustomsForge()
        {
            Process.Start(Constants.RequestURL);
        }

        public static void CheckForUpdateStatus(this SongData currentSong, bool isAsync = false)
        {
            currentSong.Status = SongDataStatus.None;

            string url = currentSong.GetInfoURL();
            string response = String.Empty;
            string cfUrl = String.Empty;
            var client = new WebClient();
            int version = 0;

            //isAsync = false;

            if (!isAsync)
            {
                response = client.DownloadString(new Uri(url));

                currentSong.IgnitionID = GetDLCInfoFromResponse(response, "id");
                currentSong.IgnitionUpdated = GetDLCInfoFromResponse(response, "updated");
                currentSong.IgnitionVersion = GetDLCInfoFromResponse(response, "version");
                currentSong.IgnitionAuthor = GetDLCInfoFromResponse(response, "name");

                if (Int32.TryParse(currentSong.Version, out version))
                {
                    currentSong.Version += ".0";
                }

                if (Int32.TryParse(currentSong.IgnitionVersion, out version))
                {
                    currentSong.IgnitionVersion += ".0";
                }

                if (currentSong.IgnitionVersion == "No Results")
                {
                    currentSong.Status = SongDataStatus.NotFound;
                }
                else if (currentSong.Version == "N/A")
                {
                    //TODO: Check for updates by release/update date
                }
                else if (currentSong.IgnitionVersion != currentSong.Version)
                {
                    currentSong.Status = SongDataStatus.OutDated;
                }
                else if (currentSong.IgnitionVersion == currentSong.Version)
                {
                    currentSong.Status = SongDataStatus.UpToDate;
                }
            }
            else
            {
                client.DownloadStringCompleted += (sender, e) =>
                    {
                        response = e.Result;

                        currentSong.IgnitionID = GetDLCInfoFromResponse(response, "id");
                        currentSong.IgnitionUpdated = GetDLCInfoFromResponse(response, "updated");
                        currentSong.IgnitionVersion = GetDLCInfoFromResponse(response, "version");
                        currentSong.IgnitionAuthor = GetDLCInfoFromResponse(response, "name");

                        if (Int32.TryParse(currentSong.Version, out version))
                        {
                            currentSong.Version += ".0";
                        }

                        if (Int32.TryParse(currentSong.IgnitionVersion, out version))
                        {
                            currentSong.IgnitionVersion += ".0";
                        }

                        if (currentSong.IgnitionVersion == "No Results")
                        {
                            currentSong.Status = SongDataStatus.NotFound;
                        }
                        else if (currentSong.Version == "N/A")
                        {
                            //TODO: Check for updates by release/update date
                        }
                        else if (currentSong.IgnitionVersion != currentSong.Version)
                        {
                            currentSong.Status = SongDataStatus.OutDated;
                        }
                        else if (currentSong.IgnitionVersion == currentSong.Version)
                        {
                            currentSong.Status = SongDataStatus.UpToDate;
                        }
                    };

                client.DownloadStringAsync(new Uri(url));
            }
        }
    }
}