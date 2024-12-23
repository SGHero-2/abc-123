﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ProjectA.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int HoaDonId { get; set; }
        [ForeignKey("HoaDonId")]
        [ValidateNever]
        public HoaDon HoaDon { get; set; }
        [Required]
        public int SanPhamId { get; set; }
        [ForeignKey("SanPhamId")]
        [ValidateNever]
        public SanPham SanPham { get; set; }
        public int Quantity { get; set; }
        public double ProductPrice { get; set; }
    }
}
