using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class GroupMenuPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public GroupMenuTableViewModel TableList { get; set; }
    }
    public class GroupMenuTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<GroupMenuViewModel> ModelList { get; set; }
    }

    public class GroupMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public long CategoryGroup_Id { get; set; }
        public string LanguageId { get; set; }
        public SelectList Lang { get; set; }
    }
}