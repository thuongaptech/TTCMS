using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class ProductColor
    {
        public ProductColor()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public string NameColor { get; set; }
        public string ColorCode { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
