using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynatone.Models
{
    public class DynatoneOptions
    {
        public const string SectionName = "SupplierDynatone";

        public DateTime LastUpdate { get; set; }

        public int MinStockAvail { get; set; }
        public int PreorderInDays { get; set; }
        public bool IsFavorite { get; set; }

        public bool PreviewWithIds { get; set; }
        public bool ExcludeNotInStock { get; set; }
        public bool ExcludeExcludedItems { get; set; }
        public bool ExcludeExcludedGroups { get; set; }
        public double ExchangeRate { get; set; }

        public string ExcludeGroups { get; set; }
        public string ExcludeItems { get; set; }
    }
}
