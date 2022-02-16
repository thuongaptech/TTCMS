using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TTCMS.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        public string FullName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số địa chỉ liên hệ.")]
        public string Address { get; set; }
        public bool Sex { get; set; }
    }
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "User name")]
        public string TaiKhoan { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        public string FullName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{9,11}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số địa chỉ liên hệ.")]
        public string Address { get; set; }
        public bool Sex { get; set; }
    }
    public class ChangeViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.", MinimumLength = 6)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu mới không khớp.")]
        public string ConfirmPassword { get; set; }
    }
    public class QuenMatKhauViewModel
    {
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ!")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        public string UserName { get; set; }
    }
    public class LayLaiMKViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không hợp lệ")]
        public string ConfirmPassword { get; set; }
    }
}