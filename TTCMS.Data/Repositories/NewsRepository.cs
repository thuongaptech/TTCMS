
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class NewsRepository : RepositoryBase<News>, INewsRepository
        {
        public NewsRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface INewsRepository : IRepository<News>
    {
    }
}