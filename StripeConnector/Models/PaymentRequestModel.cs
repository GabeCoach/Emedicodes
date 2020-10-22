using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.StripeConnector.Models
{
    public class PaymentRequestModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Auth0Id { get; set; }
        public string coupon_code { get; set; }
    }
}