using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IOrderDetailService
    {
        IEnumerable<OrderDetail> GetList();
        IEnumerable<OrderDetail> GetListbyOrder(int Id);
        OrderDetail GetbyId(int id);
        int TongDaMua(int id);
        OrderDetail Create(OrderDetail model);
        void Update(OrderDetail model);
        void Delete(int id);
        void SaveChange();
    }

    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderDetailService(IOrderDetailRepository _orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderDetailRepository = _orderDetailRepository;
            this.unitOfWork = unitOfWork;
        }

        #region OrderDetailService
        public IEnumerable<OrderDetail> GetList()
        {
            var model = _orderDetailRepository.GetAll();
            return model;
        }
        public IEnumerable<OrderDetail> GetListbyOrder(int Id)
        {
            var model = _orderDetailRepository.GetAll().Where(x=>x.OrderID == Id);
            return model;
        }
        public OrderDetail GetbyId(int id)
        {
            var model = _orderDetailRepository.GetById(id);
            return model;
        }
        public int TongDaMua(int id)
        {
            return _orderDetailRepository.GetMany(x => x.ProductID == id).Count(); ;
        }
        public OrderDetail Create(OrderDetail model)
        {
            var result = _orderDetailRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(OrderDetail model)
        {
            _orderDetailRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = GetListbyOrder(id);
            foreach (var item in model)
            {
                _orderDetailRepository.Delete(item);
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
