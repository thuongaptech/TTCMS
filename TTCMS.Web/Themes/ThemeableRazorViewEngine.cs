using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TTCMS.Web.Themes
{
    /// <summary>
    /// ThemeableRazorViewEngine (inspired by NopCommerce)
    /// </summary>
    public class ThemeableRazorViewEngine : RazorViewEngine
    {
        public ThemeableRazorViewEngine()
        {

            AreaViewLocationFormats = new[]
             {
                 //themes
                // "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 //"~/Areas/{2}/Views/Shared/{0}.cshtml",
                                              
                 //default
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml",
             };

            AreaMasterLocationFormats = new[]
             {
                 //themes
                // "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 //"~/Areas/{2}/Views/Shared/{0}.cshtml",


                 //default
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml",
             };

            AreaPartialViewLocationFormats = new[]
             {
                 //themes
                 //"~/Areas/{2}/Views/{1}/{0}.cshtml",
                // "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                    
                 //default
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
             };

            ViewLocationFormats = new[]
            {
                //themes
                //"~/Themes/{2}/Views/{1}/{0}.cshtml", 
               // "~/Themes/{2}/Views/Shared/{0}.cshtml",

                //default
                "~/Views/{1}/{0}.cshtml", 
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Emails/{0}.cshtml",
            };

            MasterLocationFormats = new[]
            {
                //themes
               // "~/Themes/{2}/Views/{1}/{0}.cshtml", 
                //"~/Themes/{2}/Views/Shared/{0}.cshtml", 

                //default
                "~/Views/{1}/{0}.cshtml", 
                "~/Views/Shared/{0}.cshtml",
            };

            PartialViewLocationFormats = new[]
            {
                //themes
               // "~/Themes/{2}/Views/{1}/{0}.cshtml",
               // "~/Themes/{2}/Views/Shared/{0}.cshtml",

                //default
                "~/Views/{1}/{0}.cshtml", 
                "~/Views/Shared/{0}.cshtml",
            };
        }

    }
}