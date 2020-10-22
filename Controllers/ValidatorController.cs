using EmediCodesWebApplication.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class ValidatorController : ApiController
    {
        private Logger oLogger = new Logger();

        [HttpGet]
        public IHttpActionResult Get()
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                oLogger.LogData("ROUTE: api/Validator; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Ok("User is authorized to use this API");
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Validator; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }
    }
}
