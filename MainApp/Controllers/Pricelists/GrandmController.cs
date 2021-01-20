using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCore;
using DbCore.PLModels;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using DbCore.Models;
using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Security.Policy;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;
using OfficeOpenXml;
using MainApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using NotVisualBasic.FileIO;

namespace MainApp.Controllers.Pricelists
{
    [Route("grandm")]
    public class GrandmController : Controller
    {
        private DbSet<GrandmOffer> supplierSet;

        protected delegate double GenericPriceFormula(double supplierPrice, double exchangeRate);

        protected static Guid pricelistId;
        protected string supplierSourceFileURL;
        protected string supplierName;
        protected double exchangeRate;
        protected string currency;
        protected GenericPriceFormula PriceFormula;
        protected GenericPriceFormula PriceLimitFormula;


        protected MainDbContext db;
        protected HttpClient hc;
        protected ControllersManager cManager;

        public GrandmController(MainDbContext dbContext, HttpClient client, ControllersManager manager)
        {
            db = dbContext;
            hc = client;
            cManager = manager;
            pricelistId = Guid.Parse("9a35d1cc-bff4-4b52-ab08-b06f0934d933");
            supplierName = cManager[pricelistId].SupplierName;
            supplierSourceFileURL = "https://grandm.ru/upload/grandm_ru_dealer_price.csv";
            currency = "RUB";
            PriceFormula = Formulas.StandardPriceFormula;
            PriceLimitFormula = Formulas.StandardPriceLimitFormula;
            supplierSet = db.Grandm;
            UpdateExchangeRate();
        }

