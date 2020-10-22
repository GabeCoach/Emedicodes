using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationModels
{
    public class PaymentRateReturnModel
    {
        public string CPTCode { get; set; }
        public string CPTMod { get; set; }
        public string Scope { get; set; }
        public Double PaymentRate { get; set; }
        public string Locale { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}