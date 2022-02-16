using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class Product
    {
        public Product()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Published = DateTime.Now;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaSP { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
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
        public string LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int DaMua { get; set; }
        public virtual Category Category { get; set; }


    }
}
