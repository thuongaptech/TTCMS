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

namespace TTCMS.Web.Controllers
{
    public class HinhAnhController : BaseController
    {

        private readonly ISlideService slideService;
        private readonly IQuangCaoService quangCaoService;
        public HinhAnhController(IQuangCaoService quangCaoService, ISlideService slideService)
        {
            this.quangCaoService = quangCaoService;
            this.slideService = slideService;
        }
        
        [ChildActionOnly]
        public ActionResult Slide()
        {
            string _slideService = "SlideService";
            IList<Slide> model = new List<Slide>();

            if (!Cache.IsSet(_slideService))
            {
                model = slideService.GetListbyActive(SlideType.Slide, cultureName).OrderBy(x => x.Order).ToArray();
                Cache.Set(_slideService, model);
            }
            else
            {
                model = Cache.Get(_slideService) as Slide[];
            }

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Slide_Right()
        {
            string _slideService = "Slide_Right";
            IList<Advertisements> model = new List<Advertisements>();

            if (!Cache.IsSet(_slideService))
            {
                model = quangCaoService.GetListbyActive(cultureName, QuangCaoType.Silder_Right).OrderBy(x => x.Order).ToArray();
                Cache.Set(_slideService, model);
            }
            else
            {
                model = Cache.Get(_slideService) as Advertisements[];
            }

            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult Slide_Right_Mobile()
        {
            string _slideService = "Slide_Right_Mobile";
            IList<Advertisements> model = new List<Advertisements>();

            if (!Cache.IsSet(_slideService))
            {
                model = quangCaoService.GetListbyActive(cultureName, QuangCaoType.Silder_Right).OrderBy(x => x.Order).ToArray();
                Cache.Set(_slideService, model);
            }
            else
            {
                model = Cache.Get(_slideService) as Advertisements[];
            }

            return PartialView(model);
        }
        public ActionResult FAQ()
        {
            var mdel = slideService.GetListbyActive(SlideType.Slide, cultureName).OrderBy(x => x.Order).ToArray();
            return View();
        }
    }
}