using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ILienHeService
    {
        IEnumerable<Contacts> GetList();
        IPagedList<Contacts> GetPageList(Page page);
        Contacts GetbyId(int id);
        Contacts Create(Contacts model);
        void Update(Contacts model);
        void Delete(int id);
        void SaveChange();
    }

    public class LienHeService : ILienHeService
    {
        private readonly ILienHeRepository lienHeRepository;
        private readonly IUnitOfWork unitOfWork;

        public LienHeService(ILienHeRepository lienHeRepository, IUnitOfWork unitOfWork)
        {
            this.lienHeRepository = lienHeRepository;
            this.unitOfWork = unitOfWork;
        }

        #region MenuService
        public IEnumerable<Contacts> GetList()
        {
            return lienHeRepository.GetAll();
        }
        public IPagedList<Contacts> GetPageList(Page page)
        {
            return lienHeRepository.GetPageDESC(page, x => true, order => order.CreatedDate);
        }
        public Contacts GetbyId(int id)
        {
            var model = lienHeRepository.GetById(id);
            return model;
        }
        public Contacts Create(Contacts model)
        {
            var result = lienHeRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Contacts model)
        {
            lienHeRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = lienHeRepository.GetById(id);
            lienHeRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
