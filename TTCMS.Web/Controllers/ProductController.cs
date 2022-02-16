using AutoMapper;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Models.Product;

namespace TTCMS.Web.Controllers
{
    public class ProductController : BaseController
    {

        private readonly IProductService _proSrv;
        private readonly IOrderDetailService _orderDetailService;
        private readonly ICategoryService categoryService;
        private readonly IProductColorService _colorSrv;
        private readonly IProductImageService _imageSrv;
        private readonly IProductSizeService _sizeSrv;
        public ProductController(IOrderDetailService _orderDetailService, IProductSizeService _sizeSrv,IProductImageService _imageSrv,IProductColorService _colorSrv,ICategoryService categoryService,IProductService _proSrv)
        {
            this._orderDetailService = _orderDetailService;
            this._sizeSrv = _sizeSrv;
            this._colorSrv = _colorSrv;
            this._imageSrv = _imageSrv;
            this.categoryService = categoryService;
            this._proSrv = _proSrv;
        }
        
        [ChildActionOnly]
        public ActionResult Home_Sale()
        {
            string _Cahe_Home_Sale = "Home_Sale";
            IList<ProductVM> model = new List<ProductVM>();

            if (!Cache.IsSet(_Cahe_Home_Sale))
            {
                model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(_proSrv.GetListbyActive().Where(x => x.GiaKM > 0).Take(8).OrderByDescending(x => x.Published)).ToArray();
                Cache.Set(_Cahe_Home_Sale, model);
            }
            else
            {
                model = Cache.Get(_Cahe_Home_Sale) as ProductVM[];
            }

            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult CateHome()
        {
            string _Cahe_CateHome = "CateHome";
            IList<CateHomeViewModel> model = new List<CateHomeViewModel>();
            if (!Cache.IsSet(_Cahe_CateHome))
            {
                model = Mapper.Map<IEnumerable<Category>, IEnumerable<CateHomeViewModel>>(categoryService.GetListbyActive(CategoryType.Product,cultureName).Where(x => x.IsHome && x.IsActive).OrderBy(x => x.Order)).ToArray();
                foreach (var item in model)
                {
                    item.ListParent = Mapper.Map<IEnumerable<Category>, IEnumerable<CateHomeViewModel>>(categoryService.GetListbyActive(CategoryType.Product, cultureName).Where(x=>x.ParentID == item.Id).OrderBy(x=>x.Order).Take(3)).ToList();
                    item.ListProductNews = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(item.Id,item.ShowProduct)).ToList();
                    foreach (var pro in item.ListProductNews)
                    {
                        pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
                    }
                    item.ListProductHot = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(item.Id)).Where(x=>x.IsBanChay).OrderByDescending(x=>x.Published).Take(item.ShowProduct).ToList();
                    foreach (var pro in item.ListProductHot)
                    {
                        pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
                    }
                    item.ListProductPrice = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(item.Id)).OrderByDescending(x => x.Published).ToList().OrderByDescending(x=>x.Percent).ThenByDescending(x=>x.Published).ToList();
                    foreach (var pro in item.ListProductPrice)
                    {
                        pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
                    }
                }
                Cache.Set(_Cahe_CateHome, model);
            }
            else
            {
                model = Cache.Get(_Cahe_CateHome) as CateHomeViewModel[];
            }

