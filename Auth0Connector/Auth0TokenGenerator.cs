using EmediCodesWebApplication.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace EmediCodesWebApplication.Auth0Connector
{
    public class Auth0TokenGenerator
    {
        Logger oLogger = new Logger();

        public string GetAuth0AccessToken()
        {
            try
            {
                var clientID = ConfigurationManager.AppSettings["client_id"];
                var clientSecret = ConfigurationManager.AppSettings["client_secret"];
                var audience = ConfigurationManager.AppSettings["audience"];

                var RequestBody = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" },
                {"client_id", clientID },
                {"client_secret", clientSecret },
                {"audience", audience }
            };

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://emedicodes.auth0.com/oauth/token");
                request.Method = "POST";
                request.ContentType = "application/json";

                var jsonRequestBody = JsonConvert.SerializeObject(RequestBody);

                var writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonRequestBody);
                writer.Close();

                var response = (HttpWebResponse)request.GetResponse();

                string sResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return sResponse;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetAuth0AccessToken; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}