using System;
using System.Collections.Generic;
using System.Text;

namespace DbCore.Models
{
    public class Pricelist
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public string Name { get; set; }
        public DateTime? LastPull { get; set; }
        public int ItemsToVerify { get; set; }
        
        public int PreorderInDays { get; set; }
        public int MinStockAvail { get; set; }
        public bool IsFavorite { get; set; }
        public double? ExchangeRate { get; set; }

        public string Controller { get; set; }
    }
}