        [Route("pull")]
        [HttpGet]
        public async Task<IActionResult> PullPricelist()
        {
            if (cManager[pricelistId].IsPulling)
                return Ok();

            cManager[pricelistId].IsPulling = true;
            cManager[pricelistId].PullRecordsProcessed = 0;

            List<Guid> processedRecordIds = new List<Guid>();
            try
            {
                using (CsvTextFieldParser reader = new CsvTextFieldParser(await hc.GetStreamAsync(supplierSourceFileURL), System.Text.Encoding.GetEncoding(1251)))
                {
                    reader.Delimiters = new string[] { ";" };
                    reader.ReadFields(); //read header

                    string[] col;

                    while (!reader.EndOfData)
                    {
                        try
                        {
                            col = reader.ReadFields();
                        }
                        catch
                        {
                            continue;
                        }

                        int itemCode;

                        if (!int.TryParse(col[5], out itemCode))
                            continue;

                        var existingSupplierRecord = supplierSet.Where(i => i.IdTovara == itemCode).FirstOrDefault();
                        Guid newItemId = Guid.NewGuid();

                        var supplierRecordToProcess = new GrandmOffer
                        {
                            Id = newItemId,
                            Brand = string.IsNullOrEmpty(col[4]) ? (string.IsNullOrEmpty(col[3]) ? (string.IsNullOrEmpty(col[2]) ? (string.IsNullOrEmpty(col[1]) ? col[0] : col[1]) : col[2]) : col[3]) : col[4],
                            CategoryName = string.IsNullOrEmpty(col[4]) ? (string.IsNullOrEmpty(col[3]) ? (string.IsNullOrEmpty(col[2]) ? (string.IsNullOrEmpty(col[1]) ? "-" : col[0]) : col[1]) : col[2]) : col[3],
                            CenaDiler = col[13].Substring(0, col[13].Length - 4).Replace(" ", "").ParseNullableInt(),
                            Kornevaya = col[0],
                            Podkategoriya1 = col[1],
                            Podkategoriya2 = col[2],
                            Podkategoriya3 = col[3],
                            Podkategoriya4 = col[4],
                            IdTovara = col[5].ParseNullableInt(),
                            NazvanieTovara = col[6],
                            URL = col[7],
                            KratkoeOpisanie = col[8],
                            Izobrazheniya = col[9],
                            SvoistvoRazmer = col[10],
                            SvoistvoCvet = col[11],
                            Articul = col[12],
                            CenaProdazhi = col[13],
                            Ostatok = col[14].ParseNullableInt(),
                            Ves = col[15]
                        };

                        if (existingSupplierRecord == null)
                        {
                            VectorOffer newVectorOffer = new VectorOffer
                            {
                                Id = newItemId,
                                Supplier = supplierName,
                                PricelistId = pricelistId
                            };

                            db.Add(supplierRecordToProcess);
                            db.Add(newVectorOffer);
                            processedRecordIds.Add(supplierRecordToProcess.Id);
                        }
                        else
                        {
                            bool isDescriptionChanged = supplierRecordToProcess.IsDescriptionChanged(existingSupplierRecord);
                            bool isPriceChanged = supplierRecordToProcess.IsPriceChanged(existingSupplierRecord);
                            existingSupplierRecord.CopyFromWithoutId(supplierRecordToProcess);
                            db.Update(existingSupplierRecord);

                            VectorOffer existingVectorOffer = db.VectorOffers.Where(i => i.Id == existingSupplierRecord.Id).FirstOrDefault();
                            if (isDescriptionChanged)
                            {
                                if (existingVectorOffer.Status == VectorOfferStatus.PriceChanged)
                                {
                                    existingVectorOffer.Status = VectorOfferStatus.PriceAndDescriptionChanged;
                                }
                                else
                                {
                                    existingVectorOffer.Status = VectorOfferStatus.DescriptionChanged;
                                }
                                existingVectorOffer.IsVerified = false;
                            }
                            if (isPriceChanged && existingVectorOffer.Price != null)
                            {
                                if (existingVectorOffer.Status == VectorOfferStatus.DescriptionChanged)
                                {
                                    existingVectorOffer.Status = VectorOfferStatus.PriceAndDescriptionChanged;
                                }
                                else
                                {
                                    existingVectorOffer.Status = VectorOfferStatus.PriceChanged;
                                }
                                existingVectorOffer.IsVerified = false;
                            }
                            db.Update(existingVectorOffer);
                            processedRecordIds.Add(existingSupplierRecord.Id);
                        }
                        cManager[pricelistId].PullRecordsProcessed++;
                    }

                    cManager[pricelistId].PullRecordsProcessed = -1;

                    db.VectorOffers.RemoveRange(db.VectorOffers.Where(vo => vo.PricelistId == pricelistId && !processedRecordIds.Contains(vo.Id)));
                    supplierSet.RemoveRange(supplierSet.Where(ss => !processedRecordIds.Contains(ss.Id)));

                    var pricelistRec = db.Pricelists.Where(i => i.Id == pricelistId).FirstOrDefault();
                    pricelistRec.LastPull = DateTime.Now;
                    db.SaveChanges();

                    var allRecs = db.VectorOffers.Where(vo => vo.PricelistId == pricelistId).Select(vo => new { vo.IsVerified, vo.Status }).AsEnumerable();
                    SendTelegramStatus(allRecs.Count(i => i.IsVerified == false),
                                       allRecs.Count(i => i.Status == VectorOfferStatus.DescriptionChanged),
                                       allRecs.Count(i => i.Status == VectorOfferStatus.PriceChanged),
                                       allRecs.Count(i => i.Status == VectorOfferStatus.PriceAndDescriptionChanged));

                    cManager[pricelistId].IsPulling = false;
                    return Ok();
                }
            }
            catch
            {
                cManager[pricelistId].IsPulling = false;
                cManager[pricelistId].PullRecordsProcessed = 0;
                return StatusCode(400);
            }
        }

        [Route("getbriefdata")]
        [HttpGet]
        public IActionResult GetBriefData()
        {
            var pl = db.Pricelists.Where(p => p.Id == pricelistId).FirstOrDefault();

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var res = db.VectorOffers.Where(v => v.PricelistId == pricelistId).Join(supplierSet, v => v.Id, d => d.Id, (v, d) => new
            {
                Id = v.Id,
                IsVerified = v.IsVerified,
                Status = v.Status,
                GroupCode = d.CategoryName,
                Sku = v.Sku == null ? GetAutoSKU(d.IdTovara) : v.Sku,
                isSkuCustom = v.Sku != null,
                Brand = v.Brand == null ? d.Brand : v.Brand,
                isBrandCustom = v.Brand != null,
                Name = v.Name == null ? d.NazvanieTovara : v.Name,
                isNameCustom = v.Name != null,
                Price = v.Price == null ? PriceFormula((double)d.CenaDiler, exchangeRate) : v.Price,
                isPriceCustom = v.Price != null,
                PriceLimit = v.PriceLimit == null ? PriceLimitFormula((double)d.CenaDiler, exchangeRate) : v.PriceLimit,
                isPriceLimitCustom = v.PriceLimit != null,
                isAvailable = d.Ostatok >= pl.MinStockAvail
            }).OrderByDescending(i => i.isAvailable);
            return Ok(res);
        }

