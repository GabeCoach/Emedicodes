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
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class DiseaseSitesController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();

        // GET: api/DiseaseSites
        public IQueryable<DiseaseSite> GetDiseaseSites()
        {
            return db.DiseaseSites;
        }

        // GET: api/DiseaseSites/5
        [ResponseType(typeof(DiseaseSite))]
        public async Task<IHttpActionResult> GetDiseaseSite(int id)
        {
            DiseaseSite diseaseSite = await db.DiseaseSites.FindAsync(id);
            if (diseaseSite == null)
            {
                return NotFound();
            }

            return Ok(diseaseSite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiseaseSiteExists(int id)
        {
            return db.DiseaseSites.Count(e => e.DiseaseSiteID == id) > 0;
        }
    }
}