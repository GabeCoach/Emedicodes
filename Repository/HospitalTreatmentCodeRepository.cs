using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository
{
    public class HospitalTreatmentCodeRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        public IQueryable<HospitalTreatmentCode> GetHospTreatmentCodesByModality(int ModalityID)
        {
            try
            {
                var query = from h in db.HospitalTreatmentCodes
                            where h.ModalityID == ModalityID
                            select h;

                return (IQueryable<HospitalTreatmentCode>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetHospTreatmentCodesByModality; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}