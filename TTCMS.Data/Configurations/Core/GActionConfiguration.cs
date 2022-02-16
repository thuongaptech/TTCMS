using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class GActionConfiguration: EntityTypeConfiguration<GAction>
    {
        public GActionConfiguration()
        {
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(120);
            Property(g => g.CreatedDate).IsRequired();
        }
    }
}
