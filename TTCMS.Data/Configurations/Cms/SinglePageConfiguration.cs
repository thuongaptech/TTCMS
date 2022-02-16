using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class SinglePageConfiguration : EntityTypeConfiguration<SinglePage>
    {
        public SinglePageConfiguration()
        {
            Property(g => g.Img_Thumbnail).HasMaxLength(250);
            Property(g => g.CssClass).HasMaxLength(50);
            Property(g => g.CreatedById).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.UpdatedById).HasMaxLength(128);
        }
    }
}
