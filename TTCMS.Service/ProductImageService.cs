using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IProductImageService
    {
        IEnumerable<ProductImage> GetList();
        IEnumerable<ProductImage> GetListbyPro(int Id);
        IPagedList<ProductImage> GetPageList(Page page, string sreach);
        ProductImage GetbyId(int id);
        int GetSort();
        ProductImage Create(ProductImage model);
        void Update(ProductImage model);
        void Delete(int id);
        void SaveChange();
        void DeleteByProductId(int id);
    }

    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository ProductImageRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductImageService(IProductImageRepository ProductImageRepository, IUnitOfWork unitOfWork)
        {
            this.ProductImageRepository = ProductImageRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ProductImageService
        public IEnumerable<ProductImage> GetList()
        {
            var model = ProductImageRepository.GetAll();
            return model;
        }
        public  IEnumerable<ProductImage> GetListbyPro(int Id)
        {
            return ProductImageRepository.GetAll().Where(x => x.ProductId == Id);

        }
        public IPagedList<ProductImage> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return ProductImageRepository.GetPageASC(page, x =>  x.UrlImage.Contains(search), order => order.CreatedDate);
            }
            return ProductImageRepository.GetPageASC(page, x => true, order => order.CreatedDate);
        }
        public ProductImage GetbyId(int id)
        {
            var model = ProductImageRepository.GetById(id);
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
        public ProductImage Create(ProductImage model)
        {
            var result = ProductImageRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(ProductImage model)
        {
            ProductImageRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = ProductImageRepository.GetById(id);
            ProductImageRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }
        public void DeleteByProductId(int id)
        {
            var model = ProductImageRepository.GetAll().Where(x => x.ProductId == id);
            if (model != null)
            {
                foreach (var item in model)
                {
                    var m_model = ProductImageRepository.GetById(item.Id);
                    ProductImageRepository.Delete(m_model);
                    SaveChange();
                }

            }
        }
        #endregion
    }
}
