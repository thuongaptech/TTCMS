using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ILanguage_SinglePageService
    {
        IEnumerable<Language_SinglePage> GetList();
        Language_SinglePage GetbyId(string languageId, int id);
        bool Check(string languageId, int Id);
        void Create(Language_SinglePage model);
        void Update(Language_SinglePage model);
        void Delete(int id);
        void SaveChange();
    }

    public class Language_SinglePageService : ILanguage_SinglePageService
    {
        private readonly ILanguage_SinglePageRepository language_SinglePageRepository;
        private readonly IUnitOfWork unitOfWork;

        public Language_SinglePageService(ILanguage_SinglePageRepository language_SinglePageRepository, IUnitOfWork unitOfWork)
        {
            this.language_SinglePageRepository = language_SinglePageRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Language_CategoryService Members
        public IEnumerable<Language_SinglePage> GetList()
        {
            var model = language_SinglePageRepository.GetAll();
            return model;
        }
        public Language_SinglePage GetbyId(string languageId, int id)
        {
            var model = GetList().SingleOrDefault(x => x.LanguageId == languageId && x.SinglePageId == id);
            return model;
        }
        public bool Check(string languageId, int Id)
        {
            var language = GetList().SingleOrDefault(x => x.LanguageId == languageId && x.SinglePageId == Id);
            if (language != null)
            {
                return true;
            }
            return false;

        }
        public void Create(Language_SinglePage model)
        {
            language_SinglePageRepository.Add(model);
            SaveChange();
        }
        public void Update(Language_SinglePage model)
        {
            language_SinglePageRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = language_SinglePageRepository.GetMany(x => x.SinglePageId == id);
            foreach (var lang in model)
            {
                language_SinglePageRepository.Delete(lang);
            }
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
