
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
        {
        public OrderRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IOrderRepository : IRepository<Order>
    {
    }
}