using DbCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.ViewModels
{
    public class HomeVM
    {
        public List<PricelistBriefView> Pricelists;
        
        public class PricelistBriefView
        {
            public Guid Id { get; set; }
            public string SupplierName { get; set; }
            public string Name { get; set; }
            public DateTime? LastUpdate { get; set; }
            public int UncheckedCount { get; set; }
        }
    }
}
