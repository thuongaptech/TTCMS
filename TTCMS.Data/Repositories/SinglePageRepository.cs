

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class SinglePageRepository : RepositoryBase<SinglePage>, ISinglePageRepository
        {
        public SinglePageRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ISinglePageRepository : IRepository<SinglePage>
    {
    }
}