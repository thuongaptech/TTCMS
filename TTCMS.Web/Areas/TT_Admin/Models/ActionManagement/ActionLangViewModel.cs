using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class ActionPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public ActionLangTableViewModel TableList { get; set; }
    }
    public class ActionLangTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<ActionLangViewModel> ModelList { get; set; }
    }

    public class ActionLangViewModel
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString50")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Id { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
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
        public long Lang_ActionId { get; set; }
        public SelectList Lang { get; set; }
    }
}