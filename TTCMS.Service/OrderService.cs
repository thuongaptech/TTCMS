using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IOrderService
    {
        IEnumerable<Order> GetList();
        IPagedList<Order> GetPageList(Page page, string sreach);
        Order GetbyId(int id);
        Order Create(Order model);
        void Update(Order model);
        void Delete(int id);
        void SaveChange();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IOrderRepository _repository, IUnitOfWork unitOfWork)
        {
            this._repository = _repository;
            this.unitOfWork = unitOfWork;
        }

        #region OrderService
        public IEnumerable<Order> GetList()
        {
            var model = _repository.GetAll().OrderByDescending(x => x.DateCreate);
            return model;
        }

        public IPagedList<Order> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return _repository.GetPageDESC(page, x => x.UserName.Contains(search) || x.Phone.Contains(search) || x.Email.Contains(search), order => order.DateCreate);
            }
            return _repository.GetPageDESC(page, x => true, order => order.DateCreate);
        }
        public Order GetbyId(int id)
        {
            var model = _repository.GetById(id);
            return model;
        }
        public Order Create(Order model)
        {
            var result = _repository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Order model)
        {
            _repository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = _repository.GetById(id);
            _repository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
