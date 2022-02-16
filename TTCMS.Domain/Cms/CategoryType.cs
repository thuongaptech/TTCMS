using System.ComponentModel;

namespace TTCMS.Domain
{
    public enum CategoryType
    {
        [Description("News")]
        News,
        [Description("Product")]
        Product,
        [Description("Size")]
        Size,
        [Description("Color")]
        Color,
        [Description("District")]
        District,
    }
}
