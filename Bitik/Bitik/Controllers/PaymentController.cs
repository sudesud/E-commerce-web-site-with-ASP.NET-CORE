using Microsoft.AspNetCore.Mvc;
using Bitik.Data;
using Bitik.Models;
using System.Globalization;
using Bitik.Oturum;
using Bitik.Dto;

namespace Bitik.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            // Kullanıcının toplam tutarını session'dan alalım
            string userId = User.Identity.Name;
            string sessionKey = $"Cart_{userId}";
            var cartItems = HttpContext.Session.GetJson<List<CartItem>>(sessionKey);
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            decimal grandTotal = cartItems.Sum(x => x.Quantity * x.Price);

            // Kullanıcıya gösterilecek model
            var model = new CheckoutViewModel
            {
                GrandTotal = grandTotal
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kullanıcı bilgilerini alalım
            string userId = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.UserName == userId);

            if (user == null)
            {
                return Unauthorized();
            }

            // Siparişi veritabanına kaydet
            Order order = new()
            {
                UserId = user.Id,
                FullName = model.FullName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                TotalAmount = model.GrandTotal,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Sepeti temizle
            string sessionKey = $"Cart_{userId}";
            HttpContext.Session.Remove(sessionKey);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
