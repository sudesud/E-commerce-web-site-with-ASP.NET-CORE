using Bitik.Data;
using Bitik.Services; // WeatherService ve SoapCurrencyService için gerekli
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq; // JSON işleme için
using System.Threading.Tasks;

namespace Bitik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly WeatherService _weatherService; // WeatherService ekledik
        private readonly SoapCurrencyService _currencyService; // SoapCurrencyService ekledik

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, WeatherService weatherService, SoapCurrencyService currencyService)
        {
            _logger = logger;
            _context = context;
            _weatherService = weatherService; // WeatherService'i yapılandırdık
            _currencyService = currencyService; // SoapCurrencyService'i yapılandırdık
        }

        public async Task<IActionResult> Index()
        {
            // Veritabanından kategorileri çekiyoruz
            var categories = _context.Categories.ToList();
            var sliderImages = _context.Sliders.ToList();
            // Hava durumu bilgisi için API çağrısı
            var cityName = "Istanbul";
            var weatherData = await _weatherService.GetWeatherDataAsync(cityName);
            var weatherJson = JObject.Parse(weatherData);

            // Hava durumu bilgilerini ViewBag ile gönderiyoruz
            ViewBag.Temperature = weatherJson["main"]["temp"];
            ViewBag.Description = weatherJson["weather"][0]["description"];
            ViewBag.City = cityName;

            // Floatrates API'si ile döviz kuru verisini alıyoruz
            var exchangeRateXml = await _currencyService.GetExchangeRateAsync();
            var usdExchangeRate = _currencyService.GetCurrencyRateFromXml(exchangeRateXml, "TRY"); // USD/TL kuru

            // Döviz kuru bilgisini ViewBag ile gönderiyoruz
            ViewBag.UsdExchangeRate = usdExchangeRate;
            ViewBag.SliderImages = sliderImages;
            // Kategoriler ile birlikte View'a gönderiyoruz
            return View(categories);
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult CustomerService()
        {
            return View();
        }

        public IActionResult ProductDetail(int id)
        {
            var product = _context.productview.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
