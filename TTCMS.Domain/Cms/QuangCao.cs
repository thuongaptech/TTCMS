using System;
namespace TTCMS.Domain
{
    public class Advertisements
    {
        public Advertisements()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Images { get; set; }
        public string Target { get; set; }
        public QuangCaoType QuangCaoType { get; set; }
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
