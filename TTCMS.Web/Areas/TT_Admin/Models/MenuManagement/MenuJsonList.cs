using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin.Models
{

        [Serializable]
        public class MenuJsonList
        {
            public int id { get; set; }

            public IList<MenuJsonList> children { get; set; }
        }
}