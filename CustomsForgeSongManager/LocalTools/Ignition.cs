using System;
using System.Diagnostics;
using System.Net;
using CustomsForgeSongManager.DataObjects;
using Newtonsoft.Json.Linq;

namespace CustomsForgeSongManager.LocalTools
{
    public static class Ignition
    {
        public static string GetSongInfoFromURL(string apiUrl, string fieldName)
        {
            string result = "No Internet Connection";
            WebClient client = new WebClient();
            try
            {
                result = client.DownloadString(apiUrl);
            }
            catch (WebException ex)
            {
                Debug.WriteLine(result + " : " + ex.Message);
                return result;
            }

            if (result == "No Results")
            { /* DO NOTHING */}
            else if (result == "No album with that name")
            { /* DO NOTHING */}
            else
            {
                if (result != "")
                {
                    JArray jsonJArray = JArray.Parse(result);
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

        public static string GetSongInfoFromResponse(string response, string fieldName)
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
                    song.IgnitionID = GetSongInfoFromResponse(response, "id");
                    song.IgnitionUpdated = GetSongInfoFromResponse(response, "updated");
                    song.IgnitionVersion = GetSongInfoFromResponse(response, "version");
                    song.IgnitionAuthor = GetSongInfoFromResponse(response, "name");
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

                currentSong.IgnitionID = GetSongInfoFromResponse(response, "id");
                currentSong.IgnitionUpdated = GetSongInfoFromResponse(response, "updated");
                currentSong.IgnitionVersion = GetSongInfoFromResponse(response, "version");
                currentSong.IgnitionAuthor = GetSongInfoFromResponse(response, "name");

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

                        currentSong.IgnitionID = GetSongInfoFromResponse(response, "id");
                        currentSong.IgnitionUpdated = GetSongInfoFromResponse(response, "updated");
                        currentSong.IgnitionVersion = GetSongInfoFromResponse(response, "version");
                        currentSong.IgnitionAuthor = GetSongInfoFromResponse(response, "name");

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

        public static string CleanForAPI(this string text)
        {
            //return text.Replace("/", "_"); //.Replace("\\","");
            var result = text.Replace("/", "_1_").Replace("\\", "_2_"); //WebUtility.HtmlEncode(text);
            return result; //WebUtility.HtmlDecode(text);
        }

    }
}