using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class LanguageConfiguration: EntityTypeConfiguration<Language>
    {
        public LanguageConfiguration()
        {
            Property(g => g.Id).IsRequired().HasMaxLength(5);
            Property(g => g.Name).IsRequired().HasMaxLength(50);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(100);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.Description).HasMaxLength(500);
            Property(g => g.Img_Icon).HasMaxLength(500);
        }
    }
}
