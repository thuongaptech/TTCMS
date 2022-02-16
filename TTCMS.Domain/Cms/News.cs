using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class News
    {
        public News()
        {
            CreatedDate = DateTime.Now;
        }
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
        public string StrCategoryIds { get; set; }
        public string StrIsHotCategoryIds { get; set; }
        public byte IsPost { get; set; }
        public byte IsApprove { get; set; }
    }
}
