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
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.Logging;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class ModalitiesController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private ModalityRepository oModalityRepo = new ModalityRepository();
        private Logger oLogger = new Logger();

        // GET: api/Modalities
        public IQueryable<Modality> GetModalities()
        {
            return db.Modalities;
        }

        // GET: api/Modalities/5
        [ResponseType(typeof(Modality))]
        public async Task<IHttpActionResult> GetModality(int id)
        {
            Modality modality = await db.Modalities.FindAsync(id);
            if (modality == null)
            {
                return NotFound();
            }

            return Ok(modality);
        }

        [HttpGet]
        [Route("api/Modalities/{DiseaseSite}/DiseaseSiteApplicableModalities")]
        public IHttpActionResult GetModalityByDiseaseSite(string DiseaseSite)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                List<Modality> lstApplicableModalities = oModalityRepo.GetModalityByChosenDiseaseSite(DiseaseSite);
                oLogger.LogData("ROUTE: api/Modalities/{DiseaseSite}/DiseaseSiteApplicableModalities; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(lstApplicableModalities);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Modalities/{DiseaseSite}/DiseaseSiteApplicableModalities; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
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

        private bool ModalityExists(int id)
        {
            return db.Modalities.Count(e => e.ModalityID == id) > 0;
        }
    }
}