using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CustomsForgeManager_Winforms.Utilities;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CustomsForgeManager_Winforms.lib
{
    public static class Ignition
    {
        public static string GetDLCInfoFromURL(string apiUrl, string fieldName)
        {
            string result = "";
            var response = "";
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (sender, e) =>
            {
                response = e.Result;
                if (response == "No Results")
                    result = response;
                else if (response == "No album with that name")
                    result = "No Results";
                else
                {
                    JArray jsonJArray = JArray.Parse(response);
                    JToken jsonJToken = jsonJArray.First;
                    if (jsonJToken != null)
                        result = jsonJToken.SelectToken(fieldName).ToString();
                    else
                        result = "No Results";
                }
            };
            client.DownloadStringAsync(new Uri(apiUrl));
            return result;
        }
    }
}
