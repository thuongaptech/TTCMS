using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class MenuConfiguration: EntityTypeConfiguration<Menu>
    {
        public MenuConfiguration()
        {
            Property(g => g.Target).IsRequired().HasMaxLength(20);
            Property(g => g.CssClass).HasMaxLength(250);
            Property(g => g.Name).IsRequired().HasMaxLength(500);
            Property(g => g.Link).IsRequired().HasMaxLength(250);
            Property(g => g.TextType).HasMaxLength(100);
            Property(g => g.CreatedBy).IsRequired().HasMaxLength(128);
            Property(g => g.CreatedDate).IsRequired();
            Property(g => g.UpdatedBy).HasMaxLength(128);
        }
    }
}
