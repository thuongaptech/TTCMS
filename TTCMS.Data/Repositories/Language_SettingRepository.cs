
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class Language_SettingRepository : RepositoryBase<Language_Setting>, ILanguage_SettingRepository
        {
        public Language_SettingRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguage_SettingRepository : IRepository<Language_Setting>
    {
    }
}