using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository.Models;
using System.Text.RegularExpressions;

namespace EmediCodesWebApplication.Repository
{
    public class MPFSRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        Logger oLogger = new Logger();

        public IQueryable<C2018_MPFS_Addendum_B> RetrieveMPFSByGlobalCPT(string CPTCode)
        {
            try
            {
                var query = from m in db.C2018_MPFS_Addendum_B
                            where m.CPT1__HCPCS.Equals(CPTCode)
                            select m;

                return (IQueryable<C2018_MPFS_Addendum_B>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: RetrieveMPFSByGlobalCPT; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public IQueryable<C2018_MPFS_Addendum_B> SearchMPFSByCPT(string CPTCode)
        {
            try
            {
                IQueryable<C2018_MPFS_Addendum_B> query;
                var searchQuery = Regex.Matches(CPTCode, @"[a-zA-Z]");

                if (searchQuery.Count != 0)
                {
                    query = from m in db.C2018_MPFS_Addendum_B
                            where m.Description.Contains(CPTCode)
                            select m;
                }
                else
                {
                    query = from m in db.C2018_MPFS_Addendum_B
                            where m.CPT1__HCPCS.Contains(CPTCode)
                            select m;
                }

                return (IQueryable<C2018_MPFS_Addendum_B>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: SearchMPFSByCPT; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public IQueryable<C2018_MPFS_Addendum_B> RetrieveMPFSWithPaging(PagingModel oPager)
        {
            try
            {
                int page = oPager.page;
                int pageSize = oPager.pageSize;

                int skip = pageSize * (page - 1);

                var query = (from m in db.C2018_MPFS_Addendum_B
                             select m)
                            .OrderBy(m => m.CPT1__HCPCS)
                            .Skip(skip)
                            .Take(pageSize);


                return (IQueryable<C2018_MPFS_Addendum_B>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: RetrieveMPFSWithPaging; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}