            return PartialView(model.ToArray());
        }
        [ChildActionOnly]
        public ActionResult BarMenu()
        {
            string _Cahe_CateHome = "BarMenu";
            IList<CateHomeViewModel> model = new List<CateHomeViewModel>();
            if (!Cache.IsSet(_Cahe_CateHome))
            {
                model = Mapper.Map<IEnumerable<Category>, IEnumerable<CateHomeViewModel>>(categoryService.GetListbyActive(CategoryType.Product, cultureName).Where(x => x.IsHome && x.IsActive).OrderBy(x => x.Order)).ToArray();
                foreach (var item in model)
                {
                    item.ListProductNews = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(item.Id, item.ShowProduct)).ToList();
                }
            }
            else
            {
                model = Cache.Get(_Cahe_CateHome) as CateHomeViewModel[];
            }

            return PartialView(model.ToArray());
        }
        public ActionResult Detail(int Id = 0)
        {
            if (Id > 0)
            {
                var product = _proSrv.GetbyId(Id);
                if (product != null)
                {
                    var model = Mapper.Map<Product, ProductDetailViewModel>(product);
                    model.ListHinhAnh = Mapper.Map<IEnumerable<ProductImage>, IEnumerable<HinhAnh>>(_imageSrv.GetListbyPro(Id)).ToList();
                    model.ListMauSac = Mapper.Map<IEnumerable<ProductColor>, IEnumerable<MauSac>>(_colorSrv.GetListbyPro(Id)).ToList();
                    model.SPLienQuan = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDetailViewModel>>(_proSrv.GetListbyActive(model.CategoryId)).Take(8).ToList();
                    model.ListSize = Mapper.Map<IEnumerable<ProductSize>, IEnumerable<Size>>(_sizeSrv.GetListbyPro(Id)).ToList();
                    model.DaMua = model.DaMua + _orderDetailService.TongDaMua(model.Id);
                    Config.Description = model.Description;
                    Config.Keywords = model.Keywords;
                    Config.Seo_Image = model.Img_Thumbnail;
                    ViewBag.Meta = Config;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult BanChay()
        {
            var model = new BanChayViewModel();
            model.ListBanChay = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive().Where(x => x.IsBanChay).OrderByDescending(x => x.Published).Take(8)).ToList();
            foreach (var pro in model.ListBanChay)
            {
                pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
            }
            model.ListMoiNhat = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive().OrderByDescending(x => x.Published).Take(8)).ToList();
            foreach (var pro in model.ListMoiNhat)
            {
                pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
            }
            return PartialView(model);
        }
        public ActionResult Category(int Id = 0, int? page = 1, string field = "")
        {
            if (Id == 0)
            {
                return HttpNotFound();
            }
            string _Cahe_Category = "Category";
            int pageSize = Config.pageSize;
            int pageNumber = (page ?? 1);
            IList<ProductHomeViewModel> model = new List<ProductHomeViewModel>();
            if (!Cache.IsSet(_Cahe_Category + Id.ToString() + page.ToString()))
            {
                model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(Id)).ToArray();
                foreach (var pro in model)
                {
                    pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
                }
                Cache.Set(_Cahe_Category + Id.ToString() + page.ToString(), model);
            }
            else
            {
                model = Cache.Get(_Cahe_Category + Id.ToString() + page.ToString()) as ProductHomeViewModel[];
            }
            if(field != "")
            {
                switch (field)
                {
                    case "hot":
                        model = model.Where(x => x.IsBanChay).OrderByDescending(x => x.Published).ToList();
                        break;
                    case "price":
                        model = model.OrderByDescending(x => x.Percent).ThenByDescending(x=>x.Published).ToList();
                        break;
                }  
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Ajax_Category", model.ToPagedList(pageNumber, pageSize));
            }
            var cate = categoryService.GetbyId(Id);
            Config.Description = cate.Description;
            Config.Keywords = cate.Keywords;
            Config.Seo_Image = cate.Img_Thumbnail;
            ViewBag.Meta = Config;
            ViewBag.CateName = cate.Name;
            ViewBag.Route = cate.Route;
            ViewBag.Id = Id;
            ViewBag.field = field;
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        [ChildActionOnly]
        public ActionResult Breadcrumb(int Id = 0)
        {
            var model = new BreadcrumbViewModel();
            if (Id > 0)
            {
                var cate = categoryService.GetbyId(Id);
                if (cate.ParentID == 0 || cate.ParentID == null)
                {
                    model.RootId = cate.Id;
                    model.RootName = cate.Name;
                    model.RootRouter = cate.Route;
                    model.Childrent = false;
                }
                else
                {
                    var root = categoryService.GetbyId(cate.ParentID ?? 0);
                    model.RootId = root.Id;
                    model.RootName = root.Name;
                    model.RootRouter = root.Route;
                    model.Childrent = true;
                    model.ParentId = cate.Id;
                    model.ParentName = cate.Name;
                    model.ParentRouter = cate.Route;
                }
            }
            ViewBag.Meta = Config;
            return PartialView(model);
        }
         [ChildActionOnly]
        public ActionResult CateParent(int Id = 0)
        {
             IList<CateHomeViewModel> model = new List<CateHomeViewModel>(); 
            if(Id > 0)
            {
                int IDRoot = 0;
                var cate = categoryService.GetbyId(Id);
                if(cate.ParentID == 0 || cate.ParentID == null)
                {
                    IDRoot = cate.Id;
                }
                else
                {
                    IDRoot = cate.ParentID??0;
                }
                model = Mapper.Map<IEnumerable<Category>, IEnumerable<CateHomeViewModel>>(categoryService.GetListbyActive(CategoryType.Product, cultureName).Where(x => x.ParentID == IDRoot).OrderBy(x => x.Order)).ToArray();
                foreach (var item in model)
                {
                    item.CountProduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive(item.Id, item.ShowProduct)).Count();
                    item.ActiveClass = item.Id == Id ? true : false;
                }
            }
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult MoiNhat()
        {
            string _Cahe_MoiNhat = "MoiNhat";
            IList<ProductVM> model = new List<ProductVM>();

            if (!Cache.IsSet(_Cahe_MoiNhat))
            {
                model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(_proSrv.GetListbyActive().Take(8).OrderByDescending(x => x.Published)).ToArray();
                Cache.Set(_Cahe_MoiNhat, model);
            }
            else
            {
                model = Cache.Get(_Cahe_MoiNhat) as ProductVM[];
            }

            return PartialView(model);
        }
        public ActionResult TimKiem(int? page = 1, string q = "", string field = "", int category = 0)
        {
            string _Cahe_TimKiem = "TimKiem" + q + page.ToString();
            int pageSize = Config.pageSize;
            int pageNumber = (page ?? 1);
            IList<ProductHomeViewModel> model = new List<ProductHomeViewModel>();

            if (!Cache.IsSet(_Cahe_TimKiem))
            {
                var list = _proSrv.GetListbyActive().OrderByDescending(x => x.Published).ToList();
                if (q != "")
                {
                    list = list.Where(x => x.Name.Contains(q) || x.MaSP.Contains(q) || x.Summary.Contains(q) || (x.Name !="" && XString.BoDau(x.Name).Contains(q))).ToList();

                }
                if(category > 0)
                {
                    list = list.Where(x => x.CategoryId == category || x.Category.ParentID == category).ToList();
                }
                model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(list).ToArray();
                foreach (var pro in model)
                {
                    pro.DaMua = pro.DaMua + _orderDetailService.TongDaMua(pro.Id);
                }
                Cache.Set(_Cahe_TimKiem, model);
            }
            else
            {
                model = Cache.Get(_Cahe_TimKiem) as ProductHomeViewModel[];
            }
            if (field != "")
            {
                switch (field)
                {
                    case "hot":
                        model = model.Where(x => x.IsBanChay).OrderByDescending(x => x.Published).ToList();
                        break;
                    case "price":
                        model = model.OrderByDescending(x => x.Percent).ThenByDescending(x => x.Published).ToList();
                        break;
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Ajax_TimKiem", model.ToPagedList(pageNumber, pageSize));
            }
            ViewBag.Meta = Config;
            ViewBag.tukhoa = q;
            ViewBag.field = field;
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Sales(int? page = 1)
        {
            string _Cahe_Sales = "Sales";
            int pageSize = Config.pageSize;
            int pageNumber = (page ?? 1);
            IList<ProductHomeViewModel> model = new List<ProductHomeViewModel>();

            if (!Cache.IsSet(_Cahe_Sales))
            {
                model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductHomeViewModel>>(_proSrv.GetListbyActive().Where(x => x.GiaKM > 0).OrderByDescending(x => x.Published)).ToArray();
                Cache.Set(_Cahe_Sales, model);
            }
            else
            {
                model = Cache.Get(_Cahe_Sales) as ProductHomeViewModel[];
            }
            ViewBag.show = "0";
            if (model.Count > pageSize)
            {
                ViewBag.show = "1";
            }
            ViewBag.Meta = Config;
            return View(model.ToPagedList(pageNumber, pageSize));
        }
    }
}