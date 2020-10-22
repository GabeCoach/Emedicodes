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
using EmediCodesWebApplication.Repository.Models;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/Case")]
    public class CaseController : ApiController
    {
        private CaseRepository oCaseRepo = new CaseRepository();
        private CaseCodesRepository oCaseCodeRepo = new CaseCodesRepository();
        private Logger oLogger = new Logger();

        [HttpGet]
        [Route("{UserId}/UserCases")]
        public IHttpActionResult GetCasesByUser(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var UserCases = oCaseRepo.GetCasesByUser(UserId);
                oLogger.LogData("ROUTE: api/Case/{UserId}/UserCases; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(UserCases);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE:  api/Case/{UserId}/UserCases; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostCase(Case oCase)
        {
            if (!ModelState.IsValid)
            {
                return InternalServerError();
            }

            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var Case = await oCaseRepo.CreateCase(oCase);
                oLogger.LogData("ROUTE: api/Case; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                var Return = new
                {
                    CaseID = Case.CaseID
                };
                return Json(Return);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE:  api/Case; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{CaseId}")]
        public async Task<IHttpActionResult> DeleteCase(int CaseId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                Case oCase = await oCaseRepo.DeleteCase(CaseId);
                oLogger.LogData("ROUTE: api/Case; METHOD: DELETE; IP_ADDRESS: " + sIPAddress);
                return Json(oCase);
            }
            catch(Exception ex)
            {
                oLogger.LogData("ROUTE: api/Case; METHOD: DELETE; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("CaseCodes/{CaseID}")]
        public async Task<IHttpActionResult> PostCaseCodes(List<CaseCode> lstCaseCodes, int CaseID)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                if (!lstCaseCodes.Any())
                {
                    return BadRequest();
                }

                string sResult = await oCaseCodeRepo.SaveCaseCodesToDB(lstCaseCodes, CaseID);

                if (!String.IsNullOrEmpty(sResult))
                {
                    oLogger.LogData("ROUTE: api/Case/CaseCodes/{CaseID}; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                    return Ok(201);
                }
                else
                {
                    oLogger.LogData("ROUTE: api/Case/CaseCodes/{CaseID}; METHOD: POST; IP_ADDRESS: " + sIPAddress + ": ERROR: CASECODES NOT SAVES SUCCESSFULLY");
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE:  api/Case/CaseCodes/{CaseID}; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/CaseList")]
        public IHttpActionResult GetUserCaseList(int UserID)
        {

            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var UserCases = oCaseRepo.GetCasesByUser(UserID);
                return Json(UserCases);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE:  api/Case/{UserId}/CaseList; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("{CaseId}/CaseSummary")]
        public async Task<IHttpActionResult> GetCaseSummary(int CaseId, CaseSummaryRequestModel oCaseRequest)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var CaseCodesByID = await oCaseCodeRepo.GetCaseCaseCodesByCaseID(CaseId);
                var lstCaseSummary = oCaseRepo.GetCaseSummary(CaseCodesByID, oCaseRequest);
                return Json(lstCaseSummary);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE:  api/Case/{CaseId}/CaseSummary; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }
    }
}