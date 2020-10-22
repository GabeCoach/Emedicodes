using EmediCodesWebApplication.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.StripeConnector
{
    public class StripeCustomersHandler
    {
        private Logger oLogger = new Logger();

        public StripeCustomer CreateStripeCustomer(string sEmail, string sToken, string sInputDiscountCode)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);
                var customerService = new StripeCustomerService();

                string sDiscountCode = ConfigurationManager.AppSettings["discount_code"];

                var CustomerOptions = new StripeCustomerCreateOptions()
                {
                    Email = sEmail,
                    Description = "EmediCodes Basic Plan for " + sEmail,
                    AccountBalance = 0,
                    PlanId = "1",
                    SourceToken = sToken,
                    TrialEnd = DateTime.Now + TimeSpan.FromDays(14)
                };

                if (sInputDiscountCode.Equals(sDiscountCode))
                {
                    CustomerOptions.CouponId = sDiscountCode;
                }               

                StripeCustomer customer = customerService.Create(CustomerOptions);

                return customer;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CreateStripeCustomer; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public StripeCard AddCustomerPaymentMethod(string sStripeCustomerId, string sStripeToken)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                var cardOptions = new StripeCardCreateOptions()
                {
                    SourceToken = sStripeToken
                };

                var cardService = new StripeCardService();
                StripeCard card = cardService.Create(sStripeCustomerId, cardOptions);

                return card;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: AddCustomerPaymentMethod; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public StripeSubscription CancelCustomerSubscription(string sStripeCustomerId)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(sStripeCustomerId);

                var subscriptionService = new StripeSubscriptionService();
                StripeSubscription subscription = subscriptionService.Cancel(customer.Subscriptions.FirstOrDefault().Id);

                return subscription;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CreateStripeCustomer; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public bool CheckCustomerPaymentMethod(string sStripeCustomerId)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                bool blnPaymentMethodExists = false;

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(sStripeCustomerId);

                if (!customer.Sources.Any())
                    return blnPaymentMethodExists;
                else
                {
                    blnPaymentMethodExists = true;
                    return blnPaymentMethodExists;
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CheckCustomerPaymentMethod; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public StripeSubscription CreateNewSubscription(string sStripeCustomerId)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                var subscriptionOptions = new StripeSubscriptionCreateOptions()
                {
                    PlanId = "1"
                };

                var subscriptionService = new StripeSubscriptionService();

                StripeSubscription subscription = subscriptionService.Create(sStripeCustomerId, subscriptionOptions);

                return subscription;
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CreateNewSubscription; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public bool CheckCustomerSubscription(string stripeCustomerId)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                bool blnSubscriptionIsValid = false;

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(stripeCustomerId);

                if (!customer.Subscriptions.Any())
                    return blnSubscriptionIsValid;
                else if (customer.Subscriptions.Count() > 1)
                    return blnSubscriptionIsValid;             
                else if (customer.Subscriptions.FirstOrDefault().EndedAt < DateTime.Now)
                    return blnSubscriptionIsValid;
                else if (customer.Subscriptions.FirstOrDefault().CanceledAt < DateTime.Now)
                    return blnSubscriptionIsValid;
                else
                {
                    blnSubscriptionIsValid = true;
                    return blnSubscriptionIsValid;
                }
            }
            catch (Exception ex)
            {
                oLogger.LogData("METHOD: CheckCustomerSubscription; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public bool CheckFreeTrialPeriod(string sStripeCustomerId)
        {

            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                bool blnIsFreeTrialPeriod = false;

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(sStripeCustomerId);

                if (customer.Subscriptions.FirstOrDefault().TrialEnd > DateTime.Now)
                    blnIsFreeTrialPeriod = true;

                return blnIsFreeTrialPeriod;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: CheckFreeTrialPeriod; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }

        public int GetFreeTrialTime(string sStripeCustomerId)
        {
            try
            {
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeApi_LiveKey"]);

                int iFreeTrialDaysLeft = 0;

                var customerService = new StripeCustomerService();
                var customer = customerService.Get(sStripeCustomerId);

                iFreeTrialDaysLeft = (customer.Subscriptions.FirstOrDefault().TrialEnd.Value - DateTime.Now).Days;

                return iFreeTrialDaysLeft;
            }
            catch(Exception ex)
            {
                oLogger.LogData("METHOD: FreeTrialTime; ERROR: TRUE; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException + "; STACKTRACE: " + ex.StackTrace);
                throw;
            }
        }
    }
}