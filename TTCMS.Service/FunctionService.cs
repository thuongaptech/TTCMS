using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using PagedList;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IFunctionService
    {
        IEnumerable<Function> GetFunction();
        IPagedList<Function> GetFunction(Page page);
        Function Function(string id);
        int GetFunctionSort();
        bool CanAddFunction(string FunctionId);
        void CreateFunction(Function function);
        void UpdateFunction(Function function);
        void DeleteFunction(string id);
        void SaveFunction();
    }

    public class FunctionService : IFunctionService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IFunctionRepository functionRepository;
        private readonly IUnitOfWork unitOfWork;

        public FunctionService(IFunctionRepository functionRepository, IUnitOfWork unitOfWork, IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            this.functionRepository = functionRepository;
            this.unitOfWork = unitOfWork;
        }

        #region FunctionService Members

        public IEnumerable<Function> GetFunction()
        {
            var function = functionRepository.GetAll();
            return function;
        }
        public ApplicationRole GetRole(string roleId)
        {
            return roleRepository.Get(u => u.Id == roleId);
        }
        public IPagedList<Function> GetFunction(Page page)
        {
            return functionRepository.GetPageASC(page, x => true, order => order.Order);
        }
        public Function Function(string id)
        {
            var function = functionRepository.GetById(id);
            return function;
        }
        public int GetFunctionSort()
        {
            int Sort = 0;
            int key = GetFunction().Max(m => m.Order);
            if (GetFunction().ToList().Count == 0 || key == 0)
            {
                Sort = 1;
            }
            else
            {
                Sort = key + 1;
            }
            return Sort;
        }
        public bool CanAddFunction(string FunctionId)
        {
            var function = functionRepository.GetById(FunctionId);
            if (function == null)
                return true;
            else
                return false;
        }
        public void CreateFunction(Function function)
        {
            functionRepository.Add(function);
            SaveFunction();
        }
        public void UpdateFunction(Function function)
        {
            functionRepository.Update(function);
            SaveFunction();
        }
        public void DeleteFunction(string id)
        {
            var function = functionRepository.GetById(id);
            functionRepository.Delete(function);
            SaveFunction();
        }

        public void SaveFunction()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
