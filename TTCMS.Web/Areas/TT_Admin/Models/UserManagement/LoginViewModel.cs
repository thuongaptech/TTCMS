using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class LoginViewModel
    {
        [StringLength(250, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordMinLong")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string Password { get; set; }
        public short Type { get; set; }
        [Display(Name = "RememberMeText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public bool RememberMe { get; set; }
    }
}