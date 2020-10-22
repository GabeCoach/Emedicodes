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
using EmediCodesWebApplication.CalculationsLayer;
using EmediCodesWebApplication.Logging;
using System.Web.Http.Cors;
using EmediCodesWebApplication.CalculationModels;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class WageIndexController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private WageIndexCalculations oWageIndexCalculations = new WageIndexCalculations();
        private Logger oLogger = new Logger();

        // GET: api/WageIndex
        public IQueryable<CMS_Wage_Index> GetCMS_Wage_Index()
        {
            return db.CMS_Wage_Index;
        }

        [HttpPost]
        [Route("api/WageIndex/Calculate")]
        public IHttpActionResult CalculateWageIndex(WageIndexRequestModel oWageIndexReg)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var oReturn = oWageIndexCalculations.CalculateWageIndexAdjustedPayment(oWageIndexReg);
                oLogger.LogData("ROUTE: api/WageIndex/Calculate; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(oReturn);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/WageIndex/Calculate; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool CMS_Wage_IndexExists(int id)
        {
            return db.CMS_Wage_Index.Count(e => e.Id == id) > 0;
        }
    }
}