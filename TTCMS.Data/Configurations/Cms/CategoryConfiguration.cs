using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            Property(g => g.Target).IsRequired().HasMaxLength(20);
            Property(g => g.Img_Icon).HasMaxLength(250);
            Property(g => g.Img_Thumbnail).HasMaxLength(250);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.UpdatedBy).HasMaxLength(128);
            Property(g => g.Name).IsRequired().HasMaxLength(250);
            Property(g => g.Description).HasMaxLength(500);
            Property(g => g.Keywords).HasMaxLength(500);
            Property(g => g.Route).IsRequired().HasMaxLength(250);
            Property(g => g.LanguageId).IsRequired();
        }
    }
}
