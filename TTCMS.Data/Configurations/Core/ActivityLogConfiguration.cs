using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ActivityLogConfiguration: EntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogConfiguration()
        {
            Property(g => g.ServiceName).IsRequired().HasMaxLength(50);
            Property(g => g.MethodName).IsRequired().HasMaxLength(50);
            Property(g => g.ExecutionTime).IsRequired();
        }
    }
}
