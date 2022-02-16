using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class OrderConfiguration: EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {

            Property(g => g.Id).IsRequired();
            Property(g => g.UserName).HasMaxLength(250).IsRequired();
            Property(g => g.FirtName).HasMaxLength(250);
            Property(g => g.LastName).HasMaxLength(250);
            Property(g => g.Phone).IsRequired().HasMaxLength(11);
            Property(g => g.DateCreate).IsRequired();
            Property(g => g.Email).IsRequired();
            Property(g => g.Address).HasMaxLength(250);
            Property(g => g.Total);
            Property(g => g.Status);
        }
    }
    public class OrderDetailConfiguration : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailConfiguration()
        {

            Property(g => g.Id).IsRequired();
            Property(g => g.OrderID).IsRequired();
            Property(g => g.ProductID).IsRequired();
            Property(g => g.Price);
            Property(g => g.Quanlity);
           
        }
    }
}
