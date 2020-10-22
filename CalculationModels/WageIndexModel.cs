using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationModels
{
    public class WageIndexRequestModel
    {
        public string geo_location { get; set; }
        public Decimal? wage_index { get; set; }
        public string national_payment_rate { get; set; }
        public string state_code { get; set; }
        public string state { get; set; }
        public bool has_error { get; set; }
        public string error_message { get; set; }

    }

    public class WageIndexResponseModel
    {
        public Decimal preliminary_adjustment_amount { get; set; }
        public Decimal wage_index_adjusted_payment { get; set; }
        public string state_code { get; set; }
        public string state { get; set; }
        public bool has_error { get; set; }
        public string error_message { get; set; }
    }
}