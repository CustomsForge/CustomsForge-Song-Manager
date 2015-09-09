using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CustomsForgeManager.CustomsForgeManagerLib
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
    }
}