        [Route("getSupplierItemData/{id}")]
        [HttpGet]
        public IActionResult GetSupplierItemData([FromRoute] string id)
        {
            var rec = supplierSet.Where(i => i.Id == Guid.Parse(id)).FirstOrDefault();
            return Ok(rec);
        }

        [Route("setGroupStatus")]
        [HttpPost]
        public IActionResult SetGroupStatus([FromBody] dynamic qparams)
        {
            var groupCode = (string)qparams["groupCode"];
            var allRecIds = db.VectorOffers.Join(supplierSet, v => v.Id, d => d.Id, (v, d) =>
            new
            {
                id = v.Id,
                v.PricelistId,
                groupCode = d.CategoryName,
            }).Where(i => i.PricelistId == pricelistId && i.groupCode.Equals(groupCode)).Select(i => i.id).ToList();

            foreach (var recId in allRecIds)
            {
                var rec = db.VectorOffers.Where(i => i.Id == recId).FirstOrDefault();
                rec.Status = (VectorOfferStatus)(int)qparams["value"];
                db.Update(rec);
            }
            db.SaveChanges();
            return Ok();
        }

        [Route("setDefaultValue/{id}")]
        [HttpPost]
        public IActionResult SetDefaultValue([FromRoute] string id, [FromBody] string field)
        {
            var rec = db.VectorOffers.Where(i => i.Id == Guid.Parse(id)).FirstOrDefault();
            var recField = char.ToUpper(field[0]) + field.Substring(1);

            rec[recField] = null;
            db.Update(rec);
            db.SaveChanges();

            switch (field)
            {
                case "sku":
                    return Ok(supplierSet.Where(i => i.Id == Guid.Parse(id)).Select(i => GetAutoSKU(i.IdTovara)).FirstOrDefault());
                case "brand":
                    return Ok(supplierSet.Where(i => i.Id == Guid.Parse(id)).Select(i => i.Brand).FirstOrDefault());
                case "name":
                    return Ok(supplierSet.Where(i => i.Id == Guid.Parse(id)).Select(i => i.NazvanieTovara).FirstOrDefault());
                case "price":
                    return Ok(supplierSet.Where(i => i.Id == Guid.Parse(id)).Select(i => PriceFormula((double)i.CenaDiler, exchangeRate)).FirstOrDefault());
                case "priceLimit":
                    return Ok(supplierSet.Where(i => i.Id == Guid.Parse(id)).Select(i => PriceLimitFormula((double)i.CenaDiler, exchangeRate)).FirstOrDefault());
            }
            return StatusCode(400);
        }

        [Route("generateXLS")]
        [HttpPost]
        public IActionResult GenerateXLS([FromBody] dynamic qParams)
        {
            using (var package = new ExcelPackage())
            {
                int start = 0;
                var worksheet = package.Workbook.Worksheets.Add(supplierName);

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
                var pl = db.Pricelists.Where(p => p.Id == pricelistId).FirstOrDefault();
                var allRecs = db.VectorOffers.Where(v => v.PricelistId == pricelistId).Join(supplierSet, v => v.Id, d => d.Id, (v, d) => new
                {
                    IsVerified = v.IsVerified,
                    Status = v.Status,
                    Sku = v.Sku == null ? GetAutoSKU(d.IdTovara) : v.Sku,
                    Brand = v.Brand == null ? d.Brand : v.Brand,
                    Name = v.Name == null ? d.NazvanieTovara : v.Name,
                    Price = v.Price == null ? PriceFormula((double)d.CenaDiler, exchangeRate) : v.Price,
                    PriceLimit = v.PriceLimit == null ? PriceLimitFormula((double)d.CenaDiler, exchangeRate) : v.PriceLimit,
                    isAvailable = d.Ostatok >= pl.MinStockAvail
                });

                foreach (var rec in allRecs)
                {
                    bool IsAddingItem = true;
                    if ((bool)qParams["includeExcluded"] == false &&
                        rec.Status != VectorOfferStatus.Active)
                    {
                        IsAddingItem = false;
                    }

                    if ((bool)qParams["includeUnavailable"] == false &&
                        rec.isAvailable == false)
                    {
                        IsAddingItem = false;
                    }

                    if ((bool)qParams["includeUnverified"] == false &&
                        rec.IsVerified == false)
                    {
                        IsAddingItem = false;
                    }

                    if (IsAddingItem)
                    {
                        start = 0;

                        worksheet.Cells[curRow, start + 1].Value = rec.Sku;
                        worksheet.Cells[curRow, start + 2].Value = rec.Brand;
                        worksheet.Cells[curRow, start + 3].Value = rec.Name;
                        worksheet.Cells[curRow, start + 4].Value = rec.Price;
                        worksheet.Cells[curRow, start + 5].Value = rec.PriceLimit;

                        worksheet.Cells[curRow, start + 7].Value = 0;
                        worksheet.Cells[curRow, start + 8].Value = 0;
                        worksheet.Cells[curRow, start + 9].Value = rec.isAvailable ? 1 : 0;
                        worksheet.Cells[curRow, start + 10].Value = 0;
                        worksheet.Cells[curRow, start + 11].Value = pl.PreorderInDays;
                        worksheet.Cells[curRow, start + 12].Value = pl.IsFavorite ? 1 : 0;
                        worksheet.Cells[curRow, start + 13].Value = supplierName;
                        curRow++;
                    }
                }
                cManager[pricelistId].ExcelFileData = package.GetAsByteArray();
            }
            return Ok();
        }

