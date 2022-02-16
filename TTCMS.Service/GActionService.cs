using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;
using PagedList;

namespace TTCMS.Service
{

    public interface IGActionService
    {
        IEnumerable<GAction> GetGAction();
        IPagedList<GAction> GetGAction(Page page, string search);
        GAction GAction(string gactionId);
        bool CheckGAction(string gactionId);
        void CreateGAction(GAction gaction);
        void UpdateGAction(GAction gaction);
        void DeleteGAction(string gactionId);
        void SaveGAction();
    }

    public class GActionService : IGActionService
    {
        private readonly IGActionRepository gactionRepository;
        private readonly IUnitOfWork unitOfWork;

        public GActionService(IGActionRepository gactionRepository, IUnitOfWork unitOfWork)
        {
            this.gactionRepository = gactionRepository;
            this.unitOfWork = unitOfWork;
        }

        #region GActionService Members

        public IEnumerable<GAction> GetGAction()
        {
            var GAction = gactionRepository.GetAll();
            return GAction;
        }
        public IPagedList<GAction> GetGAction(Page page, string search)
        {
            if (search != "")
            {
                return gactionRepository.GetPageDESC(page, x => x.Language_GActions.Any(s => s.Name.Contains(search) || s.Description.Contains(search)), order => order.CreatedDate);
            }
            return gactionRepository.GetPageDESC(page, x => true, order => order.CreatedDate);
        }
        public GAction GAction(string gactionId)
        {
            var GAction = gactionRepository.GetById(gactionId);
            return GAction;
        }
        public bool CheckGAction(string gactionId)
        {
            var GAction = gactionRepository.GetById(gactionId);
            if (GAction != null)
            {
                return true;
            }
            return false;
        }
        public void CreateGAction(GAction gaction)
        {
            gactionRepository.Add(gaction);
            SaveGAction();
        }
        public void UpdateGAction(GAction gaction)
        {
            gactionRepository.Update(gaction);
            SaveGAction();
        }
        public void DeleteGAction(string gactionId)
        {
            var GAction = gactionRepository.GetById(gactionId);
            gactionRepository.Delete(GAction);
            SaveGAction();
        }

        public void SaveGAction()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
