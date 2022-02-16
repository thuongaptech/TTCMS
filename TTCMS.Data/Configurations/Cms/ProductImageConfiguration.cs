using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ProductImageConfiguration : EntityTypeConfiguration<ProductImage>
    {
        public ProductImageConfiguration()
        {

            Property(g => g.Id).IsRequired();
            Property(g => g.UrlImage).IsRequired();
            Property(g => g.ProductId).IsRequired();

        }
    }
}
