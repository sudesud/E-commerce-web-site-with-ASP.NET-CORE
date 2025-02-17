using Microsoft.AspNetCore.Mvc;
using Bitik.Models;
using Bitik.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class AdminLoginController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminLoginController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Admin doğrulama
        var admin = await _context.Admins
                                  .FirstOrDefaultAsync(a => a.Email == model.Email && a.Password == model.Password);

        if (admin != null)
        {
            // Giriş başarılı, oturum aç
            HttpContext.Session.SetString("AdminId", admin.AdminId.ToString());
            HttpContext.Session.SetString("AdminName", admin.FullName);

            return RedirectToAction("Index", "Admin");
        }

        ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "AdminLogin");
    }
}
