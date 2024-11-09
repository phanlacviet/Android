using K22CNT2_PhanLacViet_2210900079_project2.Models;
using K22CNT2_PhanLacViet_2210900079_project2.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace K22CNT2_PhanLacViet_2210900079_project2.Controllers
{
    public class ShoppingCartController : Controller
    {
        private PhanLacViet_K22CNT2_2210900079_project2Entities db = new PhanLacViet_K22CNT2_2210900079_project2Entities();
        // GET: ShoppingCart
        public List<CartItem> GetCartItems()
        {
            return Session["Cart"] as List<CartItem> ?? new List<CartItem>();
        }

        private string GenerateOrderId()
        {
            return Guid.NewGuid().ToString().Substring(0, 10); // Tạo mã đơn hàng duy nhất  
        }

        private void ClearCart()
        {
            Session["Cart"] = null;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SanPham()
        {
            var sanPhams = db.SanPhams.Include("NhaCC").ToList();
            return View(sanPhams);
        }
        public ActionResult ShoppingCart()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }
        public ActionResult AddToCart(string id)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa  
            var khachHang = Session["khachhang"] as KhachHang; // Lấy thông tin khách hàng từ session  
            if (khachHang == null)
            {
                // Nếu chưa đăng nhập, điều hướng họ đến trang đăng nhập
                return RedirectToAction("Login", "Home");
            }

            var product = db.SanPhams.Find(id);
            if (product != null)
            {
                var cartItems = GetCartItems();
                var existingItem = cartItems.FirstOrDefault(c => c.MaSP == id);

                if (existingItem != null)
                {
                    existingItem.SoLuong++;
                }
                else
                {
                    cartItems.Add(new CartItem
                    {
                        MaSP = product.MaSP,
                        TenSP = product.TenSP,
                        GiaCa = product.GiaCa,
                        SoLuong = 1
                    });
                }
                Session["Cart"] = cartItems; // Lưu giỏ hàng vào session  
            }

            return RedirectToAction("ShoppingCart");
        }

        // Thanh toán  
        [HttpPost]
        public ActionResult Checkout()
        {
            var khachHang = Session["khachhang"] as KhachHang;
            if (khachHang == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = GetCartItems();
            if (!cartItems.Any())
            {
                TempData["Message"] = "Giỏ hàng trống. Vui lòng thêm sản phẩm vào giỏ hàng để thanh toán.";
                return RedirectToAction("Index", "Products");
            }

            string maDH = GenerateOrderId();
            var ngayDat = DateTime.Now;
            int thanhTien = cartItems.Sum(c => c.GiaCa * c.SoLuong);

            // Tạo đơn hàng  
            DonHang donHang = new DonHang
            {
                MaDH = maDH,
                MaKH = khachHang.MaKH,
                NgayDat = ngayDat,
                ThanhTien = thanhTien
            };

            db.DonHangs.Add(donHang);
            db.SaveChanges();
            // Thêm chi tiết đơn hàng và cập nhật số lượng sản phẩm trong kho  
            foreach (var item in cartItems)
            {
                // Tìm sản phẩm trong kho  
                var sanPham = db.SanPhams.Find(item.MaSP);
                if (sanPham != null)
                {
                    // Kiểm tra xem số lượng sản phẩm trong kho có đủ không  
                    if (sanPham.SoLuong >= item.SoLuong)
                    {
                        // Cập nhật số lượng sản phẩm trong kho  
                        sanPham.SoLuong -= item.SoLuong;

                        // Thêm chi tiết đơn hàng  
                        CTDH ctDonHang = new CTDH
                        {
                            MaDH = maDH,
                            MaSp = item.MaSP,
                            Soluong = item.SoLuong,
                        };
                        db.CTDHs.Add(ctDonHang);
                    }
                    else
                    {
                        TempData["Message"] = $"Không đủ hàng cho sản phẩm: {sanPham.TenSP}";
                        return RedirectToAction("ShoppingCart");
                    }
                }
            }

            db.SaveChanges();
            ClearCart(); // Xóa giỏ hàng sau khi thanh toán  

            return RedirectToAction("Index", "Home"); 
        }
        public ActionResult RemoveShoppingCartItem(String id)
        {
            var cartItems = GetCartItems();
            var itemToRemove = cartItems.FirstOrDefault(c => c.MaSP == id);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove); // Xóa sản phẩm khỏi giỏ hàng  
                Session["Cart"] = cartItems; // Cập nhật giỏ hàng trong session  
            }

            return RedirectToAction("ShoppingCart");
        }
    }

}