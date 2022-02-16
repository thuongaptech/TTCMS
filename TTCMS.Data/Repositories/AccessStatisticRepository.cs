
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class AccessStatisticRepository : RepositoryBase<AccessStatistic>, IAccessStatisticRepository
        {
        public AccessStatisticRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IAccessStatisticRepository : IRepository<AccessStatistic>
    {
    }

}