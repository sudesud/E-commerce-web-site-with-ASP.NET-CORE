using System.Net.Http;
using System.Threading.Tasks;

namespace Bitik.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherDataAsync(string cityName)
        {
            string apiKey = "d54a3133ce0b5a464ce69bda502ae91e"; // API anahtarınız
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric"; // Sıcaklık için metric birim

            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}
