using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ILanguage_SettingService
    {
        IEnumerable<Language_Setting> GetList();
        Language_Setting GetbyId(string languageId, string id);
        Language_Setting GetbyId(int id);
        void Create(Language_Setting model);
        void Update(Language_Setting model);
        void SaveChange();
    }

    public class Language_SettingService : ILanguage_SettingService
    {
        private readonly ILanguage_SettingRepository language_SettingRepository;
        private readonly IUnitOfWork unitOfWork;

        public Language_SettingService(ILanguage_SettingRepository language_SettingRepository, IUnitOfWork unitOfWork)
        {
            this.language_SettingRepository = language_SettingRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Language_SettingService Members
        public IEnumerable<Language_Setting> GetList()
        {
            var model = language_SettingRepository.GetAll();
            return model;
        }
        public Language_Setting GetbyId(string languageId, string id)
        {
            var model = GetList().SingleOrDefault(x => x.LanguageId == languageId && x.SettingId == id);
            return model;
        }
        public Language_Setting GetbyId(int id)
        {
            var model = language_SettingRepository.GetById(id);
            return model;
        }
        public void Create(Language_Setting model)
        {
            language_SettingRepository.Add(model);
            SaveChange();
        }
        public void Update(Language_Setting model)
        {
            language_SettingRepository.Update(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
