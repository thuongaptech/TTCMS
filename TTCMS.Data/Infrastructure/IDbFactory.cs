using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCMS.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TTCMSDbContext Init();
    }
}
