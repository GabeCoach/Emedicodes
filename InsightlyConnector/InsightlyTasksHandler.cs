using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Insightly;
using EmediCodesWebApplication.HelperModels;
using System.Configuration;
using InsightlySDK;

namespace EmediCodesWebApplication.InsightlyConnector
{
    public class InsightlyTasksHandler
    {
        string sInsightlyApiKey = ConfigurationManager.AppSettings["insightlyApiKey"];

        public Task CreateBugFixTask(BugReportingModel oBugReport)
        {
            InsightlyService i = new InsightlyService(sInsightlyApiKey);

            Task oInsightlyTask = new Task();

            oInsightlyTask.Status = "Not Started";
            oInsightlyTask.Priority = 3;
            oInsightlyTask.OwnerUserId = 1507578;
            oInsightlyTask.Title = "Bug Report From EmediCodes User: " + oBugReport.bug_reporter_name;
            oInsightlyTask.StartDate = DateTime.Now;
            oInsightlyTask.PubliclyVisible = true;
            oInsightlyTask.DueDate = DateTime.Now.AddDays(7);
            oInsightlyTask.Details = oBugReport.bug_description;
            oInsightlyTask.Completed = false;
            oInsightlyTask.OwnerVisible = true;
            oInsightlyTask.ResponsibleUserId = 1507578;
            oInsightlyTask.DateCreatedUtc = DateTime.Now;
            oInsightlyTask.AssignedByUserId = 1507578;

            i.CreateTask(oInsightlyTask);

            return oInsightlyTask;
        }

    }
}