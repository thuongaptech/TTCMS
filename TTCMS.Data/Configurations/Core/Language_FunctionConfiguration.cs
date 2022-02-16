using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class Language_FunctionConfiguration: EntityTypeConfiguration<Language_Function>
    {
        public Language_FunctionConfiguration()
        {
            Property(g => g.Name).IsRequired().HasMaxLength(1000);
            Property(g => g.Description).IsRequired().IsMaxLength();
            Property(g => g.Text).IsRequired().HasMaxLength(1000);
            Property(g => g.FunctionId).IsRequired().HasMaxLength(100);
            Property(g => g.LanguageId).IsRequired().HasMaxLength(100);
        }
    }
}
