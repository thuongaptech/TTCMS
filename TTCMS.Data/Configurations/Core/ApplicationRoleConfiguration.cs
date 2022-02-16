using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ApplicationRoleConfiguration : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleConfiguration()
        {
            Property(c => c.CreatedBy).IsRequired().HasMaxLength(128);
            Property(c => c.CreatedDate).IsRequired();
            Property(c => c.IsActived).IsRequired();
            Property(c => c.IsDeleted).IsRequired();
        }

    }
}
