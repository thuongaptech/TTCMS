using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class RolePageViewModel
    {
        public RoleTableViewModel RoleTableList { get; set; }

        public IEnumerable<SelectListItem> Lang { get; set; }

    }
    public class RoleTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<RoleViewModel> RoleList { get; set; }
    }
    public class RoleViewModel
    {
        public string Id { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public long Language_RoleId { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }
        public string LanguageId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<SelectListItem> Lang { get; set; }
    }
}