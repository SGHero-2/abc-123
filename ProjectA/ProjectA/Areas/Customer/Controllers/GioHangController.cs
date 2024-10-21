using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectA.Data;
using ProjectA.Models;
using System.Security.Claims;

namespace ProjectA.Areas.Customer.Controllers
{
    [Area("Customer")]
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

            // Lấy danh sách sản phẩm trong giỏ hàng của User
            IEnumerable<GioHang> dsGioHang = _db.GioHang
                .Include(gh => gh.SanPham)
                .Where(gh => gh.ApplicationUserId == claim.Value)
                .ToList();

            // Gộp các sản phẩm giống nhau dựa trên SanPhamId và tính tổng số lượng
            var groupedGioHang = dsGioHang
                .GroupBy(gh => gh.SanPhamId)
                .Select(g => new GioHang
                {
                    SanPhamId = g.Key, // Lấy SanPhamId từ group key
                    SanPham = g.First().SanPham, // Lấy sản phẩm từ mục đầu tiên
                    Quantity = g.Sum(item => item.Quantity), // Tính tổng số lượng của sản phẩm
                    ApplicationUserId = claim.Value // Giữ thông tin người dùng
                }).ToList();

            // Trả về danh sách sản phẩm đã gộp
            return View(groupedGioHang);
        }
    }
}
