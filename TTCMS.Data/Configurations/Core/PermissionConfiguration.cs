using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class PermissionConfiguration: EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            Property(g => g.Id).IsRequired();
            Property(g => g.FunctionId).IsRequired().HasMaxLength(100);
            Property(g => g.GActionId).IsRequired().HasMaxLength(100);
            Property(g => g.RoleId).IsRequired().HasMaxLength(100);
        }
    }
}
