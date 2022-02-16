using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class NewsConfiguration: EntityTypeConfiguration<News>
    {
         public NewsConfiguration()
        {
            Property(g => g.Id).IsRequired();
            Property(g => g.Tag).HasMaxLength(500);
            Property(g => g.Img_Thumbnail).HasMaxLength(250);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.Published).IsRequired();
            Property(g => g.UpdatedBy).HasMaxLength(128);
            Property(g => g.Description).HasMaxLength(500);
            Property(g => g.Keywords).HasMaxLength(500);
            Property(g => g.Route).IsRequired().HasMaxLength(250);
            Property(g => g.LanguageId).IsRequired();
        }
    }
}
