using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class AccessStatisticConfiguration: EntityTypeConfiguration<AccessStatistic>
    {
        public AccessStatisticConfiguration()
        {
            Property(g => g.DayId).HasColumnOrder(1);
            Property(g => g.MonthId).HasColumnOrder(2);
            Property(g => g.YearId).HasColumnOrder(3);
        }
    }
}
