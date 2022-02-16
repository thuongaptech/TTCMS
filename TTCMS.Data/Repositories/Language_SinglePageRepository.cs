

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class Language_SinglePageRepository: RepositoryBase<Language_SinglePage>, ILanguage_SinglePageRepository
        {
        public Language_SinglePageRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguage_SinglePageRepository : IRepository<Language_SinglePage>
    {
    }
}