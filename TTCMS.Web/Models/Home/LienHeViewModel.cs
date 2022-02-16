using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models
{
    public class LienHeViewModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại công ty.")]
        public string PhoneNumber { get; set; }
        public string LoaiLienHe { get; set; }
         [Required(ErrorMessage = "Vui lòng nhập nội dung!")]
        public string NoiDung { get; set; }
        public bool IsActive { get; set; }

    }
}