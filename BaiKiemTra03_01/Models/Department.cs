namespace BaiKiemTra03_01.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int NumberOfEmployees { get; set; }
        public int ManagerDepartmentId { get; set; }
    }
}
