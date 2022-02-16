using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTCMS.Domain
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
		public int ProductID { get; set; }
		public int Quanlity { get; set; }
        public double Price { get; set; }
       
    }
}
