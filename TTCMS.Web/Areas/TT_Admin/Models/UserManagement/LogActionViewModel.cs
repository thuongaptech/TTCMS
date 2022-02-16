using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class LogActionViewModel
    {
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public System.DateTime ExecutionTime { get; set; }
        public string Action
        {
            get { return MethodName + "/" + ServiceName; }
        }
    }
}