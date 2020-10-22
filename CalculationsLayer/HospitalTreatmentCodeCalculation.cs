using EmediCodesWebApplication.CalculationModels;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationsLayer
{
    public class HospitalTreatmentCodeCalculation
    {
        private HospitalTreatmentCodeRepository oHospTreatmentRepo = new HospitalTreatmentCodeRepository();
        private OPPSRepository oOPPSReop = new OPPSRepository();
        private ModalityRepository oModalityRepo = new ModalityRepository();
        private Logger oLogger = new Logger();

        public List<HospitalTreatmentCalculationModel> GetHospitalTreatmentCalculation(int ModalityID)
        {

            var HospitalTreatmentCodes = oHospTreatmentRepo.GetHospTreatmentCodesByModality(ModalityID);
            List<HospitalTreatmentCalculationModel> lstTreatmentCalculations = new List<HospitalTreatmentCalculationModel>();

            try
            {
                foreach (var treatment in HospitalTreatmentCodes)
                {
                    HospitalTreatmentCalculationModel oHospTreatmentCalculationModel = new HospitalTreatmentCalculationModel();
                    var OPPS = oOPPSReop.GetOPPSCByCPT(treatment.CPTCode).FirstOrDefault();
                    string ModalityName = oModalityRepo.GetModalityByID((int)treatment.ModalityID).FirstOrDefault().ModalityName;

                    if (OPPS.Payment_Rate_ == 0)
                    {
                        oHospTreatmentCalculationModel.cpt_code = treatment.CPTCode;
                        oHospTreatmentCalculationModel.modality_name = ModalityName;
                        oHospTreatmentCalculationModel.number_of_treatments = (int)treatment.NumberOfTreatment;
                        oHospTreatmentCalculationModel.treatment_dollar_amount = 0;
                        oHospTreatmentCalculationModel.opps_payment_rate = 0;

                        lstTreatmentCalculations.Add(oHospTreatmentCalculationModel);
                    }
                    else
                    {
                        Double dblPaymentRate = Convert.ToDouble(OPPS.Payment_Rate_);

                        oHospTreatmentCalculationModel.cpt_code = treatment.CPTCode;
                        oHospTreatmentCalculationModel.modality_name = ModalityName;
                        oHospTreatmentCalculationModel.number_of_treatments = (int)treatment.NumberOfTreatment;
                        oHospTreatmentCalculationModel.opps_payment_rate = dblPaymentRate;

                        oHospTreatmentCalculationModel.treatment_dollar_amount = dblPaymentRate * oHospTreatmentCalculationModel.number_of_treatments;

                        lstTreatmentCalculations.Add(oHospTreatmentCalculationModel);
                    }
                }

                return lstTreatmentCalculations;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetHospitalTreatmentCalculation; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}