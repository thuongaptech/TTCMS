using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface INewsService
    {
        IEnumerable<News> GetList();
        IEnumerable<News> GetListbyActive(string search, string lang);
        IPagedList<News> GetPageList(Page page, string search, string lang );
        IPagedList<News> GetPageListByActive(Page page, string search, string lang);
        IPagedList<News> GetPageListApproves(Page page, string search, string lang);
        News GetbyId(int id);
        News Create(News model);
        void Update(News model);
        void Delete(int id);
        void SaveChange();
    }

    public class NewsService : INewsService
    {
        private readonly INewsRepository newsRepository;
        private readonly IUnitOfWork unitOfWork;

        public NewsService(INewsRepository newsRepository, IUnitOfWork unitOfWork)
        {
            this.newsRepository = newsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region NewsService
        public IEnumerable<News> GetList()
        {
            return newsRepository.GetAll();
        }
        public IEnumerable<News> GetListbyActive(string search, string lang)
        {
            var model =  GetList();
            if (search != "")
            {
                model = model.Where(x => x.Title.Contains(search) || x.Summary.Contains(search) || x.Keywords.Contains(search));
            }
            if (lang != "")
            {
                model = model.Where(x => x.LanguageId == lang || x.LanguageId == "*");
            }
            return model;
        }
        public IPagedList<News> GetPageList(Page page, string search, string lang)
        {
            if (lang != "")
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && (x.LanguageId == lang || x.LanguageId == "*") && x.IsPost == 0 && x.IsApprove == 0, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x => x.LanguageId == lang || x.LanguageId == "*" && x.IsPost == 0 && x.IsApprove == 0, order => order.Published);
            }
            else
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && x.IsPost == 0 && x.IsApprove == 0, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x => true && x.IsPost == 0 && x.IsApprove == 0, order => order.Published);
            }
        }
        public IPagedList<News> GetPageListByActive(Page page, string search, string lang)
        {
            if (lang != "")
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && (x.LanguageId == lang || x.LanguageId == "*") && x.IsActive == true, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x => (x.LanguageId == lang|| x.LanguageId == "*") && x.IsActive == true, order => order.Published);
            }
            else
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && x.IsActive == true, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x =>x.IsActive == true, order => order.Published);
            }
        }
        public News GetbyId(int id)
        {
            var model = newsRepository.GetById(id);
            return model;
        }
        public News Create(News model)
        {
            var result = newsRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(News model)
        {
            newsRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = newsRepository.GetById(id);
            newsRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        public IPagedList<News> GetPageListApproves(Page page, string search, string lang)
        {
            if (lang != "")
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && (x.LanguageId == lang || x.LanguageId == "*") && x.IsApprove == 1, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x => x.LanguageId == lang || x.LanguageId == "*" && x.IsApprove == 1, order => order.Published);
            }
            else
            {
                if (search != "")
                {
                    return newsRepository.GetPageDESC(page, x => (x.Description.Contains(search) || x.Title.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) || x.Summary.Contains(search)) && x.IsApprove == 1, order => order.Published);
                }
                return newsRepository.GetPageDESC(page, x => true && x.IsApprove == 1, order => order.Published);
            }
        }

        #endregion
    }
}
