using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCMS.Data
{
    public class TTCMSDbInitializer : DropCreateDatabaseIfModelChanges<TTCMSDbContext>
    {
        protected override void Seed(TTCMSDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(TTCMSDbContext context)
        {
            // initial configuration will go here
        }

    }
}
