using System;
namespace DbCore.PLModels
{
    public class PleerOffer
    {
        public Guid Id { get; set; }
        public int NomerTovara { get; set; }
        public string Catalog { get; set; }
        public string KodTovara { get; set; }
        public string Naimenovanie { get; set; }
        public string Garantiya { get; set; }
        public int Nalichie { get; set; }
        public int Diler1 { get; set; }
        public int Diler2 { get; set; }
        public int Diler3 { get; set; }
        public int Diler4 { get; set; }


        public bool IsDescriptionChanged(PleerOffer oldOffer)
        {
            if (Catalog != oldOffer.Catalog ||
                KodTovara != oldOffer.KodTovara ||
                Naimenovanie != oldOffer.Naimenovanie ||
                Garantiya != oldOffer.Garantiya)
                return true;

            return false;
        }

        public bool IsPriceChanged(PleerOffer oldOffer)
        {
            if (Diler4 != oldOffer.Diler4)
                return true;
            return false;
        }

        public void CopyFromWithoutId(PleerOffer offer)
        {
            NomerTovara = offer.NomerTovara;
            Catalog = offer.Catalog;
            KodTovara = offer.KodTovara;
            Naimenovanie = offer.Naimenovanie;
            Garantiya = offer.Garantiya;
            Nalichie = offer.Nalichie;
            Diler1 = offer.Diler1;
            Diler2 = offer.Diler2;
            Diler3 = offer.Diler3;
            Diler4 = offer.Diler4;
        }
    }
}
