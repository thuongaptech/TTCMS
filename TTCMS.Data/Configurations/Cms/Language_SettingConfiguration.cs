using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class Language_SettingConfiguration: EntityTypeConfiguration<Language_Setting>
    {
        public Language_SettingConfiguration()
        {
            Property(g => g.ApplicationName).HasMaxLength(500);
            Property(g => g.Description).HasMaxLength(500);
            Property(g => g.Keywords).HasMaxLength(500);
            Property(g => g.SettingId).IsRequired();
            Property(g => g.LanguageId).IsRequired();
        }
    }
}
