using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Services;
using DbCore;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainApp.Controllers
{
    public class HomeController : Controller
    {
        MainDbContext db;
        HttpClient hc;

        public HomeController(MainDbContext dbContext, HttpClient client)
        {
            db = dbContext;
            hc = client;
        }

        [HttpGet("pricelists/getallbrief")]
        public IActionResult GetAllPricelistsBriefData()
        {

            var pricelists = db.Pricelists.Select(p => new
            {
                p.Id,
                p.Name,
                p.SupplierName,
                lastPull = "",
                itemsToVerify = 0,
                p.Controller,
                isPulling = false
            });

            return Ok(pricelists);
        }

        public IActionResult Index()
        {
            AutoExchangeRates.UpdateRatesAsync(hc);
            return View();
        }

        [Route("findBySku/{id}")]
        [HttpGet]
        public IActionResult FindBySku([FromRoute] string id)
        {
            Guid plId = db.VectorOffers
                                .Join(db.Dynatone,
                                      v => v.Id,
                                      d => d.Id,
                                      (v, d) => new { v.Id, Sku = v.Sku == null ? d.KodSPrefixom : v.Sku, v.PricelistId }).Where(j => j.Sku == id).Select(j => j.PricelistId).FirstOrDefault();

            if (plId != Guid.Empty)
            {
                return Ok(db.Pricelists.Where(p => p.Id == plId).Select(p => p.Controller).FirstOrDefault());
            }

            plId = db.VectorOffers.Join(db.Grandm,
                                        v => v.Id,
                                        g => g.Id,
                                        (v, g) => new { v.Id, Sku = v.Sku == null ? $"GM-{g.IdTovara}" : v.Sku, v.PricelistId }).ToList().Where(i => i.Sku == id).Select(j => j.PricelistId).FirstOrDefault();

            if (plId != Guid.Empty)
            {
                return Ok(db.Pricelists.Where(p => p.Id == plId).Select(p => p.Controller).FirstOrDefault());
            }

            return Ok(null);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
