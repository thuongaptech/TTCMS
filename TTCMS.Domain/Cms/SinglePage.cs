using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class SinglePage
    {
        public SinglePage()
        {
            CreatedDate = DateTime.Now;
            Language_SinglePages = new HashSet<Language_SinglePage>();
        }
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
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ICollection<Language_SinglePage> Language_SinglePages { set; get; }

    }
}
