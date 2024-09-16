using System.ComponentModel.DataAnnotations;

namespace BaiTap07.Models
{
    public class TheLoai
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
    }
}
