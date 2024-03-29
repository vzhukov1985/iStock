﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using MainApp.Extensions;

namespace MainApp.Services
{
    public static class ExchangeRatesService
    {
        private static Dictionary<string, double?> rates;
        private static DateTime? LastUpdate;

        static ExchangeRatesService()
        {
            LastUpdate = null;
            rates = new Dictionary<string, double?>();
        }

        public static double getRate(string currency)
        {
            if (rates.ContainsKey(currency) && LastUpdate!=null && rates[currency] != null)
            {
                return (double)rates[currency];
            }
            else
            {
                return 0;
            }
        }

        public static async void UpdateRatesAsync(HttpClient hc)
        {
            string allRatesData;

            try
            {
                allRatesData = await hc.GetStringAsync("https://www.nationalbank.kz/rss/rates_all.xml");
            }
            catch
            {
                return;
            }

            XDocument doc = XDocument.Parse(allRatesData);

            rates = doc.Descendants("item").ToDictionary(i => i.Element("title").Value, i => i.Element("description").Value.ParseNullableDouble());
            LastUpdate = DateTime.Now;
        }
    }
}