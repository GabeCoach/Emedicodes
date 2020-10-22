using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationModels
{
    public class HospitalTreatmentCalculationModel
    {
        public string cpt_code { get; set; }
        public string modality_name { get; set; }
        public int number_of_treatments { get; set; }
        public Double treatment_dollar_amount { get; set; }
        public Double opps_payment_rate { get; set; }
    }
}