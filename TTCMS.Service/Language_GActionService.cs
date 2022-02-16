using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;
using PagedList;

namespace TTCMS.Service
{

    public interface ILanguage_GActionService
    {
        IEnumerable<Language_GAction> GetLanguage_GAction();
        IPagedList<Language_GAction> GetLanguage_GAction(Page page, string search, string langId);
        Language_GAction Language_GAction(string languageId, string gactionId);
        bool Language_GActionCheck(string languageId, string gactionId);
        void CreateLanguage_GAction(Language_GAction language_gaction);
        void UpdateLanguage_GAction(Language_GAction language_gaction);
        void DeleteLanguage_GAction(string languageId, string gactionId);
        void DeleteLanguage_GActionForAction(string gactionId);
        void SaveChange();
    }

    public class Language_GActionService : ILanguage_GActionService
    {
        private readonly ILanguage_GActionRepository language_GActionRepository;
        private readonly IUnitOfWork unitOfWork;

        public Language_GActionService(ILanguage_GActionRepository language_GActionRepository, IUnitOfWork unitOfWork)
        {
            this.language_GActionRepository = language_GActionRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Language_FunctionService

        public IEnumerable<Language_GAction> GetLanguage_GAction()
        {
            var model = language_GActionRepository.GetAll();
            return model;
        }
        public IPagedList<Language_GAction> GetLanguage_GAction(Page page,string search, string langId)
        {
            if (search != null)
            {
                return language_GActionRepository.GetPageASC(page, x => x.LanguageId == langId && (x.Name.Contains(search)||x.Description.Contains(search)), order => order.Name);
            }
            return language_GActionRepository.GetPageASC(page, x => x.LanguageId == langId, order => order.Name);
        }
        public Language_GAction Language_GAction(string languageId, string gactionId)
        {
            return GetLanguage_GAction().SingleOrDefault(x => x.LanguageId == languageId && x.GActionId == gactionId);
        }
        public bool Language_GActionCheck(string languageId, string gactionId)
        {
            var language = GetLanguage_GAction().SingleOrDefault(x => x.LanguageId == languageId && x.GActionId == gactionId);
            if (language != null)
            {
                return true;
            }
            return false;
            
        }
        public void CreateLanguage_GAction(Language_GAction language_gaction)
        {
            language_GActionRepository.Add(language_gaction);
            SaveChange();
        }
        public void UpdateLanguage_GAction(Language_GAction language_gaction)
        {
            language_GActionRepository.Update(language_gaction);
            SaveChange();
        }
        public void DeleteLanguage_GAction(string languageId, string gactionId)
        {
            var language = GetLanguage_GAction().SingleOrDefault(x => x.LanguageId == languageId && x.GActionId == gactionId);
            language_GActionRepository.Delete(language);
            SaveChange();
        }
        public void DeleteLanguage_GActionForAction(string gactionId)
        {
            var language = language_GActionRepository.GetMany (x => x.GActionId == gactionId);
            foreach (var lang in language)
            {
                language_GActionRepository.Delete(lang);
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
