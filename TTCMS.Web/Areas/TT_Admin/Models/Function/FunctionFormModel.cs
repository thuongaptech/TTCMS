using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public partial class FunctionFormModel
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString50")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "KeyFunction", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string Id { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Link { get; set; }
        public string Target { get; set; }
        public int Order { get; set; }
        public string CssClass { set; get; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFavorited { get; set; }
        public string ParentID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        //language function
        public long Lang_FunId { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Description { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Text { get; set; }
        public string FunctionId { get; set; }
        public string LanguageId { get; set; }
        public SelectList Lang { get; set; }
    }
}