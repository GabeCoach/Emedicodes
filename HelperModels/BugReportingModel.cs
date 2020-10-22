using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.HelperModels
{
    public class BugReportingModel
    {
        public string time_reported { get; set; }
        public string time_experienced { get; set; }
        public string date_reported { get; set; }
        public string date_experienced { get; set; }
        public string bug_description { get; set; }
        public string bug_reporter_name { get; set; }
    }
}