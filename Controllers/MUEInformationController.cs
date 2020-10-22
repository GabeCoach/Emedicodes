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
    public class MUEInformationController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        [HttpGet]
        [Route("api/MUEInformation/{CPTCode}/CPT")]
        public IHttpActionResult GetMUEByCPT(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var query = from m in db.MUE_Information
                            where m.HCPCS_CPT_Code.Equals(CPTCode)
                            select m;

                oLogger.LogData("ROUTE: api/MUEInformation/{CPTCode}/CPT/; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(query);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/GPCI; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool MUE_InformationExists(int id)
        {
            return db.MUE_Information.Count(e => e.MUE_ID == id) > 0;
        }
    }
}