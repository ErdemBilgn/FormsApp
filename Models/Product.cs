using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name = "Urun Id")]
        public int ProductId { get; set; }

        [Display(Name = "Urun Adı")]
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        public string Name { get; set; } = null!;

        [Display(Name = "Urun Fiyat")]
        [Range(0, 100000, ErrorMessage = "Fiyat alanı 0 ile 10.000 arasında olmalı")]
        [Required(ErrorMessage = "Fiyat alanı boş bırakılamaz")]
        public decimal? Price { get; set; }

        [Display(Name = "Resim")]
        // [Required(ErrorMessage = "Resim alanı boş bırakılamaz")]
        public string? Image { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Kategori alanı boş bırakılamaz")]
        public int? CategoryId { get; set; }

    }
}