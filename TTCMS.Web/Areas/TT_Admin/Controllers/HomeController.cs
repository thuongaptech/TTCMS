using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCMS.Areas.TT_Admin.CustomAttributes;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: TT_Admin/Home
        [AuthorizeUser(FunctionID = "DASHBOARD", RoleID = "VIEW")]
        public ActionResult Index()
        {
            return RedirectToAction("index", "NewsManagement");
        }
    }
}