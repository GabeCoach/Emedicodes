using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationModels
{
    public class PaymentRateRequestModel
    {
        public string CPTCode { get; set; }
        public string Locale { get; set; }
        public string PaymentRateScope { get; set; }
    }
}