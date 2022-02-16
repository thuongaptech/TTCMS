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
    public class ProductManagementController : BaseController
    {
         private readonly IProductService ProductService;
         private readonly ILanguageService languageService;
         private readonly ICategoryService categoryService;
         private readonly IProductColorService _colorSrv;
         private readonly IProductImageService _imageSrv;
         private readonly IProductSizeService _sizeSrv;
         public ProductManagementController(ICategoryService categoryService, 
             IProductService ProductService, ILanguageService languageService,
             IProductColorService _colorSrv, IProductImageService _imageSrv,
             IProductSizeService _sizeSrv
             )
        {
            this.categoryService = categoryService;
            this.ProductService = ProductService;
            this.languageService = languageService;
            this._imageSrv = _imageSrv;
            this._colorSrv = _colorSrv;
            this._sizeSrv = _sizeSrv;
        }
         // GET: /PageManagement/Role/
         //[OutputCache(Duration = 60)]
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "VIEW")]
         public ActionResult Index(int page = 1, string show = "", string search = "")
         {
             if (show != "")
             {
                 cultureName = show;
             }
             int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
             var list = ProductService.GetPageList(new Page(page, pageSize), search);
             // map it to a paged list of models.
             var actionViewModel = Mapper.Map<IPagedList<Product>, IPagedList<ProductViewModel>>(list);
             foreach (var item in actionViewModel)
             {
                 var texts = Resources.Resources.NoUpdates;
             }
             var table = new ProductTableViewModel { ModelList = actionViewModel, Show = show, Search = search };
             if (Request.IsAjaxRequest())
             {
                 return PartialView("_ProductTable", table);
             }
             var lang = languageService.GetListForActive();
             var model = new ProductPageViewModel();
             model.TableList = table;
             return View(model);
         }
         ////
         //// GET: /PageManagement/Role/Create
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "CREATE")]
         [HttpGet]
         public ActionResult Create()
         {

             var model = new ProductViewModel();
             model.GiaBan = 0;
             model.GiaKM = 0;
             model.CreatedDate = DateTime.Now;
             model.Published = DateTime.Now;
             ViewData["MauSac"] = new SelectList(categoryService.GetListbyActive(CategoryType.Color, ""), "Id", "Name");
             PopulateParentIDDropDownList();
             ViewBag.Size = new MultiSelectList(categoryService.GetListbyActive(CategoryType.Size, ""), "Id", "Name");
             return View(model);
         }
         private void PopulateParentIDDropDownList(object selectedParent = null, object selfId = null)
         {
             var items = GetFunctionViewModel(selfId);
             var abc = new SelectList(items, "Id", "Name", selectedParent);
             ViewBag.Parents = abc;

         }
         private List<CategoryViewModel> GetFunctionViewModel(object selfId = null, string show = "")
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
    //    //// ////
    //    ////// // POST: /GroupMenuManagement/Role/Create

         [HttpPost,ValidateInput(false)]
         [ValidateAntiForgeryToken, Log("{model}")]
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "CREATE")]
         public ActionResult Create(ProductViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     model.UpdatedDate = DateTime.Now;
                     Product m_pro = new Product();
                     m_pro.CreatedDate = DateTime.Now;
                     m_pro.Img_Thumbnail = model.Img_Thumbnail;
                     m_pro.Body = model.Body;
                     m_pro.CreatedDate = model.CreatedDate;
                     m_pro.Description = model.Description;
                     m_pro.CategoryId = model.CategoryId;
                     m_pro.GiaBan = model.GiaBan;
                     m_pro.GiaKM = model.GiaKM;
                     m_pro.LanguageId = "*";
                     m_pro.IsActive = model.IsActive;
                     m_pro.IsBanChay = model.IsBanChay;
                     m_pro.IsHot = model.IsHot;
                     m_pro.Name = model.Name;
                     m_pro.CreatedBy = User.Identity.Name;
                     m_pro.Keywords = model.Keywords;
                     m_pro.MaSP = model.MaSP;
                     m_pro.Route = model.Route;
                     m_pro.Published = DateTime.Now;
                     m_pro.Summary = model.Summary;
                     m_pro.Tag = model.Tag;
                     m_pro.Views = 0;
                     m_pro.DaMua = model.DaMua;
                     var _proId = ProductService.Create(m_pro);
                     if(model.ProductImages!=null)
                     {
                         foreach(var item in model.ProductImages)
                         {
                             ProductImage m_image = new ProductImage();
                             m_image.ProductId = _proId.Id;
                             m_image.UrlImage = item.Url;
                             _imageSrv.Create(m_image);
                         }
                     }
                     //if(model.ColorImages!=null)
                     //{
                     //    foreach (var item in model.ColorImages)
                     //    {
                     //        var catecolor = categoryService.GetbyId(item.Color);
                     //        ProductColor m_color = new ProductColor();
                     //        m_color.CategoryId = item.Color;
                     //        m_color.ProductId = _proId.Id;
                     //        m_color.UrlImage = item.Url;
                     //        m_color.ColorCode = catecolor.CssClass;
                     //        m_color.NameColor = catecolor.Name;
                     //        m_color.CreatedDate = DateTime.Now;
                     //        m_color.UpdatedDate = DateTime.Now;
                     //        _colorSrv.Create(m_color);
                     //    }
                     //}
                     //if (model.ProductSize.Length > 0)
                     //{
                     //    foreach (var item in model.ProductSize)
                     //    {
                     //        var catesize = categoryService.GetbyId(item);
                     //        ProductSize m_size = new ProductSize();
                     //        m_size.CategoryId = catesize.Id;
                     //        m_size.NameSize = catesize.Name;
                     //        m_size.ProductId = _proId.Id;
                     //        m_size.CreatedDate = DateTime.Now;
                     //        m_size.UpdatedDate = DateTime.Now;
                     //        _sizeSrv.Create(m_size);
                     //    }
                     //}
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessCreate);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 model.CreatedDate = DateTime.Now;
                 model.Published = DateTime.Now;
                 //ViewData["MauSac"] = new SelectList(categoryService.GetListbyActive(CategoryType.Color, ""), "Id", "Name");
                 PopulateParentIDDropDownList();
                 //ViewBag.Size = new MultiSelectList(categoryService.GetListbyActive(CategoryType.Size, ""), "Id", "Name", model.ProductSize);
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
                    ViewBag.Slug = XString.ToAscii(val).ToLower();
                }
                return PartialView();
            }
            throw new NotSupportedException(Resources.Resources.NotAjaxRequest);
         }
         [ChildActionOnly]
         public ActionResult EditImage(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 var editmodel = _imageSrv.GetListbyPro(Id);
                 if (editmodel == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     return  PartialView(editmodel.ToArray());
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }
         [ChildActionOnly]
         public ActionResult EditColor(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 var editmodel = _colorSrv.GetListbyPro(Id);
                 if (editmodel == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     ViewBag.MauSac = categoryService.GetList(CategoryType.Color);
                     return PartialView(editmodel.ToArray());

                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
         }
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "EDIT")]
         public ActionResult Edit(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 var editmodel = ProductService.GetbyId(Id);
                 if (editmodel == null)
                 {
                     return RedirectToAction("Index").WithError(Resources.Resources.ErrorNoData);
                 }
                 else
                 {
                     ProductViewModel model = Mapper.Map<Product, ProductViewModel>(editmodel);
                     ViewData["MauSac"] = new SelectList(categoryService.GetListbyActive(CategoryType.Color, ""), "Id", "Name");
                     PopulateParentIDDropDownList();
                     int[] array = _sizeSrv.GetListbyPro(Id).Select(x => x.CategoryId).ToArray();
                     ViewBag.Size = new MultiSelectList(categoryService.GetListbyActive(CategoryType.Size, ""), "Id", "Name", array);
                     return View(model);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "EDIT"), ValidateInput(false)]
         [HttpPost, ValidateAntiForgeryToken, Log("{model}")]
         public ActionResult Edit(ProductViewModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     var m_pro = ProductService.GetbyId(model.Id);
                     m_pro.UpdatedBy = User.Identity.Name;
                     m_pro.UpdatedDate = DateTime.Now;
                     m_pro.Img_Thumbnail = model.Img_Thumbnail;
                     m_pro.Body = model.Body;
                     m_pro.Description = model.Description;
                     m_pro.CategoryId = model.CategoryId;
                     m_pro.GiaBan = model.GiaBan;
                     m_pro.GiaKM = model.GiaKM;
                     m_pro.IsActive = model.IsActive;
                     m_pro.IsBanChay = model.IsBanChay;
                     m_pro.IsHot = model.IsHot;
                     m_pro.Name = model.Name;
                     m_pro.Keywords = model.Keywords;
                     m_pro.MaSP = model.MaSP;
                     m_pro.Route = model.Route;
                     m_pro.Published = model.Published;
                     m_pro.Summary = model.Summary;
                     m_pro.Tag = model.Tag;
                     m_pro.Views = model.Views;
                     m_pro.DaMua = model.DaMua;
                     ProductService.Update(m_pro);
                     //change lang_fun
                     if (model.ProductImages != null)
                     {
                         _imageSrv.DeleteByProductId(model.Id);
                         foreach (var item in model.ProductImages)
                         {
                             ProductImage m_image = new ProductImage();
                             m_image.ProductId = m_pro.Id;
                             m_image.UrlImage = item.Url;
                             _imageSrv.Create(m_image);
                         }
                     }
                     //if (model.ColorImages != null)
                     //{
                     //    _colorSrv.DeleteByProductId(model.Id);
                     //    foreach (var item in model.ColorImages)
                     //    {
                     //        var catecolor = categoryService.GetbyId(item.Color);
                     //        ProductColor m_color = new ProductColor();
                     //        m_color.CategoryId = item.Color;
                     //        m_color.ProductId = m_pro.Id;
                     //        m_color.UrlImage = item.Url;
                     //        m_color.ColorCode = catecolor.CssClass;
                     //        m_color.NameColor = catecolor.Name;
                     //        m_color.CreatedDate = DateTime.Now;
                     //        m_color.UpdatedDate = DateTime.Now;
                     //        _colorSrv.Create(m_color);
                     //    }
                     //}
                     //if (model.ProductSize.Length > 0)
                     //{
                     //    _sizeSrv.DeleteByProductId(model.Id);
                     //    foreach (var item in model.ProductSize)
                     //    {
                     //        var catesize = categoryService.GetbyId(item);
                     //        ProductSize m_size = new ProductSize();
                     //        m_size.CategoryId = catesize.Id;
                     //        m_size.NameSize = catesize.Name;
                     //        m_size.ProductId = m_pro.Id;
                     //        m_size.CreatedDate = DateTime.Now;
                     //        m_size.UpdatedDate = DateTime.Now;
                     //        _sizeSrv.Create(m_size);
                     //    }
                     //}
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.UpdateSuccess);
                 }
                 else
                 {
                     ModelState.AddModelError("", Resources.Resources.WarningData);
                 }
                 ViewData["MauSac"] = new SelectList(categoryService.GetList(CategoryType.Color), "Id", "Name");
                 PopulateParentIDDropDownList();
                 ViewBag.Size = new MultiSelectList(categoryService.GetListbyActive(CategoryType.Size, ""), "Id", "Name", model.ProductSize);
                 return View(model).WithWarning(Resources.Resources.WarningData);
             }
             catch (Exception ex)
             {
                 HandleException("Message", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }

         }
         [AuthorizeUser(FunctionID = "PRODUCT", RoleID = "DELETE"), Log("{Id}")]
         public ActionResult Delete(int Id = 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var function = ProductService.GetbyId(Id);
                 if (function == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 ProductService.Delete(Id);
                 _colorSrv.DeleteByProductId(Id);
                 _imageSrv.DeleteByProductId(Id);
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