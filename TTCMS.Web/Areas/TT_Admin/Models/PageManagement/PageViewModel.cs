using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class SinglePageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public PageTableViewModel TableList { get; set; }
    }
    public class PageTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<PageViewModel> ModelList { get; set; }
    }

    public class PageViewModel
    {
        public int Id { get; set; }
        public int Views { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public bool IsHome { get; set; }
        public int Order { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedById { get; set; }
        //
        public long SinglePage_Id { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        public string Summary { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString200")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int SinglePageId { get; set; }
        public string LanguageId { get; set; }
        public string CreatedByUserName { get; set; }
        public SelectList Lang { get; set; }
    }
}