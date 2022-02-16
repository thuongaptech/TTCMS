using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class Menu
    {
        public Menu()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int? WithId { get; set; }
        public int ParentID { get; set; }
        public string Target { get; set; }
        public string CssClass { set; get; }
        public string TextType { set; get; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public int Order { get; set; }
        public MenuGroupType GroupType { get; set; }
        public string LanguageId { get; set; }
        public virtual Language Language { get; set; }

    }
}
