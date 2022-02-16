using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class LogPageViewModel
    {
        public LogTableViewModel TableList { get; set; }
    }
    public class LogTableViewModel
    {
        public string Search { get; set; }
        public IPagedList<LogViewModel> ModelList { get; set; }
    }
    public class LogViewModel
    {
        public Guid ActivityLogId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public DateTime ExecutionTime { get; set; }
        public long ExecutionDuration { get; set; }
        public string ClientIpAddress { get; set; }
        public string SessionID { get; set; }
        public string ClientName { get; set; }
        public string BrowserInfo { get; set; }
        public string UserUserName { get; set; }
    }
}