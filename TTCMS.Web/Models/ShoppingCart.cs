using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TTCMS.Data;
using TTCMS.Domain;

public class ShoppingCart
{
    // Lấy giỏ hàng từ Session
    public static ShoppingCart Cart
    {
        get
        {
            var cart = HttpContext.Current.Session["Cart"] as ShoppingCart;
            // Nếu chưa có giỏ hàng trong session -> tạo mới và lưu vào session
            if (cart == null)
            {
                cart = new ShoppingCart();
                HttpContext.Current.Session["Cart"] = cart;
            }
            return cart;
        }
    }

    // Chứa các mặt hàng đã chọn
    public List<Product> Items = new List<Product>();

    public void Add(int id, int quantity)
    {
        try // tìm thấy trong giỏ -> tăng số lượng lên 1
        {
            var item = Items.Single(i => i.Id == id);
            item.Views += quantity;
        }
        catch // chưa có trong giỏ -> truy vấn CSDL và bỏ vào giỏ
        {
            var db = new TTCMSDbContext();
            var item = db.Products.Find(id);
            item.Views = quantity;
            Items.Add(item);
        }
    }
    
    public void Remove(int id)
    {
        var item = Items.Single(i => i.Id == id);
        Items.Remove(item);
    }
    
    public void Update(int id, int newQuantity)
    {
        var item = Items.Single(i => i.Id == id);
        item.Views = newQuantity;
    }

    public void Clear()
    {
        Items.Clear();
    }

    public int Count
    {
        get
        {
            return Items.Sum(x=>x.Views);
        }
    }
    
    public double Total
    {
        get
        {
            return Items.Sum(p => 
                p.GiaKM * p.Views);
        }
    }
}