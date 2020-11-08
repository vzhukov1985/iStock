using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DbCore;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainApp.Controllers
{
    public class HomeController : Controller
    {
        MainDbContext db;

        public HomeController(MainDbContext dbContext)
        {
            db = dbContext;
        }

        [HttpGet("pricelists/getallbrief")]
        public IActionResult GetAllPricelistsBriefData()
        {

            var pricelists = db.Pricelists.Select(p => new
            {
                p.Id,
                p.Name,
                p.SupplierName,
                lastPull = p.LastPull == null ? null : ((DateTime)p.LastPull).ToString("G"),
                p.ItemsToVerify,
                p.Controller
            });

            return Ok(pricelists);
        }

        [HttpGet("pricelists/getlastpull/{id}")]
        public IActionResult GetLastPull(string id)
        {
            Guid plId;
            if (Guid.TryParse(id, out plId))
            {
                return Ok(db.Pricelists.Where(p => p.Id == plId).Select(p => p.LastPull == null ? null: ((DateTime)p.LastPull).ToString("G")).FirstOrDefault());
            }
            return StatusCode(400);
        }

        [HttpGet("pricelists/getItemsToVerify/{id}")]
        public IActionResult GetItemsToVerifyCount(string id)
        {
            Guid plId;
            if (Guid.TryParse(id, out plId))
            {
                return Ok(db.Pricelists.Where(p => p.Id == plId).Select(p => p.ItemsToVerify).FirstOrDefault());
            }
            return StatusCode(400);
        }


        [HttpGet("pricelists/plsettings/{id}")]
        public IActionResult GetPricelistSettings(string id)
        {
            Guid plId;
            if (Guid.TryParse(id, out plId))
            {
                var res = db.Pricelists.Where(p => p.Id == plId).Select(p => new
                {
                    p.Name,
                    p.SupplierName,
                    p.PreorderInDays,
                    p.MinStockAvail,
                    p.IsFavorite
                }).FirstOrDefault();

                if (res != null)
                    return Ok(res);
            }
            return StatusCode(400);
        }

        [HttpPost("pricelists/plsettings/{id}")]
        public IActionResult SetPricelistSettings([FromRoute] string id, [FromBody] PLSettings a)
        {
            Guid plId;
            if (Guid.TryParse(id, out plId))
            {
                var pl = db.Pricelists.Where(p => p.Id == plId).FirstOrDefault();
                if (pl != null)
                {
                    pl.SupplierName = a.SupplierName;
                    pl.Name = a.Name;
                    pl.PreorderInDays = a.PreorderInDays;
                    pl.MinStockAvail = a.MinStockAvail;
                    pl.IsFavorite = a.IsFavorite;
                    db.SaveChanges();
                    return Ok();
                }
            }
            return StatusCode(400);
        }
        public class PLSettings
        {
            public string SupplierName { get; set; }
            public string Name { get; set; }
            public int PreorderInDays { get; set; }
            public int MinStockAvail { get; set; }
            public bool IsFavorite { get; set; }
        }



        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
