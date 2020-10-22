using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Auth0Connector.Models
{
    public class Auth0TokenModel
    {
        public class Auth0TokenRequestModel
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }

        public class Auth0TokenReturnModel
        {
            public string email { get; set; }
            public bool email_verified { get; set; }
            public string user_id { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
        }
    }
}