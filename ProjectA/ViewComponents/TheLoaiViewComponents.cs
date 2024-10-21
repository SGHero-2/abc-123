using Microsoft.AspNetCore.Mvc;
using ProjectA.Data;
using ProjectA.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ProjectA.ViewComponents
{
    public class TheLoaiViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public TheLoaiViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            var theloai = await _db.TheLoai.ToListAsync();


            return View(theloai);
        }
    }
}