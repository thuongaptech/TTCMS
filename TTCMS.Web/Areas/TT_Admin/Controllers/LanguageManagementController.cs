using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Service.Common;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Areas.TT_Admin.Filters;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class LanguageManagementController : BaseController
    {
        private readonly ILanguageService languageService;
        public LanguageManagementController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }
        // GET: TT_Admin/LanguageManagement
        [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "VIEW")]
        public ActionResult Index(int page = 1, string search = "")
        {
            try {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                var lang = languageService.GetList(new Page(page, pageSize), search);
                // map it to a paged list of models.
                var langModel = Mapper.Map<IPagedList<Language>, IPagedList<LanguageViewModel>>(lang);
                var table = new LangTableViewModel {ModelList=langModel,Search=search };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_LangTable", table);
                }
                var model = new LangPageViewModel();
                model.TableList = table;
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List Lang", ex);
                return RedirectToAction("Index","Home").WithError(Resources.Resources.ErrorCatch);

            }
        }
         [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "CREATE")]
        public ActionResult Create()
        {
            return PartialView(new LanguageViewModel() {IsActived = true });
        }
        [ChildActionOnly]
         public string GetLangName(string Id)
         {
             return Id == "*" ? TTCMS.Resources.Resources.AllLanguage : languageService.GetForId(Id).Name;
         }
         [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "CREATE"), HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Create(LanguageViewModel model)
         { try
             {
                 if (ModelState.IsValid)
                 {
                     model.CreatedDate = DateTime.Now;
                     model.CreatedBy = User.Identity.Name;
                     model.UpdatedBy = User.Identity.Name;
                     model.UpdatedDate = DateTime.Now;
                     model.IsDeleted = true;
                      Language lang = Mapper.Map<LanguageViewModel, Language>(model);
                      if (lang.IsDefault == true)
                      {
                          var langdefault = languageService.GetForDefaultModel();
                          if (langdefault != null)
                          {
                              langdefault.IsDefault = false;
                              languageService.Update(langdefault);
                          }
                          
                      }
                     languageService.Create(lang);
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
              return RedirectToAction("Index").WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Create lang", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);

             }
         }
         [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "EDIT")]
         public ActionResult Edit(string Id)
         {
             try
             {
                 var model = languageService.GetForId(Id);
                 var langModel = Mapper.Map<Language, LanguageViewModel>(model);
                 return PartialView(langModel);
             }
             catch (Exception ex)
             {
                 HandleException("Edit action", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
             
         }
         [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "EDIT"), HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(LanguageViewModel model)
         { 
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedBy = User.Identity.Name;
                     model.UpdatedDate = DateTime.Now;
                      Language lang = Mapper.Map<LanguageViewModel, Language>(model);
                      if (lang.IsDefault == true)
                      {
                          var langdefault = languageService.GetForDefaultModel();
                          if (langdefault != null)
                          {
                              langdefault.IsDefault = false;
                              languageService.Update(langdefault);
                          }

                      }
                     languageService.Update(lang);
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
              return RedirectToAction("Index").WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Create lang", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);

             }
         }
         [AuthorizeUser(FunctionID = "LANGUAGE", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(string Id)
         {
             try
             {
                 if (Id == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var lang = languageService.GetForId(Id);
                 if (lang == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 if (lang.IsDeleted == false)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithWarning, Msg = Resources.Resources.AlertDataSystem };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 languageService.Delete(Id);
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessDeleteFunction);
             }
             catch (Exception ex)
             {
                 HandleException("Delete Lang", ex);
                 var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                 return Json(reponse, JsonRequestBehavior.AllowGet);
             }
         }
    }
}