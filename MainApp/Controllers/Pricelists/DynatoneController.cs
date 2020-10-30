using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers.Pricelists
{
    public class DynatoneController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
