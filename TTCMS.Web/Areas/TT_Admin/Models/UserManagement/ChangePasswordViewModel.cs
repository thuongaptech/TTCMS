using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string ProfilePicUrl { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordMinLong")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordOldext", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordRequired")]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PasswordMinLong")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordNewText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "ConfirmPassword")]
        [Display(Name = "ConfirmPasswordNewText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }
}