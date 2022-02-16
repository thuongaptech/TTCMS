using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TTCMS.Data;
using TTCMS.Domain;
using System;
using System.Web.Helpers;
using System.Diagnostics;
using log4net;
using System.Reflection;
using System.Web;
using System.IO.Compression;

namespace TTCMS.Areas.TT_Admin.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private IDictionary<string, object> _parameters;
        public string UserName { get; set; }
        public string ClientIpAddress { get; set; }
        public string SessionID { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string BrowserInfo { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public long ExecutionDuration { get; set; }
        public string Id { get; set; }

        public LogAttribute(string description)
        {
            Description = description;
        }
        private Stopwatch stopWatch = new Stopwatch();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopWatch.Reset();
            stopWatch.Start();
            //
            HttpRequestBase request = filterContext.HttpContext.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];

            if (!String.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }
            //
            _parameters = filterContext.ActionParameters;
            UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous";
            ClientIpAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            SessionID =  filterContext.HttpContext.Session.SessionID;
            BrowserInfo =  filterContext.HttpContext.Request.UserAgent;
            ClientName = System.Net.Dns.GetHostName();
            ServiceName = filterContext.ActionDescriptor.ActionName;
            MethodName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            Id = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.GetUserId() : "Anonymous";
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopWatch.Stop();
            ExecutionDuration = stopWatch.ElapsedMilliseconds;
            TTCMSDbContext context = new TTCMSDbContext();
            var key = Description;
            var description = "";
            foreach (var kvp in _parameters)
            {
                if (key.Contains("{" + kvp.Key + "}"))
                {
                    description += kvp.Key.ToString() + "=" + Json.Encode(kvp.Value);
                }
               
            }
            try {
                var log = new ActivityLog() { ActivityLogId = Guid.NewGuid(), ServiceName = ServiceName, MethodName = MethodName, Parameters = description, ExecutionTime = DateTime.Now, ExecutionDuration = ExecutionDuration, ClientIpAddress = ClientIpAddress, SessionID = SessionID, ClientName = ClientName, BrowserInfo = BrowserInfo, UserId = Id };
                context.ActivityLogs.Add(log);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error(ex);
            }
        }
    }
}