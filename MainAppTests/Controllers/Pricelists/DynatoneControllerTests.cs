using Microsoft.VisualStudio.TestTools.UnitTesting;
using MainApp.Controllers.Pricelists;
using System;
using System.Collections.Generic;
using System.Text;
using DbCore;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers.Pricelists.Tests
{
    [TestClass()]
    public class DynatoneControllerTests
    {
        [TestMethod()]
        public void PullPricelistTest()
        {
           // DynatoneController test = new DynatoneController(new MainDbContext());
           // Assert.AreEqual(test.PullPricelist().ToString(), test.Ok().ToString());
        }
    }
}