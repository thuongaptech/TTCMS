using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;


namespace TTCMS.Domain
{

    public enum OrderStatus
    {
        [Description("Chưa xử lý")]
        Chua_Xy_Ly = 1,
        [Description("Đang xử lý")]
        Dang_Xu_Ly = 2,
        [Description("Đã xữ lý")]
        Da_Giao = 3
    }
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }
		public string FirtName { get; set; }
		public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Total { get; set; }
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
        public DateTime? DateCreate { get; set; }
        public string UserId { get; set; }
        public Order()
        {
            DateCreate = DateTime.Now;
        }
    }
}
