using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EmediCodesWebApplication.Repository
{
    public class CaseCodesRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();


        public async Task<string> SaveCaseCodesToDB(List<CaseCode> lstCaseCode, int CaseID)
        {
            try
            {
                foreach (var code in lstCaseCode)
                {
                    code.CaseID = CaseID;
                    db.CaseCodes.Add(code);
                    await db.SaveChangesAsync();
                }

                return "CaseCodes Saved To DB";
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: SaveCaseCodesToDB; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<List<CaseCode>> GetCaseCaseCodesByCaseID(int CaseID)
        {
            try
            {
                var CaseCodesByCase = await db.CaseCodes.Where(cc => cc.CaseID == CaseID).ToListAsync<CaseCode>();
                return (List<CaseCode>)CaseCodesByCase;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetCaseCaseCodesByCaseID; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}