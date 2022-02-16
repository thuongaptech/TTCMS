using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Domain;

namespace TTCMS.Web.Models.Product
{
    public class CateHomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Route { get; set; }
        public int? ParentID { get; set; }
        public string Target { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public CategoryType CategoryType { get; set; }
        public int Order { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsHome { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public string CodeColor { get; set; }
        public int ShowProduct { get; set; }
        public int CountProduct { get; set; }
        public bool ActiveClass { get; set; }
        public List<ProductHomeViewModel> ListProductNews { get; set; }
        public List<ProductHomeViewModel> ListProductHot { get; set; }
        public List<ProductHomeViewModel> ListProductPrice { get; set; }
        public List<CateHomeViewModel> ListParent { get; set; }
    }
    
    public class ProductHomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaSP { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public double GiaBan { get; set; }
        public double GiaKM { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public int Order { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public bool IsBanChay { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Published { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int DaMua { get; set; }
        public double Percent
        {
            get { return Math.Round(Convert.ToDouble(100 - ((GiaKM * 100) / GiaBan))); }
        }
        public double Price
        {
            get { return GiaKM > 0 ? GiaKM : GiaBan; }
        }
    }
    public class ThongTinDatHang
    {
        public ProductHomeViewModel[] CartList { get; set; }
        public DonHangViewModel ThongTin { get; set; }
    }
    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MaSP { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public double GiaBan { get; set; }
        public double GiaKM { get; set; }
        public string Route { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Tag { get; set; }
        public int Views { get; set; }
        public int Order { get; set; }
        public string Img_Icon { get; set; }
        public string Img_Thumbnail { get; set; }
        public string CssClass { set; get; }
        public bool IsActive { get; set; }
        public bool IsHot { get; set; }
        public bool IsBanChay { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Published { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int DaMua { get; set; }
        public string  CategoryName { get; set; }
        public string CategoryRoute { get; set; }
        public double Percent
        {
            get { return Math.Round(Convert.ToDouble(100 - ((GiaKM * 100) / GiaBan))); }
        }
        public double Price
        {
            get { return GiaKM > 0 ? GiaKM : GiaBan; }
        }
        public List<HinhAnh> ListHinhAnh { get; set; }
        public List<Size> ListSize { get; set; }
        public List<MauSac> ListMauSac { get; set; }
        public List<ProductDetailViewModel> SPLienQuan { get; set; }
    }
    public class HinhAnh
    {
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public int ProductId { get; set; }
    }
    public class MauSac
    {
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public string NameColor { get; set; }
        public string ColorCode { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
    public class Size
    {
        public int Id { get; set; }
        public string NameSize { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
    public class BanChayViewModel
    {
        public List<ProductHomeViewModel> ListBanChay { get; set; }
        public List<ProductHomeViewModel> ListMoiNhat { get; set; }
    }
    public class ThongTinDonHangViewModel
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
        public IEnumerable<ProductDetailViewModel> ListPro { get; set; }
    }
    public class HomeDonHangViewModel
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
        public IEnumerable<ProductDetailViewModel> ListPro { get; set; }
    }
}