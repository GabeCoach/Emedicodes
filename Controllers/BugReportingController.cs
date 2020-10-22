using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmediCodesWebApplication.HelperModels;
using EmediCodesWebApplication.InsightlyConnector;
using System.Web.Http.Cors;
using EmediCodesWebApplication.Repository;
using System.Threading.Tasks;
using EmediCodesWebApplication.Models;
using EmediCodesWebApplication.Logging;

namespace EmediCodesWebApplication.Controllers
{
    [EnableCors(origins: "https://www.app.emedicodes.com, http://localhost", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/BugReport")]
    public class BugReportingController : ApiController
    {
        private UserRepository oUserRepo = new UserRepository();
        private InsightlyTasksHandler oInsTaskHandler = new InsightlyTasksHandler();
        private Logger oLogger = new Logger();

        [HttpPost]
        [Route("{UserId}")]
        public async Task<IHttpActionResult> Post(int UserId, BugReportingModel oBugReport)
        {
            string sIPAddress = Request.GetOwinContext().Request.RemoteIpAddress;

            try
            {
                User oUser = await oUserRepo.GetUser(UserId);
                string sCustomerName = oUser.FirstName + " " + oUser.LastName;
                oBugReport.bug_reporter_name = sCustomerName;
                oInsTaskHandler.CreateBugFixTask(oBugReport);
                oLogger.LogData("ROUTE: api/BugReport/{UserId}; METHOD: POST; IP_ADDRESS: " + sIPAddress);
                return Ok();
            }
            catch(Exception ex)
            {
                oLogger.LogData("ROUTE: api/BugReport/{UserId}; METHOD: GET; IP_ADDRESS: " + sIPAddress + "; EXCEPTION: " + ex.Message + "; INNER EXCEPTION: " + ex.InnerException);
                return InternalServerError();
            }
        }
    }
}
