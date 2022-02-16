using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;
using PagedList;

namespace TTCMS.Service
{

    public interface ILanguageService
    {
        IEnumerable<Language> GetList();
        IEnumerable<Language> GetListForActive();
        IPagedList<Language> GetList(Page page, string search);
        Language GetForId(string Id);
        Language GetForDefaultModel();
        string GetForDefault();
        void Create(Language model);
        void Update(Language model);
        void Delete(string Id);
        void SaveChange();
    }

    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository languageRepository;
        private readonly IUnitOfWork unitOfWork;

        public LanguageService(ILanguageRepository languageRepository, IUnitOfWork unitOfWork)
        {
            this.languageRepository = languageRepository;
            this.unitOfWork = unitOfWork;
        }

        #region GActionService Members

        public IEnumerable<Language> GetList()
        {
            var language = languageRepository.GetAll();
            return language;
        }
        public IEnumerable<Language> GetListForActive()
        {
            return languageRepository.GetMany(g => g.IsActived == true).OrderBy(x => x.CreatedBy).ToList();
        }
        public IPagedList<Language> GetList(Page page, string search)
        {
            if (search != "")
            {
                return languageRepository.GetPageASC(page, x => x.Name.Contains(search) || x.Id.Contains(search) || x.Description.Contains(search), order => order.Name);
            }
            return languageRepository.GetPageASC(page, x => true, order => order.Name);
        }
        public Language GetForId(string Id)
        {
            var language = languageRepository.GetById(Id);
            return language;
        }
        public Language GetForDefaultModel()
        {
            var language = GetList().SingleOrDefault(x => x.IsDefault == true);
            return language;
        }
        public string GetForDefault()
        {
            return GetList().SingleOrDefault(x => x.IsActived == true && x.IsDefault == true).Id;
        }
        public void Create(Language model)
        {
            languageRepository.Add(model);
            SaveChange();
        }
        public void Update(Language model)
        {
            languageRepository.Update(model);
            SaveChange();
        }
        public void Delete(string Id)
        {
            var language = languageRepository.GetById(Id);
            languageRepository.Delete(language);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
