using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bitik.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
