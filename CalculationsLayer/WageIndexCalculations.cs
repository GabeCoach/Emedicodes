using EmediCodesWebApplication.CalculationModels;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationsLayer
{
    public class WageIndexCalculations
    {
        private Logger oLogger = new Logger();

        #region Public Functions
        public WageIndexResponseModel CalculateWageIndexAdjustedPayment(WageIndexRequestModel oWageIndexRequest)
        {
            WageIndexResponseModel oWageIndexResponse = new WageIndexResponseModel();
            CalculateWageIndexAdjustedPaymentGateKeeper(oWageIndexRequest);
            WageIndexRepository oWageIndexRepo = new WageIndexRepository();

            var NationalPaymentRate = JsonConvert.DeserializeObject<string>(oWageIndexRequest.national_payment_rate.Trim().Replace("$", ""));

            oWageIndexRequest.national_payment_rate = NationalPaymentRate;

            if (oWageIndexRequest.wage_index == null || oWageIndexRequest.wage_index == 0)
            {
                IQueryable<CMS_Wage_Index> AreasByStateCode = oWageIndexRepo.GetAllAreasByStateCode(oWageIndexRequest.state_code);
                List<string> StateList = new List<string>();

                try
                {
                    foreach (CMS_Wage_Index area in AreasByStateCode)
                    {
                        int CBSA = Convert.ToInt32(area.CBSA);

                        if (CBSA < 53)
                        {
                            StateList.Add(area.Area_Name);
                        }
                    }

                    var oWageIndex = oWageIndexRepo.GetWageIndexByState(StateList.First()).FirstOrDefault();

                    Decimal decWageIndex = Convert.ToDecimal(oWageIndex.Wage_Index);

                    oWageIndexResponse = PerformWageIndexCalculation(oWageIndexRequest, decWageIndex);
                    return oWageIndexResponse;
                }
                catch (Exception ex)
                {
                    oLogger.LogData("METHOD: CalculateWageIndexAdjustedPayment; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                    throw;
                }
            }
            else
            {
                try
                {
                    CMS_Wage_Index oWageIndex = oWageIndexRepo.GetGAFByGeoLocation(
                    oWageIndexRequest.geo_location).FirstOrDefault();

                    var WageIndexCalculi = "";

                    if (String.IsNullOrEmpty(oWageIndex.Wage_Index))
                    {
                        WageIndexCalculi = oWageIndex.Reclassified_Wage_Index;
                    }
                    else
                    {
                        WageIndexCalculi = oWageIndex.Wage_Index;
                    }

                    oWageIndexResponse = PerformWageIndexCalculation(
                        oWageIndexRequest,
                        Convert.ToDecimal(WageIndexCalculi));

                    return oWageIndexResponse;
                }
                catch (Exception ex)
                {
                    oLogger.LogData("METHOD: CalculateWageIndexAdjustedPayment; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                    throw;
                }

            }

        }
        #endregion

        #region Private Functions
        private WageIndexResponseModel PerformWageIndexCalculation(WageIndexRequestModel oWageIndexRequest, Decimal decWageIndex)
        {
            try
            {
                WageIndexResponseModel oReturn = new WageIndexResponseModel();

                Double dblSixtyPercentFactor = Convert.ToDouble(oWageIndexRequest.national_payment_rate) * .60;
                Double dblFortyPercentFactor = Convert.ToDouble(oWageIndexRequest.national_payment_rate) * .40;

                Decimal decPreliminaryAdjustmentAmount = Convert.ToDecimal(dblSixtyPercentFactor) * decWageIndex;
                oReturn.preliminary_adjustment_amount = decPreliminaryAdjustmentAmount;

                Decimal dblGeoAdjustedPayment = decPreliminaryAdjustmentAmount + Convert.ToDecimal(dblFortyPercentFactor);
                oReturn.wage_index_adjusted_payment = dblGeoAdjustedPayment;

                return oReturn;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: PerformWageIndexCalculation; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }


        }

        private void CalculateWageIndexAdjustedPaymentGateKeeper(WageIndexRequestModel oWageIndexRequest)
        {
            try
            {
                if (oWageIndexRequest == null)
                {
                    oLogger.LogData("FUNCTION: CalculateWageIndexAdjustedPayment; ERROR: oWageIndexRequest is null or empty;");
                    throw new Exception("oWageIndexRequest is null or empty");
                }
                if (oWageIndexRequest.geo_location == null)
                {
                    oLogger.LogData("FUNCTION: CalculateWageIndexAdjustedPayment; ERROR: geo_location is null or empty;");
                    throw new Exception("geo_location is null or empty");
                }
                if (oWageIndexRequest.state_code == null)
                {
                    oLogger.LogData("FUNCTION: CalculateWageIndexAdjustedPayment; ERROR: geo_location is null or empty;");
                    throw new Exception("geo_location is null or empty");
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CalculateWageIndexAdjustedPaymentGateKeeper; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }

        #endregion
    }
}