using System.Collections.Generic;
using System.Linq;
using System;
using TTCMS.Domain;
using TTCMS.Data.Repositories;
using TTCMS.Data.Infrastructure;

namespace TTCMS.Service
{

    public interface IPermissionService
    {
        IEnumerable<Permission> GetPermission();
        IEnumerable<Permission> GetPermissionForRole(string role);
        IEnumerable<Permission> GetPermissionForUser(List<string> userName);
        Permission Permission(int id);
        void CreatePermission(Permission permission);
        void DeletePermission(int id);
        void DeletePermissionbyFunctionId(string id);
        void SavePermission();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IUnitOfWork unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.permissionRepository = permissionRepository;
            this.unitOfWork = unitOfWork;
        }

        #region PermissionService Members

        public IEnumerable<Permission> GetPermission()
        {
            var permission = permissionRepository.GetAll();
            return permission;
        }
        public IEnumerable<Permission> GetPermissionForRole(string role)
        {
            return permissionRepository.GetMany(g => g.RoleId == role).ToList();
        }
        public IEnumerable<Permission> GetPermissionForUser(List<string> userName)
        {
            return permissionRepository.GetMany(g => userName.Contains(g.RoleId)).Distinct().ToList();
        }

        public Permission Permission(int id)
        {
            var permission = permissionRepository.GetById(id);
            return permission;
        }

        public void CreatePermission(Permission permission)
        {
            permissionRepository.Add(permission);
            SavePermission();
        }

        public void DeletePermission(int id)
        {
            var permission = permissionRepository.GetById(id);
            permissionRepository.Delete(permission);
            SavePermission();
        }
        public void DeletePermissionbyFunctionId(string id)
        {
            var permission = GetPermission().Where(x => x.FunctionId == id);
            foreach (var item in permission)
            {
                permissionRepository.Delete(item);
            }
            SavePermission();
        }
        public void SavePermission()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
