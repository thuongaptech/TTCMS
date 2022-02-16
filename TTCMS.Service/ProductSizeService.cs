using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IProductSizeService
    {
        IEnumerable<ProductSize> GetList();
        IEnumerable<ProductSize> GetListbyPro(int Id);
        IPagedList<ProductSize> GetPageList(Page page, string sreach);
        ProductSize GetbyId(int id);
        int GetSort();
        ProductSize Create(ProductSize model);
        void Update(ProductSize model);
        void Delete(int id);
        void SaveChange();
        void DeleteByProductId(int id);
    }

    public class ProductSizeService : IProductSizeService
    {
        private readonly IProductSizeRepository productSizeRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductSizeService(IProductSizeRepository productSizeRepository, IUnitOfWork unitOfWork)
        {
            this.productSizeRepository = productSizeRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ProductSizeService
        public IEnumerable<ProductSize> GetList()
        {
            var model = productSizeRepository.GetAll();
            return model;
        }
        public IEnumerable<ProductSize> GetListbyPro(int Id)
        {
            var model = productSizeRepository.GetAll().Where(x => x.ProductId == Id);
            return model;
        }
        public IPagedList<ProductSize> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return productSizeRepository.GetPageASC(page, x => x.NameSize.Contains(search), order => order.CreatedDate);
            }
            return productSizeRepository.GetPageASC(page, x => true, order => order.CreatedDate);
        }
        public ProductSize GetbyId(int id)
        {
            var model = productSizeRepository.GetById(id);
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
        public ProductSize Create(ProductSize model)
        {
            var result = productSizeRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(ProductSize model)
        {
            productSizeRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = productSizeRepository.GetById(id);
            productSizeRepository.Delete(model);
            SaveChange();
        }
        public void DeleteByProductId(int id)
        {
            var model = productSizeRepository.GetAll().Where(x => x.ProductId == id);
            if (model != null)
            {
                foreach (var item in model)
                {
                    var m_model = productSizeRepository.GetById(item.Id);
                    productSizeRepository.Delete(m_model);
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
