using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmediCodesWebApplication.Logging;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EmediCodesWebApplication.HelperMethods
{
    public class HTTPMethods
    {
        private Logger oLogger = new Logger();

        public async Task<string> GetAsync (string sEndpoint, string sAuthToken, string sAuthType)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sEndpoint);
                request.Method = "GET";
                request.Headers.Add(HttpRequestHeader.Authorization, sAuthType + " " + sAuthToken);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                string sResponse = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

                return sResponse;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: HTTPGetAsync; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> PostAsync (string sEndPoint, string sAuthToken, string sAuthType, dynamic objPostData)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sEndPoint);
                request.Method = "POST";
                request.Headers.Add(HttpRequestHeader.Authorization, sAuthType + " " + sAuthToken);
                request.ContentType = "application/json";

                var jsnRequestBody = JsonConvert.SerializeObject(objPostData);

                var writer = new StreamWriter(await request.GetRequestStreamAsync());
                await writer.WriteAsync(jsnRequestBody);
                writer.Close();

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                string sResponse = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

                return sResponse;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: HTTPPostAsync; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> PutAsync(string sEndPoint, string sAuthToken, string sAuthType, dynamic objPostData)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sEndPoint);
                request.Method = "PUT";
                request.Headers.Add(HttpRequestHeader.Authorization, sAuthType + " " + sAuthToken);
                request.ContentType = "application/json";

                var jsnRequestBody = JsonConvert.SerializeObject(objPostData);

                var writer = new StreamWriter(await request.GetRequestStreamAsync());
                await writer.WriteAsync(jsnRequestBody);
                writer.Close();

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string sResponse = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

                return sResponse;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: HTTPPutAsync; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteAsync(string sEndPoint, string sAuthToken, string sAuthType)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sEndPoint);
                request.Method = "DELETE";
                request.Headers.Add(HttpRequestHeader.Authorization, sAuthType + " " + sAuthToken);
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string sResponse = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

                return sResponse;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: HTTPDeleteAsync; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}