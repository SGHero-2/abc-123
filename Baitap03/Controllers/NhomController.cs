using Microsoft.AspNetCore.Mvc;

namespace Baitap03.Controllers
{
    public class NhomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
