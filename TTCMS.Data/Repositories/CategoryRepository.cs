
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
        {
        public CategoryRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ICategoryRepository : IRepository<Category>
    {
    }
}