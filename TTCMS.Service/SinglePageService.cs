using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ISinglePageService
    {
        IEnumerable<SinglePage> GetList();
        IPagedList<SinglePage> GetPageList(Page page, string sreach);
        SinglePage GetbyId(int id);
        int GetSort();
        SinglePage Create(SinglePage model);
        void Update(SinglePage model);
        void Delete(int id);
        void SaveChange();
    }

    public class SinglePageService : ISinglePageService
    {
        private readonly ISinglePageRepository singlePageRepository;
        private readonly IUnitOfWork unitOfWork;

        public SinglePageService(ISinglePageRepository singlePageRepository, IUnitOfWork unitOfWork)
        {
            this.singlePageRepository = singlePageRepository;
            this.unitOfWork = unitOfWork;
        }

        #region SinglePageService
        public IEnumerable<SinglePage> GetList()
        {
            var model = singlePageRepository.GetAll();
            return model;
        }
        public IPagedList<SinglePage> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return singlePageRepository.GetPageASC(page, x => x.Language_SinglePages.Any(s => s.Description.Contains(search) || s.Title.Contains(search) || s.Keywords.Contains(search) || s.Route.Contains(search)), order => order.Order);
            }
            return singlePageRepository.GetPageASC(page, x => true, order => order.Order);
        }
        public SinglePage GetbyId(int id)
        {
            var model = singlePageRepository.GetById(id);
            return model;
        }
        public int GetSort()
        {
            int Sort = 0;
            var model = GetList().ToList().Count;
            if (model > 0)
            {
                int key = GetList().Max(m => m.Order);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public SinglePage Create(SinglePage model)
        {
            var result = singlePageRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(SinglePage model)
        {
            singlePageRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = singlePageRepository.GetById(id);
            singlePageRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
