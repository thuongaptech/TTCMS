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
using TTCMS.Web.Models;
using TTCMS.Core;
using TTCMS.Service;
using TTCMS.Data;
using TTCMS.Core.Infrastructure.Alerts;
using System.Net.Mail;
namespace TTCMS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IPermissionService permissionService;
        private readonly IFunctionService functionService;
        TTCMSDbContext db = new TTCMSDbContext();
        public AccountController(IPermissionService permissionService, IFunctionService functionService)
        {
            this.permissionService = permissionService;
            this.functionService = functionService;
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
        public AccountController()
        {

        }
        [AllowAnonymous, HttpGet]
        public ActionResult ForgotPassword()
        {
            var user = User.Identity.Name;
            ViewBag.Meta = Config;
            if (user != "")
            {
                return RedirectToAction("Profile").WithError("Bạn đang ở trạng thái đăng nhập.");
            }
            return View(new QuenMatKhauViewModel());
        }

        //
        // POST: /Account/ForgotPassword
        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(QuenMatKhauViewModel model)
        {
            ViewBag.Meta = Config;
            var username = User.Identity.Name;
            if (username != "")
            {
                return RedirectToAction("Profile").WithError("Bạn đang ở trạng thái đăng nhập.");
            }
            var user = UserManager.Users.SingleOrDefault(x => x.UserName == model.UserName);
            if (user == null)
            {
                return View(model).WithError("Không tìm thấy tài khoản trên hệ thống!");
            }
            else
            {
                var token = Guid.NewGuid().ToString();
                var sub = "";
                var email = Config.BodyForgotPassword;
                if(!string.IsNullOrEmpty(email))
                {
                    sub = Config.SubForgotPassword;
                    email = email.Replace("{FullName}", user.LastName);
                    Uri url = System.Web.HttpContext.Current.Request.Url;
                    string UrlLink = url.OriginalString.Replace(url.PathAndQuery, "");
                    UrlLink = String.Concat(UrlLink, Url.Action("Reset", "Account", new { userId = user.Id, token = token }));
                    email = email.Replace("{Link}", UrlLink);
                    var to = model.UserName;
                    var subject = sub;
                    var body = email;
                    try
                    {
                        SendMail(to, subject, body);
                        user.ToKen = token;
                        await UserManager.UpdateAsync(user);
                        return View(model).WithSuccess("Hướng dẫn khôi phục mật khẩu đã được gửi đến email của bạn. Vui lòng kiểm tra email và làm theo hướng dẫn.");
                    }
                    catch
                    {
                        return View(model).WithError("Có lỗi xảy ra vui lòng thử lại hoặc liên hệ bộ phận kỹ thuật");
                    }
                }
                
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Reset(string userId = "", string token = "")
        {
            ViewBag.Meta = Config;
            string userid = User.Identity.GetUserId();
            if (userid != null)
            {
                return RedirectToAction("Profile").WithError("Bạn đang ở trạng thái đăng nhập.");
            }
            if (userId == "" || token == "")
            {
                ViewBag.Meta = Config;
                return View("Error").WithError("Không tìm thấy tài khoản hoặc Token không hợp lệ!");
            }
            try
            {
                var user = UserManager.Users.SingleOrDefault(x => x.Id == userId && x.ToKen == token);
                if (user != null)
                {
                    ViewBag.Meta = Config;
                    var model = new LayLaiMKViewModel();
                    model.Id = user.Id;
                    return View(model);
                }
                else
                {
                    ViewBag.Meta = Config;
                    return View("Error").WithError("Không tìm thấy tài khoản hoặc Token không hợp lệ!");
                }
            }
            catch
            {
                ViewBag.Meta = Config;
                return View("Error").WithError("Không tìm thấy tài khoản hoặc Token không hợp lệ!");
            }
        }
        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Reset(LayLaiMKViewModel model)
        {
            string userid = User.Identity.GetUserId();
            if (userid != null)
            {
                return RedirectToAction("Profile").WithError("Bạn đang ở trạng thái đăng nhập.");
            }
            var user =  UserManager.Users.SingleOrDefault(x => x.Id == model.Id);
            ViewBag.Meta = Config;
            if (user != null)
            {
                if (UserManager.HasPassword(user.Id))
                    UserManager.RemovePassword(user.Id);
                IdentityResult result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                if (result.Succeeded)
                {
                    user.ToKen = "";
                    await UserManager.UpdateAsync(user);
                    return View(model).WithSuccess("Đổi mật khẩu thành công.");
                }
                else
                {
                    return View(model).WithError("Đổi mật khẩu không thành công. Vui lòng thử lại.");
                }
            }
            return View("Error").WithError("Không tìm thấy tài khoản hoặc Token không hợp lệ!");
        }
        [AllowAnonymous]
        public ActionResult LoginS(string returnUrl, string type = "")
        {
            ViewBag.Meta = Config;
            ViewBag.ReturnUrl = returnUrl;
            return PartialView(new LoginViewModel());
        }
        [ChildActionOnly]
        public ActionResult User_Info()
        {
            ViewBag.FullName = "";
            if (User.Identity.GetUserId() != null)
            {
                ViewBag.FullName = UserManager.FindById(User.Identity.GetUserId()).LastName;
            }
            return PartialView();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginS(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không hợp lệ.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        // GET: Account
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult Login(string returnUrl, string type = "")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.FullName = "";
            if(User.Identity.GetUserId() != null)
            {
                ViewBag.FullName = UserManager.FindById(User.Identity.GetUserId()).LastName;
            }
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.Meta = Config;
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectTo(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Logins").WithError("Tài khoản hoặc mật khẩu không hợp lệ!");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Meta = Config;
            var model = new RegisterViewModel();
            model.Sex = true;
            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, Sex = model.Sex, LastName = model.FullName, PhoneNumber = model.PhoneNumber, Address = model.Address };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("LoginS").WithSuccess("Đăng ký tài khoản thành công, vui lòng đăng nhập.");
                }
                else
                {
                    AddErrors(result);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            AuthManager.SignOut();
            SessionUtil.RemoveFromSession(SystemConsts.SESSION_USER_AUTHORITY);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Profile()
        {
            ViewBag.Meta = Config;
            string Id = User.Identity.GetUserId();
            if(Id == "")
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để chỉnh sửa thông tin cá nhân!");
            }
            var model = new ProfileViewModel();
            var user = UserManager.FindById(Id);
            if(user != null)
            {
                model.TaiKhoan = user.UserName;
                model.FullName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.Address = user.Address;
                model.Sex = user.Sex;
            }
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            ViewBag.Meta = Config;
            string Id = User.Identity.GetUserId();
            if (Id == "")
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để cập nhật thông tin cá nhân!");
            }
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(Id);
                user.LastName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.Sex = model.Sex;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile").WithSuccess("Cập nhật tài khoản thành công!");
                }
                else
                {
                    return RedirectToAction("Profile").WithError("Lỗi trong quá trình cập nhật, vui lòng kiểm tra lại!");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult Info_Right()
        {
            ViewBag.FullName = "";
            if (User.Identity.GetUserId() != null)
            {
                ViewBag.FullName = UserManager.FindById(User.Identity.GetUserId()).LastName;
            }
            return PartialView();
        }
        [Authorize]
        public ActionResult ChangePass()
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để sử dụng chức năng này!");
            }
            ViewBag.Meta = Config;
            return View(new ChangeViewModel());
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePass(ChangeViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để sử dụng chức năng này!");
            }
            ViewBag.Meta = Config;
            if (ModelState.IsValid)
            {
                IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("ChangePass").WithSuccess("Thay đổi mật khẩu thành công.");
                }
                else
                {
                    return RedirectToAction("ChangePass").WithError("Mật khẩu cũ không chính xác.");
                }
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("ChangePass").WithError("Dữ liệu không đầy đủ.");
        }
        #region Helper methods
        public bool SendMail(string emailto, string subject, string body)
        {

            string displayname = Config.ApplicationName;
            if (displayname == null) { displayname = Config.ApplicationName; }

            string email_account = Config.EmailAccount;

            string email_admin = Config.RecieveEmail;
            if (emailto == "")
                emailto = email_admin;

            string password_account = Config.EmailPassword;

            string host = Config.EmailHost;

            int port = int.Parse(Config.EmailPort);

            bool enablessl = Config.EmailEnableSSL;

            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Credentials = new System.Net.NetworkCredential(email_account, password_account);
            SmtpServer.Port = port;
            SmtpServer.Host = host;
            SmtpServer.EnableSsl = enablessl;
            MailMessage mail = new MailMessage();

            try
            {
                mail.From = new MailAddress(email_account, displayname, System.Text.Encoding.UTF8);
                mail.To.Add(emailto);
                mail.Subject = subject;
                mail.Body = body;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mail.ReplyTo = new MailAddress(email_account);
                mail.Priority = MailPriority.High;
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception) { return false; }
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