using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TTCMS.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {
            DateCreated = DateTime.Now;
             ActivityLogs = new HashSet<ActivityLog>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool Sex { get; set; }
        public decimal Point { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool Activated { get; set; }
        public string ToKen { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { set; get; }
        public virtual ICollection<SinglePage> PageCreates { set; get; }
        public virtual ICollection<SinglePage> PageUpdates { set; get; }
        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }
        
    }
}
