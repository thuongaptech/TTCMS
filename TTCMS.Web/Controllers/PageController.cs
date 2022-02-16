using AutoMapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TTCMS.Domain;
using TTCMS.Service;
using TTCMS.Web.Models.News;
using TTCMS.Web.Models.Product;

namespace TTCMS.Web.Controllers
{
    public class PageController : BaseController
    {

        private readonly IMenuService _menuSrv;
        private readonly ISinglePageService _pageSrv;
        private readonly INewsService _newsSrv;
        private readonly ICategoryService _cataSrv;
        private readonly ISinglePageService singlePageService;
        private readonly ILanguage_SinglePageService language_SinglePageService;
        public PageController(ILanguage_SinglePageService language_SinglePageService,ISinglePageService singlePageService,IMenuService _menuSrv, ISinglePageService _pageSrv, INewsService _newsSrv, ICategoryService _cataSrv)
        {
            this.language_SinglePageService = language_SinglePageService;
            this.singlePageService = singlePageService;
            this._pageSrv = _pageSrv;
            this._menuSrv = _menuSrv;
            this._newsSrv = _newsSrv;
            this._cataSrv = _cataSrv;
        }



        public ActionResult Index(int Id = 0)
        {
            ViewBag.Meta = Config;
            string string_model = "NewList";
            int _catID = 0;
            NewsCatVM m_news;
            var m_cata = _cataSrv.GetbyId(Id);
            if(m_cata!=null)
            {
                _catID = m_cata.Id;
            }
            if (!Cache.IsSet(string_model + Id))
            {
                m_news = new NewsCatVM()
                {
                    Catalogy = _cataSrv.GetList(CategoryType.News).ToArray(),
                    NewsList = _newsSrv.GetList().Where(x => x.IsActive /*&& x.CategoryId == _catID*/).OrderByDescending(x => x.Published)
                    .Select(o=> new NewsList()
                    {
                        //CategoryId = o.CategoryId,
                        Description = o.Description,
                        Id= o.Id,
                        Img_Thumbnail = o.Img_Thumbnail,
                        Keywords = o.Keywords,
                        LanguageId = o.LanguageId,
                        Published = o.Published,
                        Route = o.Route,
                        Summary = o.Summary,
                        Tag = o.Tag,
                        Title = o.Title,
                        Views= o.Views
                        
                    }).ToArray()
                };
                Cache.Set(string_model + Id, m_news);
            }
            else
            {
                m_news = Cache.Get(string_model + Id) as NewsCatVM;
            }
            ViewBag.routecate = m_cata.Route;
            return View(m_news);
        }

        public ActionResult DetailNews(int Id=0)
        {
            ViewBag.Meta = Config;
             var cata = _cataSrv.GetList(CategoryType.News);
            ViewBag.Catalog = cata;
            News m_news = _newsSrv.GetList().SingleOrDefault(x => x.IsActive && x.Id == Id);
            if(m_news != null)
            {
                m_news.Views++;
                _newsSrv.Update(m_news);
                //ViewBag.Id = m_news.CategoryId;
                Config.Description = m_news.Description;
                Config.Keywords = m_news.Keywords;
                //ViewBag.CatalogName = m_news.Category.Name;
                return View(m_news);
            }
            else
            {
                return RedirectToAction("Index", new { catRoute = "Tin-tuc" });
            }
        }

 
        public ActionResult NewViewMore()
        {
            string string_model = "NewViewMore";
            IList<News> m_news = new List<News>();

            if (!Cache.IsSet(string_model))
            {
                m_news = _newsSrv.GetList().Where(x => x.IsActive).OrderByDescending(x => x.Views).Take(10).ToArray();
                Cache.Set(string_model, m_news);
            }
            else
            {
                m_news = Cache.Get(string_model) as News[];
            }
            
            return PartialView(m_news);
        }
        public ActionResult PageDetail(int Id = 0)
        {
            if (Id == 0)
            {
                return HttpNotFound();
            }
            var editmodel = singlePageService.GetbyId(Id);
            if (editmodel == null)
            {
                return HttpNotFound();
            }
            var lang_model = language_SinglePageService.GetbyId(cultureName, Id);
            if (lang_model == null)
            {
                return HttpNotFound();
            }
            SinglePageHomeViewModel model = Mapper.Map<SinglePageHomeViewModel>(new Tuple<SinglePage, Language_SinglePage>(editmodel, lang_model));
            Config.Description = model.Description;
            Config.Keywords = model.Keywords;
            Config.Seo_Image = model.Img_Thumbnail;
            ViewBag.Meta = Config;
            ViewBag.Id = Id;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Meta = Config;
            ViewBag.Title = Config.Title;
            return View();
        }

        public ActionResult News()
        {
            ViewBag.Meta = Config;
            ViewBag.Title = Config.Title;
            return View();
        }

        public ActionResult InvestorRelations()
        {
            ViewBag.Meta = Config;
            ViewBag.Title = Config.Title;
            return View();
        }

        public ActionResult CareerOpportunities()
        {
            ViewBag.Meta = Config;
            ViewBag.Title = Config.Title;
            return View();
        }

        public ActionResult Maintenance()
        {
            ViewBag.Meta = Config;
            return View();
        }
 
    }
}