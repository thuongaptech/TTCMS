using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class Language_GActionConfiguration: EntityTypeConfiguration<Language_GAction>
    {
        public Language_GActionConfiguration()
        {
            Property(g => g.Name).IsRequired().HasMaxLength(200);
            Property(g => g.Description).IsRequired().HasMaxLength(500);
            Property(g => g.GActionId).IsRequired().HasMaxLength(100);
            Property(g => g.LanguageId).IsRequired().HasMaxLength(100);
        }
    }
}
