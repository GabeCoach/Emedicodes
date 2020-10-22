using EmediCodesWebApplication.Auth0Connector;
using EmediCodesWebApplication.Logging;
using EmediCodesWebApplication.Repository;
using EmediCodesWebApplication.StripeConnector;
using EmediCodesWebApplication.StripeConnector.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/Payments")]
    public class PaymentController : ApiController
    {
        private UserRepository oUserRepo = new UserRepository();
        private StripeCustomersHandler oStripeCustomerHandler = new StripeCustomersHandler();
        private Logger oLogger = new Logger();
        private Auth0Users oAuth0User = new Auth0Users();

        [HttpPost]
        [AllowAnonymous]
        [Route("Capture")]
        public IHttpActionResult PostPayment(PaymentRequestModel oCustomerPaymentRequest)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string userEmail = oUserRepo.GetUserEmail(oCustomerPaymentRequest.UserId).ToString();

                if(String.IsNullOrEmpty(oCustomerPaymentRequest.coupon_code))
                {
                    oCustomerPaymentRequest.coupon_code = "";
                }

                StripeCustomer stripeCustomer = oStripeCustomerHandler.CreateStripeCustomer(userEmail, oCustomerPaymentRequest.Token, oCustomerPaymentRequest.coupon_code);
                oUserRepo.UpdateUserWithStripeID(stripeCustomer.Id, oCustomerPaymentRequest.UserId);
                oAuth0User.SendUserVerificationEmail(oCustomerPaymentRequest.Auth0Id);
                oLogger.LogData("ROUTE: api/Payments/Capture; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Ok();
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/CancelSubscription")]
        public IHttpActionResult CancelSubscription(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string stripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                StripeSubscription calceledSubscription = oStripeCustomerHandler.CancelCustomerSubscription(stripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CancelSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(calceledSubscription.CanceledAt);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CancelSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("PaymentMethod")]
        public IHttpActionResult AddCustomerPaymentMethod(PaymentRequestModel oPaymentRequest)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string sStripeCustomerId = oUserRepo.GetStripeCustomerId(oPaymentRequest.UserId);
                StripeCard oStripeCard = oStripeCustomerHandler.AddCustomerPaymentMethod(sStripeCustomerId, oPaymentRequest.Token);
                StripeSubscription oStripeSubscription = oStripeCustomerHandler.CreateNewSubscription(sStripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CancelSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(oStripeCard.Id);

            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/PaymentMethod; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/CheckSubscription")]
        public IHttpActionResult CheckSubscription(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string stripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                var blnSubscriptionIsValid = oStripeCustomerHandler.CheckCustomerSubscription(stripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CheckSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(new { ValidSubscription = blnSubscriptionIsValid });
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CheckSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/FreeTrialPeriod")]
        public IHttpActionResult CheckFreeTrialPeriod(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string sStripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                var blnIsFreeTrialActive = oStripeCustomerHandler.CheckFreeTrialPeriod(sStripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/FreeTrialPeriod; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(new { InFreeTrial = blnIsFreeTrialActive });
            }
            catch(Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/FreeTrialPeriod; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/FreeTrialTime")]
        public IHttpActionResult GetFreeTrialTime(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string sStripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                int FreeTrialDaysLeft = oStripeCustomerHandler.GetFreeTrialTime(sStripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/FreeTrialTime; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(new { FreeTrialDays = FreeTrialDaysLeft });
            }
            catch(Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/FreeTrialTime; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/PaymentMethod")]
        public IHttpActionResult CheckUserPaymentMethod(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string stripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                var blnPaymentMethodExists = oStripeCustomerHandler.CheckCustomerPaymentMethod(stripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/PaymentMethod; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(new { PaymentMethodExists = blnPaymentMethodExists });
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/PaymentMethod; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{UserId}/Subscribe")]
        public IHttpActionResult CreateNewSubscription(int UserId)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                string sStripeCustomerId = oUserRepo.GetStripeCustomerId(UserId);
                StripeSubscription stripeSubscription = oStripeCustomerHandler.CreateNewSubscription(sStripeCustomerId);
                oLogger.LogData("ROUTE: api/Payments/{UserId}/CheckSubscription; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Json(stripeSubscription);
            }
            catch (Exception ex)
            {
                oLogger.LogData("ROUTE: api/Payments/{UserId}/PaymentMethod; METHOD: POST; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }
    }
}
