using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using AutoMapper;
using TTCMS.Service;
using TTCMS.Core;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Domain;
using Microsoft.Owin.Security;
using TTCMS.Web.Infrastructure;


namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    public class NavigationManagementController : BaseController
    {
        private readonly IFunctionService functionService;
        private readonly IPermissionService permissionService;
        private readonly ILanguageService languageService;
         private readonly IGActionService gactionService;
         public NavigationManagementController(IGActionService gactionService, IFunctionService functionService, IPermissionService permissionService, ILanguageService languageService)
        {
            this.languageService = languageService;
            this.gactionService = gactionService;
            this.functionService = functionService;
            this.permissionService = permissionService;
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public NavigationManagementController()
        {

        }
        // GET: TT_Admin/Navigation
        [ChildActionOnly]
        public ActionResult _MainMenu()
        {
             var listUserAuthority = SessionUtil.GetFromSession<List<SectionPermissionViewModel>>(SystemConsts.SESSION_USER_AUTHORITY);
             if (listUserAuthority == null)
             {
                 listUserAuthority = this.GetPermissionByUser(User.Identity.GetUserId());
                 SessionUtil.SetSession(SystemConsts.SESSION_USER_AUTHORITY, listUserAuthority); 
             }

             var list = (from a in functionService.GetFunction()
                         join b in listUserAuthority on a.Id equals b.FunctionId
                         where b.UserGActionId == "VIEW" & a.IsLocked
                         select a).OrderBy(x => x.Order).ToList();
             List<MenuViewModel> model = CreateVM(null, list);  // transform it into the ViewModel

             return PartialView(model.ToArray());

        }
        public ActionResult _Language()
        {
            IEnumerable<LanguageItemViewModel> model = Mapper.Map<IEnumerable<Language>, IEnumerable<LanguageItemViewModel>>(languageService.GetListForActive());
            var key = model.SingleOrDefault(x => x.Id == cultureName);
            ViewBag.Language = key;
            return PartialView(model.ToArray());
        }
        #region Helpers
        public List<MenuViewModel> CreateVM(string parentid, List<Function> source)
        {
            var query = from men in source
                        where men.ParentID == parentid
                        select new MenuViewModel()
                        {
                            MenuId = men.Id,
                            Text = men.Language_Functions.SingleOrDefault(x => x.Language.Id == cultureName).Text,
                            Link = men.Link,
                            Target = men.Target,
                            CssClass = men.CssClass,
                            Active = men.Id == "PAGE" ? "active" : "",
                            Children = CreateVM(men.Id, source)
                        };
            return query.ToList();
        }
        private List<SectionPermissionViewModel> GetPermissionByUser(string userName)
        {
            var list = new List<SectionPermissionViewModel>();
            List<string> listUserGrsoupId = UserManager.GetRoles(userName).ToList();
            var listperomision = permissionService.GetPermissionForUser(listUserGrsoupId).ToList();
            foreach (var item in listperomision)
            {
                var model = new SectionPermissionViewModel()
                {
                    FunctionId = item.FunctionId,
                    Role_Id = item.RoleId,
                    UserGActionId = item.GActionId
                };
                list.Add(model);
            }
            return list;
        }
        #endregion
    }
}