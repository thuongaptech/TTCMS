using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class Language_RoleConfiguration: EntityTypeConfiguration<Language_Role>
    {
        public Language_RoleConfiguration()
        {
            Property(g => g.Description).IsRequired().HasMaxLength(1000);
            Property(g => g.RoleId).IsRequired().HasMaxLength(100);
            Property(g => g.LanguageId).IsRequired().HasMaxLength(100);
        }
    }
}
