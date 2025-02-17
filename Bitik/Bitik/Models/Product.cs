using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bitik.Models
{
    public class Product
    {
        public int ProductId { get; set; }


        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string? ProductName { get; set; }


        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }


        [Range(0.01, 1000000, ErrorMessage = "Fiyat 0.01 ile 1.000.000 arasında olmalıdır.")]
        public decimal? Price { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
        public int? Stock { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile? ImageUpload { get; set; }

    }
}