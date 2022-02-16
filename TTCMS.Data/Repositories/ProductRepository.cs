

using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
        {
        public ProductRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IProductRepository : IRepository<Product>
    {
    }
}