using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{

    public partial class Function
    {
        public Function()
        {
            CreatedDate = DateTime.Now;
            IsFavorited = false;
            IsDeleted = true;
            Permissions = new HashSet<Permission>();
            Language_Functions = new HashSet<Language_Function>();
        }
        public string Id { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public int Order { get; set; }
        public string CssClass { set; get; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFavorited { get; set; }
        public string ParentID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<Permission> Permissions { set; get; }
        public virtual ICollection<Language_Function> Language_Functions { set; get; }
    }
}
