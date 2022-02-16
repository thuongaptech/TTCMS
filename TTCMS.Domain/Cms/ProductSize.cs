using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TTCMS.Domain
{
    public class ProductSize
    {
        public ProductSize()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string NameSize { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
