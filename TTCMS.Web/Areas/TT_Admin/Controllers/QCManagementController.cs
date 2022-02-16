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
using SocialGoal.Web.Core.Extensions;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class QCManagementController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly ILanguageService languageService;
        private readonly ISlideService slideService;
        private readonly IQuangCaoService quangCaoService;
        public QCManagementController(IQuangCaoService quangCaoService,ISlideService slideService, ICategoryService categoryService, ILanguageService languageService)
        {
            this.quangCaoService = quangCaoService;
            this.slideService = slideService;
            this.categoryService = categoryService;
            this.languageService = languageService;
        }
        //
        // GET: /GroupMenuManagement/Role/
        //[OutputCache(Duration = 60)]
        [AuthorizeUser(FunctionID = "QUANGCAO ", RoleID = "VIEW")]
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

            // map it to a paged list of models
            var table = new QuangCaoTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_QuangCaoCateTable", table);
            }
            var lang = languageService.GetListForActive();
            var model = new QuangCaoPageViewModel();
            model.TableList = table;
            return View(model);
        }
        //    // ////
        //    // //// GET: /GroupMenuManagement/Role/Create
        [AuthorizeUser(FunctionID = "QUANGCAO", RoleID = "CREATE")]
        [HttpGet]
        public ActionResult Create()
        {
            var model = new QuangCaoViewModel();
            model.Order = quangCaoService.GetSort();
            model.QCType = SelectExtensions.ToSelectList(typeof(QuangCaoType), "");
            return Json(JsonSuccess(null, null, RenderPartialViewToString("Create", model)), JsonRequestBehavior.AllowGet);
        }
        //    //// ////
        //    ////// // POST: /GroupMenuManagement/Role/Create

        [HttpPost]
        [ValidateAntiForgeryToken, Log("{model}")]
        [AuthorizeUser(FunctionID = "QUANGCAO", RoleID = "CREATE")]
        public ActionResult Create(QuangCaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;
                    model.UpdatedBy = User.Identity.Name;
                    model.LanguageId = "*";
                    Advertisements modelCreate = Mapper.Map<QuangCaoViewModel, Advertisements>(model);
                    var result = quangCaoService.Create(modelCreate);
                    return Json(JsonSuccess(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                return Json(JsonError(Resources.Resources.WarningData), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HandleException("Message ", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }
        }
        [AuthorizeUser(FunctionID = "QUANGCAO", RoleID = "EDIT")]
        public ActionResult Edit(int Id = 0)
        {
            try
            {
                if (Id == 0)
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                var group = quangCaoService.GetbyId(Id);
                if (group == null)
                {
                    return Json(JsonError(Resources.Resources.AlertDataNull), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    QuangCaoViewModel model = Mapper.Map<Advertisements, QuangCaoViewModel>(group);
                    model.QCType = SelectExtensions.ToSelectList(typeof(QuangCaoType), group.QuangCaoType.ToString());
                    return Json(JsonSuccess(null, null, RenderPartialViewToString("Edit", model)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                HandleException("Message", ex);
                return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
            }

        }
        [AuthorizeUser(FunctionID = "QUANGCAO", RoleID = "EDIT")]
        [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
        public ActionResult Edit(QuangCaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = User.Identity.Name;
                    Advertisements modelCreate = Mapper.Map<QuangCaoViewModel, Advertisements>(model);
                    quangCaoService.Update(modelCreate);
                    return Json(JsonSuccess(Url.Action("Index"), Resources.Resources.SuccessCreate), JsonRequestBehavior.AllowGet);
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
        [AuthorizeUser(FunctionID = "QUANGCAO", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult Delete(int Id = 0)
        {
            try
            {
                if (Id == 0)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = quangCaoService.GetbyId(Id);
                if (function == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                quangCaoService.Delete(Id);
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
        private List<QuangCaoViewModel> GetFunctionViewModel(object selfId = null, string show = "")
        {
            var all = quangCaoService.GetList().OrderBy(x => x.Order).ToList();
            List<QuangCaoViewModel> items = new List<QuangCaoViewModel>();

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
                var cate = new QuangCaoViewModel()
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Images = cat.Images,
                    GroupQC = Enum.GetName(typeof(QuangCaoType), cat.QuangCaoType),
                    NgonNgu = cat.LanguageId == "*" ? TTCMS.Resources.Resources.AllLanguage : languageService.GetForId(cat.LanguageId).Name,
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