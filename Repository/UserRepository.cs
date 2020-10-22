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
    public class UserRepository
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Logger oLogger = new Logger();

        public async Task<User> SaveUser(User user)
        {
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: SaveUser; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<User> GetUser(int UserId)
        {
            try
            {
                User oUser = await db.Users.FindAsync(UserId);
                return oUser;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: GetUser; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public string GetUserEmail(int UserId)
        {
            try
            {
                var email = from u in db.Users
                            where u.Id == UserId
                            select u.Email;

                var returnEmail = email.FirstOrDefault().ToString();

                return returnEmail;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetUserEmail; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }


        }

        public string ValidateEmailExists(string sEmail)
        {
            try
            {
                var query = (from u in db.Users
                             where u.Email.Equals(sEmail)
                             select u.Email).FirstOrDefault();

                return query;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: ValidateEmailExists; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }


        }

        public string GetStripeCustomerId(int UserId)
        {
            try
            {
                var stripeCustomerId = (from u in db.Users
                                        where u.Id == UserId
                                        select u.StripeIdentifier).FirstOrDefault();

                return stripeCustomerId;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetStripeCustomerId; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public void UpdateUserWithStripeID(string stripeCustomerID, int UserId)
        {
            try
            {
                User user = (from u in db.Users
                             where u.Id == UserId
                             select u).FirstOrDefault();

                user.StripeIdentifier = stripeCustomerID;

                db.Entry(user).State = EntityState.Modified;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: UpdateUserWithStripeID; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<User> UpdateUserAfterRegistration(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: UpdateUser; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<User> GetUserProfileInfo(string Auth0ID)
        {
            try
            {
                var query = from u in db.Users
                            where u.Auth0Identifier.Equals(Auth0ID)
                            select u;

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: GetUserProfileInfo; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async Task<User> GetUserByEmail(string sEmail)
        {
            try
            {
                var query = from u in db.Users
                            where u.Email.Equals(sEmail)
                            select u;

                return await query.FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: GetUserByEmail; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public async void DeleteUser(string sEmail)
        {
            try
            {
                var user = await (from u in db.Users
                           where u.Email.Equals(sEmail)
                           select u).FirstOrDefaultAsync();

                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: DeleteUserByEmail; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}