using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Logging;

namespace EmediCodesWebApplication.Repository
{
    public class OPPSRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        public IQueryable<C2018_OPPS_Addendum_B> GetOPPSCByCPT(string CPTCode)
        {
            try
            {
                var query = from o in db.C2018_OPPS_Addendum_B
                            where o.HCPCS_Code.Equals(CPTCode)
                            select o;

                return (IQueryable<C2018_OPPS_Addendum_B>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: RetrieveMPFSWithPaging; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}