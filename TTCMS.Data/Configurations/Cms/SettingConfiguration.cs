using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class SettingConfiguration: EntityTypeConfiguration<Setting>
    {
        public SettingConfiguration()
        {
            Property(g => g.Id).IsRequired().HasMaxLength(128);
            Property(g => g.ApplicationURL).HasMaxLength(250);
            Property(g => g.Address).HasMaxLength(500);
            Property(g => g.EmailAccount).HasMaxLength(250);
            Property(g => g.EmailPassword).HasMaxLength(256);
            Property(g => g.RecieveEmail).HasMaxLength(250);
        }
    }
}
