using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ISlideService
    {
        IEnumerable<Slide> GetList(SlideType filter);
        IEnumerable<Slide> GetList(SlideType filter, string lang);
        IEnumerable<Slide> GetListbyActive(SlideType filter, string lang);
        IPagedList<Slide> GetPageList(Page page, string sreach, SlideType filter, string lang);
        Slide GetbyId(int id);
        int GetSort(SlideType filter);
        Slide Create(Slide model);
        void Update(Slide model);
        void Delete(int id);
        void SaveChange();
    }

    public class SlideService : ISlideService
    {
        private readonly ISlideRepository slideRepository;
        private readonly IUnitOfWork unitOfWork;

        public SlideService(ISlideRepository slideRepository, IUnitOfWork unitOfWork)
        {
            this.slideRepository = slideRepository;
            this.unitOfWork = unitOfWork;
        }

        #region CategoryService
        public IEnumerable<Slide> GetList(SlideType filter)
        {
            switch (filter)
            {
                case SlideType.Slide:
                    {
                        var model = slideRepository.GetAll().Where(x => x.SlideType == SlideType.Slide);
                        return model;
                    }
                case SlideType.QuangCao:
                    {
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.QuangCao);
                    }
                case SlideType.DoiTac:
                    {
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.DoiTac);
                    }

                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IEnumerable<Slide> GetList(SlideType filter, string lang)
        {
            switch (filter)
            {
                case SlideType.Slide:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.Slide && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.Slide);
                    }
                case SlideType.QuangCao:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.QuangCao && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.QuangCao);
                    }
                case SlideType.DoiTac:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.DoiTac && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.DoiTac);
                    }
                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IEnumerable<Slide> GetListbyActive(SlideType filter, string lang)
        {
            switch (filter)
            {
                case SlideType.Slide:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.Slide && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*")).OrderBy(x=>x.Order);
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.Slide && x.IsActive == true).OrderBy(x => x.Order);
                    }
                case SlideType.QuangCao:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.QuangCao && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*")).OrderBy(x => x.Order);
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.QuangCao && x.IsActive == true);
                    }
                case SlideType.DoiTac:
                    {
                        if (lang != "")
                        {
                            return slideRepository.GetAll().Where(x => x.SlideType == SlideType.DoiTac && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*")).OrderBy(x => x.Order);
                        }
                        return slideRepository.GetAll().Where(x => x.SlideType == SlideType.DoiTac && x.IsActive == true).OrderBy(x => x.Order);
                    }
                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IPagedList<Slide> GetPageList(Page page, string search, SlideType filter, string lang)
        {
            if (lang != "")
            {
                switch (filter)
                {
                    case SlideType.Slide:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.Slide && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.Slide && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case SlideType.QuangCao:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.QuangCao && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.QuangCao && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case SlideType.DoiTac:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.DoiTac && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.DoiTac && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    default:
                        {
                            throw new ApplicationException("Filter not understood");
                        }
                }
            }
            else
            {
                switch (filter)
                {
                    case SlideType.Slide:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.Slide, order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.Slide, order => order.Order);
                        }
                    case SlideType.QuangCao:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.QuangCao, order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.QuangCao, order => order.Order);
                        }
                    case SlideType.DoiTac:
                        {
                            if (search != "")
                            {
                                return slideRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) && x.SlideType == SlideType.DoiTac, order => order.Order);
                            }
                            return slideRepository.GetPageASC(page, x => x.SlideType == SlideType.DoiTac, order => order.Order);
                        }
                    default:
                        {
                            throw new ApplicationException("Filter not understood");
                        }
                }
            }
           
        }
        public Slide GetbyId(int id)
        {
            var model = slideRepository.GetById(id);
            return model;
        }
        public int GetSort(SlideType filter)
        {
            int Sort = 0;
            var model = GetList(filter).ToList().Count;
            if (model > 0)
            {
                int key = GetList(filter).Max(m => m.Order);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public Slide Create(Slide model)
        {
            var result = slideRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Slide model)
        {
            slideRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = slideRepository.GetById(id);
            slideRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
