using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dynatone.Models;
using System.Threading;
using System.Net;
using System.ComponentModel;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Configuration;
using OfficeOpenXml;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;

namespace Dynatone.Controllers
{
    public class DynatoneController : Controller
    {
        private readonly Uri plSourceUri = new Uri("http://opt.dynatone.ru/opt/getfile.php?fn=Product&ft=CSV&pr=all&i=6174&d=1514451600");

        private readonly ILogger<DynatoneController> _logger;
        private readonly IConfiguration configuration;

        private string contentRoot;
        private string sourcePLFile;

        private static int downloadProgress = 0;
        private static bool isDownloading = false;
        private static byte[] excelFileData;

        private static WebClient wc = new WebClient();

        public void AddOrUpdateAppSetting<T>(string key, T value)
        {
            try
            {
                var filePath = Path.Combine(contentRoot, "appsettings.json");
                string json = System.IO.File.ReadAllText(filePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                key = "SupplierDynatone:" + key;
                var sectionPath = key.Split(":")[0];
                if (!string.IsNullOrEmpty(sectionPath))
                {
                    var keyPath = key.Split(":")[1];
                    jsonObj[sectionPath][keyPath] = value;
                }
                else
                {
                    jsonObj[sectionPath] = value; // if no sectionpath just set the value
                }
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filePath, output);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        public DynatoneController(ILogger<DynatoneController> logger, IWebHostEnvironment env, IConfiguration config)
        {
            _logger = logger;
            contentRoot = env.ContentRootPath;
            sourcePLFile = contentRoot + "/sourcepl.csv";
            configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Dynatone/startUpdatePl")]
        public IActionResult StartUpdatePl()
        {
            if (isDownloading)
                return Json(new { success = false, responseText = "В данный момент уже происходит обновление" });

            isDownloading = true;
            downloadProgress = 0;
            wc.DownloadProgressChanged += PlDownloadProgressChanged;
            wc.DownloadFileCompleted += PlDownloadCompleted;
            wc.DownloadFileAsync(plSourceUri, sourcePLFile);

            return Json(new { success = true });
        }

        private void PlDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            isDownloading = false;
            wc.DownloadProgressChanged -= PlDownloadProgressChanged;
            wc.DownloadFileCompleted -= PlDownloadCompleted;
            AddOrUpdateAppSetting("LastUpdate", DateTime.Now);
            downloadProgress = 500;
        }

        private void PlDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress = e.ProgressPercentage;
        }

        [HttpGet("Dynatone/getUpdatePlProgress")]
        public IActionResult GetUpdatePlProgress()
        {
            return Ok(downloadProgress);
        }

        [HttpGet("Dynatone/getPlLastUpdate")]
        public IActionResult GetPlLastUpdate()
        {
            var dynatoneOptions = configuration.GetSection(DynatoneOptions.SectionName).Get<DynatoneOptions>();
            if (dynatoneOptions == null)
                return Ok("Нет");
            return Ok(dynatoneOptions.LastUpdate.ToString("dd/MM/yyyy - HH:mm:ss"));
        }

        [HttpGet("Dynatone/settings")]
        public IActionResult GetSettings()
        {
            var dynatoneOptions = configuration.GetSection(DynatoneOptions.SectionName).Get<DynatoneOptions>();
            if (dynatoneOptions == null)
                return Ok("Нет");
            return Ok(
                new
                {
                    preorderInDays = dynatoneOptions.PreorderInDays,
                    minStockAvail = dynatoneOptions.MinStockAvail,
                    isFavorite = dynatoneOptions.IsFavorite,
                    previewWithIds = dynatoneOptions.PreviewWithIds,
                    excludeNotInStock = dynatoneOptions.ExcludeNotInStock,
                    excludeExcludedItems = dynatoneOptions.ExcludeExcludedItems,
                    excludeExcludedGroups = dynatoneOptions.ExcludeExcludedGroups,
                    exchangeRate = dynatoneOptions.ExchangeRate
                });;
        }

        [HttpPost("Dynatone/settings")]
        public IActionResult SetSettings([FromBody] SettingsData settings)
        {
            AddOrUpdateAppSetting("PreorderInDays", settings.preorderInDays);
            AddOrUpdateAppSetting("MinStockAvail", settings.minStockAvail);
            AddOrUpdateAppSetting("IsFavorite", settings.isFavorite);
            return Ok();
        }
        public class SettingsData
        {
            public int preorderInDays { get; set; }
            public int minStockAvail { get; set; }
            public bool isFavorite { get; set; }
        }

        [HttpPost("Dynatone/generatexls")]
        public IActionResult GenerateXLS([FromBody] ResultOptions resultOptions)
        {
            AddOrUpdateAppSetting("PreviewWithIds", resultOptions.PreviewWithIds);
            AddOrUpdateAppSetting("ExcludeNotInStock", resultOptions.ExcludeNotInStock);
            AddOrUpdateAppSetting("ExcludeExcludedItems", resultOptions.ExcludeExcludedItems);
            AddOrUpdateAppSetting("ExcludeExcludedGroups", resultOptions.ExcludeExcludedGroups);
            AddOrUpdateAppSetting("ExchangeRate", resultOptions.ExchangeRate);

            var dynatoneOptions = configuration.GetSection(DynatoneOptions.SectionName).Get<DynatoneOptions>();

            dynatoneOptions.PreviewWithIds = resultOptions.PreviewWithIds;
            dynatoneOptions.ExcludeNotInStock = resultOptions.ExcludeNotInStock;
            dynatoneOptions.ExcludeExcludedItems = resultOptions.ExcludeExcludedItems;
            dynatoneOptions.ExcludeExcludedGroups = resultOptions.ExcludeExcludedGroups;
            dynatoneOptions.ExchangeRate = resultOptions.ExchangeRate;

            string[] excludedItems = dynatoneOptions.ExcludeItems.Split("\n");
            string[] excludedGroups = dynatoneOptions.ExcludeGroups.Split("\n");

            double exchangeRate = dynatoneOptions.ExchangeRate;


            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                int start = 0;
                var worksheet = package.Workbook.Worksheets.Add("Dynatone");
                if (resultOptions.PreviewWithIds)
                {
                    worksheet.Cells[1, start + 1].Value = "item_ID";
                    worksheet.Column(start + 1).Width = 15;
                    worksheet.Cells[1, start + 2].Value = "group_ID";
                    worksheet.Column(start + 2).Width = 15;
                    start = 2;
                }
                worksheet.Cells[1, start + 1].Value = "sku";
                worksheet.Column(start + 1).Width = 15;
                worksheet.Cells[1, start + 2].Value = "brand";
                worksheet.Column(start + 2).Width = 25;
                worksheet.Cells[1, start + 3].Value = "ware_name";
                worksheet.Column(start + 3).Width = 40;
                worksheet.Cells[1, start + 4].Value = "price";
                worksheet.Column(start + 4).Width = 15;
                worksheet.Cells[1, start + 5].Value = "price_limit";
                worksheet.Column(start + 5).Width = 15;
                worksheet.Cells[1, start + 6].Value = "actual_price";
                worksheet.Column(start + 6).Width = 15;
                worksheet.Cells[1, start + 7].Value = "pp1";
                worksheet.Column(start + 7).Width = 8;
                worksheet.Cells[1, start + 8].Value = "pp2";
                worksheet.Column(start + 8).Width = 8;
                worksheet.Cells[1, start + 9].Value = "mskdnt";
                worksheet.Column(start + 9).Width = 8;
                worksheet.Cells[1, start + 10].Value = "nnach";
                worksheet.Column(start + 10).Width = 8;
                worksheet.Cells[1, start + 11].Value = "preorder_in_days";
                worksheet.Column(start + 11).Width = 8;
                worksheet.Cells[1, start + 12].Value = "is_favorite";
                worksheet.Column(start + 12).Width = 8;
                worksheet.Cells[1, start + 13].Value = "supplier";
                worksheet.Column(start + 13).Width = 30;
                worksheet.Row(1).Style.Font.Bold = true;


                int curRow = 2;
                using (TextFieldParser reader = new TextFieldParser(sourcePLFile))
                {
                    reader.TextFieldType = FieldType.Delimited;
                    reader.SetDelimiters(";");
                    reader.ReadFields(); //read header
                    while (!reader.EndOfData)
                    {
                        string[] col = reader.ReadFields();

                        int itemCode = 0, groupCode = 0, dealerPrice = 0, stock=0;

                        bool IsAddingItem = false;
                        if (int.TryParse(col[0], out itemCode) &&
                            int.TryParse(col[1], out groupCode) &&
                            int.TryParse(col[4], out dealerPrice) &&
                            int.TryParse(col[5], out stock) &&
                            !string.IsNullOrEmpty(col[2])
                            )
                        {
                            IsAddingItem = true;
                        }

                        if (dynatoneOptions.ExcludeNotInStock)
                        {
                            if (stock < dynatoneOptions.MinStockAvail)
                                IsAddingItem = false;
                        }

                        if (dynatoneOptions.ExcludeExcludedItems)
                        {
                            if (excludedItems.Contains(col[0]))
                                IsAddingItem = false;
                        }

                        if (dynatoneOptions.ExcludeExcludedGroups)
                        {
                            if (excludedGroups.Contains(col[1]))
                                IsAddingItem = false;
                        }

                        if (IsAddingItem)
                        { 
                            start = 0;
                            if (resultOptions.PreviewWithIds)
                            {
                                worksheet.Cells[curRow, start + 1].Value = itemCode;
                                worksheet.Cells[curRow, start + 2].Value = groupCode;
                                start = 2;
                            }

                            worksheet.Cells[curRow, start + 1].Value = col[13];
                            worksheet.Cells[curRow, start + 2].Value = col[7];
                            worksheet.Cells[curRow, start + 3].Value = col[2].Trim('"');
                            double dealerPriceVatTrans = ((((double)dealerPrice / 120) * 112) * 1.1);
                            worksheet.Cells[curRow, start + 4].Value = (int)Math.Ceiling((dealerPriceVatTrans + (dealerPriceVatTrans * 0.5)) * exchangeRate);
                            worksheet.Cells[curRow, start + 5].Value = (int)Math.Ceiling((dealerPriceVatTrans + (dealerPriceVatTrans * 0.25)) * exchangeRate);

                            worksheet.Cells[curRow, start + 7].Value = 0;
                            worksheet.Cells[curRow, start + 8].Value = 0;
                            worksheet.Cells[curRow, start + 9].Value = stock >= dynatoneOptions.MinStockAvail ? 1 : 0;
                            worksheet.Cells[curRow, start + 10].Value = 0;
                            worksheet.Cells[curRow, start + 11].Value = dynatoneOptions.PreorderInDays;
                            worksheet.Cells[curRow, start + 12].Value = dynatoneOptions.IsFavorite ? 1 : 0;
                            worksheet.Cells[curRow, start + 13].Value = "dynatone";
                            curRow++;
                        }
                    }
                }

                excelFileData = package.GetAsByteArray();
                return Ok();
            }
        }
        public class ResultOptions
        {
            public bool PreviewWithIds { get; set; }
            public bool ExcludeNotInStock { get; set; }
            public bool ExcludeExcludedItems { get; set; }
            public bool ExcludeExcludedGroups { get; set; }
            public double ExchangeRate { get; set; }
        }

        [HttpGet("Dynatone/getxls")]
        public IActionResult GetXLS()
        {
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Dynatone - {DateTime.Now:dd.MM.yyyy - HH-mm}.xlsx";
            return File(excelFileData, contentType, fileName);
        }

        [HttpGet("Dynatone/Exclusions")]
        public IActionResult ShowExclusions()
        {
            return View("Exclusions");
        }

        [HttpPost("Dynatone/Exclusions/save")]
        public IActionResult SaveExclusions([FromBody] ExclusionsData exclusions)
        {
            AddOrUpdateAppSetting("ExcludeItems", exclusions.Items);
            AddOrUpdateAppSetting("ExcludeGroups", exclusions.Groups);
            return Ok();
        }

        [HttpGet("Dynatone/Exclusions/get")]
        public IActionResult GetExclusions()
        {
            var dynatoneOptions = configuration.GetSection(DynatoneOptions.SectionName).Get<DynatoneOptions>();
            return Ok(new { excludeItems = dynatoneOptions.ExcludeItems, excludeGroups = dynatoneOptions.ExcludeGroups });
        }

        public class ExclusionsData
        {
            public string Items { get; set; }
            public string Groups { get; set; }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
