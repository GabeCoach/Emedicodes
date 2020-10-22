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
    public class CCIEditsController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();
        // GET: api/CCIEdits
        public IHttpActionResult GetCCI_Edits()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/CCIEdits; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(db.CCI_Edits);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/CCIEdits; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        // GET: api/CCIEdits/5
        [ResponseType(typeof(CCI_Edits))]
        public async Task<IHttpActionResult> GetCCI_Edits(int id)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                CCI_Edits cCI_Edits = await db.CCI_Edits.FindAsync(id);
                if (cCI_Edits == null)
                {
                    return NotFound();
                }

                oLogger.LogData("ROUTE: api/CCIEdits/:id; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Ok(cCI_Edits);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/CCIEdits/:id; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }


        }

        [HttpGet]
        [Route("api/CCIEdits/{CPTCode}/CPT")]
        public async Task<IHttpActionResult> GetCCIByCPT(string CPTCode)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var query = from c in db.CCI_Edits
                            where c.Column_1.Equals(CPTCode)
                            select c;

                oLogger.LogData("ROUTE: api/CCIEdits/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/CCIEdits/{CPTCode}/CPT; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool CCI_EditsExists(int id)
        {
            return db.CCI_Edits.Count(e => e.CCI_Edit_ID == id) > 0;
        }
    }
}