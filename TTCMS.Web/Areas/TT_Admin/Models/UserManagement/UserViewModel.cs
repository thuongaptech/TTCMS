using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class UserPageViewModel
    {
        public UserTableViewModel UserTableList { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        
    }
    public class UserTableViewModel
    {
        public string Show { get; set; }
        public string Search { get; set; }
        public int Page { get; set; }
        public IPagedList<UserViewModel> UserList { get; set; }
    }
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool Sex { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public bool Activated { get; set; }
    }
}