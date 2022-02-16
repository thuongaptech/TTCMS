
using System;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public  class Language_Role
    {
        public long Language_RoleId { get; set; }
        public string Description { get; set; }
        public string RoleId { get; set; }
        public string LanguageId { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public virtual Language Language { get; set; }
    }
}
