using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Domain;
using TTCMS.Web.Areas.TT_Admin.Models;
using TTCMS.Web.Models;

namespace TTCMS.Web.Mappings
{

    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            //qc
            Mapper.CreateMap<LienHeViewModel, Contacts>();
            //qc
            Mapper.CreateMap<QuangCaoViewModel, Advertisements>();
            //slide
            Mapper.CreateMap<SlideViewModel, Slide>();
            //news
            Mapper.CreateMap<NewsViewModel, News>();
            //
            Mapper.CreateMap<ProductViewModel, Product>();
            Mapper.CreateMap<SlideViewModel, Slide>();
            //setting
            Mapper.CreateMap<SettingViewModel, Setting>();
            Mapper.CreateMap<LangSettingViewModel, Language_Setting>();
            //menu
            Mapper.CreateMap<MenuManagerViewModel, Menu>();
            //category
            Mapper.CreateMap<CategoryViewModel, Category>();
            //single page
            Mapper.CreateMap<PageViewModel, SinglePage>();
            Mapper.CreateMap<PageViewModel, Language_SinglePage>().ForMember(dest => dest.Id,
             opts => opts.MapFrom(src => src.SinglePage_Id));
            //lang
            Mapper.CreateMap<LanguageViewModel, Language>();
            Mapper.CreateMap<FunctionFormModel, Function>();
            Mapper.CreateMap<ActionLangViewModel, GAction>();
            Mapper.CreateMap<RoleViewModel, ApplicationRole>();
            Mapper.CreateMap<RoleViewModel, Language_Role>();
            Mapper.CreateMap<ActionLangViewModel, Language_GAction>().ForMember(dest => dest.Id,
             opts => opts.MapFrom(src => src.Lang_ActionId));
            Mapper.CreateMap<FunctionFormModel, Language_Function>().ForMember(dest => dest.Id,
               opts => opts.MapFrom(src => src.Lang_FunId));
            Mapper.CreateMap<ActionLangViewModel, Language_GAction>().ForMember(dest => dest.Id,
              opts => opts.MapFrom(src => src.Lang_ActionId));
        }
    }
}