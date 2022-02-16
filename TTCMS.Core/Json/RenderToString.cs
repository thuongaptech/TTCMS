using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace TTCMS.Core.Json
{
    class RenderToString
    {
        public static string RazorRender(Controller context, string DefaultAction)
        {

            string Cache = string.Empty;



            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            System.IO.TextWriter tw = new System.IO.StringWriter(sb);



            RazorView view_ = new RazorView(context.ControllerContext, DefaultAction, null, false, null);

            view_.Render(new ViewContext(context.ControllerContext, view_, new ViewDataDictionary(), new TempDataDictionary(), tw), tw);



            Cache = sb.ToString();



            return Cache;

        }


        public static class HtmlHelperExtensions
        {

            public static string RenderPartialToString(ControllerContext context, string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
            {

                ViewEngineResult result = ViewEngines.Engines.FindPartialView(context, partialViewName);



                if (result.View != null)
                {

                    StringBuilder sb = new StringBuilder();

                    using (StringWriter sw = new StringWriter(sb))
                    {

                        using (HtmlTextWriter output = new HtmlTextWriter(sw))
                        {

                            ViewContext viewContext = new ViewContext(context, result.View, viewData, tempData, output);

                            result.View.Render(viewContext, output);

                        }

                    }



                    return sb.ToString();

                }



                return String.Empty;

            }

        }
    }
}
