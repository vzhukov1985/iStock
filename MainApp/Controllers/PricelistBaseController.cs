using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Services;
using DbCore;
using DbCore.Models;
using DbCore.PLModels;
using MainApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainApp.Controllers
{
    public abstract class PricelistBaseController: Controller
    {
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

        public PricelistBaseController()
        {
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
