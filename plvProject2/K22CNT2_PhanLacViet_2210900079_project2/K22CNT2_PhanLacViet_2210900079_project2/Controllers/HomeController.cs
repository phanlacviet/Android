using K22CNT2_PhanLacViet_2210900079_project2.Areas.QuanTri.Controllers;
using K22CNT2_PhanLacViet_2210900079_project2.Models;
using K22CNT2_PhanLacViet_2210900079_project2.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace K22CNT2_PhanLacViet_2210900079_project2.Controllers
{
    public class HomeController : Controller
    {
        private PhanLacViet_K22CNT2_2210900079_project2Entities db = new PhanLacViet_K22CNT2_2210900079_project2Entities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang khachhangdangky)
        {
            db.KhachHangs.Add(khachhangdangky);
            db.SaveChanges();
            return RedirectToAction("Login") ;
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KhachHang khachhangdangky)
        {
            var taikhoan = khachhangdangky.TaiKhoan;
            var matkhau = khachhangdangky.MatKhau;
            var checkkhachhang = db.KhachHangs.SingleOrDefault(x => x.TaiKhoan.Equals(taikhoan) && x.MatKhau.Equals(matkhau));
            if(checkkhachhang != null)
            {
                Session["khachhang"] = checkkhachhang;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.Loginfail = "Tài khoản hoặc mật khẩu sai, vui lòng kiểm tra lại";
                return View("Login");
            }
        }

        // Đăng xuất  
        public ActionResult Logout()
        {
            Session["khachhang"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Thongtincanhan(String id)
        {
            // Lấy thông tin khách hàng từ session  
            var khachhang = Session["khachhang"] as KhachHang;

            // Kiểm tra xem khách hàng có tồn tại trong session không  
            if (khachhang == null)
            {
                return RedirectToAction("Login"); // Nếu không có khách hàng trong session, chuyển hướng đến trang đăng nhập  
            }

            // Tìm kiếm khách hàng trong cơ sở dữ liệu bằng MaKH  
            KhachHang khachHang = db.KhachHangs.Find(khachhang.MaKH);
            if (khachHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy khách hàng, trả về 404  
            }
            return View(khachHang);
        }
        public ActionResult DonHang()
        {
            var khachhang = Session["khachhang"] as KhachHang;

            // Kiểm tra xem khách hàng có tồn tại trong session không  
            if (khachhang == null)
            {
                return RedirectToAction("Login"); // Nếu không có khách hàng trong session, chuyển hướng đến trang đăng nhập  
            }

            // Lấy danh sách đơn hàng của khách hàng  
            var donHangs = db.DonHangs.Where(dh => dh.MaKH == khachhang.MaKH).ToList();

            return View(donHangs);
        }
        [HttpPost]
        public ActionResult XoaDonHang(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var khachhang = Session["khachhang"] as KhachHang;
            if (khachhang == null)
            {
                return RedirectToAction("Login"); // Nếu không có khách hàng trong session, chuyển hướng đến trang đăng nhập  
            }

            // Tìm kiếm đơn hàng theo mã đơn hàng  
            var donHang = db.DonHangs.SingleOrDefault(dh => dh.MaDH == id && dh.MaKH == khachhang.MaKH);
            if (donHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy đơn hàng, trả về 404  
            }

            // Xóa tất cả chi tiết đơn hàng liên quan  
            var chiTietDonHangs = db.CTDHs.Where(ct => ct.MaDH == donHang.MaDH).ToList();
            foreach (var chiTiet in chiTietDonHangs)
            {
                // Cập nhật lại số lượng sản phẩm  
                var sanPham = db.SanPhams.SingleOrDefault(sp => sp.MaSP == chiTiet.MaSp);
                if (sanPham != null)
                {
                    sanPham.SoLuong += chiTiet.Soluong ?? 0; // Cộng lại số lượng sản phẩm  
                }
            }

            // Xóa tất cả chi tiết đơn hàng  
            db.CTDHs.RemoveRange(chiTietDonHangs); // Xóa tất cả các chi tiết đơn hàng  


            // Xóa đơn hàng  
            db.DonHangs.Remove(donHang);
            db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu  

            return RedirectToAction("DonHang"); // Chuyển hướng về danh sách đơn hàng 
        }
        public ActionResult ChiTietDonHang(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy chi tiết đơn hàng theo mã đơn hàng  
            var chiTietDonHang = db.CTDHs.Where(ct => ct.MaDH == id).ToList();
            if (chiTietDonHang == null || !chiTietDonHang.Any())
            {
                return HttpNotFound(); // Nếu không tìm thấy chi tiết đơn hàng, trả về 404  
            }

            return View(chiTietDonHang); // Trả về view với danh sách chi tiết đơn hàng  
        }
        public ActionResult EditThongtincanhan()
        {
            var khachhang = Session["khachhang"] as KhachHang;
            // Kiểm tra xem khách hàng có tồn tại trong session không  
            if (khachhang == null)
            {
                return RedirectToAction("Login"); // Nếu không có khách hàng trong session, chuyển hướng đến trang đăng nhập  
            }

            // Tìm kiếm khách hàng trong cơ sở dữ liệu bằng MaKH  
            KhachHang khachHang = db.KhachHangs.Find(khachhang.MaKH);
            if (khachHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy khách hàng, trả về 404  
            }
            return View(khachhang);
        }
        [HttpPost]
        public ActionResult EditThongtincanhan(KhachHang khachhang)
        {
            var sessionKhachHang = Session["khachhang"] as KhachHang;
            // Tìm kiếm khách hàng trong cơ sở dữ liệu bằng MaKH  
            var khachHang = db.KhachHangs.Find(khachhang.MaKH);
            if (khachHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy khách hàng, trả về 404  
            }

            // Cập nhật thông tin khách hàng  
            khachHang.TenKH = khachhang.TenKH;
            khachHang.MatKhau = khachhang.MatKhau; // Chỉ cập nhật mật khẩu nếu người dùng thay đổi  
            khachHang.Diachi = khachhang.Diachi;
            khachHang.SDT = khachhang.SDT;
            khachHang.Email = khachhang.Email;

            db.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu  

            sessionKhachHang.TenKH = khachHang.TenKH;
            sessionKhachHang.Diachi = khachHang.Diachi;
            sessionKhachHang.SDT = khachHang.SDT;
            sessionKhachHang.Email = khachHang.Email;
            // Cập nhật lại thông tin khách hàng trong session  
            Session["khachhang"] = khachHang;

            return RedirectToAction("Thongtincanhan");
        }
    }
}