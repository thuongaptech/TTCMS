

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class ProductSizeRepository : RepositoryBase<ProductSize>, IProductSizeRepository
        {
        public ProductSizeRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IProductSizeRepository : IRepository<ProductSize>
    {
    }
}