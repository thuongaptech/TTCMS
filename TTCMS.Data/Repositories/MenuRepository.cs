
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class MenuRepository : RepositoryBase<Menu>, IMenuRepository
        {
        public MenuRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IMenuRepository : IRepository<Menu>
    {
    }
}