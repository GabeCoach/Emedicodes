using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository
{
    public class WageIndexRepository
    {
        DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        Logger oLogger = new Logger();

        public IQueryable<CMS_Wage_Index> GetGAFByGeoLocation(string geo_location)
        {
            try
            {
                var query = from w in db.CMS_Wage_Index
                            where w.Area_Name.Equals(geo_location)
                            select w;

                return (IQueryable<CMS_Wage_Index>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetGAFByGeoLocation; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }

        public IQueryable<CMS_Wage_Index> GetAllAreasByStateCode(string state_code)
        {
            try
            {
                var query = from w in db.CMS_Wage_Index
                            where w.State_Code.Equals(state_code)
                            select w;

                return (IQueryable<CMS_Wage_Index>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetAllAreasByStateCode; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }

        }

        public IQueryable<CMS_Wage_Index> GetWageIndexByState(string state)
        {
            try
            {
                var query = from w in db.CMS_Wage_Index
                            where w.Area_Name.Equals(state)
                            select w;

                return (IQueryable<CMS_Wage_Index>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: RetrieveMPFSWithPaging; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}