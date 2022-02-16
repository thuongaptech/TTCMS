
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class Language_GActionRepository : RepositoryBase<Language_GAction>, ILanguage_GActionRepository
        {
        public Language_GActionRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguage_GActionRepository : IRepository<Language_GAction>
    {
    }

}