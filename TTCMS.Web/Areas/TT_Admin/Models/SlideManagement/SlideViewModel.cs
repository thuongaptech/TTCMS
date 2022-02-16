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
    public class SlidePageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public SlideTableViewModel TableList { get; set; }
    }
    public class SlideTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<SlideViewModel> ModelList { get; set; }
    }

    public class SlideViewModel
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Images { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
        public SlideType SlideType { get; set; }
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
        public SelectList Lang { get; set; }
    }
}