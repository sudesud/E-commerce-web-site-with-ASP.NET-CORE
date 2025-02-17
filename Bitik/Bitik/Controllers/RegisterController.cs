using System;
using System.Threading.Tasks;
using Bitik.Dto;
using Bitik.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit; // MimeMessage ve BodyBuilder için
using MailKit.Net.Smtp; // MailKit'in SmtpClient'ı için

namespace Bitik.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        {
            Console.WriteLine("RegisterController Index metodu tetiklendi!");

            // Random kod oluşturma
            Random random = new Random();
            int code = random.Next(10000, 1000000);

            // Yeni kullanıcı oluşturma
            AppUser appuser = new AppUser()
            {
                FirstName = appUserRegisterDto.FirstName,
                LastName = appUserRegisterDto.LastName,
                City = appUserRegisterDto.City,
                UserName = appUserRegisterDto.UserName,
                Email = appUserRegisterDto.Email,
                ConfirmCode = code,
            };

            // Model validasyon kontrolü
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model geçerli değil!");
                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Validasyon hatası: {error.ErrorMessage}");
                    }
                }
                return View(appUserRegisterDto);
            }

            // Kullanıcı oluşturma
            var result = await _userManager.CreateAsync(appuser, appUserRegisterDto.Password);
            if (result.Succeeded)
            {
                Console.WriteLine("Kullanıcı başarıyla oluşturuldu.");

                // Mail gönderimi
                try
                {
                    MimeMessage mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress("Eticaret Uygulaması", "sudetacer@gmail.com"));
                    mimeMessage.To.Add(new MailboxAddress("User", appuser.Email));
                    mimeMessage.Subject = "ETicaret Uygulaması - Doğrulama Kodu";

                    BodyBuilder bodyBuilder = new BodyBuilder
                    {
                        TextBody = "Kaydınız başarıyla gerçekleşti. Doğrulama Kodunuz: " + code
                    };
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("obas1869@gmail.com", "rlbb bpko jyun djad");
                        client.Send(mimeMessage);
                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
                }

                TempData["Mail"] = appUserRegisterDto.Email;
                return RedirectToAction("Index", "ConfirmMail");
            }
            else
            {
                Console.WriteLine("Kullanıcı oluşturulamadı!");
                foreach (var item in result.Errors)
                {
                    Console.WriteLine($"Identity hatası: {item.Description}");
                    ModelState.AddModelError("", item.Description);
                }
            }

            return View();
        }
    }
}
