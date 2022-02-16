
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
        {
        public OrderDetailRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
    }
}