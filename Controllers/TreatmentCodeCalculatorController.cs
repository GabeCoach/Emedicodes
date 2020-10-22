using EmediCodesWebApplication.CalculationsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Logging;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    public class TreatmentCodeCalculatorController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private HospitalTreatmentCodeCalculation oHospTreatmentCalculation = new HospitalTreatmentCodeCalculation();
        private Logger oLogger = new Logger();

        [HttpGet]
        [Route("api/TreatmentCodeCalculator/{ModalityID}/HospitalTreatmentCodes")]
        public IHttpActionResult GetHospTreatmentCodesByModality(int ModalityID)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                var HospTreatmentCodes = oHospTreatmentCalculation.GetHospitalTreatmentCalculation(ModalityID);
                oLogger.LogData("ROUTE: api/TreatmentCodeCalculator/{ModalityID}/HospitalTreatmentCodes; METHOD: GET; IP_ADDRESS: " + sIPAddress);
                return Json(HospTreatmentCodes);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/TreatmentCodeCalculator/{ModalityID}/HospitalTreatmentCodes; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }
    }
}
