using BaiKiemTra02.Models;
using Microsoft.EntityFrameworkCore;

namespace BaiKiemTra02.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LopHoc> LopHocs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
