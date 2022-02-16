
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class UserRepository: RepositoryBase<ApplicationUser>, IUserRepository
        {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }        
        }
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        
    }
}