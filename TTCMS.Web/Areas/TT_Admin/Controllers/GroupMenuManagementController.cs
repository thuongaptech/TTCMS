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
    public class GroupMenuManagementController : BaseController
    {
         private readonly IMenuGroupService menuGroupService;
         private readonly ILanguage_MenuGroupService language_MenuGroupService;
         private readonly ILanguageService languageService;
         public GroupMenuManagementController(IMenuGroupService menuGroupService, ILanguage_MenuGroupService language_MenuGroupService, ILanguageService languageService)
        {
            this.menuGroupService = menuGroupService;
            this.language_MenuGroupService = language_MenuGroupService;
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
                         IsYes = language_MenuGroupService.Check(item.Id, Id)
                     });
                 }
                 return PartialView("_AjaxTabLanguge", model.ToArray());
             }
             catch (Exception ex)
             {
                 HandleException("_TabLanguge Group menu", ex);
                 return null;
             }
         }
         //
         // GET: /GroupMenuManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "GROUPMENU ", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult Index(int page = 1, string show = "", string search="")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
             var list = menuGroupService.GetPageList(new Page(page, pageSize), search);
             // map it to a paged list of models.
             var actionViewModel = Mapper.Map<IPagedList<MenuGroup>, IPagedList<GroupMenuViewModel>>(list);
             foreach (var item in actionViewModel)
             {
                 var texts = Resources.Resources.NoUpdates;
                 var lang_model = language_MenuGroupService.GetbyId(cultureName, item.Id);
                 item.Name = lang_model == null ? texts : lang_model.Name;
                 item.Description = lang_model == null ? texts : lang_model.Description;
             }
             var table = new GroupMenuTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_GroupMenuTable", table);
             }
              var lang = languageService.GetListForActive();
              var model = new GroupMenuPageViewModel();
              model.TableList = table;
             model.Lang = lang.ToSelectListItems();
             return View(model);
         }
         ////
         //// GET: /GroupMenuManagement/Role/Create
         [AuthorizeUser(FunctionID = "GROUPMENU", RoleID = "CREATE")]
         [HttpGet]
         public ActionResult Create()
         {
             var lang = languageService.GetListForActive();
             var model = new GroupMenuFormModel();
             model.Order = menuGroupService.GetSort();
             model.Lang = new SelectList(lang, "Id", "Name");
             return Json(JsonSuccess(null, null, RenderPartialViewToString("Create", model)), JsonRequestBehavior.AllowGet);
         }
         ////
        // // POST: /GroupMenuManagement/Role/Create

         [HttpPost]
         [ValidateAntiForgeryToken, Log("{model}")]
         [AuthorizeUser(FunctionID = "GROUPMENU", RoleID = "CREATE")]
         public ActionResult Create(GroupMenuFormModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                         model.CreatedDate = DateTime.Now;
                         model.UpdatedDate = DateTime.Now;
                         model.CreatedBy = User.Identity.Name;
                         model.UpdatedBy = User.Identity.Name;
                         MenuGroup modelCreate = Mapper.Map<GroupMenuFormModel, MenuGroup>(model);
                         var result = menuGroupService.Create(modelCreate);
                         if (model.LanguageId == null)
                         {
                             var lang = languageService.GetListForActive();
                             foreach (var item in lang)
                             {
                                 Language_MenuGroup lang_model = Mapper.Map<GroupMenuFormModel, Language_MenuGroup>(model);
                                 lang_model.LanguageId = item.Id;
                                 lang_model.MenuGroupId = result.Id;
                                 language_MenuGroupService.Create(lang_model);
                             }
                         }
                         else
                         {
                             Language_MenuGroup lang_model = Mapper.Map<GroupMenuFormModel, Language_MenuGroup>(model);
                             lang_model.LanguageId = model.LanguageId;
                             lang_model.MenuGroupId = result.Id;
                             language_MenuGroupService.Create(lang_model);
                         }
                         return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }

                 var listlang = languageService.GetListForActive();
                 model.Lang = new SelectList(listlang, "Id", "Name");
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }
         }
         [AuthorizeUser(FunctionID = "GROUPMENU", RoleID = "EDIT")]
         public ActionResult Edit(int Id= 0, string Lang = "")
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 if (Lang != "")
                 {
                     cultureName = Lang;
                 }
                 var group = menuGroupService.GetbyId(Id);
                 if (group == null)
                 {
                     return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     var lang_model = language_MenuGroupService.GetbyId(cultureName, Id);
                     GroupMenuFormModel model = Mapper.Map<GroupMenuFormModel>(new Tuple<MenuGroup, Language_MenuGroup>(group, lang_model));
                     if (model.LanguageId == null)
                     {
                         model.LanguageId = cultureName;
                     }
                     var lang = languageService.GetListForActive();
                     model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
                     
                     return Json(JsonSuccess(null, null, RenderPartialViewToString("Edit", model)), JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "GROUPMENU", RoleID = "EDIT")]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(GroupMenuFormModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedBy = User.Identity.Name;
                     MenuGroup modelCreate = Mapper.Map<GroupMenuFormModel, MenuGroup>(model);
                     menuGroupService.Update(modelCreate);
                     //change lang_fun
                     var lang_action = language_MenuGroupService.GetbyId(model.LanguageId, model.Id);
                     var editlang_model = Mapper.Map<GroupMenuFormModel, Language_MenuGroup>(model);
                     editlang_model.MenuGroupId = model.Id;
                     if (lang_action != null)
                     {
                         lang_action.Name = editlang_model.Name;
                         lang_action.Description = editlang_model.Description;
                         language_MenuGroupService.Update(lang_action);
                         return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
                     }
                     else
                     {

                         language_MenuGroupService.Create(editlang_model);
                         return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
                     }

                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }

         }
         [AuthorizeUser(FunctionID = "GROUPMENU", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var function = menuGroupService.GetbyId(Id);
                 if (function == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 language_MenuGroupService.Delete(Id);
                 menuGroupService.Delete(Id);
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