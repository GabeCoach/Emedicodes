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
    public class GPCIController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        // GET: api/GPCI
        public IHttpActionResult GetGPCI()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/GPCI; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(db.C2018_GPCI_Addendum_E);
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

        private bool GPCI2017Exists(int id)
        {
            return db.GPCI2017.Count(e => e.GPCI_ID == id) > 0;
        }
    }
}