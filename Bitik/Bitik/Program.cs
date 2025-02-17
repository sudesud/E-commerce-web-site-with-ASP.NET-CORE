using Bitik.Data;
using Bitik.Models;
using Bitik.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS yapýlandýrmasý ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNodeServer",
        builder => builder.WithOrigins("http://127.0.0.1:5000") // Node.js API'nin çalýþtýðý port
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Diðer servisleri ekleyin
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Bellek cache ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// Ekstra servisler...
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddHttpClient<SoapCurrencyService>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = false;
   
});


var app = builder.Build();

// CORS middleware'ini kullanýn
app.UseCors("AllowNodeServer");  // CORS'u kullanmak için bu satýrý ekleyin

// Diðer middleware yapýlandýrmalarý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
