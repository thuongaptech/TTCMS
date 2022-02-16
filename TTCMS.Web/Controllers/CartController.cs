using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Models;
using TTCMS.Web.Models.Product;
using TTCMS.Core.Infrastructure.Alerts;
using TTCMS.Web.Infrastructure;
using Microsoft.Owin.Security;
using PagedList;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService ProductService;
        public CartController(IProductService ProductService, IOrderDetailService _orderDetailService, IOrderService _orderService)
        {
            this.ProductService = ProductService;
            this._orderDetailService = _orderDetailService;
            this._orderService = _orderService;
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
        public CartController()
        {

        }
        public ActionResult Info(int page = 1)
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để sử dụng chức năng này!");
            }
            int pageSize = Config.pageSize;
            // map it to a paged list of models.
            var model = Mapper.Map<IEnumerable<Order>, IEnumerable<TTCMS.Web.Areas.TT_Admin.Models.ListDonHangViewModel>>(_orderService.GetList().Where(x => x.UserId == userId).OrderByDescending(x=>x.DateCreate));
            foreach(var item in model)
            {
                var orderdetail = _orderDetailService.GetListbyOrder(item.Id);
                var ListPro = new List<TTCMS.Web.Areas.TT_Admin.Models.ProductViewModel>();
                foreach (var detail in orderdetail)
                {
                    var product = ProductService.GetbyId(detail.ProductID);
                    if (product != null)
                    {
                        product.GiaBan = detail.Price;
                        product.Views = detail.Quanlity;
                        ListPro.Add(Mapper.Map<Product, TTCMS.Web.Areas.TT_Admin.Models.ProductViewModel>(product));
                    }

                }
                item.ListPro = ListPro;
            }
            ViewBag.Meta = Config;
            return View(model.ToPagedList(page,pageSize));
        }
        public ActionResult Info_Detail(int order_id = 0)
        {
            var userId = User.Identity.GetUserId();
            var user = UserManager.Users.SingleOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return RedirectToAction("LoginS").WithWarning("Vui lòng đăng nhập để sử dụng chức năng này!");
            }
            if (order_id == 0)
            {
                return RedirectToAction("Info").WithError("Không tìm thấy đơn hàng của bạn!");
            }
            var log = _orderService.GetbyId(order_id);
            if (log == null)
            {
                return RedirectToAction("Info").WithError("Không tìm thấy đơn hàng của bạn!");
            }
            var logModel = Mapper.Map<Order, TTCMS.Web.Areas.TT_Admin.Models.ListDonHangViewModel>(log);
            if (logModel.UserId != userId)
            {
                return RedirectToAction("Info").WithError("Không tìm thấy đơn hàng của bạn!");
            }
            var orderdetail = _orderDetailService.GetListbyOrder(order_id);
            var ListPro = new List<TTCMS.Web.Areas.TT_Admin.Models.ProductViewModel>();
            foreach (var item in orderdetail)
            {
                var product = ProductService.GetbyId(item.ProductID);
                if (product != null)
                {
                    product.GiaBan = item.Price;
                    product.Views = item.Quanlity;
                    ListPro.Add(Mapper.Map<Product, TTCMS.Web.Areas.TT_Admin.Models.ProductViewModel>(product));
                }

            }
            logModel.ListPro = ListPro;
            ViewBag.Meta = Config;
            return View(logModel);
        }
        [ChildActionOnly]
        public ActionResult GioHang()
        {
            var cart = ShoppingCart.Cart;
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart.Items).ToArray();
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult GioHang_Fixed()
        {
            var cart = ShoppingCart.Cart;
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart.Items).ToArray();
            return PartialView(model);
        }
        public ActionResult Index()
        {
            ViewBag.Meta = Config;
            var cart = ShoppingCart.Cart;
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart.Items).ToArray();
            ViewBag.Step = "1";
            return View(model);
        }
        public ActionResult ThongTinDonHang()
        {
            ViewBag.Meta = Config;
            var cart = ShoppingCart.Cart;
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart.Items).ToArray();
            ViewBag.Step = "2";
            return View(model);
        }
        public ActionResult Add(int id, int quantity)
        {
            var cart = ShoppingCart.Cart;
            cart.Add(id,quantity);
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart.Items).ToArray();
            var info = new { cart.Count, cart.Total, data = RenderPartialViewToString("_Ajax_GioHang", model) };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            var cart = ShoppingCart.Cart;
            cart.Remove(id);
            var info = new { cart.Count, cart.Total };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int id, int quantity)
        {
            var cart = ShoppingCart.Cart;
            cart.Update(id, quantity);

            var p = cart.Items.Single(i => i.Id == id);
            var info = new
            {
                cart.Count,
                cart.Total,
                Amount = p.GiaKM * p.Views
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clear()
        {
            var cart = ShoppingCart.Cart;
            cart.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult FormDatHang()
        {
            var model = new DonHangViewModel();
            var username = User.Identity.Name;

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                model.Address = user.Address;
                user.PhoneNumber = user.PhoneNumber;
                user.UserName = user.LastName;
            }
            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(DonHangViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var cart = ShoppingCart.Cart;
                    if (cart.Count > 0)
                    {
                        var order = new Order();
                        order.UserName = model.UserName;
                        order.Address = model.Address;
                        order.Phone = model.Phone;
                        order.Email = model.Email;
                        order.Note = model.Note;
                        order.DateCreate = DateTime.Now;
                        order.Status = OrderStatus.Chua_Xy_Ly;
                        order.Total = cart.Total;
                        order.UserId = User.Identity.GetUserId();
                        var result = _orderService.Create(order);
                        foreach (var item in cart.Items)
                        {
                            var d = new OrderDetail
                            {
                                OrderID = result.Id,
                                ProductID = item.Id,
                                Quanlity = item.Views,
                                Price = item.GiaKM
                            };
                            _orderDetailService.Create(d);
                        }
                        Uri url = System.Web.HttpContext.Current.Request.Url;
                        string UrlLink = url.OriginalString.Replace(url.PathAndQuery, "");
                        //send mail khách hàng
                        string body = Config.TeamplateDatHang;
                        if (body != "")
                        {
                            body = body.Replace("{FullName}", model.UserName);
                            body = body.Replace("{Email}", model.Email);
                            body = body.Replace("{PhoneNumber}", model.Phone);
                            body = body.Replace("{Notes}", model.Note);
                            body = body.Replace("{Address}", model.Address);
                            body = body.Replace("{CountProduct}", cart.Count.ToString());
                            body = body.Replace("{TotalPrice}", string.Format("{0:#,### VNĐ}", cart.Total));
                            body = body.Replace("{Status}", OrderStatus.Chua_Xy_Ly.ToDescription());
                            UrlLink = String.Concat(UrlLink, Url.Action("ThanhToan", "Cart", new { area = string.Empty, orderId = result.Id }));
                            body = body.Replace("{Link}", UrlLink);
                            SendMail(Config.RecieveEmail, "Chi tiết đơn đặt hàng từ " + Config.CompanyName, body);
                        }
                        body = "";
                        //send mail admin
                        body = Config.TeamplateAdminDatHang;
                        if (body != "")
                        {
                            body = body.Replace("{FullName}", model.UserName);
                            body = body.Replace("{Email}", model.Email);
                            body = body.Replace("{PhoneNumber}", model.Phone);
                            body = body.Replace("{Notes}", model.Note);
                            body = body.Replace("{Address}", model.Address);
                            body = body.Replace("{CountProduct}", cart.Count.ToString());
                            body = body.Replace("{TotalPrice}", string.Format("{0:#,### VNĐ}", cart.Total));
                            body = body.Replace("{Status}", OrderStatus.Chua_Xy_Ly.ToDescription());
                            string UrlLink1 = url.OriginalString.Replace(url.PathAndQuery, "");
                            UrlLink1 = String.Concat(UrlLink1, Url.Action("Index", "DonHangManagement", new { area = "TT_Admin" }));
                            body = body.Replace("{LinkAdmin}", UrlLink1);
                            SendMail(Config.RecieveEmail, "Đơn đặt hàng mới từ " + model.UserName, body);
                        }


                        ShoppingCart.Cart.Clear();
                        return RedirectToAction("ThanhToan", new { orderId = result.Id }).WithSuccess("Đặt hàng thành công, chúng tôi sẽ sớm xữ lý và liên hệ với bạn!");
                    }
                    return RedirectToAction("Index").WithError("Không có sản phẩm trong giỏ hàng");
                }
                var cart1 = ShoppingCart.Cart;
                var list = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(cart1.Items).ToArray();
                ViewBag.Step = "2";
                ViewBag.Meta = Config;
                return View("ThongTinDonHang", list).WithError("Vui lòng nhập đầy đủ thông tin người nhận hàng.");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index").WithError("Ngoại lệ đã xảy ra. Vui lòng liên hệ bộ phận kỹ thuật để được hỗ trợ.");
            }
            
        }
        public ActionResult ThanhToan(int orderId = 0)
        {
            if (orderId == 0)
            {
                return RedirectToAction("Index").WithError("Không tìm thấy đơn hàng");
            }
            var donhang = _orderService.GetbyId(orderId);
            if (donhang == null)
            {
                return RedirectToAction("Index").WithError("Không tìm thấy đơn hàng");
            }
            var logModel = Mapper.Map<Order, HomeDonHangViewModel>(donhang);
            var orderdetail = _orderDetailService.GetListbyOrder(orderId);
            var ListPro = new List<ProductDetailViewModel>();
            foreach (var item in orderdetail)
            {
                var product = ProductService.GetbyId(item.ProductID);
                if (product != null)
                {
                    product.GiaKM = item.Price;
                    product.Views = item.Quanlity;
                    ListPro.Add(Mapper.Map<Product, ProductDetailViewModel>(product));
                }
            }
            logModel.ListPro = ListPro;
            ViewBag.Step = "3";
            ViewBag.Meta = Config;
            return View(logModel);
        }
        public bool SendMail(string emailto, string subject, string body)
        {

            string displayname = Config.ApplicationName;
            if (displayname == null) { displayname = "Kiến Phong CMS"; }

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