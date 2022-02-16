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
    public class SlideManagementController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly ILanguageService languageService;
        private readonly ISlideService slideService;
        public SlideManagementController(ISlideService slideService,ICategoryService categoryService, ILanguageService languageService)
        {
            this.slideService = slideService;
            this.categoryService = categoryService;
            this.languageService = languageService;
        }
        //
        // GET: /GroupMenuManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "SLIDE ", RoleID = "VIEW")]
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

            var table = new SlideTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_SlideCateTable", table);
            }
            var lang = languageService.GetListForActive();
            var model = new SlidePageViewModel();
            model.TableList = table;
            model.Lang = lang.ToSelectListItems();
            return View(model);
        }
        //    // ////
        //    // //// GET: /GroupMenuManagement/Role/Create
        [AuthorizeUser(FunctionID = "SLIDE", RoleID = "CREATE")]
        [HttpGet]
        public ActionResult Create()
        {
            var lang = languageService.GetListForActive().ToList();
            var model = new SlideViewModel();
            model.Order = slideService.GetSort(SlideType.Slide);
            lang.Add(new Language { Id = "*", Name = TTCMS.Resources.Resources.AllLanguage, CreatedDate=DateTime.Now});
            model.Lang = new SelectList(lang.OrderByDescending(x=>x.CreatedDate), "Id", "Name","*");
            return Json(JsonSuccess(null, null, RenderPartialViewToString("Create", model)), JsonRequestBehavior.AllowGet);
        }
        //    //// ////
        //    ////// // POST: /GroupMenuManagement/Role/Create

        [HttpPost]
        [ValidateAntiForgeryToken, Log("{model}")]
        [AuthorizeUser(FunctionID = "SLIDE", RoleID = "CREATE")]
        public ActionResult Create(SlideViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;
                    model.UpdatedBy = User.Identity.Name;
                    model.SlideType = SlideType.Slide;
                    Slide modelCreate = Mapper.Map<SlideViewModel, Slide>(model);
                    var result = slideService.Create(modelCreate);
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
        [AuthorizeUser(FunctionID = "SLIDE", RoleID = "EDIT")]
        public ActionResult Edit(int Id = 0)
        {
            try
            {
                if (Id == 0)
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                var group = slideService.GetbyId(Id);
                if (group == null)
                {
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SlideViewModel model = Mapper.Map<Slide, SlideViewModel>(group);
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
        [AuthorizeUser(FunctionID = "SLIDE", RoleID = "EDIT")]
        [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
        public ActionResult Edit(SlideViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = User.Identity.Name;
                    Slide modelCreate = Mapper.Map<SlideViewModel, Slide>(model);
                    slideService.Update(modelCreate);
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
        [AuthorizeUser(FunctionID = "SLIDE", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult Delete(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = slideService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                slideService.Delete(Id);
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
        private List<SlideViewModel> GetFunctionViewModel(object selfId = null, string show = "")
        {
            var all = slideService.GetList(SlideType.Slide).OrderBy(x => x.Order).ToList();
            List<SlideViewModel> items = new List<SlideViewModel>();

            if (selfId != null)
                all = all.Where(x => x.Id != (int)selfId).ToList();
            if (show != "")
            {
                cultureName = show;
            }
            //get parent categories
            foreach (var cat in all)
            {
                //add the parent category to the item list
                var cate = new SlideViewModel()
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Description = cat.Description,
                    Images = cat.Images,
                    NgonNgu = cat.LanguageId== "*"?TTCMS.Resources.Resources.AllLanguage:languageService.GetForId(cat.LanguageId).Name,
                    Order = cat.Order,
                    IsActive = cat.IsActive,
                    CreatedDate = cat.CreatedDate
                };
                items.Add(cate);
                //now get all its children (separate function in case you need recursion)
            }
            return items;
        }
    }
}