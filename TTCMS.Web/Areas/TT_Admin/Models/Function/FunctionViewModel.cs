using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class FunctionPageViewModel
    {
        public ListTableViewModel TableList { get; set; }

        public IEnumerable<SelectListItem> Lang { get; set; }
        
    }
    public class ListTableViewModel
    {
        public string Show { get; set; }
        public int Page { get; set; }
        public IPagedList<FunctionViewModel> FunctionList { get; set; }
    }
    public class FunctionViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public int Order { set; get; }
        public bool IsLocked { set; get; }
        public DateTime CreatedDate { set; get; }
    }
}