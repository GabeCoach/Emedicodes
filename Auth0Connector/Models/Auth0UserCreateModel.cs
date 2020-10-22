using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Auth0Connector.Models
{
    public class Auth0UserCreateModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool verify_email { get; set; }
        public string connection { get; set; }
    }
}