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
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.Logging;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class DiseasesController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private DiseasesRepository oDiseaseRepo = new DiseasesRepository();
        private Logger oLogger = new Logger();

        // GET: api/Diseases
        public IHttpActionResult Getdiseases()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/Diseases; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(oDiseaseRepo.GetAllDiseases());
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Diseases; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        [HttpPost]
        [Route("api/Diseases/PageRecords")]
        public IHttpActionResult GetDiseasesWithPaging(PagingModel oPager)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var SelectedPageDiseases = oDiseaseRepo.GetDiseasesWithPaging(oPager);
                oLogger.LogData("ROUTE: api/Diseases/PageRecords; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(SelectedPageDiseases);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Diseases/PageRecords; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/Diseases/{ICD10}/Search")]
        public IHttpActionResult SearchDiseases(string ICD10)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var SearchedDiseases = oDiseaseRepo.SearchDiseasesByICD10(ICD10);
                oLogger.LogData("ROUTE: api/Diseases/Search; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(SearchedDiseases);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Diseases/Search; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/Diseases/Count")]
        public IHttpActionResult GetTotalDiseasesCount()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/Diseases/Count; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Ok(db.diseases.Count());
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Diseases/Count; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool diseaseExists(string id)
        {
            return db.diseases.Count(e => e.ID == id) > 0;
        }
    }
}