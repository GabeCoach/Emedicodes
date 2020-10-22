using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.RegistrationLayer
{
    public class UserRegistration
    {
        private Logger oLogger = new Logger();

        public User CheckUserRegistration(UserRegistrationRequestModel oUserRegistrationRequest)
        {
            try
            {
                if (oUserRegistrationRequest.first_name.Length < 3 || oUserRegistrationRequest.first_name.Length > 16)
                    throw new Exception("First name is too short or too long");
                if (oUserRegistrationRequest.last_name.Length < 3 || oUserRegistrationRequest.last_name.Length > 16)
                    throw new Exception("Last name is too short or too long");
                if (oUserRegistrationRequest.password != oUserRegistrationRequest.password_confirm)
                    throw new Exception("Passwords must match");

                User oUser = new User();

                oUser.FirstName = oUserRegistrationRequest.first_name;
                oUser.LastName = oUserRegistrationRequest.last_name;
                oUser.PhoneNumber = oUserRegistrationRequest.phone_number;
                oUser.Email = oUserRegistrationRequest.email_address;

                return oUser;

            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CheckUserRegistration; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}