using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using TTCMS.Core;
using TTCMS.Data;
using TTCMS.Domain;
using TTCMS.Web.Areas.TT_Admin.Models;

namespace TTCMS.Areas.TT_Admin.CustomAttributes
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string RoleID { get; set; }
        public string FunctionID { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //get meta data
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var actionName = string.Empty;
            var controllerName = string.Empty;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action"))
                {
                    actionName = routeValues["action"].ToString();
                }
                if (routeValues.ContainsKey("controller"))
                {
                    controllerName = routeValues["controller"].ToString();
                }
            }
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            var listUserAuthority = SessionUtil.GetFromSession<List<SectionPermissionViewModel>>(SystemConsts.SESSION_USER_AUTHORITY);
                if (listUserAuthority == null)
                {
                    listUserAuthority = this.GetPermissionByUser(httpContext.User.Identity.GetUserId());
                    if (listUserAuthority != null)
                    {
                        SessionUtil.SetSession(SystemConsts.SESSION_USER_AUTHORITY, listUserAuthority);
                    }
                    else
                    {
                        return false;
                    }

                }

                var listRoleId = listUserAuthority.Where(x => x.FunctionId == this.FunctionID.Trim()).Select(x => x.UserGActionId);
                string privilegeLevels = string.Join(";", listRoleId); // Call another method to get rights of the user from DB
                if (!string.IsNullOrEmpty(privilegeLevels) && privilegeLevels.Contains(this.RoleID.Trim()))
                {
                    return true;
                }
                else
                {
                    return false;
                }


        }
        private List<SectionPermissionViewModel> GetPermissionByUser(string userName)
        {
            TTCMSDbContext context = new TTCMSDbContext();
            var roleStore = new RoleStore<ApplicationRole>(context);
            var RoleManager = new RoleManager<ApplicationRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(userStore);
            var list = new List<SectionPermissionViewModel>();
            List<string> listUserGrsoupId = UserManager.GetRoles(userName).ToList();
            List<string> listRoleId = RoleManager.Roles.Where(x => listUserGrsoupId.Contains(x.Name)).Select(a => a.Id).ToList();
            var listperomision = context.Permissions.Where(x => listRoleId.Contains(x.RoleId)).Distinct().ToList();
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
    }
}