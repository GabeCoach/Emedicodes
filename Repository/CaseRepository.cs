using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Repository.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EmediCodesWebApplication.Logging;
using System.Data.Entity;
using EmediCodesWebApplication.CalculationModels;
using EmediCodesWebApplication.CalculationsLayer;

namespace EmediCodesWebApplication.Repository
{
    public class CaseRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();
        private OPPSRepository oOPPSRepo = new OPPSRepository();

        public IQueryable<Case> GetCasesByUser(int UserID)
        {
            try
            {
                var UserCases = db.Cases.Where(d => d.UserID == UserID);
                return (IQueryable<Case>)UserCases;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetCasesByUser; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<Case> CreateCase(Case oCase)
        {
            try
            {
                oCase.CreatedAt = DateTime.Now;
                db.Cases.Add(oCase);
                await db.SaveChangesAsync();
                return oCase;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CreateCase; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<Case> DeleteCase(int CaseId)
        {
            try
            {
                Case oCase = await db.Cases.FindAsync(CaseId);

                if (oCase == null)
                {
                    throw new KeyNotFoundException();
                }

                db.Cases.Remove(oCase);
                await db.SaveChangesAsync();
                return oCase;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: DeleteCase; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<IQueryable<List<Case>>> GetUserCaseList(int UserID)
        {
            try
            {
                var UserCases = await db.Cases.Where(c => c.UserID == UserID).ToListAsync<Case>();
                return (IQueryable<List<Case>>)UserCases;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetUserCaseList; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public List<CaseSummaryReturnModel> GetCaseSummary(List<CaseCode> lstCaseCodes, CaseSummaryRequestModel oCaseSummary)
        {
            List<CaseSummaryReturnModel> lstCaseReturn = new List<CaseSummaryReturnModel>();

            foreach (var caseCode in lstCaseCodes)
            {
                CaseSummaryReturnModel oCaseReturn = new CaseSummaryReturnModel();
                PaymentRateReturnModel oPaymentRateReturn = new PaymentRateReturnModel();
                PaymentRateRequestModel oPaymentRateRequest = new PaymentRateRequestModel();
                PaymentRateCalculations oPaymentCalculations = new PaymentRateCalculations();

                if (oCaseSummary.treatment_setting.Equals("Hospital"))
                {
                    var OppsByCPT = oOPPSRepo.GetOPPSCByCPT(caseCode.CaseCode1).FirstOrDefault();

                    if (OppsByCPT.Payment_Rate_ == 0 || OppsByCPT.Payment_Rate_ == null)
                    {
                        oCaseReturn.payment_rate = 0;
                    }
                    else
                    {
                        oCaseReturn.payment_rate = Convert.ToDouble(OppsByCPT.Payment_Rate_);
                    }

                    oCaseReturn.cpt_code = OppsByCPT.HCPCS_Code;
                    oCaseReturn.number_of_treatments = 0;
                    oCaseReturn.treatment_cost = oCaseReturn.payment_rate * oCaseReturn.number_of_treatments;
                    oCaseReturn.treatment_setting = oCaseSummary.treatment_setting;

                    lstCaseReturn.Add(oCaseReturn);
                }
                if (oCaseSummary.treatment_setting.Equals("Global"))
                {
                    oPaymentRateRequest.CPTCode = caseCode.CaseCode1;
                    oPaymentRateRequest.Locale = oCaseSummary.locality;
                    oPaymentRateRequest.PaymentRateScope = oCaseSummary.scope;

                    var GlobalCalculation = oPaymentCalculations.CalculateGlobalPaymentRate(oPaymentRateRequest);

                    oCaseReturn.cpt_code = GlobalCalculation.CPTCode;
                    oCaseReturn.number_of_treatments = 0;
                    oCaseReturn.treatment_setting = oCaseSummary.treatment_setting;
                    oCaseReturn.payment_rate = GlobalCalculation.PaymentRate;
                    oCaseReturn.treatment_cost = oCaseReturn.payment_rate * oCaseReturn.number_of_treatments;
                    lstCaseReturn.Add(oCaseReturn);
                }
                if (oCaseSummary.treatment_setting.Equals("Professional"))
                {
                    oPaymentRateRequest.CPTCode = caseCode.CaseCode1;
                    oPaymentRateRequest.Locale = oCaseSummary.locality;
                    oPaymentRateRequest.PaymentRateScope = oCaseSummary.scope;

                    var ProfessionalCalculation = oPaymentCalculations.CalculateProfessionalPaymentRate(oPaymentRateRequest);

                    oCaseReturn.cpt_code = ProfessionalCalculation.CPTCode;
                    oCaseReturn.number_of_treatments = 0;
                    oCaseReturn.treatment_setting = oCaseSummary.treatment_setting;
                    oCaseReturn.payment_rate = ProfessionalCalculation.PaymentRate;
                    oCaseReturn.treatment_cost = oCaseReturn.payment_rate * oCaseReturn.number_of_treatments;
                    lstCaseReturn.Add(oCaseReturn);
                }
                if (oCaseSummary.treatment_setting.Equals("Technical"))
                {
                    oPaymentRateRequest.CPTCode = caseCode.CaseCode1;
                    oPaymentRateRequest.Locale = oCaseSummary.locality;
                    oPaymentRateRequest.PaymentRateScope = oCaseSummary.scope;

                    var TechnicalCalculation = oPaymentCalculations.CalculateTechnicalPaymentRate(oPaymentRateRequest);

                    oCaseReturn.cpt_code = TechnicalCalculation.CPTCode;
                    oCaseReturn.number_of_treatments = 0;
                    oCaseReturn.treatment_setting = oCaseSummary.treatment_setting;
                    oCaseReturn.payment_rate = TechnicalCalculation.PaymentRate;
                    oCaseReturn.treatment_cost = oCaseReturn.payment_rate * oCaseReturn.number_of_treatments;
                    lstCaseReturn.Add(oCaseReturn);
                }
            }

            return lstCaseReturn;
        }
    }
}