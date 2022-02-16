using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public partial class UserProfileViewModel
    {
        public string Id { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string UserName { get; set; }
         [EmailAddress(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EmailAddressValid")]
        [StringLength(250, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Display(Name = "Email", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string Email { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString100")]
        [Display(Name = "FirstNameText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string FirstName { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString100")]
        [Display(Name = "LastNameText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string LastName { get; set; }
        [StringLength(500, ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "LenghtString500")]
        [Display(Name = "AddressText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string Address { get; set; }
         [Display(Name = "SexText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public bool Sex { get; set; }
         [Display(Name = "PointText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public decimal Point { get; set; }
         [Display(Name = "CreatedByText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string CreatedBy { get; set; }
         [Display(Name = "UpdatedByText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string UpdatedBy { get; set; }
         [Display(Name = "Status", ResourceType = typeof(TTCMS.Resources.Resources))]
        public bool IsDeleted { get; set; }
         [Display(Name = "ProfilePicUrlText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public string ProfilePicUrl { get; set; }
         [Display(Name = "Created", ResourceType = typeof(TTCMS.Resources.Resources))]
        public DateTime DateCreated { get; set; }
         [DataType(DataType.PhoneNumber)]
         [RegularExpression(@"^\d{9,11}$", ErrorMessageResourceType = typeof(TTCMS.Resources.Resources), ErrorMessageResourceName = "PhoneNumberValidate")]
         [Display(Name = "PhoneNumberText", ResourceType = typeof(TTCMS.Resources.Resources))]
         public string PhoneNumber { get; set; }
         [Display(Name = "ActiveText", ResourceType = typeof(TTCMS.Resources.Resources))]
        public bool Activated { get; set; }
         public DateTime? LastLoginTime { get; set; }
         [Display(Name = "KeyFunction", ResourceType = typeof(TTCMS.Resources.Resources))]
         public List<string> ListRole { get; set; }
         public List<LogActionViewModel> ListLog { get; set; }
         public string FullName
         {
             get { return FirstName + " " + LastName; }
         }
    }
}
