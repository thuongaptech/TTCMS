
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class ActivityLogRepository : RepositoryBase<ActivityLog>, IActivityLogRepository
        {
        public ActivityLogRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
    }

}