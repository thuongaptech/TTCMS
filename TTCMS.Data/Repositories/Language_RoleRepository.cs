
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class Language_RoleRepository : RepositoryBase<Language_Role>, ILanguage_RoleRepository
        {
        public Language_RoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguage_RoleRepository : IRepository<Language_Role>
    {
    }

}