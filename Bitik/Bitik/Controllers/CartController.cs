using System.Globalization;
using System.Security.Claims;
using Bitik.Data;
using Bitik.Dto;
using Bitik.Models;
using Bitik.Oturum;
using Microsoft.AspNetCore.Mvc;

namespace Bitik.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        private List<CartItem> GetCartItems()
        {
            string userId = User.Identity.Name;
            var sessionKey = $"Cart_{userId}";
            return HttpContext.Session.GetJson<List<CartItem>>(sessionKey) ?? new List<CartItem>();
        }

        private void SaveCartItems(List<CartItem> cartItems)
        {
            string userId = User.Identity.Name;
            var sessionKey = $"Cart_{userId}";
            HttpContext.Session.SetJson(sessionKey, cartItems);
        }

        public IActionResult Index()
        {
            var items = GetCartItems();
            if (items == null || !items.Any())
            {
                ViewBag.CartIsEmpty = true;
            }

            CartViewModel cartvm = new()
            {
                CartItems = items,
                GrandTotal = items.Sum(x => x.Quantity * x.Price)
            };

            return View(cartvm);
        }

        public async Task<IActionResult> Add(int id)
        {
            Product product = _context.Products.Find(id);
            List<CartItem> items = GetCartItems();
            CartItem cartItem = items.FirstOrDefault(x => x.ProducutId == id);
            if (cartItem == null)
            {
                items.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }
            SaveCartItems(items);
            TempData["Mesaj"] = "Ürün Sepete Eklenmiştir";

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int id)
        {
            List<CartItem> cart = GetCartItems();
            CartItem cartItem = cart.FirstOrDefault(c => c.ProducutId == id);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
                else
                {
                    cart.RemoveAll(c => c.ProducutId == id);
                }
                SaveCartItems(cart);
            }

            TempData["Mesaj"] = "Ürün Sepetten Silindi";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            List<CartItem> cart = GetCartItems();
            cart.RemoveAll(c => c.ProducutId == id);
            SaveCartItems(cart);

            TempData["Mesaj"] = "Ürün Sepeti Silindi";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            string userId = User.Identity.Name;
            HttpContext.Session.Remove($"Cart_{userId}");
            return RedirectToAction("Index");
        }
    }
}