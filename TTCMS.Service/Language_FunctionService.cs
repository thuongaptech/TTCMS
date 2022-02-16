using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ILanguage_FunctionService
    {
        IEnumerable<Language_Function> GetLanguage_Function();
        Language_Function Language_Function(string languageId, string functionId);
        bool Language_FunctionCheck(string languageId, string functionId);
        void CreateLanguage_Function(Language_Function language_function);
        void UpdateLanguage_Function(Language_Function language_function);
        void DeleteLanguage_Function(string languageId, string functionId);
        void DeleteLanguage_FunctionForFun(string functionId);
        void SaveLanguage_Function();
    }

    public class Language_FunctionService : ILanguage_FunctionService
    {
        private readonly ILanguage_FunctionRepository language_FunctionRepository;
        private readonly IUnitOfWork unitOfWork;

        public Language_FunctionService(ILanguage_FunctionRepository language_FunctionRepository, IUnitOfWork unitOfWork)
        {
            this.language_FunctionRepository = language_FunctionRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Language_FunctionService

        public IEnumerable<Language_Function> GetLanguage_Function()
        {
            var language = language_FunctionRepository.GetAll();
            return language;
        }
        public Language_Function Language_Function(string languageId, string functionId)
        {
            return GetLanguage_Function().SingleOrDefault(x => x.LanguageId == languageId && x.FunctionId == functionId);
        }
        public bool Language_FunctionCheck(string languageId, string functionId)
        {
            var language = GetLanguage_Function().SingleOrDefault(x => x.LanguageId == languageId && x.FunctionId == functionId);
            if (language != null)
            {
                return true;
            }
            return false;
        }
        public void CreateLanguage_Function(Language_Function language_function)
        {
            language_FunctionRepository.Add(language_function);
            SaveLanguage_Function();
        }
        public void UpdateLanguage_Function(Language_Function language_function)
        {
            language_FunctionRepository.Update(language_function);
            SaveLanguage_Function();
        }
        public void DeleteLanguage_Function(string languageId, string functionId)
        {
            var language = GetLanguage_Function().SingleOrDefault(x => x.LanguageId == languageId && x.FunctionId == functionId);
            language_FunctionRepository.Delete(language);
            SaveLanguage_Function();
        }
        public void DeleteLanguage_FunctionForFun(string functionId)
        {
            var language = language_FunctionRepository.GetMany(x => x.FunctionId == functionId);
            foreach (var lang in language)
            {
                language_FunctionRepository.Delete(lang);
            }
           
            SaveLanguage_Function();
        }
        public void SaveLanguage_Function()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
