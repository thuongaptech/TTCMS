using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ProductSizeConfiguration : EntityTypeConfiguration<ProductSize>
    {
        public ProductSizeConfiguration()
        {

            Property(g => g.Id).IsRequired();
            Property(g => g.NameSize).IsRequired();
            Property(g => g.ProductId).IsRequired();
            Property(g => g.CategoryId).IsRequired();
        }
    }
}
