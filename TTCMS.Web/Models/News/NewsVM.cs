using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Domain;

namespace TTCMS.Web.Models.News
{
    public class NewsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
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
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

    public class NewsCatVM
    {
        public Category[] Catalogy { get; set; }
        public NewsList[] NewsList { get; set; }
    }

    public class NewsList {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public string Img_Thumbnail { get; set; }
        public DateTime Published { get; set; }
        public string LanguageId { get; set; }
        public int CategoryId { get; set; }
    }
    public class SinglePageHomeViewModel
    {
        public int Id { get; set; }
        public int Views { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public bool IsHome { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedById { get; set; }
        //
        public long SinglePage_Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int SinglePageId { get; set; }
        public string LanguageId { get; set; }
        public string CreatedByUserName { get; set; }
    }
}