        private static string GetAutoSKU(int? idTovara)
        {
            return $"GM-{idTovara}";
        }

        protected void UpdateExchangeRate()
        {
            double? exRate = db.Pricelists.Where(pl => pl.Id == pricelistId).Select(pl => pl.ExchangeRate).FirstOrDefault();
            if (exRate == null)
            {
                exchangeRate = AutoExchangeRates.getRate(currency);
            }
            else
            {
                exchangeRate = (double)exRate;
            }
        }

        [Route("")]
        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] string id)
        {
            UpdateExchangeRate();
            ViewData["SearchSku"] = string.IsNullOrEmpty(id) ? "" : id;
            return View("Main");
        }

        [Route("getAutoExchangeRate")]
        [HttpGet]
        public IActionResult GetAutoExchangeRate()
        {
            return Ok(AutoExchangeRates.getRate(currency));
        }

        [Route("setControllerId")]
        [HttpPost]
        public IActionResult SetControllerId([FromBody] string id)
        {
            pricelistId = Guid.Parse(id);
            return Ok();
        }

        [Route("getPricelistHeader")]
        public IActionResult GetPricelistHeader()
        {
            return Ok(db.Pricelists.Where(pl => pl.Id == pricelistId).Select(pl => new
            {
                pl.SupplierName,
                pl.Name
            }).FirstOrDefault());

        }

        [Route("getLastPull")]
        [HttpGet]
        public IActionResult GetLastPull()
        {
            return Ok(db.Pricelists.Where(i => i.Id == pricelistId).Select(i => i.LastPull == null ? "Нет" : ((DateTime)i.LastPull).ToString("dd/MM/yyyy HH:mm")).FirstOrDefault());
        }

        [Route("getItemsToVerifyCount")]
        [HttpGet]
        public IActionResult GetItemsToVerifyCount()
        {
            return Ok(db.VectorOffers.Count(vo => vo.PricelistId == pricelistId && vo.IsVerified == false));
        }

        [HttpGet("settings")]
        public IActionResult GetPricelistSettings()
        {
            var res = db.Pricelists.Where(p => p.Id == pricelistId).Select(p => new
            {
                p.Name,
                p.SupplierName,
                p.PreorderInDays,
                p.MinStockAvail,
                p.IsFavorite,
                IsAutoExchangeRate = p.ExchangeRate == null,
                ExchangeRate = p.ExchangeRate == null ? AutoExchangeRates.getRate(currency) : p.ExchangeRate,
                ExchangeRateCurrency = currency
            }).FirstOrDefault();

            return Ok(res);

        }

        [HttpPost("settings")]
        public IActionResult SetPricelistSettings([FromBody] dynamic a)
        {
            var pl = db.Pricelists.Where(p => p.Id == pricelistId).FirstOrDefault();
            pl.SupplierName = (string)a["supplierName"];
            pl.Name = (string)a["name"];
            pl.PreorderInDays = (int)a["preorderInDays"];
            pl.MinStockAvail = (int)a["minStockAvail"];
            pl.IsFavorite = (bool)a["isFavorite"];
            if ((bool)a["isCustomExchangeRate"])
            {
                pl.ExchangeRate = (double)a["CustomExchangeRate"];
            }
            else
            {
                pl.ExchangeRate = null;
            }
            db.SaveChanges();
            UpdateExchangeRate();
            return Ok();
        }

        protected void SendTelegramStatus(int unverifiedCount, int descriptionChangedCount, int priceChangedCount, int priceAndDescriptionChangedCount)
        {
            if (unverifiedCount == 0 && descriptionChangedCount == 0 && priceChangedCount == 0 && priceAndDescriptionChangedCount == 0)
                return;

            var plInfo = db.Pricelists.Where(pl => pl.Id == pricelistId).Select(pl => new { pl.SupplierName, pl.Name }).FirstOrDefault();

            string message = $"ВНИМАНИЕ!!!\nВ прайс-листе <b>\"{plInfo.SupplierName} - { plInfo.Name}\"</b>\n";
            if (unverifiedCount > 0)
            {
                message += $"<b>{unverifiedCount}</b> непросмотренных позиций\n";
            }
            if (descriptionChangedCount > 0)
            {
                message += $"<b>{descriptionChangedCount}</b> позиций, у которых изменилось описание\n";
            }
            if (priceChangedCount > 0)
            {
                message += $"<b>{priceChangedCount}</b> позиций, у которых изменилось цена\n";
            }
            if (priceAndDescriptionChangedCount > 0)
            {
                message += $"<b>{priceAndDescriptionChangedCount}</b> позиций, у которых изменились и цена, и описание\n";
            }
            TelegramOperatorBot.Broadcast(message);
        }

        [Route("pull/ispulling")]
        [HttpGet]
        public IActionResult IsPulling()
        {
            return Ok(cManager[pricelistId].IsPulling);
        }

        [Route("pull/pullrecsprocessed")]
        [HttpGet]
        public IActionResult GetPullRecordsProcessed()
        {
            return Ok(cManager[pricelistId].PullRecordsProcessed);
        }

        [Route("setIsVerified/{id}")]
        [HttpPost]
        public IActionResult SetIsVerified([FromRoute] string id, [FromBody] bool value)
        {
            var rec = db.VectorOffers.Where(i => i.Id == Guid.Parse(id)).FirstOrDefault();
            if (rec.IsVerified != value)
            {
                rec.IsVerified = value;
                db.Update(rec);
                db.SaveChanges();
            }
            return Ok();
        }

        [Route("setAllIsVerified")]
        [HttpPost]
        public IActionResult SetAllIsVerified([FromBody] bool value)
        {
            var allRecs = db.VectorOffers.Where(i => i.PricelistId == pricelistId);
            foreach (var rec in allRecs)
            {
                rec.IsVerified = value;
                db.Update(rec);
            }
            var plRec = db.Pricelists.Where(i => i.Id == pricelistId).FirstOrDefault();
            db.Update(plRec);
            db.SaveChanges();
            return Ok();
        }

        [Route("setStatus/{id}")]
        [HttpPost]
        public IActionResult SetStatus([FromRoute] string id, [FromBody] int value)
        {
            var rec = db.VectorOffers.Where(i => i.Id == Guid.Parse(id)).FirstOrDefault();
            rec.Status = (VectorOfferStatus)value;
            db.Update(rec);
            db.SaveChanges();
            return Ok();
        }

        [Route("setAllStatus")]
        [HttpPost]
        public IActionResult SetAllStatus([FromBody] int value)
        {
            var allRecs = db.VectorOffers.Where(i => i.PricelistId == pricelistId);
            foreach (var rec in allRecs)
            {
                rec.Status = (VectorOfferStatus)value;
                db.Update(rec);
            }
            db.SaveChanges();
            return Ok();
        }

        [Route("setCustomValue/{id}")]
        [HttpPost]
        public IActionResult SetCustomValue([FromRoute] string id, [FromBody] dynamic qParams)
        {
            string field = (string)qParams["field"];
            var rec = db.VectorOffers.Where(i => i.Id == Guid.Parse(id)).FirstOrDefault();
            var recField = char.ToUpper(field[0]) + field.Substring(1);

            string strVal = (string)qParams["value"];
            if (recField == "Price" || recField == "PriceLimit")
            {
                rec[recField] = strVal.ParseNullableDouble();
            }
            else
            {
                rec[recField] = strVal;
            }
            db.Update(rec);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet("getXLS")]
        public IActionResult GetXLS()
        {
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"{supplierName} - {DateTime.Now:dd.MM.yyyy - HH-mm}.xlsx";
            return File(cManager[pricelistId].ExcelFileData, contentType, fileName);
        }
    }
}
