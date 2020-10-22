using EmediCodesWebApplication.Auth0Connector.Models;
using EmediCodesWebApplication.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using static EmediCodesWebApplication.Auth0Connector.Models.Auth0TokenModel;
using EmediCodesWebApplication.Repository;

namespace EmediCodesWebApplication.Auth0Connector
{
    public class Auth0Users
    {
        private Auth0TokenGenerator tokenGenerator = new Auth0TokenGenerator();
        private Logger oLogger = new Logger();
        private UserRepository oUserRepo = new UserRepository();

        public string CreateAuth0User(string sEmail, string sPassword)
        {
            try
            {
                string sUsersEndpoint = "https://emedicodes.auth0.com/api/v2/users";

                var Token = JsonConvert.DeserializeObject<Auth0TokenRequestModel>(tokenGenerator.GetAuth0AccessToken()).access_token;

                var values = new Auth0UserCreateModel()
                {
                    email = sEmail,
                    password = sPassword,
                    connection = "Username-Password-Authentication",
                    verify_email = false
                };

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sUsersEndpoint);
                request.Method = "POST";
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + Token);
                request.ContentType = "application/json";

                var jsonRequestBody = JsonConvert.SerializeObject(values);

                var writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonRequestBody);
                writer.Close();

                var response = (HttpWebResponse)request.GetResponse();

                string sResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return sResponse;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CreateAuth0User; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                oUserRepo.DeleteUser(sEmail);
                throw;
            }
        }

        public void SendUserVerificationEmail(string userId)
        {
            try
            {
                string sUsersEndpoint = "https://emedicodes.auth0.com/api/v2/jobs/verification-email";

                var Token = JsonConvert.DeserializeObject<Auth0TokenRequestModel>(tokenGenerator.GetAuth0AccessToken()).access_token;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(sUsersEndpoint);
                request.Method = "POST";
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + Token);
                request.ContentType = "application/json";

                string clientId = ConfigurationManager.AppSettings["client_id"];

                var values = new Dictionary<string, string>()
                {
                    { "user_id", userId },
                    {"client_id", clientId }
                };

                var jsonRequestBody = JsonConvert.SerializeObject(values);

                var writer = new StreamWriter(request.GetRequestStream());
                writer.Write(jsonRequestBody);
                writer.Close();

                var response = (HttpWebResponse)request.GetResponse();

                string sResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var jsonResponse = JsonConvert.DeserializeObject(sResponse);

                oLogger.LogData("Verification email sent: " + jsonResponse);
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: SendUserVerificationEmail; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}