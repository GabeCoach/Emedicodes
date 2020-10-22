using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository
{
    public class ModalityRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        public List<Modality> GetModalityByChosenDiseaseSite(string DiseaseSite)
        {
            var oDiseaseSite = GetDiseaseSite(DiseaseSite).FirstOrDefault();
            var oApplicableModalities = GetApplicableModalities(oDiseaseSite);
            List<Modality> lstApplicableModalities = new List<Modality>();

            try
            {
                foreach (var applicableModality in oApplicableModalities)
                {
                    var query = (from m in db.Modalities
                                 where m.ModalityID == applicableModality.ModalityID
                                 select m).FirstOrDefault();

                    lstApplicableModalities.Add(query);
                }

                return (List<Modality>)lstApplicableModalities;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetModalityByChosenDiseaseSite; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public IQueryable<Modality> GetModalityByID(int ModalityID)
        {
            try
            {
                var query = from m in db.Modalities
                            where m.ModalityID == ModalityID
                            select m;

                return (IQueryable<Modality>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetModalityByID; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        private IQueryable<DiseaseSite> GetDiseaseSite(string DiseaseSite)
        {
            try
            {
                var query = from d in db.DiseaseSites
                            where d.DiseaseSiteName.Equals(DiseaseSite)
                            select d;

                return (IQueryable<DiseaseSite>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetDiseaseSite; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        private IQueryable<ApplicableDiseaseSiteModality> GetApplicableModalities(DiseaseSite oDiseaseSite)
        {
            try
            {
                var query = from a in db.ApplicableDiseaseSiteModalities
                            where a.DiseaseSiteID == oDiseaseSite.DiseaseSiteID
                            select a;

                return (IQueryable<ApplicableDiseaseSiteModality>)query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetApplicableModalities; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}