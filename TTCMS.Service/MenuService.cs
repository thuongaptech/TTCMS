using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IMenuService
    {
        IEnumerable<Menu> GetList(MenuGroupType group, string show);
        IPagedList<Menu> GetPageList(Page page, string sreach);
        Menu GetbyId(int id);
        int GetSort(MenuGroupType group, string show);
        Menu Create(Menu model);
        void Update(Menu model);
        void Delete(int id);
        void SaveChange();
    }

    public class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;
        private readonly IUnitOfWork unitOfWork;

        public MenuService(IMenuRepository menuRepository, IUnitOfWork unitOfWork)
        {
            this.menuRepository = menuRepository;
            this.unitOfWork = unitOfWork;
        }

        #region MenuService
        public IEnumerable<Menu> GetList(MenuGroupType group, string show)
        {
            if (show != "")
            {
                return menuRepository.GetAll().Where(x => x.GroupType == group && x.LanguageId == show);
            }
            else
            {
                return menuRepository.GetAll().Where(x => x.GroupType == group);
            }
        }
        public IPagedList<Menu> GetPageList(Page page, string search)
        {
            if (search != "")
            {
                return menuRepository.GetPageASC(page, x => x.Name.Contains(search), order => order.Order);
            }
            return menuRepository.GetPageASC(page, x => true, order => order.Order);
        }
        public Menu GetbyId(int id)
        {
            var model = menuRepository.GetById(id);
            return model;
        }
        public int GetSort(MenuGroupType group, string show)
        {
            int Sort = 0;
            var model = GetList(group, show).ToList().Count;
            if (model > 0)
            {
                int key = GetList(group, show).Max(m => m.Order);
                Sort = key + 1;
            }
            else
            {
                Sort = 1;
            }
            return Sort;
        }
        public Menu Create(Menu model)
        {
            var result = menuRepository.Add(model);
            SaveChange();
            return result;
        }
        public void Update(Menu model)
        {
            menuRepository.Update(model);
            SaveChange();
        }
        public void Delete(int id)
        {
            var list = menuRepository.GetAll().Where(x => x.ParentID == id);
            foreach (var item in list)
            {
                menuRepository.Delete(item);
            }
            var model = menuRepository.GetById(id);
            menuRepository.Delete(model);
            SaveChange();
        }

        public void SaveChange()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
