using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class LienHeConfiguration: EntityTypeConfiguration<Contacts>
    {
        public LienHeConfiguration()
        {
            Property(g => g.Id).IsRequired();
            Property(g => g.FullName).IsRequired().HasMaxLength(250);
            Property(g => g.Email).IsRequired().HasMaxLength(250);
            Property(g => g.PhoneNumber).IsRequired().HasMaxLength(128);
            Property(g => g.NoiDung).IsRequired();
        }
    }
}
