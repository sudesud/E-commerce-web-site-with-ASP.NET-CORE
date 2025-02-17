using Bitik.Data;
using Bitik.Models;
using Bitik.Oturum;
using Microsoft.AspNetCore.Mvc;

public class FavoritesController : Controller
{
    private readonly ApplicationDbContext _context;

    public FavoritesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Kullanıcıya özel favoriler sayfasını göster
    public IActionResult Index()
    {
        var userId = HttpContext.User.Identity.Name; // Kullanıcının kimliğini alıyoruz.
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Kullanıcının favori listesine Session'dan ulaşalım
        List<FavoriteItem> favorites = HttpContext.Session.GetJson<List<FavoriteItem>>(userId + "_Favorites") ?? new List<FavoriteItem>();

        return View(favorites);
    }

    // Favorilere ürün ekleme işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(int id)
    {
        var userId = HttpContext.User.Identity.Name;
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Ürünü veritabanından al
        var product = _context.Products.Find(id);
        if (product == null)
        {
            TempData["Error"] = "Ürün bulunamadı!";
            return RedirectToAction("Index", "Home");
        }

        // Kullanıcıya özel favori listesine ekle
        List<FavoriteItem> favorites = HttpContext.Session.GetJson<List<FavoriteItem>>(userId + "_Favorites") ?? new List<FavoriteItem>();

        // Eğer ürün favorilerde yoksa ekle
        if (!favorites.Any(f => f.ProductId == id))
        {
            favorites.Add(new FavoriteItem(product));
        }

        // Favori listesini session'a kaydet
        HttpContext.Session.SetJson(userId + "_Favorites", favorites);

        TempData["Message"] = "Ürün favorilere eklendi!";
        return RedirectToAction("Index", "Home");
    }

    // Favorilerden ürünü çıkarma
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int id)
    {
        var userId = HttpContext.User.Identity.Name;
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Kullanıcının favori listesini al
        List<FavoriteItem> favorites = HttpContext.Session.GetJson<List<FavoriteItem>>(userId + "_Favorites") ?? new List<FavoriteItem>();

        // Favorilerden ürünü kaldır
        favorites.RemoveAll(f => f.ProductId == id);

        // Güncellenmiş favori listelerini session'a kaydet
        HttpContext.Session.SetJson(userId + "_Favorites", favorites);

        TempData["Message"] = "Ürün favorilerden kaldırıldı!";
        return RedirectToAction("Index");
    }
}
