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
using Newtonsoft.Json;
using SocialGoal.Web.Core.Extensions;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
     [Authorize]
    public class MenuManagementController : BaseController
    {
        private readonly ICategoryService categoryService;
         private readonly IMenuService menuService;
         private readonly ISinglePageService singlePageService;
         private readonly ILanguage_SinglePageService language_SinglePageService;
         private readonly ILanguageService languageService;
         public MenuManagementController(ICategoryService categoryService,ILanguage_SinglePageService language_SinglePageService,ISinglePageService singlePageService, IMenuService menuService, ILanguageService languageService)
        {
            this.categoryService = categoryService;
            this.singlePageService = singlePageService;
            this.language_SinglePageService = language_SinglePageService;
            this.menuService = menuService;
            this.languageService = languageService;
        }
         //
         // GET: /GroupMenuManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult Index(string group = "", string show = "")
         {
             var model = new MenuTableViewModel();
             model.Group = SelectExtensions.ToSelectList(typeof(MenuGroupType), group);
             var lang = languageService.GetListForActive();
             model.Lang = lang.ToSelectListItems(show);
             return View(model);
         }
          [AuthorizeUser(FunctionID = "MENU", RoleID = "VIEW")]
         public ActionResult _AjaxLoadMenu(string group= "", string show="")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             var model = Mapper.Map<IEnumerable<Menu>, IEnumerable<MenuManagerViewModel>>(menuService.GetList((MenuGroupType)Enum.Parse(typeof(MenuGroupType), group), cultureName));
             return PartialView(model.ToArray());
         }
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         public ActionResult UpMenuList(MenuJsonList[] model)
         {
             UpSortMenu(model);
             return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
         }
          [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         private void UpSortMenu(MenuJsonList[] model)
         {
             int icount = 1;
             foreach (var item in model)
             {
                 var menu = menuService.GetbyId(item.id);
                 menu.Order = icount;
                 menu.ParentID = 0;
                 menuService.Update(menu);
                 if (item.children !=  null)
                 {
                   icount = TreeUpMenu(item.children,item.id,icount);
                 }
                 icount++;
             }
         }
         private int TreeUpMenu(IEnumerable<MenuJsonList> model, int parent, int icount)
         {
             int idem = icount;
             foreach (var item in model)
             {
                 idem++;
                 var menu = menuService.GetbyId(item.id);
                 menu.Order = idem;
                 menu.ParentID = parent;
                 menuService.Update(menu);
                 if (item.children != null)
                 {
                     TreeUpMenu(item.children, item.id, idem);
                 }
                
             }
             return idem;
         }
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         public ActionResult UpPage(int[] ids, string group= "", string show ="")
         {
             try
             {
                 if (show != "")
                 {
                     cultureName = show;
                 }
                 if (ids.Length > 0)
                 {
                     foreach (var item in ids)
                     {
                       var page = singlePageService.GetbyId(item);
                       var lang_page = language_SinglePageService.GetbyId(cultureName, item);
                       var menu = new Menu()
                       {
                           Name = lang_page.Title,
                           Link = lang_page.Route,
                           WithId = item,
                           Action = "PageDetail",
                           Controller = "Page",
                           ParentID = 0,
                           Target = "_self",
                           TextType = "Page",
                           CreatedBy = User.Identity.Name,
                           CreatedDate=DateTime.Now,
                           UpdatedBy= User.Identity.Name,
                           UpdatedDate=DateTime.Now,
                           IsActived= true,
                           LanguageId = cultureName,
                           GroupType = (MenuGroupType)Enum.Parse(typeof(MenuGroupType), group),
                           Order = menuService.GetSort((MenuGroupType)Enum.Parse(typeof(MenuGroupType), group),cultureName)
                       };
                       menuService.Create(menu);
                     }
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                 }
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }
             
         }
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         public ActionResult UpNewsCate(int[] ids, string group = "", string show = "")
         {
             try
             {
                 if (show != "")
                 {
                     cultureName = show;
                 }
                 if (ids.Length > 0)
                 {
                     foreach (var item in ids)
                     {
                         var cate = categoryService.GetbyId(item);
                         var menu = new Menu()
                         {
                             Name = cate.Name,
                             Link = cate.Route,
                             WithId = item,
                             Action = "Index",
                             Controller ="Page",
                             ParentID = 0,
                             Target = "_self",
                             TextType = "News Category",
                             CreatedBy = User.Identity.Name,
                             CreatedDate = DateTime.Now,
                             UpdatedBy = User.Identity.Name,
                             UpdatedDate = DateTime.Now,
                             IsActived = true,
                             LanguageId = cultureName,
                             GroupType = (MenuGroupType)Enum.Parse(typeof(MenuGroupType), group),
                             Order = menuService.GetSort((MenuGroupType)Enum.Parse(typeof(MenuGroupType), group), cultureName)
                         };
                         menuService.Create(menu);
                     }
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                 }
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }

         }
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         public ActionResult UpProCate(int[] ids, string group = "", string show = "")
         {
             try
             {
                 if (show != "")
                 {
                     cultureName = show;
                 }
                 if (ids.Length > 0)
                 {
                     foreach (var item in ids)
                     {
                         var cate = categoryService.GetbyId(item);
                         var menu = new Menu()
                         {
                             CssClass = cate.CssClass,
                             Name = cate.Name,
                             Link = cate.Route,
                             WithId = item,
                             Action = "Category",
                             Controller = "Product",
                             ParentID = 0,
                             Target = "_self",
                             TextType = "Product Category",
                             CreatedBy = User.Identity.Name,
                             CreatedDate = DateTime.Now,
                             UpdatedBy = User.Identity.Name,
                             UpdatedDate = DateTime.Now,
                             IsActived = true,
                             LanguageId = cultureName,
                             GroupType = (MenuGroupType)Enum.Parse(typeof(MenuGroupType), group),
                             Order = menuService.GetSort((MenuGroupType)Enum.Parse(typeof(MenuGroupType), group), cultureName)
                         };
                         menuService.Create(menu);
                     }
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                 }
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }

         }
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "CREATE")]
         public ActionResult UpCustomLink(string url = "", string item = "", string group = "", string show = "")
         {
             try
             {
                 if (show != "")
                 {
                     cultureName = show;
                 }
                 if (url != "" && item != "")
                 {
                     var menu = new Menu()
                     {
                         Name = item,
                         Link = url,
                         ParentID = 0,
                         Target = "_self",
                         TextType = "Custom Link",
                         CreatedBy = User.Identity.Name,
                         CreatedDate = DateTime.Now,
                         UpdatedBy = User.Identity.Name,
                         UpdatedDate = DateTime.Now,
                         IsActived = true,
                         LanguageId = cultureName,
                         GroupType = (MenuGroupType)Enum.Parse(typeof(MenuGroupType), group),
                         Order = menuService.GetSort((MenuGroupType)Enum.Parse(typeof(MenuGroupType), group), cultureName)
                     };
                     menuService.Create(menu);
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                 }
                 return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }

         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "VIEW")]
         public ActionResult _Page(string show = "")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             var actionViewModel = Mapper.Map<IEnumerable<SinglePage>, IEnumerable<PageViewModel>>(singlePageService.GetList().Where(x => x.IsActive == true));
             foreach (var item in actionViewModel)
             {
                 var lang_model = language_SinglePageService.GetbyId(cultureName, item.Id);
                 if (lang_model != null)
                 {
                     item.Title = lang_model.Title;
                     item.Route = lang_model.Route;
                 }

             }
             return PartialView(actionViewModel.ToArray());
         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "VIEW")]
         public ActionResult _NewsCate(string show = "")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             var actionViewModel = GetFunctionViewModel(null, show);
             return PartialView(actionViewModel.ToArray());
         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "VIEW")]
         public ActionResult _ProductCate(string show = "")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             var actionViewModel = GetFunctionViewModelProduct(null, show);
             return PartialView(actionViewModel.ToArray());
         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "DELETE")]
         public ActionResult Delete(int Id)
         {
             try
             {
                 if (Id == 0)
                 {
                     return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
                 }
                 var function = menuService.GetbyId(Id);
                 if (function == null)
                 {
                     return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
                 }
                 menuService.Delete(Id);
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.AlertDataSuccess), JsonRequestBehavior.AllowGet);
                 }
                 return Json(JsonError(Resources.Resources.AlertDataSuccess), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }

         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "EDIT")]
         public ActionResult Edit(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                     return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                 var model = menuService.GetbyId(Id);
                 if (model == null)
                 {
                     return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                 }
                 else
                 {
                     MenuManagerViewModel viewmodel = Mapper.Map<Menu, MenuManagerViewModel>(model);
                     return Json(JsonSuccess(null, null, RenderPartialViewToString("Edit", viewmodel)), JsonRequestBehavior.AllowGet);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }
         }
         [AuthorizeUser(FunctionID = "MENU", RoleID = "EDIT")]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(MenuManagerViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedBy = User.Identity.Name;
                     Menu modelEdit = Mapper.Map<MenuManagerViewModel, Menu>(model);
                     menuService.Update(modelEdit);
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
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
         [HttpPost]
         [AuthorizeUser(FunctionID = "MENU", RoleID = "EDIT")]
         public ActionResult _Activated(int Id = 0, bool key = true)
         {

             try
             {
                 if (Id == 0)
                 {
                     return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
                 }
                 var model = menuService.GetbyId(Id);
                 if (model == null)
                 {
                     return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
                 }
                 model.IsActived = key;
                 menuService.Update(model);
                 if (Request.IsAjaxRequest())
                 {
                     return Json(JsonSuccessWidthIndex(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
                 }
                 //báo lỗi ko requet ajax
                 throw new NotSupportedException(Resources.Resources.NotAjaxRequest);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }
         }
         private void PopulateParentIDDropDownList(object selectedParent = null, object selfId = null)
         {
             var items = GetFunctionViewModel(selfId);
             var abc = new SelectList(items, "Id", "Name", selectedParent);
             ViewBag.Parents = abc;
         }
         private List<CategoryViewModel> GetFunctionViewModel(object selfId = null, string show = "")
         {
             var all = categoryService.GetListbyActive(CategoryType.News,show).OrderBy(x => x.Order).ToList();
             List<CategoryViewModel> items = new List<CategoryViewModel>();

             if (selfId != null)
                 all = all.Where(x => x.Id != (int)selfId).ToList();
             if (show != "")
             {
                 cultureName = show;
             }
             //get parent categories
             IEnumerable<Category> parentFunctions = all.Where(c => c.ParentID == null).OrderBy(c => c.Order);

             foreach (var cat in parentFunctions)
             {
                 var cate = new CategoryViewModel()
                 {
                     Id = cat.Id,
                     Name = cat.Name,
                     Description = cat.Description,
                     Order = cat.Order,
                     IsActive = cat.IsActive,
                     CreatedDate = cat.CreatedDate
                 };
                 items.Add(cate);
                 GetSubTree(all.ToList(), cate, items);
                 //add the parent category to the item list

                 //now get all its children (separate function in case you need recursion)
             }
             return items;
         }
         private void GetSubTree(IList<Category> allCats, CategoryViewModel parent, IList<CategoryViewModel> items)
         {
             var subCats = allCats.Where(c => c.ParentID == parent.Id);
             foreach (var cat in subCats)
             {
                 var cate = new CategoryViewModel()
                 {
                     Id = cat.Id,
                     Name = parent.Name + " >> " + cat.Name,
                     Description = cat.Description,
                     Order = cat.Order,
                     IsActive = cat.IsActive,
                     CreatedDate = cat.CreatedDate
                 };
                 items.Add(cate);
                 GetSubTree(allCats, cate, items);
                 //add this category

                 //recursive call in case your have a hierarchy more than 1 level deep

             }
         }
         private List<CategoryViewModel> GetFunctionViewModelProduct(object selfId = null, string show = "")
         {
             var all = categoryService.GetListbyActive(CategoryType.Product, show).OrderBy(x => x.Order).ToList();
             List<CategoryViewModel> items = new List<CategoryViewModel>();

             if (selfId != null)
                 all = all.Where(x => x.Id != (int)selfId).ToList();
             if (show != "")
             {
                 cultureName = show;
             }
             //get parent categories
             IEnumerable<Category> parentFunctions = all.Where(c => c.ParentID == null).OrderBy(c => c.Order);

             foreach (var cat in parentFunctions)
             {
                 var cate = new CategoryViewModel()
                 {
                     Id = cat.Id,
                     Name = cat.Name,
                     Description = cat.Description,
                     Order = cat.Order,
                     IsActive = cat.IsActive,
                     CreatedDate = cat.CreatedDate
                 };
                 items.Add(cate);
                 GetSubTree(all.ToList(), cate, items);
                 //add the parent category to the item list

                 //now get all its children (separate function in case you need recursion)
             }
             return items;
         }
    }
}