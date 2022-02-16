using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class GAction
    {
        public GAction()
        {
            CreatedDate = DateTime.Now;
            Permissions = new HashSet<Permission>();
            Language_GActions = new HashSet<Language_GAction>();
        }
        public string Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Permission> Permissions { set; get; }
        public virtual ICollection<Language_GAction> Language_GActions { set; get; }

    }
}
