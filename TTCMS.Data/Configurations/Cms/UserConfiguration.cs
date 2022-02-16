using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class UserConfiguration: EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(g => g.Id).IsRequired();
            Property(g => g.UserName).IsRequired().HasMaxLength(250);
            Property(g => g.Password).IsRequired();
            Property(g => g.FirtName);
            Property(g => g.LastName);
            Property(g => g.Phone).HasMaxLength(11);
            Property(g => g.Token).IsRequired();
            Property(g => g.Address).IsRequired();
            Property(g => g.Fax);
            Property(g => g.Email).IsRequired();
            Property(g => g.Created);
        }
    }
}
