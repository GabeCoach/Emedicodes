using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.RegistrationLayer
{
    public class UserRegistrationRequestModel
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string street_address { get; set; }
        public string phone_number { get; set; }
        public string coupon_code { get; set; }
        public string password { get; set; }
        public string password_confirm { get; set; }
    }
}