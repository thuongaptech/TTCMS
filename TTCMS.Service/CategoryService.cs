using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface ICategoryService
    {
        IEnumerable<Category> GetList(CategoryType filter);
        IEnumerable<Category> GetList(CategoryType filter, string lang);
        IEnumerable<Category> GetListbyActive(CategoryType filter, string lang);
        IPagedList<Category> GetPageList(Page page, string sreach, CategoryType filter, string lang);
        Category GetbyId(int id);
        int GetSort(CategoryType filter);
        Category Create(Category model);
        void Update(Category model);
        void Delete(int id);
        void SaveChange();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this.categoryRepository = categoryRepository;
            this.unitOfWork = unitOfWork;
        }

        #region CategoryService
        public IEnumerable<Category> GetList(CategoryType filter)
        {
            switch (filter)
            {
                case CategoryType.News:
                    {
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.News);
                    }
                case CategoryType.Product:
                    {
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Product);
                    }
                case CategoryType.Size:
                    {
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Size);
                    }
                case CategoryType.Color:
                    {
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Color);
                    }
                case CategoryType.District:
                    {
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.District);
                    }
                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IEnumerable<Category> GetList(CategoryType filter, string lang)
        {
            switch (filter)
            {
                case CategoryType.News:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.News && (x.LanguageId== lang || x.LanguageId =="*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.News);
                    }
                case CategoryType.Product:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Product && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Product);
                    }
                case CategoryType.Size:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Size && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Size);
                    }
                case CategoryType.Color:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Color && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Color);
                    }
                case CategoryType.District:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.District && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.District);
                    }
                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IEnumerable<Category> GetListbyActive(CategoryType filter, string lang)
        {
            switch (filter)
            {
                case CategoryType.News:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.News && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.News && x.IsActive == true);
                    }
                case CategoryType.Product:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Product && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Product && x.IsActive == true);
                    }
                case CategoryType.Size:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Size && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Size && x.IsActive == true);
                    }
                case CategoryType.Color:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Color && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.Color && x.IsActive == true);
                    }
                case CategoryType.District:
                    {
                        if (lang != "")
                        {
                            return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.District && x.IsActive == true && (x.LanguageId == lang || x.LanguageId == "*"));
                        }
                        return categoryRepository.GetAll().Where(x => x.CategoryType == CategoryType.District && x.IsActive == true);
                    }
                default:
                    {
                        throw new ApplicationException("Filter not understood");
                    }
            }
        }
        public IPagedList<Category> GetPageList(Page page, string search, CategoryType filter, string  lang)
        {
            if (lang != "")
            {
                switch (filter)
                {
                    case CategoryType.News:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.News && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.News && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case CategoryType.Product:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Product && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Product && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case CategoryType.Size:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Size && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Size && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case CategoryType.Color:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Color && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Color && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    case CategoryType.District:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.District && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.District && (x.LanguageId == lang || x.LanguageId == "*"), order => order.Order);
                        }
                    default:
                        {
                            throw new ApplicationException("Filter not understood");
                        }
                }
            }
            else
            {
                switch (filter)
                {
                    case CategoryType.News:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.News, order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.News, order => order.Order);
                        }
                    case CategoryType.Product:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Product, order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Product, order => order.Order);
                        }
                    case CategoryType.Size:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Size, order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Size, order => order.Order);
                        }
                    case CategoryType.Color:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.Color, order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.Color, order => order.Order);
                        }
                    case CategoryType.District:
                        {
                            if (search != "")
                            {
                                return categoryRepository.GetPageASC(page, x => x.Description.Contains(search) || x.Name.Contains(search) || x.Keywords.Contains(search) || x.Route.Contains(search) && x.CategoryType == CategoryType.District, order => order.Order);
                            }
                            return categoryRepository.GetPageASC(page, x => x.CategoryType == CategoryType.District, order => order.Order);
                        }
                    default:
                        {
                            throw new ApplicationException("Filter not understood");
                        }
                }
            }
           
        }
        public Category GetbyId(int id)
        {
            var model = categoryRepository.GetById(id);
            return model;
        }
        public int GetSort(CategoryType filter)
        {
            int Sort = 0;
            var model = GetList(filter).ToList().Count;
            if (model > 0)
            {
                int key = GetList(filter).Max(m => m.Order);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public Category Create(Category model)
        {
            var result = categoryRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Category model)
        {
            categoryRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var model = categoryRepository.GetById(id);
            categoryRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
