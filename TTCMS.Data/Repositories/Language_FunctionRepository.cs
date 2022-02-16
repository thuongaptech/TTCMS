
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class Language_FunctionRepository : RepositoryBase<Language_Function>, ILanguage_FunctionRepository
        {
        public Language_FunctionRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILanguage_FunctionRepository : IRepository<Language_Function>
    {
    }

}