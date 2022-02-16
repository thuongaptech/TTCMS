using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class Slide
    {
        public Slide()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
        public SlideType SlideType { get; set; }
        public int Order { get; set; }
        public string LinkRedirec { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }

    }
}
