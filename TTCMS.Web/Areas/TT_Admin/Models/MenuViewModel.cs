﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class MenuViewModel
    {
       public string MenuId { get; set; }
       public string Text { get; set; }
       public string Link { get; set; }
       public string Target { set; get; }
       public string CssClass { set; get; }
       public string Active { get; set; }
       public List<MenuViewModel> Children { get; set; }
    }
}