using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using TTCMS.Service;
using TTCMS.Web.Infrastructure;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Domain;
using TTCMS.Core.Infrastructure.Alerts;
using AutoMapper;
namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
     [Authorize]
    public class PermissionManagementController : BaseController
    {
         //private readonly IRoleService roleService;
         private readonly IGActionService gactionService;
         private readonly ILanguage_GActionService language_GActionService;
          private readonly IFunctionService functionService;
          private readonly IPermissionService permissionService;
          public PermissionManagementController(IFunctionService functionService, IPermissionService permissionService, IGActionService gactionService, ILanguage_GActionService language_GActionService)
        {
            this.language_GActionService = language_GActionService;
            this.functionService = functionService;
            this.permissionService = permissionService;
            this.gactionService = gactionService;
        }
          private ApplicationUserManager UserManager
          {
              get
              {
                  return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
              }
          }
          private ApplicationRoleManager RoleManager
          {
              get
              {
                  return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
              }
          }
         //GET: TT_Admin/Permission
         [AuthorizeUser(FunctionID = "SYSTEM", RoleID = "VIEW")]
         [HttpGet]
         public ActionResult SetRole(string Id)
         {
             var model = new List<PermissionViewModel>();
             var listViewModel = GetFunctionViewModel();
             foreach (var item in listViewModel)
             {
                 var modelItem = new PermissionViewModel();
                 modelItem.Functions = item;
                 modelItem.GAction = GetGActionViewModel();
                 model.Add(modelItem);
             }
             ViewBag.roleId = Id;
             ViewBag.Permissions = permissionService.GetPermissionForRole(Id);
             return View(model.ToList());

         }
         [AcceptVerbs(HttpVerbs.Post)]
         [AuthorizeUser(FunctionID = "SYSTEM", RoleID = "EDIT")]
         public ActionResult Update(string roleId, FormCollection collection)
         {
             List<Permission> list = new List<Permission>();
             //delete permission
             var listPermission = permissionService.GetPermissionForRole(roleId).ToList();
             foreach (var item in listPermission)
             {
                 permissionService.DeletePermission(item.Id);
             }
             var functionList = this.GetFunctionViewModel();
             for (int i = 0; i < functionList.Count; i++)
             {

                 //get function id
                 var functionID = collection["Model[" + i + "].Functions.Id"];
                 //get role list
                 var roles = gactionService.GetGAction().ToList();

                 for (int j = 0; j < roles.Count; j++)
                 {
                     //get checkbox value form collection
                     var cbRoleId = collection["Cb.Model[" + i + "].GAction[" + j + "].GActionId"];
                     var hdRoleId = collection["Hd.Model[" + i + "].GAction[" + j + "].GActionId"];
                     //split value from char array
                     char[] delimiterChars = { ',' };
                     string[] checkedvalue = cbRoleId.Split(delimiterChars);
                     var returnValue = checkedvalue.Length > 1;
                     if (returnValue)
                     {
                         
                         Permission permission = new Permission();
                         permission.FunctionId = functionService.Function(functionID).Id;
                         permission.RoleId = RoleManager.Roles.SingleOrDefault(x => x.Id == roleId).Id;
                         permission.GActionId = gactionService.GAction(hdRoleId).Id;
                         list.Add(permission);
                     }
                 }
             }

             foreach (var permission in list)
             {
                 permissionService.CreatePermission(permission);
             }

             ViewBag.GroupId = roleId;
             return RedirectToAction("Index", "RoleManagement").WithSuccess(TTCMS.Resources.Resources.UpdateSuccess);
         }
         [NonAction]
         private void PopulateFunctionIDDropDownList(object selectedParent = null)
         {
             var items = GetFunctionViewModel();
             ViewBag.Functions = new SelectList(items, "ID", "Text", selectedParent);
         }
          [NonAction]
         private List<FunctionViewModel> GetFunctionViewModel(object selfId = null, string show = "")
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
                 var cate = new FunctionViewModel()
                 {
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
          [NonAction]
         private void GetSubTree(IList<Function> allCats, FunctionViewModel parent, IList<FunctionViewModel> items)
         {
             var subCats = allCats.Where(c => c.ParentID == parent.Id);
             foreach (var cat in subCats)
             {
                 //add this category
                 var cate = new FunctionViewModel()
                 {
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
         private List<ActionLangViewModel> GetGActionViewModel()
         {
             IEnumerable<ActionLangViewModel> actionViewModel = Mapper.Map<IEnumerable<GAction>, IEnumerable<ActionLangViewModel>>(gactionService.GetGAction().Where(x => x.IsActived == true));
             foreach (var item in actionViewModel)
             {
                 var texts = Resources.Resources.NoUpdates;
                 var lang_model = language_GActionService.Language_GAction(cultureName, item.Id);
                 item.Name = lang_model == null ? texts : lang_model.Name;
                 item.Description = lang_model == null ? texts : lang_model.Description;
             }
             return actionViewModel.ToList();
         }
    }
}