using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationsLayer
{
    public class RVUCalculations
    {
        private const Double ConversionFactor = 35.9996;

        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private MPFSRepository oMPFSRepo = new MPFSRepository();
        private GPCIRepository oGPCIRepo = new GPCIRepository();
        private Logger oLogger = new Logger();

        public Decimal CalculateGlobalRVU(string cptCode)
        {
            try
            {
                var GlobalCPT = oMPFSRepo.RetrieveMPFSByGlobalCPT(cptCode).Where(c => c.Mod.Equals("")).FirstOrDefault();

                CalculateGlobalRVUGateKeeper(GlobalCPT);

                var GlobalRVU = Convert.ToDecimal(GlobalCPT.Work_RVUs2) + Convert.ToDecimal(GlobalCPT.Non_Facility_PE_RVUs2) + Convert.ToDecimal(GlobalCPT.Mal_Practice_RVUs2);

                return GlobalRVU;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CalculateGlobalRVUGateKeeper; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public Decimal CalculateTechnicalRVU(string cptCode)
        {

            try
            {
                var CPT = oMPFSRepo.RetrieveMPFSByGlobalCPT(cptCode);
                Decimal TechnicalRVU = 0;

                if (CPT.Count() == 1)
                {
                    var GlobalOnlyCPT = CPT.FirstOrDefault();

                    TechnicalRVU = Convert.ToDecimal(GlobalOnlyCPT.Work_RVUs2) + Convert.ToDecimal(GlobalOnlyCPT.Non_Facility_PE_RVUs2) + Convert.ToDecimal(GlobalOnlyCPT.Mal_Practice_RVUs2);
                    return TechnicalRVU;
                }
                else
                {
                    var TechnicalCPT = CPT.Where(c => c.Mod.Equals("TC")).FirstOrDefault();

                    if (TechnicalCPT == null)
                    {
                        return TechnicalRVU;
                    }
                    else
                    {
                        TechnicalRVU = Convert.ToDecimal(TechnicalCPT.Work_RVUs2) + Convert.ToDecimal(TechnicalCPT.Non_Facility_PE_RVUs2) + Convert.ToDecimal(TechnicalCPT.Mal_Practice_RVUs2);
                        return TechnicalRVU;
                    }
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CalculateGlobalRVUGateKeeper; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }

        public Decimal CalculateProfessionalRVU(string cptCode)
        {
            try
            {
                var CPT = oMPFSRepo.RetrieveMPFSByGlobalCPT(cptCode);
                Decimal ProfessionalRVU = 0;

                if (CPT.Count() == 1)
                {
                    var GlobalOnlyCPT = CPT.FirstOrDefault();

                    ProfessionalRVU = Convert.ToDecimal(GlobalOnlyCPT.Work_RVUs2) + Convert.ToDecimal(GlobalOnlyCPT.Facility_PE_RVUs2) + Convert.ToDecimal(GlobalOnlyCPT.Mal_Practice_RVUs2);
                    return ProfessionalRVU;
                }
                else
                {
                    var ProfessionalCPT = CPT.Where(c => c.Mod.Equals("26")).FirstOrDefault();

                    if (ProfessionalCPT == null)
                    {
                        return ProfessionalRVU;
                    }
                    else
                    {
                        ProfessionalRVU = Convert.ToDecimal(ProfessionalCPT.Work_RVUs2) + Convert.ToDecimal(ProfessionalCPT.Facility_PE_RVUs2) + Convert.ToDecimal(ProfessionalCPT.Mal_Practice_RVUs2);
                        return ProfessionalRVU;
                    }
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CalculateGlobalRVUGateKeeper; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }

        private void CalculateGlobalRVUGateKeeper(C2018_MPFS_Addendum_B GlobalCPT)
        {
            try
            {
                if (GlobalCPT.Work_RVUs2 == "NA" || GlobalCPT.Work_RVUs2 == "")
                {
                    GlobalCPT.Work_RVUs2 = "0";
                }
                if (GlobalCPT.Non_Facility_PE_RVUs2 == "NA" || GlobalCPT.Non_Facility_PE_RVUs2 == "")
                {
                    GlobalCPT.Non_Facility_PE_RVUs2 = "0";
                }
                if (GlobalCPT.Mal_Practice_RVUs2 == "NA" || GlobalCPT.Mal_Practice_RVUs2 == "")
                {
                    GlobalCPT.Mal_Practice_RVUs2 = "0";
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CalculateGlobalRVUGateKeeper; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }
    }
}