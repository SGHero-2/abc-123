using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using ProjectA.Data;
using ProjectA.Models; // Thay bằng namespace chứa ProjectAContext
using System.Linq;

namespace ProjectA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChiTietHoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietHoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietHoaDon/Edit/5
        public IActionResult Edit(int id)
        {
            var chiTiet = _context.ChiTietHoaDons.Find(id);
            if (chiTiet == null)
            {
                return NotFound();
            }
            return View(chiTiet);
        }

        // POST: ChiTietHoaDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ChiTietHoaDon chiTietHoaDon)
        {
            if (id != chiTietHoaDon.HoaDonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(chiTietHoaDon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDon/Delete/5
        public IActionResult Delete(int id)
        {
            var chiTiet = _context.ChiTietHoaDons.Find(id);
            if (chiTiet == null)
            {
                return NotFound();
            }
            return View(chiTiet);
        }

        // POST: ChiTietHoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var chiTiet = _context.ChiTietHoaDons.Find(id);
            _context.ChiTietHoaDons.Remove(chiTiet);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
