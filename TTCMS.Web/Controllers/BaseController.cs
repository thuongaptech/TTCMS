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
using TTCMS.Core.Interfaces;
using TTCMS.Core.Json;
using TTCMS.Data;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Models;

namespace TTCMS.Web.Controllers
{
    [AllowAnonymous]
    public class BaseController : Controller
    {
        protected readonly ICacheHelper Cache;
        protected const string cultureName = "vi-VN";
        protected ConfigViewModel Config = new ConfigViewModel();
        private const string CacheSystemConfig = "SystemConfig";
        private readonly TTCMSDbContext db = new TTCMSDbContext();
        protected BaseController() : this(new CacheHelper()) { }
        protected BaseController(ICacheHelper cacheHelper)
        {
            // TODO: Complete member initialization
            this.Cache = cacheHelper;
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //binding language dropdownlist
            ////get system name from cache
            var cauhinh = new CauHinhViewModel();
            if (!Cache.IsSet(CacheSystemConfig))
            {
                CauHinhViewModel model = Mapper.Map<Setting, CauHinhViewModel>(db.Settings.FirstOrDefault());
                var lang_setting = db.Language_Settings.SingleOrDefault(x => x.LanguageId == cultureName && x.SettingId == model.Id);
                if (lang_setting == null)
                {
                    lang_setting = db.Language_Settings.FirstOrDefault();
                }
                model.ApplicationName = lang_setting.ApplicationName;
                model.Description = lang_setting.Description;
                model.Keywords = lang_setting.Keywords;
                model.Page_Footer = lang_setting.Page_Footer;
                Cache.Set(CacheSystemConfig, model);
                cauhinh = model;
            }
            else
            {
                cauhinh = Cache.Get(CacheSystemConfig) as CauHinhViewModel;
            }
            Config = Mapper.Map<CauHinhViewModel, ConfigViewModel>(cauhinh);
            return base.BeginExecuteCore(callback, state);
        }
        public string getLinkMenu(string type, string Link, int WithId, string Action, string Controller)
        {
            string Strlink = "";
            switch (type)
            {
                case "Custom Link":
                    Strlink = Link;
                    break;
                case "Page":
                    Strlink = Url.Action(Action, Controller, new { Id = WithId, alias = Link });
                    break;
                case "Download":
                    Strlink = Url.Action(Action, Controller, new { Id = WithId, alias = Link });
                    break;
                case "Custom Action":
                    Strlink = Url.Action(Action, Controller);
                    break;
                case "Product Category":
                    Strlink = Url.Action(Action, Controller, new { Id = WithId, alias = Link });
                    break;
                case "News Category":
                    Strlink = Url.Action(Action, Controller, new { Id = WithId, alias = Link });
                    break;
            }
            return Strlink;
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
        //public void SetConfig()
        //{
        //    var setting = settingService.GetbyFirst();
        //    ConfigViewModel model = Mapper.Map<Setting, ConfigViewModel>(setting);
        //    var lang_setting = language_SettingService.GetbyId(cultureName, setting.Id);
        //    if (lang_setting == null)
        //    {
        //        lang_setting = language_SettingService.GetList().FirstOrDefault();
        //    }
        //    model.ApplicationName = lang_setting.ApplicationName;
        //    model.Description = lang_setting.Description;
        //    model.Keywords = lang_setting.Keywords;
        //    model.Page_Footer = lang_setting.Page_Footer;
        //    //Cache.Set(CacheSystemConfig, model);
        //    Config = model;
        //    //binding language dropdownlist
        //    ////get system name from cache
        //    //if (!Cache.IsSet(CacheSystemConfig))
        //    //{
        //    //    var setting = settingService.GetbyFirst();
        //    //    ConfigViewModel model = Mapper.Map<Setting, ConfigViewModel>(setting);
        //    //    var lang_setting = language_SettingService.GetbyId(cultureName, setting.Id);
        //    //    model.ApplicationName = lang_setting.ApplicationName;
        //    //    model.Description = lang_setting.Description;
        //    //    model.Keywords = lang_setting.Keywords;
        //    //    model.Page_Footer = lang_setting.Page_Footer;
        //    //    Cache.Set(CacheSystemConfig, model);
        //    //    Config = model;
        //    //}
        //    //else
        //    //{
        //    //    Config = Cache.Get(CacheSystemConfig) as ConfigViewModel;
        //    //}
        //    ////binding language dropdownlist
        //}

    }
}
