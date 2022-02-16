using AutoMapper;
using TTCMS.Domain;
using TTCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Web.Areas.TT_Admin.Models;
using PagedList;
using TTCMS.Core.AutoMapperConverters;
using TTCMS.Web.Models.Product;
using TTCMS.Web.Models.News;

namespace TTCMS.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Order, HomeDonHangViewModel>();
            ////Config
            Mapper.CreateMap<ConfigViewModel, CauHinhViewModel>();
            Mapper.CreateMap<CauHinhViewModel, ConfigViewModel>();
            //home single page
            Mapper.CreateMap<SinglePage, SinglePageHomeViewModel>();
            Mapper.CreateMap<IPagedList<SinglePage>, IPagedList<SinglePageHomeViewModel>>().ConvertUsing<PagedListConverter<SinglePage, SinglePageHomeViewModel>>();
            Mapper.CreateMap<Tuple<SinglePage, Language_SinglePage>, SinglePageHomeViewModel>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item1.Id))
                .ForMember(d => d.Views, opt => opt.MapFrom(s => s.Item1.Views))
                 .ForMember(d => d.Img_Thumbnail, opt => opt.MapFrom(s => s.Item1.Img_Thumbnail))
                 .ForMember(d => d.CssClass, opt => opt.MapFrom(s => s.Item1.CssClass))
                   .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.Item1.IsActive))
                    .ForMember(d => d.IsHot, opt => opt.MapFrom(s => s.Item1.IsHot))
                   .ForMember(d => d.IsHome, opt => opt.MapFrom(s => s.Item1.IsHome))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Item1.Order))
                .ForMember(d => d.CreatedById, opt => opt.MapFrom(s => s.Item1.CreatedById))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.Item1.CreatedDate))
                .ForMember(d => d.SinglePage_Id, opt => opt.MapFrom(s => s.Item2.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Item2.Title))
               .ForMember(d => d.Summary, opt => opt.MapFrom(s => s.Item2.Summary))
                .ForMember(d => d.Body, opt => opt.MapFrom(s => s.Item2.Body))
                 .ForMember(d => d.Route, opt => opt.MapFrom(s => s.Item2.Route))
                .ForMember(d => d.Keywords, opt => opt.MapFrom(s => s.Item2.Keywords))
                .ForMember(d => d.Tag, opt => opt.MapFrom(s => s.Item2.Tag))
                .ForMember(d => d.LanguageId, opt => opt.MapFrom(s => s.Item2.LanguageId))
                 .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Item2.Description));
            ////don hang
            Mapper.CreateMap<Order, ListDonHangViewModel>();
            Mapper.CreateMap<Order, DonHangAdminViewModel>();
            Mapper.CreateMap<IPagedList<Order>, IPagedList<DonHangAdminViewModel>>().ConvertUsing<PagedListConverter<Order, DonHangAdminViewModel>>();
            ////pro image
            Mapper.CreateMap<ProductImage, HinhAnh>();
            ////pro color
            Mapper.CreateMap<ProductColor, MauSac>();
            ////pro size
            Mapper.CreateMap<ProductSize, Size>();
            ////pro detail
            Mapper.CreateMap<Product, ProductDetailViewModel>();
            ////home cate
            Mapper.CreateMap<Category, CateHomeViewModel>();
            ////lien he
            Mapper.CreateMap<Contacts, ContactViewModel>();
            Mapper.CreateMap<IPagedList<Contacts>, IPagedList<ContactViewModel>>().ConvertUsing<PagedListConverter<Contacts, ContactViewModel>>();
            ////qc
            Mapper.CreateMap<Advertisements, QuangCaoViewModel>();
            Mapper.CreateMap<IPagedList<Advertisements>, IPagedList<QuangCaoViewModel>>().ConvertUsing<PagedListConverter<Advertisements, QuangCaoViewModel>>();
            ////slide
            Mapper.CreateMap<Slide, SlideViewModel>();
            Mapper.CreateMap<IPagedList<Slide>, IPagedList<SlideViewModel>>().ConvertUsing<PagedListConverter<Slide, SlideViewModel>>();
            //config
            Mapper.CreateMap<Setting, CauHinhViewModel>();

            Mapper.CreateMap<Product, ProductVM>();
            //news
            Mapper.CreateMap<News, NewsViewModel>();
            Mapper.CreateMap<IPagedList<News>, IPagedList<NewsViewModel>>().ConvertUsing<PagedListConverter<News, NewsViewModel>>();
            Mapper.CreateMap<Slide, SlideViewModel>();
            Mapper.CreateMap<IPagedList<Slide>, IPagedList<SlideViewModel>>().ConvertUsing<PagedListConverter<Slide, SlideViewModel>>();
            //product
            //product
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<Product, ProductHomeViewModel>();
            Mapper.CreateMap<Product, ProductVM>();
            Mapper.CreateMap<IPagedList<Product>, IPagedList<ProductHomeViewModel>>().ConvertUsing<PagedListConverter<Product, ProductHomeViewModel>>();
            Mapper.CreateMap<IPagedList<Product>, IPagedList<ProductViewModel>>().ConvertUsing<PagedListConverter<Product, ProductViewModel>>();
            //setting
            Mapper.CreateMap<Language_Setting, LangSettingViewModel>();
            Mapper.CreateMap<Setting, SettingViewModel>();
            Mapper.CreateMap<Language, ListLang>();
            //menu
            Mapper.CreateMap<Menu, MenuManagerViewModel>();
            //single page
            Mapper.CreateMap<SinglePage, PageViewModel>();
            Mapper.CreateMap<IPagedList<SinglePage>, IPagedList<PageViewModel>>().ConvertUsing<PagedListConverter<SinglePage, PageViewModel>>();
            Mapper.CreateMap<Tuple<SinglePage, Language_SinglePage>, PageViewModel>()
              .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item1.Id))
                .ForMember(d => d.Views, opt => opt.MapFrom(s => s.Item1.Views))
                 .ForMember(d => d.Img_Thumbnail, opt => opt.MapFrom(s => s.Item1.Img_Thumbnail))
                 .ForMember(d => d.CssClass, opt => opt.MapFrom(s => s.Item1.CssClass))
                   .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.Item1.IsActive))
                    .ForMember(d => d.IsHot, opt => opt.MapFrom(s => s.Item1.IsHot))
                   .ForMember(d => d.IsHome, opt => opt.MapFrom(s => s.Item1.IsHome))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Item1.Order))
                .ForMember(d => d.CreatedById, opt => opt.MapFrom(s => s.Item1.CreatedById))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.Item1.CreatedDate))
                .ForMember(d => d.SinglePage_Id, opt => opt.MapFrom(s => s.Item2.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Item2.Title))
               .ForMember(d => d.Summary, opt => opt.MapFrom(s => s.Item2.Summary))
                .ForMember(d => d.Body, opt => opt.MapFrom(s => s.Item2.Body))
                 .ForMember(d => d.Route, opt => opt.MapFrom(s => s.Item2.Route))
                .ForMember(d => d.Keywords, opt => opt.MapFrom(s => s.Item2.Keywords))
                .ForMember(d => d.Tag, opt => opt.MapFrom(s => s.Item2.Tag))
                .ForMember(d => d.LanguageId, opt => opt.MapFrom(s => s.Item2.LanguageId))
                 .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Item2.Description));
            //category
            Mapper.CreateMap<Category, CategoryViewModel>();
            Mapper.CreateMap<IPagedList<Category>, IPagedList<CategoryViewModel>>().ConvertUsing<PagedListConverter<Category, CategoryViewModel>>();
            //category type
           //Language
            Mapper.CreateMap<Language, LanguageItemViewModel>();
            Mapper.CreateMap<Language, LanguageViewModel>();
            Mapper.CreateMap<IPagedList<Language>, IPagedList<LanguageViewModel>>().ConvertUsing<PagedListConverter<Language, LanguageViewModel>>();
            //user manager
            Mapper.CreateMap<ApplicationUser, UserFormModel>();
            Mapper.CreateMap<ApplicationUser, UserProfileViewModel>();
            //role manager
            Mapper.CreateMap<ApplicationRole, RoleViewModel>();
            Mapper.CreateMap<ApplicationRole, RoleOptiViewModel>();
            //Log manager
            Mapper.CreateMap<ActivityLog, LogActionViewModel>();
            Mapper.CreateMap<ActivityLog, LogViewModel>();
            Mapper.CreateMap<IPagedList<ActivityLog>, IPagedList<LogViewModel>>().ConvertUsing<PagedListConverter<ActivityLog, LogViewModel>>();
            //gaction
            Mapper.CreateMap<GAction, ActionLangViewModel>();
            Mapper.CreateMap<IPagedList<GAction>, IPagedList<ActionLangViewModel>>().ConvertUsing<PagedListConverter<GAction, ActionLangViewModel>>();
            Mapper.CreateMap<IPagedList<Language_GAction>, IPagedList<ActionLangViewModel>>().ConvertUsing<PagedListConverter<Language_GAction, ActionLangViewModel>>();
            Mapper.CreateMap<GAction, ActionLangViewModel>();
            //Group  map
            Mapper.CreateMap<Tuple<ApplicationRole, Language_Role>, RoleViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item1.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Item1.Name))
                  .ForMember(d => d.IsActived, opt => opt.MapFrom(s => s.Item1.IsActived))
                   .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.Item1.CreatedDate))
                     .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Item1.CreatedBy))
                  .ForMember(d => d.IsDeleted, opt => opt.MapFrom(s => s.Item1.IsDeleted))
                  .ForMember(d => d.Language_RoleId, opt => opt.MapFrom(s => s.Item2.Language_RoleId))
                 .ForMember(d => d.LanguageId, opt => opt.MapFrom(s => s.Item2.LanguageId))
                   .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Item2.Description));

            Mapper.CreateMap<Tuple<GAction, Language_GAction>, ActionLangViewModel>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item1.Id))
                   .ForMember(d => d.IsActived, opt => opt.MapFrom(s => s.Item1.IsActived))
                    .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.Item1.CreatedDate))
                      .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Item1.CreatedBy))
                   .ForMember(d => d.IsDeleted, opt => opt.MapFrom(s => s.Item1.IsDeleted))
                   .ForMember(d => d.Lang_ActionId, opt => opt.MapFrom(s => s.Item2.Id))
                  .ForMember(d => d.LanguageId, opt => opt.MapFrom(s => s.Item2.LanguageId))
                   .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Item2.Name))
                    .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Item2.Description));
            Mapper.CreateMap<Tuple<Function, Language_Function>, FunctionFormModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Item1.Id))
                .ForMember(d => d.LanguageId, opt => opt.MapFrom(s => s.Item2.LanguageId))
                 .ForMember(d => d.Link, opt => opt.MapFrom(s => s.Item1.Link))
                 .ForMember(d => d.Target, opt => opt.MapFrom(s => s.Item1.Target))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Item1.Order))
                 .ForMember(d => d.CssClass, opt => opt.MapFrom(s => s.Item1.CssClass))
             .ForMember(d => d.IsLocked, opt => opt.MapFrom(s => s.Item1.IsLocked))
                 .ForMember(d => d.IsDeleted, opt => opt.MapFrom(s => s.Item1.IsDeleted))
                 .ForMember(d => d.IsFavorited, opt => opt.MapFrom(s => s.Item1.IsFavorited))
                 .ForMember(d => d.CreatedDate, opt => opt.MapFrom(s => s.Item1.CreatedDate))
                 .ForMember(d => d.CreatedBy, opt => opt.MapFrom(s => s.Item1.CreatedBy))
                 .ForMember(d => d.UpdatedDate, opt => opt.MapFrom(s => s.Item1.UpdatedDate))
                  .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.Item1.UpdatedBy))
                 .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Item2.Name))
                 .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Item2.Description))
                 .ForMember(d => d.Text, opt => opt.MapFrom(s => s.Item2.Text))
            .ForMember(d => d.ParentID, opt => opt.MapFrom(s => s.Item1.ParentID));
            Mapper.CreateMap<ApplicationUser, UserViewModel>();

          
        }


    }
}