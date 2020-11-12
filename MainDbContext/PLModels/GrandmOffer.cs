using System;
using System.Collections.Generic;
using System.Text;

namespace DbCore.PLModels
{
    public class GrandmOffer
    {
        public Guid Id { get; set; }
        public string Brand {get;set;}
        public string CategoryName { get; set; }
        public int? CenaDiler { get; set; }
        public string Kornevaya { get; set; }
        public string Podkategoriya1 { get; set; }
        public string Podkategoriya2 { get; set; }
        public string Podkategoriya3 { get; set; }
        public string Podkategoriya4 { get; set; }
        public int? IdTovara { get; set; }
        public string NazvanieTovara { get; set; }
        public string URL { get; set; }
        public string KratkoeOpisanie { get; set; }
        public string Izobrazheniya { get; set; }
        public string SvoistvoRazmer { get; set; }
        public string SvoistvoCvet { get; set; }
        public string Articul { get; set; }
        public string CenaProdazhi { get; set; }
        public int? Ostatok { get; set; }
        public string Ves { get; set; }

        public bool IsDescriptionChanged(GrandmOffer oldOffer)
        {
            if (NazvanieTovara != oldOffer.NazvanieTovara ||
                KratkoeOpisanie != oldOffer.KratkoeOpisanie ||
                SvoistvoRazmer != oldOffer.SvoistvoRazmer ||
                SvoistvoCvet != oldOffer.SvoistvoCvet ||
                Articul != oldOffer.Articul)
                return true;

            return false;
        }

        public bool IsPriceChanged(GrandmOffer oldOffer)
        {
            if (CenaProdazhi != oldOffer.CenaProdazhi)
                return true;
            return false;
        }

        public void CopyFromWithoutId(GrandmOffer offer)
        {
            Brand = offer.Brand;
            CategoryName = offer.CategoryName;
            CenaDiler = offer.CenaDiler;
            Kornevaya = offer.Kornevaya;
            Podkategoriya1 = offer.Podkategoriya1;
            Podkategoriya2 = offer.Podkategoriya2;
            Podkategoriya3 = offer.Podkategoriya3;
            Podkategoriya4 = offer.Podkategoriya4;
            IdTovara = offer.IdTovara;
            NazvanieTovara = offer.NazvanieTovara;
            URL = offer.URL;
            KratkoeOpisanie = offer.KratkoeOpisanie;
            Izobrazheniya = offer.Izobrazheniya;
            SvoistvoRazmer = offer.SvoistvoRazmer;
            SvoistvoCvet = offer.SvoistvoCvet;
            Articul = offer.Articul;
            CenaProdazhi = offer.CenaProdazhi;
            Ostatok = offer.Ostatok;
            Ves = offer.Ves;
        }
    }
}
