using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.RegistrationLayer;
using static EmediCodesWebApplication.Auth0Connector.Models.Auth0TokenModel;
using Newtonsoft.Json;
using EmediCodesWebApplication.Auth0Connector;
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.StripeConnector;

namespace EmediCodesWebApplication.Controllers
{
    public class UsersController : ApiController
    {
        private DB_A3003E_emedicodesEntities db = new DB_A3003E_emedicodesEntities();
        private Auth0Users oAuth0Users = new Auth0Users();
        private UserRepository oUserRepo = new UserRepository();
        private Logger oLogger = new Logger();
        private UserRegistration oUserRegistration = new UserRegistration();
        private StripeCustomersHandler oStripeCustomerHandler = new StripeCustomersHandler();

        [Authorize]
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        [Authorize]
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("api/Users/{Auth0ID}/ProfileInfo")]
        public async Task<IHttpActionResult> GetUserProfile(string Auth0ID)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                User oUser = await oUserRepo.GetUserProfileInfo(Auth0ID);
                return Json(oUser);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Users/{Auth0ID}/ProfileInfo; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }


        // POST: api/Users
        [AllowAnonymous]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserRegistrationRequestModel oUserRequestModel)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool blnIsEmailValid = ValidateEmailExists(oUserRequestModel.email_address);

                if (!blnIsEmailValid)
                    return BadRequest("E-mail address already exists");

                if (!oUserRequestModel.password.Equals(oUserRequestModel.password_confirm))
                    return BadRequest("Passwords Do not Match");

                if (!oUserRequestModel.password.Any(p => char.IsUpper(p)) && !oUserRequestModel.password_confirm.Any(cp => char.IsUpper(cp)))
                    return BadRequest("Passwords Don't Contain An Uppercase Letter");

                User user = oUserRegistration.CheckUserRegistration(oUserRequestModel);

                try
                {
                    await oUserRepo.SaveUser(user);

                    Auth0TokenReturnModel Auth0User = JsonConvert.DeserializeObject<Auth0TokenReturnModel>(oAuth0Users.CreateAuth0User(oUserRequestModel.email_address, oUserRequestModel.password));

                    user.EmailConfirmed = Auth0User.email_verified;
                    user.Auth0Identifier = Auth0User.user_id;
                    user.CreationDate = Convert.ToDateTime(Auth0User.created_at);

                    await oUserRepo.UpdateUserAfterRegistration(user);
                }
                catch (DbUpdateException)
                {
                    if (UserExists(user.Id))
                    {
                        return Conflict();
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }

                return CreatedAtRoute("DefaultApi", new { stripeCustId = user.StripeIdentifier }, user);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Users; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }

        }

        private bool ValidateEmailExists(string sEmail)
        {
            string email = oUserRepo.ValidateEmailExists(sEmail);
            bool blnIsValid = false;

            if (!String.IsNullOrEmpty(email))
            {
                return blnIsValid;
            }
            else
            {
                blnIsValid = true;
                return blnIsValid;
            }
        }

        private User MapUserRequestModelToUserModel(UserRegistrationRequestModel oUserRequestModel)
        {
            User oUser = new Models.User();

            oUser.FirstName = oUserRequestModel.first_name;
            oUser.LastName = oUserRequestModel.last_name;
            oUser.PhoneNumber = oUserRequestModel.phone_number;
            oUser.Email = oUserRequestModel.email_address;

            return oUser;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}