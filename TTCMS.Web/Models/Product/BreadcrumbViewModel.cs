using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models.Product
{
    public class BreadcrumbViewModel
    {
        public int RootId { get; set; }
        public string RootName { get; set; }
        public string RootRouter { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public bool Childrent { get; set; }
        public string ParentRouter { get; set; }
    }
}