using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class NewsPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public NewsTableViewModel TableList { get; set; }
    }
    public class NewsTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<NewsViewModel> ModelList { get; set; }
    }

    public class NewsViewModel
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Published { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public SelectList Lang { get; set; }
        public string[] IsHotCategoryIds { get; set; }
        public string[] CategoryIds { get; set; }
        public byte IsPost { get; set; }
        public byte IsApprove { get; set; }
    }
}