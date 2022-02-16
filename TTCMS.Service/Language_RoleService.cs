using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;
using PagedList;

namespace TTCMS.Service
{

    public interface ILanguage_RoleService
    {
        IEnumerable<Language_Role> GetLanguage_Role();
        Language_Role Language_Role(string languageId, string roleId);
        bool Language_RoleCheck(string languageId, string roleId);
        void CreateLanguage_Role(Language_Role language_role);
        void UpdateLanguage_Role(Language_Role language_role);
        void DeleteLanguage_Role(string languageId, string language_role);
        void DeleteLanguage_RoleForRole(string language_role);
        void SaveChange();
    }

    public class Language_RoleService : ILanguage_RoleService
    {
        private readonly ILanguage_RoleRepository language_RoleRepository;
        private readonly IUnitOfWork unitOfWork;

        public Language_RoleService(ILanguage_RoleRepository language_RoleRepository, IUnitOfWork unitOfWork)
        {
            this.language_RoleRepository = language_RoleRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Language_FunctionService

        public IEnumerable<Language_Role> GetLanguage_Role()
        {
            var model = language_RoleRepository.GetAll();
            return model;
        }
        public Language_Role Language_Role(string languageId, string roleId)
        {
            return GetLanguage_Role().SingleOrDefault(x => x.LanguageId == languageId && x.RoleId == roleId);
        }
        public bool Language_RoleCheck(string languageId, string roleId)
        {
            var language = GetLanguage_Role().SingleOrDefault(x => x.LanguageId == languageId && x.RoleId == roleId);
            if (language != null)
            {
                return true;
            }
            return false;
            
        }
        public void CreateLanguage_Role(Language_Role language_role)
        {
            language_RoleRepository.Add(language_role);
            SaveChange();
        }
        public void UpdateLanguage_Role(Language_Role language_role)
        {
            language_RoleRepository.Update(language_role);
            SaveChange();
        }
        public void DeleteLanguage_Role(string languageId, string roleId)
        {
            var language = GetLanguage_Role().SingleOrDefault(x => x.LanguageId == languageId && x.RoleId == roleId);
            language_RoleRepository.Delete(language);
            SaveChange();
        }
        public void DeleteLanguage_RoleForRole(string roleId)
        {
            var language = language_RoleRepository.GetMany(x => x.RoleId == roleId);
            foreach (var lang in language)
            {
                language_RoleRepository.Delete(lang);
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
