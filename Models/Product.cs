using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display(Name = "Urun Id")]
        public int ProductId { get; set; }

        [Display(Name = "Urun Adı")]
        public string Name { get; set; } = String.Empty;

        [Display(Name = "Urun Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "Resim")]
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
    }
}