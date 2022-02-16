using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCMS.Domain;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class QuangCaoPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public QuangCaoTableViewModel TableList { get; set; }
    }
    public class QuangCaoTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<QuangCaoViewModel> ModelList { get; set; }
    }

    public class QuangCaoViewModel
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Images { get; set; }
        public string Target { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public QuangCaoType QuangCaoType { get; set; }
        public int Order { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string LinkRedirec { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public string NgonNgu { get; set; }
        public string GroupQC { get; set; }
        public SelectList QCType { get; set; }

    }
}