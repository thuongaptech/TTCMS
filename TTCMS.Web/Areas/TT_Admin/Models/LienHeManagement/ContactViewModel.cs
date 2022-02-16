using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class ContactPageViewModel
    {
        public ContactTableViewModel TableList { get; set; }
    }
    public class ContactTableViewModel
    {
        public string Search { get; set; }
        public IPagedList<ContactViewModel> ModelList { get; set; }
    }
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LoaiLienHe { get; set; }
        public string NoiDung { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}