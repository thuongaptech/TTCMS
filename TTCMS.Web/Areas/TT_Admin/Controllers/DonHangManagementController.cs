using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Service.Common;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Areas.TT_Admin.Filters;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    [Authorize]
    public class DonHangManagementController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IActivityLogService activityLogService;
        private readonly ILienHeService lienHeService;
        private readonly IProductService ProductService;
        public DonHangManagementController(IProductService ProductService,IOrderDetailService _orderDetailService,IOrderService _orderService,ILienHeService lienHeService, IActivityLogService activityLogService)
        {
            this.ProductService = ProductService;
            this._orderService = _orderService;
            this._orderDetailService = _orderDetailService;
            this.lienHeService = lienHeService;
            this.activityLogService = activityLogService;
        }
        // GET: TT_Admin/LogManagement
        [AuthorizeUser(FunctionID = "DONHANG", RoleID = "VIEW")]
        public ActionResult Index(int page = 1, string search = "")
        {
            try {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                var log = _orderService.GetPageList(new Page(page, pageSize), search);
                // map it to a paged list of models.
                var logModel = Mapper.Map<IPagedList<Order>, IPagedList<DonHangAdminViewModel>>(log);
                var table = new DonHangTableViewModel { ModelList = logModel, Search = search };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_DonHangTable", table);
                }
                var model = new DonHangPageViewModel();
                model.TableList = table;
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List log", ex);
                return RedirectToAction("Index","Home").WithError(Resources.Resources.ErrorCatch);

            }
        }
         [AuthorizeUser(FunctionID = "DONHANG", RoleID = "VIEW"), Log("Id")]
        public ActionResult Detail(int Id = 0)
        {
            if (Id == 0)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            var log = _orderService.GetbyId(Id);
            if (log == null)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            var logModel = Mapper.Map<Order, ListDonHangViewModel>(log);
            var orderdetail = _orderDetailService.GetListbyOrder(Id);
             var ListPro = new List<ProductViewModel>();
             foreach(var item in orderdetail)
             {
                 var product = ProductService.GetbyId(item.ProductID);
                 if(product!=null)
                 {
                     product.GiaKM = item.Price;
                     product.Views = item.Quanlity;
                     ListPro.Add(Mapper.Map<Product, ProductViewModel>(product));
                 }
                
             }
             logModel.ListPro = ListPro;
            return PartialView(logModel);
        }
         [HttpPost]
         [ValidateAntiForgeryToken, Log("{model}")]
         [AuthorizeUser(FunctionID = "DONHANG", RoleID = "EDIT")]
         public ActionResult Update(DonHangAdminViewModel model)
         {
             try
             {
                 if(model.Id > 0)
                 {
                     var log = _orderService.GetbyId(model.Id);
                     if(log != null)
                     {
                         log.Status = OrderStatus.Da_Giao;
                         _orderService.Update(log);
                         return Json(JsonSuccess(Url.Action("Index"), Resources.Resources.UpdateSuccess), JsonRequestBehavior.AllowGet);
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
                 HandleException("Message ", ex);
                 return Json(JsonError(Resources.Resources.ErrorCatch), JsonRequestBehavior.AllowGet);
             }
         }
        [AuthorizeUser(FunctionID = "DONHANG", RoleID = "DELETE"), Log("Id")]
         public ActionResult Delete(int Id= 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var model = _orderService.GetbyId(Id);
                 if (model == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 _orderDetailService.Delete(Id);
                 _orderService.Delete(Id);
                 if (Request.IsAjaxRequest())
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 return RedirectToAction("Index").WithSuccess(Resources.Resources.DeleteSuccess);
             }
             catch (Exception ex)
             {
                 HandleException("Delete Log", ex);
                 var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                 return Json(reponse, JsonRequestBehavior.AllowGet);
             }
         }
    }
}