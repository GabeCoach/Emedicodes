using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository
{
    public class GPCIRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        public IQueryable<C2018_GPCI_Addendum_E> RetrieveGPCIByLocale(string Locale)
        {
            try
            {
                var query = from g in db.C2018_GPCI_Addendum_E
                            where g.LocalName.Equals(Locale)
                            select g;

                return (IQueryable<C2018_GPCI_Addendum_E>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: RetrieveGPCIByLocale; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}