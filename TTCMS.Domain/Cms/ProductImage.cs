using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class ProductImage
    {
        public ProductImage()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
