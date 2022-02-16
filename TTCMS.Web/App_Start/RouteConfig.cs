using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TTCMS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               "PageDetail", // Route name
               "page/{alias}/{id}.html", // URL with parameters
               new { controller = "Page", action = "Detail", id = UrlParameter.Optional, alias = UrlParameter.Optional }
           );
            routes.MapRoute(
               "Product-detail", // Route name
               "san-pham/{alias}-{id}.html", // URL with parameters
               new { controller = "Product", action = "Detail", id = UrlParameter.Optional, alias = UrlParameter.Optional }
           );
            routes.MapRoute(
               "NewCata", // Route name
               "cate/{alias}-{id}.html", // URL with parameters
               new { controller = "Page", action = "Index", id = UrlParameter.Optional, alias = UrlParameter.Optional }, namespaces: new[] { "TTCMS.Web.Controllers" }
           );
            routes.MapRoute(
              "page-detail", // Route name
              "page/{alias}-{id}.html", // URL with parameters
              new { controller = "Page", action = "PageDetail", id = UrlParameter.Optional, alias = UrlParameter.Optional }
          );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "AboutUS",
            url: "lien-he.html",
             defaults: new { controller = "Home", action = "LienHe", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "thong-tin-gio-hang",
            url: "thong-tin-gio-hang.html",
             defaults: new { controller = "Cart", action = "ThongTinDonHang", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "chi-tiet-don-hang",
            url: "chi-tiet-don-hang.html",
             defaults: new { controller = "Cart", action = "ThanhToan", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "thay-doi-mat-khau",
            url: "thay-doi-mat-khau.html",
             defaults: new { controller = "Account", action = "ChangePass", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "thong-tin-tai-khoan",
            url: "thong-tin-tai-khoan.html",
             defaults: new { controller = "Account", action = "Profile", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "thong-tin-don-hang",
            url: "thong-tin-don-hang.html",
             defaults: new { controller = "Cart", action = "Info", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "thon-tin-chi-tiet-don-hang",
            url: "thon-tin-chi-tiet-don-hang.html",
             defaults: new { controller = "Cart", action = "Info_Detail", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "dang-ky-tai-khoan",
            url: "dang-ky-tai-khoan.html",
             defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "khoi-phuc-mat-khau",
            url: "khoi-phuc-mat-khau.html",
             defaults: new { controller = "Account", action = "ForgotPassword", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "dang-nhap",
            url: "dang-nhap.html",
             defaults: new { controller = "Account", action = "LoginS", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "gio-hang",
            url: "gio-hang.html",
             defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "timkiem",
            url: "tim-kiem",
             defaults: new { controller = "Home", action = "TimKiem", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "dangxuat",
            url: "dang-xuat",
             defaults: new { controller = "Account", action = "LogOff", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
             name: "spkm",
            url: "san-pham-khuyen-mai.html",
             defaults: new { controller = "Product", action = "Sales", id = UrlParameter.Optional },
             namespaces: new[] { "TTCMS.Web.Controllers" }
         );
            routes.MapRoute(
         "DetailNew", // Route name
         "tin-tuc/{alias}-{id}.html", // URL with parameters
         new { controller = "Page", action = "DetailNews" , id = UrlParameter.Optional, alias = UrlParameter.Optional } // Parameter defaults
     );
            routes.MapRoute(
        "dm-product", // Route name
        "danh-muc/{alias}-{id}.html", // URL with parameters
        new { controller = "Product", action = "Category", newsRoute = UrlParameter.Optional } // Parameter defaults
    );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "TTCMS.Web.Controllers" }
            );
        }
    }
}
