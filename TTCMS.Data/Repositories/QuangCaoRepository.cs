
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class QuangCaoRepository : RepositoryBase<Advertisements>, IQuangCaoRepository
        {
        public QuangCaoRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IQuangCaoRepository : IRepository<Advertisements>
    {
    }
}