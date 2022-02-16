using TTCMS.Domain;
using TTCMS.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using TTCMS.Core;
using TTCMS.Service;
using TTCMS.Web.Areas.TT_Admin.Models;
using PagedList;
using TTCMS.Data.Infrastructure;
using System.Linq.Expressions;
using TTCMS.Web.Extensions;
using AutoMapper;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.Filters;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Service.Common;
using System.Drawing;
using System.IO;
using System.Net;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
     [Authorize]
    public class RoleManagementController : BaseController
    {
         private readonly ILanguageService languageService;
         private readonly ILanguage_RoleService language_RoleService;
         public RoleManagementController(ILanguageService languageService, ILanguage_RoleService language_RoleService)
        {
            this.language_RoleService = language_RoleService;
            this.languageService = languageService;
        }
         private ApplicationUserManager UserManager
         {
             get
             {
                 return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
             }
         }
         private ApplicationRoleManager RoleManager
         {
             get
             {
                 return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
             }
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
                         IsYes = language_RoleService.Language_RoleCheck(item.Id, Id)
                     });
                 }
                 return PartialView(model.ToArray());
             }
             catch (Exception ex)
             {
                 HandleException("_TabLanguge Role", ex);
                 return null;
             }
         }
         //
         // GET: /RoleManagement/Index/
         //[OutputCache(Duration = 60)]
         [HttpGet]
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "VIEW")]
         public ActionResult Index(int? page = 1, string show = "", string search = "")
         {
             try
             {
                 if (show != "")
                 {
                     cultureName = show;
                 }
                 int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                 int pageNumber = (page ?? 1);
                 //role list
                 var role = RoleManager.Roles;
                 //user
                 if (search != "")
                 {
                     role = role.Where(x => x.Language_Roles.Any(s=>s.Description.Contains(search)) || x.Name.Contains(search));
                 }
                 var model = new List<RoleViewModel>();
                 foreach (var item in role)
                 {
                     var texts = Resources.Resources.NoUpdates;
                     var lang_role = language_RoleService.Language_Role(cultureName, item.Id);
                     if (lang_role != null)
                         texts = lang_role.Description;
                     //add the parent category to the item list
                     var cate = new RoleViewModel()
                     {
                         Id = item.Id,
                         Name = item.Name,
                         Description = texts,
                         CreatedDate = item.CreatedDate,
                         IsActived = item.IsActived
                     };
                     model.Add(cate);
                 }
                 var rolePage = new RoleTableViewModel { RoleList = model.ToPagedList(pageNumber, pageSize), Search = search, Show = show };
                 var lang = languageService.GetListForActive();
                 var modelPage = new RolePageViewModel { RoleTableList = rolePage, Lang = lang.ToSelectListItems() };
                 if (Request.IsAjaxRequest())
                 {
                     return PartialView("_RoleTable", rolePage);
                 }
                 return View(modelPage);
             }
             catch (Exception ex)
             {
                 HandleException("List Role", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }
         
         // GET: /Admin/UserGroup/Create
         [HttpGet]
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "CREATE")]
         public ActionResult Create()
         {
             var lang = languageService.GetListForActive();
             var model = new RoleViewModel();
             model.Lang = lang.ToSelectListItems();
             return View(model);
         }
         ////
         //// POST: /Admin/UserGroup/Create
         [HttpPost]
         [ValidateAntiForgeryToken]
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "CREATE"), Log("{model}")]
         public async Task<ActionResult> Create(RoleViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     var newrole = new ApplicationRole
                     {
                         Name = model.Name,
                         IsActived = model.IsActived,
                         CreatedDate = DateTime.Now,
                         UpdatedDate = DateTime.Now,
                         CreatedBy = User.Identity.Name,
                         UpdatedBy = User.Identity.Name,
                         IsDeleted = true
                     };
                     if (!RoleManager.RoleExists(model.Name))
                     {
                         IdentityResult result = await RoleManager.CreateAsync(newrole);
                         if (result.Succeeded)
                         {
                             string roleid = RoleManager.FindByName(model.Name).Id;

                             Language_Role lang_role = Mapper.Map<RoleViewModel, Language_Role>(model);
                             if (model.LanguageId == null)
                             {
                                 var lang = languageService.GetListForActive();
                                 foreach (var item in lang)
                                 {
                                     lang_role.LanguageId = item.Id;
                                     lang_role.RoleId = RoleManager.FindByName(model.Name).Id;
                                     language_RoleService.CreateLanguage_Role(lang_role);
                                 }
                             }
                             else
                             {
                                 lang_role.LanguageId = model.LanguageId;
                                 lang_role.RoleId = roleid;
                                 language_RoleService.CreateLanguage_Role(lang_role);
                             }
                             return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                         }
                     }
                     else
                     {
                         return RedirectToAction("Index").WithWarning(Resources.Resources.ErrorRole);
                     }
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 var ListItemlang = languageService.GetListForActive();
                 model.Lang = ListItemlang.ToSelectListItems(model.LanguageId);
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Create group", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }
         [HttpGet]
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "EDIT")]
         public ActionResult Edit(string Id, string Lang)
         {
             if (Id == null)
             {
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
             }
             if (Lang != null)
             {
                 cultureName = Lang;
             }
             var role = RoleManager.FindById(Id);
             if (role == null)
             {
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
             }
             else
             {
                 var lang_role = language_RoleService.Language_Role(cultureName, Id);
                 RoleViewModel model = Mapper.Map<RoleViewModel>(new Tuple<ApplicationRole, Language_Role>(role, lang_role));
                 if (model.LanguageId == null)
                 {
                     model.LanguageId = cultureName;
                 }
                 //get selectlist language
                 var lang = languageService.GetListForActive();
                 model.Lang = lang.ToSelectListItems(model.LanguageId);
                 return View(model);
             }
             
         }
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "EDIT")]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public async Task<ActionResult> Edit(RoleViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedBy = User.Identity.Name;
                     var roleedit = await RoleManager.FindByIdAsync(model.Id);
                     if (roleedit != null)
                     {
                         roleedit.Name = model.Name;
                         roleedit.IsActived = model.IsActived;
                         roleedit.UpdatedDate = DateTime.Now;
                         roleedit.UpdatedBy = User.Identity.Name;
                     }
                     var adminresult = await this.RoleManager.UpdateAsync(roleedit);
                     if (adminresult.Succeeded)
                     {
                         var lang_role = language_RoleService.Language_Role(model.LanguageId, model.Id);
                         if (lang_role == null)
                         {
                             var newlang = new Language_Role()
                             {
                                 Description = model.Description,
                                 RoleId = model.Id,
                                 LanguageId = model.LanguageId
                             };
                             language_RoleService.CreateLanguage_Role(newlang);
                             return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                         }
                         else
                         {
                             lang_role.Description = model.Description;
                             language_RoleService.UpdateLanguage_Role(lang_role);
                             return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                         }
                     }
                     else
                     {
                         ModelState.AddModelError("", Resources.Resources.WarningData);
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
         [AuthorizeUser(FunctionID = "ROLE", RoleID = "DELETE")]
         [Log("{Id}")]
         public async Task<ActionResult> Delete(string Id)
         {
             try
             {
                 if (Id == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var role = await RoleManager.FindByIdAsync(Id);
                 if (role == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 if (role.IsDeleted == false)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithWarning, Msg = Resources.Resources.AlertDataSystem };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 language_RoleService.DeleteLanguage_RoleForRole(Id);
                 IdentityResult result;
                 result = await RoleManager.DeleteAsync(role);
                 if (!result.Succeeded)
                 {
                     ModelState.AddModelError("", result.Errors.First());
                 }
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 return RedirectToAction("Index").WithSuccess(Resources.Resources.AlertDataSuccess);
             }
             catch (Exception ex)
             {
                 HandleException("Delete role", ex);
                 var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                 return Json(reponse, JsonRequestBehavior.AllowGet);
             }

         }
    }
}