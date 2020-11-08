using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbCore;
using DbCore.PLModels;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using DbCore.Models;
using Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Security.Policy;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;

namespace MainApp.Controllers.Pricelists
{
    [Route("dynatone")]
    public class DynatoneController : Controller
    {
        private delegate double GenericPriceFormula(double supplierPrice, double exchangeRate);

        private static Guid controllerId;
        private string supplierSourceFileURL;
        private string supplierName;
        private static bool isPulling;
        private static int pullRecordsProcessed;
        private static double exchangeRate;
        private string currency;
        private GenericPriceFormula PriceFormula;
        private GenericPriceFormula PriceLimitFormula;

        private MainDbContext db;
        private HttpClient hc;

        public DynatoneController(MainDbContext dbContext, HttpClient client)
        {
            db = dbContext;
            hc = client;
            controllerId = Guid.Parse("82f9dbe9-1519-430d-9f37-2fe2a6786900");
            supplierName = "dynatone";
            supplierSourceFileURL = "D://dynatone-small.csv";
            currency = "RUB";
            PriceFormula = Formulas.StandardPriceFormula;
            PriceLimitFormula = Formulas.StandardPriceLimitFormula;
        }

        [Route("")]
        public IActionResult Index()
        {
            double? exRate = db.Pricelists.Where(pl => pl.Id == controllerId).Select(pl =>pl.ExchangeRate).FirstOrDefault();
            if (exRate == null)
            {
                exRate = hc.GetExchangeRate(currency).Result;
            }

            if (exRate == null)
            {
                exchangeRate = 0;
            }
            else
            {
                exchangeRate = (double)exRate;
            }

            return View("Main");
        }

        [Route("setControllerId")]
        [HttpPost]
        public IActionResult SetControllerId([FromBody] string id)
        {
            controllerId = Guid.Parse(id);
            return Ok();
        }


        [Route("getPricelistHeader")]
        public IActionResult GetPricelistHeader()
        {
            var plRec = db.Pricelists.Where(i => i.Id == controllerId).Select(i => new { i.Id, i.SupplierName, i.Name }).FirstOrDefault();
            return Ok(new
            {
                plRec.SupplierName,
                plRec.Name
            });
        }

        [Route("getLastPull")]
        [HttpGet]
        public IActionResult GetLastPull()
        {
            return Ok(db.Pricelists.Where(i => i.Id == controllerId).Select(i => i.LastPull == null ? "Нет" : ((DateTime)i.LastPull).ToString("dd/MM/yyyy HH:mm")).FirstOrDefault());
        }

        [Route("getItemsToVerifyCount")]
        [HttpGet]
        public IActionResult GetItemsToVerifyCount()
        {
            return Ok(db.VectorOffers.Count(vo => vo.Supplier == supplierName && vo.IsVerified == false));
        }

        [HttpGet("settings")]
        public IActionResult GetPricelistSettings()
        {
            var res = db.Pricelists.Where(p => p.Id == controllerId).Select(p => new
            {
                p.Name,
                p.SupplierName,
                p.PreorderInDays,
                p.MinStockAvail,
                p.IsFavorite,
                IsAutoExchangeRate = p.ExchangeRate == null,
                ExchangeRateCurrency = currency
            }).FirstOrDefault();

            return Ok(new
            {
                res.Name,
                res.SupplierName,
                res.PreorderInDays,
                res.MinStockAvail,
                res.IsFavorite,
                res.IsAutoExchangeRate,
                ExchangeRate = exchangeRate,
                res.ExchangeRateCurrency
            });
        }

        [HttpPost("settings")]
        public IActionResult SetPricelistSettings([FromBody] dynamic a)
        {
            var pl = db.Pricelists.Where(p => p.Id == controllerId).FirstOrDefault();
            pl.SupplierName = (string)a["supplierName"];
            pl.Name = (string)a["name"];
            pl.PreorderInDays = (int)a["preorderInDays"];
            pl.MinStockAvail = (int)a["minStockAvail"];
            pl.IsFavorite = (bool)a["isFavorite"];
            db.SaveChanges();
            return Ok();
        }

