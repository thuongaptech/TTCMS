using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double GiaBan { get; set; }
        public double GiaKM { get; set; }
        public string Route { get; set; }
        public int CategoryId { get; set; }
        public string Img_Thumbnail { get; set; }
    }
}