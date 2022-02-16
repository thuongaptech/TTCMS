
using TTCMS.Data.Infrastructure;
using TTCMS.Domain;
namespace TTCMS.Data.Repositories
{
    public class SlideRepository : RepositoryBase<Slide>, ISlideRepository
        {
        public SlideRepository(IDbFactory dbFactory)
            : base(dbFactory)
            {
            }           
        }
    public interface ISlideRepository : IRepository<Slide>
    {
    }
}