using Microsoft.AspNetCore.Mvc;

namespace ProjectA.Models
{

    public class HomeViewModel : Controller
    {
        public IEnumerable<SanPham> SanPham { get; set; }
        public List<string> SlideImages { get; set; } // Danh sách ảnh slides*
        public IActionResult Index()
        {
            return View();
        }
    }
}
