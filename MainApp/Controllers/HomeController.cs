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
            HomeVM vm = new HomeVM();

            IEnumerable<HomeVM.PricelistBriefView> pricelists = db.Pricelists.Select(p => new HomeVM.PricelistBriefView
            {
                Id = p.Id,
                Name = p.Name,
                SupplierName = p.SupplierName,
                LastUpdate = p.LastUpdate,
                UncheckedCount = p.UncheckedCount
            });

            return Ok(pricelists);
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
