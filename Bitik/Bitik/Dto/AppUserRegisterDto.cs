using System.ComponentModel.DataAnnotations;

namespace Bitik.Dto
{
    public class AppUserRegisterDto
    {
        [Display(Name = "Kullanıcı Adını Girin")]
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez")]
        public string UserName { get; set; }

        [Display(Name = "Adınız")]
        [Required(ErrorMessage = "Adınızı boş geçemezsiniz")]
        public string FirstName { get; set; }

        [Display(Name = "Soyadınız")]
        [Required(ErrorMessage = "Soyadınızı boş geçemezsiniz")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "E-posta adresinizi boş geçemezsiniz")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi")]
        public string Email { get; set; }

        [Display(Name = "Şehir")]
        [Required(ErrorMessage = "Şehir boş geçilemez")]
        public string City { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon numarası boş geçilemez")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Şifreyi Tekrar Girin")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
