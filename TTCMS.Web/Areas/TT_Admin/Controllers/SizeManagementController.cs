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
    public class SizeManagementController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly ILanguageService languageService;
        public SizeManagementController(ICategoryService categoryService, ILanguageService languageService)
        {
            this.categoryService = categoryService;
            this.languageService = languageService;
        }
        //
        // GET: /GroupMenuManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "SIZE ", RoleID = "VIEW")]
        [HttpGet]
        public ActionResult Index(int? page = 1, string show = "", string search = "")
        {
            if (show != "")
            {
                cultureName = show;
            }
            int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
            int pageNumber = (page ?? 1);
            // map it to a paged list of models.
            var actionViewModel = GetFunctionViewModel(null, show).ToPagedList(pageNumber, pageSize);
            var table = new CategoryTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SizeCateTable", table);
            }
            var lang = languageService.GetListForActive();
            var model = new CategoryPageViewModel();
            model.TableList = table;
            model.Lang = lang.ToSelectListItems();
            return View(model);
        }
        //    // ////
        //    // //// GET: /GroupMenuManagement/Role/Create
        [AuthorizeUser(FunctionID = "SIZE", RoleID = "CREATE")]
        [HttpGet]
        public ActionResult Create()
        {
            var lang = languageService.GetListForActive().ToList();
            var model = new CategoryViewModel();
            model.Order = categoryService.GetSort(CategoryType.Size);
            lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate=DateTime.Now});
            model.Lang = new SelectList(lang.OrderByDescending(x=>x.CreatedDate), "Id", "Name","*");
            return Json(JsonSuccess(null, null, RenderPartialViewToString("Create", model)), JsonRequestBehavior.AllowGet);
        }
        //    //// ////
        //    ////// // POST: /GroupMenuManagement/Role/Create

        [HttpPost]
        [ValidateAntiForgeryToken, Log("{model}")]
        [AuthorizeUser(FunctionID = "SIZE", RoleID = "CREATE")]
        public ActionResult Create(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;
                    model.Target = "_self";
                    model.Route = "/";
                    model.UpdatedBy = User.Identity.Name;
                    model.CategoryType = CategoryType.Size;
                    Category modelCreate = Mapper.Map<CategoryViewModel, Category>(model);
                    var result = categoryService.Create(modelCreate);
                    return Json(JsonSuccess(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }

                var listlang = languageService.GetListForActive().ToList();
                listlang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                model.Lang = new SelectList(listlang.OrderByDescending(x => x.CreatedDate), "Id", "Name", model.LanguageId);
                return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HandleException("Message ", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }
        }
        [AuthorizeUser(FunctionID = "SIZE", RoleID = "EDIT")]
        public ActionResult Edit(int Id = 0)
        {
            try
            {
                if (Id == 0)
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                var group = categoryService.GetbyId(Id);
                if (group == null)
                {
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CategoryViewModel model = Mapper.Map<Category, CategoryViewModel>(group);
                    var lang = languageService.GetListForActive().ToList();
                    lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                    model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
                    return Json(JsonSuccess(null, null, RenderPartialViewToString("Edit", model)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }

        }
        [AuthorizeUser(FunctionID = "SIZE", RoleID = "EDIT")]
        [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
        public ActionResult Edit(CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = User.Identity.Name;
                    Category modelCreate = Mapper.Map<CategoryViewModel, Category>(model);
                    categoryService.Update(modelCreate);
                    return Json(JsonSuccess(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                var lang = languageService.GetListForActive().ToList();
                lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate = DateTime.Now });
                model.Lang = new SelectList(lang, "Id", "Name", model.LanguageId);
                return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }

        }
        [AuthorizeUser(FunctionID = "SIZE", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult Delete(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = categoryService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                categoryService.Delete(Id);
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
        private void PopulateParentIDDropDownList(object selectedParent = null, object selfId = null)
        {
            var items = GetFunctionViewModel(selfId);
            var abc = new SelectList(items, "Id", "Name", selectedParent);
            ViewBag.Parents = abc;
        }
        private List<CategoryViewModel> GetFunctionViewModel(object selfId = null, string show = "")
        {
            var all = categoryService.GetList(CategoryType.Size).OrderBy(x => x.Order).ToList();
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
                    NgonNgu = cat.LanguageId== "*"?TTCMS.Resources.Resources.AllLanguage:languageService.GetForId(cat.LanguageId).Name,
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
                    NgonNgu = cat.LanguageId == "*" ? TTCMS.Resources.Resources.AllLanguage : languageService.GetForId(cat.LanguageId).Name,
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