using AutoMapper;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TTCMS.Areas.TT_Admin.Filters;
using TTCMS.Core;
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Service.Common;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Web.Extensions;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class SettingManagementController : BaseController
    {
        private readonly ISettingService settingService;
        private readonly ILanguage_SettingService language_SettingService;
        private readonly ILanguageService languageService;
        public SettingManagementController(ISettingService settingService, ILanguage_SettingService language_SettingService, ILanguageService languageService)
        {
            this.settingService = settingService;
            this.language_SettingService = language_SettingService;
            this.languageService = languageService;
        }
        //
        // GET: /GroupMenuManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "SETTING ", RoleID = "VIEW")]
        [HttpGet]
        public ActionResult Index()
        {
            // map it to a paged list of models.
            SettingViewModel model = Mapper.Map<Setting, SettingViewModel>(settingService.GetbyFirst());
            if (model == null)
            {
                model = new SettingViewModel();
                model.Id = Guid.NewGuid().ToString();
            }
            if (!string.IsNullOrEmpty(model.BindPass))
                model.BindPass = settingService.Decrypt(model.BindPass);
            IEnumerable<ListLang> listlang = Mapper.Map<IEnumerable<Language>, IEnumerable<ListLang>>(languageService.GetListForActive());
            model.ListLangs = listlang.ToList();
            var filename = Server.MapPath("~/robots.txt");
            var css = Server.MapPath("~/Content/style-custom.css");
            var js = Server.MapPath("~/Content/js-custom.js");
            model.Robots = System.IO.File.ReadAllText(filename);
            model.Css = System.IO.File.ReadAllText(css); ;
            model.Js = System.IO.File.ReadAllText(js);
            return View(model);
        }
        [HttpPost,ValidateInput(false)]
        [AuthorizeUser(FunctionID = "SETTING ", RoleID = "EDIT"),Log("{model}")]
        public ActionResult Update(SettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Setting modelCreate = Mapper.Map<SettingViewModel, Setting>(model);
                    //var filename = Server.MapPath("~/robots.txt");
                    //var css = Server.MapPath("~/Content/style-custom.css");
                    //var js = Server.MapPath("~/Content/js-custom.js");
                    //System.IO.File.WriteAllText(filename, model.Robots);
                    //System.IO.File.WriteAllText(css, model.Css);
                    //System.IO.File.WriteAllText(js, model.Js);
                    var setting = settingService.GetbyId(model.Id);
                    if (setting == null)
                    {
                        model.BindPass = settingService.Encrypt(model.BindPass);
                        settingService.Create(modelCreate);
                    }
                    else
                    {
                        setting.SubForgotPassword = modelCreate.SubForgotPassword;
                        setting.BodyForgotPassword = modelCreate.BodyForgotPassword;
                        setting.TeamplateAdminDatHang = modelCreate.TeamplateAdminDatHang;
                        setting.TeamplateDatHang = modelCreate.TeamplateDatHang;
                        setting.BackgroudColor = modelCreate.BackgroudColor;
                        setting.ActiveColor = modelCreate.ActiveColor;
                        setting.HotLine = modelCreate.HotLine;
                        setting.LinkMaps = modelCreate.LinkMaps;
                        setting.Email = modelCreate.Email;
                        setting.CompanyName = modelCreate.CompanyName;
                        setting.Strfooter = modelCreate.Strfooter;
                        setting.LinkFooter = modelCreate.LinkFooter;
                        setting.TemplateEmail = modelCreate.TemplateEmail;
                        setting.FileXML = modelCreate.FileXML;
                        setting.Title = modelCreate.Title;
                        setting.ApplicationName = modelCreate.ApplicationName;
                        setting.ApplicationURL = modelCreate.ApplicationURL;
                        setting.pageSize = modelCreate.pageSize;
                        setting.Favicon = modelCreate.Favicon;
                        setting.Logo = modelCreate.Logo;
                        setting.Seo_Image = modelCreate.Seo_Image;
                        setting.Copyright = modelCreate.Copyright;
                        setting.contactus_setting = modelCreate.contactus_setting;
                        setting.google_map = modelCreate.google_map;
                        setting.Address = modelCreate.Address;
                        setting.IsClosed = modelCreate.IsClosed;
                        setting.EmailAccount = modelCreate.EmailAccount;
                        setting.EmailPassword = modelCreate.EmailPassword;
                        setting.EmailHost = modelCreate.EmailHost;
                        setting.EmailPort = modelCreate.EmailPort;
                        setting.EmailEnableSSL = modelCreate.EmailEnableSSL;
                        setting.RecieveEmail = modelCreate.RecieveEmail;
                        setting.Host = modelCreate.Host;
                        setting.Port = modelCreate.Port;
                        setting.BindDN = modelCreate.BindDN;
                        setting.BindPass = settingService.Encrypt(modelCreate.BindPass);
                        setting.UserSearchBase = modelCreate.UserSearchBase;
                        setting.UserFilter = modelCreate.UserFilter;
                        settingService.Update(setting);
                    }
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                return RedirectToAction("Index").WithError(Resources.Resources.WarningData);
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
          
        }
        public ActionResult GetSettingLang(string Id = "", string Lang = "")
        {
            if (Id == "")
            {
                return Json(JsonError(Resources.Resources.PleaseUpdateTheGeneralConfiguration), JsonRequestBehavior.AllowGet);
            }
            LangSettingViewModel model = Mapper.Map<Language_Setting, LangSettingViewModel>(language_SettingService.GetbyId(Lang, Id));
            if (model == null)
            {
                model = new LangSettingViewModel();
                model.SettingId = Id;
                model.LanguageId = Lang;
            }
            model.Name_Language = languageService.GetForId(Lang).Name;
            return Json(JsonSuccess(null, null, RenderPartialViewToString("GetSettingLang", model)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost,ValidateInput(false)]
        [AuthorizeUser(FunctionID = "SETTING ", RoleID = "EDIT"),Log("{model}")]
        public ActionResult UpSettingLang(LangSettingViewModel model)
        {
            try
            {
                        var lang_setting = language_SettingService.GetbyId(model.LanguageId, model.SettingId);
                        if (lang_setting != null)
                        {
                            lang_setting.ApplicationName = model.ApplicationName;
                            lang_setting.Description = model.Description;
                            lang_setting.Keywords = model.Keywords;
                            lang_setting.Page_Footer = model.Page_Footer;
                            language_SettingService.Update(lang_setting);
                        }
                        else
                        {
                            Language_Setting modelEdit = Mapper.Map<LangSettingViewModel, Language_Setting>(model);
                            language_SettingService.Create(modelEdit);
                        }
                    return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }
        }
        ////////
        //[AuthorizeUser(FunctionID = "NEWSCATE", RoleID = "EDIT")]
        //public ActionResult Edit(int Id = 0, string Lang = "")
        //{
        //    try
        //    {
        //        if (Id == 0)
        //            return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
        //        if (Lang != "")
        //        {
        //            cultureName = Lang;
        //        }
        //        var group = categoryService.GetbyId(Id);
        //        if (group == null)
        //        {
        //            return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
        //        }
        //        else
        //        {
        //            var lang_model = language_CategoryService.GetbyId(cultureName, Id);
        //            CategoryViewModel model = Mapper.Map<CategoryViewModel>(new Tuple<Category, Language_Category>(group, lang_model));
        //            if (model.LanguageId == null)
        //            {
        //                model.LanguageId = cultureName;
        //            }
        //            var lang = languageService.GetListForActive();
        //            model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
        //            PopulateParentIDDropDownList(model.ParentID,model.Id);
        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException("Message", ex);
        //        return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
        //    }

        //}
        //[AuthorizeUser(FunctionID = "NEWSCATE", RoleID = "EDIT")]
        //[HttpPost, ValidateAntiForgeryToken, Log("{model}")]
        //public ActionResult Edit(CategoryViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            model.UpdatedDate = DateTime.Now;
        //            model.UpdatedBy = User.Identity.Name;
        //            Category modelCreate = Mapper.Map<CategoryViewModel, Category>(model);
        //            categoryService.Update(modelCreate);
        //            //change lang_fun
        //            var lang_action = language_CategoryService.GetbyId(model.LanguageId, model.Id);
        //            var editlang_model = Mapper.Map<CategoryViewModel, Language_Category>(model);
        //            editlang_model.CategoryId = model.Id;
        //            if (lang_action != null)
        //            {
        //                lang_action.Name = editlang_model.Name;
        //                lang_action.Description = editlang_model.Description;
        //                lang_action.Keywords = editlang_model.Keywords;
        //                lang_action.Route = editlang_model.Route;
        //                language_CategoryService.Update(lang_action);
        //                return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
        //            }
        //            else
        //            {
        //                language_CategoryService.Create(editlang_model);
        //                return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
        //            }

        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", Resources.Resources.WarningData);
        //        }
        //        return View(model).WithWarning(Resources.Resources.WarningData);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException("Message", ex);
        //        return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
        //    }

        //}
    }
}