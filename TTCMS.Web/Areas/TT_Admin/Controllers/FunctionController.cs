using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Core;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Areas.TT_Admin.Models;
using PagedList;
using AutoMapper;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.Filters;
using System.Net;
using TTCMS.Service.Common;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class FunctionController : BaseController
    {
        private readonly IPermissionService permissionService;
        private readonly ILanguage_FunctionService language_FunctionService;
        private readonly IFunctionService functionService;
        private readonly ILanguageService languageService;
        public FunctionController(IPermissionService permissionService,IFunctionService functionService, ILanguageService languageService, ILanguage_FunctionService language_FunctionService)
        {
            this.permissionService = permissionService;
            this.language_FunctionService = language_FunctionService;
            this.languageService = languageService;
            this.functionService = functionService;
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
                        IsYes = language_FunctionService.Language_FunctionCheck(item.Id, Id)
                    });
                }
                return PartialView(model.ToArray());
            }
            catch (Exception ex)
            {
                HandleException("_TabLanguge Function", ex);
                return null;
            }
        }
        // GET: TT_Admin/Function
        [HttpGet]
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "VIEW")]
        public ActionResult Index(int? page = 1, string show = "")
        {
            try
            {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                int pageNumber = (page ?? 1);
                IEnumerable<LanguageItemViewModel> lang = Mapper.Map<IEnumerable<Language>, IEnumerable<LanguageItemViewModel>>(languageService.GetListForActive());
                var listlang = lang.Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                });
                var listfunction = GetFunctionViewModel(null, show).ToPagedList(pageNumber, pageSize);
                var tableList = new ListTableViewModel { FunctionList = listfunction, Page = pageNumber, Show=show };
                var model = new FunctionPageViewModel { TableList = tableList, Lang = listlang };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_FunctionTable", tableList);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List Function", ex);
                return RedirectToAction("Index", "Home").WithError(Resources.Resources.ErrorCatch);
            }
        }
        [HttpGet]
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "CREATE")]
        public ActionResult Create()
        {
            PopulateParentIDDropDownList();
            var lang = languageService.GetListForActive();
            var model = new FunctionFormModel();
            model.Order = functionService.GetFunctionSort();
            model.Lang = new SelectList(lang, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken, Log("{function}")]
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "CREATE")]
        public ActionResult Create(FunctionFormModel function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (functionService.CanAddFunction(function.Id))
                    {
                        function.CreatedDate = DateTime.Now;
                        function.UpdatedDate = DateTime.Now;
                        function.UpdatedBy = User.Identity.Name;
                        function.CreatedBy = User.Identity.Name;
                        function.IsDeleted = true;
                        Function newFunction = Mapper.Map<FunctionFormModel, Function>(function);
                        functionService.CreateFunction(newFunction);
                        if (function.LanguageId == null)
                        {
                            var lang = languageService.GetListForActive();
                            foreach (var item in lang)
                            {
                                Language_Function lang_fun = Mapper.Map<FunctionFormModel, Language_Function>(function);
                                lang_fun.LanguageId = item.Id;
                                lang_fun.FunctionId = function.Id;
                                language_FunctionService.CreateLanguage_Function(lang_fun);
                            }
                        }
                        else
                        {
                            Language_Function lang_fun = Mapper.Map<FunctionFormModel, Language_Function>(function);
                            lang_fun.LanguageId =  function.LanguageId;
                            lang_fun.FunctionId = function.Id;
                            language_FunctionService.CreateLanguage_Function(lang_fun);
                        }
                        return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessAddFunction);
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
                PopulateParentIDDropDownList(function.ParentID);
                var langs = languageService.GetListForActive();
                function.Lang = new SelectList(langs, "Id", "Name", function.LanguageId);
                return View(function).WithWarning(Resources.Resources.WarningData);
            }
            catch (Exception ex)
            {
                HandleException("Create Function", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
            
        }
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "EDIT")]
        public ActionResult Edit(string Id, string Lang)
        {
            if (Lang != null)
            {
                cultureName = Lang;
            }
           var function =  functionService.Function(Id);
           var lang_fun = language_FunctionService.Language_Function(cultureName, Id);
           if (function == null)
           {
               return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
           }
           FunctionFormModel model = Mapper.Map<FunctionFormModel>(new Tuple<Function, Language_Function>(function, lang_fun));
           if (model.LanguageId == null)
           {
               model.LanguageId = cultureName;
           }
           model.Lang_FunId = lang_fun.Id;
           var lang = languageService.GetListForActive();
           model.Lang = new SelectList(lang, "Id", "Name",model.LanguageId);
           PopulateParentIDDropDownList(model.ParentID);
           return View(model);
        }
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "EDIT")]
        [HttpPost, ValidateAntiForgeryToken, Log("{function}")]
        public ActionResult Edit(FunctionFormModel function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        function.UpdatedDate = DateTime.Now;
                        function.UpdatedBy = User.Identity.Name;
                        function.IsDeleted = true;
                        Function newFunction = Mapper.Map<FunctionFormModel, Function>(function);
                        functionService.UpdateFunction(newFunction);
                        //change lang_fun
                        Language_Function lang_fun = Mapper.Map<FunctionFormModel, Language_Function>(function);
                        lang_fun.LanguageId = function.LanguageId;
                        lang_fun.FunctionId = function.Id;
                        if (language_FunctionService.Language_FunctionCheck(function.LanguageId, function.Id))
                        {
                            var langs1 = language_FunctionService.Language_Function(function.LanguageId, function.Id);
                            langs1.Name = lang_fun.Name;
                            langs1.Description = lang_fun.Description;
                            langs1.Text = lang_fun.Text;
                            langs1.LanguageId = lang_fun.LanguageId;
                            langs1.FunctionId = lang_fun.FunctionId;
                            language_FunctionService.UpdateLanguage_Function(langs1);
                        }
                        else
                        {
                            language_FunctionService.CreateLanguage_Function(lang_fun);
                        }
                        return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessEditFunction);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                PopulateParentIDDropDownList(function.ParentID);
                var langs = languageService.GetListForActive();
                function.Lang = new SelectList(langs, "Id", "Name", function.LanguageId);
                return View(function).WithWarning(Resources.Resources.WarningData);
            }
            catch (Exception ex)
            {
                HandleException("Edit Function", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
            
        }
        [AuthorizeUser(FunctionID = "FUNCTION", RoleID = "DELETE"), Log("{Id}")]
        public ActionResult Delete(string Id)
        {
            try
            {
                if (Id == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var function = functionService.Function(Id);
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
                permissionService.DeletePermissionbyFunctionId(Id);
                language_FunctionService.DeleteLanguage_FunctionForFun(Id);
                functionService.DeleteFunction(Id);
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
        private void PopulateParentIDDropDownList(object selectedParent = null, object selfId = null)
        {
            var items = GetFunctionViewModel(selfId);
            ViewBag.Parents = new SelectList(items, "Id", "Text", selectedParent);
        }
        private List<FunctionViewModel> GetFunctionViewModel(object selfId = null, string show= "")
        {
            var allFunctions = functionService.GetFunction().OrderBy(x => x.Order).ToList();
            List<FunctionViewModel> items = new List<FunctionViewModel>();

            if (selfId != null)
                allFunctions = allFunctions.Where(x => x.Id != selfId.ToString()).ToList();
            if (show != "")
            {
                cultureName = show;
            }
            //get parent categories
            IEnumerable<Function> parentFunctions = allFunctions.Where(c => c.ParentID == null).OrderBy(c => c.Order);

            foreach (var cat in parentFunctions)
            {
                var texts = Resources.Resources.NoUpdates;
                var lang_fun = cat.Language_Functions.FirstOrDefault(x => x.LanguageId == cultureName);
                if (lang_fun != null)
                    texts = lang_fun.Text;
                //add the parent category to the item list
                var cate = new FunctionViewModel() {
                    Id = cat.Id,
                    Text = texts,
                    Order = cat.Order,
                    IsLocked = cat.IsLocked,
                    CreatedDate = cat.CreatedDate
                };
                items.Add(cate);
                //now get all its children (separate function in case you need recursion)
                GetSubTree(allFunctions.ToList(), cate, items);
            }
            return items;
        }
        private void GetSubTree(IList<Function> allCats, FunctionViewModel parent, IList<FunctionViewModel> items)
        {
            var subCats = allCats.Where(c => c.ParentID == parent.Id);
            foreach (var cat in subCats)
            {
                //add this category
                var cate = new FunctionViewModel() {
                    Id = cat.Id,
                    Text = parent.Text + " >> " + cat.Language_Functions.SingleOrDefault(x => x.Language.Id == cultureName).Text,
                    Order = cat.Order,
                    IsLocked = cat.IsLocked,
                    CreatedDate = cat.CreatedDate
                };
                items.Add(cate);
                //recursive call in case your have a hierarchy more than 1 level deep
                GetSubTree(allCats, cate, items);
            }
        }
    }
}