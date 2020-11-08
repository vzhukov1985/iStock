using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Services
{
    public static class StringExtensionMethods
    {
        public static int? ParseNullableInt(this string str)
        {
            if (int.TryParse(str, out int tmpValue))
                return tmpValue;
            return null;
        }

        public static long? ParseNullableLong(this string str)
        {
            if (long.TryParse(str, out long tmpValue))
                return tmpValue;
            return null;
        }

        public static float? ParseNullableFloat(this string str)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            // if the first regex matches, the number string is in us culture
            if (Regex.IsMatch(str, @"^(:?[\d,]+\.)*\d+$"))
            {
                cultureInfo = new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in de culture
            else if (Regex.IsMatch(str, @"^(:?[\d.]+,)*\d+$"))
            {
                cultureInfo = new CultureInfo("de-DE");
            }
            NumberStyles styles = NumberStyles.Number;

            if (float.TryParse(str, styles, cultureInfo, out float tmpValue))
                return tmpValue;
            return null;
        }
        public static double? ParseNullableDouble(this string str)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            // if the first regex matches, the number string is in us culture
            if (Regex.IsMatch(str, @"^(:?[\d,]+\.)*\d+$"))
            {
                cultureInfo = new CultureInfo("en-US");
            }
            // if the second regex matches, the number string is in de culture
            else if (Regex.IsMatch(str, @"^(:?[\d.]+,)*\d+$"))
            {
                cultureInfo = new CultureInfo("de-DE");
            }
            NumberStyles styles = NumberStyles.Number;

            if (double.TryParse(str, styles, cultureInfo, out double tmpValue))
                return tmpValue;
            return null;
        }
    }

    public static class HttpClientExtensionMethods
    {
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
    }
}
