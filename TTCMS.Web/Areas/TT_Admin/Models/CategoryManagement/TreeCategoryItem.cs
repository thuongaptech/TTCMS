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

    public class TreeCategoryItem
    {
        public int Id { get; set; }
        public int? ParentID { get; set; }
        public string Target { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public int Order { get; set; }
        public CategoryType CategoryType { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsHome { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        //
        public long Category_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int CategoryId { get; set; }
        public string LanguageId { get; set; }
        public SelectList Lang { get; set; }
    }
}