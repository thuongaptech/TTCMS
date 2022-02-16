using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class UserInfoViewModel
    {
        public string LastName { get; set; }
        public string ProfilePicUrl { get; set; }
    }
}