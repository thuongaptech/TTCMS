using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class TabLanguageViewModel
    {
       public string LangId { get; set; }
        public string ContentId { get; set; }
        public bool IsYes { get; set; }
    }
}