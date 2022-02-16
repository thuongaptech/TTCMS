using System.Data.Entity.ModelConfiguration;
using TTCMS.Domain;

namespace TTCMS.Data.Configurations
{
    public class ProductColorConfiguration : EntityTypeConfiguration<ProductColor>
    {
        public ProductColorConfiguration()
        {

            Property(g => g.Id).IsRequired();
            Property(g => g.UrlImage).IsRequired();
            Property(g => g.ColorCode).IsRequired();
            Property(g => g.CategoryId).IsRequired();
            Property(g => g.ProductId).IsRequired();

        }
    }
}
