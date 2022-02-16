using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class ProductPageViewModel
    {
        //public IEnumerable<SelectListItem> Lang { get; set; }
        public ProductTableViewModel TableList { get; set; }
    }
    public class ProductTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public IPagedList<ProductViewModel> ModelList { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string MaSP { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Summary { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Body { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public double GiaBan { get; set; }
        public double GiaKM { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public int Order { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public bool IsBanChay { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Published { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int CategoryId { get; set; }
        public int DaMua { get; set; }
        public int[] ProductSize { get; set; }
        public ProductImages[] ProductImages { get; set; }
        public ColorImages[] ColorImages { get; set; }
    }
    [Serializable]
    public class ProductImages
    {
        public string Url { get; set; }
    }
    [Serializable]
    public class ColorImages
    {
        public string Url { get; set; }
        public int Color { get; set; }
    }
}