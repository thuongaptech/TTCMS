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
    public class LienHeManagementController : BaseController
    {
        private readonly IActivityLogService activityLogService;
        private readonly ILienHeService lienHeService;
        public LienHeManagementController(ILienHeService lienHeService,IActivityLogService activityLogService)
        {
            this.lienHeService = lienHeService;
            this.activityLogService = activityLogService;
        }
        // GET: TT_Admin/LogManagement
        [AuthorizeUser(FunctionID = "CONTACT", RoleID = "VIEW")]
        public ActionResult Index(int page = 1)
        {
            try {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                var log = lienHeService.GetPageList(new Page(page, pageSize));
                // map it to a paged list of models.
                var logModel = Mapper.Map<IPagedList<Contacts>, IPagedList<ContactViewModel>>(log);
                var table = new ContactTableViewModel { ModelList = logModel };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_LienHeTable", table);
                }
                var model = new ContactPageViewModel();
                model.TableList = table;
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List log", ex);
                return RedirectToAction("Index","Home").WithError(Resources.Resources.ErrorCatch);

            }
        }
         [AuthorizeUser(FunctionID = "CONTACT", RoleID = "VIEW"), Log("Id")]
        public ActionResult Detail(int Id = 0)
        {
            if (Id == 0)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            var log = lienHeService.GetbyId(Id);
            if (log == null)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            log.IsActive = true;
            lienHeService.Update(log);
            var logModel = Mapper.Map<Contacts, ContactViewModel>(log);
            return PartialView(logModel);
        }
        [AuthorizeUser(FunctionID = "CONTACT", RoleID = "DELETE"), Log("Id")]
         public ActionResult Delete(int Id= 0)
         {
             try
             {
                 if (Id == 0)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var model = lienHeService.GetbyId(Id);
                 if (model == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 lienHeService.Delete(Id);
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