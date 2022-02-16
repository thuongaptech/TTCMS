using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCMS.Domain;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class MenuPageViewModel
    {
        public IEnumerable<SelectListItem> Lang { get; set; }
        public MenuTableViewModel TableList { get; set; }
    }
    public class MenuTableViewModel
    {
        public string LanguageId { get; set; }
        public IEnumerable<SelectListItem> Lang { get; set; }
        public SelectList Group { get; set; }
    }

    public class MenuManagerViewModel
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "LenghtString250")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Required")]
        public string Name { get; set; }
        public string Link { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int WithId { get; set; }
        public string TextType { set; get; }
        public int ParentID { get; set; }
        public string Target { get; set; }
        public string CssClass { set; get; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActived { get; set; }
        public int Order { get; set; }
        public MenuGroupType GroupType { get; set; }
        public string LanguageId { get; set; }
        public SelectList Lang { get; set; }
    }
}