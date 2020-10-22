using EmediCodesWebApplication.CalculationModels;
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.CalculationsLayer
{
    public class PaymentRateCalculations
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private MPFSRepository oMPFSRepo = new MPFSRepository();
        private GPCIRepository oGPCIRepo = new GPCIRepository();
        private RVUCalculations oRVU = new RVUCalculations();
        private const Double ConversionFactor = 35.9996;

        public PaymentRateReturnModel CalculateGlobalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            const string CPTMod = "";

            if (oPaymentRate.PaymentRateScope.Equals("National"))
            {
                var GlobalNationalPaymentRate = Convert.ToDouble(oRVU.CalculateGlobalRVU(oPaymentRate.CPTCode)) * ConversionFactor;

                return new PaymentRateReturnModel()
                {
                    Scope = oPaymentRate.PaymentRateScope,
                    Locale = oPaymentRate.Locale,
                    PaymentRate = GlobalNationalPaymentRate,
                    CPTCode = oPaymentRate.CPTCode,
                    CPTMod = CPTMod
                };
            }
            else
            {
                var oLocaleGPCI = oGPCIRepo.RetrieveGPCIByLocale(oPaymentRate.Locale).FirstOrDefault();
                var oGlobalCPTCode = oMPFSRepo.RetrieveMPFSByGlobalCPT(oPaymentRate.CPTCode).Where(c => c.Mod.Equals(CPTMod)).FirstOrDefault();
                var oValidCPT = ValidateCPT(oGlobalCPTCode);

                var GlobalRegionalPaymentRate = (
                    (Convert.ToDouble(oLocaleGPCI.PW_GPCI) * Convert.ToDouble(oValidCPT.Work_RVUs2)) +
                    (Convert.ToDouble(oLocaleGPCI.PE_GPCI) * Convert.ToDouble(oValidCPT.Non_Facility_PE_RVUs2)) +
                    (Convert.ToDouble(oLocaleGPCI.MP_GPCI) * Convert.ToDouble(oValidCPT.Mal_Practice_RVUs2))
                    ) * ConversionFactor;

                return new PaymentRateReturnModel()
                {
                    Scope = oPaymentRate.PaymentRateScope,
                    Locale = oPaymentRate.Locale,
                    PaymentRate = GlobalRegionalPaymentRate,
                    CPTCode = oPaymentRate.CPTCode,
                    CPTMod = CPTMod
                };
            }
        }

        private static C2018_MPFS_Addendum_B ValidateCPT(C2018_MPFS_Addendum_B oGlobalCPTCode)
        {
            if (oGlobalCPTCode.Work_RVUs2 == "NA" || oGlobalCPTCode.Work_RVUs2 == "")
            {
                oGlobalCPTCode.Work_RVUs2 = "0";
            }
            if (oGlobalCPTCode.Non_Facility_PE_RVUs2 == "NA" || oGlobalCPTCode.Non_Facility_PE_RVUs2 == "")
            {
                oGlobalCPTCode.Non_Facility_PE_RVUs2 = "0";
            }
            if (oGlobalCPTCode.Mal_Practice_RVUs2 == "NA" || oGlobalCPTCode.Mal_Practice_RVUs2 == "")
            {
                oGlobalCPTCode.Mal_Practice_RVUs2 = "0";
            }

            return oGlobalCPTCode;
        }

        public PaymentRateReturnModel CalculateTechnicalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            const string CPTMod = "TC";
            Double TechnicalRegionalPaymentRate = 0;
            Double TechnicalNationalPaymentRate = 0;

            if (oPaymentRate.PaymentRateScope.Equals("National"))
            {
                TechnicalNationalPaymentRate = Convert.ToDouble(oRVU.CalculateTechnicalRVU(oPaymentRate.CPTCode)) * ConversionFactor;

                return new PaymentRateReturnModel()
                {
                    Scope = oPaymentRate.PaymentRateScope,
                    Locale = oPaymentRate.Locale,
                    PaymentRate = TechnicalNationalPaymentRate,
                    CPTCode = oPaymentRate.CPTCode,
                    CPTMod = CPTMod
                };
            }
            else
            {
                var oLocaleGPCI = oGPCIRepo.RetrieveGPCIByLocale(oPaymentRate.Locale).FirstOrDefault();
                var oGlobalCPTCode = oMPFSRepo.RetrieveMPFSByGlobalCPT(oPaymentRate.CPTCode);

                if (!oGlobalCPTCode.Any() || oGlobalCPTCode == null)
                {
                    return new PaymentRateReturnModel()
                    {
                        Scope = oPaymentRate.PaymentRateScope,
                        Locale = oPaymentRate.Locale,
                        PaymentRate = TechnicalRegionalPaymentRate,
                        CPTCode = oPaymentRate.CPTCode,
                        CPTMod = CPTMod,
                        HasError = true,
                        ErrorMessage = "Global CPT has a null error."

                    };
                }

                if (oGlobalCPTCode.Count() == 1)
                {
                    if (oGlobalCPTCode.FirstOrDefault().Work_RVUs2 == "NA" || oGlobalCPTCode.FirstOrDefault().Work_RVUs2 == "" || oGlobalCPTCode.FirstOrDefault().Work_RVUs2 == "0")
                    {
                        var TechnicalCPT = oGlobalCPTCode.FirstOrDefault();

                        TechnicalRegionalPaymentRate = (
                           (Convert.ToDouble(oLocaleGPCI.PW_GPCI) * Convert.ToDouble(TechnicalCPT.Work_RVUs2)) +
                           (Convert.ToDouble(oLocaleGPCI.PE_GPCI) * Convert.ToDouble(TechnicalCPT.Non_Facility_PE_RVUs2)) +
                           (Convert.ToDouble(oLocaleGPCI.MP_GPCI) * Convert.ToDouble(TechnicalCPT.Mal_Practice_RVUs2))
                           ) * ConversionFactor;

                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = TechnicalRegionalPaymentRate,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }
                    else
                    {
                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = TechnicalRegionalPaymentRate,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }
                }
                else
                {
                    if (!oGlobalCPTCode.Any() || oGlobalCPTCode == null)
                    {
                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = 0,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }

                    var TechnicalCPT = oGlobalCPTCode.Where(c => c.Mod.Equals(CPTMod)).FirstOrDefault();

                    if (TechnicalCPT.Work_RVUs2 == "NA" || TechnicalCPT.Work_RVUs2 == "")
                    {
                        TechnicalCPT.Work_RVUs2 = "0";
                    }
                    if (TechnicalCPT.Non_Facility_PE_RVUs2 == "NA" || TechnicalCPT.Non_Facility_PE_RVUs2 == "")
                    {
                        TechnicalCPT.Non_Facility_PE_RVUs2 = "0";
                    }
                    if (TechnicalCPT.Mal_Practice_RVUs2 == "NA" || TechnicalCPT.Mal_Practice_RVUs2 == "")
                    {
                        TechnicalCPT.Mal_Practice_RVUs2 = "0";
                    }

                    TechnicalRegionalPaymentRate = (
                       (Convert.ToDouble(oLocaleGPCI.PW_GPCI) * Convert.ToDouble(TechnicalCPT.Work_RVUs2)) +
                       (Convert.ToDouble(oLocaleGPCI.PE_GPCI) * Convert.ToDouble(TechnicalCPT.Non_Facility_PE_RVUs2)) +
                       (Convert.ToDouble(oLocaleGPCI.MP_GPCI) * Convert.ToDouble(TechnicalCPT.Mal_Practice_RVUs2))
                       ) * ConversionFactor;

                    return new PaymentRateReturnModel()
                    {
                        Scope = oPaymentRate.PaymentRateScope,
                        Locale = oPaymentRate.Locale,
                        PaymentRate = TechnicalRegionalPaymentRate,
                        CPTCode = oPaymentRate.CPTCode,
                        CPTMod = CPTMod
                    };
                }


            }
        }

        public PaymentRateReturnModel CalculateProfessionalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            const string CPTMod = "26";
            Double ProfessionalNationalPaymentRate = 0;
            Double ProfessionalRegionalPaymentRate = 0;

            if (oPaymentRate.PaymentRateScope.Equals("National"))
            {
                ProfessionalNationalPaymentRate = Convert.ToDouble(oRVU.CalculateProfessionalRVU(oPaymentRate.CPTCode)) * ConversionFactor;

                return new PaymentRateReturnModel()
                {
                    Scope = oPaymentRate.PaymentRateScope,
                    Locale = oPaymentRate.Locale,
                    PaymentRate = ProfessionalNationalPaymentRate,
                    CPTCode = oPaymentRate.CPTCode,
                    CPTMod = CPTMod
                };
            }
            else
            {
                var oLocaleGPCI = oGPCIRepo.RetrieveGPCIByLocale(oPaymentRate.Locale).FirstOrDefault();
                var oGlobalCPTCode = oMPFSRepo.RetrieveMPFSByGlobalCPT(oPaymentRate.CPTCode);

                if (oGlobalCPTCode.Count() == 1)
                {
                    if (oGlobalCPTCode.FirstOrDefault().Work_RVUs2 != "NA" || oGlobalCPTCode.FirstOrDefault().Work_RVUs2 != "")
                    {
                        var ProfessionalCPT = oGlobalCPTCode.FirstOrDefault();

                        ProfessionalRegionalPaymentRate = (
                           (Convert.ToDouble(oLocaleGPCI.PW_GPCI) * Convert.ToDouble(ProfessionalCPT.Work_RVUs2)) +
                           (Convert.ToDouble(oLocaleGPCI.PE_GPCI) * Convert.ToDouble(ProfessionalCPT.Facility_PE_RVUs2)) +
                           (Convert.ToDouble(oLocaleGPCI.MP_GPCI) * Convert.ToDouble(ProfessionalCPT.Mal_Practice_RVUs2))
                           ) * ConversionFactor;

                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = ProfessionalRegionalPaymentRate,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }
                    else
                    {
                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = ProfessionalRegionalPaymentRate,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }
                }
                else
                {
                    var ProfessionalCPT = oGlobalCPTCode.Where(c => c.Mod.Equals(CPTMod)).FirstOrDefault();

                    if (oGlobalCPTCode == null)
                    {
                        return new PaymentRateReturnModel()
                        {
                            Scope = oPaymentRate.PaymentRateScope,
                            Locale = oPaymentRate.Locale,
                            PaymentRate = 0,
                            CPTCode = oPaymentRate.CPTCode,
                            CPTMod = CPTMod
                        };
                    }

                    if (ProfessionalCPT.Work_RVUs2 == "NA" || ProfessionalCPT.Work_RVUs2 == "")
                    {
                        ProfessionalCPT.Work_RVUs2 = "0";
                    }
                    if (ProfessionalCPT.Non_Facility_PE_RVUs2 == "NA" || ProfessionalCPT.Non_Facility_PE_RVUs2 == "")
                    {
                        ProfessionalCPT.Non_Facility_PE_RVUs2 = "0";
                    }
                    if (ProfessionalCPT.Mal_Practice_RVUs2 == "NA" || ProfessionalCPT.Mal_Practice_RVUs2 == "")
                    {
                        ProfessionalCPT.Mal_Practice_RVUs2 = "0";
                    }

                    ProfessionalRegionalPaymentRate = (
                        (Convert.ToDouble(oLocaleGPCI.PW_GPCI) * Convert.ToDouble(ProfessionalCPT.Work_RVUs2)) +
                        (Convert.ToDouble(oLocaleGPCI.PE_GPCI) * Convert.ToDouble(ProfessionalCPT.Facility_PE_RVUs2)) +
                        (Convert.ToDouble(oLocaleGPCI.MP_GPCI) * Convert.ToDouble(ProfessionalCPT.Mal_Practice_RVUs2))
                        ) * ConversionFactor;

                    return new PaymentRateReturnModel()
                    {
                        Scope = oPaymentRate.PaymentRateScope,
                        Locale = oPaymentRate.Locale,
                        PaymentRate = ProfessionalRegionalPaymentRate,
                        CPTCode = oPaymentRate.CPTCode,
                        CPTMod = CPTMod
                    };
                }
            }
        }
    }
}