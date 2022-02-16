
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
        {
        public LanguageRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguageRepository : IRepository<Language>
    {
    }

}