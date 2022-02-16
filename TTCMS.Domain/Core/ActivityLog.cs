
using System;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public  class ActivityLog
    {
        public Guid ActivityLogId { get; set; }
       public string ServiceName { get; set; }
       public string MethodName { get; set; }
       public string Parameters { get; set; }
       public System.DateTime ExecutionTime { get; set; }
       public long ExecutionDuration { get; set; }
       public string ClientIpAddress { get; set; }
        public string SessionID { get; set; }
        public string ClientName { get; set; }
        public string BrowserInfo { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User  { set; get; }
        public ActivityLog()
        {
            ExecutionTime = DateTime.Now;
        }
    }
}
