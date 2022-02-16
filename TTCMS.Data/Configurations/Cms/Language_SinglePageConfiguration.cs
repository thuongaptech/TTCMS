using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class Language_SinglePageConfiguration : EntityTypeConfiguration<Language_SinglePage>
    {
        public Language_SinglePageConfiguration()
        {
            Property(g => g.Title).IsRequired().HasMaxLength(500);
            Property(g => g.Summary).HasMaxLength(1000);
            Property(g => g.Description).HasMaxLength(500);
            Property(g => g.Keywords).HasMaxLength(500);
            Property(g => g.Route).IsRequired().HasMaxLength(250);
            Property(g => g.Tag).HasMaxLength(250);
            Property(g => g.SinglePageId).IsRequired();
            Property(g => g.LanguageId).IsRequired();
        }
    }
}
