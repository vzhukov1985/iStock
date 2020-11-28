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


        public PricelistBaseController()
        {
            
        }


    }
}
