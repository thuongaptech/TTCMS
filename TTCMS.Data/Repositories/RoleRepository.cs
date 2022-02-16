
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class RoleRepository: RepositoryBase<ApplicationRole>, IRoleRepository
        {
        public RoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }        
        }
    public interface IRoleRepository : IRepository<ApplicationRole>
    {
        
    }
}