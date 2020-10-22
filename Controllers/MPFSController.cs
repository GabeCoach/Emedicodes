using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.CalculationModels;
using EmediCodesWebApplication.Repository.Models;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.CalculationsLayer;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class MPFSController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private RVUCalculations oRVUCalc = new RVUCalculations();
        private PaymentRateCalculations oPaymentCalc = new PaymentRateCalculations();
        private MPFSRepository oMPFSRepo = new MPFSRepository();
        private Logger oLogger = new Logger();

        // GET: api/MPFS
        [Route("api/MPFS", Name = "CPTList")]
        public IHttpActionResult GetMPFS_Addendum_B()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/MPFS; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(db.MPFS_Addendum_B);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpGet]
        [Route("api/MPFS/{CPTCode}/Search")]
        public IHttpActionResult SearchMPFSByCPT(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var SearchedCPT = oMPFSRepo.SearchMPFSByCPT(CPTCode);
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/Search; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(SearchedCPT);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/Search; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/MPFS/PageRecords")]
        public IHttpActionResult GetMPFSWithPaging(PagingModel oPager)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var SelectedPageMPFS = oMPFSRepo.RetrieveMPFSWithPaging(oPager);
                oLogger.LogData("ROUTE: api/MPFS/PageRecords; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(SelectedPageMPFS);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/PageRecords; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpGet]
        [Route("api/MPFS/Count")]
        public IHttpActionResult GetTotalMPFSCount()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/MPFS/Count; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Ok(db.C2018_MPFS_Addendum_B.Count());
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/Count; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        // GET: api/MPFS/5
        [ResponseType(typeof(MPFS_Addendum_B))]
        public async Task<IHttpActionResult> GetMPFS_Addendum_B(int id)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                MPFS_Addendum_B mPFS_Addendum_B = await db.MPFS_Addendum_B.FindAsync(id);
                if (mPFS_Addendum_B == null)
                {
                    oLogger.LogData("MPFS Item Not Found");
                    return NotFound();
                }

                oLogger.LogData("ROUTE: api/MPFS/:id; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Ok(mPFS_Addendum_B);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpGet]
        [Route("api/MPFS/{CPTCode}/GlobalRVUCalculation")]
        public IHttpActionResult GlobalRVUCalculation(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                Decimal GlobalRVU = oRVUCalc.CalculateGlobalRVU(CPTCode);

                var returnObject = new
                {
                    GlobalRVU = GlobalRVU,
                    Mod = "GL",
                    CPTCode = CPTCode
                };
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/GlobalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(returnObject);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/GlobalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        [HttpGet]
        [Route("api/MPFS/{CPTCode}/TechnicalRVUCalculation")]
        public IHttpActionResult GetNatTechnicalRVUCalculation(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                Decimal TechnicalRVU = oRVUCalc.CalculateTechnicalRVU(CPTCode);

                var returnObject = new
                {
                    TechnicalRVU = TechnicalRVU,
                    Mod = "TC",
                    CPTCode = CPTCode
                };

                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/TechnicalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(returnObject);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/TechnicalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        [HttpGet]
        [Route("api/MPFS/{CPTCode}/ProfessionalRVUCalculation")]
        public IHttpActionResult ProfessionalRVUCalculation(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                Decimal ProfessionalRVU = oRVUCalc.CalculateProfessionalRVU(CPTCode);

                var returnObject = new
                {
                    ProfessionalRVU = ProfessionalRVU,
                    Mod = "26",
                    CPTCode = CPTCode
                };

                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/ProfessionalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(returnObject);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/ProfessionalRVUCalculation; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        [HttpGet]
        [Route("api/MPFS/{CPTCode}/CPT")]
        public IHttpActionResult GetMPFSByCPT(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var query = from c in db.MPFS_Addendum_B
                            where c.CPT1_HCPCS.Equals(CPTCode)
                            select c;

                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(query);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        [HttpPost]
        [Route("api/MPFS/GlobalPaymentRate")]
        public IHttpActionResult GlobalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                PaymentRateReturnModel oPaymentRateReturn = oPaymentCalc.CalculateGlobalPaymentRate(oPaymentRate);

                oLogger.LogData("ROUTE: api/MPFS/GlobalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress);

                return Json<PaymentRateReturnModel>(oPaymentRateReturn);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/GlobalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpPost]
        [Route("api/MPFS/TechnicalPaymentRate")]
        public IHttpActionResult TechnicalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                PaymentRateReturnModel oPaymentRateReturn = oPaymentCalc.CalculateTechnicalPaymentRate(oPaymentRate);

                oLogger.LogData("ROUTE: api/MPFS/TechnicalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress);

                return Json<PaymentRateReturnModel>(oPaymentRateReturn);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/TechnicalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpPost]
        [Route("api/MPFS/ProfessionalPaymentRate")]
        public IHttpActionResult ProfessionalPaymentRate(PaymentRateRequestModel oPaymentRate)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                PaymentRateReturnModel oPaymentRateReturn = oPaymentCalc.CalculateProfessionalPaymentRate(oPaymentRate);

                oLogger.LogData("ROUTE: api/MPFS/ProfessionalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress);

                return Json<PaymentRateReturnModel>(oPaymentRateReturn);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/MPFS/ProfessionalPaymentRate; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MPFS_Addendum_BExists(int id)
        {
            return db.MPFS_Addendum_B.Count(e => e.MPFS_ID == id) > 0;
        }
    }
}