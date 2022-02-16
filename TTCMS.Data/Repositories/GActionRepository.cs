
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class GActionRepository : RepositoryBase<GAction>, IGActionRepository
        {
        public GActionRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IGActionRepository : IRepository<GAction>
    {
    }

}