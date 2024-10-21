using Microsoft.AspNetCore.Mvc;
using ProjectA.Data;
using ProjectA.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProjectA.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<SanPham> sanPham = _db.SanPham.Include("TheLoai").ToList();
            return View(sanPham);
        }

        public IActionResult Danhsach(string sortOrder, string searchString)
        {
            IEnumerable<SanPham> sanPham = _db.SanPham.Include("TheLoai").ToList();

            // Tìm kiếm theo tên sản phẩm
            if (!string.IsNullOrEmpty(searchString))
            {
                sanPham = sanPham.Where(s => s.Name.Contains(searchString));
            }

            // Sắp xếp theo tên hoặc giá
            switch (sortOrder)
            {
                case "name_desc":
                    sanPham = sanPham.OrderByDescending(s => s.Name);
                    break;
                case "price":
                    sanPham = sanPham.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    sanPham = sanPham.OrderByDescending(s => s.Price);
                    break;
                default:
                    sanPham = sanPham.OrderBy(s => s.Name); // Mặc định sắp xếp theo tên A-Z
                    break;
            }

            return View(sanPham);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Details(int sanphamId)
        {
            // Tạo giỏ hàng ở trang Details để xử lý chức năng thêm vào giỏ sau này
            GioHang giohang = new GioHang()
            {
                SanPhamId = sanphamId,
                SanPham = _db.SanPham.Include("TheLoai").FirstOrDefault(sp => sp.Id == sanphamId),
                Quantity = 1
            }; // Mặc định số lượng ban đầu sẽ là 1
            return View(giohang);
        }

        [HttpPost]
        [Authorize] // Yêu cầu đăng nhập
        public IActionResult Details(GioHang giohang)
        {
            // Lấy thông tin tài khoản
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
            giohang.ApplicationUserId = claim.Value;

            // Thêm sản phẩm vào giỏ hàng
            _db.GioHang.Add(giohang);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult FilterByTheLoai(int id)
        {
            IEnumerable<SanPham> sanpham = _db.SanPham.Include("TheLoai")
                .Where(sp => sp.TheLoai.Id == id)
                .ToList();
            return View("Index", sanpham);
        }

    }
}
