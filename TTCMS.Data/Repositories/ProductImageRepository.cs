

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class ProductImageRepository : RepositoryBase<ProductImage>, IProductImageRepository
        {
        public ProductImageRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IProductImageRepository : IRepository<ProductImage>
    {
    }
}