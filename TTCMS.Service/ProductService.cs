using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IProductService
    {
        IEnumerable<Product> GetList();
        IEnumerable<Product> GetListbyActive();
        IEnumerable<Product> GetListbyActive(int cate);
        IEnumerable<Product> GetListbyActive(int cate, int take);
        IPagedList<Product> GetPageList(Page page, string sreach);
        Product GetbyId(int id);
        int GetSort();
        Product Create(Product model);
        void Update(Product model);
        void Delete(int id);
        void SaveChange();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository ProductRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IProductRepository ProductRepository, IUnitOfWork unitOfWork)
        {
            this.ProductRepository = ProductRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ProductService
        public IEnumerable<Product> GetList()
        {
            var model = ProductRepository.GetAll();
            return model;
        }

        public IEnumerable<Product> GetListbyActive()
        {
            var model = ProductRepository.GetAll().Where(x=>x.IsActive==true);
            return model;
        }
        public IEnumerable<Product> GetListbyActive(int cate)
        {
            var model = ProductRepository.GetAll().Where(x => x.IsActive == true && (x.CategoryId == cate || x.Category.ParentID == cate)).OrderByDescending(x => x.Published);
            return model;
        }
        public IEnumerable<Product> GetListbyActive(int cate, int take)
        {
            var model = ProductRepository.GetAll().Where(x => x.IsActive == true && (x.CategoryId == cate || x.Category.ParentID == cate)).OrderByDescending(x => x.Published).Take(take);
            return model;
        }
        public IPagedList<Product> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return ProductRepository.GetPageDESC(page, x => x.Name.Contains(search) ||  x.Description.Contains(search) || x.Body.Contains(search), order => order.Published);
            }
            return ProductRepository.GetPageDESC(page, x => true, order => order.Published);
        }
        public Product GetbyId(int id)
        {
            var model = ProductRepository.GetById(id);
            return model;
        }
        public int GetSort()
        {
            int Sort = 0;
            var model = GetList().ToList().Count;
            if (model > 0)
            {
                int key = GetList().Max(m => m.Order);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public Product Create(Product model)
        {
            var result = ProductRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Product model)
        {
            ProductRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = ProductRepository.GetById(id);
            ProductRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
