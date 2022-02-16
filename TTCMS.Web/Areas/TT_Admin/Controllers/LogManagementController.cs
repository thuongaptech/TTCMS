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
    public class LogManagementController : BaseController
    {
        private readonly IActivityLogService activityLogService;
        public LogManagementController(IActivityLogService activityLogService)
        {
            this.activityLogService = activityLogService;
        }
        // GET: TT_Admin/LogManagement
        [AuthorizeUser(FunctionID = "ACTIVITYLOG", RoleID = "VIEW")]
        public ActionResult Index(int page = 1, string show = "", string search = "")
        {
            try {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                var log = activityLogService.ListActivityLog(new Page(page, pageSize), search);
                // map it to a paged list of models.
                var logModel = Mapper.Map<IPagedList<ActivityLog>, IPagedList<LogViewModel>>(log);
                var table = new LogTableViewModel {ModelList = logModel,Search=search };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_LogTable", table);
                }
                var model = new LogPageViewModel();
                model.TableList = table;
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List log", ex);
                return RedirectToAction("Index","Home").WithError(Resources.Resources.ErrorCatch);

            }
        }
         [AuthorizeUser(FunctionID = "ACTIVITYLOG", RoleID = "VIEW"), Log("Id")]
        public ActionResult Detail(string Id)
        {
            if (Id == null)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            var log = activityLogService.ActivityLog(Id);
            if (log == null)
            {
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
            var logModel = Mapper.Map<ActivityLog, LogViewModel>(log);
            return PartialView(logModel);
        }
        [AuthorizeUser(FunctionID = "ACTIVITYLOG", RoleID = "DELETE"), Log("Id")]
         public ActionResult Delete(string Id)
         {
             try
             {
                 if (Id == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 var model = activityLogService.ActivityLog(Id);
                 if (model == null)
                 {
                     var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                     return Json(reponse, JsonRequestBehavior.AllowGet);
                 }
                 activityLogService.Delete(Id);
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
        [AuthorizeUser(FunctionID = "ACTIVITYLOG", RoleID = "DELETE"), Log("Clear ALL")]
        public ActionResult DeleteALL(string Id)
        {
            try
            {
                activityLogService.DeleteAll();
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