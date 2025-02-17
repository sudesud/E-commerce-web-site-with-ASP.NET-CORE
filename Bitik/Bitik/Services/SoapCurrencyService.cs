using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bitik.Services
{
    public class SoapCurrencyService
    {
        private readonly HttpClient _httpClient;

        public SoapCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Döviz kuru verilerini içeren XML'i alır.
        /// </summary>
        /// <returns>XML verisi (string formatında)</returns>
        public async Task<string> GetExchangeRateAsync()
        {
            var requestUrl = "https://www.floatrates.com/daily/usd.xml"; // Floatrates API'si
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(); // XML verisini döndürür
            }

            throw new HttpRequestException("Floatrates API'sine erişim başarısız oldu.");
        }

        /// <summary>
        /// XML verisinden belirtilen döviz kuru bilgisini çeker.
        /// </summary>
        /// <param name="xmlData">XML verisi</param>
        /// <param name="isoCode">ISO Kodu (örneğin: USD)</param>
        /// <returns>Döviz kuru bilgisi</returns>
        public string GetCurrencyRateFromXml(string xmlData, string isoCode)
        {
            // XML verisini parse ediyoruz
            var xDocument = XDocument.Parse(xmlData);

            // İlgili ISO koduna sahip döviz bilgisini buluyoruz
            var currencyElement = xDocument.Descendants("item")
                                            .FirstOrDefault(c => c.Element("targetCurrency")?.Value == isoCode);

            // Döviz kuru bilgisini döndürüyoruz (ExchangeRate)
            return currencyElement?.Element("exchangeRate")?.Value ?? "Döviz kuru bulunamadı";
        }
    }
}
