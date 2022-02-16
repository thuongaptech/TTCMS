using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class LanguageItemViewModel
    {
       public string Id { get; set; }
        public string Name { get; set; }
        public string Img_Icon { get; set; }
    }
}