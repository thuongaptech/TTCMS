using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class Category
    {
        public Category()
        {
            CreatedDate = DateTime.Now;
            News = new HashSet<News>();
            Products = new HashSet<Product>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int? ParentID { get; set; }
        public string Target { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public CategoryType CategoryType { get; set; }
        public int Order { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsHome { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public string CodeColor { get; set; }
        public int ShowProduct { get; set; }
        public virtual ICollection<News> News { set; get; }
        public virtual ICollection<Product> Products { set; get; }

    }
}
