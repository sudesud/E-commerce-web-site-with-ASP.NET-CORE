using Bitik.Data;
using Bitik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
        _httpClient = new HttpClient();
    }
    // Her action'dan önce çalışır
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var adminId = HttpContext.Session.GetString("AdminId");
        if (string.IsNullOrEmpty(adminId))
        {
            context.Result = RedirectToAction("Index", "AdminLogin");
        }

        base.OnActionExecuting(context);
    }
    // GET: Admin
    public async Task<IActionResult> Index()
    {
        // Product ve Category tablosunu ilişkilendiriyoruz
        var products = await _context.Products
                                      .Include(p => p.Category)  // Category ile ilişkilendiriyoruz
                                      .ToListAsync(); // Veritabanından tüm ürünleri alıyoruz

        return View(products); // Ürünlerle birlikte kategori bilgilerini view'a gönderiyoruz
    }


    // Arama işlemi Node.js API'ye yönlendirilir
    [HttpPost]
    public async Task<IActionResult> Search(string query)
    {
        var products = await GetProductsFromApi(query); // Arama sorgusuna göre ürünleri alıyoruz
        return View("Index", products); // Arama sonuçlarını Index view'a döndür
    }

    // Node.js API'den ürünleri alıyoruz
    private async Task<List<Product>> GetProductsFromApi(string query = "")
    {
        string apiUrl = string.IsNullOrEmpty(query)
            ? "http://127.0.0.1:5000/search"
            : $"http://127.0.0.1:5000/search?q={query}";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(result);
                return products;
            }
            else
            {
                ViewBag.ErrorMessage = "Ürünler getirilirken bir hata oluştu.";
                return new List<Product>();
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Bir hata oluştu: {ex.Message}";
            return new List<Product>();
        }
    }

    // GET: Admin/Create
    public IActionResult Create()
    {
        // Kategori listesini dropdown olarak göndermek için
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
        return View();
    }

    // POST: Admin/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProductName,Description,ImageUrl,Price,CategoryId")] Product product, IFormFile ImageUpload)
    {
        if (ImageUpload != null)
        {
            var uzanti = Path.GetExtension(ImageUpload.FileName);
            string yeniIsim = Guid.NewGuid().ToString() + uzanti;
            string yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Urunler", yeniIsim);

            using (var stream = new FileStream(yol, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }

            product.ImageUrl = yeniIsim;
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Başarıyla kaydedildikten sonra yönlendir
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
            }
        }

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
        return View(product);
    }

    // GET: Admin/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
        return View(product);
    }

    // POST: Admin/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,Stock,CategoryId")] Product product, IFormFile ImageUpload)
    {
        if (id != product.ProductId)
        {
            return NotFound();
        }

        if (ImageUpload != null)
        {
            var uzanti = Path.GetExtension(ImageUpload.FileName);
            string yeniIsim = Guid.NewGuid().ToString() + uzanti;
            string yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Urunler", yeniIsim);

            using (var stream = new FileStream(yol, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }
            product.ImageUrl = yeniIsim;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(e => e.ProductId == product.ProductId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Admin/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Admin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