        [Route("pull")]
        [HttpGet]
        public IActionResult PullPricelist()
        {
            isPulling = true;
            pullRecordsProcessed = 0;
            using (TextFieldParser reader = new TextFieldParser(supplierSourceFileURL))
            {
                reader.TextFieldType = FieldType.Delimited;
                reader.SetDelimiters(";");
                reader.ReadFields(); //read header
                while (!reader.EndOfData)
                {
                    string[] col = reader.ReadFields();

                    int itemCode;

                    if (!int.TryParse(col[0], out itemCode))
                        continue;

                    var existingSupplierRecord = db.Dynatone.Where(i => i.Kod == itemCode).FirstOrDefault();
                    Guid newItemId = Guid.NewGuid();

                    var supplierRecordToProcess = new DynatoneOffer
                    {
                        Id = newItemId,
                        Kod = col[0].ParseNullableInt(),
                        KodGruppy = col[1].ParseNullableInt(),
                        Naimenovanie = col[2],
                        RRC = col[3].ParseNullableInt(),
                        CenaDiler = col[4].ParseNullableInt(),
                        KolichestvoDlyaOpta = col[5].ParseNullableInt(),
                        VidNomenklatury = col[6],
                        Brand = col[7],
                        ModelSModifikaciyey = col[8],
                        Articul = col[9],
                        Barcode = col[10].ParseNullableLong(),
                        Model = col[11],
                        Modifikaciya = col[12],
                        KodSPrefixom = col[13],
                        StranaProishojdenia = col[14],
                        Ves = col[15].ParseNullableFloat(),
                        Dlina = col[16].ParseNullableFloat(),
                        Shirina = col[17].ParseNullableFloat(),
                        Vysota = col[18].ParseNullableFloat()
                    };

                    if (existingSupplierRecord == null)
                    {
                        VectorOffer newVectorOffer = new VectorOffer
                        {
                            Id = newItemId,
                            Supplier = supplierName
                        };

                        db.Add(supplierRecordToProcess);
                        db.Add(newVectorOffer);
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
                    }
                    pullRecordsProcessed++;
                }

                isPulling = false;
                pullRecordsProcessed = -1;
                var pricelistRec = db.Pricelists.Where(i => i.Id == controllerId).FirstOrDefault();
                pricelistRec.LastPull = DateTime.Now;
                db.SaveChanges();

                return Ok();
            }
        }

        [Route("pull/ispulling")]
        [HttpGet]
        public IActionResult IsPulling()
        {
            return Ok(isPulling);
        }

        [Route("pull/pullrecsprocessed")]
        [HttpGet]
        public IActionResult GetPullRecordsProcessed()
        {
            return Ok(pullRecordsProcessed);
        }

        [Route("getbriefdata")]
        [HttpGet]
        public IActionResult GetBriefData()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var res = db.VectorOffers.Where(v => v.Supplier == supplierName).Join(db.Dynatone, v => v.Id, d => d.Id, (v, d) => new
            {
                Id = v.Id,
                IsVerified = v.IsVerified,
                Status = v.Status,
                GroupCode = d.KodGruppy,
                Sku = v.Sku == null ? d.KodSPrefixom : v.Sku,
                isSkuCustom = v.Sku != null,
                Brand = v.Brand == null ? d.Brand : v.Brand,
                isBrandCustom = v.Brand != null,
                Name = v.Name == null ? d.Naimenovanie : v.Name,
                isNameCustom = v.Name != null,
                Price = v.Price == null ? PriceFormula((double)d.CenaDiler, exchangeRate) : v.Price,
                isPriceCustom = v.Price != null,
                PriceLimit = v.PriceLimit == null ? PriceLimitFormula((double)d.CenaDiler, exchangeRate) : v.PriceLimit,
                isPriceLimitCustom = v.PriceLimit != null
            });
            return Ok(res);
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
            var allRecs = db.VectorOffers.Where(i => i.Supplier == supplierName);
            foreach (var rec in allRecs)
            {
                rec.IsVerified = value;
                db.Update(rec);
            }
            var plRec = db.Pricelists.Where(i => i.Id == controllerId).FirstOrDefault();
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
            var allRecs = db.VectorOffers.Where(i => i.Supplier == supplierName);
            foreach (var rec in allRecs)
            {
                rec.Status = (VectorOfferStatus)value;
                db.Update(rec);
            }
            db.SaveChanges();
            return Ok();
        }

        [Route("setGroupStatus")]
        [HttpPost]
        public IActionResult SetGroupStatus([FromBody] dynamic qparams)
        {
            var groupCode = (int)qparams["groupCode"];
            var allRecIds = db.VectorOffers.Join(db.Dynatone, v => v.Id, d => d.Id, (v, d) =>
            new
            {
                id = v.Id,
                Supplier = v.Supplier,
                groupCode = d.KodGruppy,
            }).Where(i => i.Supplier == supplierName && i.groupCode == groupCode).Select(i => i.id).ToList();

            foreach (var recId in allRecIds)
            {
                var rec = db.VectorOffers.Where(i => i.Id == recId).FirstOrDefault();
                rec.Status = (VectorOfferStatus)(int)qparams["value"];
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
                    return Ok(db.Dynatone.Where(i => i.Id == Guid.Parse(id)).Select(i => i.KodSPrefixom).FirstOrDefault());
                case "brand":
                    return Ok(db.Dynatone.Where(i => i.Id == Guid.Parse(id)).Select(i => i.Brand).FirstOrDefault());
                case "name":
                    return Ok(db.Dynatone.Where(i => i.Id == Guid.Parse(id)).Select(i => i.Naimenovanie).FirstOrDefault());
                case "price":
                    return Ok(db.Dynatone.Where(i => i.Id == Guid.Parse(id)).Select(i => PriceFormula((double)i.CenaDiler, exchangeRate)).FirstOrDefault());
                case "priceLimit":
                    return Ok(db.Dynatone.Where(i => i.Id == Guid.Parse(id)).Select(i => PriceLimitFormula((double)i.CenaDiler, exchangeRate)).FirstOrDefault());
            }
            return StatusCode(400);
        }
    }
}
