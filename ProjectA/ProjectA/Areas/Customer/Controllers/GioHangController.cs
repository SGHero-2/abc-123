﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Security.Claims;

namespace ProjectA.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class GioHangController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GioHangController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Lấy thông tin tài khoản
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            // Khởi tạo đối tượng ViewModel cho giỏ hàng
            var giohang = new GioHangViewModel
            {
                DsGioHang = _db.GioHang.Include(gh => gh.SanPham)
                                .Where(gh => gh.ApplicationUserId == claim.Value)
                                .ToList(),
                HoaDon = new HoaDon()
            };

            // Tính tiền sản phẩm theo số lượng và tổng số tiền trong giỏ hàng
            foreach (var item in giohang.DsGioHang)
            {
                double ProductPrice = item.Quantity * item.SanPham.Price;
                giohang.HoaDon.Total += ProductPrice; // Cộng dồn tổng số tiền
            }

            return View(giohang);
        }
        public IActionResult Giam(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);
            giohang.Quantity -= 1;
            if (giohang.Quantity == 0)
            {
                _db.GioHang.Remove(giohang);
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Tang(int giohangId)
        {
            var giohang = _db.GioHang.FirstOrDefault(gh => gh.Id == giohangId);
            giohang.Quantity += 1;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        // DELETE: Xóa sản phẩm khỏi giỏ hàng
        [HttpGet]
        public IActionResult Remove(int sanphamId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            // Kiểm tra sản phẩm có tồn tại trong giỏ hàng
            var gioHang = _db.GioHang
                .FirstOrDefault(gh => gh.SanPhamId == sanphamId && gh.ApplicationUserId == claim.Value);

            if (gioHang != null)
            {
                _db.GioHang.Remove(gioHang); // Xóa sản phẩm
                _db.SaveChanges(); // Lưu thay đổi
            }
            else
            {
                // Ghi lại log hoặc thông báo nếu sản phẩm không tồn tại
                ModelState.AddModelError("", "Sản phẩm không tồn tại trong giỏ hàng.");
            }

            return RedirectToAction(nameof(Index)); // Quay lại trang giỏ hàng
        }
        [HttpGet]
        public IActionResult ThanhToan()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            var giohang = new GioHangViewModel
            {
                DsGioHang = _db.GioHang.Include(gh => gh.SanPham)
                                .Where(gh => gh.ApplicationUserId == claim.Value)
                                .ToList(),
                HoaDon = new HoaDon()
            };

            // Lấy thông tin tài khoản để hiển thị trên trang thanh toán
            giohang.HoaDon.ApplicationUser = _db.ApplicationUser.FirstOrDefault(user => user.Id == claim.Value);
            if (giohang.HoaDon.ApplicationUser != null)
            {
                giohang.HoaDon.Name = giohang.HoaDon.ApplicationUser.Name;
                giohang.HoaDon.Address = giohang.HoaDon.ApplicationUser.Address;
                giohang.HoaDon.PhoneNumber = giohang.HoaDon.ApplicationUser.PhoneNumber;
            }

            foreach (var item in giohang.DsGioHang)
            {
                double ProductPrice = item.Quantity * item.SanPham.Price;
                giohang.HoaDon.Total += ProductPrice; // Cộng dồn tổng số tiền
            }

            return View(giohang);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan(GioHangViewModel giohang)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            giohang.DsGioHang = await _db.GioHang.Include(gh => gh.SanPham)
                .Where(gh => gh.ApplicationUserId == claim.Value).ToListAsync();

            giohang.HoaDon.ApplicationUserId = claim.Value;
            giohang.HoaDon.OrderDate = DateTime.Now;
            giohang.HoaDon.OrderStatus = "Đang xác nhận";

            foreach (var item in giohang.DsGioHang)
            {
                double ProductPrice = item.Quantity * item.SanPham.Price;
                giohang.HoaDon.Total += ProductPrice; // Cộng dồn tổng số tiền
            }

            _db.HoaDon.Add(giohang.HoaDon);
            await _db.SaveChangesAsync(); // Lưu hóa đơn

            foreach (var item in giohang.DsGioHang)
            {
                var chiTiethoadon = new ChiTietHoaDon
                {
                    SanPhamId = item.SanPhamId,
                    HoaDonId = giohang.HoaDon.Id,
                    ProductPrice = item.SanPham.Price * item.Quantity,
                    Quantity = item.Quantity,
                };
                _db.ChiTietHoaDons.Add(chiTiethoadon);
            }
            await _db.SaveChangesAsync(); // Lưu chi tiết hóa đơn

            // Xóa thông tin trong giỏ hàng
            _db.GioHang.RemoveRange(giohang.DsGioHang);
            await _db.SaveChangesAsync(); // Lưu thay đổi

            return RedirectToAction("Index", "Home"); // Quay lại trang chính
        }
    }
}
