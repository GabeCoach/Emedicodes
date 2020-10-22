using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace EmediCodesWebApplication.Repository
{
    public class DiseasesRepository
    {
        private Logger oLogger = new Logger();
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();

        public IQueryable<disease> GetAllDiseases()
        {
            try
            {
                var oDiseases = db.diseases;
                return oDiseases;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetAllDiseases; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public IQueryable<disease> GetDiseasesWithPaging(PagingModel oPager)
        {
            try
            {
                int page = oPager.page;
                int pageSize = oPager.pageSize;

                int skip = pageSize * (page - 1);

                var query = (from d in db.diseases
                             select d)
                            .OrderBy(d => d.CODE)
                            .Skip(skip)
                            .Take(pageSize);

                return (IQueryable<disease>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetDiseasesWithPaging; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public IQueryable<disease> SearchDiseasesByICD10(string ICD10)
        {
            try
            {
                IQueryable<disease> query;
                var searchQuery = Regex.Matches(ICD10, @"[a-zA-Z]");

                if(searchQuery.Count > 1)
                {
                    query = from d in db.diseases
                            where d.Name.Contains(ICD10)
                            select d;
                }
                else
                {
                    query = from d in db.diseases
                            where d.CODE.Contains(ICD10)
                            select d;
                }

                return (IQueryable<disease>)query;

            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: SearchDiseasesByICD10; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}