using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.Admin.Models
{
    public class ClientMenuViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int Order { set; get; }
        public bool Status { set; get; }
        public string GroupId { set; get; }
        public string GroupName { set; get; }
    }
}