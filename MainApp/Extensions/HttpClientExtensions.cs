using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace MainApp.Extensions
{
    public static class HttpClientExtensions
    {
        private static WebClient wc = new WebClient();

        public static async Task<double?> GetExchangeRate(this HttpClient client, string currency)
        {
            string allRatesData;
            try
            {
                allRatesData = await client.GetStringAsync("https://www.nationalbank.kz/rss/rates_all.xml");
            }
            catch
            {
                return null;
            }

            XDocument doc = XDocument.Parse(allRatesData);

            var rates = doc.Descendants("item").Select(x => new
            {
                Currency = x.Element("title").Value,
                Rate = x.Element("description").Value
            }).ToList();

            return rates.Where(r => r.Currency == currency).Select(r => r.Rate).FirstOrDefault().ParseNullableDouble();
        }
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dataAsString);
        }

        public static bool DownloadFile(this HttpClient client, string uri, string filePath)
        {
            try
            {
                wc.DownloadFile(uri, filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
