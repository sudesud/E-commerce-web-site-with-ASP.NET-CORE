using System.ComponentModel.DataAnnotations;

namespace Bitik.Dto
{
    public class CheckoutViewModel
    {
        public decimal GrandTotal { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        public string FullName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 karakter olabilir.")]
        public string Address { get; set; }

        [Required]
        [Phone]

        [StringLength(11, ErrorMessage = "Telefon Numaranız 11 haneli olmalıdır.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Kart numarası 16 haneli olmalıdır.")]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "MM/YY formatında olmalıdır.")]
        public string ExpiryDate { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "CVV 3 haneli olmalıdır.")]
        public string CVV { get; set; }
    }
}
