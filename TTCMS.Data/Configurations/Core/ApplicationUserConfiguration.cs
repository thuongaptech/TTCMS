using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(c => c.Email).IsRequired().HasMaxLength(150);
            Property(c => c.FirstName).IsRequired().HasMaxLength(150);
            Property(c => c.LastName).HasMaxLength(100);
            Property(c => c.Email).HasMaxLength(250);
            Property(c => c.PhoneNumber).HasMaxLength(20);
            Property(c => c.Address).HasMaxLength(500);
            Property(c => c.CreatedBy).HasMaxLength(300);
            Property(c => c.UpdatedBy).HasMaxLength(300);
            Property(c => c.ProfilePicUrl).HasMaxLength(500);
            Property(c => c.UserName).IsRequired().HasMaxLength(250);
        }

    }
}
