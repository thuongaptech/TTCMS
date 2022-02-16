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
    public class KyGuiPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public KyGuiTableViewModel TableList { get; set; }
    }
    public class KyGuiTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<KyGuiViewModel> ModelList { get; set; }
    }

    public class KyGuiViewModel
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public int? ParentID { get; set; }
        public string Target { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public int Order { get; set; }
        public CategoryType CategoryType { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsHome { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public string NgonNgu { get; set; }
        public string HangBay { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid decimalNumber")]
        public decimal Price { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid intNumber")]
        public int SoKg { get; set; }
        public SelectList Lang { get; set; }
    }
}