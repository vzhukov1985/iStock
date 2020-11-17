using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbCore;
using DbCore.Models;

namespace MainApp.Services
{
    public class ControllersManager
    {
        public List<ControllerInfo> Controllers { get;}

        public ControllerInfo this[Guid id]
        {
            get
            {
                return Controllers.Where(i => i.Id == id).FirstOrDefault();
            }
        }

        public ControllerInfo this[string guid]
        {
            get
            {
                Guid id;
                if (Guid.TryParse(guid, out id))
                {
                    return Controllers.Where(i => i.Id == id).FirstOrDefault();
                }
                return null;
            }
        }

        public ControllersManager()
        {
            using (MainDbContext db = new MainDbContext())
            {
                Controllers = db.Pricelists.Select(pl => new ControllerInfo(pl)).ToList();
            }
        }

    }

    public class ControllerInfo
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
        public string PricelistName { get; set; }
        public string ControllerName { get; set; }
        public bool IsPulling { get; set; }
        public int PullRecordsProcessed { get; set; }
        public byte[] ExcelFileData {get;set;}

        public ControllerInfo(Pricelist pl)
        {
            Id = pl.Id;
            SupplierName = pl.SupplierName;
            ControllerName = pl.Controller;
            PricelistName = pl.Name;
            IsPulling = false;
            PullRecordsProcessed = -1;
        }
    }
}
