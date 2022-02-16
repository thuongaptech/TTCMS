using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace TTCMS.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() {
             CreatedDate = DateTime.Now;
            Permissions = new HashSet<Permission>();
            Language_Roles = new HashSet<Language_Role>();
        }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Permission> Permissions { set; get; }
        public virtual ICollection<Language_Role> Language_Roles { set; get; }
    }
}