using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Models;
using TTCMS.Web.Models.Product;
using TTCMS.Core.Infrastructure.Alerts;
namespace TTCMS.Web.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IProductService _proSrv;
        private readonly ILienHeService lienHeService;
        private readonly ICategoryService _category;
        public HomeController(ICategoryService _category, ILienHeService lienHeService, IProductService _proSrv)
        {
            this.lienHeService = lienHeService;
            this._proSrv = _proSrv;
            this._category = _category;
        }
        
        // Starting page
        public ActionResult Index()
        {
            ViewBag.Meta = Config;
            ViewBag.Title = Config.Title;
            return View();
        }
        [ChildActionOnly]
        public ActionResult ProductPromotion()
        {
            var list = _proSrv.GetList().Where(x=>x.GiaKM!=0).Take(4).ToArray();
            var _newlist = Mapper.Map<Product[],ProductVM[]>(list);
            return PartialView(_newlist);
        }

        public ActionResult LienHe()
        {
            ViewBag.BanDo = Config.google_map;
            ViewBag.Meta = Config;
            ViewBag.Key = "0";
            return View(new LienHeViewModel());
        }
        [ChildActionOnly]
        public ActionResult TimKiem(string q = "")
        {
            ViewBag.q = q;
            string _Cahe_CateHome = "TimKiem";
            IList<CateHomeViewModel> model = new List<CateHomeViewModel>();
            if (!Cache.IsSet(_Cahe_CateHome))
            {
                model = Mapper.Map<IEnumerable<Category>, IEnumerable<CateHomeViewModel>>(_category.GetListbyActive(CategoryType.Product, cultureName).Where(x => x.ParentID == null && x.IsActive).OrderBy(x => x.Order)).ToArray();
                Cache.Set(_Cahe_CateHome, model);
            }
            else
            {
                model = Cache.Get(_Cahe_CateHome) as CateHomeViewModel[];
            }

            return PartialView(model.ToArray());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LienHe(LienHeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contacts modelCreate = Mapper.Map<LienHeViewModel, Contacts>(model);
                lienHeService.Create(modelCreate);
                ViewBag.Key = "1";
                ViewBag.BanDo = Config.google_map;
                ViewBag.Meta = Config;
                string body = Config.TemplateEmail;
               body = body.Replace("{FullName}",model.FullName);
               body = body.Replace("{Email}", model.Email);
               body = body.Replace("{PhoneNumber}", model.PhoneNumber);
               body = body.Replace("{Content}", model.NoiDung);
                SendMail(Config.RecieveEmail, "Liên hệ từ " + model.FullName, body);
                return RedirectToAction("LienHe").WithSuccess("Gửi liên hệ thành công!");
            }
            else
            {
                ModelState.AddModelError("", Resources.Resources.WarningData);
            }
            ViewBag.BanDo = Config.google_map;
            ViewBag.Meta = Config;
            return View(model).WithError("Lỗi: Vui lòng kiểm tra lại dữ liệu!");
        }
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
    }
}