
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public  class Language
    {
        public Language()
        {
            CreatedDate = DateTime.Now;
            Language_Roles = new HashSet<Language_Role>();
            Language_Functions = new HashSet<Language_Function>();
            Language_GActions = new HashSet<Language_GAction>();
            Language_Settings = new HashSet<Language_Setting>();
            Menus = new HashSet<Menu>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img_Icon { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
        public virtual ICollection<Menu> Menus { set; get; }
        public virtual ICollection<Language_Role> Language_Roles { set; get; }
        public virtual ICollection<Language_Function> Language_Functions { set; get; }
        public virtual ICollection<Language_GAction> Language_GActions { set; get; }
        public virtual ICollection<Language_Setting> Language_Settings { set; get; }
    }
}
