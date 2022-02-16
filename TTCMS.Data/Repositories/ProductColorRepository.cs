

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class ProductColorRepository : RepositoryBase<ProductColor>, IProductColorRepository
        {
        public ProductColorRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IProductColorRepository : IRepository<ProductColor>
    {
    }
}