using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bitik.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; } // ApplicationUser, AspNetUsers tablosuyla eşleşir.

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Toplam tutar sıfırdan büyük olmalıdır.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 karakter olabilir.")]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
