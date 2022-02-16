using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCMS.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        TTCMSDbContext dbContext;

        public TTCMSDbContext Init()
        {
            return dbContext ?? (dbContext = new TTCMSDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
