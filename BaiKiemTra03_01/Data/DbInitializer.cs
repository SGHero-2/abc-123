using BaiKiemTra03_01.Models;

namespace BaiKiemTra03_01.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Đảm bảo rằng DB đã được tạo
            context.Database.EnsureCreated();

            // Kiểm tra xem có dữ liệu Employee chưa, nếu chưa thì thêm
            if (context.Employees.Any())
            {
                return; // DB đã có dữ liệu
            }

            var departments = new Department[]
            {
            new Department { DepartmentName = "HR", NumberOfEmployees = 10, ManagerDepartmentId = 1 },
            new Department { DepartmentName = "IT", NumberOfEmployees = 25, ManagerDepartmentId = 2 }
            };

            foreach (var d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();

            var employees = new Employee[]
            {
            new Employee { EmployeeName = "Phạm Thanh Tùng", Position = "Manager", DepartmentId = 1, HireDate = DateTime.Now },
            new Employee { EmployeeName = "Nguyễn Hoài Nam", Position = "Developer", DepartmentId = 2, HireDate = DateTime.Now }
            };

            foreach (var e in employees)
            {
                context.Employees.Add(e);
            }
            context.SaveChanges();
        }
    }
}
