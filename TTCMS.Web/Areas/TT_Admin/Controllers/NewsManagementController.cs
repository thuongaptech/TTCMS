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
    public class NewsManagementController : BaseController
    {
         private readonly ICategoryService categoryService;
         private readonly INewsService newsService;
         private readonly ILanguageService languageService;
         public NewsManagementController(ICategoryService categoryService,INewsService newsService, ILanguageService languageService)
        {
            this.categoryService = categoryService;
            this.newsService = newsService;
            this.languageService = languageService;
        }
         //
         // GET: /PageManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "NEWS", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult Index(int page = 1, string show = "", string search = "")
         {
             string langid = "";
             if (show != "")
             {
                 langid = languageService.GetForId(show).Id;
             }
             int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));

             var list = newsService.GetPageList(new Page(page, pageSize), search, langid);

             // map it to a paged list of models.
             var actionViewModel = Mapper.Map<IPagedList<News>, IPagedList<NewsViewModel>>(list);

             var table = new NewsTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_NewsTable", table);
             }
             var lang = languageService.GetListForActive();
             var model = new NewsPageViewModel();
             model.TableList = table;
             model.Lang = lang.ToSelectListItems();
             return View(model);
         }

        //
        // GET: /PageManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "NEWS", RoleID = "VIEW")]
        [HttpGet]
        public ActionResult Approves(int page = 1, string show = "", string search = "")
        {
            string langid = "";
            if (show != "")
            {
                langid = languageService.GetForId(show).Id;
            }
            int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));

            var list = newsService.GetPageListApproves(new Page(page, pageSize), search, langid);


            // map it to a paged list of models.
            var actionViewModel = Mapper.Map<IPagedList<News>, IPagedList<NewsViewModel>>(list);

            var table = new NewsTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NewsTableApproves", table);
            }
            var lang = languageService.GetListForActive();
            var model = new NewsPageViewModel();
            model.TableList = table;
            model.Lang = lang.ToSelectListItems();
            return View(model);
        }

        //
        // GET: /PageManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "NEWS", RoleID = "VIEW")]
        [HttpGet]
        public ActionResult ListPost(int page = 1, string show = "", string search = "")
        {
            string langid = "";
            if (show != "")
            {
                langid = languageService.GetForId(show).Id;
            }
            int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));

            var list = newsService.GetPageListApproves(new Page(page, pageSize), search, langid);


            // map it to a paged list of models.
            var actionViewModel = Mapper.Map<IPagedList<News>, IPagedList<NewsViewModel>>(list);

            var table = new NewsTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NewsTableListPost", table);
            }
            var lang = languageService.GetListForActive();
            var model = new NewsPageViewModel();
            model.TableList = table;
            model.Lang = lang.ToSelectListItems();
            return View(model);
        }

        [AuthorizeUser(FunctionID = "NEWS", RoleID = "CREATE")]
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
         ////
         //// GET: /PageManagement/Role/Create
         [AuthorizeUser(FunctionID = "NEWS", RoleID = "CREATE")]
         [HttpGet]
         public ActionResult Create()
         {
             var lang = languageService.GetListForActive().ToList();
             PopulateParentIDDropDownList();
             var model = new NewsViewModel();
             model.Published = DateTime.Now;
             lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
             model.Lang = new SelectList(lang.OrderByDescending(x => x.CreatedDate), "Id", "Name", "*");
             return View(model);
         }
    ////    //// ////
    ////    ////// // POST: /GroupMenuManagement/Role/Create

         [HttpPost, ValidateInput(false)]
         [ValidateAntiForgeryToken, Log("{model}")]
         [AuthorizeUser(FunctionID = "NEWS", RoleID = "CREATE")]
         public ActionResult Create(NewsViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.CreatedDate = DateTime.Now;
                     model.UpdatedDate = DateTime.Now;
                     model.CreatedBy = User.Identity.Name;
                     model.UpdatedBy = User.Identity.Name;
                     News modelCreate = Mapper.Map<NewsViewModel, News>(model);
                    if (model.IsHotCategoryIds.Count() > 0)
                    {
                        modelCreate.StrIsHotCategoryIds = String.Join(",", model.IsHotCategoryIds);
                    }
                    if (model.CategoryIds.Count() > 0)
                    {
                        modelCreate.StrCategoryIds = String.Join(",", model.CategoryIds);
                    }
                    var result = newsService.Create(modelCreate);
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }

                 var listlang = languageService.GetListForActive().ToList();
                 listlang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                 model.Lang = new SelectList(listlang.OrderByDescending(x => x.CreatedDate), "Id", "Name", model.LanguageId);
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Message ", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }

         [AuthorizeUser(FunctionID = "NEWS", RoleID = "EDIT")]
         public ActionResult Edit(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 var editmodel = newsService.GetbyId(Id);
                 if (editmodel == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     NewsViewModel model = Mapper.Map<News, NewsViewModel>(editmodel);
                     if (editmodel.StrCategoryIds != null)
                    {
                        model.CategoryIds = editmodel.StrCategoryIds.Split(',');
                    }
                    if (editmodel.StrIsHotCategoryIds != null)
                    {
                        model.IsHotCategoryIds = editmodel.StrIsHotCategoryIds.Split(',');
                    }
                    var lang = languageService.GetListForActive().ToList();
                     lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                     model.Lang = new SelectList(lang.OrderByDescending(x => x.CreatedDate), "Id", "Name", model.LanguageId);
                     PopulateParentIDDropDownList(model.CategoryId);
                     return View(model);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "NEWS", RoleID = "EDIT"), ValidateInput(false)]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(NewsViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     model.UpdatedBy = User.Identity.Name;
                     News modelCreate = Mapper.Map<NewsViewModel, News>(model);
                    if (model.IsHotCategoryIds.Count() > 0)
                    {
                        modelCreate.StrIsHotCategoryIds = String.Join(",", model.IsHotCategoryIds);
                    }
                    if (model.CategoryIds.Count() > 0)
                    {
                        modelCreate.StrCategoryIds = String.Join(",", model.CategoryIds);
                    }
                    newsService.Update(modelCreate);
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 var lang = languageService.GetListForActive().ToList();
                 lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                 model.Lang = new SelectList(lang.OrderByDescending(x => x.CreatedDate), "Id", "Name", model.LanguageId);
                 PopulateParentIDDropDownList(model.CategoryId);
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "NEWS", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var function = newsService.GetbyId(Id);
                 if (function == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 newsService.Delete(Id);
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

        [AuthorizeUser(FunctionID = "NEWS", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult IsApprove(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = newsService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                function.IsApprove = 1;
                newsService.Update(function);
                if (Request.IsAjaxRequest())
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = "Duyệt bản tin thành công" };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index").WithSuccess("Duyệt bản tin thành công");
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }

        }

        [AuthorizeUser(FunctionID = "NEWS", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult UnApprove(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = newsService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                function.IsApprove = 0;
                newsService.Update(function);
                if (Request.IsAjaxRequest())
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = "Bỏ duyệt bản tin thành công" };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index").WithSuccess("Bỏ duyệt bản tin thành công");
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }

        }

        [AuthorizeUser(FunctionID = "NEWS", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult IsPost(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = newsService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                function.IsPost = 1;
                newsService.Update(function);
                if (Request.IsAjaxRequest())
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = "Đăng bài thành công" };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index").WithSuccess("Đăng bài thành công");
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                return Json(reponse, JsonRequestBehavior.AllowGet);
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
                 //add the parent category to the item list
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
                 //now get all its children (separate function in case you need recursion)
                 GetSubTree(all.ToList(), cate, items);
             }
             return items;
         }
         private void GetSubTree(IList<Category> allCats, CategoryViewModel parent, IList<CategoryViewModel> items)
         {
             var subCats = allCats.Where(c => c.ParentID == parent.Id);
             foreach (var cat in subCats)
             {
                 //add this category
                 var cate = new CategoryViewModel()
                 {
                     Id = cat.Id,
                     Name = parent.Name + " >> " + cat.Name,
                     Description = cat.Name,
                     Order = cat.Order,
                     IsActive = cat.IsActive,
                     CreatedDate = cat.CreatedDate
                 };
                 items.Add(cate);
                 //recursive call in case your have a hierarchy more than 1 level deep
                 GetSubTree(allCats, cate, items);
             }
         }
    }
}