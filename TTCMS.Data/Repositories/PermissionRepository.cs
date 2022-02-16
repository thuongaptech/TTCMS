
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class PermissionRepository: RepositoryBase<Permission>, IPermissionRepository
        {
        public PermissionRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IPermissionRepository : IRepository<Permission>
    {
    }
}