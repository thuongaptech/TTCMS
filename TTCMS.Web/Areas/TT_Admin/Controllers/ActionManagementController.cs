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
    public class ActionManagementController : BaseController
    {
         private readonly IGActionService gactionService;
         private readonly ILanguage_GActionService language_GActionService;
         private readonly ILanguageService languageService;
         public ActionManagementController(IGActionService gactionService, ILanguage_GActionService language_GActionService, ILanguageService languageService)
        {
            this.languageService = languageService;
            this.language_GActionService = language_GActionService;
            this.gactionService = gactionService;
        }
         [ChildActionOnly]
         public ActionResult _TabLanguge(string Id)
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
                         ContentId = Id,
                         IsYes = language_GActionService.Language_GActionCheck(item.Id, Id)
                     });
                 }
                 return PartialView(model.ToArray());
             }
             catch (Exception ex)
             {
                 HandleException("_TabLanguge GAction", ex);
                 return null;
             }
         }
         //
         // GET: /ActionManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult Index(int page = 1, string show = "", string search="")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
             var action = gactionService.GetGAction(new Page(page, pageSize),search);
             // map it to a paged list of models.
             var actionViewModel = Mapper.Map<IPagedList<GAction>, IPagedList<ActionLangViewModel>>(action);
             foreach (var item in actionViewModel)
             {
                 var texts = Resources.Resources.NoUpdates;
                 var lang_model = language_GActionService.Language_GAction(cultureName, item.Id);
                 item.Name = lang_model == null ? texts : lang_model.Name;
                 item.Description = lang_model == null?texts: lang_model.Description;
             }
             actionViewModel.OrderBy(x=>x.Name);
             var table = new ActionLangTableViewModel {ModelList= actionViewModel, Show=show, Search=search };
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_ActionTable", table);
             }
              var lang = languageService.GetListForActive();
              var model = new ActionPageViewModel();
              model.TableList = table;
             model.Lang = lang.ToSelectListItems();
             return View(model);
         }
         //
         // GET: /ActionManagement/Role/Create
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "CREATE")]
          [HttpGet]
         public ActionResult Create()
         {
             var lang = languageService.GetListForActive();
             var model = new ActionLangViewModel();
             model.Lang = new SelectList(lang, "Id", "Name");
             return View(model);
         }
         //
          // POST: /ActionManagement/Role/Create

         [HttpPost]
         [ValidateAntiForgeryToken,Log("{model}")]
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "CREATE")]
          public ActionResult Create(ActionLangViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     if (!gactionService.CheckGAction(model.Id))
                     {
                         model.CreatedDate = DateTime.Now;
                         model.UpdatedDate = DateTime.Now;
                         model.CreatedBy = User.Identity.Name;
                         model.UpdatedBy = User.Identity.Name;
                         model.IsDeleted = true;
                         GAction newGAction = Mapper.Map<ActionLangViewModel, GAction>(model);
                         gactionService.CreateGAction(newGAction);
                         if (model.LanguageId == null)
                         {
                             var lang = languageService.GetListForActive();
                             foreach (var item in lang)
                             {
                                 Language_GAction lang_fun = Mapper.Map<ActionLangViewModel, Language_GAction>(model);
                                 lang_fun.LanguageId = item.Id;
                                 lang_fun.GActionId = model.Id;
                                 language_GActionService.CreateLanguage_GAction(lang_fun);
                             }
                         }
                         else
                         {
                             Language_GAction lang_fun = Mapper.Map<ActionLangViewModel, Language_GAction>(model);
                             lang_fun.LanguageId = model.LanguageId;
                             lang_fun.GActionId = model.Id;
                             language_GActionService.CreateLanguage_GAction(lang_fun);
                         }
                         return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                     }
                     else
                     {
                         ModelState.AddModelError("", Resources.Resources.WarningChongqingKey);
                     }
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
                 HandleException("Create role", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);

             }
         }
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "EDIT")]
         public ActionResult Edit(string Id, string Lang)
         {
             try {
                 if (Id == null)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 if (Lang != null)
                 {
                     cultureName = Lang;
                 }
                 var action = gactionService.GAction(Id);
                 if (action == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     var lang_action = language_GActionService.Language_GAction(cultureName, Id);
                     ActionLangViewModel model = Mapper.Map<ActionLangViewModel>(new Tuple<GAction, Language_GAction>(action, lang_action));
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
                 HandleException("Edit action", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
             
         }
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "EDIT")]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(ActionLangViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedBy = User.Identity.Name;
                     GAction editGaction = Mapper.Map<ActionLangViewModel, GAction>(model);
                     gactionService.UpdateGAction(editGaction);
                     //change lang_fun
                     var lang_action = language_GActionService.Language_GAction(model.LanguageId, model.Id);
                    var editlang_action = Mapper.Map<ActionLangViewModel, Language_GAction>(model);
                    editlang_action.GActionId = model.Id;
                     if (lang_action != null)
                     {
                         lang_action.Name = editlang_action.Name;
                         lang_action.Description = editlang_action.Description;
                         language_GActionService.UpdateLanguage_GAction(lang_action);
                         return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                     }
                     else
                     {

                         language_GActionService.CreateLanguage_GAction(editlang_action);
                         return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                     }
                     
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 var lang = languageService.GetListForActive();
                 model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Edit Function", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "GACTION", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(string Id)
         {
             try
             {
                 if (Id == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var function = gactionService.GAction(Id);
                 if (function == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 if (function.IsDeleted == false)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithWarning, Msg = Resources.Resources.AlertDataSystem };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 language_GActionService.DeleteLanguage_GActionForAction(Id);
                 gactionService.DeleteGAction(Id);
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessDeleteFunction);
             }
             catch (Exception ex)
             {
                 HandleException("Delete Function", ex);
                 var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                 return Json(reponse, JsonRequestBehavior.AllowGet);
             }

         }
    }
}