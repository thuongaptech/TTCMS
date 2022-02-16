using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using TTCMS.Core;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Core.Json;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Areas.TT_Admin.Models;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        //logger
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly  ILanguageService languageService;
        private readonly ISinglePageService singlePageService;
        public BaseController(ILanguageService languageService, ISinglePageService singlePageService)
        {
            this.languageService = languageService;
            this.singlePageService = singlePageService;
        }
        protected string cultureName = null;
        public BaseController()
        {
            // TODO: Complete member initialization
        }
        protected void HandleApplicationException(ApplicationException ex)
        {
            logger.Error(ex);
            RedirectToAction("Index", "Home");
        }
        protected void HandleException(string message, Exception ex)
        {
            logger.Error(message + "::" + ex.Message);
            RedirectToAction("Index", "Home");
        }
        [ChildActionOnly]
        public ActionResult _TabLanguage()
        {
            IEnumerable<LanguageItemViewModel> model = Mapper.Map<IEnumerable<Language>, IEnumerable<LanguageItemViewModel>>(languageService.GetListForActive());
            return PartialView(model.ToArray());
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length == 0 ?
                           Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                           null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
          
            return base.BeginExecuteCore(callback, state);
        }
        /// <summary>
        /// change culture method
        /// </summary>
        /// <param name="language"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        /// 

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index", "NewsManagement");
        }
        public ActionResult ChangeCulture(string language, string returnUrl)
        {
            Session[SystemConsts.CULTURE] = new CultureInfo(language);
            return Redirect(returnUrl);
        }
        public ActionResult AjaxAlert(string code="", string msg="")
        {
            var model = new AjaxAlertViewModel();
            if (code != "" && msg != "")
            {
                model.Code = code;
                model.Msg = msg;
            }
            return PartialView(model);
        }
        public JsonResponse JsonError(string message)
        {
            JsonResponse response = new JsonResponse() { Code = AjaxAlertConsts.WithError, Msg = message, Success = false };
            return response;
        }
        public JsonResponse JsonErrorWidthIndex(string message , string redirectUrl)
        {
            JsonResponse response = new JsonResponse() { Code = AjaxAlertConsts.WithError, Msg = message, Success = false, RedirectUrl = redirectUrl };
            return response;
        }
        public JsonResponse JsonSuccess(string redirectUrl = null, string message = null, string partialViewHtml = null)
        {
            JsonResponse response = new JsonResponse();
            response.Code = AjaxAlertConsts.WithSuccess;
            response.RedirectUrl = redirectUrl;
            response.Msg = message;
            response.PartialViewHtml = partialViewHtml;
            response.Success = true;
            return response;
        }
        public JsonResponse JsonSuccessWidthIndex(string redirectUrl = null, string message = null)
        {
            JsonResponse response = new JsonResponse();
            response.Code = AjaxAlertConsts.WithSuccess;
            response.RedirectUrl = redirectUrl;
            response.Msg = message;
            response.PartialViewHtml = null;
            response.Success = true;
            return response;
        }
        protected string RenderPartialViewToString(string viewName, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        protected string RenderViewToString(string viewName, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            var viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, viewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        protected string RenderViewToString<T>(string viewName, T model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            var viewData = new ViewDataDictionary<T>(model);

            using (StringWriter sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, viewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}