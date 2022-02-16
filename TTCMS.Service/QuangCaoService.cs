using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IQuangCaoService
    {
        IEnumerable<Advertisements> GetList();
        IEnumerable<Advertisements> GetList(string lang);
        IEnumerable<Advertisements> GetListbyActive(string lang, QuangCaoType type);
        IPagedList<Advertisements> GetPageList(Page page, string sreach, string lang);
        Advertisements GetbyId(int id);
        int GetSort();
        Advertisements Create(Advertisements model);
        void Update(Advertisements model);
        void Delete(int id);
        void SaveChange();
    }

    public class QuangCaoService : IQuangCaoService
    {
        private readonly IQuangCaoRepository quangCaoRepository;
        private readonly IUnitOfWork unitOfWork;

        public QuangCaoService(IQuangCaoRepository quangCaoRepository, IUnitOfWork unitOfWork)
        {
            this.quangCaoRepository = quangCaoRepository;
            this.unitOfWork = unitOfWork;
        }

        #region CategoryService
        public IEnumerable<Advertisements> GetList()
        {
            return quangCaoRepository.GetAll();
        }
        public IEnumerable<Advertisements> GetList(string lang)
        {
            if (lang != "")
            {
                return quangCaoRepository.GetAll().Where(x => x.LanguageId == lang || x.LanguageId == "*");
            }
            return quangCaoRepository.GetAll();
        }
        public IEnumerable<Advertisements> GetListbyActive(string lang, QuangCaoType type)
        {
            if (lang != "")
            {
                return quangCaoRepository.GetAll().Where(x => x.IsActive == true && x.QuangCaoType == type && (x.LanguageId == lang || x.LanguageId == "*")).OrderBy(x => x.Order);
            }
            return quangCaoRepository.GetAll().Where(x => x.IsActive == true && x.QuangCaoType == type).OrderBy(x => x.Order);
        }
        public IPagedList<Advertisements> GetPageList(Page page, string search, string lang)
        {
            if (lang != "")
            {
                if (search != "")
                {
                    return quangCaoRepository.GetPageASC(page, x =>  x.Title.Contains(search) && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                }
                return quangCaoRepository.GetPageASC(page, x => x.LanguageId == lang || x.LanguageId == "*", order => order.Order);
            }
            else
            {
                if (search != "")
                {
                    return quangCaoRepository.GetPageASC(page, x => x.Title.Contains(search), order => order.Order);
                }
                return quangCaoRepository.GetPageASC(page, x => true, order => order.Order);
            }
           
        }
        public Advertisements GetbyId(int id)
        {
            var model = quangCaoRepository.GetById(id);
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
        public Advertisements Create(Advertisements model)
        {
            var result = quangCaoRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Advertisements model)
        {
            quangCaoRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = quangCaoRepository.GetById(id);
            quangCaoRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
