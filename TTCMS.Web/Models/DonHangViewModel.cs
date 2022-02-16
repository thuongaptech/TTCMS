using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models
{
    public class DonHangViewModel 
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập PhoneNumber.")]
        [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
         [Required(ErrorMessage = "Vui lòng nhập Email.")]
         [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }

    }
}