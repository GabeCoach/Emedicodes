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
using EmediCodesWebApplication.Logging;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class OPPSController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();


        [HttpGet]
        [Route("api/OPPS/{CPTCode}/CPT")]
        public IHttpActionResult GetOPPSByCPT(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var query = from o in db.OPPS_Addendum_B
                            where o.HCPCS_Code.Equals(CPTCode)
                            select o;

                var rtnObject = query.FirstOrDefault();

                oLogger.LogData("ROUTE: api/OPPS/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(rtnObject);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/OPPS/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool OPPS_Addendum_BExists(string id)
        {
            return db.OPPS_Addendum_B.Count(e => e.HCPCS_Code == id) > 0;
        }
    }
}