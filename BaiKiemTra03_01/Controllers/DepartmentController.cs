using Microsoft.AspNetCore.Mvc;
using BaiKiemTra03_01.Data;
using BaiKiemTra03_01.Models;

public class DepartmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepartmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách phòng ban
    public IActionResult Index()
    {
        var departments = _context.Departments.ToList();
        return View(departments);
    }

    // Thêm phòng ban
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Department department)
    {
        if (ModelState.IsValid)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    // Sửa phòng ban
    public IActionResult Edit(int id)
    {
        var department = _context.Departments.Find(id);
        if (department == null)
        {
            return NotFound();
        }
        return View(department);
    }

    [HttpPost]
    public IActionResult Edit(Department department)
    {
        if (ModelState.IsValid)
        {
            _context.Departments.Update(department);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    // Xóa phòng ban
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var department = _context.Departments.Find(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
