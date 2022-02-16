
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class FunctionRepository: RepositoryBase<Function>, IFunctionRepository
        {
        public FunctionRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IFunctionRepository : IRepository<Function>
    {
    }
}