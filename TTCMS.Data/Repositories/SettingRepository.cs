
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
        {
        public SettingRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ISettingRepository : IRepository<Setting>
    {
    }
}