using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class FunctionConfiguration: EntityTypeConfiguration<Function>
    {
        public FunctionConfiguration()
        {
            Property(g => g.Link).IsRequired().HasMaxLength(200);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.Target).IsRequired().HasMaxLength(50);
        }
    }
}
