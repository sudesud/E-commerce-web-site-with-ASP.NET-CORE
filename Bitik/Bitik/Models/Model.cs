namespace Bitik.Models
{
    public class Model
    {
        public int CartId { get; set; }  // Sepet ID'si
        public string UserId { get; set; } // Kullanıcı ID'si
        public AppUser User { get; set; } // Kullanıcı ile ilişkilendirme
        public List<CartItem> CartItems { get; set; } // Sepet öğeleri (CartItem)
    }
}
