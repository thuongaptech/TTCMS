using TTCMS.Domain;
using TTCMS.Web.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using TTCMS.Core;
using TTCMS.Service;
using TTCMS.Web.Areas.TT_Admin.Models;
using PagedList;
using TTCMS.Data.Infrastructure;
using System.Linq.Expressions;
using TTCMS.Web.Extensions;
using AutoMapper;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Areas.TT_Admin.Filters;
using TTCMS.Areas.TT_Admin.CustomAttributes;
using TTCMS.Service.Common;
using System.Drawing;
using System.IO;
using TTCMS.Core.Images;
using System.Net;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace TTCMS.Web.Areas.TT_Admin.Controllers
{
    public class UserManagementController : BaseController
    {
         private readonly IPermissionService permissionService;
        private readonly ISettingService settingService;
        public UserManagementController(IPermissionService permissionService, ISettingService settingService)
        {
            this.permissionService = permissionService;
            this.settingService = settingService;
        }
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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
        
        // GET: TT_Admin/UserManagement
        [HttpGet]
        [AuthorizeUser(FunctionID = "USER", RoleID = "VIEW")]
        public ActionResult Index(int? page = 1, string show = "", string search = "")
        {
            try
            {
                int pageSize = int.Parse(ConfigSettings.ReadSetting("pageSize"));
                int pageNumber = (page ?? 1);
                //role list
                var role = RoleManager.Roles.Where(x => x.IsActived == true);
                var listrole = role.ToSelectListItems(show);
                //user
                var user = UserManager.Users;
                if (show != "")
                {
                    user = user.Where(x => x.Roles.Select(r => r.RoleId).Contains(show));
                }
                if (search != "")
                {
                    user = user.Where(x => x.UserName.Contains(search) || x.Email.Contains(search) || x.FirstName.Contains(search) || x.LastName.Contains(search));
                }
                IEnumerable<UserViewModel> userlist = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(user);
                var userPage = new UserTableViewModel { UserList = userlist.ToPagedList(pageNumber, pageSize), Page = pageNumber, Search = search, Show=show };
                var model = new UserPageViewModel { UserTableList = userPage, Roles = listrole };
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_UserTable", userPage);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("List User", ex);
                return RedirectToAction("Index","Home").WithError(Resources.Resources.ErrorCatch);
            }
            
        }
        [Authorize]
        public ActionResult EditProfile(string Id)
        {
            var user = UserManager.FindById(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserFormModel model = Mapper.Map<ApplicationUser, UserFormModel>(user);
            var Allroles = RoleManager.Roles.Where(x => x.IsActived == true);
            List<string> roleuser = UserManager.GetRoles(user.Id).ToList();
            foreach (var role in Allroles)
            {
                var listItem = new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = roleuser.Contains(role.Name)
                };
                model.ListRole.Add(listItem);
            }
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken, Log("{user}")]
        public async Task<ActionResult> EditProfile(UserEditFormModel user, HttpPostedFileBase File)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.UpdatedBy = User.Identity.Name;
                    var useredit = await UserManager.FindByIdAsync(user.Id);
                    if (useredit == null)
                    {
                        return HttpNotFound();
                    }
                    var fileName = "";
                    if (File != null)
                    {
                        fileName = Guid.NewGuid().ToString() + ".png";
                    }
                    else
                    {
                        fileName = useredit.ProfilePicUrl;
                    }
                    useredit.FirstName = user.FirstName;
                    useredit.LastName = user.LastName;
                    useredit.Email = user.Email;
                    useredit.Address = user.Address;
                    useredit.PhoneNumber = user.PhoneNumber;
                    useredit.Activated = user.Activated;
                    useredit.Sex = user.Sex;
                    useredit.UpdatedBy = User.Identity.Name;
                    useredit.ProfilePicUrl = fileName;
                    var adminresult = await this.UserManager.UpdateAsync(useredit);
                    if (adminresult.Succeeded)
                    {
                        if (File != null)
                        {
                            var oldFile = Server.MapPath("~/Content/avatar/" + user.ProfilePicUrl);
                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                            Bitmap original = null;
                            original = Bitmap.FromStream(File.InputStream) as Bitmap;
                            if (original != null)
                            {
                                var img = ResizeImage.ResizeBitmapIsMax(original, 180, 180);
                                var fn = Server.MapPath("~/Content/avatar/" + fileName);
                                img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                    }

                    return RedirectToAction("Profile", new { Id = user.Id }).WithSuccess(Resources.Resources.SuccessEditFunction);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                return View(user).WithWarning(Resources.Resources.WarningData);
            }
            catch (Exception ex)
            {
                HandleException("Edit User", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }

        }
        [Authorize]
        public ActionResult Profile()
        {
            try {
                string userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                UserProfileViewModel model = Mapper.Map<ApplicationUser, UserProfileViewModel>(user);
                IEnumerable<LogActionViewModel> log = Mapper.Map<IEnumerable<ActivityLog>, IEnumerable<LogActionViewModel>>(user.ActivityLogs.OrderByDescending(x => x.ExecutionTime).Take(5));
                model.ListLog = log.ToList();
                model.ListRole = UserManager.GetRoles(userId).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                HandleException("User Profile", ex);
                return RedirectToAction("Index", "Home").WithError(Resources.Resources.ErrorCatch);
            }
           
        }
         [Authorize]
        public ActionResult ProChangePass()
        {
            string Id = User.Identity.GetUserId();
             if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new ChangePasswordViewModel();
            model.Id = user.Id;
            model.Name = user.UserName;
            model.ProfilePicUrl = user.ProfilePicUrl;
            return PartialView(model);
        }
         [HttpPost]
         [ValidateAntiForgeryToken, Log("{model}")]
         public async Task<ActionResult> ProChangePass(ChangePasswordViewModel model)
         {
             try
             {
                 if (!ModelState.IsValid)
                 {
                     return RedirectToAction("Index").WithWarning(Resources.Resources.WarningData);
                 }
                 var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                 if (result.Succeeded)
                 {
                     return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessChangePassword);
                 }
                 else
                 {
                     return RedirectToAction("Index").WithWarning(Resources.Resources.ErrorIncorrectOldPassword);
                 }
             }
             catch (Exception ex)
             {
                 HandleException("ChangePassword User", ex);
                 return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
             }
             return View();
         }
        // GET: /UserManagement/Create
        [HttpGet]
        [AuthorizeUser(FunctionID = "USER", RoleID = "CREATE")]
        public ActionResult Create()
        {
            var model = new UserFormModel();
            var roles= RoleManager.Roles.Where(x=>x.IsActived==true);
             //role list
            model.ListRole = roles.ToSelectListItems();
            model.Activated = true;
            return View(model);
        }
        //
        // POST: /UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken, Log("{userViewModel}{selecteRole}")]
        [AuthorizeUser(FunctionID = "USER", RoleID = "CREATE")]
        public async Task<ActionResult> Create(UserFormModel userViewModel, HttpPostedFileBase File, params string[] selecteRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Bitmap original = null;
                    var filename = Guid.NewGuid().ToString();
                    var user = new ApplicationUser
                    {
                        UserName = userViewModel.UserName,
                        Email = userViewModel.Email,
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Address = userViewModel.Address,
                        PhoneNumber = userViewModel.PhoneNumber,
                        Activated = userViewModel.Activated,
                        Sex = userViewModel.Sex,
                        IsLocked = false,
                        IsDeleted = true,
                        DateCreated = DateTime.Now,
                        CreatedBy = User.Identity.Name,
                        ProfilePicUrl = filename + ".png"
                    };
                    var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
                    //Add User to the selected Roles 
                    if (adminresult.Succeeded)
                    {
                        if (File != null)
                        {
                            original = Bitmap.FromStream(File.InputStream) as Bitmap;
                            if (original != null)
                            {
                                var img = ResizeImage.ResizeBitmapIsMax(original, 180, 180);
                                var fn = Server.MapPath("~/Content/avatar/" + filename + ".png");
                                img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                        if (selecteRole != null)
                        {
                            selecteRole = selecteRole ?? new string[] { };
                            var result = await UserManager.AddToRolesAsync(user.Id, selecteRole);
                        }
                    }
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessAddFunction);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                var roles = RoleManager.Roles.Where(x => x.IsActived == true);
                foreach (var role in roles)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = role.Name,
                        Value = role.Id,
                        Selected = selecteRole != null ? selecteRole.Contains(role.Name) : false
                    };
                    userViewModel.ListRole.Add(listItem);
                }
                return View(userViewModel).WithWarning(Resources.Resources.WarningData); ;
            }
            catch (Exception ex)
            {
                HandleException("Create User",ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
            
        }
        [HttpGet]
        [AuthorizeUser(FunctionID = "USER", RoleID = "EDIT")]
        public ActionResult Edit(string Id)
        {
            var user = UserManager.FindById(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserFormModel model = Mapper.Map<ApplicationUser, UserFormModel>(user);
            var Allroles = RoleManager.Roles.Where(x => x.IsActived == true);
            List<string> roleuser = UserManager.GetRoles(user.Id).ToList();
            foreach (var role in Allroles)
            {
                var listItem = new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.Id,
                    Selected = roleuser.Contains(role.Name)
                };
                model.ListRole.Add(listItem);
            }
            return View(model);
        }
        [AuthorizeUser(FunctionID = "USER", RoleID = "EDIT")]
        [HttpPost, ValidateAntiForgeryToken, Log("{user}{selecteRole}")]
        public async Task<ActionResult> Edit(UserEditFormModel user, HttpPostedFileBase File, params string[] selecteRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.UpdatedBy = User.Identity.Name;
                    var useredit = await UserManager.FindByIdAsync(user.Id);
                    if (useredit == null)
                    {
                        return HttpNotFound();
                    }
                    var fileName = "";
                    if (File != null)
                    {
                         fileName = Guid.NewGuid().ToString() + ".png";
                    }
                    else
                    {
                        fileName = useredit.ProfilePicUrl;
                    }
                    useredit.FirstName = user.FirstName;
                    useredit.LastName = user.LastName;
                    useredit.Email = user.Email;
                    useredit.Address = user.Address;
                    useredit.PhoneNumber = user.PhoneNumber;
                    useredit.Activated = user.Activated;
                    useredit.Sex = user.Sex;
                    useredit.UpdatedBy = User.Identity.Name;
                    useredit.ProfilePicUrl = fileName;
                 var adminresult = await this.UserManager.UpdateAsync(useredit);
                 if (adminresult.Succeeded)
                 {
                     if (File != null)
                     {
                         var oldFile = Server.MapPath("~/Content/avatar/"+user.ProfilePicUrl);
                         if (System.IO.File.Exists(oldFile))
                         {
                             System.IO.File.Delete(oldFile);
                         }
                         Bitmap original = null;
                         original = Bitmap.FromStream(File.InputStream) as Bitmap;
                         if (original != null)
                         {
                             var img = ResizeImage.ResizeBitmapIsMax(original, 180, 180);
                             var fn = Server.MapPath("~/Content/avatar/" + fileName);
                             img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                         }
                     }
                     selecteRole = selecteRole ?? new string[] { };
                     //remove role
                     string[] listroleold = UserManager.GetRoles(useredit.Id).ToArray();
                     await this.UserManager.RemoveFromRolesAsync(useredit.Id, listroleold);
                     //add role
                     var result = await UserManager.AddToRolesAsync(useredit.Id, selecteRole);
                 }
                    
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessEditFunction);
                }
                else
                {
                    ModelState.AddModelError("", Resources.Resources.WarningData);
                }
                var roles = RoleManager.Roles.Where(x => x.IsActived == true);
                foreach (var role in roles)
                {
                    var listItem = new SelectListItem()
                    {
                        Text = role.Name,
                        Value = role.Id,
                        Selected = selecteRole != null ? selecteRole.Contains(role.Name) : false
                    };
                    user.ListRole.Add(listItem);
                }
                return View(user).WithWarning(Resources.Resources.WarningData);
            }
            catch (Exception ex)
            {
                HandleException("Edit User", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
            
        }
        // POST: /UserManagement/Delete/5
        [AuthorizeUser(FunctionID = "USER", RoleID = "DELETE")]
         [Log("{Id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            try
            {
                if (Id == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                var user = await UserManager.FindByIdAsync(Id);
                if (user == null)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataNull };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                if (user.IsDeleted == false)
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithWarning, Msg = Resources.Resources.AlertDataSystem };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                IdentityResult result;
                result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                }
                if (Request.IsAjaxRequest())
                {
                    var reponse = new { Id = Id, Code = AjaxAlertConsts.WithSuccess, Msg = Resources.Resources.AlertDataSuccess };
                    return Json(reponse, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index").WithSuccess(Resources.Resources.AlertDataSuccess);
            }
            catch (Exception ex)
            {
                HandleException("Delete User", ex);
                var reponse = new { Id = Id, Code = AjaxAlertConsts.WithError, Msg = Resources.Resources.AlertDataCatch };
                return Json(reponse, JsonRequestBehavior.AllowGet);
            }
                
        }
        //
        // GET: /UserManagement/ChangePassword
        [AuthorizeUser(FunctionID = "USER", RoleID = "EDIT")]
        public ActionResult ChangePassword(string Id = "")
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var model = new ChangePasswordViewModel();
            model.Id = user.Id;
            model.Name = user.UserName;
            model.ProfilePicUrl = user.ProfilePicUrl;
            return PartialView(model);
        }

        //
        // POST: /UserManagement/Manage
        [HttpPost]
        [AuthorizeUser(FunctionID = "USER", RoleID = "EDIT")]
        [ValidateAntiForgeryToken, Log("{model}")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index").WithWarning(Resources.Resources.WarningData);
                }
                var result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index").WithSuccess(Resources.Resources.SuccessChangePassword);
                }
                else
                {
                    return RedirectToAction("Index").WithWarning(Resources.Resources.ErrorIncorrectOldPassword);
                }
            }
            catch (Exception ex)
            {
                HandleException("ChangePassword User", ex);
                return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
            }
        }
        [Authorize]
        public ActionResult _UserInfo()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var model = new UserInfoViewModel() { 
                 LastName = user.LastName,
                 ProfilePicUrl = user.ProfilePicUrl
                 
            };
            return PartialView(model);
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string type = "")
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.Type == 1)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {
                        if (user.Activated == true)
                        {
                            await SignInAsync(user, model.RememberMe);
                            var usermodel = UserManager.Users.SingleOrDefault(x => x.UserName == model.UserName);
                            SessionUtil.SetSession(SystemConsts.SESSION_USER_AUTHORITY, this.GetPermissionByUser(usermodel.Id));
                            usermodel.LastLoginTime = DateTime.Now;
                            await this.UserManager.UpdateAsync(usermodel);
                            return RedirectTo(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", Resources.Resources.ErrorUserActive);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.Resources.ErrorLogin);
                    }
                }
                else
                {
                    // login ldap
                    try
                    {
                        var setting = settingService.GetbyFirst();
                        DirectoryEntry ldapConnection = new DirectoryEntry();
                        ldapConnection.Path = $"LDAP://{setting.Host}:{setting.Port}/{setting.UserSearchBase}";
                        ldapConnection.Username = setting.BindDN;
                        ldapConnection.Password = settingService.Decrypt(setting.BindPass);
                        ldapConnection.AuthenticationType = AuthenticationTypes.None;
                        DirectorySearcher search = new DirectorySearcher(ldapConnection);
                        var result = search.FindOne();
                        if (result != null)
                        {
                            string username = Regex.Replace(model.UserName, @"(\s+|@|&|'|\(|\)|<|>|#|-)", "");
                            var user = await UserManager.FindAsync(username, model.Password);
                            if (user != null)
                            {
                                await SignInAsync(user, model.RememberMe);
                                var usermodel = UserManager.Users.SingleOrDefault(x => x.UserName == username);
                                SessionUtil.SetSession(SystemConsts.SESSION_USER_AUTHORITY, this.GetPermissionByUser(usermodel.Id));
                                usermodel.LastLoginTime = DateTime.Now;
                                await this.UserManager.UpdateAsync(usermodel);
                                return RedirectTo(returnUrl);
                            }
                            else
                            {

                                var userInfor = new ApplicationUser
                                {
                                    UserName = username,
                                    Email = String.Empty,
                                    FirstName = String.Empty,
                                    LastName = String.Empty,
                                    Address = String.Empty,
                                    PhoneNumber = String.Empty,
                                    Activated = true,
                                    Sex = true,
                                    IsLocked = false,
                                    IsDeleted = true,
                                    DateCreated = DateTime.Now,
                                    CreatedBy = User.Identity.Name
                                };

                                var adminresult = await UserManager.CreateAsync(userInfor, model.Password);
                                //Add User to the selected Roles 
                                if (adminresult.Succeeded)
                                {
                                    var result2 = await UserManager.AddToRolesAsync(userInfor.Id, "Mod");
                                    if (result2.Succeeded)
                                    {
                                        var user2 = await UserManager.FindAsync(username, model.Password);
                                        if (user2 != null)
                                        {
                                            await SignInAsync(user2, model.RememberMe);
                                            var usermodel = UserManager.Users.SingleOrDefault(x => x.UserName == username);
                                            SessionUtil.SetSession(SystemConsts.SESSION_USER_AUTHORITY, this.GetPermissionByUser(usermodel.Id));
                                            usermodel.LastLoginTime = DateTime.Now;
                                            await this.UserManager.UpdateAsync(usermodel);
                                            return RedirectTo(returnUrl);
                                        }
                                        else
                                        {
                                            ModelState.AddModelError("", Resources.Resources.ErrorLogin);
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "chưa add được quyền");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        HandleException("Ldap login", ex);
                        ModelState.AddModelError("", "Không thể kết nối với Ldap");
                        //return RedirectToAction("Index").WithError(Resources.Resources.ErrorCatch);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            AuthManager.SignOut();
            SessionUtil.RemoveFromSession(SystemConsts.SESSION_USER_AUTHORITY);
            return RedirectToAction("Login");
        }
        #region Helper methods
        private List<SectionPermissionViewModel> GetPermissionByUser(string userName)
        {
            var list = new List<SectionPermissionViewModel>();
            List<string> listUserGrsoupId = UserManager.GetRoles(userName).ToList();
            List<string> listRoleId = RoleManager.Roles.Where(x => listUserGrsoupId.Contains(x.Name)).Select(a => a.Id).ToList();
            var listperomision = permissionService.GetPermissionForUser(listRoleId).ToList();
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

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private ActionResult RedirectTo(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion
    }
    
}