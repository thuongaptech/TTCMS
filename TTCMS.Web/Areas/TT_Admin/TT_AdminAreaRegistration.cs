using System.Web.Mvc;

namespace TTCMS.Web.Areas.TT_Admin
{
    public class TT_AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TT_Admin";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "login",
                "pk_admin/dang-nhap",
                new { controller = "UserManagement", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "TTCMS.Web.Areas.TT_Admin.Controllers" }
            );
            context.MapRoute(
                 "pk_admin",
                 "pk_admin",
                 new { controller = "NewsManagement", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "TTCMS.Web.Areas.TT_Admin.Controllers" }
             );
            context.MapRoute(
                "TT_Admin_default",
                "TT_Admin/{controller}/{action}/{id}",
                new { controller = "NewsManagement", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TTCMS.Web.Areas.TT_Admin.Controllers" }
            );
        }
    }
}