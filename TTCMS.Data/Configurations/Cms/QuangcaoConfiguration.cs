using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class QuangCaoConfiguration : EntityTypeConfiguration<Advertisements>
    {
        public QuangCaoConfiguration()
        {
            Property(g => g.Id).IsRequired();
            Property(g => g.Title).IsRequired().HasMaxLength(250);
            Property(g => g.Images).IsRequired().HasMaxLength(250);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.Target).IsRequired().HasMaxLength(20);
            Property(g => g.UpdatedBy).HasMaxLength(128);
            Property(g => g.LinkRedirec).IsRequired().HasMaxLength(250);
            Property(g => g.LanguageId).IsRequired();
        }
    }
}
