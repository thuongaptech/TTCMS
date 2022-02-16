using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TTCMS.Domain;

namespace TTCMS.Web.Areas.TT_Admin.Models
{
    public class DonHangPageViewModel
    {
        public DonHangTableViewModel TableList { get; set; }
    }
    public class DonHangTableViewModel
    {
        public string Search { get; set; }
        public IPagedList<DonHangAdminViewModel> ModelList { get; set; }
    }
    public class DonHangAdminViewModel
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

    }
    public class ListDonHangViewModel
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
        public IEnumerable<ProductViewModel> ListPro { get; set; }
    }
}