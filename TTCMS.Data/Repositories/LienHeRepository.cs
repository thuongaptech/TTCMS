
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class LienHeRepository : RepositoryBase<Contacts>, ILienHeRepository
        {
        public LienHeRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ILienHeRepository : IRepository<Contacts>
    {
    }
}