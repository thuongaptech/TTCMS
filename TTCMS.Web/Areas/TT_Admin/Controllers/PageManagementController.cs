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
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
     [Authorize]
    public class PageManagementController : BaseController
    {
         private readonly ISinglePageService singlePageService;
         private readonly ILanguage_SinglePageService language_SinglePageService;
         private readonly ILanguageService languageService;
         public PageManagementController(ISinglePageService singlePageService,ILanguage_SinglePageService language_SinglePageService, ILanguageService languageService)
        {
            this.singlePageService = singlePageService;
            this.language_SinglePageService = language_SinglePageService;
            this.languageService = languageService;
        }
         [ChildActionOnly]
         public ActionResult _TabLanguge(int Id)
         {
             try
             {
                 var lang = languageService.GetListForActive();
                 var model = new List<TabLanguageViewModel>();
                 foreach (var item in lang)
                 {
                     model.Add(new TabLanguageViewModel
                     {
                         LangId = item.Id,
                         ContentId = Id.ToString(),
                         IsYes = language_SinglePageService.Check(item.Id, Id)
                     });
                 }
                 return PartialView("_TabLanguge", model.ToArray());
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return null;
             }
         }
         //
         // GET: /PageManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult Index(int page = 1, string show = "", string search = "")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
             var list = singlePageService.GetPageList(new Page(page, pageSize), search);
             // map it to a paged list of models.
             var actionViewModel = Mapper.Map<IPagedList<SinglePage>, IPagedList<PageViewModel>>(list);
             foreach (var item in actionViewModel)
             {
                 var texts = Resources.Resources.NoUpdates;
                 var lang_model = language_SinglePageService.GetbyId(cultureName, item.Id);
                 item.Title = lang_model == null ? texts : lang_model.Title;
                 item.Route = lang_model == null ? texts : lang_model.Route;
             }
             var table = new PageTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_PageTable", table);
             }
             var lang = languageService.GetListForActive();
             var model = new SinglePageViewModel();
             model.TableList = table;
             model.Lang = lang.ToSelectListItems();
             return View(model);
         }
         ////
         //// GET: /PageManagement/Role/Create
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "CREATE")]
         [HttpGet]
         public ActionResult Create()
         {
             var lang = languageService.GetListForActive();
             var model = new PageViewModel();
             model.Order = singlePageService.GetSort();
             model.CreatedDate = DateTime.Now;
             model.Lang = new SelectList(lang, "Id", "Name");
             return View(model);
         }
    //    //// ////
    //    ////// // POST: /GroupMenuManagement/Role/Create

         [HttpPost,ValidateInput(false)]
         [ValidateAntiForgeryToken, Log("{model}")]
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "CREATE")]
         public ActionResult Create(PageViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.CreatedById = User.Identity.GetUserId();
                     model.UpdatedById = User.Identity.GetUserId();
                     SinglePage modelCreate = Mapper.Map<PageViewModel, SinglePage>(model);
                     var result = singlePageService.Create(modelCreate);
                     if (model.LanguageId == null)
                     {
                         var lang = languageService.GetListForActive();
                         model.Route = model.Route.Replace("/","");
                         foreach (var item in lang)
                         {
                             Language_SinglePage lang_model = Mapper.Map<PageViewModel, Language_SinglePage>(model);
                             lang_model.LanguageId = item.Id;
                             lang_model.SinglePageId = result.Id;
                             language_SinglePageService.Create(lang_model);
                         }
                     }
                     else
                     {
                         Language_SinglePage lang_model = Mapper.Map<PageViewModel, Language_SinglePage>(model);
                         lang_model.LanguageId = model.LanguageId;
                         lang_model.SinglePageId = result.Id;
                         lang_model.Route = lang_model.Route.Replace("/", "");
                         language_SinglePageService.Create(lang_model);
                     }
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }

                 var listlang = languageService.GetListForActive();
                 model.Lang = new SelectList(listlang, "Id", "Name");
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }
         public ActionResult _getSlug(string val)
         {
            if (Request.IsAjaxRequest())
            {
                if (val != null)
                {
                    ViewBag.Slug = XString.ToAscii(val);
                }
                return PartialView();
            }
            throw new NotSupportedException(Resources.Resources.NotAjaxRequest);
         }
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "EDIT")]
         public ActionResult Edit(int Id = 0, string Lang = "")
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 if (Lang != "")
                 {
                     cultureName = Lang;
                 }
                 var editmodel = singlePageService.GetbyId(Id);
                 if (editmodel == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     var lang_model = language_SinglePageService.GetbyId(cultureName, Id);
                     PageViewModel model = Mapper.Map<PageViewModel>(new Tuple<SinglePage, Language_SinglePage>(editmodel, lang_model));
                     if (model.LanguageId == null)
                     {
                         model.LanguageId = cultureName;
                     }
                     var lang = languageService.GetListForActive();
                     model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
                     return View(model);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "EDIT"), ValidateInput(false)]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(PageViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedById = User.Identity.GetUserId();
                     SinglePage modelCreate = Mapper.Map<PageViewModel, SinglePage>(model);
                     singlePageService.Update(modelCreate);
                     //change lang_fun
                     var lang_action = language_SinglePageService.GetbyId(model.LanguageId, model.Id);
                     var editlang_model = Mapper.Map<PageViewModel, Language_SinglePage>(model);
                     editlang_model.SinglePageId = model.Id;
                     if (lang_action != null)
                     {
                         lang_action.Title = editlang_model.Title;
                         lang_action.Summary = editlang_model.Summary;
                         lang_action.Body = editlang_model.Body;
                         lang_action.Route = editlang_model.Route;
                         lang_action.Keywords = editlang_model.Keywords;
                         lang_action.Tag = editlang_model.Tag;
                         lang_action.Description = editlang_model.Description;
                         language_SinglePageService.Update(lang_action);
                         return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                     }
                     else
                     {
                         language_SinglePageService.Create(editlang_model);
                         return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                     }

                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "PAGE", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var function = singlePageService.GetbyId(Id);
                 if (function == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 language_SinglePageService.Delete(Id);
                 singlePageService.Delete(Id);
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessDeleteFunction);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                 return Json(reponse, JsonRequestBehavior.AllowGet);
             }

         }
    }
}