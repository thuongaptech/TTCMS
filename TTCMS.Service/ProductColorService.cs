using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IProductColorService
    {
        IEnumerable<ProductColor> GetList();
        IEnumerable<ProductColor> GetListbyPro(int Id);
        IPagedList<ProductColor> GetPageList(Page page, string sreach);
        ProductColor GetbyId(int id);
        int GetSort();
        ProductColor Create(ProductColor model);
        void Update(ProductColor model);
        void Delete(int id);
        void SaveChange();
        void DeleteByProductId(int id);
    }

    public class ProductColorService : IProductColorService
    {
        private readonly IProductColorRepository ProductColorRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductColorService(IProductColorRepository ProductColorRepository, IUnitOfWork unitOfWork)
        {
            this.ProductColorRepository = ProductColorRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ProductColorService
        public IEnumerable<ProductColor> GetList()
        {
            var model = ProductColorRepository.GetAll();
            return model;
        }
        public IEnumerable<ProductColor> GetListbyPro(int Id)
        {
            var model = ProductColorRepository.GetAll().Where(x=>x.ProductId== Id);
            return model;
        }
        public IPagedList<ProductColor> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return ProductColorRepository.GetPageASC(page, x => x.ColorCode.Contains(search) ||  x.UrlImage.Contains(search), order => order.CreatedDate);
            }
            return ProductColorRepository.GetPageASC(page, x => true, order => order.CreatedDate);
        }
        public ProductColor GetbyId(int id)
        {
            var model = ProductColorRepository.GetById(id);
            return model;
        }
        public int GetSort()
        {
            int Sort = 0;
            var model = GetList().ToList().Count;
            if (model > 0)
            {
                int key = GetList().Max(m => m.Id);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public ProductColor Create(ProductColor model)
        {
            var result = ProductColorRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(ProductColor model)
        {
            ProductColorRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = ProductColorRepository.GetById(id);
            ProductColorRepository.Delete(model);
            SaveChange();
        }
        public void DeleteByProductId(int id)
        {
            var model = ProductColorRepository.GetAll().Where(x => x.ProductId == id);
            if (model != null)
            {
                foreach (var item in model)
                {
                    var m_model = ProductColorRepository.GetById(item.Id);
                    ProductColorRepository.Delete(m_model);
                    SaveChange();
                }

            }
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
