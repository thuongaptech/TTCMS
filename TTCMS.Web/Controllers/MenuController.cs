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
using TTCMS.Web.Models.Product;

namespace TTCMS.Web.Controllers
{
    public class MenuController : BaseController
    {

        private readonly IMenuService _menuSrv;
        private readonly ICategoryService _category;
        public MenuController(ICategoryService _category, IMenuService _menuSrv)
        {
            this._menuSrv = _menuSrv;
            this._category = _category;
        }
        [ChildActionOnly]
        public ActionResult TOPMenu()
        {
            string _Cahe_TOPMenu = "TOPMenu";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_Cahe_TOPMenu))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.TopMenu, cultureName).Where(x => x.IsActived).OrderBy(x => x.Order).ToArray();
                Cache.Set(_Cahe_TOPMenu, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_Cahe_TOPMenu) as Menu[];
            }

            return PartialView(m_menu);
        }
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            string _menuMain = "MenuMain";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_menuMain))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.MainMenu, cultureName).Where(x => x.IsActived).OrderBy(x => x.Order).ToArray();
                foreach (var item in m_menu)
                {
                    item.Link = getLinkMenu(item.TextType, item.Link, item.WithId ?? 0, item.Action, item.Controller);
                }
                Cache.Set(_menuMain, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_menuMain) as Menu[];
            }
            ViewBag.HotLine = Config.HotLine;
            return PartialView(m_menu);
        }
        [ChildActionOnly]
        public ActionResult Category()
        {
            string _Cache_BotMenu = "Category_Menu";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_Cache_BotMenu))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.Category, cultureName).Where(x=>x.IsActived).OrderBy(x => x.Order).ToArray();
                foreach (var item in m_menu)
                {
                    item.Link = getLinkMenu(item.TextType, item.Link, item.WithId ?? 0, item.Action, item.Controller);
                }
                Cache.Set(_Cache_BotMenu, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_Cache_BotMenu) as Menu[];
            }
            return PartialView(m_menu);
        }
        [ChildActionOnly]
        public ActionResult Category_Mobile()
        {
            string _Cache_BotMenu = "Category_Mobile";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_Cache_BotMenu))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.Category_Mobile, cultureName).Where(x => x.IsActived).OrderBy(x => x.Order).ToArray();
                foreach (var item in m_menu)
                {
                    item.Link = getLinkMenu(item.TextType, item.Link, item.WithId ?? 0, item.Action, item.Controller);
                }
                Cache.Set(_Cache_BotMenu, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_Cache_BotMenu) as Menu[];
            }
            return PartialView(m_menu);
        }
        [ChildActionOnly]
        public ActionResult BotMenu()
        {
            string _Cache_BotMenu = "BotMenu";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_Cache_BotMenu))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.BotMenu, cultureName).Where(x => x.IsActived).OrderBy(x => x.Order).ToArray();
                foreach (var item in m_menu)
                {
                    item.Link = getLinkMenu(item.TextType, item.Link, item.WithId ?? 0, item.Action, item.Controller);
                }
                Cache.Set(_Cache_BotMenu, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_Cache_BotMenu) as Menu[];
            }
            ViewBag.Contact = Config.contactus_setting;
            return PartialView(m_menu);
        }
        [ChildActionOnly]
        public ActionResult PageMenu(int Id = 0)
        {
            string _Cache_BotMenu = "PageMenu";
            IList<Menu> m_menu = new List<Menu>();

            if (!Cache.IsSet(_Cache_BotMenu))
            {
                m_menu = _menuSrv.GetList(Domain.MenuGroupType.BotMenu, cultureName).Where(x => x.IsActived).OrderBy(x => x.Order).ToArray();
                foreach (var item in m_menu)
                {
                    item.Link = getLinkMenu(item.TextType, item.Link, item.WithId ?? 0, item.Action, item.Controller);
                }
                Cache.Set(_Cache_BotMenu, m_menu);
            }
            else
            {
                m_menu = Cache.Get(_Cache_BotMenu) as Menu[];
            }
            ViewBag.Id = Id;
            return PartialView(m_menu);
        }
    }
}