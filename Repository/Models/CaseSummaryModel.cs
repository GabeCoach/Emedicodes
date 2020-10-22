using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository.Models
{
    public class CaseSummaryRequestModel
    {
        public string treatment_setting { get; set; }
        public string locality { get; set; }
        public string scope { get; set; }
    }

    public class CaseSummaryReturnModel
    {
        public string cpt_code { get; set; }
        public int number_of_treatments { get; set; }
        public string treatment_setting { get; set; }
        public Double payment_rate { get; set; }
        public Double treatment_cost { get; set; }
    }
}